using SmartLab.XBee.Type;

namespace SmartLab.XBee.Core
{
    public class APIFrame : BufferedArray
    {
        public const byte StartDelimiter = 0x7E;

        private byte CheckSum;

        /// <summary>
        /// a state to indicate whether this packet's checksum is verified while process
        /// </summary>
        private bool isVerify = false;

        public APIFrame(APIFrame frame)
            : base(frame)
        {
            if (frame != null)
            {
                this.CheckSum = frame.CheckSum;
                this.isVerify = frame.isVerify;
            }
        }

        public APIFrame(int payloadLength)
            : base(payloadLength)
        { }

        public API_IDENTIFIER GetFrameType() { return (API_IDENTIFIER)data[0]; }

        /// <summary>
        /// this does not affect the position, will always use position 0
        /// </summary>
        /// <param name="identifier"></param>
        public void SetFrameType(API_IDENTIFIER identifier) { this.SetContent(0, (byte)identifier); }

        public override void Allocate(int length)
        {
            base.Allocate(length);
            this.isVerify = false;
        }

        public override void Rewind()
        {
            base.Rewind();
            this.isVerify = false;
        }

        public bool Convert(APIFrame frame)
        {
            if (frame == null)
                return false;

            this.data = frame.data;
            this.position = frame.position;
            this.CheckSum = frame.CheckSum;
            this.isVerify = frame.isVerify;
            return true;
        }

        /// <summary>
        /// write the value into the current posiont and the posiont + 1
        /// will create more space if position overflow
        /// </summary>
        /// <param name="value"></param>
        public override void SetContent(byte value)
        {
            base.SetContent(value);
            this.isVerify = false;
        }

        /// <summary>
        /// write the value into anywhere and the current positon not affected
        /// will create more space if position overflow
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="value"></param>
        public override void SetContent(int index, byte value)
        {
            base.SetContent(index, value);
            this.isVerify = false;
        }

        /// <summary>
        /// write the value into the current posiont and the posiont + value.length
        /// will create more space if position overflow
        /// </summary>
        /// <param name="value"></param>
        public override void SetContent(byte[] value)
        {
            base.SetContent(value);
            this.isVerify = false;
        }

        /// <summary>
        /// write the value into anywhere and the current positon not affected
        /// will create more space if position overflow
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public override void SetContent(int index, byte[] value)
        {
            base.SetContent(index, value);
            this.isVerify = false;
        }

        /// <summary>
        /// write the value into the current posiont and the posiont + length
        /// will create more space if position overflow
        /// </summary>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        public override void SetContent(byte[] value, int offset, int length)
        {
            base.SetContent(value, offset, length);
            this.isVerify = false;
        }

        /// <summary>
        /// write the value into anywhere and the current positon not affected
        /// will create more space if position overflow
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        public override void SetContent(int index, byte[] value, int offset, int length)
        {
            base.SetContent(index, value, offset, length);
            this.isVerify = false;
        }

        public byte GetCheckSum() { return CheckSum; }

        public void SetCheckSum(byte value) { this.CheckSum = value; }

        public bool VerifyChecksum()
        {
            if (isVerify)
                return true;

            byte temp = 0x00;
            for (int i = 0; i < this.position; i++)
                temp += this.data[i];
            if (temp + this.CheckSum == 0xFF)
                isVerify = true;
            else
                isVerify = false;

            return isVerify;
        }

        public void CalculateChecksum()
        {
            if (this.isVerify)
                return;

            byte CS = 0x00;
            for (int i = 0; i < this.position; i++)
                CS += this.data[i];
            this.CheckSum = (byte)(0xFF - CS);
            this.isVerify = true;
        }
    }
}