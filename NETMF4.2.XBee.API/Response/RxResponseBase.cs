using SmartLab.XBee.Status;
using SmartLab.XBee.Type;
using SmartLab.XBee.Device;

namespace SmartLab.XBee.Response
{
    public abstract class RxResponseBase : ResponseBase
    {
        public RxResponseBase(APIFrame frame)
            : base(frame)
        { }

        public abstract ReceiveStatus GetReceiveStatus();

        public abstract Address GetRemoteDevice();

        public abstract byte[] GetReceivedData();

        public abstract int GetReceivedDataOffset();
    }
}