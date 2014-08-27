namespace Services.ZWaveHost.Actions
{
    public sealed class IsOnState : JobActionWithSerializableData<bool>
    {
        public IsOnState(JobData jobData)
            : base(jobData)
        {
            OnReceive += onReceiveResponse;
        }

        public IsOnState(byte nodeId)
            : this(new JobData
                   {
                       Function = ZWaveFunction.SendData,
                       ExpectedFunction = ZWaveFunction.ApplicationCommandHandler,
                       NodeId = nodeId,
                       CommandClass = ZWaveCommandClass.SwitchBinary,
                       ExpectedCommandClass = ZWaveCommandClass.SwitchBinary,
                       Command = ZWaveCommand.Get,
                       ExpectedCommand = ZWaveCommand.Report
                   })
        {
        }

        private bool onReceiveResponse(ZWaveContext context, ZWaveMessage message)
        {
            return message.Message[9] == 0xFF;
        }
    }
}
