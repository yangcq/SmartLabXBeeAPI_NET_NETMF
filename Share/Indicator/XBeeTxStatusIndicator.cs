using SmartLab.XBee.Core;
using SmartLab.XBee.Status;

namespace SmartLab.XBee.Indicator
{
    public class XBeeTxStatusIndicator : RxBase, ITransmitStatus
    {
        public XBeeTxStatusIndicator(APIFrame frame)
            : base(frame)
        { }

        public int GetFrameID()
        {
            return this.GetFrameData()[1];
        }

        public DeliveryStatus GetDeliveryStatus()
        {
            return (DeliveryStatus)this.GetFrameData()[2];
        }
    }
}