using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using OpenCvSharp;
using System.Threading;
using System.Net.Http;
using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace MRLibrary
{
    public class OpenCV3MatchUMat
    {
        public int MinimiumArea = 5192;

        //public enum AlignAlgorithm { TemplateMatch, EdgeMatch, MatchTemplate, WaferTemplate, ORB, AIMatch };

        public enum AlignAlgorithm { TemplateMatch, EdgeMatch, AIMatch };
        public AlignAlgorithm MatchAlgorithm;

        public int iMethod;
        public int iBlockSize = 27;
        public int iBitwiseNot = 0;
        public int Max = 255;
        public int Gaussian = 7;
        public int Threashold = 127;

        public EVisionMatch _eVisionMatch = null;

        public Mat Tr1 = null;
        public Mat Tr2 = null;
        public Mat sSample = null;
        public Mat sTemplate = null;
        public Mat matDontCare = null;

        MatchPosition mps = new MatchPosition();

        public int imageCount = 501;
        public int imageCountLimit = 500;
        public BFMatcher matcher;
        public ORB orbDetector;
        private Mat descriptorsTemplate;

        //-----------------------------------------//
        public string dMsg = null;
        public Mat LearnMat = null;
        public Mat TransImg1 = null;
        public Mat TransImg2 = null;
        public Mat invertedImg;
        public Mat desTemplate;
        public Mat template = null;

        private static readonly Uri BaseUri = new Uri("http://127.0.0.1:9300");
        private readonly HttpClient _client;
        private readonly bool _enableInferGate;
        private readonly SemaphoreSlim _inferGate;

        public Mat[] sLevel = new Mat[10];
        public Mat[] tLevel = new Mat[10];

        //-----------------------------------------//

        public OpenCV3MatchUMat(string debugmsg, bool enableInferGate = false, int maxConcurrency = 1, TimeSpan? timeout = null)
        {
            mps = new MatchPosition();
            _eVisionMatch = new EVisionMatch(debugmsg);
            dMsg = debugmsg;
            //-----------------------------------------//
            LearnMat = new Mat(4000, 3000, MatType.CV_8UC1);
            TransImg2 = new Mat(4000, 3000, MatType.CV_8UC1);
            sSample = new Mat(4000, 3000, MatType.CV_8UC1);
            TransImg1 = new Mat(4000, 3000, MatType.CV_8UC1);
            sTemplate = new Mat(4000, 3000, MatType.CV_8UC1);
            Tr1=new Mat(4000, 3000, MatType.CV_8UC1);
            Tr2= new Mat(4000, 3000, MatType.CV_8UC1);
            for (int i = 0; i < 10; i++)
            {
                sLevel[i] = new Mat();
                tLevel[i] = new Mat();
            }
            _client = new HttpClient
            {
                BaseAddress = BaseUri,
                Timeout = timeout ?? TimeSpan.FromSeconds(15)
            };
            _enableInferGate = enableInferGate;
            _inferGate = enableInferGate ? new SemaphoreSlim(maxConcurrency, maxConcurrency) : null;
            desTemplate = new Mat();
            //-----------------------------------------//

        }
        void ParabolaVertex(int x1, int x2, int x3, double y1, double y2, double y3, out double xv, out double yv)
        {
            double denom = (x1 - x2) * (x1 - x3) * (x2 - x3);
            double A = (x3 * (y2 - y1) + x2 * (y1 - y3) + x1 * (y3 - y2)) / denom;
            double B = (x3 * x3 * (y1 - y2) + x2 * x2 * (y3 - y1) + x1 * x1 * (y2 - y3)) / denom;
            double C = (x2 * x3 * (x2 - x3) * y1 + x3 * x1 * (x3 - x1) * y2 + x1 * x2 * (x1 - x2) * y3) / denom;

            xv = -B / (2 * A);
            yv = C - B * B / (4 * A);
        }

        float ParabolicInterpolation(float a, float b, float c)
        {
            if (a - 2 * b + c == 0) return 0;
            return 0.5f * (a - c) / (a - 2 * b + c);
        }

        float PyramidInterpolationLocalMax(float a, float b, float c)
        {
            if (b - (a < c ? a : c) == 0) return 0;
            return 0.5f * (c - a) / (b - (a < c ? a : c));
        }

        float PyramidInterpolationLocalMin(float a, float b, float c)
        {
            if (b - (a > c ? a : c) == 0) return 0;
            return 0.5f * (c - a) / (b - (a > c ? a : c));
        }

        //MatchPosition CvMatch(Mat sample, Mat template)
        //{
        //    try
        //    {
        //        Matrix<float> ret = new Matrix<float>(sample.Cols - template.Cols + 1, sample.Rows - template.Rows + 1);
        //        CvInvoke.MatchTemplate(sample, template, ret, Emgu.CV.CvEnum.TemplateMatchingType.CcoeffNormed);

        //        double minValue, maxValue;
        //        Point minLocation, maxLocation;
        //        ret.MinMax(out minValue, out maxValue, out minLocation, out maxLocation);

        //        int mx = maxLocation.X;
        //        int my = maxLocation.Y;

        //        float sx = 0;
        //        float sy = 0;
        //        if (mx > 0 && mx < ret.Cols - 1 && my > 0 && my < ret.Rows - 1)
        //        {
        //            sx = PyramidInterpolationLocalMax(ret[my, mx - 1], ret[my, mx], ret[my, mx + 1]);
        //            sy = PyramidInterpolationLocalMax(ret[my - 1, mx], ret[my, mx], ret[my + 1, mx]);
        //        }

        //        return new MatchPosition(mx + sx, my + sy, (float)maxValue, template.Size);
        //    }
        //    catch (Exception)
        //    {
        //        return new MatchPosition(0, 0, (float)0.1, template.Size);
        //    }
        //}
        MatchPosition CvMatch(Mat sample, Mat template)
        {
            Mat result = new Mat();
            Cv2.MatchTemplate(sample, template, result, TemplateMatchModes.CCoeffNormed);
            Cv2.MinMaxLoc(result, out double minVal, out double maxVal, out Point minLoc, out Point maxLoc);
            int mx = maxLoc.X;
            int my = maxLoc.Y;

            float sx = 0, sy = 0;
            if (mx > 0 && mx < result.Cols - 1 && my > 0 && my < result.Rows - 1)
            {
                float left = result.At<float>(my, mx - 1);
                float center = result.At<float>(my, mx);
                float right = result.At<float>(my, mx + 1);
                sx = 0.5f * (left - right) / (left - 2 * center + right);

                float top = result.At<float>(my - 1, mx);
                float bottom = result.At<float>(my + 1, mx);
                sy = 0.5f * (top - bottom) / (top - 2 * center + bottom);
            }
            return new MatchPosition(mx + sx, my + sy, (float)maxVal, new System.Drawing.SizeF(template.Width, template.Height));
        }

        //void CvMatch(Mat sample, Mat template, ref MatchPosition match)
        //{
        //    try
        //    {
        //        Matrix<float> ret = new Matrix<float>(sample.Cols - template.Cols + 1, sample.Rows - template.Rows + 1);
        //        CvInvoke.MatchTemplate(sample, template, ret, Emgu.CV.CvEnum.TemplateMatchingType.CcoeffNormed);

        //        double minValue, maxValue;
        //        Point minLocation, maxLocation;
        //        ret.MinMax(out minValue, out maxValue, out minLocation, out maxLocation);

        //        int mx = maxLocation.X;
        //        int my = maxLocation.Y;

        //        float sx = 0;
        //        float sy = 0;
        //        if (mx > 0 && mx < ret.Cols - 1 && my > 0 && my < ret.Rows - 1)
        //        {
        //            sx = PyramidInterpolationLocalMax(ret[my, mx - 1], ret[my, mx], ret[my, mx + 1]);
        //            sy = PyramidInterpolationLocalMax(ret[my - 1, mx], ret[my, mx], ret[my + 1, mx]);
        //        }

        //        match.X = mx + sx;
        //        match.Y = my + sy;
        //        match.Score = maxValue;
        //        match.TemplateSize = template.Size;
        //    }
        //    catch (Exception)
        //    {
        //        match.X = 0;
        //        match.Y = 0;
        //        match.Score = 0.1;
        //        match.TemplateSize = template.Size;
        //        // return new MatchPosition(0, 0, (float)0.1, template.Size);
        //    }
        //}
        void CvMatch(Mat sample, Mat template, ref MatchPosition mp)
        {
            try
            {
                int resultCols = sample.Cols - template.Cols + 1;
                int resultRows = sample.Rows - template.Rows + 1;
                Mat result = new Mat(resultRows, resultCols, MatType.CV_32FC1);
                Cv2.MatchTemplate(sample, template, result, TemplateMatchModes.CCoeffNormed);
                Cv2.MinMaxLoc(result, out double minVal, out double maxValm, out Point minLoc, out Point maxLoc);
                int mx = maxLoc.X;
                int my = maxLoc.Y;
                float sx = 0;
                float sy = 0;

                if (mx > 0 && mx < result.Cols - 1 && my > 0 && my < result.Rows - 1)
                {
                    float left = result.At<float>(my, mx - 1);
                    float center = result.At<float>(my, mx);
                    float right = result.At<float>(my, mx + 1);

                    float top = result.At<float>(my - 1, mx);
                    float bottom = result.At<float>(my + 1, mx);
                    sx = PyramidInterpolationLocalMax(left, center, right);
                    sy = PyramidInterpolationLocalMax(top, center, bottom);
                }
                result.Dispose();
                mp.X = mx + sx;
                mp.Y = my + sy;
                mp.Score = (float)maxValm;
                mp.TemplateSize = new System.Drawing.SizeF(template.Width, template.Height) ;
            }
            catch (Exception)
            {
                mp.X = 0;
                mp.Y = 0;
                mp.Score = 0.1F;
                mp.TemplateSize = new System.Drawing.SizeF(template.Width, template.Height);
            }
        }

        //MatchPosition CvMatch(Mat sample, Mat template, Mat Mask)
        //{
        //    Matrix<float> ret = new Matrix<float>(sample.Cols - template.Cols + 1, sample.Rows - template.Rows + 1);
        //    CvInvoke.MatchTemplate(sample, template, ret, Emgu.CV.CvEnum.TemplateMatchingType.CcorrNormed, Mask);

        //    double minValue, maxValue;
        //    Point minLocation, maxLocation;
        //    ret.MinMax(out minValue, out maxValue, out minLocation, out maxLocation);

        //    int mx = maxLocation.X;
        //    int my = maxLocation.Y;

        //    float sx = 0;
        //    float sy = 0;
        //    if (mx > 0 && mx < ret.Cols - 1 && my > 0 && my < ret.Rows - 1)
        //    {
        //        sx = PyramidInterpolationLocalMax(ret[my, mx - 1], ret[my, mx], ret[my, mx + 1]);
        //        sy = PyramidInterpolationLocalMax(ret[my - 1, mx], ret[my, mx], ret[my + 1, mx]);
        //    }

        //    return new MatchPosition(mx + sx, my + sy, (float)maxValue, template.Size);
        //}

        MatchPosition MatchPyrDown(Mat sample, Mat template, int level)
        {
            MatchPosition mp;
            if (level <= 0)
                return CvMatch(sample, template);
            else
            {
                Mat s = new Mat();
                Cv2.PyrDown(sample, s);
                Mat t = new Mat();
                Cv2.PyrDown(template, t);
                mp = MatchPyrDown(s, t, level - 1);
            }

            Rect r = new Rect((int)Math.Round(mp.X * 2) - 3, (int)Math.Round(mp.Y * 2) - 3, template.Cols + 6, template.Rows + 6);
            if (r.X < 0) r.X = 0;
            if (r.Y < 0) r.Y = 0;
            if (r.X + r.Width > sample.Cols) r.Width = sample.Cols - r.X;
            if (r.Y + r.Height > sample.Rows) r.Height = sample.Rows - r.Y;
            Mat level2 = new Mat(sample, r);

            MatchPosition p2 = CvMatch(level2, template);
            p2.X += r.Left;
            p2.Y += r.Top;

            level2.Dispose();
            return p2;
        }

        void MatchPyrDown(Mat sample, Mat template, int level, ref MatchPosition match)
        {
            if (level <= 0)
                CvMatch(sample, template, ref match);
            else
            {
                Mat s = new Mat();
                Cv2.PyrDown(sample, s);
                Mat t = new Mat();
                Cv2.PyrDown(template, t);
                MatchPyrDown(s, t, level - 1, ref match);
            }

            Rect r = new Rect((int)Math.Round(match.X * 2) - 3, (int)Math.Round(match.Y * 2) - 3, template.Cols + 6, template.Rows + 6);
            if (r.X < 0) r.X = 0;
            if (r.Y < 0) r.Y = 0;
            if (r.X + r.Width > sample.Cols) r.Width = sample.Cols - r.X;
            if (r.Y + r.Height > sample.Rows) r.Height = sample.Rows - r.Y;
            Mat level2 = new Mat(sample, r);

            CvMatch(level2, template, ref match);
            match.X += r.Left;
            match.Y += r.Top;
            level2.Dispose();
        }

        MatchPosition PyramidMatch(Mat sample, Mat template)
        {
            int level = 0;
            Size sz = template.Size();
            while (true)
            {
                sz.Width /= 2;
                sz.Height /= 2;
                if (sz.Width * sz.Height > MinimiumArea)
                    level++;
                else
                    break;
            }
            MatchPosition mp = MatchPyrDown(sample, template, level);

            mp.X += (template.Cols) / 2.0f;
            mp.Y += (template.Rows) / 2.0f;
            return mp;
        }

        void PyramidMatch(Mat sample, Mat template, ref MatchPosition match)
        {
            int level = 0;
            Size sz = template.Size();
            while (true)
            {
                sz.Width /= 2;
                sz.Height /= 2;
                if (sz.Width * sz.Height > MinimiumArea)
                    level++;
                else
                    break;
            }

            MatchPyrDown(sample, template, level, ref match);

            match.X += (template.Cols) / 2.0f;
            match.Y += (template.Rows) / 2.0f;
        }

        public MatchPosition Match(Mat sample, Mat template)
        {
            if (sample != null && template != null)
            {
                //MatchPosition mp = PyramidMatch(sample, template);
                //mp.ImageSize = sample.Size;
                MatchPosition mp = new MatchPosition();
                GrayImage mask = new GrayImage(template.Height, template.Width);
                mask.Fill(255);
                EMatch(sample, template, mask, ref mp);
                return mp;
            }
            else
                return new MatchPosition();
        }

        public void Match(Mat sample, Mat template, GrayImage mask, ref MatchPosition mp)
        {
            if (sample != null && template != null)
            {
                EMatch(sample, template, mask, ref mp);
            }
            else
            {
                mp.X = 0;
                mp.Y = 0;
                mp.Score = (float)0.1;
                if (template != null)
                    mp.TemplateSize = new System.Drawing.SizeF(template.Width, template.Height);
                else
                    mp.TemplateSize = new System.Drawing.SizeF(sample.Width, sample.Height);
            }
        }

        void EMatch(Mat sample, Mat template, GrayImage Mask, ref MatchPosition mp)
        {
            bool bRet = false;
            try
            {
                Mat mat1 = CvHelper.GaussianBlur(sample, 3);
                Mat mat2 = CvHelper.GaussianBlur(template, 3);
                if (_eVisionMatch != null)
                    bRet = _eVisionMatch.BMatch(ImageConvert.MatToGrayImage(mat1), ImageConvert.MatToGrayImage(mat2), Mask, ref mp);
                if (!bRet)
                {
                    mp.X = 0;
                    mp.Y = 0;
                    mp.Score = 0.01F;
                    mp.TemplateSize = new System.Drawing.SizeF(template.Width, template.Height);

                }
            }
            catch (Exception)
            {
                mp.X = 0;
                mp.Y = 0;
                mp.Score = 0.02F;
                mp.TemplateSize = new System.Drawing.SizeF(template.Width, template.Height);
            }
        }

        public void Match(Mat sample, Mat template, ref MatchPosition match)
        {
            if (sample != null && template != null)
            {
                PyramidMatch(sample, template, ref match);
                match.ImageSize = new System.Drawing.Size(sample.Width, sample.Height);
            }
        }

        public MatchPosition Match(GrayImage bmp, GrayImage template, GrayImage mask)
        {
            MatchPosition matchPosition;
            bool flag;
            if (bmp != null)
            {
                // EImageBW8 imageBW8 = ImageConvert.GrayImageToImageBW8(bmp);
                try
                {
                    try
                    {
                        if (mask == null)
                        {
                            flag = false;
                        }
                        else
                        {
                            flag = (template.Width != mask.Width || template.Height != mask.Height);
                        }
                        if (!flag)
                        {
                            // Learn(template, mask);
                            // eMatch.CorrelationMode = ECorrelationMode.Normalized;
                            // eMatch.Match(imageBW8);
                            //if (eMatch.NumPositions != 0)
                            //{
                            //    EMatchPosition position = eMatch.GetPosition(0);
                            //    matchPosition = new MatchPosition(position.CenterX, position.CenterY, position.Score, new SizeF((float)template.Width, (float)template.Height));
                            //}
                            //else
                            //{
                            //    matchPosition = new MatchPosition();
                            //    return matchPosition;
                            //}
                            matchPosition = new MatchPosition();
                        }
                        else
                        {
                            matchPosition = new MatchPosition();
                            return matchPosition;
                        }
                    }
                    catch (Exception)
                    {
                        //Exception exception = exception1;
                        //MessageBoxM.Show(exception.Message, "Open eVision", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        matchPosition = new MatchPosition();
                        return matchPosition;
                    }
                }
                finally
                {

                }
                // matchPosition = MPosition;
            }
            else
            {
                matchPosition = new MatchPosition();
            }
            return matchPosition;
        }

        /*
        public MatchPosition Match(GrayImage sample, GrayImage template)
        {
            Mat s = new Mat(sample.Size, DepthType.Cv8U, 1);
            s.SetTo<byte>(sample.Bits);
            Mat t = new Mat(template.Size, DepthType.Cv8U, 1);
            t.SetTo<byte>(template.Bits);

            MatchPosition mp = Match(s, t);
            return mp;
        }
        */

        public MatchPosition MatchWithOption(Mat sample, Mat template, AlignAlgorithm algorithm)
        {
            /*
            if (algorithm == AlignAlgorithm.TemplateMatch)
                return Match(sample.Clone(), template.Clone());
            if (algorithm == AlignAlgorithm.EdgeMatch)
                return MatchThickEdge(sample.Clone(), template.Clone());
            else
                return null;
            */
            return MatchWithOptionTime(sample, template, algorithm, 0);
        }

        public void MatchWithOption(Mat sample, Mat template, GrayImage Mask, AlignAlgorithm algorithm, ref MatchPosition matchPosition)
        {
            MatchWithOptionTime(sample, template, Mask, algorithm, ref matchPosition, 0);
        }

        public void MatchWithOptionTime(Mat sample, Mat template, GrayImage Mask, AlignAlgorithm algorithm, ref MatchPosition matchPos, int Times)
        {
            int i;

            if (algorithm == AlignAlgorithm.TemplateMatch)
            {
                EMatch(sample.Clone(), template.Clone(), Mask, ref matchPos);
                for (i = 0; i < Times; i++)
                {
                    EMatch(sample.Clone(), template.Clone(), Mask, ref mps);
                    if (mps.Score > matchPos.Score)
                    {
                        mps.CopyTo(ref matchPos);
                    }
                }
            }
            else if (algorithm == AlignAlgorithm.EdgeMatch)
            {
                MatchPosition mp = MatchThickEdge(sample.Clone(), template.Clone());
                mp.CopyTo(ref matchPos);
                for (i = 0; i < Times; i++)
                {
                    mp = MatchThickEdge(sample.Clone(), template.Clone());
                    if (mp.Score > matchPos.Score)
                    {
                        mp.CopyTo(ref matchPos);
                    }
                }
            }
        }

        public MatchPosition MatchWithOptionTime(Mat sample, Mat template, AlignAlgorithm algorithm, int Times)
        {
            int i;
            MatchPosition mps;
            MatchPosition mp = null;

            if (algorithm == AlignAlgorithm.TemplateMatch)
            {
                mp = Match(sample.Clone(), template.Clone());
                for (i = 0; i < Times; i++)
                {
                    mps = Match(sample.Clone(), template.Clone());
                    if (mps.Score > mp.Score) mp = mps;
                }
            }
            else if (algorithm == AlignAlgorithm.EdgeMatch)
            {
                mp = MatchThickEdge(sample.Clone(), template.Clone());
                for (i = 0; i < Times; i++)
                {
                    mps = MatchThickEdge(sample.Clone(), template.Clone());
                    if (mps.Score > mp.Score) mp = mps;
                }
            }
            return mp;
        }

        public MatchPosition MatchThickEdge(Mat sample, Mat template)
        {

            Mat thr1 = new Mat();
            Cv2.AdaptiveThreshold(template, thr1, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.Binary, 9, 4);
            Mat sTemplate = new Mat();
            Cv2.GaussianBlur(thr1, sTemplate, new Size(7, 7), 0);

            Mat thr2 = new Mat();
            Cv2.AdaptiveThreshold(sample, thr2, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.Binary, 9, 4);
            Mat sSample = new Mat();
            Cv2.GaussianBlur(thr2, sSample, new Size(7, 7), 0);

            OpenCV3MatchUMat matcher = new OpenCV3MatchUMat("Edge");
            MatchPosition mp = matcher.Match(sSample, sTemplate);

            return mp;
        }

        public Point FindPatternCenter(Mat template)
        {
            int rows = template.Rows;
            int cols = template.Cols;
            int len = rows * cols;
            byte[] data = new byte[len];
            Marshal.Copy(template.Data, data, 0, len);
            //byte[,] crossManagedData = (byte[,])template.GetData();   // scan x axis first, then y axis

            int[] yVector = new int[template.Rows];
            int[] xVector = new int[template.Cols];

            for (int j = 0; j < template.Rows; j++)
                for (int i = 0; i < template.Cols; i++)
                    yVector[j] += data[j * cols + i];
            int yVectorPeak = yVector.Max();

            float yCenter = 0F;
            int passCount = 0;
            for (int j = 0; j < template.Rows; j++)
            {
                if (yVector[j] > yVectorPeak * 0.9)
                {
                    yCenter += j;
                    passCount++;
                }
            }
            yCenter /= passCount;

            for (int i = 0; i < template.Cols; i++)
                for (int j = 0; j < template.Rows; j++)
                    xVector[i] += data[j * cols + i];
            int xVectorPeak = xVector.Max();
            float xCenter = 0F;
            passCount = 0;
            for (int i = 0; i < template.Cols; i++)
            {
                if (xVector[i] > xVectorPeak * 0.9)
                {
                    xCenter += i;
                    passCount++;
                }
            }
            xCenter /= passCount;
            return new Point((int)Math.Round(xCenter), (int)Math.Round(yCenter));
        }

        public Point FindPatternCenter1(Mat template)
        {
            Mat thr = template.Clone();

            Cv2.Threshold(template, thr, 0, 255, ThresholdTypes.Otsu);

            Moments m = Cv2.Moments(thr, true);

            Point p = new Point((int)(m.M10 / m.M00), (int)(m.M01 / m.M00));

            return new Point(p.X, p.Y);
        }

        public Point FindPatternGrivityCenter(Mat template)
        {
            //byte[,] crossManagedData = (byte[,])template.GetData();
            byte[] crossManagedData = new byte[template.Rows * template.Cols];
            Marshal.Copy(template.Data, crossManagedData, 0, crossManagedData.Length);

            long xSum = 0;
            int xCount = 0;
            long ySum = 0;
            int yCount = 0;

            for (int i = 0; i < template.Cols; i++)
                for (int j = 0; j < template.Rows; j++)
                    if (crossManagedData[j * template.Cols + i] > 255 * 0.9)
                    {
                        xSum += i;
                        xCount++;
                        ySum += j;
                        yCount++;
                    }
            Point grivityCenter = new Point(xSum / xCount, ySum / yCount);
            return grivityCenter;
        }

        public void Learn(Mat pattern, GrayImage dontCare)
        {
            GrayImage image = ImageConvert.MatToGrayImage(pattern);
            // LogActivities.GDebugPattern(dMsg, pattern, dontCare);
            _eVisionMatch.Learn(image, dontCare);
        }

        //public void LearnWithAlgo(Mat pattern, GrayImage dontCare, AlignAlgorithm algorithm)
        //{
        //    if (!pattern.IsEmpty)
        //    {
        //        if (algorithm == AlignAlgorithm.TemplateMatch)
        //        {
        //            Learn(pattern, dontCare);
        //        }
        //        else if (algorithm == AlignAlgorithm.EdgeMatch)
        //        {
        //            if (imageCount > imageCountLimit)
        //            {
        //                if (Tr2 != null)
        //                {
        //                    Tr2 = null;
        //                }
        //                Tr2 = new Mat(pattern.Height, pattern.Width, DepthType.Cv8U, 1);
        //                if (Tr1 != null)
        //                {
        //                    Tr1 = null;
        //                }
        //                Tr1 = new Mat(pattern.Height, pattern.Width, DepthType.Cv8U, 1);
        //                if (sSample != null)
        //                {
        //                    sSample = null;
        //                }
        //                sSample = new Mat(pattern.Height, pattern.Width, DepthType.Cv8U, 1);
        //                imageCount = 0;
        //            }
        //            else
        //            {
        //                imageCount++;
        //            }
        //            CvInvoke.GaussianBlur(pattern, Tr1, new Size(7, 7), 0);
        //            CvInvoke.AdaptiveThreshold(Tr1, sSample, 255, Emgu.CV.CvEnum.AdaptiveThresholdType.MeanC, Emgu.CV.CvEnum.ThresholdType.Binary, 9, 4);

        //            Learn(sSample, dontCare);
        //        }
        //        else if (algorithm == AlignAlgorithm.MatchTemplate)
        //        {
        //            Learn(pattern, dontCare);
        //        }
        //        else if (algorithm == AlignAlgorithm.WaferTemplate)
        //        {
        //            if (imageCount > imageCountLimit)
        //            {
        //                if (Tr2 != null)
        //                {
        //                    Tr2 = null;
        //                }
        //                Tr2 = new Mat(pattern.Height, pattern.Width, DepthType.Cv8U, 1);
        //                if (Tr1 != null)
        //                {
        //                    Tr1 = null;
        //                }
        //                Tr1 = new Mat(pattern.Height, pattern.Width, DepthType.Cv8U, 1);
        //                if (sSample != null)
        //                {
        //                    sSample = null;
        //                }
        //                sSample = new Mat(pattern.Height, pattern.Width, DepthType.Cv8U, 1);
        //                imageCount = 0;
        //            }
        //            else
        //            {
        //                imageCount++;
        //            }
        //            CvInvoke.AdaptiveThreshold(pattern, Tr1, 255, Emgu.CV.CvEnum.AdaptiveThresholdType.MeanC, Emgu.CV.CvEnum.ThresholdType.Binary, iBlockSize, 2);
        //            CvInvoke.GaussianBlur(Tr1, Tr2, new Size(7, 7), 0);
        //            if (iBitwiseNot == 1)
        //            {
        //                CvInvoke.BitwiseNot(Tr2, sSample);
        //            }
        //            else
        //            {
        //                Tr2.CopyTo(sSample);
        //            }
        //            Learn(sSample, dontCare);
        //        }
        //        else if (algorithm == AlignAlgorithm.FindCenter)
        //        {
        //            if (imageCount > imageCountLimit)
        //            {
        //                if (Tr2 != null)
        //                {
        //                    Tr2 = null;
        //                }
        //                Tr2 = new Mat(pattern.Height, pattern.Width, DepthType.Cv8U, 1);
        //                if (Tr1 != null)
        //                {
        //                    Tr1 = null;
        //                }
        //                Tr1 = new Mat(pattern.Height, pattern.Width, DepthType.Cv8U, 1);
        //                if (sSample != null)
        //                {
        //                    sSample = null;
        //                }
        //                sSample = new Mat(pattern.Height, pattern.Width, DepthType.Cv8U, 1);
        //                imageCount = 0;
        //            }
        //            else
        //            {
        //                imageCount++;
        //            }

        //            if (iMethod == 0)
        //            {
        //                if (iBlockSize < 3) iBlockSize = 3;
        //                CvInvoke.GaussianBlur(pattern, Tr1, new System.Drawing.Size(Gaussian, Gaussian), 0, 0, BorderType.Default);
        //                CvInvoke.AdaptiveThreshold(Tr1, Tr2, Max, AdaptiveThresholdType.MeanC, ThresholdType.Binary, iBlockSize, 2);
        //            }
        //            else if (iMethod == 1)
        //            {
        //                CvInvoke.GaussianBlur(pattern, Tr1, new System.Drawing.Size(Gaussian, Gaussian), 0, 0, BorderType.Default);
        //                CvInvoke.Threshold(Tr1, Tr2, Threashold, Max, Emgu.CV.CvEnum.ThresholdType.Otsu);
        //            }

        //            if (iBitwiseNot == 1)
        //            {
        //                CvInvoke.BitwiseNot(Tr2, sSample);
        //            }
        //            else
        //            {
        //                Tr2.CopyTo(sSample);
        //            }

        //            Learn(sSample, dontCare);
        //        }
        //        MatchAlgorithm = algorithm;
        //    }
        //    else
        //    {
        //        _eVisionMatch.SetUnLearn();
        //    }
        //}
        public void LearnWithAlgo(Mat pattern, GrayImage dontCare, AlignAlgorithm algorithm)
        {
            if (algorithm == AlignAlgorithm.TemplateMatch)
            {
                Learn(pattern, dontCare);
            }
            else if (algorithm == AlignAlgorithm.EdgeMatch)
            {
                Cv2.GaussianBlur(pattern, sTemplate, new Size(7, 7), 0);
                Learn(sTemplate, dontCare);
            }
            else if(algorithm==AlignAlgorithm.AIMatch)
            {
                if(template != null)
                {
                    template.Dispose();
                }
                template = pattern.Clone();
                MatchAlgorithm = algorithm;
            }
            //else if (algorithm == AlignAlgorithm.MatchTemplate)
            //{
            //    Learn(pattern, dontCare);
            //}
            //else if (algorithm == AlignAlgorithm.WaferTemplate)
            //{
            //    Cv2.AdaptiveThreshold(pattern, Tr1, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.Binary, iBlockSize, 2);
            //    Cv2.GaussianBlur(Tr1, Tr2, new Size(7, 7), 0);
            //    if (iBitwiseNot == 1)
            //    {
            //        Cv2.BitwiseNot(Tr2, sTemplate);
            //    }
            //    else
            //    {
            //        Tr2.CopyTo(sTemplate);
            //    }
            //    Learn(sTemplate, dontCare);
            //}
            //else if (algorithm == AlignAlgorithm.ORB)
            //{
            //    //matDontCare = ImageConvert.GrayImageToMat(dontCare);
            //    //Cv2.GaussianBlur(pattern, Tr1, new Size(7, 7), 0, 0, BorderTypes.Default);
            //    //Cv2.AdaptiveThreshold(Tr1, Tr2, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.Binary, 27, 2);
            //    //Cv2.BitwiseNot(matDontCare, matDontCare);
            //    //Cv2.BitwiseNot(Tr2, pattern);
            //    //var orbDetector = ORB.Create();
            //    //KeyPoint[] kpTemplate;
            //    //Mat desTemplate = new Mat();
            //    //orbDetector.DetectAndCompute(pattern, matDontCare, out kpTemplate, desTemplate);
            //    //if (!desTemplate.Empty() && kpTemplate.Length > 0)
            //    //{
            //    //    matcher = new BFMatcher(NormTypes.Hamming, crossCheck: false);
            //    //}
            //    //template = pattern.Clone();
            //}
        }

        public void MatMatch(int debug, Mat image, ref MatchPosition mPos)
        {
            _eVisionMatch.MatMatch(image, ref mPos);
            if (debug == 1) LogActivities.GDebugMatch(dMsg, image, ref mPos);
        }

        public void MatMatchWithAlgo(int debug, Mat image, ref MatchPosition mPos, AlignAlgorithm algorithm)
        {
            if (algorithm == AlignAlgorithm.TemplateMatch)
            {
                MatMatch(debug, image, ref mPos);
            }
            else if (algorithm == AlignAlgorithm.EdgeMatch)
            {
                Cv2.GaussianBlur(image, Tr1, new Size(7, 7), 0);
                Cv2.AdaptiveThreshold(Tr1, Tr2, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.Binary, iBlockSize, 2);
                Cv2.BitwiseNot(Tr2, sSample);
                MatMatch(debug, sSample, ref mPos);
            }
            //else if (algorithm == AlignAlgorithm.MatchTemplate)
            //{
            //    MatMatch(debug, image, ref mPos);
            //}
            //else if (algorithm == AlignAlgorithm.WaferTemplate)
            //{
            //    if (imageCount > imageCountLimit)
            //    {
            //        if (Tr2 != null)
            //        {
            //            Tr2 = null;
            //        }
            //        Tr2 = new Mat(image.Height, image.Width, MatType.CV_8UC1);
            //        if (Tr1 != null)
            //        {
            //            Tr1 = null;
            //        }
            //        Tr1 = new Mat(image.Height, image.Width, MatType.CV_8UC1);
            //        if (sSample != null)
            //        {
            //            sSample = null;
            //        }
            //        sSample = new Mat(image.Height, image.Width, MatType.CV_8UC1);
            //        imageCount = 0;
            //    }
            //    else
            //    {
            //        imageCount++;
            //    }
            //    Cv2.AdaptiveThreshold(image, Tr2, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.Binary, iBlockSize, 2);
            //    Cv2.GaussianBlur(Tr2, Tr1, new Size(7, 7), 0);

            //    if (iBitwiseNot == 1)
            //    {
            //        Cv2.BitwiseNot(Tr1, sSample);
            //    }
            //    else
            //    {
            //        Tr1.CopyTo(sSample);
            //    }
            //    MatMatch(debug, sSample, ref mPos);
            //}
            //else if (algorithm == AlignAlgorithm.ORB)
            //{
            //    ORBMatch(0, sSample, ref mPos);
            //}
            else if (algorithm == AlignAlgorithm.AIMatch)
            {
                AIMatch(0, image, ref mPos);
            }
            else
            {
                throw new NotSupportedException("不支援的對齊演算法");
            }

            GC.KeepAlive(Tr2);
            GC.KeepAlive(Tr1);
            GC.KeepAlive(sSample);
        }

        private void AIMatch(int debug, Mat sSample, ref MatchPosition mPos)
        {
            string result = PostMatAsync(sSample).GetAwaiter().GetResult();
            var boxes = JArray.Parse(result);
            if (boxes.Count > 0)
            {
                foreach(var box in boxes)
                {
                    int cls = (int)box["class"];
                    string clsName = (string)box["name"];
                    int x1 = (int)box["x1"];
                    int y1 = (int)box["y1"];
                    int x2 = (int)box["x2"];    
                    int y2 = (int)box["y2"];
                    float Score = (float)box["conf"];
                    mPos.X = (x1 + x2) / 2;
                    mPos.Y = (y1 + y2) / 2;
                    mPos.Score = (double)Score;                    
                }
                
            }
            else
            {
                mPos.X = 0;
                mPos.Y = 0;
                mPos.Score = 0.1F;
                AIImageProcess.SaveAIFailImage(sSample, mPos);
            }
            if (debug == 1) LogActivities.GDebugMatch(dMsg, sSample, ref mPos);
        }
        public async Task AIMatchAsync(int debug, Mat sSample,MatchPosition mPos,CancellationToken ct = default)
        {
            string result = await PostMatAsync(sSample, ct).ConfigureAwait(false);

            var boxes = JArray.Parse(result);

            if (boxes.Count > 0)
            {
                var box = boxes[0];
                int x1 = (int)box["x1"];
                int y1 = (int)box["y1"];
                int x2 = (int)box["x2"];
                int y2 = (int)box["y2"];
                float score = (float)box["conf"];
                mPos.X = (x1 + x2) / 2;
                mPos.Y = (y1 + y2) / 2;
                mPos.Score = score;
            }
            if (debug == 1)
            {
                LogActivities.GDebugMatch("infer", sSample, ref mPos);
            }
        }

        private async Task<string> PostMatAsync(Mat mat, CancellationToken ct=default)
        {
            if (_enableInferGate)
                await _inferGate.WaitAsync(ct).ConfigureAwait(false);
            try
            {
                byte[] imgBytes = MatToBytes(mat);
                using (var content= new MultipartFormDataContent())
                using (var imgContent= new ByteArrayContent(imgBytes))
                {
                    imgContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
                    content.Add(imgContent, "image", "frame.jpg");
                    using (var response= await _client.PostAsync("infer", content, ct).ConfigureAwait(false))
                    {
                        response.EnsureSuccessStatusCode();
                        return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    }
                }
            }
            finally
            {
                if (_enableInferGate) _inferGate.Release();
            }
        }

        private static byte[] MatToBytes(Mat mat)
        {
            return mat.ToBytes(".jpg");
        }

        private bool ORBMatch(int debug, Mat image, ref MatchPosition mPos)
        {
            //Step 1: Detect and Compute keypoints and descriptors
            Mat desScene = new Mat();
            orbDetector.DetectAndCompute(image, null, out KeyPoint[] kpScene, desScene);
            if (desScene.Empty() || descriptorsTemplate.Empty()) return false;
            //Step 2:
            DMatch[][] knnMatches = matcher.KnnMatch(descriptorsTemplate, desScene, k: 2);

            //Step 3:
            List<DMatch> goodMatches = new List<DMatch>();
            foreach (var m in knnMatches)
            {
                if (m.Length >= 2 && m[0].Distance < 0.75 * m[1].Distance) goodMatches.Add(m[0]);
            }
            //Step 4:
            if (goodMatches.Count >= 4)
            {
                List<Point2f> ptsTemplate = new List<Point2f>();
                List<Point2f> ptsScene = new List<Point2f>();
                foreach (var match in goodMatches)
                {
                    ptsScene.Add(kpScene[match.TrainIdx].Pt);
                }
                using (var srcMat = Mat.FromArray(ptsTemplate.ToArray()))
                using (var dstMat = Mat.FromArray(ptsScene.ToArray()))
                {
                    Mat homography = Cv2.FindHomography(srcMat, dstMat, HomographyMethods.Ransac, 3);

                    if (!homography.Empty())
                    {
                        var patternSize = template.Size();
                        Point2f[] corners = new Point2f[]
                        {
                            new Point2f(0,0),
                            new Point2f(patternSize.Width,0),
                            new Point2f(patternSize.Width,patternSize.Height),
                            new Point2f(0,patternSize.Height)
                        };
                        Point2f[] transformedCorners = Cv2.PerspectiveTransform(corners, homography);
                        Mat resultImage = image.Clone();
                        for (int i = 0; i < 4; i++)
                        {
                            Cv2.Line(resultImage,
                                new Point(transformedCorners[i].X, transformedCorners[i].Y),
                                new Point(transformedCorners[(i + 1) % 4].X, transformedCorners[(i + 1) % 4].Y),
                                Scalar.Lime, 2);
                        }
                        float sumX = transformedCorners.Sum(p => p.X);
                        float sumY = transformedCorners.Sum(p => p.Y);
                        Point2f center = new Point2f(sumX / 4f, sumY / 4f);

                        mPos.X = center.X;
                        mPos.Y = center.Y;
                        mPos.Score = goodMatches.Count > 0 ? 1.0f - goodMatches.Average(m => m.Distance / 256f) : 0f;
                        mPos.TemplateSize = new System.Drawing.SizeF( patternSize.Width,patternSize.Height);
                        Cv2.Circle(resultImage, new Point(center.X, center.Y), 5, Scalar.Red, -1);
                        if (debug == 1)
                        {
                            Cv2.ImShow("ORB Match",resultImage);
                            Cv2.WaitKey(0);
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        public void MatMatch(Mat image, ref MatchPosition mPos)
        {
            if (MatchAlgorithm == AlignAlgorithm.TemplateMatch)
            {
                _eVisionMatch.MatMatch(image, ref mPos);
            }
            else if (MatchAlgorithm == AlignAlgorithm.EdgeMatch)
            {
                Cv2.GaussianBlur(image, Tr2, new Size(7, 7), 0);
                Cv2.AdaptiveThreshold(Tr2, sSample, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.Binary, iBlockSize, 2);

                _eVisionMatch.MatMatch(sSample, ref mPos);
            }
            //else if (MatchAlgorithm == AlignAlgorithm.FindCenter)
            //{
            //    if (imageCount > imageCountLimit)
            //    {
            //        if (Tr2 != null)
            //        {
            //            Tr2 = null;
            //        }
            //        Tr2 = new Mat(image.Height, image.Width, MatType.CV_8UC1);
            //        if (Tr1 != null)
            //        {
            //            Tr1 = null;
            //        }
            //        Tr1 = new Mat(image.Height, image.Width, MatType.CV_8UC1);
            //        if (sSample != null)
            //        {
            //            sSample = null;
            //        }
            //        sSample = new Mat(image.Height, image.Width, MatType.CV_8UC1);
            //        imageCount = 0;
            //    }
            //    else
            //    {
            //        imageCount++;
            //    }

            //    if (iMethod == 0)
            //    {
            //        if (iBlockSize < 3) iBlockSize = 3;
            //        Cv2.GaussianBlur(image, Tr1, new Size(7, 7), 0, 0, BorderTypes.Default);
            //        Cv2.AdaptiveThreshold(Tr1, Tr2, Max, AdaptiveThresholdTypes.MeanC, ThresholdTypes.Binary, iBlockSize, 2);
            //    }
            //    else if (iMethod == 1)
            //    {
            //        Cv2.GaussianBlur(image, Tr1, new Size(7, 7), 0, 0, BorderTypes.Default);
            //        Cv2.Threshold(Tr1, Tr2, Threashold, Max, ThresholdTypes.Otsu);
            //    }

            //    if (iBitwiseNot == 1)
            //    {
            //        Cv2.BitwiseNot(Tr2, sSample);
            //    }
            //    else
            //    {
            //        Tr2.CopyTo(sSample);
            //    }

            //    _eVisionMatch.MatMatch(sSample, ref mPos);
            //}
            //else if (MatchAlgorithm == AlignAlgorithm.MatchTemplate)
            //{
            //    _eVisionMatch.MatMatch(image, ref mPos);
            //}
            //else if (MatchAlgorithm == AlignAlgorithm.WaferTemplate)
            //{
            //    if (imageCount > imageCountLimit)
            //    {
            //        if (Tr2 != null)
            //        {
            //            Tr2 = null;
            //        }
            //        Tr2 = new Mat(image.Height, image.Width, MatType.CV_8UC1);
            //        if (Tr1 != null)
            //        {
            //            Tr1 = null;
            //        }
            //        Tr1 = new Mat(image.Height, image.Width, MatType.CV_8UC1);
            //        if (sSample != null)
            //        {
            //            sSample = null;
            //        }
            //        sSample = new Mat(image.Height, image.Width, MatType.CV_8UC1);
            //        imageCount = 0;
            //    }
            //    else
            //    {
            //        imageCount++;
            //    }
            //    Cv2.AdaptiveThreshold(image, Tr2, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.Binary, iBlockSize, 2);
            //    Cv2.GaussianBlur(Tr2, Tr1, new Size(7, 7), 0);

            //    if (iBitwiseNot == 1)
            //    {
            //        Cv2.BitwiseNot(Tr1, sSample);
            //    }
            //    else
            //    {
            //        Tr1.CopyTo(sSample);
            //    }
            //    _eVisionMatch.MatMatch(image, ref mPos);
            //}
            //else if (MatchAlgorithm == AlignAlgorithm.ORB)
            //{

            //}
            else if(MatchAlgorithm== AlignAlgorithm.AIMatch)
            {
                AIMatch(0, image, ref mPos);
            }

            GC.KeepAlive(Tr2);
            GC.KeepAlive(Tr1);
            GC.KeepAlive(sSample);
        }

        public void SetAlgoParameter(int ThresholdAlgorithm, int BlockSize, int BitwiseNot, int iMax, int iGaussian, int iThreshold)
        {
            iMethod = ThresholdAlgorithm;
            iBlockSize = BlockSize;
            iBitwiseNot = BitwiseNot;
            Max = iMax;
            Gaussian = iGaussian;
            Threashold = iThreshold;
        }
        public async Task MatMatchWithAlgoAsync(int debug, Mat image, MatchPosition mPos,AlignAlgorithm algorithm,CancellationToken ct = default)
        {
            if (algorithm == AlignAlgorithm.TemplateMatch)
            {
                MatMatch(debug, image, ref mPos);
            }
            else if (algorithm== AlignAlgorithm.EdgeMatch)
            {
                Cv2.GaussianBlur(image, Tr1, new Size(7, 7), 0);
                Cv2.AdaptiveThreshold(Tr1, Tr2, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.Binary, 27, 2);
                Cv2.BitwiseNot(Tr2, sSample);
                MatMatch(debug, sSample, ref mPos);
            }
            //else if (algorithm == AlignAlgorithm.MatchTemplate)
            //{
            //    MatMatch(debug, image, ref mPos);
            //}
            //else if(algorithm == AlignAlgorithm.WaferTemplate)
            //{
            //    if (imageCount > imageCountLimit)
            //    {
            //        Tr2?.Dispose();
            //        Tr2 = new Mat(image.Height, image.Width, MatType.CV_8UC1);
            //        Tr1?.Dispose();
            //        Tr1 = new Mat(image.Height, image.Width, MatType.CV_8UC1);
            //        sSample?.Dispose();
            //        sSample = new Mat(image.Height, image.Width, MatType.CV_8UC1);
            //        imageCount = 0;
            //    }
            //    else
            //    {
            //        imageCount++;
            //    }
            //    Cv2.AdaptiveThreshold(image, Tr2, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.Binary, iBlockSize, 2);
            //    Cv2.GaussianBlur(Tr2, Tr1, new Size(7, 7), 0);
            //    if (iBitwiseNot == 1) 
            //        Cv2.BitwiseNot(Tr1, sSample);
            //    else 
            //        Tr1.CopyTo(sSample);
            //    MatMatch(debug, sSample, ref mPos);
            //}
            //else if(algorithm == AlignAlgorithm.ORB)
            //{

            //}
            else if (algorithm == AlignAlgorithm.AIMatch)
            {
                await AIMatchAsync(debug, image, mPos, ct).ConfigureAwait(false);
            }
            else
            {
                throw new NotSupportedException("不支援的對齊算法");
            }
            GC.KeepAlive(Tr2);
            GC.KeepAlive(Tr1);
            GC.KeepAlive(sSample);
        }
    }
}
