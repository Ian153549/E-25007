using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MRVisionLib
{
    class ThreeAMLightSource
    {
        public SerialPort Com = new SerialPort();
        private byte DeviceNumber = 0x03;
        private byte DeviceID = 0xFF;
        public int LNowBrightness = 0;
        public int RNowBrightness = 0;
        public int LBNowBrightness = 0;
        public int RBNowBrightness = 0;
        private object _locker = new object();
        public ThreeAMLightSource()
        {
            Com.ReadTimeout = 1000;
            Com.WriteTimeout = 1000;
        }

        public void Open(byte deviceNumber, byte deviceID, string portName, int baudRate, int dataBits, Parity parity, StopBits stopBits)
        {
            DeviceNumber = deviceNumber;
            DeviceID = deviceID;
            Com.PortName = portName;
            Com.BaudRate = baudRate;
            Com.DataBits = dataBits;
            Com.Parity = parity;
            Com.StopBits = stopBits;

            if (Com.IsOpen)
                return;
            Com.Open();
        }

        public void Close()
        {
            Com.Close();
        }
        public bool IsOpen()
        {
            if (Com.IsOpen)
                return true;
            else
                return false;
        }

        public byte[] GenerateLrcAndOutputBuffer(string sendMsg, out string resultSendMsg)
        {
            int intLrc = 0;
            for (int i = 0; i < sendMsg.Length; i += 2)
            {
                string part = sendMsg.Substring(i, 2);
                byte newByte = byte.Parse(part, System.Globalization.NumberStyles.HexNumber);
                intLrc += newByte;
            }
            intLrc = ~intLrc + 1;
            byte[] byteLrc = { (byte)intLrc };
            sendMsg += BitConverter.ToString(byteLrc).Replace("-", "") + "\r\n";
            sendMsg = sendMsg.Insert(0, ":");
            resultSendMsg = sendMsg;
            return Encoding.ASCII.GetBytes(sendMsg);
        }
        
        public bool ChangeBrightness(string side, int brightness)
        {
            lock (_locker)
            {
                Thread.Sleep(100);
                string channel = string.Empty;
                string sendMsg = string.Empty;
                if (side == "left") channel = "0001";
                else if (side == "right") channel = "0002";
                else if (side == "leftback") channel = "0003";
                else if (side == "rightback") channel = "0004";
                else throw new Exception("please type the correct side");
                byte[] byteBrightness = { (byte)brightness };
                string strBrightness = (BitConverter.ToString(byteBrightness));
                if (!Com.IsOpen) return false;
                if (brightness < 0 || brightness > 255)
                    return false;
                sendMsg = "0106" + channel + "00" + strBrightness;

                try
                {
                    string resultSendMsg = string.Empty;
                    byte[] byteSendMsg = GenerateLrcAndOutputBuffer(sendMsg, out resultSendMsg);
                    Com.Write(byteSendMsg, 0, byteSendMsg.Length);
                    Thread.Sleep(200);
                    string response;
                    response = Com.ReadLine() + "\n";
                    if (resultSendMsg != response)
                        throw new Exception("Cannot change brightness.");
                    if (side == "left")
                        LNowBrightness = brightness;
                    if (side == "right")
                        RNowBrightness = brightness;
                    if (side == "leftback")
                        LBNowBrightness = brightness;
                    if (side == "rightback")
                        RBNowBrightness = brightness;
                    return true;
                }
                catch
                {
                    throw;
                }
            }
        }
        
        public int ReadBrightness(string side)
        {
            lock (_locker)
            {
                string channel = string.Empty;
                string sendMsg = string.Empty;
                if (side == "left")
                    channel = "0001";
                else if (side == "right")
                    channel = "0002";
                else if (side == "leftback")
                    channel = "0003";
                else if (side == "rightback")
                    channel = "0004";
                else throw new Exception("please type the correct side");

                sendMsg = "0103" + channel + "0001";

                try
                {
                    string resultSendMsg = string.Empty;
                    byte[] byteSendMsg = GenerateLrcAndOutputBuffer(sendMsg, out resultSendMsg);
                    Com.Write(byteSendMsg, 0, byteSendMsg.Length);
                    Thread.Sleep(200);
                    string response;
                    response = Com.ReadLine();
                    int brightness = Int32.Parse(response.Substring(9, 2), System.Globalization.NumberStyles.HexNumber);
                    if (side == "left")
                        LNowBrightness = brightness;
                    if (side == "right")
                        RNowBrightness = brightness;
                    if (side == "leftback")
                        LBNowBrightness = brightness;
                    if (side == "rightback")
                        RBNowBrightness = brightness;
                    if ( response.Substring(0,7) != ":010302")
                        throw new Exception("Cannot read brightness.");
                    return brightness;
                }
                catch
                {
                    throw;
                }
            }
        }


        public int[] ReadAllChannelBrightness()
        {
            int[] brightness = new int[4];
            lock (_locker)
            {

                string channel = string.Empty;
                string sendMsg = string.Empty;
                sendMsg = "010300010004";

                try
                {
                    string resultSendMsg = string.Empty;
                    byte[] byteSendMsg = GenerateLrcAndOutputBuffer(sendMsg, out resultSendMsg);
                    Com.Write(byteSendMsg, 0, byteSendMsg.Length);
                    Thread.Sleep(200);
                    string response;
                    response = Com.ReadLine();
                    LNowBrightness = brightness[0] = Int32.Parse(response.Substring(9, 2), System.Globalization.NumberStyles.HexNumber);
                    RNowBrightness = brightness[1] = Int32.Parse(response.Substring(13, 2), System.Globalization.NumberStyles.HexNumber);
                    LBNowBrightness = brightness[2] = Int32.Parse(response.Substring(17, 2), System.Globalization.NumberStyles.HexNumber);
                    RBNowBrightness = brightness[3] = Int32.Parse(response.Substring(21, 2), System.Globalization.NumberStyles.HexNumber);
                    if (response.Substring(0, 7) != ":010308")
                        throw new Exception("Cannot read all brightness.");
                    return brightness;
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}
