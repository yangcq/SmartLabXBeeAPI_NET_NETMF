using System.IO.Ports;

namespace SmartLab.XBee
{
    public class SerialData : ISerial
    {
        private SerialPort serialPort;
        private byte[] buffer = new byte[1];

        public SerialData(string COM)
            : this(COM, 9600)
        { }

        public SerialData(string COM, int baudRate)
        {
            this.serialPort = new SerialPort(COM, baudRate, Parity.None, 8, StopBits.One);
        }

        public int ReadByte()
        {
            serialPort.Read(buffer, 0, 1);
            return buffer[0];
        }

        public void WriteByte(byte data)
        {
            buffer[0] = data;
            serialPort.Write(buffer, 0, 1);
        }

        public bool IsOpen()
        {
            return serialPort.IsOpen;
        }

        public void Open()
        {
            if (!serialPort.IsOpen)
                serialPort.Open();
        }

        public void Close()
        {
            serialPort.Close();
        }
    }
}
