using MRLibrary;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;

namespace NSAA_16Axis
{
    public partial class UCGrayPaintMask : UserControl
    {
        private BufferedGraphicsContext currentContext = null;

        private BufferedGraphics myBuffer = null;

        private readonly MaskEditImageZoomer ImgZoomer = new MaskEditImageZoomer();

        private bool IsMouseDown = false;

        private Point PtMouseDown;

        private Bitmap BkImage;

        private Bitmap CoverLayer;

        private Cursor UserCursor;

        private UCGrayPaintMask.EnumTool enumTool;

        private int iPenSize;

        private int iLineThickness;

        private Bitmap SaveLayer;

        private PointF PointStart;

        private bool LeftButtonDown;

        public int LineThickness
        {
            get
            {
                return iLineThickness;
            }
            set
            {
                iLineThickness = value;
                UpdateCursor();
            }
        }

        public PointF Origin
        {
            get
            {
                return ImgZoomer.Location;
            }
        }

        public Color PaintColor
        {
            get;
            set;
        }

        public int PenSize
        {
            get
            {
                return iPenSize;
            }
            set
            {
                iPenSize = value;
                UpdateCursor();
            }
        }

        public UCGrayPaintMask.EnumTool Tool
        {
            get
            {
                return enumTool;
            }
            set
            {
                enumTool = value;
                UpdateCursor();
            }
        }

        public float ZoomFactor
        {
            get
            {
                return ImgZoomer.CurrentRatio;
            }
        }
        public UCGrayPaintMask()
        {
            InitializeComponent();
            DoubleBuffered = true;
            PenSize = 20;
            LineThickness = 10;
            Tool = EnumTool.Pen;
            PaintColor = Color.FromArgb(128, 255, 0, 255);
        }

        public RectangleF BoundingRect(PointF center, PointF p)
        {
            RectangleF x = new RectangleF();
            int num = (int)Math.Abs(center.X - p.X);
            int num1 = (int)Math.Abs(center.Y - p.Y);
            x.X = center.X - (float)num;
            x.Y = center.Y - (float)num1;
            x.Width = (float)(num * 2);
            x.Height = (float)(num1 * 2);
            return x;
        }

        private void CircleMove(Point p, bool leftDown)
        {
            CoverLayer = new Bitmap(SaveLayer);
            using (Graphics graphic = Graphics.FromImage(CoverLayer))
            {
                graphic.InterpolationMode = InterpolationMode.NearestNeighbor;
                graphic.CompositingMode = CompositingMode.SourceCopy;
                SolidBrush solidBrush = new SolidBrush((LeftButtonDown ? PaintColor : Color.FromArgb(0, 0, 0, 0)));
                PointF @virtual = DeviceToVirtual(p);
                RectangleF rectangleF = BoundingRect(PointStart, @virtual);
                graphic.FillEllipse(solidBrush, Normalize(rectangleF));
                solidBrush.Dispose();
            }
            Render();
        }

        private void CirclePenDown(Point p, bool leftDown)
        {
            PointF @virtual = DeviceToVirtual(p);
            Rectangle rectangle = new Rectangle((int)@virtual.X - PenSize / 2, (int)@virtual.Y - PenSize / 2, PenSize, PenSize);
            using (Graphics graphic = Graphics.FromImage(CoverLayer))
            {
                graphic.InterpolationMode = InterpolationMode.NearestNeighbor;
                graphic.CompositingMode = CompositingMode.SourceCopy;
                SolidBrush solidBrush = new SolidBrush((LeftButtonDown ? PaintColor : Color.FromArgb(0, 0, 0, 0)));
                graphic.FillEllipse(solidBrush, rectangle);
                solidBrush.Dispose();
            }
            Render();
        }

        private void CirclePenMove(Point p, bool leftDown)
        {
            CirclePenDown(p, leftDown);
        }

        private Cursor crossCursor(Pen pen, Brush brush, string name, int w, int h)
        {
            Bitmap bitmap = new Bitmap(w, h);
            Graphics graphic = Graphics.FromImage(bitmap);
            GraphicsPath graphicsPath = new GraphicsPath();
            GraphicsPath graphicsPath1 = new GraphicsPath();
            graphicsPath.AddLine(0, h / 2, w, h / 2);
            graphicsPath1.AddLine(w / 2, 0, w / 2, h);
            graphic.DrawPath(pen, graphicsPath);
            graphic.DrawPath(pen, graphicsPath1);
            graphic.DrawString(name, Font, brush, (float)(w / 2 + 5), (float)(h - 35));
            IntPtr hicon = bitmap.GetHicon();
            bitmap.Dispose();
            return new Cursor(hicon);
        }

        private PointF DeviceToVirtual(Point p)
        {
            PointF pointF = new PointF((float)p.X, (float)p.Y);
            float x = pointF.X;
            PointF location = ImgZoomer.Location;
            pointF.X = (x - location.X) / ImgZoomer.CurrentRatio;
            float y = pointF.Y;
            location = ImgZoomer.Location;
            pointF.Y = (y - location.Y) / ImgZoomer.CurrentRatio;
            return pointF;
        }

        public GrayImage GetMask()
        {
            GrayImage grayImage = new GrayImage(CoverLayer);
            for (int i = 0; i < (int)grayImage.Bits.Length; i++)
            {
                if (grayImage.Bits[i] == 0)
                {
                    grayImage.Bits[i] = 255;
                }
                else
                {
                    grayImage.Bits[i] = 0;
                }
            }
            return grayImage;
        }

        private Cursor LineCursor(int w, int h)
        {
            if (w < 0)
            {
                w = 1;
            }
            if (h < 0)
            {
                h = 1;
            }
            int num = w / 4;
            int num1 = h / 4;
            if (num < 3)
            {
                num = 3;
            }
            if (num1 < 3)
            {
                num1 = 3;
            }
            Bitmap bitmap = new Bitmap(w + num * 2, h + num1 * 2);
            Graphics graphic = Graphics.FromImage(bitmap);
            Pen pen = new Pen(Color.Cyan, 1f);
            graphic.DrawRectangle(pen, new Rectangle(num, num1, w, h));
            graphic.DrawLine(pen, new PointF((float)w / 2f + (float)num, 0f), new PointF((float)w / 2f + (float)num, (float)(h + 2 * num1)));
            graphic.DrawLine(pen, new PointF(0f, (float)h / 2f + (float)num1), new PointF((float)(w + 2 * num), (float)h / 2f + (float)num1));
            pen.Dispose();
            return new Cursor(bitmap.GetHicon());
        }

        private void LineMove(Point p, bool leftDown)
        {
            CoverLayer = new Bitmap(SaveLayer);
            using (Graphics graphic = Graphics.FromImage(CoverLayer))
            {
                graphic.InterpolationMode = InterpolationMode.NearestNeighbor;
                graphic.CompositingMode = CompositingMode.SourceCopy;
                Color color = (LeftButtonDown ? PaintColor : Color.FromArgb(0, 0, 0, 0));
                PointF @virtual = DeviceToVirtual(p);
                Pen pen = new Pen(color, (float)LineThickness);
                graphic.DrawLine(pen, PointStart, @virtual);
                pen.Dispose();
            }
            Render();
        }

        private void MoveImage(Point p)
        {
            Point point = new Point(p.X - PtMouseDown.X, p.Y - PtMouseDown.Y);
            PtMouseDown = p;
            ImgZoomer.Drag(point.X, point.Y);
            Render();
        }

        private RectangleF Normalize(RectangleF rect)
        {
            RectangleF x = new RectangleF();
            x = rect;
            if (rect.Width < 0f)
            {
                x.X = rect.X + rect.Width;
                x.Width = -rect.Width;
            }
            if (rect.Height < 0f)
            {
                x.Y = rect.Y + rect.Height;
                x.Height = -rect.Height;
            }
            return x;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            IsMouseDown = true;
            SaveLayer = new Bitmap(CoverLayer);
            PtMouseDown = e.Location;
            PointStart = DeviceToVirtual(e.Location);
            LeftButtonDown = (Control.MouseButtons & MouseButtons.Left) != MouseButtons.None;
            if (Tool == EnumTool.Pen)
            {
                PenDown(e.Location, LeftButtonDown);
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            if (!Focused)
            {
                base.Focus();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (Focused)
            {
                base.Parent.Focus();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (IsMouseDown)
            {
                bool mouseButtons = (Control.MouseButtons & MouseButtons.Left) != MouseButtons.None;
                switch (Tool)
                {
                    case EnumTool.CirclePen:
                        {
                            CirclePenMove(e.Location, mouseButtons);
                            break;
                        }
                    case EnumTool.Pen:
                        {
                            PenMove(e.Location, mouseButtons);
                            break;
                        }
                    case EnumTool.Line:
                        {
                            LineMove(e.Location, mouseButtons);
                            break;
                        }
                    case EnumTool.Circle:
                        {
                            CircleMove(e.Location, mouseButtons);
                            break;
                        }
                    case EnumTool.Rectangle:
                        {
                            RectangleMove(e.Location, mouseButtons);
                            break;
                        }
                    case EnumTool.DiamonPen:
                        {
                            DiamonPenMove(e.Location, mouseButtons);
                        }
                        break;
                    case EnumTool.Diamon:
                        {
                            DiamonMove(e.Location, mouseButtons);
                            break;
                        }
                    case EnumTool.Move:
                        {
                            MoveImage(e.Location);
                            break;
                        }
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (IsMouseDown)
            {
                IsMouseDown = false;
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            myBuffer?.Render();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            currentContext = BufferedGraphicsManager.Current;
            myBuffer?.Dispose();
            myBuffer = currentContext.Allocate(base.CreateGraphics(), DisplayRectangle);
        }

        private void PenDown(Point p, bool leftDown)
        {
            PointF @virtual = DeviceToVirtual(p);
            Rectangle rectangle = new Rectangle((int)Math.Round((double)@virtual.X - (double)PenSize / 2), (int)Math.Round((double)@virtual.Y - (double)PenSize / 2), PenSize, PenSize);
            using (Graphics graphic = Graphics.FromImage(CoverLayer))
            {
                graphic.InterpolationMode = InterpolationMode.NearestNeighbor;
                graphic.CompositingMode = CompositingMode.SourceCopy;
                SolidBrush solidBrush = new SolidBrush((LeftButtonDown ? PaintColor : Color.FromArgb(0, 0, 0, 0)));
                graphic.FillRectangle(solidBrush, rectangle);
                solidBrush.Dispose();
            }
            Render();
        }

        private void PenMove(Point p, bool leftDown)
        {
            PenDown(p, leftDown);
        }

        private void DiamonPenMove(Point p, bool leftDown)
        {
            DiamonPenDown(p, leftDown);
        }

        private void DiamonPenDown(Point p, bool leftDown)
        {
            PointF @virtual = DeviceToVirtual(p);
            Point[] diamonPoints = {
                new Point((int)(@virtual.X), (int)(@virtual.Y - PenSize / 2)),
                new Point((int)(@virtual.X + PenSize/2), (int)(@virtual.Y)),
                new Point((int)@virtual.X, (int)(@virtual.Y + PenSize/2)),
                new Point((int)(@virtual.X - PenSize / 2), (int)(@virtual.Y))
            };

            using (Graphics graphic = Graphics.FromImage(CoverLayer))
            {
                graphic.InterpolationMode = InterpolationMode.NearestNeighbor;
                graphic.CompositingMode = CompositingMode.SourceCopy;
                SolidBrush solidBrush = new SolidBrush((leftDown ? PaintColor : Color.FromArgb(0, 0, 0, 0)));
                graphic.FillPolygon(solidBrush, diamonPoints);
                solidBrush.Dispose();
            }
            Render();
        }

        private void RectangleMove(Point p, bool leftDown)
        {
            CoverLayer = new Bitmap(SaveLayer);
            using (Graphics graphic = Graphics.FromImage(CoverLayer))
            {
                graphic.InterpolationMode = InterpolationMode.NearestNeighbor;
                graphic.CompositingMode = CompositingMode.SourceCopy;
                Color color = (LeftButtonDown ? PaintColor : Color.FromArgb(0, 0, 0, 0));
                PointF @virtual = DeviceToVirtual(p);
                SolidBrush solidBrush = new SolidBrush(color);
                RectangleF rectangleF = new RectangleF(PointStart, new SizeF(@virtual.X - PointStart.X, @virtual.Y - PointStart.Y));
                rectangleF = Normalize(rectangleF);
                graphic.FillRectangle(solidBrush, rectangleF);
                solidBrush.Dispose();
            }
            Render();
        }

        private Cursor RectCursor(int w, int h)
        {
            if (w < 1)
            {
                w = 1;
            }
            if (h < 1)
            {
                h = 1;
            }
            Bitmap bitmap = new Bitmap(w, h);
            Graphics graphic = Graphics.FromImage(bitmap);
            Pen pen = new Pen(Color.Cyan, 1f);
            graphic.DrawRectangle(pen, new Rectangle(0, 0, w - 1, h - 1));
            pen.Dispose();
            IntPtr hicon = bitmap.GetHicon();
            bitmap.Dispose();
            return new Cursor(hicon);
        }

        private Cursor DiamonCursor(int w, int h)
        {
            if (w < 1)
            {
                w = 1;
            }
            if (h < 1)
            {
                h = 1;
            }

            Bitmap bitmap = new Bitmap((int)w, (int)h);
            Graphics graphic = Graphics.FromImage(bitmap);
            Pen pen = new Pen(Color.Cyan, 1f);
            Point[] diamonPoints = {
                new Point((int)w/2, 0),
                new Point((int)w, (int)h/2),
                new Point((int)(w/2), h),
                new Point(0, (int)h/2)
            };
            graphic.DrawPolygon(pen, diamonPoints);
            pen.Dispose();
            IntPtr hicon = bitmap.GetHicon();
            bitmap.Dispose();
            return new Cursor(hicon);
        }

        private void Render()
        {
            if (BkImage != null)
            {
                if (CoverLayer != null)
                {
                    Graphics graphics = myBuffer.Graphics;
                    graphics.Clear(BackColor);
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                    graphics.PageUnit = GraphicsUnit.Pixel;
                    ImgZoomer.Draw(graphics);
                    ImgZoomer.DrawMask(graphics, CoverLayer);
                    ExtraPaint?.Invoke(graphics, ImgZoomer.Location);
                    graphics.ResetTransform();
                    myBuffer.Render();
                }
            }
        }

        public void SetBackImage(GrayImage img, GrayImage mask)
        {
            BkImage = new Bitmap(img.Width, img.Height);
            using (Graphics graphic = Graphics.FromImage(BkImage))
            {
                Bitmap bitmap = img.ToBitmap();
                graphic.DrawImageUnscaled(bitmap, 0, 0);
                bitmap.Dispose();
            }
            CoverLayer = new Bitmap(mask.Width, mask.Height);
            using (Graphics graphic1 = Graphics.FromImage(CoverLayer))
            {
                Bitmap maskBitmap32 = mask.ToMaskBitmap32();
                graphic1.DrawImageUnscaled(maskBitmap32, 0, 0);
                maskBitmap32.Dispose();
            }
            SaveLayer = new Bitmap(CoverLayer);
            ImgZoomer.UpdateImage(BkImage, base.ClientRectangle);
            UpdateCursor();
            Render();
        }

        public void Undo()
        {
            CoverLayer = SaveLayer;
            Render();
        }

        private void DiamonMove(Point p, bool leftDown)
        {
            CoverLayer = new Bitmap(SaveLayer);
            using (Graphics graphic = Graphics.FromImage(CoverLayer))
            {
                graphic.InterpolationMode = InterpolationMode.NearestNeighbor;
                graphic.CompositingMode = CompositingMode.SourceCopy;
                Color color = (LeftButtonDown ? PaintColor : Color.FromArgb(0, 0, 0, 0));
                SolidBrush solidBrush = new SolidBrush(color);
                PointF @virtual = DeviceToVirtual(p);
                Point[] diamonPoints = {
                    new Point((int)(PointStart.X), (int)(PointStart.Y + (@virtual.Y - PointStart.Y) / 2)),
                    new Point((int)(PointStart.X + (@virtual.X - PointStart.X)/2), (int)(@virtual.Y)),
                    new Point((int)@virtual.X, (int)(PointStart.Y + (@virtual.Y - PointStart.Y) / 2)),
                    new Point((int)(PointStart.X + (@virtual.X - PointStart.X)/2), (int)(PointStart.Y))
                };
                graphic.FillPolygon(solidBrush, diamonPoints);
                solidBrush.Dispose();
            }
            Render();
        }


        private void UpdateCursor()
        {
            switch (enumTool)
            {
                case EnumTool.CirclePen:
                case EnumTool.Pen:
                    {
                        Cursor = Cursors.Cross;
                        UserCursor?.Dispose();
                        UserCursor = RectCursor((int)((float)PenSize * ImgZoomer.CurrentRatio), (int)((float)PenSize * ImgZoomer.CurrentRatio));
                        Cursor = UserCursor;
                        break;
                    }
                case EnumTool.Line:
                    {
                        Cursor = Cursors.Cross;
                        UserCursor?.Dispose();
                        UserCursor = LineCursor((int)((float)LineThickness * ImgZoomer.CurrentRatio), (int)((float)LineThickness * ImgZoomer.CurrentRatio));
                        Cursor = UserCursor;
                        break;
                    }
                case EnumTool.Circle:
                case EnumTool.Rectangle:
                case EnumTool.Diamon:
                    {
                        Cursor = Cursors.Cross;
                        break;
                    }
                case EnumTool.DiamonPen:
                    UserCursor?.Dispose();
                    UserCursor = DiamonCursor((int)((float)PenSize * ImgZoomer.CurrentRatio), (int)((float)PenSize * ImgZoomer.CurrentRatio));
                    Cursor = UserCursor;
                    break;
                case EnumTool.Move:
                    {
                        Cursor = Cursors.Hand;
                        break;
                    }
            }
        }

        public event UCGrayPaintMask.ExtraPaitEvent ExtraPaint;

        public enum EnumTool
        {
            CirclePen,
            Pen,
            Line,
            Circle,
            Rectangle,
            DiamonPen,
            Diamon,
            Move
        }

        public delegate void ExtraPaitEvent(Graphics g, PointF imageZoomerLocation);
    }
}
