////using Advantech.Motion;
//using OpenCvSharp;
//using MRLibrary;

////using Microsoft.VisualBasic.Logging;
//using Sentech.StApiDotNET;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Runtime.InteropServices;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Forms;
//using static System.Net.Mime.MediaTypeNames;
//using OpenCvSharp.Extensions;

//namespace NSAA_16Axis
//{
//    public partial class DialogFindCenter : Form
//    {
//        public Mat OriginalImage;
//        public Mat img;
//        public Mat mat1 = new Mat();
//        public Mat matFind = new Mat();
//        public double iMaskWaferRadio;
//        public OpenCvSharp.Rect rect;
//        public int centerX = 0;
//        public int centerY = 0;
//        public int err = 0;
//        public int iBlockSize = 0;
//        public int iAlgo = 0;
//        public int iBitwiseNot = 0;
//        public int ThresholdAlgorithm = 0;
//        public int Gaussian = 7;
//        public int Max = 255;
//        public int Threashold = 127;

//        public GrayImage imgM;
//        public GrayImage imgK;

//        private readonly string[] errstr = {
//            "成功!",
//            "左邊留白不夠",
//            "上邊留白不夠",
//            "右邊留白不夠",
//            "下邊留白不夠",
//            "找不到標記或是不夠寬",
//            "找不到標記或是不夠高"
//        };

//        public DialogFindCenter()
//        {
//            InitializeComponent();
//            //L.ReplaceControlText(this);
//            //btOK.Enabled = false;
//        }

//        private void TrackBarMax_ValueChanged(object sender, EventArgs e)
//        {
//            LblMax.Text = TrackBarMax.Value.ToString();
//            //btOK.Enabled = false;
//            DisplayImage();
//        }

//        private void TrackBarThreashold_ValueChanged(object sender, EventArgs e)
//        {
//            LblMin.Text = TrackBarThreashold1.Value.ToString();
//            //btOK.Enabled = false;
//            DisplayImage();
//        }

//        private void TrackBarGaussian_ValueChanged(object sender, EventArgs e)
//        {
//            LblGaussian.Text = TrackBarGaussian.Value.ToString();
//            //btOK.Enabled = false;
//            DisplayImage();
//        }

//        private void DisplayImage()
//        {
//            Mat mat;
//            Mat edges = new Mat();

//            if (CBFilter2D.SelectedIndex == 1)
//            {
//                // 銳化核矩陣 (Laplacian 形式)
//                int ksize = 11;
//                float[,] sharpenKernel = new float[,]
//                {
//                    { -1, -1, -1 },
//                    { -1, ksize, -1 },
//                   {-1, -1, -1}
//                };

//                // 創建卷積濾波器
//                //Mat kernel = new Mat(3, 3, DepthType.Cv8U, 1);
//                //// SetTo(kernel, sharpenKernel);
//                //SetKernelToMat(kernel, sharpenKernel);
//                //ConvolutionKernelF matrixKernel = new ConvolutionKernelF(sharpenKernel);
//                Mat kernel = CreateKernel(sharpenKernel);

//                //// 銳化圖像
//                Mat sharpenedImage = new Mat(OriginalImage.Size(), MatType.CV_8UC1);
//                Cv2.Filter2D(OriginalImage, sharpenedImage, MatType.CV_8UC1, kernel, new OpenCvSharp.Point(-1, -1));

//                // mat = sharpenedImage.Clone();

//                //int iksize = 11;
//                //int[,,] date = new[, ,]
//                //{
//                //    {
//                //        { -1 },
//                //        { -1 },
//                //        { -1 }
//                //    },
//                //    {
//                //        { -1 },
//                //        { iksize },
//                //        { -1 }
//                //    },
//                //    {
//                //        { -1 },
//                //        { -1 },
//                //        { -1 }
//                //    }
//                //};
//                //Mat tempMat = OriginalImage.Clone();
//                //Mat dstMat = new Mat(tempMat.Size, tempMat.Depth, tempMat.NumberOfChannels);
//                //Image<Gray, int> kernel = new Image<Gray, int>(date);
//                //CvInvoke.Filter2D(tempMat, dstMat, kernel, new System.Drawing.Point(-1, -1));

//                //mat = dstMat.Clone();

//                //Image<Gray, byte> image = OriginalImage.ToImage<Gray, byte>();
//                //for (int i = 0; i < image.Rows; i++)
//                //{
//                //    for (int j = 0; j < image.Cols; j++)
//                //    {
//                //        image.Data[i, j, 0] = (byte)Math.Log(1 + image.Data[i, j, 0]); 
//                //    }
//                //}

//                // mat = image.Mat;

//                int iksize = 11;

//                // 定義 2D 銳化內核
//                float[,] kernelValues = new float[,]
//                {
//                    { 0, -1, 0 },
//                    { -1, iksize, -1 },
//                    { 0, -1, 0 }
//                };

//                // 複製原始影像
//                Mat tempMat = OriginalImage.Clone();
//                Mat dstMat = new Mat(tempMat.Size(), tempMat.Type());
//                //Image<Gray, float> kernel = new Image<Gray, float>(kernelValues);

//                // 使用浮點數內核
//                //Image<Gray, float> kernel = new Image<Gray, float>(kernelValues.GetLength(0), kernelValues.GetLength(1));
//                //for (int i = 0; i < kernelValues.GetLength(0); i++)
//                //{
//                //    for (int j = 0; j < kernelValues.GetLength(1); j++)
//                //    {
//                //        kernel.Data[i, j, 0] = kernelValues[i, j];
//                //    }
//                //}
//                kernel = CreateKernel(kernelValues);
//                // 使用 Filter2D 濾波銳化
//                Cv2.Filter2D(tempMat, dstMat, MatType.CV_8UC1, kernel, new OpenCvSharp.Point(-1, -1));

//                Cv2.Normalize(dstMat, dstMat, 0, 255, NormTypes.MinMax);

//                // 複製結果影像
//                mat = dstMat.Clone();

//            }
//            else
//            {
//                mat = OriginalImage.Clone();
//            }

//            if (CBAlpahBata.SelectedIndex == 1)
//            {
//                double alpha = TBAlpha.Value / 100; // 對比控制
//                int beta = TBBata.Value;
//                Cv2.ConvertScaleAbs(mat, mat, alpha, beta);
//            }
//            else
//            {
//                //mat = OriginalImage.Clone();
//            }

//            if (CBEdge.SelectedIndex == 1)
//            {
//                // 使用 Canny 邊緣檢測
//                double threshold1 = trackBar1.Value;  // 第一個閾值
//                double threshold2 = trackBar2.Value;  // 第二個閾值
//                Cv2.Canny(mat, edges, threshold1, threshold2);
//                mat = edges.Clone();

//            }
//            else if (CBEdge.SelectedIndex == 2)
//            {
//                Mat sobelX = new Mat();
//                Mat sobelY = new Mat();
//                Mat sobelCombined = new Mat();

//                // 使用 Sobel 邊緣檢測，計算水平方向和垂直方向的梯度
//                Cv2.Sobel(mat, sobelX, MatType.CV_8UC1, 1, 0);  // X 方向
//                Cv2.Sobel(mat, sobelY, MatType.CV_8UC1, 0, 1);  // Y 方向

//                // 合併 X 和 Y 梯度
//                Cv2.AddWeighted(sobelX, 0.5, sobelY, 0.5, 0, sobelCombined);
//                mat = sobelCombined.Clone();
//            }
//            else if (CBEdge.SelectedIndex == 3)
//            {
//                double threshold1 = trackBar1.Value;  // 第一個閾值
//                double threshold2 = trackBar2.Value;  // 填入值
//                double threshold3 = trackBar3.Value;

//                Mat image = OriginalImage.Clone();

//                for (int i = 0; i < image.Rows; i++)
//                {
//                    for (int j = 0; j < image.Cols; j++)
//                    {
//                        byte pixel = image.At<byte>(i, j);
//                        if (pixel > threshold1)
//                        {
//                            image.Set<byte>(i, j, (byte)threshold2);
//                        }
//                        else if (pixel < threshold3)
//                        {
//                            image.Set<byte>(i, j, (byte)(pixel / 2));
//                        }
//                    }
//                }
//                //Image<Gray, byte> image = OriginalImage.ToImage<Gray, byte>();
//                //for (int i = 0; i < image.Rows; i++)
//                //{
//                //    for (int j = 0; j < image.Cols; j++)
//                //    {
//                //        if (image.Data[i, j, 0] > (int)threshold1)
//                //        {
//                //            image.Data[i, j, 0] = (byte)threshold2;
//                //        }
//                //        else if (image.Data[i, j, 0] < (int)threshold3)
//                //        {
//                //            image.Data[i, j, 0] /= 2;
//                //        }

//                //    }
//                //}

//                mat = image;
//            }

//            Gaussian = TrackBarGaussian.Value;
//            if (Gaussian % 2 == 0) Gaussian += 1;
//            if (Gaussian < 3) Gaussian = 3;
//            if (Gaussian > 65) Gaussian = 65;

//            Threashold = TrackBarThreashold1.Value;
//            Max = TrackBarMax.Value;
//            int blockSize = TBBlockSize.Value;

//            if (CBThresholdAlgorithm.SelectedIndex == 0)
//            {
//                Cv2.GaussianBlur(mat, mat, new OpenCvSharp.Size(Gaussian, Gaussian), 0, 0, BorderTypes.Default);
//                Cv2.Threshold(mat, mat, Threashold, Max, ThresholdTypes.Binary);
//            }
//            else if (CBThresholdAlgorithm.SelectedIndex == 1)
//            {
//                if (blockSize < 3) blockSize = 3;
//                Cv2.GaussianBlur(mat, mat, new OpenCvSharp.Size(Gaussian, Gaussian), 0, 0, BorderTypes.Default);
//                Cv2.AdaptiveThreshold(mat, mat, Max, AdaptiveThresholdTypes.MeanC, ThresholdTypes.Binary, blockSize, 2);
//            }
//            else if (CBThresholdAlgorithm.SelectedIndex == 2)
//            {
//                Cv2.GaussianBlur(mat, mat, new OpenCvSharp.Size(Gaussian, Gaussian), 0, 0, BorderTypes.Default);
//                Cv2.Threshold(mat, mat, Threashold, Max, ThresholdTypes.Otsu);
//            }
//            else if (CBThresholdAlgorithm.SelectedIndex == 3)
//            {
//                Cv2.GaussianBlur(mat, mat, new OpenCvSharp.Size(Gaussian, Gaussian), 0, 0, BorderTypes.Default);
//                int Threashold2 = TrackBarThreashold2.Value;
//                Cv2.Threshold(mat, mat, Threashold, Max, ThresholdTypes.Binary);
//                Cv2.Threshold(mat, mat, Threashold2, Max, ThresholdTypes.Binary);
//            }
//            else if (CBThresholdAlgorithm.SelectedIndex == 4)
//            {
//                //CvInvoke.GaussianBlur(mat, mat, new System.Drawing.Size(7, 7), 0);
//                //CvInvoke.AdaptiveThreshold(mat, mat, 255, Emgu.CV.CvEnum.AdaptiveThresholdType.MeanC, Emgu.CV.CvEnum.ThresholdType.Binary, 9, 4);
//            }

//            if (CBBitwiseNot.Checked)
//            {
//                Mat invertedImg = new Mat();
//                Cv2.BitwiseNot(mat, invertedImg);
//                mat = invertedImg.Clone();
//                // mat.Save("ftcv.png");
//                invertedImg.Dispose();
//            }

//            img = mat.Clone();
//            //GrayImage imgK = new GrayImage(img.Width, img.Height);
//            //imgK.Fill(255);
//            ProcessView.Image = mat.ToBitmap();
//            mat.Dispose();
//        }

//        private Mat CreateKernel(float[,] sharpenKernel)
//        {
//            throw new NotImplementedException();
//        }

//        private void DialogFindCenter_Load(object sender, EventArgs e)
//        {
//            LblMax.Text = TrackBarMax.Value.ToString();
//            LblMin.Text = TrackBarThreashold1.Value.ToString();
//            LblGaussian.Text = TrackBarGaussian.Value.ToString();
//            OriginalImage = img.Clone();
//            OriginalView.Image = OriginalImage.ToBitmap();
//            int iValue = (int)(iMaskWaferRadio * 10);
//            if (iValue < 5) iValue = 5;
//            if (iValue > 20) iValue = 20;
//            TrackBarRadio.Value = iValue;
//            LblRadio.Text = iMaskWaferRadio.ToString("F1");
//            CbCenterAlgorithm.SelectedIndex = 1;
//            CBAlpahBata.SelectedIndex = 0;
//            CBEdge.SelectedIndex = 0;
//            CBFilter2D.SelectedIndex = 0;
//            LbBlockSize.Text = TBBlockSize.Value.ToString();
//            if (iBitwiseNot == 0)
//            {
//                CBBitwiseNot.Checked = false;
//            }
//            else
//            {
//                CBBitwiseNot.Checked = true;
//            }
//            if (iAlgo == 3)
//            {
//                CBThresholdAlgorithm.SelectedIndex = 0;
//            }
//            else
//            {
//                CBThresholdAlgorithm.SelectedIndex = 1;
//            }
//            rect = new OpenCvSharp.Rect(0, 0, OriginalImage.Width, OriginalImage.Height);

//            DisplayImage();
//        }

//        public OpenCvSharp.Rect GetRectWithParam(Mat img, int high, int low, int Gaussian, bool Bitwise, double iMaskWafer, out int err)
//        {
//            OpenCvSharp.Rect Rect;
//            Mat mat = new Mat();
//            //Rect rectm = new Rect();
//            err = 0;

//            try
//            {
//                Cv2.ImWrite("ftc.png", img);
//                Cv2.GaussianBlur(img, mat, new OpenCvSharp.Size(Gaussian, Gaussian), 0, 0, BorderTypes.Default);
//                Cv2.Threshold(mat, mat, low, high, ThresholdTypes.Otsu);
//                if (Bitwise)
//                {
//                    Cv2.BitwiseNot(mat, mat);
//                    Cv2.ImWrite("ftcv.png", mat);
//                }

//                Cv2.FindContours(mat, out OpenCvSharp.Point[][] contours, out HierarchyIndex[] hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

//                if (contours.Length == 0)
//                {
//                    Rect = new OpenCvSharp.Rect(0, 0, img.Width, img.Height);
//                    err = 7;
//                    return Rect;
//                }

//                double areaMax = 0;
//                OpenCvSharp.Rect boundingBox = new OpenCvSharp.Rect();
//                OpenCvSharp.Rect rectm = new OpenCvSharp.Rect();
//                int centerX = 0, centerY = 0;

//                //for (int i = 0; i < count; i++)
//                //{
//                //    VectorOfPoint contour = contours[i];
//                //    double area = Cv2.ContourArea(contour);
//                //    rectm = Cv2.BoundingRectangle(contour);
//                //    Cv2.Rectangle(mat, rectm, new MCvScalar(255, 0, 0), 2);

//                //    if (area > areaMax)
//                //    {
//                //        Moments moments = Cv2.Moments(contour);
//                //        centerX = (int)(moments.M10 / moments.M00);
//                //        centerY = (int)(moments.M01 / moments.M00);
//                //        areaMax = area;
//                //        BoundingBox = rectm;
//                //    }
//                //}
//                foreach (var contour in contours)
//                {
//                    double area = Cv2.ContourArea(contour);
//                    OpenCvSharp.Rect rect = Cv2.BoundingRect(contour);
//                    Cv2.Rectangle(mat, rect, new Scalar(255, 0, 0), 2);
//                    if (area > areaMax)
//                    {
//                        areaMax = area;
//                        Moments moments = Cv2.Moments(contour);
//                        centerX = (int)(moments.M10 / moments.M00);
//                        centerY = (int)(moments.M01 / moments.M00);
//                        boundingBox = rect;
//                        rectm = rect;
//                    }
//                }

//                // mat.Save("img.png");

//                //if (!mat1.IsEmpty) mat1 = null;
//                mat1 = new Mat(img, boundingBox);
//                mat1.SaveImage("fct1.png");

//                Mat matm = new Mat(mat, rectm);
//                matm.SaveImage("ftcm.png");
//                matm.Dispose();

//                int width = (int)(mat1.Width * iMaskWafer);
//                int height = (int)(mat1.Height * iMaskWafer);
//                int x = (int)(centerX - width / 2);
//                int y = (int)(centerY - height / 2);
//                if ((x < 0) || ((x + width) > mat.Width))
//                {
//                    if (x < 0)
//                    {
//                        x = 0;
//                    }

//                    if ((x + width) > mat.Width)
//                    {
//                        width = mat.Width - x;
//                    }

//                    int minx = centerX - x;
//                    int x1 = (x + width) - centerX;
//                    if (x1 < minx) minx = x1;
//                    x = centerX - minx;
//                    width = minx * 2;
//                }

//                if ((y < 0) || ((y + height) > mat.Height))
//                {
//                    if (y < 0)
//                    {
//                        y = 0;
//                    }

//                    if ((y + height) > mat.Height)
//                    {
//                        height = mat.Height - y;
//                    }

//                    int miny = centerY - y;
//                    int y1 = (y + height) - centerY;
//                    if (y1 < miny) miny = y1;
//                    y = centerY - miny;
//                    height = miny * 2;
//                }

//                if (x < 0)
//                {
//                    Rect = new OpenCvSharp.Rect(0, 0, mat.Width, mat.Height);
//                    err = 1; // 左邊留白不夠
//                }
//                else if (y < 0)
//                {
//                    Rect = new OpenCvSharp.Rect(0, 0, mat.Width, mat.Height);
//                    err = 2; // 上邊留白不夠
//                }
//                else if ((x + width) > mat.Width)
//                {
//                    Rect = new OpenCvSharp.Rect(0, 0, mat.Width, mat.Height);
//                    err = 3; // 右邊留白不夠 
//                }
//                else if ((y + height) > mat.Height)
//                {
//                    Rect = new OpenCvSharp.Rect(0, 0, mat.Width, mat.Height);
//                    err = 4; // 下邊留白不夠 
//                }
//                else if (width < 64)
//                {
//                    Rect = new OpenCvSharp.Rect(0, 0, mat.Width, mat.Height);
//                    err = 5; // 不夠寬
//                }
//                else if (height < 64)
//                {
//                    Rect = new OpenCvSharp.Rect(0, 0, mat.Width, mat.Height);
//                    err = 6; // 不夠高 
//                }
//                else
//                {
//                    Rect = new OpenCvSharp.Rect(x, y, width, height);
//                }
//                Cv2.ImWrite("fmc.png", mat1);
//                mat1.Dispose();
//                mat.Dispose();
//            }
//            catch (Exception ex)
//            {
//                Rect = new OpenCvSharp.Rect(0, 0, img.Width, img.Height);
//                Console.WriteLine(ex.Message);
//            }
//            return Rect;
//        }

//        private void BtFind_Click(object sender, EventArgs e)
//        {
//            int err = 0;

//            Mat colorMat = new Mat();
//            Cv2.CvtColor(img, colorMat, ColorConversionCodes.GRAY2BGR);

//            Mat mat = colorMat.Clone();
//            Mat imgForContour = new Mat();
//            imgM = ProcessView.GetMask();
//            if (imgM == null)
//            {
//                Mat donCareMask = new Mat();
//                donCareMask = MRLibrary.ImageConvert.GrayImageToMat(imgM);
//                Cv2.BitwiseAnd(img, donCareMask, imgForContour);
//            }
//            //CvInvoke.FindContours(img, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);
//            Cv2.FindContours(imgForContour, out OpenCvSharp.Point[][] contours, out _, RetrievalModes.External, ContourApproximationModes.ApproxNone);
//            //int count = contours.Size;
//            if (contours.Length == 0)
//            {
//                MessageBox.Show("未檢測到任何輪廓！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                return;
//            }
//            OpenCvSharp.Rect boundingBox = new OpenCvSharp.Rect();
//            double areaMax = 0;

//            foreach (OpenCvSharp.Point[] contour in contours)
//            {
//                double contourArea = Cv2.ContourArea(contour);
//                rect = Cv2.BoundingRect(contour);
//                double area = rect.Width * rect.Height;
//                if (area > areaMax)
//                {
//                    Moments moments = Cv2.Moments(contour);
//                    if (CbCenterAlgorithm.SelectedIndex == 0)
//                    {
//                        if (moments.M00 != 0)
//                        {
//                            centerX = (int)(moments.M10 / moments.M00 + 0.5);
//                            centerY = (int)(moments.M01 / moments.M00 + 0.5);
//                        }
//                    }
//                    else
//                    {
//                        Point2f center;
//                        float radius;
//                        Cv2.MinEnclosingCircle(contour, out center, out radius);
//                        centerX = (int)(center.X + 0.5);
//                        centerY = (int)(center.Y + 0.5);
//                    }
//                    areaMax = area;
//                    boundingBox = rect;
//                }
//            }
//            Cv2.Rectangle(mat, boundingBox, Scalar.Blue, 2);
//            //for (int i = 0; i < count; i++)
//            //{
//            //    VectorOfPoint contour = contours[i];
//            //    double area = Cv2.ContourArea(contour);
//            //    rectm = Cv2.BoundingRectangle(contour);
//            //    Cv2.Rectangle(mat, rectm, new MCvScalar(255, 0, 0), 2);

//            //    if (area > areaMax)
//            //    {
//            //        Moments moments = CvInvoke.Moments(contour);

//            //        if (CbCenterAlgorithm.SelectedIndex == 0)
//            //        {
//            //            if (moments.M00 != 0)
//            //            {
//            //                if ((centerX != 0) || (centerY != 0))
//            //                {
//            //                    Cv2.Circle(mat, new System.Drawing.Point(centerX, centerY), 3, new MCvScalar(255, 0, 0), -1);
//            //                }
//            //                centerX = (int)(moments.M10 / moments.M00 + 0.5);
//            //                centerY = (int)(moments.M01 / moments.M00 + 0.5);
//            //                Cv2.Circle(mat, new System.Drawing.Point(centerX, centerY), 3, new MCvScalar(0, 255, 0), -1);
//            //            }
//            //        }
//            //        else
//            //        {
//            //            if ((centerX != 0) || (centerY != 0))
//            //            {
//            //                Cv2.Circle(mat, new System.Drawing.Point(centerX, centerY), 3, new MCvScalar(255, 0, 0), -1);
//            //            }

//            //            System.Drawing.Point[] pointsArray = contour.ToArray();
//            //            PointF[] pointsFArray = Array.ConvertAll(pointsArray, point => new PointF(point.X, point.Y));

//            //            // 計算最小外接圓
//            //            CircleF enclosingCircle = Cv2.MinEnclosingCircle(pointsFArray);

//            //            // 圓心和半徑
//            //            System.Drawing.Point center = new System.Drawing.Point((int)enclosingCircle.Center.X, (int)enclosingCircle.Center.Y);
//            //            PointF centerF = enclosingCircle.Center;
//            //            float radius = enclosingCircle.Radius;

//            //            centerX = center.X; centerY = center.Y;
//            //            CvInvoke.Circle(mat, new System.Drawing.Point(centerX, centerY), 3, new MCvScalar(0, 255, 0), -1);

//            //            //System.Drawing.Size winSize = new System.Drawing.Size(5, 5);
//            //            //System.Drawing.Size zeroZone = new System.Drawing.Size(-1, -1);

//            //            //MCvTermCriteria criteria = new MCvTermCriteria(30, 0.01); // 修正的停止條件

//            //            //Mat gray = new Mat();
//            //            //CvInvoke.CvtColor(img, gray, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);

//            //            // 使用子像素修正
//            //            //PointF[] subPixelCorners = new PointF[] { centerF };  // 初始點
//            //            // CvInvoke.CornerSubPix(gray, subPixelCorners, winSize, zeroZone, criteria);

//            //            // 獲取修正後的圓心
//            //            //PointF subPixelCenter = subPixelCorners[0];

//            //            //centerX = (int)(subPixelCenter.X + 0.5);
//            //            //centerY = (int)(subPixelCenter.Y + 0.5);
//            //            //CvInvoke.Circle(mat, new System.Drawing.Point(centerX, centerY), 3, new MCvScalar(0, 255, 0), -1);
//            //        }

//            //        areaMax = area;
//            //        BoundingBox = rectm;
//            //    }
//            //}

//            //if (!mat1.IsEmpty)
//            //{
//            //    mat1 = null;
//            //}
//            //mat1 = new Mat(img, BoundingBox);

//            //if (!matFind.IsEmpty)
//            //{
//            //    matFind = null;
//            //}
//            mat1?.Dispose();
//            mat1 = new Mat(img, boundingBox);
//            matFind?.Dispose();
//            matFind = mat.Clone();

//            int width = (int)(mat1.Width * iMaskWaferRadio);
//            int height = (int)(mat1.Height * iMaskWaferRadio);

//            int x = (int)(centerX - width / 2);
//            int y = (int)(centerY - height / 2);

//            if ((x < 0) || ((x + width) > OriginalImage.Width))
//            {
//                //if (x < 0)
//                //{
//                //    x = 0;
//                //}

//                //if ((x + width) > OriginalImage.Width)
//                //{
//                //    width = OriginalImage.Width - x;
//                //}

//                //int minx = centerX - x;
//                //int x1 = (x + width) - centerX;
//                //if (x1 < minx) minx = x1;
//                //x = centerX - minx;
//                //width = minx * 2;
//                x = Math.Max(0, Math.Min(centerX - Math.Min(centerX, OriginalImage.Width - centerX), OriginalImage.Width - width));
//                width = Math.Min(width, OriginalImage.Width - x);
//            }

//            if ((y < 0) || ((y + height) > OriginalImage.Height))
//            {
//                y = Math.Max(0, Math.Min(centerY - Math.Min(centerY, OriginalImage.Height - centerY), OriginalImage.Height - height));
//                height = Math.Min(height, OriginalImage.Height - y);
//            }

//            if (width < 64 || height < 64)
//            {
//                rect = new OpenCvSharp.Rect(0, 0, OriginalImage.Width, OriginalImage.Height);
//                err = (width < 64) ? 5 : 6;
//            }
//            else if (x < 0)
//            {
//                rect = new OpenCvSharp.Rect(0, 0, OriginalImage.Width, OriginalImage.Height);
//                err = 1; // 左邊留白不夠
//            }
//            else if (y < 0)
//            {
//                rect = new OpenCvSharp.Rect(0, 0, OriginalImage.Width, OriginalImage.Height);
//                err = 2; // 上邊留白不夠
//            }
//            else if ((x + width) > OriginalImage.Width)
//            {
//                rect = new OpenCvSharp.Rect(0, 0, OriginalImage.Width, OriginalImage.Height);
//                err = 3; // 右邊留白不夠 
//            }
//            else if ((y + height) > OriginalImage.Height)
//            {
//                rect = new OpenCvSharp.Rect(0, 0, OriginalImage.Width, OriginalImage.Height);
//                err = 4; // 下邊留白不夠 
//            }

//            else
//            {
//                btOK.Enabled = true;
//                rect = new OpenCvSharp.Rect(x, y, width, height);
//            }

//            if ((err > 0) && (err < 7))
//            {
//                string ErrorMessage = errstr[err];
//                MessageBox.Show(ErrorMessage, "自動找中心失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//            Cv2.Circle(mat, new OpenCvSharp.Point(centerX,centerY),3, new Scalar(0, 255, 0), -1);
//            Cv2.Rectangle(mat, new OpenCvSharp.Rect(rect.X,rect.Y,rect.Width,rect.Height), new Scalar(0, 0, 255), 2);
//            GrayImage imgK= new GrayImage(mat.Width, mat.Height);
//            imgK.Fill(255);
//            ProcessView.SetBackImage(mat, imgK, imgM);
//            //ProcessView.Image = mat.ToBitmap();
//        }

//        private void CBBitwiseNot_CheckedChanged(object sender, EventArgs e)
//        {
//            //btOK.Enabled = false;
//            if (CBBitwiseNot.Checked)
//            {
//                iBitwiseNot = 1;
//            }
//            else
//            {
//                iBitwiseNot = 0;
//            }
//            DisplayImage();
//        }

//        private void btOK_Click(object sender, EventArgs e)
//        {
//        }

//        private void TrackBarRadio_ValueChanged(object sender, EventArgs e)
//        {
//            //btOK.Enabled = false;
//            iMaskWaferRadio = (double)TrackBarRadio.Value / 10;
//            LblRadio.Text = iMaskWaferRadio.ToString("F1");
//            if (mat1 == null || mat1.Empty() || matFind == null || matFind.Empty())
//                return;

//            Mat tmat = null;
//            try
//            {
//                tmat = matFind.Clone();
//                int width=(int)(mat1.Width * iMaskWaferRadio);
//                int height=(int)(mat1.Height* iMaskWaferRadio);
//                int x = centerX - width / 2;
//                int y = centerY - height / 2;
//                OpenCvSharp.Rect rectm=new OpenCvSharp.Rect(x, y, width, height);
//                Cv2.Rectangle(tmat, rectm, new Scalar(0, 0, 255), 2);
//                Cv2.Circle(tmat, new OpenCvSharp.Point(centerX, centerY), 3, new Scalar(0, 255, 0), -1);
//                GrayImage imgK = new GrayImage(img.Width, img.Height);
//                imgK.Fill(255);
//                ProcessView.SetBackImage(tmat, imgK, imgM);
//                if (x < 0)
//                {
//                    err = 1;
//                }
//                else if (y < 0)
//                {
//                    err = 2;
//                }
//                else if ((x + width) > OriginalImage.Width)
//                {
//                    err = 3;
//                }
//                else if ((y + height) > OriginalImage.Height)
//                {
//                    err = 4;
//                }
//                else if (width < 64)
//                {
//                    err = 5;
//                }
//                else if (height < 64)
//                {
//                    err = 6;
//                }
//                else
//                {
//                    err = 0;
//                    rect = new OpenCvSharp.Rect(x, y, width, height);
//                    //btOK.Enabled = true;
//                }
//                rect=new OpenCvSharp.Rect(0,0,OriginalImage.Width, OriginalImage.Height);
//            }
//            finally
//            {
//                tmat?.Dispose();
//            }            
//        }

//        private void CbCenterAlgorithm_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            DisplayImage();
//        }

//        private void CBThresholdAlgorithm_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            if (CBThresholdAlgorithm.SelectedIndex == 4)
//            {
//                lbThreasholdDown.Visible = true;
//                lbDown.Visible = true;
//                TrackBarThreashold2.Visible = true;
//                LbBlockSize.Visible = false;
//                TBBlockSize.Visible = false;
//                lblBlockSize.Visible = false;
//            }
//            if (CBThresholdAlgorithm.SelectedIndex == 1)
//            {
//                lbThreasholdDown.Visible = false;
//                lbDown.Visible = false;
//                TrackBarThreashold2.Visible = false;
//                LbBlockSize.Visible = true;
//                TBBlockSize.Visible = true;
//                lblBlockSize.Visible = true;
//            }
//            else
//            {
//                lbThreasholdDown.Visible = false;
//                lbDown.Visible = false;
//                TrackBarThreashold2.Visible = false;
//                LbBlockSize.Visible = false;
//                TBBlockSize.Visible = false;
//                lblBlockSize.Visible = false;
//            }
//            ThresholdAlgorithm = CBThresholdAlgorithm.SelectedIndex;
//            DisplayImage();
//        }


//        private void TBAlpha_ValueChanged(object sender, EventArgs e)
//        {
//            DisplayImage();
//        }

//        private void CBAlpahBata_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            DisplayImage();
//        }

//        private void TBBata_ValueChanged(object sender, EventArgs e)
//        {
//            DisplayImage();
//        }

//        private void trackBar2_ValueChanged(object sender, EventArgs e)
//        {
//            DisplayImage();
//        }

//        private void trackBar1_ValueChanged(object sender, EventArgs e)
//        {
//            DisplayImage();
//        }

//        private void CBEdge_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            DisplayImage();
//        }

//        private void trackBar3_ValueChanged(object sender, EventArgs e)
//        {
//            DisplayImage();
//        }

//        //// SetTo 函數：用於將 2D 的浮點數陣列轉換為 Mat 的數據
//        //public void SetTo(Mat mat, float[,] data)
//        //{
//        //    int rows = data.GetLength(0);
//        //    int cols = data.GetLength(1);

//        //    // 將 2D 陣列展開為 1D 陣列
//        //    float[] flattenedArray = new float[rows * cols];
//        //    for (int i = 0; i < rows; i++)
//        //    {
//        //        for (int j = 0; j < cols; j++)
//        //        {
//        //            flattenedArray[i * cols + j] = data[i, j];
//        //        }
//        //    }

//        //    // 使用 GCHandle 銷定展開後的 1D 陣列
//        //    GCHandle gCHandle = GCHandle.Alloc(flattenedArray, GCHandleType.Pinned);
//        //    try
//        //    {
//        //        // 使用 Mat 的 SetTo 函數
//        //        mat.SetTo(gCHandle.AddrOfPinnedObject());
//        //    }
//        //    finally
//        //    {
//        //        gCHandle.Free();
//        //    }
//        //}

//        public void SetKernelToMat(Mat mat, float[,] data)
//        {
//            int rows = data.GetLength(0);
//            int cols = data.GetLength(1);

//            // 將 2D 陣列展開為 1D 陣列
//            float[] flattenedArray = new float[rows * cols];
//            for (int i = 0; i < rows; i++)
//            {
//                for (int j = 0; j < cols; j++)
//                {
//                    flattenedArray[i * cols + j] = data[i, j];
//                }
//            }

//            // 將 1D 陣列數據複製到 Mat 的資料區域
//            Marshal.Copy(flattenedArray, 0, mat.Data, flattenedArray.Length);
//        }

//        private void CBFilter2D_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            DisplayImage();
//        }

//        private void TBBlockSize_ValueChanged(object sender, EventArgs e)
//        {
//            if (TBBlockSize.Value % 2 == 0)
//            {
//                TBBlockSize.Value++;
//            }
//            else
//            {
//                LbBlockSize.Text = TBBlockSize.Value.ToString();
//                DisplayImage();
//            }
//        }
//    }
//}
//using Advantech.Motion;

using Euresys.Open_eVision;
using MRLibrary;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.Internal.Vectors;

//using Microsoft.VisualBasic.Logging;
using Sentech.StApiDotNET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.Net.Mime.MediaTypeNames;

namespace NSAA_16Axis
{
    public partial class DialogFindCenter : Form
    {
        public Mat OriginalImage;
        public Mat img;
        public Mat mat1 = new Mat();
        public Mat matFind = new Mat();
        public double iMaskWaferRadio;
        public OpenCvSharp.Rect rect;
        public int centerX = 0;
        public int centerY = 0;
        public int err = 0;
        public int iBlockSize = 0;
        public int iAlgo = 0;
        public int iBitwiseNot = 0;
        public int ThresholdAlgorithm = 0;
        public int Gaussian = 7;
        public int Max = 255;
        public int Threashold = 127;
        public GrayImage imgM;
        public GrayImage imgK;
        private List<CrossROI> selectedROIs = new List<CrossROI>();
        private bool isSelectingROI = false;
        private CrossROI currentROI = null;
        public bool IsMultiCrossMode { get; private set; } = false;
        public class CrossROI
        {
            public int Index { get; set; }
            public OpenCvSharp.Rect Region { get; set; }
            public string Name { get; set; }
            public OpenCvSharp.Point Center { get; set; }
            public bool isValid { get; set; }
        }

        private readonly string[] errstr = {
            "成功!",
            "左邊留白不夠",
            "上邊留白不夠",
            "右邊留白不夠",
            "下邊留白不夠",
            "找不到標記或是不夠寬",
            "找不到標記或是不夠高"
        };

        public DialogFindCenter()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            //L.ReplaceControlText(this);
            //btOK.Enabled = false;
        }

        private void TrackBarMax_ValueChanged(object sender, EventArgs e)
        {
            LblMax.Text = TrackBarMax.Value.ToString();
            //btOK.Enabled = false;
            DisplayImage();
        }

        private void TrackBarThreashold_ValueChanged(object sender, EventArgs e)
        {
            LblMin.Text = TrackBarThreashold1.Value.ToString();
            //btOK.Enabled = false;
            DisplayImage();
        }

        private void TrackBarGaussian_ValueChanged(object sender, EventArgs e)
        {
            LblGaussian.Text = TrackBarGaussian.Value.ToString();
            //btOK.Enabled = false;
            DisplayImage();
        }

        private void DisplayImage()
        {
            Mat mat = null;
            Mat edges = null;
            Mat kernel = null;
            Mat sharpenedImage = null;
            Mat tempMat = null;
            Mat dstMat = null;
            Mat sobelX = null;
            Mat sobelY = null;
            Mat sobelCombined = null;
            Mat invertedImg = null;

            try
            {
                if (CBFilter2D.SelectedIndex == 1)
                {
                    // 銳化核矩陣 (Laplacian 形式)
                    int ksize = 11;
                    float[,] sharpenKernel = new float[,]
                    {
                { -1, -1, -1 },
                { -1, ksize, -1 },
                { -1, -1, -1 }
                    };

                    //  使用 OpenCvSharp 建立 Mat 卷積核心
                    kernel = CreateKernelMat(sharpenKernel);

                    // 銳化圖像
                    sharpenedImage = new Mat();
                    Cv2.Filter2D(OriginalImage, sharpenedImage, MatType.CV_8UC1, kernel, new OpenCvSharp.Point(-1, -1));

                    int iksize = 11;
                    // 定義第二個銳化內核
                    float[,] kernelValues = new float[,]
                    {
                { 0, -1, 0 },
                { -1, iksize, -1 },
                { 0, -1, 0 }
                    };

                    // 複製原始影像
                    tempMat = OriginalImage.Clone();
                    dstMat = new Mat();

                    kernel?.Dispose();
                    kernel = CreateKernelMat(kernelValues);

                    // 使用 Filter2D 濾波銳化
                    Cv2.Filter2D(tempMat, dstMat, MatType.CV_8UC1, kernel, new OpenCvSharp.Point(-1, -1));
                    Cv2.Normalize(dstMat, dstMat, 0, 255, NormTypes.MinMax);

                    // 複製結果影像
                    mat = dstMat.Clone();
                }
                else
                {
                    mat = OriginalImage.Clone();
                }

                if (CBAlpahBata.SelectedIndex == 1)
                {
                    double alpha = TBAlpha.Value / 100.0; // 對比控制
                    int beta = TBBata.Value;
                    Cv2.ConvertScaleAbs(mat, mat, alpha, beta);
                }

                if (CBEdge.SelectedIndex == 1)
                {
                    // 使用 Canny 邊緣檢測
                    double threshold1 = trackBar1.Value;
                    double threshold2 = trackBar2.Value;
                    edges = new Mat();
                    Cv2.Canny(mat, edges, threshold1, threshold2);
                    mat?.Dispose();
                    mat = edges.Clone();
                }
                else if (CBEdge.SelectedIndex == 2)
                {
                    sobelX = new Mat();
                    sobelY = new Mat();
                    sobelCombined = new Mat();

                    // 使用 Sobel 邊緣檢測
                    Cv2.Sobel(mat, sobelX, MatType.CV_8UC1, 1, 0);
                    Cv2.Sobel(mat, sobelY, MatType.CV_8UC1, 0, 1);

                    // 合併 X 和 Y 梯度
                    Cv2.AddWeighted(sobelX, 0.5, sobelY, 0.5, 0, sobelCombined);
                    mat?.Dispose();
                    mat = sobelCombined.Clone();
                }
                else if (CBEdge.SelectedIndex == 3)
                {
                    double threshold1 = trackBar1.Value;
                    double threshold2 = trackBar2.Value;
                    double threshold3 = trackBar3.Value;

                    Mat image = OriginalImage.Clone();
                    var indexer = image.GetGenericIndexer<byte>();

                    for (int i = 0; i < image.Rows; i++)
                    {
                        for (int j = 0; j < image.Cols; j++)
                        {
                            byte pixel = indexer[i, j];
                            if (pixel > threshold1)
                            {
                                indexer[i, j] = (byte)threshold2;
                            }
                            else if (pixel < threshold3)
                            {
                                indexer[i, j] = (byte)(pixel / 2);
                            }
                        }
                    }

                    mat?.Dispose();
                    mat = image;
                }

                // 調整 Gaussian 參數
                Gaussian = TrackBarGaussian.Value;
                if (Gaussian % 2 == 0) Gaussian += 1;
                if (Gaussian < 3) Gaussian = 3;
                if (Gaussian > 65) Gaussian = 65;

                Threashold = TrackBarThreashold1.Value;
                Max = TrackBarMax.Value;
                int blockSize = TBBlockSize.Value;

                // 閾值處理
                if (CBThresholdAlgorithm.SelectedIndex == 0)
                {
                    Cv2.GaussianBlur(mat, mat, new OpenCvSharp.Size(Gaussian, Gaussian), 0, 0, BorderTypes.Default);
                    Cv2.Threshold(mat, mat, Threashold, Max, ThresholdTypes.Binary);
                }
                else if (CBThresholdAlgorithm.SelectedIndex == 1)
                {
                    if (blockSize < 3) blockSize = 3;
                    if (blockSize % 2 == 0) blockSize += 1; // 確保是奇數
                    Cv2.GaussianBlur(mat, mat, new OpenCvSharp.Size(Gaussian, Gaussian), 0, 0, BorderTypes.Default);
                    Cv2.AdaptiveThreshold(mat, mat, Max, AdaptiveThresholdTypes.MeanC, ThresholdTypes.Binary, blockSize, 2);
                }
                else if (CBThresholdAlgorithm.SelectedIndex == 2)
                {
                    Cv2.GaussianBlur(mat, mat, new OpenCvSharp.Size(Gaussian, Gaussian), 0, 0, BorderTypes.Default);
                    Cv2.Threshold(mat, mat, Threashold, Max, ThresholdTypes.Otsu);
                }
                else if (CBThresholdAlgorithm.SelectedIndex == 3)
                {
                    Cv2.GaussianBlur(mat, mat, new OpenCvSharp.Size(Gaussian, Gaussian), 0, 0, BorderTypes.Default);
                    int Threashold2 = TrackBarThreashold2.Value;
                    Cv2.Threshold(mat, mat, Threashold, Max, ThresholdTypes.Binary);
                    Cv2.Threshold(mat, mat, Threashold2, Max, ThresholdTypes.Binary);
                }
                else if (CBThresholdAlgorithm.SelectedIndex == 5)
                {
                    Cv2.GaussianBlur(mat, mat, new OpenCvSharp.Size(Gaussian, Gaussian), 0, 0, BorderTypes.Default);
                    Cv2.Threshold(mat, mat, Threashold, Max, ThresholdTypes.Binary);
                }
                else if (CBThresholdAlgorithm.SelectedIndex == 6)
                {
                    Cv2.GaussianBlur(mat, mat, new OpenCvSharp.Size(Gaussian, Gaussian), 0, 0, BorderTypes.Default);
                    int Threashold2 = TrackBarThreashold2.Value;
                    Cv2.Threshold(mat, mat, Threashold, Max, ThresholdTypes.Binary);
                    Cv2.Threshold(mat, mat, Threashold2, Max, ThresholdTypes.Binary);
                }

                // BitwiseNot 處理
                if (CBBitwiseNot.Checked)
                {
                    invertedImg = new Mat();
                    Cv2.BitwiseNot(mat, invertedImg);
                    mat?.Dispose();
                    mat = invertedImg.Clone();
                }

                // 更新顯示
                //img?.Dispose();
                img = mat.Clone();
                GrayImage imgK = new GrayImage(img.Width, img.Height);
                imgK.Fill(255);
                ProcessView.SetBackImage(img, imgK, imgM);
                mat.Dispose();
                //ProcessView.Image = img.ToBitmap();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"DisplayImage 錯誤: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // 清理資源
                edges?.Dispose();
                kernel?.Dispose();
                sharpenedImage?.Dispose();
                tempMat?.Dispose();
                dstMat?.Dispose();
                sobelX?.Dispose();
                sobelY?.Dispose();
                sobelCombined?.Dispose();
                invertedImg?.Dispose();
                mat?.Dispose();
            }
        }

        private void DialogFindCenter_Load(object sender, EventArgs e)
        {
            LblMax.Text = TrackBarMax.Value.ToString();
            LblMin.Text = TrackBarThreashold1.Value.ToString();
            LblGaussian.Text = TrackBarGaussian.Value.ToString();
            OriginalImage = img.Clone();
            OriginalView.Image = OriginalImage.ToBitmap();
            int iValue = (int)(iMaskWaferRadio * 10);
            if (iValue < 5) iValue = 5;
            if (iValue > 20) iValue = 20;
            TrackBarRadio.Value = iValue;
            LblRadio.Text = iMaskWaferRadio.ToString("F1");
            CbCenterAlgorithm.SelectedIndex = 1;
            CBAlpahBata.SelectedIndex = 0;
            CBEdge.SelectedIndex = 0;
            CBFilter2D.SelectedIndex = 0;
            LbBlockSize.Text = TBBlockSize.Value.ToString();
            if (iBitwiseNot == 0)
            {
                CBBitwiseNot.Checked = false;
            }
            else
            {
                CBBitwiseNot.Checked = true;
            }
            if (iAlgo == 3)
            {
                CBThresholdAlgorithm.SelectedIndex = 0;
            }
            else
            {
                CBThresholdAlgorithm.SelectedIndex = 1;
            }
            rect = new OpenCvSharp.Rect(0, 0, OriginalImage.Width, OriginalImage.Height);

            timer1.Enabled = true;
            //DisplayImage();
        }

        public Rectangle GetRectWithParam(Mat img, int high, int low, int Gaussian, bool Bitwise, double iMaskWafer, out int err)
        {
            Rectangle Rect;
            Mat mat = null;
            Mat invertedImg = null;
            Mat mat1Temp = null;
            Mat matmTemp = null;
            int centerX = 0, centerY = 0;
            err = 0;

            try
            {
                // 儲存原始影像
                Cv2.ImWrite("ftc.png", img);

                // 高斯模糊
                mat = new Mat();
                Cv2.GaussianBlur(img, mat, new OpenCvSharp.Size(Gaussian, Gaussian), 0, 0, BorderTypes.Default);

                // 閾值處理
                Cv2.Threshold(mat, mat, low, high, ThresholdTypes.Otsu);

                // BitwiseNot 處理
                if (Bitwise)
                {
                    invertedImg = new Mat();
                    Cv2.BitwiseNot(mat, invertedImg);
                    mat?.Dispose();
                    mat = invertedImg.Clone();
                    Cv2.ImWrite("ftcv.png", mat);
                }

                // 尋找輪廓
                Cv2.FindContours(mat, out OpenCvSharp.Point[][] contours, out HierarchyIndex[] hierarchy,
                    RetrievalModes.External, ContourApproximationModes.ApproxSimple);

                if (contours == null || contours.Length == 0)
                {
                    Rect = new Rectangle(0, 0, img.Width, img.Height);
                    err = 7;
                    return Rect;
                }

                // 找最大面積的輪廓
                double areaMax = 0;
                OpenCvSharp.Rect boundingBox = new OpenCvSharp.Rect();
                OpenCvSharp.Rect rectm = new OpenCvSharp.Rect();

                foreach (var contour in contours)
                {
                    double area = Cv2.ContourArea(contour);
                    OpenCvSharp.Rect rect = Cv2.BoundingRect(contour);
                    Cv2.Rectangle(mat, rect, new Scalar(255, 0, 0), 2);

                    if (area > areaMax)
                    {
                        areaMax = area;
                        Moments moments = Cv2.Moments(contour);
                        if (moments.M00 != 0)
                        {
                            centerX = (int)(moments.M10 / moments.M00);
                            centerY = (int)(moments.M01 / moments.M00);
                        }
                        boundingBox = rect;
                        rectm = rect;
                    }
                }

                // 檢查是否找到有效輪廓
                if (areaMax == 0 || boundingBox.Width == 0 || boundingBox.Height == 0)
                {
                    Rect = new Rectangle(0, 0, img.Width, img.Height);
                    err = 7;
                    return Rect;
                }

                // 建立子區域影像
                mat1?.Dispose();
                mat1 = new Mat(img, boundingBox);
                Cv2.ImWrite("fct1.png", mat1);

                matmTemp = new Mat(mat, rectm);
                Cv2.ImWrite("ftcm.png", matmTemp);
                matmTemp?.Dispose();

                // 計算目標矩形
                int width = (int)(mat1.Width * iMaskWafer);
                int height = (int)(mat1.Height * iMaskWafer);
                int x = (int)(centerX - width / 2);
                int y = (int)(centerY - height / 2);

                // X 方向邊界檢查與調整
                if ((x < 0) || ((x + width) > mat.Width))
                {
                    if (x < 0)
                    {
                        x = 0;
                    }

                    if ((x + width) > mat.Width)
                    {
                        width = mat.Width - x;
                    }

                    int minx = centerX - x;
                    int x1 = (x + width) - centerX;
                    if (x1 < minx) minx = x1;
                    x = centerX - minx;
                    width = minx * 2;
                }

                // Y 方向邊界檢查與調整
                if ((y < 0) || ((y + height) > mat.Height))
                {
                    if (y < 0)
                    {
                        y = 0;
                    }

                    if ((y + height) > mat.Height)
                    {
                        height = mat.Height - y;
                    }

                    int miny = centerY - y;
                    int y1 = (y + height) - centerY;
                    if (y1 < miny) miny = y1;
                    y = centerY - miny;
                    height = miny * 2;
                }

                // 驗證結果並設定錯誤碼
                if (x < 0)
                {
                    Rect = new Rectangle(0, 0, mat.Width, mat.Height);
                    err = 1; // 左邊留白不夠
                }
                else if (y < 0)
                {
                    Rect = new Rectangle(0, 0, mat.Width, mat.Height);
                    err = 2; // 上邊留白不夠
                }
                else if ((x + width) > mat.Width)
                {
                    Rect = new Rectangle(0, 0, mat.Width, mat.Height);
                    err = 3; // 右邊留白不夠 
                }
                else if ((y + height) > mat.Height)
                {
                    Rect = new Rectangle(0, 0, mat.Width, mat.Height);
                    err = 4; // 下邊留白不夠 
                }
                else if (width < 64)
                {
                    Rect = new Rectangle(0, 0, mat.Width, mat.Height);
                    err = 5; // 不夠寬
                }
                else if (height < 64)
                {
                    Rect = new Rectangle(0, 0, mat.Width, mat.Height);
                    err = 6; // 不夠高 
                }
                else
                {
                    Rect = new Rectangle(x, y, width, height);
                    err = 0; // 成功
                }

                // 儲存最終結果
                Cv2.ImWrite("fmc.png", mat1);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetRectWithParam 錯誤: {ex.Message}");
                Rect = new Rectangle(0, 0, img.Width, img.Height);
                err = 7;
            }
            finally
            {
                // 清理資源
                mat?.Dispose();
                invertedImg?.Dispose();
                matmTemp?.Dispose();
                // 注意: mat1 不要在這裡 Dispose，因為它是類別成員變數
            }

            return Rect;
        }

        private void BtFind_Click(object sender, EventArgs e)
        {
            Mat colorMat = null;
            Mat mat = null;
            Mat imgForContour = null;
            IsMultiCrossMode = false;
            try
            {
                // 轉換為彩色影像以便繪製
                colorMat = new Mat();
                Cv2.CvtColor(img, colorMat, ColorConversionCodes.GRAY2BGR);
                mat = colorMat.Clone();

                // 準備輪廓檢測影像
                //imgForContour = img.Clone();
                imgForContour = new Mat();
                imgM = ProcessView.GetMask();
                if (imgM != null)
                {
                    Mat dontCareMask = new Mat();
                    dontCareMask = MRLibrary.ImageConvert.GrayImageToMat(imgM);
                    Cv2.BitwiseAnd(img, dontCareMask, imgForContour);
                }
                // 尋找輪廓
                Cv2.FindContours(imgForContour, out OpenCvSharp.Point[][] contours, out HierarchyIndex[] hierarchy,
                    RetrievalModes.External, ContourApproximationModes.ApproxNone);

                if (contours == null || contours.Length == 0)
                {
                    MessageBox.Show("未檢測到任何輪廓！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 找最大面積的輪廓
                OpenCvSharp.Rect boundingBox = new OpenCvSharp.Rect();
                double areaMax = 0;

                foreach (OpenCvSharp.Point[] contour in contours)
                {
                    double contourArea = Cv2.ContourArea(contour);
                    OpenCvSharp.Rect currentRect = Cv2.BoundingRect(contour);
                    double area = currentRect.Width * currentRect.Height;

                    if (area > areaMax)
                    {
                        Moments moments = Cv2.Moments(contour);

                        if (CbCenterAlgorithm.SelectedIndex == 0)
                        {
                            // 使用質心計算中心
                            if (moments.M00 != 0)
                            {
                                centerX = (int)(moments.M10 / moments.M00 + 0.5);
                                centerY = (int)(moments.M01 / moments.M00 + 0.5);
                            }
                        }
                        else
                        {
                            // 使用最小外接圓計算中心
                            Point2f center;
                            float radius;
                            Cv2.MinEnclosingCircle(contour, out center, out radius);
                            centerX = (int)(center.X + 0.5);
                            centerY = (int)(center.Y + 0.5);
                        }

                        areaMax = area;
                        boundingBox = currentRect;
                    }
                }

                // 繪製邊界框
                Cv2.Rectangle(mat, boundingBox, new Scalar(255, 0, 0), 2);

                // 更新 mat1 和 matFind
                mat1?.Dispose();
                mat1 = new Mat(img, boundingBox);

                matFind?.Dispose();
                matFind = mat.Clone();

                // 計算目標矩形
                int width = (int)(mat1.Width * iMaskWaferRadio);
                int height = (int)(mat1.Height * iMaskWaferRadio);
                int x = (int)(centerX - width / 2);
                int y = (int)(centerY - height / 2);

                // X 方向邊界檢查與調整
                if ((x < 0) || ((x + width) > OriginalImage.Width))
                {
                    if (x < 0)
                    {
                        x = 0;
                    }

                    if ((x + width) > OriginalImage.Width)
                    {
                        width = OriginalImage.Width - x;
                    }

                    int minx = centerX - x;
                    int x1 = (x + width) - centerX;
                    if (x1 < minx) minx = x1;
                    x = centerX - minx;
                    width = minx * 2;
                }

                // Y 方向邊界檢查與調整
                if ((y < 0) || ((y + height) > OriginalImage.Height))
                {
                    if (y < 0)
                    {
                        y = 0;
                    }

                    if ((y + height) > OriginalImage.Height)
                    {
                        height = OriginalImage.Height - y;
                    }

                    int miny = centerY - y;
                    int y1 = (y + height) - centerY;
                    if (y1 < miny) miny = y1;
                    y = centerY - miny;
                    height = miny * 2;
                }

                // 驗證並設定錯誤碼
                if (x < 0)
                {
                    rect = new OpenCvSharp.Rect(0, 0, OriginalImage.Width, OriginalImage.Height);
                    err = 1; // 左邊留白不夠
                }
                else if (y < 0)
                {
                    rect = new OpenCvSharp.Rect(0, 0, OriginalImage.Width, OriginalImage.Height);
                    err = 2; // 上邊留白不夠
                }
                else if ((x + width) > OriginalImage.Width)
                {
                    rect = new OpenCvSharp.Rect(0, 0, OriginalImage.Width, OriginalImage.Height);
                    err = 3; // 右邊留白不夠 
                }
                else if ((y + height) > OriginalImage.Height)
                {
                    rect = new OpenCvSharp.Rect(0, 0, OriginalImage.Width, OriginalImage.Height);
                    err = 4; // 下邊留白不夠 
                }
                else if (width < 64)
                {
                    rect = new OpenCvSharp.Rect(0, 0, OriginalImage.Width, OriginalImage.Height);
                    err = 5; // 不夠寬
                }
                else if (height < 64)
                {
                    rect = new OpenCvSharp.Rect(0, 0, OriginalImage.Width, OriginalImage.Height);
                    err = 6; // 不夠高 
                }
                else
                {
                    btOK.Enabled = true;
                    rect = new OpenCvSharp.Rect(x, y, width, height);
                    err = 0; // 成功
                }

                // 顯示錯誤訊息
                if ((err > 0) && (err < 7))
                {
                    string ErrorMessage = errstr[err];
                    MessageBox.Show(ErrorMessage, "自動找中心失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // 繪製中心點和矩形
                Cv2.Circle(mat, new OpenCvSharp.Point(centerX, centerY), 3, new Scalar(0, 255, 0), -1);

                OpenCvSharp.Rect drawRect = new OpenCvSharp.Rect(rect.X, rect.Y, rect.Width, rect.Height);
                Cv2.Rectangle(mat, drawRect, new Scalar(0, 0, 255), 2);

                // 更新顯示
                //ProcessView.Image = mat.ToBitmap();
                GrayImage imgK = new GrayImage(mat.Width, mat.Height);
                imgK.Fill(255);
                ProcessView.SetBackImage(mat, imgK, imgM);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"BtFind_Click 錯誤: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // 清理資源
                colorMat?.Dispose();
                imgForContour?.Dispose();
                mat?.Dispose();
            }
        }

        private void CBBitwiseNot_CheckedChanged(object sender, EventArgs e)
        {
            //btOK.Enabled = false;
            if (CBBitwiseNot.Checked)
            {
                iBitwiseNot = 1;
            }
            else
            {
                iBitwiseNot = 0;
            }
            DisplayImage();
        }

        private void btOK_Click(object sender, EventArgs e)
        {
        }

        private void TrackBarRadio_ValueChanged(object sender, EventArgs e)
        {
            iMaskWaferRadio = (double)TrackBarRadio.Value / 10;
            LblRadio.Text = iMaskWaferRadio.ToString("F1");

            // 檢查必要的 Mat 是否有效
            if (mat1 == null || mat1.Empty() || matFind == null || matFind.Empty())
                return;

            Mat tmat = null;

            try
            {
                tmat = matFind.Clone();
                int width = (int)(mat1.Width * iMaskWaferRadio);
                int height = (int)(mat1.Height * iMaskWaferRadio);

                int x = (int)(centerX - width / 2);
                int y = (int)(centerY - height / 2);

                // OpenCvSharp 使用 OpenCvSharp.Rect
                OpenCvSharp.Rect rectm = new OpenCvSharp.Rect(x, y, width, height);

                // 使用 Cv2.Rectangle 替代 CvInvoke.Rectangle
                Cv2.Rectangle(tmat, rectm, new Scalar(0, 0, 255), 2);

                // 繪製中心點
                Cv2.Circle(tmat, new OpenCvSharp.Point(centerX, centerY), 3, new Scalar(0, 255, 0), -1);

                // 更新顯示
                //ProcessView.Image = tmat.ToBitmap();
                GrayImage imgK = new GrayImage(img.Width, img.Height);
                imgK.Fill(255);
                ProcessView.SetBackImage(tmat, imgK, imgM);

                // 驗證並設定錯誤碼
                if (x < 0)
                {
                    rect = new OpenCvSharp.Rect(0, 0, OriginalImage.Width, OriginalImage.Height);
                    err = 1; // 左邊留白不夠
                }
                else if (y < 0)
                {
                    rect = new OpenCvSharp.Rect(0, 0, OriginalImage.Width, OriginalImage.Height);
                    err = 2; // 上邊留白不夠
                }
                else if ((x + width) > OriginalImage.Width)
                {
                    rect = new OpenCvSharp.Rect(0, 0, OriginalImage.Width, OriginalImage.Height);
                    err = 3; // 右邊留白不夠 
                }
                else if ((y + height) > OriginalImage.Height)
                {
                    rect = new OpenCvSharp.Rect(0, 0, OriginalImage.Width, OriginalImage.Height);
                    err = 4; // 下邊留白不夠 
                }
                else if (width < 64)
                {
                    rect = new OpenCvSharp.Rect(0, 0, OriginalImage.Width, OriginalImage.Height);
                    err = 5; // 不夠寬
                }
                else if (height < 64)
                {
                    rect = new OpenCvSharp.Rect(0, 0, OriginalImage.Width, OriginalImage.Height);
                    err = 6; // 不夠高 
                }
                else
                {
                    err = 0; // 成功
                    rect = new OpenCvSharp.Rect(x, y, width, height);
                    //btOK.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"TrackBarRadio_ValueChanged 錯誤: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // 清理資源
                tmat?.Dispose();
            }
        }

        private void CbCenterAlgorithm_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayImage();
        }

        private void CBThresholdAlgorithm_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CBThresholdAlgorithm.SelectedIndex == 4)
            {
                lbThreasholdDown.Visible = true;
                lbDown.Visible = true;
                TrackBarThreashold2.Visible = true;
                LbBlockSize.Visible = false;
                TBBlockSize.Visible = false;
                lblBlockSize.Visible = false;
            }
            if (CBThresholdAlgorithm.SelectedIndex == 1)
            {
                lbThreasholdDown.Visible = false;
                lbDown.Visible = false;
                TrackBarThreashold2.Visible = false;
                LbBlockSize.Visible = true;
                TBBlockSize.Visible = true;
                lblBlockSize.Visible = true;
            }
            else
            {
                lbThreasholdDown.Visible = false;
                lbDown.Visible = false;
                TrackBarThreashold2.Visible = false;
                LbBlockSize.Visible = false;
                TBBlockSize.Visible = false;
                lblBlockSize.Visible = false;
            }
            ThresholdAlgorithm = CBThresholdAlgorithm.SelectedIndex;
            DisplayImage();
        }


        private void TBAlpha_ValueChanged(object sender, EventArgs e)
        {
            DisplayImage();
        }

        private void CBAlpahBata_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayImage();
        }

        private void TBBata_ValueChanged(object sender, EventArgs e)
        {
            DisplayImage();
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            DisplayImage();
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            DisplayImage();
        }

        private void CBEdge_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayImage();
        }

        private void trackBar3_ValueChanged(object sender, EventArgs e)
        {
            DisplayImage();
        }

        //// SetTo 函數：用於將 2D 的浮點數陣列轉換為 Mat 的數據
        //public void SetTo(Mat mat, float[,] data)
        //{
        //    int rows = data.GetLength(0);
        //    int cols = data.GetLength(1);

        //    // 將 2D 陣列展開為 1D 陣列
        //    float[] flattenedArray = new float[rows * cols];
        //    for (int i = 0; i < rows; i++)
        //    {
        //        for (int j = 0; j < cols; j++)
        //        {
        //            flattenedArray[i * cols + j] = data[i, j];
        //        }
        //    }

        //    // 使用 GCHandle 銷定展開後的 1D 陣列
        //    GCHandle gCHandle = GCHandle.Alloc(flattenedArray, GCHandleType.Pinned);
        //    try
        //    {
        //        // 使用 Mat 的 SetTo 函數
        //        mat.SetTo(gCHandle.AddrOfPinnedObject());
        //    }
        //    finally
        //    {
        //        gCHandle.Free();
        //    }
        //}

        public void SetKernelToMat(Mat mat, float[,] data)
        {
            int rows = data.GetLength(0);
            int cols = data.GetLength(1);

            // 將 2D 陣列展開為 1D 陣列
            float[] flattenedArray = new float[rows * cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    flattenedArray[i * cols + j] = data[i, j];
                }
            }

            // 將 1D 陣列數據複製到 Mat 的資料區域
            //Marshal.Copy(flattenedArray, 0, mat.DataPointer, flattenedArray.Length);
        }

        private void CBFilter2D_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayImage();
        }

        private void TBBlockSize_ValueChanged(object sender, EventArgs e)
        {
            if (TBBlockSize.Value % 2 == 0)
            {
                TBBlockSize.Value++;
            }
            else
            {
                LbBlockSize.Text = TBBlockSize.Value.ToString();
                DisplayImage();
            }
        }
        private Mat CreateKernelMat(float[,] kernelData)
        {
            int rows = kernelData.GetLength(0);
            int cols = kernelData.GetLength(1);

            // 建立 CV_32FC1 類型的 Mat
            Mat kernel = new Mat(rows, cols, MatType.CV_32FC1);

            // 使用 Indexer 填入資料
            var indexer = kernel.GetGenericIndexer<float>();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    indexer[i, j] = kernelData[i, j];
                }
            }

            return kernel;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            DisplayImage();
        }

        private void udPenSize_ValueChanged(object sender, EventArgs e)
        {
            ProcessView.PenSize = (int)udPenSize.Value;
        }

        private void rbMask_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMask.Checked)
            {
                ProcessView.SetPaintLayer(0);
                ProcessView.Tool = (UCPaintMask.EnumTool)cbControl.SelectedIndex;
            }
            else
            {

            }
        }

        private void cbControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rbMask.Checked)
                ProcessView.Tool = (UCPaintMask.EnumTool)cbControl.SelectedIndex;
        }

        //private void btSelectedCenter_Click(object sender, EventArgs e)
        //{
        //    Mat colorMat = null;
        //    Mat mat = null;
        //    Mat imgForContour = null;

        //    try
        //    {
        //        // 轉換為彩色影像以便繪製
        //        colorMat = new Mat();
        //        Cv2.CvtColor(img, colorMat, ColorConversionCodes.GRAY2BGR);
        //        mat = colorMat.Clone();

        //        // 準備輪廓檢測影像
        //        //imgForContour = img.Clone();
        //        imgForContour = new Mat();
        //        imgM = ProcessView.GetMask();
        //        if (imgM != null)
        //        {
        //            Mat dontCareMask = new Mat();
        //            dontCareMask = MRLibrary.ImageConvert.GrayImageToMat(imgM);
        //            Cv2.BitwiseAnd(img, dontCareMask, imgForContour);
        //        }
        //        // 尋找輪廓
        //        Cv2.FindContours(imgForContour, out OpenCvSharp.Point[][] contours, out HierarchyIndex[] hierarchy,
        //            RetrievalModes.External, ContourApproximationModes.ApproxNone);

        //        if (contours == null || contours.Length == 0)
        //        {
        //            MessageBox.Show("未檢測到任何輪廓！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            return;
        //        }

        //        // 找最大面積的輪廓
        //        OpenCvSharp.Rect boundingBox = new OpenCvSharp.Rect();
        //        double areaMax = 0;

        //        foreach (OpenCvSharp.Point[] contour in contours)
        //        {
        //            double contourArea = Cv2.ContourArea(contour);
        //            OpenCvSharp.Rect currentRect = Cv2.BoundingRect(contour);
        //            double area = currentRect.Width * currentRect.Height;

        //            if (area > areaMax)
        //            {
        //                Moments moments = Cv2.Moments(contour);

        //                if (CbCenterAlgorithm.SelectedIndex == 0)
        //                {
        //                    // 使用質心計算中心
        //                    if (moments.M00 != 0)
        //                    {
        //                        centerX = (int)(moments.M10 / moments.M00 + 0.5);
        //                        centerY = (int)(moments.M01 / moments.M00 + 0.5);
        //                    }
        //                }
        //                else
        //                {
        //                    // 使用最小外接圓計算中心
        //                    Point2f center;
        //                    float radius;
        //                    Cv2.MinEnclosingCircle(contour, out center, out radius);
        //                    centerX = (int)(center.X + 0.5);
        //                    centerY = (int)(center.Y + 0.5);
        //                }

        //                areaMax = area;
        //                boundingBox = currentRect;
        //            }
        //        }

        //        // 繪製邊界框
        //        Cv2.Rectangle(mat, boundingBox, new Scalar(255, 0, 0), 2);

        //        // 更新 mat1 和 matFind
        //        mat1?.Dispose();
        //        mat1 = new Mat(img, boundingBox);

        //        matFind?.Dispose();
        //        matFind = mat.Clone();

        //        // 計算目標矩形
        //        int width = (int)(mat1.Width * iMaskWaferRadio);
        //        int height = (int)(mat1.Height * iMaskWaferRadio);
        //        int x = (int)(centerX - width / 2);
        //        int y = (int)(centerY - height / 2);

        //        // X 方向邊界檢查與調整
        //        if ((x < 0) || ((x + width) > OriginalImage.Width))
        //        {
        //            if (x < 0)
        //            {
        //                x = 0;
        //            }

        //            if ((x + width) > OriginalImage.Width)
        //            {
        //                width = OriginalImage.Width - x;
        //            }

        //            int minx = centerX - x;
        //            int x1 = (x + width) - centerX;
        //            if (x1 < minx) minx = x1;
        //            x = centerX - minx;
        //            width = minx * 2;
        //        }

        //        // Y 方向邊界檢查與調整
        //        if ((y < 0) || ((y + height) > OriginalImage.Height))
        //        {
        //            if (y < 0)
        //            {
        //                y = 0;
        //            }

        //            if ((y + height) > OriginalImage.Height)
        //            {
        //                height = OriginalImage.Height - y;
        //            }

        //            int miny = centerY - y;
        //            int y1 = (y + height) - centerY;
        //            if (y1 < miny) miny = y1;
        //            y = centerY - miny;
        //            height = miny * 2;
        //        }

        //        // 驗證並設定錯誤碼
        //        if (x < 0)
        //        {
        //            rect = new OpenCvSharp.Rect(0, 0, OriginalImage.Width, OriginalImage.Height);
        //            err = 1; // 左邊留白不夠
        //        }
        //        else if (y < 0)
        //        {
        //            rect = new OpenCvSharp.Rect(0, 0, OriginalImage.Width, OriginalImage.Height);
        //            err = 2; // 上邊留白不夠
        //        }
        //        else if ((x + width) > OriginalImage.Width)
        //        {
        //            rect = new OpenCvSharp.Rect(0, 0, OriginalImage.Width, OriginalImage.Height);
        //            err = 3; // 右邊留白不夠 
        //        }
        //        else if ((y + height) > OriginalImage.Height)
        //        {
        //            rect = new OpenCvSharp.Rect(0, 0, OriginalImage.Width, OriginalImage.Height);
        //            err = 4; // 下邊留白不夠 
        //        }
        //        else if (width < 64)
        //        {
        //            rect = new OpenCvSharp.Rect(0, 0, OriginalImage.Width, OriginalImage.Height);
        //            err = 5; // 不夠寬
        //        }
        //        else if (height < 64)
        //        {
        //            rect = new OpenCvSharp.Rect(0, 0, OriginalImage.Width, OriginalImage.Height);
        //            err = 6; // 不夠高 
        //        }
        //        else
        //        {
        //            btOK.Enabled = true;
        //            rect = new OpenCvSharp.Rect(x, y, width, height);
        //            err = 0; // 成功
        //        }

        //        // 顯示錯誤訊息
        //        if ((err > 0) && (err < 7))
        //        {
        //            string ErrorMessage = errstr[err];
        //            MessageBox.Show(ErrorMessage, "自動找中心失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }

        //        // 繪製中心點和矩形
        //        Cv2.Circle(mat, new OpenCvSharp.Point(centerX, centerY), 3, new Scalar(0, 255, 0), -1);

        //        OpenCvSharp.Rect drawRect = new OpenCvSharp.Rect(rect.X, rect.Y, rect.Width, rect.Height);
        //        Cv2.Rectangle(mat, drawRect, new Scalar(0, 0, 255), 2);

        //        // 更新顯示
        //        //ProcessView.Image = mat.ToBitmap();
        //        GrayImage imgK = new GrayImage(mat.Width, mat.Height);
        //        imgK.Fill(255);
        //        ProcessView.SetBackImage(mat, imgK, imgM);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"BtFind_Click 錯誤: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    finally
        //    {
        //        // 清理資源
        //        colorMat?.Dispose();
        //        imgForContour?.Dispose();
        //        mat?.Dispose();
        //    }
        //}
        private void btSelectedCenter_Click(object sender, EventArgs e)
        {
            Mat colorMat = null;
            Mat mat = null;
            Mat imgForContour = null;
            IsMultiCrossMode = true;

            try
            {
                //  轉換為彩色影像以便繪製
                colorMat = new Mat();
                Cv2.CvtColor(img, colorMat, ColorConversionCodes.GRAY2BGR);
                mat = colorMat.Clone();

                //  準備輪廓檢測影像(使用遮罩屏蔽不需要的區域)
                imgForContour = new Mat();
                imgM = ProcessView.GetMask();
                if (imgM != null)
                {
                    Mat dontCareMask = MRLibrary.ImageConvert.GrayImageToMat(imgM);
                    Cv2.BitwiseAnd(img, dontCareMask, imgForContour);
                }
                else
                {
                    imgForContour = img.Clone();
                }

                //  尋找輪廓
                Cv2.FindContours(imgForContour, out OpenCvSharp.Point[][] contours, out HierarchyIndex[] hierarchy,
                    RetrievalModes.External, ContourApproximationModes.ApproxNone);

                if (contours == null || contours.Length == 0)
                {
                    MessageBox.Show("未檢測到任何輪廓！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                //  找最大面積的輪廓
                OpenCvSharp.Rect boundingBox = new OpenCvSharp.Rect();
                double areaMax = 0;

                foreach (OpenCvSharp.Point[] contour in contours)
                {
                    double contourArea = Cv2.ContourArea(contour);
                    OpenCvSharp.Rect currentRect = Cv2.BoundingRect(contour);
                    double area = currentRect.Width * currentRect.Height;

                    if (area > areaMax)
                    {
                        Moments moments = Cv2.Moments(contour);

                        if (CbCenterAlgorithm.SelectedIndex == 0)
                        {
                            if (moments.M00 != 0)
                            {
                                centerX = (int)(moments.M10 / moments.M00 + 0.5);
                                centerY = (int)(moments.M01 / moments.M00 + 0.5);
                            }
                        }
                        else
                        {
                            Point2f center;
                            float radius;
                            Cv2.MinEnclosingCircle(contour, out center, out radius);
                            centerX = (int)(center.X + 0.5);
                            centerY = (int)(center.Y + 0.5);
                        }

                        areaMax = area;
                        boundingBox = currentRect;
                    }
                }

                //  關鍵修改:保留原始完整影像作為 template
                mat1?.Dispose();
                mat1 = img.Clone();  // ← 完整的原始影像

                matFind?.Dispose();
                matFind = mat.Clone();

                //  計算偏移量 (相對於原始影像中心)
                int imgCenterX = img.Width / 2;
                int imgCenterY = img.Height / 2;

                //  rect.Width 和 rect.Height 儲存的是偏移量
                int offsetX = centerX - imgCenterX;
                int offsetY = centerY - imgCenterY;

                //  儲存偏移量到 rect (使用 Width 和 Height 欄位)
                rect = new OpenCvSharp.Rect(centerX, centerY, offsetX, offsetY);

                btOK.Enabled = true;
                err = 0;

                //  繪製找到的中心點
                Cv2.Circle(mat, new OpenCvSharp.Point(centerX, centerY), 5, new Scalar(0, 255, 0), -1);
                Cv2.Line(mat, new OpenCvSharp.Point(centerX - 20, centerY),
                         new OpenCvSharp.Point(centerX + 20, centerY), new Scalar(0, 255, 0), 2);
                Cv2.Line(mat, new OpenCvSharp.Point(centerX, centerY - 20),
                         new OpenCvSharp.Point(centerX, centerY + 20), new Scalar(0, 255, 0), 2);
                Cv2.PutText(mat, $"Center: ({centerX}, {centerY})",
                            new OpenCvSharp.Point(centerX + 10, centerY - 10),
                            HersheyFonts.HersheySimplex, 0.5, new Scalar(0, 255, 0), 2);

                //  更新顯示(顯示原始 template 加上中心點標記)
                GrayImage imgK = new GrayImage(mat.Width, mat.Height);
                imgK.Fill(255);
                ProcessView.SetBackImage(mat, imgK, imgM);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"btSelectedCenter_Click 錯誤: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                colorMat?.Dispose();
                imgForContour?.Dispose();
                mat?.Dispose();
            }
        }

        private void btCancelMask_Click(object sender, EventArgs e)
        {
            try
            {
                // 清除遮罩:建立全白遮罩(255 = 不遮罩)
                if (img != null && !img.Empty())
                {
                    imgM = new GrayImage(img.Width, img.Height);
                    imgM.Fill(255); // 全白 = 完全不遮罩

                    //  判斷是否已經找到中心點
                    if (matFind != null && !matFind.Empty())
                    {
                        // 如果已找到中心,重新顯示找到的結果(保留中心標記)
                        GrayImage imgK = new GrayImage(matFind.Width, matFind.Height);
                        imgK.Fill(255);
                        ProcessView.SetBackImage(matFind, imgK, imgM);
                    }
                    else
                    {
                        // 如果尚未找中心,顯示處理後的影像
                        GrayImage imgK = new GrayImage(img.Width, img.Height);
                        imgK.Fill(255);
                        ProcessView.SetBackImage(img, imgK, imgM);
                    }

                    MessageBox.Show("已清除所有遮罩\n中心點偏差資料已保留", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"清除遮罩錯誤: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}