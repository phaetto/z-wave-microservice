﻿namespace ZHostService.Actions
{
    public sealed class LoadMemoryId : Job
    {
        public LoadMemoryId()
            : base(new JobData
                   {
                       Function = ZWaveFunction.MemoryGetId,
                       ExpectedFunction = ZWaveFunction.MemoryGetId
                   })
        {
            OnReceive += LoadMemoryId_OnReceive;
        }

        private void LoadMemoryId_OnReceive(ZWaveContext context, ZWaveMessage message)
        {
            context.HomeId = Utils.ByteSubstring(message.Message, 4, 4);
            context.ControllerId = message.Message[8];
        }
    }
}