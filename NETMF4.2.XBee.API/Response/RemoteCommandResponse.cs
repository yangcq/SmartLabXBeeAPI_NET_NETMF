using SmartLab.XBee.Status;
using SmartLab.XBee.Type;
using SmartLab.XBee.Device;

namespace SmartLab.XBee.Response
{
    public class RemoteCommandResponse : CommandResponseBase
    {
        public RemoteCommandResponse(APIFrame frame)
            : base(frame)
        { }

        public override ATCommand GetRequestCommand()
        {
            return new ATCommand(new byte[2] { this.GetFrameData()[12], this.GetFrameData()[13] });
        }

        public override CommandStatus GetCommandStatus()
        {
            return (CommandStatus)this.GetFrameData()[14];
        }

        public override byte[] GetParameter()
        {
            if (this.GetPosition() > 15)
                return this.GetFrameData().ExtractRangeFromArray(15, this.GetPosition() - 15);
            else return null;
        }

        public Address GetRemoteDevice()
        {
            return new Address(GetFrameData().ExtractRangeFromArray(2, 10));
        }
    }
}