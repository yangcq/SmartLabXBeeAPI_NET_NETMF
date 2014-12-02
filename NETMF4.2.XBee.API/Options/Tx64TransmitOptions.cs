namespace SmartLab.XBee.Options
{
    public class Tx64TransmitOptions : OptionsBase
    {
        //0x01 - Disable retries and route repair (XTEND and XBEE)
        //0x02 - Don't repeat this packet (not implemented)
        //0x04 - Send packet with Broadcast Pan ID (XBEE only)
        //0x08 - Invoke Traceroute (XTEND version 8030 only)
        //0x10 (XB868DP) If the packet would be delayed due to duty cycle then purge it. All other bits must be set to 0

        public bool GetDonotRepeatPacket()
        {
            if ((this.value & 0x02) == 0x02)
                return true;
            else return false;
        }

        public void SetDonotRepeatPacket(bool status)
        {
            if (status)
                this.value = (byte)(this.value | 0x02);
            else
                this.value = (byte)(this.value & 0xFD);
        }

        public bool GetSendPacketWithBroadcastPanID()
        {
            if ((this.value & 0x04) == 0x04)
                return true;
            else return false;
        }

        public void SetSendPacketWithBroadcastPanID(bool status)
        {
            if (status)
                this.value = (byte)(this.value | 0x04);
            else
                this.value = (byte)(this.value & 0xFB);
        }

        public bool GetInvokeTraceroute()
        {
            if ((this.value & 0x08) == 0x08)
                return true;
            else return false;
        }

        public void SetInvokeTraceroute(bool status)
        {
            if (status)
                this.value = (byte)(this.value | 0x08);
            else
                this.value = (byte)(this.value & 0xF7);
        }

        public bool GetPurgePacketWhenDelayed()
        {
            if ((this.value & 0x10) == 0x10)
                return true;
            else return false;
        }

        public void SetPurgePacketWhenDelayed(bool status)
        {
            if (status)
                this.value = (byte)(this.value | 0x10);
            else
                this.value = (byte)(this.value & 0xEF);
        }

        public static Tx64TransmitOptions DonotRepeatPacket
        {
            get
            {
                Tx64TransmitOptions option = new Tx64TransmitOptions();
                option.value = 0x02;
                return option;
            }
        }

        public static Tx64TransmitOptions SendPacketWithBroadcastPanID
        {
            get
            {
                Tx64TransmitOptions option = new Tx64TransmitOptions();
                option.value = 0x04;
                return option;
            }
        }

        public static Tx64TransmitOptions InvokeTraceroute
        {
            get
            {
                Tx64TransmitOptions option = new Tx64TransmitOptions();
                option.value = 0x08;
                return option;
            }
        }

        public static Tx64TransmitOptions PurgePacketWhenDelayed
        {
            get
            {
                Tx64TransmitOptions option = new Tx64TransmitOptions();
                option.value = 0x10;
                return option;
            }
        }

        public Tx64TransmitOptions()
            : base()
        { }

        public Tx64TransmitOptions(bool disable_retries_and_route_repair, bool donot_repeat_packet, bool send_packet_with_broadcast_PanID, bool invoke_traceroute, bool purge_packet_if_delayed_due_to_duty_cycle)
        {
            value = 0x00;
            if (disable_retries_and_route_repair)
                value = (byte)(value | 0x01);
            if (donot_repeat_packet)
                value = (byte)(value | 0x02);
            if (send_packet_with_broadcast_PanID)
                value = (byte)(value | 0x04);
            if (invoke_traceroute)
                value = (byte)(value | 0x08);
            if (purge_packet_if_delayed_due_to_duty_cycle)
                value = (byte)(value | 0x10);
        }
    }
}
