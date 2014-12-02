using SmartLab.XBee.Status;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Response
{
    public class XBeeIODataSampleRx64Response : XBeeIODataSampleRxBase
    {
        public XBeeIODataSampleRx64Response(ResponseBase Frame)
            : base(Frame)
        { }

        public override int GetRSSI()
        {
            return this.FrameData[9] * -1;
        }

        public override IOSamples GetIOSamples()
        {
            return SamplesParse(this.FrameData.ExtractRangeFromArray(11, this.Length - 11));
        }

        public override ReceiveStatus GetReceiveStatus()
        {
            return (ReceiveStatus)this.FrameData[10];
        }

        public override DeviceAddress GetRemoteDevice()
        {
            return new DeviceAddress(new byte[] { FrameData[1], FrameData[2], FrameData[3], FrameData[4], FrameData[5], FrameData[6], FrameData[7], FrameData[8], 0x00, 0x00 });
        }

        public override byte[] GetReceivedData()
        {
            return this.FrameData.ExtractRangeFromArray(9, this.Length - 9);
        }
    }
}