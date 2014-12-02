namespace SmartLab.XBee.Status
{
    public enum CommandStatus
    {
        OK = 0x00,
        ERROR = 0x01,
        INVALID_COMMAND = 0x02,
        INVALID_Parameter = 0x03,
        TRANSMISSION_FAILED = 0x04,
    }
}
