using SmartLab.XBee.Status;

namespace SmartLab.XBee.Response
{
    public abstract class TxStatusBase : ResponseBase , ITxStatus
    {
        public TxStatusBase(ResponseBase Frame) 
        :base(Frame)
        { }

        public int GetFrameID()
        {
            return this.FrameData[1];
        }

        public abstract DeliveryStatus GetDeliveryStatus();
    }
}
