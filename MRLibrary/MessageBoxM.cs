using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace MRLibrary
{
    public partial class MessageBoxM : Form
    {
        private MessageBoxButtons Buttons;

        private MessageBoxIcon MsgIcon;

        public MessageBoxM(string msg, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            InitializeComponent();
            Text = caption;
            Buttons = buttons;
            MsgIcon = icon;
            label.Text = msg;
            // L.ReplaceControlText(this);
            if (buttons == MessageBoxButtons.OKCancel)
            {
                btCancel.Visible = true;
            }
        }

        private void plIcon_Paint(object sender, PaintEventArgs e)
        {
            MessageBoxIcon msgIcon = this.MsgIcon;
            if (msgIcon <= MessageBoxIcon.Question)
            {
                if (msgIcon == MessageBoxIcon.Hand)
                {
                    e.Graphics.DrawIcon(SystemIcons.Hand, 16, 16);
                }
                else if (msgIcon == MessageBoxIcon.Question)
                {
                    e.Graphics.DrawIcon(SystemIcons.Question, 16, 16);
                }
            }
            else if (msgIcon == MessageBoxIcon.Exclamation)
            {
                e.Graphics.DrawIcon(SystemIcons.Asterisk, 16, 16);
            }
            else if (msgIcon == MessageBoxIcon.Asterisk)
            {
                e.Graphics.DrawIcon(SystemIcons.Asterisk, 16, 16);
            }
        }

        public static System.Windows.Forms.DialogResult Show(string msg, string caption, MessageBoxButtons buttons = 0, MessageBoxIcon icon = (MessageBoxIcon)64)
        {
            return (new MessageBoxM(msg, caption, buttons, icon)).ShowDialog();
        }
    }
}
