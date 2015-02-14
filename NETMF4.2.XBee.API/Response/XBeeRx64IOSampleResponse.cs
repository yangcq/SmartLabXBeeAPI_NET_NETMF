using SmartLab.XBee.Status;
using SmartLab.XBee.Type;
using SmartLab.XBee.Device;

namespace SmartLab.XBee.Response
{
    public class XBeeRx64IOSampleResponse : RxIOSampleBase
    {
        public XBeeRx64IOSampleResponse(APIFrame frame)
            : base(frame)
        { }

        public override int GetRSSI()
        {
            return this.GetFrameData()[9] * -1;
        }

        public override IOSamples GetIOSamples()
        {
            return RxIOSampleBase.XBeeSamplesParse(this.GetFrameData(), 11);
        }

        public override ReceiveStatus GetReceiveStatus()
        {
            return (ReceiveStatus)this.GetFrameData()[10];
        }

        public override Address GetRemoteDevice()
        {
            return new Address(new byte[] { GetFrameData()[1], GetFrameData()[2], GetFrameData()[3], GetFrameData()[4], GetFrameData()[5], GetFrameData()[6], GetFrameData()[7], GetFrameData()[8], 0x00, 0x00 });
        }
    }
}