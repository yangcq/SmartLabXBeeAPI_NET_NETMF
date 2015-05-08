using SmartLab.XBee.Status;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Indicator
{
    public abstract class CommandResponseBase : RxBase
    {
        public CommandResponseBase(APIFrame frame)
            : base(frame)
        { }

        public int GetFrameID() { return GetFrameData()[1]; }

        public abstract ATCommand GetRequestCommand();

        public abstract CommandStatus GetCommandStatus();

        public abstract byte[] GetParameter();

        public abstract byte GetParameter(int index);

        public abstract int GetParameterLength();

        public abstract int GetParameterOffset();
    }
}
