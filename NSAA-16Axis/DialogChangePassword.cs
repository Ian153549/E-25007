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
    public partial class DialogChangePassword : Form
    {
        public enum PasswordType { Engineer, ProductionMode}
        public PasswordType Type;
        public DialogChangePassword(PasswordType type)
        {
            InitializeComponent();
            Type = type;
            if (GV.AppSettingParm.Language != "default")
            {
                lbOldPassword.Text = GV.Dlang.flbOldPassword;
                lbNewPassword.Text = GV.Dlang.flbNewPassword;
                lbVerify.Text = GV.Dlang.flbVerify;
                btnCancel.Text = GV.Dlang.fbtCancel;
                btnConfirm.Text = GV.Dlang.fbtnConfirm;
            }
        }

        private void ButtonConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                if (Type == PasswordType.Engineer)
                    if (textBoxOldPassword.Text == GV.AppSettingParm.EngineerPassword)
                        if (textBoxNewPassword.Text == textBoxVerifyPassword.Text)
                        {
                            GV.AppSettingParm.EngineerPassword = textBoxNewPassword.Text;
                            GM.WriteAppSettingParmXml("AppSettingParm.xml", GV.AppSettingParm);
                            MessageBox.Show("Success.");
                            this.Close();
                        }
                        else
                            throw new MRException("Fail.");
                    else
                        throw new MRException("Fail.");

                if (Type == PasswordType.ProductionMode)
                    if (textBoxOldPassword.Text == GV.AppSettingParm.ProductionModePassword)
                        if (textBoxNewPassword.Text == textBoxVerifyPassword.Text)
                        {
                            GV.AppSettingParm.ProductionModePassword = textBoxNewPassword.Text;                            
                            GM.WriteAppSettingParmXml("AppSettingParm.xml", GV.AppSettingParm);
                            MessageBox.Show("Success.");
                            this.Close();
                        }  
                        else
                            throw new MRException("Fail.");
                    else
                        throw new MRException("Fail.");
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
