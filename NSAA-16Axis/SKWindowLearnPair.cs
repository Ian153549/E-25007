using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using Emgu.CV;
using System.Windows.Forms;

namespace MRVisionLib
{
    class SKWindowLearnPair
    {
        enum SIDE { center, up, down, left, right, leftUp, leftDown, rightUp, rightDown, nowhere };

        public RectangleF PatternRoi;
        public RectangleF PatternOldRoi;

        private Point _ptMouseDown;
        private bool _isMouseDown = false;
        private int _dragMode = (int)SIDE.nowhere;
        private Cursor[] _adjustCursor = { Cursors.SizeAll, Cursors.SizeNS, Cursors.SizeNS, Cursors.SizeWE, Cursors.SizeWE, Cursors.SizeNWSE, Cursors.SizeNESW, Cursors.SizeNESW, Cursors.SizeNWSE, Cursors.Arrow };
        private SKZoomAndPanWindow _ownerWindow;

        public bool IsInRoi;
        public bool IsAdjustStatus;
        public int MaxSideLength;
        public int MinSideLength;

        public SKWindowLearnPair(Rectangle roi, SKZoomAndPanWindow ownerWindow)
        {
            MaxSideLength = 900;
            MinSideLength = 5;
            PatternRoi = roi;
            PatternOldRoi = roi;
            _ownerWindow = ownerWindow;
        }
        public SKWindowLearnPair(Rectangle roi, SKZoomAndPanWindow ownerWindow, int maxSideLength, int minSideLength)
        {
            MaxSideLength = maxSideLength;
            MinSideLength = minSideLength;
            PatternRoi = roi;
            PatternOldRoi = roi;
            _ownerWindow = ownerWindow;
        }

        public void PatternRoiMouseUp()
        {
            PatternOldRoi = PatternRoi;
            _dragMode = (int)SIDE.nowhere;
            IsAdjustStatus = false;
            _isMouseDown = false;
        }

        public void PatternRoiMouseDown(MouseEventArgs e)
        {
            _ptMouseDown = e.Location;
            _isMouseDown = true;
        }
        public void PatternRoiMouseMove(MouseEventArgs e, Point aftZoomRoiLeftUpPt, int edgeWidth, int edgeHeight, float zoomRatio)
        {
            SIDE side;
            side = SIDE.nowhere;

            Rectangle patternRoiAftZoom = new Rectangle((int)(PatternRoi.X * zoomRatio), (int)(PatternRoi.Y * zoomRatio)
                , (int)(PatternRoi.Width * zoomRatio), (int)(PatternRoi.Height * zoomRatio));

            AdjustRect(e, patternRoiAftZoom, zoomRatio, edgeWidth, edgeHeight);

            if (IsAdjustStatus) return;
            for (int s = (int)SIDE.center; s < (int)SIDE.nowhere; s++) //new a rectangle to adjust PatternRoi and decide which side to select
                if (RectAdjustArea(patternRoiAftZoom, (SIDE)s).Contains(e.X + aftZoomRoiLeftUpPt.X, e.Y + aftZoomRoiLeftUpPt.Y))
                    side = (SIDE)s;

            if ((int)side >= (int)SIDE.center && (int)side < (int)SIDE.nowhere)  //change cursor
                _ownerWindow.Cursor = _adjustCursor[(int)side];

            for (int s = (int)SIDE.center; s < (int)SIDE.nowhere + 1; s++)  //select dragMode
                if (side == (SIDE)s && _isMouseDown)
                    _dragMode = (int)side;

            if (side != SIDE.nowhere) IsInRoi = true;
            if (side == SIDE.nowhere) IsInRoi = false;
            if (_dragMode != (int)SIDE.nowhere) IsAdjustStatus = true;
        }

        private Rectangle RectAdjustArea(Rectangle roi, SIDE side)
        {
            int oneOfTenWidth = roi.Width / 10;
            int oneOfTenHeight = roi.Height / 10;
            switch (side)
            {
                case SIDE.center:
                    return Rectangle.FromLTRB(roi.Left + oneOfTenWidth, roi.Top + oneOfTenHeight, roi.Right - oneOfTenWidth, roi.Bottom - oneOfTenHeight);
                case SIDE.up:
                    return Rectangle.FromLTRB(roi.Left + oneOfTenWidth, roi.Top - oneOfTenHeight, roi.Right - oneOfTenWidth, roi.Top + oneOfTenHeight);
                case SIDE.down:
                    return Rectangle.FromLTRB(roi.Left + oneOfTenWidth, roi.Bottom - oneOfTenHeight, roi.Right - oneOfTenWidth, roi.Bottom + oneOfTenHeight);
                case SIDE.left:
                    return Rectangle.FromLTRB(roi.Left - oneOfTenWidth, roi.Top + oneOfTenHeight, roi.Left + oneOfTenWidth, roi.Bottom - oneOfTenHeight);
                case SIDE.right:
                    return Rectangle.FromLTRB(roi.Right - oneOfTenWidth, roi.Top + oneOfTenHeight, roi.Right + oneOfTenWidth, roi.Bottom - oneOfTenWidth);
                case SIDE.leftUp:
                    return Rectangle.FromLTRB(roi.Left - oneOfTenWidth, roi.Top - oneOfTenHeight, roi.Left + oneOfTenWidth, roi.Top + oneOfTenHeight);
                case SIDE.leftDown:
                    return Rectangle.FromLTRB(roi.Left - oneOfTenWidth, roi.Bottom - oneOfTenHeight, roi.Left + oneOfTenWidth, roi.Bottom + oneOfTenHeight);
                case SIDE.rightUp:
                    return Rectangle.FromLTRB(roi.Right - oneOfTenWidth, roi.Top - oneOfTenHeight, roi.Right + oneOfTenWidth, roi.Top + oneOfTenHeight);
                case SIDE.rightDown:
                    return Rectangle.FromLTRB(roi.Right - oneOfTenWidth, roi.Bottom - oneOfTenHeight, roi.Right + oneOfTenWidth, roi.Bottom + oneOfTenHeight);
                case SIDE.nowhere:
                    return Rectangle.Empty;
                default:
                    return Rectangle.Empty;
            }
        }

        private void AdjustRect(MouseEventArgs e, Rectangle patternRoiAftZoom, float zoomRatio, int edgeRight, int edgeBottom)
        {

            float dx = (e.X - _ptMouseDown.X) / zoomRatio;
            float dy = (e.Y - _ptMouseDown.Y) / zoomRatio;

            if (PatternRoi.Width < MinSideLength)
            {
                PatternRoi.Width += 1;
                _dragMode = (int)SIDE.nowhere;
                return;
            }
            if (PatternRoi.Height < MinSideLength)
            {
                PatternRoi.Height += 1;
                _dragMode = (int)SIDE.nowhere;
                return;
            }
            if (PatternRoi.X < 0)
            {
                PatternRoi.X += 1;
                _dragMode = (int)SIDE.nowhere;
                return;
            }
            if (PatternRoi.Y < 0)
            {
                PatternRoi.Y += 1;
                _dragMode = (int)SIDE.nowhere;
                return;
            }


            /*
            if (PatternRoi.Width >= edgeRight)
            {
                PatternRoi.Width -= 1;
                DragMode = (int)SIDE.nowhere;
                return;
            }
            if (PatternRoi.Height >= edgeBottom)
            {
                PatternRoi.Height -= 1;
                DragMode = (int)SIDE.nowhere;
                return;
            }*/

            switch (_dragMode)
            {
                case (int)SIDE.center:
                    PatternRoi.X = dx + PatternOldRoi.X;
                    PatternRoi.Y = dy + PatternOldRoi.Y;
                    break;
                case (int)SIDE.up:
                    PatternRoi = RectangleF.FromLTRB(PatternOldRoi.Left, PatternOldRoi.Top + dy, PatternOldRoi.Right, PatternOldRoi.Bottom);
                    break;
                case (int)SIDE.down:
                    PatternRoi = RectangleF.FromLTRB(PatternOldRoi.Left, PatternOldRoi.Top, PatternOldRoi.Right, dy + PatternOldRoi.Bottom);
                    break;
                case (int)SIDE.left:
                    PatternRoi = RectangleF.FromLTRB(PatternOldRoi.Left + dx, PatternOldRoi.Top, PatternOldRoi.Right, PatternOldRoi.Bottom);
                    break;
                case (int)SIDE.right:
                    PatternRoi = RectangleF.FromLTRB(PatternOldRoi.Left, PatternOldRoi.Top, PatternOldRoi.Right + dx, PatternOldRoi.Bottom);
                    break;
                case (int)SIDE.leftUp:
                    PatternRoi = RectangleF.FromLTRB(PatternOldRoi.Left + dx, PatternOldRoi.Top + dy, PatternOldRoi.Right, PatternOldRoi.Bottom);
                    break;
                case (int)SIDE.leftDown:
                    PatternRoi = RectangleF.FromLTRB(PatternOldRoi.Left + dx, PatternOldRoi.Top, PatternOldRoi.Right, PatternOldRoi.Bottom + dy);
                    break;
                case (int)SIDE.rightUp:
                    PatternRoi = RectangleF.FromLTRB(PatternOldRoi.Left, PatternOldRoi.Top + dy, PatternOldRoi.Right + dx, PatternOldRoi.Bottom);
                    break;
                case (int)SIDE.rightDown:
                    PatternRoi = RectangleF.FromLTRB(PatternOldRoi.Left, PatternOldRoi.Top, PatternOldRoi.Right + dx, PatternOldRoi.Bottom + dy);
                    break;
            }

            if (_dragMode != (int)SIDE.nowhere)  //avoid rect fail
            {
                if (PatternRoi.X <= 0) PatternRoi.X = 0;
                if (PatternRoi.Y <= 0) PatternRoi.Y = 0;
                if (PatternRoi.X + PatternRoi.Width >= edgeRight) PatternRoi.X = edgeRight - PatternRoi.Width - 1;
                if (PatternRoi.Y + PatternRoi.Height >= edgeBottom) PatternRoi.Y = edgeBottom - PatternRoi.Height - 1;
            }

        }

        public UMat DrawPatternRectangle(UMat image, float zoomRatio, Point aftZoomRoiLeftUpPt, Emgu.CV.Structure.MCvScalar color)
        {
            RectangleF patternRoiAftZoomF;
            Rectangle patternRoiAftZoom;
            Rectangle patternRoiInWindow;
            patternRoiAftZoomF = new RectangleF(PatternRoi.X * zoomRatio, PatternRoi.Y * zoomRatio,
                                PatternRoi.Width * zoomRatio, PatternRoi.Height * zoomRatio);
            patternRoiAftZoom = Rectangle.Round(patternRoiAftZoomF);
            patternRoiInWindow = new Rectangle(patternRoiAftZoom.X - aftZoomRoiLeftUpPt.X, patternRoiAftZoom.Y - aftZoomRoiLeftUpPt.Y
                                , patternRoiAftZoom.Width, patternRoiAftZoom.Height);

            if (new Rectangle(aftZoomRoiLeftUpPt, _ownerWindow.ClientSize).IntersectsWith(patternRoiAftZoom))
            {
                CvInvoke.Rectangle(image, patternRoiInWindow, color, 1);
                CvInvoke.Line(image, new Point(patternRoiInWindow.Left, patternRoiInWindow.Top + patternRoiInWindow.Height / 2)
                    , new Point(patternRoiInWindow.Right, patternRoiInWindow.Top + patternRoiInWindow.Height / 2), color, 1);
                CvInvoke.Line(image, new Point(patternRoiInWindow.Left + patternRoiInWindow.Width / 2, patternRoiInWindow.Top)
                    , new Point(patternRoiInWindow.Right - patternRoiInWindow.Width / 2, patternRoiInWindow.Bottom), color, 1);
                CvInvoke.Line(image, new Point(patternRoiInWindow.Left, patternRoiInWindow.Top)
                    , new Point(patternRoiInWindow.Right, patternRoiInWindow.Bottom), color, 1);
                CvInvoke.Line(image, new Point(patternRoiInWindow.Left, patternRoiInWindow.Bottom)
                    , new Point(patternRoiInWindow.Right, patternRoiInWindow.Top), color, 1);

                image = DrawScale(image, new Point(patternRoiInWindow.Left, patternRoiInWindow.Top), patternRoiInWindow.Width, patternRoiInWindow.Width / 30, 11, color, true);
                image = DrawScale(image, new Point(patternRoiInWindow.Left, patternRoiInWindow.Bottom), patternRoiInWindow.Width, patternRoiInWindow.Width / 30, 11, color, true);
                image = DrawScale(image, new Point(patternRoiInWindow.Left, patternRoiInWindow.Top), patternRoiInWindow.Height, patternRoiInWindow.Height / 30, 11, color, false);
                image = DrawScale(image, new Point(patternRoiInWindow.Right, patternRoiInWindow.Top), patternRoiInWindow.Height, patternRoiInWindow.Height / 30, 11, color, false);

            }
            return image;
        }

        private UMat DrawScale(UMat image, Point startPt, int sideLength, int ScaleLength, int quantity, Emgu.CV.Structure.MCvScalar color, bool IsHorizontalLine)
        {

            quantity++;
            if (IsHorizontalLine)
            {
                for (int i = 0; i < quantity + 1; i++)
                {
                    if (i % 3 != 0)
                        CvInvoke.Line(image, new Point(startPt.X + sideLength * i / quantity, startPt.Y - ScaleLength),
                            new Point(startPt.X + sideLength * i / quantity, startPt.Y + ScaleLength), color, 1);
                    else
                        CvInvoke.Line(image, new Point(startPt.X + sideLength * i / quantity, (int)(startPt.Y - ScaleLength * 1.25)),
                            new Point(startPt.X + sideLength * i / quantity, (int)(startPt.Y + ScaleLength * 1.25)), color, 2);
                }
            }
            if (!IsHorizontalLine)
            {
                for (int i = 0; i < quantity + 1; i++)
                {
                    if (i % 3 != 0)
                        CvInvoke.Line(image, new Point(startPt.X - ScaleLength, startPt.Y + sideLength * i / quantity),
                            new Point(startPt.X + ScaleLength, startPt.Y + sideLength * i / quantity), color, 1);
                    else
                        CvInvoke.Line(image, new Point((int)(startPt.X - ScaleLength * 1.25), startPt.Y + sideLength * i / quantity),
                            new Point((int)(startPt.X + ScaleLength * 1.25), startPt.Y + sideLength * i / quantity), color, 2);
                }
            }
            return image;
        }
    }
}
