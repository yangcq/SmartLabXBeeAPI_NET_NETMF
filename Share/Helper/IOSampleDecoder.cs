using System.Collections;
using SmartLab.XBee.Type;
using SmartLab.XBee.Indicator;

namespace SmartLab.XBee.Helper
{
    public static class IOSampleDecoder
    {
        public static IOSamples[] XBeeSamplesParse(byte[] IOSamplePayload, int offset)
        {
            // at least 3 bytes, 1 byte of [number of samples] + 2 bytes of [digital channel mask] and [analog channel mask].
            if (IOSamplePayload.Length - offset < 3)
                return null;

            int numofsamples = IOSamplePayload[offset];

            if (numofsamples <= 0)
                return null;

            // first byte is the number of sample
            int index = offset + 1;

            IOSamples[] smaplearray = new IOSamples[numofsamples];

            int digitMask = (IOSamplePayload[index] & 0x01) << 8 | IOSamplePayload[index + 1];
            int analogMask = IOSamplePayload[index] & 0xFE;

            // sample start at +2 [mask]
            index += 2;

            for (int i = 0; i < numofsamples; i++)
            {
                Hashtable Analog = new Hashtable();
                Hashtable Digital = new Hashtable();
                if (digitMask != 0)
                {
                    if ((digitMask & 0x01) == 0x01)
                        Digital.Add(Device.Pin.XBee.P20_AD0_DIO0, (IOSamplePayload[index + 1] & 0x01) == 0x01 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);
                    if ((digitMask & 0x02) == 0x02)
                        Digital.Add(Device.Pin.XBee.P19_AD1_DIO1, (IOSamplePayload[index + 1] & 0x02) == 0x02 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);
                    if ((digitMask & 0x04) == 0x04)
                        Digital.Add(Device.Pin.XBee.P18_AD2_DIO2, (IOSamplePayload[index + 1] & 0x04) == 0x04 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);
                    if ((digitMask & 0x08) == 0x08)
                        Digital.Add(Device.Pin.XBee.P17_AD3_DIO3, (IOSamplePayload[index + 1] & 0x08) == 0x08 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);
                    if ((digitMask & 0x10) == 0x10)
                        Digital.Add(Device.Pin.XBee.P11_AD4_DIO4, (IOSamplePayload[index + 1] & 0x10) == 0x10 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);
                    if ((digitMask & 0x20) == 0x20)
                        Digital.Add(Device.Pin.XBee.P15_ASSOCIATE_AD5_DIO5, (IOSamplePayload[index + 1] & 0x20) == 0x20 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);
                    if ((digitMask & 0x40) == 0x40)
                        Digital.Add(Device.Pin.XBee.P16_RTS_AD6_DIO6, (IOSamplePayload[index + 1] & 0x40) == 0x40 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);
                    if ((digitMask & 0x80) == 0x80)
                        Digital.Add(Device.Pin.XBee.P12_CTS_DIO7, (IOSamplePayload[index + 1] & 0x80) == 0x80 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);
                    if (((digitMask >> 8) & 0x01) == 0x01)
                        Digital.Add(Device.Pin.XBee.P9_DTR_SLEEP_DIO8, (IOSamplePayload[index] & 0x01) == 0x01 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);

                    //skip the 2 [digital sample]
                    index += 2;
                }

                if (analogMask != 0)
                {
                    if ((analogMask & 0x02) == 0x02)
                        Analog.Add(Device.Pin.XBee.P20_AD0_DIO0, IOSamplePayload[index++] << 8 | IOSamplePayload[index++]);

                    if ((analogMask & 0x04) == 0x04)
                        Analog.Add(Device.Pin.XBee.P19_AD1_DIO1, IOSamplePayload[index++] << 8 | IOSamplePayload[index++]);

                    if ((analogMask & 0x08) == 0x08)
                        Analog.Add(Device.Pin.XBee.P18_AD2_DIO2, IOSamplePayload[index++] << 8 | IOSamplePayload[index++]);

                    if ((analogMask & 0x10) == 0x10)
                        Analog.Add(Device.Pin.XBee.P17_AD3_DIO3, IOSamplePayload[index++] << 8 | IOSamplePayload[index++]);

                    if ((analogMask & 0x20) == 0x20)
                        Analog.Add(Device.Pin.XBee.P11_AD4_DIO4, IOSamplePayload[index++] << 8 | IOSamplePayload[index++]);

                    if ((analogMask & 0x40) == 0x40)
                        Analog.Add(Device.Pin.XBee.P15_ASSOCIATE_AD5_DIO5, IOSamplePayload[index++] << 8 | IOSamplePayload[index++]);
                }

                smaplearray[i] = new IOSamples(Analog, Digital, 0);
            }

            return smaplearray;
        }

        /// <summary>
        /// Parse byte array into IO sample details.
        /// </summary>
        /// <param name="IOSamplePayload">Source data frame</param>
        /// <param name="offset">The begin index of the source data (the first byte should be the number of sample)</param>
        /// <returns></returns>
        public static IOSamples[] ZigBeeSamplesParse(byte[] IOSamplePayload, int offset)
        {
            // at least 4 bytes, 1 byte of [number of samples] + 2 bytes of [digital channel mask] + 1 bytes of [analog channel mask].
            if (IOSamplePayload.Length - offset < 4)
                return null;

            // the [number of samples] always set to 1.
            int numofsamples = IOSamplePayload[offset];

            if (numofsamples <= 0)
                return null;

            int index = offset + 1;

            int digitMask = IOSamplePayload[index++] << 8 | IOSamplePayload[index++];
            int analogMask = IOSamplePayload[index++];

            IOSamples[] smaplearray = new IOSamples[numofsamples];

            for (int i = 0; i < numofsamples; i++)
            {
                Hashtable Analog = new Hashtable();
                Hashtable Digital = new Hashtable();
                int SUPPLY_VOLTAGE = 0;

                if (digitMask != 0)
                {
                    if ((digitMask & 0x01) == 0x01)
                        Digital.Add(Device.Pin.ZigBee.P20_AD0_DIO0_COMMISSIONONG_BUTTON, (IOSamplePayload[index + 1] & 0x01) == 0x01 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);
                    if ((digitMask & 0x02) == 0x02)
                        Digital.Add(Device.Pin.ZigBee.P19_AD1_DIO1, (IOSamplePayload[index + 1] & 0x02) == 0x02 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);
                    if ((digitMask & 0x04) == 0x04)
                        Digital.Add(Device.Pin.ZigBee.P18_AD2_DIO2, (IOSamplePayload[index + 1] & 0x04) == 0x04 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);
                    if ((digitMask & 0x08) == 0x08)
                        Digital.Add(Device.Pin.ZigBee.P17_AD3_DIO3, (IOSamplePayload[index + 1] & 0x08) == 0x08 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);
                    if ((digitMask & 0x10) == 0x10)
                        Digital.Add(Device.Pin.ZigBee.P11_DIO4, (IOSamplePayload[index + 1] & 0x10) == 0x10 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);
                    if ((digitMask & 0x20) == 0x20)
                        Digital.Add(Device.Pin.ZigBee.P15_ASSOCIATE_DIO5, (IOSamplePayload[index + 1] & 0x20) == 0x20 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);
                    if ((digitMask & 0x40) == 0x40)
                        Digital.Add(Device.Pin.ZigBee.P16_RTS_DIO6, (IOSamplePayload[index + 1] & 0x40) == 0x40 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);
                    if ((digitMask & 0x80) == 0x80)
                        Digital.Add(Device.Pin.ZigBee.P12_CTS_DIO7, (IOSamplePayload[index + 1] & 0x80) == 0x80 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);

                    if (((digitMask >> 8) & 0x04) == 0x04)
                        Digital.Add(Device.Pin.ZigBee.P6_RSSI_PWM_DIO10, (IOSamplePayload[index] & 0x04) == 0x04 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);
                    if (((digitMask >> 8) & 0x08) == 0x08)
                        Digital.Add(Device.Pin.ZigBee.P7_PWM_DIO11, (IOSamplePayload[index] & 0x08) == 0x08 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);
                    if (((digitMask >> 8) & 0x10) == 0x10)
                        Digital.Add(Device.Pin.ZigBee.P4_DIO12, (IOSamplePayload[index] & 0x10) == 0x10 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);

                    index += 2;// 2 [digital sample]
                }
                if (analogMask != 0x00)//analog mask
                {
                    if ((analogMask & 0x01) == 0x01)
                        Analog.Add(Device.Pin.ZigBee.P20_AD0_DIO0_COMMISSIONONG_BUTTON, IOSamplePayload[index++] << 8 | IOSamplePayload[index++]);

                    if ((analogMask & 0x02) == 0x02)
                        Analog.Add(Device.Pin.ZigBee.P19_AD1_DIO1, IOSamplePayload[index++] << 8 | IOSamplePayload[index++]);

                    if ((analogMask & 0x04) == 0x04)
                        Analog.Add(Device.Pin.ZigBee.P18_AD2_DIO2, IOSamplePayload[index++] << 8 | IOSamplePayload[index++]);

                    if ((analogMask & 0x08) == 0x08)
                        Analog.Add(Device.Pin.ZigBee.P17_AD3_DIO3, IOSamplePayload[index++] << 8 | IOSamplePayload[index++]);

                    if ((analogMask & 0x80) == 0x80)
                        SUPPLY_VOLTAGE = IOSamplePayload[index++] << 8 | IOSamplePayload[index++];
                }
                smaplearray[i] = new IOSamples(Analog, Digital, SUPPLY_VOLTAGE);
            }

            return smaplearray;
        }
    }
}