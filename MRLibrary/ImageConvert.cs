//using System;
//using System.Collections.Generic;
//using System.Drawing.Imaging;
//using System.Drawing;
//using System.Linq;
//using System.Runtime.InteropServices;
//using System.Text;
//using System.Threading.Tasks;
//using Euresys.Open_eVision;
//using OpenCvSharp;

//namespace MRLibrary
//{
//    public class ImageConvert
//    {
//        public ImageConvert()
//        {
//        }

//        public static EImageBW8 BitmapToImageBW8(Bitmap bmp)
//        {
//            BitmapData bitmapDatum = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);
//            IntPtr scan0 = bitmapDatum.Scan0;
//            EImageBW8 eImageBW8 = new EImageBW8(bmp.Width, bmp.Height);
//            IntPtr imagePtr = eImageBW8.GetImagePtr(0, 0);
//            for (int i = 0; i < bmp.Height; i++)
//            {
//                IntPtr intPtr = new IntPtr(scan0.ToInt64() + (long)(i * bitmapDatum.Stride));
//                IntPtr intPtr1 = new IntPtr(imagePtr.ToInt64() + (long)(i * eImageBW8.RowPitch));
//                ImageConvert.CopyMemory(intPtr1, intPtr, (uint)bitmapDatum.Stride);
//            }
//            bmp.UnlockBits(bitmapDatum);
//            return eImageBW8;
//        }

//        [DllImport("kernel32.dll", CharSet = CharSet.None, ExactSpelling = false)]
//        private static extern void CopyMemory(IntPtr destination, IntPtr source, uint length);

//        public static EImageBW8 GrayImageToImageBW8(GrayImage img)
//        {
//            Bitmap bitmap = img.ToBitmap();
//            EImageBW8 imageBW8 = ImageConvert.BitmapToImageBW8(bitmap);
//            bitmap.Dispose();
//            return imageBW8;
//        }

//        public static Mat GrayImageToMat(GrayImage img)
//        {
//            Mat mat = new Mat(img.Height, img.Width, MatType.CV_8UC1);
//            Marshal.Copy(img.Bits, 0, mat.Data, img.Bits.Length);
//            return mat;
//        }

//        //public static GrayImage MatToGrayImage(Mat img)
//        //{
//        //    byte[] numArray = new byte[img.Width * img.Height];
//        //    img.CopyTo<byte>(numArray);
//        //    GrayImage grayImage = new GrayImage(numArray, img.Width, img.Height);
//        //    return grayImage;
//        //}

//        public static GrayImage MatToGrayImage(Mat img)
//        {
//            if(img.Type() != MatType.CV_8UC1) throw new ArgumentException("Mat 應該為單通道 CV_8UC1");
//            int width = img.Width;
//            int height = img.Height;
//            int step = (int)img.Step();
//            int size = width * height;

//            byte[] buffer = new byte[size];
//            IntPtr src = img.Data;
//            for(int row =0; row<height; row++)
//            {
//                Marshal.Copy(src + row * step, buffer, row * width, width);
//            }
//            return new GrayImage(buffer, width, height);
//        }

//        public static void MatToEImageBW8(Mat matImage, EImageBW8 eVisionImage)
//        {
//            // 確認 Mat 的資料類型
//            //if (matImage.Depth != DepthType.Cv8U || matImage.NumberOfChannels != 1)
//            //{
//            //    throw new ArgumentException("Mat should be single-channel with 8-bit depth");
//            //}
//            if (matImage.Type() != MatType.CV_8UC1) throw new ArgumentException("Mat 應該為單通道 CV_8UC1");

//            int width = matImage.Width;
//            int height = matImage.Height;

//            //if (eVisionImage == null || eVisionImage.Width != width || eVisionImage.Height != height)
//            //{
//            //    if (eVisionImage != null) eVisionImage.Dispose();
//            //    eVisionImage = new EImageBW8(width, height);
//            //}
//            bool needsRecreate = true;
//            if (eVisionImage != null)
//            {
//                try
//                {
//                    // 嘗試訪問屬性來檢查物件是否有效
//                    if (eVisionImage.Width == width && eVisionImage.Height == height)
//                    {
//                        needsRecreate = false;
//                    }
//                }
//                catch (AccessViolationException)
//                {
//                    // 物件已經無效,需要重新創建
//                    needsRecreate = true;
//                }
//                catch (Exception)
//                {
//                    // 其他異常也需要重新創建
//                    needsRecreate = true;
//                }
//            }

//            if (needsRecreate)
//            {
//                // 安全地釋放舊物件
//                if (eVisionImage != null)
//                {
//                    try
//                    {
//                        eVisionImage.Dispose();
//                    }
//                    catch
//                    {
//                        // 忽略 Dispose 時的錯誤
//                    }
//                }
//                // 創建新物件
//                eVisionImage = new EImageBW8(width, height);
//            }

//            // 取得 Mat 的資料指標
//            IntPtr matData = matImage.Data;

//            // 取得 EImageBW8 的資料指標
//            IntPtr eVisionData = eVisionImage.GetImagePtr(0, 0);

//            // 複製資料
//            //for (int i = 0; i < matImage.Height; i++)
//            //{
//            //    IntPtr intPtrs = new IntPtr(matData.ToInt64() + (long)(i * matImage.Width));
//            //    IntPtr intPtrd = new IntPtr(eVisionData.ToInt64() + (long)(i * eVisionImage.RowPitch));
//            //    CopyMemory(intPtrd, intPtrs, (uint)matImage.Width);
//            //}
//            int matStep = (int)matImage.Step();
//            int eVisionPitch = eVisionImage.RowPitch;
//            if(matStep== eVisionPitch)
//            {
//                CopyMemory(eVisionData, matData, (uint)(matStep * height));
//            }
//            else
//            {
//                for(int row=0; row< height; row++)
//                {
//                    IntPtr srcPtr =IntPtr.Add(matData,row*matStep);
//                    IntPtr dstPtr =IntPtr.Add(eVisionData,row* eVisionPitch);
//                    CopyMemory(dstPtr, srcPtr, (uint)width);
//                }
//            }
//        }
//    }
//}

using Euresys.Open_eVision;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MRLibrary
{
    public class ImageConvert
    {
        public ImageConvert()
        {
        }
        public static Mat GrayImageToMat(GrayImage img)
        {
            Mat mat = new Mat(img.Height, img.Width, MatType.CV_8UC1);
            Marshal.Copy(img.Bits, 0, mat.Data, img.Bits.Length);
            return mat;
        }

        public static EImageBW8 BitmapToImageBW8(Bitmap bmp)
        {
            BitmapData bitmapDatum = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);
            IntPtr scan0 = bitmapDatum.Scan0;
            EImageBW8 eImageBW8 = new EImageBW8(bmp.Width, bmp.Height);
            IntPtr imagePtr = eImageBW8.GetImagePtr(0, 0);
            for (int i = 0; i < bmp.Height; i++)
            {
                IntPtr intPtr = new IntPtr(scan0.ToInt64() + (long)(i * bitmapDatum.Stride));
                IntPtr intPtr1 = new IntPtr(imagePtr.ToInt64() + (long)(i * eImageBW8.RowPitch));
                ImageConvert.CopyMemory(intPtr1, intPtr, (uint)bitmapDatum.Stride);
            }
            bmp.UnlockBits(bitmapDatum);
            return eImageBW8;
        }

        [DllImport("kernel32.dll", CharSet = CharSet.None, ExactSpelling = false)]
        private static extern void CopyMemory(IntPtr destination, IntPtr source, uint length);

        public static EImageBW8 GrayImageToImageBW8(GrayImage img)
        {
            Bitmap bitmap = img.ToBitmap();
            EImageBW8 imageBW8 = ImageConvert.BitmapToImageBW8(bitmap);
            bitmap.Dispose();
            return imageBW8;
        }

        //public static GrayImage MatToGrayImage(Mat img)
        //{
        //    byte[] numArray = new byte[img.Width * img.Height];
        //    //img.CopyTo<byte>(numArray);
        //    img.GetArray(out byte[] numArray);
        //    GrayImage grayImage = new GrayImage(numArray, img.Width, img.Height);
        //    return grayImage;
        //}
        public static GrayImage MatToGrayImage(Mat img)
        {
            if (img == null) throw new ArgumentNullException(nameof(img));
            if (img.Type() != MatType.CV_8UC1) throw new ArgumentException("Mat 應該為單通道 CV_8UC1");

            int width = img.Width;
            int height = img.Height;
            int step = (int)img.Step(); // OpenCvSharp 的每列位元組距

            byte[] buffer = new byte[width * height];

            if (img.IsContinuous())
            {
                // 連續記憶體可一次拷貝
                Marshal.Copy(img.Data, buffer, 0, buffer.Length);
            }
            else
            {
                // 非連續：逐列拷貝每列有效寬度的資料
                for (int row = 0; row < height; row++)
                {
                    IntPtr srcRowPtr = img.Ptr(row);
                    Marshal.Copy(srcRowPtr, buffer, row * width, width);
                }
            }

            return new GrayImage(buffer, width, height);
        }

        //public static void MatToEImageBW8(Mat matImage, EImageBW8 eVisionImage)
        //{
        //    // 確認 Mat 的資料類型
        //    if (matImage.Depth != MatType.CV_8UC1 || matImage.Channels() != 1)
        //    {
        //        throw new ArgumentException("Mat should be single-channel with 8-bit depth");
        //    }

        //    // 取得 Mat 的資料指標
        //    IntPtr matData = matImage.GetDataPointer();

        //    // 取得 EImageBW8 的資料指標
        //    IntPtr eVisionData = eVisionImage.GetImagePtr(0, 0);

        //    // 複製資料
        //    for (int i = 0; i < matImage.Height; i++)
        //    {
        //        IntPtr intPtrs = new IntPtr(matData.ToInt64() + (long)(i * matImage.Width));
        //        IntPtr intPtrd = new IntPtr(eVisionData.ToInt64() + (long)(i * eVisionImage.RowPitch));
        //        CopyMemory(intPtrd, intPtrs, (uint)matImage.Width);
        //    }
        //}
        //public static void MatToEImageBW8(Mat matImage, EImageBW8 eVisionImage)
        //{

        //    if (matImage.Type() != MatType.CV_8UC1)
        //        throw new ArgumentException("Mat 應為單通道 CV_8UC1");

        //    int width = matImage.Width;
        //    int height = matImage.Height;

        //    // 檢查或重新建立 EImageBW8 大小
        //    if (eVisionImage == null || eVisionImage.Width != width || eVisionImage.Height != height)
        //    {
        //        if (eVisionImage != null)
        //            eVisionImage.Dispose();
        //        eVisionImage = new EImageBW8(width, height);
        //    }

        //    IntPtr matData = matImage.Data;
        //    IntPtr eVisionData = eVisionImage.GetImagePtr(0, 0);

        //    int matStep = (int)matImage.Step();
        //    int eVisionPitch = eVisionImage.RowPitch;

        //    if (matStep == eVisionPitch)
        //    {
        //        // 步長相同直接複製
        //        CopyMemory(eVisionData, matData, (uint)(height * matStep));
        //    }
        //    else
        //    {
        //        // 步長不同，逐行複製
        //        for (int row = 0; row < height; row++)
        //        {
        //            IntPtr srcPtr = IntPtr.Add(matData, row * matStep);
        //            IntPtr dstPtr = IntPtr.Add(eVisionData, row * eVisionPitch);
        //            CopyMemory(dstPtr, srcPtr, (uint)width);
        //        }
        //    }
        //}
        public static EImageBW8 MatToEImageBW8(Mat matImage, EImageBW8 eVisionImage)
        {
            if (matImage == null) throw new ArgumentNullException(nameof(matImage));
            if (matImage.Type() != MatType.CV_8UC1)
                throw new ArgumentException("Mat 應為單通道 CV_8UC1");

            int width = matImage.Width;
            int height = matImage.Height;
            int matStep = (int)matImage.Step();
            IntPtr srcBase = matImage.Ptr(0);

            bool needRecreate = false;

            // 嘗試安全檢查 eVisionImage 是否可用（避免直接拋出 AccessViolation）
            if (eVisionImage == null)
            {
                needRecreate = true;
            }
            else
            {
                try
                {
                    if (eVisionImage.Width != width || eVisionImage.Height != height)
                        needRecreate = true;
                }
                catch
                {
                    // 任何讀取屬性時的例外都視為物件已無效，需重建
                    needRecreate = true;
                }
            }

            if (needRecreate)
            {
                if (eVisionImage != null)
                {
                    try { eVisionImage.Dispose(); } catch { }
                }
                eVisionImage = new EImageBW8(width, height);
            }

            IntPtr dstBase = eVisionImage.GetImagePtr(0, 0);
            int dstPitch = eVisionImage.RowPitch;

            try
            {
                // 若 mat 連續且步長相同可一次複製
                if (matImage.IsContinuous() && matStep == dstPitch)
                {
                    int total = checked(matStep * height);
                    CopyMemory(dstBase, srcBase, (uint)total);
                }
                else
                {
                    // 否則逐列複製，每列只拷 width bytes（單通道）
                    for (int row = 0; row < height; row++)
                    {
                        IntPtr srcRow = IntPtr.Add(srcBase, row * matStep);
                        IntPtr dstRow = IntPtr.Add(dstBase, row * dstPitch);
                        CopyMemory(dstRow, srcRow, (uint)width);
                    }
                }
            }
            catch
            {
                // 若複製失敗，回收新建的物件並重新拋出（呼叫端可再處理）
                try { eVisionImage.Dispose(); } catch { }
                throw;
            }

            return eVisionImage;
        }
    }
}