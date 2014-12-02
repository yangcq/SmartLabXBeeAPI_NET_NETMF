namespace SmartLab.XBee.Status
{
    public enum ZigBeeDiscoveryStatus
    {
        NO_DISCOVERY_OVERHEAD = 0x00,
        ADDRESS_DISCOVERY = 0x01,
        ROUTE_DISCOVERY = 0x02,
        ADDRESS_AND_ROUTE = 0x03,
        EXTENED_TIMEOUT_DISCOVERY = 0x40,
    }
}
