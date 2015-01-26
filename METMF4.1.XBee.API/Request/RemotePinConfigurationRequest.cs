using SmartLab.XBee.Options;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Request
{
    public class RemotePinConfigurationRequest : RemoteATCommandRequest
    {
        public RemotePinConfigurationRequest(byte FrameID, DeviceAddress RemoteDevice, Device.Pin Pin, Device.Pin.Functions Function)
            : base(FrameID, RemoteDevice, new ATCommand(Pin.COMMAND), RemoteCommandOptions.ApplyChanges, new byte[] { (byte)Function })
        { }

        public void SetPinFunction(Device.Pin.Functions Functions) { this.SetContent(15, (byte)Functions); }
    }
}