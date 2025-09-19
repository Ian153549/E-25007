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
    public partial class DialogCycleTest : Form
    {
        public int CycleTestTarget = 0;
        public DialogCycleTest()
        {
            InitializeComponent();
        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {
            CycleTestTarget = (int)numericUpDownCycleTimes.Value;
            this.Close();
        }
    }
}
