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
    public partial class CMZCalibration : UserControl
    {
        public CMZCalibration()
        {
            InitializeComponent();
            ucButton1.Caption = "Scan Wafer";
            ucButton1.FaceColor = Color.DarkKhaki;
            cbPatternAlgorithm.SelectedIndex = 0;         
        }

        private void ucButton1_Click(object sender, EventArgs e)
        {

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void MKey_CheckedChanged(object sender, EventArgs e)
        {
            if (rbBLDownKey.Checked)
            {
                gbMeasurePattern.Text = "Back Left Down Key";
            } else if (rbBLUpKey.Checked)
            {
                gbMeasurePattern.Text = "Back Left Up Key";
            }
            else if (rbBRDownKey.Checked)
            {
                gbMeasurePattern.Text = "Back Right Down Key";
            }
            else if (rbBRUpKey.Checked)
            {
                gbMeasurePattern.Text = "Back Right Up Key";

            }
            else if (rbTLDownKey.Checked) 
            {
                gbMeasurePattern.Text = "Top Left Down Key";

            }
            else if (rbTLUpKey.Checked) 
            {
                gbMeasurePattern.Text = "Top Left Up Key";

            }
            else if (rbTRDownKey.Checked)
            {
                gbMeasurePattern.Text = "Top Right Down Key";

            }
            else if (rbTRUpKey.Checked)
            {
                gbMeasurePattern.Text = "Top Right Up Key";

            }
        }

        private void cbLMaskAlgorithm_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
}
