using System;

namespace SmartLab.XBee.Type
{
    public abstract class APIFrame
    {
        public const byte StartDelimiter = 0x7E;

        protected int Length;

        protected byte[] FrameData;

        protected byte CheckSum;

        public APIFrame() { }
        
        public APIFrame(int payloadLength)
        {
            this.Length = payloadLength;
            this.FrameData = new byte[Length];
        }

        public API_IDENTIFIER GetFrameType()
        {
            return (API_IDENTIFIER)FrameData[0];
        }

        protected void SetFrameType(API_IDENTIFIER identifier)
        {
            FrameData[0] = (byte)identifier;
        }

        public int GetLength()
        {
            return Length;
        }

        public byte[] GetFrameData()
        {
            return this.FrameData;
        }

        public byte GetCheckSum()
        {
            return CheckSum;
        }

        protected bool isVerify = false;

        public bool VerifyChecksum()
        {
            if (isVerify)
                return true;

            byte temp = 0x00;
            for (int i = 0; i < Length; i++)
                temp += this.FrameData[i];
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
            for (int i = 0; i < Length; i++)
                CS += this.FrameData[i];
            this.CheckSum = (byte)(0xFF - CS);
            this.isVerify = true;
        }

        private bool isLock = false;
        private int current = 0;

        public void rewind(int payloadLength)
        {
            isVerify = false;
            isLock = false;
            current = 0;

            // only if the required length is large than the max data frame can hold
            if (payloadLength > FrameData.Length)
            {
                byte[] temp = FrameData;
                FrameData = new byte[payloadLength];
                Array.Copy(temp, FrameData, temp.Length);
            }

            Length = payloadLength;
        }

        public bool append(byte value)
        {
            if (isLock)
                return false;
            if (current >= Length)
            {
                CheckSum = value;
                isLock = true;
                return true;
            }
            else
            {
                FrameData[current++] = value;
                return false;
            }
        }
    }
}