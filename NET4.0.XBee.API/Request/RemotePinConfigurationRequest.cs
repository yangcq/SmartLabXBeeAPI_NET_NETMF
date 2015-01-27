using SmartLab.XBee.Options;
using SmartLab.XBee.Type;
using SmartLab.XBee.Device;

namespace SmartLab.XBee.Request
{
    public class RemotePinConfigurationRequest : RemoteATCommandRequest
    {
        public RemotePinConfigurationRequest(byte frameID, Address remoteAddress, Pin pin, Pin.Functions function)
            : base(frameID, remoteAddress, new ATCommand(pin.COMMAND), RemoteCommandOptions.ApplyChanges, new byte[] { (byte)function })
        { }

        public void SetPinFunction(Pin.Functions functions) { this.SetContent(15, (byte)functions); }
    }
}