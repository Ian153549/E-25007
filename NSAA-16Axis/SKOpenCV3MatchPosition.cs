using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRVisionLib
{
    public class MatchPosition : IComparable<MatchPosition>
    {
        public float X;
        public float Y;
        public float Score;
        public PointF HotSpot;
        public float Scale;
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
                return this.X.CompareTo(comparePart.X);
        }

        public MatchPosition(float x, float y, float score, SizeF tempSize)
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

        public static MatchPosition Clone(MatchPosition mp)
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
                return new RectangleF(new PointF(X - TemplateSize.Width / 2.0f, Y - TemplateSize.Height / 2.0f), TemplateSize);
            }
        }
    }

    public class ProductMatchPositions : MatchPosition
    {
        public MatchPosition LMaskMp = new MatchPosition();
        public MatchPosition LWaferMp = new MatchPosition();
        public MatchPosition RMaskMp = new MatchPosition();
        public MatchPosition RWaferMp = new MatchPosition();
        public double PatternCenterDistancePixel;

        public ProductMatchPositions(MatchPosition lMaskMp, MatchPosition lWaferMp, MatchPosition rMaskMp, MatchPosition rWaferMp, double patternCenterDistancePixel)
        {
            LMaskMp = lMaskMp;
            LWaferMp = lWaferMp;
            RMaskMp = rMaskMp;
            RWaferMp = rWaferMp;
            PatternCenterDistancePixel = patternCenterDistancePixel;
        }
        public ProductMatchPositions()
        {
        }
    }
}
