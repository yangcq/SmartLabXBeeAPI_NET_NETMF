using SmartLab.XBee.Device;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Request
{
    public class CreateSourceRouteRequest : TxBase
    {
        //0x21
        //FrameID
        //RemoteDevice (64 + 16)
        //0x00
        //Number of Address
        //Address List

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FrameID"></param>
        /// <param name="AT_Command"></param>
        /// <param name="Parameter_Value">this can be null</param>
        public CreateSourceRouteRequest(byte frameID, Address remoteAddress, int[] addresses)
            : base(12 + (addresses.Length << 2), API_IDENTIFIER.Create_Source_Route, frameID)
        {
            this.SetContent(remoteAddress.GetAddressValue());
            this.SetContent(0x00);
            this.SetAddresses(addresses);
        }

        public void SetRemoteAddress(Address remoteAddress)
        {
            this.SetContent(2, remoteAddress.GetAddressValue());
        }

        public void SetAddresses(int[] addresses)
        {
            this.SetPosition(13);
            this.SetContent((byte)addresses.Length);
            foreach (int value in addresses)
            {
                this.SetContent((byte)(value >> 8));
                this.SetContent((byte)value);
            }
        }
    }
}