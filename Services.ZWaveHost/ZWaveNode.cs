namespace Services.ZWaveHost
{
    using System.Collections.Generic;

    public sealed class ZWaveNode
    {
        public byte NodeId { get; set; }

        public bool Sleeping { get; set; }

        public ZWaveType.Basic Basictype { get; set; }

        public ZWaveType.Generic GenericType { get; set; }

        public ZWaveType.Specific SpecificType { get; set; }

        public List<ZWaveCommandClass> Capabilities { get; set; }
    }
}
