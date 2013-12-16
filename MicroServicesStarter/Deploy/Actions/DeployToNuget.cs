namespace MicroServicesStarter.Deploy.Actions
{
    using System.Diagnostics;
    using Chains;

    class DeployToNuget : IChainableAction<PreparedDeploySetupContext, PreparedDeploySetupContext>
    {
        public PreparedDeploySetupContext Act(PreparedDeploySetupContext context)
        {
            foreach (var projectRegistration in context.Parent.Parent.Projects)
            {
                if (!projectRegistration.IsNugetPackage)
                {
                    continue;
                }

                var nuspecFile = string.Format(@"Packages\{0}\{0}.nuspec", projectRegistration.Name);

                context.Parent.Parent.LogToUi(string.Format("Packing nuspec '{0}'", nuspecFile));

                // Pack the nuspec as nupkg
                Process.Start(
                    new ProcessStartInfo
                    {
                        FileName = string.Format("{0}nuget.exe", context.Parent.NugetFolder),
                        Arguments = string.Format("pack \"{0}\"", nuspecFile),
                        WorkingDirectory = context.Parent.NugetFolder,
                        CreateNoWindow = true,
                    }).WaitForExit();

                // Upload the nupkg
                var nupkgFilename = string.Format(
                    "{0}.{1}.nupkg", projectRegistration.Name, context.UpdateToVersion);

                context.Parent.Parent.LogToUi(string.Format("Uploading nuget-package '{0}'", nupkgFilename));

                Process.Start(
                    new ProcessStartInfo
                    {
                        FileName = string.Format("{0}nuget.exe", context.Parent.NugetFolder),
                        Arguments = string.Format("push {0} -Source {1}", nupkgFilename, context.Parent.NugetServer),
                        WorkingDirectory = context.Parent.NugetFolder,
                        CreateNoWindow = true,
                    }).WaitForExit();
            }

            return context;
        }
    }
}
