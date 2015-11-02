using System;
using SmartLab.XBee.Core;
using SmartLab.XBee.Device;
using SmartLab.XBee.Status;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Indicator
{
    public class RemoteCommandIndicator : RxBase, ICommandResponse
    {
        public RemoteCommandIndicator(APIFrame frame)
            : base(frame)
        { }

        public int GetFrameID() { return GetFrameData()[1]; }

        public ATCommand GetRequestCommand()
        {
            return new ATCommand(new byte[2] { this.GetFrameData()[12], this.GetFrameData()[13] });
        }

        public CommandStatus GetCommandStatus()
        {
            return (CommandStatus)this.GetFrameData()[14];
        }

        public Address GetRemoteDevice()
        {
            byte[] cache = new byte[10];
            Array.Copy(this.GetFrameData(), 2, cache, 0, 10);
            return new Address(cache);
        }

        public byte[] GetParameter()
        {
            int length = this.GetParameterLength();

            if (length <= 0)
                return null;

            byte[] cache = new byte[length];
            Array.Copy(this.GetFrameData(), 15, cache, 0, length);
            return cache;
        }

        public byte GetParameter(int index) { return this.GetFrameData()[15 + index]; }

        public int GetParameterLength() { return this.GetPosition() - 15; }

        public int GetParameterOffset() { return 15; }
    }
}