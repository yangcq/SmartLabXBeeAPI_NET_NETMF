using SmartLab.XBee.Type;

namespace SmartLab.XBee.Request
{
    public interface ICommandRequest
    {
        void SetAppleChanges(bool appleChanges);

        void SetCommand(ATCommand command);

        void SetParameter(byte[] parameter);

        void SetParameter(byte[] parameter, int offset, int length);
    }
}
