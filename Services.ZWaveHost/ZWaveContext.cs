namespace Services.ZWaveHost
{
    using System;
    using System.Collections.Generic;
    using System.IO.Ports;
    using System.Threading;
    using Chains;
    using Services.Management.Administration.Worker;
    using Services.ZWaveHost.Actions;

    public class ZWaveContext : Chain<ZWaveContext>, IWorkerEvents, IDisposable
    {
        private readonly SerialPort serialPort;

        private readonly Thread runnerThread;

        private readonly Object queueLock = new Object();

        private readonly LinkedList<ZWaveJob> jobQueue = new LinkedList<ZWaveJob>();

        internal readonly List<ZWaveEventHandler> BroadcastingHandlers = new List<ZWaveEventHandler>();

        public byte ControllerId = 0x00;

        public byte[] HomeId;

        public byte[] ProtocolVersion;

        public byte[] ApplicationVersion;

        public readonly WorkUnitContext WorkUnitContext;

        public List<ZWaveNode> Nodes = new List<ZWaveNode>();

        public void OnStart()
        {
            try
            {
                Open();

                var existingSucNode = Do(new LoadVersion()).Do(new LoadMemoryId()).Do(new GetSucNodeId());

                if (ControllerId != existingSucNode)
                {
                    Do(new EnableSuc()).Do(new SetSucNodeid(ControllerId));
                }

                Do(new SerialApiInitData());
            }
            catch (Exception exception)
            {
                WorkUnitContext.LogException(exception);
                WorkUnitContext.ReportToAdmin();
                WorkUnitContext.Stop();
            }

            // Ready event here
        }

        public void OnStop()
        {
            Close();
        }

        public ZWaveContext(WorkUnitContext workUnitContext = null)
        {
            this.WorkUnitContext = workUnitContext;

            this.serialPort = new SerialPort
                              {
                                  Parity = Parity.None,
                                  BaudRate = 115200,
                                  Handshake = Handshake.None,
                                  StopBits = StopBits.One,
                                  DtrEnable = true,
                                  RtsEnable = true,
                                  NewLine = Environment.NewLine
                              };

            this.runnerThread = new Thread(Run);
        }

        public void Open()
        {
            if (this.serialPort.IsOpen)
            {
                return;
            }

            var portPrefix = "COM";
            var startNumber = 1;
            if (AbstractChain.IsMono)
            {
                portPrefix = "/dev/ttyUSB";
                startNumber = 0;
            }

            for (var i = startNumber; i < 5; i++)
            {
                WorkUnitContext.LogLine("Opening port: " + this.serialPort.PortName);

                var port = portPrefix + i;
                this.serialPort.PortName = port;

                try
                {
                    this.serialPort.Open();
                }
                catch
                {
                }

                if (this.serialPort.IsOpen)
                {
                    break;
                }
            }

            if (this.serialPort.IsOpen)
            {
                WorkUnitContext.LogLine("Found ZWave controller at port: " + this.serialPort.PortName);
                this.runnerThread.Start();

                return;
            }

            throw new InvalidProgramException("Could not find a zwave controller.");
        }

        public void Close()
        {
            try
            {
                this.serialPort.Close();
                this.serialPort.Dispose();
            }
            catch
            {
            }
        }

        private void Run()
        {
            var buf = new byte[1024];
            while (this.serialPort.IsOpen)
            {
                ZWaveJob currentJob = null;
                lock (this.queueLock)
                {
                    if (this.jobQueue.Count > 0)
                    {
                        currentJob = this.jobQueue.First.Value;
                        if (currentJob.JobDone)
                        {
                            this.jobQueue.RemoveFirst();
                            currentJob = null;
                            if (this.jobQueue.Count > 0)
                            {
                                currentJob = this.jobQueue.First.Value;
                            }
                        }
                    }
                }

                // Check for incoming messages
                var btr = this.serialPort.BytesToRead;
                if (btr > 0)
                {
                    // Read first byte
                    this.serialPort.Read(buf, 0, 1);
                    switch ((ZWaveProtocol)buf[0])
                    {
                        case ZWaveProtocol.Sof:

                            // Read the length byte
                            this.serialPort.Read(buf, 1, 1);
                            byte len = buf[1];

                            // Read rest of the frame
                            this.serialPort.Read(buf, 2, len);
                            var message = Utils.ByteSubstring(buf, 0, (len + 2));
#if DEBUG
                            WorkUnitContext.LogLine("Received: " + Utils.ByteArrayToString(message));
#endif
                            // Verify checksum
                            if (message[(message.Length - 1)]
                                == CalculateChecksum(Utils.ByteSubstring(message, 0, (message.Length - 1))))
                            {
                                var zwaveMessage = new ZWaveMessage(message);

                                if (currentJob != null && currentJob.AwaitResponse)
                                {
                                    if (currentJob.AwaitAck)
                                    {
                                        // We wanted an ACK instead. Resend...
                                        currentJob.AwaitAck = false;
                                        currentJob.AwaitResponse = false;
                                        currentJob.Resend = true;
                                    }
                                    else
                                    {
                                        // Activate job/action ability to response here
                                        if (currentJob.CanBeActivated(zwaveMessage))
                                        {
                                            ThreadPool.QueueUserWorkItem(x => currentJob.Activate(zwaveMessage));
                                        }
                                        else
                                        {
                                            ReceiveBroadcastedMessage(zwaveMessage);
                                        }
                                    }
                                }
                                else
                                {
                                    // Broadcasted - assign this to the proper class/listener and execute
                                    ReceiveBroadcastedMessage(zwaveMessage);
                                }

                                // Send ACK - Checksum is correct
                                this.serialPort.Write(
                                    new[]
                                    {
                                        (byte)ZWaveProtocol.Ack
                                    },
                                    0,
                                    1);

#if DEBUG
                                WorkUnitContext.LogLine("Sent: ACK");
#endif
                            }
                            else
                            {
                                // Send NAK
                                this.serialPort.Write(
                                    new[]
                                    {
                                        (byte)ZWaveProtocol.Nak
                                    },
                                    0,
                                    1);
#if DEBUG
                                WorkUnitContext.LogLine("Sent: NACK");
#endif
                            }

                            break;
                        case ZWaveProtocol.Can:
#if DEBUG
                            WorkUnitContext.LogLine("Received: CAN");
#endif
                            break;
                        case ZWaveProtocol.Nak:
#if DEBUG
                            WorkUnitContext.LogLine("Received: NAK");
#endif
                            currentJob.AwaitAck = false;
                            currentJob.JobStarted = false;
                            break;
                        case ZWaveProtocol.Ack:
#if DEBUG
                            WorkUnitContext.LogLine("Received: ACK");
#endif
                            if (currentJob != null)
                            {
                                if (currentJob.AwaitAck && !currentJob.AwaitResponse)
                                {
                                    currentJob.AwaitResponse = true;
                                    currentJob.AwaitAck = false;
                                }
                            }
                            break;
                        default:
                            WorkUnitContext.LogException(new InvalidOperationException("Critical error. Out of frame flow."));
                            break;
                    }
                }
                else
                {
                    if (currentJob == null)
                    {
                        lock (this.queueLock)
                        {
                            if (this.jobQueue.Count > 0)
                            {
                                currentJob = this.jobQueue.First.Value;
                            }
                        }
                    }

                    if (currentJob != null)
                    {
                        if (currentJob.SendCount >= 3)
                        {
                            currentJob.CancelJob();
                        }

                        if ((!currentJob.JobStarted && !currentJob.JobDone) || currentJob.Resend)
                        {
                            var msg = currentJob.Request;
                            if (msg != null)
                            {
                                this.serialPort.Write(msg.Message, 0, msg.Message.Length);
                                currentJob.Start();
                                currentJob.AwaitResponse = false;
                                currentJob.Resend = false;
                                currentJob.AwaitAck = true;
                                currentJob.SendCount++;
#if DEBUG
                                WorkUnitContext.LogLine("Sent: " + Utils.ByteArrayToString(msg.Message));
#endif
                            }
                        }
                    }
                }

                Thread.Sleep(100);
            }
        }

        private void ReceiveBroadcastedMessage(ZWaveMessage zwaveMessage)
        {
            foreach (var handler in BroadcastingHandlers)
            {
                if (handler.CanBeActivated(zwaveMessage))
                {
                    handler.Activate(zwaveMessage);
                }
            }
        }

        internal void EnqueueJob(ZWaveJob job)
        {
            lock (this.queueLock)
            {
                this.jobQueue.AddLast(job);
            }
        }

        internal void InjectJob(ZWaveJob job)
        {
            lock (this.queueLock)
            {
                this.jobQueue.AddFirst(job);
            }
        }

        internal static byte CalculateChecksum(byte[] message)
        {
            byte chksum = 0xff;
            for (var i = 1; i < message.Length; i++)
            {
                chksum ^= message[i];
            }

            return chksum;
        }

        public void Dispose()
        {
            Close();
        }
    }
}
