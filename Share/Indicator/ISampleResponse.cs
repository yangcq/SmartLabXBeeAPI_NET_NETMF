using SmartLab.XBee.Device;
using SmartLab.XBee.Status;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Indicator
{
    public interface ISampleResponse
    {
        IOSamples[] GetIOSamples();

        ReceiveStatus GetReceiveStatus();

        Address GetRemoteDevice();

        /// <summary>
        /// not apply to ZigBee
        /// </summary>
        /// <returns></returns>
        int GetRSSI();
    }
}