using System;
using System.Text;

namespace SmartLab.XBee.Type
{
    public class ATCommand
    {
        protected byte[] value;

        public ATCommand() { }

        public ATCommand(byte[] commad)
        {
            if (commad == null) 
                throw new ArgumentNullException();

            if (commad.Length != 2)
                throw new ArgumentException("AT Command must be 2 bytes");

            this.value = commad;
        }

        public ATCommand(string commad)
        {
            if (commad == null)
                throw new ArgumentNullException();

            if (commad.Length != 2) 
                throw new ArgumentException("AT Command must be 2 bytes");

            this.value = UTF8Encoding.UTF8.GetBytes(commad);
        }

        public byte[] GetValue()
        {
            return this.value;
        }

        public override string ToString()
        {
            return new string(UTF8Encoding.UTF8.GetChars(this.value));
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(ATCommand))
                if (this.value[0] == ((ATCommand)obj).value[0])
                    if (this.value[1] == ((ATCommand)obj).value[1])
                        return true;
            return false;
        }
    }

    public static class ATCommands
    {
        public static readonly ATCommand IO_Sampling_Rate = new ATCommand(new byte[] { 0x49, 0x52 });

        public static readonly ATCommand Digital_IO_Change_Detection = new ATCommand(new byte[] { 0x49, 0x43 });

        public static readonly ATCommand Instant_Sample = new ATCommand(new byte[] { 0x49, 0x53 });

        public static readonly ATCommand Node_Descovery = new ATCommand(new byte[] { 0x4E, 0x44 });

        public static readonly ATCommand Destination_Node_Descovery = new ATCommand(new byte[] { 0x44, 0x4E });

        public static readonly ATCommand Node_Identifier = new ATCommand(new byte[] { 0x4E, 0x49 });

        public static readonly ATCommand Node_Join_Time = new ATCommand(new byte[] { 0x4E, 0x4A });

        public static readonly ATCommand Network_Address = new ATCommand(new byte[] { 0x4D, 0x59 });

        public static readonly ATCommand PAN_ID = new ATCommand(new byte[] { 0x49, 0x44 });

        public static readonly ATCommand Destination_Address_High = new ATCommand(new byte[] { 0x44, 0x48 });

        public static readonly ATCommand Destination_Address_Low = new ATCommand(new byte[] { 0x44, 0x4C });

        public static readonly ATCommand Serial_Number_High = new ATCommand(new byte[] { 0x53, 0x48 });

        public static readonly ATCommand Serial_Number_Low = new ATCommand(new byte[] { 0x53, 0x4C });
    }
}
