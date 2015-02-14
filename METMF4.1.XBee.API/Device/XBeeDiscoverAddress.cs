using System.Text;
using SmartLab.XBee.Response;
using System;

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
        /// extension method for convert DN / ND (with or without NI String) response to address 
        /// </summary>
        /// <param name="response">muset be non null parameter</param>
        /// <returns></returns>
        public static new XBeeDiscoverAddress Parse(CommandResponseBase response)
        {
            if (response == null)
                return null;

            if (response.GetRequestCommand().ToString().ToUpper() != "ND")
                return null;

            int length = response.GetParameterLength();
            if (length <= 0)
                return null;

            XBeeDiscoverAddress device = new XBeeDiscoverAddress();

            Array.Copy(response.GetFrameData(), response.GetParameterOffset() + 2, device.value, 0, 8);
            device.value[8] = response.GetParameter(0);
            device.value[9] = response.GetParameter(1);

            device.RSSI = response.GetParameter(10) * -1;

            try
            {
                int nilength = length - 11;

                if (nilength <= 0)
                    device.NIString = string.Empty;
                else
                {
                    byte[] cache = new byte[nilength];
                    Array.Copy(response.GetFrameData(), response.GetParameterOffset() + 11, cache, 0, nilength);
                    device.NIString = new string(UTF8Encoding.UTF8.GetChars(cache));
                }
            }
            catch { device.NIString = "error while encoding"; }

            return device;
        }
    }
}