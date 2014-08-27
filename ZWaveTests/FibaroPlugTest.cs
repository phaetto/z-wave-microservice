namespace ZWaveTests
{
    using System.Threading;
    using Chains;
    using Chains.Play.Web;
    using Services.Communication.Protocol;
    using Services.Management.Administration.Worker;
    using Services.ZWaveHost.Actions;

    public sealed class FibaroPlugTest : Chain<FibaroPlugTest>
    {
        private readonly WorkUnitContext workUnitContext;

        public FibaroPlugTest(WorkUnitContext workUnitContext)
        {
            this.workUnitContext = workUnitContext;

            workUnitContext.LogLine("Connecting to zwave micro service...");

            using (var zWaveConnection = new Client("localhost", 27123).Do(new WaitUntilClientConnects()))
            {
                GetValue(47, zWaveConnection);
                GetValue(61, zWaveConnection);
                GetValue(62, zWaveConnection);

                // Default #47: 3600
                zWaveConnection.Do(new Send(new SetConfigurationValue(2, 47, 3600)));
                // zWaveConnection.Do(new Send(new SetConfigurationValue(2, 47, 60)));
                GetValue(47, zWaveConnection);

                //for (var i = 0; i < 9; ++i)
                //{
                //    zWaveConnection.Do(new Send(new SetConfigurationValue(2, 62, (byte)i)));
                //    Thread.Sleep(10000);
                //}

                //zWaveConnection.Do(new Send(new SetConfigurationValue(2, 62, (byte)8)));
                zWaveConnection.Do(new Send(new SetConfigurationValue(2, 62, (byte)3)));
                //Thread.Sleep(10000);
                //zWaveConnection.Do(new Send(new SetConfigurationValue(2, 62, (byte)8)));

                workUnitContext.LogLine("1. Is on: {0}", zWaveConnection.Do(new Send<bool>(new IsOnState(2))));
                //zWaveConnection.Do(new Send(new SwitchOn(2)));

                //Thread.Sleep(5000);

                //workUnitContext.LogLine("2. Is on: {0}", zWaveConnection.Do(new Send<bool>(new IsOnState(2))));
                //zWaveConnection.Do(new Send(new SwitchOff(2)));

                workUnitContext.LogLine("Finished.");

                workUnitContext.ReportToAdmin();
                workUnitContext.Stop();
            }
        }

        private void GetValue(byte configId, ClientConnectionContext zWaveConnection)
        {
            workUnitContext.LogLine(
                "Configuration value {0} = {1}", configId, zWaveConnection.Do(new Send<int>(new GetConfigurationValue(2, configId))));
        }
    }
}
