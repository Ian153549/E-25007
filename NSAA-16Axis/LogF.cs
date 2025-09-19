using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NSAA_16Axis
{
    class LogF
    {
        private static object _locker = new object();

        public static void WriteLog1(string strLog)
        {
            StreamWriter log;
            FileStream fileStream = null;
            DirectoryInfo logDirInfo = null;
            FileInfo logFileInfo;

            lock (_locker)
            {
                string logFilePath = Application.StartupPath + "\\Logs\\";
                logFilePath = logFilePath + "Log-" + DateTime.Now.ToString("yyyy-MM-dd-HH") + "." + "txt";
                logFileInfo = new FileInfo(logFilePath);
                logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
                if (!logDirInfo.Exists) logDirInfo.Create();
                if (!logFileInfo.Exists)
                {
                    fileStream = logFileInfo.Create();
                }
                else
                {
                    fileStream = new FileStream(logFilePath, FileMode.Append);
                }
                log = new StreamWriter(fileStream);
                log.WriteLine(strLog);
                log.Close();
            }
        }

        public static void WriteLog(String strlog)
        {
            String slog;

            String dattime = DateTime.Now.ToString("yyyy-MM-dd:HH:mm:ss ");

            slog = dattime + strlog;

            WriteLog1(slog);
        }
    }
}
