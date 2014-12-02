using SmartLab.XBee.Status;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Response
{
    public abstract class CommandResponseBase : ResponseBase, ICommandResponse
    {
        public CommandResponseBase(ResponseBase Frame)
            : base(Frame)
        { }

        public int GetFrameID() { return FrameData[1]; }

        public abstract ATCommand GetRequestCommand();

        public abstract CommandStatus GetCommandStatus();

        public abstract byte[] GetParameter();
    }
}
