using OpenCvSharp;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MRLibrary
{
    public class FindTargetCenterCG
    {
        public FindTargetCenterCG()
        {
        }

        public OpenCvSharp.Rect2f AutoCenterCG(Mat img, OpenCvSharp.Rect2f roi)
        {
            //RectangleF rectangleF;
            if (img != null)
            {
                img = CvHelper.BitwiseNot(img);
                img.SaveImage("ftc.png");
                for (int i = 0; i < 3; i++)
                {
                    Mat mat = new Mat();
                    Size size = new Size((int)roi.Width, (int)roi.Height);
                    Point2f pointF = new Point2f((float)(roi.Left + roi.Width / 2f), (float)(roi.Top + roi.Height / 2f));
                    Cv2.GetRectSubPix(img, size, pointF, mat);
                    mat.SaveImage("ftc" + i.ToString() + ".png");
                    Point2f center = FindCenter(mat);
                    roi = new OpenCvSharp.Rect2f(center.X - roi.Width / 2f, center.Y - roi.Height / 2f, roi.Width, roi.Height);
                }

            }

            return roi;
        }

        public OpenCvSharp.Rect AutoCenterCG(Mat img, OpenCvSharp.Rect roi)
        {

            if (img != null)
            {
                img.SaveImage("ftc.png");
                //Mat mat = new Mat();
                //Size size = new Size((int)roi.Width, (int)roi.Height);
                //PointF pointF = new PointF((float)(roi.Left + roi.Width / 2f), (float)(roi.Top + roi.Height / 2f));
                //CvInvoke.GetRectSubPix(img, size, pointF, mat, DepthType.Default);
                //mat.Save("ftc1.png");
                //rectangle = GetRect(mat);
                //rectangle = GetRect(img);
                return GetRect(img);
            }

            return roi;
        }

        public OpenCvSharp.Rect AutoCenterCGWafer(Mat img, OpenCvSharp.Rect roi)
        {

            if (img != null)
            {
                img.SaveImage("ftc.png");
                return GetRectWafer(img);
            }

            return roi;
        }

        private OpenCvSharp.Rect GetRectWafer(Mat img)
        {
            //Rectangle Rect;
            //VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            try
            {
                Mat mat = new Mat();
                Cv2.GaussianBlur(img, mat, new Size(7, 7), 0);
                Cv2.Threshold(mat, mat, 127, 255, ThresholdTypes.Otsu);
                Point2f cG = GetCG(mat);
                Cv2.FindContours(mat, out Point[][] contours, out _, RetrievalModes.External, ContourApproximationModes.ApproxSimple);
                //int count = contours.Size;
                OpenCvSharp.Rect BoundingBox = Cv2.BoundingRect(contours[0]);
                Mat mat1 = new Mat(mat, BoundingBox);
                mat1.SaveImage("ftc1.png");
                int width = (int)(mat1.Width * 0.6);
                int height = (int)(mat1.Height * 0.6);
                int x = (int)(cG.X - width / 2);
                int y = (int)(cG.Y - height / 2);
                mat1.Dispose();
                mat.Dispose();

                if (x < 0 || y < 0 || x + width > mat.Width || y + height > mat.Height)
                    return new OpenCvSharp.Rect(0, 0, mat.Width, mat.Height);

                return new OpenCvSharp.Rect(x, y, width, height);
                //mat1.Save("fmc.png");
            }
            catch
            {
                return new OpenCvSharp.Rect(0, 0, img.Width, img.Height);
            }

        }

        private OpenCvSharp.Rect GetRect(Mat img)
        {
            //Rectangle Rect;
            //VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            try
            {
                Mat mat = new Mat();
                Cv2.GaussianBlur(img, mat, new Size(7, 7), 0);
                Cv2.Threshold(mat, mat, 127, 255, ThresholdTypes.Otsu);
                Point2f cG = GetCG(mat);
                Cv2.FindContours(mat, out Point[][] contours, out _, RetrievalModes.External, ContourApproximationModes.ApproxSimple);
                //int count = contours.Size;
                if (contours.Length == 0) return new OpenCvSharp.Rect(0, 0, img.Width, img.Height);
                OpenCvSharp.Rect BoundingBox = Cv2.BoundingRect(contours[0]);
                Mat mat1 = new Mat(mat, BoundingBox);
                mat1.SaveImage("ftc1.png");
                int width = (int)(mat1.Width * 1.2);
                int height = (int)(mat1.Height * 1.2);
                int x = (int)(cG.X - width / 2);
                int y = (int)(cG.Y - height / 2);

                if (x < 0 || y < 0 || x + width > mat.Width || y + height > mat.Height)
                    return new OpenCvSharp.Rect(0, 0, mat.Width, mat.Height);

                return new OpenCvSharp.Rect(x, y, width, height);
                //mat1.Save("fmc.png");
            }
            catch
            {
               return new OpenCvSharp.Rect(0,0, img.Width, img.Height);
            }
            
        }

        public Point2f FindCenter(Mat img)
        {
            //PointF pointF;
            try
            {
                Mat mat = new Mat();
                Cv2.GaussianBlur(img, mat, new Size(7, 7), 0);
                Cv2.Threshold(mat,mat,127,225,ThresholdTypes.Otsu);
                Point2f cG=GetCG(mat);
                mat.Dispose();
                return cG;
            }
            catch
            {
                return new Point2f(img.Width / 2f, img.Height / 2f);
            }
            
        }

        //public PointF FindCenter(Mat img)
        //{
        //    PointF pointF;
        //    VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
        //    try
        //    {
        //        Mat mat = new Mat();
        //        CvInvoke.GaussianBlur(img, mat, new Size(7, 7), 0, 0, BorderType.Default);
        //        CvInvoke.Threshold(mat, mat, 127, 255, Emgu.CV.CvEnum.ThresholdType.Otsu);
        //        PointF cG = GetCG(mat);
        //        CvInvoke.FindContours(mat, contours, null, RetrType.External, ChainApproxMethod.ChainApproxSimple);
        //        int count = contours.Size;
        //        Rectangle BoundingBox = CvInvoke.BoundingRectangle(contours[0]);
        //        Mat mat1 = new Mat(mat, BoundingBox);
        //        // mat1.Width
        //        mat1.Save("fmc.png");
        //        pointF = cG;
        //    }
        //    catch
        //    {
        //        pointF = new PointF((float)img.Width / 2f, (float)img.Height / 2f);
        //    }
        //    return pointF;
        //}

        private Point2f GetCG(Mat img)
        {
            Moments mCvMoment = Cv2.Moments(img, false);
            return new Point2f((float)mCvMoment.M10 / (float)mCvMoment.M00, (float)mCvMoment.M01 / (float)mCvMoment.M00);
        }

        private float XCenter(GrayImage img)
        {
            float[] singleArray = img.Sobel().HorizontalProjection();
            double num = 0;
            double num1 = 0;
            for (int i = 0; i < (int)singleArray.Length; i++)
            {
                num += (double)((float)(i + 1) * singleArray[i]);
                num1 += (double)singleArray[i];
            }
            return (float)((int)(num / num1 - 1));
        }

        private float YCenter(GrayImage img)
        {
            float[] singleArray = img.Sobel().VerticalProjection();
            double num = 0;
            double num1 = 0;
            for (int i = 0; i < (int)singleArray.Length; i++)
            {
                num += (double)((float)(i + 1) * singleArray[i]);
                num1 += (double)singleArray[i];
            }
            return (float)((int)(num / num1 - 1));
        }
    }
}
