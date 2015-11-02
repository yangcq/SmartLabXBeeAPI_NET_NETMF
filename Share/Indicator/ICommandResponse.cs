using SmartLab.XBee.Core;
using SmartLab.XBee.Status;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Indicator
{
    public interface ICommandResponse
    {
        int GetFrameID();

        ATCommand GetRequestCommand();

        CommandStatus GetCommandStatus();

        byte[] GetParameter();

        byte GetParameter(int index);

        int GetParameterLength();

        int GetParameterOffset();
    }
}