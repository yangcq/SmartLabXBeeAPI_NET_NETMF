namespace SmartLab.XBee.Type
{
    public enum API_IDENTIFIER
    {
        Tx64_Request =0x00,
        Tx16_Request =0x01,
        AT_Command = 0x08,
        AT_Command_Queue_Parameter_Value = 0x09,
        ZigBee_Transmit_Request = 0x10,
        Explicit_Addressing_ZigBee_Command_Frame = 0x11,
        Remote_Command_Request = 0x17,
        Create_Source_Route = 0x21,
        Register_Joining_Device = 0x24,
        Rx64_Receive_Packet = 0x80,
        Rx16_Receive_Packet = 0x81,
        Rx64_IO_Data_Sample_Rx_Indicator = 0x82,
        Rx16_IO_Data_Sample_Rx_Indicator = 0x83,
        AT_Command_Response = 0x88,
        XBee_Transmit_Status = 0x89,
        Modem_Status = 0x8A,
        ZigBee_Transmit_Status = 0x8B,
        ZigBee_Receive_Packet = 0x90,
        ZigBee_Explicit_Rx_Indicator = 0x91,
        ZigBee_IO_Data_Sample_Rx_Indicator = 0x92,
        XBee_Sensor_Read_Indicato = 0x94,
        Node_Identification_Indicator = 0x95,
        Remote_Command_Response = 0x97,
        Over_the_Air_Firmware_Update_Status = 0xA0,
        Route_Record_Indicator = 0xA1,
        Device_Authenticated_Indicator = 0xA2,
        Many_to_One_Route_Request_Indicator = 0xA3,
    }
}