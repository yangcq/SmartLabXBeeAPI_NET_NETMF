using SmartLab.XBee.Type;
namespace SmartLab.XBee.Response
{
    public abstract class ZigBeeRxBase : RxResponseBase
    {
        public ZigBeeRxBase(APIFrame frame)
            : base(frame)
        { }
    }
}
