using System.Collections;
using SmartLab.XBee.Status;
using SmartLab.XBee.Type;
using SmartLab.XBee.Device;

namespace SmartLab.XBee.Response
{
    public class ZigBeeIODataSampleRxResponse : ZigBeeRxBase
    {
        public ZigBeeIODataSampleRxResponse(APIFrame frame)
            : base(frame)
        { }

        public static IOSamples SamplesParse(byte[] IOSamplePayload , int offset)
        {
            Hashtable Digital = new Hashtable();
            Hashtable Analog = new Hashtable();
            int SUPPLY_VOLTAGE = 0;
            int index = 0;
            if (IOSamplePayload.Length > 3)
            {
                if (IOSamplePayload[offset + 1] + IOSamplePayload[offset + 2] == 0)//digital mask
                    index = 4;
                else
                {
                    index = 6;
                    if ((IOSamplePayload[offset + 2] & 0x01) == 0x01)
                        Digital.Add(Device.Pin.ZigBee.AD0_DIO0_COMMISSIONONG_BUTTON, (IOSamplePayload[offset + 5] & 0x01) == 0x01 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);
                    if ((IOSamplePayload[offset + 2] & 0x02) == 0x02)
                        Digital.Add(Device.Pin.ZigBee.AD1_DIO1, (IOSamplePayload[offset + 5] & 0x02) == 0x02 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);
                    if ((IOSamplePayload[offset + 2] & 0x04) == 0x04)
                        Digital.Add(Device.Pin.ZigBee.AD2_DIO2, (IOSamplePayload[offset + 5] & 0x04) == 0x04 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);
                    if ((IOSamplePayload[offset + 2] & 0x08) == 0x08)
                        Digital.Add(Device.Pin.ZigBee.AD3_DIO3, (IOSamplePayload[offset + 5] & 0x08) == 0x08 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);
                    if ((IOSamplePayload[offset + 2] & 0x10) == 0x10)
                        Digital.Add(Device.Pin.ZigBee.DIO4, (IOSamplePayload[offset + 5] & 0x10) == 0x10 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);
                    if ((IOSamplePayload[offset + 2] & 0x20) == 0x20)
                        Digital.Add(Device.Pin.ZigBee.ASSOCIATE_DIO5, (IOSamplePayload[offset + 5] & 0x20) == 0x20 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);
                    if ((IOSamplePayload[offset + 2] & 0x40) == 0x40)
                        Digital.Add(Device.Pin.ZigBee.RTS_DIO6, (IOSamplePayload[offset + 5] & 0x40) == 0x40 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);
                    if ((IOSamplePayload[offset + 2] & 0x80) == 0x80)
                        Digital.Add(Device.Pin.ZigBee.CTS_DIO7, (IOSamplePayload[offset + 5] & 0x80) == 0x80 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);

                    if ((IOSamplePayload[offset + 1] & 0x04) == 0x04)
                        Digital.Add(Device.Pin.ZigBee.RSSI_PWM_DIO10, (IOSamplePayload[offset + 4] & 0x04) == 0x04 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);
                    if ((IOSamplePayload[offset + 1] & 0x08) == 0x08)
                        Digital.Add(Device.Pin.ZigBee.PWM_DIO11, (IOSamplePayload[offset + 4] & 0x08) == 0x08 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);
                    if ((IOSamplePayload[offset + 1] & 0x10) == 0x10)
                        Digital.Add(Device.Pin.ZigBee.DIO12, (IOSamplePayload[offset + 4] & 0x10) == 0x10 ? Device.Pin.Status.HIGH : Device.Pin.Status.LOW);
                }
                if (IOSamplePayload[offset + 3] != 0x00)//analog mask
                {
                    if ((IOSamplePayload[offset + 3] & 0x01) == 0x01)
                    {
                        Analog.Add(Device.Pin.ZigBee.AD0_DIO0_COMMISSIONONG_BUTTON, IOSamplePayload[offset + index] << 8 | IOSamplePayload[offset + index + 1]);
                        index += 2;
                    }
                    if ((IOSamplePayload[offset + 3] & 0x02) == 0x02)
                    {
                        Analog.Add(Device.Pin.ZigBee.AD1_DIO1, IOSamplePayload[offset + index] << 8 | IOSamplePayload[offset + index + 1]);
                        index += 2;
                    }
                    if ((IOSamplePayload[offset + 3] & 0x04) == 0x04)
                    {
                        Analog.Add(Device.Pin.ZigBee.AD2_DIO2, IOSamplePayload[offset + index] << 8 | IOSamplePayload[offset + index + 1]);
                        index += 2;
                    }
                    if ((IOSamplePayload[offset + 3] & 0x08) == 0x08)
                    {
                        Analog.Add(Device.Pin.ZigBee.AD3_DIO3, IOSamplePayload[offset + index] << 8 | IOSamplePayload[offset + index + 1]);
                        index += 2;
                    }
                    if ((IOSamplePayload[offset + 3] & 0x80) == 0x80)
                        SUPPLY_VOLTAGE = IOSamplePayload[offset + index] << 8 | IOSamplePayload[offset + index + 1];
                }
            }
            return new IOSamples(Analog, Digital, SUPPLY_VOLTAGE);
        }

        public override Address GetRemoteDevice()
        {
            return new Address(this.GetFrameData().ExtractRangeFromArray(1, 10));
        }

        public override ReceiveStatus GetReceiveStatus()
        {
            return (ReceiveStatus)this.GetFrameData()[11];
        }

        public IOSamples GetIOSamples()
        {
            return SamplesParse(this.GetFrameData(), 12);
        }

        public override byte[] GetReceivedData()
        {
            return GetFrameData().ExtractRangeFromArray(11, this.GetPosition() - 11);
        }

        public override int GetReceivedDataOffset() { return 11; }
    }
}