using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRLibrary
{
    public class LineD
    {
        public PointD Start;

        public PointD End;

        public PointD Center
        {
            get
            {
                return (Start + End) / 2;
            }
        }

        public PointF CenterF
        {
            get
            {
                PointF pointF = ((Start + End) / 2).ToPointF();
                return pointF;
            }
        }

        public double Length
        {
            get
            {
                return Start.Distance(End);
            }
        }

        public double Theta
        {
            get
            {
                double num = Math.Atan2(End.Y - Start.Y, End.X - Start.X);
                if (num < 0)
                {
                    num += 6.28318530717959;
                }
                return num;
            }
        }

        public LineF ToLineF
        {
            get
            {
                LineF lineF = new LineF()
                {
                    Start = Start.ToPointF(),
                    End = End.ToPointF()
                };
                return lineF;
            }
        }

        public Vector UnitVector
        {
            get
            {
                Vector vector = new Vector((End.X - Start.X) / Length, (End.Y - Start.Y) / Length);
                return vector;
            }
        }

        public LineD()
        {
            Start = new PointD();
            End = new PointD();
        }

        public LineD(LineD ln)
        {
            Start = new PointD(ln.Start);
            End = new PointD(ln.End);
        }

        public LineD(PointD s, PointD e)
        {
            Start = s;
            End = e;
        }

        public LineD(PointF s, PointF e)
        {
            Start = new PointD(s);
            End = new PointD(e);
        }

        public LineD(LineF ln)
        {
            Start = new PointD(ln.Start);
            End = new PointD(ln.End);
        }

        public LineD(PointD start, Vector v)
        {
            Start = start;
            End = Start + v.ToPointD();
        }

        public double Distance(PointF p, LineD line)
        {
            double x;
            double y;
            bool flag;
            double num = (double)p.X - line.Start.X;
            double y1 = (double)p.Y - line.Start.Y;
            double x1 = line.End.X - line.Start.X;
            double num1 = line.End.Y - line.Start.Y;
            double num2 = num * x1 + y1 * num1;
            double num3 = x1 * x1 + num1 * num1;
            double num4 = num2 / num3;
            if (num4 < 0)
            {
                flag = true;
            }
            else
            {
                flag = (line.Start.X != line.End.X ? false : line.Start.Y == line.End.Y);
            }
            if (flag)
            {
                x = line.Start.X;
                y = line.Start.Y;
            }
            else if (num4 <= 1)
            {
                x = line.Start.X + num4 * x1;
                y = line.Start.Y + num4 * num1;
            }
            else
            {
                x = line.End.X;
                y = line.End.Y;
            }
            double x2 = (double)p.X - x;
            double y2 = (double)p.Y - y;
            double num5 = Math.Sqrt(x2 * x2 + y2 * y2);
            return num5;
        }

        public void FlipHorizontalRelativeTo(PointD pc)
        {
            PointD pointD = new PointD();
            PointD x = new PointD();
            pointD.X = pc.X * 2 - End.X;
            x.X = pc.X * 2 - Start.X;
            pointD.Y = End.Y;
            x.Y = Start.Y;
            Start = pointD;
            End = x;
        }

        public void FlipVerticalRelativeTo(PointD pc)
        {
            PointD pointD = new PointD();
            PointD y = new PointD();
            pointD.Y = pc.Y * 2 - Start.Y;
            y.Y = pc.Y * 2 - End.Y;
            pointD.X = Start.X;
            y.X = End.X;
            Start = pointD;
            End = y;
        }

        public PointD InterSection(LineD l2)
        {
            PointD pointD;
            double x = Start.X;
            double y = Start.Y;
            double num = End.X;
            double y1 = End.Y;
            double x1 = l2.Start.X;
            double num1 = l2.Start.Y;
            double x2 = l2.End.X;
            double y2 = l2.End.Y;
            try
            {
                double num2 = (x - num) * (num1 - y2) - (y - y1) * (x1 - x2);
                if (num2 != 0)
                {
                    PointD pointD1 = new PointD()
                    {
                        X = ((x * y1 - y * num) * (x1 - x2) - (x - num) * (x1 * y2 - num1 * x2)) / num2,
                        Y = ((x * y1 - y * num) * (num1 - y2) - (y - y1) * (x1 * y2 - num1 * x2)) / num2
                    };
                    pointD = pointD1;
                }
                else
                {
                    pointD = new PointD();
                }
            }
            catch
            {
                pointD = new PointD();
            }
            return pointD;
        }

        public LineD Inverse()
        {
            LineD lineD = new LineD()
            {
                Start = End,
                End = Start
            };
            return lineD;
        }

        public static LineD operator *(LineD a, double b)
        {
            LineD lineD = new LineD()
            {
                Start = a.Start * b,
                End = a.End * b
            };
            return lineD;
        }

        public PointD PerpendicularTo(PointF p)
        {
            double x;
            double y;
            bool flag;
            double num = (double)p.X - Start.X;
            double y1 = (double)p.Y - Start.Y;
            double x1 = End.X - Start.X;
            double num1 = End.Y - Start.Y;
            double num2 = num * x1 + y1 * num1;
            double num3 = x1 * x1 + num1 * num1;
            double num4 = num2 / num3;
            if (num4 < 0)
            {
                flag = true;
            }
            else
            {
                flag = (Start.X != End.X ? false : Start.Y == End.Y);
            }
            if (flag)
            {
                x = Start.X;
                y = Start.Y;
            }
            else if (num4 <= 1)
            {
                x = Start.X + num4 * x1;
                y = Start.Y + num4 * num1;
            }
            else
            {
                x = End.X;
                y = End.Y;
            }
            return new PointD(x, y);
        }

        public LineD PerpendicularToCenter(float len)
        {
            Vector vector = (new Vector(this)).UnitVector.Rotate(1.5707963267949);
            LineD lineD = new LineD()
            {
                Start = ((vector * (double)(len / 2f)) + new Vector(Center)).ToPointD(),
                End = ((vector * (double)(-len / 2f)) + new Vector(Center)).ToPointD()
            };
            return lineD;
        }

        public LineD Rotate(double rad)
        {
            Vector vector = (new Vector(this)).Rotate(rad);
            LineD lineD = new LineD(Start, Start + vector.ToPointD());
            return lineD;
        }

        public void Scale(double xrate, double yrate)
        {
            Start.Scale(xrate, yrate);
            End.Scale(xrate, yrate);
        }

        public RotationTranslation TransferTo(LineD dest)
        {
            RotationTranslation rotationTranslation = new RotationTranslation()
            {
                ObjectLocation = Center,
                Rotation = dest.Theta - Theta,
                Translation = dest.Center - Center
            };
            return rotationTranslation;
        }

        public void Translation(PointD t)
        {
            Start += t;
            End += t;
        }
    }
}
