using SmartLab.XBee.Status;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Response
{
    public abstract class RxResponseBase : ResponseBase
    {
        public RxResponseBase(APIFrame frame)
            : base(frame)
        { }

        public abstract ReceiveStatus GetReceiveStatus();

        public abstract DeviceAddress GetRemoteDevice();

        public abstract byte[] GetReceivedData();

        public abstract int GetReceivedDataOffset();
    }
}