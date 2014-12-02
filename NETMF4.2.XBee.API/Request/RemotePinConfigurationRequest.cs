using SmartLab.XBee.Options;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Request
{
    public class RemotePinConfigurationRequest : RemoteATCommandRequest
    {
        public RemotePinConfigurationRequest(byte FrameID, DeviceAddress RemoteDevice, Device.Pin Pin, Device.Pin.Functions Function)
            : base(FrameID, RemoteDevice, RemoteCommandOptions.ApplyChanges, new ATCommand(Pin.COMMAND), new byte[] { (byte)Function })
        { }

        public RemotePinConfigurationRequest(DeviceAddress RemoteDevice, Device.Pin Pin, Device.Pin.Functions Function)
            : base(RemoteDevice, RemoteCommandOptions.ApplyChanges, new ATCommand(Pin.COMMAND), new byte[] { (byte)Function })
        { }

        public void SetPinFunction(Device.Pin.Functions Functions)
        {
            SetParameter(new byte[] { (byte)Functions });
        }
    }
}
