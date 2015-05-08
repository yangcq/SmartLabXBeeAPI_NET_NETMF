using SmartLab.XBee.Status;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Indicator
{
    public class ModemStatusIndicator : RxBase
    {
        public ModemStatusIndicator(APIFrame frame)
            : base(frame)
        { }

        public ModemStatus GetModemStatus()
        {
            return (ModemStatus)this.GetFrameData()[1];
        }
    }
}