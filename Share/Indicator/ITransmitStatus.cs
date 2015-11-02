using SmartLab.XBee.Status;

namespace SmartLab.XBee.Indicator
{
    public interface ITransmitStatus
    {
        int GetFrameID();

        DeliveryStatus GetDeliveryStatus();
    }
}