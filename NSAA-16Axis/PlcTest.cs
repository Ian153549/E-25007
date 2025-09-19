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
    public partial class PlcTest : Form
    {
        public PlcTest()
        {
            InitializeComponent();
        }

        private void BtNowRecipeNumber_Click(object sender, EventArgs e)
        {
            short iRec = Convert.ToInt16(txtNowRecipeNumber.Text);
            GV.Plc.MxSetData(GV.Plc.iNowRecipeNumber, iRec);
        }

        private void BtDownAlign_Click(object sender, EventArgs e)
        {
            short iRec = 0;
            bool bDown = Int16.TryParse(txtDownAlign.Text, out iRec);
            
            if (bDown && (iRec == 1))
            {
                // GV.Plc.MxSetDataM(GV.Plc.iDownAlign, 1);
                // GV.Plc.MxSetDataM(GV.Plc.iUpAlign, 0);
                txtDownAlign.Text = "0";
                GV.Plc.MxSetData(13030, 1);
            }
            else
            {
                // GV.Plc.MxSetDataM(GV.Plc.iDownAlign, 0);
                txtDownAlign.Text = "1";
                GV.Plc.MxSetData(13030, 0);
            }
        }

        private void btUpAlign_Click(object sender, EventArgs e)
        {
            short iRec = 0;
            bool bUp = Int16.TryParse(txtAlignMode.Text, out iRec);

            if (bUp && (iRec == 1))
            {
                GV.Plc.MxSetDataM(GV.Plc.iUpAlign, 1);
                GV.Plc.MxSetDataM(GV.Plc.iDownAlign, 0);
                txtDownAlign.Text = "0";
            }
            else
            {
                GV.Plc.MxSetDataM(GV.Plc.iDownAlign, 0);
                txtAlignMode.Text = "0";
            }
        }

        private void btCapture_Click(object sender, EventArgs e)
        {
            short iRec = 0;
            bool bCapture = Int16.TryParse(txtCapture.Text, out iRec);
            
            if (bCapture && (iRec == 1))
            {
                GV.Plc.MxSetDataM(GV.Plc.iCapture, 1);
            }
            else
            {
                GV.Plc.MxSetDataM(GV.Plc.iCapture, 0);
                txtCapture.Text = "0";
            }
        }

        private void btTabEnable_Click(object sender, EventArgs e)
        {
            GV.Plc.MxSetDataM(GV.Plc.iTabEnable, 0);
        }

        private void btTabDisenable_Click(object sender, EventArgs e)
        {
            GV.Plc.MxSetDataM(GV.Plc.iTabEnable, 1);
        }

        private void btUpStart_Click(object sender, EventArgs e)
        {
            GV.Plc.MxSetDataM(GV.Plc.iUpCCDStart, 1);
            txtUpCCDStatus.Text = "1";
        }

        private void btUpEnd_Click(object sender, EventArgs e)
        {
            GV.Plc.MxSetDataM(GV.Plc.iUpCCDStart, 0);
            txtUpCCDStatus.Text = "0";
        }

        private void btStopAlign_Click(object sender, EventArgs e)
        {
            btStopAlign.Enabled = false;
            GV.Plc.MxSetDataM(GV.Plc.iStopAlign, 1);
            Thread.Sleep(1000);
            GV.Plc.MxSetDataM(GV.Plc.iStopAlign, 0);
            btStopAlign.Enabled = true;
        }

        private void M13415_Click(object sender, EventArgs e)
        {
            if (M13415.Checked)
            {
                GV.Plc.MxSetDataM(GV.Plc.iExposure, 1);
            } else
            {
                GV.Plc.MxSetDataM(GV.Plc.iExposure, 0);
            }
        }

        private void M13420_Click(object sender, EventArgs e)
        {
            if (M13420.Checked)
            {
                GV.Plc.MxSetDataM(GV.Plc.iCapture, 1);
            } else
            {
                GV.Plc.MxSetDataM(GV.Plc.iCapture, 0);
            }
        }

        private void M13422_CheckedChanged(object sender, EventArgs e)
        {
            if (M13422.Checked)
            {
                GV.Plc.MxSetDataM(GV.Plc.iRunOver, 1);
            } else
            {
                GV.Plc.MxSetDataM(GV.Plc.iRunOver, 0);
            }
        }

        private void BtReadUDAlign_Click(object sender, EventArgs e)
        {
            int i = GV.Plc.ReadData16(GV.Plc.iUpDownAlign);
            txtDownAlign.Text = i.ToString();
        }

        private void PlcTest_Load(object sender, EventArgs e)
        {
            GV.Plc.MxSetData(13030, 1);
            GV.Plc.MxSetData(GV.Plc.iHmi, 1);
            GV.Plc.MxSetData(GV.Plc.iAlignMode, 0);
        }

        private void BtAlignMode_Click(object sender, EventArgs e)
        {
            bool bDown = Int16.TryParse(txtAlignMode.Text, out short iRec);

            if (bDown)
            {
                if ((iRec > -1) && (iRec < 3))
                {
                    GV.Plc.MxSetData(GV.Plc.iAlignMode, iRec);
                } else
                {
                    txtAlignMode.Text = "0";
                    GV.Plc.MxSetData(GV.Plc.iAlignMode, 0);
                }
            }
            else
            {
                txtAlignMode.Text = "0";
                GV.Plc.MxSetData(GV.Plc.iAlignMode, 0);
            }
        }
    }
}
