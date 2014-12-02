using SmartLab.XBee.Type;

namespace SmartLab.XBee.Request
{
    public class IOCDetectionConfigRequest : ATCommandRequest
    {
        public IOCDetectionConfigRequest(byte FrameID, Device.Pin[] Pins)
            : base(FrameID, ATCommands.Digital_IO_Change_Detection, Device.Pin.IOChangeDetectionConfiguration(Pins))
        { }
        public IOCDetectionConfigRequest(Device.Pin[] Pins)
            : base(ATCommands.Digital_IO_Change_Detection, Device.Pin.IOChangeDetectionConfiguration(Pins))
        { }
    }
}