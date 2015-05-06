namespace SmartLab.XBee.Options
{
    public class TransmitOptions : OptionsBase
    {
        //Default = 0x00;
        //Disable_Retries_Route_Repair = 0x01;
        //Enable_APS = 0x20;
        //Use_Extended_Timeout = 0x40;

        public bool GetEnableAPS()
        {
            if ((this.value & 0x20) == 0x20)
                    return true;
                else return false;
        }

        public void SetEnableAPS(bool status)
        {
            if (status)
                this.value = (byte)(this.value | 0x20);
            else
                this.value = (byte)(this.value & 0xDF);
        }

        public bool GetUseExtendedTimeout()
        {
            if ((this.value & 0x40) == 0x40)
                return true;
            else return false;
        }

        public void SetUseExtendedTimeout(bool status)
        {
            if (status)
                this.value = (byte)(this.value | 0x40);
            else
                this.value = (byte)(this.value & 0xBF);
        }

        public static TransmitOptions EnableAPS
        {
            get
            {
                TransmitOptions option = new TransmitOptions();
                option.value = 0x20;
                return option;
            }
        }

        public static TransmitOptions UseExtendedTimeout
        {
            get
            {
                TransmitOptions option = new TransmitOptions();
                option.value = 0x40;
                return option;
            }
        }

        public TransmitOptions()
            : base()
        { }

        public TransmitOptions(bool disable_retries_and_route_repair, bool enable_APS_encryption, bool use_extended_transmission_timeout) 
        {
            value = 0x00;
            if (disable_retries_and_route_repair)
                value = (byte)(value | 0x01);
            if (enable_APS_encryption)
                value = (byte)(value | 0x20);
            if (use_extended_transmission_timeout)
                value = (byte)(value | 0x40);
        }
    }
}