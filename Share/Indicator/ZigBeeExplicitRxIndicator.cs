using System;
using SmartLab.XBee.Core;
using SmartLab.XBee.Device;
using SmartLab.XBee.Status;

namespace SmartLab.XBee.Indicator
{
    public class ZigBeeExplicitRxIndicator : RxBase, IPayloadResponse
    {
        public ZigBeeExplicitRxIndicator(APIFrame frame)
            : base(frame)
        { }

        public byte[] GetReceivedData()
        {
            int length = this.GetReceivedDataLength();

            if (length <=0)
                return null;

            byte[] cache = new byte[length];
            Array.Copy(this.GetFrameData(), 18, cache, 0, length);
            return cache;
        }

        public int GetReceivedDataOffset() { return 18; }

        public byte GetReceivedData(int index) { return this.GetFrameData()[18 + index]; }

        public int GetReceivedDataLength() { return this.GetPosition() - 18; }

        public ExplicitAddress GetExplicitRemoteDevice()
        {
            return (ExplicitAddress) GetRemoteDevice();
        }

        public  Address GetRemoteDevice()
        {
            byte[] address1 = new byte[10];
            Array.Copy(this.GetFrameData(), 1, address1, 0, 10);

            byte[] address2 = new byte[6];
            Array.Copy(this.GetFrameData(), 11, address2, 0, 6);

            return new ExplicitAddress(address1, address2);
        }

        public ReceiveStatus GetReceiveStatus()
        {
            return (ReceiveStatus)this.GetFrameData()[17];
        }

        public int GetRSSI()
        {
            return 0;
        }
    }
}