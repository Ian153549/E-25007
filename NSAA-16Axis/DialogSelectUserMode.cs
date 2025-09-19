using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SemiAutomaticAligner
{
    public partial class DialogSelectUserMode : Form
    {
        public DialogSelectUserMode()
        {
            InitializeComponent();
        }

        private void ButtonEducationalMode_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ButtonProductionMode_Click(object sender, EventArgs e)
        {
            DialogLogIn dialog = new DialogLogIn(DialogLogIn.UseEnum.Mode);
            dialog.StartPosition = FormStartPosition.CenterScreen;
            dialog.ShowDialog();
            if (GV.ModeSelected == GV.Mode.Production)
            {
                this.Close();
            }
               
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            GM.Quit();
            if (!GV.AppEnding)
                return;
            else
                Close();
        }
    }
}
