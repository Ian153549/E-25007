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
    public partial class DialogManualPlcCom : Form
    {

        PlcCommunication Plc = new PlcCommunication();
        public DialogManualPlcCom()
        {
            InitializeComponent();
            textBoxIp.Text = GV.AppSettingParm.PlcIp;
            numericUpDownPort.Value = 8501;
            if (GV.AppSettingParm.Language != "default")
            {
                this.Text = GV.Dlang.frmManualPLCCom;
                IPAddress.Text = GV.Dlang.fIPAddress;
                PLCPort.Text = GV.Dlang.fPLCPort;
                buttonConnect.Text = GV.Dlang.fConnect;
                fWrite.Text = GV.Dlang.fWrite;
                buttonSend.Text = GV.Dlang.fSend;
                fRead.Text = GV.Dlang.fRead;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
               Plc.Open(textBoxIp.Text, (int)numericUpDownPort.Value);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            buttonConnect.Text = "Connected";
            buttonConnect.Enabled = false;
            
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (Plc.IsConnected())
                    //textBoxReadMsg.Text = "Write : " + textBoxSendMsg.Text + Environment.NewLine + "Read : " + Plc.Send(textBoxSendMsg.Text);
                    textBoxReadMsg.Text = "Write : " + textBoxSendMsg.Text + Environment.NewLine + "Read : " + Plc.McSend(textBoxSendMsg.Text);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DialogManualPlcCom_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Plc.IsConnected())
                Plc.Close();
        }
    }
}
