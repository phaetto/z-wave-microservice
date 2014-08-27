namespace Services.ZWaveHost
{
    internal enum ZWaveProtocol : byte
    {
        Sof = 0x01,
        Ack = 0x06,
        Nak = 0x15,
        Can = 0x18,
        AutoRoute = 0x4,
    }

    public enum ZWaveMessageType : byte
    {
        Request = 0x00,
        Response = 0x01,
    }

    public enum ZWaveCommandClass : byte
    {
        // FUNCTION ADD_NODE_TO_NETWORK
        NodeAny = 0x01,
        AddNodeLearnReady = 0x01,
        AddNodeStatusFound = 0x02,
        AddNodeStatusProtocolDone = 0x05,
        AddNodeStop = 0x05,
        AddNodeStatusAddingSlave = 0x03,
        AddNodeStatusDone = 0x06,
        AddNodeStatusFailed = 0x07,

        // FUNCTION REMOVE_NODE_FROM_NETWORK
        RemoveNodeStop = 0x05,

        // Imported
        Alarm = 0x71,
        ApplicationStatus = 0x22,
        AssociationCommandConfiguration = 0x9B,
        Association = 0x85,
        AvContentDirectoryMd = 0x95,
        AvContentSearchMd = 0x97,
        AvRendererStatus = 0x96,
        AvTaggingMd = 0x99,
        BasicWindowCovering = 0x50,
        Basic = 0x20,
        Battery = 0x80,
        ChimneyFan = 0x2A,
        ClimateControlSchedule = 0x46,
        Clock = 0x81,
        Composite = 0x8D,
        Configuration = 0x70,
        ControllerReplication = 0x21,
        DoorLock = 0x62,
        DoorLockLogging = 0x4C,
        EnergyProduction = 0x90,
        FirmwareUpdateMd = 0x7A,
        GeographicLocation = 0x8C,
        GroupingName = 0x7B,
        Hail = 0x82,
        Indicator = 0x87,
        IpConfiguration = 0x9A,
        Language = 0x89,
        Lock = 0x76,
        ManufacturerProprietary = 0x91,
        ManufacturerSpecific = 0x72,
        Mark = 0xEF,
        MeterPulse = 0x35,
        Meter = 0x32,
        MtpWindowCovering = 0x51,
        MultiChannelAssociationV2 = 0x8E,
        MultiChannelV2 = 0x60,
        MultiCmd = 0x8F,
        MultiInstanceAssociation = 0x8E,
        MultiInstance = 0x60,
        NoOperation = 0x00,
        NodeNaming = 0x77,
        NonInteroperable = 0xF0,
        Powerlevel = 0x73,
        Proprietary = 0x88,
        Protection = 0x75,
        RemoteAssociationActivate = 0x7C,
        RemoteAssociation = 0x7D,
        SceneActivation = 0x2B,
        SceneActuatorConf = 0x2C,
        SceneControllerConf = 0x2D,
        ScheduleEntryLock = 0x4E,
        ScreenAttributes = 0x93,
        ScreenMd = 0x92,
        Security = 0x98,
        SensorAlarm = 0x9C,
        SensorBinary = 0x30,
        SensorConfiguration = 0x9E,
        SensorMultilevel = 0x31,
        SilenceAlarm = 0x9D,
        SimpleAvControl = 0x94,
        SwitchAll = 0x27,
        SwitchBinary = 0x25,
        SwitchMultilevel = 0x26,
        SwitchToggleBinary = 0x28,
        SwitchToggleMultilevel = 0x29,
        ThermostatFanMode = 0x44,
        ThermostatFanState = 0x45,
        ThermostatHeating = 0x38,
        ThermostatMode = 0x40,
        ThermostatOperatingState = 0x42,
        ThermostatSetback = 0x47,
        ThermostatSetpoint = 0x43,
        TimeParameters = 0x8B,
        Time = 0x8A,
        UserCode = 0x63,
        Version = 0x86,
        WakeUp = 0x84,
        ZipAdvClient = 0x34,
        ZipAdvServer = 0x33,
        ZipAdvServices = 0x2F,
        ZipClient = 0x2E,
        ZipServer = 0x24,
        ZipServices = 0x23,
    }

    public enum ZWaveCommand : byte
    {
        Set = 0x01,
        Get = 0x02,
        Report = 0x03,
        ConfigurationSet = 0x04,
        ConfigurationGet = 0x05,
        ConfigurationReport = 0x06,
    }

    public enum ZWaveFunction : byte
    {
        SendData = 0x13,
        ApplicationCommandHandler = 0x04,
        GetVersion = 0x15,
        MemoryGetId = 0x20,
        SerialApiGetCapabilities = 0x07,
        SerialApiInitData = 0x02,
        GetNodeProtocolInfo = 0x41,
        GetNodeCapabilities = 0x60,
        GetNodeCapabilitiesResponse = 0x49,
        GetSucNodeId = 0x56,
        SetSucNodeId = 0x54,
        EnableSuc = 0x52,
        NodeIdServer = 0x01,
        SetDefault = 0x42,
        AddNodeToNetwork = 0x4A,
        RemoveNodeFromNetwork = 0x4B,
        RequestNodeNeighborUpdate = 0x48,
    }

    public class ZWaveType
    {
        public enum Basic : byte
        {
            Controller = 0x01,
            StaticController = 0x02,
            Slave = 0x03,
            RoutingSlave = 0x04,
        }

        public enum Generic : byte
        {
            GenericController = 0x01,
            StaticController = 0x02,
            SwitchBinary = 0x10,
            SwitchMultilevel = 0x11,
            SensorBinary = 0x20,
            SensorMultilevel = 0x21,
            Meter = 0x31,
        }

        public enum Specific : byte
        {
            PowerSwitchBinary = 0x01,
        }
    }
}