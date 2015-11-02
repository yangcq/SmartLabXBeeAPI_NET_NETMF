using SmartLab.XBee.Core;
using SmartLab.XBee.Device;
using SmartLab.XBee.Helper;
using SmartLab.XBee.Status;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Indicator
{
    public class XBeeRx16IOSampleIndicator : RxBase, ISampleResponse
    {
        public XBeeRx16IOSampleIndicator(APIFrame frame)
            : base(frame)
        { }

        public int GetRSSI()
        {
            return GetFrameData()[3] * -1;
        }

        public IOSamples[] GetIOSamples()
        {
            return IOSampleDecoder.XBeeSamplesParse(this.GetFrameData(), 5);
        }

        public ReceiveStatus GetReceiveStatus()
        {
            return (ReceiveStatus)this.GetFrameData()[4];
        }

        public Address GetRemoteDevice()
        {
            return new Address(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, GetFrameData()[1], GetFrameData()[2] });
        }
    }
}