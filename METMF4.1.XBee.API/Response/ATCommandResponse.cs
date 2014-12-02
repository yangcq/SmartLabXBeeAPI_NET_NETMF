using SmartLab.XBee.Status;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Response
{
    public class ATCommandResponse : CommandResponseBase
    {
        public ATCommandResponse(ResponseBase Frame)
            : base(Frame)
        { }

        public override ATCommand GetRequestCommand()
        {
            return new ATCommand(new byte[2] { this.FrameData[2], this.FrameData[3] });
        }

        public override CommandStatus GetCommandStatus()
        {
            return (CommandStatus)this.FrameData[4];
        }

        public override byte[] GetParameter()
        {
            if (Length > 5)
                return this.FrameData.ExtractRangeFromArray(5, Length - 5);
            else return null;
        }
    }
}