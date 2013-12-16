
namespace ZHostService
{
    using System;
    using System.Collections.Generic;
    using System.Timers;

    public class ZWaveJob
    {
        private Timer timeout;

        private readonly ZWaveEventHandler eventHandler;

        public readonly ZWaveMessage Request;

        private readonly int timeoutInterval;

        private readonly Action onCancel;

        public ZWaveJob(
            ZWaveFunction function,
            byte nodeId = 0x00,
            ZWaveCommandClass commandClass = 0x00,
            ZWaveCommand command = 0x00,
            byte[] parameters = null,
            ZWaveEventHandler eventHandler = null,
            int timeoutInterval = 10000,
            Action onCancel = null)
        {
            this.eventHandler = eventHandler;
            this.timeoutInterval = timeoutInterval;
            this.onCancel = onCancel;
            Request = new ZWaveMessage(ZWaveMessageType.Request, function, nodeId, commandClass, command);

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    Request.AddParameter(param);
                }
            }
        }

        public bool JobStarted { get; set; }
        public ZWaveMessage Response { get; private set; }
        public bool JobDone { get; set; } public bool AwaitAck { get; set; }
        public bool AwaitResponse { get; set; }
        public int SendCount { get; set; }
        public bool Resend { get; set; }

        public void CancelJob()
        {
            Console.WriteLine("*** Canceled");

            this.Done();
            this.AwaitAck = false;
            this.AwaitResponse = false;

            if (onCancel != null)
            {
                onCancel();
            }
        }

        public void TriggerResend()
        {
            Console.WriteLine("*** Trigger resend");

            this.RemoveTimeout();
            this.AwaitAck = false;
            this.AwaitResponse = false;
            this.Resend = true;
        }

        public void SetTimeout(int interval)
        {
            this.timeout = new Timer(interval);
            this.timeout.Elapsed += Timeout;
            this.timeout.Start();
        }

        public void RemoveTimeout()
        {
            if (this.timeout != null)
            {
                this.timeout.Elapsed -= Timeout;
                this.timeout.Dispose();
                this.timeout = null;
            }
        }

        private void Timeout(object sender, EventArgs e)
        {
            this.TriggerResend();
        }

        public void Done()
        {
            this.JobDone = true;
            this.RemoveTimeout();
        }

        public void Start()
        {
            this.JobStarted = true;
            this.SetTimeout(timeoutInterval);
        }

        public virtual bool CanBeActivated(ZWaveMessage response)
        {
            return (eventHandler == null || eventHandler.CanBeActivated(response));
        }

        public virtual void Activate(ZWaveMessage response)
        {
            Done();

            if (eventHandler != null)
            {
                Response = response;
                eventHandler.Activate(response);
            }
        }
    }
}
