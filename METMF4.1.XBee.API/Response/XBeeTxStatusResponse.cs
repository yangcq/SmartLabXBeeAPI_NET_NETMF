using SmartLab.XBee.Status;

namespace SmartLab.XBee.Response
{
    public class XBeeTxStatusResponse : TxStatusBase
    {
        public XBeeTxStatusResponse(ResponseBase Frame)
            : base(Frame)
        { }

        public override DeliveryStatus GetDeliveryStatus()
        {
            return (DeliveryStatus)this.FrameData[2];
        }
    }
}