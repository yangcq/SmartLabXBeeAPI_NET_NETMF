namespace SmartLab.XBee.Type
{
    public static class Device
    {
        public class Pin
        {
            public class XBee
            {
                public static readonly Pin VCC = new Pin(1);
                public static readonly Pin DOUT = new Pin(2);
                public static readonly Pin DIN_CONFIG = new Pin(3);
                public static readonly Pin DO8 = new Pin(4, new byte[2] { 0x44, 0x38 }, new byte[] { 0x01, 0x00 });
                public static readonly Pin RESET = new Pin(5);
                public static readonly Pin RSSI_PWM0 = new Pin(6);
                public static readonly Pin PWM1 = new Pin(7);
                public static readonly Pin RESERVED = new Pin(8);
                public static readonly Pin DTR_SLEEP_DIO8 = new Pin(9);
                public static readonly Pin GND = new Pin(10);
                public static readonly Pin AD4_DIO4 = new Pin(11, new byte[2] { 0x44, 0x34 }, new byte[] { 0x00, 0x10 });
                public static readonly Pin CTS_DIO7 = new Pin(12, new byte[2] { 0x44, 0x37 }, new byte[] { 0x00, 0x80 });
                public static readonly Pin ON_SLEEP = new Pin(13);
                public static readonly Pin VREF = new Pin(14);
                public static readonly Pin ASSOCIATE_AD5_DIO5 = new Pin(15, new byte[2] { 0x44, 0x35 }, new byte[] { 0x00, 0x20 });
                public static readonly Pin RTS_AD6_DIO6 = new Pin(16, new byte[2] { 0x44, 0x36 }, new byte[] { 0x00, 0x40 });
                public static readonly Pin AD3_DIO3 = new Pin(17, new byte[2] { 0x44, 0x33 }, new byte[] { 0x00, 0x08 });
                public static readonly Pin AD2_DIO2 = new Pin(18, new byte[2] { 0x44, 0x32 }, new byte[] { 0x00, 0x04 });
                public static readonly Pin AD1_DIO1 = new Pin(19, new byte[2] { 0x44, 0x31 }, new byte[] { 0x00, 0x02 });
                public static readonly Pin AD0_DIO0 = new Pin(20, new byte[2] { 0x44, 0x30 }, new byte[] { 0x00, 0x01 });
            }

            public class ZigBee
            {
                public static readonly Pin VCC = new Pin(1);
                public static readonly Pin DOUT = new Pin(2);
                public static readonly Pin DIN_CONFIG = new Pin(3);
                public static readonly Pin DIO12 = new Pin(4, new byte[2] { 0x50, 0x32 }, new byte[] { 0x10, 0x00 });
                public static readonly Pin RESET = new Pin(5);
                public static readonly Pin RSSI_PWM_DIO10 = new Pin(6, new byte[2] { 0x50, 0x30 }, new byte[] { 0x04, 0x00 });
                public static readonly Pin PWM_DIO11 = new Pin(7, new byte[2] { 0x50, 0x31 }, new byte[] { 0x08, 0x00 });
                public static readonly Pin RESERVED = new Pin(8);
                public static readonly Pin DTR_SLEEP_DIO8 = new Pin(9);
                public static readonly Pin GND = new Pin(10);
                public static readonly Pin DIO4 = new Pin(11, new byte[2] { 0x44, 0x34 }, new byte[] { 0x00, 0x10 });
                public static readonly Pin CTS_DIO7 = new Pin(12, new byte[2] { 0x44, 0x37 }, new byte[] { 0x00, 0x80 });
                public static readonly Pin ON_SLEEP = new Pin(13);
                public static readonly Pin VREF = new Pin(14);
                public static readonly Pin ASSOCIATE_DIO5 = new Pin(15, new byte[2] { 0x44, 0x35 }, new byte[] { 0x00, 0x20 });
                public static readonly Pin RTS_DIO6 = new Pin(16, new byte[2] { 0x44, 0x36 }, new byte[] { 0x00, 0x40 });
                public static readonly Pin AD3_DIO3 = new Pin(17, new byte[2] { 0x44, 0x33 }, new byte[] { 0x00, 0x08 });
                public static readonly Pin AD2_DIO2 = new Pin(18, new byte[2] { 0x44, 0x32 }, new byte[] { 0x00, 0x04 });
                public static readonly Pin AD1_DIO1 = new Pin(19, new byte[2] { 0x44, 0x31 }, new byte[] { 0x00, 0x02 });
                public static readonly Pin AD0_DIO0_COMMISSIONONG_BUTTON = new Pin(20, new byte[2] { 0x44, 0x30 }, new byte[] { 0x00, 0x01 });
            }

            private byte pinNum;
            private byte[] pinCom;
            private byte[] pinDet;

            public Pin(byte pinNum)
            {
                this.pinNum = pinNum;
            }

            public Pin(byte pinNum, byte[] pinCom, byte[] pinDet)
            {
                this.pinNum = pinNum;
                this.pinCom = pinCom;
                this.pinDet = pinDet;
            }

            public byte NUM
            {
                get { return this.pinNum; }
            }

            public byte[] COMMAND
            {
                get { return this.pinCom; }
            }

            public byte[] IO_DETECTION
            {
                get { return this.pinDet; }
            }

            public enum Functions
            {
                DISABLED = 0x00,
                RESERVED_FOR_PIN_SPECIFIC_ALTERNATE_FUNCTIONALITIES = 0x01,
                ANALOG_INPUT_SINGLE_ENABLED = 0x02,
                DIGITAL_INPUT_MONITORED = 0x03,
                DIGITAL_OUTPUT_DEFAULT_LOW = 0x04,
                DIGITAL_OUTPUT_DEFAULT_HIGH = 0x05,
                //0x06~0x09
                ALTERNATE_FUNCTIONALITIES_WHERE_APPLICABLE = 0x06,
            }

            public enum Status
            {
                LOW,
                HIGH,
                UNMONITORED,
            }

            public static byte[] IOChangeDetectionConfiguration(Pin[] Pins)
            {
                int tempmsb = 0;
                int templsb = 0;
                foreach (Pin pin in Pins)
                {
                    tempmsb |= pin.pinDet[0];
                    templsb |= pin.pinDet[1];
                }
                return new byte[2] { (byte)tempmsb, (byte)templsb };
            }
        }

        public enum Type
        {
            COORDINATOR = 0x00,
            ROUTER = 0x01,
            END_DEVICE = 0x02,
        }

        public enum SourceEvent
        {
            FRAME_SENT_BY_NODE_IDENTIFICATION_PUSHBUTTON_EVENT = 0x01,
            FRAME_SENT_AFTER_JOINING_EVENT_OCCURRED = 0x02,
            FRAME_SENT_AFTER_POWER_CYCLE_EVENT_OCCURRED = 0x03,
        }

        public enum OneWireSensors
        {
            AD_SENSOR_READ = 0x01,
            TEMPERATURE_SENSOR_READ = 0x02,
            HUMIDITY_SENSOR_READ = 0x03,
            WATER_PRESENT = 0x60,
        }
    }
}