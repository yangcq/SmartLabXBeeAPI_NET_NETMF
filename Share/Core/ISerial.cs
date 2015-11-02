namespace SmartLab.XBee.Core
{
    public interface ISerial
    {
        /// <summary>
        /// if success return non zero, -1 means something is wrong
        /// </summary>
        /// <returns></returns>
        int ReadByte();

        void WriteByte(byte data);

        /// <summary>
        /// check if the serial port is already open
        /// </summary>
        /// <returns></returns>
        bool IsOpen();

        void Open();

        void Close();

        bool Peek();
    }
}