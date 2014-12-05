using System;

namespace SmartLab.XBee.Type
{
    public abstract class APIFrame
    {
        public const byte StartDelimiter = 0x7E;

        private bool isLock = false;
        private int current = 0;

        /// <summary>
        /// payload length not include the checksum
        /// </summary>
        protected int Length;
        /// <summary>
        /// payload content not include the checksum, the valid length is indicated by this.Length
        /// !! do not use FrameData.Length, this is not the packet's payload length
        /// </summary>
        protected byte[] FrameData;
        protected byte CheckSum;
        /// <summary>
        /// a state to indicate whether this packet's checksum is verified while process
        /// </summary>
        protected bool isVerify = false;

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

        /// <summary>
        /// reset only reallocate memory when the payloadLength is large than max length FrameData can hold
        /// </summary>
        /// <param name="payloadLength"></param>
        public void rewind(int payloadLength)
        {
            isVerify = false;
            isLock = false;
            current = 0;

            // only if the required length is large than the max data frame can hold
            if (payloadLength > FrameData.Length)
                FrameData = new byte[payloadLength];

            Length = payloadLength;
        }

        /// <summary>
        /// return true until all the payload and checksum been fill
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
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