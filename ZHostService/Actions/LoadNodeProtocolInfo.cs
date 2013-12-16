namespace ZHostService.Actions
{
    using System;

    public sealed class LoadNodeProtocolInfo : Job
    {
        public LoadNodeProtocolInfo(byte nodeId)
            : base(new JobData
                   {
                       Function = ZWaveFunction.GetNodeProtocolInfo,
                       ExpectedFunction = ZWaveFunction.GetNodeProtocolInfo,
                       NodeId = nodeId
                   })
        {
            OnReceive += onReceiveResponse;
        }

        private void onReceiveResponse(ZWaveContext context, ZWaveMessage message)
        {
            var msg = message.Message;
            var sleeping = !((msg[4] & (0x01<<7)) > 0x00);

            lock (context)
            {
                Console.WriteLine("\t NodeID:" + Data.NodeId);
                Console.WriteLine("\t Sleeping:" + sleeping);
                Console.WriteLine("\t Basic type:" + msg[7]);
                Console.WriteLine("\t Generic type:" + msg[8]);
                Console.WriteLine("\t Specific type:" + msg[9]);

                context.Nodes.Add(
                    new ZWaveNode
                    {
                        NodeId = Data.NodeId,
                        Sleeping = sleeping,
                        Basictype = (ZWaveType.Basic)msg[7],
                        GenericType = (ZWaveType.Generic)msg[8],
                        SpecificType = (ZWaveType.Specific)msg[9]
                    });
            }
        }
    }
}
