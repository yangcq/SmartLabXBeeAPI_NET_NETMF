using SmartLab.XBee.Response;

namespace SmartLab.XBee.Type
{
    public class DeviceAddress
    {
        public static DeviceAddress BROADCAST = new DeviceAddress(0x00000000, 0x0000FFFF, 0xFFFE);

        // total 10 bytes
        // IEEE 64 + 16bit networ address
        protected byte[] value;

        public DeviceAddress() 
        {
            value = new byte[10]; 
        }

        public DeviceAddress(byte[] Address64, byte[] NET16)
        {
            this.value = Address64.CombineArray(NET16);
        }

        public DeviceAddress(byte[] value) 
        {
            this.value = value;
        }

        public DeviceAddress(int SerialNumberHigh, int SerialNumberLow, int NetworkAddress)
        {
            value = new byte[10];

            value[0] = (byte)(SerialNumberHigh >> 24);
            value[1] = (byte)(SerialNumberHigh >> 16);
            value[2] = (byte)(SerialNumberHigh >> 8);
            value[3] = (byte)SerialNumberHigh;

            value[4] = (byte)(SerialNumberLow >> 24);
            value[5] = (byte)(SerialNumberLow >> 16);
            value[6] = (byte)(SerialNumberLow >> 8);
            value[7] = (byte)SerialNumberLow;

            value[8] = (byte)(NetworkAddress >> 8);
            value[9] = (byte)NetworkAddress;
        }

        public int GetSerialNumberHigh()
        {
            return (value[0] << 24) | (value[1] << 16) | (value[2] << 8) | value[3];
        }

        public int GetSerialNumberLow()
        {
            return (value[4] << 24) | (value[5] << 16) | (value[5] << 8) | value[7];
        }

        public int GetNetworkAddress()
        {
            return (value[8] << 8) | value[9];
        }

        public byte[] GetAddressValue()
        {
            return value;
        }

        /// <summary>
        /// extension method for convert DN / ND (with or without NI String) response to address 
        /// </summary>
        /// <param name="response">muset be non null parameter</param>
        /// <returns></returns>
        public static DeviceAddress Parse(ICommandResponse response)
        {
            byte[] message = response.GetParameter();
            if (message != null)
                if (response.GetRequestCommand().ToString().ToUpper() == "ND")
                {
                    DeviceAddress device = new DeviceAddress();

                    device.value[0] = message[2];
                    device.value[1] = message[3];
                    device.value[2] = message[4];
                    device.value[3] = message[5];
                    device.value[4] = message[6];
                    device.value[5] = message[7];
                    device.value[6] = message[8];
                    device.value[7] = message[9];

                    device.value[8] = message[0];
                    device.value[9] = message[1];

                    return device;
                }
            return null;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(DeviceAddress))
                return false;

            for (int i = 0; i < 10; i++)
                if (this.value[i] != ((DeviceAddress)obj).value[i])
                    return false;

            return true;
        }
    }
}