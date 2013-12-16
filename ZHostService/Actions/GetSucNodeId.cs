namespace ZHostService.Actions
{
    public sealed class GetSucNodeId : JobActionWithSerializableData<byte>
    {
        public GetSucNodeId()
            : base(new JobData
                   {
                       Function = ZWaveFunction.MemoryGetId,
                       ExpectedFunction = ZWaveFunction.MemoryGetId
                   })
        {
            OnReceive += onReceiveSucNodeId;
        }

        private byte onReceiveSucNodeId(ZWaveContext context, ZWaveMessage message)
        {
            return message.Message[4];
        }
    }
}
