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
    public partial class PlcStatus : Form
    {
        public delegate void PlcReadMemory(int address, bool bCheck);
        public delegate void PlcReadData16(int address, int iData);
        public PlcReadMemory ReadMemoryDelegate;
        public PlcReadData16 ReadData16Delegate;

        public PlcStatus()
        {
            InitializeComponent();
            ReadMemoryDelegate = new PlcReadMemory(ReadMemoryMethod);
            ReadData16Delegate = new PlcReadData16(ReadData16Method);
        }

        public void ReadMemory(int address, bool bCheck)
        {
            BeginInvoke(ReadMemoryDelegate, new object[] { address, bCheck });
        }

        public void ReadData16(int address, int iData)
        {
            BeginInvoke(ReadData16Delegate, new object[] { address, iData });
        }

        public void ReadMemoryMethod(int address, bool bCheck)
        {
            //int iChange = 0;

            if (address == GV.Plc.iStopAlign)
            {
                M13425.Enabled = true;
                M13425.Checked = bCheck;
                M13425.ForeColor = Color.Red;
                //iChange = 1;
            }
            else if (address == GV.Plc.iSkipAlign)
            {
                M13410.Enabled = true;
                M13410.Checked = bCheck;
                M13410.ForeColor = Color.Red;
                //iChange = 1;
            }
            else if (address == GV.Plc.iUpAlign)
            {
                M13411.Enabled = true;
                M13411.Checked = bCheck;
                M13411.ForeColor = Color.Red;
                //iChange = 1;
            }
            else if (address == GV.Plc.iDownAlign)
            {
                M13412.Enabled = true;
                M13412.Checked = bCheck;
                M13412.ForeColor = Color.Red;
                //iChange = 1;
            }
            else if (address == GV.Plc.iExposure)
            {
                M13415.Enabled = true;
                M13415.Checked = bCheck;
                M13415.ForeColor = Color.Red;
                //iChange = 1;
            }
            else if (address == GV.Plc.iAutoRun)
            {
                M13417.Enabled = true;
                M13417.Checked = bCheck;
                M13417.ForeColor = Color.Red;
                //iChange = 1;
            }
            else if (address == GV.Plc.iEditRun)
            {
                M13418.Enabled = true;
                M13418.Checked = bCheck;
                M13418.ForeColor = Color.Red;
                //iChange = 1;
            }
            else if (address == GV.Plc.iCapture)
            {
                M13420.Enabled = true;
                M13420.Checked = bCheck;
                M13420.ForeColor = Color.Red;
                //iChange = 1;
            }
            else if (address == GV.Plc.iNewRun)
            {
                M13421.Enabled = true;
                M13421.Checked = bCheck;
                M13421.ForeColor = Color.Red;
                //iChange = 1;
            }
            else if (address == GV.Plc.iRunOver)
            {
                M13422.Enabled = true;
                M13422.Checked = bCheck;
                M13422.ForeColor = Color.Red;
                //iChange = 1;
            }
            else if (address == GV.Plc.iUpCCDStart)
            {
                M13000.Enabled = true;
                M13000.Checked = bCheck;
                M13000.ForeColor = Color.Red;
                //iChange = 1;
            }
            else if (address == GV.Plc.iUpCCDEnd)
            {
                M13001.Enabled = true;
                M13001.Checked = bCheck;
                M13001.ForeColor = Color.Red;
                //iChange = 1;
            }
            else if (address == GV.Plc.iTopContactOK)
            {
                M13002.Enabled = true;
                M13002.Checked = bCheck;
                M13002.ForeColor = Color.Red;
                //iChange = 1;
            }
            else if (address == GV.Plc.iDownCCDStart)
            {
                M13040.Enabled = true;
                M13040.Checked = bCheck;
                M13040.ForeColor = Color.Red;
                //iChange = 1;
            }
            else if (address == GV.Plc.iDownCCDEnd)
            {
                M13041.Enabled = true;
                M13041.Checked = bCheck;
                M13041.ForeColor = Color.Red;
                //iChange = 1;
            }
            else if (address == GV.Plc.iChuckStart)
            {
                M13020.Enabled = true;
                M13020.Checked = bCheck;
                M13020.ForeColor = Color.Red;
                //iChange = 1;
            }
            else if (address == GV.Plc.iChuckEnd)
            {
                M13021.Enabled = true;
                M13021.Checked = bCheck;
                M13021.ForeColor = Color.Red;
                //iChange = 1;
            }
            else if (address == GV.Plc.iBOTContactOK)
            {
                M13022.Enabled = true;
                M13022.Checked = bCheck;
                M13022.ForeColor = Color.Red;
                //iChange = 1;
            }

            Update();
            /*
            if (iChange == 1)
            {
                Update();
                Thread.Sleep(50);
                foreach(Control X in this.Controls)
                {
                    X.ForeColor = SystemColors.ControlText;
                    X.Enabled = false;
                }
            }
            */
        }

        public void ReadData16Method(int address, int iData)
        {
            //int iChange = 0;

            if (address == GV.Plc.iNowRecipeNumber)
            {
                D14942.Enabled = true;
                D14942.Text = iData.ToString();
                D14942.ForeColor = Color.Red;
                //iChange = 1;
            }
            else if (address == GV.Plc.iModifyRecipeNumber)
            {
                D14940.Enabled = true;
                D14940.Text = iData.ToString();
                D14940.ForeColor = Color.Red;
                //iChange = 1;
            }

            Update();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                foreach (Control X in this.Controls)
                {
                    X.ForeColor = SystemColors.ControlText;
                    X.Enabled = false;
                }
            }
            catch (Exception)
            {

            }
        }

        private void PlcStatus_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Stop();
            // Application.DoEvents();
        }
    }
}
