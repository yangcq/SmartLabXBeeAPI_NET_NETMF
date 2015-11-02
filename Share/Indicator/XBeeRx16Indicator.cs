using System;
using SmartLab.XBee.Core;
using SmartLab.XBee.Device;
using SmartLab.XBee.Status;

namespace SmartLab.XBee.Indicator
{
    public class XBeeRx16Indicator : RxBase, IPayloadResponse
    {
        public XBeeRx16Indicator(APIFrame frame)
            : base(frame)
        { }

        public byte[] GetReceivedData()
        {
            int length = this.GetReceivedDataLength();

            if (length <= 0)
                return null;

            byte[] cache = new byte[length];
            Array.Copy(this.GetFrameData(), 5, cache, 0, length);
            return cache;
        }

        public int GetReceivedDataOffset() { return 5; }

        public byte GetReceivedData(int index) { return this.GetFrameData()[5 + index]; }

        public int GetReceivedDataLength() { return this.GetPosition() - 5; }

        public int GetRSSI()
        {
            return this.GetFrameData()[3] * -1;
        }

        public ReceiveStatus GetReceiveStatus()
        {
            return (ReceiveStatus)this.GetFrameData()[4];
        }

        public Address GetRemoteDevice()
        {
            return new Address(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, GetFrameData()[1], GetFrameData()[2] });
        }
    }
}