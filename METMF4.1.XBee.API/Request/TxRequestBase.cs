using SmartLab.XBee.Options;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Request
{
    public abstract class TxRequestBase : RequestBase, ITx
    {
        public TxRequestBase(int length, API_IDENTIFIER identifier, byte frameID) :
            base(length, identifier, frameID)
        { }

        public abstract void SetPayload(byte[] data);

        public abstract void SetPayload(byte[] data,int offset, int length);

        public abstract void SetTransmitOptions(OptionsBase transmitOptions);

        public abstract void SetRemoteAddress(DeviceAddress remoteAddress);
    }
}