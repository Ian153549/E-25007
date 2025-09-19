using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MRLibrary
{
    public class CvHelper
    {
        public CvHelper()
        {
        }

        public static Mat AdaptiveThreshold(Mat img, int blockSize, int offset)
        {
            Mat mat = new Mat();
            Cv2.AdaptiveThreshold(img, mat, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.Binary, blockSize, (double)offset);
            return mat;
        }

        public static Mat BitwiseNot(Mat img)
        {
            Mat mat = new Mat();
            Cv2.BitwiseNot(img, mat, null);
            return mat;
        }

        public static byte[] BuildLUT(int lowerRange, int upperRange)
        {
            byte[] numArray;
            byte[] numArray1 = new byte[256];
            if (upperRange > lowerRange)
            {
                for (int i = 0; i < lowerRange; i++)
                {
                    numArray1[i] = 0;
                }
                for (int j = upperRange; j < 256; j++)
                {
                    numArray1[j] = 255;
                }
                double num = 255 / (double)(upperRange - lowerRange);
                for (int k = 0; k < upperRange - lowerRange + 1; k++)
                {
                    numArray1[lowerRange + k] = (byte)Math.Round((double)k * num);
                }
                numArray = numArray1;
            }
            else
            {
                numArray = numArray1;
            }
            return numArray;
        }

        public static float[] CalculateHistogram(Mat img)
        {
            //float[] singleArray;
            //using (VectorOfMat vectorOfMat = new VectorOfMat())
            //{
            //    vectorOfMat.Push(img);
            //    float[] singleArray1 = new float[] { default, 255f };
            //    Mat mat = new Mat(new Size(256, 1), Emgu.CV.CvEnum.DepthType.Cv32F, 1);
            //    CvInvoke.CalcHist(vectorOfMat, new int[1], null, mat, new int[] { 256 }, singleArray1, false);
            //    float[] singleArray2 = new float[mat.Height * mat.Width];
            //    Marshal.Copy(mat.DataPointer, singleArray2, 0, mat.Height * mat.Width);
            //    singleArray = singleArray2;
            //}
            //return singleArray;
            int[] channels = {0 };
            int[] histSize = { 256 };
            Rangef[] ranges = { new Rangef(0, 256) };
            Mat hist = new Mat();
            Cv2.CalcHist(
                new[] { img },
                channels,
                null,
                hist,
                1,
                histSize,
                ranges
                );
            float[] result = new float[hist.Rows * hist.Cols];
            Marshal.Copy(hist.Data, result, 0, result.Length);
            return result;
        }

        public static Mat Canny(Mat img, int threshold1, int threshold2)
        {
            Mat mat = new Mat();
            Cv2.Canny(img, mat, (double)threshold1, (double)threshold2, 3, false);
            return mat;
        }

        public static Mat Contrast(Mat img, int lowerRange, int upperRange)
        {
            //VectorOfByte vectorOfByte = new VectorOfByte(CvHelper.BuildLUT(lowerRange, upperRange));
            //Mat mat = new Mat();
            //CvInvoke.LUT(img, vectorOfByte, mat);
            //return mat;
            byte[] lutArray = CvHelper.BuildLUT(lowerRange, upperRange);
            Mat lut = new Mat();
            Marshal.Copy(lutArray, 0, lut.Data, lutArray.Length);
            Mat result = new Mat();
            Cv2.LUT(img, lut, result);
            return result;
        }

        public static Mat Crop(Mat img, Rectangle r)
        {
            Mat mat = new Mat();
            OpenCvSharp.Size size = new OpenCvSharp.Size(r.Width, r.Height);
            Cv2.GetRectSubPix(img, size, new Point2f(r.Center().X, r.Center().Y), mat);
            return mat;
        }

        public static Mat Crop(Mat img, RectangleF r)
        {
            Mat mat = new Mat();
            OpenCvSharp.Size size = new OpenCvSharp.Size(r.Width, r.Height);
            Cv2.GetRectSubPix(img, size, new Point2f(r.Center().X, r.Center().Y), mat);
            return mat;
        }

        /*
        public static CvBlob[] DetectBlob(Mat img)
        {
            CvBlobs cvBlob = new CvBlobs();
            CvBlobDetector cvBlobDetector = new CvBlobDetector();
            cvBlobDetector.Detect(img.ToImage<Gray, byte>(false), cvBlob);
            return cvBlob.Values.ToArray<CvBlob>();
        }
        */

        public static int[] DetectPeaks<T>(T[] arr, int range)
        {
            List<int> nums = new List<int>();
            T[] tArray = new T[range];
            for (int i = 0; i < (int)arr.Length - range + 1; i++)
            {
                Array.Copy(arr, i, tArray, 0, range);
                int num = Array.IndexOf<T>(tArray, tArray.Max<T>());
                if (num == range / 2)
                {
                    nums.Add(num + i);
                }
            }
            return nums.ToArray();
        }

        public static Mat Dilate(Mat img, OpenCvSharp.Size kSize, MorphShapes shape = 0, int iterations = 1)
        {
            OpenCvSharp.Point point = new OpenCvSharp.Point(-1, -1);
            Mat structuringElement = Cv2.GetStructuringElement(shape, kSize, point);
            Mat mat = new Mat();
            Cv2.Dilate(img, mat, structuringElement, point, iterations, BorderTypes.Reflect101, new Scalar(0, 0, 0));
            return mat;
        }

        public static Mat EqualizeHist(Mat img)
        {
            Mat mat = new Mat();
            Cv2.EqualizeHist(img, mat);
            return mat;
        }

        public static Mat Erode(Mat img, OpenCvSharp.Size kSize, MorphShapes shape = 0, int iterations = 1)
        {
            OpenCvSharp.Point point = new OpenCvSharp.Point(-1, -1);
            Mat structuringElement = Cv2.GetStructuringElement(shape, kSize, point);
            Mat mat = new Mat();
            Cv2.Erode(img, mat, structuringElement, point, iterations, BorderTypes.Reflect101, new Scalar(0, 0, 0));
            return mat;
        }

        public static RotatedRect[] FindContours(Mat canny)
        {
            //VectorOfVectorOfPoint vectorOfVectorOfPoint = new VectorOfVectorOfPoint();
            //Mat mat = new Mat();
            //Point point = new Point();
            //CvInvoke.FindContours(canny, vectorOfVectorOfPoint, mat, 0, ChainApproxMethod.ChainApproxSimple, point);
            //List<RotatedRect> rotatedRects = new List<RotatedRect>();
            //for (int i = 0; i < vectorOfVectorOfPoint.Size; i++)
            //{
            //    // rotatedRects.Add(CvInvoke.MinAreaRect(vectorOfVectorOfPoint.get_Item(i)));
            //    rotatedRects.Add(CvInvoke.MinAreaRect(vectorOfVectorOfPoint[i]));
            //}
            //return rotatedRects.ToArray();
            OpenCvSharp.Point[][] contours;
            HierarchyIndex[] hierarchy;
            Cv2.FindContours(
                canny,
                out contours,
                out hierarchy,
                RetrievalModes.External,
                ContourApproximationModes.ApproxSimple);
            List<RotatedRect> rotatedRects = new List<RotatedRect>();
            foreach (var contour in contours)
            {
                if (contour.Length > 0)
                    rotatedRects.Add(Cv2.MinAreaRect(contour));
            }
            return rotatedRects.ToArray();
        }

        public static Mat GaussianBlur(Mat img, int kernalSize)
        {
            Mat mat = new Mat();
            Cv2.GaussianBlur(img, mat, new OpenCvSharp.Size(kernalSize, kernalSize), 0, 0, BorderTypes.Reflect101);
            return mat;
        }

        public static Mat LaplacianOfGaussian(Mat img, int kernalSize = 1)
        {
            Mat mat = new Mat();
            Cv2.GaussianBlur(img, mat, new OpenCvSharp.Size(3, 3), 0, 0, BorderTypes.Reflect101);
            Mat mat1 = new Mat();
            Cv2.Laplacian(mat, mat1, MatType.CV_16S, kernalSize, 1, 0, BorderTypes.Reflect101);
            Mat mat2 = new Mat();
            Cv2.Normalize(mat1, mat2, 0, 255, NormTypes.MinMax);
            Mat mat3 = new Mat();
            mat2.ConvertTo(mat3, 0, 1, 0);
            mat2.Dispose();
            mat1.Dispose();
            mat.Dispose();
            return mat3;
        }

        public static Mat MedianBlur(Mat img, int kernalSize)
        {
            Mat mat = new Mat();
            Cv2.MedianBlur(img, mat, kernalSize);
            return mat;
        }

        public static Tuple<int, int> MinMax(Mat img)
        {
            double num = 0;
            double num1 = 0;
            OpenCvSharp.Point point = new OpenCvSharp.Point();
            OpenCvSharp.Point point1 = new OpenCvSharp.Point();
            Cv2.MinMaxLoc(img, out num, out num1, out point, out point1, null);
            return Tuple.Create<int, int>((int)num, (int)num1);
        }

        public static Mat Sharpen(Mat img)
        {
            float[] kernel = new float[]
            {
                -1f,-1f,-1f,
                -1f, 9f,-1f,
                -1f,-1f,-1f
            };
            Mat kernelMat = new Mat(3, 3, MatType.CV_32F);
            Marshal.Copy(kernel, 0, kernelMat.Data, kernel.Length);
            Mat mat = new Mat();
            Cv2.Filter2D(img, mat, -1, kernelMat, new OpenCvSharp.Point(-1, -1), 0, BorderTypes.Reflect101);
            return mat;
        }

        public static Mat Sobel(Mat img, int kernelSize = 3)
        {
            Mat mat = new Mat();
            Cv2.Sobel(img, mat, MatType.CV_16S, 1, 0, kernelSize, 1, 0, BorderTypes.Reflect101);
            Mat mat1 = new Mat();
            Cv2.ConvertScaleAbs(mat, mat1, 1, 0);
            Mat mat2 = new Mat();
            Cv2.Sobel(img, mat2, MatType.CV_16S, 0, 1, kernelSize, 1, 0, BorderTypes.Reflect101);
            Mat mat3 = new Mat();
            Cv2.ConvertScaleAbs(mat2, mat3, 1, 0);
            Mat mat4 = new Mat();
            // CvInvoke.AddWeighted(mat1, 1, mat3, 1, 0, mat4, -1);
            Cv2.AddWeighted(mat1, 1, mat3, 1, 0, mat4);
            Cv2.Normalize(mat4, mat4, 0, 255, NormTypes.MinMax);
            Mat mat5 = new Mat();
            mat4.ConvertTo(mat5, MatType.CV_8U, 1, 0);
            mat.Dispose();
            mat1.Dispose();
            mat2.Dispose();
            mat3.Dispose();
            mat4.Dispose();
            return mat5;
        }

        public static Mat Threshold(Mat img, int threshold)
        {
            Mat mat = new Mat();
            Cv2.Threshold(img, mat, (double)threshold, 255, ThresholdTypes.Binary);
            return mat;
        }

        public static Mat ThresholdIsodata(Mat img)
        {
            float[] singleArray = CvHelper.CalculateHistogram(img);
            int num = 51;
            float single = 2.55f;
            try
            {
                int num1 = 0;
                while (true)
                {
                    float single1 = 0f;
                    float single2 = 0f;
                    for (int i = 0; i <= num; i++)
                    {
                        single1 = single1 + (float)i * singleArray[i];
                        single2 += singleArray[i];
                    }
                    if (single2 != 0f)
                    {
                        single1 = (single2 == 0f ? 0f : single1 / single2);
                        float single3 = 0f;
                        single2 = 0f;
                        for (int j = num + 1; j < 256; j++)
                        {
                            single3 = single3 + (float)j * singleArray[j];
                            single2 += singleArray[j];
                        }
                        single3 = (single2 == 0f ? 0f : single3 / single2);
                        float single4 = (single1 + single3) / 2f;
                        if (Math.Abs((float)num - single4) >= single)
                        {
                            num = (int)Math.Round((double)single4);
                            num1++;
                            if (num1 > 15)
                            {
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        num++;
                        if (num == 255)
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            Mat mat = new Mat();
            Cv2.Threshold(img, mat, (double)num, 255, ThresholdTypes.Binary);
            return mat;
        }

        public static Mat ThresholdOtsu(Mat img)
        {
            Mat mat = new Mat();
            Cv2.Threshold(img, mat, 0, 255, ThresholdTypes.Otsu);
            return mat;
        }
    }
}
