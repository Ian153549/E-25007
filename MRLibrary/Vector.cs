using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRLibrary
{
    public class Vector
    {
        public double X;

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

        public Vector UnitVector
        {
            get
            {
                Vector vector = new Vector(X / Abs, Y / Abs);
                return vector;
            }
        }

        public Vector()
        {
            X = 0;
            Y = 0;
        }

        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Vector(PointD p)
        {
            X = p.X;
            Y = p.Y;
        }

        public Vector(PointD start, PointD end)
        {
            X = end.X - start.X;
            Y = end.Y - start.Y;
        }

        public Vector(LineD ln)
        {
            X = ln.End.X - ln.Start.X;
            Y = ln.End.Y - ln.Start.Y;
        }

        public static double AngleOf(Vector v1, Vector v2)
        {
            double num = Math.Acos(Vector.DotProduct(v1, v2) / (v1.Abs * v2.Abs));
            if (Vector.CrossProduct(v1, v2) < 0)
            {
                num = -num;
            }
            return num;
        }

        public static double CrossProduct(Vector v1, Vector v2)
        {
            double x = v1.X * v2.Y - v1.Y * v2.X;
            return x;
        }

        public static double DotProduct(Vector v1, Vector v2)
        {
            double x = v1.X * v2.X + v1.Y * v2.Y;
            return x;
        }

        public static Vector operator +(Vector v, Vector a)
        {
            Vector vector = new Vector()
            {
                X = v.X + a.X,
                Y = v.Y + a.Y
            };
            return vector;
        }

        public static Vector operator *(Vector a, double b)
        {
            Vector vector = new Vector()
            {
                X = a.X * b,
                Y = a.Y * b
            };
            return vector;
        }

        public Vector Rotate(double angle)
        {
            Vector vector = new Vector()
            {
                X = X * Math.Cos(angle) - Y * Math.Sin(angle),
                Y = X * Math.Sin(angle) + Y * Math.Cos(angle)
            };
            return vector;
        }

        public static Vector SplitAngle(Vector v1, Vector v2)
        {
            double num = Vector.AngleOf(v1, v2);
            return v1.Rotate(num / 2);
        }

        public PointD ToPointD()
        {
            return new PointD(X, Y);
        }
    }
}
