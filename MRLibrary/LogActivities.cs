using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRLibrary
{
    public class LogActivities
    {
        private static object lockObject;

        private static object alarmObject;

        private static object debugObject;

        static LogActivities()
        {
            lockObject = new object();
            alarmObject = new object();
            debugObject = new object();
        }

        public LogActivities()
        {
        }

        public static void GenerateAlarmLog(AlarmItem al)
        {
            lock (alarmObject)
            {
                try
                {
                    string logDirectory = GetLogDirectory();
                    DateTime now = DateTime.Now;
                    using (StreamWriter sw = new StreamWriter(string.Concat(logDirectory, "\\AlarmLog ", now.ToString("yyyyMMdd"), ".csv"), true))
                    {
                        sw.Write(string.Concat(al.DateTime.ToString("HH:mm:ss"), ", "));
                        sw.Write(al.Message);
                        sw.WriteLine(al.Status.ToString());
                    }
                }
                catch
                {
                }
            }
        }

        public static void GenerateImageLog(string namesuffix, Mat imgL, Mat imgR)
        {
            lock (LogActivities.alarmObject)
            {
                try
                {
                    string dir = string.Concat(LogActivities.GetLogDirectory(), "\\Images");
                    Directory.CreateDirectory(dir);
                    DateTime now = DateTime.Now;
                    string fn = string.Concat(dir, "\\", now.ToString("HH_mm_ss_"), namesuffix);
                    Cv2.ImWrite(string.Concat(fn, "L.jpg"), imgL);
                    Cv2.ImWrite(string.Concat(fn, "R.jpg"), imgR);
                }
                catch
                {
                }
            }
        }

        public static void GenerateImageLog(int iRecipe, string namesuffix, Mat imgL, Mat imgR)
        {
            lock (LogActivities.alarmObject)
            {
                try
                {
                    string dir = string.Concat(LogActivities.GetLogDirectory(), "\\Images");
                    Directory.CreateDirectory(dir);
                    DateTime now = DateTime.Now;
                    string fn = string.Concat(dir, "\\", now.ToString("HH-mm-ss_["), iRecipe.ToString(), "]_", namesuffix);
                    Cv2.ImWrite(string.Concat(fn, "-L.jpg"), imgL);
                    Cv2.ImWrite(string.Concat(fn, "-R.jpg"), imgR);
                }
                catch
                {
                }
            }
        }

        public static void GenerateLog(string message)
        {
            lock (lockObject)
            {
                try
                {
                    string logDirectory = LogActivities.GetLogDirectory();
                    DateTime now = DateTime.Now;
                    using (StreamWriter sw = new StreamWriter(string.Concat(logDirectory, "\\Activities", now.ToString("yyyyMMdd"), ".csv"), true))
                    {
                        now = DateTime.Now;
                        sw.Write(string.Concat(now.ToString("HH:mm:ss"), ", "));
                        sw.WriteLine(message);
                    }
                }
                catch
                {
                }
            }
        }

        public static void DebugLog(string message)
        {
            lock (debugObject)
            {
                try
                {
                    string logDirectory = GetDebugDirectory();
                    DateTime now = DateTime.Now;
                    using (StreamWriter sw = new StreamWriter(string.Concat(logDirectory, "\\Debug", now.ToString("yyyyMMdd"), ".csv"), true))
                    {
                        now = DateTime.Now;
                        sw.Write(string.Concat(now.ToString("HH:mm:ss"), " "));
                        sw.WriteLine(message);
                    }
                }
                catch
                {
                }
            }
        }

        public static string GetLogDirectory()
        {
            string dir = "C:\\Log";
            Directory.CreateDirectory(dir);
            DateTime now = DateTime.Now;
            dir = string.Concat(dir, "\\", now.ToString("yyyyMMdd"));
            Directory.CreateDirectory(dir);
            return dir;
        }

        public static string GetDebugDirectory()
        {
            string dir = "C:\\Log";
            Directory.CreateDirectory(dir);
            DateTime now = DateTime.Now;
            dir = string.Concat(dir, "\\", now.ToString("yyyyMMdd"));
            Directory.CreateDirectory(dir);
            return dir;
        }

        public static void GDebugPattern(string dMsg, Mat pattern, GrayImage dontcare)
        {
            lock (debugObject)
            {
                try
                {
                    string dir = string.Concat(LogActivities.GetDebugDirectory(), "\\Images");
                    Directory.CreateDirectory(dir);
                    DateTime now = DateTime.Now;
                    string fn = string.Concat(dir, "\\", now.ToString("HH-mm-ss"), dMsg);
                    Cv2.ImWrite(string.Concat(fn, "-P.jpg"), pattern);
                    dontcare.Save(string.Concat(fn, "-D.jpg"), ImageFormat.Jpeg);
                }
                catch
                {
                }
            }
        }

        public static void RemoveAgedFiles(int days)
        {
            string logDir = "C:\\Log";
            DateTime dt = DateTime.Now.AddDays((double)(-days));
            string datestr = dt.ToString("yyyyMMdd");
            try
            {
                string[] directories = Directory.GetDirectories(logDir);
                for (int i = 0; i < (int)directories.Length; i++)
                {
                    string d = directories[i];
                    string dir = Path.GetFileNameWithoutExtension(d);
                    if (dir.Length == 8)
                    {
                        if (dir.CompareTo(datestr) < 0)
                        {
                            Directory.Delete(d, true);
                        }
                    }
                }
            }
            catch
            {
            }
        }

        public static void GDebugMatch(string dMsg, Mat match, ref MatchPosition mPos)
        {
            lock (debugObject)
            {
                Mat colorImage = null;
                try
                {
                    // 將灰階圖像轉換為彩色圖像（BGR 格式）
                    colorImage = new Mat();
                    Cv2.CvtColor(match, colorImage, ColorConversionCodes.GRAY2BGR);

                    // 先在圖像上畫一個十字線
                    OpenCvSharp.Point center = new OpenCvSharp.Point((int)mPos.X, (int)mPos.Y);

                    // 設定十字線的顏色和粗細
                    Scalar crossColor = new Scalar(0, 255, 0); // 綠色
                    int crossThickness = 2;

                    // 畫水平線
                    // 線長為圖的5分之1 
                    int ixLong = match.Width / 10;
                    int iyLong = match.Height / 10;

                    Cv2.Line(colorImage, new OpenCvSharp.Point(center.X - ixLong, center.Y), new OpenCvSharp.Point(center.X + ixLong, center.Y), crossColor, crossThickness);

                    // 畫垂直線
                    Cv2.Line(colorImage, new OpenCvSharp.Point(center.X, center.Y - iyLong), new OpenCvSharp.Point(center.X, center.Y + iyLong), crossColor, crossThickness);

                    // 顯示 score 在十字線上方
                    string scoreText = $"Score: {mPos.Score:F2}"; // 格式化 score 顯示兩位小數
                    HersheyFonts fontFace = HersheyFonts.HersheySimplex; // 使用簡單字型
                    double fontScale = 5; // 字體大小
                    int fontThickness = 3;  // 字體粗細
                    Scalar textColor = new Scalar(255, 0, 0); // 藍色文字

                    // 計算文字在圖像中的位置，位於十字線的上方
                    OpenCvSharp.Point textPosition = new OpenCvSharp.Point(center.X - ixLong, center.Y - iyLong); // 讓文字稍微偏離中心點

                    // 將 score 顯示在圖像上
                    Cv2.PutText(colorImage, scoreText, textPosition, fontFace, fontScale, textColor, fontThickness);

                    string dir = string.Concat(LogActivities.GetDebugDirectory(), "\\Images");
                    Directory.CreateDirectory(dir);
                    DateTime now = DateTime.Now;
                    string fn = string.Concat(dir, "\\", now.ToString("HH-mm-ss"), dMsg);
                    Cv2.ImWrite(string.Concat(fn, "-M.jpg"), colorImage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Image Debug failed: {ex.Message}");
                }
                finally
                {
                    // 確保在結束後釋放 colorImage 資源
                    colorImage?.Dispose();
                }
            }
        }

    }
}
