namespace ZHostService.Actions
{
    public sealed class SwitchOff : Job
    {
        public const byte State = 0x00;

        public SwitchOff(byte nodeId)
            : base(new JobData
                   {
                       Function = ZWaveFunction.SendData,
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

        public SwitchOff(JobData data)
            : base(data)
        {
        }
    }
}
