using SmartLab.XBee.Device;
using SmartLab.XBee.Options;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Request
{
    public class ZigBeeExplicitTxRequest : TxBase, IPayloadRequest
    {
        //0x11
        //FrameID;
        //ExplicitRemoteDevice
        //Source Endpoint
        //Destination Endpoint
        //Cluster ID
        //Profile ID
        //Broadcast_Radius;
        //TransmitOptions;
        //RF_Data;

        public ZigBeeExplicitTxRequest(byte frameID, ExplicitAddress remoteAddress, OptionsBase transmitOptions, byte[] payload)
            : this(frameID, remoteAddress, transmitOptions, payload, 0, payload.Length)
        { }

        public ZigBeeExplicitTxRequest(byte frameID, ExplicitAddress remoteAddress, OptionsBase transmitOptions, byte[] payload, int offset, int length)
            : base(18 + payload.Length, API_IDENTIFIER.Explicit_Addressing_ZigBee_Command_Frame, frameID)
        {
            this.SetContent(remoteAddress.GetAddressValue());
            this.SetContent(remoteAddress.GetExplicitValue());
            this.SetContent(0x00);
            this.SetContent(transmitOptions.GetValue());
            this.SetContent(payload, offset, length);
        }

        public void SetBroadcastRadius(byte broadcastRadius)
        {
            this.SetContent(18, broadcastRadius);
        }

        public void SetTransmitOptions(OptionsBase transmitOptions)
        {
            this.SetContent(19, transmitOptions.GetValue());
        }

        public void SetPayload(byte[] data) { SetPayload(data, 0, data.Length); }

        public void SetPayload(byte[] data, int offset, int length)
        {
            this.SetPosition(20);
            this.SetContent(data, offset, length);
        }

        public void SetRemoteAddress(Address remoteAddress)
        {
            this.SetContent(2, remoteAddress.GetAddressValue());
        }

        public void SetRemoteExplicit(ExplicitAddress remoteAddress) 
        {
            this.SetContent(2, remoteAddress.GetAddressValue());
            this.SetContent(12, remoteAddress.GetExplicitValue());
        }
    }
}