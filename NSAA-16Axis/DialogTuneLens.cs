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
using MRLibrary;
using OpenCvSharp.Extensions;

namespace NSAA_16Axis
{
    public partial class DialogTuneLens : Form
    {
        Image MeasureImage;
        Point MeasureLineStartPt;
        Point MeasureLineEndPt;
        double OriginalLinePixelLength;


        public DialogTuneLens()
        {
            InitializeComponent();
            for (int i = 1; i < 13; i++)
                comboBoxGoto.Items.Add("X" + i.ToString());

            comboBoxGoto.SelectedIndex = 0;

            skZoomAndPanWindowMeasure.CanZoom = true;
            skZoomAndPanWindowMeasure.CanZoom = true;
            numericUpDownPatternWidth.Value = GV.ZoomLensInfo.PatternWidth;
            pictureBoxMeasure.Visible = false;
            gbParameter.Enabled = false;
            rbLive.Checked = true;
            if (GV.AppSettingParm.Language != "default")
            {
                this.Text = GV.Dlang.frmTuneLens;
                rbLeftLens.Text = GV.Dlang.frbLeftLens;
                rbRightLens.Text = GV.Dlang.frbRightLens;
                gbParameter.Text = GV.Dlang.fgbParameter;
                lbMoveTo.Text = GV.Dlang.flbMoveGoto;
                lbPatternWidth.Text = GV.Dlang.flbPatternWidth;
                rbLive.Text = GV.Dlang.frbLive;
                rbMeasure.Text = GV.Dlang.frbMeasure;
                btnPreCalculate.Text = GV.Dlang.fbtnPreCalculate;
                btnCalAndSave.Text = GV.Dlang.fbtnCalAndSave;
                lbX1.Text = GV.Dlang.flbX1;
                lbX2.Text = GV.Dlang.flbX2;
                lbX3.Text = GV.Dlang.flbX3;
                lbX4.Text = GV.Dlang.flbX4;
                lbX5.Text = GV.Dlang.flbX5;
                lbX6.Text = GV.Dlang.flbX6;
                lbX7.Text = GV.Dlang.flbX7;
                lbX8.Text = GV.Dlang.flbX8;
                lbX9.Text = GV.Dlang.flbX9;
                lbX10.Text = GV.Dlang.flbX10;
                lbX11.Text = GV.Dlang.flbX11;
                lbX12.Text = GV.Dlang.flbX12;
            }
        }
        private void DialogTuneLens_Load(object sender, EventArgs e)
        {
            GV.RightUpCam.Freeze();
            GV.LeftUpCam.Freeze();
            GV.LeftUpCam.SetWindow(skZoomAndPanWindowMeasure);
            rbLeftLens.Checked = true;
            GV.LeftUpCam.Live();
        }

        private void rbLeftLens_CheckedChanged(object sender, EventArgs e)
        {
           if (rbLeftLens.Checked == true)
           {
                rbRightLens.Checked = false;
                tableLayoutPanel1.Enabled = false;
                rbMeasure.Checked = false;
                GV.RightUpCam.Freeze();
                GV.LeftUpCam.SetWindow(skZoomAndPanWindowMeasure);
                GV.LeftUpCam.Live();
                rbLive.Checked = true;
                gbParameter.Enabled = true;
                comboBoxGoto.SelectedIndex = GV.LeftZoomLens.Magnification;
           }
        }

        private void rbRightLens_CheckedChanged(object sender, EventArgs e)
        {
            if (rbRightLens.Checked == true)
            {
                rbLeftLens.Checked = false;
                rbMeasure.Checked = false;
                tableLayoutPanel1.Enabled = false;
                GV.LeftUpCam.Freeze();
                GV.RightUpCam.SetWindow(skZoomAndPanWindowMeasure);
                GV.RightUpCam.Live();
                rbLive.Checked = true;
                gbParameter.Enabled = true;
                comboBoxGoto.SelectedIndex = GV.RightZoomLens.Magnification;
            }
        }

        private void DialogTuneLens_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                GV.Plc.Send("WR MR8614 1");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void radioButtonMeasure_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMeasure.Checked == true)
            {
                rbLive.Checked = false;
                GV.LeftUpCam.Freeze();
                GV.RightUpCam.Freeze();
                comboBoxGoto.Enabled = false;
                MeasureImage = pictureBoxMeasure.Image = skZoomAndPanWindowMeasure.GetWindowImage().ToBitmap();
                tableLayoutPanel1.Enabled = true;
                skZoomAndPanWindowMeasure.Visible = false;
                skZoomAndPanWindowMeasure.Enabled = false;
                pictureBoxMeasure.Visible = true;
                pictureBoxMeasure.BringToFront();
                pictureBoxMeasure.Update();
            }
        }

        private void radioButtonLive_CheckedChanged(object sender, EventArgs e)
        {
            if (rbLive.Checked == true)
            {
                rbMeasure.Checked = false;
                comboBoxGoto.Enabled = true;
                tableLayoutPanel1.Enabled = false;
                skZoomAndPanWindowMeasure.Visible = true;
                skZoomAndPanWindowMeasure.Enabled = true;
                pictureBoxMeasure.Visible = false;
                pictureBoxMeasure.BringToFront();
                if (rbRightLens.Checked == true)
                {
                    GV.RightUpCam.Live();
                } else
                {
                    GV.LeftUpCam.Live();
                }
            }
        }

        private bool IsPictureBoxMeasureMouseDown = false;
        private void pictureBoxMeasure_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MeasureLineStartPt = e.Location;
                IsPictureBoxMeasureMouseDown = true;
            }
        }


        
        private void pictureBoxMeasure_MouseUp(object sender, MouseEventArgs e)
        {
            IsPictureBoxMeasureMouseDown = false;
            if (MeasureLineStartPt == MeasureLineEndPt) return;
            ZoomInfo zi = skZoomAndPanWindowMeasure.GetZoomInfo();
            OriginalLinePixelLength = (Math.Sqrt(Math.Pow((MeasureLineEndPt.X - MeasureLineStartPt.X) / zi.AftFitMagnificationX, 2) +
                Math.Pow((MeasureLineEndPt.Y - MeasureLineStartPt.Y) / zi.AftFitMagnificationY, 2)) / zi.ZoomRatio);
            Graphics g = pictureBoxMeasure.CreateGraphics();
            g.DrawString(OriginalLinePixelLength. ToString("F2"), new Font("Arial", 14), new SolidBrush(Color.Red), new PointF(MeasureLineEndPt.X + 10, MeasureLineEndPt.Y));
            SendKeys.Send(OriginalLinePixelLength.ToString("F2"));
        }

        private void pictureBoxMeasure_MouseMove(object sender, MouseEventArgs e)
        {
            Graphics g = pictureBoxMeasure.CreateGraphics();
            Pen pen = new Pen(Color.Red);

            MeasureLineEndPt = e.Location;

            if (IsPictureBoxMeasureMouseDown)
            {
                g.DrawImage(MeasureImage, new Point(0, 0));
                g.DrawLine(pen, MeasureLineStartPt, e.Location);
                Thread.Sleep(10);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Sure to calculate and save ?", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

            double[] zoomLensUmPerPixel = new double[10];
            zoomLensUmPerPixel[0] = (double)numericUpDownPatternWidth.Value / (double)numericUpDownX1.Value;
            zoomLensUmPerPixel[1] = (double)numericUpDownPatternWidth.Value / (double)numericUpDownX2.Value;
            zoomLensUmPerPixel[2] = (double)numericUpDownPatternWidth.Value / (double)numericUpDownX3.Value;
            zoomLensUmPerPixel[3] = (double)numericUpDownPatternWidth.Value / (double)numericUpDownX4.Value;
            zoomLensUmPerPixel[4] = (double)numericUpDownPatternWidth.Value / (double)numericUpDownX5.Value;
            zoomLensUmPerPixel[5] = (double)numericUpDownPatternWidth.Value / (double)numericUpDownX6.Value;
            zoomLensUmPerPixel[6] = (double)numericUpDownPatternWidth.Value / (double)numericUpDownX7.Value;
            zoomLensUmPerPixel[7] = (double)numericUpDownPatternWidth.Value / (double)numericUpDownX8.Value;
            zoomLensUmPerPixel[8] = (double)numericUpDownPatternWidth.Value / (double)numericUpDownX9.Value;
            zoomLensUmPerPixel[9] = (double)numericUpDownPatternWidth.Value / (double)numericUpDownX10.Value;

            foreach (var v in zoomLensUmPerPixel)
                if (double.IsNaN(v) || double.IsInfinity(v))
                {
                    MessageBox.Show("Wrong value. Please retype.", "Save", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

            GV.ZoomLensInfo.PatternWidth = (int)numericUpDownPatternWidth.Value;

            if (rbLeftLens.Checked)
            {
                GV.ZoomLensInfo.LeftUmPerPixelX = zoomLensUmPerPixel;
                GM.WriteZoomLensInfoToXml("ZoomLensInfo.xml", GV.ZoomLensInfo);
            }
            if (rbRightLens.Checked)
            {
                GV.ZoomLensInfo.RightUmPerPixelX = zoomLensUmPerPixel;
                GM.WriteZoomLensInfoToXml("ZoomLensInfo.xml", GV.ZoomLensInfo);
            }
        }

        private void buttonPrecalculate_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Sure to precalculate ? \n If you change the default pattern width , the result will be wrong."
                , "Precalculate", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            double[] zoomLensUmPerPixel = new double[10];
            if (rbLeftLens.Checked)
                zoomLensUmPerPixel = GV.ZoomLensInfo.LeftUmPerPixelX;
            if (rbRightLens.Checked)
                zoomLensUmPerPixel = GV.ZoomLensInfo.RightUmPerPixelX;

            numericUpDownX1.Value = numericUpDownPatternWidth.Value / (decimal)zoomLensUmPerPixel[0];
            numericUpDownX2.Value = numericUpDownPatternWidth.Value / (decimal)zoomLensUmPerPixel[1];
            numericUpDownX3.Value = numericUpDownPatternWidth.Value / (decimal)zoomLensUmPerPixel[2];
            numericUpDownX4.Value = numericUpDownPatternWidth.Value / (decimal)zoomLensUmPerPixel[3];
            numericUpDownX5.Value = numericUpDownPatternWidth.Value / (decimal)zoomLensUmPerPixel[4];
            numericUpDownX6.Value = numericUpDownPatternWidth.Value / (decimal)zoomLensUmPerPixel[5];
            numericUpDownX7.Value = numericUpDownPatternWidth.Value / (decimal)zoomLensUmPerPixel[6];
            numericUpDownX8.Value = numericUpDownPatternWidth.Value / (decimal)zoomLensUmPerPixel[7];
            numericUpDownX9.Value = numericUpDownPatternWidth.Value / (decimal)zoomLensUmPerPixel[8];
            numericUpDownX10.Value = numericUpDownPatternWidth.Value / (decimal)zoomLensUmPerPixel[9];

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxGoto.SelectedIndex > 0) 
            {
                if (rbLeftLens.Checked)
                {
                    GV.Light.ChangeBrightness("left", GV.NowAlignCondition.LeftBrightness[comboBoxGoto.SelectedIndex]);
                    GV.LeftZoomLens.MoveGoto(GV.ZoomLensInfo.LeftMagnificationMotorSteps, comboBoxGoto.SelectedIndex);

                }
                if (rbRightLens.Checked)
                {
                    GV.Light.ChangeBrightness("right", GV.NowAlignCondition.RightBrightness[comboBoxGoto.SelectedIndex]);
                    GV.RightZoomLens.MoveGoto(GV.ZoomLensInfo.RightMagnificationMotorSteps, comboBoxGoto.SelectedIndex);
                }
            }
        }
    }
}
