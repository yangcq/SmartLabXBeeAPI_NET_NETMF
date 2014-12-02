using System.Text;
using SmartLab.XBee.Status;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Response
{
    public class NodeIdentificationResponse : ZigBeeRxBase
    {
        private int offset = 0;

        public NodeIdentificationResponse(ResponseBase Frame)
            : base(Frame)
        { this.offset = this.Length - 8; }

        public override byte[] GetReceivedData()
        {
            return FrameData.ExtractRangeFromArray(22, Length - 22);
        }

        public override ReceiveStatus GetReceiveStatus()
        {
            return (ReceiveStatus)this.FrameData[11];
        }

        public override DeviceAddress GetRemoteDevice()
        {
            return new DeviceAddress(FrameData.ExtractRangeFromArray(14, 8), FrameData.ExtractRangeFromArray(12, 2));
        }

        public DeviceAddress GetSenderDevice()
        {
            return new DeviceAddress(FrameData.ExtractRangeFromArray(1, 10));
        }

        public string GetNIString()
        {
            return new string(UTF8Encoding.UTF8.GetChars(this.FrameData.ExtractRangeFromArray(22, this.Length - 31)));
        }

        public byte[] GetParentNetworkAddress()
        {
            return new byte[2] { this.FrameData[offset], this.FrameData[offset + 1] };
        }

        public Device.Type GetDeviceType()
        {
            return (Device.Type)this.FrameData[offset + 2];
        }

        public Device.SourceEvent GetSourceEvent()
        {
            return (Device.SourceEvent)this.FrameData[offset + 3];
        }

        public byte[] GetDigiProfileID()
        {
            return new byte[2] { this.FrameData[offset + 4], this.FrameData[offset + 5] };
        }

        public byte[] GetManufacturerID()
        {
            return new byte[2] { this.FrameData[offset + 6], this.FrameData[offset + 7] };
        }
    }
}