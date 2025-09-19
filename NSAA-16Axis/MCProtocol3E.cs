using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace NSAA_16Axis
{
    public class MCProtocolExceptionConnetionLost : Exception
    {
        public MCProtocolExceptionConnetionLost(string msg) : base(msg)
        {

        }
    }

    public class MCProtocol3E
    {
        string RemoteHost;
        int Port;

        TcpClient client;
        NetworkStream stream;
        private static Object LockObj = new object();
        public bool IsOpen = false;


        static MCProtocol3E Instance;

        byte[] PacketHeader = { 
                0x50,0x00,  // 3E Binary Frame
                0x00,       //Network No.
                0xff,       //PC No.
                0xff,0x03,  //IO No
                0x00,       //Station No
                0x0c,0x00,  //Length Low   length of the request data which is started from timer byte to end 
                0x0a,       //Timer Low
                0x00        //Timer High
                };
        
        public MCProtocol3E(string remoteHost, int port)
        {
            //RemoteHost = "192.168.3.39";
            //Port = 8500;
            RemoteHost = remoteHost;
            Port = port;
            Instance = this;
        }

        public static MCProtocol3E Inst
        {
            get
            {
                if (Instance == null) throw new Exception("MCProtocol3E is not initialized.");
                return Instance;
            }
        }

        public void Open(int timeOut = 2000)
        {
            client = new TcpClient();
            var result = client.BeginConnect(RemoteHost, this.Port, null, null);
            var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(timeOut));

            if (!success) throw new Exception("MCProtocol3E: Failed to connect.");
            
            client.EndConnect(result);
            client.ReceiveTimeout = 1000;
            client.SendTimeout = 2000;
            stream = client.GetStream();
            IsOpen = true;
        }

        void Open()
        {
            if (IsOpen) return;
            client = new TcpClient(RemoteHost, Port);
            client.ReceiveTimeout = 2000;
            client.SendTimeout = 2000;
            stream = client.GetStream();
            IsOpen = true;

        }

        private void Application_ApplicationExit(object sender, EventArgs e)
        {
            //Close();
        }

        public void Close()
        {
            if (stream != null)
            {
                stream.Close();
                stream.Dispose();
            }
            if (client != null)
            {
                client.Close();
                client.Dispose();
            }
            IsOpen = false;
        }

        void ReConnect()
        {
            Close();
            client = new TcpClient(RemoteHost, Port);
            client.ReceiveTimeout = 2000;
            client.SendTimeout = 2000;
            stream = client.GetStream();
            IsOpen = true;
        }

        public void QueryReconnect()
        {
            //IsOpen = false;
            //if (MessageBox.Show("Network connection is lost. Try to re-connect?", "Network Connection", MessageBoxButtons.RetryCancel, MessageBoxIcon.Stop)
            //            == DialogResult.Retry)
            ReConnect();
            //else
            //    Application.Exit();
        }

        byte[] ReadNetwork(NetworkStream stm, int expectedBytes, int timeout)
        {
            byte[] data = new byte[expectedBytes];

            List<byte> list = new List<byte>();
            var bytes = 0;
            int bytesLeft = expectedBytes;
            Stopwatch sw = Stopwatch.StartNew();
            do
            {
                try
                {
                    bytes = stm.Read(data, 0, bytesLeft);
                    if (bytes > 0)
                    {
                        byte[] rbytes = new byte[bytes];
                        Array.Copy(data, rbytes, bytes);
                        list.AddRange(rbytes);
                        bytesLeft -= bytes;
                    }
                }
                catch (IOException ex)
                {
                    // if the ReceiveTimeout is reached an IOException will be raised...
                    // with an InnerException of type SocketException and ErrorCode 10060
                    var socketExept = ex.InnerException as SocketException;
                    if (socketExept == null || socketExept.ErrorCode != 10060)
                        // if it's not the "expected" exception, let's not hide the error
                        throw new MCProtocolExceptionConnetionLost("MCProtocol Error: " + ex.Message);
                    // if it is the receive timeout, then reading ended
                    bytes = 0;
                }
            } while (bytesLeft > 0 && sw.ElapsedMilliseconds < timeout);
            

            if (list.Count != expectedBytes) throw new MCProtocolExceptionConnetionLost("MCProtocol Error: Network read error.");
            return list.ToArray();
        }
        
        public bool ReadMemory(int address)
        {
            byte[] Data = { 0x01, 0x04, // Read
                           0x01, 0x00, // Bit
                           0x00, 0x00, 0x00, // Address, low to high
                           0x90, // M*
                           0x01, 0x00 // Units low to high
                          };
            byte[] intBytes = BitConverter.GetBytes(address);
            Data[4] = intBytes[0];
            Data[5] = intBytes[1];
            Data[6] = intBytes[2];

            byte[] Cmd = PacketHeader.Concat(Data).ToArray();

            int len = Cmd.Length - 9;
            intBytes = BitConverter.GetBytes(len);
            Cmd[7] = intBytes[0];
            Cmd[8] = intBytes[1];

            lock (LockObj)
            {
                try
                {
                    stream.Write(Cmd, 0, Cmd.Length);
                    //byte[] data = new Byte[256];
                    //Int32 bytes = stream.Read(data, 0, data.Length);
                    
                    byte[] data = ReadNetwork(stream, 1 * 1 + 11, 2000);
                    return (data[11] & 0xF0) != 0;
                }
                catch (Exception)
                {
                    QueryReconnect();
                    return false;
                }
                finally
                {
                    
                }
            }
        }

        public bool[] ReadMemory(int address, int count)
        {
            byte[] Data = { 0x01, 0x04, // Read
                           0x01, 0x00, // Bit
                           0x00, 0x00, 0x00, // Address, low to high
                           0x90, // M*
                           0x01, 0x00 // Units low to high
                          };
            byte[] intBytes = BitConverter.GetBytes(address);
            Data[4] = intBytes[0];
            Data[5] = intBytes[1];
            Data[6] = intBytes[2];
            byte[] byteCount = BitConverter.GetBytes(count);
            Data[8] = byteCount[0];
            Data[9] = byteCount[1];

            byte[] Cmd = PacketHeader.Concat(Data).ToArray();
            int len = Cmd.Length - 9;
            intBytes = BitConverter.GetBytes(len);
            Cmd[7] = intBytes[0];
            Cmd[8] = intBytes[1];

            lock (LockObj)
            {
                try
                {
                    stream.Write(Cmd, 0, Cmd.Length);
                    int returnedBytes = (count + 1) / 2;
                    byte[] data = ReadNetwork(stream, returnedBytes + 11, 2000);
                    bool[] ret = new bool[count];
                    for (int i = 0; i < count; i++)
                    {
                        byte b = data[11 + i / 2];
                        if ((i % 2) == 0)
                            b = (byte)(b & 0xF0);
                        else
                            b = (byte)(b & 0x0F);
                        ret[i] = b != 0;
                    }

                    return ret;
                }
                catch (Exception)
                {
                    QueryReconnect();
                    return new bool[0];
                }
                finally
                {

                }
            }
        }

        public bool[] ReadX(int address, int count)
        {
            byte[] Data = { 0x01, 0x04, // Read
                           0x01, 0x00, // Bit
                           0x00, 0x00, 0x00, // Address, low to high
                           0x9C, // X*
                           0x01, 0x00 // Units low to high
                          };
            byte[] intBytes = BitConverter.GetBytes(address);
            Data[4] = intBytes[0];
            Data[5] = intBytes[1];
            Data[6] = intBytes[2];
            byte[] byteCount = BitConverter.GetBytes(count);
            Data[8] = byteCount[0];
            Data[9] = byteCount[1];

            byte[] Cmd = PacketHeader.Concat(Data).ToArray();
            int len = Cmd.Length - 9;
            intBytes = BitConverter.GetBytes(len);
            Cmd[7] = intBytes[0];
            Cmd[8] = intBytes[1];

            lock (LockObj)
            {
                try
                {
                    stream.Write(Cmd, 0, Cmd.Length);
                    int returnedBytes = (count + 1) / 2;
                    byte[] data = ReadNetwork(stream, returnedBytes + 11, 2000);
                    bool[] ret = new bool[count];
                    for (int i = 0; i < count; i++)
                    {
                        byte b = data[11 + i / 2];
                        if ((i % 2) == 0)
                            b = (byte)(b & 0xF0);
                        else
                            b = (byte)(b & 0x0F);
                        ret[i] = b != 0;
                    }

                    return ret;
                }
                catch (Exception)
                {
                    QueryReconnect();
                    return new bool[0];
                }
                finally
                {

                }
            }
        }

        public bool[] ReadY(int address, int count)
        {
            byte[] Data = { 0x01, 0x04, // Read
                           0x01, 0x00, // Bit
                           0x00, 0x00, 0x00, // Address, low to high
                           0x9D, // Y*
                           0x01, 0x00 // Units low to high
                          };
            byte[] intBytes = BitConverter.GetBytes(address);
            Data[4] = intBytes[0];
            Data[5] = intBytes[1];
            Data[6] = intBytes[2];
            byte[] byteCount = BitConverter.GetBytes(count);
            Data[8] = byteCount[0];
            Data[9] = byteCount[1];

            byte[] Cmd = PacketHeader.Concat(Data).ToArray();
            int len = Cmd.Length - 9;
            intBytes = BitConverter.GetBytes(len);
            Cmd[7] = intBytes[0];
            Cmd[8] = intBytes[1];

            lock (LockObj)
            {
                try
                {
                    stream.Write(Cmd, 0, Cmd.Length);
                    int returnedBytes = (count + 1) / 2;
                    byte[] data = ReadNetwork(stream, returnedBytes + 11, 2000);
                    bool[] ret = new bool[count];
                    for (int i = 0; i < count; i++)
                    {
                        byte b = data[11 + i / 2];
                        if ((i % 2) == 0)
                            b = (byte)(b & 0xF0);
                        else
                            b = (byte)(b & 0x0F);
                        ret[i] = b != 0;
                    }

                    return ret;
                }
                catch (Exception)
                {
                    QueryReconnect();
                    return new bool[0];
                }
                finally
                {

                }
            }
        }

        public void WriteMemory(int address, bool value)
        {
            byte[] Data = { 0x01, 0x14, // Write
                           0x01, 0x00, // Bit
                           0x00, 0x00, 0x00, // Address, low to high
                           0x90, // M*
                           0x01, 0x00, // Units low to high
                           0x00,       // Value
                          };
            byte[] intBytes = BitConverter.GetBytes(address);
            Data[4] = intBytes[0];
            Data[5] = intBytes[1];
            Data[6] = intBytes[2];

            byte[] Cmd = PacketHeader.Concat(Data).ToArray();

            int len = Cmd.Length - 9;
            intBytes = BitConverter.GetBytes(len);
            Cmd[7] = intBytes[0];
            Cmd[8] = intBytes[1];

            Cmd[Cmd.Length - 1] = (byte)(value ? 0x10 : 0x00);

            lock (LockObj)
            {
                try
                {
                    stream.Write(Cmd, 0, Cmd.Length);
                    //byte[] data = new Byte[256];
                    //Int32 bytes = stream.Read(data, 0, data.Length);
                    ReadNetwork(stream, 11, 2000);
                }
                catch (Exception)
                {
                    QueryReconnect();
                }
                finally
                {

                }
            }
        }

        public void WriteY(int address, bool value)
        {
            byte[] Data = { 0x01, 0x14, // Write
                           0x01, 0x00, // Bit
                           0x00, 0x00, 0x00, // Address, low to high
                           0x9D, // Y*
                           0x01, 0x00, // Units low to high
                           0x00,       // Value
                          };
            byte[] intBytes = BitConverter.GetBytes(address);
            Data[4] = intBytes[0];
            Data[5] = intBytes[1];
            Data[6] = intBytes[2];

            byte[] Cmd = PacketHeader.Concat(Data).ToArray();

            int len = Cmd.Length - 9;
            intBytes = BitConverter.GetBytes(len);
            Cmd[7] = intBytes[0];
            Cmd[8] = intBytes[1];

            Cmd[Cmd.Length - 1] = (byte)(value ? 0x10 : 0x00);

            lock (LockObj)
            {
                try
                {
                    stream.Write(Cmd, 0, Cmd.Length);
                    //byte[] data = new Byte[256];
                    //Int32 bytes = stream.Read(data, 0, data.Length);
                    ReadNetwork(stream, 11, 2000);
                }
                catch (Exception)
                {
                    QueryReconnect();
                }
                finally
                {

                }
            }
        }

        public bool ReadLatch(int address)
        {
            byte[] Data = { 0x01, 0x04, // Read
                           0x01, 0x00, // Bit
                           0x00, 0x00, 0x00, // Address, low to high
                           0x92, // L*
                           0x01, 0x00 // Units low to high
                          };
            byte[] intBytes = BitConverter.GetBytes(address);
            Data[4] = intBytes[0];
            Data[5] = intBytes[1];
            Data[6] = intBytes[2];

            byte[] Cmd = PacketHeader.Concat(Data).ToArray();

            int len = Cmd.Length - 9;
            intBytes = BitConverter.GetBytes(len);
            Cmd[7] = intBytes[0];
            Cmd[8] = intBytes[1];

            lock (LockObj)
            {
                try
                {
                    stream.Write(Cmd, 0, Cmd.Length);
                    //byte[] data = new Byte[256];
                    //Int32 bytes = stream.Read(data, 0, data.Length);

                    byte[] data = ReadNetwork(stream, 1 * 1 + 11, 2000);
                    return (data[11] & 0xF0) != 0;
                }
                catch (Exception)
                {
                    QueryReconnect();
                    return false;
                }
                finally
                {

                }
            }
        }

        public bool[] ReadLatch(int address, int count)
        {
            byte[] Data = { 0x01, 0x04, // Read
                           0x01, 0x00, // Bit
                           0x00, 0x00, 0x00, // Address, low to high
                           0x92, // L*
                           0x01, 0x00 // Units low to high
                          };
            byte[] intBytes = BitConverter.GetBytes(address);
            Data[4] = intBytes[0];
            Data[5] = intBytes[1];
            Data[6] = intBytes[2];
            byte[] byteCount = BitConverter.GetBytes(count);
            Data[8] = byteCount[0];
            Data[9] = byteCount[1];

            byte[] Cmd = PacketHeader.Concat(Data).ToArray();
            int len = Cmd.Length - 9;
            intBytes = BitConverter.GetBytes(len);
            Cmd[7] = intBytes[0];
            Cmd[8] = intBytes[1];

            lock (LockObj)
            {
                try
                {
                    stream.Write(Cmd, 0, Cmd.Length);
                    int returnedBytes = (count + 1) / 2;
                    byte[] data = ReadNetwork(stream, returnedBytes + 11, 2000);
                    bool[] ret = new bool[count];
                    for (int i = 0; i < count; i++)
                    {
                        byte b = data[11 + i / 2];
                        if ((i % 2) == 0)
                            b = (byte)(b & 0xF0);
                        else
                            b = (byte)(b & 0x0F);
                        ret[i] = b != 0;
                    }

                    return ret;
                }
                catch (Exception)
                {
                    QueryReconnect();
                    return new bool[0];
                }
                finally
                {

                }
            }
        }

        public void WriteLatch(int address, bool value)
        {
            byte[] Data = { 0x01, 0x14, // Write
                           0x01, 0x00, // Bit
                           0x00, 0x00, 0x00, // Address, low to high
                           0x92, // L*
                           0x01, 0x00, // Units low to high
                           0x00,       // Value
                          };
            byte[] intBytes = BitConverter.GetBytes(address);
            Data[4] = intBytes[0];
            Data[5] = intBytes[1];
            Data[6] = intBytes[2];

            byte[] Cmd = PacketHeader.Concat(Data).ToArray();

            int len = Cmd.Length - 9;
            intBytes = BitConverter.GetBytes(len);
            Cmd[7] = intBytes[0];
            Cmd[8] = intBytes[1];

            Cmd[Cmd.Length - 1] = (byte)(value ? 0x10 : 0x00);

            lock (LockObj)
            {
                try
                {
                    stream.Write(Cmd, 0, Cmd.Length);
                    //byte[] data = new Byte[256];
                    //Int32 bytes = stream.Read(data, 0, data.Length);
                    ReadNetwork(stream, 11, 2000);
                }
                catch (Exception)
                {
                    QueryReconnect();
                }
                finally
                {

                }
            }
        }

        public bool ReadDataBit(int address, int bit)
        {
            int d = ReadData16(address);
            int mask = 0x01;
            mask = mask << bit;
            return (d & mask) != 0;
        }

        public void WriteDataBit(int address, int bit, bool value)
        {
            int d = ReadData16(address);
            int mask = 0x01;
            mask = mask << bit;
            if (value)
                d = d | mask;
            else
            {
                mask = ~mask;
                d = d & mask;
            }
            WriteData16(address, d);
        }

        public int ReadData16(int address)
        {
            byte[] Data = { 0x01, 0x04, // Read
                           0x00, 0x00, // Word
                           0x00, 0x00, 0x00, // Address, low to high
                           0xA8, // D*
                           0x01, 0x00 // Units low to high
                          };
            byte[] intBytes = BitConverter.GetBytes(address);
            Data[4] = intBytes[0];
            Data[5] = intBytes[1];
            Data[6] = intBytes[2];

            byte[] Cmd = PacketHeader.Concat(Data).ToArray();

            int len = Cmd.Length - 9;
            intBytes = BitConverter.GetBytes(len);
            Cmd[7] = intBytes[0];
            Cmd[8] = intBytes[1];

            lock (LockObj)
            {
                try
                {
                    stream.Write(Cmd, 0, Cmd.Length);
                    //byte[] data = new Byte[256];
                    //Int32 bytes = stream.Read(data, 0, data.Length); // 11 + 2
                    byte[] data = ReadNetwork(stream, 1 * 2 + 11, 2000);
                    return BitConverter.ToInt16(data, 11); 
                }
                catch (Exception)
                {
                    QueryReconnect();
                    return -1;
                }
                finally
                {
                    
                }
            }
        }

        public int[] ReadData16(int address, int count)
        {
            byte[] Data = { 0x01, 0x04, // Read
                           0x00, 0x00, // Word
                           0x00, 0x00, 0x00, // Address, low to high
                           0xA8, // D*
                           0x01, 0x00 // Units low to high
                          };
            byte[] intBytes = BitConverter.GetBytes(address);
            Data[4] = intBytes[0];
            Data[5] = intBytes[1];
            Data[6] = intBytes[2];
            byte[] byteCount = BitConverter.GetBytes(count);
            Data[8] = byteCount[0];
            Data[9] = byteCount[1];

            byte[] Cmd = PacketHeader.Concat(Data).ToArray();

            int len = Cmd.Length - 9;
            intBytes = BitConverter.GetBytes(len);
            Cmd[7] = intBytes[0];
            Cmd[8] = intBytes[1];

            lock (LockObj)
            {
                try
                {
                    stream.Write(Cmd, 0, Cmd.Length);
                    //byte[] data = new Byte[256 + count * 2];
                    //Int32 bytes = stream.Read(data, 0, data.Length);  
                    // len = 11 + count*2
                    byte[] data = ReadNetwork(stream, count * 2 + 11, 2000);
                    int[] val = new int[count];
                    for (int i = 0; i < count; i++) val[i] = BitConverter.ToInt16(data, i * 2 + 11);
                    return val;
                    //return data[bytes - 2] + data[bytes - 1] * 256;
                }
                catch (Exception)
                {
                    QueryReconnect();
                    return new int[0];
                }
                finally
                {

                }
            }
        }

        public int[] ReadDataZR16(int address, int count)
        {
            byte[] Data = { 0x01, 0x04, // Read
                           0x00, 0x00, // Word
                           0x00, 0x00, 0x00, // Address, low to high
                           0xB0, // ZR*
                           0x01, 0x00 // Units low to high
                          };
            byte[] intBytes = BitConverter.GetBytes(address);
            Data[4] = intBytes[0];
            Data[5] = intBytes[1];
            Data[6] = intBytes[2];
            byte[] byteCount = BitConverter.GetBytes(count);
            Data[8] = byteCount[0];
            Data[9] = byteCount[1];

            byte[] Cmd = PacketHeader.Concat(Data).ToArray();

            int len = Cmd.Length - 9;
            intBytes = BitConverter.GetBytes(len);
            Cmd[7] = intBytes[0];
            Cmd[8] = intBytes[1];

            lock (LockObj)
            {
                try
                {
                    stream.Write(Cmd, 0, Cmd.Length);
                    //byte[] data = new Byte[256 + count * 2];
                    //Int32 bytes = stream.Read(data, 0, data.Length);  
                    // len = 11 + count*2
                    byte[] data = ReadNetwork(stream, count * 2 + 11, 2000);
                    int[] val = new int[count];
                    for (int i = 0; i < count; i++) val[i] = BitConverter.ToInt16(data, i * 2 + 11);
                    return val;
                    //return data[bytes - 2] + data[bytes - 1] * 256;
                }
                catch (Exception)
                {
                    QueryReconnect();
                    return new int[0];
                }
                finally
                {

                }
            }
        }

        public int ReadData32(int address)
        {
            byte[] Data = { 0x01, 0x04, // Read
                           0x00, 0x00, // Word
                           0x00, 0x00, 0x00, // Address, low to high
                           0xA8, // D*
                           0x01, 0x00 // Units low to high
                          };
            byte[] intBytes = BitConverter.GetBytes(address);
            Data[4] = intBytes[0];
            Data[5] = intBytes[1];
            Data[6] = intBytes[2];
            byte[] byteCount = BitConverter.GetBytes(2);
            Data[8] = byteCount[0];
            Data[9] = byteCount[1];

            byte[] Cmd = PacketHeader.Concat(Data).ToArray();

            int len = Cmd.Length - 9;
            intBytes = BitConverter.GetBytes(len);
            Cmd[7] = intBytes[0];
            Cmd[8] = intBytes[1];

            lock (LockObj)
            {
                try
                {
                    stream.Write(Cmd, 0, Cmd.Length);
                    //byte[] data = new Byte[256 + count * 2];
                    //Int32 bytes = stream.Read(data, 0, data.Length);  
                    // len = 11 + count*2
                    byte[] data = ReadNetwork(stream, 2*2 + 11, 2000);
                    int val = BitConverter.ToInt32(data, 11);
                    return val;
                }
                catch (Exception)
                {
                    QueryReconnect();
                    return 0;
                }
                finally
                {

                }
            }
        }

        public int[] ReadData32(int address, int count)
        {
            byte[] Data = { 0x01, 0x04, // Read
                           0x00, 0x00, // Word
                           0x00, 0x00, 0x00, // Address, low to high
                           0xA8, // D*
                           0x01, 0x00 // Units low to high
                          };
            byte[] intBytes = BitConverter.GetBytes(address);
            Data[4] = intBytes[0];
            Data[5] = intBytes[1];
            Data[6] = intBytes[2];
            byte[] byteCount = BitConverter.GetBytes(count * 2);
            Data[8] = byteCount[0];
            Data[9] = byteCount[1];

            byte[] Cmd = PacketHeader.Concat(Data).ToArray();

            int len = Cmd.Length - 9;
            intBytes = BitConverter.GetBytes(len);
            Cmd[7] = intBytes[0];
            Cmd[8] = intBytes[1];

            lock (LockObj)
            {
                try
                {
                    stream.Write(Cmd, 0, Cmd.Length);
                    //byte[] data = new Byte[256 + count * 2];
                    //Int32 bytes = stream.Read(data, 0, data.Length);  
                    // len = 11 + count*2
                    byte[] data = ReadNetwork(stream, count * 2 * 2 + 11, 2000);
                    int[] val = new int[count];
                    for (int i = 0; i < count; i++) val[i] = BitConverter.ToInt32(data, i * 4 + 11);
                    return val;
                }
                catch (Exception)
                {
                    QueryReconnect();
                    return new int[0];
                }
                finally
                {

                }
            }
        }

        public int ReadDataZR16(int address)
        {
            return ReadDataZR16(address, 1)[0];
        }

        public int ReadDataZR32(int address)
        {
            byte[] Data = { 0x01, 0x04, // Read
                           0x00, 0x00, // Word
                           0x00, 0x00, 0x00, // Address, low to high
                           0xB0, // ZR
                           0x01, 0x00 // Units low to high
                          };
            byte[] intBytes = BitConverter.GetBytes(address);
            Data[4] = intBytes[0];
            Data[5] = intBytes[1];
            Data[6] = intBytes[2];
            byte[] byteCount = BitConverter.GetBytes(2);
            Data[8] = byteCount[0];
            Data[9] = byteCount[1];

            byte[] Cmd = PacketHeader.Concat(Data).ToArray();

            int len = Cmd.Length - 9;
            intBytes = BitConverter.GetBytes(len);
            Cmd[7] = intBytes[0];
            Cmd[8] = intBytes[1];

            lock (LockObj)
            {
                try
                {
                    stream.Write(Cmd, 0, Cmd.Length);
                    //byte[] data = new Byte[256 + count * 2];
                    //Int32 bytes = stream.Read(data, 0, data.Length);  
                    // len = 11 + count*2
                    byte[] data = ReadNetwork(stream, 2 * 2 + 11, 2000);
                    int val = BitConverter.ToInt32(data, 11);
                    return val;
                }
                catch (Exception)
                {
                    QueryReconnect();
                    return 0;
                }
                finally
                {

                }
            }
        }

        public int[] ReadDataZR32(int address, int count)
        {
            byte[] Data = { 0x01, 0x04, // Read
                           0x00, 0x00, // Word
                           0x00, 0x00, 0x00, // Address, low to high
                           0xB0, // ZR
                           0x01, 0x00 // Units low to high
                          };
            byte[] intBytes = BitConverter.GetBytes(address);
            Data[4] = intBytes[0];
            Data[5] = intBytes[1];
            Data[6] = intBytes[2];
            byte[] byteCount = BitConverter.GetBytes(count * 2);
            Data[8] = byteCount[0];
            Data[9] = byteCount[1];

            byte[] Cmd = PacketHeader.Concat(Data).ToArray();

            int len = Cmd.Length - 9;
            intBytes = BitConverter.GetBytes(len);
            Cmd[7] = intBytes[0];
            Cmd[8] = intBytes[1];

            lock (LockObj)
            {
                try
                {
                    stream.Write(Cmd, 0, Cmd.Length);
                    //byte[] data = new Byte[256 + count * 2];
                    //Int32 bytes = stream.Read(data, 0, data.Length);  
                    // len = 11 + count*2
                    byte[] data = ReadNetwork(stream, count * 2 * 2 + 11, 2000);
                    int[] val = new int[count];
                    for (int i = 0; i < count; i++) val[i] = BitConverter.ToInt32(data, i * 4 + 11);
                    return val;
                }
                catch (Exception)
                {
                    QueryReconnect();
                    return new int[0];
                }
                finally
                {

                }
            }
        }

        public bool WaitForMemory(int addr, int timeout)
        {
            Stopwatch sw = Stopwatch.StartNew();
            while (sw.ElapsedMilliseconds < timeout)
            {
                if (ReadMemory(addr)) return true;
                Thread.Sleep(10);
            }
            return false;
        }

        public bool WaitForData(int addr, int expectedValue, int timeout)
        {
            Stopwatch sw = Stopwatch.StartNew();
            while (sw.ElapsedMilliseconds < timeout)
            {
                if (ReadData16(addr) == expectedValue) return true;
                Thread.Sleep(10);
            }
            return false;
        }

        public int WaitForMultiple(int addr, int[] expectValues, int timeout)
        {
            Stopwatch sw = Stopwatch.StartNew();
            while (sw.ElapsedMilliseconds < timeout)
            {
                for (int i = 0; i < expectValues.Length; i++)
                {
                    if (ReadData16(addr) == expectValues[i]) return expectValues[i];
                }
            }
            return -1;
        }
        
        public void ToggleMemory(int m, int duration = 500)
        {
            WriteMemory(m, true);
            Thread.Sleep(duration);
            WriteMemory(m, false);
        }

        public void WriteData16(int address, int value)
        {
            byte[] Data = { 0x01, 0x14, // Write
                           0x00, 0x00, // Word
                           0x00, 0x00, 0x00, // Address, low to high
                           0xA8, // D*
                           0x01, 0x00, // Units low to high
                           0x00, 0x00 // Value
                          };
            byte[] intBytes = BitConverter.GetBytes(address);
            Data[4] = intBytes[0];
            Data[5] = intBytes[1];
            Data[6] = intBytes[2];

            byte[] Cmd = PacketHeader.Concat(Data).ToArray();

            int len = Cmd.Length - 9;
            intBytes = BitConverter.GetBytes(len);
            Cmd[7] = intBytes[0];
            Cmd[8] = intBytes[1];

            intBytes = BitConverter.GetBytes(value);
            Cmd[Cmd.Length - 2] = intBytes[0];
            Cmd[Cmd.Length - 1] = intBytes[1];

            lock (LockObj)
            {
                try
                {
                    stream.Write(Cmd, 0, Cmd.Length);
                    //byte[] data = new Byte[256];
                    //Int32 bytes = stream.Read(data, 0, data.Length);
                    ReadNetwork(stream, 11, 2000);
                }
                catch (Exception)
                {
                    QueryReconnect();
                }
                finally
                {
                    
                }
            }
        }

        public byte[] IntToBytes2(int value)
        {
            byte[] intBytes = BitConverter.GetBytes(value);
            byte[] ret = new byte[2];
            ret[0] = intBytes[0];
            ret[1] = intBytes[1];
            return ret;
        }

        public byte[] IntToBytes3(int value)
        {
            byte[] intBytes = BitConverter.GetBytes(value);
            byte[] ret = new byte[3];
            ret[0] = intBytes[0];
            ret[1] = intBytes[1];
            ret[2] = intBytes[2];
            return ret;
        }

        public byte[] IntToBytes4(int value)
        {
            return BitConverter.GetBytes(value);
        }

        public void WriteData16(int address, params int[] value)
        {
            byte[] cmd = { 0x01, 0x14, // Write
                           0x00, 0x00 // Word
                         };
                          

            List<byte> list = new List<byte>();
            list.AddRange(PacketHeader);
            list.AddRange(cmd);
            list.AddRange(IntToBytes3(address));
            list.Add(0xA8); // D*
            list.AddRange(IntToBytes2(value.Length));
            for (int i = 0; i < value.Length; i++) list.AddRange(IntToBytes2(value[i]));

            byte[] packet = list.ToArray();

            int len = packet.Length - 9;
            byte[] intBytes = BitConverter.GetBytes(len);
            packet[7] = intBytes[0];
            packet[8] = intBytes[1];

            lock (LockObj)
            {
                try
                {
                    stream.Write(packet, 0, packet.Length);
                    //byte[] data = new Byte[256];
                    //Int32 bytes = stream.Read(data, 0, data.Length);
                    ReadNetwork(stream, 11, 2000);
                }
                catch (Exception)
                {
                    QueryReconnect();
                }
                finally
                {
                    
                }
            }
        }

        public void WriteData32(int address, params int[] value)
        {
            byte[] cmd = { 0x01, 0x14, // Write
                           0x00, 0x00 // Word
                         };


            List<byte> list = new List<byte>();
            list.AddRange(PacketHeader);
            list.AddRange(cmd);
            list.AddRange(IntToBytes3(address));
            list.Add(0xA8); // D*
            list.AddRange(IntToBytes2(value.Length * 2));
            for (int i = 0; i < value.Length; i++) list.AddRange(IntToBytes4(value[i]));

            byte[] packet = list.ToArray();

            int len = packet.Length - 9;
            byte[] intBytes = BitConverter.GetBytes(len);
            packet[7] = intBytes[0];
            packet[8] = intBytes[1];

            lock (LockObj)
            {
                try
                {
                    stream.Write(packet, 0, packet.Length);
                    ReadNetwork(stream, 11, 2000);
                }
                catch (Exception)
                {
                    QueryReconnect();
                }
                finally
                {
                    
                }
            }
        }

        public void WriteDataZR32(int address, params int[] value)
        {
            byte[] cmd = { 0x01, 0x14, // Write
                           0x00, 0x00 // Word
                         };


            List<byte> list = new List<byte>();
            list.AddRange(PacketHeader);
            list.AddRange(cmd);
            list.AddRange(IntToBytes3(address));
            list.Add(0xB0); // ZR
            list.AddRange(IntToBytes2(value.Length * 2));
            for (int i = 0; i < value.Length; i++) list.AddRange(IntToBytes4(value[i]));

            byte[] packet = list.ToArray();

            int len = packet.Length - 9;
            byte[] intBytes = BitConverter.GetBytes(len);
            packet[7] = intBytes[0];
            packet[8] = intBytes[1];

            lock (LockObj)
            {
                try
                {
                    stream.Write(packet, 0, packet.Length);
                    ReadNetwork(stream, 11, 2000);
                }
                catch (Exception)
                {
                    QueryReconnect();
                }
                finally
                {

                }
            }
        }

        public void WriteDataZR16(int address, params int[] value)
        {
            byte[] cmd = { 0x01, 0x14, // Write
                           0x00, 0x00 // Word
                         };


            List<byte> list = new List<byte>();
            list.AddRange(PacketHeader);
            list.AddRange(cmd);
            list.AddRange(IntToBytes3(address));
            list.Add(0xB0); // ZR
            list.AddRange(IntToBytes2(value.Length));
            for (int i = 0; i < value.Length; i++) list.AddRange(IntToBytes2(value[i]));

            byte[] packet = list.ToArray();

            int len = packet.Length - 9;
            byte[] intBytes = BitConverter.GetBytes(len);
            packet[7] = intBytes[0];
            packet[8] = intBytes[1];

            lock (LockObj)
            {
                try
                {
                    stream.Write(packet, 0, packet.Length);
                    //byte[] data = new Byte[256];
                    //Int32 bytes = stream.Read(data, 0, data.Length);
                    ReadNetwork(stream, 11, 2000);
                }
                catch (Exception)
                {
                    QueryReconnect();
                }
                finally
                {

                }
            }
        }

    }
}
