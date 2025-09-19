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
    public partial class DialogAppSetting : Form
    {
        public DialogAppSetting()
        {
            InitializeComponent();

            checkBoxWheatherInitialLens.Checked = GV.AppSettingParm.EnableInitialZoomLensWhenStart;
            cbEnableContactShift.Checked = GV.AppSettingParm.EnableContactShiftCompensation;
            cbEnableAutoSearch.Checked = GV.AppSettingParm.EnableAutoSearch;
            cbDownThruHome.Checked = GV.AppSettingParm.EnableDownThruHome;
            cbEmulationMode.Checked = GV.AppSettingParm.Emulation;
            numericUpDown3By3SearchingSteps.Value = GV.AppSettingParm.SearchingSteps;
            cBPLCStatus.Checked = GV.AppSettingParm.PlcStatusEnable;

            if (GV.AppSettingParm.Language != "default")
            {
                this.Text = GV.Dlang.frmAppSetting;
                checkBoxWheatherInitialLens.Text = GV.Dlang.fcbWheatherInitialLens;
                cbDownThruHome.Text = GV.Dlang.fcbDownThruHome;
                cbEnableContactShift.Text = GV.Dlang.fcbEnableContactShift;
                cbEnableAutoSearch.Text = GV.Dlang.fcbEnableAutoSearch;
                btnAbout.Text = GV.Dlang.fbtnAbout;
                lbSearchSteps.Text = GV.Dlang.flbSearchSteps;
                btnChangeEngineerPassword.Text = GV.Dlang.fbtnChangeEngineerPassword;
                btnChangeProductionModePassword.Text = GV.Dlang.fbtnChangeProductionPassword;
                cbEmulationMode.Text = GV.Dlang.fcbEmulationMode;
                btOK.Text = GV.Dlang.strbtOK;
                btCancel.Text = GV.Dlang.fbtCancel;
                cBPLCStatus.Text = GV.Dlang.strPLCStatus;
                gbLanguage.Text = GV.Dlang.strLanguageSelect;
                rbDefault.Text = GV.Dlang.strEnglish;
                rbCustom.Text = GV.Dlang.strTChinese;
                rbCChinese.Text = GV.Dlang.strSChinese;
                if (GV.AppSettingParm.Language == "CChinese")
                {
                    rbCChinese.Checked = true;
                }
                else if (GV.AppSettingParm.Language == "TChinese")
                {
                    rbCustom.Checked = true;
                }
                else
                {
                    rbDefault.Checked = true;
                }
            } else
            {
                rbDefault.Checked = true;
            }
        }

        private void CheckBoxWheatherInitialLens_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxWheatherInitialLens.CheckState == CheckState.Checked)
                GV.AppSettingParm.EnableInitialZoomLensWhenStart = true;
            else
                GV.AppSettingParm.EnableInitialZoomLensWhenStart = false;
        }

        private void CheckBoxEnableContactShift_CheckedChanged(object sender, EventArgs e)
        {
            if (cbEnableContactShift.CheckState == CheckState.Checked)
                GV.AppSettingParm.EnableContactShiftCompensation = true;
            else
                GV.AppSettingParm.EnableContactShiftCompensation = false;
        }

        private void CheckBoxEnableAutoSearch_CheckedChanged(object sender, EventArgs e)
        {
            if (cbEnableAutoSearch.CheckState == CheckState.Checked)
                GV.AppSettingParm.EnableAutoSearch = true;
            else
                GV.AppSettingParm.EnableAutoSearch = false;
        }

        private void checkBoxDownThruHome_CheckedChanged(object sender, EventArgs e)
        {
            if (cbDownThruHome.Checked)
            {
                GV.AppSettingParm.EnableDownThruHome = true;
                // GV.LeftZoomLens.SetDownThruHome(true);
                // GV.RightZoomLens.SetDownThruHome(true);
            }
            else
            {
                GV.AppSettingParm.EnableDownThruHome = false;
                // GV.LeftZoomLens.SetDownThruHome(false);
                // GV.RightZoomLens.SetDownThruHome(false);
            }
        }
        private void DialogAppSetting_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void ChanegePassword(object sender, EventArgs e)
        {
            if (btnChangeEngineerPassword == (Button)sender)
            {
                DialogChangePassword d = new DialogChangePassword(DialogChangePassword.PasswordType.Engineer);
                d.StartPosition = FormStartPosition.CenterScreen;
                d.ShowDialog();
            }
            if (btnChangeProductionModePassword == (Button)sender)
            {
                DialogChangePassword d = new DialogChangePassword(DialogChangePassword.PasswordType.ProductionMode);
                d.StartPosition = FormStartPosition.CenterScreen;
                d.ShowDialog();
            }
        }

        private void ButtonAbout_Click(object sender, EventArgs e)
        {
            DialogUpdatingLog d = new DialogUpdatingLog();
            d.StartPosition = FormStartPosition.CenterScreen;
            d.ShowDialog();
        }

        private void EmulationMode_CheckedChanged(object sender, EventArgs e)
        {
            if (cbEmulationMode.Checked)
            {
                GV.AppSettingParm.Emulation = true;
                if (GV.AppSettingParm.ProgramMode == false)
                {
                    GV.AppSettingParm.BackSideCamEnable = false;
                    GV.AppSettingParm.LightComPortEnable = false;
                    GV.AppSettingParm.RingLightPortEnable = false;
                    GV.AppSettingParm.LeftZoomLensEnable = false;
                    GV.AppSettingParm.RightZoomLensEnable = false;
                    GV.AppSettingParm.LeftUpCamEnable = false;
                    GV.AppSettingParm.RightUpCamEnable = false;
                    GV.AppSettingParm.LeftBackCamEnable = false;
                    GV.AppSettingParm.RightBackCamEnable = false;
                    GV.AppSettingParm.PlcEnable = false;
                }
            }
            else
            {
                GV.AppSettingParm.Emulation = false;
                if (GV.AppSettingParm.ProgramMode == false)
                {
                    GV.AppSettingParm.BackSideCamEnable = true;
                    GV.AppSettingParm.LightComPortEnable = true;
                    GV.AppSettingParm.RingLightPortEnable = true;
                    GV.AppSettingParm.LeftZoomLensEnable = true;
                    GV.AppSettingParm.RightZoomLensEnable = true;
                    GV.AppSettingParm.LeftUpCamEnable = true;
                    GV.AppSettingParm.RightUpCamEnable = true;
                    GV.AppSettingParm.LeftBackCamEnable = true;
                    GV.AppSettingParm.RightBackCamEnable = true;
                    GV.AppSettingParm.PlcEnable = true;
                }
            }
        }

        private void BtOK_Click(object sender, EventArgs e)
        {
            string Oldlang = GV.AppSettingParm.Language;

            if (rbDefault.Checked == true)
            {
                GV.AppSettingParm.Language = "default";
            }
            else if (rbCChinese.Checked == true)
            {
                GV.AppSettingParm.Language = "CChinese";
                GV.AppSettingParm.LanguageFile = "CChinese";
            }
            else if (rbCustom.Checked == true)
            {
                GV.AppSettingParm.Language = "TChinese";
                GV.AppSettingParm.LanguageFile = "TChinese";
            }
            // GV.AppSettingParm.SearchingSteps = (int)numericUpDown3By3SearchingSteps.Value;
            if (cBPLCStatus.Checked == true)
            {
                GV.AppSettingParm.PlcStatusEnable = true;
            } else
            {
                GV.AppSettingParm.PlcStatusEnable = false;
            }

            GM.WriteAppSettingParmXml("AppSettingParm.xml", GV.AppSettingParm);

            if (Oldlang != GV.AppSettingParm.Language)
            {
                if (rbDefault.Checked == true)
                {
                    GV.Dlang = new DisplayLanguage();
                    MessageBox.Show("Please Restart Program Change to English Language");
                }
                else
                {
                    GV.Dlang = GM.ReadLanguageFile(GV.AppSettingParm.LanguageFile + ".xml");
                    MessageBox.Show(GV.Dlang.ChangeLanguage);
                }

                // if (MessageBox.Show(GV.Dlang.btSureQuit, GV.Dlang.btnQuit, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                // MessageBox.Show(GV.Dlang.btSureQuit);
                {
                    //GV.LeftUpVideoOnAlignPage.Exit();
                    //GV.RightUpVideoOnAlignPage.Exit();
                    //GV.LeftUpVideoOnLearnPage.Exit();
                    //GV.RightUpVideoOnLearnPage.Exit();
                    //GV.LeftUpVideoOnParameterSettingPage.Exit();
                    //GV.RightUpVideoOnParameterSettingPage.Exit();
                    GV.AppEnding = true;
                    // WriteAlignConditionsAllListToXml("AlignConditions.xml");
                    GM.WriteAlignConditionsAcarToXml("AlignConditions.xml");
                    GV.Light.ChangeBrightness("left", 0);
                    GV.Light.ChangeBrightness("right", 0);
                    GV.Light.ChangeBrightness("leftback", 0);
                    GV.Light.ChangeBrightness("rightback", 0);
                    GV.RingLight.ChangeBrightness("left", 0);
                    GV.RingLight.ChangeBrightness("right", 0);
                    // Thread.Sleep(1000);

                    GV.LeftZoomLens.Close();
                    GV.RightZoomLens.Close();
                    GV.Plc.Close();
                    GV.Light.Close();
                    GV.RingLight.Close();

                    Environment.Exit(Environment.ExitCode);
                }
            }
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gbLanguage_Enter(object sender, EventArgs e)
        {

        }
    }
}
