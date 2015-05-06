using System;
using SmartLab.XBee.Options;
using SmartLab.XBee.Type;
using SmartLab.XBee.Device;

namespace SmartLab.XBee.Request
{
    public class XBeeTx64Request : TxPayloadBase
    {
        //0x00
        //FrameID
        //Destination 64
        //Option
        //RFData

        /// <summary>
        /// the ieee 16 bit address is ignored
        /// </summary>
        /// <param name="frameID"></param>
        /// <param name="remoteAddress"></param>
        /// <param name="transmitOptions"></param>
        /// <param name="RFData"></param>
        public XBeeTx64Request(byte frameID, Address remoteAddress, OptionsBase transmitOptions, byte[] payload)
            : this(frameID, remoteAddress, transmitOptions, payload, 0, payload.Length) 
        { }

        /// <summary>
        /// the ieee 16 bit address is ignored
        /// </summary>
        /// <param name="frameID"></param>
        /// <param name="remoteAddress"></param>
        /// <param name="transmitOptions"></param>
        /// <param name="RFData"></param>
        public XBeeTx64Request(byte frameID, Address remoteAddress, OptionsBase transmitOptions, byte[] payload, int offset, int length)
            : base(9 + payload.Length, API_IDENTIFIER.Tx64_Request, frameID)
        {
            this.SetContent(remoteAddress.GetAddressValue(), 2, 8);
            this.SetContent(transmitOptions.GetValue());
            this.SetContent(payload, offset, length);
        }

        public override void SetPayload(byte[] data) { SetPayload(data, 0, data.Length); }

        public override void SetPayload(byte[] data, int offset, int length)
        {
            this.SetContent(11, data, offset, length);
            this.SetPosition(11 + length - offset);
        }

        public override void SetTransmitOptions(OptionsBase transmitOptions) { this.SetContent(10, transmitOptions.GetValue()); }

        /// <summary>
        /// the ieee 16 bit address is ignored
        /// </summary>
        /// <param name="networkAddress"></param>
        public override void SetRemoteAddress(Address remoteAddress) { this.SetContent(2, remoteAddress.GetAddressValue(), 2, 8); }
    }
}