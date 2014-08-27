namespace Services.ZWaveHost.Actions
{
    public sealed class SetSucNodeid : Job
    {
        public SetSucNodeid(byte nodeId)
            : base(new JobData
                   {
                       Function = ZWaveFunction.SetSucNodeId,
                       ExpectedFunction = ZWaveFunction.SetSucNodeId,
                       Parameters = new byte[]
                                    {
                                        nodeId, 0x01, 0x00, (byte)ZWaveFunction.NodeIdServer
                                    }
                   })
        {
        }
    }
}
