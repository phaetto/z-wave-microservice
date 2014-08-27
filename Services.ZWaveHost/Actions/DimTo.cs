namespace Services.ZWaveHost.Actions
{
    public sealed class DimTo : Job
    {
        public DimTo(byte nodeId, int percentageOn)
            : base(new JobData
                   {
                       Function = ZWaveFunction.SendData,
                       ExpectedFunction = ZWaveFunction.ApplicationCommandHandler,
                       NodeId = nodeId,
                       CommandClass = ZWaveCommandClass.SwitchMultilevel,
                       ExpectedCommandClass = ZWaveCommandClass.SwitchMultilevel,
                       Command = ZWaveCommand.Set,
                       ExpectedCommand = ZWaveCommand.Report,
                       Parameters = new[]
                                    {
                                        (byte)percentageOn
                                    }
                   })
        {
        }

        public DimTo(JobData data)
            : base(data)
        {
        }
    }
}
