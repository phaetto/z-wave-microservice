namespace ZHostService.Actions
{
    public sealed class GetDimState : JobActionWithSerializableData<int>
    {
        public GetDimState(JobData jobData)
            : base(jobData)
        {
            OnReceive += onReceiveResponse;
        }

        public GetDimState(byte nodeId)
            : this(new JobData
                   {
                       Function = ZWaveFunction.GetNodeProtocolInfo,
                       ExpectedFunction = ZWaveFunction.GetNodeProtocolInfo,
                       NodeId = nodeId
                   })
        {
        }

        private int onReceiveResponse(ZWaveContext context, ZWaveMessage message)
        {
            return message.Message[9];
        }
    }
}
