using SmartLab.XBee.Status;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Response
{
    public class ZigBeeRxResponse : ZigBeeRxBase
    {
        public ZigBeeRxResponse(APIFrame frame)
            : base(frame)
        { }

        public override ReceiveStatus GetReceiveStatus()
        {
            return (ReceiveStatus)this.GetFrameData()[11];
        }

        public override byte[] GetReceivedData()
        {
            return this.GetFrameData().ExtractRangeFromArray(12, this.GetPosition() - 12);
        }

        public override DeviceAddress GetRemoteDevice()
        {
            return new DeviceAddress(GetFrameData().ExtractRangeFromArray(1, 10));
        }

        public override int GetReceivedDataOffset() { return 12; }
    }
}