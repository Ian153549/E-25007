using MRLibrary;
using OpenCvSharp.Extensions;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NSAA_16Axis
{
    public partial class DialogTuneLens : Form
    {
        Image MeasureImage;
        Point MeasureLineStartPt;
        Point MeasureLineEndPt;
        double OriginalLinePixelLength;
        private Recipe currentRecipe;
        private bool isUpperCamera = true;

        public DialogTuneLens()
        {
            InitializeComponent();
            for (int i = 1; i < 13; i++)
                comboBoxGoto.Items.Add("X" + i.ToString());

            comboBoxGoto.SelectedIndex = 0;

            //skZoomAndPanWindowMeasure.CanZoom = true;
           
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
            InitializeRecipeDisplay();       // 改用新方法
            InitializeCameraSelection();
            //LoadCurrentRecipe();
        }
        private void InitializeRecipeDisplay()
        {
            comboBoxRecipe.Items.Clear();
            int iNowRecipe = GV.Plc.ReadData16(GV.Plc.iNowRecipeNumber);
            comboBoxRecipe.Items.Add($"Recipe {iNowRecipe}");
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
                rbUpperCamera.Checked = false;
                isUpperCamera = false;
            }
            else
            {
                // Top CCD 模式
                rbUpperCamera.Checked = true;
                rbLowerCamera.Checked = false;
                isUpperCamera = true;
            }
        }
        private void DialogTuneLens_Load(object sender, EventArgs e)
        {
            //if (!this.IsHandleCreated)
            //{
            //    this.CreateHandle();
            //}

            //if (!skZoomAndPanWindowMeasure.IsHandleCreated)
            //{
            //    skZoomAndPanWindowMeasure.CreateControl();
            //}

            //skZoomAndPanWindowMeasure.CanZoom = true;
            //isUpperCamera = (GV.UpBackAlign != 2);
            //FreezeAllCameras();
            //rbLeftLens.Checked = true;

            //// 延遲設定相機，確保 Form 完全顯示後再執行
            //this.BeginInvoke(new Action(() =>
            //{
            //    SetCameraToWindow(true);
            //}));
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    if (!this.IsHandleCreated)
                    {
                        this.CreateHandle();
                    }

                    if (!skZoomAndPanWindowMeasure.IsHandleCreated)
                    {
                        skZoomAndPanWindowMeasure.CreateControl();
                    }

                    skZoomAndPanWindowMeasure.CanZoom = true;
                    isUpperCamera = (GV.UpBackAlign != 2);
                    FreezeAllCameras();

                    rbLeftLens.Checked = true;

                    // 延遲設定相機，確保 Form 完全顯示後再執行
                    this.BeginInvoke(new Action(() =>
                    {
                        SetCameraToWindow(true);
                    }));
                }));
                return;
            }

            if (!this.IsHandleCreated)
            {
                this.CreateHandle();
            }

            if (!skZoomAndPanWindowMeasure.IsHandleCreated)
            {
                skZoomAndPanWindowMeasure.CreateControl();
            }

            skZoomAndPanWindowMeasure.CanZoom = true;
            isUpperCamera = (GV.UpBackAlign != 2);
            FreezeAllCameras();
            rbLeftLens.Checked = true;

            // 延遲設定相機，確保 Form 完全顯示後再執行
            this.BeginInvoke(new Action(() =>
            {
                SetCameraToWindow(true);
            }));
        }
        private void FreezeAllCameras()
        {
            GV.LeftUpCam?.Freeze();
            GV.RightUpCam?.Freeze();
            GV.LeftBackCam?.Freeze();
            GV.RightBackCam?.Freeze();
        }
        private void SetCameraToWindow(bool isLeftLens)
        {
            // 確保在 UI 執行緒上執行
            if (InvokeRequired)
            {
                Invoke(new Action(() => SetCameraToWindow(isLeftLens)));
                return;
            }

            // 確保控制項 Handle 已建立
            if (!skZoomAndPanWindowMeasure.IsHandleCreated)
            {
                skZoomAndPanWindowMeasure.CreateControl();
            }

            // 先凍結所有相機
            FreezeAllCameras();

            // 短暫等待確保相機完全停止
            System.Threading.Thread.Sleep(100);

            try
            {
                if (GV.UpBackAlign == 2)
                {
                    // Bottom CCD 模式
                    if (isLeftLens)
                    {
                        if (GV.LeftBackCam != null && GV.AppSettingParm.LeftBackCamEnable)
                        {
                            GV.LeftBackCam.SetWindow(skZoomAndPanWindowMeasure);
                            GV.LeftBackCam.Live();
                        }
                        else
                        {
                            ShowCameraNotAvailableMessage("Left Back Camera");
                        }
                    }
                    else
                    {
                        if (GV.RightBackCam != null && GV.AppSettingParm.RightBackCamEnable)
                        {
                            GV.RightBackCam.SetWindow(skZoomAndPanWindowMeasure);
                            GV.RightBackCam.Live();
                        }
                        else
                        {
                            ShowCameraNotAvailableMessage("Right Back Camera");
                        }
                    }
                }
                else
                {
                    // Top CCD 模式
                    if (isLeftLens)
                    {
                        if (GV.LeftUpCam != null && GV.AppSettingParm.LeftUpCamEnable)
                        {
                            GV.LeftUpCam.SetWindow(skZoomAndPanWindowMeasure);
                            GV.LeftUpCam.Live();
                        }
                        else
                        {
                            ShowCameraNotAvailableMessage("Left Up Camera");
                        }
                    }
                    else
                    {
                        if (GV.RightUpCam != null && GV.AppSettingParm.RightUpCamEnable)
                        {
                            GV.RightUpCam.SetWindow(skZoomAndPanWindowMeasure);
                            GV.RightUpCam.Live();
                        }
                        else
                        {
                            ShowCameraNotAvailableMessage("Right Up Camera");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"設定相機視窗時發生錯誤: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void ShowCameraNotAvailableMessage(string cameraName)
        {
            MessageBox.Show($"{cameraName} is not available.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        private void rbLeftLens_CheckedChanged(object sender, EventArgs e)
        {
           if (rbLeftLens.Checked == true)
           {
                rbRightLens.Checked = false;
                tableLayoutPanel1.Enabled = false;
                rbMeasure.Checked = false;
                SetCameraToWindow(true); // 左鏡頭
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
                SetCameraToWindow(false);
                rbLive.Checked = true;
                gbParameter.Enabled = true;
                comboBoxGoto.SelectedIndex = GV.RightZoomLens.Magnification;
            }
        }

        private void DialogTuneLens_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                FreezeAllCameras();
                GV.Plc.Send("WR MR8614 1");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void radioButtonMeasure_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMeasure.Checked)
            {
                rbLive.Checked = false;
                GV.LeftUpCam.Freeze();
                GV.RightUpCam.Freeze();
                comboBoxGoto.Enabled = false;

                using (var mat = skZoomAndPanWindowMeasure.GetWindowImage())
                {
                    if (mat != null && !mat.Empty())
                    {
                        using (var bmp = BitmapConverter.ToBitmap(mat))
                        {
                            MeasureImage = (Image)bmp.Clone();
                            pictureBoxMeasure.Image = MeasureImage;
                        }
                    }
                }

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
                SetCameraToWindow(rbLeftLens.Checked);
            }
        }

        private bool IsPictureBoxMeasureMouseDown = false;
        private void pictureBoxMeasure_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MeasureLineStartPt = ClipPointToPicture(e.Location);
                IsPictureBoxMeasureMouseDown = true;
            }
        }


        
        private void pictureBoxMeasure_MouseUp(object sender, MouseEventArgs e)
        {
            IsPictureBoxMeasureMouseDown = false;
            //if (MeasureLineStartPt == MeasureLineEndPt) return;
            //ZoomInfo zi = skZoomAndPanWindowMeasure.GetZoomInfo();
            //OriginalLinePixelLength = (Math.Sqrt(Math.Pow((MeasureLineEndPt.X - MeasureLineStartPt.X) / zi.AftFitMagnificationX, 2) +
            //    Math.Pow((MeasureLineEndPt.Y - MeasureLineStartPt.Y) / zi.AftFitMagnificationY, 2)) / zi.ZoomRatio);
            //Graphics g = pictureBoxMeasure.CreateGraphics();
            //g.DrawString(OriginalLinePixelLength. ToString("F2"), new Font("Arial", 14), new SolidBrush(Color.Red), new PointF(MeasureLineEndPt.X + 10, MeasureLineEndPt.Y));
            //SendKeys.Send(OriginalLinePixelLength.ToString("F2"));
            MeasureLineEndPt = ClipPointToPicture(e.Location);
            if (MeasureLineStartPt == MeasureLineEndPt) return;
            if (MeasureImage == null) return;

            ZoomInfo zi = skZoomAndPanWindowMeasure.GetZoomInfo();
            OriginalLinePixelLength = (Math.Sqrt(Math.Pow((MeasureLineEndPt.X - MeasureLineStartPt.X) / zi.AftFitMagnificationX, 2) +
                Math.Pow((MeasureLineEndPt.Y - MeasureLineStartPt.Y) / zi.AftFitMagnificationY, 2)) / zi.ZoomRatio);

            Bitmap resultBmp = new Bitmap(MeasureImage);
            using (Graphics g = Graphics.FromImage(resultBmp))
            {
                
                g.DrawImage(MeasureImage, Point.Empty);

                using (Pen pen = new Pen(Color.Red, 2))
                {
                    g.DrawLine(pen, MeasureLineStartPt, MeasureLineEndPt);
                }

                string text = OriginalLinePixelLength.ToString("F2");
                using (Font font = new Font("Arial", 14))
                {
                    SizeF sz = g.MeasureString(text, font);

                    float tx = MeasureLineEndPt.X + 10f;
                    float ty = MeasureLineEndPt.Y + 10f;

                    if (tx + sz.Width > pictureBoxMeasure.ClientSize.Width) tx = pictureBoxMeasure.ClientSize.Width - sz.Width - 2f;
                    if (tx < 2f) tx = 2f;

                    if (ty + sz.Height > pictureBoxMeasure.ClientSize.Height) ty = MeasureLineEndPt.Y - sz.Height - 2f;
                    if (ty < 2f) ty = 2f;

                    g.DrawString(text, font, Brushes.Red, tx, ty);
                }
            }
            var prevImg = pictureBoxMeasure.Image;
            if (prevImg != null && !Object.ReferenceEquals(prevImg, MeasureImage))
            {
                pictureBoxMeasure.Image = null;
                prevImg.Dispose();
            }

            pictureBoxMeasure.Image = resultBmp;
            SendKeys.Send(OriginalLinePixelLength.ToString("F2"));
        }

        private void pictureBoxMeasure_MouseMove(object sender, MouseEventArgs e)
      {
            //Graphics g = pictureBoxMeasure.CreateGraphics();
            //Pen pen = new Pen(Color.Red);

            //MeasureLineEndPt = e.Location;

            MeasureLineEndPt=ClipPointToPicture(e.Location);
            if (IsPictureBoxMeasureMouseDown)
            {
                if (MeasureImage == null) return;
                Bitmap displayBmp = new Bitmap(MeasureImage);
                using(Graphics g=Graphics.FromImage(displayBmp))
                    using (Pen pen= new Pen(Color.Red, 2))
                {
                    g.DrawLine(pen,MeasureLineStartPt, MeasureLineEndPt);
                }
                var prev = pictureBoxMeasure.Image;
                if (prev != null && !Object.ReferenceEquals(prev, MeasureImage))
                {
                    pictureBoxMeasure.Image = null;
                    prev.Dispose();
                }

                pictureBoxMeasure.Image = displayBmp;

                // 輕微延遲以降低 CPU 使用（保留短暫等待）
                Thread.Sleep(10);
                //g.DrawImage(MeasureImage, new Point(0, 0));
                //g.DrawLine(pen, MeasureLineStartPt, e.Location);
                //Thread.Sleep(10);
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

        private Point ClipPointToPicture(Point pt)
        {
            int w=Math.Max(1,pictureBoxMeasure.ClientSize.Width);
            int h=Math.Max(1,pictureBoxMeasure.ClientSize.Height);

            int x = Math.Max(0, Math.Min(pt.X, w - 1));
            int y = Math.Max(0, Math.Min(pt.Y, h - 1));
            return new Point(x, y);
        }

        private void comboBoxRecipe_SelectedIndexChanged(object sender, EventArgs e)
        {
            //LoadCurrentRecipe();
        }


        private void rbUpperCamera_CheckedChanged(object sender, EventArgs e)
        {
            if (rbUpperCamera.Checked)
            {
                isUpperCamera = true;
                GV.LeftUpCam.SetWindow(skZoomAndPanWindowMeasure);
                if (rbLeftLens.Checked)
                    GV.LeftUpCam.Live();
                else
                    GV.RightUpCam.Live();
            }
        }

        private void rbLowerCamera_CheckedChanged(object sender, EventArgs e)
        {
            if (rbLowerCamera.Checked)
            {
                isUpperCamera = false;
                if (GV.LeftBackCam != null)
                {
                    GV.LeftBackCam.SetWindow(skZoomAndPanWindowMeasure);
                    if (rbLeftLens.Checked)
                        GV.LeftBackCam.Live();
                    else
                        GV.RightBackCam?.Live();
                }
                else
                {
                    MessageBox.Show("Lower camera not available.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    rbUpperCamera.Checked = true;
                }
            }
        }

        private async void btnMoveToRecipePos_Click(object sender, EventArgs e)
        {
            if (currentRecipe == null) return;

            btnMoveToRecipePos.Enabled = false;
            try
            {
                await Task.Run(() =>
                {
                    var pos = isUpperCamera ? currentRecipe.UpperMPosition : null;
                    var lowerPos = isUpperCamera ? null : currentRecipe.LowerMPosition;

                    if (pos != null)
                    {
                        GV.Plc.CorrectionCameraMove((int)pos.XL, (int)pos.XR, (int)pos.YL, (int)pos.YR);
                    }
                    else if (lowerPos != null)
                    {
                        GV.Plc.CorrectionCameraMove((int)lowerPos.XL, (int)lowerPos.XR, (int)lowerPos.YL, (int)lowerPos.YR);
                    }
                });
                MessageBox.Show("Move completed.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Move failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnMoveToRecipePos.Enabled = true;
            }
        }

        private void btnSaveCurrentPos_Click(object sender, EventArgs e)
        {
            if (currentRecipe == null) return;

            if (MessageBox.Show($"Save position to Recipe {currentRecipe.RecipeNumber}?",
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            try
            {
                int [] loc = GV.NowLocation;

                if (isUpperCamera)
                {
                    currentRecipe.UpperMPosition.XL = loc[2];
                    currentRecipe.UpperMPosition.YL = loc[3];
                    currentRecipe.UpperMPosition.ZL = loc[11];
                    currentRecipe.UpperMPosition.XR = loc[4];
                    currentRecipe.UpperMPosition.YR = loc[5];
                    currentRecipe.UpperMPosition.ZR = loc[12];
                    currentRecipe.UpperMPosition.Y = loc[1];
                }
                else
                {
                    currentRecipe.LowerMPosition.XL = loc[2];
                    currentRecipe.LowerMPosition.YL = loc[3];
                    currentRecipe.LowerMPosition.ZL = loc[11];
                    currentRecipe.LowerMPosition.XR = loc[4];
                    currentRecipe.LowerMPosition.YR = loc[5];
                    currentRecipe.LowerMPosition.ZR = loc[12];
                }

                currentRecipe.Save();
                MessageBox.Show("Position saved.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Save failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
