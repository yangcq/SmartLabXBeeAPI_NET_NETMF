using SmartLab.XBee.Options;

namespace SmartLab.XBee.Request
{
    public interface ITx
    {
        void SetTransmitOptions(OptionsBase TransmitOptions);

        void SetPayload(byte[] data);
    }
}
