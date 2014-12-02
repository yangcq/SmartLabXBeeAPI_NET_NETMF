using System;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Request
{
    public class ATCommandRequest : CommandRequestBase
    {
        //0x08 or 0x09
        //FrameID
        //AT_Command
        //Parameter_Value
        
        public ATCommandRequest(ATCommand Command, byte[] Parameter)
            : this(0x00, Command, Parameter)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FrameID"></param>
        /// <param name="AT_Command"></param>
        /// <param name="Parameter_Value">this can be null</param>
        public ATCommandRequest(byte FrameID, ATCommand Command, byte[] Parameter)
            : base(2 + (Parameter == null ? 0 : Parameter.Length), API_IDENTIFIER.AT_Command, FrameID)
        {
            this.FrameData[2] = Command.GetValue()[0];
            this.FrameData[3] = Command.GetValue()[1];

            if (Parameter != null)
                Array.Copy(Parameter, 0, this.FrameData, 4, Parameter.Length);
        }

        public override void SetAppleChanges(bool AppleChanges)
        {
            if (AppleChanges)
                SetFrameType(API_IDENTIFIER.AT_Command);
            else 
                SetFrameType(API_IDENTIFIER.AT_Command_Queue_Parameter_Value);
        }

        public override void SetCommand(ATCommand Command)
        {
            if (Command == null) 
                throw new ArgumentNullException();

            this.FrameData[2] = Command.GetValue()[0];
            this.FrameData[3] = Command.GetValue()[1];
        }

        public override void SetParameter(byte[] Parameter)
        {
            SetData(4, Parameter);
        }
    }
}