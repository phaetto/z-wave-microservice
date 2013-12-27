namespace ZHostService.Actions
{
    public sealed class EnableSuc : Job
    {
        public EnableSuc(JobData jobData)
            : base(jobData)
        {
        }

        public EnableSuc()
            : base(new JobData
                   {
                       Function = ZWaveFunction.EnableSuc
                   })
        {
        }
    }
}
