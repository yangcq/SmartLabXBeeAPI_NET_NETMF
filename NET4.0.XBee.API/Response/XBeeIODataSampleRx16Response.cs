using SmartLab.XBee.Status;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Response
{
    public class XBeeIODataSampleRx16Response : XBeeIODataSampleRxBase
    {
        public XBeeIODataSampleRx16Response(ResponseBase Frame)
            : base(Frame)
        { }

        public override int GetRSSI()
        {
            return FrameData[3] * -1;
        }

        public override IOSamples GetIOSamples()
        {
            return SamplesParse(this.FrameData.ExtractRangeFromArray(5, this.Length - 5));
        }

        public override ReceiveStatus GetReceiveStatus()
        {
            return (ReceiveStatus)this.FrameData[4];
        }

        public override DeviceAddress GetRemoteDevice()
        {
            return new DeviceAddress(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, FrameData[1], FrameData[2] });
        }

        public override byte[] GetReceivedData()
        {
            return this.FrameData.ExtractRangeFromArray(3, this.Length - 3);
        }
    }
}