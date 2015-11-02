using SmartLab.XBee.Device;
using SmartLab.XBee.Options;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Request
{
    public class XBeeTx64Request : TxBase, IPayloadRequest
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
            this.SetContent(remoteAddress.GetAddressValue(), 0, 8);
            this.SetContent(transmitOptions.GetValue());
            this.SetContent(payload, offset, length);
        }

        public void SetPayload(byte[] data) { SetPayload(data, 0, data.Length); }

        public void SetPayload(byte[] data, int offset, int length)
        {
            this.SetPosition(11);
            this.SetContent(data, offset, length);
        }

        public void SetTransmitOptions(OptionsBase transmitOptions)
        {
            this.SetContent(10, transmitOptions.GetValue());
        }

        /// <summary>
        /// the ieee 16 bit address is ignored
        /// </summary>
        /// <param name="networkAddress"></param>
        public void SetRemoteAddress(Address remoteAddress)
        {
            this.SetContent(2, remoteAddress.GetAddressValue(), 0, 8);
        }
    }
}