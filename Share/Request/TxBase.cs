using SmartLab.XBee.Core;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Request
{
    public abstract class TxBase : APIFrame
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Length">the length of payload not include Frame Type, Frame ID and CheckSum</param>
        /// <param name="identifier">Frame Type</param>
        /// <param name="frameID">Frame ID</param>
        public TxBase(int Length, API_IDENTIFIER identifier, byte frameID)
            : base(Length + 2)
        {
            this.SetFrameType(identifier);
            this.SetFrameID(frameID);
            this.SetPosition(2);
        }

        /// <summary>
        /// this does not affect the position, will always write to position 1
        /// </summary>
        /// <param name="identifier"></param>
        public void SetFrameID(byte frameID) { this.SetContent(1, frameID); }

        public byte GetFrameID() { return this.GetFrameData()[1]; }
    }
}