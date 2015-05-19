using SmartLab.XBee.Status;
using SmartLab.XBee.Type;
using SmartLab.XBee.Device;

namespace SmartLab.XBee.Indicator
{
    public class XBeeRx16IOSampleIndicator : RxIOSampleBase
    {
        public XBeeRx16IOSampleIndicator(APIFrame frame)
            : base(frame)
        { }

        public override int GetRSSI()
        {
            return GetFrameData()[3] * -1;
        }

        public override IOSamples[] GetIOSamples()
        {
            return RxIOSampleBase.XBeeSamplesParse(this.GetFrameData(), 5);
        }

        public override ReceiveStatus GetReceiveStatus()
        {
            return (ReceiveStatus)this.GetFrameData()[4];
        }

        public override Address GetRemoteDevice()
        {
            return new Address(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, GetFrameData()[1], GetFrameData()[2] });
        }
    }
}