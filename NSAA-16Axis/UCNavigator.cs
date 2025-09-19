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
    public partial class UCNavigator : UserControl
    {
        public UCNavigator()
        {
            InitializeComponent();
        }

        private void BtRight_Click(object sender, EventArgs e)
        {
            if (CommandPressed != null)
            {
                CommandPressed(0);
            }
        }

        private void BtLeft_Click(object sender, EventArgs e)
        {
            if (CommandPressed != null)
            {
                CommandPressed(2);
            }
        }

        private void BtDown_Click(object sender, EventArgs e)
        {
            if (CommandPressed != null)
            {
                CommandPressed(3);
            }
        }

        private void BtUp_Click(object sender, EventArgs e)
        {
            if (CommandPressed != null)
            {
                CommandPressed(1);
            }
        }

        public event UCNavigator.CommandPressedEvent CommandPressed;

        public delegate void CommandPressedEvent(int dir);

    }
}
