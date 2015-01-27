using SmartLab.XBee.Device;
using SmartLab.XBee.Status;

namespace SmartLab.XBee.Response
{
    public class ZigBeeExplicitRxResponse : ZigBeeRxBase
    {
        public ZigBeeExplicitRxResponse(APIFrame frame)
            : base(frame)
        { }

        public ExplicitAddress GetExplicitRemoteDevice()
        {
            return (ExplicitAddress) GetRemoteDevice();
        }

        public override Address GetRemoteDevice()
        {
            return new ExplicitAddress(GetFrameData().ExtractRangeFromArray(1, 10), GetFrameData().ExtractRangeFromArray(11, 6));
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