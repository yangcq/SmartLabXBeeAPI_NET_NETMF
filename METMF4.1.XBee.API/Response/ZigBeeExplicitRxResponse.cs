using SmartLab.XBee.Device;
using SmartLab.XBee.Status;
using System;

namespace SmartLab.XBee.Response
{
    public class ZigBeeExplicitRxResponse : RxPayloadBase
    {
        public ZigBeeExplicitRxResponse(APIFrame frame)
            : base(frame)
        { }

        public override byte[] GetReceivedData()
        {
            int length = this.GetReceivedDataLength();

            if (length <=0)
                return null;

            byte[] cache = new byte[length];
            Array.Copy(this.GetFrameData(), 18, cache, 0, length);
            return cache;
        }

        public override int GetReceivedDataOffset() { return 18; }

        public override byte GetReceivedData(int index) { return this.GetFrameData()[18 + index]; }

        public override int GetReceivedDataLength() { return this.GetPosition() - 18; }

        public ExplicitAddress GetExplicitRemoteDevice()
        {
            return (ExplicitAddress) GetRemoteDevice();
        }

        public override Address GetRemoteDevice()
        {
            byte[] address1 = new byte[10];
            Array.Copy(this.GetFrameData(), 18, address1, 0, 10);

            byte[] address2 = new byte[6];
            Array.Copy(this.GetFrameData(), 18, address2, 0, 6);

            return new ExplicitAddress(address1, address2);
        }

        public override ReceiveStatus GetReceiveStatus()
        {
            return (ReceiveStatus)this.GetFrameData()[17];
        }
    }
}