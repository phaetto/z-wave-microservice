namespace MicroServicesStarter.Deploy.Actions
{
    using System.Diagnostics;
    using Chains;

    public sealed class InitNuget : IChainableAction<DeploySetupContext, DeploySetupContext>
    {
        public DeploySetupContext Act(DeploySetupContext context)
        {
            // Self-update nuget if needed
            Process.Start(
                new ProcessStartInfo
                {
                    FileName = string.Format("{0}nuget.exe", context.NugetFolder),
                    Arguments = "Update -self",
                    WorkingDirectory = context.NugetFolder,
                    CreateNoWindow = true,
                }).WaitForExit();

            // Set the api-key
            Process.Start(
                new ProcessStartInfo
                {
                    FileName = string.Format("{0}nuget.exe", context.NugetFolder),
                    Arguments = string.Format("setApiKey {0} -Source {1}", context.NugetApiKey, context.NugetServer),
                    WorkingDirectory = context.NugetFolder,
                    CreateNoWindow = true,
                }).WaitForExit();

            return context;
        }
    }
}
