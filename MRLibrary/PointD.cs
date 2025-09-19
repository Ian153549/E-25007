using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MRLibrary
{
    public class PointD
    {
        [XmlAttribute]
        public double X;

        [XmlAttribute]
        public double Y;

        public double Abs
        {
            get
            {
                double num = Math.Sqrt(X * X + Y * Y);
                return num;
            }
        }

        public double Theta
        {
            get
            {
                double num = Math.Atan2(Y, X);
                if (num < 0)
                {
                    num += 6.28318530717959;
                }
                return num;
            }
        }

        public PointD()
        {
            X = 0;
            Y = 0;
        }

        public PointD(double x, double y)
        {
            X = x;
            Y = y;
        }

        public PointD(PointD p)
        {
            X = p.X;
            Y = p.Y;
        }

        public PointD(PointF p)
        {
            X = (double)p.X;
            Y = (double)p.Y;
        }

        public void Clear()
        {
            X = 0;
            Y = 0;
        }

        public double Distance(PointD p)
        {
            double num = Math.Sqrt((X - p.X) * (X - p.X) + (Y - p.Y) * (Y - p.Y));
            return num;
        }

        public PointD FlipHorizontal(PointD c)
        {
            PointD pointD = new PointD()
            {
                X = c.X * 2 - X,
                Y = Y
            };
            return pointD;
        }

        public static PointD operator +(PointD a, PointD b)
        {
            PointD pointD = new PointD()
            {
                X = a.X + b.X,
                Y = a.Y + b.Y
            };
            return pointD;
        }

        public static PointD operator /(PointD a, double b)
        {
            PointD pointD = new PointD()
            {
                X = a.X / b,
                Y = a.Y / b
            };
            return pointD;
        }

        public static PointD operator ^(PointD a, double b)
        {
            PointD pointD = new PointD()
            {
                X = Math.Pow(a.X, b),
                Y = Math.Pow(a.Y, b)
            };
            return pointD;
        }

        public static PointD operator *(PointD a, double b)
        {
            PointD pointD = new PointD()
            {
                X = a.X * b,
                Y = a.Y * b
            };
            return pointD;
        }

        public static PointD operator -(PointD a, PointD b)
        {
            PointD pointD = new PointD()
            {
                X = a.X - b.X,
                Y = a.Y - b.Y
            };
            return pointD;
        }

        public static PointD operator -(PointD a)
        {
            PointD pointD = new PointD()
            {
                X = -a.X,
                Y = -a.Y
            };
            return pointD;
        }

        public static PointD operator +(PointD a)
        {
            PointD pointD = new PointD()
            {
                X = a.X,
                Y = a.Y
            };
            return pointD;
        }

        public PointD Rotate(double angle)
        {
            PointD pointD = new PointD()
            {
                X = X * Math.Cos(angle) - Y * Math.Sin(angle),
                Y = X * Math.Sin(angle) + Y * Math.Cos(angle)
            };
            return pointD;
        }

        public PointD Rotate(double angle, PointD relativePoint)
        {
            PointD pointD = this - relativePoint;
            PointD pointD1 = new PointD()
            {
                X = pointD.X * Math.Cos(angle) - pointD.Y * Math.Sin(angle),
                Y = pointD.X * Math.Sin(angle) + pointD.Y * Math.Cos(angle)
            };
            return relativePoint + pointD1;
        }

        public PointD RotateDegree(double angle)
        {
            return Rotate(angle / 180 * 3.14159265358979);
        }

        public PointD Scale(double xScale, double yScale)
        {
            PointD pointD = new PointD()
            {
                X = X * xScale,
                Y = Y * yScale
            };
            return pointD;
        }

        public PointF ToPointF()
        {
            return new PointF((float)X, (float)Y);
        }
    }
}
