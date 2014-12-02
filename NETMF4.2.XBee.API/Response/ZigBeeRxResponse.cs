using SmartLab.XBee.Status;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Response
{
    public class ZigBeeRxResponse : ZigBeeRxBase
    {
        public ZigBeeRxResponse(ResponseBase Frame)
            : base(Frame)
        { }

        public override ReceiveStatus GetReceiveStatus()
        {
            return (ReceiveStatus)this.FrameData[11];
        }

        public override byte[] GetReceivedData()
        {
            return this.FrameData.ExtractRangeFromArray(12, this.Length - 12);
        }

        public override DeviceAddress GetRemoteDevice()
        {
            return new DeviceAddress(FrameData.ExtractRangeFromArray(1, 10));
        }
    }
}