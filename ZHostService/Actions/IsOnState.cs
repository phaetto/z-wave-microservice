namespace ZHostService.Actions
{
    public sealed class IsOnState : JobActionWithSerializableData<bool>
    {
        public IsOnState(byte nodeId)
            : base(new JobData
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
            OnReceive += onReceiveResponse;
        }

        private bool onReceiveResponse(ZWaveContext context, ZWaveMessage message)
        {
            return message.Message[9] == 0xFF;
        }
    }
}
