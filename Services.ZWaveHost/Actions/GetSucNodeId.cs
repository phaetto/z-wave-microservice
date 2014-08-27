namespace Services.ZWaveHost.Actions
{
    public sealed class GetSucNodeId : JobActionWithSerializableData<byte>
    {
        public GetSucNodeId(JobData jobData)
            : base(jobData)
        {
            OnReceive += onReceiveSucNodeId;
        }

        public GetSucNodeId()
            : this(new JobData
                   {
                       Function = ZWaveFunction.MemoryGetId,
                       ExpectedFunction = ZWaveFunction.MemoryGetId
                   })
        {
        }

        private byte onReceiveSucNodeId(ZWaveContext context, ZWaveMessage message)
        {
            return message.Message[4];
        }
    }
}
