using System;

using System.Drawing;
using System.Drawing.Drawing2D;

using System.Windows.Forms;
using Emgu.CV;
using System.Threading;
using Emgu.CV.CvEnum;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MRVisionLib
{
    public partial class SKZoomAndPanWindow : UserControl
    {
        //zoom parm//
        private int _ratioIndex = 0;
        private int _lastRatioIndex = 0;
        private float[] _ratio = new float[7];

        //image//
        private object _lock = new object();
        private UMat _srcImage, _fitWindowImage, _dstImage;
        private int _fitWindowImageWidth, _fitWindowImageHeight;
        private float _imgMagnificationX;
        private float _imgMagnificationY;

        private bool _isGettingWindowImage;
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
        private Point _aftZoomRoiLeftUpPt = new Point(0, 0);
        private Point _oldPt = new Point(0, 0);
        private Point _ptMouseMove = new Point(0, 0);
        private Point _ptMousedown = new Point(0, 0);
        private Point _ptMouseWheel = new Point(0, 0);

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
            if (CanRectCommonRoi) _common.PatternRoiMouseDown(e);
            if (CanRectMaskAndWafer)
            {
                _mask.PatternRoiMouseDown(e);
                _wafer.PatternRoiMouseDown(e);
            }
        }

        private void SKZoomAndPanWindow_MouseUp(object sender, MouseEventArgs e)
        {
            _isMouseDown = false;
            _oldPt = _aftZoomRoiLeftUpPt;
            if (CanRectCommonRoi) _common.PatternRoiMouseUp();
            if (CanRectMaskAndWafer)
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

        private void SKZoomAndPanWindow_MouseWheel(object sender, MouseEventArgs e)
        {
            if (CanZoom)
            {
                if (e.Delta > 0 && _ratioIndex < _ratio.Length - 1) _ratioIndex++;  
                if (e.Delta < 0 && _ratioIndex > 0 ) _ratioIndex--;
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


        //avoid roi area not in image plane 
        private void preventRectFail(ref Point aftZoomRoiLeftUp)
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

        private void imageInput(Mat image)
        {
            try
            {
                if (image == null) return;
                _srcImage =  image.GetUMat(Emgu.CV.CvEnum.AccessType.Fast);
                _fitWindowImage = new UMat();
                CvInvoke.Resize(_srcImage, _fitWindowImage, ClientSize, 0, 0, Emgu.CV.CvEnum.Inter.Linear); //to fit window
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


        private void drawImage()
        {
            try
            {
                if (!_fitWindowImage.IsEmpty)
                {
                    
                    UMat roi = new UMat();
                    int failTimes = 0;
                roiCheckPoint:   //to check if ZoomRatio changed after resize before DstImage = new Mat(roi, s);
                    CvInvoke.Resize(_fitWindowImage, roi, new Size(0, 0), _ratio[_ratioIndex], _ratio[_ratioIndex], Emgu.CV.CvEnum.Inter.Linear);
                    Rectangle s = new Rectangle(_aftZoomRoiLeftUpPt, ClientSize);
                    if (0 <= s.X && 0 <= s.Width && s.X + s.Width <= roi.Cols
                        && 0 <= s.Y && 0 <= s.Height && s.Y + s.Height <= roi.Rows)
                    {
                        _dstImage = new UMat(roi, s);
                        if (_dstImage.NumberOfChannels == 1)
                            CvInvoke.CvtColor(_dstImage, _dstImage, Emgu.CV.CvEnum.ColorConversion.Gray2Bgr);
                        if (CanZoom) //draw zoom led
                        {
                            for (int i = 0; i < _ratio.Length; i++)
                            {
                                CvInvoke.Rectangle(_dstImage, new Rectangle(10, 10 + 15 * i, 15, 8),
                                new Emgu.CV.Structure.MCvScalar(0, 125, 0), 2);
                            }
                            CvInvoke.Rectangle(_dstImage, new Rectangle(10, 10 + 15 * _ratioIndex, 15, 8),
                                new Emgu.CV.Structure.MCvScalar(0, 200, 0), -1);
                        }                       
                        if (CanRectMaskAndWafer)
                        {
                            _dstImage = _mask.DrawPatternRectangle(_dstImage, _ratio[_ratioIndex], _aftZoomRoiLeftUpPt, new Emgu.CV.Structure.MCvScalar(0, 255, 0));
                            _dstImage = _wafer.DrawPatternRectangle(_dstImage, _ratio[_ratioIndex], _aftZoomRoiLeftUpPt, new Emgu.CV.Structure.MCvScalar(0, 0, 255));
                            if (CanRectLineScanArea)
                            {
                                CvInvoke.PutText(_dstImage, "Vertical", new Point((int)_mask.PatternRoi.X - 10, (int)_mask.PatternRoi.Y - 10), FontFace.HersheyComplexSmall, 1, new Emgu.CV.Structure.MCvScalar(0, 255, 0));
                                CvInvoke.PutText(_dstImage, "Horizontal", new Point((int)_wafer.PatternRoi.X - 10, (int)_wafer.PatternRoi.Y - 10), FontFace.HersheyComplexSmall, 1, new Emgu.CV.Structure.MCvScalar(0, 0, 255));
                            }
                        }
                        if (CanRectCommonRoi)
                        {
                            _dstImage = _common.DrawPatternRectangle(_dstImage, _ratio[_ratioIndex], _aftZoomRoiLeftUpPt, new Emgu.CV.Structure.MCvScalar(255, 0, 0));
                        }
                        if (CanCenterLine)
                        {
                            CvInvoke.Line(_dstImage, new Point(_dstImage.Cols / 2, 0), new Point(_dstImage.Cols / 2, _dstImage.Rows), new Emgu.CV.Structure.MCvScalar(255, 0, 0));
                            CvInvoke.Line(_dstImage, new Point(0, _dstImage.Rows / 2), new Point(_dstImage.Cols, _dstImage.Rows / 2), new Emgu.CV.Structure.MCvScalar(255, 0, 0));
                        }
                        if (CanTrackPattern)
                        {
                            if (MaskMp != null && WaferMp != null)
                            {
                                Point maskPt = Point.Round(new PointF(MaskMp.X * _imgMagnificationX * _ratio[_ratioIndex], MaskMp.Y * _imgMagnificationY * _ratio[_ratioIndex]));
                                Point waferPt = Point.Round(new PointF(WaferMp.X * _imgMagnificationX * _ratio[_ratioIndex], WaferMp.Y * _imgMagnificationY * _ratio[_ratioIndex]));
                                if (s.Contains(maskPt) || s.Contains(waferPt))
                                {
                                    //CvInvoke.Line(DstImage, new Point(maskPt.X - s.X, maskPt.Y - s.Y - 30) , new Point(maskPt.X - s.X, maskPt.Y - s.Y + 30)
                                    //    ,new Emgu.CV.Structure.MCvScalar(0, 255, 0));
                                    //CvInvoke.Line(DstImage, new Point(maskPt.X - s.X - 30, maskPt.Y - s.Y), new Point(maskPt.X - s.X + 30, maskPt.Y - s.Y)
                                    //    , new Emgu.CV.Structure.MCvScalar(0, 255, 0));
                                    CvInvoke.Line(_dstImage, new Point(maskPt.X - s.X - 60, maskPt.Y - s.Y), new Point(maskPt.X - s.X - 30, maskPt.Y - s.Y)
                                        , new Emgu.CV.Structure.MCvScalar(0, 255, 0));
                                    CvInvoke.Line(_dstImage, new Point(maskPt.X - s.X + 30, maskPt.Y - s.Y), new Point(maskPt.X - s.X + 60, maskPt.Y - s.Y)
                                        , new Emgu.CV.Structure.MCvScalar(0, 255, 0));
                                    CvInvoke.Line(_dstImage, new Point(maskPt.X - s.X, maskPt.Y - s.Y - 60), new Point(maskPt.X - s.X, maskPt.Y - s.Y - 30)
                                        ,new Emgu.CV.Structure.MCvScalar(0, 255, 0));
                                    CvInvoke.Line(_dstImage, new Point(maskPt.X - s.X, maskPt.Y - s.Y + 30), new Point(maskPt.X - s.X, maskPt.Y - s.Y + 60)
                                        , new Emgu.CV.Structure.MCvScalar(0, 255, 0));
                                    CvInvoke.Line(_dstImage, new Point(waferPt.X - s.X, waferPt.Y - s.Y - 30), new Point(waferPt.X - s.X, waferPt.Y - s.Y + 30)
                                        , new Emgu.CV.Structure.MCvScalar(0, 0, 255));
                                    CvInvoke.Line(_dstImage, new Point(waferPt.X - s.X - 30, waferPt.Y - s.Y), new Point(waferPt.X - s.X + 30, waferPt.Y - s.Y)
                                        , new Emgu.CV.Structure.MCvScalar(0, 0, 255));
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
                                        Rectangle ocrRect = new Rectangle((int)OcrInfoMp[i].X, (int)OcrInfoMp[i].Y, (int)OcrInfoMp[i].TemplateSize.Width, (int)OcrInfoMp[i].TemplateSize.Height);
                                        ocrRect.X = (int)(ocrRect.X * _imgMagnificationX * _ratio[_ratioIndex] - s.X);
                                        ocrRect.Y = (int)(ocrRect.Y * _imgMagnificationY * _ratio[_ratioIndex] - s.Y);
                                        ocrRect.Width = (int)(ocrRect.Width * _imgMagnificationX * _ratio[_ratioIndex]);
                                        ocrRect.Height = (int)(ocrRect.Height * _imgMagnificationY * _ratio[_ratioIndex]);

                                        CvInvoke.PutText(_dstImage, OcrInfoMp[i].Char.ToString(), new Point((int)ocrRect.X, (int)(ocrRect.Y - ocrRect.Height * 0.5)), Emgu.CV.CvEnum.FontFace.HersheyComplexSmall, 1 * _ratio[_ratioIndex], new Emgu.CV.Structure.MCvScalar(0));
                                        CvInvoke.Rectangle(_dstImage, ocrRect, new Emgu.CV.Structure.MCvScalar(0), 1, Emgu.CV.CvEnum.LineType.FourConnected);
                                        if (i % 2 == 1)
                                            CvInvoke.PutText(_dstImage, OcrInfoMp[i].Score.ToString("f2"), new Point((int)ocrRect.X, (int)(ocrRect.Y - ocrRect.Height * 0.1)), Emgu.CV.CvEnum.FontFace.HersheyComplexSmall, 0.5 * _ratio[_ratioIndex], new Emgu.CV.Structure.MCvScalar(0));
                                        else
                                            CvInvoke.PutText(_dstImage, OcrInfoMp[i].Score.ToString("f2"), new Point((int)ocrRect.X, (int)(ocrRect.Y + ocrRect.Height * 1.3)), Emgu.CV.CvEnum.FontFace.HersheyComplexSmall, 0.5 * _ratio[_ratioIndex], new Emgu.CV.Structure.MCvScalar(0));
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        failTimes++;
                        if (failTimes > 3) return;
                        goto roiCheckPoint;
                    }
                    
                    roi.Dispose();
                }
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

        Graphics G;
        bool CreateGraphic = false;
        private void displayImage()
        {
            try
            {
                if (!_fitWindowImage.IsEmpty)
                {
                    //drawImage();
                    Bitmap showImg = _dstImage.ToBitmap();

                    if (!CreateGraphic)
                    {
                        G = this.CreateGraphics();
                        G.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        G.InterpolationMode = InterpolationMode.NearestNeighbor;
                        G.PageUnit = GraphicsUnit.Pixel;
                        CreateGraphic = true;
                    }
                    G.DrawImage(showImg, ClientRectangle.Location);

                    //if (_isGettingWindowImage)
                    //{
                    //    if (_gettingWindowImage != null) _gettingWindowImage.Dispose();
                    //    _gettingWindowImage = _dstImage.Clone();
                    //    _isGettingWindowImage = false;
                    //}
                
                    //showImg.Dispose();
                    //SrcImage.Dispose();
                    //FitWindowImage.Dispose();
                    //DstImage.Dispose();
                    //G.Dispose();
                    GC.Collect();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SetImage(Mat image)
        {
            imageInput(image);
            drawImage();
            displayImage();
        }

        public Rectangle GetMaskRectangle()
        {
            RectangleF correctRectF = new RectangleF(_mask.PatternRoi.X / _imgMagnificationX, _mask.PatternRoi.Y / _imgMagnificationY
                , _mask.PatternRoi.Width / _imgMagnificationX, _mask.PatternRoi.Height / _imgMagnificationY);
            Rectangle correctRect = Rectangle.Round(correctRectF);
            return correctRect;
        }
        public Rectangle GetWaferRectangle()
        {
            RectangleF correctRectF = new RectangleF(_wafer.PatternRoi.X / _imgMagnificationX, _wafer.PatternRoi.Y / _imgMagnificationY
                , _wafer.PatternRoi.Width / _imgMagnificationX, _wafer.PatternRoi.Height / _imgMagnificationY);
            Rectangle correctRect = Rectangle.Round(correctRectF);
            return correctRect;
        }
        public Rectangle GetCommonRectangle()
        {
            RectangleF correctRectF = new RectangleF(_common.PatternRoi.X / _imgMagnificationX, _common.PatternRoi.Y / _imgMagnificationY
                , _common.PatternRoi.Width / _imgMagnificationX, _common.PatternRoi.Height / _imgMagnificationY);
            Rectangle correctRect = Rectangle.Round(correctRectF);
            return correctRect;
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
                    return _fitWindowImage.GetMat(AccessType.Fast);


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
            _mask.PatternRoi.X += x / _ratio[_ratioIndex];
            _mask.PatternRoi.Y += y / _ratio[_ratioIndex];

            _mask.PatternOldRoi.X += x / _ratio[_ratioIndex];
            _mask.PatternOldRoi.Y += y / _ratio[_ratioIndex];
        }

        public void WaferMove(int x, int y)
        {
            _wafer.PatternRoi.X += x / _ratio[_ratioIndex];
            _wafer.PatternRoi.Y += y / _ratio[_ratioIndex];

            _wafer.PatternOldRoi.X += x / _ratio[_ratioIndex];
            _wafer.PatternOldRoi.Y += y / _ratio[_ratioIndex];
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
