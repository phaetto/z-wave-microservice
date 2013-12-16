namespace ZHostService.Actions
{
    public sealed class SwitchOn : Job
    {
        public const byte State = 0xFF;

        public SwitchOn(byte nodeId)
            : base(new JobData
                   {
                       Function = ZWaveFunction.SendData,
                       ExpectedFunction = ZWaveFunction.SendData,
                       NodeId = nodeId,
                       CommandClass = ZWaveCommandClass.SwitchBinary,
                       Command = ZWaveCommand.Set,
                       Parameters = new[]
                                    {
                                        State
                                    }
                   })
        {
        }

        public SwitchOn(JobData data)
            : base(data)
        {
        }
    }
}
