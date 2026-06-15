using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MRLibrary
{
    public class MvotemZoom
    {
        private object _locker = new object();
        //private int _limitPosition;
        public bool _idle;
        public int NowPosition;
        public int Magnification = 0;
        public int _Magni = 0;
        private bool isOpen = false;
        public SerialPort Com = new SerialPort();
        byte[] dataReceived = new byte[4096];
        //private int dataIndex = 0;
        private int newIndex = 0;
        private int PulseCount = 0;
        private int zPulseCount = 0;
        private int PulseCount4 = 0;
        private int iMagni = 0;
        private int isOK = 0;
        public event Action OnMoveCompleted;
        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            lock (_locker)
            {
                int bytes = Com.BytesToRead;
                if ((newIndex + bytes) < 4096)
                {
                    Com.Read(dataReceived, newIndex, bytes);
                    newIndex += bytes;
                }
                while (newIndex >= 5)
                {
                    if ((dataReceived[0] == 0xab) && (dataReceived[4] == 0xcd))
                    {
                        if (dataReceived[1] == 0x51)
                        {
                            zPulseCount = dataReceived[3] + dataReceived[2] * 256;
                            if (zPulseCount < 10) isOK = 1;
                        }
                        else if (dataReceived[1] == 0x52)
                        {
                            PulseCount = dataReceived[3] + dataReceived[2] * 256;
                            if (PulseCount < 10) isOK = 1;
                        }
                        else if (dataReceived[1] == 0x53)
                        {
                            iMagni = dataReceived[3] + dataReceived[2] * 256;
                            NowPosition = iMagni;
                        }
                        else if (dataReceived[1] == 0x54)
                        {
                            PulseCount4 = dataReceived[3] + dataReceived[2] * 256;
                            if (PulseCount4 < 10) isOK = 1;
                        }
                        newIndex -= 5;
                        byteMove(5, newIndex);
                    }
                    else
                    {
                        newIndex -= 1;
                        byteMove(1, newIndex);
                    }
                }
            }
        }

        public void byteMove(int iStart, int ilenght)
        {
            int j = iStart;
            for (int i = 0; i < ilenght; i++)
            {
                dataReceived[i] = dataReceived[j++];
            }
        }

        public MvotemZoom()
        {
            Com.ReadTimeout = 3000;
            Com.WriteTimeout = 3000;
            Com.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
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
                isOpen = true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Close()
        {
            try
            {
                if (Com.IsOpen)
                    Com.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Connected
        {
            get { return isOpen; }
        }

        public int GetMaxLength()
        {
            return 12;
        }

        public int GetPulseCount()
        {
            return PulseCount;
        }

        public int GetPulseCount4()
        {
            return PulseCount4;
        }

        public int GetMagni()
        {
            return iMagni;
        }

        public bool MoveHome()
        {
            byte[] byteSendMsg = {
                0xAB,       // 指令標頭
                0xE5,       // 歸零
                0x00,       // 保留
                0x00,       // 保留
                0xCD        // 指令結尾
                };

            if (isOpen)
            {
                lock (_locker)
                {
                    try
                    {
                        string resultSendMsg = string.Empty;
                        Com.Write(byteSendMsg, 0, byteSendMsg.Length);
                        Thread.Sleep(250);
                    }
                    catch
                    {
                        throw;
                    }
                }
            }

            _Magni = 0;

            return true;
        }

        public bool MoveGoto(int dstPosition)
        {
            byte[] byteSendMsg = {
                0xab,       // 指令標頭
                0x11,       // 歸零
                0x00,       // 保留
                0x00,       // 保留
                0xcd        // 指令結尾
                };

            bool needConfirm = false;

            if (isOpen)
            {
                lock (_locker)
                {
                    try
                    {
                        if (NowPosition != dstPosition)
                        {
                            needConfirm = true;
                            isOK = 0;
                            string resultSendMsg = string.Empty;
                            byteSendMsg[3] = Convert.ToByte(dstPosition % 256);
                            byteSendMsg[2] = Convert.ToByte(dstPosition / 256);
                            Com.Write(byteSendMsg, 0, byteSendMsg.Length);
                            _Magni = (dstPosition / 60) - 1;
                            NowPosition = dstPosition;
                        }
                        else
                        {
                            isOK = 1;
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException("Zoom move command failed.", ex);
                    }
                }

                if (needConfirm && !ConfirmMagnification(dstPosition))
                {
                    return false;
                }

                return true;
            }           
            else
            {
                isOK = 0;  
            }
            return true;
        }

        private bool ConfirmMagnification(int dstPosition, int timeoutMilliseconds = 5000, int pollDelayMilliseconds = 80)
        {
            if (!isOpen)
            {
                return true;
            }

            int expectedMagni = dstPosition; 
            Stopwatch stopwatch = Stopwatch.StartNew();

            while (stopwatch.ElapsedMilliseconds <= timeoutMilliseconds)
            {
                GetPositioncmd();
                Thread.Sleep(pollDelayMilliseconds);

                if (Math.Abs(iMagni - expectedMagni) <= 1)
                {
                    _Magni = expectedMagni;
                    return true;
                }
            }

            return false;
        }

        public bool MoveGotoM(int Magni)
        {
            bool isOK = false;

            return isOK;
        }

        public bool MoveGoto(int[] magnificationSteps, int magnification)
        {
            return MoveGoto(magnificationSteps[magnification]);
        }

        public int SearchingMagnification(int[] magnificationSteps)
        {
            return 1;
        }

        public int GetPosition()
        {
            GetPositioncmd();
            Thread.Sleep(250);
            return NowPosition;
        }

        public void GetPositioncmd()
        {
            byte[] byteSendMsg = {
                0xab,       // 指令標頭
                0x21,       // 讀取當前倍率
                0x00,       // 保留
                0x00,       // 保留
                0xcd        // 指令結尾
                };

            if (isOpen)
            {
                lock (_locker)
                {
                    if (newIndex >= dataReceived.Length - 5)
                    {
                        newIndex = 0;
                    }

                    try
                    {
                        // byteSendMsg[3] = Convert.ToByte(dst);
                        // byteSendMsg[2] = Convert.ToByte(dst / 256);
                        Com.Write(byteSendMsg, 0, byteSendMsg.Length);
                        Thread.Sleep(200);
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
            else
            {
            }
        }

        public bool CheckPosition(int steps)
        {
            if (Math.Abs(GetPosition() - steps) < 10)
                return true;
            else
                return false;
        }

        public bool GetStatus()
        {
            return (isOK == 1);
        }

        public void SetStatus()
        {
            isOK = 1;
        }

        public void Stop()
        {
        }

        public void SetDownThruHome(bool switchOnOff)
        {
            if (switchOnOff) MoveHome();
        }
    }

    public class MvotemZoomFake : MvotemZoom
    {
        private object _locker = new object();
        private int _limitPosition = 26689;

        public MvotemZoomFake()
        {

        }

        public new int GetMaxLength()
        {
            // _limitPosition = _lens.Read(Controller.regSetupLimit_1);
            return _limitPosition;
        }

        public new bool MoveHome()
        {
            Stopwatch sw = Stopwatch.StartNew();
            _Magni = 0;
            Thread.Sleep(1000);
            while (true)
            {
                GetStatus();
                if (_idle)
                    break;
                if (sw.ElapsedMilliseconds > 10000)
                    break;
            }
            return true;
        }

        public new bool MoveGoto(int dstPosition)
        {

            try
            {
                lock (_locker)
                {
                    Stopwatch sw = Stopwatch.StartNew();
                    Stop();
                    GetStatus();
                    while (true)
                    {
                        if (_idle)
                        {
                            if (dstPosition > _limitPosition)
                                throw new Exception("Over limit position");
                            Thread.Sleep(1000);
                            while (true)
                            {
                                if (GetStatus())
                                    return true;
                                if (sw.ElapsedMilliseconds > 5000)
                                    return true;
                            }
                        }
                        if (sw.ElapsedMilliseconds > 10000)
                        {
                            throw new Exception("Lens motor time out");
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public new bool MoveGoto(int[] magnificationSteps, int magnification)
        {
            try
            {
                lock (_locker)
                {
                    if (!Connected)
                        return false;
                    if (magnificationSteps[magnification] < 0 || magnificationSteps[magnification] > _limitPosition)
                        return false;
                    if (magnification == _Magni)
                        return true;
                    Stopwatch sw = Stopwatch.StartNew();
                    Stop();
                    GetStatus();
                    bool bRun = true;
                    while (bRun)
                    {
                        if (_idle)
                        {
                            if (magnificationSteps[magnification] > _limitPosition)
                                throw new Exception("Over limit position");
                            Thread.Sleep(250);
                            while (true)
                            {
                                if (GetStatus())
                                    break;

                                if (sw.ElapsedMilliseconds > 5000)
                                    break;
                            }
                            bRun = false;
                        }
                        if (sw.ElapsedMilliseconds > 10000)
                            break;
                    }
                    _Magni = magnification;
                    return true;
                }
            }
            catch (Exception e)
            {
                string smg = e.Message.ToString();

                if (smg == "輸入字串格式不正確。")
                {
                    return true;
                }
                else
                {
                    throw;
                }
            }

        }

        public new int SearchingMagnification(int[] magnificationSteps)
        {
            int position = GetPosition();
            for (int mag = 0; mag < magnificationSteps.Length; mag++)
                if (Math.Abs(magnificationSteps[mag] - position) < 15)
                    return mag;
            return 7;
        }

        public new bool CheckPosition(int steps)
        {
            return true;
        }
    }
}
