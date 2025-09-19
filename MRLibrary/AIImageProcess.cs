using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRLibrary
{
    public static class AIImageProcess
    {
        private const string AI_FAIL_FOLDER = @"C:\AIData";
        private const double AI_FAIL_THRESHOLD = 0.3;
        public static void SaveAIFailImage(Mat image, MatchPosition matchPosition)
        {
            //if (image == null || image.Empty())
            //    return;

            try
            {
                DateTime now = DateTime.Now;
                string dateFolder = Path.Combine(AI_FAIL_FOLDER, now.ToString("yyyyMMdd"));

                // 根據失敗類型決定子資料夾
                //string subFolder;
                //if (matchPosition == null || matchPosition.Score < 0.1)
                //{
                //    subFolder = Path.Combine(dateFolder, "Failed");
                //}
                //else
                //{
                //    subFolder = Path.Combine(dateFolder, "LowScore");
                //}

                // 建立資料夾
                //if (!Directory.Exists(subFolder))
                //{
                //    Directory.CreateDirectory(subFolder);
                //}

                // 生成檔案名稱
                //string fileName = string.Format(
                //    "{0}_Score{1:F2}_{2}.jpg",
                //    now.ToString("HHmmss_fff"),
                //    matchPosition?.Score ?? 0,
                //    reason.Replace(" ", "_")
                //);
                string fileName = string.Format(
                    "{0}.jpg",
                    now.ToString("HHmmss_fff")                    
                );
                string fullPath = Path.Combine(dateFolder, fileName);

                // 保存影像
                Cv2.ImWrite(fullPath, image);

                // 同時保存匹配資訊
                //SaveMatchInfo(fullPath, matchPosition, reason);
            }
            catch (Exception ex)
            {
                // 記錄錯誤但不中斷流程
                System.Diagnostics.Debug.WriteLine($"保存 AI 失敗圖片錯誤: {ex.Message}");
                LogActivities.DebugLog($"SaveAIFailImage Error: {ex.Message}");
            }
        }
        private static void SaveMatchInfo(string imagePath, MatchPosition matchPosition, string reason)
        {
            try
            {
                string infoPath = Path.ChangeExtension(imagePath, ".txt");

                StringBuilder info = new StringBuilder();
                info.AppendLine($"Time: {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
                info.AppendLine($"Image: {Path.GetFileName(imagePath)}");
                info.AppendLine($"Reason: {reason}");

                if (matchPosition != null)
                {
                    info.AppendLine($"Position: X={matchPosition.X:F2}, Y={matchPosition.Y:F2}");
                    info.AppendLine($"Score: {matchPosition.Score:F4}");
                    info.AppendLine($"Template Size: {matchPosition.TemplateSize.Width}x{matchPosition.TemplateSize.Height}");
                }
                else
                {
                    info.AppendLine("Match Position: NULL");
                }

                File.WriteAllText(infoPath, info.ToString());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"保存匹配資訊錯誤: {ex.Message}");
            }

        }
        public static void CleanupOldAIFailImages(int keepDays = 30)
        {
            try
            {
                if (!Directory.Exists(AI_FAIL_FOLDER))
                    return;

                DateTime cutoffDate = DateTime.Now.AddDays(-keepDays);
                string cutoffDateStr = cutoffDate.ToString("yyyyMMdd");

                var dateFolders = Directory.GetDirectories(AI_FAIL_FOLDER);

                foreach (var folder in dateFolders)
                {
                    string folderName = Path.GetFileName(folder);

                    // 檢查是否為日期資料夾格式
                    if (folderName.Length == 8 && int.TryParse(folderName, out _))
                    {
                        if (string.Compare(folderName, cutoffDateStr) < 0)
                        {
                            Directory.Delete(folder, true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"清理舊圖片錯誤: {ex.Message}");
            }
        }
    }
}
