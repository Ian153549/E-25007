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
    public class NarvitarMotorLens
    {
        private object _locker = new object();
        private int _limitPosition;
        public ControllerGen2 _lens;
        public bool _idle;
        public int NowPosition;
        public int Magnification = 0;
        private bool isOpen = false;

        public NarvitarMotorLens()
        {
            
        }
        public void Open(string portName)
        {
            try
            {
                _lens = new ControllerGen2(portName);
                _lens.Connect();
                GetMaxLength();
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
                if (_lens != null)
                    if (Connected) 
                        _lens.Disconnect();
            }   
            catch(Exception)
            {
                throw;
            }
        }

        public bool Connected
        {
            get
            {
                if (isOpen)
                {
                    return _lens.Connected;
                } else
                {
                    return false;
                }
            } 
        }

        public int GetMaxLength()
        {
            _limitPosition = _lens.Read(Controller.regSetupLimit_1);
            return _limitPosition;
        }


        public bool MoveHome()
        {
            if (isOpen)
            {
                Stopwatch sw = Stopwatch.StartNew();
                Magnification = 0;
                _lens.Write(Controller.regLimit_1, 0);
                Thread.Sleep(1000);
                while (true)
                {
                    GetStatus();
                    if (_idle)
                        break;
                    if (sw.ElapsedMilliseconds > 20000)
                        throw new Exception("Len home time out.");
                }
                return true;
            } else
            {
                return true;
            }
        }

        public bool MoveGoto(int dstPosition)
        { 
            if (isOpen)
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
                                _lens.Write(Controller.regTarget_1, dstPosition);
                                Thread.Sleep(1000);
                                while (true)
                                {
                                    if (GetStatus())
                                        return true;
                                    if (sw.ElapsedMilliseconds > 20000)
                                        return false;
                                        // throw new Exception("Lens motor time out");
                                }
                            }
                            if (sw.ElapsedMilliseconds > 20000)
                            {
                                return false;
                                // throw new Exception("Lens motor time out");
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    return false;
                    // throw;
                }
            } else
            {
                return false;
            }
        }

        public bool MoveGoto(int[] magnificationSteps, int magnification)
        {       
            try
            {
                lock (_locker)
                {
                    if (!Connected)
                        return false;
                    if (magnificationSteps[magnification] < 0 || magnificationSteps[magnification] > _limitPosition)
                        return false;
                    if (magnification == Magnification)
                        return true;
                    Stopwatch sw = Stopwatch.StartNew();
                    Stop();
                    GetStatus();
                    Magnification = magnification;
                    while (true)
                    {
                        if (_idle)
                        {
                            if (magnificationSteps[magnification] > _limitPosition)
                                throw new Exception("Over limit position");
                            _lens.Write(Controller.regTarget_1, magnificationSteps[magnification]);
                            Thread.Sleep(1000);
                            while (true)
                            {
                                if (GetStatus())
                                    return true;
                                if (sw.ElapsedMilliseconds > 10000)
                                    throw new Exception("Lens motor time out");
                            }
                        }
                        if (sw.ElapsedMilliseconds > 10000)
                            throw new Exception("Lens motor time out");
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        public int SearchingMagnification(int[] magnificationSteps)
        {
            int position = GetPosition();
            for (int mag = 0; mag < magnificationSteps.Length; mag++)
                if (Math.Abs(magnificationSteps[mag] - position) < 15)
                    return mag;
            return -1;
        }

        public int GetPosition()
        {
            if (isOpen)
            {
                NowPosition = _lens.Read(Controller.regCurrent_1);
            }
            else
            {
                NowPosition = 0;
            }
            return NowPosition;
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
            if (isOpen)
            {
                Thread.Sleep(100);
                int status = _lens.Read(Controller.regStatus_1);
                _idle = (((uint)status & 0x000000ff) == 0) ? true : false;
                return _idle;   //true -> Idle
            } else
            {
                return false;
            }
        }

        //public bool GetStatus()

        public void Stop()
        {
            if (isOpen)
            {
                _lens.Stop();
            }
        }

        public void SetDownThruHome(bool switchOnOff)
        {
            if (isOpen)
            {
                int regVal = _lens.Read(Controller.regSetupConfig_1);
                if (switchOnOff)
                    regVal |= 0x02;
                else
                    regVal &= ~((int)0x02);
                _lens.Write(Controller.regSetupConfig_1, regVal);
            }
        }
    }

    public class NarvitarMotorLensFake : NarvitarMotorLens
    {
        private object _locker = new object();
        private int _limitPosition = 26689;

        public NarvitarMotorLensFake()
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
            Magnification = 0;
            _lens.Write(Controller.regLimit_1, 0);
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
                            _lens.Write(Controller.regTarget_1, dstPosition);
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
                    if (magnification == Magnification)
                        return true;
                    Stopwatch sw = Stopwatch.StartNew();
                    Stop();
                    GetStatus();
                    Magnification = magnification;
                    while (true)
                    {
                        if (_idle)
                        {
                            if (magnificationSteps[magnification] > _limitPosition)
                                throw new Exception("Over limit position");
                            _lens.Write(Controller.regTarget_1, magnificationSteps[magnification]);
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
                            return true;
                    }
                }
            }
            catch (Exception e)
            {
                string smg = e.Message.ToString();

                if (smg == "輸入字串格式不正確。")
                {
                    return true;
                } else {
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
