using SmartLab.XBee.Type;

namespace SmartLab.XBee.Response
{
    public abstract class ResponseBase: APIFrame
    {
        public ResponseBase(APIFrame frame)
            : base(frame)
        { }
    }
}
