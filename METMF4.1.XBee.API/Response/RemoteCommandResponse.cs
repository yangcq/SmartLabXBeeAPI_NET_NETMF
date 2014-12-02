using SmartLab.XBee.Status;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Response
{
    public class RemoteCommandResponse : CommandResponseBase
    {
        public RemoteCommandResponse(ResponseBase Frame)
            : base(Frame)
        { }

        public override ATCommand GetRequestCommand()
        {
            return new ATCommand(new byte[2] { this.FrameData[12], this.FrameData[13] });
        }

        public override CommandStatus GetCommandStatus()
        {
            return (CommandStatus)this.FrameData[14];
        }

        public override byte[] GetParameter()
        {
            if (this.Length > 15)
                return this.FrameData.ExtractRangeFromArray(15, this.Length - 15);
            else return null;
        }

        public DeviceAddress GetRemoteDevice()
        {
            return new DeviceAddress(FrameData.ExtractRangeFromArray(2, 10));
        }
    }
}