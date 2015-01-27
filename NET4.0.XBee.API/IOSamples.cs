using System.Collections;
using SmartLab.XBee.Device;

namespace SmartLab.XBee
{
    public class IOSamples
    {
        private int SUPPLY_VOLTAGE;
        private Hashtable analog;
        private Hashtable digital;

        /// <summary>
        /// SUPPLY_VOLTAGE not required by XBee
        /// </summary>
        /// <param name="analog"></param>
        /// <param name="digital"></param>
        /// <param name="SUPPLY_VOLTAGE"></param>
        public IOSamples(Hashtable analog, Hashtable digital, int SUPPLY_VOLTAGE = 0) 
        {
            this.analog = analog;
            this.digital = digital;
            this.SUPPLY_VOLTAGE = SUPPLY_VOLTAGE;
        }

        public Hashtable GetAnalogs()
        {
            return this.analog;
        }

        public int GetAnalog(Pin pin)
        {
            if (this.analog.Contains(pin))
                return (int)analog[pin];
            return 0;
        }

        public Device.Pin.Status GetDigital(Pin pin)
        {
            if (this.analog.Contains(pin))
                return (Device.Pin.Status)analog[pin];
            return Device.Pin.Status.UNMONITORED;
        }

        public Hashtable GetDigitals()
        {
            return this.digital;
        }

        /// <summary>
        /// only avaliable in ZigBee device and when it is battery powered
        /// </summary>
        /// <returns></returns>
        public int GetSupplyVoltage()
        {
            return this.SUPPLY_VOLTAGE;
        }
    }
}