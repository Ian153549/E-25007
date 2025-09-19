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
    public partial class DialogLogIn : Form
    {
        public enum UseEnum {Permission, Mode};
        public UseEnum Use;
        public DialogLogIn(UseEnum use)
        {
            InitializeComponent();
            Use = use;
            if (GV.AppSettingParm.Language != "default")
            {
                this.Text = GV.Dlang.frmLogin;
                btLogIn.Text = GV.Dlang.fbtLogin;
                btCancel.Text = GV.Dlang.fbtCancel;
                btBack.Text = GV.Dlang.fbtBack;
            }
        }

        private void bt1_Click(object sender, EventArgs e)
        {
            tbPassword.Text += "1";
        }

        private void bt2_Click(object sender, EventArgs e)
        {
            tbPassword.Text += "2";
        }

        private void bt3_Click(object sender, EventArgs e)
        {
            tbPassword.Text += "3";
        }

        private void bt4_Click(object sender, EventArgs e)
        {
            tbPassword.Text += "4";
        }

        private void bt5_Click(object sender, EventArgs e)
        {
            tbPassword.Text += "5";
        }

        private void bt6_Click(object sender, EventArgs e)
        {
            tbPassword.Text += "6";
        }

        private void bt7_Click(object sender, EventArgs e)
        {
            tbPassword.Text += "7";
        }

        private void bt8_Click(object sender, EventArgs e)
        {
            tbPassword.Text += "8";
        }

        private void bt9_Click(object sender, EventArgs e)
        {
            tbPassword.Text += "9";
        }

        private void bt0_Click(object sender, EventArgs e)
        {
            tbPassword.Text += "0";
        }

        private void btBack_Click(object sender, EventArgs e)
        {
            if (tbPassword.Text.Length > 0)
                tbPassword.Text = tbPassword.Text.Substring(0, tbPassword.Text.Length - 1);
        }

        private void btLogIn_Click(object sender, EventArgs e)
        {
            if (Use == UseEnum.Permission)
            {
                if (tbPassword.Text == GV.AppSettingParm.EngineerPassword)
                    GV.UserLevel = GV.User.Engineer;
                if (tbPassword.Text == GV.AppSettingParm.AdministratorPassword)
                    GV.UserLevel = GV.User.Administrator;

                if (GV.UserLevel == GV.User.Operator)
                    MessageBox.Show(GV.Dlang.strWrongPassword, "LogIn", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    this.Close();
            }
            

            if (Use == UseEnum.Mode)
            {
                if (tbPassword.Text == GV.AppSettingParm.ProductionModePassword)
                {
                    GV.ModeSelected = GV.Mode.Production;
                    this.Close();
                }
                else
                    MessageBox.Show("Wrong Password.", "LogIn", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ButtonSelectMode_Click(object sender, EventArgs e)
        {
            
        }
    }
}
