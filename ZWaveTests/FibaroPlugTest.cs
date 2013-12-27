namespace ZWaveTests
{
    using System.Threading;
    using Chains;
    using Services.Management.Administration.Worker;
    using ZHostService;
    using ZHostService.Actions;

    public sealed class FibaroPlugTest : Chain<FibaroPlugTest>
    {
        private readonly WorkUnitContext workUnitContext;

        public FibaroPlugTest(WorkUnitContext workUnitContext)
        {
            this.workUnitContext = workUnitContext;
            using (var zWaveContext = new ZWaveContext(workUnitContext))
            {
                zWaveContext.OnStart();

                GetValue(61, zWaveContext);
                GetValue(62, zWaveContext);

                // zWaveContext.Do(new SetConfigurationValue(2, 62, 8));

                workUnitContext.LogLine("1. Is on: {0}", zWaveContext.DoRemotable(new IsOnState(2)));
                zWaveContext.Do(new SwitchOn(2));

                Thread.Sleep(5000);

                workUnitContext.LogLine("2. Is on: {0}", zWaveContext.DoRemotable(new IsOnState(2)));
                zWaveContext.Do(new SwitchOff(2));

                workUnitContext.LogLine("Finished.");
            }
        }

        private void GetValue(byte configId, ZWaveContext zWaveContext)
        {
            workUnitContext.LogLine(
                "{0} = {1}", configId, zWaveContext.DoRemotable(new GetConfigurationValue(2, configId)));
        }
    }
}
