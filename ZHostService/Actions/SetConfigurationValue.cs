﻿namespace ZHostService.Actions
{
    public sealed class SetConfigurationValue : Job
    {
        public SetConfigurationValue(byte nodeId, byte configurationId, byte parameter)
            : base(new JobData
                   {
                       Function = ZWaveFunction.SendData,
                       ExpectedFunction = ZWaveFunction.SendData,
                       NodeId = nodeId,
                       CommandClass = ZWaveCommandClass.Configuration,
                       Command = ZWaveCommand.ConfigurationSet,
                       Parameters = new byte[]
                                    {
                                        configurationId, 1, parameter
                                    }
                   })
        {
        }

        public SetConfigurationValue(JobData data)
            : base(data)
        {
        }
    }
}