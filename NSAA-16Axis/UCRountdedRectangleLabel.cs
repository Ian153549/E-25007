using MRLibrary;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace NSAA_16Axis
{
    public partial class UCRoundedRectangleLabel : Label
    {
        int radius;
        Color faceColor;
        bool drawBorder = false;

        public UCRoundedRectangleLabel()
        {
            InitializeComponent();
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
                Update();
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
                ForeColor = ((faceColor.R * 0.114 + faceColor.G * 0.587 + faceColor.B * 0.299) < 128) ? Color.White : Color.Black;
                Invalidate();
                Update();
            }
        }

        public bool DrawBorder
        {
            get
            {
                return drawBorder;
            }
            set
            {
                drawBorder = value;
                if (drawBorder) BorderStyle = BorderStyle.None;
                Invalidate();
                Update();
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            GraphicsPath path = RoundedRectanglePath.Create(0, 0, Width - 1, Height - 1, Radius);
            if (DrawBorder) e.Graphics.DrawPath(Pens.Black, path);
            using (SolidBrush br = new SolidBrush(Enabled ? FaceColor : Color.LightGray))
            {
                e.Graphics.FillPath(br, path);
            }
        }
    }
}
