namespace SmartLab.XBee.Device
{
    public class Pin
    {
        public class XBee
        {
            /// <summary>
            /// Power supply.
            /// Poor power supply can lead to poor radio performance, especially if the supply voltage is not kept within tolerance or is excessively noisy. To help reduce noise, we recommend placing a 1.0 μF and 8.2 pF capacitor as near as possible to pin 1 on the XBee. If using a switching regulator for the power supply, switching frequencies above 500 kHz are preferred. Power supply ripple should be limited to a maximum 100 mV peak to peak.
            /// </summary>
            public static readonly Pin P1_VCC = new Pin(1);
            
            /// <summary>
            /// DOUT, Output, UART data out.
            /// </summary>
            public static readonly Pin P2_DOUT = new Pin(2);
            
            /// <summary>
            /// DIN / CONFIG(active low), Input, UART data In.
            /// </summary>
            public static readonly Pin P3_DIN_CONFIG = new Pin(3);

            /// <summary>
            /// DO8, Either, Digital output 8, Function is not supported at the time of this release(18/05/2015).
            /// </summary>
            public static readonly Pin P4_DO8 = new Pin(4, new byte[2] { 0x44, 0x38 }, new byte[] { 0x01, 0x00 });

            /// <summary>
            /// RESET(active low), Input, Module reset (reset pulse must be at least 200 ns).
            /// </summary>
            public static readonly Pin P5_RESET = new Pin(5);

            /// <summary>
            /// PWM0 / RSSI, Either, PWM output 0 / RX signal strength indicator.
            /// </summary>
            public static readonly Pin P6_RSSI_PWM0 = new Pin(6);

            /// <summary>
            /// PWM1, Either, PWM output 1.
            /// </summary>
            public static readonly Pin P7_PWM1 = new Pin(7);
            
            /// <summary>
            /// Reserved, Do not connect.
            /// </summary>
            public static readonly Pin P8_RESERVED = new Pin(8);

            /// <summary>
            /// DTR(active low) / SLEEP_RQ/ DI8, Either, Pin sleep control line or digital input 8.
            /// </summary>
            public static readonly Pin P9_DTR_SLEEP_DIO8 = new Pin(9);

            /// <summary>
            /// Ground.
            /// </summary>
            public static readonly Pin P10_GND = new Pin(10);

            /// <summary>
            /// AD4 / DIO4, Either, Analog input 4 or digital I/O 4.
            /// </summary>
            public static readonly Pin P11_AD4_DIO4 = new Pin(11, new byte[2] { 0x44, 0x34 }, new byte[] { 0x00, 0x10 });

            /// <summary>
            /// CTS(active low) / DIO7 Either Clear-to-send flow control or digital I/O 7.
            /// </summary>
            public static readonly Pin P12_CTS_DIO7 = new Pin(12, new byte[2] { 0x44, 0x37 }, new byte[] { 0x00, 0x80 });

            /// <summary>
            /// ON / SLEEP(active low), Output, Module status indicator
            /// </summary>
            public static readonly Pin P13_ON_SLEEP = new Pin(13);

            /// <summary>
            /// VREF, Input, Voltage reference for A/D inputs.
            /// </summary>
            public static readonly Pin P14_VREF = new Pin(14);

            /// <summary>
            /// Associate / AD5 / DIO5, Either, Associated indicator, analog input 5 or digital I/O 5.
            /// </summary>
            public static readonly Pin P15_ASSOCIATE_AD5_DIO5 = new Pin(15, new byte[2] { 0x44, 0x35 }, new byte[] { 0x00, 0x20 });

            /// <summary>
            /// RTS(active low) / DIO6, Either, Request-to-send flow control, or digital I/O 6.
            /// </summary>
            public static readonly Pin P16_RTS_AD6_DIO6 = new Pin(16, new byte[2] { 0x44, 0x36 }, new byte[] { 0x00, 0x40 });

            /// <summary>
            /// AD3 / DIO3 Either Analog input 3 or digital I/O 3.
            /// </summary>
            public static readonly Pin P17_AD3_DIO3 = new Pin(17, new byte[2] { 0x44, 0x33 }, new byte[] { 0x00, 0x08 });

            /// <summary>
            /// AD2 / DIO2 Either Analog input 2 or digital I/O 2.
            /// </summary>
            public static readonly Pin P18_AD2_DIO2 = new Pin(18, new byte[2] { 0x44, 0x32 }, new byte[] { 0x00, 0x04 });

            /// <summary>
            /// AD1 / DIO1 Either Analog input 1 or digital I/O 1.
            /// </summary>
            public static readonly Pin P19_AD1_DIO1 = new Pin(19, new byte[2] { 0x44, 0x31 }, new byte[] { 0x00, 0x02 });

            /// <summary>
            /// AD0 / DIO0 Either Analog input 0, digital I/O 0.
            /// </summary>
            public static readonly Pin P20_AD0_DIO0 = new Pin(20, new byte[2] { 0x44, 0x30 }, new byte[] { 0x00, 0x01 });

            /// <summary>
            /// Pin number from 1 - 20.
            /// </summary>
            /// <param name="pinNumber"></param>
            /// <returns></returns>
            public static Pin GetPinFromNumber(int pinNumber) 
            {
                switch (pinNumber)
                {
                    case 1: return P1_VCC;
                    case 2: return P2_DOUT;
                    case 3: return P3_DIN_CONFIG;
                    case 4: return P4_DO8;
                    case 5: return P5_RESET;
                    case 6: return P6_RSSI_PWM0;
                    case 7: return P7_PWM1;
                    case 8: return P8_RESERVED;
                    case 9: return P9_DTR_SLEEP_DIO8;
                    case 10: return P10_GND;
                    case 11: return P11_AD4_DIO4;
                    case 12: return P12_CTS_DIO7;
                    case 13: return P13_ON_SLEEP;
                    case 14: return P14_VREF;
                    case 15: return P15_ASSOCIATE_AD5_DIO5;
                    case 16: return P16_RTS_AD6_DIO6;
                    case 17: return P17_AD3_DIO3;
                    case 18: return P18_AD2_DIO2;
                    case 19: return P19_AD1_DIO1;
                    case 20: return P20_AD0_DIO0;
                    default: return null;
                }
            }
        }

        public class ZigBee
        {
            /// <summary>
            /// VCC - - Power supply.
            /// Poor power supply can lead to poor radio performance especially if the supply voltage is not kept within tolerance or is excessively noisy. To help reduce noise a 1uF and 8.2pF capacitor are recommended to be placed as near to pin1 on the PCB as possible. If using a switching regulator for your power supply, switching frequencies above 500 kHz are preferred. Power supply ripple should be limited to a maximum 50 mV peak to peak. 
            /// </summary>
            public static readonly Pin P1_VCC = new Pin(1);

            /// <summary>
            /// DOUT Output Output UART Data Out.
            /// </summary>
            public static readonly Pin P2_DOUT = new Pin(2);

            /// <summary>
            /// DIN / CONFIG(active low) Input Input UART Data In
            /// </summary>
            public static readonly Pin P3_DIN_CONFIG = new Pin(3);

            /// <summary>
            /// DIO12 Both Disabled Digital I/O 12.
            /// </summary>
            public static readonly Pin P4_DIO12 = new Pin(4, new byte[2] { 0x50, 0x32 }, new byte[] { 0x10, 0x00 });

            /// <summary>
            /// RESET Both Open-Collector with pull-up
            /// Module Reset (reset pulse must be at least 200 ns).
            /// </summary>
            public static readonly Pin P5_RESET = new Pin(5);

            /// <summary>
            /// RSSI PWM / DIO10 Both Output RX Signal Strength Indicator / Digital IO.
            /// </summary>
            public static readonly Pin P6_RSSI_PWM_DIO10 = new Pin(6, new byte[2] { 0x50, 0x30 }, new byte[] { 0x04, 0x00 });

            /// <summary>
            /// DIO11 Both Input Digital I/O 11.
            /// </summary>
            public static readonly Pin P7_PWM_DIO11 = new Pin(7, new byte[2] { 0x50, 0x31 }, new byte[] { 0x08, 0x00 });

            /// <summary>
            /// [reserved] - Disabled Do not connect.
            /// </summary>
            public static readonly Pin P8_RESERVED = new Pin(8);

            /// <summary>
            /// DTR(avtive low) / SLEEP_RQ/ DIO8 Both Input Pin Sleep Control Line or Digital IO 8.
            /// </summary>
            public static readonly Pin P9_DTR_SLEEP_DIO8 = new Pin(9);

            /// <summary>
            /// GND - - Ground.
            /// </summary>
            public static readonly Pin P10_GND = new Pin(10);

            /// <summary>
            /// DIO4 Both Disabled Digital I/O 4.
            /// </summary>
            public static readonly Pin P11_DIO4 = new Pin(11, new byte[2] { 0x44, 0x34 }, new byte[] { 0x00, 0x10 });

            /// <summary>
            /// CTS(active low) / DIO7 Both Output Clear-to-Send Flow Control or Digital I/O 7. CTS, if enabled, is an output.
            /// </summary>
            public static readonly Pin P12_CTS_DIO7 = new Pin(12, new byte[2] { 0x44, 0x37 }, new byte[] { 0x00, 0x80 });

            /// <summary>
            /// ON / SLEEP(active low) Output Output Module Status Indicator or Digital I/O 9.
            /// </summary>
            public static readonly Pin P13_ON_SLEEP = new Pin(13);

            /// <summary>
            /// VREF Input - Not used for EM250. Used for programmable secondary processor. For compatibility with other XBee modules, we recommend connecting this pin voltage reference if Analog sampling is desired. Otherwise, connect to GND.
            /// </summary>
            public static readonly Pin P14_VREF = new Pin(14);

            /// <summary>
            /// Associate / DIO5 Both Output Associated Indicator, Digital I/O 5.
            /// </summary>
            public static readonly Pin P15_ASSOCIATE_DIO5 = new Pin(15, new byte[2] { 0x44, 0x35 }, new byte[] { 0x00, 0x20 });

            /// <summary>
            /// RTS(active low) / DIO6 Both Input Request-to-Send Flow Control, Digital I/O 6. RTS, if enabled, is an input.
            /// </summary>
            public static readonly Pin P16_RTS_DIO6 = new Pin(16, new byte[2] { 0x44, 0x36 }, new byte[] { 0x00, 0x40 });

            /// <summary>
            /// AD3 / DIO3 Both Disabled Analog Input 3 or Digital I/O 3.
            /// </summary>
            public static readonly Pin P17_AD3_DIO3 = new Pin(17, new byte[2] { 0x44, 0x33 }, new byte[] { 0x00, 0x08 });

            /// <summary>
            /// AD2 / DIO2 Both Disabled Analog Input 2 or Digital I/O 2.
            /// </summary>
            public static readonly Pin P18_AD2_DIO2 = new Pin(18, new byte[2] { 0x44, 0x32 }, new byte[] { 0x00, 0x04 });

            /// <summary>
            /// AD1 / DIO1 Both Disabled Analog Input 1 or Digital I/O 1.
            /// </summary>
            public static readonly Pin P19_AD1_DIO1 = new Pin(19, new byte[2] { 0x44, 0x31 }, new byte[] { 0x00, 0x02 });

            /// <summary>
            /// AD0 / DIO0 / Commissioning Button Both Disabled Analog Input 0, Digital IO 0, or Commissioning Button.
            /// </summary>
            public static readonly Pin P20_AD0_DIO0_COMMISSIONONG_BUTTON = new Pin(20, new byte[2] { 0x44, 0x30 }, new byte[] { 0x00, 0x01 });
            
            public static Pin GetPinFromNumber(int pinNumber)
            {
                switch (pinNumber)
                {
                    case 1: return P1_VCC;
                    case 2: return P2_DOUT;
                    case 3: return P3_DIN_CONFIG;
                    case 4: return P4_DIO12;
                    case 5: return P5_RESET;
                    case 6: return P6_RSSI_PWM_DIO10;
                    case 7: return P7_PWM_DIO11;
                    case 8: return P8_RESERVED;
                    case 9: return P9_DTR_SLEEP_DIO8;
                    case 10: return P10_GND;
                    case 11: return P11_DIO4;
                    case 12: return P12_CTS_DIO7;
                    case 13: return P13_ON_SLEEP;
                    case 14: return P14_VREF;
                    case 15: return P15_ASSOCIATE_DIO5;
                    case 16: return P16_RTS_DIO6;
                    case 17: return P17_AD3_DIO3;
                    case 18: return P18_AD2_DIO2;
                    case 19: return P19_AD1_DIO1;
                    case 20: return P20_AD0_DIO0_COMMISSIONONG_BUTTON;
                    default: return null;
                }
            }
        }

        private byte pinNum;
        private byte[] pinCom;
        private byte[] pinDet;

        protected Pin(byte pinNum)
        {
            this.pinNum = pinNum;
        }

        protected Pin(byte pinNum, byte[] pinCom, byte[] pinDet)
        {
            this.pinNum = pinNum;
            this.pinCom = pinCom;
            this.pinDet = pinDet;
        }

        public byte NUM
        {
            get { return this.pinNum; }
        }

        /// <summary>
        /// if no such command null will return
        /// </summary>
        public byte[] COMMAND
        {
            get { return this.pinCom; }
        }

        /// <summary>
        /// if no such io detection command null will return
        /// </summary>
        public byte[] IO_DETECTION
        {
            get { return this.pinDet; }
        }

        public enum Functions
        {
            DISABLED = 0x00,
            /// <summary>
            /// ZigBee Pin 20 - Commisioning Button
            /// ZugBee Pin 6 - RSSI PWM Output
            /// </summary>
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
}
