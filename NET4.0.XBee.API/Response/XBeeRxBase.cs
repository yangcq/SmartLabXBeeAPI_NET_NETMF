namespace SmartLab.XBee.Response
{
    public abstract class XBeeRxBase : RxResponseBase, IRSSI
    {
        public XBeeRxBase(ResponseBase Frame)
            : base(Frame)
        { }

        public abstract int GetRSSI();
    }
}