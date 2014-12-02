using SmartLab.XBee.Type;

namespace SmartLab.XBee.Response
{
    public class ResponseBase: APIFrame
    {
        public ResponseBase(int payloadLength)
            : base(payloadLength)
        { }

        public ResponseBase(ResponseBase Frame)
        {
            this.Length = Frame.Length;
            this.FrameData = Frame.FrameData;
            this.CheckSum = Frame.CheckSum;
            this.isVerify = Frame.isVerify;
        }
    }
}
