using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using MRLibrary;

namespace NSAA_16Axis
{
    public partial class DialogTuneTable : Form
    {
        Mat LeftPattern;
        Mat RightPattern;

        PointF LeftPatternMoveStart;
        PointF LeftPatternMoveEnd;
        PointF RightPatternMoveStart;
        PointF RightPatternMoveEnd;

        double LeftStepsPerPixel;
        double RightStepsPerPixel;
        double StepsPerDegree;

        public DialogTuneTable()
        {
            InitializeComponent();
            gbXYYTable.Enabled = false;
            gbCamera.Enabled = false;
            if (GV.AppSettingParm.Language != "default")
            {
                this.Text = GV.Dlang.frmTuneTable;
                buttonSaveLEetPattern.Text = GV.Dlang.strSave;
                SaveRightPattern.Text = GV.Dlang.strSave;
                XYYTableSave.Text = GV.Dlang.strSave;
                buttonCameraSave.Text = GV.Dlang.strSave;
                radioButtonCamera.Text = GV.Dlang.frdCamera;
                radioButtonTable.Text = GV.Dlang.frdTable;
                groupBoxZoomLens.Text = GV.Dlang.fgbZoomLens;
                lbMovGoto.Text = GV.Dlang.flbMoveGoto;
                lbPettrnCenterDis.Text = GV.Dlang.flbPettrnCenterDis;
                lbRecord.Text = GV.Dlang.flbRecord;
                buttonCameraMove.Text = GV.Dlang.strGo;
                buttonXYYTableMove.Text = GV.Dlang.strGo;
                checkBoxTableManual.Text = GV.Dlang.strManual;
                checkBoxCameraManual.Text = GV.Dlang.strManual;
                gbCamera.Text = GV.Dlang.fgbCamera;
                gbXYYTable.Text = GV.Dlang.fgbXYYTable;
                radioButtonTableTheta.Text = GV.Dlang.frdTheta;
                labelLeftResult.Text = GV.Dlang.flabelLeftResult;
                labelRightResult.Text = GV.Dlang.flabelRightResult;
                labelLeftCameraResult.Text = GV.Dlang.flabelLeftResult;
                labelRightCameraResult.Text = GV.Dlang.flabelRightResult;
                radioButtonCameraLeftX.Text = GV.Dlang.flLeftX;
                radioButtonrCameraRghtX.Text = GV.Dlang.flRightX;
                radioButtonCameraLeftY.Text = GV.Dlang.flLeftY;
                radioButtonCameraRightY.Text = GV.Dlang.flRightY;
                lbCameraStep.Text = GV.Dlang.strStep;
                lbTableStep.Text = GV.Dlang.strStep;
            }
        }

        private void DialogTuneTable_Load(object sender, EventArgs e)
        {
            this.Visible = true;
            LeftCameraWindow.CanZoom = true;
            LeftCameraWindow.CanRectCommonRoi = true;
            RightCameraWindow.CanZoom = true;
            RightCameraWindow.CanRectCommonRoi = true;


            GV.LeftUpCam.SetWindow(LeftCameraWindow);
            GV.RightUpCam.SetWindow(RightCameraWindow);
            
            Update();
        }

        private async void comboBoxZoomLensGoto_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ControlBox = false;
            comboBoxZoomLensGoto.Enabled = false;
            gbXYYTable.Enabled = false;
            gbCamera.Enabled = false;

            bool isLeftZoomSuccess = false;
            bool isRightZoomSuccess = false;

            int leftMagnification = comboBoxZoomLensGoto.SelectedIndex;
            int rightMagnification = comboBoxZoomLensGoto.SelectedIndex;

            double rcdx = GV.ZoomLensInfo.LeftMotorStepsPerPixelX[comboBoxZoomLensGoto.SelectedIndex];
            double rcdy = GV.ZoomLensInfo.RightMotorStepsPerPixelX[comboBoxZoomLensGoto.SelectedIndex];

            lbRecord.Text = "Record : X : " + rcdx.ToString("F2") + " , Y : " + rcdy.ToString("F2");

            await Task.Run(() =>
            {
                Task leftLensZoom = Task.Factory.StartNew(() =>
                {
                    GV.Light.ChangeBrightness("left", GV.NowAlignCondition.LeftBrightness[leftMagnification]);
                    isLeftZoomSuccess = GV.LeftZoomLens.MoveGoto(GV.ZoomLensInfo.LeftMagnificationMotorSteps, leftMagnification);
                });

                Task rightLensZoom = Task.Factory.StartNew(() =>
                {
                    GV.Light.ChangeBrightness("right", GV.NowAlignCondition.RightBrightness[leftMagnification]);
                    isRightZoomSuccess = GV.RightZoomLens.MoveGoto(GV.ZoomLensInfo.RightMagnificationMotorSteps, leftMagnification);

                });
                leftLensZoom.Wait(10000);
                rightLensZoom.Wait(10000);
            });
            
            if (isLeftZoomSuccess && isRightZoomSuccess)
            {
                gbXYYTable.Enabled = true;
                gbCamera.Enabled = true;
            }
                
            comboBoxZoomLensGoto.Enabled = true;
            this.ControlBox = true;
        }

        private void buttonXYYTableMove_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkBoxTableManual.Checked)
                {
                    if (radioButtonTableX.Checked)
                        GV.Plc.CorrectionXyyTableMove((int)numericUpDownTableSteps.Value, 0, 0);
                    if (radioButtonTableY.Checked)
                        GV.Plc.CorrectionXyyTableMove(0, (int)numericUpDownTableSteps.Value, 0);
                    if (radioButtonTableTheta.Checked)
                        GV.Plc.CorrectionXyyTableMove(0, 0, (int)(numericUpDownTableSteps.Value));
                    return;
                }

                if (LeftPattern == null || RightPattern == null)
                {
                    MessageBox.Show("Learn pattern first.", "XYYTableMove", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                OpenCV3MatchUMat matcher = new OpenCV3MatchUMat("TableMove");
                if (matcher.Match(GV.LeftUpCam.Grab(), LeftPattern).Score < 0.8F || matcher.Match(GV.RightUpCam.Grab(), RightPattern).Score < 0.8F)
                {
                    MessageBox.Show("Can't find pattern.", "XYYTableMove", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MatchPosition mp = new MatchPosition();
                mp = matcher.Match(GV.LeftUpCam.Grab(), LeftPattern);
                LeftPatternMoveStart.X = (float)mp.X;
                LeftPatternMoveStart.Y = (float)mp.Y;
                mp = matcher.Match(GV.RightUpCam.Grab(), RightPattern);
                RightPatternMoveStart.X = (float)mp.X;
                RightPatternMoveStart.Y = (float)mp.Y;

                if (radioButtonTableX.Checked)
                {
                    GV.Plc.CorrectionXyyTableMove((int)numericUpDownTableSteps.Value, 0, 0);
                }
                if (radioButtonTableY.Checked)
                {
                    GV.Plc.CorrectionXyyTableMove(0, (int)numericUpDownTableSteps.Value, 0);
                }
                if (radioButtonTableTheta.Checked)
                {
                    GV.Plc.CorrectionXyyTableMove(0, 0, (int)(numericUpDownTableSteps.Value));
                }


                mp = matcher.Match(GV.LeftUpCam.Grab(), LeftPattern);
                LeftPatternMoveEnd.X = (float)mp.X;
                LeftPatternMoveEnd.Y = (float)mp.Y;
                mp = matcher.Match(GV.RightUpCam.Grab(), RightPattern);
                RightPatternMoveEnd.X = (float)mp.X;
                RightPatternMoveEnd.Y = (float)mp.Y;

                LeftStepsPerPixel = Math.Abs((double)numericUpDownTableSteps.Value / Math.Sqrt(Math.Pow(LeftPatternMoveEnd.X - LeftPatternMoveStart.X, 2) + Math.Pow(LeftPatternMoveEnd.Y - LeftPatternMoveStart.Y, 2)));
                RightStepsPerPixel = Math.Abs((double)numericUpDownTableSteps.Value / Math.Sqrt(Math.Pow(RightPatternMoveEnd.X - RightPatternMoveStart.X, 2) + Math.Pow(RightPatternMoveEnd.Y - RightPatternMoveStart.Y, 2)));

                if (radioButtonTableTheta.Checked)
                {
                    if (GV.ZoomLensInfo.LeftUmPerPixelX[comboBoxZoomLensGoto.SelectedIndex] <= 0 || GV.ZoomLensInfo.RightUmPerPixelX[comboBoxZoomLensGoto.SelectedIndex] <= 0)
                        throw new MRException("Please tune the lens first");
                    if ((int)numericUpDownPatternCenterDistance.Value <= 0)
                        throw new MRException("Please type the pattern center distance value");

                    //double tanA = Math.Sqrt(Math.Pow(LeftPatternMoveEnd.X - LeftPatternMoveStart.X, 2) + Math.Pow(LeftPatternMoveEnd.Y - LeftPatternMoveStart.Y, 2)) * GV.ZoomLensInfo.LeftUmPerPixel[comboBoxZoomLensGoto.SelectedIndex] + 
                    //                Math.Sqrt(Math.Pow(RightPatternMoveEnd.X - RightPatternMoveStart.X, 2) + Math.Pow(RightPatternMoveEnd.Y - RightPatternMoveStart.Y, 2)) * GV.ZoomLensInfo.RightUmPerPixel[comboBoxZoomLensGoto.SelectedIndex];
                    //double tanB = (double)numericUpDownPatternCenterDistance.Value * 1000;
                    //double rotateDegree = Math.Atan(tanA / tanB) * 180 / Math.PI;
                    //double rotateDegree = tanA / (2 * Math.PI * tanB) * 360;

                    //double r = (double)numericUpDownPatternCenterDistance.Value * 1000 / 2;
                    //double c = Math.Sqrt(Math.Pow(LeftPatternMoveEnd.X - LeftPatternMoveStart.X, 2) + Math.Pow(LeftPatternMoveEnd.Y - LeftPatternMoveStart.Y, 2)) * GV.ZoomLensInfo.LeftUmPerPixel[comboBoxZoomLensGoto.SelectedIndex];
                    //double cosC = (r * r + r * r - c * c) / (2 * r * r);

                    //double rad = Math.Acos(cosC);
                    //double rotateDegree = rad * 180 / Math.PI;

                    double heronA, heronB, heronC;  //Heron's Foemula
                    heronA = heronB = (double)numericUpDownPatternCenterDistance.Value * 1000 / 2;
                    heronC = Math.Sqrt(Math.Pow(LeftPatternMoveEnd.X - LeftPatternMoveStart.X, 2) + Math.Pow(LeftPatternMoveEnd.Y - LeftPatternMoveStart.Y, 2)) * GV.ZoomLensInfo.LeftUmPerPixelX[comboBoxZoomLensGoto.SelectedIndex];
                    double cosC = (heronA * heronA + heronB * heronB - heronC * heronC) / (2 * heronA * heronB);
                    double rotateDegree = Math.Acos(cosC) * 180 / Math.PI;

                    StepsPerDegree = Math.Abs((double)numericUpDownTableSteps.Value) / rotateDegree;
                    labelLeftResult.Text = string.Empty;
                    labelRightResult.Text = string.Empty;
                    labelLeftResult.Text = "Steps per degree : " + Math.Round(StepsPerDegree);

                    return;
                }

                labelLeftResult.Text = "Left steps per pixel : " + LeftStepsPerPixel.ToString("F2");
                labelRightResult.Text = "Right steps per pixel : " + RightStepsPerPixel.ToString("F2");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DialogTuneTable_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                GV.Plc.Send("WR MR8614 1");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //LeftVideo.Exit();
            //RightVideo.Exit();

            Thread.Sleep(200);
        }

        private void buttonSaveLEetPattern_Click(object sender, EventArgs e)
        {
            LeftPattern = new Mat(GV.LeftUpCam.Grab(), LeftCameraWindow.GetCommonRectangle());
            pictureBoxLeftPattern.Image = LeftPattern.ToBitmap();
        }

        private void SaveRightPattern_Click(object sender, EventArgs e)
        {
            RightPattern = new Mat(GV.RightUpCam.Grab(), RightCameraWindow.GetCommonRectangle());
            pictureBoxRightPattern.Image = RightPattern.ToBitmap();
        }


        private async void ChangeMotorSteps(object sender, MouseEventArgs e)
        {
            Stopwatch sw = Stopwatch.StartNew();
            await Task.Run(() => {
                while (true)
                {
                    if (sw.ElapsedMilliseconds > 1000) break;
                    if (IsMouseUp) break;
                }
            });

            if (IsMouseUp)
            {
                IsMouseUp = false;
                return;
            }

            try
            {
                NumericUpDown nud = (NumericUpDown)sender;
                DialogCalculator cal = new DialogCalculator();
                cal.StartPosition = FormStartPosition.CenterScreen;
                cal.ShowDialog();
                if (cal.IsDouble) nud.Value = (decimal)cal.NumberDouble;
                if (cal.IsInt) nud.Value = (decimal)cal.NumberInt;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        bool IsMouseUp = false;
        private void numericUpDownSteps_MouseUp(object sender, MouseEventArgs e)
        {
            IsMouseUp = true;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            string MagnificationStr = (comboBoxZoomLensGoto.SelectedIndex + 1).ToString();
            if (MessageBox.Show("Sure to save X" + MagnificationStr + " Magnification", "XyyTableMotorStepsSave", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            if (radioButtonTableX.Checked)
            {
                if (MessageBox.Show("Sure to save xyy table X axis parameters ?", "XyyTableMotorStepsSave", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
                GV.ZoomLensInfo.LeftMotorStepsPerPixelX[comboBoxZoomLensGoto.SelectedIndex] = LeftStepsPerPixel;
                GV.ZoomLensInfo.RightMotorStepsPerPixelX[comboBoxZoomLensGoto.SelectedIndex] = RightStepsPerPixel;
            }

            if (radioButtonTableY.Checked)
            {
                if (MessageBox.Show("Sure to save xyy table Y axis parameters ?", "XyyTableMotorStepsSave", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
                GV.ZoomLensInfo.LeftMotorStepsPerPixelY[comboBoxZoomLensGoto.SelectedIndex] = LeftStepsPerPixel;
                GV.ZoomLensInfo.RightMotorStepsPerPixelY[comboBoxZoomLensGoto.SelectedIndex] = RightStepsPerPixel;
            }

            if (radioButtonTableTheta.Checked)
            {
                if (MessageBox.Show("Sure to save xyy table Theta axis parameters ?", "XyyTableMotorStepsSave", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
                GV.ZoomLensInfo.MotorStepsPerPixelDegree = StepsPerDegree;
            }
            GM.WriteZoomLensInfoToXml("ZoomLensInfo.xml", GV.ZoomLensInfo);
        }


        private void RadioButtonCamera_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButtonCamera.Checked)
            {
                gbCamera.Visible = true;
                gbXYYTable.Visible = false;
            }
        }

        private void RadioButtonTable_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonTable.Checked)
            {
                gbCamera.Visible = false;
                gbXYYTable.Visible = true;
            }
        }

        private void ButtonCameraMove_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkBoxCameraManual.Checked)
                {
                    if (radioButtonCameraLeftX.Checked)
                        GV.Plc.CorrectionCameraMove((int)numericUpDownCameraSteps.Value, 0, 0, 0);
                    if (radioButtonrCameraRghtX.Checked)
                        GV.Plc.CorrectionCameraMove(0, (int)numericUpDownCameraSteps.Value, 0, 0);
                    if (radioButtonCameraLeftY.Checked)
                        GV.Plc.CorrectionCameraMove(0, 0, (int)(numericUpDownCameraSteps.Value), 0);
                    if (radioButtonCameraRightY.Checked)
                        GV.Plc.CorrectionCameraMove(0, 0, 0, (int)(numericUpDownCameraSteps.Value));
                    return;
                } 


                if (LeftPattern == null || RightPattern == null)
                {
                    MessageBox.Show("Learn pattern first.", "XYYTableMove", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                OpenCV3MatchUMat matcher = new OpenCV3MatchUMat("CameraMove");
                if (matcher.Match(GV.LeftUpCam.Grab(), LeftPattern).Score < 0.8F || matcher.Match(GV.RightUpCam.Grab(), RightPattern).Score < 0.8F)
                {
                    MessageBox.Show("Can't find pattern.", "XYYTableMove", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MatchPosition mp = new MatchPosition();
                mp = matcher.Match(GV.LeftUpCam.Grab(), LeftPattern);
                LeftPatternMoveStart.X = (float)mp.X;
                LeftPatternMoveStart.Y = (float)mp.Y;
                mp = matcher.Match(GV.RightUpCam.Grab(), RightPattern);
                RightPatternMoveStart.X = (float)mp.X;
                RightPatternMoveStart.Y = (float)mp.Y;

                if (radioButtonCameraLeftX.Checked)
                    GV.Plc.CorrectionCameraMove((int)numericUpDownCameraSteps.Value, 0, 0, 0);
                if (radioButtonrCameraRghtX.Checked)
                    GV.Plc.CorrectionCameraMove(0, (int)numericUpDownCameraSteps.Value, 0, 0);
                if (radioButtonCameraLeftY.Checked)
                    GV.Plc.CorrectionCameraMove(0, 0, (int)(numericUpDownCameraSteps.Value), 0);
                if (radioButtonCameraRightY.Checked)
                    GV.Plc.CorrectionCameraMove(0, 0, 0, (int)(numericUpDownCameraSteps.Value));

                mp = matcher.Match(GV.LeftUpCam.Grab(), LeftPattern);
                LeftPatternMoveEnd.X = (float)mp.X;
                LeftPatternMoveEnd.Y = (float)mp.Y;
                mp = matcher.Match(GV.RightUpCam.Grab(), RightPattern);
                RightPatternMoveEnd.X = (float)mp.X;
                RightPatternMoveEnd.Y = (float)mp.Y;

                LeftStepsPerPixel = Math.Abs((double)numericUpDownCameraSteps.Value / Math.Sqrt(Math.Pow(LeftPatternMoveEnd.X - LeftPatternMoveStart.X, 2) + Math.Pow(LeftPatternMoveEnd.Y - LeftPatternMoveStart.Y, 2)));
                RightStepsPerPixel = Math.Abs((double)numericUpDownCameraSteps.Value / Math.Sqrt(Math.Pow(RightPatternMoveEnd.X - RightPatternMoveStart.X, 2) + Math.Pow(RightPatternMoveEnd.Y - RightPatternMoveStart.Y, 2)));

                labelLeftCameraResult.Text = "Left steps per pixel : ";
                labelRightCameraResult.Text = "Right steps per pixel : ";

                if (radioButtonCameraLeftX.Checked || radioButtonCameraLeftY.Checked)
                    labelLeftCameraResult.Text = "Left steps per pixel : " + LeftStepsPerPixel.ToString("F2");
                if (radioButtonrCameraRghtX.Checked || radioButtonCameraRightY.Checked)
                    labelRightCameraResult.Text = "Right steps per pixel : " + RightStepsPerPixel.ToString("F2");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButtonCameraSave_Click(object sender, EventArgs e)
        {
            string magnificationStr = (comboBoxZoomLensGoto.SelectedIndex + 1).ToString();
            if (MessageBox.Show("Sure to save X" + magnificationStr + " magnification", "CameraMotorStepsSave", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;


            if (radioButtonCameraLeftX.Checked)
            {
                if (MessageBox.Show("Sure to save left camera motor X axis parameters ?", "CameraMotorStepsSave", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
                GV.ZoomLensInfo.LeftCameraMotorStepsPerPixelX[comboBoxZoomLensGoto.SelectedIndex] = LeftStepsPerPixel; 
            }
            if (radioButtonrCameraRghtX.Checked)
            {
                if (MessageBox.Show("Sure to save right camera motor X axis parameters ?", "CameraMotorStepsSave", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
                GV.ZoomLensInfo.RightCameraMotorStepsPerPixelX[comboBoxZoomLensGoto.SelectedIndex] = RightStepsPerPixel;
            }
            if (radioButtonCameraLeftY.Checked)
            {
                if (MessageBox.Show("Sure to save left camera motor Y axis parameters ?", "CameraMotorStepsSave", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
                GV.ZoomLensInfo.LeftCameraMotorStepsPerPixelY[comboBoxZoomLensGoto.SelectedIndex] = LeftStepsPerPixel;
            }
            if (radioButtonCameraRightY.Checked)
            {
                if (MessageBox.Show("Sure to save right camera motor Y axis parameters ?", "CameraMotorStepsSave", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
                GV.ZoomLensInfo.RightCameraMotorStepsPerPixelY[comboBoxZoomLensGoto.SelectedIndex] = RightStepsPerPixel;
            }
            GM.WriteZoomLensInfoToXml("ZoomLensInfo.xml", GV.ZoomLensInfo);
        }
    }
}
