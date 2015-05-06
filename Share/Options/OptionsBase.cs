namespace SmartLab.XBee.Options
{
    public class OptionsBase
    {
        public static OptionsBase DEFAULT
        {
            get { return new OptionsBase(); }
        }

        public static OptionsBase DisableRetriesRouteRepair
        {
            get
            {
                OptionsBase option = new OptionsBase();
                option.value = 0x01;
                return option;
            }
        }

        protected byte value;

        public bool GetDisableRetriesRouteRepair()
        {
            if ((this.value & 0x01) == 0x01)
                return true;
            else return false;
        }

        public void SetDisableRetriesRouteRepair(bool status)
        {
            if (status)
                this.value = (byte)(this.value | 0x01);
            else
                this.value = (byte)(this.value & 0xFE);
        }

        public OptionsBase() 
        {
            this.value = 0x00;
        }

        public byte GetValue() 
        {
            return this.value;
        }
    }
}
