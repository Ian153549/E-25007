using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MRLibrary
{
    public class cvImageConvert
    {
        public cvImageConvert()
        {
        }

        [DllImport("kernel32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        private static extern void CopyMemory(IntPtr destination, IntPtr source, uint length);

        //public static Image<Gray, byte> GrayImageToImageBW8(GrayImage img)
        //{
        //    Image<Gray, byte> image = new Image<Gray, byte>(img.Size);
        //    int width = 0;
        //    int num = (img.Width + 3) / 4 * 4;
        //    for (int i = 0; i < img.Height; i++)
        //    {
        //        Buffer.BlockCopy(img.Bits, width, image.ManagedArray, num * i, img.Width);
        //        width += img.Width;
        //    }
        //    return image;
        //}
        public static Mat GrayImageToMatBW8(GrayImage img)
        {
            int widthAligned = (img.Width + 3) / 4 * 4;
            Mat mat = new Mat(img.Height, widthAligned, MatType.CV_8UC1);

            int srcOffset = 0;
            for (int i = 0; i < img.Height; i++)
            {
                IntPtr destRowPtr = mat.Ptr(i);
                Marshal.Copy(img.Bits, srcOffset, destRowPtr, img.Width);
                srcOffset += img.Width;
            }
            return new Mat(mat, new Rect(0, 0, img.Width, img.Height)).Clone();
        }

        //public static GrayImage ImageBW8ToGrayImage(Image<Gray, byte> img)
        //{
        //    GrayImage grayImage = new GrayImage(img.Width, img.Height);
        //    int num = 0;
        //    int width = (img.Width + 3) / 4 * 4;
        //    for (int i = 0; i < img.Height; i++)
        //    {
        //        Buffer.BlockCopy(img.ManagedArray, num, grayImage.Bits, img.Width * i, img.Width);
        //        num += width;
        //    }
        //    return grayImage;
        //}

        public static GrayImage MatBW8ToGrayImage(Mat mat)
        {
            if (mat.Type() != MatType.CV_8UC1)
                throw new ArgumentException("Input Mat must be of type CV_8UC1");
            int width = mat.Width;
            int height = mat.Height;
            int stride = (width + 3) / 4 * 4;
            GrayImage grayImage = new GrayImage(width, height);

            int dstOffset = 0;
            for(int i = 0; i < height; i++)
            {
                IntPtr srcRowPtr = mat.Ptr(i);
                Marshal.Copy(srcRowPtr, grayImage.Bits, dstOffset, width);
                dstOffset += width;
            }
            return grayImage;
        }
    }
}
