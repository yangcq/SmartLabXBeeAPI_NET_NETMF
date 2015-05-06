using SmartLab.XBee.Status;
using SmartLab.XBee.Type;
using SmartLab.XBee.Device;
using System;

namespace SmartLab.XBee.Response
{
    public class XBeeRx16Response : RxPayloadBase
    {
        public XBeeRx16Response(APIFrame frame)
            : base(frame)
        { }

        public override byte[] GetReceivedData()
        {
            int length = this.GetReceivedDataLength();

            if (length <= 0)
                return null;

            byte[] cache = new byte[length];
            Array.Copy(this.GetFrameData(), 5, cache, 0, length);
            return cache;
        }

        public override int GetReceivedDataOffset() { return 5; }

        public override byte GetReceivedData(int index) { return this.GetFrameData()[5 + index]; }

        public override int GetReceivedDataLength() { return this.GetPosition() - 5; }

        public override int GetRSSI()
        {
            return this.GetFrameData()[3] * -1;
        }

        public override ReceiveStatus GetReceiveStatus()
        {
            return (ReceiveStatus)this.GetFrameData()[4];
        }

        public override Address GetRemoteDevice()
        {
            return new Address(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, GetFrameData()[1], GetFrameData()[2] });
        }
    }
}