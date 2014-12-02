namespace SmartLab.XBee.Options
{
    public class Tx16TransmitOptions : OptionsBase
    {
        //0x01 - Disable retries and route repair
        //0x02 - Force a long header to precede this packet
        //0x04 - Disable Sending of long header
        //0x08 - Invoke Traceroute

        public bool GetForceLongHeader()
        {
            if ((this.value & 0x02) == 0x02)
                return true;
            else return false;
        }

        public void SetForceLongHeader(bool status)
        {
            if (status)
                this.value = (byte)(this.value | 0x02);
            else
                this.value = (byte)(this.value & 0xFD);
        }

        public bool GetDisableLongHeader()
        {
            if ((this.value & 0x04) == 0x04)
                return true;
            else return false;
        }

        public void SetDisableLongHeader(bool status)
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

        public static Tx16TransmitOptions ForceLongHeader
        {
            get
            {
                Tx16TransmitOptions option = new Tx16TransmitOptions();
                option.value = 0x02;
                return option;
            }
        }

        public static Tx16TransmitOptions DisableLongHeader
        {
            get
            {
                Tx16TransmitOptions option = new Tx16TransmitOptions();
                option.value = 0x04;
                return option;
            }
        }

        public static Tx16TransmitOptions InvokeTraceroute
        {
            get
            {
                Tx16TransmitOptions option = new Tx16TransmitOptions();
                option.value = 0x08;
                return option;
            }
        }

        public Tx16TransmitOptions()
            : base()
        { }

        public Tx16TransmitOptions(bool disable_retries_and_route_repair, bool force_long_header, bool disable_long_header, bool invoke_traceroute)
        {
            value = 0x00;
            if (disable_retries_and_route_repair)
                value = (byte)(value | 0x01);
            if (force_long_header)
                value = (byte)(value | 0x02);
            if (disable_long_header)
                value = (byte)(value | 0x04);
            if (invoke_traceroute)
                value = (byte)(value | 0x08);
        }
    }
}
