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

        public RemoteATCommandRequest(byte frameID, DeviceAddress remoteAddress, ATCommand command, OptionsBase transmitOptions, byte[] parameter = null)
            : this(frameID, remoteAddress, command, transmitOptions, parameter, 0, parameter == null ? 0 : parameter.Length)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FrameID"></param>
        /// <param name="RemoteDevice"></param>
        /// <param name="options">RemoteCommandOptions</param>
        /// <param name="AT_Command"></param>
        /// <param name="Parameter_Value">this can be null</param>
        public RemoteATCommandRequest(byte frameID, DeviceAddress remoteAddress, ATCommand command, OptionsBase transmitOptions, byte[] parameter, int parameterOffset, int parameterLength)
            : base(13 + (parameter == null ? 0 : parameter.Length), API_IDENTIFIER.Remote_Command_Request, frameID)
        {
            this.SetContent(remoteAddress.GetAddressValue());
            this.SetContent(transmitOptions.GetValue());
            this.SetContent(command.GetValue());

            if (parameter != null)
                this.SetContent(parameter, parameterOffset, parameterLength);
        }

        public void SetTransmitOptions(OptionsBase TransmitOptions) { this.SetContent(12, TransmitOptions.GetValue()); }

        public override void SetAppleChanges(bool appleChanges)
        {
            if (appleChanges)
                this.GetFrameData()[12] |= 0x02;
            else this.GetFrameData()[12] &= 0xFD;
        }

        public override void SetCommand(ATCommand command) { this.SetContent(13, command.GetValue()); }

        public override void SetParameter(byte[] parameter) { this.SetContent(15, parameter, 0, parameter.Length); }

        public override void SetParameter(byte[] parameter, int offset, int length)
        {
            this.SetContent(15, parameter, offset, length);
            this.SetPosition(15 + length - offset);
        }

        public void SetRemoteAddress(DeviceAddress remoteAddress) { this.SetContent(2, remoteAddress.GetAddressValue()); }
    }
}