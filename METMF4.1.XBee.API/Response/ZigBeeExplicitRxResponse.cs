using SmartLab.XBee.Status;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Response
{
    public class ZigBeeExplicitRxResponse : ZigBeeRxBase
    {
        public ZigBeeExplicitRxResponse(APIFrame frame)
            : base(frame)
        { }

        public ExplicitDeviceAddress GetExplicitRemoteDevice()
        {
            return (ExplicitDeviceAddress) GetRemoteDevice();
        }

        public override DeviceAddress GetRemoteDevice()
        {
            return new ExplicitDeviceAddress(GetFrameData().ExtractRangeFromArray(1, 10), GetFrameData().ExtractRangeFromArray(11, 6));
        }

        public override ReceiveStatus GetReceiveStatus()
        {
            return (ReceiveStatus)this.GetFrameData()[17];
        }

        public override byte[] GetReceivedData()
        {
            return this.GetFrameData().ExtractRangeFromArray(18, this.GetPosition() - 18);
        }

        public override int GetReceivedDataOffset() { return 18; }
    }
}