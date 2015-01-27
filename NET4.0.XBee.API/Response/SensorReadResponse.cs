using SmartLab.XBee.Status;
using SmartLab.XBee.Type;
using SmartLab.XBee.Device;

namespace SmartLab.XBee.Response
{
    public class SensorReadResponse : ZigBeeRxBase
    {
        public SensorReadResponse(APIFrame frame)
            : base(frame)
        { }

        public override int GetReceivedDataOffset() { return 12; }

        public override byte[] GetReceivedData()
        {
            return GetFrameData().ExtractRangeFromArray(12, GetPosition() - 12);
        }

        public override Address GetRemoteDevice()
        {
            return new Address(GetFrameData().ExtractRangeFromArray(1, 10));
        }

        public override ReceiveStatus GetReceiveStatus()
        {
            return (ReceiveStatus)this.GetFrameData()[11];
        }

        public OneWireSensor GetOneWireSensor()
        {
            return (OneWireSensor)this.GetFrameData()[12];
        }

        public int GetAD0()
        {
            return this.GetFrameData()[13] << 8 | this.GetFrameData()[14];
        }

        public int GetAD1()
        {
            return this.GetFrameData()[15] << 8 | this.GetFrameData()[16];
        }

        public int GetAD2()
        {
            return this.GetFrameData()[17] << 8 | this.GetFrameData()[18];
        }

        public int GetAD3()
        {
            return this.GetFrameData()[19] << 8 | this.GetFrameData()[20];
        }

        public int GetThemometer()
        {
            return this.GetFrameData()[21] << 8 | this.GetFrameData()[22];
        }
    }
}