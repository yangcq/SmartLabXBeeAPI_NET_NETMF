using System.Threading;
using SmartLab.XBee.Device;
using SmartLab.XBee.Helper;
using SmartLab.XBee.Indicator;
using SmartLab.XBee.Options;
using SmartLab.XBee.Type;

namespace SmartLab.XBee.Core
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

    public class CoreAPI
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

        XBeeRx64Indicator xBeeRx64Indicator = new XBeeRx64Indicator(null);
        XBeeRx16Indicator xBeeRx16Indicator = new XBeeRx16Indicator(null);
        XBeeRx64IOSampleIndicator xBeeRx64IOSampleIndicator = new XBeeRx64IOSampleIndicator(null);
        XBeeRx16IOSampleIndicator xBeeRx16IOSampleIndicator = new XBeeRx16IOSampleIndicator(null);
        XBeeTxStatusIndicator xBeeTxStatusIndicator = new XBeeTxStatusIndicator(null);
        ATCommandIndicator aTCommandIndicator = new ATCommandIndicator(null);
        ModemStatusIndicator modemStatusIndicator = new ModemStatusIndicator(null);
        ZigBeeTxStatusIndicator zigBeeTxStatusIndicator = new ZigBeeTxStatusIndicator(null);
        ZigBeeRxIndicator zigBeeRxIndicator = new ZigBeeRxIndicator(null);
        ZigBeeExplicitRxIndicator zigBeeExplicitRxIndicator = new ZigBeeExplicitRxIndicator(null);
        ZigBeeIOSampleIndicator zigBeeIOSampleIndicator = new ZigBeeIOSampleIndicator(null);
        SensorReadIndicator sensorReadIndicator = new SensorReadIndicator(null);
        NodeIdentificationIndicator nodeIdentificationIndicator = new NodeIdentificationIndicator(null);
        RemoteCommandIndicator remoteCommandIndicator = new RemoteCommandIndicator(null);
        RouteRecordIndicator routeRecordIndicator = new RouteRecordIndicator(null);
        ManyToOneRouteIndicator manyToOneRouteIndicator = new ManyToOneRouteIndicator(null);

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

        public CoreAPI(ISerial serial, APIMode mode)
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

        #region IO Sample

        /// <summary>
        /// The command will immediately return an "OK" response. The data will follow in the normal API format for DIO data event.
        /// </summary>
        /// <returns>true if the command is "OK", false if no IO is enabled.</returns>
        public bool ForceXBeeLocalIOSample()
        {
            ATCommandIndicator re = SendATCommand(ATCommand.Force_Sample, true);

            if (re == null)
                return false;

            if (re.GetCommandStatus() != Status.CommandStatus.OK)
                return false;

            return true;
        }

        /// <summary>
        /// Return 1 IO sample from the local module.
        /// </summary>
        /// <returns></returns>
        public IOSamples ForceZigBeeLocalIOSample()
        {
            ATCommandIndicator re = SendATCommand(ATCommand.Force_Sample, true);

            if (re == null)
                return null;

            IOSamples[] array = ATInterpreter.FromZigBeeIS(re);

            if (array != null && array.Length > 0)
                return array[0];

            return null;
        }

        /// <summary>
        /// Return 1 IO sample only, Samples before TX (IT) does not affect.
        /// </summary>
        /// <param name="remote"Remote address of the device></param>
        /// <returns></returns>
        public IOSamples ForceXBeeRemoteIOSample(Address remote)
        {
            RemoteCommandIndicator re = SendRemoteATCommand(remote, ATCommand.Force_Sample, OptionsBase.DEFAULT);

            IOSamples[] array = ATInterpreter.FromXBeeIS(re);

            if (array != null && array.Length > 0)
                return array[0];

            return null;
        }

        /// <summary>
        /// Return 1 IO sample only.
        /// </summary>
        /// <param name="remote">Remote address of the device</param>
        /// <returns></returns>
        public IOSamples ForceZigBeeRemoteIOSample(Address remote)
        {
            RemoteCommandIndicator re = SendRemoteATCommand(remote, ATCommand.Force_Sample, OptionsBase.DEFAULT);

            IOSamples[] array = ATInterpreter.FromZigBeeIS(re);

            if (array != null && array.Length > 0)
                return array[0];

            return null;
        }

        #endregion

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
                    if (onXBeeRx64Indicator != null && xBeeRx64Indicator.Convert(response))
                        onXBeeRx64Indicator(xBeeRx64Indicator);
                    break;
                case API_IDENTIFIER.Rx16_Receive_Packet:
                    if (onXBeeRx16Indicator != null && xBeeRx16Indicator.Convert(response))
                        onXBeeRx16Indicator(xBeeRx16Indicator);
                    break;
                case API_IDENTIFIER.Rx64_IO_Data_Sample_Rx_Indicator:
                    if (onXBeeRx64IOSampleIndicator != null && xBeeRx64IOSampleIndicator.Convert(response))
                        onXBeeRx64IOSampleIndicator(xBeeRx64IOSampleIndicator);
                    break;
                case API_IDENTIFIER.Rx16_IO_Data_Sample_Rx_Indicator:
                    if (onXBeeRx16IOSampleIndicator != null && xBeeRx16IOSampleIndicator.Convert(response))
                        onXBeeRx16IOSampleIndicator(xBeeRx16IOSampleIndicator);
                    break;
                case API_IDENTIFIER.XBee_Transmit_Status:
                    if (onXBeeTxStatusIndicator != null && xBeeTxStatusIndicator.Convert(response))
                        onXBeeTxStatusIndicator(xBeeTxStatusIndicator);
                    break;
                case API_IDENTIFIER.AT_Command_Response:
                    if (onATCommandIndicator != null && aTCommandIndicator.Convert(response))
                        onATCommandIndicator(aTCommandIndicator);
                    break;
                case API_IDENTIFIER.Modem_Status:
                    if (onModemStatusIndicator != null && modemStatusIndicator.Convert(response))
                        onModemStatusIndicator(modemStatusIndicator);
                    break;
                case API_IDENTIFIER.ZigBee_Transmit_Status:
                    if (onZigBeeTxStatusIndicator != null && zigBeeTxStatusIndicator.Convert(response))
                        onZigBeeTxStatusIndicator(zigBeeTxStatusIndicator);
                    break;
                case API_IDENTIFIER.ZigBee_Receive_Packet:
                    if (onZigBeeRxIndicator != null && zigBeeRxIndicator.Convert(response))
                        onZigBeeRxIndicator(zigBeeRxIndicator);
                    break;
                case API_IDENTIFIER.ZigBee_Explicit_Rx_Indicator:
                    if (onZigBeeExplicitRxIndicator != null && zigBeeExplicitRxIndicator.Convert(response))
                        onZigBeeExplicitRxIndicator(zigBeeExplicitRxIndicator);
                    break;
                case API_IDENTIFIER.ZigBee_IO_Data_Sample_Rx_Indicator:
                    if (onZigBeeIOSampleIndicator != null && zigBeeIOSampleIndicator.Convert(response))
                        onZigBeeIOSampleIndicator(zigBeeIOSampleIndicator);
                    break;
                case API_IDENTIFIER.XBee_Sensor_Read_Indicato:
                    if (onSensorReadIndicator != null && sensorReadIndicator.Convert(response))
                        onSensorReadIndicator(sensorReadIndicator);
                    break;
                case API_IDENTIFIER.Node_Identification_Indicator:
                    if (onNodeIdentificationIndicator != null && nodeIdentificationIndicator.Convert(response))
                        onNodeIdentificationIndicator(nodeIdentificationIndicator);
                    break;
                case API_IDENTIFIER.Remote_Command_Response:
                    if (onRemoteCommandIndicator != null && remoteCommandIndicator.Convert(response))
                        onRemoteCommandIndicator(remoteCommandIndicator);
                    break;
                case API_IDENTIFIER.Route_Record_Indicator:
                    if (onRouteRecordIndicator != null && routeRecordIndicator.Convert(response))
                        onRouteRecordIndicator(routeRecordIndicator);
                    break;
                case API_IDENTIFIER.Many_to_One_Route_Request_Indicator:
                    if (onManyToOneRequestIndicator != null && manyToOneRouteIndicator.Convert(response))
                        onManyToOneRequestIndicator(manyToOneRouteIndicator);
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