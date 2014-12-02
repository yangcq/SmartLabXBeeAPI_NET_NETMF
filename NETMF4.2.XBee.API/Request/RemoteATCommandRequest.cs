using System;
using SmartLab.XBee.Options;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Request
{
    public class RemoteATCommandRequest : CommandRequestBase
    {
        //0x17
        //FrameID;
        //RemoteDevice
        //Remote Command Options
        //AT_Command
        //Parameter_Value

        public RemoteATCommandRequest(DeviceAddress RemoteDevice, OptionsBase TransmitOptions, ATCommand Command, byte[] Parameter)
            : this(0x00, RemoteDevice, TransmitOptions, Command, Parameter)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FrameID"></param>
        /// <param name="RemoteDevice"></param>
        /// <param name="options">RemoteCommandOptions</param>
        /// <param name="AT_Command"></param>
        /// <param name="Parameter_Value">this can be null</param>
        public RemoteATCommandRequest(byte FrameID, DeviceAddress RemoteDevice, OptionsBase TransmitOptions, ATCommand Command, byte[] Parameter)
            : base(13 + (Parameter == null ? 0 : Parameter.Length), API_IDENTIFIER.Remote_Command_Request, FrameID)
        {
            Array.Copy(RemoteDevice.GetAddressValue(), 0, this.FrameData, 2, 10);
            this.FrameData[12] = TransmitOptions.GetValue();
            this.FrameData[13] = Command.GetValue()[0];
            this.FrameData[14] = Command.GetValue()[1];

            if (Parameter != null)
                Array.Copy(Parameter, 0, this.FrameData, 15, Parameter.Length);
        }

        public void SetTransmitOptions(OptionsBase TransmitOptions)
        {
            this.FrameData[12] = TransmitOptions.GetValue();
        }

        public override void SetCommand(ATCommand Command)
        {
            if (Command == null)
                throw new ArgumentNullException();
            this.FrameData[13] = Command.GetValue()[0];
            this.FrameData[14] = Command.GetValue()[1];
        }

        public override void SetAppleChanges(bool AppleChanges)
        {
            if (AppleChanges)
                FrameData[12] |= 0x02;
            else FrameData[12] &= 0xFD;
        }

        public override void SetParameter(byte[] Parameter)
        {
            SetData(15, Parameter);
        }
    }
}