using OpenCvSharp;
using MRLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace NSAA_16Axis
{
    public partial class DialogPaintMask : Form
    {
        public Mat Image;

        public GrayImage Mask;

        public DialogPaintMask()
        {
            InitializeComponent();
            // L.ReplaceControlText(this);
        }

        private void BtOK_Click(object sender, EventArgs e)
        {
            Mask = ucGrayPaintMask.GetMask();
        }

        private void BtCancel_Click(object sender, EventArgs e)
        {

        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            //byte[] numArray = new byte[Image.Width * Image.Height];
            //Image.CopyTo<byte>(numArray);
            Timer1.Enabled = false;
            if (Image == null || Image.Empty()) return;
            if(Image.Type()!=MatType.CV_8UC1) throw new InvalidOperationException("Image 必須為單通道 CV_8UC1");

            int width = Image.Width;
            int height = Image.Height;
            int step = (int)Image.Step();
            int size = width * height;
            byte[] buffer = new byte[size];
            IntPtr src= Image.Data;
            for(int y = 0; y < height; y++)
            {
                Marshal.Copy(src + y * step, buffer, y * width, width);
            }
            GrayImage grayImage = new GrayImage(buffer, Image.Width, Image.Height);
            ucGrayPaintMask.SetBackImage(grayImage, Mask);
        }

        private void UdPenSize_ValueChanged(object sender, EventArgs e)
        {
            ucGrayPaintMask.PenSize = (int)UdPenSize.Value;
        }

        private void CbControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            ucGrayPaintMask.Tool = (UCGrayPaintMask.EnumTool)CbControl.SelectedIndex;
        }

        private void DialogPaintMask_Load(object sender, EventArgs e)
        {
            ucGrayPaintMask.PenSize = (Image.Height + Image.Width) / 10;
            UdPenSize.Value = ucGrayPaintMask.PenSize;
            CbControl.SelectedIndex = 1;
            Timer1.Enabled = true;
        }

        private void BTClear_Click_1(object sender, EventArgs e)
        {
            if (Mask == null || Image == null || Image.Empty()) return;
            Mask.Fill(255);
            if(Image.Type() != MatType.CV_8UC1) throw new InvalidOperationException("Image 必須為單通道 CV_8UC1");
            
            int width = Image.Width;
            int height = Image.Height;
            int step = (int)Image.Step();
            int size = width * height;
            byte[] buffer = new byte[size];
            IntPtr src = Image.Data;
            for(int y = 0; y < height; y++)
            {
                Marshal.Copy(src + y * step, buffer, y * width, width);
            }
            //byte[] numArray = new byte[Image.Width * Image.Height];
            //Image.CopyTo<byte>(numArray);
            GrayImage grayImage = new GrayImage(buffer, Image.Width, Image.Height);
            ucGrayPaintMask.SetBackImage(grayImage, Mask);
        }
    }
}
