using SmartLab.XBee.Status;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Response
{
    public class ZigBeeTxStatusResponse : TxStatusBase
    {
        public ZigBeeTxStatusResponse(APIFrame frame)
            : base(frame)
        { }

        public override DeliveryStatus GetDeliveryStatus()
        {
            return (DeliveryStatus)this.GetFrameData()[5];
        }

        public int GetDestinationAddress16()
        {
            return this.GetFrameData()[2] << 8 | this.GetFrameData()[3];
        }

        public byte GetTransmitRetryCount()
        {
            return this.GetFrameData()[4];
        }

        public ZigBeeDiscoveryStatus GetDiscoveryStatus()
        {
            return (ZigBeeDiscoveryStatus)this.GetFrameData()[6];
        }
    }
}
