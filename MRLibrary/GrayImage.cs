using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MRLibrary
{
    [XmlRoot("GrayImage")]
    public class GrayImage
    {
        public int Width;

        public int Height;

        public byte[] Bits;

        public GrayImage()
        {
            Width = 4000;
            Height = 3000;
            Bits = new byte[Width * Height];
        }

        public System.Drawing.Size Size
        {
            get
            {
                return new System.Drawing.Size(Width, Height);
            }
        }

        public GrayImage(byte[] buf, int w, int h)
        {
            Bits = buf;
            Width = w;
            Height = h;
        }

        //    public GrayImage(IntPtr strAddr, int w, int h)
        //    {
        //        Bits = new byte[w * h];
        //        Width = w;
        //        Height = h;
        //        int num = (w + 3) / 4 * 4;
        //        if (w != num)
        //        {
        //            for (int i = 0; i < h; i++)
        //            {
        //                Marshal.Copy(new IntPtr(strAddr.ToInt64() + (long)(i * num)), Bits, i * num, w);
        //            }
        //        }
        //        else
        //        {
        //            Marshal.Copy(strAddr, Bits, 0, num * h);
        //        }
        //    }

        public GrayImage(int w, int h)
        {
            Bits = new byte[w * h];
            Width = w;
            Height = h;
            Fill(255);
        }

        public GrayImage(GrayImage src)
        {
            Width = src.Width;
            Height = src.Height;
            Bits = new byte[Width * Height];
            Array.Copy(src.Bits, Bits, (int)Bits.Length);
        }

        public GrayImage(Bitmap bitmap)
        {
            Width = bitmap.Width;
            Height = bitmap.Height;
            PixelFormat pixelFormat = bitmap.PixelFormat;
            if (pixelFormat != PixelFormat.Format24bppRgb)
            {
                if (pixelFormat == PixelFormat.Format8bppIndexed)
                {
                    Bits = GrayImage.ToByteArray(bitmap);
                    return;
                }
                else
                {
                    if (pixelFormat == PixelFormat.Format32bppArgb)
                    {
                        Bits = ConvertToGrayscale(bitmap);
                        return;
                    }
                    return;
                }
            }
            Bits = ConvertToGrayscale(bitmap);
        }

        public GrayImage(string fileName)
        {
            Bitmap bitmap = new Bitmap(fileName);
            Width = bitmap.Width;
            Height = bitmap.Height;
            Bits = GrayImage.ToByteArray(bitmap);
            bitmap.Dispose();
        }

        //    public void AlphaBlend(GrayImage src, int alpha)
        //    {
        //        for (int i = 0; i < (int)Bits.Length; i++)
        //        {
        //            Bits[i] = (byte)((Bits[i] * alpha + src.Bits[i] * (256 - alpha)) / 256);
        //        }
        //    }

        //    public void Contrast(int contrast)
        //    {
        //        byte[] numArray = new byte[256];
        //        for (int i = 0; i < 256; i++)
        //        {
        //            numArray[i] = (byte)(255 - i);
        //        }
        //        byte[] bits = new byte[Width * Height];
        //        for (int j = 0; j < (int)Bits.Length; j++)
        //        {
        //            bits[j] = Bits[j];
        //        }
        //        Bits = bits;
        //    }

        private byte[] ConvertToGrayscale(Bitmap bitmap)
        {
            int num = 3;
            if (bitmap.PixelFormat == PixelFormat.Format32bppArgb)
            {
                num = 4;
            }
            BitmapData bitmapDatum = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
            IntPtr scan0 = bitmapDatum.Scan0;
            byte[] numArray = new byte[bitmap.Width * bitmap.Height];
            int num1 = Math.Abs(bitmapDatum.Stride);
            int num2 = 0;
            int height = bitmap.Height;
            int width = bitmap.Width;
            for (int i = 0; i < height; i++)
            {
                byte[] numArray1 = new byte[num1];
                Marshal.Copy(new IntPtr(scan0.ToInt64() + (long)(i * bitmapDatum.Stride)), numArray1, 0, num1);
                for (int j = 0; j < width; j++)
                {
                    numArray[num2] = (byte)((double)numArray1[j * num] * 0.114 + (double)numArray1[j * num + 1] * 0.587 + (double)numArray1[j * num + 2] * 0.299);
                    num2++;
                }
            }
            bitmap.UnlockBits(bitmapDatum);
            return numArray;
        }

        //    public void Crop(Rectangle r)
        //    {
        //        byte[] numArray = new byte[0];
        //        numArray = new byte[r.Width * r.Height];
        //        int left = r.Left;
        //        for (int i = 0; i < r.Height; i++)
        //        {
        //            try
        //            {
        //                Array.Copy(Bits, Width * (i + r.Top) + left, numArray, r.Width * i, r.Width);
        //            }
        //            catch
        //            {
        //            }
        //        }
        //        Width = r.Width;
        //        Height = r.Height;
        //        Bits = numArray;
        //    }

        //    public GrayImage CropNew(Rectangle r)
        //    {
        //        GrayImage grayImage = new GrayImage(this);
        //        grayImage.Crop(r);
        //        return grayImage;
        //    }

        //    public GrayImage CropNewF(float x, float y, int w, int h)
        //    {
        //        byte[] numArray = new byte[w * h];
        //        for (int i = 0; i < h; i++)
        //        {
        //            for (int j = 0; j < w; j++)
        //            {
        //                try
        //                {
        //                    float single = x + (float)j;
        //                    float single1 = y + (float)i;
        //                    int num = (int)single;
        //                    int num1 = (int)single1;
        //                    double num2 = (double)(single - (float)num);
        //                    double num3 = (double)(single1 - (float)num1);
        //                    byte pixel = GetPixel(num, num1);
        //                    byte pixel1 = GetPixel(num, num1 + 1);
        //                    byte pixel2 = GetPixel(num + 1, num1);
        //                    byte pixel3 = GetPixel(num + 1, num1 + 1);
        //                    double num4 = (1 - num2) * (double)pixel + num2 * (double)pixel2;
        //                    double num5 = (1 - num2) * (double)pixel1 + num2 * (double)pixel3;
        //                    byte num6 = (byte)((1 - num3) * num4 + num3 * num5);
        //                    numArray[i * w + j] = num6;
        //                }
        //                catch
        //                {
        //                }
        //            }
        //        }
        //        return new GrayImage(w, h)
        //        {
        //            Bits = numArray
        //        };
        //    }

        //public void Dispose()
        //{
        //}

        public void Fill(byte c)
        {
            for (int i = 0; i < (int)Bits.Length; i++)
            {
                Bits[i] = c;
            }
        }

        //    public void FillRectangle(Rectangle r, byte c)
        //    {
        //        for (int i = r.Y; i < r.Y + r.Height; i++)
        //        {
        //            int width = Width * i + r.X;
        //            for (int j = 0; j < r.Width; j++)
        //            {
        //                int num = width;
        //                width = num + 1;
        //                Bits[num] = c;
        //            }
        //        }
        //    }

        //    public void FlipHorizontal()
        //    {
        //        byte[] numArray = new byte[(int)Bits.Length];
        //        byte[] numArray1 = new byte[Width];
        //        for (int i = 0; i < Height; i++)
        //        {
        //            Array.Copy(Bits, Width * i, numArray1, 0, Width);
        //            Array.Reverse(numArray1);
        //            Array.Copy(numArray1, 0, numArray, Width * i, Width);
        //        }
        //        Bits = numArray;
        //    }

        //    public void FlipVertical()
        //    {
        //        byte[] numArray = new byte[(int)Bits.Length];
        //        for (int i = 0; i < Height; i++)
        //        {
        //            Array.Copy(Bits, Width * i, numArray, Width * (Height - i - 1), Width);
        //        }
        //        Bits = numArray;
        //    }

        //public GrayImage GaussianBlue(int size)
        //{
        //    Image<Gray, byte> imageBW8 = cvImageConvert.GrayImageToImageBW8(this);
        //    Image<Gray, byte> image = imageBW8.SmoothGaussian(size);
        //    GrayImage grayImage = cvImageConvert.ImageBW8ToGrayImage(image);
        //    return grayImage;
        //}
        public GrayImage GaussianBlur(int size)
        {
            Mat matInput = null;
            Mat matOutput = null;
            try
            {
                matInput =new Mat(Height, Width, MatType.CV_8UC1);
                matInput.SetArray<byte>(this.Bits);
                matOutput = new Mat();

                Cv2.GaussianBlur(matInput,matOutput,new OpenCvSharp.Size(size,size),0);

                byte[] result = new byte[this.Width * this.Height];
                Marshal.Copy(matOutput.Data, result, 0, result.Length);

                return new GrayImage(result, this.Width, this.Height);
            }
            finally
            {
                matInput?.Dispose();
                matOutput?.Dispose();
            }
        }

        //    public byte GetPixel(int x, int y)
        //    {
        //        byte bits = Bits[y * Width + x];
        //        return bits;
        //    }

        //    private int[] GetScaleArray(int fromSize, int toSize)
        //    {
        //        int[] numArray;
        //        int[] numArray1 = new int[toSize];
        //        if (toSize > 1)
        //        {
        //            double num = (double)(fromSize - 1) * 1 / (double)toSize;
        //            for (int i = 0; i < toSize; i++)
        //            {
        //                numArray1[i] = (int)((double)i * num + 0.5);
        //            }
        //            numArray = numArray1;
        //        }
        //        else
        //        {
        //            numArray1[0] = 0;
        //            numArray = numArray1;
        //        }
        //        return numArray;
        //    }

        //    private int[] GetStepArray(int fromSize, int toSize)
        //    {
        //        int[] numArray;
        //        int[] numArray1 = new int[toSize];
        //        if (toSize > 1)
        //        {
        //            double num = (double)(fromSize - 1) * 1 / (double)toSize;
        //            for (int i = 0; i < toSize; i++)
        //            {
        //                numArray1[i] = (int)((double)i * num + 0.5);
        //            }
        //            for (int j = 0; j < toSize - 1; j++)
        //            {
        //                numArray1[j] = numArray1[j + 1] - numArray1[j];
        //            }
        //            numArray = numArray1;
        //        }
        //        else
        //        {
        //            numArray1[0] = 0;
        //            numArray = numArray1;
        //        }
        //        return numArray;
        //    }

        public float[] HorizontalProjection()
        {
            float[] singleArray = new float[Width];
            for (int i = 0; i < Width; i++)
            {
                float bits = 0f;
                int width = i;
                for (int j = 0; j < Height; j++)
                {
                    bits += (float)Bits[width];
                    width += Width;
                }
                singleArray[i] = bits;
            }
            return singleArray;
        }

        //    private byte[] NearestNeighborResize(byte[] src, int w, int h, int nw, int nh)
        //    {
        //        byte[] numArray;
        //        if ((nw <= 1 ? false : nh > 1))
        //        {
        //            byte[] numArray1 = new byte[nw * nh];
        //            int[] stepArray = GetStepArray(w, nw);
        //            int[] scaleArray = GetScaleArray(h, nh);
        //            int num = 0;
        //            for (int i = 0; i < (int)stepArray.Length - 1; i++)
        //            {
        //                num += stepArray[i];
        //            }
        //            int num1 = 0;
        //            int num2 = 0;
        //            for (int j = 0; j < nh; j++)
        //            {
        //                num1 = scaleArray[j] * w;
        //                num2 = j * nw;
        //                for (int k = 0; k < nw; k++)
        //                {
        //                    int num3 = num2;
        //                    num2 = num3 + 1;
        //                    numArray1[num3] = src[num1];
        //                    num1 += stepArray[k];
        //                }
        //            }
        //            numArray = numArray1;
        //        }
        //        else
        //        {
        //            numArray = null;
        //        }
        //        return numArray;
        //    }

        //    public void Overlay(GrayImage src, int alpha)
        //    {
        //        for (int i = 0; i < (int)Bits.Length; i++)
        //        {
        //            int bits = (Bits[i] * alpha + src.Bits[i] * alpha * (256 - alpha) / 256) / 256;
        //            if (bits > 255)
        //            {
        //                bits = 255;
        //            }
        //            Bits[i] = (byte)bits;
        //        }
        //    }

        //    public GrayImage Resize(int w, int h)
        //    {
        //        byte[] numArray = NearestNeighborResize(Bits, Width, Height, w, h);
        //        return new GrayImage(numArray, w, h);
        //    }

        //    public GrayImage ResizeAntiAlias(int w, int h)
        //    {
        //        Image<Gray, byte> imageBW8 = cvImageConvert.GrayImageToImageBW8(this);
        //        Image<Gray, byte> image = imageBW8.Resize(w, h, Emgu.CV.CvEnum.Inter.Linear);
        //        GrayImage grayImage = cvImageConvert.ImageBW8ToGrayImage(image);
        //        return grayImage;
        //    }

        //    public GrayImage ResizeFitIn(System.Drawing.Size frame)
        //    {
        //        double width = (double)frame.Width * 1 / (double)Width;
        //        double height = (double)frame.Height * 1 / (double)Height;
        //        double num = (width < height ? width : height);
        //        int width1 = (int)((double)Width * num + 0.5);
        //        int height1 = (int)((double)Height * num + 0.5);
        //        return Resize(width1, height1);
        //    }

        //    public void ResizeInPlace(int w, int h)
        //    {
        //        Bits = NearestNeighborResize(Bits, Width, Height, w, h);
        //        Width = w;
        //        Height = h;
        //    }

        //    public void Rotate180()
        //    {
        //        FlipVertical();
        //        FlipHorizontal();
        //    }

        //    public void RotateCCW()
        //    {
        //        byte[] bits = new byte[(int)Bits.Length];
        //        int num = 0;
        //        for (int i = 0; i < Height; i++)
        //        {
        //            int height = Height * (Width - 1) + i;
        //            for (int j = 0; j < Width; j++)
        //            {
        //                int num1 = num;
        //                num = num1 + 1;
        //                bits[height] = Bits[num1];
        //                height -= Height;
        //            }
        //        }
        //        int height1 = Height;
        //        Height = Width;
        //        Width = height1;
        //        Bits = bits;
        //    }

        //    public void RotateCW()
        //    {
        //        byte[] bits = new byte[(int)Bits.Length];
        //        int num = 0;
        //        for (int i = 0; i < Height; i++)
        //        {
        //            int height = Height - i - 1;
        //            for (int j = 0; j < Width; j++)
        //            {
        //                int num1 = num;
        //                num = num1 + 1;
        //                bits[height] = Bits[num1];
        //                height += Height;
        //            }
        //        }
        //        int height1 = Height;
        //        Height = Width;
        //        Width = height1;
        //        Bits = bits;
        //    }

        public void Save(string filename, ImageFormat format)
        {
            Bitmap bitmap = ToBitmap();
            bitmap.Save(filename, format);
            bitmap.Dispose();
        }

        public void SetMask(GrayImage mask)
        {
            for (int i = 0; i < Height; i++)
            {
                int width = i * Width;
                for (int j = 0; j < Width; j++)
                {
                    if (mask.Bits[width + j] == 0)
                    {
                        Bits[width + j] = 0;
                    }
                }
            }
        }

        //    public void SizeSquare()
        //    {
        //        int width = 0;
        //        byte[] numArray = new byte[0];
        //        if (Width <= Height)
        //        {
        //            width = Width;
        //            numArray = new byte[width * width];
        //            int height = (Height - width) / 2;
        //            for (int i = 0; i < width; i++)
        //            {
        //                Array.Copy(Bits, Width * (height + i), numArray, width * i, width);
        //            }
        //        }
        //        else
        //        {
        //            width = Height;
        //            numArray = new byte[width * width];
        //            int num = (Width - width) / 2;
        //            for (int j = 0; j < Height; j++)
        //            {
        //                Array.Copy(Bits, Width * j + num, numArray, width * j, width);
        //            }
        //        }
        //        Width = width;
        //        Height = width;
        //        Bits = numArray;
        //    }

        public GrayImage Sobel()
        {
            GrayImage grayImage;
            GrayImage grayImage1 = new GrayImage(Width, Height);
            byte[] numArray = grayImage1.Bits;
            if (Height >= 4)
            {
                int height = Height / 2;
                Task task = Task.Factory.StartNew(() =>
                {
                    for (int i = 1; i < height; i++)
                    {
                        int width = (i - 1) * Width + 1;
                        int num = i * Width + 1;
                        int width1 = (i + 1) * Width + 1;
                        for (int j = 1; j < Width - 1; j++)
                        {
                            int bits = -Bits[width - 1] - Bits[num - 1] * 2 - Bits[width1 - 1] + Bits[width + 1] + Bits[num + 1] * 2 + Bits[width1 + 1];
                            if (bits < 0)
                            {
                                bits = -bits;
                            }
                            int bits1 = -Bits[width - 1] - Bits[width] * 2 - Bits[width + 1] + Bits[width1 - 1] + Bits[width1] * 2 + Bits[width1 + 1];
                            if (bits1 < 0)
                            {
                                bits1 = -bits1;
                            }
                            int num1 = bits + bits1;
                            if (num1 > 255)
                            {
                                num1 = 255;
                            }
                            numArray[num] = (byte)num1;
                            width++;
                            num++;
                            width1++;
                        }
                    }
                });
                Task task1 = Task.Factory.StartNew(() =>
                {
                    for (int i = height; i < Height - 1; i++)
                    {
                        int width = (i - 1) * Width + 1;
                        int num = i * Width + 1;
                        int width1 = (i + 1) * Width + 1;
                        for (int j = 1; j < Width - 1; j++)
                        {
                            int bits = -Bits[width - 1] - Bits[num - 1] * 2 - Bits[width1 - 1] + Bits[width + 1] + Bits[num + 1] * 2 + Bits[width1 + 1];
                            if (bits < 0)
                            {
                                bits = -bits;
                            }
                            int bits1 = -Bits[width - 1] - Bits[width] * 2 - Bits[width + 1] + Bits[width1 - 1] + Bits[width1] * 2 + Bits[width1 + 1];
                            if (bits1 < 0)
                            {
                                bits1 = -bits1;
                            }
                            int num1 = bits + bits1;
                            if (num1 > 255)
                            {
                                num1 = 255;
                            }
                            numArray[num] = (byte)num1;
                            width++;
                            num++;
                            width1++;
                        }
                    }
                });
                Task.WaitAll(new Task[] { task, task1 });
                grayImage = grayImage1;
            }
            else
            {
                grayImage = grayImage1;
            }
            return grayImage;
        }

        //    public GrayImage SobelX()
        //    {
        //        GrayImage grayImage;
        //        GrayImage grayImage1 = new GrayImage(Width, Height);
        //        byte[] numArray = grayImage1.Bits;
        //        if (Height >= 4)
        //        {
        //            int height = Height / 2;
        //            Task task = Task.Factory.StartNew(() => {
        //                for (int i = 1; i < height; i++)
        //                {
        //                    int width = (i - 1) * Width + 1;
        //                    int num = i * Width + 1;
        //                    int width1 = (i + 1) * Width + 1;
        //                    for (int j = 1; j < Width - 1; j++)
        //                    {
        //                        int bits = -Bits[width - 1] - Bits[num - 1] * 2 - Bits[width1 - 1] + Bits[width + 1] + Bits[num + 1] * 2 + Bits[width1 + 1];
        //                        if (bits < 0)
        //                        {
        //                            bits = 0;
        //                        }
        //                        if (bits > 255)
        //                        {
        //                            bits = 255;
        //                        }
        //                        numArray[num] = (byte)bits;
        //                        width++;
        //                        num++;
        //                        width1++;
        //                    }
        //                }
        //            });
        //            Task task1 = Task.Factory.StartNew(() => {
        //                for (int i = height; i < Height - 1; i++)
        //                {
        //                    int width = (i - 1) * Width + 1;
        //                    int num = i * Width + 1;
        //                    int width1 = (i + 1) * Width + 1;
        //                    for (int j = 1; j < Width - 1; j++)
        //                    {
        //                        int bits = -Bits[width - 1] - Bits[num - 1] * 2 - Bits[width1 - 1] + Bits[width + 1] + Bits[num + 1] * 2 + Bits[width1 + 1];
        //                        if (bits < 0)
        //                        {
        //                            bits = 0;
        //                        }
        //                        if (bits > 255)
        //                        {
        //                            bits = 255;
        //                        }
        //                        numArray[num] = (byte)bits;
        //                        width++;
        //                        num++;
        //                        width1++;
        //                    }
        //                }
        //            });
        //            Task.WaitAll(new Task[] { task, task1 });
        //            grayImage = grayImage1;
        //        }
        //        else
        //        {
        //            grayImage = grayImage1;
        //        }
        //        return grayImage;
        //    }

        public Bitmap ToBitmap()
        {
            Bitmap bitmap = new Bitmap(Width, Height, PixelFormat.Format8bppIndexed);
            ColorPalette palette = bitmap.Palette;
            for (int i = 0; i < 256; i++)
            {
                palette.Entries[i] = Color.FromArgb(i, i, i);
            }
            bitmap.Palette = palette;
            BitmapData bitmapDatum = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
            IntPtr scan0 = bitmapDatum.Scan0;
            int width = Width;
            if (width != bitmapDatum.Stride)
            {
                for (int j = 0; j < Height; j++)
                {
                    Marshal.Copy(Bits, j * width, new IntPtr(scan0.ToInt64() + (long)(j * bitmapDatum.Stride)), Width);
                }
            }
            else
            {
                Marshal.Copy(Bits, 0, scan0, bitmapDatum.Stride * bitmap.Height);
            }
            bitmap.UnlockBits(bitmapDatum);
            return bitmap;
        }

        public static byte[] ToByteArray(Bitmap bitmap)
        {
            BitmapData bitmapDatum = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
            IntPtr scan0 = bitmapDatum.Scan0;
            byte[] numArray = new byte[bitmap.Width * bitmap.Height];
            int width = bitmap.Width;
            if (width != bitmapDatum.Stride)
            {
                int height = bitmap.Height;
                int num = bitmap.Width;
                for (int i = 0; i < height; i++)
                {
                    Marshal.Copy(new IntPtr(scan0.ToInt64() + (long)(i * bitmapDatum.Stride)), numArray, i * width, num);
                }
            }
            else
            {
                Marshal.Copy(scan0, numArray, 0, bitmapDatum.Stride * bitmap.Height);
            }
            bitmap.UnlockBits(bitmapDatum);
            return numArray;
        }

        public Bitmap ToMaskBitmap32()
        {
            int obj;
            Bitmap bitmap = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
            BitmapData bitmapDatum = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
            IntPtr scan0 = bitmapDatum.Scan0;
            int height = bitmap.Height;
            int width = bitmap.Width;
            int num = 0;
            for (int i = 0; i < height; i++)
            {
                byte[] numArray = new byte[bitmapDatum.Stride];
                for (int j = 0; j < width; j++)
                {
                    int num1 = j * 4;
                    int num2 = num;
                    num = num2 + 1;
                    byte bits = Bits[num2];
                    numArray[num1] = (byte)(255 - bits);
                    numArray[num1 + 1] = 0;
                    numArray[num1 + 2] = (byte)(255 - bits);
                    byte[] numArray1 = numArray;
                    int num3 = num1 + 3;
                    if (bits == 0)
                    {
                        obj = 128;
                    }
                    else
                    {
                        obj = 0;
                    }
                    numArray1[num3] = (byte)obj;
                }
                Marshal.Copy(numArray, 0, new IntPtr(scan0.ToInt64() + (long)(i * bitmapDatum.Stride)), (int)numArray.Length);
            }
            bitmap.UnlockBits(bitmapDatum);
            return bitmap;
        }

        public float[] VerticalProjection()
        {
            float[] singleArray = new float[Height];
            for (int i = 0; i < Height; i++)
            {
                float bits = 0f;
                int width = i * Width;
                for (int j = 0; j < Width; j++)
                {
                    bits += (float)Bits[width];
                    width++;
                }
                singleArray[i] = bits;
            }
            return singleArray;
        }
    }
}
