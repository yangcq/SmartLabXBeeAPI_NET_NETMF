using SmartLab.XBee.Device;
using SmartLab.XBee.Status;
using System;

namespace SmartLab.XBee.Indicator
{
    public class ZigBeeIOSampleIndicator : RxIOSampleBase
    {
        public ZigBeeIOSampleIndicator(APIFrame frame)
            : base(frame)
        { }

        public override Address GetRemoteDevice()
        {
            byte[] cache = new byte[10];
            Array.Copy(this.GetFrameData(), 1, cache, 0, 10);
            return new Address(cache);
        }

        public override ReceiveStatus GetReceiveStatus()
        {
            return (ReceiveStatus)this.GetFrameData()[11];
        }

        public override IOSamples[] GetIOSamples()
        {
            return RxIOSampleBase.ZigBeeSamplesParse(this.GetFrameData(), 12);
        }
    }
}