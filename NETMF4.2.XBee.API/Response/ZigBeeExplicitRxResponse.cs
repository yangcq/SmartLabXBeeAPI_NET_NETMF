using SmartLab.XBee.Status;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Response
{
    public class ZigBeeExplicitRxResponse : ZigBeeRxBase
    {
        public ZigBeeExplicitRxResponse(ResponseBase Frame)
            : base(Frame)
        { }

        public ExplicitDeviceAddress GetExplicitRemoteDevice()
        {
            return (ExplicitDeviceAddress) GetRemoteDevice();
        }

        public override DeviceAddress GetRemoteDevice()
        {
            return new ExplicitDeviceAddress(FrameData.ExtractRangeFromArray(1, 10), FrameData.ExtractRangeFromArray(11, 6));
        }

        public override ReceiveStatus GetReceiveStatus()
        {
            return (ReceiveStatus)this.FrameData[17];
        }

        public override byte[] GetReceivedData()
        {
            return this.FrameData.ExtractRangeFromArray(18, this.Length - 18);
        }
    }
}