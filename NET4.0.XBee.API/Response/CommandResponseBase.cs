using SmartLab.XBee.Status;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Response
{
    public abstract class CommandResponseBase : ResponseBase, ICommandResponse
    {
        public CommandResponseBase(APIFrame frame)
            : base(frame)
        { }

        public int GetFrameID() { return GetFrameData()[1]; }

        public abstract ATCommand GetRequestCommand();

        public abstract CommandStatus GetCommandStatus();

        public abstract byte[] GetParameter();
    }
}
