using SmartLab.XBee.Status;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Response
{
    public class ModemStatusResponse : RxBase
    {
        public ModemStatusResponse(APIFrame frame)
            : base(frame)
        { }

        public ModemStatus GetModemStatus()
        {
            return (ModemStatus)this.GetFrameData()[1];
        }
    }
}