using SmartLab.XBee.Device;
using SmartLab.XBee.Options;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Request
{
    public class ZigBeeTxRequest : TxBase, IPayloadRequest
    {
        //0x10
        //FrameID;
        //RemoteDevice
        //Broadcast_Radius;
        //TransmitOptions;
        //RF_Data;
        public ZigBeeTxRequest(byte frameID, Address remoteAddress, OptionsBase transmitOptions, byte[] payload)
            : this(frameID, remoteAddress, transmitOptions, payload, 0, payload.Length)
        { }

        public ZigBeeTxRequest(byte frameID, Address remoteAddress, OptionsBase transmitOptions, byte[] payload, int offset, int length)
            : base(12 + payload.Length, API_IDENTIFIER.ZigBee_Transmit_Request, frameID)
        {
            this.SetContent(remoteAddress.GetAddressValue());
            this.SetContent(0x00);
            this.SetContent(transmitOptions.GetValue());
            this.SetContent(payload, offset, length);
        }

        public void SetBroadcastRadius(byte broadcastRadius)
        {
            this.SetContent(12, broadcastRadius);
        }

        public void SetTransmitOptions(OptionsBase transmitOptions)
        {
            this.SetContent(13, transmitOptions.GetValue());
        }

        public void SetPayload(byte[] data) { SetPayload(data, 0, data.Length); }

        public void SetPayload(byte[] data, int offset, int length)
        {
            this.SetPosition(14);
            this.SetContent(data, offset, length);
        }

        public void SetRemoteAddress(Address remoteAddress)
        {
            this.SetContent(2, remoteAddress.GetAddressValue());
        }
    }
}