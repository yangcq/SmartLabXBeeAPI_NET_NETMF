using System.IO.Ports;
using System.Threading;
using SmartLab.XBee.Device;
using SmartLab.XBee.Options;
using SmartLab.XBee.Indicator;
using SmartLab.XBee.Type;

namespace SmartLab.XBee
{
    #region Delegate
    public delegate void ChecksumErrorHandler(APIFrame e);
    public delegate void UndefinedPacketHandler(APIFrame e);

    public delegate void ATCommandIndicatorHandler(ATCommandIndicator indicator);
    public delegate void ModemStatusIndicatorHandler(ModemStatusIndicator indicator);
    public delegate void NodeIdentificationIndicatorHandler(NodeIdentificationIndicator indicator);
    public delegate void RemoteCommandIndicatorHandler(RemoteCommandIndicator indicator);
    public delegate void XBeeRx16IOSampleIndicatorHandler(XBeeRx16IOSampleIndicator indicator);
    public delegate void XBeeRx64IOSampleIndicatorHandler(XBeeRx64IOSampleIndicator indicator);
    public delegate void XBeeRx16IndicatorHandler(XBeeRx16Indicator indicator);
    public delegate void XBeeRx64IndicatorHandler(XBeeRx64Indicator indicator);
    public delegate void SensorReadIndicatorHandler(SensorReadIndicator indicator);
    public delegate void XBeeTxStatusIndicatorHandler(XBeeTxStatusIndicator indicator);
    public delegate void ZigBeeExplicitRxIndicatorHandler(ZigBeeExplicitRxIndicator indicator);
    public delegate void ZigBeeIOSampleIndicatorHandler(ZigBeeIOSampleIndicator indicator);
    public delegate void ZigBeeRxIndicatorHandler(ZigBeeRxIndicator indicator);
    public delegate void ZigBeeTxStatusIndicatorHandler(ZigBeeTxStatusIndicator indicator);
    public delegate void ManyToOneRouteIndicatorHandler(ManyToOneRouteIndicator indicator);
    public delegate void RouteRecordIndicatorHandler(RouteRecordIndicator indicator);
    #endregion

    public class Core
    {
        public event ChecksumErrorHandler onChecksumErrorIndicator;
        public event UndefinedPacketHandler onUndefinedPacketIndicator;

        public event ATCommandIndicatorHandler onATCommandIndicator;
        public event ModemStatusIndicatorHandler onModemStatusIndicator;
        public event NodeIdentificationIndicatorHandler onNodeIdentificationIndicator;
        public event RemoteCommandIndicatorHandler onRemoteCommandIndicator;
        public event XBeeRx16IOSampleIndicatorHandler onXBeeRx16IOSampleIndicator;
        public event XBeeRx64IOSampleIndicatorHandler onXBeeRx64IOSampleIndicator;
        public event XBeeRx16IndicatorHandler onXBeeRx16Indicator;
        public event XBeeRx64IndicatorHandler onXBeeRx64Indicator;
        public event SensorReadIndicatorHandler onSensorReadIndicator;
        public event XBeeTxStatusIndicatorHandler onXBeeTxStatusIndicator;
        public event ZigBeeExplicitRxIndicatorHandler onZigBeeExplicitRxIndicator;
        public event ZigBeeIOSampleIndicatorHandler onZigBeeIOSampleIndicator;
        public event ZigBeeRxIndicatorHandler onZigBeeRxIndicator;
        public event ZigBeeTxStatusIndicatorHandler onZigBeeTxStatusIndicator;
        public event ManyToOneRouteIndicatorHandler onManyToOneRequestIndicator;
        public event RouteRecordIndicatorHandler onRouteRecordIndicator;

        private const byte KEY = 0x7E;
        private const byte ESCAPED = 0x7D;
        private const byte XON = 0x11;
        private const byte XOFF = 0x13;
        private const int INITIAL_FRAME_LENGTH = 100;

        private ISerial serial;
        private APIMode mode;

        private APIFrame request;
        private APIFrame response;
        private bool isSignal = false;
        private byte waitFrameID;
        private API_IDENTIFIER waitFrameType;
        private const int DEFAULT_WAIT = 10000;
        private AutoResetEvent waitEvent;

        private bool isRunning = false;
        private bool isChecksum = true;

        public Core(ISerial serial, APIMode mode)
        {
            this.serial = serial;
            this.waitEvent = new AutoResetEvent(false);
            this.mode = mode;
            this.response = new APIFrame(INITIAL_FRAME_LENGTH);
            this.request = new APIFrame(INITIAL_FRAME_LENGTH);
        }

        #region General Function

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

                if (!serial.IsOpen())
                    this.serial.Open();

                new Thread(readingThread).Start();
            }
        }

        /// <summary>
        /// stop so the serial port can be used for other purpose
        /// </summary>
        public void Stop()
        {
            if (isRunning)
            {
                isRunning = false;
                serial.Close();
            }
        }

        /// <summary>
        /// a general function to send frame out, do not process response
        /// </summary>
        /// <param name="request"></param>
        public void Send(APIFrame request)
        {
            if (!isRunning)
                return;

            lock (serial)
            {
                request.CalculateChecksum();

                byte msb = (byte)(request.GetPosition() >> 8);
                byte lsb = (byte)request.GetPosition();

                serial.WriteByte(KEY);

                WriteByte(msb);
                WriteByte(lsb);

                for (int i = 0; i < request.GetPosition(); i++)
                    WriteByte(request.GetFrameData()[i]);

                WriteByte(request.GetCheckSum());
            }
        }

        #endregion

        #region Advance Function

        public XBeeTxStatusIndicator SendXBeeTx16(Address remoteAddress, OptionsBase option, byte[] payload) { return SendXBeeTx16(remoteAddress, option, payload, 0, payload.Length); }

        public XBeeTxStatusIndicator SendXBeeTx16(Address remoteAddress, OptionsBase option, byte[] payload, int offset, int length)
        {
            waitFrameID++;
            if (waitFrameID == 0)
                waitFrameID = 0x01;

            waitFrameType = API_IDENTIFIER.XBee_Transmit_Status;

            request.Rewind();
            request.SetContent((byte)API_IDENTIFIER.Tx16_Request);
            request.SetContent(waitFrameID);
            request.SetContent((byte)(remoteAddress.GetNetworkAddress() >> 8));
            request.SetContent((byte)remoteAddress.GetNetworkAddress());
            request.SetContent(option.GetValue());
            request.SetContent(payload, offset, length);

            isSignal = true;

            Send(request);

            waitEvent.WaitOne(DEFAULT_WAIT, false);

            if (isSignal)
            {
                isSignal = false;
                return null;
            }

            return new XBeeTxStatusIndicator(request);
        }

        public XBeeTxStatusIndicator SendXBeeTx64(Address remoteAddress, OptionsBase option, byte[] payload) { return SendXBeeTx64(remoteAddress, option, payload, 0, payload.Length); }

        public XBeeTxStatusIndicator SendXBeeTx64(Address remoteAddress, OptionsBase option, byte[] payload, int offset, int length)
        {
            waitFrameID++;
            if (waitFrameID == 0)
                waitFrameID = 0x01;

            waitFrameType = API_IDENTIFIER.XBee_Transmit_Status;

            request.Rewind();
            request.SetContent((byte)API_IDENTIFIER.Tx64_Request);
            request.SetContent(waitFrameID);
            request.SetContent(remoteAddress.GetAddressValue(), 0, 8);
            request.SetContent(option.GetValue());
            request.SetContent(payload, offset, length);

            isSignal = true;

            Send(request);

            waitEvent.WaitOne(DEFAULT_WAIT, false);

            if (isSignal)
            {
                isSignal = false;
                return null;
            }

            return new XBeeTxStatusIndicator(request);
        }

        public ATCommandIndicator SendATCommand(ATCommand command, bool applyChange, byte[] parameter = null)
        {
            if (parameter == null)
                return SendATCommand(command, applyChange, parameter, 0, 0);
            else
                return SendATCommand(command, applyChange, parameter, 0, parameter.Length);
        }

        public ATCommandIndicator SendATCommand(ATCommand command, bool applyChange, byte[] parameter, int offset, int length)
        {
            waitFrameID++;
            if (waitFrameID == 0)
                waitFrameID = 0x01;

            waitFrameType = API_IDENTIFIER.AT_Command_Response;

            request.Rewind();
            if (applyChange)
                request.SetContent((byte)API_IDENTIFIER.AT_Command);
            else request.SetContent((byte)API_IDENTIFIER.AT_Command_Queue_Parameter_Value);
            request.SetContent(waitFrameID);
            request.SetContent(command.GetValue());
            if (parameter != null)
                request.SetContent(parameter, offset, length);

            isSignal = true;

            Send(request);

            waitEvent.WaitOne(DEFAULT_WAIT, false);

            if (isSignal)
            {
                isSignal = false;
                return null;
            }

            return new ATCommandIndicator(request);
        }

        public RemoteCommandIndicator SendRemoteATCommand(Address remoteAddress, ATCommand command, OptionsBase transmitOptions, byte[] parameter = null)
        {
            if (parameter == null)
                return SendRemoteATCommand(remoteAddress, command, transmitOptions, parameter, 0, 0);
            else
                return SendRemoteATCommand(remoteAddress, command, transmitOptions, parameter, 0, parameter.Length);
        }

        public RemoteCommandIndicator SendRemoteATCommand(Address remoteAddress, ATCommand command, OptionsBase transmitOptions, byte[] parameter, int parameterOffset, int parameterLength)
        {
            waitFrameID++;
            if (waitFrameID == 0)
                waitFrameID = 0x01;

            waitFrameType = API_IDENTIFIER.Remote_Command_Response;

            request.Rewind();
            request.SetContent((byte)API_IDENTIFIER.Remote_Command_Request);
            request.SetContent(waitFrameID);

            request.SetContent(remoteAddress.GetAddressValue());
            request.SetContent(transmitOptions.GetValue());
            request.SetContent(command.GetValue());

            if (parameter != null)
                request.SetContent(parameter, parameterOffset, parameterLength);

            isSignal = true;

            Send(request);

            waitEvent.WaitOne(DEFAULT_WAIT, false);

            if (isSignal)
            {
                isSignal = false;
                return null;
            }

            return new RemoteCommandIndicator(request);
        }

        public ZigBeeTxStatusIndicator SendZigBeeTx(Address remoteAddress, OptionsBase option, byte[] payload) { return SendZigBeeTx(remoteAddress, option, payload, 0, payload.Length); }

        public ZigBeeTxStatusIndicator SendZigBeeTx(Address remoteAddress, OptionsBase option, byte[] payload, int offset, int length)
        {
            waitFrameID++;
            if (waitFrameID == 0)
                waitFrameID = 0x01;

            waitFrameType = API_IDENTIFIER.ZigBee_Transmit_Status;

            request.Rewind();
            request.SetContent((byte)API_IDENTIFIER.ZigBee_Transmit_Request);
            request.SetContent(waitFrameID);
            request.SetContent(remoteAddress.GetAddressValue());
            request.SetContent(0x00);
            request.SetContent(option.GetValue());
            request.SetContent(payload, offset, length);

            isSignal = true;

            Send(request);

            waitEvent.WaitOne(DEFAULT_WAIT, false);

            if (isSignal)
            {
                isSignal = false;
                return null;
            }

            return new ZigBeeTxStatusIndicator(request);
        }

        public ZigBeeTxStatusIndicator SendZigBeeExplicitTx(ExplicitAddress remoteAddress, OptionsBase option, byte[] payload) { return SendZigBeeExplicitTx(remoteAddress, option, payload, 0, payload.Length); }

        public ZigBeeTxStatusIndicator SendZigBeeExplicitTx(ExplicitAddress remoteAddress, OptionsBase option, byte[] payload, int offset, int length)
        {
            waitFrameID++;
            if (waitFrameID == 0)
                waitFrameID = 0x01;

            waitFrameType = API_IDENTIFIER.ZigBee_Transmit_Status;

            request.Rewind();
            request.SetContent((byte)API_IDENTIFIER.Explicit_Addressing_ZigBee_Command_Frame);
            request.SetContent(waitFrameID);
            request.SetContent(remoteAddress.GetAddressValue());
            request.SetContent(remoteAddress.GetExplicitValue());
            request.SetContent(0x00);
            request.SetContent(option.GetValue());
            request.SetContent(payload, offset, length);

            isSignal = true;

            Send(request);

            waitEvent.WaitOne(DEFAULT_WAIT, false);

            if (isSignal)
            {
                isSignal = false;
                return null;
            }

            return new ZigBeeTxStatusIndicator(request);
        }

        public ATCommandIndicator SetPinFunction(Pin pin, Pin.Functions function) { return SendATCommand(new ATCommand(pin.COMMAND), true, new byte[] { (byte)function }); }

        public ATCommandIndicator SetIODetection(Pin[] pins) { return SendATCommand(ATCommand.Digital_IO_Change_Detection, true, Pin.IOChangeDetectionConfiguration(pins)); }

        public RemoteCommandIndicator SetRemotePinFunction(Address remoteAddress, Pin pin, Pin.Functions function) { return SendRemoteATCommand(remoteAddress, new ATCommand(pin.COMMAND), RemoteCommandOptions.ApplyChanges, new byte[] { (byte)function }); }

        public RemoteCommandIndicator SetRemoteIODetection(Address remoteAddress, Pin[] pins) { return SendRemoteATCommand(remoteAddress, ATCommand.Digital_IO_Change_Detection, RemoteCommandOptions.ApplyChanges, Pin.IOChangeDetectionConfiguration(pins)); }

        #endregion

        #region Packet Process

        private void PacketProcess()
        {
            if (isChecksum)
            {
                if (!response.VerifyChecksum())
                {
                    if (onChecksumErrorIndicator != null)
                        onChecksumErrorIndicator(response);
                    return;
                }
            }

            if (isSignal && response.GetFrameData()[1] == request.GetFrameData()[1] && response.GetFrameType() == waitFrameType)
            {
                isSignal = false;
                request.Rewind();
                request.SetContent(response.GetFrameData(), 0, response.GetPosition());
                waitEvent.Set();
                return;
            }

            switch (response.GetFrameType())
            {
                case API_IDENTIFIER.Rx64_Receive_Packet:
                    if (onXBeeRx64Indicator != null)
                        onXBeeRx64Indicator(new XBeeRx64Indicator(response));
                    break;
                case API_IDENTIFIER.Rx16_Receive_Packet:
                    if (onXBeeRx16Indicator != null)
                        onXBeeRx16Indicator(new XBeeRx16Indicator(response));
                    break;
                case API_IDENTIFIER.Rx64_IO_Data_Sample_Rx_Indicator:
                    if (onXBeeRx64IOSampleIndicator != null)
                        onXBeeRx64IOSampleIndicator(new XBeeRx64IOSampleIndicator(response));
                    break;
                case API_IDENTIFIER.Rx16_IO_Data_Sample_Rx_Indicator:
                    if (onXBeeRx16IOSampleIndicator != null)
                        onXBeeRx16IOSampleIndicator(new XBeeRx16IOSampleIndicator(response));
                    break;
                case API_IDENTIFIER.XBee_Transmit_Status:
                    if (onXBeeTxStatusIndicator != null)
                        onXBeeTxStatusIndicator(new XBeeTxStatusIndicator(response));
                    break;
                case API_IDENTIFIER.AT_Command_Response:
                    if (onATCommandIndicator != null)
                        onATCommandIndicator(new ATCommandIndicator(response));
                    break;
                case API_IDENTIFIER.Modem_Status:
                    if (onModemStatusIndicator != null)
                        onModemStatusIndicator(new ModemStatusIndicator(response));
                    break;
                case API_IDENTIFIER.ZigBee_Transmit_Status:
                    if (onZigBeeTxStatusIndicator != null)
                        onZigBeeTxStatusIndicator(new ZigBeeTxStatusIndicator(response));
                    break;
                case API_IDENTIFIER.ZigBee_Receive_Packet:
                    if (onZigBeeRxIndicator != null)
                        onZigBeeRxIndicator(new ZigBeeRxIndicator(response));
                    break;
                case API_IDENTIFIER.ZigBee_Explicit_Rx_Indicator:
                    if (onZigBeeExplicitRxIndicator != null)
                        onZigBeeExplicitRxIndicator(new ZigBeeExplicitRxIndicator(response));
                    break;
                case API_IDENTIFIER.ZigBee_IO_Data_Sample_Rx_Indicator:
                    if (onZigBeeIOSampleIndicator != null)
                        onZigBeeIOSampleIndicator(new ZigBeeIOSampleIndicator(response));
                    break;
                case API_IDENTIFIER.XBee_Sensor_Read_Indicato:
                    if (onSensorReadIndicator != null)
                        onSensorReadIndicator(new SensorReadIndicator(response));
                    break;
                case API_IDENTIFIER.Node_Identification_Indicator:
                    if (onNodeIdentificationIndicator != null)
                        onNodeIdentificationIndicator(new NodeIdentificationIndicator(response));
                    break;
                case API_IDENTIFIER.Remote_Command_Response:
                    if (onRemoteCommandIndicator != null)
                        onRemoteCommandIndicator(new RemoteCommandIndicator(response));
                    break;
                case API_IDENTIFIER.Route_Record_Indicator:
                    if (onRouteRecordIndicator != null)
                        onRouteRecordIndicator(new RouteRecordIndicator(response));
                    break;
                case API_IDENTIFIER.Many_to_One_Route_Request_Indicator:
                    if (onManyToOneRequestIndicator != null)
                        onManyToOneRequestIndicator(new ManyToOneRouteIndicator(response));
                    break;
                case API_IDENTIFIER.Create_Source_Route: break;
                default:
                    if (onUndefinedPacketIndicator != null)
                        onUndefinedPacketIndicator(response); break;
            }
        }

        private void readingThread()
        {
            while (isRunning)
            {
                try
                {
                    if (ReadByte() != KEY)
                        continue;

                    int length = getLength();

                    response.Allocate(length);

                    readPayLoad(length);

                    PacketProcess();
                }
                catch { break; }
            }
        }

        private int getLength()
        {
            int msb = ReadByte();

            int lsb = ReadByte();

            return (msb << 8) | lsb;
        }

        private void readPayLoad(int length)
        {
            for (int i = 0; i < length; i++)
                response.SetContent((byte)ReadByte());

            response.SetCheckSum((byte)ReadByte());
        }

        #endregion

        #region IO additional layer to handle ESCAPE automatically
        /// <summary>
        /// read one byte payload, which allready handle the escape char, if less than 0 means error occured
        /// </summary>
        /// <returns></returns>
        private int ReadByte()
        {
            int value = serial.ReadByte();

            if (mode == APIMode.ESCAPED && value == ESCAPED)
                return serial.ReadByte() ^ 0x20;

            return value;
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
                    serial.WriteByte(ESCAPED);
                    serial.WriteByte((byte)(data ^ 0x20));
                    return;
                }
            }

            serial.WriteByte(data);
        }
        #endregion
    }
}