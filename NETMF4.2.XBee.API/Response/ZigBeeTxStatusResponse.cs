using SmartLab.XBee.Status;

namespace SmartLab.XBee.Response
{
    public class ZigBeeTxStatusResponse : TxStatusBase
    {
        public ZigBeeTxStatusResponse(ResponseBase Frame)
            : base(Frame)
        { }

        public override DeliveryStatus GetDeliveryStatus()
        {
            return (DeliveryStatus)this.FrameData[5];
        }

        public byte[] GetDestinationAddress16()
        {
            return new byte[2] { this.FrameData[2], this.FrameData[3] };
        }

        public byte GetTransmitRetryCount()
        {
            return this.FrameData[4];
        }

        public ZigBeeDiscoveryStatus GetDiscoveryStatus()
        {
            return (ZigBeeDiscoveryStatus)this.FrameData[6];
        }
    }
}
