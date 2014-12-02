using SmartLab.XBee.Status;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Response
{
    public interface IRxResponse
    {
        ReceiveStatus GetReceiveStatus();

        DeviceAddress GetRemoteDevice();

        byte[] GetReceivedData();
    }
}
