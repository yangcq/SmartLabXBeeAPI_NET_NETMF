﻿using System.Text;
using SmartLab.XBee.Indicator;
using SmartLab.XBee.Type;
using System;

namespace SmartLab.XBee.Device
{
    public class ZigBeeDiscoverAddress : XBeeDiscoverAddress
    {
        // total 8 bytes
        // 2 bytes of ParentAddress16 + 1 byte of Type + 1 byte of Status (Reserved) + 2 bytes of ProfileID + 2 bytes of ManufacturerID
        private byte[] zigbeeAdditional;

        public int ManufacturerID()
        {
            return (zigbeeAdditional[6] << 8) | zigbeeAdditional[7];
        }

        public int GetProfileID()
        {
            return (zigbeeAdditional[4] << 8) | zigbeeAdditional[5];
        }

        public int GetParentNetworkAddress16()
        {
            return (zigbeeAdditional[0] << 8) | zigbeeAdditional[1];
        }

        public DeviceType GetDeviceType()
        {
            return (DeviceType)zigbeeAdditional[2];
        }

        /// <summary>
        /// extension method for convert ND (with or without NI String) response to address 
        /// </summary>
        /// <param name="response">muset be non null parameter</param>
        /// <returns></returns>
        public static new ZigBeeDiscoverAddress Parse(CommandIndicatorBase indicator)
        {
            if (indicator == null)
                return null;

            if (!indicator.GetRequestCommand().ToString().ToUpper().Equals("ND"))
                return null;

            int length = indicator.GetParameterLength();
            if (length <= 0)
                return null;

            ZigBeeDiscoverAddress device = new ZigBeeDiscoverAddress();
            int offset = indicator.GetParameterLength() - 8;

            Array.Copy(indicator.GetFrameData(), indicator.GetParameterOffset() + 2, device.value, 0, 8);
            device.value[8] = indicator.GetParameter(0);
            device.value[9] = indicator.GetParameter(1);

            try
            {
                int nilength = length - 18;

                if (nilength <= 0)
                    device.NIString = string.Empty;
                else
                {
                    byte[] cache = new byte[nilength];
                    Array.Copy(indicator.GetFrameData(), indicator.GetParameterOffset() + 10, cache, 0, nilength);
                    device.NIString = new string(UTF8Encoding.UTF8.GetChars(cache));
                }
            }
            catch { device.NIString = "error while encoding"; }

            byte[] add = new byte[8];
            Array.Copy(indicator.GetFrameData(), offset, add, 0, 8);
            device.zigbeeAdditional = add;

            return device;
        }
    }
}