namespace MicroServicesStarter.ServiceManagement.Action
{
    using System.Diagnostics;
    using Chains;
    using MicroServicesStarter.ServiceManagement.Debugger;

    public sealed class AttachDebuggerToProcess : IChainableAction<AdminSetupContext, AdminSetupContext>
    {
        private readonly Process processToAttach;

        public AttachDebuggerToProcess(Process processToAttach)
        {
            this.processToAttach = processToAttach;
        }

        public AdminSetupContext Act(AdminSetupContext context)
        {
            var vsProcess = VisualStudioAttacher.GetVisualStudioForSolution(context.SolutionDirectory);
            VisualStudioAttacher.AttachVisualStudioToProcess(vsProcess, processToAttach);

            return context;
        }
    }
}
