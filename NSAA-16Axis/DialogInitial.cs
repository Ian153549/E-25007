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
    public partial class DialogInitial : Form
    {
        public DialogInitial()
        {
            InitializeComponent();
        }

        public void WriteToInitialTextBox(string message)
        {
            TextBoxInitial.Text += message + Environment.NewLine;
            Update();
        }
    }
}
