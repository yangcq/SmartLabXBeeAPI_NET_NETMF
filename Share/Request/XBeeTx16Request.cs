using SmartLab.XBee.Device;
using SmartLab.XBee.Options;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Request
{
    public class XBeeTx16Request : TxBase, IPayloadRequest
    {
        /// <summary>
        /// the ieee 64 bit address is ignored
        /// </summary>
        /// <param name="frameID"></param>
        /// <param name="remoteAddress"></param>
        /// <param name="transmitOptions"></param>
        /// <param name="RFData"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        public XBeeTx16Request(byte frameID, Address remoteAddress, OptionsBase transmitOptions, byte[] payload)
            : this(frameID, remoteAddress, transmitOptions, payload, 0, payload.Length)
        { }

        /// <summary>
        /// the ieee 64 bit address is ignored
        /// </summary>
        /// <param name="frameID"></param>
        /// <param name="remoteAddress"></param>
        /// <param name="transmitOptions"></param>
        /// <param name="RFData"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        public XBeeTx16Request(byte frameID, Address remoteAddress, OptionsBase transmitOptions, byte[] payload, int offset, int length)
            : base(3 + payload.Length, API_IDENTIFIER.Tx16_Request, frameID)
        {
            this.SetContent((byte)(remoteAddress.GetNetworkAddress() >> 8));
            this.SetContent((byte)remoteAddress.GetNetworkAddress());
            this.SetContent(transmitOptions.GetValue());
            this.SetContent(payload, offset, length);
        }

        public void SetPayload(byte[] data) { SetPayload(data, 0, data.Length); }

        public void SetPayload(byte[] data, int offset, int length)
        {
            this.SetPosition(5);
            this.SetContent(data, offset, length);
        }

        public void SetTransmitOptions(OptionsBase transmitOptions)
        {
            this.SetContent(4, transmitOptions.GetValue());
        }
        
        /// <summary>
        /// the ieee 64 bit address is ignored
        /// </summary>
        /// <param name="networkAddress"></param>
        public void SetRemoteAddress(Address remoteAddress)
        {
            this.SetContent(2, (byte)(remoteAddress.GetNetworkAddress() >> 8));
            this.SetContent(3, (byte)(remoteAddress.GetNetworkAddress()));
        }
    }
}