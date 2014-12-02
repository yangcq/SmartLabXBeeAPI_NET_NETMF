using System;
using SmartLab.XBee.Options;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Request
{
    public class ZigBeeExplicitTxRequest : TxRequestBase
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

        public ZigBeeExplicitTxRequest(ExplicitDeviceAddress RemoteDevice, OptionsBase TransmitOptions, byte[] RFData)
            : this(0x00, RemoteDevice, TransmitOptions, RFData)
        { }

        public ZigBeeExplicitTxRequest(byte FrameID, ExplicitDeviceAddress RemoteDevice, OptionsBase TransmitOptions, byte[] RFData)
            : base(18 + RFData.Length, API_IDENTIFIER.Explicit_Addressing_ZigBee_Command_Frame, FrameID)
        {
            Array.Copy(RemoteDevice.GetAddressValue(), 0, FrameData, 2, 10);
            Array.Copy(RemoteDevice.GetExplicitValue(), 0, FrameData, 12, 6);
            this.FrameData[18] = 0x00;
            this.FrameData[19] = TransmitOptions.GetValue();
            Array.Copy(RFData, 0, this.FrameData, 20, RFData.Length);
        }

        public void SetBroadcastRadius(byte BroadcastRadius)
        {
            this.FrameData[18] = BroadcastRadius;
        }

        public override void SetTransmitOptions(OptionsBase TransmitOptions)
        {
            this.FrameData[19] = TransmitOptions.GetValue();
        }

        public override void SetPayload(byte[] data)
        {
            SetData(20, data);
        }
    }
}