using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRLibrary
{
    public class LineF
    {
        public PointF Start;

        public PointF End;

        public PointF Center
        {
            get
            {
                PointF pointF = Start.Add(End).Multiply(0.5f);
                return pointF;
            }
        }

        public float Length
        {
            get
            {
                float single = (float)Math.Sqrt((double)((Start.X - End.X) * (Start.X - End.X) + (Start.Y - End.Y) * (Start.Y - End.Y)));
                return single;
            }
        }

        public double Theta
        {
            get
            {
                double num = Math.Atan2((double)(End.Y - Start.Y), (double)(End.X - Start.X));
                if (num < 0)
                {
                    num += 6.28318530717959;
                }
                return num;
            }
        }

        public LineD ToLineD
        {
            get
            {
                LineD lineD = new LineD()
                {
                    Start = new PointD(Start),
                    End = new PointD(End)
                };
                return lineD;
            }
        }

        public Vector UnitVector
        {
            get
            {
                Vector vector = new Vector((double)((End.X - Start.X) / Length), (double)((End.Y - Start.Y) / Length));
                return vector;
            }
        }

        public LineF()
        {
            Start = new PointF();
            End = new PointF();
        }

        public LineF(PointF s, PointF e)
        {
            Start = s;
            End = e;
        }

        public LineF(LineF ln)
        {
            Start = ln.Start;
            End = ln.End;
        }

        public PointF InterSection(LineF l2)
        {
            PointF pointF;
            PointF pointF1;
            double x = (double)Start.X;
            double y = (double)Start.Y;
            double num = (double)End.X;
            double y1 = (double)End.Y;
            double x1 = (double)l2.Start.X;
            double num1 = (double)l2.Start.Y;
            double x2 = (double)l2.End.X;
            double y2 = (double)l2.End.Y;
            try
            {
                double num2 = (x - num) * (num1 - y2) - (y - y1) * (x1 - x2);
                if (num2 != 0)
                {
                    PointD pointD = new PointD()
                    {
                        X = ((x * y1 - y * num) * (x1 - x2) - (x - num) * (x1 * y2 - num1 * x2)) / num2,
                        Y = ((x * y1 - y * num) * (num1 - y2) - (y - y1) * (x1 * y2 - num1 * x2)) / num2
                    };
                    pointF1 = pointD.ToPointF();
                }
                else
                {
                    pointF = new PointF();
                    pointF1 = pointF;
                }
            }
            catch
            {
                pointF = new PointF();
                pointF1 = pointF;
            }
            return pointF1;
        }

        public LineF Inverse()
        {
            LineF lineF = new LineF()
            {
                Start = End,
                End = Start
            };
            return lineF;
        }

        public LineF OffSet(PointF ofs)
        {
            LineF lineF = new LineF(Start.Add(ofs), End.Add(ofs));
            return lineF;
        }

        public static LineF operator *(LineF a, float b)
        {
            LineF lineF = new LineF()
            {
                Start = a.Start.Multiply(b),
                End = a.End.Multiply(b)
            };
            return lineF;
        }

        public void Translation(PointF t)
        {
            Start = Start.Add(t);
            End = End.Add(t);
        }
    }
}
