namespace ZHostService.Actions
{
    using System.Collections.Generic;
    using System.Linq;

    public sealed class SerialApiInitData : Job
    {
        public SerialApiInitData(JobData jobData)
            : base(jobData)
        {
            OnReceive += onReceiveMessage;
        }

        public SerialApiInitData()
            : this(new JobData
                   {
                       Function = ZWaveFunction.SerialApiInitData,
                       ExpectedFunction = ZWaveFunction.SerialApiInitData
                   })
        {
        }

        private void onReceiveMessage(ZWaveContext context, ZWaveMessage message)
        {
            var response = message.Message;

            var foundNodes = new List<byte>();
            for (var i = 7; i < 35; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    if ((response[i] & (0x01 << j)) != 0)
                    {
                        var nodeId = (byte)((i - 7) * 8 + (j + 1));
                        if (nodeId != context.ControllerId)
                        {
                            foundNodes.Add(nodeId);
                        }
                    }
                }
            }

            if (foundNodes.Any())
            {
                context.DoParallelFor(foundNodes.Select(x => new LoadNodeProtocolInfo(x)).ToArray());
                context.DoParallelFor(foundNodes.Select(x => new LoadNodeCapabilities(x)).ToArray());
            }
        }
    }
}
