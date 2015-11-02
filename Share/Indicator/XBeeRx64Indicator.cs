using System;
using SmartLab.XBee.Core;
using SmartLab.XBee.Device;
using SmartLab.XBee.Status;

namespace SmartLab.XBee.Indicator
{
    public class XBeeRx64Indicator : RxBase, IPayloadResponse
    {
        public XBeeRx64Indicator(APIFrame frame)
            : base(frame)
        { }

        public byte[] GetReceivedData()
        {
            int length = this.GetReceivedDataLength();

            if (length <= 0)
                return null;

            byte[] cache = new byte[length];
            Array.Copy(this.GetFrameData(), 11, cache, 0, length);
            return cache;
        }

        public int GetReceivedDataOffset() { return 11; }

        public byte GetReceivedData(int index) { return this.GetFrameData()[11 + index]; }

        public int GetReceivedDataLength() { return this.GetPosition() - 11; }

        public int GetRSSI()
        {
            return this.GetFrameData()[9] * -1;
        }

        public ReceiveStatus GetReceiveStatus()
        {
            return (ReceiveStatus)this.GetFrameData()[10];
        }

        public Address GetRemoteDevice()
        {
            return new Address(new byte[] { GetFrameData()[1], GetFrameData()[2], GetFrameData()[3], GetFrameData()[4], GetFrameData()[5], GetFrameData()[6], GetFrameData()[7], GetFrameData()[8], 0x00, 0x00 });
        }
    }
}