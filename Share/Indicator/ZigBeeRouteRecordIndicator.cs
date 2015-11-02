using System;
using SmartLab.XBee.Core;
using SmartLab.XBee.Device;
using SmartLab.XBee.Status;

namespace SmartLab.XBee.Indicator
{
    public class RouteRecordIndicator : RxBase
    {
        public RouteRecordIndicator(APIFrame frame)
            : base(frame)
        {  }

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

        public int GetNumberOfAddresses()
        {
            return this.GetFrameData()[12];
        }

        public int[] GetAddresses() 
        {
            int[] records = new int[GetNumberOfAddresses()];

            for (int i = 0; i < records.Length; i++)
                records[i] = this.GetFrameData()[13 + (i << 2)] << 8 | this.GetFrameData()[13 + (i << 2) + 1];

            return records;
        }
    }
}