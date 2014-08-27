namespace Services.ZWaveHost
{
    using System.Collections.Generic;

    public sealed class ZWaveMessage
    {
        private const ZWaveProtocol TransmissionOptions = ZWaveProtocol.Sof | ZWaveProtocol.AutoRoute;

        private readonly bool raw;

        private readonly byte[] message;

        public List<byte> Params { get; private set; }

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
                    var length = 6;
                    if (this.NodeId != 0x00) length++;
                    if (this.CommandClass != 0x00) length += 3;
                    length += this.Params.Count;

                    var message = new byte[length];
                    var index = 0;

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
                        message[index++] = (byte)(2 + Params.Count);
                        message[index++] = (byte)this.CommandClass;
                        message[index++] = (byte)this.Command;
                    }

                    for (int i = 0; i < this.Params.Count; i++)
                    {
                        message[index++] = this.Params[i];
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
            Params.Add(param);
        }

        public void Parse(byte[] message)
        {
            MessageType = (ZWaveMessageType)message[2];
            Function = (ZWaveFunction)message[3];

            switch (Function)
            {
                case ZWaveFunction.SendData:
                    CommandClass = (ZWaveCommandClass)message[4];
                    break;
                case ZWaveFunction.ApplicationCommandHandler:
                    NodeId = message[5];
                    var lengthOfData = message[6] - 2;
                    CommandClass = (ZWaveCommandClass)message[7];
                    Command = (ZWaveCommand)message[8];
                    Params = new List<byte>(Utils.ByteSubstring(message, 8, lengthOfData));
                    break;
                case ZWaveFunction.AddNodeToNetwork:
                    CommandClass = (ZWaveCommandClass)message[5];
                    break;
                case ZWaveFunction.RemoveNodeFromNetwork:
                    CommandClass = (ZWaveCommandClass)message[5];
                    break;
                default:
                    CommandClass = 0x00;
                    break;
            }
        }

        public ZWaveMessage()
        {
            Params = new List<byte>();            
        }

        public ZWaveMessage(
            ZWaveMessageType messageType,
            ZWaveFunction function,
            byte nodeId = 0x00,
            ZWaveCommandClass commandClass = 0x00,
            ZWaveCommand command = 0x00)
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

            Params = new List<byte>();
        }

        public ZWaveMessage(byte[] message)
        {
            this.message = message;
            this.raw = true;
            this.Parse(message);
        }
    }
}
