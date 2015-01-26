using SmartLab.XBee.Type;

namespace SmartLab.XBee.Request
{
    public abstract class CommandRequestBase : RequestBase
    {
        public CommandRequestBase(int length, API_IDENTIFIER identifier, byte FrameID)
            : base(length, identifier, FrameID) 
        { } 
        
        public abstract void SetAppleChanges(bool appleChanges);

        public abstract void SetCommand(ATCommand command);

        public abstract void SetParameter(byte[] parameter);

        public abstract void SetParameter(byte[] parameter, int offset, int length);
    }
}
