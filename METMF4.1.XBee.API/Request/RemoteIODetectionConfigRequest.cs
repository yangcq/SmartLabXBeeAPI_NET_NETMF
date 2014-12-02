using SmartLab.XBee.Options;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Request
{
    public class RemoteIODetectionConfigRequest : RemoteATCommandRequest
    {
        public RemoteIODetectionConfigRequest(byte FrameID, DeviceAddress RemoteDevice, Device.Pin[] Pins)
            : base(FrameID, RemoteDevice, RemoteCommandOptions.ApplyChanges, ATCommands.Digital_IO_Change_Detection, Device.Pin.IOChangeDetectionConfiguration(Pins))
        { }

        public RemoteIODetectionConfigRequest(DeviceAddress RemoteDevice, Device.Pin[] Pins)
            : base(RemoteDevice, RemoteCommandOptions.ApplyChanges, ATCommands.Digital_IO_Change_Detection, Device.Pin.IOChangeDetectionConfiguration(Pins))
        { }
    }
}