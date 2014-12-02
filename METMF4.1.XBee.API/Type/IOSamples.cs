using System.Collections;

namespace SmartLab.XBee.Type
{
    public class IOSamples
    {
        private int SUPPLY_VOLTAGE;
        private Hashtable Analog;
        private Hashtable Digital;

        public IOSamples(Hashtable Analog, Hashtable Digital, int SUPPLY_VOLTAGE) 
        {
            this.Analog = Analog;
            this.Digital = Digital;
            this.SUPPLY_VOLTAGE = SUPPLY_VOLTAGE;
        }

        public Hashtable GetAnalogs()
        {
            return this.Analog;
        }

        public int GetAnalog(Device.Pin Pin)
        {
            if (this.Analog.Contains(Pin))
                return (int)Analog[Pin];
            return 0;
        }

        public Device.Pin.Status GetDigital(Device.Pin Pin)
        {
            if (this.Analog.Contains(Pin))
                return (Device.Pin.Status)Analog[Pin];
            return Device.Pin.Status.UNMONITORED;
        }

        public Hashtable GetDigitals()
        {
            return this.Digital;
        }

        public int GetSupplyVoltage()
        {
            return this.SUPPLY_VOLTAGE;
        }
    }
}