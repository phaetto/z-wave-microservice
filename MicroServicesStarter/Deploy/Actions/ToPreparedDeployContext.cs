namespace MicroServicesStarter.Deploy.Actions
{
    using Chains;

    public sealed class ToPreparedDeployContext : IChainableAction<DeploySetupContext, PreparedDeploySetupContext>
    {
        private readonly ReleaseType releaseType;

        private readonly string releaseNotes;

        public ToPreparedDeployContext(ReleaseType releaseType, string releaseNotes)
        {
            this.releaseType = releaseType;
            this.releaseNotes = releaseNotes;
        }

        public PreparedDeploySetupContext Act(DeploySetupContext context)
        {
            return new PreparedDeploySetupContext(context, releaseNotes, releaseType);
        }
    }
}
