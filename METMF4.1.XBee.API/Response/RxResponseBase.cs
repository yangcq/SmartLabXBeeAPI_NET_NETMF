using SmartLab.XBee.Status;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Response
{
    public abstract class RxResponseBase : ResponseBase,IRxResponse
    {
        public RxResponseBase(ResponseBase Frame) 
        : base(Frame)
        { }

        public abstract ReceiveStatus GetReceiveStatus();

        public abstract DeviceAddress GetRemoteDevice();

        public abstract byte[] GetReceivedData();
    }
}
