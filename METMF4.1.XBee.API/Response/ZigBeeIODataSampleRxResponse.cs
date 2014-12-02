using System.Collections;
using SmartLab.XBee.Status;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Response
{
    public class ZigBeeIODataSampleRxResponse : ZigBeeRxBase
    {
        public ZigBeeIODataSampleRxResponse(ResponseBase Frame)
            : base(Frame)
        { }

        public static IOSamples SamplesParse(byte[] IOSamplePayload)
        {
            Hashtable Digital = new Hashtable();
            Hashtable Analog = new Hashtable();
            int SUPPLY_VOLTAGE = 0;
            int index = 0;
            if (IOSamplePayload.Length > 3)
            {
                if (IOSamplePayload[1] + IOSamplePayload[2] == 0)//digital mask
                    index = 4;
                else
                {
                    index = 6;
                    if ((IOSamplePayload[2] & 0x01) == 0x01)
                        Digital.Add(Device.Pin.ZigBee.AD0_DIO0_COMMISSIONONG_BUTTON, (IOSamplePayload[5] & 0x01) == 0x01 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);
                    if ((IOSamplePayload[2] & 0x02) == 0x02)
                        Digital.Add(Device.Pin.ZigBee.AD1_DIO1, (IOSamplePayload[5] & 0x02) == 0x02 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);
                    if ((IOSamplePayload[2] & 0x04) == 0x04)
                        Digital.Add(Device.Pin.ZigBee.AD2_DIO2, (IOSamplePayload[5] & 0x04) == 0x04 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);
                    if ((IOSamplePayload[2] & 0x08) == 0x08)
                        Digital.Add(Device.Pin.ZigBee.AD3_DIO3, (IOSamplePayload[5] & 0x08) == 0x08 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);
                    if ((IOSamplePayload[2] & 0x10) == 0x10)
                        Digital.Add(Device.Pin.ZigBee.DIO4, (IOSamplePayload[5] & 0x10) == 0x10 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);
                    if ((IOSamplePayload[2] & 0x20) == 0x20)
                        Digital.Add(Device.Pin.ZigBee.ASSOCIATE_DIO5, (IOSamplePayload[5] & 0x20) == 0x20 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);
                    if ((IOSamplePayload[2] & 0x40) == 0x40)
                        Digital.Add(Device.Pin.ZigBee.RTS_DIO6, (IOSamplePayload[5] & 0x40) == 0x40 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);
                    if ((IOSamplePayload[2] & 0x80) == 0x80)
                        Digital.Add(Device.Pin.ZigBee.CTS_DIO7, (IOSamplePayload[5] & 0x80) == 0x80 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);

                    if ((IOSamplePayload[1] & 0x04) == 0x04)
                        Digital.Add(Device.Pin.ZigBee.RSSI_PWM_DIO10, (IOSamplePayload[4] & 0x04) == 0x04 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);
                    if ((IOSamplePayload[1] & 0x08) == 0x08)
                        Digital.Add(Device.Pin.ZigBee.PWM_DIO11, (IOSamplePayload[4] & 0x08) == 0x08 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);
                    if ((IOSamplePayload[1] & 0x10) == 0x10)
                        Digital.Add(Device.Pin.ZigBee.DIO12, (IOSamplePayload[4] & 0x10) == 0x10 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);
                }
                if (IOSamplePayload[3] != 0x00)//analog mask
                {
                    if ((IOSamplePayload[3] & 0x01) == 0x01)
                    {
                        Analog.Add(Device.Pin.ZigBee.AD0_DIO0_COMMISSIONONG_BUTTON, IOSamplePayload[index] << 8 | IOSamplePayload[index + 1]);
                        index += 2;
                    }
                    if ((IOSamplePayload[3] & 0x02) == 0x02)
                    {
                        Analog.Add(Device.Pin.ZigBee.AD1_DIO1, IOSamplePayload[index] << 8 | IOSamplePayload[index + 1]);
                        index += 2;
                    }
                    if ((IOSamplePayload[3] & 0x04) == 0x04)
                    {
                        Analog.Add(Device.Pin.ZigBee.AD2_DIO2, IOSamplePayload[index] << 8 | IOSamplePayload[index + 1]);
                        index += 2;
                    }
                    if ((IOSamplePayload[3] & 0x08) == 0x08)
                    {
                        Analog.Add(Device.Pin.ZigBee.AD3_DIO3, IOSamplePayload[index] << 8 | IOSamplePayload[index + 1]);
                        index += 2;
                    }
                    if ((IOSamplePayload[3] & 0x80) == 0x80)
                        SUPPLY_VOLTAGE = IOSamplePayload[index] << 8 | IOSamplePayload[index + 1];
                }
            }
            return new IOSamples(Analog, Digital, SUPPLY_VOLTAGE);
        }

        public override DeviceAddress GetRemoteDevice()
        {
            return new DeviceAddress(this.FrameData.ExtractRangeFromArray(1, 10));
        }

        public override ReceiveStatus GetReceiveStatus()
        {
            return (ReceiveStatus)this.FrameData[11];
        }

        public IOSamples GetIOSamples()
        {
            return SamplesParse(this.FrameData.ExtractRangeFromArray(12, this.Length - 12));
        }

        public override byte[] GetReceivedData()
        {
            return FrameData.ExtractRangeFromArray(11, this.Length - 11);
        }
    }
}