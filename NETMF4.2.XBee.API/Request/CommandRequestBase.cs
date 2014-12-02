using SmartLab.XBee.Type;

namespace SmartLab.XBee.Request
{
    public abstract class CommandRequestBase : RequestBase
    {
        public CommandRequestBase(int length, API_IDENTIFIER identifier, byte FrameID)
            : base(length, identifier, FrameID) 
        { } 
        
        public abstract void SetAppleChanges(bool AppleChanges);

        public abstract void SetCommand(ATCommand Command);

        public abstract void SetParameter(byte[] Parameter);
    }
}
