using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NSAA_16Axis
{
    public partial class DialogUpdatingLog : Form
    {
        public DialogUpdatingLog()
        {
            InitializeComponent();
            if (GV.AppSettingParm.Language != "default")
            {
                this.Text = GV.Dlang.frmUpdateLog;
            }
        }

        private void DialogUpdatingLog_Load(object sender, EventArgs e)
        {
            TextBoxUpdatingLog.Text += "2019/11/15  新增教育模式及量產模式 By Ken" + Environment.NewLine; 
            TextBoxUpdatingLog.Text += "2019/12/02  新增防止重複啟動功能 By Ken" + Environment.NewLine;
            TextBoxUpdatingLog.Text += "2019/12/03  Change password dialog fixed By Ken" + Environment.NewLine;
            TextBoxUpdatingLog.Text += "2021/12/01  大澍加入語言檔設定" + Environment.NewLine;
        }

        private void TextBoxUpdatingLog_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
