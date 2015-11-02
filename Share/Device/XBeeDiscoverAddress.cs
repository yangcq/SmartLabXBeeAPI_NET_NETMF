using System;
using System.Text;
using SmartLab.XBee.Indicator;

namespace SmartLab.XBee.Device
{
    public class XBeeDiscoverAddress : Address
    {
        private int RSSI;
        protected string NIString;
        
        /// <summary>
        /// not apply to ZigBee Discovery
        /// </summary>
        /// <returns></returns>
        public int GetRSSI()
        {
            return RSSI;
        }

        public string GetNIString()
        {
            return NIString;
        }

        /// <summary>
        /// extension method for convert ND (with or without NI String) response to address 
        /// </summary>
        /// <param name="response">muset be non null parameter</param>
        /// <returns></returns>
        public static new XBeeDiscoverAddress Parse(ICommandResponse indicator)
        {
            if (indicator == null)
                return null;

            if (!indicator.GetRequestCommand().ToString().ToUpper().Equals("ND"))
                return null;

            int length = indicator.GetParameterLength();
            if (length < 10)
                return null;

            XBeeDiscoverAddress device = new XBeeDiscoverAddress();
            byte[] raw = indicator.GetParameter();
            Array.Copy(raw, 2, device.value, 0, 8);
            device.value[8] = raw[0];
            device.value[9] = raw[1];

            device.RSSI = raw[10] * -1;

            try
            {
                int nilength = length - 11;

                if (nilength <= 0)
                    device.NIString = string.Empty;
                else
                {
                    byte[] cache = new byte[nilength];
                    Array.Copy(raw, 11, cache, 0, nilength);
                    device.NIString = new string(UTF8Encoding.UTF8.GetChars(cache));
                }
            }
            catch { device.NIString = "error while encoding"; }

            return device;
        }
    }
}