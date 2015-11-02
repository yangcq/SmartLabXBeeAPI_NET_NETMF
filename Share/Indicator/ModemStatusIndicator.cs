using SmartLab.XBee.Core;
using SmartLab.XBee.Status;

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