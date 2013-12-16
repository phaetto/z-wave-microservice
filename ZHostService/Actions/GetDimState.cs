namespace ZHostService.Actions
{
    using System;

    public sealed class GetDimState : JobActionWithSerializableData<int>
    {
        public GetDimState(byte nodeId)
            : base(new JobData
                   {
                       Function = ZWaveFunction.GetNodeProtocolInfo,
                       ExpectedFunction = ZWaveFunction.GetNodeProtocolInfo,
                       NodeId = nodeId
                   })
        {
            OnReceive += onReceiveResponse;
        }

        private int onReceiveResponse(ZWaveContext context, ZWaveMessage message)
        {
            return message.Message[9];
        }
    }
}
