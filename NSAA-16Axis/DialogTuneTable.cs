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
        private Recipe currentRecipe;
        private bool isUpperCamera = true;
        private System.Windows.Forms.Timer tableLocationUpdateTimer;
        private bool isTableMode = false;
        private System.Windows.Forms.Timer cameraLocationUpdateTimer;
        private bool isCameraMode = false;
        public DialogTuneTable()
        {

            InitializeComponent();
            gbXYYTable.Enabled = false;
            gbCamera.Enabled = false;
            this.Shown += DialogTuneTable_Shown;
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
            var pnlTTHost = new System.Windows.Forms.Panel
            {
                Name = "pnlTTHost",
                Location = new System.Drawing.Point(990, 187), // 置放 panel 的表單座標（可調）
                Size = new System.Drawing.Size(291, 321),      // 與 Designer 上 groupBox 大小對齊
                BackColor = System.Drawing.Color.Transparent,
                Visible = false
            };
            this.Controls.Add(pnlTTHost);

            // 重新 parent 並將位置改為 panel 的 client 座標 (0,0)
            groupBoxTT1.Parent = pnlTTHost;
            groupBoxTT1.Location = new System.Drawing.Point(0, 0);
            groupBoxTT2.Parent = pnlTTHost;
            groupBoxTT2.Location = new System.Drawing.Point(0, 0);
            groupBoxTT4.Parent = pnlTTHost;
            groupBoxTT4.Location = new System.Drawing.Point(0, 0);

            // 預設都隱藏，由程式控制顯示
            groupBoxTT1.Visible = false;
            groupBoxTT2.Visible = false;
            groupBoxTT4.Visible = false;
            InitializeRecipeDisplay();       // 改用新方法：只顯示當前 Recipe
            InitializeCameraSelection();
            //LoadCurrentRecipe();
            tableLocationUpdateTimer = new System.Windows.Forms.Timer();
            tableLocationUpdateTimer.Interval = 500;
            tableLocationUpdateTimer.Tick += TableLocationUpdateTimer_Tick;
            cameraLocationUpdateTimer = new System.Windows.Forms.Timer();
            cameraLocationUpdateTimer.Interval = 500;
            cameraLocationUpdateTimer.Tick += CameraLocationUpdateTimer_Tick;
        }

        private void CameraLocationUpdateTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (GV.AppSettingParm.Emulation) return;
                if (!isCameraMode) return;

                GV.Plc.GetLocation();

                if (InvokeRequired)
                {
                    Invoke((MethodInvoker)delegate { UpdateCameraCurrentLocationUI(); });
                }
                else
                {
                    UpdateCameraCurrentLocationUI();
                }
            }
            catch
            {

            }
        }

        private void UpdateCameraCurrentLocationUI()
        {
            if (GV.UpBackAlign == 1) // Top CCD - groupBoxTT1
            {
                var nudTReadUpBigY = FindControlRecursive("nudTReadUpBigY") as NumericUpDown;
                var nudTReadUpLeftX = FindControlRecursive("nudTReadUpLeftX") as NumericUpDown;
                var nudTReadUpLeftY = FindControlRecursive("nudTReadUpLeftY") as NumericUpDown;
                var nudTReadUpLeftZ = FindControlRecursive("nudTReadUpLeftZ") as NumericUpDown;
                var nudTReadUpRightX = FindControlRecursive("nudTReadUpRightX") as NumericUpDown;
                var nudTReadUpRightY = FindControlRecursive("nudTReadUpRightY") as NumericUpDown;
                var nudTReadUpRightZ = FindControlRecursive("nudTReadUpRightZ") as NumericUpDown;

                // ✅ 將 PLC 數值除以 10 後顯示
                if (nudTReadUpBigY != null)
                    nudTReadUpBigY.Value = (decimal)(GV.NowLocation[GV.Plc.iiDUpBigY] / 10.0);
                if (nudTReadUpLeftX != null)
                    nudTReadUpLeftX.Value = (decimal)(GV.NowLocation[GV.Plc.iiDUpLeftX] / 10.0);
                if (nudTReadUpLeftY != null)
                    nudTReadUpLeftY.Value = (decimal)(GV.NowLocation[GV.Plc.iiDUpLeftY] / 10.0);
                if (nudTReadUpLeftZ != null)
                    nudTReadUpLeftZ.Value = (decimal)(GV.NowLocation[GV.Plc.iiDSUpLeftZ] / 10.0);
                if (nudTReadUpRightX != null)
                    nudTReadUpRightX.Value = (decimal)(GV.NowLocation[GV.Plc.iiDUpRightX] / 10.0);
                if (nudTReadUpRightY != null)
                    nudTReadUpRightY.Value = (decimal)(GV.NowLocation[GV.Plc.iiDUpRightY] / 10.0);
                if (nudTReadUpRightZ != null)
                    nudTReadUpRightZ.Value = (decimal)(GV.NowLocation[GV.Plc.iiDSUpRightZ] / 10.0);
            }
            else if (GV.UpBackAlign == 2) // Bottom CCD - groupBoxTT2
            {
                var nudTReadBackLeftX = FindControlRecursive("nudTReadBackLeftX") as NumericUpDown;
                var nudTReadBackLeftY = FindControlRecursive("nudTReadBackLeftY") as NumericUpDown;
                var nudTReadBackLeftZ = FindControlRecursive("nudTReadBackLeftZ") as NumericUpDown;
                var nudTReadBackRightX = FindControlRecursive("nudTReadBackRightX") as NumericUpDown;
                var nudTReadBackRightY = FindControlRecursive("nudTReadBackRightY") as NumericUpDown;
                var nudTReadBackRightZ = FindControlRecursive("nudTReadBackRightZ") as NumericUpDown;

                // ✅ 將 PLC 數值除以 10 後顯示
                if (nudTReadBackLeftX != null)
                    nudTReadBackLeftX.Value = (decimal)(GV.NowLocation[GV.Plc.iiDDownLeftX] / 10.0);
                if (nudTReadBackLeftY != null)
                    nudTReadBackLeftY.Value = (decimal)(GV.NowLocation[GV.Plc.iiDDownLeftY] / 10.0);
                if (nudTReadBackLeftZ != null)
                    nudTReadBackLeftZ.Value = (decimal)(GV.NowLocation[GV.Plc.iiDDownLeftZ] / 10.0);
                if (nudTReadBackRightX != null)
                    nudTReadBackRightX.Value = (decimal)(GV.NowLocation[GV.Plc.iiDDownRightX] / 10.0);
                if (nudTReadBackRightY != null)
                    nudTReadBackRightY.Value = (decimal)(GV.NowLocation[GV.Plc.iiDDownRightY] / 10.0);
                if (nudTReadBackRightZ != null)
                    nudTReadBackRightZ.Value = (decimal)(GV.NowLocation[GV.Plc.iiDDownRightZ] / 10.0);
            }
        }
        private void UpdateCameraTargetPositionDisplay()
        {
            if (GV.UpBackAlign == 1) // Top CCD - groupBoxTT1
            {
                var nudTWriteUpBigY = FindControlRecursive("nudTWriteUpBigY") as NumericUpDown;
                var nudTWriteUpLeftX = FindControlRecursive("nudTWriteUpLeftX") as NumericUpDown;
                var nudTWriteUpLeftY = FindControlRecursive("nudTWriteUpLeftY") as NumericUpDown;
                var nudTWriteUpLeftZ = FindControlRecursive("nudTWriteUpLeftZ") as NumericUpDown;
                var nudTWriteUpRightX = FindControlRecursive("nudTWriteUpRightX") as NumericUpDown;
                var nudTWriteUpRightY = FindControlRecursive("nudTWriteUpRightY") as NumericUpDown;
                var nudTWriteUpRightZ = FindControlRecursive("nudTWriteUpRightZ") as NumericUpDown;

                // ✅ 將 RecipeData 數值除以 10 後顯示
                if (nudTWriteUpBigY != null)
                    nudTWriteUpBigY.Value = (decimal)(GV.Plc.RecipeData[29] / 10.0);
                if (nudTWriteUpLeftX != null)
                    nudTWriteUpLeftX.Value = (decimal)(GV.Plc.RecipeData[30] / 10.0);
                if (nudTWriteUpLeftY != null)
                    nudTWriteUpLeftY.Value = (decimal)(GV.Plc.RecipeData[32] / 10.0);
                if (nudTWriteUpLeftZ != null)
                    nudTWriteUpLeftZ.Value = (decimal)(GV.Plc.RecipeData[34] / 10.0);
                if (nudTWriteUpRightX != null)
                    nudTWriteUpRightX.Value = (decimal)(GV.Plc.RecipeData[31] / 10.0);
                if (nudTWriteUpRightY != null)
                    nudTWriteUpRightY.Value = (decimal)(GV.Plc.RecipeData[33] / 10.0);
                if (nudTWriteUpRightZ != null)
                    nudTWriteUpRightZ.Value = (decimal)(GV.Plc.RecipeData[35] / 10.0);
            }
            else if (GV.UpBackAlign == 2) // Bottom CCD - groupBoxTT2
            {
                var nudTWriteBackLeftX = FindControlRecursive("nudTWriteBackLeftX") as NumericUpDown;
                var nudTWriteBackLeftY = FindControlRecursive("nudTWriteBackLeftY") as NumericUpDown;
                var nudTWriteBackLeftZ = FindControlRecursive("nudTWriteBackLeftZ") as NumericUpDown;
                var nudTWriteBackRightX = FindControlRecursive("nudTWriteBackRightX") as NumericUpDown;
                var nudTWriteBackRightY = FindControlRecursive("nudTWriteBackRightY") as NumericUpDown;
                var nudTWriteBackRightZ = FindControlRecursive("nudTWriteBackRightZ") as NumericUpDown;

                // ✅ 將 RecipeData 數值除以 10 後顯示
                if (nudTWriteBackLeftX != null)
                    nudTWriteBackLeftX.Value = (decimal)(GV.Plc.RecipeData[39] / 10.0);
                if (nudTWriteBackLeftY != null)
                    nudTWriteBackLeftY.Value = (decimal)(GV.Plc.RecipeData[41] / 10.0);
                if (nudTWriteBackLeftZ != null)
                    nudTWriteBackLeftZ.Value = (decimal)(GV.Plc.RecipeData[43] / 10.0);
                if (nudTWriteBackRightX != null)
                    nudTWriteBackRightX.Value = (decimal)(GV.Plc.RecipeData[40] / 10.0);
                if (nudTWriteBackRightY != null)
                    nudTWriteBackRightY.Value = (decimal)(GV.Plc.RecipeData[42] / 10.0);
                if (nudTWriteBackRightZ != null)
                    nudTWriteBackRightZ.Value = (decimal)(GV.Plc.RecipeData[44] / 10.0);
            }
        }

        private void TableLocationUpdateTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (GV.AppSettingParm.Emulation) return;
                if (!isTableMode) return;
                GV.Plc.GetLocation();
                if (InvokeRequired)
                {
                    Invoke((MethodInvoker)delegate { UpdateTableCurrentLocationUI(); });
                }
                else
                {
                    UpdateTableCurrentLocationUI();
                }
            }
            catch
            {

            }
        }

        private void UpdateTableCurrentLocationUI()
        {
            var nudTReadChuckX = FindControlRecursive("nudTReadChuckX") as NumericUpDown;
            var nudTReadChuckY1 = FindControlRecursive("nudTReadChuckY1") as NumericUpDown;
            var nudTReadChuckY2 = FindControlRecursive("nudTReadChuckY2") as NumericUpDown;

            // ✅ 將 PLC 數值除以 10 後顯示
            if (nudTReadChuckX != null)
                nudTReadChuckX.Value = (decimal)(GV.NowLocation[GV.Plc.iiDChuckX] / 10.0);
            if (nudTReadChuckY1 != null)
                nudTReadChuckY1.Value = (decimal)(GV.NowLocation[GV.Plc.iiDChuckY1] / 10.0);
            if (nudTReadChuckY2 != null)
                nudTReadChuckY2.Value = (decimal)(GV.NowLocation[GV.Plc.iiDChuckY2] / 10.0);
        }
        private void UpdateTableTargetPositionDisplay()
        {
            var nudTWriteChuckX = FindControlRecursive("nudTWriteChuckX") as NumericUpDown;
            var nudTWriteChuckY1 = FindControlRecursive("nudTWriteChuckY1") as NumericUpDown;
            var nudTWriteChuckY2 = FindControlRecursive("nudTWriteChuckY2") as NumericUpDown;

            // ✅ 將 RecipeData 數值除以 10 後顯示
            if (nudTWriteChuckX != null)
                nudTWriteChuckX.Value = (decimal)(GV.Plc.RecipeData[36] / 10.0);
            if (nudTWriteChuckY1 != null)
                nudTWriteChuckY1.Value = (decimal)(GV.Plc.RecipeData[37] / 10.0);
            if (nudTWriteChuckY2 != null)
                nudTWriteChuckY2.Value = (decimal)(GV.Plc.RecipeData[38] / 10.0);
        }

        private void DialogTuneTable_Load(object sender, EventArgs e)
        {
            if (!this.IsHandleCreated)
            {
                this.CreateHandle();
            }

            // 確保控制項 Handle 已建立
            if (!LeftCameraWindow.IsHandleCreated)
            {
                LeftCameraWindow.CreateControl();
            }
            if (!RightCameraWindow.IsHandleCreated)
            {
                RightCameraWindow.CreateControl();
            }
            this.Visible = true;
            LeftCameraWindow.CanZoom = true;
            LeftCameraWindow.CanRectCommonRoi = true;
            RightCameraWindow.CanZoom = true;
            RightCameraWindow.CanRectCommonRoi = true;
            FreezeAllCameras();
            Thread.Sleep(200);
        }
        private void DialogTuneTable_Shown(object sender, EventArgs e)
        {
            // Form 完全顯示後才設定相機
            if (isUpperCamera)
            {
                SwitchToUpperCamera();
            }
            else
            {
                SwitchToLowerCamera();
            }
        }
        private void FreezeAllCameras()
        {
            GV.LeftUpCam?.Freeze();
            GV.RightUpCam?.Freeze();
            GV.LeftBackCam?.Freeze();
            GV.RightBackCam?.Freeze();
        }
        private void InitializeRecipeDisplay()
        {
            comboBoxRecipe.Items.Clear();
            int iNowRecipeNumber = GV.Plc.ReadData16(GV.Plc.iNowRecipeNumber);
            comboBoxRecipe.Items.Add($"Recipe {iNowRecipeNumber}");
            comboBoxRecipe.SelectedIndex = 0;
            comboBoxRecipe.Enabled = false;  // 禁用選擇，只顯示當前 Recipe
        }
        private void InitializeCameraSelection()
        {
            // 根據 GV.UpBackAlign 自動選擇相機
            if (GV.UpBackAlign == 2)
            {
                // Bottom CCD 模式
                rbLowerCamera.Checked = true;
                rbUpperCamera.Enabled = false;
                isUpperCamera = false;
            }
            else
            {
                // Top CCD 模式
                rbUpperCamera.Checked = true;
                rbLowerCamera.Enabled = false;
                isUpperCamera = true;
            }
            //ShowGroupBoxesForCameraMode();
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DialogTuneTable_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (tableLocationUpdateTimer != null)
                {
                    tableLocationUpdateTimer.Stop();
                    tableLocationUpdateTimer.Dispose();
                }
                if (cameraLocationUpdateTimer != null)
                {
                    cameraLocationUpdateTimer.Stop();
                    cameraLocationUpdateTimer.Dispose();
                }
                GV.LeftUpCam?.Freeze();
                GV.RightUpCam?.Freeze();
                GV.LeftBackCam?.Freeze();
                GV.RightBackCam?.Freeze();

                // 短暫等待確保相機完全停止
                System.Threading.Thread.Sleep(100);
                GV.Plc.Send("WR MR8614 1");
            }
            catch (Exception ex)
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
            await Task.Run(() =>
            {
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
            catch (Exception ex)
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
            if (radioButtonCamera.Checked)
            {
                isTableMode = false;
                if (tableLocationUpdateTimer != null && tableLocationUpdateTimer.Enabled)
                {
                    tableLocationUpdateTimer.Stop();
                }

                isCameraMode = true;
                gbCamera.Visible = true;
                gbXYYTable.Visible = false;
                var host = this.Controls.Find("pnlTTHost", true).FirstOrDefault() as Panel;
                if (host != null)
                {
                    host.Visible = true;
                    host.BringToFront();

                    // 根據 UpBackAlign 決定顯示哪個 groupBox
                    if (GV.UpBackAlign == 1)
                    {
                        groupBoxTT1.Visible = true;
                        groupBoxTT1.BringToFront();
                        groupBoxTT2.Visible = false;
                        groupBoxTT4.Visible = false;
                    }
                    else if (GV.UpBackAlign == 2)
                    {
                        groupBoxTT2.Visible = true;
                        groupBoxTT2.BringToFront();
                        groupBoxTT1.Visible = false;
                        groupBoxTT4.Visible = false;
                    }
                    else
                    {
                        // 其他情況隱藏全部
                        groupBoxTT1.Visible = false;
                        groupBoxTT2.Visible = false;
                        groupBoxTT4.Visible = false;
                    }
                }
                UpdateCameraTargetPositionDisplay();
                if (!cameraLocationUpdateTimer.Enabled)
                {
                    cameraLocationUpdateTimer.Start();
                }
                GV.Plc.GetLocation();
                UpdateCameraCurrentLocationUI();
            }
            else
            {
                isCameraMode = false;
                if (cameraLocationUpdateTimer != null && cameraLocationUpdateTimer.Enabled)
                {
                    cameraLocationUpdateTimer.Stop();
                }
            }
        }

        private void RadioButtonTable_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonTable.Checked)
            {
                isCameraMode = false;
                if (cameraLocationUpdateTimer != null && cameraLocationUpdateTimer.Enabled)
                {
                    cameraLocationUpdateTimer.Stop();
                }
                isTableMode = true;
                gbCamera.Visible = false;
                gbXYYTable.Visible = true;

                var host = this.Controls.Find("pnlTTHost", true).FirstOrDefault() as Panel;
                if (host != null)
                {
                    host.Visible = true;
                    host.BringToFront();

                    // 隱藏不需要的，顯示需要的，並將其移到最上層
                    groupBoxTT1.Visible = false;
                    groupBoxTT2.Visible = false;
                    groupBoxTT4.Visible = true;
                    groupBoxTT4.BringToFront();
                    groupBoxTT4.Refresh();
                }
                UpdateTableTargetPositionDisplay();
                if (!tableLocationUpdateTimer.Enabled)
                {
                    tableLocationUpdateTimer.Start();
                }
                GV.Plc.GetLocation();
                UpdateTableCurrentLocationUI();
            }
            else
            {
                // 停止 Timer
                isTableMode = false;
                if (tableLocationUpdateTimer != null && tableLocationUpdateTimer.Enabled)
                {
                    tableLocationUpdateTimer.Stop();
                }
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
        private void InitializeRecipeComboBox()
        {
            comboBoxRecipe.Items.Clear();
            // 假設最多有 100 個配方 (根據 Global.cs 中 acar[101] 的設計)
            for (int i = 1; i <= 100; i++)
            {
                comboBoxRecipe.Items.Add($"Recipe {i}");
            }
            if (comboBoxRecipe.Items.Count > 0)
            {
                comboBoxRecipe.SelectedIndex = GV.NowRecipeNumber - 1;
            }
        }

        // ✅ 新增方法：載入當前 Recipe
        private void LoadCurrentRecipe()
        {
            currentRecipe = GM.ReadRecipeXml(GV.NowRecipeNumber);
        }

        // ✅ 新增事件：Recipe 選擇變更
        private void comboBoxRecipe_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCurrentRecipe();
        }

        // ✅ 新增事件：上層相機選擇
        private void rbUpperCamera_CheckedChanged(object sender, EventArgs e)
        {
            if (rbUpperCamera.Checked)
            {
                isUpperCamera = true;
                SwitchToUpperCamera();
            }
        }

        // ✅ 新增事件：下層相機選擇
        private void rbLowerCamera_CheckedChanged(object sender, EventArgs e)
        {
            if (rbLowerCamera.Checked)
            {
                isUpperCamera = false;
                SwitchToLowerCamera();
            }
        }

        // ✅ 新增方法：切換到上層相機
        private void SwitchToUpperCamera()
        {
            // 確保在 UI 執行緒上執行
            if (InvokeRequired)
            {
                Invoke(new Action(() => SwitchToUpperCamera()));
                return;
            }

            // 確保控制項 Handle 已建立
            if (!LeftCameraWindow.IsHandleCreated)
            {
                LeftCameraWindow.CreateControl();
            }
            if (!RightCameraWindow.IsHandleCreated)
            {
                RightCameraWindow.CreateControl();
            }

            try
            {
                FreezeAllCameras();
                System.Threading.Thread.Sleep(100);

                GV.LeftUpCam.SetWindow(LeftCameraWindow);
                GV.RightUpCam.SetWindow(RightCameraWindow);
                GV.LeftUpCam.Live();
                GV.RightUpCam.Live();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Switch to upper camera failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ✅ 新增方法：切換到下層相機
        private void SwitchToLowerCamera()
        {
            // 確保在 UI 執行緒上執行
            if (InvokeRequired)
            {
                Invoke(new Action(() => SwitchToLowerCamera()));
                return;
            }

            // 確保控制項 Handle 已建立
            if (!LeftCameraWindow.IsHandleCreated)
            {
                LeftCameraWindow.CreateControl();
            }
            if (!RightCameraWindow.IsHandleCreated)
            {
                RightCameraWindow.CreateControl();
            }

            try
            {
                FreezeAllCameras();
                System.Threading.Thread.Sleep(100);

                if (GV.LeftBackCam != null && GV.RightBackCam != null)
                {
                    GV.LeftBackCam.SetWindow(LeftCameraWindow);
                    GV.RightBackCam.SetWindow(RightCameraWindow);
                    GV.LeftBackCam.Live();
                    GV.RightBackCam.Live();
                }
                else
                {
                    MessageBox.Show("Lower camera not available.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    rbUpperCamera.Checked = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Switch to lower camera failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                rbUpperCamera.Checked = true;
            }
        }

        // ✅ 新增事件：移動到 Recipe 位置
        private async void btnMoveToRecipePos_Click(object sender, EventArgs e)
        {
            if (currentRecipe == null)
            {
                MessageBox.Show("Please select a recipe first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnMoveToRecipePos.Enabled = false;
            btnMoveToRecipePos.Text = "Moving...";

            try
            {
                await Task.Run(() =>
                {
                    if (isUpperCamera)
                    {
                        // 移動上層相機到 Recipe 位置
                        var pos = currentRecipe.UpperMPosition;
                        GV.Plc.CorrectionCameraMove((int)pos.XL, (int)pos.XR, (int)pos.YL, (int)pos.YR);
                        // Z 軸移動（如有需要）
                        // GV.Plc.MoveCameraZ((int)pos.ZL, (int)pos.ZR);
                    }
                    else
                    {
                        // 移動下層相機到 Recipe 位置
                        var pos = currentRecipe.LowerMPosition;
                        GV.Plc.CorrectionCameraMove((int)pos.XL, (int)pos.XR, (int)pos.YL, (int)pos.YR);
                    }
                });

                MessageBox.Show("Move to recipe position completed.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Move failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnMoveToRecipePos.Enabled = true;
                btnMoveToRecipePos.Text = "Move to Recipe Pos";
            }
        }

        // ✅ 新增事件：儲存當前位置
        private void btnSaveCurrentPos_Click(object sender, EventArgs e)
        {
            if (currentRecipe == null)
            {
                MessageBox.Show("Please select a recipe first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string cameraType = isUpperCamera ? "Upper" : "Lower";
            if (MessageBox.Show($"Save current position to Recipe {currentRecipe.RecipeNumber} ({cameraType} Camera)?",
                "Confirm Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            try
            {
                // 從 PLC 讀取當前位置
                int[] nowLocation = GV.NowLocation;

                if (isUpperCamera)
                {
                    currentRecipe.UpperMPosition.XL = nowLocation[2];
                    currentRecipe.UpperMPosition.YL = nowLocation[3];
                    currentRecipe.UpperMPosition.ZL = nowLocation[11];
                    currentRecipe.UpperMPosition.XR = nowLocation[4];
                    currentRecipe.UpperMPosition.YR = nowLocation[5];
                    currentRecipe.UpperMPosition.ZR = nowLocation[12];
                    currentRecipe.UpperMPosition.Y = nowLocation[1];
                }
                else
                {
                    currentRecipe.LowerMPosition.XL = nowLocation[2];
                    currentRecipe.LowerMPosition.YL = nowLocation[3];
                    currentRecipe.LowerMPosition.ZL = nowLocation[11];
                    currentRecipe.LowerMPosition.XR = nowLocation[4];
                    currentRecipe.LowerMPosition.YR = nowLocation[5];
                    currentRecipe.LowerMPosition.ZR = nowLocation[12];
                }

                currentRecipe.Save();
                MessageBox.Show($"Position saved to Recipe {currentRecipe.RecipeNumber}.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Save failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ✅ 修改：取得相機影像的輔助方法
        private Mat GrabLeftCamera()
        {
            return isUpperCamera ? GV.LeftUpCam.Grab() : GV.LeftBackCam?.Grab() ?? new Mat();
        }

        private Mat GrabRightCamera()
        {
            return isUpperCamera ? GV.RightUpCam.Grab() : GV.RightBackCam?.Grab() ?? new Mat();
        }
        private Control FindControlRecursive(string name)
        {
            var matches = this.Controls.Find(name, true);
            return (matches != null && matches.Length > 0) ? matches[0] : null;
        }

        private void btTChuckUpdate_Click(object sender, EventArgs e)
        {
            var nudTReadChuckX = FindControlRecursive("nudTReadChuckX") as NumericUpDown;
            var nudTReadChuckY1 = FindControlRecursive("nudTReadChuckY1") as NumericUpDown;
            var nudTReadChuckY2 = FindControlRecursive("nudTReadChuckY2") as NumericUpDown;

            // 將 Now Located 的數值更新到 Setting
            if (nudTReadChuckX != null)
                GV.Plc.RecipeData[36] = (int)nudTReadChuckX.Value;
            if (nudTReadChuckY1 != null)
                GV.Plc.RecipeData[37] = (int)nudTReadChuckY1.Value;
            if (nudTReadChuckY2 != null)
                GV.Plc.RecipeData[38] = (int)nudTReadChuckY2.Value;

            UpdateTableTargetPositionDisplay();

            GM.WriteRecipeXml(GV.NowRecipeNumber);

            //MessageBox.Show("WEC 位置已更新", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void btTUpCCDUpdate_Click(object sender, EventArgs e)
        {
            

            var nudTReadUpBigY = FindControlRecursive("nudTReadUpBigY") as NumericUpDown;
            var nudTReadUpLeftX = FindControlRecursive("nudTReadUpLeftX") as NumericUpDown;
            var nudTReadUpLeftY = FindControlRecursive("nudTReadUpLeftY") as NumericUpDown;
            var nudTReadUpLeftZ = FindControlRecursive("nudTReadUpLeftZ") as NumericUpDown;
            var nudTReadUpRightX = FindControlRecursive("nudTReadUpRightX") as NumericUpDown;
            var nudTReadUpRightY = FindControlRecursive("nudTReadUpRightY") as NumericUpDown;
            var nudTReadUpRightZ = FindControlRecursive("nudTReadUpRightZ") as NumericUpDown;

            // 將 Now Located 的數值更新到 Setting
            if (nudTReadUpBigY != null)
                GV.Plc.RecipeData[27] = (int)nudTReadUpBigY.Value;
            if (nudTReadUpLeftX != null)
                GV.Plc.RecipeData[28] = (int)nudTReadUpLeftX.Value;
            if (nudTReadUpLeftY != null)
                GV.Plc.RecipeData[29] = (int)nudTReadUpLeftY.Value;
            if (nudTReadUpLeftZ != null)
                GV.Plc.RecipeData[30] = (int)nudTReadUpLeftZ.Value;
            if (nudTReadUpRightX != null)
                GV.Plc.RecipeData[31] = (int)nudTReadUpRightX.Value;
            if (nudTReadUpRightY != null)
                GV.Plc.RecipeData[32] = (int)nudTReadUpRightY.Value;
            if (nudTReadUpRightZ != null)
                GV.Plc.RecipeData[33] = (int)nudTReadUpRightZ.Value;

            // 更新顯示
            UpdateCameraTargetPositionDisplay();

            // 儲存到 Recipe
            GM.WriteRecipeXml(GV.NowRecipeNumber);

            MessageBox.Show("CCD 位置已更新", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void btTBackCCDUpdate_Click(object sender, EventArgs e)
        {
           
            var nudTReadBackLeftX = FindControlRecursive("nudTReadBackLeftX") as NumericUpDown;
            var nudTReadBackLeftY = FindControlRecursive("nudTReadBackLeftY") as NumericUpDown;
            var nudTReadBackLeftZ = FindControlRecursive("nudTReadBackLeftZ") as NumericUpDown;
            var nudTReadBackRightX = FindControlRecursive("nudTReadBackRightX") as NumericUpDown;
            var nudTReadBackRightY = FindControlRecursive("nudTReadBackRightY") as NumericUpDown;
            var nudTReadBackRightZ = FindControlRecursive("nudTReadBackRightZ") as NumericUpDown;

            // 將 Now Located 的數值更新到 Setting
            if (nudTReadBackLeftX != null)
                GV.Plc.RecipeData[40] = (int)nudTReadBackLeftX.Value;
            if (nudTReadBackLeftY != null)
                GV.Plc.RecipeData[41] = (int)nudTReadBackLeftY.Value;
            if (nudTReadBackLeftZ != null)
                GV.Plc.RecipeData[42] = (int)nudTReadBackLeftZ.Value;
            if (nudTReadBackRightX != null)
                GV.Plc.RecipeData[43] = (int)nudTReadBackRightX.Value;
            if (nudTReadBackRightY != null)
                GV.Plc.RecipeData[44] = (int)nudTReadBackRightY.Value;
            if (nudTReadBackRightZ != null)
                GV.Plc.RecipeData[45] = (int)nudTReadBackRightZ.Value;

            // 更新顯示
            UpdateCameraTargetPositionDisplay();

            // 儲存到 Recipe
            GM.WriteRecipeXml(GV.NowRecipeNumber);

            MessageBox.Show("CCD 位置已更新", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
    }
}
