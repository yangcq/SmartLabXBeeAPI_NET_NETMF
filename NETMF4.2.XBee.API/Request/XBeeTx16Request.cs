using System;
using SmartLab.XBee.Options;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Request
{
    public class XBeeTx16Request : TxRequestBase
    {
        public XBeeTx16Request(int NetworkAddress, OptionsBase TransmitOptions, byte[] RFData)
            : this(0x00, NetworkAddress, TransmitOptions, RFData)
        { }

        public XBeeTx16Request(byte FrameID, int NetworkAddress, OptionsBase TransmitOptions, byte[] RFData)
            : base(3 + RFData.Length, API_IDENTIFIER.Tx16_Request, FrameID)
        {
            this.FrameData[2] = (byte)(NetworkAddress >> 8);
            this.FrameData[3] = (byte)NetworkAddress;
            this.FrameData[4] = TransmitOptions.GetValue();
            Array.Copy(RFData, 0, this.FrameData, 5, RFData.Length);
        }

        public override void SetPayload(byte[] data)
        {
            SetData(5, data);
        }

        public override void SetTransmitOptions(OptionsBase TransmitOptions)
        {
            this.FrameData[4] = TransmitOptions.GetValue();
        }
    }
}