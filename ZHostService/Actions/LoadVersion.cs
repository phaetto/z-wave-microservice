namespace ZHostService.Actions
{
    public sealed class LoadVersion : Job
    {
        public LoadVersion(JobData jobData)
            : base(jobData)
        {
            OnReceive += onReceiveVersion;
        }

        public LoadVersion()
            : this(new JobData
                   {
                       Function = ZWaveFunction.GetVersion,
                       ExpectedFunction = ZWaveFunction.GetVersion
                   })
        {
        }

        private void onReceiveVersion(ZWaveContext context, ZWaveMessage message)
        {
            context.ProtocolVersion = Utils.ByteSubstring(message.Message, 11, 2);
            context.ApplicationVersion = Utils.ByteSubstring(message.Message, 13, 2);
        }
    }
}
