using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.Threading;

using Basler.Pylon;
using OpenCvSharp;
using MRLibrary;
using System.Runtime.InteropServices;


namespace NSAA_16Axis
{
    class PylonNetToMat
    {

        private Camera _cam;
        private Mat _grabImage = new Mat(2592, 1944, MatType.CV_8UC1);
        private object _lock = new object();
        private AutoResetEvent _grabEvent = new AutoResetEvent(false);
        private SKZoomAndPanWindow _window;
        private bool _isLive;
        public bool IsOpen = false;
        //public string CamName;
        public Mat _mat0 = null;
        public int _w = 2592;
        public int _x = 0;
        public int _y = 32;
        public int _h = 1944;
        public Rectangle _roi;
        public Mat _grabImageN = new Mat(2592, 1944, MatType.CV_8UC1);

        /// <summary>
        /// use SetWindow() and Is Live to display camera image
        /// </summary>
        public PylonNetToMat(string serialNum)
        {
            try
            {
                _cam = new Camera(serialNum);

                //Cv2.UseOpenCL = true;
                IsOpen = true;
            }
            catch (Exception)
            {
                throw;
            }
            SerialNum = serialNum;
            _roi = new Rectangle(_x, _y, _w, _h);
        }
        public PylonNetToMat()
        {

        }

        public string SerialNum
        {
            get; set;
        }

        public double CameraGainPercentageValue
        {
            get; set;
        }
        public double CameraGammaPercentageValue
        {
            get; set;
        }
        public double CameraBlackLevelPercentageValue
        {
            get; set;
        }
        public double CameraExposureTimePercentageValue
        {
            get; set;
        }


        public void OpenCamera()
        {

            _cam.StreamGrabber.ImageGrabbed += OnImageGrabbed;
            _cam.CameraOpened += Configuration.AcquireContinuous;
            _cam.Open();

            // load parameters
            _cam.Parameters[PLUsbCamera.ReverseX].SetValue(false);
            _cam.Parameters[PLCameraInstance.MaxNumBuffer].SetValue(5);
            //Cam.Parameters[PLCameraInstance.OutputQueueSize].SetValue(3);

            // Start the grabbing of images until grabbing is stopped.
            //Cam.Parameters[PLCamera.AcquisitionMode].SetValue(PLCamera.AcquisitionMode.Continuous);
            _cam.StreamGrabber.Start(GrabStrategy.LatestImages, GrabLoop.ProvidedByStreamGrabber);

            //CameraGainValue = Cam.Parameters[PLCamera.Gain].GetValuePercentOfRange();
            //CameraGammaValue = Cam.Parameters[PLCamera.Gamma].GetValuePercentOfRange();
            //CameraBlackLevelValue = Cam.Parameters[PLCamera.BlackLevel].GetValuePercentOfRange();


            if (_cam.CameraInfo[CameraInfoKey.DeviceType] == "BaslerUsb")
            {
                CameraGainPercentageValue = _cam.Parameters[PLCamera.Gain].GetValuePercentOfRange();
                CameraGammaPercentageValue = _cam.Parameters[PLCamera.Gamma].GetValuePercentOfRange();
                CameraBlackLevelPercentageValue = _cam.Parameters[PLCamera.BlackLevel].GetValuePercentOfRange();
                CameraExposureTimePercentageValue = _cam.Parameters[PLCamera.ExposureTime].GetValuePercentOfRange();
            }
            if (_cam.CameraInfo[CameraInfoKey.DeviceType] == "BaslerGigE")
            {
                _cam.Parameters[PLCamera.GammaEnable].TrySetValue(true);
                CameraGainPercentageValue = _cam.Parameters[PLCamera.GainRaw].GetValuePercentOfRange();
                CameraGammaPercentageValue = _cam.Parameters[PLCamera.Gamma].GetValuePercentOfRange();
                CameraBlackLevelPercentageValue = _cam.Parameters[PLCamera.BlackLevelRaw].GetValuePercentOfRange();
            }
            //if (DeviceType == "BaslerGigE") Cam.Parameters[PLCamera.ExposureTimeAbs].SetValue(35000, FloatValueCorrection.ClipToRange);
            //if (DeviceType == "BaslerUsb") Cam.Parameters[PLCamera.ExposureTime].SetValue(35000, FloatValueCorrection.ClipToRange);
            IsOpen = true;
        }

        public Mat Grab()
        {
            if (!_grabEvent.WaitOne(1000))
            {
                // throw new Exception("Grab fail."); 
            }

            Mat img;
            if (IsOpen)
            {
                try
                {
                    lock (_lock)
                    {
                        //img = new Mat(new Size(grabResult.Width, grabResult.Height), Emgu.CV.CvEnum.DepthType.Cv8U, 1);
                        //img.SetTo<byte>((byte[])grabResult.PixelData);
                        img = _grabImage.Clone();
                    }
                }
                catch (Exception)
                {
                    img = new Mat(_w, _h, MatType.CV_8UC1);
                }
            }
            else
            {
                img = new Mat(_w, _h, MatType.CV_8UC1);
            }
            return img;
        }

        public void Grab(Mat CMat)
        {
            if (IsOpen)
            {
                lock (_lock)
                {
                    _grabImage.CopyTo(CMat);
                }
            }
        }

        //int ImageGrabbedFailTimes = 0;
        private void OnImageGrabbed(Object sender, ImageGrabbedEventArgs e)
        {
            //Mat mat = null;
            try
            {
                IGrabResult grabResult = e.GrabResult;
                if (grabResult.GrabSucceeded)
                {
                    //_mat0 = new Mat(new Size(grabResult.Width, grabResult.Height), DepthType.Cv8U, 1);
                    //_mat0.SetTo<byte>((byte[])grabResult.PixelData);
                    //mat = new Mat(_mat0, _roi);
                    byte[] imageData = grabResult.PixelData as byte[];
                    int length = grabResult.Width * grabResult.Height;
                    Marshal.Copy(imageData, 0, _grabImageN.Data, length);

                    if (_window != null && _isLive) _window.SetImage(_grabImageN);

                    lock (_lock)
                    {
                        //if (_grabImage != null)
                        //{

                        //}
                        Marshal.Copy(imageData,0, _grabImage.Data, length);
                        _grabEvent.Set();
                    }
                }
                else
                {
                    // MessageBox.Show("Camera disconnect", "PylonNetToUMat", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }
            catch (Exception)
            {
                // MessageBox.Show(ex.Message, "PylonNetToUMat", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                e.DisposeGrabResultIfClone();
            }
        }

        public void StopCamera()
        {
            if (IsOpen)
            {
                _cam.StreamGrabber.Stop();
                _cam.StreamGrabber.ImageGrabbed -= OnImageGrabbed;
                _cam.CameraOpened -= Configuration.AcquireContinuous;
                _cam.Close();
                IsOpen = false;
            }

        }


        public void CameraGain(double value)
        {
            CameraGainPercentageValue = value;
            try
            {
                if (_cam.CameraInfo[CameraInfoKey.DeviceType] == "BaslerUsb")
                {
                    _cam.Parameters[PLCamera.Gain].SetValuePercentOfRange(value);
                }
                if (_cam.CameraInfo[CameraInfoKey.DeviceType] == "BaslerGigE")
                    _cam.Parameters[PLCamera.GainRaw].SetValuePercentOfRange(value);
            }
            catch
            {
                throw;
            }
        }

        public bool CameraFrameRate(double frameRate)
        {
            bool bRet = false;
            try
            {
                _cam.Parameters[PLCamera.AcquisitionFrameRateEnable].TrySetValue(true);
                if (_cam.CameraInfo[CameraInfoKey.DeviceType] == "BaslerUsb")
                    _cam.Parameters[PLCamera.AcquisitionFrameRate].TrySetValue(frameRate);
                if (_cam.CameraInfo[CameraInfoKey.DeviceType] == "BaslerGigE")
                {
                    double dmin, dmax;
                    dmin = _cam.Parameters[PLCamera.AcquisitionFrameRateAbs].GetMinimum();
                    dmax = _cam.Parameters[PLCamera.AcquisitionFrameRateAbs].GetMaximum();
                    if ((frameRate > dmin) && (frameRate < dmax))
                        bRet = _cam.Parameters[PLCamera.AcquisitionFrameRateAbs].TrySetValue(frameRate);
                }
            }
            catch (Exception)
            {

            }
            return bRet;
        }

        public void CameraGamma(double value)
        {
            CameraGammaPercentageValue = value;
            try
            {
                //Cam.Parameters[PLCamera.Gamma].SetValuePercentOfRange(value);
                if (_cam.CameraInfo[CameraInfoKey.DeviceType] == "BaslerUsb")
                    _cam.Parameters[PLCamera.Gamma].SetValuePercentOfRange(value);
                if (_cam.CameraInfo[CameraInfoKey.DeviceType] == "BaslerGigE")
                    _cam.Parameters[PLCamera.Gamma].SetValuePercentOfRange(value);
            }
            catch
            {
                throw;
            }
        }

        public void CameraBlackLevel(double value)
        {
            CameraBlackLevelPercentageValue = value;
            try
            {
                if (_cam.CameraInfo[CameraInfoKey.DeviceType] == "BaslerUsb")
                    _cam.Parameters[PLCamera.BlackLevel].SetValuePercentOfRange(value);
                if (_cam.CameraInfo[CameraInfoKey.DeviceType] == "BaslerGigE")
                    _cam.Parameters[PLCamera.BlackLevelRaw].SetValuePercentOfRange(value);
            }
            catch
            {
                throw;
            }
        }

        public void CameraExposureTime(double value)
        {
            CameraExposureTimePercentageValue = value;
            try
            {
                if (_cam.CameraInfo[CameraInfoKey.DeviceType] == "BaslerUsb")
                {
                    // _cam.Parameters[PLCamera.ExposureTime].SetValuePercentOfRange(value);
                    // _cam.Parameters[PLCamera.ExposureTimeMode].SetValue(PLCamera.ExposureTimeMode.Standard);
                    _cam.Parameters[PLCamera.ExposureTime].SetValue(value);
                }
                if (_cam.CameraInfo[CameraInfoKey.DeviceType] == "BaslerGigE")
                {
                    // _cam.Parameters[PLCamera.ExposureTime].SetValuePercentOfRange(value);
                    _cam.Parameters[PLCamera.ExposureTimeAbs].SetValue(value);
                }
            }
            catch
            {
                throw;
            }
        }

        public void SetReverseX(bool bvalue)
        {
            try
            {
                if (_cam.CameraInfo[CameraInfoKey.DeviceType] == "BaslerUsb")
                {
                    _cam.Parameters[PLCamera.ReverseX].SetValue(bvalue);
                }
                if (_cam.CameraInfo[CameraInfoKey.DeviceType] == "BaslerGigE")
                {
                    _cam.Parameters[PLCamera.ReverseX].SetValue(bvalue);
                }
            }
            catch
            {

            }
        }

        public void SetReverseY(bool bvalue)
        {
            try
            {
                if (_cam.CameraInfo[CameraInfoKey.DeviceType] == "BaslerUsb")
                {
                    _cam.Parameters[PLCamera.ReverseY].SetValue(bvalue);
                }
                if (_cam.CameraInfo[CameraInfoKey.DeviceType] == "BaslerGigE")
                {
                    _cam.Parameters[PLCamera.ReverseY].SetValue(bvalue);
                }
            }
            catch
            {

            }
        }
        public void SetWindow(SKZoomAndPanWindow window) { _window = window; }
        public void Live() { _isLive = true; }
        public void Freeze() { _isLive = false; }
        
    }
}
