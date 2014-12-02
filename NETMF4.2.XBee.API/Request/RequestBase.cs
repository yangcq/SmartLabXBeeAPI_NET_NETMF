using System;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Request
{
    public abstract class RequestBase : APIFrame
    {
        public RequestBase(int Length, API_IDENTIFIER identifier, byte FrameID)
            : base(Length + 2)
        {
            this.FrameData[0] = (byte)identifier;
            this.FrameData[1] = FrameID;
        }

        protected void SetData(int index, byte[] data)
        {
            rewind(index + data.Length);

            Array.Copy(data, 0, FrameData, index, data.Length);
        }

        public void SetFrameID(byte FrameID)
        {
            this.FrameData[1] = FrameID;
        }

        public byte GetFrameID()
        {
            return this.FrameData[1];
        }
    }
}