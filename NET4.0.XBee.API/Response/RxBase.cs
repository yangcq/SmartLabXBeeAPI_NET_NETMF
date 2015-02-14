using SmartLab.XBee.Type;

namespace SmartLab.XBee.Response
{
    public abstract class RxBase: APIFrame
    {
        public RxBase(APIFrame frame)
            : base(frame)
        { }
    }
}
