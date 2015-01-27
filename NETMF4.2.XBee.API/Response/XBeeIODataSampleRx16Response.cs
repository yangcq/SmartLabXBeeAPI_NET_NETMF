using SmartLab.XBee.Status;
using SmartLab.XBee.Type;
using SmartLab.XBee.Device;

namespace SmartLab.XBee.Response
{
    public class XBeeIODataSampleRx16Response : XBeeIODataSampleRxBase
    {
        public XBeeIODataSampleRx16Response(APIFrame frame)
            : base(frame)
        { }

        public override int GetRSSI()
        {
            return GetFrameData()[3] * -1;
        }

        public override IOSamples GetIOSamples()
        {
            return SamplesParse(this.GetFrameData(), 5);
        }

        public override ReceiveStatus GetReceiveStatus()
        {
            return (ReceiveStatus)this.GetFrameData()[4];
        }

        public override Address GetRemoteDevice()
        {
            return new Address(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, GetFrameData()[1], GetFrameData()[2] });
        }

        public override byte[] GetReceivedData()
        {
            return this.GetFrameData().ExtractRangeFromArray(3, this.GetPosition() - 3);
        }

        public override int GetReceivedDataOffset() { return 3; }
    }
}