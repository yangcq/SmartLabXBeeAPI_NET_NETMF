using SmartLab.XBee.Type;
using SmartLab.XBee.Device;

namespace SmartLab.XBee.Request
{
    public class IOCDetectionConfigRequest : ATCommandRequest
    {
        public IOCDetectionConfigRequest(byte frameID, Pin[] pins)
            : base(frameID, ATCommand.Digital_IO_Change_Detection, Pin.IOChangeDetectionConfiguration(pins))
        { }
    }
}