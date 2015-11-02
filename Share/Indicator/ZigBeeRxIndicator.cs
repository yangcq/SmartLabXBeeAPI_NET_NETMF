using System;
using SmartLab.XBee.Core;
using SmartLab.XBee.Device;
using SmartLab.XBee.Status;

namespace SmartLab.XBee.Indicator
{
    public class ZigBeeRxIndicator : RxBase, IPayloadResponse
    {
        public ZigBeeRxIndicator(APIFrame frame)
            : base(frame)
        { }

        public byte[] GetReceivedData()
        {
            int length = this.GetReceivedDataLength();

            if (length <= 0)
                return null;

            byte[] cache = new byte[length];
            Array.Copy(this.GetFrameData(), 12, cache, 0, length);
            return cache;
        }

        public int GetReceivedDataOffset() { return 12; }

        public byte GetReceivedData(int index) { return this.GetFrameData()[12 + index]; }

        public int GetReceivedDataLength() { return this.GetPosition() - 12; }

        public ReceiveStatus GetReceiveStatus()
        {
            return (ReceiveStatus)this.GetFrameData()[11];
        }

        public Address GetRemoteDevice()
        {
            byte[] cache = new byte[10];
            Array.Copy(this.GetFrameData(), 1, cache, 0, 10);
            return new Address(cache);
        }

        public int GetRSSI()
        {
            return 0;
        }

    }
}