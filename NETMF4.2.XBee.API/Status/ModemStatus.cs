namespace SmartLab.XBee.Status
{
    public enum ModemStatus
    {
        HARDWARE_RESET = 0x00,
        WATCHDOG_TIMER_RESET = 0x01,
        JOINED_NETWORK = 0x02,
        DISASSOCIATED = 0x03,
        COORDINATOR_START = 0x06,
        NETWORK_SECURITY_KEY_WAS_UPDATED = 0x07,
        VOLTAGE_SPPLY_LIMIT_EXCEEDED = 0x0D,
        MODEM_CONFIGURATION_CHANGED_WHILE_JOIN_IN_PRIGRESS = 0x11,
        //0x80+
        STACK_ERROR = 0x80,
    }
}
