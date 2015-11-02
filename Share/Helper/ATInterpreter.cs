using SmartLab.XBee.Device;
using SmartLab.XBee.Indicator;
using SmartLab.XBee.Type;
using System;
using System.Text;

namespace SmartLab.XBee.Helper
{
    public static class ATInterpreter
    {
        /// <summary>
        /// extension method for convert ND (with or without NI String) response to address 
        /// </summary>
        /// <param name="indicator"></param>
        /// <returns></returns>
        public static Address FromND(ICommandResponse indicator)
        {
            return Address.Parse(indicator);
        }

        public static XBeeDiscoverAddress FromXBeeND(ICommandResponse indicator)
        {
            return XBeeDiscoverAddress.Parse(indicator);
        }

        public static ZigBeeDiscoverAddress FromZigBeeND(ICommandResponse indicator)
        {
            return ZigBeeDiscoverAddress.Parse(indicator);
        }

        public static IOSamples[] FromXBeeIS(ICommandResponse indicator)
        {
            if (indicator == null)
                return null;

            if (indicator.GetCommandStatus() != Status.CommandStatus.OK)
                return null;

            if (!indicator.GetRequestCommand().ToString().ToUpper().Equals("IS"))
                return null;

            if (indicator.GetParameterLength() <= 0)
                return null;

            return IOSampleDecoder.XBeeSamplesParse(indicator.GetParameter(), 0);
        }

        /// <summary>
        /// Parse remote AT command "IS" into IO sample details.
        /// </summary>
        /// <param name="indicator"></param>
        /// <returns></returns>
        public static IOSamples[] FromZigBeeIS(ICommandResponse indicator)
        {
            if (indicator == null)
                return null;

            if (indicator.GetCommandStatus() != Status.CommandStatus.OK)
                return null;

            if (!indicator.GetRequestCommand().ToString().ToUpper().Equals("IS"))
                return null;

            if (indicator.GetParameterLength() <= 0)
                return null;

            return IOSampleDecoder.ZigBeeSamplesParse(indicator.GetParameter(), 0);
        }

        public static ActiveScanResult FromAS(ICommandResponse indicator)
        {
            if (indicator == null)
                return null;

            if (indicator.GetCommandStatus() != Status.CommandStatus.OK)
                return null;

            if (!indicator.GetRequestCommand().ToString().ToUpper().Equals("AS"))
                return null;

            if (indicator.GetParameterLength() <= 0)
                return null;

            ActiveScanResult result = new ActiveScanResult();

            int index = 0;
            byte[] data = indicator.GetParameter();

            result.AS_Type = data[index];
            result.Channel = data[index + 1];

            result.PanID = new byte[] { data[index + 2], data[index + 3] };
            result.ExtendedPanID = new byte[] { data[index + 4], data[index + 5], data[index + 6], data[index + 7], data[index + 8], data[index + 8], data[index + 10], data[index + 11] };

            result.AllowJoin = (data[index + 12] == 0x00 ? false : true);

            result.StackProfile = data[index + 13];
            result.LQI = data[index + 14];
            result.RSSI = (sbyte)data[index + 15];

            return result;
        }
    }
}