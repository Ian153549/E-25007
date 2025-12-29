using Sentech.StApiDotNET;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;

namespace NSAA_16Axis
{
    class PlcCommunication
    {
        private TcpClient _client = new TcpClient();
        private NetworkStream _stream;
        private object _locker = new object();
        private bool isOpen = false;

        //pitch = 1mm  table side = 220mm  Step angle = 0.072degree
        

        public PlcCommunication()
        {
        }

        public void Open(string ip, int port)
        {
            try
            {
                _client = new TcpClient(ip, port);
                _stream = _client.GetStream();
                _stream.ReadTimeout = 3000;
                _stream.WriteTimeout = 3000;
                isOpen = true;
            }
            catch
            {
                throw;
            }
        }

        public string Send(string message)
        {
            if (isOpen)
            {
                lock (_locker)
                {
                    try
                    {

                        byte cr = 0x0D;
                        byte[] data = new byte[System.Text.Encoding.ASCII.GetBytes(message).Length + 1];
                        for (int i = 0; i < data.Length - 1; i++)
                            data[i] = System.Text.Encoding.ASCII.GetBytes(message)[i];
                        data[data.Length - 1] = cr;
                        _stream.Write(data, 0, data.Length);


                        data = new Byte[256];
                        string responseData = String.Empty;

                        Int32 bytes = _stream.Read(data, 0, data.Length);
                        responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                        Thread.Sleep(100);
                        return responseData;
                    }
                    catch
                    {
                        Debug.WriteLine("PLC Send Error!");
                        throw;
                        // return "";
                    }
                }
            } else
            {
                return "";
            }
        }

        public bool AlignXyyTableMove(int xPulse, int yPulse, int degreePulse)
        {
            lock (_locker)
            {
                Stopwatch sw = Stopwatch.StartNew();
                Send("WR DM6402.L " + "0");
                Send("WR DM6404.L " + "0");
                Send("WR DM6408.L " + "0");
                Send("WR DM6410.L " + "0");
                string xStr = ((xPulse + degreePulse) * -1).ToString();
                string y1Str = (yPulse + degreePulse * -1).ToString();
                string y2Str = (yPulse + degreePulse).ToString();

                Send("WR DM6202.L " + xStr);
                Send("WR DM6204.L " + y1Str);
                Send("WR DM6206.L " + y2Str);
                Send("WR DM6201.S 1");
                while (true)
                {
                    Thread.Sleep(50);
                    if (Send("RD DM6201.S") == "+00000\r\n")
                        break;
                    if (sw.ElapsedMilliseconds > 10000)
                    {
                        GV.Plc.Send("WR DM6400.S 1");
                        // throw new MRException("Error : xyyTable timeout");
                    }

                }
                return true;
            }
        }
        
        public bool AlignCameraMove(double leftX, double rightX, double leftY, double rightY)
        {
            lock (_locker)
            {
                Stopwatch sw = Stopwatch.StartNew();
                Send("WR DM6202.L " + "0");
                Send("WR DM6204.L " + "0");
                Send("WR DM6206.L " + "0");

                int ileftX = (int)leftX;
                int irightX = (int)rightX;
                int ileftY = (int)leftY;
                int irightY = (int)rightY;

                string lXStr = ileftX.ToString();
                string rXStr = irightX.ToString();
                string lYStr = ileftY.ToString();
                string rYStr = irightY.ToString();
                //string yStr = ((leftY + rightY) / 2).ToString();

                Send("WR DM6402.L " + lXStr);
                Send("WR DM6404.L " + rXStr);
                Send("WR DM6408.L " + lYStr);
                Send("WR DM6410.L " + rYStr);
                //Send("WR DM6406.L " + yStr);
                Send("WR DM6201.S 1");
                while (true)
                {
                    Thread.Sleep(50);
                    if (Send("RD DM6201.S") == "+00000\r\n")
                        break;
                    if (sw.ElapsedMilliseconds > 10000)
                    {
                        GV.Plc.Send("WR DM6400.S 1");
                        throw new MRException("Error : Camera motor timeout");
                    }

                }
                return true;
            }
        }
        
        public bool RecipeModeXyyTableMove(int xPulse, int yPulse, int degreePulse)
        {
            lock(_locker)
            {
                Stopwatch sw = Stopwatch.StartNew(); // 68
                //sw.Reset();
                if (Send("RD MR18300") == "0\r\n" || Send("RD MR12701") == "0\r\n" || Send("RD MR31302") == "0\r\n")
                    throw new MRException("Not in recipe page or leveling unfinished");

                Send("WR MR15113 1");
                while(true)
                {
                    if (Send("RD MR15114") == "1\r\n")
                        break;
                    if (sw.ElapsedMilliseconds > 10000)
                        throw new MRException("Enter measure mark distance mode time out");
                }

                string xStr = ((xPulse + degreePulse) * -1).ToString();
                string y1Str = (yPulse + degreePulse * -1).ToString();
                string y2Str = (yPulse + degreePulse).ToString();
                Send("WR DM6802.L " + xStr);
                Send("WR DM6804.L " + y1Str);
                Send("WR DM6806.L " + y2Str);
                Send("WR DM6801.S 1");
                while (true)
                {
                    if (Send("RD DM6801.S") == "+00000\r\n")
                        break;
                    if (sw.ElapsedMilliseconds > 10000)
                        throw new MRException("Error : xyyTable timeout");
                    Thread.Sleep(50);
                }
                Send("WR MR15113 0");
                while (true)
                {
                    if (Send("RD MR15114") == "0\r\n")
                        break;
                    if (sw.ElapsedMilliseconds > 10000)
                        throw new MRException("Quit measure mark distance mode time out");
                }
            }
            return true;
        }

        public bool CorrectionXyyTableMove(int xPulse, int yPulse, int degreePulse)
        {
            lock (_locker)
            {
                ChangeXyyTableSpeed(7000);
                Stopwatch sw = Stopwatch.StartNew();
                sw.Reset();
                if (Send("RD DM6300.S") != "+00001\r\n")
                {

                    throw new MRException("Not in correction mode");
                }
                Send("WR DM6602.L " + "0");
                Send("WR DM6604.L " + "0");
                Send("WR DM6608.L " + "0");
                Send("WR DM6610.L " + "0");

                string xStr = ((xPulse + degreePulse) * -1).ToString();
                string y1Str = (yPulse + degreePulse * -1).ToString();
                string y2Str = (yPulse + degreePulse).ToString();
                Send("WR DM6302.L " + xStr);
                Send("WR DM6304.L " + y1Str);
                Send("WR DM6306.L " + y2Str);
                Send("WR DM6301.S 1");
                while (true)
                {
                    if (Send("RD DM6301.S") == "+00000\r\n")
                        break;
                    if (sw.ElapsedMilliseconds > 10000)
                        throw new MRException("Error : xyyTable timeout");
                    Thread.Sleep(50);
                }
                return true;
            }
        }
        
        public bool CorrectionCameraMove(int leftX, int rightX, int leftY, int rightY)
        {
            lock (_locker)
            {
                Stopwatch sw = Stopwatch.StartNew();
                
                if (Send("RD DM6300.S") != "+00001\r\n")
                {

                    throw new MRException("Not in correction mode");
                }

                leftX *= -1;
                rightY *= -1;
                leftY *= -1;
                Send("WR DM6302.L " + "0");
                Send("WR DM6304.L " + "0");
                Send("WR DM6306.L " + "0");

                string lXStr = leftX.ToString();
                string rXStr = rightX.ToString();
                string lYStr = leftY.ToString();
                string rYStr = rightY.ToString();
                Send("WR DM6602.L " + lXStr);
                Send("WR DM6604.L " + rXStr);
                Send("WR DM6608.L " + lYStr);
                Send("WR DM6610.L " + rYStr);
                //Send("WR DM6606.L " + yStr);
                Send("WR DM6301.S 1");
                while (true)
                {
                    if (Send("RD DM6301.S") == "+00000\r\n")
                        break;
                    if (sw.ElapsedMilliseconds > 10000)
                        throw new MRException("Error : Camera motor timeout");
                    Thread.Sleep(50);
                }
                return true;
            }
        }

        public void TellPlcToEnterContactUnContactMode()
        {
            Stopwatch sw = Stopwatch.StartNew();
            GV.Plc.Send("WR MR8612 1");
            while (true)
            {
                if (GV.Plc.Send("RD MR8612") == "0\r\n")
                    break;
                if (sw.ElapsedMilliseconds > 5000)
                {
                    Debug.WriteLine("Enter Contact/Uncontact Mode time out");
                    throw new MRException("Enter Contact/Uncontact Mode time out");
                }
            }
        }

        public void Contact()
        {
            Stopwatch sw = Stopwatch.StartNew();
            GV.Plc.Send("WR DM6208.S 1");
            while (true)
            {
                Thread.Sleep(50);
                if (GV.Plc.Send("RD DM6209.S") == "+00001\r\n")
                    break;
                if (sw.ElapsedMilliseconds > 5000)
                {
                    Debug.WriteLine("Contact time out");
                    throw new MRException("Contact time out");
                }
            }
            Thread.Sleep(500);
        }

        public void Uncontact()
        {
            Stopwatch sw = Stopwatch.StartNew();
            GV.Plc.Send("WR DM6208.S 2");  
            while (true)
            {
                Thread.Sleep(50);
                if (GV.Plc.Send("RD DM6209.S") == "+00000\r\n")
                    break;
                if (sw.ElapsedMilliseconds > 5000)
                    throw new MRException("Unontact time out");
            }
            Thread.Sleep(500);
        }

        public void ChangeXyyTableSpeed(int speed)
        {
            Stopwatch sw = Stopwatch.StartNew();
            GV.Plc.Send("WR DM6702.L " + speed.ToString());
            while (true)
            {
                Thread.Sleep(50);
                if (Convert.ToInt32(GV.Plc.Send("RD DM6702.L")) == speed)
                    break;
                if (sw.ElapsedMilliseconds > 5000)
                {
                    Debug.WriteLine("ChangeXyyTableSpeed time out");
                    throw new MRException("ChangeXyyTableSpeed time out");
                }
            }
        }

        public void Exposure()
        {
            GV.Plc.Send("WR DM6000.S 3");
        }

        public void Close()
        {
            if (_client.Connected)
            {
                _client.Close();
                _stream.Close();
            }
            
        }

        public bool IsConnected()
        {
            if (_client == null)
                return false;
            return _client.Connected;
        }

        public string McSend(string message)
        {
            try
            {

                //byte cr = 0x0D;
                byte[] data = new byte[System.Text.Encoding.ASCII.GetBytes(message).Length];
                for (int i = 0; i < data.Length; i++)
                    data[i] = System.Text.Encoding.ASCII.GetBytes(message)[i];
                //data[data.Length - 1] = cr;
                _stream.Write(data, 0, data.Length);


                data = new Byte[256];
                string responseData = String.Empty;

                Int32 bytes = _stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Thread.Sleep(100);
                return responseData;
            }
            catch
            {
                throw;
            }
        }
    }

    class MPlcCommunication
    {
        public enum PlcStatusKind
        {
            Off,
            Power_Off,
            Serov_Off,
            Init,
            Ready,
            Busy,
            Error,
            AlignNG,
            RobotFeed,
            PinUnwind,
            ChuckSmooth,
            Align,
            Exposure,
            PinWind,
            RobotDischarge,
            ExposureOK
        }

        string RemoteHost;
        int Port;
        NetworkStream stream;
        public bool IsOpen = false;

        static MPlcCommunication Instance;
        public string PlcErrorMessage = "";

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

        private TcpClient client = new TcpClient();

        private object _locker = new object();

        public static int iPadMoveStopStatus = 0;
        public static int MxokCount = 0;
        public static int MRerrorCount = 1;
        public static int MDerrorCount = 1;
        public int ChuckOffset = 600;
        public int iMainModeStatus = 0;

        public static int PlcMRSize = 96;
        public static int PlcMWSize = 128;
        public static int PlcDRSize = 100;
        public static int PlcDWSize = 50;
        public static int PlcDW1Size = 20;
        public static int PlcRWait = 250;
        public static int PlcWWait = 250;

        public int iRCount = 20;
        public int[] iRMemory = new int[50];
        public int iRMemoryAddress = 13200;
        //public int iDMaskY = 13402;
        //public int iDUpBigY = 13402;
        //public int iDUpLeftX = 13404;
        //public int iDUpRightX = 13406;
        //public int iDChuckZ = 13408;
        //public int iDChuckX = 13410;
        //public int iDChuckY1 = 13412;
        //public int iDChuckY2 = 13414;
        //public int iDUpLeftY = 13416;
        //public int iDUpRightY = 13418;
        //public int iDDownLeftX = 13420;
        //public int iDUpLeftZ = 13432;
        //public int iDUpRightZ = 13434;

        //public int iDDownRightX = 13422;
        //public int iDDownLeftY = 13424;
        //public int iDDownRightY = 13426;
        //public int iDDownLeftZ = 13428;
        //public int iDDownRightZ = 13430;
        
        //** 沒使用到
        
        //**

        public int iModifyRecipeNumber = 14940;
        public int iNowRecipeNumber = 13000;
        public int iLWaferHigh = 14944;
        public int iRWaferHigh = 14946;
        public int iHartBeat = 14948;

        public int iWCount = 20;
        public int[] iWMemory = new int[20];
        public int iWMemoryAddress = 13250;
        public int iDSMaskY = 13300;
        public int iDSBigY = 13302;
        public int iDSUpLeftX = 13304;
        public int iDSUpRightX = 13306;
        public int iDScChuckZ = 13308;
        public int iDScChuckX = 13310;
        public int iDScChuckY1 = 13312;
        public int iDScChuckY2 = 13314;
        public int iDSUpLeftY = 13316;
        public int iDSUpRightY = 13318;
        public int iDSUpLeftZ = 13320;
        public int iDSUpRightZ = 13322;

        //待確認下面移動點位
        //public int iDSDownLeftX = 13320;
        //public int iDSDownRightX = 13322;
        //public int iDSDownLeftY = 13324;
        //public int iDSDownRightY = 13326;
        //public int iDSDownLeftZ = 13328;
        //public int iDSDownRightZ = 13330;
        public int iDSDownLeftX = 13324;
        public int iDSDownRightX = 13326;
        public int iDSDownLeftY = 13328;
        public int iDSDownRightY = 13330;
        public int iDSDownLeftZ = 13332;
        public int iDSDownRightZ = 13334;
        /// 待確認
        
        //public int iDSReserve3 = 14988;
        //public int iDSChuckXSpeed = 14990;
        //public int iDSChuckY1Speed = 14992;
        //public int iDSChuckY2Speed = 14994;

        public int iiDSMaskY = 0;
        public int iiDSBigY = 1;
        public int iiDSUpLeftX = 2;
        public int iiDSUpRightX = 3;
        public int iiDSChuckZ = 4;
        public int iiDSChuckX = 5;
        public int iiDSChuckY1 = 6;
        public int iiDSChuckY2 = 7;
        public int iiDSUpLeftY = 8;
        public int iiDSUpRightY = 9;
        public int iiDSUpLeftZ = 10;
        public int iiDSUpRightZ = 11;
        public int iiDSDownLeftX = 12;
        public int iiDSDownRightX = 13;
        public int iiDSDownLeftY = 14;
        public int iiDSDownRightY = 15;
        public int iiDSDownLeftZ = 16;
        public int iiDSDownRightZ = 17;

        public int iiDMaskY = 0;
        public int iiDUpBigY = 1;
        public int iiDUpLeftX = 2;
        public int iiDUpRightX = 3;
        public int iiDChuckZ = 4;
        public int iiDChuckX = 5;
        public int iiDChuckY1 = 6;
        public int iiDChuckY2 = 7;
        public int iiDUpLeftY = 8;
        public int iiDUpRightY = 9;
        public int iiDUpLeftZ = 10;
        public int iiDUpRightZ = 11;
        public int iiDDownLeftX = 12;
        public int iiDDownRightX = 13;
        public int iiDDownLeftY = 14;
        public int iiDDownRightY = 15;
        public int iiDDownLeftZ = 16;
        public int iiDDownRightZ = 17;
        


        public int iWSCount = 20;
        public int[] iWSMemory = new int[20];
        public int iWSMemoryAddress = 13300;
        public int iDSSMaskY = 13200;
        public int iDSSBigY = 13202;
        public int iDSSUpLeftX = 13204;
        public int iDSSUpRightX = 13206;
        public int iDSSChuckZ = 13208;
        public int iDSSChuckX = 13210;
        public int iDSSChuckY1 = 13212;
        public int iDSSChuckY2 = 13214;
        public int iDSSUpLeftY = 13216;
        public int iDSSUpRightY = 13218;
        public int iDSSDownLeftX = 13220;
        public int iDSSDownRightX = 13222;
        public int iDSSDownLeftY = 13224;
        public int iDSSDownRightY = 13226;
        public int iDSSDownLeftZ = 13228;
        public int iDSSDownRightZ = 13230;
        public int iDSSUpLeftZ = 13232;
        public int iDSSUpRightZ = 13234;


        public int iAutoRun = 13500;
        public int iHmi = 13502;

        public int iDLimitAddress = 14000;
        public int iLimitCount = 80;
        public int[] iLimit = new int[80];

        public int iDownLHigh;
        public int iDownRHigh;

        // public int iDownCCDStart = 15050;
        //沒使用到**
        public int iDSUpLX = 14956;
        public int iDSUpLY = 14968;
        public int iDSUpRX = 14958;
        public int iDSUpRY = 14970;
        public int iDSUpBY = 14954;
        public int iDSChuckX = 14962;
        public int iDSChuckY1 = 14964;
        public int iDSChuckY2 = 14966;
        public int iDSChuckZ = 15116;
        //**

        // public int iDSChuckXSpeed = 15140;
        // public int iDSChuckY1Speed = 15142;
        // public int iDSChuckY2Speed = 15144;
        //**沒有使用
        public int iRecipeUpLX = 16521;
        public int iRecipeUpLY = 16523;
        public int iRecipeUpRX = 16525;
        public int iRecipeUpRY = 16527;
        public int iRecipeUpBY = 16529;

        public int iRecipeDownLX = 16541;
        public int iRecipeDownLY = 16543;
        public int iRecipeDownLZ = 16545;
        public int iRecipeDownRX = 16547;
        public int iRecipeDownRY = 16549;
        public int iRecipeDownRZ = 16551;

        public int iRecipeChuckX = 16561;
        public int iRecipeChuckY1 = 16563;
        public int iRecipeChuckY2 = 16565;
        public int iRecipeChuckZ = 16567;

        public int iRecipeExpPower = 16581;
        public int iAlignGap = 16583;
        public int iExposureGap = 16585;
        public int iContactMode = 16587;
        public int iRecipeSize = 16589;       
                
        public static string RmAddress = "M12992";
        public static string WmAddress = "M14992";
        public static int iReadBase = 12992;
        public static int iWriteBase = 14992;
        public static string RdAddress = "D14900";
        public static int iReadDbase = 14900;
        public static string WdAddress = "D15000";
        public static string W1dAddress = "D15100";
        public static int[] PLCmR = new int[PlcMRSize];
        public static int[] PLCmRT = new int[PlcMRSize];
        public static int[] PLCmW = new int[PlcMWSize];
        public static int[] PLCmWT = new int[PlcMWSize];
        public static int[] PLCdR = new int[PlcDRSize];
        public static int[] PLCdRT = new int[PlcDRSize];
        public static int[] PLCdW = new int[PlcDWSize];
        public static int[] PLCdW1 = new int[PlcDW1Size];
        //**

        public int[] NowLocation = new int[20];
        public bool[] NowAction = new bool[60];

        public bool readLoop = true;
        public bool writeLoop = true;

        public int iRecipeNumber = 13000;
        public int iAlignMode = 13012;
        public int iUpDownAlign = 13030;
        public int iMaskBond = 13036;

        public int iAction = 13000;
        public int iACCDStart = 0;
        public int iAMaskStart = 10;
        public int iAAdjStart = 50;

        public int iUpCCDStart = 13000;
        public int iUpCCDEnd = 13001;
        public int iTopContactOK = 13002;
        public int iUnContactOK = 13003; 

        public int iMaskStart = 13010;
        public int iMaskEnd = 13011;

        public int iChuckStart = 13020;
        public int iChuckEnd = 13021;
        public int iBOTContactOK = 13022;

        public int iDownCCDStart = 13040;
        public int iDownCCDEnd = 13041;
        public int iCCDAdjust = 13050;
        public int iCCDAdjOk = 13051;
        public int iCCDAdjStart = 13061;
        public int iCCDAdjEnd = 13062;

        public int iUpCCDAlignStart = 13500;
        public int iUpCCDAlignOK = 13501;
        public int iUpCCDAlignACK = 13502;
        public int iUpContactOK = 13503;
        public int iUpContactNG = 13504;
        public int iUpContactFault = 13505;
        public int iUpAlignNG = 13506;

        public int iMaskMoveStart = 13510;
        public int iMaskMoveOK = 13511;
        public int iMaskMoveNG = 13512;

        public int iBackCCDAlignStart = 13520;
        public int iBackCCDAlignOK = 13521;
        public int iBackCCDAlignNG = 13522;
        public int iBackContactOK = 13503;
        public int iBackContactNG = 13524;
        public int iBackContactFault = 13525;

        public int iBackMaskStart = 13510;
        public int iBackMaskOK = 13511;
        public int iBackMaskACK = 13512;
        public int iBackMaskNG = 13513;

        public int iCCDAdjMoveStart = 13550;
        
        public int iCCDAdjMoveOK = 13561;
        public int iRecipeSave = 13400;
        public int iSkipAlign = 13410;
        public int iUpAlign = 13411;
        public int iDownAlign = 13412;
        public int iEMSON = 13414;
        public int iExposure = 13415;
        public int iRecipeCopy = 13416;
        // public int iAutoRun = 13417;
        public int iEditRun = 13418;
        public int iMachinStart = 13419;
        public int iCapture = 13420;
        public int iNewRun = 13421;
        public int iRunOver = 13422;
        public int iTabEnable = 13424;
        public int iStopAlign = 13425;

        public int iPadMoveOkDown = 13011;
        public int iPadMoveStopOk = 13012;
        public int iPadMoveOkChuck = 13015;
        
        public int iBarcodeRead = 16020;
        public int iBarcodeChageOK = 16021;
        public int iAlignTestStart = 16120;
        public int iDownCCDAlignStart = 13030;
        public int iUpCCDRecipeTeach = 13040;
        public int iDownCCDRecipeTeach = 13041;
        public int iChuckRecipeTeach = 13042;
        public int iDownCCDAlign = 13050;
        // public int iUpCCDAlignStart = 13060;
        public int iUpCCDok = 13061;
        public int iChuckok = 13062;
        public int isStartExposure = 13080;
        public int iExposureOK = 13081;
        public int iInChurkOk = 13100;
        public int iChurkVacOK = 13101;
        public int iChurkVacFault = 13102;
        public int iChurkReady = 13200;

        public int iPadMoveUp = 15000;
        public int iPadMoveOver = 15001;
        public int iPadWork = 15002;
        public int iPadZwork = 15003;
        public int iPadMoveDown = 15011;
        public int iPadMoveStop = 15012;
        public int iPadMoveChuck = 15015;

        public int iBarcodeOK = 15020;
        public int iBarcodeNG = 15021;
        public int iUpCCDMask = 15022;
        public int iDownCCDMask = 15023;

        public int iHeartBeat = 15051;

        // public int iUpCCDStart = 15060;
        // public int iChuckStart = 15061;
        public int iAlignOK = 15062;
        public int iAlignNG = 15063;

        public int iExposureStart = 15080;

        public int iRobotAction = 15100;
        public int iPinCanVac = 15101;
        public int iWaferOnLoad = 15102;
        public int iRobotGet = 15200;
        public int iWaitVac = 15201;

        public int iAuto = 15500;
        public int iManual = 15501;
        public int iMaintain = 15502;
        public int iAbReturn = 15504;
        public int iStopBeep = 15505;

        public int iInitOK = 13600;     // 初始化完成 M
        public int iChuckXOK = 13601;     // Chuck X接料位置到達 M
        public int iChuckY1OK = 13602;    //  Chuck Y1接料位置到達 M13602
        public int iChuckY2OK = 13603;    //  Chuck Y2接料位置到達    M13603
        public int iPINupOK = 13604;    //  PIN上升定位 M13604
        public int iMotorNoAct = 13605; //  馬達無動作   M13605
        public int iChuckVacNo = 13606; //  Chuck Vac無真空 M13606
        public int iPinVacNo = 13607;   //  PIN Vac無真空  M13607
        public int iPLCError = 13608;   //  PLC No Error 設備無異常   M13608

        public int iAlign = 1000;
        public int iAlinger = 1001;
        public int iMaskMemory= 1003;

        public int iLimitLeftDownZmin = 29;
        public int iLimitLeftDownZmax = 28;
        public int iLimitRightDownZmin = 31;
        public int iLimitRightDownZmax = 30;

        public Thread thClearGet;
        public Thread thClearPut;

        public static int[] bitCheck = { 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384, 32768 };
        public static int[] NbitCheck = { 65534, 65533, 65531, 65527, 65519, 65503, 65471, 65407, 65279, 65023, 64511, 63487, 61439, 57343, 49151, 32767 };

        public int[] RecipeData = new int[50];

        public static int iMoveBU;
        public static int iMoveBV;
        public static int iMoveBW;

        public static List<KeyValuePair<string, int>> list = new List<KeyValuePair<string, int>>();

        public static Dictionary<string, Int16> PlcDataList = new Dictionary<string, Int16>();

        public MPlcCommunication()
        {
            Instance = this;
        }

        public MPlcCommunication(string remoteHost, int port)
        {
            RemoteHost = remoteHost;
            Port = port;
            Instance = this;
        }

        public void MxCreateEmuData()
        {
            PlcDataList.Add("D" + iNowRecipeNumber.ToString(), 1);
            PlcDataList.Add("D" + iModifyRecipeNumber.ToString(), 1);
            PlcDataList.Add("D13019", 0);
            
            if (GV.AppSettingParm.StartUpBack == 1)
            {
                PlcDataList.Add("M" + iDownAlign.ToString(), 0);
                PlcDataList.Add("M" + iUpAlign.ToString(), 1);
            }
            else if (GV.AppSettingParm.StartUpBack == 2)
            {
                PlcDataList.Add("M" + iDownAlign.ToString(), 1);
                PlcDataList.Add("M" + iUpAlign.ToString(), 0);
            }
            else
            {
                PlcDataList.Add("M" + iDownAlign.ToString(), 0);
                PlcDataList.Add("M" + iUpAlign.ToString(), 0);
            }
            PlcDataList.Add("M" + iCapture.ToString(), 0);
            PlcDataList.Add("M" + iTabEnable.ToString(), 0);
            PlcDataList.Add("M" + iUpCCDStart.ToString(), 0);
            PlcDataList.Add("M" + iStopAlign.ToString(), 0);
            PlcDataList.Add("M" + iEditRun.ToString(), 0);
            PlcDataList.Add("M" + iRunOver.ToString(), 0);
            PlcDataList.Add("M" + iExposure.ToString(), 0);
            PlcDataList.Add("M" + iDownCCDStart.ToString(), 0);
            PlcDataList.Add("M" + iChuckStart.ToString(), 0);
            PlcDataList.Add("M" + iUpCCDEnd.ToString(), 0);
            // PlcDataList.Add("M" + iAutoRun.ToString(), 0);
            PlcDataList.Add("M" + iNewRun.ToString(), 0);
        }

        public void MxSetData(int iadd, Int16 idata)
        {
            string sadd = "D" + iadd.ToString();
            PlcDataList[sadd] = idata;
        }

        public void MxSetDataM(int iadd, Int16 idata)
        {
            string sadd = "M" + iadd.ToString();
            if (idata != 0)
            {
                PlcDataList[sadd] = 1;
            }
            else
            {
                PlcDataList[sadd] = 0;
            }
        }

        public void MxCheckMethod()
        {
            /*
            while (true)
            {
                Thread.Sleep(1000);
                MxokCount++;
                if (MxokCount > 5)
                {
                    try
                    {
                        readLoop = true;
                        if (thPlcRead != null)
                        {
                            thPlcRead.Abort();
                        }
                        thPlcRead = new Thread(PlcReadMethod);
                        thPlcRead.Start();
                    }
                    catch (Exception ex)
                    {
                    }
                }

                if (ReciveM(iAlignNG) == 1)
                {
                    // GV.Exposure.ExposureStatus = ExposureStatusDef.ExposureStatusKind.AlignNG;
                }
                else if (ReciveM(iBarcodeRead) == 1)
                {
                    // GV.Exposure.ExposureStatus = ExposureStatusDef.ExposureStatusKind.ReadQRcode;
                }
                else if (ReciveM(iDownCCDAlignStart) == 1)
                {
                    //GV.Exposure.ExposureStatus = ExposureStatusDef.ExposureStatusKind.DownAlignStart;
                }
                else if (ReciveM(iPadMoveStart) == 1)
                {
                    // GV.Exposure.ExposureStatus = ExposureStatusDef.ExposureStatusKind.iPadMove;
                }
                else if (ReciveM(iUpCCDAlignStart) == 1)
                {
                    if (GV.Exposure.ExposureStatus == ExposureStatusDef.ExposureStatusKind.AlignNG)
                    {

                    }
                    else
                    {
                        GV.Exposure.ExposureStatus = ExposureStatusDef.ExposureStatusKind.Align;
                    }
                }
                else if (ReciveM(iUpCCDRecipeTeach) == 1)
                {
                    GV.Exposure.ExposureStatus = ExposureStatusDef.ExposureStatusKind.iUpCCDRecipeTeach;
                }
                else if (ReciveM(iDownCCDRecipeTeach) == 1)
                {
                    GV.Exposure.ExposureStatus = ExposureStatusDef.ExposureStatusKind.iDownCCDRecipeTeach;
                }
                else if (ReciveM(iChuckRecipeTeach) == 1)
                {
                    GV.Exposure.ExposureStatus = ExposureStatusDef.ExposureStatusKind.iChuckRecipeTeach;
                }
                else if (ReciveM(iDownCCDAlign) == 1)
                {
                    GV.Exposure.ExposureStatus = ExposureStatusDef.ExposureStatusKind.DownAlign;
                }
                else if (ReciveM(iPLCError) == 1)
                {
                    GV.Exposure.ExposureStatus = ExposureStatusDef.ExposureStatusKind.Error;
                }
                else
                {
                    GV.Exposure.ExposureStatus = ExposureStatusDef.ExposureStatusKind.Ready;
                }

                if (ReciveM(iChurkReady) == 1)
                {
                    if (ReciveM(iAlignNG) == 1) SendM(iAlignNG, 0);
                    if (ReciveM(iAlignOK) == 1) SendM(iAlignOK, 0);
                }
            }
            */
        }

        public void PlcReadMethod()
        {
            int hResult = 0;

            while (readLoop)
            {
                // hResult = ActPlc.ReadDeviceBlock(RmAddress, PlcMRSize, out PLCmRT[0]);

                if (hResult != 0)
                {
                    readLoop = false;
                    throw new MRException("Error : PLC Read Memory M failure!!");
                }
                else
                {
                    if (PLCmRT[1] > 0)
                    {
                        for (int i = 0; i < PlcMRSize; i++)
                        {
                            PLCmR[i] = PLCmRT[i];
                        }
                        MxokCount = 0;
                    }
                    else
                    {
                        MRerrorCount++;
                    }
                }

                Thread.Sleep(PlcRWait);

                if (readLoop == true)
                {
                    // hResult = ActPlc.ReadDeviceBlock(WmAddress, PlcMWSize, out PLCmWT[0]);

                    if (hResult != 0)
                    {
                        readLoop = false;
                        throw new MRException("Error : PLC Read Memory WM failure!!");
                    }
                    else
                    {
                        if (PLCmWT[1] > 0)
                        {
                            for (int i = 0; i < PlcMWSize; i++)
                            {
                                PLCmW[i] = PLCmWT[i];
                            }
                            MxokCount = 0;
                        }
                        else
                        {
                            MRerrorCount++;
                        }
                    }
                }

                Thread.Sleep(PlcRWait);

                if (readLoop == true)
                {
                    // hResult = ActPlc.ReadDeviceBlock(RdAddress, PlcDRSize, out PLCdRT[0]);

                    if (hResult != 0)
                    {
                        readLoop = false;
                        throw new MRException("Error : PLC Read Memory D failure!!");
                    }
                    else
                    {
                        if (PLCdRT[48] > 0)
                        {
                            for (int i = 0; i < PlcDRSize; i++)
                            {
                                PLCdR[i] = PLCdRT[i];
                            }
                            MxokCount = 0;
                        }
                        else
                        {
                            MDerrorCount++;
                        }
                    }
                    Thread.Sleep(PlcRWait);
                }
            }
            LogF.WriteLog("PLC Read Stop!!");
        }

        public void Open(string remoteHost, int port, int timeOut = 2000)
        {
            RemoteHost = remoteHost;
            Port = port;
            Open(5000);
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

        public string Send(string message)
        {
            
            //lock (_locker)
            //{
            //    try
            //    {

            //        byte cr = 0x0D;
            //        byte[] data = new byte[System.Text.Encoding.ASCII.GetBytes(message).Length + 1];
            //        for (int i = 0; i < data.Length - 1; i++)
            //            data[i] = System.Text.Encoding.ASCII.GetBytes(message)[i];
            //        data[data.Length - 1] = cr;
            //        _stream.Write(data, 0, data.Length);


            //        data = new Byte[256];
            //        string responseData = String.Empty;

            //        Int32 bytes = _stream.Read(data, 0, data.Length);
            //        responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            //        Thread.Sleep(100);
            //        return responseData;
            //    }
            //    catch
            //    {
            //        Debug.WriteLine("PLC Send Error!");
            //        throw;
            //        // return "";
            //    }
            //}
            
            return "not implement!!";
        }

        public int Send(string sMem, int idata)
        {
            try
            {
                int hResult = 0;
                int[] arrDeviceValue;
                string sAddress = sMem;
                arrDeviceValue = new int[1];
                arrDeviceValue[0] = idata;

                short[] arrValue;
                arrValue = new short[1];
                arrValue[0] = (short)idata;

                // lock (_locker)
                //{
                    // hResult = ActPlc.WriteDeviceRandom2(sAddress, 1, arrValue[0]);
                //}

                return hResult;
            }
            catch
            {
                return -1;
            }
        }

        public int SendM(int iAddress, int idata)
        {
            if (idata == 0)
            {
                WriteMemory(iAddress, false);
            } else
            {
                WriteMemory(iAddress, true);
            }
            return 0;
        }

        public string Recive(string sMem)
        {
            short[] arrDeviceValue;
            string sAddress = sMem;
            arrDeviceValue = new short[1];
            arrDeviceValue[0] = 0;
            string sRet = "No";

            int hResult = 0;
            // int hResult = ActPlc.ReadDeviceRandom2(sAddress, 1, out arrDeviceValue[0]);

            if (hResult != 0)
            {
                sRet = "ReadDeviceBlock Failure !!\r\n";
            }
            else
            {
                sRet = arrDeviceValue[0].ToString();
            }

            return sRet;
        }

        public int ReciveM(int iMem)
        {
            int iRet = 0;

            if (ReadMemory(iMem)) iRet = 1;

            return iRet;
        }

        public int ReciveD(int iMem)
        {
            int iRet = ReadData32(iMem);
            return iRet;
        }

        public bool AlignXyyTableMove(int xPulse, int yPulse, int degreePulse)
        {
            int iX, iY1, iY2;

            bool bRet = true;
            bool bwait = true;

            int[] TempArray = ReadData32(iRMemoryAddress, iRCount);
            Array.Copy(TempArray, iWMemory, iRCount);

            int xStr = -xPulse + degreePulse;
            int y1Str = yPulse + degreePulse;
            int y2Str = yPulse + degreePulse * -1;

            iWMemory[iiDSChuckX] += xStr;
            iWMemory[iiDSChuckY1] += y1Str;
            iWMemory[iiDSChuckY2] += y2Str;

            WriteData32(iWMemoryAddress, iWMemory);

            iX = Math.Abs(xStr);
            iY1 = Math.Abs(y1Str);
            iY2 = Math.Abs(y2Str);

            int iMax = iX;

            if (iY1 > iMax)
            {
                iMax = iY1;
            }
            if (iY2 > iMax)
            {
                iMax = iY2;
            }

            if (iMax < 1) iMax = 10;

            int sX = (GV.AppSettingParm.XYYTableSpeed * iX) / iMax;
            int sY1 = (GV.AppSettingParm.XYYTableSpeed * iY1) / iMax;
            int sY2 = (GV.AppSettingParm.XYYTableSpeed * iY2) / iMax;

            if (sX < GV.AppSettingParm.TableMinSpeed) sX = GV.AppSettingParm.TableMinSpeed;
            if (sY1 < GV.AppSettingParm.TableMinSpeed) sY1 = GV.AppSettingParm.TableMinSpeed;
            if (sY2 < GV.AppSettingParm.TableMinSpeed) sY2 = GV.AppSettingParm.TableMinSpeed;

            TempArray = ReadData32(iWSMemoryAddress, iRCount);
            Array.Copy(TempArray, iWSMemory, iRCount);
            iWSMemory[iiDSChuckX] = sX;
            iWSMemory[iiDSChuckY1] = sY1;
            iWSMemory[iiDSChuckY2] = sY2;
            WriteData32(iWSMemoryAddress, iWSMemory);

            Stopwatch sw = Stopwatch.StartNew();

            WriteData16(iAlinger, 3);
            
            Thread.Sleep(400);

            while (bwait)
            {
                Thread.Sleep(100);

                int iCode = ReadData16(iAlinger);

                if (iCode == 2)
                {
                    bwait = false;
                }
                else if (sw.ElapsedMilliseconds > GV.PLCTimeOut)
                {
                    bwait = false;
                    bRet = false;
                }
            }
            return bRet;
        }

        public bool AlignXyyTableMove(bool ack, int xPulse, int yPulse, int degreePulse)
        {
            int iX, iY1, iY2;

            bool bRet = true;
            bool bwait = true;

            int[] TempArray = ReadData32(iRMemoryAddress, iRCount);
            Array.Copy(TempArray, iWMemory, iRCount);
            GM.DebugMessage(String.Format("{0} , {1} , {2}", xPulse, yPulse, degreePulse));
            GM.DebugMessage(String.Format("{0} , {1} , {2}", iWMemory[iiDSChuckX], iWMemory[iiDSChuckY1], iWMemory[iiDSChuckY2]));

            int xStr = degreePulse - xPulse;
            int y1Str = yPulse + degreePulse;
            int y2Str = yPulse - degreePulse;

            iWMemory[iiDSChuckX] += xStr;
            iWMemory[iiDSChuckY1] += y1Str;
            iWMemory[iiDSChuckY2] += y2Str;

            WriteData32(iWMemoryAddress, iWMemory);

            iX = Math.Abs(xStr);
            iY1 = Math.Abs(y1Str);
            iY2 = Math.Abs(y2Str);

            int iMax = iX;

            if (iY1 > iMax)
            {
                iMax = iY1;
            }
            if (iY2 > iMax)
            {
                iMax = iY2;
            }

            if (iMax < 1) iMax = 10;

            int sX = (GV.AppSettingParm.XYYTableSpeed * iX) / iMax;
            int sY1 = (GV.AppSettingParm.XYYTableSpeed * iY1) / iMax;
            int sY2 = (GV.AppSettingParm.XYYTableSpeed * iY2) / iMax;

            if (sX < GV.AppSettingParm.TableMinSpeed) sX = GV.AppSettingParm.TableMinSpeed;
            if (sY1 < GV.AppSettingParm.TableMinSpeed) sY1 = GV.AppSettingParm.TableMinSpeed;
            if (sY2 < GV.AppSettingParm.TableMinSpeed) sY2 = GV.AppSettingParm.TableMinSpeed;

            TempArray = ReadData32(iWSMemoryAddress, iRCount);
            Array.Copy(TempArray, iWSMemory, iRCount);
            iWSMemory[iiDSChuckX] = sX;
            iWSMemory[iiDSChuckY1] = sY1;
            iWSMemory[iiDSChuckY2] = sY2;
            WriteData32(iWSMemoryAddress, iWSMemory);

            Stopwatch sw = Stopwatch.StartNew();

            WriteData16(iAlinger, 3);

            while (bwait)
            {
                Thread.Sleep(100);

                int iCode = ReadData16(iAlinger);

                if (iCode == 2)
                {
                    bwait = false;
                }
                else if (sw.ElapsedMilliseconds > GV.PLCTimeOut)
                {
                    bwait = false;
                    bRet = false;
                }
            }
            return bRet;
        }

        public bool XyyTableMove(int xPulse, int y1Pulse, int y2Pulse)
        {
            int iX, iY1, iY2;

            bool bRet = true;
            bool bwait = true;

            int[] TempArray = ReadData32(iRMemoryAddress, iRCount);
            Array.Copy(TempArray, iWMemory, iRCount);

            iWMemory[iiDSChuckX] += xPulse;
            iWMemory[iiDSChuckY1] += y1Pulse;
            iWMemory[iiDSChuckY2] += y2Pulse;

            WriteData32(iWMemoryAddress, iWMemory);

            iX = Math.Abs(xPulse);
            iY1 = Math.Abs(y1Pulse);
            iY2 = Math.Abs(y2Pulse);

            int iMax = iX;

            if (iY1 > iMax)
            {
                iMax = iY1;
            }
            if (iY2 > iMax)
            {
                iMax = iY2;
            }

            if (iMax < 1) iMax = 10;

            int sX = (GV.AppSettingParm.XYYTableSpeed * iX) / iMax;
            int sY1 = (GV.AppSettingParm.XYYTableSpeed * iY1) / iMax;
            int sY2 = (GV.AppSettingParm.XYYTableSpeed * iY2) / iMax;

            if (sX < GV.AppSettingParm.TableMinSpeed) sX = GV.AppSettingParm.TableMinSpeed;
            if (sY1 < GV.AppSettingParm.TableMinSpeed) sY1 = GV.AppSettingParm.TableMinSpeed;
            if (sY2 < GV.AppSettingParm.TableMinSpeed) sY2 = GV.AppSettingParm.TableMinSpeed;

            TempArray = ReadData32(iWSMemoryAddress, iRCount);
            Array.Copy(TempArray, iWSMemory, iRCount);
            iWSMemory[iiDSChuckX] = sX;
            iWSMemory[iiDSChuckY1] = sY1;
            iWSMemory[iiDSChuckY2] = sY2;
            WriteData32(iWSMemoryAddress, iWSMemory);

            Stopwatch sw = Stopwatch.StartNew();

            WriteMemory(iUpCCDAlignStart, true);

            while (bwait)
            {
                Thread.Sleep(100);

                if ((ReadMemory(iUpCCDEnd)) || (!ReadMemory(iUpCCDStart)))
                {
                    bwait = false;
                    WriteMemory(iUpCCDAlignACK, true);
                }

                if (sw.ElapsedMilliseconds > GV.PLCTimeOut)
                {
                    SendAlignNG();
                    bwait = false;
                    bRet = false;
                    // throw new MRException("Error : Camera motor timeout");  
                }
            }
            return bRet;
        }

        public bool XyyTableMoveAbs(int xPulse, int y1Pulse, int y2Pulse)
        {
            int iX, iY1, iY2;

            bool bRet = true;
            bool bwait = true;

            int[] TempArray = ReadData32(iRMemoryAddress, iRCount);
            Array.Copy(TempArray, iWMemory, iRCount);

            int dx = iWMemory[iiDSChuckX] - xPulse;
            int dy1 = iWMemory[iiDSChuckY1] - y1Pulse;
            int dy2 = iWMemory[iiDSChuckY2] - y2Pulse;

            iWMemory[iiDSChuckX] = xPulse;
            iWMemory[iiDSChuckY1] = y1Pulse;
            iWMemory[iiDSChuckY2] = y2Pulse;

            WriteData32(iWMemoryAddress, iWMemory);

            iX = Math.Abs(dx);
            iY1 = Math.Abs(dy1);
            iY2 = Math.Abs(dy2);

            int iMax = iX;

            if (iY1 > iMax)
            {
                iMax = iY1;
            }
            if (iY2 > iMax)
            {
                iMax = iY2;
            }

            if (iMax < 1) iMax = 10;

            int sX = (GV.AppSettingParm.XYYTableSpeed * iX) / iMax;
            int sY1 = (GV.AppSettingParm.XYYTableSpeed * iY1) / iMax;
            int sY2 = (GV.AppSettingParm.XYYTableSpeed * iY2) / iMax;

            if (sX < GV.AppSettingParm.TableMinSpeed) sX = GV.AppSettingParm.TableMinSpeed;
            if (sY1 < GV.AppSettingParm.TableMinSpeed) sY1 = GV.AppSettingParm.TableMinSpeed;
            if (sY2 < GV.AppSettingParm.TableMinSpeed) sY2 = GV.AppSettingParm.TableMinSpeed;

            TempArray = ReadData32(iWSMemoryAddress, iRCount);
            Array.Copy(TempArray, iWSMemory, iRCount);
            iWSMemory[iiDSChuckX] = sX;
            iWSMemory[iiDSChuckY1] = sY1;
            iWSMemory[iiDSChuckY2] = sY2;
            WriteData32(iWSMemoryAddress, iWSMemory);

            Stopwatch sw = Stopwatch.StartNew();

            WriteMemory(iUpCCDAlignStart, true);

            while (bwait)
            {
                Thread.Sleep(100);

                if ((ReadMemory(iUpCCDEnd)) || (!ReadMemory(iUpCCDStart)))
                {
                    bwait = false;
                    // WriteMemory(iUpCCDAlignStart, false);
                }

                if (sw.ElapsedMilliseconds > GV.PLCTimeOut)
                {
                    SendAlignNG();
                    bwait = false;
                    bRet = false;
                    // throw new MRException("Error : Camera motor timeout");  
                }
            }
            return bRet;
        }

        public bool WaitforExposure()
        {
            bool bRet = true;
            bool bwait = true;

            Stopwatch sw = Stopwatch.StartNew();

            while (bwait)
            {
                Thread.Sleep(100);

                if (ReadMemory(iExposure))
                {
                    bwait = false;
                }

                if (sw.ElapsedMilliseconds > GV.PLCTimeOut)
                {
                    bwait = false;
                    bRet = false;
                }
            }
            return bRet;
        }

        public bool AlignXyyTableMove(int UpBack, int xPulse, int yPulse, int degreePulse)
        {
            if (UpBack == 1)
            {
                return AlignXyyTableMove(xPulse, yPulse, degreePulse);
            }
            else if (UpBack == 2)
            {
                return AlignXyyTableMoveDown(xPulse, yPulse, degreePulse);
            }
            else
            {
                return false;
            }
        
        }

        public bool AlignXyyTableMovePad(int xPulse, int yPulse, int degreePulse)
        {
            Stopwatch sw = Stopwatch.StartNew();

            int ix = -xPulse;
            int iy1 = yPulse;
            int iy2 = degreePulse;

            double dRetR = (double)GV.NowAlignCondition.PatternCenterDistanceUm / 2000;

            double dx = (double)ix / 100000;
            double dy1 = (double)iy1 / 100000;
            double dy2 = (double)iy2 / 100000;

            int iRetU = ReciveD(GV.Plc.iDSSChuckX);
            int iRetV = ReciveD(GV.Plc.iDSSChuckY1);
            int iRetW = ReciveD(GV.Plc.iDSSChuckY2);

            double dRU = (-1 * dy1) * Math.Cos(30 * (Math.PI / 180)) + (-1 * dx) * Math.Sin(30 * (Math.PI / 180)) + dRetR * Math.Sin(dy2 * (Math.PI / 180));

            double dRV = dy1 * Math.Cos(30 * (Math.PI / 180)) + (-1 * dx) * Math.Sin(30 * (Math.PI / 180)) + dRetR * Math.Sin(dy2 * (Math.PI / 180));

            double dRW = dx + dRetR * Math.Sin(dy2 * (Math.PI / 180));

            int iRu = (int)(dRU * 100000);
            int iRv = (int)(dRV * 100000);
            int iRw = (int)(dRW * 100000);

            if (iMoveBU > 0)
            {
                if (iRu < 0)
                {
                    iRu -= ChuckOffset;
                }
            }
            else
            {
                if (iRu > 0)
                {
                    iRu += ChuckOffset;
                }
            }
            iMoveBU = iRu;

            if (iMoveBV > 0)
            {
                if (iRv < 0)
                {
                    iRv -= ChuckOffset;
                }
            }
            else
            {
                if (iRv > 0)
                {
                    iRv += ChuckOffset;
                }
            }
            iMoveBV = iRv;

            if (iMoveBW > 0)
            {
                if (iRw < 0)
                {
                    iRw -= ChuckOffset;
                }
            }
            else
            {
                if (iRw > 0)
                {
                    iRw += ChuckOffset;
                }
            }
            iMoveBW = iRw;

            McSend(iDScChuckX, iRetU + iRu);
            McSend(iDScChuckY1, iRetV + iRv);
            McSend(iDScChuckY2, iRetW + iRw);

            SendM(iPadMoveChuck, 1);

            while (true)
            {
                Thread.Sleep(100);

                if (ReciveM(iPadMoveOkChuck) == 1)
                {
                    SendM(iPadMoveChuck, 0);
                    break;
                }
                else if (iPadMoveStop == 1)
                {
                    return false;
                }
                else if (sw.ElapsedMilliseconds > GV.PLCTimeOut)
                {
                    return false;
                }
            }
            return true;
        }

        public bool MoveStop()
        {
            bool bWaitStop = true;
            bool bRet = true;

            int iRet = 0;
            int iWaitTime = 0;

            SendM(iPadMoveUp, 0);
            SendM(iPadMoveDown, 0);
            SendM(iPadMoveChuck, 0);
            SendM(iPadMoveStop, 1);

            while (bWaitStop)
            {
                iRet = ReciveM(iPadMoveStopOk);

                if (iRet == 1)
                {
                    bWaitStop = false;
                }
                else
                {
                    Thread.Sleep(250);
                    iWaitTime += 1;
                    if (iWaitTime > 20)
                    {
                        bWaitStop = false;
                        bRet = false;
                    }
                }
            }

            Thread.Sleep(500);

            SendM(iPadMoveStop, 0);

            return bRet;
        }

        public bool AlignXyyTableMoveUp(int xPulse, int yPulse, int degreePulse)
        {
            Stopwatch sw = Stopwatch.StartNew();

            int ix = xPulse;
            int iy1 = yPulse;
            int iy2 = degreePulse;

            double dRetR = (double)GV.NowAlignCondition.PatternCenterDistanceUm / 2000;

            double dx = (double)ix / 100000;
            double dy1 = (double)iy1 / 100000;
            double dy2 = (double)iy2 / 100000;

            int iRetU = GV.Plc.ReciveD(GV.Plc.iDSSChuckX);
            int iRetV = GV.Plc.ReciveD(GV.Plc.iDSSChuckY1);
            int iRetW = GV.Plc.ReciveD(GV.Plc.iDSSChuckY2);

            double dRU = (-1 * dy1) * Math.Cos(30 * (Math.PI / 180)) + (-1 * dx) * Math.Sin(30 * (Math.PI / 180)) + dRetR * Math.Sin(dy2 * (Math.PI / 180));

            double dRV = dy1 * Math.Cos(30 * (Math.PI / 180)) + (-1 * dx) * Math.Sin(30 * (Math.PI / 180)) + dRetR * Math.Sin(dy2 * (Math.PI / 180));

            double dRW = dx + dRetR * Math.Sin(dy2 * (Math.PI / 180));

            int iRu = (int)(dRU * 100000);
            int iRv = (int)(dRV * 100000);
            int iRw = (int)(dRW * 100000);

            McSend(iDSChuckX, iRetU + iRu);
            McSend(iDSChuckY1, iRetV + iRv);
            McSend(iDSChuckY2, iRetW + iRw);

            SendM(iChuckStart, 1);

            while (true)
            {
                Thread.Sleep(100);
                if (ReciveM(iChuckok) == 1)
                {
                    SendM(iChuckStart, 0);
                    break;
                }
                else if (sw.ElapsedMilliseconds > GV.PLCTimeOut)
                {
                    SendAlignNG();
                    throw new MRException("Error : xyyTable timeout");
                }
            }
            return true;
        }

        public bool AlignXyyTableMoveDown(int xPulse, int yPulse, int degreePulse)
        {
            int iX, iY1, iY2;

            bool bRet = true;
            bool bwait = true;

            Stopwatch sw = Stopwatch.StartNew();

            int[] TempArray = ReadData32(iRMemoryAddress, iRCount);
            Array.Copy(TempArray, iRMemory, iRCount);
            Array.Copy(iRMemory, iWMemory, iRCount);

            int xStr = degreePulse - xPulse;
            int y1Str = yPulse + degreePulse;
            int y2Str = yPulse - degreePulse;

            iWMemory[iiDSChuckX] += xStr;
            iWMemory[iiDSChuckY1] += y1Str;
            iWMemory[iiDSChuckY2] += y2Str;

            WriteData32(iWMemoryAddress, iWMemory);

            iX = Math.Abs(xStr);
            iY1 = Math.Abs(y1Str);
            iY2 = Math.Abs(y2Str);

            int iMax = iX;

            if (iY1 > iMax)
            {
                iMax = iY1;
            }
            if (iY2 > iMax)
            {
                iMax = iY2;
            }

            if (iMax < 1) iMax = 1;

            int sX = (GV.AppSettingParm.XYYTableSpeed * iX) / iMax;
            int sY1 = (GV.AppSettingParm.XYYTableSpeed * iY1) / iMax;
            int sY2 = (GV.AppSettingParm.XYYTableSpeed * iY2) / iMax;

            if (sX < GV.AppSettingParm.TableMinSpeed) sX = GV.AppSettingParm.TableMinSpeed;
            if (sY1 < GV.AppSettingParm.TableMinSpeed) sY1 = GV.AppSettingParm.TableMinSpeed;
            if (sY2 < GV.AppSettingParm.TableMinSpeed) sY2 = GV.AppSettingParm.TableMinSpeed;

            TempArray = ReadData32(iWSMemoryAddress, iRCount);
            Array.Copy(TempArray, iWSMemory, iRCount);

            iWSMemory[iiDSChuckX] = sX;
            iWSMemory[iiDSChuckY1] = sY1;
            iWSMemory[iiDSChuckY2] = sY2;
            
            WriteData32(iWSMemoryAddress, iWSMemory);

            WriteData16(iAlinger, 3);

            Thread.Sleep(250);

            while (bwait)
            {
                Thread.Sleep(100);

                int iCode = ReadData16(iAlinger);

                if (iCode == 2)
                {
                    bwait = false;
                    bRet = true;
                }
                else if (sw.ElapsedMilliseconds > GV.PLCTimeOut)
                {
                    SendAlignNG();
                    bwait = false;
                    bRet = false;
                }
            }
            return bRet;
        }

        public bool AlignCameraMove(int iupDown, double leftX, double rightX, double leftY, double rightY)
        {
            bool bRet = true;
            bool bwait = true;
            int iEnable = iUpCCDStart;
            int iStart = iAlinger;
            int iEnd = iUpCCDEnd;

            int[] tempR = ReadData32(iRMemoryAddress, iRCount);
            Array.Copy(tempR, iWMemory, iRCount);

            int ileftX = (int)leftX;
            int irightX = (int)rightX;
            int ileftY = (int)leftY;
            int irightY = (int)rightY;

            if (iupDown == 0)
            {
                iWMemory[iiDSUpLeftX] += ileftX;
                iWMemory[iiDSUpLeftY] += ileftY;
                iWMemory[iiDSUpRightX] += irightX;
                iWMemory[iiDSUpRightY] += irightY;
            } else if (iupDown == 1)
            {
                iWMemory[iiDSDownLeftX] += ileftX;
                iWMemory[iiDSDownLeftY] += ileftY;
                iWMemory[iiDSDownRightX] += irightX;
                iWMemory[iiDSDownRightY] += irightY;

            } else if (iupDown == 2)
            {
                iWMemory[iiDSDownLeftX] += ileftX;
                iWMemory[iiDSDownLeftY] += ileftY;
                iWMemory[iiDSDownRightX] += irightX;
                iWMemory[iiDSDownRightY] += irightY;
                iStart = iMaskMemory;
            }

            WriteData32(iWMemoryAddress, iWMemory);

            Thread.Sleep(250);

            Stopwatch sw = Stopwatch.StartNew();

            WriteData16(iStart, 3);

            Thread.Sleep(250);

            while (bwait)
            {
                Thread.Sleep(100);

                int iCode = ReadData16(iStart);

                if (iCode == 2)
                {
                    bwait = false;
                    bRet = true;
                }
                else if (sw.ElapsedMilliseconds > GV.PLCTimeOut)
                {
                    bwait = false;
                    bRet = false;
                }
            }
            return bRet;
        }

        public bool AlignCameraMove(int iupDown, double leftX, double rightX, double leftY, double rightY, double leftZ, double rightZ)
        {
            bool bRet = true;
            bool bwait = true;
            int iEnable = iUpCCDStart;
            int iStart = iUpCCDAlignStart;
            int iEnd = iUpCCDEnd;

            int[] tempR = ReadData32(iRMemoryAddress, iRCount);
            Array.Copy(tempR, iWMemory, iRCount);

            int ileftX = (int)leftX;
            int irightX = (int)rightX;
            int ileftY = (int)leftY;
            int irightY = (int)rightY;
            int ileftZ = (int)leftZ;
            int irightZ = (int)rightZ;

            if (iupDown == 1)
            {
                if (NowAction[iAMaskStart])
                {
                    iEnable = iMaskStart;
                    iStart = iMaskMoveStart;
                    iEnd = iMaskEnd;
                }

                iWMemory[iiDSDownLeftX] += ileftX;
                iWMemory[iiDSDownLeftY] += ileftY;
                iWMemory[iiDSDownLeftZ] += ileftZ;
                iWMemory[iiDSDownRightX] += irightX;
                iWMemory[iiDSDownRightY] += irightY;
                iWMemory[iiDSDownRightZ] += irightZ;

            }
            else
            {
                iWMemory[iiDSUpLeftX] += ileftX;
                iWMemory[iiDSUpLeftY] += ileftY;
                iWMemory[iiDSUpLeftZ] += ileftZ;
                iWMemory[iiDSUpRightX] += irightX;
                iWMemory[iiDSUpRightY] += irightY;
                iWMemory[iiDSUpRightZ] += irightZ;
            }

            WriteData32(iWMemoryAddress, iWMemory);

            Stopwatch sw = Stopwatch.StartNew();

            WriteMemory(iStart, true);

            while (bwait)
            {
                Thread.Sleep(100);

                if (ReadMemory(iEnd))
                {
                    bwait = false;
                    WriteMemory(iUpCCDAlignACK, true);
                }
                else if (!ReadMemory(iEnable))
                {
                    bwait = false;
                    bRet = false;
                    // WriteMemory(iStart, false);
                }
                else if (sw.ElapsedMilliseconds > GV.PLCTimeOut)
                {
                    SendAlignNG();
                    bwait = false;
                    bRet = false;
                    // throw new MRException("Error : Camera motor timeout");  
                }
            }
            return bRet;
        }

        public bool AlignDownCameraMove(int iMask, double leftX, double rightX, double leftY, double rightY)
        {
            bool bRet = true;
            bool bwait = true;
            int iStart = iMaskMemory;
            int iEnd = iMaskEnd;
            int iACK = iBackMaskACK;

            Stopwatch sw = Stopwatch.StartNew();

            int[] tempR = ReadData32(iRMemoryAddress, iRCount);
            Array.Copy(tempR, iWMemory, iRCount);

            int ileftX = (int)leftX;
            int irightX = (int)rightX;
            int ileftY = (int)leftY;
            int irightY = (int)rightY;

            iWMemory[iiDSDownLeftX] += ileftX;
            iWMemory[iiDSDownLeftY] += ileftY;
            iWMemory[iiDSDownRightX] += irightX;
            iWMemory[iiDSDownRightY] += irightY;

            WriteData32(iWMemoryAddress, iWMemory);

            WriteData16(iStart, 3);

            Thread.Sleep(250);

            while (bwait)
            {
                Thread.Sleep(100);

                int iAlig = ReadData16(iStart);
                
                if (iAlig == 2)
                {
                    bwait = false;
                } else if (iAlig == 0)
                {
                    bwait = false;
                    bRet = false;
                }

                if (sw.ElapsedMilliseconds > GV.PLCTimeOut)
                {
                    SendAlignNG();
                    bwait = false;
                    bRet = false;
                }
            }
            return bRet;
        }

        public bool AlignDownCameraMoveZ(int iMask, double leftX, double rightX, double leftY, double rightY, double leftZ, double rightZ)
        {
            bool bRet = true;
            bool bwait = true;
            int iStart = iMaskMemory;
            int iEnd = iMaskEnd;

            Stopwatch sw = Stopwatch.StartNew();

            int[] tempR = ReadData32(iRMemoryAddress, iRCount);
            Array.Copy(tempR, iWMemory, iRCount);

            int ileftX = (int)leftX;
            int irightX = (int)rightX;
            int ileftY = (int)leftY;
            int irightY = (int)rightY;
            int ileftZ = (int)leftZ;
            int irightZ = (int)rightZ;

            iWMemory[iiDSDownLeftX] += ileftX;
            iWMemory[iiDSDownLeftY] += ileftY;
            iWMemory[iiDSDownRightX] += irightX;
            iWMemory[iiDSDownRightY] += irightY;
            iWMemory[iiDSDownLeftZ] += ileftZ;
            iWMemory[iiDSDownRightZ] += irightZ;

            WriteData32(iWMemoryAddress, iWMemory);

            WriteData16(iStart, 3);

            Thread.Sleep(250);

            while (bwait)
            {
                Thread.Sleep(100);

                int iMaskCode = ReadData16(iStart);
                if (iMaskCode == 2)
                {
                    bwait = false;
                } else if (iMaskCode == 0)
                {
                    bwait = false;
                    bRet = false;
                }

                if (sw.ElapsedMilliseconds > GV.PLCTimeOut)
                {
                    SendAlignNG();
                    bwait = false;
                    bRet = false;
                }
            }
            return bRet;
        }

        public bool AlignDownCameraMoveZAbs(int iMask, double leftX, double rightX, double leftY, double rightY, double leftZ, double rightZ)
        {
            bool bRet = true;
            bool bwait = true;
            int iStart = iBackMaskStart;
            int iEnd = iMaskEnd;

            Stopwatch sw = Stopwatch.StartNew();

            int[] tempR = ReadData32(iRMemoryAddress, iRCount);
            Array.Copy(tempR, iWMemory, iRCount);

            int ileftX = (int)leftX;
            int irightX = (int)rightX;
            int ileftY = (int)leftY;
            int irightY = (int)rightY;
            int ileftZ = (int)leftZ;
            int irightZ = (int)rightZ;

            iDownLHigh = iWMemory[iiDSDownLeftZ];
            iDownRHigh = iWMemory[iiDSDownRightZ];

            iWMemory[iiDSDownLeftX] += ileftX;
            iWMemory[iiDSDownLeftY] += ileftY;
            iWMemory[iiDSDownRightX] += irightX;
            iWMemory[iiDSDownRightY] += irightY;
            iWMemory[iiDSDownLeftZ] = ileftZ;
            iWMemory[iiDSDownRightZ] = irightZ;

            WriteData32(iWMemoryAddress, iWMemory);

            WriteMemory(iStart, true);

            while (bwait)
            {
                Thread.Sleep(100);

                if (ReadMemory(iEnd))
                {
                    bwait = false;
                    WriteMemory(iStart, false);
                }

                if (sw.ElapsedMilliseconds > GV.PLCTimeOut)
                {
                    SendAlignNG();
                    bwait = false;
                    bRet = false;
                    // throw new MRException("Error : Camera motor timeout");  
                }
            }
            return bRet;
        }

        public bool AlignDownCameraMoveZAbsW(int iMask, double leftX, double rightX, double leftY, double rightY, double leftZ, double rightZ)
        {
            bool bRet = true;
            bool bwait = true;
            int iStart = iAlinger;
            // int iEnd = iUpCCDEnd;

            if (iMask == 0)
            {
                iStart = iMaskMemory;
                // iEnd = iMaskEnd;
            }

            Stopwatch sw = Stopwatch.StartNew();

            int[] tempR = ReadData32(iRMemoryAddress, iRCount);
            Array.Copy(tempR, iWMemory, iRCount);

            int ileftX = (int)leftX;
            int irightX = (int)rightX;
            int ileftY = (int)leftY;
            int irightY = (int)rightY;
            int ileftZ = (int)leftZ;
            int irightZ = (int)rightZ;

            iDownLHigh = iWMemory[iiDSDownLeftZ];
            iDownRHigh = iWMemory[iiDSDownRightZ];

            iWMemory[iiDSDownLeftX] += ileftX;
            iWMemory[iiDSDownLeftY] += ileftY;
            iWMemory[iiDSDownRightX] += irightX;
            iWMemory[iiDSDownRightY] += irightY;
            iWMemory[iiDSDownLeftZ] = ileftZ;
            iWMemory[iiDSDownRightZ] = irightZ;

            WriteData32(iWMemoryAddress, iWMemory);

            WriteData16(iStart, 3);

            Thread.Sleep(350);

            while (bwait)
            {
                Thread.Sleep(100);

                int iCode = ReadData16(iStart);

                if (iCode == 2)
                {
                    bwait = false;
                    bRet = true;
                }
                else if (sw.ElapsedMilliseconds > GV.PLCTimeOut)
                {
                    // SendAlignNG();
                    bwait = false;
                    bRet = false;
                }
            }
            return bRet;
        }

        public void DownCameraZToHigh(int iWaferMask)
        {
            GetRecipe();
            int lz;
            int rz;

            if (iWaferMask == 1)
            {
                lz = GV.Plc.RecipeData[43];
                rz = GV.Plc.RecipeData[44];
            } else
            {
                lz = GV.Plc.RecipeData[45];
                rz = GV.Plc.RecipeData[46];
            }

            GV.Plc.AlignDownCameraMoveZAbsW(0, 0, 0, 0, 0, lz, rz);
        }
        public bool MoveCCDToRecipe()
        {
            bool bRet = true;
            bool bwait = true;

            Stopwatch sw = Stopwatch.StartNew();

            int[] tempR = ReadData32(iRMemoryAddress, iRCount);
            Array.Copy(tempR, iWMemory, iRCount);

            iWMemory[iiDSBigY] = 2120000;
            iWMemory[iiDSUpLeftX] = 717959;
            iWMemory[iiDSUpLeftY] = 10809;
            iWMemory[iiDSUpRightX] = 485108;
            iWMemory[iiDSUpRightY] = -17660;

            // iWMemory[iiDScChuckX] = -16108;
            // iWMemory[iiDScChuckY1] = 19808;
            // iWMemory[iiDScChuckY2] = -41808;

            WriteData32(iWMemoryAddress, iWMemory);

            WriteMemory(iUpCCDAlignStart, true);

            while (bwait)
            {
                Thread.Sleep(100);

                if (ReadMemory(iUpCCDEnd))
                {
                    bwait = false;
                    // WriteMemory(iUpCCDAlignStart, false);
                }

                if (sw.ElapsedMilliseconds > GV.PLCTimeOut)
                {
                    SendAlignNG();
                    bwait = false;
                    bRet = false;
                    // throw new MRException("Error : Camera motor timeout");  
                }
            }
            return bRet;
        }

        public bool MoveCCDChuckToRecipe()
        {
            int ilRecipeChuckX = GV.NowRecipe.iWECX;
            int ilRecipeChuckY1 = GV.NowRecipe.iWECY1;
            int ilRecipeChuckY2 = GV.NowRecipe.iWECY2;

            bool bRet = true;
            bool bwait = true;

            Stopwatch sw = Stopwatch.StartNew();

            int[] tempR = ReadData32(iRMemoryAddress, iRCount);
            Array.Copy(tempR, iWMemory, iRCount);

            iWMemory[iiDSBigY] = GV.NowRecipe.iUpCCDBigY;
            iWMemory[iiDSUpLeftX] = GV.NowRecipe.iUpCCDLeftX;
            iWMemory[iiDSUpLeftY] = GV.NowRecipe.iUpCCDLeftY;
            iWMemory[iiDSUpRightX] = GV.NowRecipe.iUpCCDRightX;
            iWMemory[iiDSUpRightY] = GV.NowRecipe.iUpCCDRightY;

            iWMemory[iiDSChuckX] = ilRecipeChuckX;
            iWMemory[iiDSChuckY1] = ilRecipeChuckY1;
            iWMemory[iiDSChuckY2] = ilRecipeChuckY2;

            WriteData32(iWMemoryAddress, iWMemory);

            int x = iWMemory[iiDSChuckX];
            int y1 = iWMemory[iiDSChuckY1];
            int y2 = iWMemory[iiDSChuckY2];

            double dx = Math.Abs(x - ilRecipeChuckX);
            double dy1 = Math.Abs(y1 - ilRecipeChuckY1);
            double dy2 = Math.Abs(y2 - ilRecipeChuckY2);

            double dmax = dx;
            if (dy1 > dmax) dmax = dy1;
            if (dy2 > dmax) dmax = dy2;

            int sX = (int)((GV.AppSettingParm.XYYTableSpeed * dx) / dmax);
            int sY1 = (int)((GV.AppSettingParm.XYYTableSpeed * dy1) / dmax);
            int sY2 = (int)((GV.AppSettingParm.XYYTableSpeed * dy2) / dmax);

            if (sX < GV.AppSettingParm.TableMinSpeed) sX = GV.AppSettingParm.TableMinSpeed;
            if (sY1 < GV.AppSettingParm.TableMinSpeed) sY1 = GV.AppSettingParm.TableMinSpeed;
            if (sY2 < GV.AppSettingParm.TableMinSpeed) sY2 = GV.AppSettingParm.TableMinSpeed;

            tempR = ReadData32(iRMemoryAddress, iRCount);
            Array.Copy(tempR, iWSMemory, iRCount);
            
            iWSMemory[iiDSChuckX] = sX;
            iWSMemory[iiDSChuckY1] = sY1;
            iWSMemory[iiDSChuckY2] = sY2;

            WriteMemory(iUpCCDAlignStart, true);

            while (bwait)
            {
                Thread.Sleep(100);

                if (ReadMemory(iUpCCDEnd))
                {
                    bwait = false;
                    // WriteMemory(iUpCCDAlignStart, false);
                }

                if (sw.ElapsedMilliseconds > GV.PLCTimeOut)
                {
                    SendAlignNG();
                    bwait = false;
                    bRet = false;
                    // throw new MRException("Error : Camera motor timeout");  
                }
            }
            return bRet;
        }
        public bool RecipeModeXyyTableMove(int xPulse, int yPulse, int degreePulse)
        {
            // lock (_locker)
            // {
            Stopwatch sw = Stopwatch.StartNew(); // 68
                                                 //sw.Reset();
            if (Send("RD MR18300") == "0\r\n" || Send("RD MR12701") == "0\r\n" || Send("RD MR31302") == "0\r\n")
                throw new MRException("Not in recipe page or leveling unfinished");

            Send("WR MR15113 1");
            while (true)
            {
                if (Send("RD MR15114") == "1\r\n")
                    break;
                if (sw.ElapsedMilliseconds > GV.PLCTimeOut)
                    throw new MRException("Enter measure mark distance mode time out");
            }

            string xStr = ((xPulse + degreePulse) * -1).ToString();
            string y1Str = (yPulse + degreePulse * -1).ToString();
            string y2Str = (yPulse + degreePulse).ToString();
            Send("WR DM6802.L " + xStr);
            Send("WR DM6804.L " + y1Str);
            Send("WR DM6806.L " + y2Str);
            Send("WR DM6801.S 1");
            while (true)
            {
                if (Send("RD DM6801.S") == "+00000\r\n")
                    break;
                if (sw.ElapsedMilliseconds > GV.PLCTimeOut)
                    throw new MRException("Error : xyyTable timeout");
                Thread.Sleep(50);
            }
            Send("WR MR15113 0");
            while (true)
            {
                if (Send("RD MR15114") == "0\r\n")
                    break;
                if (sw.ElapsedMilliseconds > GV.PLCTimeOut)
                    throw new MRException("Quit measure mark distance mode time out");
            }
            // }
            return true;
        }

        public bool CorrectionXyyTableMove(int xPulse, int yPulse, int degreePulse)
        {
            // lock (_locker)
            // {
            ChangeXyyTableSpeed(7000);
            Stopwatch sw = Stopwatch.StartNew();
            sw.Reset();
            if (Send("RD DM6300.S") != "+00001\r\n")
            {

                throw new MRException("Not in correction mode");
            }
            Send("WR DM6602.L " + "0");
            Send("WR DM6604.L " + "0");
            Send("WR DM6608.L " + "0");
            Send("WR DM6610.L " + "0");

            string xStr = ((xPulse + degreePulse) * -1).ToString();
            string y1Str = (yPulse + degreePulse * -1).ToString();
            string y2Str = (yPulse + degreePulse).ToString();
            Send("WR DM6302.L " + xStr);
            Send("WR DM6304.L " + y1Str);
            Send("WR DM6306.L " + y2Str);
            Send("WR DM6301.S 1");
            while (true)
            {
                if (Send("RD DM6301.S") == "+00000\r\n")
                    break;
                if (sw.ElapsedMilliseconds > GV.PLCTimeOut)
                    throw new MRException("Error : xyyTable timeout");
                Thread.Sleep(50);
            }
            return true;
            // }
        }

        public bool CorrectionCameraMove(int leftX, int rightX, int leftY, int rightY)
        {
            // lock (_locker)
            // {
            Stopwatch sw = Stopwatch.StartNew();

            if (!ReadMemory(iCCDAdjust))
            {

                throw new MRException("Not in correction mode");
            }

            // leftX *= -1;
            // leftY *= -1;
            // rightX *= -1;
            // 72.8rightY *= -1;

            int[] TempArry = ReadData32(iRMemoryAddress, iRCount);
            Array.Copy(TempArry, iRMemory, iRCount);
            Array.Copy(iRMemory, iWMemory, iRCount);

            iWMemory[iiDSUpLeftX] += leftX;
            iWMemory[iiDSUpLeftY] += leftY;
            iWMemory[iiDSUpRightX] += rightX;
            iWMemory[iiDSUpRightY] += rightY;

            WriteData32(iWMemoryAddress, iWCount);

            WriteMemory(iCCDAdjMoveStart, true);

            while (true)
            {
                if (ReadMemory(iCCDAdjEnd))
                    break;
                if (sw.ElapsedMilliseconds > GV.PLCTimeOut)
                    throw new MRException("Error : Camera motor timeout");
                Thread.Sleep(50);
            }
            return true;
        }

        public void TellPlcToEnterContactUnContactMode()
        {
            Stopwatch sw = Stopwatch.StartNew();
            GV.Plc.Send("WR MR8612 1");
            while (true)
            {
                if (GV.Plc.Send("RD MR8612") == "0\r\n")
                    break;
                if (sw.ElapsedMilliseconds > GV.PLCTimeOut)
                {
                    Debug.WriteLine("Enter Contact/Uncontact Mode time out");
                    throw new MRException("Enter Contact/Uncontact Mode time out");
                }
            }
        }

        public void Contact()
        {
            int iWaitTime = 0;

            bool bWait = true;
            // bool bRet = false;

            int iOKaddress = iUpCCDAlignOK;

            SendM(iAlignOK, 1);

            while (bWait)
            {
                if (ReciveM(iAlignOK) == 0)
                {
                    SendM(iAlignOK, 1);
                }

                if (ReciveM(iExposureOK) == 1)
                {
                    // bRet = true;
                    // SendM(iExposureStart, 0);
                    SendM(iAlignOK, 0);
                    break;
                }
                Thread.Sleep(250);
                iWaitTime += 1;
                if (iWaitTime > 240)
                {
                    bWait = false;
                    // bRet = false;
                }
            }
            return;
        }

        public bool Contact(int iUpBack)
        {
            int iWaitTime = 0;

            bool bWait = true;
            bool bRet = false;
            int iRet;

            int iOKaddress = iAlinger;
            int iContactAddress = iTopContactOK;

            if (iUpBack == 2)
            {
                iOKaddress = iAlign;
                iContactAddress = iBOTContactOK;
            }
            
            WriteData16(iOKaddress, 5);

            while (bWait)
            {
                Thread.Sleep(250);

                iRet = ReadData16(iOKaddress);
    
                if (iRet == 8) 
                {
                    bRet = true;
                    bWait = false;
                } else if (iRet == 0)
                {
                    bRet = false;
                    bWait = false;
                }

                iWaitTime += 1;
                if (iWaitTime > 240)
                {
                    bWait = false;
                    bRet = false;
                }
            }
            return bRet;
        }

        public bool ContactFault(int iUpBack)
        {
            bool bRet = false;

            int iFaultAddress = iUpContactFault;

            if (iUpBack == 2)
            {
                iFaultAddress = iBackContactFault;
            }

            WriteMemory(iFaultAddress, true);

            return bRet;
        }

        public void SetManualMode()
        {
            SendM(iAuto, 0);
            SendM(iManual, 1);
            SendM(iRobotAction, 0);
            SendM(iPinCanVac, 0);
            SendM(iWaferOnLoad, 0);

            Thread.Sleep(200);
        }

        public void SetAutoMode()
        {
            SendM(iManual, 0);
            SendM(iAuto, 1);
            Thread.Sleep(200);
        }

        public void Uncontact()
        {
            Stopwatch sw = Stopwatch.StartNew();
            GV.Plc.Send("WR DM6208.S 2");
            while (true)
            {
                Thread.Sleep(50);
                if (GV.Plc.Send("RD DM6209.S") == "+00000\r\n")
                    break;
                if (sw.ElapsedMilliseconds > GV.PLCTimeOut)
                    throw new MRException("Unontact time out");
            }
            Thread.Sleep(500);
        }

        public bool Uncontact(int iTopBOT)
        {
            int iWaitTime = 0;
            int iRet;
            bool bWait = true;
            bool bRet = false;

            int iNGAddress = iAlinger;
            int iStart = iUnContactOK;

            if (iTopBOT == 2)
            {
                iNGAddress = iBackContactNG;
                iStart = iChuckStart;
            }

            WriteData16(iNGAddress, 7);

            while (bWait)
            {
                Thread.Sleep(250);
                iRet = ReadData16(iNGAddress);
                if (iRet == 2)
                {
                    bWait = false;
                    bRet = true;
                    break;
                } else if (iRet == 0)
                {
                    bWait = false;
                    bRet = false;
                    break;
                }

                iWaitTime += 1;
                if (iWaitTime > 240)
                {
                    bWait = false;
                    bRet = false;
                }
            }
            return bRet;
        }


        public void ChangeXyyTableSpeed(int speed)
        {
            Stopwatch sw = Stopwatch.StartNew();
            // GV.Plc.Send("WR DM6702.L " + speed.ToString());
            while (true)
            {
                Thread.Sleep(50);
                // if (Convert.ToInt32(GV.Plc.Send("RD DM6702.L")) == speed)
                break;
                // if (sw.ElapsedMilliseconds > 5000)
                // {
                //     Debug.WriteLine("ChangeXyyTableSpeed time out");
                //     throw new MRException("ChangeXyyTableSpeed time out");
                // }
            }
        }

        public bool Exposure()
        {
            int iWaitTime = 0;

            bool bWait = true;
            bool bRet = false;

            SendM(iAlignOK, 1);

            while (bWait)
            {
                if (ReciveM(iAlignOK) == 0)
                {
                    SendM(iAlignOK, 1);
                }

                if (ReciveM(iExposureOK) == 1)
                {
                    bRet = true;
                    // SendM(iExposureStart, 0);
                    SendM(iAlignOK, 0);
                    break;
                }
                Thread.Sleep(250);
                iWaitTime += 1;
                if (iWaitTime > 240)
                {
                    bWait = false;
                    bRet = false;
                }
            }
            return bRet;
        }

        public bool Exposure(int iUpDown)
        {
            bool bRet = true;

            int iOKaddress = iAlinger;

            if (iUpDown == 2)
            {
                iOKaddress = iAlinger;
            }

            WriteData16(iOKaddress, 9);

            return bRet;
        }

        public void Close()
        {
            if (client.Connected)
            {
                stream.Close();
                client.Close();
            }
        }

        public bool IsConnected()
        {
            bool bresult = false;
            /*
            short sYear;
            short sMonth;
            short sDay;
            short sDayOfWeek;
            short sHour;
            short sMiniute;
            short sSecond;
            */

            int hResult = 0;
            // int hResult = ActPlc.GetClockData(out sYear, out sMonth, out sDay, out sDayOfWeek, out sHour, out sMiniute, out sSecond);

            if (hResult == 0)
            {
                bresult = true;
            }

            return bresult;
        }

        public string McSend(int iaddress, int idata)
        {
            string str = "ok";
            
            int[] iw = new int[1];
            iw[0] = idata;
            WriteData32(iaddress, iw);

            return str;
        }

        public string McSendD(int iaddress, int idata)
        {
            string sRet = "";
            int[] arrDeviceValue;
            int hResult = 0;
            string sAddress = "D" + iaddress.ToString();
            arrDeviceValue = new int[2];
            arrDeviceValue[0] = idata;

            byte[] byte1 = BitConverter.GetBytes(idata);
            byte[] byte2 = new byte[4];
            byte[] byte3 = new byte[4];
            byte2[0] = byte1[0];
            byte2[1] = byte1[1];
            byte2[2] = 0;
            byte2[3] = 0;
            byte3[0] = byte1[2];
            byte3[1] = byte1[3];
            byte3[2] = 0;
            byte3[3] = 0;
            arrDeviceValue[0] = (short)BitConverter.ToInt32(byte2, 0);
            arrDeviceValue[1] = (short)BitConverter.ToInt32(byte3, 0);
            try
            {
                // hResult = ActPlc.WriteDeviceBlock(sAddress, 2, ref arrDeviceValue[0]);
                // hResult = ActPlc.WriteDeviceBlock2(sAddress, 2, arrDeviceValue[0]);

                if (hResult != 0)
                {
                    sRet = "WriteDeviceBlock Failure !!\r\n";
                }
                else
                {
                }
                return sRet;
            }
            catch
            {
                throw;
            }
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

        public void QueryReconnect()
        {
            IsOpen = false;
            //if (MessageBox.Show("Network connection is lost. Try to re-connect?", "Network Connection", MessageBoxButtons.RetryCancel, MessageBoxIcon.Stop)
            //            == DialogResult.Retry)
            //    ReConnect();
            //else
            //    Application.Exit();
        }

        public void WriteData16(int address, int value)
        {
            if (IsOpen)
            {
                byte[] numArray = new byte[] { 1, 20, 0, 0, 0, 0, 0, 168, 1, 0, 0, 0 };
                byte[] bytes = BitConverter.GetBytes(address);
                numArray[4] = bytes[0];
                numArray[5] = bytes[1];
                numArray[6] = bytes[2];
                byte[] array = PacketHeader.Concat<byte>(numArray).ToArray<byte>();
                bytes = BitConverter.GetBytes((int)array.Length - 9);
                array[7] = bytes[0];
                array[8] = bytes[1];
                bytes = BitConverter.GetBytes(value);
                array[(int)array.Length - 2] = bytes[0];
                array[(int)array.Length - 1] = bytes[1];
                lock (_locker)
                {
                    try
                    {
                        try
                        {
                            stream.Write(array, 0, (int)array.Length);
                            ReadNetwork(stream, 11, 2000);
                        }
                        catch (Exception)
                        {
                            QueryReconnect();
                        }
                    }
                    finally
                    {
                    }
                }
            }
            else
            {
                if (address == iAlinger)
                {
                    if (value == 5)
                    {
                        try
                        {
                            Thread.Sleep(500);
                            PlcDataList["D" + iAlinger.ToString()] = 8;
                        }
                        catch (Exception)
                        {

                        }
                    }
                    else if (value == 11)
                    {
                        try
                        {
                            Thread.Sleep(500);
                            PlcDataList["D" + iAlinger.ToString()] = 2;
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
        }

        public void WriteData16(int address, params int[] value)
        {
            byte[] cmd = { 0x01, 0x14,  // Write
                           0x00, 0x00   // Word
                         };


            List<byte> list = new List<byte>();
            list.AddRange(PacketHeader);
            list.AddRange(cmd);
            list.AddRange(IntToBytes3(address));
            list.Add(0xA8);         // D*
            list.AddRange(IntToBytes2(value.Length));
            for (int i = 0; i < value.Length; i++) list.AddRange(IntToBytes2(value[i]));

            byte[] packet = list.ToArray();

            int len = packet.Length - 9;
            byte[] intBytes = BitConverter.GetBytes(len);
            packet[7] = intBytes[0];
            packet[8] = intBytes[1];

            lock (_locker)
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

        public void WriteData32(int address, params int[] value)
        {
            if (IsOpen)
            {
                byte[] cmd = { 0x01, 0x14, // Write
                           0x00, 0x00  // Word
                         };

                List<byte> list = new List<byte>();
                list.AddRange(PacketHeader);
                list.AddRange(cmd);
                list.AddRange(IntToBytes3(address));
                list.Add(0xA8);         // D*
                list.AddRange(IntToBytes2(value.Length * 2));
                for (int i = 0; i < value.Length; i++) list.AddRange(IntToBytes4(value[i]));

                byte[] packet = list.ToArray();

                int len = packet.Length - 9;
                byte[] intBytes = BitConverter.GetBytes(len);
                packet[7] = intBytes[0];
                packet[8] = intBytes[1];

                lock (_locker)
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
        }

        public string MCGetMemory(int address, int size)
        {
            int i;
            int isize = size * 2;
            int[] arrDeviceValue;
            string sRet = "";
            string sAddress = "D" + address.ToString();
            arrDeviceValue = new int[isize];
            for (i = 0; i < isize; i++)
                arrDeviceValue[i] = 0;

            int hResult = 0;
            // int hResult = ActPlc.ReadDeviceBlock(sAddress, isize, out arrDeviceValue[0]);

            if (hResult != 0)
            {
                sRet = "ReadDeviceBlock Failure !!\r\n";
            }
            else
            {
                for (i = 0; i < isize; i += 2)
                {
                    byte[] byte1 = BitConverter.GetBytes(arrDeviceValue[i]);
                    byte[] byte2 = BitConverter.GetBytes(arrDeviceValue[i + 1]);
                    byte[] byte3 = new byte[4];
                    byte3[3] = byte2[1];
                    byte3[2] = byte2[0];
                    byte3[1] = byte1[1];
                    byte3[0] = byte1[0];
                    int ibyte = BitConverter.ToInt32(byte3, 0);
                    sRet = sRet + ibyte + " ";
                }
            }

            return sRet;
        }

        public void MCGetMemoryArray(int address, int size)
        {
            int i;
            int[] arrDeviceValue;
            // string sRet = "";
            string sAddress = "D" + address.ToString();
            arrDeviceValue = new int[size];
            for (i = 0; i < size; i++)
                arrDeviceValue[i] = 0;

            int hResult = 0;
            // int hResult = ActPlc.ReadDeviceBlock(sAddress, size, out arrDeviceValue[0]);

            if (hResult != 0)
            {
                // sRet = "ReadDeviceBlock Failure !!\r\n";
            }
            else
            {
                for (i = 0; i < size; i += 2)
                {
                    byte[] byte1 = BitConverter.GetBytes(arrDeviceValue[i]);
                    byte[] byte2 = BitConverter.GetBytes(arrDeviceValue[i + 1]);
                    byte[] byte3 = new byte[4];
                    byte3[3] = byte2[1];
                    byte3[2] = byte2[0];
                    byte3[1] = byte1[1];
                    byte3[0] = byte1[0];
                    int ibyte = BitConverter.ToInt32(byte3, 0);
                    // PLCm[i / 2] = ibyte;
                }
            }

            //  return sRet;
        }

        public void ClearGet()
        {
            thClearGet = new Thread(ClearGetMethod);
            thClearGet.Start();
        }

        public void ClearPut()
        {
            thClearPut = new Thread(ClearPutMethod);
            thClearPut.Start();
        }

        void ClearGetMethod()
        {
            int iRet = 1;
            // string sRet;

            bool bWait = true;
            Send("M15203", 1);

            while (bWait)
            {
                iRet = ReciveM(13200);
                if (iRet == 0)
                {
                    bWait = false;
                }
                else
                {
                    Thread.Sleep(500);
                }
                /*
                sRet = Recive("M13200");
                bool b = Int32.TryParse(sRet, out iRet);
                if (b)
                {
                    if (iRet == 0)
                    {
                        bWait = false;
                    } else
                    {
                        Thread.Sleep(500);
                    }
                } else
                {
                    Thread.Sleep(500);
                }
                */
            }
            Send("M15200", 0);
            Send("M15201", 0);
            Send("M15202", 0);
            Send("M15203", 0);
        }

        void ClearPutMethod()
        {
            int iRet = 1;
            // string sRet;

            bool bWait = true;
            Send("M15102", 1);

            while (bWait)
            {
                iRet = ReciveM(13100);

                if (iRet == 0)
                {
                    bWait = false;
                }
                else
                {
                    Thread.Sleep(500);
                }

                /*
                sRet = Recive("M13100");
                bool b = Int32.TryParse(sRet, out iRet);
                if (b)
                {
                    if (iRet == 0)
                    {
                        bWait = false;
                    } else
                    {
                        Thread.Sleep(500);
                    }
                } else
                {
                    Thread.Sleep(500);
                }
                */
            }
            Send("M15100", 0);
            Send("M15101", 0);
            Send("M15102", 0);
            Send("M15103", 0);
        }

        public void waitGetVac()
        {
            Thread.Sleep(3000);
            SendM(iWaitVac, 1);
        }

        public void waitPutVac()
        {
            Thread.Sleep(3000);
            SendM(iPinCanVac, 1);
        }

        public void SendAlignNG()
        {
            WriteData16(iAlinger, 11);
            Thread.Sleep(100);
        }

        public void SendAlignOK()
        {
            SendM(iChuckStart, 0);
            SendM(iUpCCDStart, 0);
            SendM(iDownCCDStart, 0);

            // GV.Exposure.ExposureStatus = ExposureStatusDef.ExposureStatusKind.Exposure;

            Thread.Sleep(250);
            // SendM(iAlignOK, 1);
        }

        public void ClearStatus()
        {
            SendM(iAlignNG, 0);
            SendM(iUpCCDStart, 0);
            SendM(iChuckStart, 0);
            SendM(iAlignOK, 0);
        }

        public void MaintainMode()
        {
            if (iMainModeStatus == 0)
            {
                SendM(iMaintain, 1);
                iMainModeStatus = 1;
            }
            else
            {
                SendM(iMaintain, 0);
                iMainModeStatus = 0;
            }
            Thread.Sleep(250);
        }

        public int ReadData16(int address)
        {
            int iret = 0;

            if (IsOpen)
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

                lock (_locker)
                {
                    try
                    {
                        stream.Write(Cmd, 0, Cmd.Length);
                        //byte[] data = new Byte[256];
                        //Int32 bytes = stream.Read(data, 0, data.Length); // 11 + 2
                        byte[] data = ReadNetwork(stream, 1 * 2 + 11, 2000);
                        iret = BitConverter.ToInt16(data, 11);
                        // return iret;
                    }
                    catch (Exception)
                    {
                        QueryReconnect();
                        iret = -1;
                        // return -1;
                    }
                    finally
                    {

                    }
                }
            } else
            {
                try
                {
                    iret = Convert.ToInt16(PlcDataList["D" + address.ToString()]);
                }
                catch (Exception)
                {
                    iret = -1;
                }
                // return iret;
            }
            GV.PlcStatusForm?.ReadData16(address, iret);
            return iret;
        }

        public int[] ReadData16(int address, int count)
        {
            if (IsOpen)
            {
                byte[] Data = { 0x01, 0x04,         // Read
                           0x00, 0x00,          // Word
                           0x00, 0x00, 0x00,    // Address, low to high
                           0xA8,                // D*
                           0x01, 0x00           // Units low to high
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

                lock (_locker)
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
            } else
            {
                int[] val = new int[count];
                return val;
            }
        }

        public int ReadData32(int address)
        {
            int iRet = 0;
            if (IsOpen)
            {
                byte[] Data = { 0x01, 0x04,         // Read
                           0x00, 0x00,          // Word
                           0x00, 0x00, 0x00,    // Address, low to high
                           0xA8,                // D*
                           0x01, 0x00           // Units low to high
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

                lock (_locker)
                {
                    try
                    {
                        stream.Write(Cmd, 0, Cmd.Length);
                        byte[] data1 = ReadNetwork(stream, 2 * 2 + 11, 2000);
                        iRet = BitConverter.ToInt32(data1, 11);
                    }
                    catch (Exception)
                    {
                        QueryReconnect();
                        iRet = 0;
                    }
                    finally
                    {

                    }
                }
            } else
            {
                try
                {
                    iRet = Convert.ToInt16(PlcDataList["D" + address.ToString()]);
                }
                catch (Exception)
                {
                    iRet = 0;
                }
            }
            return iRet;
        }

        public int[] ReadData32(int address, int count)
        {
            if (IsOpen)
            {
                byte[] Data = { 0x01, 0x04,         // Read
                           0x00, 0x00,          // Word
                           0x00, 0x00, 0x00,    // Address, low to high
                           0xA8,                // D*
                           0x01, 0x00           // Units low to high
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

                lock (_locker)
                {
                    try
                    {
                        stream.Write(Cmd, 0, Cmd.Length);
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
            else
            {
                int[] ret = new int[count];
                Array.Clear(ret, 0, count);
                return ret;
            }
        }

        public bool ReadMemory(int address)
        {
            bool bRet = false;

            if (IsOpen)
            {
                byte[] Data = { 0x01, 0x04,         // Read
                           0x01, 0x00,              // Bit
                           0x00, 0x00, 0x00,        // Address, low to high
                           0x90,                    // M*
                           0x01, 0x00               // Units low to high
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

                lock (_locker)
                {
                    try
                    {
                        stream.Write(Cmd, 0, Cmd.Length);
                        //byte[] data = new Byte[256];
                        //Int32 bytes = stream.Read(data, 0, data.Length);

                        byte[] data = ReadNetwork(stream, 1 * 1 + 11, 2000);
                        // return (data[11] & 0xF0) != 0;
                        bRet = ((data[11] & 0xF0) != 0);
                    }
                    catch (Exception)
                    {
                        QueryReconnect();
                        // return false;
                        bRet = false;
                    }
                    finally
                    {

                    }
                }
            }
            else
            {
                int iret = 0;
                try
                {
                    iret = Convert.ToInt16(PlcDataList["M" + address.ToString()]);
                }
                catch (Exception)
                {
                    iret = -1;
                }

                if (iret == 1)
                {
                    bRet = true;
                } else
                {
                    bRet = false;
                }
            }
            GV.PlcStatusForm?.ReadMemory(address, bRet);
            return bRet;
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

            lock (_locker)
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
            if (IsOpen)
            {
                byte[] Data = { 0x01, 0x14,         // Write
                           0x01, 0x00,          // Bit
                           0x00, 0x00, 0x00,    // Address, low to high
                           0x90,                // M*
                           0x01, 0x00,          // Units low to high
                           0x00,                // Value
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

                lock (_locker)
                {
                    try
                    {
                        stream.Write(Cmd, 0, Cmd.Length);
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

        internal void SendAlignBackNG()
        {
            // SendM(iChuckStart, 0);
            // SendM(iUpCCDStart, 0);
            // SendM(iDownCCDAlignStart, 0);
            SendM(iUpAlignNG, 1);
            SendM(iUpContactFault, 1);
            // SendM(iBackMaskNG, 1);
            Thread.Sleep(100);
        }

        public int[] GetLocation()
        {
            int i;

            int[] Loc1 = ReadData32(iRMemoryAddress, 20);

            for (i = 0; i < 20; i++)
            {
                GV.NowLocation[i] = NowLocation[i] = Loc1[i];
            }

            //bool[] Action = ReadMemory(iAction, 60);
            //for (i = 0; i < 60; i++)
            //{
            //    NowAction[i] = Action[i];
            //}

            return Loc1;
        }

        public void GetLimit()
        {
            int i;

            int[] Limit = ReadData32(iDLimitAddress, iLimitCount);

            for (i = 0; i < iLimitCount; i++) iLimit[i] = Limit[i];
        }

        internal void ZoomAdj(int oMagni, int mag)
        {
            double Lx = GV.ZoomLensInfo.LeftCameraAdjustX[mag] - GV.ZoomLensInfo.LeftCameraAdjustX[oMagni];
            double Ly = GV.ZoomLensInfo.LeftCameraAdjustY[mag] - GV.ZoomLensInfo.LeftCameraAdjustY[oMagni];
            //double Lz = 0;
            double Rx = GV.ZoomLensInfo.RightCameraAdjustX[mag] - GV.ZoomLensInfo.RightCameraAdjustX[oMagni];
            double Ry = GV.ZoomLensInfo.RightCameraAdjustY[mag] - GV.ZoomLensInfo.RightCameraAdjustY[oMagni];
            //double Rz = 0;

            //if (mag < 7)
            //{
                double Lz = GV.ZoomLensInfo.LeftCameraAdjustZ[mag] - GV.ZoomLensInfo.LeftCameraAdjustZ[oMagni];
                double Rz = GV.ZoomLensInfo.RightCameraAdjustZ[mag] - GV.ZoomLensInfo.RightCameraAdjustZ[oMagni];
            //}

            AlignCameraMove(0, Lx, Rx, Ly, Ry, Lz, Rz);
        }

        public void GetRecipe()
        {
            int[] recipe = ReadData32(iNowRecipeNumber, 50);

            for (int i = 0; i < 50; i++)
            {
                RecipeData[i] = recipe[i]; 
            }
        }
    }
}
