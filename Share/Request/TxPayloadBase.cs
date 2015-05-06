using SmartLab.XBee.Options;
using SmartLab.XBee.Type;
using SmartLab.XBee.Device;

namespace SmartLab.XBee.Request
{
    public abstract class TxPayloadBase : TxBase
    {
        public TxPayloadBase(int length, API_IDENTIFIER identifier, byte frameID) :
            base(length, identifier, frameID)
        { }

        public abstract void SetPayload(byte[] data);

        public abstract void SetPayload(byte[] data,int offset, int length);

        public abstract void SetTransmitOptions(OptionsBase transmitOptions);

        public abstract void SetRemoteAddress(Address remoteAddress);
    }
}