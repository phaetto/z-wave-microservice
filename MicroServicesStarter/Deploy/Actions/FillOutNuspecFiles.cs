namespace MicroServicesStarter.Deploy.Actions
{
    using System.IO;
    using Chains;

    public sealed class FillOutNuspecFiles : IChainableAction<PreparedDeploySetupContext, PreparedDeploySetupContext>
    {
        public PreparedDeploySetupContext Act(PreparedDeploySetupContext context)
        {
            foreach (var projectRegistration in context.Parent.Parent.Projects)
            {
                if (!projectRegistration.IsNugetPackage)
                {
                    continue;
                }

                var binNuspecFile = string.Format("{0}{1}.nuspec", projectRegistration.Directory, projectRegistration.Name);

                var packageDirectory = string.Format(@"{0}Packages\{1}\", context.Parent.NugetFolder, projectRegistration.Name);

                var newNuspecFile = packageDirectory + Path.GetFileName(binNuspecFile);

                var nuspecText = File.ReadAllText(binNuspecFile);

                nuspecText = nuspecText.Replace("{Version}", context.UpdateToVersion);
                nuspecText = nuspecText.Replace("{ReleaseNotes}", context.ReleaseNotes);

                File.WriteAllText(newNuspecFile, nuspecText);
            }

            return context;
        }
    }
}
