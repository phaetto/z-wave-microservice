namespace ZHostService.Actions
{
    using System.Collections.Generic;
    using System.Linq;

    public sealed class LoadNodeCapabilities : Job
    {
        public LoadNodeCapabilities(byte nodeId)
            : base(new JobData
                   {
                       Function = ZWaveFunction.GetNodeCapabilities,
                       ExpectedFunction = ZWaveFunction.GetNodeCapabilitiesResponse,
                       NodeId = nodeId
                   })
        {
            OnReceive += onReceiveResponse;
        }

        private void onReceiveResponse(ZWaveContext context, ZWaveMessage message)
        {
            for (var i = 10; i < 6 + message.Message[6]; i++)
            {
                var node = context.Nodes.FirstOrDefault(x => x.NodeId == Data.NodeId);

                if (node != null)
                {
                    if (node.Capabilities == null)
                    {
                        node.Capabilities = new List<ZWaveCommandClass>();
                    }

                    node.Capabilities.Add((ZWaveCommandClass)message.Message[i]);
                }
            }
        }
    }
}
