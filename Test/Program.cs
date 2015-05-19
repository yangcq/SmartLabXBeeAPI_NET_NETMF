using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartLab.XBee;
using SmartLab.XBee.Type;
using SmartLab.XBee.Request;
using SmartLab.XBee.Options;
using System.Globalization;
using SmartLab.XBee.Indicator;
using System.IO.Ports;
using System.Collections;
using SmartLab.XBee.Status;
using SmartLab.XBee.Device;
using SmartLab.XBee.Helper;

namespace Test
{
    class Program
    {
        static XBeeAPI xbee;

        static ExplicitAddress remote = new ExplicitAddress();
        static string[] ports = SerialPort.GetPortNames();

        static IOSamples sss;

        static void Main(string[] args)
        {
            if (ports == null || ports.Length == 0)
            {
                Console.WriteLine("!!! no avaliable ports detected !!!");
                Console.ReadLine();
                return;
            }

            Console.WriteLine(">> Please select a port : ");
            for (int i = 0; i < ports.Length; i++)
                Console.WriteLine("[" + i + "] == " + ports[i]);

            Console.Write("\r\n << Ports Number : ");
            int index = int.Parse(Console.ReadLine());

            Console.Write("\r\n << Baud Rate : ");
            int baud = int.Parse(Console.ReadLine());

            Console.Write("\r\n << API Mode [0 = NORMAL, 1 = ESCAPED] : ");
            APIMode mode = int.Parse(Console.ReadLine()) == 0 ? APIMode.NORMAL : APIMode.ESCAPED;

            xbee = new XBeeAPI(ports[index], baud, mode);

            Console.WriteLine("\r\n++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            Console.WriteLine("+ [String start with 0x will be convert to Hex]");
            Console.WriteLine("+ [Response timeout is set to 10 seconds]");
            Console.WriteLine("+ [XBee Boradcast Address] : 0 0 FFFF");
            Console.WriteLine("+ [ZigBee Boradcast Address] : 0 FFFF");
            Console.WriteLine("+ [ZigBee Unknown NET Address] : 0 0 FFFE");
            Console.WriteLine("+ [Exit the Console] : exit");
            Console.WriteLine("+ [Send XBee Tx16 Request] : s16 NET(Hex String) payload");
            Console.WriteLine("+ [Send XBee Tx64 Request] : s64 SH(Hex String) SL(Hex String) payload");
            Console.WriteLine("+ [Send AT Command Request] : at command(String) parameter(can be null)");
            Console.WriteLine("+ [Send Remote AT Command Request] : rat SH SL NET command parameter");
            Console.WriteLine("+ [Send ZigBee Tx Request] : s SH SL NET payload");
            Console.WriteLine("+ [Send Explicit ZigBee Tx Request] : se SH SL NET SE DE CID PID payload");
            Console.WriteLine("+ [Create Source Route] : csr SH SL NET addresses");
            Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\r\n");

            // 1 byte of SourceEndpoint + 1 byte of DestinationEndpoint + 2 bytes of ClusterID + 2 bytes of ProfileID

            xbee.VerifyChecksum = true;
            xbee.onModemStatusIndicator += new ModemStatusIndicatorHandler(xbee_onModemStatusResponse);
            xbee.onNodeIdentificationIndicator += new NodeIdentificationIndicatorHandler(xbee_onNodeIdentificationResponse);
            xbee.onZigBeeTxStatusIndicator += new ZigBeeTxStatusIndicatorHandler(xbee_onZigBeeTransmitStatusResponse);
            xbee.onZigBeeRxIndicator += new ZigBeeRxIndicatorHandler(xbee_onZigBeeReceivePacketResponse);
            xbee.onATCommandIndicator += new ATCommandIndicatorHandler(xbee_onATCommandResponse);
            xbee.onXBeeTxStatusIndicator += new XBeeTxStatusIndicatorHandler(xbee_onXBeeTransmitStatusResponse);
            xbee.onXBeeRx16Indicator += new XBeeRx16IndicatorHandler(xbee_onXBeeRx16Indicator);
            xbee.onXBeeRx64Indicator += new XBeeRx64IndicatorHandler(xbee_onXBeeRx64Indicator);
            xbee.onRemoteCommandIndicator += new RemoteCommandIndicatorHandler(xbee_onRemoteCommandResponse);
            xbee.onXBeeRx16IOSampleIndicator += new XBeeRx16IOSampleIndicatorHandler(xbee_onXBeeIODataSampleRx16Response);
            xbee.onXBeeRx64IOSampleIndicator += new XBeeRx64IOSampleIndicatorHandler(xbee_onXBeeIODataSampleRx64Response);
            xbee.onZigBeeIOSampleIndicator += new ZigBeeIOSampleIndicatorHandler(xbee_onZigBeeIODataSampleRXResponse);
            xbee.onZigBeeExplicitRxIndicator += new ZigBeeExplicitRxIndicatorHandler(xbee_onZigBeeExplicitRxResponse);

            xbee.onManyToOneRequestIndicator += new ManyToOneRouteIndicatorHandler(xbee_onManyToOneRequestIndicator);
            xbee.onRouteRecordIndicator += new RouteRecordIndicatorHandler(xbee_onRouteRecordIndicator);
            xbee.Start();

            Console.Write("<< ");
            string line = Console.ReadLine();

            while (line != "exit")
            {
                string[] lines = line.Split(' ');

                switch (lines[0].ToLower())
                {
                    case "s16":
                        if (lines.Length >= 3)
                            TX16(lines);
                        else Console.WriteLine(">> wrong format!\r\n");
                        break;

                    case "s64":
                        if (lines.Length >= 4)
                            TX64(lines);
                        else Console.WriteLine(">> wrong format!\r\n");
                        break;

                    case "at":
                        if (lines.Length == 2 || lines.Length == 3)
                            AT(lines);
                        else Console.WriteLine(">> wrong format!\r\n");
                        break;

                    case "rat":
                        if (lines.Length == 5 || lines.Length == 6)
                            RAT(lines);
                        else Console.WriteLine(">> wrong format!\r\n");
                        break;

                    case "s":
                        if (lines.Length >= 5)
                            S(lines);
                        else Console.WriteLine(">> wrong format!\r\n");
                        break;

                    case "se":
                        if (lines.Length >= 9)
                            SE(lines);
                        else Console.WriteLine(">> wrong format!\r\n");
                        break;


                    case "csr":
                        if (lines.Length >= 5)
                            CSR(lines);
                        else Console.WriteLine(">> wrong format!\r\n");
                        break;

                    default: Console.WriteLine(">> unknown command!\r\n");
                        break;
                }

                Console.Write("<< ");
                line = Console.ReadLine();
            }

            xbee.Stop();
        }

        static void xbee_onRouteRecordIndicator(RouteRecordIndicator indicator)
        {
            Address add = indicator.GetRemoteDevice();
            Console.WriteLine("\r\n>> Route record indicator : From " + add.GetSerialNumberHigh().ToString("X2") + " " + add.GetSerialNumberLow().ToString("X2") + " " + add.GetNetworkAddress().ToString("X2"));
            Console.WriteLine(">> Number of address : " + indicator.GetNumberOfAddresses());
            foreach (int entry in indicator.GetAddresses())
                Console.WriteLine(">> " + entry.ToString("X2"));
        }

        static void xbee_onManyToOneRequestIndicator(ManyToOneRouteIndicator indicator)
        {
            Address add = indicator.GetRemoteDevice();
            Console.WriteLine("\r\n>> Many to one route request : From " + add.GetSerialNumberHigh().ToString("X2") + " " + add.GetSerialNumberLow().ToString("X2") + " " + add.GetNetworkAddress().ToString("X2") + "\r\n");
        }

        static void CSR(string[] lines)
        {
            int addressh = int.Parse(lines[1], NumberStyles.HexNumber);
            int addressl = int.Parse(lines[2], NumberStyles.HexNumber);
            int net = int.Parse(lines[3], NumberStyles.HexNumber);
            remote.SetSerialNumberHigh(addressh);
            remote.SetSerialNumberLow(addressl);
            remote.SetNetworkAddress(net);

            int[] addresses = new int[lines.Length - 4];
            for (int i = 4; i< lines.Length; i++)
                addresses[i - 4] = int.Parse(lines[i], NumberStyles.HexNumber);

            CreateSourceRouteRequest re = new CreateSourceRouteRequest(0x10, remote, addresses);
            xbee.Send(re);
        }

        static void TX16(string[] lines)
        {
            int address = int.Parse(lines[1], NumberStyles.HexNumber);
            remote.SetNetworkAddress(address);

            string datas = lines[2];
            for (int i = 3; i < lines.Length; i++)
                datas += " " + lines[i];
            byte[] data = GetBytes(datas);

            XBeeTxStatusIndicator re = xbee.SendXBeeTx16(remote, OptionsBase.DEFAULT, data);

            if (re == null)
                Console.WriteLine(">> ERROR : Tx16 response time out\r\n");
            else
                Console.WriteLine(">> " + re.GetDeliveryStatus() + "\r\n");
        }

        static void TX64(string[] lines)
        {
            int addressh = int.Parse(lines[1], NumberStyles.HexNumber);
            int addressl = int.Parse(lines[2], NumberStyles.HexNumber);
            remote.SetSerialNumberHigh(addressh);
            remote.SetSerialNumberLow(addressl);

            string datas = lines[3];
            for (int i = 4; i < lines.Length; i++)
                datas += " " + lines[i];
            byte[] data = GetBytes(datas);

            XBeeTxStatusIndicator re = xbee.SendXBeeTx64(remote, OptionsBase.DEFAULT, data);

            if (re == null)
                Console.WriteLine(">> ERROR : Tx64 response time out\r\n");
            else
                Console.WriteLine(">> " + re.GetDeliveryStatus() + "\r\n");
        }

        static void AT(string[] lines)
        {
            ATCommand com = new ATCommand(lines[1]);

            byte[] data = null;
            if (lines.Length > 2)
                data = GetBytes(lines[2]);

            ATCommandIndicator re = xbee.SendATCommand(com, true, data);

            if (re == null)
                Console.WriteLine(">> ERROR : AT command response time out\r\n");
            else
            {
                byte[] p = re.GetParameter();
                CommandStatus code = re.GetCommandStatus();
                Console.WriteLine(">> " + code);

                if (code == CommandStatus.OK && p != null)
                    Console.WriteLine(">> " + re.GetRequestCommand() + " = " + UTF8Encoding.UTF8.GetString(p) + " [" + GetString(p) + "]\r\n");
                else Console.Write("\r\n");


                ActiveScanResult result = ATInterpreter.FromAS(re);

                if (result != null)
                {
                    Console.WriteLine("\r\n>> Active Scan Result : ");
                    Console.WriteLine(">> Channel " + result.Channel.ToString("X2"));
                    Console.WriteLine(">> Pan ID [" + GetString(result.PanID) + "]");
                    Console.WriteLine(">> Extended Pan ID [" + GetString(result.ExtendedPanID) + "]");
                    Console.WriteLine(">> Allow Join " + result.AllowJoin);
                    Console.WriteLine(">> StackProfile " + result.StackProfile);
                    Console.WriteLine(">> LQI " + result.LQI);
                    Console.WriteLine(">> RSSI " + result.RSSI + "\r\n");
                }

                /*
                //ZigBeeDiscoverAddress dd = ZigBeeDiscoverAddress.Parse(re);
                XBeeDiscoverAddress dd = XBeeDiscoverAddress.Parse(re);

                if (dd != null)
                {
                    Console.WriteLine(">> Node Discovery : ");
                    Console.WriteLine(">> SH " + dd.GetSerialNumberHigh().ToString("X2"));
                    Console.WriteLine(">> SL " + dd.GetSerialNumberLow().ToString("X2"));
                    Console.WriteLine(">> NET " + dd.GetNetworkAddress().ToString("X2"));
                    Console.WriteLine(">> NI " + dd.GetNIString() + "\r\n");
                }*/
            }
        }

        static void RAT(string[] lines)
        {
            int addressh = int.Parse(lines[1], NumberStyles.HexNumber);
            int addressl = int.Parse(lines[2], NumberStyles.HexNumber);
            int net = int.Parse(lines[3], NumberStyles.HexNumber);
            ATCommand com = new ATCommand(lines[4]);

            remote.SetSerialNumberHigh(addressh);
            remote.SetSerialNumberLow(addressl);
            remote.SetNetworkAddress(net);

            byte[] data = null;
            if (lines.Length > 5)
                data = GetBytes(lines[5]);

            RemoteCommandIndicator re = xbee.SendRemoteATCommand(remote, com, RemoteCommandOptions.ApplyChanges, data);

            if (re == null)
                Console.WriteLine(">> ERROR : Remote AT command response time out\r\n");
            else
            {
                byte[] p = re.GetParameter();
                Address add = re.GetRemoteDevice();
                CommandStatus code = re.GetCommandStatus();
                Console.WriteLine(">> " + code + " From " + add.GetSerialNumberHigh().ToString("X2") + " " + add.GetSerialNumberLow().ToString("X2") + " " + add.GetNetworkAddress().ToString("X2"));

                if (code == CommandStatus.OK && p != null)
                    Console.WriteLine(">> " + re.GetRequestCommand() + " = " + UTF8Encoding.UTF8.GetString(p) + " [" + GetString(p) + "]\r\n");
                else Console.Write("\r\n");
            }

            IOSamples[] samples = ATInterpreter.FromZigBeeIS(re);
            if (samples != null)
            {
                for (int i = 0; i < samples.Length; i++)
                {
                    Console.WriteLine(">> Digitals : ");
                    foreach (DictionaryEntry entry in samples[i].GetDigitals())
                        Console.WriteLine(">> Pin " + ((Pin)entry.Key).NUM + " = " + entry.Value);
                    Console.WriteLine(">> Analogs : ");
                    foreach (DictionaryEntry entry in samples[i].GetAnalogs())
                        Console.WriteLine(">> Pin " + ((Pin)entry.Key).NUM + " = " + entry.Value);
                }
            }
            /*
            ZigBeeDiscoverAddress dd = ZigBeeDiscoverAddress.Parse(re);

            if (dd != null)
            {
                Console.WriteLine(">> Node Discovery : ");
                Console.WriteLine(">> SH " + dd.GetSerialNumberHigh().ToString("X2"));
                Console.WriteLine(">> SL " + dd.GetSerialNumberLow().ToString("X2"));
                Console.WriteLine(">> NET " + dd.GetNetworkAddress().ToString("X2"));
                Console.WriteLine(">> NI " + dd.GetNIString() + "\r\n");
            }*/
        }

        static void S(string[] lines)
        {
            int addressh = int.Parse(lines[1], NumberStyles.HexNumber);
            int addressl = int.Parse(lines[2], NumberStyles.HexNumber);
            int net = int.Parse(lines[3], NumberStyles.HexNumber);
            remote.SetSerialNumberHigh(addressh);
            remote.SetSerialNumberLow(addressl);
            remote.SetNetworkAddress(net);

            string datas = lines[4];
            for (int i = 5; i < lines.Length; i++)
                datas += " " + lines[i];
            byte[] data = GetBytes(datas);

            ZigBeeTxStatusIndicator re = xbee.SendZigBeeTx(remote, OptionsBase.DEFAULT, data);

            if (re == null)
                Console.WriteLine(">> ERROR : ZigBee Tx response time out\r\n");
            else
            {
                Console.WriteLine(">> Packet send to " + re.GetDestinationAddress16().ToString("X2") + " is " + re.GetDeliveryStatus());
                Console.WriteLine(">> " + re.GetDiscoveryStatus() + "\r\n");
            }
        }

        static void SE(string[] lines)
        {
            int addressh = int.Parse(lines[1], NumberStyles.HexNumber);
            int addressl = int.Parse(lines[2], NumberStyles.HexNumber);
            int net = int.Parse(lines[3], NumberStyles.HexNumber);

            byte se = (byte)int.Parse(lines[4], NumberStyles.HexNumber);
            byte de = (byte)int.Parse(lines[5], NumberStyles.HexNumber);
            int cid = int.Parse(lines[6], NumberStyles.HexNumber);
            int pid = int.Parse(lines[7], NumberStyles.HexNumber);
            
            remote.SetSerialNumberHigh(addressh);
            remote.SetSerialNumberLow(addressl);
            remote.SetNetworkAddress(net);

            remote.SetSourceEndpoint(se);
            remote.SetDestinationEndpoint(de);
            remote.SetProfileID(pid);
            remote.SetClusterID(cid);

            string datas = lines[8];
            for (int i = 9; i < lines.Length; i++)
                datas += " " + lines[i];
            byte[] data = GetBytes(datas);

            ZigBeeTxStatusIndicator re = xbee.SendZigBeeExplicitTx(remote, OptionsBase.DEFAULT, data);

            if (re == null)
                Console.WriteLine(">> ERROR : ZigBee Tx response time out\r\n");
            else
            {
                Console.WriteLine(">> Packet send to " + re.GetDestinationAddress16().ToString("X2") + " is " + re.GetDeliveryStatus());
                Console.WriteLine(">> " + re.GetDiscoveryStatus() + "\r\n");
            }
        }

        static void xbee_onZigBeeReceivePacketResponse(SmartLab.XBee.Indicator.ZigBeeRxIndicator e)
        {
            Address add = e.GetRemoteDevice();
            byte[] data = e.GetReceivedData();
            Console.WriteLine("\r\n>> ZigBee Rx : " + e.GetReceiveStatus() + " From " + add.GetSerialNumberHigh().ToString("X2") + " " + add.GetSerialNumberLow().ToString("X2") + " " + add.GetNetworkAddress().ToString("X2"));
            Console.WriteLine(">> " + UTF8Encoding.UTF8.GetString(data) + " [" + GetString(data) + "]\r\n");
        }

        static void xbee_onZigBeeTransmitStatusResponse(SmartLab.XBee.Indicator.ZigBeeTxStatusIndicator e)
        {
            Console.WriteLine("\r\n>> ZigBee Tx : " + e.GetDeliveryStatus() + "\r\n");
        }

        static void xbee_onNodeIdentificationResponse(SmartLab.XBee.Indicator.NodeIdentificationIndicator e)
        {
            Address add = e.GetRemoteDevice();
            Console.WriteLine("\r\n>> node join :" + " From " + add.GetSerialNumberHigh().ToString("X2") + " " + add.GetSerialNumberLow().ToString("X2") + " " + add.GetNetworkAddress().ToString("X2"));
            Console.WriteLine(">> NI : " + e.GetNIString());
            Console.WriteLine(">> Parent : " + e.GetParentNetworkAddress().ToString("X2"));
            Console.WriteLine(">> Event : " + e.GetSourceEvent() + "\r\n");
        }

        static void xbee_onModemStatusResponse(SmartLab.XBee.Indicator.ModemStatusIndicator e)
        {
            Console.WriteLine("\r\n>> modem status " + e.GetModemStatus() + "\r\n");
        }

        static void xbee_onATCommandResponse(ATCommandIndicator e)
        {
            byte[] p = e.GetParameter();
            Console.WriteLine("\r\n>> AT command " + e.GetRequestCommand() + " " + e.GetCommandStatus() + " " + (p == null ? " " : UTF8Encoding.UTF8.GetString(p)) + "\r\n");


            ActiveScanResult result = ATInterpreter.FromAS(e);

            if (result != null)
            {
                Console.WriteLine("\r\n>> Active Scan Result : ");
                Console.WriteLine(">> Channel " + result.Channel.ToString("X2"));
                Console.WriteLine(">> Pan ID [" + GetString(result.PanID) + "]");
                Console.WriteLine(">> Extended Pan ID [" + GetString(result.ExtendedPanID) + "]");
                Console.WriteLine(">> Allow Join " + result.AllowJoin);
                Console.WriteLine(">> StackProfile " + result.StackProfile);
                Console.WriteLine(">> LQI " + result.LQI);
                Console.WriteLine(">> RSSI " + result.RSSI + "\r\n");
            }


        }

        static void xbee_onXBeeTransmitStatusResponse(XBeeTxStatusIndicator e)
        {
            Console.WriteLine("\r\n>> XBee Tx : " + e.GetDeliveryStatus() + "\r\n");
        }

        static void xbee_onXBeeRx64Indicator(XBeeRx64Indicator e)
        {
            Address add = e.GetRemoteDevice();
            byte[] data = e.GetReceivedData();
            Console.WriteLine("\r\n>> XBee Rx 64 : " + e.GetReceiveStatus() + " From " + add.GetSerialNumberHigh().ToString("X2") + " " + add.GetSerialNumberLow().ToString("X2") + " " + add.GetNetworkAddress().ToString("X2") + " RSSI = " + e.GetRSSI());
            Console.WriteLine(">> " + UTF8Encoding.UTF8.GetString(data) + " [" + GetString(data) + "]\r\n");
        }

        static void xbee_onXBeeRx16Indicator(XBeeRx16Indicator e)
        {
            byte[] data = e.GetReceivedData();
            Console.WriteLine("\r\n>> XBee Rx 16 : " + e.GetReceiveStatus() + " From " + e.GetRemoteDevice().GetNetworkAddress().ToString("X2") + " RSSI = " + e.GetRSSI());
            Console.WriteLine(">> " + UTF8Encoding.UTF8.GetString(data) + " [" + GetString(data) + "]\r\n");
        }

        static void xbee_onRemoteCommandResponse(RemoteCommandIndicator e)
        {
            byte[] p = e.GetParameter();
            Console.WriteLine("\r\n>> remote AT command " + e.GetCommandStatus() + " " + e.GetRequestCommand() + " = " + (p == null ? " " : UTF8Encoding.UTF8.GetString(p)) + "\r\n");
        }

        static void xbee_onXBeeIODataSampleRx16Response(XBeeRx16IOSampleIndicator e)
        {
            IOSamples[] samples = e.GetIOSamples();
            for (int i = 0; i < samples.Length; i++)
            {
                Address add = e.GetRemoteDevice();
                Console.WriteLine("\r\n>> XBee Rx16 IO sample : " + e.GetReceiveStatus() + " From " + add.GetNetworkAddress().ToString("X2") + " RSSI = " + e.GetRSSI());
                Console.WriteLine(">> Digitals : ");
                foreach (DictionaryEntry entry in samples[i].GetDigitals())
                    Console.WriteLine(">> Pin " + ((Pin)entry.Key).NUM + " = " + (Pin.Status)entry.Value);
                Console.WriteLine(">> Analogs : ");
                foreach (DictionaryEntry entry in samples[i].GetAnalogs())
                    Console.WriteLine(">> Pin " + ((Pin)entry.Key).NUM + " = " + (int)entry.Value);
            }
        }

        static void xbee_onXBeeIODataSampleRx64Response(XBeeRx64IOSampleIndicator e)
        {
            IOSamples[] samples = e.GetIOSamples();

            for (int i = 0; i < samples.Length; i++)
            {
                Address add = e.GetRemoteDevice();
                Console.WriteLine("\r\n>> XBee Rx64 IO sample : " + e.GetReceiveStatus() + " From " + add.GetSerialNumberHigh().ToString("X2") + " " + " " + add.GetSerialNumberLow().ToString("X2") + " " + add.GetNetworkAddress().ToString("X2") + " RSSI = " + e.GetRSSI());
                Console.WriteLine(">> Digitals : ");
                foreach (DictionaryEntry entry in samples[i].GetDigitals())
                    Console.WriteLine(">> Pin " + ((Pin)entry.Key).NUM + " = " + entry.Value);
                Console.WriteLine(">> Analogs : ");
                foreach (DictionaryEntry entry in samples[i].GetAnalogs())
                    Console.WriteLine(">> Pin " + ((Pin)entry.Key).NUM + " = " + entry.Value);
            }
        }

        static void xbee_onZigBeeIODataSampleRXResponse(ZigBeeIOSampleIndicator e)
        {
            IOSamples[] samples = e.GetIOSamples();
            for (int i = 0; i < samples.Length; i++)
            {
                Address add = e.GetRemoteDevice();
                Console.WriteLine("\r\n>> ZigBee IO sample : " + e.GetReceiveStatus() + " From " + add.GetSerialNumberHigh().ToString("X2") + " " + " " + add.GetSerialNumberLow().ToString("X2") + " " + add.GetNetworkAddress().ToString("X2"));
                Console.WriteLine(">> Digitals : ");
                foreach (DictionaryEntry entry in samples[i].GetDigitals())
                    Console.WriteLine(">> Pin " + ((Pin)entry.Key).NUM + " = " + entry.Value);
                Console.WriteLine(">> Analogs : ");
                foreach (DictionaryEntry entry in samples[i].GetAnalogs())
                    Console.WriteLine(">> Pin " + ((Pin)entry.Key).NUM + " = " + entry.Value);
            }
        }

        static void xbee_onZigBeeExplicitRxResponse(ZigBeeExplicitRxIndicator e)
        {
            ExplicitAddress add = e.GetExplicitRemoteDevice();
            byte[] data = e.GetReceivedData();
            Console.WriteLine("\r\n>> Explicit ZigBee Rx : " + e.GetReceiveStatus() + " From " + add.GetSerialNumberHigh().ToString("X2") + " " + add.GetSerialNumberLow().ToString("X2") + " " + add.GetNetworkAddress().ToString("X2"));
            Console.WriteLine(">> Source Endpoint : " + add.GetSourceEndpoint().ToString("X2"));
            Console.WriteLine(">> Destination Endpoint : " + add.GetDestinationEndpoint().ToString("X2"));
            Console.WriteLine(">> Cluster ID : " + add.GetClusterID().ToString("X2"));
            Console.WriteLine(">> Profile ID : " + add.GetProfileID().ToString("X2"));
            Console.WriteLine(">> " + UTF8Encoding.UTF8.GetString(data) + " [" + GetString(data) + "]\r\n");
        }

        private static byte[] GetBytes(string value)
        {
            if (value.IndexOf("0x") == 0)
            {
                if ((value.Length - 2) % 2 != 0)
                    value = "0" + value.Substring(2, value.Length - 2);
                else value = value.Substring(2, value.Length - 2);

                int l = value.Length / 2;
                byte[] d = new byte[l];
                for (int i = l - 1; i >= 0; i--)
                {
                    d[i] = Convert.ToByte(value.Substring(value.Length - 2 * (l - i), 2), 16);
                }
                return d;
            }
            else return UTF8Encoding.UTF8.GetBytes(value);
        }

        private static string GetString(byte[] bytes)
        {
            string m = string.Empty;
            foreach (byte b in bytes)
                m += b.ToString("X2") + " ";
            return m.TrimEnd();
        }
    }
}