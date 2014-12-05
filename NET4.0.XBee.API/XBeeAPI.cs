using System.IO.Ports;
using System.Threading;
using SmartLab.XBee.Request;
using SmartLab.XBee.Response;
using SmartLab.XBee.Type;

namespace SmartLab.XBee
{
    #region Delegate
    public delegate void ChecksumErrorHandler(APIFrame e);
    public delegate void UndefinedPacketHandler(APIFrame e);

    public delegate void ATCommandResponseHandler(ATCommandResponse e);
    public delegate void ModemStatusResponseHandler(ModemStatusResponse e);
    public delegate void NodeIdentificationResponseHandler(NodeIdentificationResponse e);
    public delegate void RemoteCommandResponseHandler(RemoteCommandResponse e);
    public delegate void XBeeIODataSampleRx16ResponseHandler(XBeeIODataSampleRx16Response e);
    public delegate void XBeeIODataSampleRx64ResponseHandler(XBeeIODataSampleRx64Response e);
    public delegate void XBeeRx16IndicatorHandler(XBeeRx16Response e);
    public delegate void XBeeRx64IndicatorHandler(XBeeRx64Response e);
    public delegate void XBeeSensorReadResponseHandler(SensorReadResponse e);
    public delegate void XBeeTransmitStatusResponseHandler(XBeeTxStatusResponse e);
    public delegate void ZigBeeExplicitRxResponseHandler(ZigBeeExplicitRxResponse e);
    public delegate void ZigBeeIODataSampleRXResponseHandler(ZigBeeIODataSampleRxResponse e);
    public delegate void ZigBeeReceivePacketResponseHandler(ZigBeeRxResponse e);
    public delegate void ZigBeeTransmitStatusResponseHandler(ZigBeeTxStatusResponse e);
    #endregion

    public class XBeeAPI
    {
        public event ChecksumErrorHandler onChecksumError;
        public event UndefinedPacketHandler onUndefinedPacket;

        public event ATCommandResponseHandler onATCommandResponse;
        public event ModemStatusResponseHandler onModemStatusResponse;
        public event NodeIdentificationResponseHandler onNodeIdentificationResponse;
        public event RemoteCommandResponseHandler onRemoteCommandResponse;
        public event XBeeIODataSampleRx16ResponseHandler onXBeeIODataSampleRx16Response;
        public event XBeeIODataSampleRx64ResponseHandler onXBeeIODataSampleRx64Response;
        public event XBeeRx16IndicatorHandler onXBeeRx16Indicator;
        public event XBeeRx64IndicatorHandler onXBeeRx64Indicator;
        public event XBeeSensorReadResponseHandler onXBeeSensorReadResponse;
        public event XBeeTransmitStatusResponseHandler onXBeeTransmitStatusResponse;
        public event ZigBeeExplicitRxResponseHandler onZigBeeExplicitRxResponse;
        public event ZigBeeIODataSampleRXResponseHandler onZigBeeIODataSampleRXResponse;
        public event ZigBeeReceivePacketResponseHandler onZigBeeReceivePacketResponse;
        public event ZigBeeTransmitStatusResponseHandler onZigBeeTransmitStatusResponse;

        private const byte KEY = 0x7E;
        private const byte ESCAPED = 0x7D;
        private const byte XON = 0x11;
        private const byte XOFF = 0x13;
        private const int INITIAL_FRAME_LENGTH = 10;

        private SerialPort serialPort;
        private ResponseBase response;
        private APIMode mode;

        private bool isRunning = false;
        private bool isChecksum = true;

        public XBeeAPI(string COM)
            : this(COM, 9600, APIMode.NORMAL)
        { }

        public XBeeAPI(string COM, APIMode mode)
            : this(COM, 9600, mode)
        { }

        public XBeeAPI(string COM, int baudRate, APIMode mode)
        {
            this.serialPort = new SerialPort(COM, baudRate, Parity.None, 8, StopBits.One);
            this.mode = mode;
            this.response = new ResponseBase(INITIAL_FRAME_LENGTH);
            this.serialPort.Open();
        }

        /// <summary>
        /// get or set whether to verify receive packet's checksum
        /// </summary>
        public bool VerifyChecksum
        {
            get { return this.isChecksum; }
            set { this.isChecksum = value; }
        }

        /// <summary>
        /// to start send and process response, must be called before any function
        /// </summary>
        public void Start()
        {
            if (!isRunning)
            {
                isRunning = true;
                new Thread(readingThread).Start();
            }
        }

        /// <summary>
        /// stop so the serial port can be used for other purpose
        /// </summary>
        public void Stop()
        {
            if (isRunning)
                isRunning = false;
        }

        public void Send(RequestBase request)
        {
            if (!serialPort.IsOpen)
                return;

            lock (serialPort)
            {
                request.CalculateChecksum();

                byte msb = (byte)(request.GetLength() >> 8);
                byte lsb = (byte)request.GetLength();

                _WriteByte(KEY);

                WriteByte(msb);
                WriteByte(lsb);

                for (int i = 0; i < request.GetLength(); i++)
                    WriteByte(request.GetFrameData()[i]);

                WriteByte(request.GetCheckSum());
            }
        }

        private void PacketProcess()
        {
            if (isChecksum)
            {
                if (!response.VerifyChecksum())
                {
                    if (onChecksumError != null)
                        onChecksumError(response);
                    return;
                }
            }

            switch (response.GetFrameType())
            {
                case API_IDENTIFIER.Rx64_Receive_Packet:
                    if (onXBeeRx64Indicator != null)
                        onXBeeRx64Indicator(new XBeeRx64Response(response));
                    break;
                case API_IDENTIFIER.Rx16_Receive_Packet:
                    if (onXBeeRx16Indicator != null)
                        onXBeeRx16Indicator(new XBeeRx16Response(response));
                    break;
                case API_IDENTIFIER.Rx64_IO_Data_Sample_Rx_Indicator:
                    if (onXBeeIODataSampleRx64Response != null)
                        onXBeeIODataSampleRx64Response(new XBeeIODataSampleRx64Response(response));
                    break;
                case API_IDENTIFIER.Rx16_IO_Data_Sample_Rx_Indicator:
                    if (onXBeeIODataSampleRx16Response != null)
                        onXBeeIODataSampleRx16Response(new XBeeIODataSampleRx16Response(response));
                    break;
                case API_IDENTIFIER.XBee_Transmit_Status:
                    if (onXBeeTransmitStatusResponse != null)
                        onXBeeTransmitStatusResponse(new XBeeTxStatusResponse(response));
                    break;
                case API_IDENTIFIER.AT_Command_Response:
                    if (onATCommandResponse != null)
                        onATCommandResponse(new ATCommandResponse(response));
                    break;
                case API_IDENTIFIER.Modem_Status:
                    if (onModemStatusResponse != null)
                        onModemStatusResponse(new ModemStatusResponse(response));
                    break;
                case API_IDENTIFIER.ZigBee_Transmit_Status:
                    if (onZigBeeTransmitStatusResponse != null)
                        onZigBeeTransmitStatusResponse(new ZigBeeTxStatusResponse(response));
                    break;
                case API_IDENTIFIER.ZigBee_Receive_Packet:
                    if (onZigBeeReceivePacketResponse != null)
                        onZigBeeReceivePacketResponse(new ZigBeeRxResponse(response));
                    break;
                case API_IDENTIFIER.ZigBee_Explicit_Rx_Indicator:
                    if (onZigBeeExplicitRxResponse != null)
                        onZigBeeExplicitRxResponse(new ZigBeeExplicitRxResponse(response));
                    break;
                case API_IDENTIFIER.ZigBee_IO_Data_Sample_Rx_Indicator:
                    if (onZigBeeIODataSampleRXResponse != null)
                        onZigBeeIODataSampleRXResponse(new ZigBeeIODataSampleRxResponse(response));
                    break;
                case API_IDENTIFIER.XBee_Sensor_Read_Indicato:
                    if (onXBeeSensorReadResponse != null)
                        onXBeeSensorReadResponse(new SensorReadResponse(response));
                    break;
                case API_IDENTIFIER.Node_Identification_Indicator:
                    if (onNodeIdentificationResponse != null)
                        onNodeIdentificationResponse(new NodeIdentificationResponse(response));
                    break;
                case API_IDENTIFIER.Remote_Command_Response:
                    if (onRemoteCommandResponse != null)
                        onRemoteCommandResponse(new RemoteCommandResponse(response));
                    break;
                case API_IDENTIFIER.Over_the_Air_Firmware_Update_Status: break;
                case API_IDENTIFIER.Route_Record_Indicator: break;
                case API_IDENTIFIER.Many_to_One_Route_Request_Indicator: break;
                default:
                    if (onUndefinedPacket != null)
                        onUndefinedPacket(response); break;
            }
        }

        private void readingThread()
        {
            while (isRunning)
            {
                if (ReadByte() != KEY)
                    continue;

                int length = getLength();

                response.rewind(length);

                if (readPayLoad())
                    PacketProcess();
                else break;
            }
        }

        private int getLength()
        {
            int msb = ReadByte();

            int lsb = ReadByte();

            return (msb << 8) | lsb;
        }

        private bool readPayLoad()
        {
            while (true)
                if (response.append((byte)ReadByte()))
                    return true;
        }

        #region IO additional layer to handle ESCAPE automatically

        /// <summary>
        /// if success return non zero, -1 means something is wrong
        /// </summary>
        /// <returns></returns>
        private int _ReadByte()
        {
            return serialPort.ReadByte();
        }

        /// <summary>
        /// read one byte payload, which allready handle the escape char, if less than 0 means error occured
        /// </summary>
        /// <returns></returns>
        private int ReadByte()
        {
            int value = _ReadByte();

            if (mode == APIMode.ESCAPED && value == ESCAPED)
                return _ReadByte() ^ 0x20;

            return value;
        }

        private void _WriteByte(byte data)
        {
            serialPort.BaseStream.WriteByte(data);
        }

        /// <summary>
        /// write one byte to the payload, which allready handle the escape char
        /// </summary>
        /// <param name="data"></param>
        private void WriteByte(byte data)
        {
            if (mode == APIMode.ESCAPED)
            {
                if (data == KEY || data == ESCAPED || data == XON || data == XOFF)
                {
                    _WriteByte(ESCAPED);
                    _WriteByte((byte)(data ^ 0x20));
                    return;
                }
            }

            _WriteByte(data);
        }
        #endregion
    }
}