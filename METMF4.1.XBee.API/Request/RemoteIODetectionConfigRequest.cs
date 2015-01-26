using SmartLab.XBee.Options;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Request
{
    public class RemoteIODetectionConfigRequest : RemoteATCommandRequest
    {
        public RemoteIODetectionConfigRequest(byte FrameID, DeviceAddress RemoteDevice, Device.Pin[] Pins)
            : base(FrameID, RemoteDevice, ATCommands.Digital_IO_Change_Detection, RemoteCommandOptions.ApplyChanges, Device.Pin.IOChangeDetectionConfiguration(Pins))
        { }
    }
}