using SmartLab.XBee.Status;
using SmartLab.XBee.Type;
using System;

namespace SmartLab.XBee.Indicator
{
    public class ATCommandIndicator : CommandResponseBase
    {
        public ATCommandIndicator(APIFrame frame)
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
            int length = this.GetParameterLength();

            if (length <= 0)
                return null;

            byte[] cache = new byte[length];
            Array.Copy(this.GetFrameData(), 5, cache, 0, length);
            return cache;
        }

        public override byte GetParameter(int index) { return this.GetFrameData()[5 + index];}

        public override int GetParameterLength() { return this.GetPosition() - 5; }

        public override int GetParameterOffset() { return 5; }
    }
}