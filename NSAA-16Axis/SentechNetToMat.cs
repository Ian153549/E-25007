using OpenCvSharp;
using MRLibrary;
using Sentech.GenApiDotNET;
using Sentech.StApiDotNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;

namespace NSAA_16Axis
{
    public class SentechNetToMat
    {
        public String SerialNum;
        public CStDevice device = null;

        private System.Timers.Timer _simulationTimer;

        private Mat _grabImage = new Mat(new OpenCvSharp.Size(4000, 3000), MatType.CV_8UC1);
        private object _lock = new object();
        private AutoResetEvent _grabEvent = new AutoResetEvent(false);
        private SKZoomAndPanWindow _window;
        private bool _isLive;
        public bool IsOpen = false;
        public string CamName;
        public int ID;
        private long[] Inteval = new long[20];
        public Mat _grabImageN = new Mat(new OpenCvSharp.Size(4000, 3000), MatType.CV_8UC1);
        public CStDataStream dataStream;
        public INodeMap nodeMapRemote;
        private int WidthMax;
        private int HeightMax;
        public int FrameCount = 5001;
        public int FrameCountLimit = 5000;
        private int _simulationFrameRate = 14;
        public bool IsFileImage = false;
        public Mat FileImage = new Mat();
        private SKZoomAndPanWindow _upWindow;
        private SKZoomAndPanWindow _backWindow;
        private bool _isUpPage = true;


        // Feature names
        const string EXPOSURE_AUTO = "ExposureAuto";            //Standard
        const string GAIN_AUTO = "GainAuto";                    //Standard
        const string BALANCE_WHITE_AUTO = "BalanceWhiteAuto";   //Standard

        const string AUTO_LIGHT_TARGET = "AutoLightTarget";     //Custom
        const string GAIN = "Gain";                             //Standard
        const string GAIN_RAW = "GainRaw";                      //Custom

        const string EXPOSURE_MODE = "ExposureMode";            //Standard
        const string EXPOSURE_TIME = "ExposureTime";            //Standard
        const string EXPOSURE_TIME_RAW = "ExposureTimeRaw";     //Custom

        const string BALANCE_RATIO_SELECTOR = "BalanceRatioSelector";   //Standard
        const string BALANCE_RATIO = "BalanceRatio";			//Standard

        public ImageMirror Mirror
        {
            get;
            set;
        }

        public ImageRotate Rotate
        {
            get;
            set;
        }

        /// <summary>
        /// use SetWindow() and Is Live to display camera image
        /// </summary>
        public SentechNetToMat(IntPtr serialNum)
        {
            //try
            //{
            //    // _cam = new Camera(serialNum);
            //    device = new CStDevice(serialNum);

            //    Cv2.UseOpenCL = true;
            //    IsOpen = true;
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
            //SerialNum = device.GetIStDeviceInfo().SerialNumber;
        }

        public SentechNetToMat(CStDevice _device)
        {
            device = _device;
            //Cv2.UseOpenCL = true;
            IsOpen = true;
            dataStream = device.CreateStDataStream(0);
            object[] param = { this };
            dataStream.RegisterCallbackMethod(OnCallback, param);
            nodeMapRemote = device.GetRemoteIStPort().GetINodeMap();
            if (nodeMapRemote != null)
            {
                IInteger nodeWidth = nodeMapRemote.GetNode<IInteger>("Width");
                if (nodeWidth != null)
                {
                    WidthMax = (int)nodeWidth.Maximum;
                }
                IInteger nodeHeight = nodeMapRemote.GetNode<IInteger>("Height");
                if (nodeHeight != null)
                {
                    HeightMax = (int)nodeHeight.Maximum;
                }
            }
            SerialNum = device.GetIStDeviceInfo().SerialNumber;
            if ((WidthMax != 0) && (HeightMax != 0))
            {
                _grabImage = new Mat(HeightMax, WidthMax, MatType.CV_8UC1);
                _grabImageN = new Mat(HeightMax, WidthMax, MatType.CV_8UC1);
                //_grabImage = new Mat(WidthMax, HeightMax, MatType.CV_8UC1);
                //_grabImageN = new Mat(WidthMax, HeightMax, MatType.CV_8UC1);
            }
            else
            {
                _grabImage = new Mat(4000, 3000, MatType.CV_8UC1);
                _grabImageN = new Mat(4000, 3000, MatType.CV_8UC1);
            }
            FrameCount = 0;
            InitializeSimulationTimer();
            _simulationTimer.Enabled = false;
        }

        private void InitializeSimulationTimer()
        {
            //_simulationTimer = new System.Timers.Timer(1000 / _simulationFrameRate); 20251212
            if (_simulationTimer != null)
            {
                System.Diagnostics.Debug.WriteLine("[InitializeSimulationTimer] Timer already exists, disposing old one");
                _simulationTimer.Stop();
                _simulationTimer.Dispose();
            }
            _simulationTimer = new System.Timers.Timer(1000 / _simulationFrameRate);
            _simulationTimer.Elapsed += (sender, e) =>
            {
                if (IsFileImage && !FileImage.Empty())
                {
                    lock (_lock)
                    {
                        if (FrameCount > FrameCountLimit)
                        {
                            if (_grabImageN != null)
                            {
                                if (!_grabImageN.Empty())
                                {
                                    _grabImageN = null;
                                    _grabImageN = new Mat(HeightMax, WidthMax, MatType.CV_8UC1);
                                }
                            }
                            if (_grabImage != null)
                            {
                                if (!_grabImage.Empty())
                                {
                                    _grabImage?.Dispose();
                                }
                                _grabImage = null;
                                _grabImage = new Mat(HeightMax, WidthMax, MatType.CV_8UC1);
                            }
                            FrameCount = 0;
                        }
                        FileImage.CopyTo(_grabImageN);
                        FileImage.CopyTo(_grabImage);
                        _grabEvent.Set();
                        OnFrameReady(_grabImage);
                        if (_window != null && _isLive)
                        {
                            _window.SetImage(_grabImage);
                        }
                        FrameCount++;
                    }
                }
            };
            _simulationTimer.AutoReset = true;
        }

        public eStInterfaceType DeviceType
        {
            get
            {
                return device.GetIStInterface().InterfaceType;
            }
        }

        // List up the contents of current enumeration node with current setting.
        static void Enumeration(INodeMap nodeMap, string nodeName)
        {
            // Get the IEnum interface object.
            IEnum enumNode = nodeMap.GetNode<IEnum>(nodeName);

            if (enumNode.IsWritable)
            {
                while (true)
                {
                    // Display a configurable option.
                    Console.WriteLine(nodeName);

                    for (int i = 0; i < enumNode.Entries.Length; i++)
                    {
                        IEnumEntry entryNode = enumNode.Entries[i];

                        if (entryNode.IsAvailable)
                        {
                            Console.Write(i + " : " + entryNode.Symbolic);

                            if (enumNode.IntValue == entryNode.Value)
                            {
                                Console.Write("(Current)");
                            }

                            Console.Write(Environment.NewLine);
                        }
                    }
                    Console.Write("Select : ");

                    // Waiting for input.
                    string strIndex = Console.ReadLine();

                    int index;
                    if (int.TryParse(strIndex.Trim(), out index))
                    {
                        // Reflect the value entered.
                        if (0 <= index && index < enumNode.Entries.Length)
                        {
                            string strValue = enumNode.Entries[index];
                            enumNode.FromString(strValue);
                            break;
                        }
                    }
                }
            }
        }

        static void Numeric<NODE_TYPE>(INodeMap nodeMap, string nodeName) where NODE_TYPE : IInteger
        {
            // Get the IInteger interface.
            NODE_TYPE node = (NODE_TYPE)nodeMap[nodeName];

            if (node.IsWritable)
            {
                while (true)
                {
                    // Display the feature name, the range, the current value and the incremental value.
                    Console.Write(nodeName);
                    Console.Write(" Minimum=" + node.Minimum);
                    Console.Write(" Maximum=" + node.Maximum);
                    Console.Write(" Current=" + node.Value);
                    if (node.IncrementMode == eIncrementMode.FixedIncrement)
                    {
                        Console.Write(" Increment=" + node.Increment);
                    }

                    Console.Write(Environment.NewLine + "New value : ");

                    // Waiting for input of new value.
                    string strValue = Console.ReadLine();

                    long value;
                    if (long.TryParse(strValue.Trim(), out value))
                    {
                        // Reflect the value entered.
                        if (node.Minimum <= value && value <= node.Maximum)
                        {
                            node.Value = value;
                            break;
                        }
                    }
                }
            }
        }

        // List up the numeric value of the current setting that the node indicated.
        void NumericF<NODE_TYPE>(INodeMap nodeMap, string nodeName) where NODE_TYPE : IFloat
        {
            // Get the IFloat interface.
            NODE_TYPE node = (NODE_TYPE)nodeMap[nodeName];

            if (node.IsWritable)
            {
                while (true)
                {
                    // Display the feature name, the range, the current value and the incremental value.
                    Console.Write(nodeName);
                    Console.Write(" Minimum={0:F2}", node.Minimum);
                    Console.Write(" Maximum={0:e5}", node.Maximum);
                    Console.Write(" Current={0:F2}", node.Value);
                    if (node.IncrementMode == eIncrementMode.FixedIncrement)
                    {
                        Console.Write(" Increment=" + node.Increment);
                    }

                    Console.Write(Environment.NewLine + "New value : ");

                    //Waiting for input of new value.
                    string strValue = Console.ReadLine();

                    double value;
                    if (double.TryParse(strValue.Trim(), out value))
                    {
                        // Reflect the value entered.
                        if (node.Minimum <= value && value <= node.Maximum)
                        {
                            node.Value = value;
                            break;
                        }
                    }
                }
            }
        }

        // List up the contents of current enumeration node with current numeric setting.
        void EnumerationAndNumericF(INodeMap nodeMap, string nodeName, string numericName)
        {
            // Get the IEnum interface object.
            IEnum enumNode = nodeMap.GetNode<IEnum>(nodeName);

            if (enumNode.IsWritable)
            {
                for (int i = 0; i < enumNode.Entries.Length; i++)
                {
                    IEnumEntry entryNode = enumNode.Entries[i];

                    if (entryNode.IsAvailable)
                    {
                        // Switch the setting target.
                        enumNode.IntValue = entryNode.Value;

                        // Display the selected setting target.
                        Console.Write(nodeName + "=" + entryNode.Symbolic + Environment.NewLine);

                        // Configure a numerical value.
                        NumericF<FloatNode>(nodeMap, numericName);
                    }
                }
            }
        }

        void ExposureAuto(INodeMap nodeMap)
        {
            // Configure the ExposureMode.
            Enumeration(nodeMap, EXPOSURE_MODE);

            // Configure the ExposureAuto.
            Enumeration(nodeMap, EXPOSURE_AUTO);

            // Configure the AutoLightTarget.
            Numeric<IntegerNode>(nodeMap, AUTO_LIGHT_TARGET);

            if (nodeMap.GetNode<FloatNode>(EXPOSURE_TIME) != null)
            {
                // Configure the ExposureTime.
                NumericF<FloatNode>(nodeMap, EXPOSURE_TIME);
            }
            else
            {
                // Configure the ExposureTimeRaw if the ExposureTime function does not exist.
                Numeric<IntegerNode>(nodeMap, EXPOSURE_TIME_RAW);
            }
        }

        // Gain Auto.
        void GainAuto(INodeMap nodeMap)
        {
            // Configure the GainAuto.
            Enumeration(nodeMap, GAIN_AUTO);

            // Configure the AutoLightTarget.
            Numeric<IntegerNode>(nodeMap, AUTO_LIGHT_TARGET);

            if (nodeMap.GetNode<FloatNode>(GAIN) != null)
            {
                // Configure the Gain.
                NumericF<FloatNode>(nodeMap, GAIN);
            }
            else
            {
                // Configure the GainRaw if the Gain function does not exist.
                Numeric<IntegerNode>(nodeMap, GAIN_RAW);
            }
        }

        // BalanceWhite Auto.
        void BalanceWhiteAuto(INodeMap nodeMap)
        {
            // Configure the BalanceWhiteAuto.
            Enumeration(nodeMap, BALANCE_WHITE_AUTO);

            // While switching the BalanceRatioSelector, configure the BalanceRatio.
            EnumerationAndNumericF(nodeMap, BALANCE_RATIO_SELECTOR, BALANCE_RATIO);
        }

        public double Gain
        {
            get
            {
                double value = 0;
                if (DeviceType == eStInterfaceType.GigEVision)
                {
                    // value = device..GigEVision.Gain;
                }
                return value;
            }
            set
            {

            }
        }

        public double FrameRate
        {
            get
            {
                double inteval = 0;
                for (int i = 0; i < (int)Inteval.Length; i++)
                {
                    inteval += (double)Inteval[i];
                }
                double length = 1000 / (inteval / (double)((int)Inteval.Length));
                return length;
            }
            set
            {
            }
        }

        public SentechNetToMat()
        {

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
        public double ExposureTime
        {
            get
            {
                double value = 0;

                return value;
            }
            set
            {

            }
        }

        public void OpenCamera()
        {
            IsOpen = true;
            try
            {
                IEnum exposureModeNode= nodeMapRemote.GetNode<IEnum>("ExposureMode");
                if (exposureModeNode != null && exposureModeNode.IsWritable) 
                {
                    exposureModeNode.FromString("Timed");
                    System.Diagnostics.Debug.WriteLine("Set ExposureMode to Timed");
                }
            }
            catch
            {
            }
            dataStream.StartAcquisition();
            device.AcquisitionStart();
        }

        //public Mat Grab()
        //{
        //    if (!_grabEvent.WaitOne(1000))
        //    {
        //        // throw new Exception("Grab fail."); 
        //    }

        //    Mat img;
        //    if (IsOpen)
        //    {
        //        lock (_lock)
        //        {
        //            //img = new Mat(new Size(grabResult.Width, grabResult.Height), Emgu.CV.CvEnum.DepthType.Cv8U, 1);
        //            //img.SetTo<byte>((byte[])grabResult.PixelData);
        //            img = _grabImage.Clone();
        //        }
        //    }
        //    else
        //    {
        //        img = new Mat(new Size(3856, 2764), Emgu.CV.CvEnum.DepthType.Cv8U, 1);
        //    }
        //    return img;
        //}
        public Mat Grab()
        {
            if (!_grabEvent.WaitOne(1000))
            {
                // throw new Exception("Grab fail."); 
            }

            Mat img;
            if (IsOpen)
            {
                lock (_lock)
                {                    
                    img = _grabImage.Clone();
                }
            }
            else
            {
                //img = new Mat(new Size(3856, 2764),MatType.CV_8UC1);
                img = new Mat(new OpenCvSharp.Size(4000, 3000), MatType.CV_8UC1);
            }
            return img;
        }

        //public void Grab(Mat CMat)
        //{
        //    if (IsOpen)
        //    {
        //        lock (_lock)
        //        {
        //            _grabImage.CopyTo(CMat);
        //        }
        //    }
        //}
        public void Grab(Mat CMat)
        {
            if (IsOpen)
            {
                if (IsFileImage)
                {
                    if (!FileImage.Empty())
                    {
                        lock (_lock)
                        {
                            FileImage.CopyTo(CMat);
                        }
                    }
                }
                else
                {
                    if (_grabEvent.WaitOne(1000))
                    {
                        lock (_lock)
                        {
                            if(_grabImage != null)
                            {
                                if (!_grabImage.Empty()) _grabImage.CopyTo(CMat);
                            }
                            
                        }
                    }
                }
            }
            else
            {
                if (!FileImage.Empty())
                {
                    lock (_lock)
                    {
                        FileImage.CopyTo(CMat);
                    }
                }
            }
        }
        public bool IsLive
        {
            get { return _isLive; }
        }

        //void OnCallback(IStCallbackParamBase paramBase, object[] param)
        //{
        //    // Check callback type. Only NewBuffer event is handled in here
        //    if (paramBase.CallbackType == eStCallbackType.TL_DataStreamNewBuffer)
        //    {
        //        // In case of receiving a NewBuffer events:
        //        // Convert received callback parameter into IStCallbackParamGenTLEventNewBuffer for acquiring additional information.

        //        if (paramBase is IStCallbackParamGenTLEventNewBuffer callbackParam)
        //        {
        //            try
        //            {
        //                // Get the IStDataStream interface object from the received callback parameter.
        //                IStDataStream dataStream = callbackParam.GetIStDataStream();

        //                // Retrieve the buffer of image data for that callback indicated there is a buffer received.
        //                using (CStStreamBuffer streamBuffer = dataStream.RetrieveBuffer(0))
        //                {
        //                    // Check if the acquired data contains image data.
        //                    if (streamBuffer.GetIStStreamBufferInfo().IsImagePresent)
        //                    {
        //                        // If yes, we create a IStImage object for further image handling.
        //                        IStImage stImage = streamBuffer.GetIStImage();

        //                        Byte[] imageData = stImage.GetByteArray();
        //                        lock (_lock)
        //                        {
        //                            if (!IsFileImage)
        //                            {
        //                                if(FrameCount > FrameCountLimit)
        //                                {
        //                                    DisposeAndRecreateMat(ref _grabImageN, WidthMax, HeightMax);
        //                                    DisposeAndRecreateMat(ref _grabImage, WidthMax, HeightMax);
        //                                    FrameCount = 0;
        //                                }
        //                                _grabImageN.SetArray(imageData);
        //                                _grabImageN.CopyTo(_grabImage);
        //                            }
        //                            _grabEvent.Set();
        //                            FrameCount++;
        //                        }
        //                        if (_window != null && _isLive)
        //                        {
        //                            if (IsFileImage)
        //                            {
        //                                if (!FileImage.Empty()) _window.SetImage(FileImage);
        //                            }
        //                            else
        //                            {
        //                                _window.SetImage(_grabImageN);
        //                            }
        //                        }
        //                        //_grabImageN.SetTo<byte>(imageData);
        //                        //// RotateMirror(_grabImageN);
        //                        //if (_window != null && _isLive)
        //                        //    _window.SetImage(_grabImageN);
        //                        //lock (_lock)
        //                        //{
        //                        //    _grabImage.SetTo<byte>(imageData);
        //                        //    // RotateMirror(_grabImage);
        //                        //    // RotateMirror(_grabImageN);  //2023-04-28
        //                        //    _grabEvent.Set();
        //                        //}

        //                        // Display the information of the acquired image data.
        //                        //Console.Write("BlockId=" + streamBuffer.GetIStStreamBufferInfo().FrameID);
        //                        //Console.Write(" Size:" + stImage.ImageWidth + " x " + stImage.ImageHeight);
        //                        //Console.Write(" First byte =" + imageData[0] + Environment.NewLine);
        //                    }
        //                    else
        //                    {
        //                        // If the acquired data contains no image data.
        //                        //Console.WriteLine("Image data does not exist.");
        //                    }
        //                }
        //            }
        //            catch (Exception e)
        //            {
        //                // If any exception occurred, display the description of the error here.
        //                Console.Error.WriteLine("An exception occurred. \r\n" + e.Message);
        //            }
        //            GC.KeepAlive(_grabImage);
        //            GC.KeepAlive(_grabImageN);
        //        }
        //    }
        //}
        void OnCallback(IStCallbackParamBase paramBase, object[] param)
        {
            // Check callback type. Only NewBuffer event is handled in here
            if (paramBase.CallbackType == eStCallbackType.TL_DataStreamNewBuffer)
            {
                if (paramBase is IStCallbackParamGenTLEventNewBuffer callbackParam)
                {
                    try
                    {
                        IStDataStream dataStream = callbackParam.GetIStDataStream();
                        using (CStStreamBuffer streamBuffer = dataStream.RetrieveBuffer(0))
                        {
                            // Check if the acquired data contains image data.
                            if (streamBuffer.GetIStStreamBufferInfo().IsImagePresent)
                            {
                                IStImage stImage = streamBuffer.GetIStImage();

                                Byte[] imageData = stImage.GetByteArray();
                                
                                // RotateMirror(_grabImageN);
                                
                                lock (_lock)
                                {
                                    _grabImageN.SetArray(imageData);
                                    _grabImage.SetArray(imageData);                                    
                                    _grabEvent.Set();
                                }
                                if (_window != null && _isLive)
                                    _window.SetImage(_grabImageN);
                                OnFrameReady(_grabImage);
                            }
                            else
                            {

                            }
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }

        private void DisposeAndRecreateMat(ref Mat mat, int width, int height)
        {
            if(mat != null)
            {
                if (!mat.Empty()) mat.Dispose();
                mat = null;               
            }
            mat = new Mat(height,width, MatType.CV_8UC1);
        }

        public void StopCamera()
        {
            if (IsOpen)
            {
                IsOpen = false;
            }

        }

        public void CameraGain(double value)
        {
            //CameraGainPercentageValue = value;
            try
            {
                IFloat node = nodeMapRemote.GetNode<IFloat>("Gain");
                if (node != null)
                {
                    node.Value = (float)value;
                }
            }
            catch
            {
                throw;
            }
        }

        public void CameraFrameRate(double frameRate)
        {
            try
            {
            }
            catch
            {
                throw;
            }
        }
        public void SetSimulationImage(Mat newImage)
        {
            lock (_lock)
            {
                if (!newImage.Empty())
                {
                    if ((FileImage != null) && (!FileImage.Empty()))
                    {
                        FileImage.Dispose();
                    }
                    FileImage = newImage.Clone();
                    IsFileImage = true;

                    // 更新尺寸相關變數
                    WidthMax = FileImage.Width;
                    HeightMax = FileImage.Height;

                    // 重新建立grabImage以匹配新尺寸
                    if (_grabImage != null) _grabImage.Dispose();
                    if (_grabImageN != null) _grabImageN.Dispose();
                    //_grabImage = new Mat(FileImage.Width, FileImage.Height, MatType.CV_8UC1);
                    //_grabImageN = new Mat(FileImage.Width, FileImage.Height, MatType.CV_8UC1);
                    _grabImage = new Mat(FileImage.Height, FileImage.Width, MatType.CV_8UC1);
                    _grabImageN = new Mat(FileImage.Height, FileImage.Width, MatType.CV_8UC1);
                    FileImage.CopyTo(_grabImage);
                    FileImage.CopyTo(_grabImageN);

                    // 新增:啟動模擬 Timer (如果尚未啟動)
                    //if (!_simulationTimer.Enabled && !_isLive)
                    //{
                    //    _simulationTimer.Start();
                    //}
                }
            }
        }
        public void CameraGamma(double value)
        {
            // CameraGammaPercentageValue = value;
            if ((value > 0.1) && (value < 4))
            {
                try
                {
                    IFloat node = nodeMapRemote.GetNode<IFloat>("Gamma");
                    if (node != null)
                    {
                        node.Value = (float)value;
                    }
                }
                catch
                {
                    throw;
                }
            }
        }

        public void CameraBlackLevel(double value)
        {
            CameraBlackLevelPercentageValue = value;
            try
            {
            }
            catch
            {
                throw;
            }
        }

        public void CameraExposureTime(double value)
        {
            // CameraExposureTimePercentageValue = value;
            try
            {
                IFloat node = nodeMapRemote.GetNode<IFloat>("ExposureTime");
                if (node != null)
                {
                    node.Value = (float)value;
                }
            }
            catch
            {
                throw;
            }
        }


        public void SetReverseX(bool bvalue)
        {
            //try
            //{
            //    IBool node = nodeMapRemote.GetNode<IBool>("ReverseX");
            //    node.Value = bvalue;
            //}
            //catch
            //{

            //}
            try
            {
                IBool node = nodeMapRemote.GetNode<IBool>("ReverseX");
                if (node != null && node.IsWritable)
                {
                    node.Value = bvalue;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"ReverseX node is not writable. IsAvailable: {node?.IsAvailable}, IsWritable: {node?.IsWritable}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SetReverseX failed: {ex.Message}");
            }
        }

        public void SetReverseY(bool bvalue)
        {
            try
            {
                IBool node = nodeMapRemote.GetNode<IBool>("ReverseY");
                node.Value = bvalue;
            }
            catch
            {

            }
        }

        public void SetWindow(SKZoomAndPanWindow window) 
        { 
            
            _window = window; 
        }

        public void Live()
        {
            //if (IsOpen)
            //{
            //    _isLive = true;
            //}
            _isLive = true;
            //if (IsFileImage && !_simulationTimer.Enabled)
            //{
            //    _simulationTimer.Start();
            //}
            if (IsFileImage)
            {
                System.Diagnostics.Debug.WriteLine($"[Live] IsFileImage=true, FileImage.Empty={FileImage?.Empty() ?? true}");

                //  確保 Timer 已初始化
                if (_simulationTimer == null)
                {
                    System.Diagnostics.Debug.WriteLine("[Live] _simulationTimer is null, calling InitializeSimulationTimer()");
                    InitializeSimulationTimer();
                }

                //  確保有影像可以顯示
                if (FileImage == null || FileImage.Empty())
                {
                    System.Diagnostics.Debug.WriteLine("[Live] Warning: FileImage is null or empty!");
                    return;
                }

                //  啟動 Timer
                if (!_simulationTimer.Enabled)
                {
                    System.Diagnostics.Debug.WriteLine("[Live] Starting simulation timer");
                    _simulationTimer.Start();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("[Live] Timer already enabled");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"[Live] IsFileImage=false, IsOpen={IsOpen}");
            }

        }

        public void Freeze()
        {
            if (dataStream != null)
            {
                //device.AcquisitionStop();
                //dataStream.StopAcquisition();
            }
            _isLive = false;
           
        }

        public void Close()
        {
            if (_isLive)
            {
                if (dataStream != null)
                {
                    dataStream.StopAcquisition();
                    device.AcquisitionStop();
                }
                _isLive = false;
            }
            if (dataStream != null)
            {
                dataStream.Dispose();
                device.Dispose();
            }
            IsOpen = false;
        }

        private void RotateMirror(Mat img)
        {
            ImageRotate rotate = Rotate;
            if (rotate <= ImageRotate.Degree90)
            {
                if (rotate != ImageRotate.None)
                {
                    if (rotate == ImageRotate.Degree90)
                    {
                        Cv2.Transpose(img, img);
                        Cv2.Flip(img, img, FlipMode.Y);
                    }
                }
            }
            else if (rotate == ImageRotate.Degree180)
            {
                Cv2.Flip(img, img, FlipMode.XY);
            }
            else if (rotate == ImageRotate.Degree270)
            {
                Cv2.Transpose(img, img);
                Cv2.Flip(img, img, FlipMode.X);
            }
            switch (Mirror)
            {
                case ImageMirror.Horizontal:
                    {
                        Cv2.Flip(img, img, FlipMode.X);
                        break;
                    }
                case ImageMirror.Vertical:
                    {
                        Cv2.Flip(img, img, FlipMode.Y);
                        break;
                    }
            }
        }

        public enum ImageMirror
        {
            None,
            Horizontal,
            Vertical
        }

        public enum ImageRotate
        {
            None = 0,
            Degree90 = 90,
            Degree180 = 180,
            Degree270 = 270
        }

        public enum OpenCameraBy
        {
            ByIPAddress,
            BySerialNumber
        }

        public event FrameReady FrameReadyHandler;
        public delegate void FrameReady(Mat img);
        private void OnFrameReady(Mat img)
        {
            FrameReadyHandler?.Invoke(img);
        }
    }
}
