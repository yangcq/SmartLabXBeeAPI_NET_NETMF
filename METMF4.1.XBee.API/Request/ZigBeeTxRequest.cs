using System;
using SmartLab.XBee.Options;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Request
{
    public class ZigBeeTxRequest : TxRequestBase
    {
        //0x10
        //FrameID;
        //RemoteDevice
        //Broadcast_Radius;
        //TransmitOptions;
        //RF_Data;

        public ZigBeeTxRequest(DeviceAddress RemoteDevice, OptionsBase TransmitOptions, byte[] RFData)
            : this(0x00, RemoteDevice, TransmitOptions, RFData) 
        { }

        public ZigBeeTxRequest(byte FrameID, DeviceAddress RemoteDevice, OptionsBase TransmitOptions, byte[] RFData)
            : base(12 + RFData.Length, API_IDENTIFIER.ZigBee_Transmit_Request, FrameID)
        {
            Array.Copy(RemoteDevice.GetAddressValue(), 0, FrameData, 2, 10);
            this.FrameData[12] = 0x00;
            this.FrameData[13] = TransmitOptions.GetValue();
            Array.Copy(RFData, 0, this.FrameData, 14, RFData.Length);
        }

        public void SetBroadcastRadius(byte BroadcastRadius)
        {
            this.FrameData[12] = BroadcastRadius;
        }

        public override void SetTransmitOptions(OptionsBase TransmitOptions)
        {
            this.FrameData[13] = TransmitOptions.GetValue();
        }

        public override void SetPayload(byte[] data)
        {
            SetData(14, data);
        }
    }
}