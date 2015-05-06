namespace SmartLab.XBee
{
    public interface ISerial
    {
        /// <summary>
        /// if success return non zero, -1 means something is wrong
        /// </summary>
        /// <returns></returns>
        int ReadByte();

        void WriteByte(byte data);

        bool IsOpen();

        void Open();

        void Close();
    }
}