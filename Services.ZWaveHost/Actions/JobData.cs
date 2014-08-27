namespace Services.ZWaveHost.Actions
{
    using System;
    using Chains.Play;

    [Serializable]
    public class JobData : SerializableSpecification
    {
        public ZWaveFunction Function;
        public ZWaveFunction ExpectedFunction;
        public byte NodeId;
        public ZWaveCommandClass CommandClass = 0x00;
        public ZWaveCommandClass ExpectedCommandClass = 0x00;
        public ZWaveCommand Command = 0x00;
        public ZWaveCommand ExpectedCommand = 0x00;
        public byte[] Parameters;

        public override int DataStructureVersionNumber
        {
            get
            {
                return 1;
            }
        }
    }
}
