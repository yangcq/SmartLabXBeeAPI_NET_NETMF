using System;
using SmartLab.XBee.Device;
using SmartLab.XBee.Status;

namespace SmartLab.XBee.Indicator
{
    public class ZigBeeRxIndicator : RxPayloadBase
    {
        public ZigBeeRxIndicator(APIFrame frame)
            : base(frame)
        { }

        public override byte[] GetReceivedData()
        {
            int length = this.GetReceivedDataLength();

            if (length <= 0)
                return null;

            byte[] cache = new byte[length];
            Array.Copy(this.GetFrameData(), 12, cache, 0, length);
            return cache;
        }

        public override int GetReceivedDataOffset() { return 12; }

        public override byte GetReceivedData(int index) { return this.GetFrameData()[12 + index]; }

        public override int GetReceivedDataLength() { return this.GetPosition() - 12; }

        public override ReceiveStatus GetReceiveStatus()
        {
            return (ReceiveStatus)this.GetFrameData()[11];
        }

        public override Address GetRemoteDevice()
        {
            byte[] cache = new byte[10];
            Array.Copy(this.GetFrameData(), 1, cache, 0, 10);
            return new Address(cache);
        }

    }
}