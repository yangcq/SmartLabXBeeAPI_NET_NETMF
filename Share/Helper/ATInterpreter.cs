using SmartLab.XBee.Device;
using SmartLab.XBee.Indicator;

namespace SmartLab.XBee.Helper
{
    public static class ATInterpreter
    {
        /// <summary>
        /// Get node discovery result form a ND command.
        /// </summary>
        /// <param name="indicator"></param>
        /// <returns></returns>
        public static Address FromND(CommandIndicatorBase indicator)
        {
            return Address.Parse(indicator);
        }

        public static XBeeDiscoverAddress FromXBeeND(CommandIndicatorBase indicator)
        {
            return XBeeDiscoverAddress.Parse(indicator);
        }

        public static ZigBeeDiscoverAddress FromZigBeeND(CommandIndicatorBase indicator)
        {
            return ZigBeeDiscoverAddress.Parse(indicator);
        }

        public static IOSamples[] FromXBeeIS(CommandIndicatorBase indicator)
        {
            if (indicator == null)
                return null;

            if (indicator.GetCommandStatus() != Status.CommandStatus.OK)
                return null;

            if (!indicator.GetRequestCommand().ToString().ToUpper().Equals("IS"))
                return null;

            if (indicator.GetParameterLength() <= 0)
                return null;

            return RxIOSampleBase.XBeeSamplesParse(indicator.GetFrameData(), indicator.GetParameterOffset());
        }

        /// <summary>
        /// Parse remote AT command "IS" into IO sample details.
        /// </summary>
        /// <param name="indicator"></param>
        /// <returns></returns>
        public static IOSamples[] FromZigBeeIS(CommandIndicatorBase indicator)
        {
            if (indicator == null)
                return null;

            if (indicator.GetCommandStatus() != Status.CommandStatus.OK)
                return null;

            if (!indicator.GetRequestCommand().ToString().ToUpper().Equals("IS"))
                return null;

            if (indicator.GetParameterLength() <= 0)
                return null;

            return RxIOSampleBase.ZigBeeSamplesParse(indicator.GetFrameData(), indicator.GetParameterOffset());
        }

        public static ActiveScanResult FromAS(CommandIndicatorBase indicator)
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

            int index = indicator.GetParameterOffset();
            byte[] data = indicator.GetFrameData();

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