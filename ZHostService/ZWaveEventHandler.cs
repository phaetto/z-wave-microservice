namespace ZHostService
{
    using System;

    public sealed class ZWaveEventHandler
    {
        public const byte Empty = 0x00;

        private readonly ZWaveFunction function;
        private readonly Action<ZWaveMessage> handler;
        private readonly ZWaveCommandClass commandClass;
        private readonly ZWaveCommand command;

        public ZWaveEventHandler(ZWaveFunction function, Action<ZWaveMessage> handler, ZWaveCommandClass commandClass = Empty, ZWaveCommand command = Empty)
        {
            this.function = function;
            this.commandClass = commandClass;
            this.command = command;
            this.handler = handler;

            if (handler == null)
            {
                throw new ArgumentNullException("handler");
            }
        }

        public bool CanBeActivated(ZWaveMessage responseMessage)
        {
            if ((function == Empty || responseMessage.Function == function)
                && (commandClass == Empty || responseMessage.CommandClass == commandClass)
                && (command == Empty || responseMessage.Command == command))
            {
                return true;
            }

            return false;
        }

        public void Activate(ZWaveMessage responseMessage)
        {
            handler(responseMessage);
        }
    }
}
