using System;
using System.Drawing;

namespace NSAA_16Axis
{
	public class MaskEditImageZoomer
	{
		private Bitmap Image;

		private Size OriginImageSize = new Size();

		private Size OriginClientSize = new Size();

		private RectangleF BaseRect;

		private RectangleF ImageRect;

		private Rectangle ClientRect;

		private float BaseRatio = 1f;

		private int RatioIndex = 0;

		private float[] Ratio = new float[8];

		private float LastZoomFactor = 1f;

		public float CurrentRatio
		{
			get
			{
				return this.BaseRatio * this.Ratio[this.RatioIndex];
			}
		}

		public PointF Location
		{
			get
			{
				return this.ImageRect.Location;
			}
		}

		public MaskEditImageZoomer()
		{
			this.Ratio[0] = 1f;
			for (int i = 1; i < (int)this.Ratio.Length; i++)
			{
				this.Ratio[i] = this.Ratio[i - 1] * 1.5f;
			}
		}

		public bool Contains(Point p)
		{
			return this.ImageRect.Contains(p);
		}

		public SizeF Drag(int dx, int dy)
		{
			SizeF sizeF;
			if (this.RatioIndex != 0)
			{
				PointF location = this.ImageRect.Location;
				this.ImageRect.Offset((float)dx, (float)dy);
				if (this.ImageRect.Width <= (float)this.ClientRect.Width)
				{
					if (this.ImageRect.X < 0f)
					{
						this.ImageRect.X = 0f;
					}
					if (this.ImageRect.X + this.ImageRect.Width > (float)this.ClientRect.Width)
					{
						this.ImageRect.X = (float)this.ClientRect.Width - this.ImageRect.Width;
					}
				}
				else
				{
					if (this.ImageRect.X > 0f)
					{
						this.ImageRect.X = 0f;
					}
					if (this.ImageRect.X + this.ImageRect.Width < (float)this.ClientRect.Width)
					{
						this.ImageRect.X = (float)this.ClientRect.Width - this.ImageRect.Width;
					}
				}
				if (this.ImageRect.Height <= (float)this.ClientRect.Height)
				{
					if (this.ImageRect.Y < 0f)
					{
						this.ImageRect.Y = 0f;
					}
					if (this.ImageRect.Y + this.ImageRect.Height > (float)this.ClientRect.Height)
					{
						this.ImageRect.Y = (float)this.ClientRect.Height - this.ImageRect.Height;
					}
				}
				else
				{
					if (this.ImageRect.Y > 0f)
					{
						this.ImageRect.Y = 0f;
					}
					if (this.ImageRect.Y + this.ImageRect.Height < (float)this.ClientRect.Height)
					{
						this.ImageRect.Y = (float)this.ClientRect.Height - this.ImageRect.Height;
					}
				}
				sizeF = new SizeF(this.ImageRect.X - location.X, this.ImageRect.Y - location.Y);
			}
			else
			{
				sizeF = new SizeF(0f, 0f);
			}
			return sizeF;
		}

		public void Draw(Graphics g)
		{
			if (this.Image != null)
			{
				this.Image.SetResolution(g.DpiX, g.DpiY);
				g.DrawImage(this.Image, this.ImageRect);
			}
		}

		public void DrawMask(Graphics g, Bitmap mask)
		{
			if (mask != null)
			{
				PointF[] pointF = new PointF[] { new PointF(this.ImageRect.Left, this.ImageRect.Top), new PointF(this.ImageRect.Right, this.ImageRect.Top), new PointF(this.ImageRect.Left, this.ImageRect.Bottom) };
				g.DrawImage(mask, pointF, new RectangleF(0f, 0f, (float)mask.Width, (float)mask.Height), GraphicsUnit.Pixel);
			}
		}

		public void DrawKey(Graphics g ,Bitmap key)
		{
            if (key!= null)
            {
				PointF[] pointF = new PointF[] { new PointF(ImageRect.Left, ImageRect.Top), new PointF(ImageRect.Right, ImageRect.Top), new PointF(ImageRect.Left, ImageRect.Bottom) };
				g.DrawImage(key, pointF, new RectangleF(0f, 0f, (float)key.Width, (float)key.Height), GraphicsUnit.Pixel);
            }
        }

		private RectangleF FitRect(Bitmap bmp, Rectangle clientRect, out float ratio)
		{
			double width = (double)clientRect.Width * 1 / (double)bmp.Width;
			double height = (double)clientRect.Height * 1 / (double)bmp.Height;
			ratio = (float)((width < height ? width : height));
			float single = (float)bmp.Width * ratio;
			float height1 = (float)bmp.Height * ratio;
			float width1 = ((float)clientRect.Width - single) / 2f;
			float single1 = ((float)clientRect.Height - height1) / 2f;
			return new RectangleF(width1, single1, single, height1);
		}

		public bool HitTest(int x, int y)
		{
			return this.ImageRect.Contains((float)x, (float)y);
		}

		public void SetImage(Bitmap image, bool disposeOld)
		{
			if (disposeOld)
			{
				Image = null;
			}
			Image = image;
		}

		private void SetZoomFactor(Point p, float factor)
		{
			if (factor != this.LastZoomFactor)
			{
				PointF pointF = new PointF((float)p.X - this.ImageRect.X, (float)p.Y - this.ImageRect.Y);
				PointF pointF1 = new PointF((float)(pointF.X / this.LastZoomFactor), (float)(pointF.Y / this.LastZoomFactor));
				
				pointF1.X *= factor;
				pointF1.Y *= factor;
				
				this.ImageRect.Width = this.BaseRect.Width * factor;
				this.ImageRect.Height = this.BaseRect.Height * factor;
				if (factor != 1f)
				{
					this.ImageRect.X = (float)p.X - pointF1.X;
					this.ImageRect.Y = (float)p.Y - pointF1.Y;
				}
				else
				{
					this.ImageRect.Location = this.BaseRect.Location;
				}
				this.LastZoomFactor = factor;
			}
		}

		public void UpdateImage(Bitmap image, Rectangle clientRect)
		{
			if (this.Image != null)
			{
				Image = null;
			}
			this.Image = new Bitmap(image);
			if ((this.OriginImageSize != image.Size ? true : this.OriginClientSize != clientRect.Size))
			{
				this.OriginImageSize = image.Size;
				this.OriginClientSize = clientRect.Size;
				this.ImageRect = this.FitRect(this.Image, clientRect, out this.BaseRatio);
				this.BaseRect = new RectangleF(this.ImageRect.Location, this.ImageRect.Size);
				this.ClientRect = new Rectangle(clientRect.Location, clientRect.Size);
				this.RatioIndex = 0;
			}
		}

		public void ZoomIn(Point p)
		{
			this.RatioIndex++;
			if (this.RatioIndex >= (int)this.Ratio.Length)
			{
				this.RatioIndex = (int)this.Ratio.Length - 1;
			}
			this.SetZoomFactor(p, this.Ratio[this.RatioIndex]);
		}

		public void ZoomOut(Point p)
		{
			this.RatioIndex--;
			if (this.RatioIndex < 0)
			{
				this.RatioIndex = 0;
			}
			this.SetZoomFactor(p, this.Ratio[this.RatioIndex]);
		}
	}
}