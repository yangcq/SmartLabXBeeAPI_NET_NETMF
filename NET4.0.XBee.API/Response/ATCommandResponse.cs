using SmartLab.XBee.Status;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Response
{
    public class ATCommandResponse : CommandResponseBase
    {
        public ATCommandResponse(APIFrame frame)
            : base(frame)
        { }

        public override ATCommand GetRequestCommand()
        {
            return new ATCommand(new byte[2] { this.GetFrameData()[2], this.GetFrameData()[3] });
        }

        public override CommandStatus GetCommandStatus()
        {
            return (CommandStatus)this.GetFrameData()[4];
        }

        /// <summary>
        /// if parameter not presented, null will be returned.
        /// </summary>
        /// <returns></returns>
        public override byte[] GetParameter()
        {
            if (this.GetPosition() > 5)
                return this.GetFrameData().ExtractRangeFromArray(5, this.GetPosition() - 5);
            else return null;
        }
    }
}