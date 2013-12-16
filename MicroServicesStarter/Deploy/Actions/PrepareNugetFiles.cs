namespace MicroServicesStarter.Deploy.Actions
{
    using System.IO;
    using Chains;
    using MicroServicesStarter.Deploy.FileCopy;

    public sealed class PrepareNugetFiles : IChainableAction<PreparedDeploySetupContext, PreparedDeploySetupContext>
    {
        public PreparedDeploySetupContext Act(PreparedDeploySetupContext context)
        {
            var packagesDirectory = string.Format(@"{0}Packages\", context.Parent.NugetFolder);

            if (Directory.Exists(packagesDirectory))
            {
                Directory.Delete(packagesDirectory, true);
            }

            foreach (var projectRegistration in context.Parent.Parent.Projects)
            {
                if (!projectRegistration.IsNugetPackage)
                {
                    continue;
                }

                var nuspecFile = string.Format("{0}{1}.nuspec", projectRegistration.Directory, projectRegistration.Name);

                if (!File.Exists(nuspecFile))
                {
                    throw new FileNotFoundException("Expected to find the nuspec file in project folder");
                }

                var filesToKeepFile = string.Format("{0}nuget-files.xml", projectRegistration.Directory);

                if (!File.Exists(filesToKeepFile))
                {
                    throw new FileNotFoundException("Expected to find the nuget-files.xml in project folder.");
                }

                var packageDirectory = string.Format(@"{0}{1}\", packagesDirectory, projectRegistration.Name);
                var libPackageDirectory = string.Format(@"{0}lib\", packageDirectory);
                new FileCopier(projectRegistration.BinDirectory, libPackageDirectory, filesToKeepFile).Execute();
            }

            return context;
        }
    }
}
