using System;
using SmartLab.XBee.Options;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Request
{
    public class XBeeTx64Request : TxRequestBase
    {
        //0x00
        //FrameID
        //Destination 64
        //Option
        //RFData

        public XBeeTx64Request(DeviceAddress RemoteDevice, OptionsBase TransmitOptions, byte[] RFData)
            : this(0x00, RemoteDevice, TransmitOptions, RFData)
        { }

        public XBeeTx64Request(byte FrameID, DeviceAddress RemoteDevice, OptionsBase TransmitOptions, byte[] RFData)
            : base(9 + RFData.Length, API_IDENTIFIER.Tx64_Request, FrameID)
        {
            Array.Copy(RemoteDevice.GetAddressValue(), 0, this.FrameData, 2, 8);
            this.FrameData[10] = TransmitOptions.GetValue();
            Array.Copy(RFData, 0, this.FrameData, 11, RFData.Length);
        }

        public override void SetPayload(byte[] data)
        {
            SetData(11, data);
        }

        public override void SetTransmitOptions(OptionsBase TransmitOptions)
        {
            this.FrameData[10] = TransmitOptions.GetValue();
        }
    }
}