using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using OpenCvSharp;
using OpenCvSharp.Extensions;


namespace TemplatesMask
{
    public partial class TemplateMask : UserControl
    {
        public TemplateMask()
        {
            InitializeComponent();
        }
        System.Drawing.Bitmap [] _temporaryImage=new System.Drawing.Bitmap[3];
        Mat _templImage;
        Mat _maskImage;
        Mat _preparationImage;
        Mat _backImage;
        bool CanIRun;
        bool ISUpdata;
        bool ISMouseDown;
        bool IsMouseDownType2;
        Point _center;
        Point _mouseMovePt;
        Scalar _whiteBackground;
        Scalar _blackBackground;
        Scalar _maskColor;
        Scalar _dragMaskColor;
        Rect _displayRoi;
        Rect _originalRoi;
        Rect _maskSizeType;
        Rect _maskSizeType1;
        Rect _maskSizeType2;
        Rect _dragDetectionRange;
        Rect _dragDetectionRangeMode1;
        Rect _dragDetectionRangeMode2;
        MaskData maskData = new MaskData();
        public enum Maskmod { None,Cross, Corner, Rectangle };
        Maskmod MaskKide;
        private void initialBackImage()
        {
            if(_backImage==null)
            {
                //_backImage = new Mat(pictureBox1.Width + 200, pictureBox1.Height + 200, _preparationImage.Depth, _preparationImage.ElementSize);
                _backImage = new Mat(pictureBox1.Width+200, pictureBox1.Height+200, _preparationImage.Type());
                _displayRoi = new Rect(0,0, pictureBox1.Width, pictureBox1.Height);
                _originalRoi = _displayRoi;
            }
            
        }
        private void initialMaskSize()
        {
            _maskImage = new Mat(pictureBox1.Width,pictureBox1.Height, MatType.CV_8UC3);
            _whiteBackground = new Scalar(255, 255, 255);
            _blackBackground=new Scalar(0, 0, 0);
            _maskColor = new Scalar(0, 0, 255);
            _center = new Point(pictureBox1.Width / 2, pictureBox1.Height / 2);
            _maskSizeType1 = new Rect(0, 0, (pictureBox1.Width / 5), (pictureBox1.Height / 5));
            _maskSizeType2 = new Rect(_center.X - 50, _center.Y - 50, 100, 100);

        }
        private void initialDragRange()
        {
            _dragDetectionRangeMode1 = new Rect(_maskSizeType1.Right - 10, _maskSizeType1.Bottom - 10, 20, 20);
            _dragDetectionRangeMode2 = new Rect(_maskSizeType2.X - 10, _maskSizeType2.Y - 10, 20, 20);
            _dragMaskColor = new Scalar(255, 255, 0);
        }
        public void InputImage(Mat matImage)
        {
            if (matImage != null)
            {
                _templImage = matImage;
                _preparationImage = matImage.Clone();
                CanIRun = true;
                MaskTypeBox.Enabled = true;
                ISUpdata = true;
            }

        }
        private void fitWindow(Mat original)
        {
                if (original.ElemSize() != 3)
                {
                    grayToRGB(original);
                }
                Cv2.Resize(original, _preparationImage, new Size(pictureBox1.Width,pictureBox1.Height));
        }
        private void grayToRGB(Mat preparationImage)
        {
            Cv2.CvtColor(preparationImage, preparationImage, ColorConversionCodes.GRAY2BGR);
        }
        private void imageOverlayAndCut()
        {
            initialBackImage();

            _backImage.SetTo(_blackBackground, _backImage);
            _preparationImage = new Mat(_backImage, _displayRoi);
            //auxiliaryLine();
        }

        private void auxiliaryLine()
        {
            Point L00 = new Point(pictureBox1.Width, pictureBox1.Height/2);
            Point R00 =new Point(0, pictureBox1.Height / 2);
            Point B00 = new Point(pictureBox1.Width/2, pictureBox1.Height);
            Point T00 = new Point(pictureBox1.Width/2, 0);
            Cv2.Line(_preparationImage, R00, L00, new Scalar(0, 255, 0), 2);
            Cv2.Line(_preparationImage, T00, B00, new Scalar(0, 255, 0), 2);
        }

        private Rect ResizeImageCalculation(Mat mat)
        {
            Rect Resize = new Rect(0, 0, mat.Width, mat.Height);

            if (_originalRoi.X - _displayRoi.X < 0 && _originalRoi.X - _displayRoi.X != 0)
            {
                Resize.X -= Convert.ToInt32(Math.Round((_originalRoi.X - _displayRoi.X) * Convert.ToDouble(Resize.Width) / Convert.ToDouble(pictureBox1.Width)));
                Resize.Width += Convert.ToInt32(Math.Round((_originalRoi.X - _displayRoi.X) * Convert.ToDouble(Resize.Width) / Convert.ToDouble(pictureBox1.Width)));
            }
            if (_originalRoi.X - _displayRoi.X > 0)
            {
                Resize.Width -= Convert.ToInt32(Math.Round((_originalRoi.X - _displayRoi.X) * Convert.ToDouble(Resize.Width) / Convert.ToDouble(pictureBox1.Width)));


            }
            if (_originalRoi.Y - _displayRoi.Y < 0 && _originalRoi.Y - _displayRoi.Y != 0)
            {
                Resize.Y -= Convert.ToInt32(Math.Round((_originalRoi.Y - _displayRoi.Y) * Convert.ToDouble(Resize.Width) / Convert.ToDouble(pictureBox1.Width)));
                Resize.Height += Convert.ToInt32(Math.Round((_originalRoi.Y - _displayRoi.Y) * Convert.ToDouble(Resize.Width) / Convert.ToDouble(pictureBox1.Width)));
            }
            else
            {
                Resize.Height -= Convert.ToInt32(Math.Round((_originalRoi.Y - _displayRoi.Y) * Convert.ToDouble(Resize.Width) / Convert.ToDouble(pictureBox1.Width)));
            }
            return Resize;
        }
        private void CutImage(Mat preparationImage)
        {
                Rect Resize = ResizeImageCalculation(_templImage);
                _preparationImage.Dispose();
                Mat OriginalImg = new Mat(_templImage, Resize);
                _preparationImage = OriginalImg;
        }
        private void Mask()
        {     
                switch (MaskKide)
                {
                    case Maskmod.None:
                        break;
                    case Maskmod.Cross:
                        _dragDetectionRange = _dragDetectionRangeMode1;
                        _maskSizeType = _maskSizeType1;
                        CrossMask();

                        break;
                    case Maskmod.Corner:
                        _dragDetectionRange = _dragDetectionRangeMode1;
                        _maskSizeType = _maskSizeType1;
                        CornerMask();
                        break;
                    case Maskmod.Rectangle:
                        _dragDetectionRange = _dragDetectionRangeMode2;
                        _maskSizeType = _maskSizeType2;
                        RectangleMask();
                        break;
                }
        }
        private void CrossMask()
        {
            _maskImage.SetTo(_blackBackground);
            Cv2.Rectangle(_maskImage, _maskSizeType, _whiteBackground, -1);
            _maskSizeType.X += (pictureBox1.Width - _maskSizeType.Width);
            Cv2.Rectangle(_maskImage, _maskSizeType, _whiteBackground, -1);
            _maskSizeType.Y += (pictureBox1.Height - _maskSizeType.Height);
            Cv2.Rectangle(_maskImage, _maskSizeType, _whiteBackground, -1);
            _maskSizeType.X -= (pictureBox1.Width - _maskSizeType.Width);
            Cv2.Rectangle(_maskImage, _maskSizeType, _whiteBackground, -1);
            _maskSizeType.Y = 0;
            _maskSizeType.X = 0;
        }
        private void CornerMask()
        {
            CrossMask();
            Cv2.BitwiseNot(_maskImage, _maskImage);
        }
        private void RectangleMask()
        {
            _maskImage.SetTo(new Scalar(0, 0, 0));
            Cv2.Rectangle(_maskImage, _maskSizeType, _whiteBackground, -1);
            Cv2.BitwiseNot(_maskImage, _maskImage);


        }
        private void Stacked(Mat toplayer,Mat backgroundlayer, Rect dragdetectionrange)
        {
            if (toplayer != null&& backgroundlayer != null)
            {
                Mat newlayer = new Mat(toplayer.Width, toplayer.Height, toplayer.ElemSize());
                Cv2.BitwiseNot(toplayer, newlayer);
                newlayer.SetTo(_maskColor, newlayer);
                Cv2.Rectangle(newlayer, dragdetectionrange, _dragMaskColor, 2);
                Cv2.AddWeighted(backgroundlayer, 0.9, newlayer, 0.4, 0, _preparationImage);
                newlayer.Dispose();
            }
        }
        public void LiveNow()
        {
            initialMaskSize();
            initialDragRange();
            if (maskData._dragDetectionRange!= maskData._maskSizeType)
            {
                coverMaskData();
            }    
            Thread DisplayLive = new Thread(Display);
            DisplayLive.Start();
        }
        private void Display()
        {
            
            
            while (CanIRun)
            {
                if(ISUpdata)
                {
                    CutImage(_preparationImage);
                    fitWindow(_preparationImage);
                    Mask();
                    if (MaskKide != Maskmod.None)
                    {
                        Stacked(_maskImage, _preparationImage, _dragDetectionRange);
                    }
                    ISUpdata = false;
                }
                

                pictureBox1.Image =Temporary(_preparationImage);
                Thread.Sleep(50);
                }
        }
        public Mat GetMaskImage()
        {
            if(MaskKide!=Maskmod.None)
            {
                Mat _outputMaskImage = new Mat();
                Cv2.Resize(_maskImage, _outputMaskImage, _templImage.Size());
                // CvInvoke.CvtColor(_outputMaskImage, _outputMaskImage, ColorConversion.Rgb2Gray);
                Rect Resize = ResizeImageCalculation(_outputMaskImage);
                //_outputMaskImage = new Mat(_outputMaskImage, Resize);
                Cv2.Resize(_outputMaskImage, _outputMaskImage, Resize.Size);
                // CvInvoke.Threshold(_outputMaskImage, _outputMaskImage, 0, 255, ThresholdType.Binary);
                return _outputMaskImage;
            }
            else
            {
                return null;
            } 
        }
        public Mat GetOriginalImage()
        {
            
            if (_templImage!=null)
            {
                Rect Resize = ResizeImageCalculation(_templImage);
                Mat OriginalImg = new Mat(_templImage, Resize);
                return OriginalImg;
            }
            else
            {
                return null;
            }

        }
        public object GetMaskData()
        {
            maskData.MaskKide = MaskKide;
            maskData._displayRoi= _displayRoi;
            maskData._dragDetectionRange= _dragDetectionRange;
            maskData._maskSizeType = _maskSizeType;

            return maskData;
        }
        public void SetMaskData(object inputmaskData)
        {
            
            if (inputmaskData.Equals(maskData))
            {
                maskData = inputmaskData as MaskData;
                coverMaskData();
                ISUpdata = true;
            }
            
        }

        private void coverMaskData()
        {
            MaskKide = maskData.MaskKide;
            _displayRoi = maskData._displayRoi;
            if (MaskKide != Maskmod.None)
            {
                if (MaskKide != Maskmod.Rectangle)
                {
                    if (MaskKide == Maskmod.Cross)
                    {
                        CrossButton.Checked = true;
                    }
                    else
                    {
                        CornerButton.Checked = true;
                    }
                    _dragDetectionRangeMode1 = maskData._dragDetectionRange;
                    _maskSizeType1 = maskData._maskSizeType;
                }
                else
                {
                    RectangleButton.Checked = true;
                    _dragDetectionRangeMode2 = maskData._dragDetectionRange;
                    _maskSizeType2 = maskData._maskSizeType;

                }
            }
            else
            {
                NoneButton.Checked = true;
            }
        }

        private System.Drawing.Bitmap Temporary(Mat matImage)
        {
            _temporaryImage[0]=matImage.ToBitmap();
            _temporaryImage[1] = _temporaryImage[0].Clone() as System.Drawing.Bitmap;
            if (_temporaryImage[2]!=null)
            {
                _temporaryImage[2].Dispose();
                _temporaryImage[0].Dispose();
            }
            _temporaryImage[2] = _temporaryImage[1];
            return _temporaryImage[1];
        }
        
        public void Close()
        {
            CanIRun = false;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (MaskKide != Maskmod.None)
            {
                _mouseMovePt = new Point(e.Location.X,e.Location.Y);

                if (_dragDetectionRange.Contains(e.Location.X,e.Location.Y))
                {
                    Cursor = Cursors.SizeNWSE;
                }
                else
                {
                    Cursor = Cursors.Default;
                }



                if (ISMouseDown && pictureBox1.ClientRectangle.Contains(e.Location) && !(e.X > pictureBox1.Width / 2 - 10 || e.Y > pictureBox1.Height / 2 - 10))
                {
                    _maskSizeType1.Width = e.X;
                    _maskSizeType1.Height = e.Y;
                    _dragDetectionRangeMode1.X = e.X - 10;
                    _dragDetectionRangeMode1.Y = e.Y - 10;
                    ISUpdata = true;
                    //GC.Collect();
                }
                if (IsMouseDownType2 && pictureBox1.ClientRectangle.Contains(e.Location) && !(e.X > pictureBox1.Width / 2 - 10 || e.Y > pictureBox1.Height / 2 - 10))
                {
                    int xy_;
                    xy_ = e.X >= e.Y ? (_center.X - _mouseMovePt.X) : (_center.Y - _mouseMovePt.Y);
                    _maskSizeType2.X = _center.X - xy_;
                    _maskSizeType2.Y = _center.Y - xy_;
                    _maskSizeType2.Width = xy_ * 2;
                    _maskSizeType2.Height = xy_ * 2;
                    _dragDetectionRangeMode2.X = _maskSizeType2.X - 10;
                    _dragDetectionRangeMode2.Y = _maskSizeType2.Y - 10;
                    ISUpdata = true;
                    //GC.Collect();
                }
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (_dragDetectionRange.Contains(e.Location.X,e.Location.Y) && e.Button == MouseButtons.Left && MaskKide != Maskmod.Rectangle)
            {
                ISMouseDown = true;
            }
            if (_dragDetectionRange.Contains(e.Location.X,e.Location.Y) && e.Button == MouseButtons.Left && MaskKide == Maskmod.Rectangle)
            {
                IsMouseDownType2 = true;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            ISMouseDown = false;
            IsMouseDownType2 = false;
        }

        private void NoneButton_CheckedChanged(object sender, EventArgs e)
        {
            MaskKide = Maskmod.None;
            ISUpdata = true;
        }

        private void CrossButton_CheckedChanged(object sender, EventArgs e)
        {
            MaskKide = Maskmod.Cross;
            ISUpdata = true;
        }

        private void CornerButton_CheckedChanged(object sender, EventArgs e)
        {
            MaskKide = Maskmod.Corner;
            ISUpdata = true;
        }

        private void RectangleButton_CheckedChanged(object sender, EventArgs e)
        {
            MaskKide = Maskmod.Rectangle;
            ISUpdata = true;
        }

        private void UpButton_Click(object sender, EventArgs e)
        {
            _displayRoi.Y += 1;
            ISUpdata = true;
        }

        private void DownButton_Click(object sender, EventArgs e)
        {
            _displayRoi.Y -= 1;
            ISUpdata = true;
        }

        private void LeftButton_Click(object sender, EventArgs e)
        {
            _displayRoi.X += 1;
            ISUpdata = true;
        }

        private void RightButton_Click(object sender, EventArgs e)
        {
            _displayRoi.X -= 1;
            ISUpdata = true;
        }

        private void TemplateMask_Load(object sender, EventArgs e)
        {
            NoneButton.Checked = true;
            if (_templImage == null)
            {
                MaskTypeBox.Enabled = false;
            }
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {

        }
    }

    public class MaskData
    {
        public TemplateMask.Maskmod MaskKide;
        public Rect _maskSizeType;
        public Rect _dragDetectionRange;
        public Rect _displayRoi;

    }









    
}
