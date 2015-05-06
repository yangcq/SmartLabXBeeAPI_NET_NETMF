using SmartLab.XBee.Type;

namespace SmartLab.XBee
{
    public class XBeeAPI : Core
    {
        public XBeeAPI(string COM)
            : this(new SerialData(COM), APIMode.NORMAL)
        { }

        public XBeeAPI(string COM, APIMode mode)
            : this(new SerialData(COM), mode)
        { }

        public XBeeAPI(string COM, int baudRate, APIMode mode)
            : this(new SerialData(COM, baudRate), mode)
        { }

        public XBeeAPI(ISerial serial, APIMode mode)
            : base(serial, mode)
        { }
    }
}
