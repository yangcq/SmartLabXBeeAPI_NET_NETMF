using SmartLab.XBee.Status;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Response
{
    public interface ICommandResponse
    {
        int GetFrameID();

        ATCommand GetRequestCommand();

        CommandStatus GetCommandStatus();

        byte[] GetParameter();
    }
}