using SmartLab.XBee.Core;
using SmartLab.XBee.Device;
using SmartLab.XBee.Helper;
using SmartLab.XBee.Status;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Indicator
{
    public class XBeeRx64IOSampleIndicator : RxBase, ISampleResponse
    {
        public XBeeRx64IOSampleIndicator(APIFrame frame)
            : base(frame)
        { }

        public int GetRSSI()
        {
            return this.GetFrameData()[9] * -1;
        }

        public IOSamples[] GetIOSamples()
        {
            return IOSampleDecoder.XBeeSamplesParse(this.GetFrameData(), 11);
        }

        public ReceiveStatus GetReceiveStatus()
        {
            return (ReceiveStatus)this.GetFrameData()[10];
        }

        public Address GetRemoteDevice()
        {
            return new Address(new byte[] { GetFrameData()[1], GetFrameData()[2], GetFrameData()[3], GetFrameData()[4], GetFrameData()[5], GetFrameData()[6], GetFrameData()[7], GetFrameData()[8], 0x00, 0x00 });
        }
    }
}