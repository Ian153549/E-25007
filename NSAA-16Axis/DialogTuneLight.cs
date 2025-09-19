using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NSAA_16Axis
{
    public partial class DialogTuneLight : Form
    {
        public DialogTuneLight()
        {
            InitializeComponent();
            if (GV.bBacksideCCD == false)
            {
                lbRightBackLight.Visible = false;
                lbLeftBackLight.Visible = false;
                buttonLeftBackDecrease.Visible = false;
                buttonLeftBackIncrease.Visible = false;
                btnLeftBackLightSave.Visible = false;
                trackBarLightLeftBack.Visible = false;
                buttonRightBackDecrease.Visible = false;
                buttonRightBackIncrease.Visible = false;
                btnRightBackLightSave.Visible = false;
                trackBarLightRightBack.Visible = false;
            }

            if (GV.AppSettingParm.Language != "default")
            {
                this.Text = GV.Dlang.frmTuneLight;
                lbLeftLight.Text = GV.Dlang.frbLeftLens;
                lbRightLight.Text = GV.Dlang.frbRightLens;
                lbLeftBackLight.Text = GV.Dlang.radioButtonLeftBackCam;
                lbRightBackLight.Text = GV.Dlang.radioButtonRightBackCam;
                btnLeftLightSave.Text = GV.Dlang.strSave;
                btnRightLightSave.Text = GV.Dlang.strSave; 
                btnLeftBackLightSave.Text = GV.Dlang.strSave;
                btnRightBackLightSave.Text = GV.Dlang.strSave;
            }
        }

        private void trackBarLightChannel1_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                GV.Light.ChangeBrightness("left", trackBarLightLeft.Value);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void trackBarLightChannel2_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                GV.Light.ChangeBrightness("right", trackBarLightRight.Value);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void trackBarLightChannel3_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                GV.Light.ChangeBrightness("leftback", trackBarLightLeftBack.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void trackBarLightChannel4_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                GV.Light.ChangeBrightness("rightback", trackBarLightRightBack.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButtonLeftLightSave_Click(object sender, EventArgs e)
        {
            if (cBLeftMagnification.SelectedIndex == -1) return;

            if (MessageBox.Show("Sure to save light brightness ?", "SaveLeftLightBrightness", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // AlignCondition temp = GM.GetAlignCondition(GV.NowRecipeNumber);
                GV.acar[GV.NowRecipeNumber].LeftBrightness[cBLeftMagnification.SelectedIndex] = trackBarLightLeft.Value;
                GM.WriteAlignConditionsListToXml("AlignConditions.xml", GV.acar[GV.NowRecipeNumber], GV.NowRecipeNumber);
            }
            
        }

        private void ButtonRightLightSave_Click(object sender, EventArgs e)
        {
            if (comboBoxRightMagnification.SelectedIndex == -1) return;

            if (MessageBox.Show("Sure to save light brightness ?", "SaveRightLightBrightness", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // AlignCondition temp = GM.GetAlignCondition(GV.NowRecipeNumber);
                GV.acar[GV.NowRecipeNumber].RightBrightness[comboBoxRightMagnification.SelectedIndex] = trackBarLightRight.Value;
                GM.WriteAlignConditionsListToXml("AlignConditions.xml", GV.acar[GV.NowRecipeNumber], GV.NowRecipeNumber);
            }

        }

        private void DialogTuneLight_Load(object sender, EventArgs e)
        {
            try
            {
                trackBarLightLeftBack.Value = GV.Light.LBNowBrightness;
                trackBarLightRightBack.Value = GV.Light.RBNowBrightness;
                trackBarLightLeft.Value = GV.Light.LNowBrightness;
                trackBarLightRight.Value = GV.Light.RNowBrightness;
                tBLeftRingLight.Value = GV.RingLight.LNowBrightness;
                tBRightRingLight.Value = GV.RingLight.RNowBrightness;
                cBLeftMagnification.SelectedIndex = GV.LeftZoomLens.Magnification;
                comboBoxRightMagnification.SelectedIndex = GV.RightZoomLens.Magnification;
                cBLeftRingMagnification.SelectedIndex = GV.LeftZoomLens.Magnification;
                cBRightRingMagnification.SelectedIndex = GV.RightZoomLens.Magnification;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void buttonLeftIncrease_Click(object sender, EventArgs e)
        {
            if (GV.Light.LNowBrightness == 255) return;
            
            GV.Light.ChangeBrightness("left", GV.Light.LNowBrightness + 1);
            trackBarLightLeft.Value++;
            
        }

        private void buttonLeftDecrease_Click(object sender, EventArgs e)
        {
            if (GV.Light.LNowBrightness == 0) return;
            
            GV.Light.ChangeBrightness("left", GV.Light.LNowBrightness - 1);
            trackBarLightLeft.Value--;
            
        }

        private void buttonRightIncrease_Click(object sender, EventArgs e)
        {
            if (GV.Light.RNowBrightness == 255) return;
            
            GV.Light.ChangeBrightness("right", GV.Light.RNowBrightness + 1);
            trackBarLightRight.Value++;
            
        }

        private void buttonRightDecrease_Click(object sender, EventArgs e)
        {
            if (GV.Light.RNowBrightness == 0) return;
            
            GV.Light.ChangeBrightness("right", GV.Light.RNowBrightness - 1);
            trackBarLightRight.Value--;     
        }

        private void ComboBoxLeftMagnification_SelectedIndexChanged(object sender, EventArgs e)
        {
            trackBarLightLeft.Value = GV.Light.LNowBrightness;
            //GV.Light.ChangeBrightness("left", GM.GetAlignCondition(GV.NowRecipeNumber).LeftBrightness[comboBoxLeftMagnification.SelectedIndex]);
            GV.LeftZoomLens.MoveGoto(GV.ZoomLensInfo.LeftMagnificationMotorSteps, cBLeftMagnification.SelectedIndex);
        }

        private void ComboBoxRightMagnification_SelectedIndexChanged(object sender, EventArgs e)
        {
            trackBarLightRight.Value = GV.Light.RNowBrightness;
            //GV.Light.ChangeBrightness("right", GM.GetAlignCondition(GV.NowRecipeNumber).RightBrightness[comboBoxRightMagnification.SelectedIndex]);
            GV.RightZoomLens.MoveGoto(GV.ZoomLensInfo.RightMagnificationMotorSteps, comboBoxRightMagnification.SelectedIndex);
        }

        private void buttonLeftBackLightSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Sure to save light back brightness ?", "SaveLeftLightBackBrightness", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // AlignCondition temp = GM.GetAlignCondition(GV.NowRecipeNumber);
                GV.acar[GV.NowRecipeNumber].LBMaskBright = trackBarLightLeftBack.Value;
                GM.WriteAlignConditionsListToXml("AlignConditions.xml", GV.acar[GV.NowRecipeNumber], GV.NowRecipeNumber);
            }
        }

        private void buttonRightBackLightSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Sure to save light back brightness ?", "SaveRightLightBrightness", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // AlignCondition temp = GM.GetAlignCondition(GV.NowRecipeNumber);
                GV.acar[GV.NowRecipeNumber].RBMaskBright = trackBarLightRightBack.Value;
                GM.WriteAlignConditionsListToXml("AlignConditions.xml", GV.acar[GV.NowRecipeNumber], GV.NowRecipeNumber);
            }
        }

        private void tBLeftRingLight_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                GV.RingLight.ChangeBrightness("left", tBLeftRingLight.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tBRightRingLight_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                GV.RingLight.ChangeBrightness("right", tBRightRingLight.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
