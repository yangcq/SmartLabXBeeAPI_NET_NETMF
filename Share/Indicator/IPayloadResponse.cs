using SmartLab.XBee.Device;
using SmartLab.XBee.Status;

namespace SmartLab.XBee.Indicator
{
    public interface IPayloadResponse
    {
        ReceiveStatus GetReceiveStatus();

        Address GetRemoteDevice();

        byte[] GetReceivedData();

        int GetReceivedDataOffset();

        byte GetReceivedData(int index);

        int GetReceivedDataLength();

        /// <summary>
        /// not apply to ZigBee
        /// </summary>
        /// <returns></returns>
        int GetRSSI();
    }
}