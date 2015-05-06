using SmartLab.XBee.Status;
using SmartLab.XBee.Type;
using SmartLab.XBee.Device;
using System;

namespace SmartLab.XBee.Response
{
    public class XBeeRx64Response : RxPayloadBase
    {
        public XBeeRx64Response(APIFrame frame)
            : base(frame)
        { }

        public override byte[] GetReceivedData()
        {
            int length = this.GetReceivedDataLength();

            if (length <= 0)
                return null;

            byte[] cache = new byte[length];
            Array.Copy(this.GetFrameData(), 11, cache, 0, length);
            return cache;
        }

        public override int GetReceivedDataOffset() { return 11; }

        public override byte GetReceivedData(int index) { return this.GetFrameData()[11 + index]; }

        public override int GetReceivedDataLength() { return this.GetPosition() - 11; }

        public override int GetRSSI()
        {
            return this.GetFrameData()[9] * -1;
        }

        public override ReceiveStatus GetReceiveStatus()
        {
            return (ReceiveStatus)this.GetFrameData()[10];
        }

        public override Address GetRemoteDevice()
        {
            return new Address(new byte[] { GetFrameData()[1], GetFrameData()[2], GetFrameData()[3], GetFrameData()[4], GetFrameData()[5], GetFrameData()[6], GetFrameData()[7], GetFrameData()[8], 0x00, 0x00 });
        }
    }
}