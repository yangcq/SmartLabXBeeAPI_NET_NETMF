using System;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Request
{
    public abstract class TxBase : APIFrame
    {
        public TxBase(int Length, API_IDENTIFIER identifier, byte frameID)
            : base(Length + 2)
        {
            this.SetFrameType(identifier);
            this.SetFrameID(frameID);
            this.SetPosition(2);
        }

        /// <summary>
        /// this does not affect the position
        /// </summary>
        /// <param name="identifier"></param>
        public void SetFrameID(byte frameID) { this.SetContent(1, frameID); }

        public byte GetFrameID() { return this.GetFrameData()[1]; }
    }
}