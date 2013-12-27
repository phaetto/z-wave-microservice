namespace ZHostService.Actions
{
    using System.Linq;

    public sealed class LoadNodeProtocolInfo : Job
    {
        public LoadNodeProtocolInfo(JobData jobData)
            : base(jobData)
        {
            OnReceive += onReceiveResponse;
        }

        public LoadNodeProtocolInfo(byte nodeId)
            : this(new JobData
                   {
                       Function = ZWaveFunction.GetNodeProtocolInfo,
                       ExpectedFunction = ZWaveFunction.GetNodeProtocolInfo,
                       NodeId = nodeId
                   })
        {
        }

        private void onReceiveResponse(ZWaveContext context, ZWaveMessage message)
        {
            var msg = message.Message;
            var sleeping = !((msg[4] & (0x01<<7)) > 0x00);

            lock (context)
            {
                context.Nodes.Add(
                    new ZWaveNode
                    {
                        NodeId = Data.NodeId,
                        Sleeping = sleeping,
                        Basictype = (ZWaveType.Basic)msg[7],
                        GenericType = (ZWaveType.Generic)msg[8],
                        SpecificType = (ZWaveType.Specific)msg[9]
                    });
#if DEBUG
                context.WorkUnitContext.LogLine(
                    "\tNodeID: {0}, Sleeping: {1}, Basic Type: {2}, Generic Type: {3}, Specific Type: {4}",
                    Data.NodeId,
                    sleeping,
                    context.Nodes.Last().Basictype.ToString(),
                    context.Nodes.Last().GenericType.ToString(),
                    context.Nodes.Last().SpecificType.ToString());
#endif
            }
        }
    }
}
