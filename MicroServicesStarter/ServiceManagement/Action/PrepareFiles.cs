namespace MicroServicesStarter.ServiceManagement.Action
{
    using System.IO;
    using Chains;
    using MicroServicesStarter.Deploy.FileCopy;

    public sealed class PrepareFiles : IChainableAction<AdminSetupContext, AdminSetupContext>
    {
        public AdminSetupContext Act(AdminSetupContext context)
        {
            var microServicesStarterDirectory = string.Format("{0}MicroServicesStarter\\", context.SolutionDirectory);
            var adminDirectory = string.Format("{0}Admin\\", context.SolutionDirectory);

            var filesToCopyToAdmin = string.Format(
                "{0}{1}-admin-files.xml", microServicesStarterDirectory, context.SetupType.ToString());

            if (File.Exists(filesToCopyToAdmin))
            {
                new FileCopier(microServicesStarterDirectory, adminDirectory, filesToCopyToAdmin).Execute();
            }

            return context;
        }
    }
}
