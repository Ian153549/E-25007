using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NSAA_16Axis
{
    public class CPLightSource
    {
        public SerialPort Com = new SerialPort();
        public int LNowBrightness = 0;
        public int RNowBrightness = 0;
        public int LBNowBrightness = 0;
        public int RBNowBrightness = 0;
        public int LightNum = 4;
        private bool isOpen = false;
        private object _locker = new object();
        public string devicename = "";

        // private NSAA_16Axis.FormMain hwndFormMain;
        public CPLightSource(string name)
        {
            devicename = name;
            Com.ReadTimeout = 1000;
            Com.WriteTimeout = 1000;
        }

        public void Open(string portName, int baudRate, int dataBits, Parity parity, StopBits stopBits)
        {
            try
            {
                Com.PortName = portName;
                Com.BaudRate = baudRate;
                Com.DataBits = dataBits;
                Com.Parity = parity;
                Com.StopBits = stopBits;

                if (Com.IsOpen)
                    return;
                Com.Open();
                if (Com.IsOpen == true)
                {
                    if (LightNum == 4)
                    {
                        ChangeBrightness("left", 32);
                        ChangeBrightness("right", 32);
                        ChangeBrightness("leftback", 32);
                        ChangeBrightness("rightback", 32);
                    }
                    else
                    {
                        ChangeBrightness("left", 32);
                        ChangeBrightness("right", 32);
                    }
                }
                isOpen = true;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public void Close()
        {
            if (isOpen)
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
            if (IsOpen())
            {
                if (GV.UserLevel == GV.User.Administrator)
                {
                    GM.WriteToStatusTextBox1(1, "Device = " + devicename + " Side = " + side + " Value = " + brightness.ToString());
                } else
                {
                    GM.WriteToStatusTextBox1(0, "Device = " + devicename + " Side = " + side + " Value = " + brightness.ToString());
                }
                lock (_locker)
                {
                    Thread.Sleep(100);
                    string channel = string.Empty;
                    string sendMsg = string.Empty;
                    if (side == "left") channel = "1";
                    else if (side == "right") channel = "2";
                    else if (side == "leftback") channel = "3";
                    else if (side == "rightback") channel = "4";
                    else throw new Exception("please type the correct side");
                    // byte[] byteBrightness = { (byte)brightness };
                    // string strBrightness = (BitConverter.ToString(byteBrightness));
                    if (!Com.IsOpen) return false;
                    if (brightness < 0 || brightness > 255)
                        return false;
                    sendMsg = "" + channel + "," + brightness.ToString() + "\r\n";

                    try
                    {
                        string resultSendMsg = string.Empty;
                        char[] textBuf = sendMsg.ToCharArray();
                        byte[] byteSendMsg = System.Text.Encoding.UTF8.GetBytes(textBuf);
                        // byte[] byteSendMsg = GenerateLrcAndOutputBuffer(sendMsg, out resultSendMsg);
                        Com.Write(byteSendMsg, 0, byteSendMsg.Length);
                        Thread.Sleep(200);
                        string response;
                        response = Com.ReadLine() + "\n";
                        if (sendMsg != response)
                            throw new Exception("Cannot change brightness.");
                        if (side == "left")
                            LNowBrightness = brightness;
                        else if (side == "right")
                            RNowBrightness = brightness;
                        else if (side == "leftback")
                            LBNowBrightness = brightness;
                        else if (side == "rightback")
                            RBNowBrightness = brightness;
                        return true;
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
            else
            {
                return true;
            }
        }


        public bool ChangeBrightness(int iUpDown, int iRb, int iLb)
        {
            if (IsOpen())
            {
                if (iUpDown == 0)
                {
                    ChangeBrightness("left", iLb);
                    ChangeBrightness("right", iRb);
                }
                else if (iUpDown == 1)
                {
                    ChangeBrightness("leftback", iLb);
                    ChangeBrightness("rightback", iRb);
                }
            }
            return true;
        }
        public int ReadBrightness(string side)
        {

            int brightness = 0;

            if (side == "left")
            {
                brightness = LNowBrightness;
            }
            else if (side == "right")
            {
                brightness = RNowBrightness;
            }
            else if (side == "leftback")
            {
                brightness = LBNowBrightness;
            }
            else if (side == "rightback")
            {
                brightness = RBNowBrightness;
            }
            else throw new Exception("please type the correct side");

            return brightness;

            /*
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
            */
        }


        public int[] ReadAllChannelBrightness()
        {
            int[] brightness = new int[4];

            brightness[0] = LNowBrightness;
            brightness[1] = RNowBrightness;
            brightness[2] = LBNowBrightness;
            brightness[3] = RBNowBrightness;

            return brightness;

            /*
            if (Com.IsOpen)
            {   
                
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
            } else
            {
                for (int i = 0; i < 4; i++)
                {
                    brightness[i] = 0;
                }
            }
            */
        }
    }

    public class NCPLightSource
    {
        public SerialPort Com = new SerialPort();
        public int LNowBrightness = 0;
        public int RNowBrightness = 0;
        public int LBNowBrightness = 0;
        public int RBNowBrightness = 0;
        public int LightNum = 4;
        private bool isOpen = false;
        private object _locker = new object();
        public string devicename = "";

        public NCPLightSource(string name)
        {
            devicename = name;
            Com.ReadTimeout = 2000;
            Com.WriteTimeout = 2000;
        }

        public void Open(string portName, int baudRate, int dataBits, Parity parity, StopBits stopBits, int lightN)
        {

            try
            {
                Com.PortName = portName;
                Com.BaudRate = baudRate;
                Com.DataBits = dataBits;
                Com.Parity = parity;
                Com.StopBits = stopBits;
                LightNum = lightN;

                if (Com.IsOpen)
                    return;
                Com.Open();
                if (Com.IsOpen == true)
                {

                    Thread.Sleep(500);
                    //ChangeBrightness("LHigh", GV.AppSettingParm.LightDefault);
                    Thread.Sleep(500);
                    //ChangeBrightness("RHigh", GV.AppSettingParm.LightDefault);
                    Thread.Sleep(500);

                }
                isOpen = true;
            }
            catch (Exception)
            {
                throw;
            }
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

        public bool ChangeBrightness(string side, int brightness)
        {
            if (isOpen)
            {
                lock (_locker)
                {
                    Thread.Sleep(100);
                    string channel = string.Empty;
                    byte Echo;

                    string sendMsg = string.Empty;
                    if ((side == "left") || (side == "LHigh"))
                    {
                        channel = "A";
                        Echo = 65;
                    }
                    else if ((side == "right") || (side == "RHigh"))
                    {
                        channel = "B";
                        Echo = 66;
                    }
                    else if ((side == "leftback") || (side == "LLow"))
                    {
                        if (LightNum == 2) return true;
                        channel = "C";
                        Echo = 67;
                    }
                    else if ((side == "rightback") || (side == "RLow"))
                    {
                        if (LightNum == 2) return true;
                        channel = "D";
                        Echo = 68;
                    }
                    else throw new Exception("please type the correct side");

                    if (!Com.IsOpen) return false;
                    if (brightness < 0 || brightness > 255)
                        return false;
                    sendMsg = "S" + channel + brightness.ToString("D4") + "#";

                    try
                    {
                        string resultSendMsg = string.Empty;
                        char[] textBuf = sendMsg.ToCharArray();
                        byte[] byteSendMsg = System.Text.Encoding.UTF8.GetBytes(textBuf);

                        Com.Write(byteSendMsg, 0, byteSendMsg.Length);
                        Thread.Sleep(200);
                        byte[] ReadMessage = new byte[128];

                        int readCount;

                        readCount = Com.Read(ReadMessage, 0, 128);
                        if (ReadMessage[0] != Echo)
                            throw new Exception("Cannot change brightness.");
                        if ((side == "left") || (side == "LHigh"))
                            LNowBrightness = brightness;
                        else if ((side == "right") || (side == "RHigh"))
                            RNowBrightness = brightness;
                        else if ((side == "leftback") || (side == "LLow"))
                            LBNowBrightness = brightness;
                        else if ((side == "rightback") || (side == "RLow"))
                            RBNowBrightness = brightness;
                        return true;
                    }
                    catch
                    {
                        //throw;
                        return false;
                    }
                }
            }
            else
            {
                return true;
            }
        }

        public bool ChangeBrightness(int iUpDown, int iRb, int iLb)
        {
            if (IsOpen())
            {
                if (iUpDown == 0)
                {
                    ChangeBrightness("left", iLb);
                    ChangeBrightness("right", iRb);
                }
                else if (iUpDown == 1)
                {
                    ChangeBrightness("leftback", iLb);
                    ChangeBrightness("rightback", iRb);
                }
            }
            return true;
        }

        public int[] ReadAllChannelBrightness()
        {
            int[] brightness = new int[4];

            brightness[0] = LNowBrightness;
            brightness[1] = RNowBrightness;
            brightness[2] = LBNowBrightness;
            brightness[3] = RBNowBrightness;

            return brightness;
        }
    }
}
