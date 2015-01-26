using System.Collections;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Response
{
    public abstract class XBeeIODataSampleRxBase : XBeeRxBase
    {
        public XBeeIODataSampleRxBase(APIFrame frame)
            : base(frame)
        { }

        public static IOSamples SamplesParse(byte[] IOSamplePayload, int offset)
        {
            Hashtable Digital = new Hashtable();
            Hashtable Analog = new Hashtable();
            int index = 0;
            if (IOSamplePayload.Length > 3)
            {
                if ((IOSamplePayload[offset + 1] & 0x01) + IOSamplePayload[offset + 2] == 0)//digital mask
                    index = 3;
                else
                {
                    index = 5;
                    if ((IOSamplePayload[offset + 2] & 0x01) == 0x01)
                        Digital.Add(Device.Pin.XBee.AD0_DIO0, (IOSamplePayload[offset + 4] & 0x01) == 0x01 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);
                    if ((IOSamplePayload[offset + 2] & 0x02) == 0x02)
                        Digital.Add(Device.Pin.XBee.AD1_DIO1, (IOSamplePayload[offset + 4] & 0x02) == 0x02 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);
                    if ((IOSamplePayload[offset + 2] & 0x04) == 0x04)
                        Digital.Add(Device.Pin.XBee.AD2_DIO2, (IOSamplePayload[offset + 4] & 0x04) == 0x04 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);
                    if ((IOSamplePayload[offset + 2] & 0x08) == 0x08)
                        Digital.Add(Device.Pin.XBee.AD3_DIO3, (IOSamplePayload[offset + 4] & 0x08) == 0x08 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);
                    if ((IOSamplePayload[offset + 2] & 0x10) == 0x10)
                        Digital.Add(Device.Pin.XBee.AD4_DIO4, (IOSamplePayload[offset + 4] & 0x10) == 0x10 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);
                    if ((IOSamplePayload[offset + 2] & 0x20) == 0x20)
                        Digital.Add(Device.Pin.XBee.ASSOCIATE_AD5_DIO5, (IOSamplePayload[offset + 4] & 0x20) == 0x20 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);
                    if ((IOSamplePayload[offset + 2] & 0x40) == 0x40)
                        Digital.Add(Device.Pin.XBee.RTS_AD6_DIO6, (IOSamplePayload[offset + 4] & 0x40) == 0x40 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);
                    if ((IOSamplePayload[offset + 2] & 0x80) == 0x80)
                        Digital.Add(Device.Pin.XBee.CTS_DIO7, (IOSamplePayload[offset + 4] & 0x80) == 0x80 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);
                    if ((IOSamplePayload[offset + 1] & 0x01) == 0x01)
                        Digital.Add(Device.Pin.XBee.DTR_SLEEP_DIO8, (IOSamplePayload[offset + 3] & 0x01) == 0x01 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);
                }
                if ((IOSamplePayload[offset + 1] & 0x02) == 0x02)
                {
                    Analog.Add(Device.Pin.XBee.AD0_DIO0, IOSamplePayload[offset + index] << 8 | IOSamplePayload[offset + index + 1]);
                    index += 2;
                }
                if ((IOSamplePayload[offset + 1] & 0x04) == 0x04)
                {
                    Analog.Add(Device.Pin.XBee.AD1_DIO1, IOSamplePayload[offset + index] << 8 | IOSamplePayload[offset + index + 1]);
                    index += 2;
                }
                if ((IOSamplePayload[offset + 1] & 0x08) == 0x08)
                {
                    Analog.Add(Device.Pin.XBee.AD2_DIO2, IOSamplePayload[offset + index] << 8 | IOSamplePayload[offset + index + 1]);
                    index += 2;
                }
                if ((IOSamplePayload[offset + 1] & 0x10) == 0x10)
                {
                    Analog.Add(Device.Pin.XBee.AD3_DIO3, IOSamplePayload[offset + index] << 8 | IOSamplePayload[offset + index + 1]);
                    index += 2;
                }
                if ((IOSamplePayload[offset + 1] & 0x20) == 0x20)
                {
                    Analog.Add(Device.Pin.XBee.AD4_DIO4, IOSamplePayload[offset + index] << 8 | IOSamplePayload[offset + index + 1]);
                    index += 2;
                }
                if ((IOSamplePayload[offset + 1] & 0x40) == 0x40)
                    Analog.Add(Device.Pin.XBee.ASSOCIATE_AD5_DIO5, IOSamplePayload[offset + index] << 8 | IOSamplePayload[offset + index + 1]);
            }
            return new IOSamples(Analog, Digital, 0);
        }

        public abstract IOSamples GetIOSamples();
    }
}