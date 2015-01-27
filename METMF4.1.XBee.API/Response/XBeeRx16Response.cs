using SmartLab.XBee.Status;
using SmartLab.XBee.Type;
using SmartLab.XBee.Device;

namespace SmartLab.XBee.Response
{
    public class XBeeRx16Response : XBeeRxBase
    {
        public XBeeRx16Response(APIFrame frame)
            : base(frame)
        { }

        public override ReceiveStatus GetReceiveStatus()
        {
            return (ReceiveStatus)this.GetFrameData()[4];
        }

        public override byte[] GetReceivedData()
        {
            return this.GetFrameData().ExtractRangeFromArray(5, this.GetPosition() - 5);
        }

        public override int GetReceivedDataOffset() { return 5; }

        public override Address GetRemoteDevice()
        {
            return new Address(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, GetFrameData()[1], GetFrameData()[2] });
        }

        public override int GetRSSI()
        {
            return this.GetFrameData()[3] * -1;
        }
    }
}