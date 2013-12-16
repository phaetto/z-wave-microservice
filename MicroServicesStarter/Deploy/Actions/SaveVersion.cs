namespace MicroServicesStarter.Deploy.Actions
{
    using System.IO;
    using Chains;

    public sealed class SaveVersion : IChainableAction<PreparedDeploySetupContext, PreparedDeploySetupContext>
    {
        public PreparedDeploySetupContext Act(PreparedDeploySetupContext context)
        {
            var versionFile = string.Format("{0}version.txt", context.Parent.Parent.SolutionDirectory);
            File.WriteAllText(versionFile, context.UpdateToVersion);

            return context;
        }
    }
}
