using System.Text;
using SmartLab.XBee.Status;
using SmartLab.XBee.Type;
using SmartLab.XBee.Device;
using System;

namespace SmartLab.XBee.Indicator
{
    public class NodeIdentificationIndicator : RxBase
    {
        private int offset = 0;

        public NodeIdentificationIndicator(APIFrame frame)
            : base(frame)
        { this.offset = this.GetPosition() - 8; }

        public ReceiveStatus GetReceiveStatus()
        {
            return (ReceiveStatus)this.GetFrameData()[11];
        }

        public Address GetRemoteDevice()
        {
            byte[] cache = new byte[10];
            Array.Copy(this.GetFrameData(), 14, cache, 0, 8);
            cache[8] = this.GetFrameData()[12];
            cache[9] = this.GetFrameData()[13];
            return new Address(cache);
        }

        public Address GetSenderDevice()
        {
            byte[] cache = new byte[10];
            Array.Copy(this.GetFrameData(), 1, cache, 0, 10);
            return new Address(cache);
        }

        public string GetNIString()
        {
            int length = this.GetPosition() - 31;

            if (length <= 0)
                return string.Empty;

            byte[] cache = new byte[length];
            Array.Copy(this.GetFrameData(), 22, cache, 0, length);
            return new string(UTF8Encoding.UTF8.GetChars(cache));
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