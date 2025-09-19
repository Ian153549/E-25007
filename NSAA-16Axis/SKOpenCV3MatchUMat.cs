using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using System.Drawing;

using Emgu.CV.CvEnum;
using System.Runtime.InteropServices;
using System.Diagnostics;


namespace MRVisionLib
{
    public class OpenCV3MatchUMat
    {

        //public int MinimiumArea = 256;
        public int MinimiumArea = 5192;
        public enum AlignAlgorithm { templateMatch, edgeMatch, lineScan };
        public OpenCV3MatchUMat()
        {
             
            CvInvoke.UseOpenCL = true;
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

        MatchPosition CvMatch(Mat sample, Mat template)
        {
            Matrix<float> ret = new Matrix<float>(sample.Cols - template.Cols + 1, sample.Rows - template.Rows + 1);
            CvInvoke.MatchTemplate(sample, template, ret, Emgu.CV.CvEnum.TemplateMatchingType.CcoeffNormed);

            double minValue, maxValue;
            Point minLocation, maxLocation;
            ret.MinMax(out minValue, out maxValue, out minLocation, out maxLocation);

            int mx = maxLocation.X;
            int my = maxLocation.Y;

            float sx = 0;
            float sy = 0;
            if (mx > 0 && mx < ret.Cols - 1 && my > 0 && my < ret.Rows - 1)
            {
                sx = PyramidInterpolationLocalMax(ret[my, mx - 1], ret[my, mx], ret[my, mx + 1]);
                sy = PyramidInterpolationLocalMax(ret[my - 1, mx], ret[my, mx], ret[my + 1, mx]);
            }

            ret.Dispose();
            return new MatchPosition(mx +  sx, my + sy, (float)maxValue, template.Size);
        }

        MatchPosition MatchPyrDown(Mat sample, Mat template, int level)
        {
            MatchPosition mp;
            if (level <= 0)
                return CvMatch(sample, template);
            else
            {
                Mat s = new Mat();
                CvInvoke.PyrDown(sample, s, Emgu.CV.CvEnum.BorderType.Default);
                Mat t = new Mat();
                CvInvoke.PyrDown(template, t, Emgu.CV.CvEnum.BorderType.Default);
                mp = MatchPyrDown(s, t, level - 1);
                s.Dispose();
                t.Dispose();
            }

            Rectangle r = new Rectangle((int)Math.Round(mp.X * 2) - 3, (int)Math.Round(mp.Y * 2) - 3, template.Cols + 6, template.Rows + 6);
            if (r.X < 0) r.X = 0;
            if (r.Y < 0) r.Y = 0;
            if (r.X + r.Width > sample.Cols) r.Width = sample.Cols - r.X;
            if (r.Y + r.Height > sample.Rows) r.Height = sample.Rows - r.Y;
            Mat level2 = new Mat(sample, r);

            MatchPosition p2 = CvMatch(level2, template);
            p2.X += r.Left;
            p2.Y += r.Top;
            return p2;
        }

        MatchPosition PyramidMatch(Mat sample, Mat template)
        {
            int level = 0;
            Size sz = template.Size;
            while (true)
            {
                sz.Width = sz.Width / 2;
                sz.Height = sz.Height / 2;
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

        public MatchPosition Match(Mat sample, Mat template)
        {
            if (sample != null && template != null)
            {
                MatchPosition mp = PyramidMatch(sample, template);
                mp.ImageSize = sample.Size;
                return mp;
            }
            else
                return new MatchPosition();
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
            if (algorithm == AlignAlgorithm.templateMatch)
                return Match(sample.Clone(), template.Clone());
            if (algorithm == AlignAlgorithm.edgeMatch)
                return MatchThickEdge(sample.Clone(), template.Clone());
            else
                return null;
        }

        //public MatchPosition MatchWithOption(Mat sample, Rectangle virticalRect, Rectangle horizontalRect, AlignAlgorithm algorithm)
        //{
            //if (algorithm == AlignAlgorithm.lineScan)
                //return LineScan(sample, virticalRect, horizontalRect);
            //else
                //return null;
        //}

        public MatchPosition MatchThickEdge(Mat sample, Mat template)
        {

            Mat thr1 = new Mat();
            CvInvoke.AdaptiveThreshold(template, thr1, 255, Emgu.CV.CvEnum.AdaptiveThresholdType.MeanC, Emgu.CV.CvEnum.ThresholdType.Binary, 9, 4);
            Mat sTemplate = new Mat();
            CvInvoke.GaussianBlur(thr1, sTemplate, new Size(7, 7), 0);
            thr1.Dispose();

            Mat thr2 = new Mat();
            CvInvoke.AdaptiveThreshold(sample, thr2, 255, Emgu.CV.CvEnum.AdaptiveThresholdType.MeanC, Emgu.CV.CvEnum.ThresholdType.Binary, 9, 4);
            Mat sSample = new Mat();
            CvInvoke.GaussianBlur(thr2, sSample, new Size(7, 7), 0);
            thr2.Dispose();

            OpenCV3MatchUMat matcher = new OpenCV3MatchUMat();
            MatchPosition mp = matcher.Match(sSample, sTemplate);

            return mp;
        }

        public Point FindPatternCenter(Mat template)
        {
            byte[,] crossManagedData = (byte[,])template.GetData();   // scan x axis first, then y axis

            int[] yVector = new int[template.Rows];
            int[] xVector = new int[template.Cols];

            for (int j = 0; j < template.Rows; j++)
                for (int i = 0; i < template.Cols; i++)
                    yVector[j] += crossManagedData[j, i];
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
                    xVector[i] += crossManagedData[j, i];
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

        public PointF FindPatternGrivityCenter(Mat template)
        {
            byte[,] crossManagedData = (byte[,])template.GetData();

            long xSum = 0;
            int xCount = 0;
            long ySum = 0;
            int yCount = 0;

            for (int i = 0; i < template.Cols; i++)
                for (int j = 0; j < template.Rows; j++)
                    if(crossManagedData[j, i] > 255*0.9)
                    {
                        xSum += i;
                        xCount++;
                        ySum += j;
                        yCount++;
                    }
            PointF grivityCenter = new PointF(xSum / xCount, ySum / yCount);
            return grivityCenter;
        }
    }
}
