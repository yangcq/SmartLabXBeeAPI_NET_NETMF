using SmartLab.XBee.Options;

namespace SmartLab.XBee.Request
{
    public interface ITx
    {
        void SetTransmitOptions(OptionsBase transmitOptions);

        void SetPayload(byte[] data);
    }
}
