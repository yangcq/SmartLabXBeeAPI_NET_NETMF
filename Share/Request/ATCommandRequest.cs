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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FrameID"></param>
        /// <param name="AT_Command"></param>
        /// <param name="Parameter_Value">this can be null</param>
        public ATCommandRequest(byte frameID, ATCommand command, byte[] parameter = null)
            : this(frameID, command, parameter, 0, parameter == null ? 0 : parameter.Length)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FrameID"></param>
        /// <param name="AT_Command"></param>
        /// <param name="Parameter_Value">this can be null</param>
        public ATCommandRequest(byte frameID, ATCommand command, byte[] parameter, int offset, int length)
            : base(2 + (parameter == null ? 0 : parameter.Length), API_IDENTIFIER.AT_Command, frameID)
        {
            this.SetContent(command.GetValue());

            if (parameter != null)
                this.SetContent(parameter, offset, length);
        }

        public override void SetAppleChanges(bool appleChanges)
        {
            if (appleChanges)
                SetFrameType(API_IDENTIFIER.AT_Command);
            else
                SetFrameType(API_IDENTIFIER.AT_Command_Queue_Parameter_Value);
        }

        public override void SetCommand(ATCommand command)
        {
            this.SetContent(2, command.GetValue());
        }

        public override void SetParameter(byte[] parameter) { this.SetParameter(parameter, 0 , parameter.Length); }

        public override void SetParameter(byte[] parameter, int offset, int length)
        {
            this.SetPosition(4);
            this.SetContent(parameter, offset, length);
        }
    }
}