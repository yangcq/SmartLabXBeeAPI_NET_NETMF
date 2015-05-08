using SmartLab.XBee.Status;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Indicator
{
    public class XBeeTxStatusIndicator : TxStatusBase
    {
        public XBeeTxStatusIndicator(APIFrame frame)
            : base(frame)
        { }

        public override DeliveryStatus GetDeliveryStatus()
        {
            return (DeliveryStatus)this.GetFrameData()[2];
        }
    }
}