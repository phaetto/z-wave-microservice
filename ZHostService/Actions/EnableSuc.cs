namespace ZHostService.Actions
{
    public sealed class EnableSuc : Job
    {
        public EnableSuc()
            : base(new JobData
                   {
                       Function = ZWaveFunction.EnableSuc
                   })
        {
        }
    }
}
