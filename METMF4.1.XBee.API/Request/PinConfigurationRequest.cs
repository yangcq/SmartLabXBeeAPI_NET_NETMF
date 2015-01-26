using SmartLab.XBee.Type;

namespace SmartLab.XBee.Request
{
    public class PinConfigurationRequest : ATCommandRequest
    {
        public PinConfigurationRequest(byte FrameID, Device.Pin Pin, Device.Pin.Functions Functions)
            : base(FrameID, new ATCommand(Pin.COMMAND), new byte[] { (byte)Functions })
        { }

        public void SetPinFunction(Device.Pin.Functions Functions) { this.SetContent(4, (byte)Functions); }
    }
}