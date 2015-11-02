using SmartLab.XBee.Device;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Request
{
    public class IOCDetectionConfigRequest : ATCommandRequest
    {
        public IOCDetectionConfigRequest(byte frameID, Pin[] pins)
            : base(frameID, ATCommand.Digital_IO_Change_Detection, Pin.IOChangeDetectionConfiguration(pins))
        { }
    }
}