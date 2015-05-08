using SmartLab.XBee.Status;
using SmartLab.XBee.Type;
using System;
using SmartLab.XBee.Device;

namespace SmartLab.XBee.Indicator
{
    public class ManyToOneRouteIndicator : RxBase
    {
        public ManyToOneRouteIndicator(APIFrame frame)
            : base(frame)
        {  }

        public Address GetRemoteDevice()
        {
            byte[] cache = new byte[10];
            Array.Copy(this.GetFrameData(), 1, cache, 0, 10);
            return new Address(cache);
        }
    }
}