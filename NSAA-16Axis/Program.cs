using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NSAA_16Axis
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            String name = Process.GetCurrentProcess().ProcessName;

            Process[] Parray = Process.GetProcessesByName(name);

            int process_delete = -1;

            if (Parray.Length > 1)
            {
                DateTime start_time_0 = Parray[0].StartTime;
                DateTime start_time_1 = Parray[1].StartTime;
                int DateTime_result = DateTime.Compare(start_time_0, start_time_1);

                if (DateTime_result < 0)
                {
                    process_delete = 0;
                }

                if (DateTime_result > 0)
                {
                    process_delete = 1;
                }

                if (process_delete != -1)
                {
                    Parray[process_delete].Kill();
                }
            }

            Application.Run(new FormMain());
        }
    }
}
