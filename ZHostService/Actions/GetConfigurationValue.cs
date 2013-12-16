namespace ZHostService.Actions
{
    using System;

    public sealed class GetConfigurationValue : JobActionWithSerializableData<int>
    {
        public GetConfigurationValue(byte nodeId, byte configurationId)
            : base(new JobData
                   {
                       Function = ZWaveFunction.SendData,
                       NodeId = nodeId,
                       CommandClass = ZWaveCommandClass.Configuration,
                       Command = ZWaveCommand.ConfigurationGet,
                       ExpectedFunction = ZWaveFunction.ApplicationCommandHandler,
                       ExpectedCommand = ZWaveCommand.ConfigurationReport,
                       Parameters = new[]
                                    {
                                        configurationId
                                    }
                   })
        {
            OnReceive += GetConfigurationValue_onReceive;
        }

        private int GetConfigurationValue_onReceive(ZWaveContext context, ZWaveMessage message)
        {
            switch (message.Message[10])
            {
                case 0x1:
                    return message.Message[11];
                case 0x2:
                    return (message.Message[11]<<8) & message.Message[12];
                case 0x4:
                    return (message.Message[11] << 32) & (message.Message[12] << 16) & (message.Message[13] << 8) & message.Message[14];
            }

            throw new InvalidOperationException(
                "Size [" + message.Message[10] + "] for config value [" + message.Message[9] + "] was not accepted");
        }
    }
}
