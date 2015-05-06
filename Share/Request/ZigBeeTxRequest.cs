using SmartLab.XBee.Device;
using SmartLab.XBee.Options;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Request
{
    public class ZigBeeTxRequest : TxPayloadBase
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

        public void SetBroadcastRadius(byte broadcastRadius) { this.SetContent(12, broadcastRadius); }

        public override void SetTransmitOptions(OptionsBase transmitOptions){ this.SetContent(13, transmitOptions.GetValue()); }

        public override void SetPayload(byte[] data) { SetPayload(data, 0, data.Length); }

        public override void SetPayload(byte[] data, int offset, int length)
        {
            this.SetContent(14, data, offset, length);
            this.SetPosition(14 + length - offset);
        }

        public override void SetRemoteAddress(Address remoteAddress) { this.SetContent(2, remoteAddress.GetAddressValue()); }
    }
}