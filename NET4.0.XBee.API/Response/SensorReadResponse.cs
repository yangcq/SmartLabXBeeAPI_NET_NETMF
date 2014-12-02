using SmartLab.XBee.Status;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Response
{
    public class SensorReadResponse : ZigBeeRxBase
    {
        public SensorReadResponse(ResponseBase Frame)
            : base(Frame)
        { }

        public override byte[] GetReceivedData()
        {
            return FrameData.ExtractRangeFromArray(12, Length - 12);
        }

        public override DeviceAddress GetRemoteDevice()
        {
            return new DeviceAddress(FrameData.ExtractRangeFromArray(1, 10));
        }

        public override ReceiveStatus GetReceiveStatus() 
        {
            return (ReceiveStatus)this.FrameData[11];
        }

        public Device.OneWireSensors GetOneWireSensor()
        {
            return (Device.OneWireSensors)this.FrameData[12];
        }

        public byte[] GetAD0()
        {
            return new byte[2] { this.FrameData[13], this.FrameData[14] };
        }

        public byte[] GetAD1()
        {
            return new byte[2] { this.FrameData[15], this.FrameData[16] };
        }

        public byte[] GetAD2()
        {
            return new byte[2] { this.FrameData[17], this.FrameData[18] };
        }

        public byte[] GetAD3()
        {
            return  new byte[2] { this.FrameData[19], this.FrameData[20] };
        }

        public byte[] GetThemometer() 
        {
            return new byte[2] { this.FrameData[21], this.FrameData[22] };
        }
    }
}