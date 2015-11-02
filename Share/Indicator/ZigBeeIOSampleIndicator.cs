using System;
using SmartLab.XBee.Core;
using SmartLab.XBee.Device;
using SmartLab.XBee.Helper;
using SmartLab.XBee.Status;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Indicator
{
    public class ZigBeeIOSampleIndicator : RxBase, ISampleResponse
    {
        public ZigBeeIOSampleIndicator(APIFrame frame)
            : base(frame)
        { }

        public int GetRSSI()
        {
            return 0;
        }

        public Address GetRemoteDevice()
        {
            byte[] cache = new byte[10];
            Array.Copy(this.GetFrameData(), 1, cache, 0, 10);
            return new Address(cache);
        }

        public ReceiveStatus GetReceiveStatus()
        {
            return (ReceiveStatus)this.GetFrameData()[11];
        }

        public IOSamples[] GetIOSamples()
        {
            return IOSampleDecoder.ZigBeeSamplesParse(this.GetFrameData(), 12);
        }
    }
}