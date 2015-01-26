using SmartLab.XBee.Status;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Response
{
    public class XBeeTxStatusResponse : TxStatusBase
    {
        public XBeeTxStatusResponse(APIFrame frame)
            : base(frame)
        { }

        public override DeliveryStatus GetDeliveryStatus()
        {
            return (DeliveryStatus)this.GetFrameData()[2];
        }
    }
}