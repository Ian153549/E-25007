//using System;
//using System.Collections.Generic;
//using OpenCvSharp;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace MRLibrary
//{
//    public class MatchPosition:IComparable<MatchPosition>
//    {
//        public double X;
//        public double Y;
//        public double Score;
//        public Point HotSpot;
//        public double Scale;
//        public char Char;
//        public bool Replaced;

//        public Size ImageSize;

//        public Size TemplateSize = new Size(0, 0);

//        public MatchPosition()
//        {
//        }

//        public int CompareTo(MatchPosition comparePart)
//        {
//            // A null value means that this object is greater.
//            if (comparePart == null)
//                return 1;
//            else
//                return X.CompareTo(comparePart.X);
//        }

//        public MatchPosition(double x, double y, double score, Size tempSize)
//        {
//            X = x;
//            Y = y;
//            Score = score;
//            TemplateSize = tempSize;
//        }

//        public MatchPosition(MatchPosition m)
//        {
//            X = m.X;
//            Y = m.Y;
//            Score = m.Score;
//            TemplateSize = m.TemplateSize;
//        }

//        public MatchPosition Clone(MatchPosition mp)
//        {
//            return (MatchPosition)mp.MemberwiseClone();
//        }

//        public static double CalculateTwoMatchPositionDistance(MatchPosition mp1, MatchPosition mp2)
//        {
//            return Math.Sqrt(Math.Pow(mp1.X - mp2.X, 2) + Math.Pow(mp1.Y - mp2.Y, 2));
//        }

//        public Rect ResultRectangle
//        {
//            get
//            {
//                return new Rect(new Point((float)(X - TemplateSize.Width / 2.0f), (float)(Y - TemplateSize.Height / 2.0f)), TemplateSize);
//            }
//        }

//        public void CopyTo(ref MatchPosition mp)
//        {
//            mp.X = X;
//            mp.Y = Y;
//            mp.Score = Score;
//            mp.TemplateSize = TemplateSize;
//            mp.Scale = Scale;
//        }
//    }

//    public class ProductMatchPositions : MatchPosition
//    {
//        public MatchPosition LMaskMp = new MatchPosition();
//        public MatchPosition LWaferMp = new MatchPosition();
//        public MatchPosition RMaskMp = new MatchPosition();
//        public MatchPosition RWaferMp = new MatchPosition();
//        public double PatternCenterDistance;

//        public ProductMatchPositions(MatchPosition lMaskMp, MatchPosition lWaferMp, MatchPosition rMaskMp, MatchPosition rWaferMp, double patternCenterDistance)
//        {
//            lMaskMp.CopyTo(ref LMaskMp);
//            lWaferMp.CopyTo(ref LWaferMp);
//            rMaskMp.CopyTo(ref RMaskMp);
//            rWaferMp.CopyTo(ref RWaferMp);
//            PatternCenterDistance = patternCenterDistance;
//        }

//        public ProductMatchPositions()
//        {
//        }
//    }

//    public class NMatchPosition : MatchPosition
//    {
//        public int find;
//        public MatchPosition LMaskMp = new MatchPosition();
//        public MatchPosition LWaferMp = new MatchPosition();
//        public MatchPosition RMaskMp = new MatchPosition();
//        public MatchPosition RWaferMp = new MatchPosition();

//        public NMatchPosition(MatchPosition lMaskMp, MatchPosition lWaferMp, MatchPosition rMaskMp, MatchPosition rWaferMp, int ifind)
//        {
//            find = ifind;
//            lMaskMp.CopyTo(ref LMaskMp);
//            lWaferMp.CopyTo(ref LWaferMp);
//            rMaskMp.CopyTo(ref RMaskMp);
//            rWaferMp.CopyTo(ref RWaferMp);
//        }

//        public NMatchPosition()
//        {
//        }
//    }
//}
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRLibrary
{
    public class MatchPosition :IComparable<MatchPosition>
    {
        public double X;
        public double Y;
        public double Score;
        public PointF HotSpot;
        public double Scale;
        public char Char;
        public bool Replaced;

        public Size ImageSize;

        public SizeF TemplateSize = new SizeF(0, 0);

        public MatchPosition()
        {
        }

        public int CompareTo(MatchPosition comparePart)
        {
            // A null value means that this object is greater.
            if (comparePart == null)
                return 1;
            else
                return X.CompareTo(comparePart.X);
        }

        public MatchPosition(double x, double y, double score, SizeF tempSize)
        {
            X = x;
            Y = y;
            Score = score;
            TemplateSize = tempSize;
        }

        public MatchPosition(MatchPosition m)
        {
            X = m.X;
            Y = m.Y;
            Score = m.Score;
            TemplateSize = m.TemplateSize;
        }

        public MatchPosition Clone(MatchPosition mp)
        {
            return (MatchPosition)mp.MemberwiseClone();
        }

        public static double CalculateTwoMatchPositionDistance(MatchPosition mp1, MatchPosition mp2)
        {
            return Math.Sqrt(Math.Pow(mp1.X - mp2.X, 2) + Math.Pow(mp1.Y - mp2.Y, 2));
        }

        public RectangleF ResultRectangle
        {
            get
            {
                return new RectangleF(new PointF((float)(X - TemplateSize.Width / 2.0f), (float)(Y - TemplateSize.Height / 2.0f)), TemplateSize);
            }
        }

        public void CopyTo(ref MatchPosition mp)
        {
            mp.X = X;
            mp.Y = Y;
            mp.Score = Score;
            mp.TemplateSize = TemplateSize;
            mp.Scale = Scale;
        }
    }

    public class ProductMatchPositions : MatchPosition
    {
        public MatchPosition LMaskMp = new MatchPosition();
        public MatchPosition LWaferMp = new MatchPosition();
        public MatchPosition RMaskMp = new MatchPosition();
        public MatchPosition RWaferMp = new MatchPosition();
        public double PatternCenterDistance;

        public ProductMatchPositions(MatchPosition lMaskMp, MatchPosition lWaferMp, MatchPosition rMaskMp, MatchPosition rWaferMp, double patternCenterDistance)
        {
            lMaskMp.CopyTo(ref LMaskMp);
            lWaferMp.CopyTo(ref LWaferMp);
            rMaskMp.CopyTo(ref RMaskMp);
            rWaferMp.CopyTo(ref RWaferMp);
            PatternCenterDistance = patternCenterDistance;
        }

        public ProductMatchPositions()
        {
        }
    }

    public class NMatchPosition : MatchPosition
    {
        public int find;
        public MatchPosition LMaskMp = new MatchPosition();
        public MatchPosition LWaferMp = new MatchPosition();
        public MatchPosition RMaskMp = new MatchPosition();
        public MatchPosition RWaferMp = new MatchPosition();

        public NMatchPosition(MatchPosition lMaskMp, MatchPosition lWaferMp, MatchPosition rMaskMp, MatchPosition rWaferMp, int ifind)
        {
            find = ifind;
            lMaskMp.CopyTo(ref LMaskMp);
            lWaferMp.CopyTo(ref LWaferMp);
            rMaskMp.CopyTo(ref RMaskMp);
            rWaferMp.CopyTo(ref RWaferMp);
        }

        public NMatchPosition()
        {
        }
    }
}
