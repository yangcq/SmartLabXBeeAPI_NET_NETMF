using SmartLab.XBee.Type;
namespace SmartLab.XBee.Response
{
    public abstract class XBeeRxBase : RxResponseBase, IRSSI
    {
        public XBeeRxBase(APIFrame frame)
            : base(frame)
        { }

        public abstract int GetRSSI();
    }
}