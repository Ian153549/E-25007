using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenCvSharp;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace MRLibrary
{
    class SKWindowLearnPair
    {
        enum SIDE { center, up, down, left, right, leftUp, leftDown, rightUp, rightDown, nowhere };

        public float fMin = 30f;

        public RectangleF PatternRoi;
        public RectangleF PatternOldRoi;

        private System.Drawing.Point _ptMouseDown;
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
        public void PatternRoiMouseMove(MouseEventArgs e, System.Drawing.Point aftZoomRoiLeftUpPt, int edgeWidth, int edgeHeight, float zoomRatio)
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
            float dt = 0;
            float db = 0;
            float dl = 0;
            float dr = 0;

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
                    dt = PatternOldRoi.Top + dy;
                    db = PatternOldRoi.Bottom - fMin;
                    if (dt > db) dt = db;
                    PatternRoi = RectangleF.FromLTRB(PatternOldRoi.Left, dt, PatternOldRoi.Right, PatternOldRoi.Bottom);
                    break;
                case (int)SIDE.down:
                    db = dy + PatternOldRoi.Bottom;
                    dt = PatternOldRoi.Top + fMin;
                    if (db < dt) db = dt;
                    PatternRoi = RectangleF.FromLTRB(PatternOldRoi.Left, PatternOldRoi.Top, PatternOldRoi.Right, db);
                    break;
                case (int)SIDE.left:
                    dl = PatternOldRoi.Left + dx;
                    dr = PatternOldRoi.Right - fMin;
                    if (dl > dr) dl = dr;
                    PatternRoi = RectangleF.FromLTRB(dl, PatternOldRoi.Top, PatternOldRoi.Right, PatternOldRoi.Bottom);
                    break;
                case (int)SIDE.right:
                    dr = PatternOldRoi.Right + dx;
                    dl = PatternOldRoi.Left + fMin;
                    if (dr < dl) dr = dl;
                    PatternRoi = RectangleF.FromLTRB(PatternOldRoi.Left, PatternOldRoi.Top, dr, PatternOldRoi.Bottom);
                    break;
                case (int)SIDE.leftUp:
                    dl = PatternOldRoi.Left + dx;
                    dr = PatternOldRoi.Right - fMin;
                    if (dl > dr) dl = dr;

                    dt = PatternOldRoi.Top + dy;
                    db = PatternOldRoi.Bottom - fMin;
                    if (dt > db) dt = db;

                    PatternRoi = RectangleF.FromLTRB(dl, dt, PatternOldRoi.Right, PatternOldRoi.Bottom);
                    break;
                case (int)SIDE.leftDown:
                    dl = PatternOldRoi.Left + dx;
                    dr = PatternOldRoi.Right - fMin;
                    if (dl > dr) dl = dr;

                    db = dy + PatternOldRoi.Bottom;
                    dt = PatternOldRoi.Top + fMin;
                    if (db < dt) db = dt;

                    PatternRoi = RectangleF.FromLTRB(dl, PatternOldRoi.Top, PatternOldRoi.Right, db);
                    break;
                case (int)SIDE.rightUp:
                    dr = PatternOldRoi.Right + dx;
                    dl = PatternOldRoi.Left + fMin;
                    if (dr < dl) dr = dl;

                    dt = PatternOldRoi.Top + dy;
                    db = PatternOldRoi.Bottom - fMin;
                    if (dt > db) dt = db;

                    PatternRoi = RectangleF.FromLTRB(PatternOldRoi.Left, dt, dr, PatternOldRoi.Bottom);
                    break;
                case (int)SIDE.rightDown:
                    dr = PatternOldRoi.Right + dx;
                    dl = PatternOldRoi.Left + fMin;
                    if (dr < dl) dr = dl;

                    db = dy + PatternOldRoi.Bottom;
                    dt = PatternOldRoi.Top + fMin;
                    if (db < dt) db = dt;

                    PatternRoi = RectangleF.FromLTRB(PatternOldRoi.Left, PatternOldRoi.Top, dr, db);
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

        public Mat DrawPatternRectangle(Mat image, float zoomRatio, System.Drawing.Point aftZoomRoiLeftUpPt, Scalar color)
        {
            int leavedge = 12;

            RectangleF patternRoiAftZoomF;
            Rectangle patternRoiAftZoom;
            Rect patternRoiInWindow;
            patternRoiAftZoomF = new RectangleF(PatternRoi.X * zoomRatio, PatternRoi.Y * zoomRatio,
                                PatternRoi.Width * zoomRatio, PatternRoi.Height * zoomRatio);
            patternRoiAftZoom = Rectangle.Round(patternRoiAftZoomF);
            patternRoiInWindow = new Rect(patternRoiAftZoom.X - aftZoomRoiLeftUpPt.X, patternRoiAftZoom.Y - aftZoomRoiLeftUpPt.Y
                                , patternRoiAftZoom.Width, patternRoiAftZoom.Height);

            if (new Rectangle(aftZoomRoiLeftUpPt, _ownerWindow.ClientSize).IntersectsWith(patternRoiAftZoom))
            {
                Cv2.Rectangle(image, patternRoiInWindow, color, 1);
                Cv2.Line(image, patternRoiInWindow.Left, patternRoiInWindow.Top + patternRoiInWindow.Height / 2
                    , patternRoiInWindow.Right, patternRoiInWindow.Top + patternRoiInWindow.Height / 2, color, 1);
                Cv2.Line(image, patternRoiInWindow.Left + patternRoiInWindow.Width / 2, patternRoiInWindow.Top
                    , patternRoiInWindow.Right - patternRoiInWindow.Width / 2, patternRoiInWindow.Bottom, color, 1);
                Cv2.Line(image, patternRoiInWindow.Left, patternRoiInWindow.Top
                    , patternRoiInWindow.Right, patternRoiInWindow.Bottom, color, 1);
                Cv2.Line(image, patternRoiInWindow.Left, patternRoiInWindow.Bottom
                    , patternRoiInWindow.Right, patternRoiInWindow.Top, color, 1);
                Cv2.Line(image, patternRoiInWindow.Left + patternRoiInWindow.Width / leavedge, patternRoiInWindow.Top
                     , patternRoiInWindow.Left + patternRoiInWindow.Width / leavedge, patternRoiInWindow.Bottom, color, 1);
                Cv2.Line(image, patternRoiInWindow.Right - patternRoiInWindow.Width / leavedge, patternRoiInWindow.Top
                     , patternRoiInWindow.Right - patternRoiInWindow.Width / leavedge, patternRoiInWindow.Bottom, color, 1);
                Cv2.Line(image, patternRoiInWindow.Left, patternRoiInWindow.Top + patternRoiInWindow.Height / leavedge
                    , patternRoiInWindow.Right, patternRoiInWindow.Top + patternRoiInWindow.Height / leavedge, color, 1);
                Cv2.Line(image, patternRoiInWindow.Left, patternRoiInWindow.Bottom - patternRoiInWindow.Height / leavedge
                    , patternRoiInWindow.Right, patternRoiInWindow.Bottom - patternRoiInWindow.Height / leavedge, color, 1);

                image = DrawScale(image, new OpenCvSharp.Point(patternRoiInWindow.Left, patternRoiInWindow.Top), patternRoiInWindow.Width, patternRoiInWindow.Width / 30, 11, color, true);
                image = DrawScale(image, new OpenCvSharp.Point(patternRoiInWindow.Left, patternRoiInWindow.Bottom), patternRoiInWindow.Width, patternRoiInWindow.Width / 30, 11, color, true);
                image = DrawScale(image, new OpenCvSharp.Point(patternRoiInWindow.Left, patternRoiInWindow.Top), patternRoiInWindow.Height, patternRoiInWindow.Height / 30, 11, color, false);
                image = DrawScale(image, new OpenCvSharp.Point(patternRoiInWindow.Right, patternRoiInWindow.Top), patternRoiInWindow.Height, patternRoiInWindow.Height / 30, 11, color, false);

            }
            return image;
        }

        //private UMat DrawScale(UMat image, Point startPt, int sideLength, int ScaleLength, int quantity, Emgu.CV.Structure.MCvScalar color, bool IsHorizontalLine)
        //{

        //    quantity++;
        //    if (IsHorizontalLine)
        //    {
        //        for (int i = 0; i < quantity + 1; i++)
        //        {
        //            if (i % 3 != 0)
        //                CvInvoke.Line(image, new Point(startPt.X + sideLength * i / quantity, startPt.Y - ScaleLength),
        //                    new Point(startPt.X + sideLength * i / quantity, startPt.Y + ScaleLength), color, 1);
        //            else
        //                CvInvoke.Line(image, new Point(startPt.X + sideLength * i / quantity, (int)(startPt.Y - ScaleLength * 1.25)),
        //                    new Point(startPt.X + sideLength * i / quantity, (int)(startPt.Y + ScaleLength * 1.25)), color, 2);
        //        }
        //    }
        //    if (!IsHorizontalLine)
        //    {
        //        for (int i = 0; i < quantity + 1; i++)
        //        {
        //            if (i % 3 != 0)
        //                CvInvoke.Line(image, new Point(startPt.X - ScaleLength, startPt.Y + sideLength * i / quantity),
        //                    new Point(startPt.X + ScaleLength, startPt.Y + sideLength * i / quantity), color, 1);
        //            else
        //                CvInvoke.Line(image, new Point((int)(startPt.X - ScaleLength * 1.25), startPt.Y + sideLength * i / quantity),
        //                    new Point((int)(startPt.X + ScaleLength * 1.25), startPt.Y + sideLength * i / quantity), color, 2);
        //        }
        //    }
        //    return image;
        //}
        private Mat DrawScale(Mat image, OpenCvSharp.Point startPoint, int sideLength, int scaleLength, int quantity, Scalar color, bool isHorizontalLine)
        {
            if (image.Empty()) return image;
            quantity++;
            double longScaleFactor = 1.25;

            for (int i = 0; i <= quantity; i++)
            {
                int position = sideLength * i / quantity;
                bool isLongScale = (i % 3 == 0);
                int adjustedScaleLength = isLongScale ? (int)(scaleLength * longScaleFactor) : scaleLength;

                if (isHorizontalLine)
                {
                    int x = startPoint.X + position;
                    Cv2.Line(image, x, startPoint.Y - adjustedScaleLength, x, startPoint.Y - adjustedScaleLength, color, isLongScale ? 2 : 1);
                }
                else
                {
                    int y = startPoint.Y + position;
                    Cv2.Line(image, startPoint.X - adjustedScaleLength, y, startPoint.X - adjustedScaleLength, y, color, isLongScale ? 2 : 1);
                }
            }
            return image;
        }
    }
}
