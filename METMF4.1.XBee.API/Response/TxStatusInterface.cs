using SmartLab.XBee.Status;

namespace SmartLab.XBee.Response
{
    public interface ITxStatus
    {
        int GetFrameID();
        
        DeliveryStatus GetDeliveryStatus();
    }
}
