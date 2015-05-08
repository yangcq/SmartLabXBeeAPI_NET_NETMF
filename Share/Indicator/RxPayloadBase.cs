using SmartLab.XBee.Status;
using SmartLab.XBee.Type;
using SmartLab.XBee.Device;

namespace SmartLab.XBee.Indicator
{
    public abstract class RxPayloadBase : RxBase
    {
        public RxPayloadBase(APIFrame frame)
            : base(frame)
        { }

        public abstract ReceiveStatus GetReceiveStatus();

        public abstract Address GetRemoteDevice();

        public abstract byte[] GetReceivedData();

        public abstract int GetReceivedDataOffset();

        public abstract byte GetReceivedData(int index);

        public abstract int GetReceivedDataLength();

        /// <summary>
        /// not apply to ZigBee
        /// </summary>
        /// <returns></returns>
        public virtual int GetRSSI() { return 0; }
    }
}