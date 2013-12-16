namespace ZHostService
{
    using System.Collections.Generic;

    public sealed class ZWaveMessage
    {
        // I do no know if those are the real names; it should add to 0x05
        private const ZWaveProtocol TransmissionOptions = ZWaveProtocol.Sof | ZWaveProtocol.AutoRoute;

        private readonly bool raw;

        private readonly List<byte> _params = new List<byte>();

        private readonly byte[] message;

        public ZWaveMessageType MessageType { get; private set; }

        public ZWaveFunction Function { get; private set; }

        public byte NodeId { get; private set; }

        public ZWaveCommandClass CommandClass { get; private set; }

        public ZWaveCommand Command { get; private set; }

        public byte[] Message
        {
            get
            {
                if (!this.raw)
                {
                    int length = 6;
                    if (this.NodeId != 0x00) length++;
                    if (this.CommandClass != 0x00) length += 3;
                    length += this._params.Count;

                    var message = new byte[length];
                    int index = 0;

                    message[index++] = (byte)ZWaveProtocol.Sof;

                    // Insert message length
                    message[index++] = (byte)(length - 2);

                    message[index++] = (byte)this.MessageType;
                    message[index++] = (byte)this.Function;

                    if (this.NodeId != 0x00)
                    {
                        message[index++] = this.NodeId;
                    }

                    if (this.CommandClass != 0x00)
                    {
                        message[index++] = (byte)(2 + _params.Count);
                        message[index++] = (byte)this.CommandClass;
                        message[index++] = (byte)this.Command;
                    }

                    for (int i = 0; i < this._params.Count; i++)
                    {
                        message[index++] = this._params[i];
                    }

                    message[index++] = (byte)TransmissionOptions;

                    // Calculate and insert the checksum
                    message[index++] = ZWaveContext.CalculateChecksum(message);

                    return message;
                }

                return this.message;
            }
        }

        public void AddParameter(byte param)
        {
            this._params.Add(param);
        }

        public void Parse(byte[] message)
        {
            this.MessageType = (ZWaveMessageType)message[2];
            this.Function = (ZWaveFunction)message[3];

            switch (this.Function)
            {
                case ZWaveFunction.SendData:
                    this.CommandClass = (ZWaveCommandClass)message[4];
                    break;
                case ZWaveFunction.ApplicationCommandHandler:
                    this.CommandClass = (ZWaveCommandClass)message[7];
                    this.Command = (ZWaveCommand)message[8];
                    break;
                case ZWaveFunction.AddNodeToNetwork:
                    this.CommandClass = (ZWaveCommandClass)message[5];
                    break;
                case ZWaveFunction.RemoveNodeFromNetwork:
                    this.CommandClass = (ZWaveCommandClass)message[5];
                    break;
                default:
                    this.CommandClass = 0x00;
                    break;
            }
        }

        public ZWaveMessage() { }

        public ZWaveMessage(ZWaveMessageType messageType, ZWaveFunction function, byte nodeId = 0x00, ZWaveCommandClass commandClass = 0x00, ZWaveCommand command = 0x00)
        {
            this.MessageType = messageType;
            this.Function = function;
            if (nodeId != 0x00)
            {
                this.NodeId = nodeId;
            }

            if (commandClass != 0x00)
            {
                this.CommandClass = commandClass;
            }

            if (command != 0x00)
            {
                this.Command = command;
            }
        }

        public ZWaveMessage(byte[] message)
        {
            this.message = message;
            this.raw = true;
            this.Parse(message);
        }
    }
}
