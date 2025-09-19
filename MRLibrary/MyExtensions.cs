using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRLibrary
{
    public static class MyExtensions
    {
        public static PointF Add(this PointF p, PointF p1)
        {
            PointF pointF = new PointF(p.X + p1.X, p.Y + p1.Y);
            return pointF;
        }

        public static PointF Add(this PointF p, float x, float y)
        {
            PointF pointF = new PointF(p.X + x, p.Y + y);
            return pointF;
        }

        public static Point Add(this Point p, Point p1)
        {
            Point point = new Point(p.X + p1.X, p.Y + p1.Y);
            return point;
        }

        public static int Area(this Rectangle r)
        {
            return r.Width * r.Height;
        }

        public static float Area(this RectangleF r)
        {
            return r.Width * r.Height;
        }

        public static PointF Center(this RectangleF r)
        {
            PointF pointF = new PointF(r.Left + r.Width / 2f, r.Top + r.Height / 2f);
            return pointF;
        }

        public static PointF Center(this Rectangle r)
        {
            PointF pointF = new PointF((float)r.Left + (float)r.Width / 2f, (float)r.Top + (float)r.Height / 2f);
            return pointF;
        }

        public static float Distance(this PointF p1, PointF p2)
        {
            float single = (float)Math.Sqrt((double)((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y)));
            return single;
        }

        public static PointF Minus(this PointF p, PointF p1)
        {
            PointF pointF = new PointF(p.X - p1.X, p.Y - p1.Y);
            return pointF;
        }

        public static Point Minus(this Point p, Point p1)
        {
            Point point = new Point(p.X - p1.X, p.Y - p1.Y);
            return point;
        }

        public static PointF Multiply(this PointF p, float scale)
        {
            PointF pointF = new PointF(p.X * scale, p.Y * scale);
            return pointF;
        }

        public static Point ToPoint(this PointF p)
        {
            Point point = new Point((int)(p.X + 0.5f), (int)(p.Y + 0.5f));
            return point;
        }

        public static Point ToPoint(this PointD p)
        {
            Point point = new Point((int)(p.X + 0.5), (int)(p.Y + 0.5));
            return point;
        }

        public static PointF ToPointF(this Point p)
        {
            return new PointF((float)p.X, (float)p.Y);
        }

        public static Rectangle ToRectangle(this RectangleF r)
        {
            return new Rectangle(r.Location.ToPoint(), r.Size.ToSize());
        }

        public static RectangleF ToRectangleF(this Rectangle r)
        {
            RectangleF rectangleF = new RectangleF(r.Location.ToPointF(), r.Size.ToSizeF());
            return rectangleF;
        }

        public static Size ToSize(this SizeF s)
        {
            Size size = new Size((int)(s.Width + 0.5f), (int)(s.Height + 0.5f));
            return size;
        }

        public static SizeF ToSizeF(this Size s)
        {
            return new SizeF((float)s.Width, (float)s.Height);
        }
    }
}
