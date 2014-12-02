using System;
using System.IO.Ports;
using SmartLab.XBee.Request;
using SmartLab.XBee.Response;
using SmartLab.XBee.Type;
using System.IO;
using System.Threading;

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
        private const byte MIN_PACKET_SIZE = 0x06;
        private const int WAIT_TIME = 100;
        private const int SYNC_TIMEOUT = 10;
        private const int INITIAL_FRAME_LENGTH = 10;

        private SerialPort serialPort;
        private APIMode Mode;
        private ResponseBase frame;

        private int currentValue;
        private bool isChecksum = true;

        public XBeeAPI(string COM)
            : this(COM, 9600, APIMode.NORMAL)
        { }

        public XBeeAPI(string COM, APIMode Mode)
            : this(COM, 9600, Mode)
        { }

        public XBeeAPI(string COM, int baudRate, APIMode Mode)
        {
            this.serialPort = new SerialPort(COM, baudRate, Parity.None, 8, StopBits.One);
            this.Mode = Mode;
            this.frame = new ResponseBase(INITIAL_FRAME_LENGTH);
        }

        public bool VerifyChecksum
        {
            get { return this.isChecksum; }
            set { this.isChecksum = value; }
        }

        public void Start()
        {
            if (!serialPort.IsOpen)
            {
                serialPort.Open();
                new Thread(readingThread).Start();
            }
        }

        public void Stop()
        {
            if (serialPort.IsOpen)
                serialPort.Close();
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

                WriteByte(KEY);
                if (Mode == APIMode.NORMAL)
                {
                    WriteByte(msb);
                    WriteByte(lsb);
                    WriteBytes(request.GetFrameData());
                    WriteByte(request.GetCheckSum());
                }
                else
                {
                    WriteWithEcsaped(msb);
                    WriteWithEcsaped(lsb);
                    for (int i = 0; i < request.GetLength(); i++)
                        WriteWithEcsaped(request.GetFrameData()[i]);
                    WriteWithEcsaped(request.GetCheckSum());
                }
            }
        }

        private void PacketProcess(ResponseBase response)
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
            while (serialPort.IsOpen)
            {
                currentValue = ReadByte();

                if (currentValue == -1)
                    break;

                if (currentValue == KEY)
                {
                    int length = getLength();
                    if (length >= MIN_PACKET_SIZE)
                    {
                        frame.rewind(length);

                        if (readPayLoad(frame))
                            PacketProcess(frame);
                    }
                }
            }
        }

        private int getLength()
        {
            int msb = ReadByte();

            if (msb == -1) return -1;

            if (Mode == APIMode.ESCAPED && msb == ESCAPED)
                msb = ReadByte() ^ 0x20;

            int lsb = ReadByte();

            if (lsb == -1) return -1;

            if (Mode == APIMode.ESCAPED && lsb == ESCAPED)
                lsb = ReadByte() ^ 0x20;

            return lsb;
            //return (msb << 8) | lsb;
        }

        private bool readPayLoad(APIFrame frame)
        {
            while (true)
            {
                currentValue = ReadByte();

                if (currentValue == -1)
                    return false;

                if (Mode == APIMode.ESCAPED && currentValue == ESCAPED)
                {
                    currentValue = ReadByte();

                    if (currentValue == -1)
                        return false;

                    currentValue ^= 0x20;
                }

                if (frame.append((byte)currentValue))
                    return true;
            }
        }

        #region IO
        private byte[] buffer = new byte[1];

        private int ReadByte()
        {
            int size = serialPort.Read(buffer, 0, 1);

            if (size == 1)
                return buffer[0];
            else return -1;
        }

        private void WriteByte(byte data)
        {
            buffer[0] = data;
            serialPort.Write(buffer, 0, 1);
        }

        private void WriteBytes(byte[] data)
        {
            serialPort.Write(buffer, 0, data.Length);
        }

        private void WriteWithEcsaped(byte data)
        {
            if (data == KEY || data == ESCAPED || data == XON || data == XOFF)
            {
                WriteByte(ESCAPED);
                WriteByte((byte)(data ^ 0x20));
            }
            else
                WriteByte(data);
        }
        #endregion
    }
}