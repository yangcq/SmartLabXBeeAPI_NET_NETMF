using SmartLab.XBee.Status;

namespace SmartLab.XBee.Response
{
    public class ModemStatusResponse : ResponseBase
    {
        public ModemStatusResponse(ResponseBase Frame)
            : base(Frame)
        { }

        public ModemStatus GetModemStatus()
        {
            return (ModemStatus)this.FrameData[1];
        }
    }
}