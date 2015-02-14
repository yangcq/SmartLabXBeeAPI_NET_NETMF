using SmartLab.XBee.Status;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Response
{
    public abstract class TxStatusBase : RxBase
    {
        public TxStatusBase(APIFrame frame)
            : base(frame)
        { }

        public int GetFrameID()
        {
            return this.GetFrameData()[1];
        }

        public abstract DeliveryStatus GetDeliveryStatus();
    }
}