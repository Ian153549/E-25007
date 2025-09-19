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
    public partial class MessageBox1 : Form
    {
        //DialogResult thisok; 
        public MessageBox1()
        {
            InitializeComponent();
        }

        public MessageBox1(string btSureLogout, string btLogout, MessageBoxIcon question)
        {
            InitializeComponent();
            Text = btLogout;
            lbMessage.Text = btSureLogout;
            if (GV.AppSettingParm.Language != "default")
            {
                btOK.Text = GV.Dlang.strbtOK;
                btCancel.Text = GV.Dlang.fbtCancel;
            }
        }

        private void btOK_Click(object sender, EventArgs e)
        {   
            //thisok = DialogResult.OK;
            Close();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            //thisok = DialogResult.Cancel;
            Close();
        }
    }
}
