namespace MicroServicesStarter.Deploy.Actions
{
    using System.IO;
    using Chains;
    using MicroServicesStarter.Deploy.FileCopy;

    public sealed class PrepareUpdateFiles : IChainableAction<PreparedDeploySetupContext, PreparedDeploySetupContext>
    {
        public PreparedDeploySetupContext Act(PreparedDeploySetupContext context)
        {
            var packagesDirectory = string.Format(@"{0}Packages\", context.Parent.UpdateFolder);

            if (Directory.Exists(packagesDirectory))
            {
                Directory.Delete(packagesDirectory, true);
            }

            foreach (var projectRegistration in context.Parent.Parent.Projects)
            {
                if (!projectRegistration.IsUpdatePackage)
                {
                    continue;
                }

                var filesToKeepFile = string.Format("{0}update-files.xml", projectRegistration.Directory);

                if (!File.Exists(filesToKeepFile))
                {
                    throw new FileNotFoundException("Expected to find the update-files.xml in project folder.");
                }

                context.Parent.Parent.LogToUi(string.Format("Preparing '{0}' for update...", projectRegistration.Name));

                var packageDirectory = string.Format(@"{0}Packages\{1}\", context.Parent.UpdateFolder, projectRegistration.Name);
                Directory.CreateDirectory(packageDirectory);

                new FileCopier(projectRegistration.BinDirectory, packageDirectory, filesToKeepFile).Execute();
            }

            return context;
        }
    }
}
