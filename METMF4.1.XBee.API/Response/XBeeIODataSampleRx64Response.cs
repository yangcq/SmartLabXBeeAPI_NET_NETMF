using SmartLab.XBee.Status;
using SmartLab.XBee.Type;
using SmartLab.XBee.Device;

namespace SmartLab.XBee.Response
{
    public class XBeeIODataSampleRx64Response : XBeeIODataSampleRxBase
    {
        public XBeeIODataSampleRx64Response(APIFrame frame)
            : base(frame)
        { }

        public override int GetRSSI()
        {
            return this.GetFrameData()[9] * -1;
        }

        public override IOSamples GetIOSamples()
        {
            return SamplesParse(this.GetFrameData(), 11);
        }

        public override ReceiveStatus GetReceiveStatus()
        {
            return (ReceiveStatus)this.GetFrameData()[10];
        }

        public override Address GetRemoteDevice()
        {
            return new Address(new byte[] { GetFrameData()[1], GetFrameData()[2], GetFrameData()[3], GetFrameData()[4], GetFrameData()[5], GetFrameData()[6], GetFrameData()[7], GetFrameData()[8], 0x00, 0x00 });
        }

        public override byte[] GetReceivedData()
        {
            return this.GetFrameData().ExtractRangeFromArray(9, this.GetPosition() - 9);
        }

        public override int GetReceivedDataOffset() { return 9; }
    }
}