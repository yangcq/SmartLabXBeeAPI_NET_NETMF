using System;
using SmartLab.XBee.Core;
using SmartLab.XBee.Status;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Indicator
{
    public class ATCommandIndicator : RxBase, ICommandResponse
    {
        public ATCommandIndicator(APIFrame frame)
            : base(frame)
        { }

        public int GetFrameID() { return GetFrameData()[1]; }

        public ATCommand GetRequestCommand()
        {
            return new ATCommand(new byte[2] { this.GetFrameData()[2], this.GetFrameData()[3] });
        }

        public CommandStatus GetCommandStatus()
        {
            return (CommandStatus)this.GetFrameData()[4];
        }

        /// <summary>
        /// if parameter not presented, null will be returned.
        /// </summary>
        /// <returns></returns>
        public byte[] GetParameter()
        {
            int length = this.GetParameterLength();

            if (length <= 0)
                return null;

            byte[] cache = new byte[length];
            Array.Copy(this.GetFrameData(), 5, cache, 0, length);
            return cache;
        }

        public byte GetParameter(int index) { return this.GetFrameData()[5 + index];}

        public int GetParameterLength() { return this.GetPosition() - 5; }

        public int GetParameterOffset() { return 5; }
    }
}