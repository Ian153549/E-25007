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
    public partial class DialogCalculator : Form
    {
        public bool IsDouble = false;
        public bool IsInt = false;
        public double NumberDouble;
        public int NumberInt;

        public DialogCalculator()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBoxNumber.Text += "1";

        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBoxNumber.Text += "2";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBoxNumber.Text += "3";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBoxNumber.Text += "4";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBoxNumber.Text += "5";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBoxNumber.Text += "6";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBoxNumber.Text += "7";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            textBoxNumber.Text += "8";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            textBoxNumber.Text += "9";
        }

        private void button10_Click(object sender, EventArgs e)
        {
            textBoxNumber.Text += "0";
        }

        private void button11_Click(object sender, EventArgs e)
        {
            textBoxNumber.Text += ".";
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (textBoxNumber.Text.Length > 0)
                textBoxNumber.Text = textBoxNumber.Text.Substring(0, textBoxNumber.Text.Length - 1);
        }


        
        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBoxNumber.Text.Contains("."))
                {
                    NumberDouble = Convert.ToDouble(textBoxNumber.Text);
                    IsDouble = true;
                    Close();
                    return;
                }
                IsInt = true;
                NumberInt = Convert.ToInt32(textBoxNumber.Text);
                Close();
            }
            catch
            {
                throw;
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            textBoxNumber.Text += "-";
        }
    }
}
