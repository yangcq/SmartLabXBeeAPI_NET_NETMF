using SmartLab.XBee.Device;
using SmartLab.XBee.Options;

namespace SmartLab.XBee.Request
{
    public interface IPayloadRequest
    {
        void SetPayload(byte[] data);

        void SetPayload(byte[] data,int offset, int length);

        void SetTransmitOptions(OptionsBase transmitOptions);

        void SetRemoteAddress(Address remoteAddress);
    }
}