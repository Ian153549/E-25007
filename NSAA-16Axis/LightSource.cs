using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SemiAutomaticAligner
{
    class LightSource
    {
        private SerialPort Com = new SerialPort();
        private byte DeviceNumber = 0x03;
        //private byte DeviceID = 0x00;
        private byte DeviceID = 0xFF;
        public int LNowBrightness = 0;
        public int RNowBrightness = 0;
        object Locker = new object();
        public LightSource()
        {
            
        }

        public void Open(byte deviceNumber, byte deviceID, string portName, int baudRate, int dataBits, Parity parity, StopBits stopBits)
        {
            //{start-0x40, length, device number-0x03, device id-0x00, command[3], checksum}
            //change brightness command = {0x1A, channel 0x00-0x03, brightness to set 0-255}
            //read brightness command = {0x31, channel 0x00-0x03}
            //change device id command {0x09, device id 0-99}
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

        public bool ChangeBrightness(string side, int brightness)
        {
            lock(Locker)
            {
                Thread.Sleep(100);
                byte channel;
                if (side == "left") channel = 2;
                else if (side == "right") channel = 3;
                else throw new MRException("please type the correct side");

                if (!Com.IsOpen) return false;

                if (brightness < 0 || brightness > 255)
                    return false;

                List<Byte> sendMsgList = new List<byte>();
                byte[] sendMsg = { 0x40, 0x05, DeviceNumber, DeviceID, 0x1A, channel, Convert.ToByte(brightness) };
                sendMsgList.AddRange(sendMsg);

                try
                {
                    Com.Write(GenerateCheckSumAndOutputBuffer(sendMsgList), 0, sendMsgList.Count);
                    if (ComRead(5000)[0] == 0x40)
                    {
                        if (side == "left") LNowBrightness = brightness;
                        if (side == "right") RNowBrightness = brightness;
                        return true;
                    }
                        
                    else
                        throw new MRException("Cannot change brightness.");
                }
                catch
                {
                    throw;
                }
            }
        }

        public int ReadBrightness(string side)
        {
            lock (Locker)
            {
                byte channel = new byte();
                if (side == "left")
                    channel = 2;
                else if (side == "right")
                    channel = 3;
                else throw new MRException("please type the correct side");

                Thread.Sleep(100);
                List<Byte> sendMsgList = new List<byte>();
                byte[] sendMsg = { 0x40, 0x04, DeviceNumber, DeviceID, 0x31, channel };
                sendMsgList.AddRange(sendMsg);

                try
                {
                    Com.Write(GenerateCheckSumAndOutputBuffer(sendMsgList), 0, sendMsgList.Count);
                    byte[] recieveMsg = ComRead(5000);
                    if (recieveMsg[0] == 0x40)
                    {
                        if (side == "left") LNowBrightness = recieveMsg[5];
                        if (side == "right") RNowBrightness = recieveMsg[5];
                        return recieveMsg[5];
                    }
                        
                    else
                        throw new MRException("Cannot read brightness.");
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
            lock (Locker)
            {

                Thread.Sleep(100);
                List<Byte> sendMsgList = new List<byte>();
                byte[] sendMsg = { 0x40, 0x04, DeviceNumber, DeviceID, 0x31, 0xFF };
                sendMsgList.AddRange(sendMsg);

                try
                {
                    Com.Write(GenerateCheckSumAndOutputBuffer(sendMsgList), 0, sendMsgList.Count);
                    byte[] recieveMsg = ComRead(5000);
                    if (recieveMsg[0] == 0x40 && recieveMsg.Length > 9)
                    {
                        brightness[0] = recieveMsg[5];
                        brightness[1] = recieveMsg[6];
                        LNowBrightness = brightness[2] = recieveMsg[7];
                        RNowBrightness = brightness[3] = recieveMsg[8];
                        return brightness;
                    }
                    else
                        throw new MRException("Read lens light brightness fail.");
                }
                catch
                {
                    throw;
                }
            }


        }

        public bool ChangeHardwareDeviceID(byte deviceIdToCahnge)
        {
            Thread.Sleep(100);
            if (deviceIdToCahnge < 0 || deviceIdToCahnge > 99)
                return false;

            List<Byte> sendMsgList = new List<byte>();
            byte[] sendMsg = { 0x40, 0x04, DeviceNumber, DeviceID, 0x09, deviceIdToCahnge };
            sendMsgList.AddRange(sendMsg);

            try
            {
                Com.Write(GenerateCheckSumAndOutputBuffer(sendMsgList), 0, sendMsgList.Count);
                byte[] recieveMsg = ComRead(5000);
                if (recieveMsg[0] == 0x40)
                    return true;
                else
                    throw new MRException("Cannot cahange deviceID");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        private byte[] GenerateCheckSumAndOutputBuffer(List<byte> msgList)
        {
            byte checkSum = 0;
            foreach (byte value in msgList)
                checkSum += value;
            msgList.Add(checkSum);

            byte[] buffer = new byte[msgList.Count];
            msgList.CopyTo(buffer);

            return buffer;
        }

        private byte[] ComRead(int timeout)
        {
            Stopwatch readTimeout = Stopwatch.StartNew();
            List<byte> recieveMsgList = new List<byte>();
            while(true)
            {
                byte[] recieveMsg = ReadImcompleteMsg(timeout);
                if (!CheckIfMsgCorrect(recieveMsg))
                    for (int i = 0; i < recieveMsg.Length; i++)
                        recieveMsgList.Add(recieveMsg[i]);
                else
                    return recieveMsg;

                if (CheckIfMsgCorrect(recieveMsgList))
                    break;
            }
            return recieveMsgList.ToArray();
            
        }

        private byte[] ReadImcompleteMsg(int timeout)
        {
            Stopwatch readTimeout = Stopwatch.StartNew();
            while (true)
            {
                if (Com.BytesToRead > 0)
                    break;
                if (readTimeout.ElapsedMilliseconds > timeout)
                    throw new MRException("Read lens light brightness timeout");
                Thread.Sleep(200);
            }
            int count = Com.BytesToRead;
            byte[] recieveMsg = new byte[count];
            Com.Read(recieveMsg, 0, count);

            return recieveMsg;
        }




        private bool CheckIfMsgCorrect(byte[] msg)
        {
            byte checkSum = 0;
            for (int i = 0; i < msg.Length - 1; i++)
                checkSum += msg[i];
            if (checkSum == msg[msg.Length - 1]) return true;
            else return false;
        }
        private bool CheckIfMsgCorrect(List<byte> msg)
        {
            byte checkSum = 0;
            for (int i = 0; i < msg.Count - 1; i++)
                checkSum += msg[i];
            if (checkSum == msg[msg.Count - 1]) return true;
            else return false;
        }
    }
}
