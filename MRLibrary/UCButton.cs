using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Threading;

namespace MRLibrary
{
    [DefaultEvent("Click")]
    public partial class UCButton : UserControl
    {
        private readonly SynchronizationContext SyncContext;

        Color faceColor;
        int radius = 5;
        Color inverseColor;
        Color disabledColor;
        protected bool IsDown = false;
        string caption;
        Bitmap image;

        public UCButton()
        {
            InitializeComponent();
            SyncContext = SynchronizationContext.Current;
            DoubleBuffered = true;
        }

        public new bool Enabled
        {
            get
            {
                return base.Enabled;
            }
            set
            {
                base.Enabled = value;
                Invalidate();
            }
        }

        public Color FaceColor
        {
            get
            {
                return faceColor;
            }
            set
            {
                faceColor = value;
                inverseColor = InverseColor(faceColor); 
                disabledColor = Color.LightGray;
                Invalidate();
            }
        }

        public string Caption
        {
            get
            {
                return caption;
            }
            set
            {
                caption = value;
                Invalidate();
            }
        }

        
        public Bitmap Image
        {
            get
            {
                return image;
            }
            set
            {
                image = value;
                Invalidate();
            }
        }

        public int Radius
        {
            get
            {
                return radius;
            }
            set
            {
                radius = value;
                Invalidate();
            }
        }

        
        Color Brighter(Color c, double gamma)
        {
            byte r = (byte)(Math.Pow(c.R / 255.0, 1 / gamma) * 255);
            byte g = (byte)(Math.Pow(c.G / 255.0, 1 / gamma) * 255);
            byte b = (byte)(Math.Pow(c.B / 255.0, 1 / gamma) * 255);
            return Color.FromArgb(r, g, b);
        }

        Color Brighter(Color c, int add)
        {
            int r = c.R + add;
            int g = c.G + add;
            int b = c.B + add;
            if (r > 255) r = 255;
            if (g > 255) g = 255;
            if (b > 255) b = 255;
            if (r < 0) r = 0;
            if (g < 0) g = 0;
            if (b < 0) b = 0;

            return Color.FromArgb(r, g, b);
        }

        protected Color InverseColor(Color c)
        {
            return Color.FromArgb(255 - c.R, 255 - c.G, 255 - c.B);
        }

        bool MouseInClient = false;
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            MouseInClient = true;
            Refresh();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            MouseInClient = false;
            Refresh();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            Color fColor = faceColor;
            if (!Enabled)
                fColor = disabledColor;
            else if (IsDown)
            {
                fColor = inverseColor;
            }
            else if (MouseInClient)
            {
                fColor = Color.LightSteelBlue;
            }

            //ExtendedGraphics eg = new ExtendedGraphics(e.Graphics);
            //using (SolidBrush br = new SolidBrush(fColor))
            //{
            //    eg.FillRoundRect(br, 0, 0, Width, Height, Radius);
            //}


            GraphicsPath path = RoundedRectanglePath.Create(0, 0, Width - 1, Height - 1, Radius);
            using (SolidBrush br = new SolidBrush(Brighter(fColor, -30)))
            {
                e.Graphics.DrawPath(Pens.Gray, path);
            }

            Rectangle cr = ClientRectangle;
            cr.Inflate(-1, -1);
            if (Enabled)
            {
                //RRectRegion rr = new RRectRegion(cr, Radius);
                LinearGradientBrush linGrBrush = new LinearGradientBrush(new Point(0, 0), new Point(0, Height),
                        Brighter(fColor, 45), Brighter(fColor, 0));

                //e.Graphics.FillRegion(linGrBrush, rr.Rgn);
                e.Graphics.FillPath(linGrBrush, path);

                linGrBrush.Dispose();
            }
            path.Dispose();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.PageUnit = GraphicsUnit.Pixel;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            
            SizeF sz = e.Graphics.MeasureString(Caption, Font);
            Color textColor = (IsDown) ? InverseColor(ForeColor) : ForeColor;
            if (!Enabled) textColor = Color.Gray;

            int x = 5;
            if (Image != null)
            {
                Image.SetResolution(e.Graphics.DpiX, e.Graphics.DpiY);
                int y = (Height - Image.Size.Height) / 2;
                       
                if (Caption != null && Caption.Length != 0)
                {
                    e.Graphics.DrawImageUnscaled(Image, x, y);
                    using (SolidBrush br = new SolidBrush(textColor))
                    {
                        e.Graphics.DrawString(Caption, Font, br, x + Image.Size.Width + 5, (Height - sz.Height) / 2);
                    }
                }
                else
                {
                    x = (Width - Image.Size.Width) / 2;
                    e.Graphics.DrawImageUnscaled(Image, x, y);
                }
            }
            else
            {
                using (SolidBrush br = new SolidBrush(textColor))
                {
                    e.Graphics.DrawString(Caption, Font, br, (Width - sz.Width) / 2, (Height - sz.Height) / 2);
                }
            }
        }
    }
}
