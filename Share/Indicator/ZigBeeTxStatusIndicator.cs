using SmartLab.XBee.Core;
using SmartLab.XBee.Status;

namespace SmartLab.XBee.Indicator
{
    public class ZigBeeTxStatusIndicator : RxBase, ITransmitStatus
    {
        public ZigBeeTxStatusIndicator(APIFrame frame)
            : base(frame)
        { }

        public int GetFrameID()
        {
            return this.GetFrameData()[1];
        }

        public DeliveryStatus GetDeliveryStatus()
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
