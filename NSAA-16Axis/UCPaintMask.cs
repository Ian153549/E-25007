using MRLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace NSAA_16Axis
{
    public partial class UCPaintMask : UserControl
    {
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Cursor Cursor
        {
            get
            {
                return base.Cursor;
            }
            set
            {
                base.Cursor = value;
            }
        }

        private BufferedGraphicsContext currentContext = null;

        private BufferedGraphics myBuffer = null;

        private readonly MaskEditImageZoomer ImgZoomer = new MaskEditImageZoomer();

        private bool IsMouseDown = false;

        private System.Drawing.Point PtMouseDown;

        private Bitmap BkImage;

        private Bitmap CoverLayer;

        private Bitmap KeyLayer;

        private Bitmap DrawLayer;

        private Cursor UserCursor;

        private EnumTool enumTool;

        private int iPenSize;

        private int iLineThickness;

        private Bitmap SaveLayer;

        private Bitmap SaveKeyLayer;

        private PointF PointStart;

        private bool LeftButtonDown;

        private int PaintLayer;
        private List<Rectangle> drawnRectangles= new List<Rectangle>();
        private Rectangle currentRectangle= Rectangle.Empty;
        private System.Drawing.Point rectangleStartPoint= System.Drawing.Point.Empty;
        private bool isDrawingRectangle= false;

        public List<Rectangle> GetDrawnRectangles()
        {
            return new List<Rectangle>(drawnRectangles);
        }
        public void ClearDrawnRectangles()
        {
            drawnRectangles.Clear();
            currentRectangle = Rectangle.Empty;
            rectangleStartPoint = System.Drawing.Point.Empty;
            isDrawingRectangle = false;
            Invalidate();
        }
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

        public Color PaintColorM
        {
            get;
            set;
        }

        public Color PaintColorK
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

        public EnumTool Tool
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

        public UCPaintMask()
        {
            InitializeComponent();
            DoubleBuffered = true;
            PenSize = 20;
            LineThickness = 10;
            Tool = EnumTool.Pen;
            PaintColorM = Color.FromArgb(128, 255, 0, 255);
            PaintColorK = Color.FromArgb(128, 255, 255, 255);
            PaintColor = PaintColorM;
            PaintLayer = 0;
        }

        public void SetPaintLayer(int Layer)
        {
            PaintLayer = Layer;
            if (PaintLayer == 0)
            {
                PaintColor = PaintColorM;
            } else if (PaintLayer == 1)
            {
                PaintColor = PaintColorK;
            } else if (PaintLayer == 2)
            {
                // PaintColor = Color.FromArgb(128, 0, 0, 0);
                Cursor = Cursors.Default;
            }
            else
            {
                PaintColor = PaintColorM;
            }
        }

        public void SetPaintColor(int colorKey)
        { 
            if (colorKey == 0)
            {
                PaintColor = PaintColorM;
            }
            else if (colorKey == 1)
            {
                PaintColor = PaintColorK;
            }
            else
            {
                PaintColor = PaintColorM;
            }
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

        private void CircleMove(System.Drawing.Point p, bool leftDown)
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

        private void CirclePenDown(System.Drawing.Point p, bool leftDown)
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

        private void CirclePenMove(System.Drawing.Point p, bool leftDown)
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

        private PointF DeviceToVirtual(System.Drawing.Point p)
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

        public GrayImage GetKey()
        {
            GrayImage grayImage = new GrayImage(KeyLayer);
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

        private void LineMove(System.Drawing.Point p, bool leftDown)
        {
            if (PaintLayer == 0)
            {
                DrawLayer = new Bitmap(SaveLayer);
            } else if (PaintLayer == 1)
            {
                DrawLayer = new Bitmap(SaveKeyLayer);
            }
            else
            {
                // CoverLayer = new Bitmap(SaveLayer);
            }
            // CoverLayer = new Bitmap(SaveLayer);
            using (Graphics graphic = Graphics.FromImage(DrawLayer))
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

        private void MoveImage(System.Drawing.Point p)
        {
            System.Drawing.Point point = new System.Drawing.Point(p.X - PtMouseDown.X, p.Y - PtMouseDown.Y);
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
            if(Tool==EnumTool.Rectangle)
            {
                isDrawingRectangle = true;
                rectangleStartPoint = e.Location;
                currentRectangle = Rectangle.Empty;
                return;
            }
            if (PaintLayer == 0)
            {
                SaveLayer = new Bitmap(CoverLayer);
            }
            else if (PaintLayer == 1)
            {
                SaveKeyLayer = new Bitmap(KeyLayer);
            }
            else
            {
                // CoverLayer = new Bitmap(SaveLayer);
            }
            // SaveLayer = new Bitmap(CoverLayer);
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
                if(Tool==EnumTool.Rectangle && isDrawingRectangle)
                {
                    int x = Math.Min(rectangleStartPoint.X, e.X);
                    int y = Math.Min(rectangleStartPoint.Y, e.Y);
                    int width = Math.Abs(e.X - rectangleStartPoint.X);
                    int height = Math.Abs(e.Y - rectangleStartPoint.Y);

                    currentRectangle = new Rectangle(x, y, width, height);
                    Invalidate(); // 觸發重繪
                    return;
                }
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
                if (Tool == EnumTool.Rectangle && isDrawingRectangle)
                {
                    isDrawingRectangle = false;

                    // 如果矩形有效（寬度和高度都大於 10 像素），則加入列表
                    if (currentRectangle.Width > 10 && currentRectangle.Height > 10)
                    {
                        // ✅ 轉換為虛擬座標（圖像座標）
                        PointF topLeft = DeviceToVirtual(new System.Drawing.Point(currentRectangle.X, currentRectangle.Y));
                        PointF bottomRight = DeviceToVirtual(new System.Drawing.Point(
                            currentRectangle.Right,
                            currentRectangle.Bottom));

                        Rectangle virtualRect = new Rectangle(
                            (int)topLeft.X,
                            (int)topLeft.Y,
                            (int)(bottomRight.X - topLeft.X),
                            (int)(bottomRight.Y - topLeft.Y)
                        );

                        drawnRectangles.Add(virtualRect);
                        Invalidate(); // 重繪以顯示新矩形
                    }

                    currentRectangle = Rectangle.Empty;
                    rectangleStartPoint = System.Drawing.Point.Empty;
                    return; // 不執行原有的繪製邏輯
                }
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
            if ( (drawnRectangles.Count > 0 || isDrawingRectangle))
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                // 繪製已完成的矩形
                using (Pen pen = new Pen(Color.Lime, 2))
                {
                    foreach (Rectangle rect in drawnRectangles)
                    {
                        // ✅ 轉換為設備座標
                        Rectangle deviceRect = VirtualToDeviceRect(rect);
                        g.DrawRectangle(pen, deviceRect);

                        // 繪製矩形編號
                        using (Font font = new Font("Arial", 10, FontStyle.Bold))
                        using (SolidBrush brush = new SolidBrush(Color.Lime))
                        {
                            int index = drawnRectangles.IndexOf(rect) + 1;
                            g.DrawString($"ROI-{index}", font, brush,
                                new PointF(deviceRect.X + 5, deviceRect.Y + 5));
                        }
                    }
                }

                // 繪製正在框選的矩形
                if (isDrawingRectangle && !currentRectangle.IsEmpty)
                {
                    using (Pen pen = new Pen(Color.Yellow, 2))
                    {
                        pen.DashStyle = DashStyle.Dash;
                        g.DrawRectangle(pen, currentRectangle);
                    }
                }
            }
        }
        private Rectangle VirtualToDeviceRect(Rectangle virtualRect)
        {
            PointF topLeft = new PointF(
                virtualRect.X * ImgZoomer.CurrentRatio + ImgZoomer.Location.X,
                virtualRect.Y * ImgZoomer.CurrentRatio + ImgZoomer.Location.Y
            );

            PointF bottomRight = new PointF(
                (virtualRect.X + virtualRect.Width) * ImgZoomer.CurrentRatio + ImgZoomer.Location.X,
                (virtualRect.Y + virtualRect.Height) * ImgZoomer.CurrentRatio + ImgZoomer.Location.Y
            );

            return new Rectangle(
                (int)topLeft.X,
                (int)topLeft.Y,
                (int)(bottomRight.X - topLeft.X),
                (int)(bottomRight.Y - topLeft.Y)
            );
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            currentContext = BufferedGraphicsManager.Current;
            myBuffer?.Dispose();
            myBuffer = currentContext.Allocate(base.CreateGraphics(), DisplayRectangle);
        }

        private void PenDown(System.Drawing.Point p, bool leftDown)
        {
            PointF @virtual = DeviceToVirtual(p);
            Rectangle rectangle = new Rectangle((int)Math.Round((double)@virtual.X - (double)PenSize / 2), (int)Math.Round((double)@virtual.Y - (double)PenSize / 2), PenSize, PenSize);
            if (PaintLayer == 0)
            {
                DrawLayer = new Bitmap(SaveLayer);
            }
            else if (PaintLayer == 1)
            {
                DrawLayer = new Bitmap(SaveKeyLayer);
            }
            else
            {
                // CoverLayer = new Bitmap(SaveLayer);
            }
            using (Graphics graphic = Graphics.FromImage(DrawLayer))
            {
                graphic.InterpolationMode = InterpolationMode.NearestNeighbor;
                graphic.CompositingMode = CompositingMode.SourceCopy;
                SolidBrush solidBrush = new SolidBrush((LeftButtonDown ? PaintColor : Color.FromArgb(0, 0, 0, 0)));
                graphic.FillRectangle(solidBrush, rectangle);
                solidBrush.Dispose();
            }
            Render();
        }

        private void PenMove(System.Drawing.Point p, bool leftDown)
        {
            PenDown(p, leftDown);
        }

        private void DiamonPenMove(System.Drawing.Point p, bool leftDown)
        {
            DiamonPenDown(p, leftDown);
        }

        private void DiamonPenDown(System.Drawing.Point p, bool leftDown)
        {
            PointF @virtual = DeviceToVirtual(p);
            System.Drawing.Point[] diamonPoints = {
                new System.Drawing.Point((int)(@virtual.X), (int)(@virtual.Y - PenSize / 2)),
                new System.Drawing.Point((int)(@virtual.X + PenSize/2), (int)(@virtual.Y)),
                new System.Drawing.Point((int)@virtual.X, (int)(@virtual.Y + PenSize/2)),
                new System.Drawing.Point((int)(@virtual.X - PenSize / 2), (int)(@virtual.Y))
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

        private void RectangleMove(System.Drawing.Point p, bool leftDown)
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
            System.Drawing.Point[] diamonPoints = {
                new System.Drawing.Point((int)w/2, 0),
                new System.Drawing.Point((int)w, (int)h/2),
                new System.Drawing.Point((int)(w/2), h),
                new System.Drawing.Point(0, (int)h/2)
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
                if ((CoverLayer != null) || (KeyLayer != null))
                {
                    Graphics graphics = myBuffer.Graphics;
                    graphics.Clear(BackColor);
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                    graphics.PageUnit = GraphicsUnit.Pixel;
                    ImgZoomer.Draw(graphics);
                    if (CoverLayer != null) ImgZoomer.DrawMask(graphics, CoverLayer);
                    if (KeyLayer != null) ImgZoomer.DrawKey(graphics, KeyLayer);
                    ExtraPaint?.Invoke(graphics, ImgZoomer.Location);
                    graphics.ResetTransform();
                    myBuffer.Render();
                    
                }
            }
        }

        public void SetBackImage(Mat img, GrayImage imgK, GrayImage mask)
        {
            BkImage = new Bitmap(img.Width, img.Height);
            using (Graphics graphic = Graphics.FromImage(BkImage))
            {
                Bitmap bitmap = img.Clone().ToBitmap();
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
            KeyLayer = new Bitmap(imgK.Width, imgK.Height);
            using (Graphics graphic2 = Graphics.FromImage(KeyLayer))
            {
                Bitmap keyBitmap32 = imgK.ToMaskBitmap32();
                graphic2.DrawImageUnscaled(keyBitmap32, 0, 0);
                keyBitmap32.Dispose();
            }
            SaveLayer = new Bitmap(CoverLayer);
            SaveKeyLayer = new Bitmap(KeyLayer);
            ImgZoomer.UpdateImage(BkImage, base.ClientRectangle);
            UpdateCursor();
            Render();
        }

        public void Undo()
        {
            CoverLayer = SaveLayer;
            Render();
        }

        private void DiamonMove(System.Drawing.Point p, bool leftDown)
        {
            CoverLayer = new Bitmap(SaveLayer);
            using (Graphics graphic = Graphics.FromImage(CoverLayer))
            {
                graphic.InterpolationMode = InterpolationMode.NearestNeighbor;
                graphic.CompositingMode = CompositingMode.SourceCopy;
                Color color = (LeftButtonDown ? PaintColor : Color.FromArgb(0, 0, 0, 0));
                SolidBrush solidBrush = new SolidBrush(color);
                PointF @virtual = DeviceToVirtual(p);
                System.Drawing.Point[] diamonPoints = {
                    new System.Drawing.Point((int)(PointStart.X), (int)(PointStart.Y + (@virtual.Y - PointStart.Y) / 2)),
                    new System.Drawing.Point((int)(PointStart.X + (@virtual.X - PointStart.X)/2), (int)(@virtual.Y)),
                    new System.Drawing.Point((int)@virtual.X, (int)(PointStart.Y + (@virtual.Y - PointStart.Y) / 2)),
                    new System.Drawing.Point((int)(PointStart.X + (@virtual.X - PointStart.X)/2), (int)(PointStart.Y))
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
