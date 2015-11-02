using System.IO.Ports;
using SmartLab.XBee.Core;

namespace SmartLab.XBee
{
    public class SerialData : ISerial
    {
        private SerialPort serialPort;

        public SerialData(string COM)
            : this(COM, 9600)
        { }

        public SerialData(string COM, int baudRate)
        {
            this.serialPort = new SerialPort(COM, baudRate, Parity.None, 8, StopBits.One);
        }

        public int ReadByte()
        {
            return serialPort.ReadByte();
        }

        public void WriteByte(byte data)
        {
            serialPort.BaseStream.WriteByte(data);
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
            if (serialPort.IsOpen)
                serialPort.Close();
        }

        public bool Peek()
        {
            if (serialPort.BytesToRead > 0)
                return true;
            else return false;
        }
    }
}