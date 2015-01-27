using System.Text;
using SmartLab.XBee.Status;
using SmartLab.XBee.Type;
using SmartLab.XBee.Device;

namespace SmartLab.XBee.Response
{
    public class NodeIdentificationResponse : ZigBeeRxBase
    {
        private int offset = 0;

        public NodeIdentificationResponse(APIFrame frame)
            : base(frame)
        { this.offset = this.GetPosition() - 8; }

        public override int GetReceivedDataOffset() { return 22; }

        public override byte[] GetReceivedData()
        {
            return GetFrameData().ExtractRangeFromArray(22, this.GetPosition() - 22);
        }

        public override ReceiveStatus GetReceiveStatus()
        {
            return (ReceiveStatus)this.GetFrameData()[11];
        }

        public override Address GetRemoteDevice()
        {
            return new Address(GetFrameData().ExtractRangeFromArray(14, 8), GetFrameData().ExtractRangeFromArray(12, 2));
        }

        public Address GetSenderDevice()
        {
            return new Address(GetFrameData().ExtractRangeFromArray(1, 10));
        }

        public string GetNIString()
        {
            return new string(UTF8Encoding.UTF8.GetChars(this.GetFrameData().ExtractRangeFromArray(22, this.GetPosition() - 31)));
        }

        public int GetParentNetworkAddress()
        {
            return this.GetFrameData()[offset] << 8 | this.GetFrameData()[offset + 1];
        }

        public DeviceType GetDeviceType()
        {
            return (DeviceType)this.GetFrameData()[offset + 2];
        }

        public SourceEvent GetSourceEvent()
        {
            return (SourceEvent)this.GetFrameData()[offset + 3];
        }

        public int GetDigiProfileID()
        {
            return this.GetFrameData()[offset + 4] << 8 | this.GetFrameData()[offset + 5];
        }

        public int GetManufacturerID()
        {
            return this.GetFrameData()[offset + 6] << 8 | this.GetFrameData()[offset + 7];
        }
    }
}