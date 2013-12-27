namespace ZHostService.Actions
{
    public sealed class LoadMemoryId : Job
    {
        public LoadMemoryId(JobData jobData)
            : base(jobData)
        {
            OnReceive += LoadMemoryId_OnReceive;
        }

        public LoadMemoryId()
            : this(new JobData
                   {
                       Function = ZWaveFunction.MemoryGetId,
                       ExpectedFunction = ZWaveFunction.MemoryGetId
                   })
        {
        }

        private void LoadMemoryId_OnReceive(ZWaveContext context, ZWaveMessage message)
        {
            context.HomeId = Utils.ByteSubstring(message.Message, 4, 4);
            context.ControllerId = message.Message[8];
        }
    }
}
