using SmartLab.XBee.Options;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Request
{
    public abstract class TxRequestBase : RequestBase, ITx
    {
        public TxRequestBase(int length, API_IDENTIFIER identifier, byte FrameID) :
            base(length, identifier, FrameID)
        { }

        public abstract void SetPayload(byte[] data);

        public abstract void SetTransmitOptions(OptionsBase TransmitOptions);
    }
}