namespace Services.ZWaveHost.Actions
{
    using System;
    using System.Threading;
    using Chains;
    using Chains.Play;

    public class JobActionWithSerializableData<TExpectedData> : RemotableActionWithSerializableData<JobData, TExpectedData, ZWaveContext>
    {
        private ZWaveContext currentContext;
        private TExpectedData returnObject;

        protected readonly AutoResetEvent AutoEvent = new AutoResetEvent(false);
        protected event Func<ZWaveContext, ZWaveMessage, TExpectedData> OnReceive;

        public JobActionWithSerializableData(JobData data)
            : base(data)
        {
        }

        protected override TExpectedData ActRemotely(ZWaveContext context)
        {
#if DEBUG
            context.WorkUnitContext.LogLine("Execute [" + this.GetType().FullName + "]");
#endif

            currentContext = context;
            context.EnqueueJob(
                new ZWaveJob(
                    Data.Function,
                    Data.NodeId,
                    Data.CommandClass,
                    Data.Command,
                    Data.Parameters,
                    new ZWaveEventHandler(
                        Data.ExpectedFunction, onPrivateReceive, Data.ExpectedCommandClass, Data.ExpectedCommand),
                    onCancel: onCancel));

            AutoEvent.WaitOne();

            return returnObject;
        }

        private void onPrivateReceive(ZWaveMessage message)
        {
            if (OnReceive != null)
            {
                returnObject = OnReceive(currentContext, message);
            }
            else
            {
                if (typeof(TExpectedData) == typeof(ZWaveContext))
                {
                    returnObject = (TExpectedData)Convert.ChangeType(currentContext, typeof(TExpectedData));
                }
            }

            AutoEvent.Set();
        }

        private void onCancel()
        {
            AutoEvent.Set();
        }
    }

    public class JobWithSerializableData : ReproducibleWithSerializableData<JobData>, IChainableAction<ZWaveContext, ZWaveContext>
    {
        private ZWaveContext currentContext;

        protected readonly AutoResetEvent AutoEvent = new AutoResetEvent(false);
        protected event Action<ZWaveContext, ZWaveMessage> OnReceive;

        public JobWithSerializableData(JobData data)
            : base(data)
        {
        }

        public ZWaveContext Act(ZWaveContext context)
        {
#if DEBUG
            context.WorkUnitContext.LogLine("Execute [" + this.GetType().FullName + "]");
#endif

            currentContext = context;
            context.EnqueueJob(
                new ZWaveJob(
                    Data.Function,
                    Data.NodeId,
                    Data.CommandClass,
                    Data.Command,
                    Data.Parameters,
                    new ZWaveEventHandler(Data.ExpectedFunction, onPrivateReceive, Data.ExpectedCommandClass, Data.ExpectedCommand),
                    onCancel: onCancel));

            AutoEvent.WaitOne();

            return context;
        }

        private void onPrivateReceive(ZWaveMessage message)
        {
            if (OnReceive != null)
            {
                OnReceive(currentContext, message);
            }

            AutoEvent.Set();
        }

        private void onCancel()
        {
            AutoEvent.Set();
        }
    }
}
