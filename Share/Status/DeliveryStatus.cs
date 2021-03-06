namespace SmartLab.XBee.Status
{
    public enum DeliveryStatus
    {
        SUCCESS = 0x00,
        MAC_ACK_FAILURE = 0x01,
        CCA_FAILURE = 0x02,
        TRANSMISSION_WAS_PURGED = 0x03,
        PHYSICAL_ERROR_OCCURRED_ON_THE_INTERFACE_WITH_THE_WIFI_TRANSCEIVER = 0x04,
        INVALID_DESTINATION_ENDPOINT = 0x15,
        NO_BUFFERS = 0x18,
        NETWORK_ACK_FAILURE = 0x21,
        NOT_JOINED_TO_NETWORK = 0x22,
        SELF_ADDRESSED = 0x23,
        ADDRESS_NOT_FOUND = 0x24,
        ROUTE_NOT_FOUND = 0x25,
        BROADCAST_SOURCE_FAILED_TO_HEAR_A_NEIGBOR_RELAY_THE_MESSAGE = 0x26,
        INVALID_BINDING_TABLE_INDEX = 0x2B,
        INVALID_ENDPOINT = 0x2C,
        ATTEMPTED_BROADCAST_WITH_APS_TRANSMISSION = 0x2D,
        ATTEMPTED_UNICAST_WITH_APS_TRANSMISSION_BUT_EE_0 = 0x2E,
        SOFTWARE_ERROR_OCCURRED = 0x31,
        RESOURCE_ERROR_LACK_OF_FREE_BUFFERS_TIMERS_ETC = 0x32,
        DATA_PAYLOAD_TOO_LARGE = 0x74,
        INDIRECT_MESSAGE_UNREQUESTED = 0x75,
        ATTEMPT_TO_CREATE_A_CLIENT_SOCKET_FAILED = 0x76,
        KEY_NOT_AUTHORIZED = 0xBB,
    }
}
