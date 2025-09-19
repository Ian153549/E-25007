using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace MRLibrary
{
    public partial class SKZoomAndPanWindow : UserControl
    {
        //zoom parm//
        private int _ratioIndex = 0;
        private int _lastRatioIndex = 0;
        private float[] _ratio = new float[7];

        //image//
        private object _lock = new object();
        private object ImageLock = new object();
        private UMat _srcImage, _fitWindowImage;
        public Mat _dstImage;
        private int _fitWindowImageWidth, _fitWindowImageHeight;
        private float _imgMagnificationX;
        private float _imgMagnificationY;
        private Mat _MaskImage;
        private Mat Dimage;
        public int SetImageCount = 501;
        public int SetImageCountLimit = 500;

        private double alpha;
        private bool bwithMaskImage = false;

        // private bool _isGettingWindowImage;
        private bool _isCreatedMaskAndWaferRect = false;
        private bool _isCreatedCommonRect = false;
        private bool _isShowedWarning = false;

        private AutoResetEvent _getWindowImageEvent = new AutoResetEvent(false);

        private SKWindowLearnPair _mask;
        private SKWindowLearnPair _wafer;
        private SKWindowLearnPair _common;

        public MatchPosition WaferMp;
        public MatchPosition MaskMp;
        public List<MatchPosition> OcrInfoMp;

        //point//
        private bool _isMouseDown = false;
        private System.Drawing.Point _aftZoomRoiLeftUpPt = new System.Drawing.Point(0, 0);
        private System.Drawing.Point _oldPt = new System.Drawing.Point(0, 0);
        private System.Drawing.Point _ptMouseMove = new System.Drawing.Point(0, 0);
        private System.Drawing.Point _ptMousedown = new System.Drawing.Point(0, 0);
        private System.Drawing.Point _ptMouseWheel = new System.Drawing.Point(0, 0);

        //switch//
        public bool CanZoom;
        public bool CanRectMaskAndWafer;
        public bool CanRectCommonRoi;
        public bool CanRectLineScanArea;
        public bool CanTrackPattern;
        public bool CanShowOcrInfo;
        public bool CanCenterLine;

        public SKZoomAndPanWindow()
        {
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            _ratio[0] = 1;
            for (int i = 1; i < _ratio.Length; i++) _ratio[i] = _ratio[i - 1] * 1.5f;
        }

        private void SKZoomAndPanWindow_MouseDown(object sender, MouseEventArgs e)
        {
            _isMouseDown = true;
            _ptMousedown = e.Location;
            //if (CanRectCommonRoi) _common.PatternRoiMouseDown(e);
            //if (CanRectMaskAndWafer)
            //{
            //    _mask.PatternRoiMouseDown(e);
            //    _wafer.PatternRoiMouseDown(e);
            //}
            if (CanRectCommonRoi && _common != null)
                _common.PatternRoiMouseDown(e);

            if (CanRectMaskAndWafer && _mask != null && _wafer != null)
            {
                _mask.PatternRoiMouseDown(e);
                _wafer.PatternRoiMouseDown(e);
            }
        }

        private void SKZoomAndPanWindow_MouseUp(object sender, MouseEventArgs e)
        {
            _isMouseDown = false;
            _oldPt = _aftZoomRoiLeftUpPt;
            //if (CanRectCommonRoi) _common.PatternRoiMouseUp();
            //if (CanRectMaskAndWafer)
            //{
            //    _mask.PatternRoiMouseUp();
            //    _wafer.PatternRoiMouseUp();
            //}
            if (CanRectCommonRoi && _common != null)
                _common.PatternRoiMouseUp();

            if (CanRectMaskAndWafer && _mask != null && _wafer != null)
            {
                _mask.PatternRoiMouseUp();
                _wafer.PatternRoiMouseUp();
            }
        }

        private void SKZoomAndPanWindow_MouseEnter(object sender, EventArgs e)
        {
            this.Focus();
        }

        private void SKZoomAndPanWindow_MouseMove(object sender, MouseEventArgs e)
        {
            _ptMouseMove = e.Location;
            if (!_isCreatedMaskAndWaferRect) return;
            if (!_isCreatedCommonRect) return;

            if (CanRectCommonRoi)
            {
                _common.PatternRoiMouseMove(e, _aftZoomRoiLeftUpPt, _fitWindowImageWidth, _fitWindowImageHeight, _ratio[_ratioIndex]);
                if (_common.IsAdjustStatus) return;
                if (_common.IsInRoi == false) Cursor = Cursors.Arrow;
            }

            if (CanRectMaskAndWafer)
            {
                if (!_mask.IsAdjustStatus)
                    _wafer.PatternRoiMouseMove(e, _aftZoomRoiLeftUpPt, _fitWindowImageWidth, _fitWindowImageHeight, _ratio[_ratioIndex]);
                if (!_wafer.IsAdjustStatus)
                    _mask.PatternRoiMouseMove(e, _aftZoomRoiLeftUpPt, _fitWindowImageWidth, _fitWindowImageHeight, _ratio[_ratioIndex]);
                if (_mask.IsAdjustStatus || _wafer.IsAdjustStatus) return;
                if (_mask.IsInRoi == false && _wafer.IsInRoi == false) Cursor = Cursors.Arrow;
            }
            if (_isMouseDown)
            {
                _aftZoomRoiLeftUpPt.X = _ptMousedown.X - e.Location.X + _oldPt.X;
                _aftZoomRoiLeftUpPt.Y = _ptMousedown.Y - e.Location.Y + _oldPt.Y;
                preventRectFail(ref _aftZoomRoiLeftUpPt);
            }
        }

        //avoid roi area not in image plane 
        private void preventRectFail(ref System.Drawing.Point aftZoomRoiLeftUp)
        {
            if (aftZoomRoiLeftUp.X <= 0)
                aftZoomRoiLeftUp.X = 0;

            if (aftZoomRoiLeftUp.Y <= 0)
                aftZoomRoiLeftUp.Y = 0;

            if (aftZoomRoiLeftUp.X + ClientRectangle.Width >= (int)(_fitWindowImageWidth * _ratio[_ratioIndex]))
                aftZoomRoiLeftUp.X = (int)(_fitWindowImageWidth * _ratio[_ratioIndex]) - ClientRectangle.Width;

            if (aftZoomRoiLeftUp.Y + ClientRectangle.Height >= (int)(_fitWindowImageHeight * _ratio[_ratioIndex]))
                aftZoomRoiLeftUp.Y = (int)(_fitWindowImageHeight * _ratio[_ratioIndex]) - ClientRectangle.Height;
        }

        private void SKZoomAndPanWindow_MouseWheel(object sender, MouseEventArgs e)
        {
            if (CanZoom)
            {
                if (e.Delta > 0 && _ratioIndex < _ratio.Length - 1) _ratioIndex++;
                if (e.Delta < 0 && _ratioIndex > 0) _ratioIndex--;
                if (_lastRatioIndex == _ratioIndex) return;

                _aftZoomRoiLeftUpPt.X = (int)((e.X + _aftZoomRoiLeftUpPt.X) * _ratio[_ratioIndex] / _ratio[_lastRatioIndex]) - e.X;
                _aftZoomRoiLeftUpPt.Y = (int)((e.Y + _aftZoomRoiLeftUpPt.Y) * _ratio[_ratioIndex] / _ratio[_lastRatioIndex]) - e.Y;
                _oldPt = _aftZoomRoiLeftUpPt;

                if (_ratioIndex == 0)   //reset oldpt
                {
                    _oldPt.X = 0;
                    _oldPt.Y = 0;
                }

                _lastRatioIndex = _ratioIndex;
                preventRectFail(ref _aftZoomRoiLeftUpPt);
            }
        }

        public void ZoomMin()
        {
            _ratioIndex = 0;
            _aftZoomRoiLeftUpPt.X = 0;
            _aftZoomRoiLeftUpPt.Y = 0;
            _oldPt = _aftZoomRoiLeftUpPt;
            _lastRatioIndex = _ratioIndex;
        }

        private void ImageInput(Mat image)
        {
            try
            {
                if (image == null) return;
                _srcImage = image.GetUMat(AccessFlag.FAST, UMatUsageFlags.DeviceMemory);
                if (_fitWindowImage != null)
                {
                    if (!_fitWindowImage.Empty()) _fitWindowImage.Dispose();
                    _fitWindowImage = null;
                }
                _fitWindowImage = new UMat();
                Cv2.Resize(_srcImage, _fitWindowImage, new OpenCvSharp.Size(ClientSize.Width, ClientSize.Height), 0, 0, InterpolationFlags.Linear); //to fit window
                _getWindowImageEvent.Set();
                _fitWindowImageWidth = _fitWindowImage.Cols;
                _fitWindowImageHeight = _fitWindowImage.Rows;

                _imgMagnificationX = (float)_fitWindowImageWidth / (float)image.Width;
                _imgMagnificationY = (float)_fitWindowImageHeight / (float)image.Height;

                if ((ClientSize.Width / image.Width - ClientSize.Height / image.Height) > 0.05F
                    || (ClientSize.Width / image.Width - ClientSize.Height / image.Height) < -0.05F)
                {
                    if (!_isShowedWarning)
                        MessageBox.Show("Input image size is not compatible with the window", "SKZoomAndPanWindow", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    _isShowedWarning = true;
                }
                if (!_isCreatedMaskAndWaferRect)
                {
                    _mask = new SKWindowLearnPair(new Rectangle((int)(ClientSize.Width * 0.6), (int)(ClientSize.Height * 0.4), (int)(ClientSize.Width / 5), (int)(ClientSize.Width / 5)), this);
                    _wafer = new SKWindowLearnPair(new Rectangle((int)(ClientSize.Width * 0.2), (int)(ClientSize.Height * 0.45), (int)(ClientSize.Width / 10), (int)(ClientSize.Width / 10)), this);
                    _isCreatedMaskAndWaferRect = true;
                }
                if (!_isCreatedCommonRect)
                {
                    _common = new SKWindowLearnPair(new Rectangle((int)(ClientSize.Width * 0.4), (int)(ClientSize.Height * 0.4), (int)(ClientSize.Width / 5), (int)(ClientSize.Width / 5)), this);
                    _isCreatedCommonRect = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void DrawImage()
        {
            try
            {
                UMat localFitWindowImage = null;
                lock (ImageLock)
                {
                    if(_fitWindowImage==null|| _fitWindowImage.Empty())
                    {
                        return;
                    }
                    localFitWindowImage = _fitWindowImage;
                }
                //if (!_fitWindowImage.Empty())
                //{
                using (Mat roi = new Mat())
                {
                    try
                    {
                        if(localFitWindowImage==null || localFitWindowImage.Empty())
                        {
                            return;
                        }
                        //int failTimes = 0;
                        //roiCheckPoint:   //to check if ZoomRatio changed after resize before DstImage = new Mat(roi, s);
                        Cv2.Resize(_fitWindowImage, roi, new OpenCvSharp.Size(0, 0), _ratio[_ratioIndex], _ratio[_ratioIndex], InterpolationFlags.Linear);
                        Rect s = new Rect(_aftZoomRoiLeftUpPt.X, _aftZoomRoiLeftUpPt.Y, ClientSize.Width, ClientSize.Height);
                        if (0 <= s.X && 0 <= s.Width && s.X + s.Width <= roi.Cols
                            && 0 <= s.Y && 0 <= s.Height && s.Y + s.Height <= roi.Rows)
                        {
                            using (Mat dst = new Mat(roi, s))
                            {
                                if (_dstImage != null && !_dstImage.IsDisposed)
                                {
                                    _dstImage.Dispose();
                                    _dstImage = null;
                                }
                                _dstImage = dst.Clone();

                                if (_dstImage.Channels() == 1)
                                    Cv2.CvtColor(_dstImage, _dstImage, ColorConversionCodes.GRAY2BGR);

                                if (CanZoom) //draw zoom led
                                {
                                    for (int i = 0; i < _ratio.Length; i++)
                                    {
                                        Cv2.Rectangle(_dstImage, new Rect(10, 10 + 15 * i, 15, 8),
                                        new Scalar(0, 125, 0), 2);
                                    }
                                    Cv2.Rectangle(_dstImage, new Rect(10, 10 + 15 * _ratioIndex, 15, 8),
                                        new Scalar(0, 200, 0), -1);
                                }
                                if (CanRectMaskAndWafer)
                                {
                                    _dstImage = _mask.DrawPatternRectangle(_dstImage, _ratio[_ratioIndex], _aftZoomRoiLeftUpPt, new Scalar(0, 255, 0));
                                    _dstImage = _wafer.DrawPatternRectangle(_dstImage, _ratio[_ratioIndex], _aftZoomRoiLeftUpPt, new Scalar(0, 0, 255));
                                    if (CanRectLineScanArea)
                                    {
                                        Cv2.PutText(_dstImage, "Vertical", new OpenCvSharp.Point((int)_mask.PatternRoi.X - 10, (int)_mask.PatternRoi.Y - 10), HersheyFonts.HersheyComplexSmall, 1, new Scalar(0, 255, 0));
                                        Cv2.PutText(_dstImage, "Horizontal", new OpenCvSharp.Point((int)_wafer.PatternRoi.X - 10, (int)_wafer.PatternRoi.Y - 10), HersheyFonts.HersheyComplexSmall, 1, new Scalar(0, 0, 255));
                                    }
                                }
                                if (CanRectCommonRoi)
                                {
                                    _dstImage = _common.DrawPatternRectangle(_dstImage, _ratio[_ratioIndex], _aftZoomRoiLeftUpPt, new Scalar(255, 0, 0));
                                }
                                if (CanCenterLine)
                                {
                                    Cv2.Line(_dstImage, (int)(_dstImage.Cols / 2), 0, (int)(_dstImage.Cols / 2), _dstImage.Rows, new Scalar(255, 0, 0));
                                    Cv2.Line(_dstImage, 0, (int)(_dstImage.Rows / 2), _dstImage.Cols, (int)(_dstImage.Rows / 2), new Scalar(255, 0, 0));
                                }
                                if (CanTrackPattern)
                                {
                                    if (MaskMp != null && WaferMp != null)
                                    {
                                        if (_ratioIndex < 0 || _ratioIndex >= _ratio.Length)
                                            throw new IndexOutOfRangeException("_rationIndex 超出範圍");

                                        float scale = _ratio[_ratioIndex];
                                        if (scale == 0 || float.IsNaN(scale))
                                            throw new InvalidOperationException("不合法的縮放比例");

                                        int maskPointX = (int)Math.Round(MaskMp.X * _imgMagnificationX * scale);
                                        int maskPointY = (int)Math.Round(MaskMp.Y * _imgMagnificationY * scale);

                                        OpenCvSharp.Point maskPoint = new OpenCvSharp.Point(maskPointX, maskPointY);

                                        int waferPointX = (int)Math.Round(WaferMp.X * _imgMagnificationX * scale);
                                        int waferPointY = (int)Math.Round(WaferMp.Y * _imgMagnificationY * scale);

                                        OpenCvSharp.Point waferPoint = new OpenCvSharp.Point(waferPointX, waferPointY);


                                        if (s.Contains(maskPoint) || s.Contains(waferPoint))
                                        {
                                            Cv2.Line(_dstImage, maskPoint.X - s.X - 60, maskPoint.Y - s.Y, maskPoint.X - s.X - 30, maskPoint.Y - s.Y, new Scalar(0, 255, 0));
                                            Cv2.Line(_dstImage, maskPoint.X - s.X + 30, maskPoint.Y - s.Y, maskPoint.X - s.X + 60, maskPoint.Y - s.Y, new Scalar(0, 255, 0));
                                            Cv2.Line(_dstImage, maskPoint.X - s.X, maskPoint.Y - s.Y - 60, maskPoint.X - s.X, maskPoint.Y - s.Y - 30, new Scalar(0, 255, 0));
                                            Cv2.Line(_dstImage, maskPoint.X - s.X, maskPoint.Y - s.Y + 30, maskPoint.X - s.X, maskPoint.Y - s.Y + 60, new Scalar(0, 255, 0));
                                            Cv2.Line(_dstImage, waferPoint.X - s.X, waferPoint.Y - s.Y - 30, waferPoint.X - s.X, waferPoint.Y - s.Y + 30, new Scalar(0, 0, 255));
                                            Cv2.Line(_dstImage, waferPoint.X - s.X - 30, waferPoint.Y - s.Y, waferPoint.X - s.X + 30, waferPoint.Y - s.Y, new Scalar(0, 0, 255));
                                        }
                                    }
                                }
                                if (CanShowOcrInfo)
                                {
                                    if (OcrInfoMp.Count > 0)
                                    {
                                        for (int i = 0; i < OcrInfoMp.Count; i++)
                                        {
                                            if (OcrInfoMp[i] != null)
                                            {
                                                Rect ocrRect = new Rect((int)OcrInfoMp[i].X, (int)OcrInfoMp[i].Y, (int)OcrInfoMp[i].TemplateSize.Width, (int)OcrInfoMp[i].TemplateSize.Height);
                                                ocrRect.X = (int)(ocrRect.X * _imgMagnificationX * _ratio[_ratioIndex] - s.X);
                                                ocrRect.Y = (int)(ocrRect.Y * _imgMagnificationY * _ratio[_ratioIndex] - s.Y);
                                                ocrRect.Width = (int)(ocrRect.Width * _imgMagnificationX * _ratio[_ratioIndex]);
                                                ocrRect.Height = (int)(ocrRect.Height * _imgMagnificationY * _ratio[_ratioIndex]);

                                                Cv2.PutText(_dstImage, OcrInfoMp[i].Char.ToString(), new OpenCvSharp.Point((int)ocrRect.X, (int)(ocrRect.Y - ocrRect.Height * 0.5)), HersheyFonts.HersheyComplexSmall, 1 * _ratio[_ratioIndex], new Scalar(0));
                                                Cv2.Rectangle(_dstImage, ocrRect, new Scalar(0), 1, LineTypes.Link4);
                                                if (i % 2 == 1)
                                                    Cv2.PutText(_dstImage, OcrInfoMp[i].Score.ToString("f2"), new OpenCvSharp.Point((int)ocrRect.X, (int)(ocrRect.Y - ocrRect.Height * 0.1)), HersheyFonts.HersheyComplexSmall, 0.5 * _ratio[_ratioIndex], new Scalar(0));
                                                else
                                                    Cv2.PutText(_dstImage, OcrInfoMp[i].Score.ToString("f2"), new OpenCvSharp.Point((int)ocrRect.X, (int)(ocrRect.Y + ocrRect.Height * 1.3)), HersheyFonts.HersheyComplexSmall, 0.5 * _ratio[_ratioIndex], new Scalar(0));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (OpenCvSharp.OpenCVException)
                    {
                        return;
                    }
                }
                //}
            }
            catch (Exception)
            {
                throw;
            }
        }

        public UMat ShowOcrInfo(UMat img)
        {

            return img;
        }

        Graphics graphics;

        bool CreateGraphic = false;

        private void DisplayImage()
        {
            try
            {
                if (!_fitWindowImage.Empty())
                {
                    lock (ImageLock) 
                    {
                        if (_dstImage == null || _dstImage.IsDisposed)
                        {
                            return;
                        }
                        using (Mat tempMat = _dstImage.Clone())
                        {
                            using (Bitmap showImg = BitmapConverter.ToBitmap(tempMat))
                            {
                                if (!CreateGraphic)
                                {
                                    graphics = this.CreateGraphics();
                                    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                                    graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                                    graphics.PageUnit = GraphicsUnit.Pixel;
                                    CreateGraphic = true;

                                }
                                graphics.DrawImage(showImg, ClientRectangle.Location);
                            }
                        }
                        //_dstImage.Dispose();
                        //_dstImage = null;
                        if (_dstImage != null && !_dstImage.IsDisposed)
                        {
                            _dstImage.Dispose();
                            _dstImage = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error displaying image: " + ex.Message);
            }
            finally
            {

            }
        }
        //private void DisplayImage()
        //{
        //    try
        //    {
        //        if (!_fitWindowImage.IsEmpty)
        //        {
        //            // //drawImage();
        //            Bitmap showImg = _dstImage.ToBitmap();
        //            // Image<Bgr, Byte> OutImg = _dstImage.ToImage<Bgr, Byte>();
        //            // Bitmap showImg = new Bitmap(OutImg.ToBitmap());

        //            if (!CreateGraphic)
        //            {
        //                G = this.CreateGraphics();
        //                G.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        //                G.InterpolationMode = InterpolationMode.NearestNeighbor;
        //                G.PageUnit = GraphicsUnit.Pixel;
        //                CreateGraphic = true;
        //            }
        //            G.DrawImage(showImg, ClientRectangle.Location);

        //        }
        //    }
        //    catch (Exception)
        //    {

        //    }
        //}
        //public void SetImage(Mat image)
        //{
        //    Mat dimage = image.Clone();

        //    if (bwithMaskImage)
        //    {
        //        double beta = 1.0 - alpha;
        //        CvInvoke.AddWeighted(image, alpha, _MaskImage, beta, 0.0, dimage);
        //    } 

        //    imageInput(dimage);
        //    drawImage();
        //    displayImage();
        //}

        public void SetImage(Mat image)
        {
            if (image != null)
            {
                lock (ImageLock)
                {
                    if (SetImageCount > SetImageCountLimit)
                    {
                        if (Dimage != null)
                        {
                            Dimage = null;
                            Dimage = new Mat(image.Width, image.Height, MatType.CV_8UC1);
                        }
                        else
                        {
                            Dimage = new Mat(image.Width, image.Height, MatType.CV_8UC1);
                        }
                        SetImageCount = 0;
                    }
                    else
                    {
                        if (Dimage == null)
                        {
                            Dimage = new Mat(image.Width, image.Height, MatType.CV_8UC1);
                        }
                        SetImageCount++;
                    }
                }

                if (bwithMaskImage)
                {
                    if(_MaskImage.Size()== image.Size() && _MaskImage.Type() == image.Type())
                    {
                        double beta = 1.0 - alpha;
                        Cv2.AddWeighted(image, alpha, _MaskImage, beta, 0.0, Dimage);
                    }
                    else
                    {
                        image.CopyTo(Dimage);
                    }                    
                }
                else
                {
                    image.CopyTo(Dimage);
                }
                ImageInput(Dimage);
                DrawImage();
                DisplayImage();
                GC.KeepAlive(Dimage);
            }
        }
        

        public void SetShowMask(bool bShow, double dalpha, Mat MaskImage)
        {
            if (bShow)
            {
                //bwithMaskImage = true;
                //_MaskImage = MaskImage.Clone();
                //alpha = dalpha;
                if (MaskImage == null || MaskImage.Empty())
                {
                    bwithMaskImage = false;
                    alpha = 0;
                    _MaskImage = null;
                    return;
                }

                bwithMaskImage = true;
                _MaskImage = MaskImage.Clone();
                alpha = dalpha;
            }
            else
            {
                //bwithMaskImage = false;
                //alpha = 0;
                //// _MaskImage.Create(MaskImage.Width, MaskImage.Height, Emgu.CV.CvEnum.DepthType.Default, 0);
                //_MaskImage = MaskImage.Clone();
                bwithMaskImage = false;
                alpha = 0;

                // 不顯示時不需要克隆 MaskImage
                if (_MaskImage != null)
                {
                    _MaskImage.Dispose();
                    _MaskImage = null;
                }
            }
        }

        public void setMaskImage(Mat MaskImage)
        {
            _MaskImage = MaskImage.Clone();
        }
        //public Rectangle GetMaskRectangle()
        //{
        //    RectangleF correctRectF= new RectangleF(0,0,2584,1944);
         
        //    try
        //    {
        //        if (_mask != null)
        //        {
        //            correctRectF = new RectangleF(_mask.PatternRoi.X / _imgMagnificationX, _mask.PatternRoi.Y / _imgMagnificationY
        //                , _mask.PatternRoi.Width / _imgMagnificationX, _mask.PatternRoi.Height / _imgMagnificationY);
        //        }
        //    }
        //    catch (Exception)
        //    {

        //    }

        //    Rectangle correctRect = Rectangle.Round(correctRectF);
        //    return correctRect;
        //}
        public Rect GetMaskRect()
        {
            Rect correctRect = new Rect(0, 0, 4000, 3000);

            try
            {
                if (_mask != null)
                {
                    correctRect = new Rect((int)(_mask.PatternRoi.X / _imgMagnificationX), (int)(_mask.PatternRoi.Y / _imgMagnificationY)
                        , (int)(_mask.PatternRoi.Width / _imgMagnificationX), (int)(_mask.PatternRoi.Height / _imgMagnificationY));
                }
            }
            catch (Exception)
            {

            }            
            return correctRect;
        }
        public Rect GetWaferRect()
        {
            Rect correctRect = new Rect(0, 0, 4000, 3000);
            try
            {
                if (_wafer != null)
                {
                    correctRect = new Rect((int)(_wafer.PatternRoi.X / _imgMagnificationX), (int)(_wafer.PatternRoi.Y / _imgMagnificationY)
                        , (int)(_wafer.PatternRoi.Width / _imgMagnificationX), (int)(_wafer.PatternRoi.Height / _imgMagnificationY));
                }
            }
            catch (Exception)
            {
            }
            return correctRect;

        }
        public OpenCvSharp.Rect GetCommonRectangle()
        {
            
            float defaultWidth = 2584f;
            float defaultHeight = 1944f;
            RectangleF correctRectF = new RectangleF(0, 0, defaultWidth, defaultHeight);
            //try
            //{
            //    if (_common != null)
            //    {
            //        correctRectF = new RectangleF(_common.PatternRoi.X / _imgMagnificationX, _common.PatternRoi.Y / _imgMagnificationY
            //            , _common.PatternRoi.Width / _imgMagnificationX, _common.PatternRoi.Height / _imgMagnificationY);
            //    }
            //}
            //catch (Exception)
            //{

            //}
            if (_common != null && _imgMagnificationX > 0 && _imgMagnificationY > 0)
            {
                correctRectF = new RectangleF(
                    _common.PatternRoi.X/_imgMagnificationX,
                    _common.PatternRoi.Y/_imgMagnificationY,
                    _common.PatternRoi.Width/_imgMagnificationX,
                    _common.PatternRoi.Height/ _imgMagnificationY
                    );
            }
            int x= (int)Math.Round(correctRectF.X);
            int y= (int)Math.Round(correctRectF.Y);
            int width= (int)Math.Round(correctRectF.Width);
            int height= (int)Math.Round(correctRectF.Height);
            return new OpenCvSharp.Rect(x,y,width,height);
        }
        public OpenCvSharp.Rect GetCommonRectangleO()
        {
            float defaultWidth = 2584f;
            float defaultHeight = 1944f;
            RectangleF roiRectF = new RectangleF(0, 0, defaultWidth, defaultHeight);

            try
            {
                if (_common != null && _imgMagnificationX > 0 && _imgMagnificationY > 0)
                {
                    roiRectF = new RectangleF(
                        _common.PatternRoi.X / _imgMagnificationX,
                        _common.PatternRoi.Y / _imgMagnificationY,
                        _common.PatternRoi.Width / _imgMagnificationX,
                        _common.PatternRoi.Height / _imgMagnificationY
                        );
                }

            }
            catch (Exception e) 
            {
                
            }
            int x = (int)Math.Round(roiRectF.X);
            int y = (int)Math.Round(roiRectF.Y);
            int width = (int)Math.Round(roiRectF.Width);
            int height = (int)Math.Round(roiRectF.Height);
            return new OpenCvSharp.Rect(x, y, width, height);
        }
        /*
        public Mat GetSrcImage()
        {
            lock (Lock)
            {
                IsGettingSrcImage = true;
                while (true)
                {
                    if (IsGettingSrcImage == false)
                        return GettingSrcImage;
                    Thread.Sleep(10);
                }
            }
        }
        */
        public Mat GetWindowImage()
        {
            lock (_lock)
            {
                //_isGettingWindowImage = true;
                //while (true)
                //{
                //    if (_isGettingWindowImage == false)
                //        return _gettingWindowImage.GetMat(AccessType.Fast);
                //    Thread.Sleep(10);
                //}
                if (!_getWindowImageEvent.WaitOne(1000))
                    throw new Exception("Get window image fail.");
                else
                    return _fitWindowImage.GetMat(AccessFlag.FAST);
            }
        }

        public ZoomInfo GetZoomInfo()
        {
            ZoomInfo zi = new ZoomInfo();
            zi.ZoomRatio = _ratio[_ratioIndex];
            zi.AftFitMagnificationX = _imgMagnificationX;
            zi.AftFitMagnificationY = _imgMagnificationY;
            return zi;
        }


        public void ChangeCommonRectLocation(Rectangle r)
        {
            _common.PatternRoi = r;
        }

        public void MaskMove(int x, int y)
        {
            if (_mask != null)
            {
                _mask.PatternRoi.X += x / _ratio[_ratioIndex];
                _mask.PatternRoi.Y += y / _ratio[_ratioIndex];

                _mask.PatternOldRoi.X += x / _ratio[_ratioIndex];
                _mask.PatternOldRoi.Y += y / _ratio[_ratioIndex];
            }
        }

        public void WaferMove(int x, int y)
        {
            if (_wafer != null)
            {
                _wafer.PatternRoi.X += x / _ratio[_ratioIndex];
                _wafer.PatternRoi.Y += y / _ratio[_ratioIndex];

                _wafer.PatternOldRoi.X += x / _ratio[_ratioIndex];
                _wafer.PatternOldRoi.Y += y / _ratio[_ratioIndex];
            }
        }

        public void MaskOriginalMove(float x, float y)
        {
            _mask.PatternRoi.X += x;
            _mask.PatternRoi.Y += y;

            _mask.PatternOldRoi.X += x;
            _mask.PatternOldRoi.Y += y;
        }

        public void WaferOriginalMove(float x, float y)
        {
            _wafer.PatternRoi.X += x;
            _wafer.PatternRoi.Y += y;

            _wafer.PatternOldRoi.X += x;
            _wafer.PatternOldRoi.Y += y;
        }
    }

    public class ZoomInfo
    {
        public float ZoomRatio;
        public float AftFitMagnificationX;
        public float AftFitMagnificationY;
    }
}
