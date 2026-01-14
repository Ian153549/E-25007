using MRLibrary;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MRLibrary.OpenCV3MatchUMat;
using static System.Net.WebRequestMethods;

namespace NSAA_16Axis
{
    public partial class CMLearnPatternBack : UserControl
    {
        public int EditRecipe;
        public int NowMagni;
        public int SleepTime = 200;
        public int NowLevel;

        public double _dLalpha;
        public double _dRalpha;

        public int iLalpha;
        public int iRalpha;

        public Mat LeftMaskMat, RightMaskMat;
        public Mat LeftMask, RightMask;

        public MatchPosition LeftWaferMp = null;
        public MatchPosition LeftMaskMp = null;
        public MatchPosition RightWaferMp = null;
        public MatchPosition RightMaskMp = null;

        ProductMatchPositions BLastPmps = new ProductMatchPositions();
        ProductMatchPositions BLivePmps = new ProductMatchPositions();

        public Mat GMPBackLeft;
        public Mat GMPBackRight;
        public MatchPosition GMPBacklMaskMp;
        public MatchPosition GMPBackrMaskMp;
        public MatchPosition GMPBacklWaferMp;
        public MatchPosition GMPBackrWaferMp;
        MatchPosition lMaskMp = new MatchPosition();
        MatchPosition rMaskMp = new MatchPosition();
        MatchPosition lWaferMp = new MatchPosition();
        MatchPosition rWaferMp = new MatchPosition();

        public Recipe _recipe = null;
        public AlignCondition _AlignC = null;

        ProductMatchPositions OrgPmps = new ProductMatchPositions();
        public int[] OrgMotors = new int[3];

        private string _ClockTime;

        public int iAlignOK = 0;
        public int iAdmin = 0;

        public Thread DrawMatchThread = null;
        public int DebugMsgCount = 0;
        private readonly object _locker = new object();
        public Mat RViewMat;
        public Mat LViewMat;
        public Mat rotate = new Mat();
        public bool isLive = true;
        private System.Windows.Forms.Timer locationUpdateTimer;
        public CMLearnPatternBack()
        {
            InitializeComponent();
            GV.LeftDownWindowOnAlignPage = skLeft;
            GV.RightDownWindowOnAlignPage = skRight;
            if (DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;
            //GV.BackLearnLeft = skLeft;
            //GV.BackLearnRight = skRight;
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            skLeft.MaskMp = lMaskMp;
            skLeft.WaferMp = lWaferMp;
            skRight.MaskMp = rMaskMp;
            skRight.WaferMp = rWaferMp;
            skLeft.CanCenterLine = true;
            skLeft.CanRectMaskAndWafer = true;
            skLeft.CanTrackPattern = true;
            skLeft.CanZoom = true;
            skRight.CanCenterLine = true;
            skRight.CanRectMaskAndWafer = true;
            skRight.CanTrackPattern = true;
            skRight.CanZoom = true;
            //GV.BackLearnLeft = skLeft;
            //GV.BackLearnRight = skRight;
        }

        private async void CMLearnPattern_Load(object sender, EventArgs e)
        {
            
            if (GV.AppSettingParm.DebugMode)
            {
                lbDebugMsg.Visible = true;
            }

            EditRecipe = GV.NowRecipeNumber;
            _recipe = GV._recipe;
            _AlignC = _recipe.AlignC;
            cbLWaferAlgo.SelectedIndex = 0;
            cbRWaferAlgo.SelectedIndex = 0;
            cbLBackMaskAlgo.SelectedIndex = 0;
            cbRBackMaskAlgo.SelectedIndex = 0;
            if (GV.AppSettingParm.Language != "default")
            {
                btLTopMaskSave.Text = GV.Dlang.strSave;
                btRTopMaskSave.Text = GV.Dlang.strSave;
                btLBackMaskSave.Text = GV.Dlang.strSave;
                btRBackMaskSave.Text = GV.Dlang.strSave;
                btLWaferSave.Text = GV.Dlang.strSave;
                btRWaferSave.Text = GV.Dlang.strSave;
                btMagSave.Text = GV.Dlang.strSave;
                btDelete.Text = GV.Dlang.strDelete;
                btDefault.Text = GV.Dlang.strDefault;
                gbLWafer.Text = GV.Dlang.strWafer;
                gbRWafer.Text = GV.Dlang.strWafer;
                gbLTopMask.Text = GV.Dlang.strTop + GV.Dlang.strMask;
                gbRTopMask.Text = GV.Dlang.strTop + GV.Dlang.strMask;
                gbLBackMask.Text = GV.Dlang.strBack + GV.Dlang.strMask;
                gbRBackMask.Text = GV.Dlang.strBack + GV.Dlang.strMask;
                gBMag.Text = GV.Dlang.groupBoxMagnification;
                rBHighMagnification.Text = GV.Dlang.radioButtonHighMagnification;
                rBLowMagnification.Text = GV.Dlang.radioButtonLowMagnification;
                rbLTopMask.Text = GV.Dlang.strLight;
                rbRTopMask.Text = GV.Dlang.strLight;
                rbLBackMask.Text = GV.Dlang.strLight;
                rbRBackMask.Text = GV.Dlang.strLight;
                rbLWafer.Text = GV.Dlang.strLight;
                rbRWafer.Text = GV.Dlang.strLight;
                label1.Text = GV.Dlang.strAlgo;
                label2.Text = GV.Dlang.strAlgo;
                label3.Text = GV.Dlang.strAlgo;
                label4.Text = GV.Dlang.strAlgo;
                label5.Text = GV.Dlang.strAlgo;
                label6.Text = GV.Dlang.strAlgo;
                lbLLive.Text = GV.Dlang.strLive;
                lbRLive.Text = GV.Dlang.strLive;
                lbLRecord.Text = GV.Dlang.strRecord;
                lbRRecord.Text = GV.Dlang.strRecord;
                cBTopMask.Text = GV.Dlang.strTopMask;
                cBBottomMask.Text = GV.Dlang.strBottomMask;
                btCapMask.Caption = GV.Dlang.strCapMask;
                btAlignTest.Caption = GV.Dlang.strAlignTest;
                BtMaskCenter.Caption = GV.Dlang.strMaskCenter;
                btCaptureN.Caption = GV.Dlang.strCapture;
                btFindLBMaskCenter.Text = GV.Dlang.strFindCenter;
                btFindLWaferCenter.Text = GV.Dlang.strFindCenter;
                btFindRWaferCenter.Text = GV.Dlang.strFindCenter;
                btFindRBMaskCenter.Text = GV.Dlang.strFindCenter;
                btLBackMask.Text = GV.Dlang.strCreateMark;
                btLWaferMask.Text = GV.Dlang.strCreateMark;
                btRWaferMask.Text = GV.Dlang.strCreateMark;
                btRBackMask.Text = GV.Dlang.strCreateMark;
                btBackLabelPatternL.Text = GV.Dlang.strImageLabel;
                btBackLabelPatternR.Text = GV.Dlang.strImageLabel;
                cbLWaferAlgo.Items.Clear();
                cbRWaferAlgo.Items.Clear();
                cbLBackMaskAlgo.Items.Clear();
                cbRBackMaskAlgo.Items.Clear();
                cbLWaferAlgo.Items.Add(GV.Dlang.strTemplate);
                cbLWaferAlgo.Items.Add(GV.Dlang.strEdge);
                cbLWaferAlgo.Items.Add(GV.Dlang.strAI);
                cbRWaferAlgo.Items.Add(GV.Dlang.strTemplate);
                cbRWaferAlgo.Items.Add(GV.Dlang.strEdge);
                cbRWaferAlgo.Items.Add(GV.Dlang.strAI);
                cbLBackMaskAlgo.Items.Add(GV.Dlang.strTemplate);
                cbLBackMaskAlgo.Items.Add(GV.Dlang.strEdge);
                cbLBackMaskAlgo.Items.Add(GV.Dlang.strAI);
                cbRBackMaskAlgo.Items.Add(GV.Dlang.strTemplate);
                cbRBackMaskAlgo.Items.Add(GV.Dlang.strEdge);
                cbRBackMaskAlgo.Items.Add(GV.Dlang.strAI);
            }

            LeftMaskMat = GV.LeftMaskMat;
            RightMaskMat = GV.RightMaskMat;
            tBLShowImage.Enabled = false;
            tBRShowImage.Enabled = false;


            if (GV.AppSettingParm.LeftUpCamEnable == true)
            {
                lbEmulationModeL.Visible = false;
            }
            else
            {
                skLeft.CanRectMaskAndWafer = false;
            }
            if (GV.AppSettingParm.RightUpCamEnable == true)
            {
                lbEmulationModeR.Visible = false;
            }
            else
            {
                skRight.CanRectMaskAndWafer = false;
            }

            if (GV.AppSettingParm.DebugMode == true)
            {
                btOpen.Visible = true;
            }

            //NUDLMaskX.Visible = false;
            //NUDLMaskY.Visible = false;
            //NUDRMaskX.Visible = false;
            //NUDRMaskY.Visible = false;
            //BtLMaskAdjSave.Visible = false;
            //BtRMaskAdjSave.Visible = false;
            //lblLMaskX.Visible = false;
            //lblLMaskY.Visible = false;
            //lblRMaskX.Visible = false;
            //lblRMaskY.Visible = false;
            //skLeft.MaskMp = lMaskMp;
            //skLeft.WaferMp = lWaferMp;
            //skRight.MaskMp = rMaskMp;
            //skRight.WaferMp = rWaferMp;
            locationUpdateTimer = new System.Windows.Forms.Timer();
            locationUpdateTimer.Interval = 500; // 200ms 更新一次
            locationUpdateTimer.Tick += LocationUpdateTimer_Tick;
            try
            {
                await Task.Run(() =>
                {
                    try
                    {
                        // 1) 取回或載入 Mask（若尚未存在）
                        if ((LeftMaskMat == null || LeftMaskMat.Empty()) && System.IO.File.Exists(GetTemplateFileName("LeftMask")))
                        {
                            try
                            {
                                var lm = Cv2.ImRead(GetTemplateFileName("LeftMask"), ImreadModes.Grayscale);
                                if (lm != null && !lm.Empty())
                                {
                                    LeftMaskMat = lm;
                                    GV.LeftMaskMat = LeftMaskMat;
                                }
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("Load LeftMask failed: " + ex.Message);
                            }
                        }

                        if ((RightMaskMat == null || RightMaskMat.Empty()) && System.IO.File.Exists(GetTemplateFileName("RightMask")))
                        {
                            try
                            {
                                var rm = Cv2.ImRead(GetTemplateFileName("RightMask"), ImreadModes.Grayscale);
                                if (rm != null && !rm.Empty())
                                {
                                    RightMaskMat = rm;
                                    GV.RightMaskMat = RightMaskMat;
                                }
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("Load RightMask failed: " + ex.Message);
                            }
                        }

                        // 2) 並行讓 matcher 做學習（若非 Emulation）
                        if (GV.AppSettingParm.Emulation != true)
                        {
                            var learnTasks = new List<Task>();

                            if (_recipe.LeftLowMaskMat != null && !_recipe.LeftLowMaskMat.Empty() && GV.matcherLLM != null)
                            {
                                learnTasks.Add(Task.Run(() => GV.matcherLLM.LearnWithAlgo(_recipe.LeftLowMaskMat, _recipe.LeftLowMaskMask, _AlignC.LLMaskAlgorithm)));
                            }
                            if (_recipe.RightLowMaskMat != null && !_recipe.RightLowMaskMat.Empty() && GV.matcherRLM != null)
                            {
                                learnTasks.Add(Task.Run(() => GV.matcherRLM.LearnWithAlgo(_recipe.RightLowMaskMat, _recipe.RightLowMaskMask, _AlignC.RLMaskAlgorithm)));
                            }
                            if (_recipe.LeftHighWaferMat != null && !_recipe.LeftHighWaferMat.Empty() && GV.matcherLHW != null)
                            {
                                learnTasks.Add(Task.Run(() => GV.matcherLHW.LearnWithAlgo(_recipe.LeftHighWaferMat, _recipe.LeftHighWaferMask, _AlignC.LHWaferAlgorithm)));
                            }
                            if (_recipe.RightHighWaferMat != null && !_recipe.RightHighWaferMat.Empty() && GV.matcherRHW != null)
                            {
                                learnTasks.Add(Task.Run(() => GV.matcherRHW.LearnWithAlgo(_recipe.RightHighWaferMat, _recipe.RightHighWaferMask, _AlignC.RHWaferAlgorithm)));
                            }

                            try
                            {
                                Task.WaitAll(learnTasks.ToArray(), 1000); // 等待，但避免無限等待（可調 timeout）
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("Matcher learn tasks error: " + ex.Message);
                            }
                        }

                        // 3) 取得 ClassList（如果需要）
                        try
                        {
                            var classList = GV.AIClassList?.GetClassList();
                            if (classList != null)
                            {
                                // 回到 UI 執行緒更新 ComboBox
                                this.BeginInvoke(new Action(() => UpdateClassList(classList)));
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("GetClassList failed: " + ex.Message);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Background init failed: " + ex.Message);
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("CMLearnPattern_Load Task.Run failed: " + ex.Message);
            }

            // 回到 UI 執行緒：完成 UI 綁定、啟動 camera 與 timer
            try
            {
                // 設定顯示相關參數與控制元件
                if (_recipe != null)
                {
                    _AlignC = _recipe.AlignC;
                }

                int iL = (int)(_AlignC.dLalpha * 100);
                if (iL > 100) iL = 100;
                if (iL < 0) iL = 0;
                int iR = (int)(_AlignC.dRalpha * 100);
                if (iR > 100) iR = 100;
                if (iR < 0) iR = 0;

                iLalpha = iL;
                iRalpha = iR;

                tBLShowImage.Value = iLalpha;
                tBRShowImage.Value = iRalpha;

                _dLalpha = (double)tBLShowImage.Value / 100;
                _dRalpha = (double)tBRShowImage.Value / 100;

                if (rbLTopMask.Checked == false)
                {
                    skLeft.SetShowMask(true, _dLalpha, LeftMaskMat);
                    skRight.SetShowMask(true, _dRalpha, RightMaskMat);
                }
                else
                {
                    skLeft.SetShowMask(false, 0, LeftMaskMat);
                    skRight.SetShowMask(false, 0, RightMaskMat);
                }

                // 設定 camera window 並啟動 Live（在 UI 執行緒）
                GV.LeftBackCam.SetWindow(skLeft);
                GV.RightBackCam.SetWindow(skRight);

                if (GV.AppSettingParm.LeftBackCamEnable)
                    GV.LeftBackCam?.Live();

                if (GV.AppSettingParm.RightBackCamEnable)
                    GV.RightBackCam?.Live();

                // 啟動 location timer（延後到重型作業完成）
                //locationUpdateTimer.Start();

                // 啟動 DrawMatchPosition 執行緒（標示為背景執行緒）
                if (DrawMatchThread == null)
                {
                    DrawMatchThread = new Thread(DrawMatchPosition) { IsBackground = true };
                    DrawMatchThread.Start();
                }

                UpdateTargetPositionDisplay();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("UI finalization failed: " + ex.Message);
            }
            //locationUpdateTimer.Start();
            //UpdateTargetPositionDisplay();
            groupBoxB1.Visible = false;
            groupBoxB2.Visible = false;
        }
        private void LocationUpdateTimer_Tick(object sender, EventArgs e)
        {
            UpdateCurrentLocationUI();
        }
        private void UpdateCurrentLocationUI()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(UpdateCurrentLocationUI));
                    return;
                }

                int[] nowLocation = GV.NowLocation;

                // ✅ 將 PLC 數值除以 10 後顯示 - WEC Location 現在位置
                nudReadChuckX.Value = (decimal)(GV.NowLocation[GV.Plc.iiDChuckX] / 10.0);
                nudReadChuckY1.Value = (decimal)(GV.NowLocation[GV.Plc.iiDChuckY1] / 10.0);
                nudReadChuckY2.Value = (decimal)(GV.NowLocation[GV.Plc.iiDChuckY2] / 10.0);
                nudReadChuckZ.Value = (decimal)(GV.NowLocation[GV.Plc.iiDChuckZ] / 10.0);

                // ✅ 將 PLC 數值除以 10 後顯示 - CCD Location 現在位置 (Back Camera)
                nudReadBigY.Value = (decimal)(GV.NowLocation[GV.Plc.iiDUpBigY] / 10.0);
                nudReadBackLeftX.Value = (decimal)(GV.NowLocation[GV.Plc.iiDDownLeftX] / 10.0);
                nudReadBackLeftY.Value = (decimal)(GV.NowLocation[GV.Plc.iiDDownLeftY] / 10.0);
                nudReadBackLeftZ.Value = (decimal)(GV.NowLocation[GV.Plc.iiDDownLeftZ] / 10.0);
                nudReadBackRightX.Value = (decimal)(GV.NowLocation[GV.Plc.iiDDownRightX] / 10.0);
                nudReadBackRightY.Value = (decimal)(GV.NowLocation[GV.Plc.iiDDownRightY] / 10.0);
                nudReadBackRightZ.Value = (decimal)(GV.NowLocation[GV.Plc.iiDDownRightZ] / 10.0);
            }
            catch (Exception ex)
            {
                // 避免更新 UI 時發生例外導致程式中斷
                System.Diagnostics.Debug.WriteLine($"UpdateCurrentLocationUI Error: {ex.Message}");
            }
        }
        private void UpdateTargetPositionDisplay()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(UpdateTargetPositionDisplay));
                    return;
                }

                if (_recipe == null) return;

                // 顯示 WEC Location 目標位置 (從 LowerWECPos)
                nudWriteChuckX.Text = _recipe.LowerWECPos.X.ToString("F0");
                nudWriteChuckY1.Text = _recipe.LowerWECPos.Y.ToString("F0");
                nudWriteChuckY2.Text = _recipe.LowerWECPos.A.ToString("F0");
                nudWriteChuckZ.Text = _recipe.LowerWECPos.Z.ToString("F0"); // 如果有對應欄位請調整

                // 顯示 CCD Location 目標位置 (從 LowerMPosition)
                nudWriteBackLeftX.Text = _recipe.LowerMPosition.XL.ToString("F0");
                nudWriteBackLeftY.Text = _recipe.LowerMPosition.YL.ToString("F0");
                nudWriteBackLeftZ.Text = _recipe.LowerMPosition.ZL.ToString("F0");
                nudWriteBackRightX.Text = _recipe.LowerMPosition.XR.ToString("F0");
                nudWriteBackRightY.Text = _recipe.LowerMPosition.YR.ToString("F0");
                nudWriteBackRightZ.Text = _recipe.LowerMPosition.ZR.ToString("F0");
                nudWriteUpBigY.Text = "0"; // 如果有對應欄位請調整
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"UpdateTargetPositionDisplay Error: {ex.Message}");
            }
        }
        private void BtBackCCDUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("確定要將現在位置更新到目標位置嗎？", "更新 CCD Location",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                if (_recipe == null) return;

                // 從現在位置讀取並寫入 Recipe 的 LowerMPosition
                _recipe.LowerMPosition.XL = (double)nudReadBackLeftX.Value;
                _recipe.LowerMPosition.YL = (double)nudReadBackLeftY.Value;
                _recipe.LowerMPosition.ZL = (double)nudReadBackLeftZ.Value;
                _recipe.LowerMPosition.XR = (double)nudReadBackRightX.Value;
                _recipe.LowerMPosition.YR = (double)nudReadBackRightY.Value;
                _recipe.LowerMPosition.ZR = (double)nudReadBackRightZ.Value;

                _recipe.LowerWECPos.Z = (double)nudReadChuckZ.Value;
                _recipe.LowerWECPos.X = (double)nudReadChuckX.Value;
                _recipe.LowerWECPos.Y = (double)nudReadChuckY1.Value;
                _recipe.LowerWECPos.A = (double)nudReadChuckY2.Value;
                // 儲存 Recipe
                _recipe.Save();

                // 更新目標位置顯示
                UpdateTargetPositionDisplay();

                MessageBox.Show("CCD Location 已更新。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"更新失敗: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //public void DrawMatchPosition()
        //{

        //    while (!GV.AppEnding)
        //    {
        //        while (GV.TabOption == GV.Tab.BackLearn)
        //        {
        //            if (GV.AppSettingParm.Emulation == true || GV.matcherLLM == null)
        //            {
        //                Thread.Sleep(2000);
        //                continue;
        //            }   
        //            Mat leftImageSrc = GV.LeftBackCam.IsFileImage ? LViewMat : GV.LeftBackCam.Grab();
        //            Mat rightImageSrc = GV.RightBackCam.IsFileImage ? RViewMat : GV.RightBackCam.Grab();
        //            using (Mat leftImage = leftImageSrc?.Clone())
        //            using (Mat rightImage = rightImageSrc?.Clone())
        //            {
        //                int lWaferClassId = _AlignC.LHWaferAIClassId;
        //                int rWaferClassId = _AlignC.RHWaferAIClassId;
        //                int lMaskClassId = _AlignC.LLMaskAIClassId;
        //                int rMaskClassId = _AlignC.RLMaskAIClassId;
        //                //if (rbLBackMask.Checked)
        //                //{

        //                //    Invoke((MethodInvoker)delegate ()
        //                //    {
        //                //        if (_AlignC.LHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && cbLBackWaferClassList.SelectedIndex >= 0)
        //                //        {
        //                //            lWaferClassId = cbLBackWaferClassList.SelectedIndex;
        //                //        }
        //                //        if (_AlignC.RHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && cbRBackWaferClassList.SelectedIndex >= 0)
        //                //        {
        //                //            rWaferClassId = cbRBackWaferClassList.SelectedIndex;
        //                //        }
        //                //        if (_AlignC.LLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && cbLBackMaskClassList.SelectedIndex >= 0)
        //                //        {
        //                //            lMaskClassId = cbLBackMaskClassList.SelectedIndex;
        //                //        }
        //                //        if (_AlignC.RLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && cbRBackMaskClassList.SelectedIndex >= 0)
        //                //        {
        //                //            rMaskClassId = cbRBackMaskClassList.SelectedIndex;
        //                //        }
        //                //    });
        //                //    //GV.matcherLLW.MatMatchWithAlgo(0, leftImage, ref lMaskMp, _AlignC.LLWaferAlgorithm);
        //                //    //GV.matcherRLW.MatMatchWithAlgo(0, rightImage, ref rMaskMp, _AlignC.RLWaferAlgorithm);
        //                //    if (_AlignC.LLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
        //                //    {
        //                //        GV.matcherLLM.MatMatchWithAlgo(0, LeftMaskMat, ref lMaskMp, _AlignC.LLMaskAlgorithm, lMaskClassId);
        //                //    }
        //                //    else
        //                //    {
        //                //        GV.matcherLLM.MatMatchWithAlgo(0, LeftMaskMat, ref lMaskMp, _AlignC.LLMaskAlgorithm);
        //                //    }

        //                //    if (_AlignC.RLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
        //                //    {
        //                //        GV.matcherRLM.MatMatchWithAlgo(0, RightMaskMat, ref rMaskMp, _AlignC.RLMaskAlgorithm, rMaskClassId);
        //                //    }
        //                //    else
        //                //    {
        //                //        GV.matcherRLM.MatMatchWithAlgo(0, RightMaskMat, ref rMaskMp, _AlignC.RLMaskAlgorithm);
        //                //    }

        //                //    if (_AlignC.LHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
        //                //    {
        //                //        GV.matcherLHW.MatMatchWithAlgo(0, leftImage, ref lWaferMp, _AlignC.LHWaferAlgorithm, lWaferClassId);
        //                //    }
        //                //    else
        //                //    {
        //                //        GV.matcherLHW.MatMatchWithAlgo(0, leftImage, ref lWaferMp, _AlignC.LHWaferAlgorithm);
        //                //    }

        //                //    if (_AlignC.RHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
        //                //    {
        //                //        GV.matcherRHW.MatMatchWithAlgo(0, rightImage, ref rWaferMp, _AlignC.RHWaferAlgorithm, rWaferClassId);
        //                //    }
        //                //    else
        //                //    {
        //                //        GV.matcherRHW.MatMatchWithAlgo(0, rightImage, ref rWaferMp, _AlignC.RHWaferAlgorithm);
        //                //    }
        //                //}
        //                //else if(rbLWafer.Checked)
        //                //{
        //                //    Invoke((MethodInvoker)delegate ()
        //                //    {
        //                //        if (_AlignC.LHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && cbLBackWaferClassList.SelectedIndex >= 0)
        //                //        {
        //                //            lWaferClassId = cbLBackWaferClassList.SelectedIndex;
        //                //        }
        //                //        if (_AlignC.RHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && cbRBackWaferClassList.SelectedIndex >= 0)
        //                //        {
        //                //            rWaferClassId = cbRBackWaferClassList.SelectedIndex;
        //                //        }
        //                //        if (_AlignC.LLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && cbLBackMaskClassList.SelectedIndex >= 0)
        //                //        {
        //                //            lMaskClassId = cbLBackMaskClassList.SelectedIndex;
        //                //        }
        //                //        if (_AlignC.RLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && cbRBackMaskClassList.SelectedIndex >= 0)
        //                //        {
        //                //            rMaskClassId = cbRBackMaskClassList.SelectedIndex;
        //                //        }
        //                //    });
        //                //    if (_AlignC.LLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
        //                //    {
        //                //        GV.matcherLLM.MatMatchWithAlgo(0, LeftMaskMat, ref lMaskMp, _AlignC.LLMaskAlgorithm,lMaskClassId);
        //                //    }
        //                //    else
        //                //    {
        //                //        GV.matcherLLM.MatMatchWithAlgo(0, LeftMaskMat, ref lMaskMp, _AlignC.LLMaskAlgorithm);
        //                //    }
        //                //    if (_AlignC.RLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
        //                //    {
        //                //        GV.matcherRLM.MatMatchWithAlgo(0, RightMaskMat, ref rMaskMp, _AlignC.RLMaskAlgorithm,rMaskClassId);
        //                //    }
        //                //    else
        //                //    {
        //                //        GV.matcherRLM.MatMatchWithAlgo(0, RightMaskMat, ref rMaskMp, _AlignC.RLMaskAlgorithm);
        //                //    }
        //                //    if (_AlignC.LHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
        //                //    {
        //                //        GV.matcherLHW.MatMatchWithAlgo(0, leftImage, ref lWaferMp, _AlignC.LHWaferAlgorithm,lWaferClassId);
        //                //    }
        //                //    else
        //                //    {
        //                //        GV.matcherLHW.MatMatchWithAlgo(0, leftImage, ref lWaferMp, _AlignC.LHWaferAlgorithm);
        //                //    }
        //                //    if (_AlignC.RHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
        //                //    {
        //                //        GV.matcherRHW.MatMatchWithAlgo(0, rightImage, ref rWaferMp, _AlignC.RHWaferAlgorithm,rWaferClassId);
        //                //    }
        //                //    else
        //                //    {
        //                //        GV.matcherRHW.MatMatchWithAlgo(0, rightImage, ref rWaferMp, _AlignC.RHWaferAlgorithm);
        //                //    }
        //                //}
        //                if (rbLBackMask.Checked || rbLWafer.Checked)
        //                {
        //                    Invoke((MethodInvoker)delegate ()
        //                    {
        //                        if (_AlignC.LHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && cbLBackWaferClassList.SelectedIndex >= 0)
        //                        {
        //                            lWaferClassId = cbLBackWaferClassList.SelectedIndex;
        //                        }
        //                        if (_AlignC.RHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && cbRBackWaferClassList.SelectedIndex >= 0)
        //                        {
        //                            rWaferClassId = cbRBackWaferClassList.SelectedIndex;
        //                        }
        //                        if (_AlignC.LLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && cbLBackMaskClassList.SelectedIndex >= 0)
        //                        {
        //                            lMaskClassId = cbLBackMaskClassList.SelectedIndex;
        //                        }
        //                        if (_AlignC.RLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && cbRBackMaskClassList.SelectedIndex >= 0)
        //                        {
        //                            rMaskClassId = cbRBackMaskClassList.SelectedIndex;
        //                        }
        //                    });

        //                    // ✅ 左下光罩搜尋（支援多十字 ROI）
        //                    if (_AlignC.LLMaskROI.Width > 0 && _AlignC.LLMaskROI.Height > 0)
        //                    {
        //                        // 多十字模式：只在指定 ROI 區域搜尋
        //                        Mat leftMaskROI = new Mat(LeftMaskMat, _AlignC.LLMaskROI);
        //                        MatchPosition tempMp = new MatchPosition();

        //                        if (_AlignC.LLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
        //                        {
        //                            GV.matcherLLM.MatMatchWithAlgo(0, leftMaskROI, ref tempMp, _AlignC.LLMaskAlgorithm, lMaskClassId);
        //                        }
        //                        else
        //                        {
        //                            GV.matcherLLM.MatMatchWithAlgo(0, leftMaskROI, ref tempMp, _AlignC.LLMaskAlgorithm);
        //                        }

        //                        // 將 ROI 相對座標轉換回完整影像座標
        //                        lMaskMp.X = tempMp.X + _AlignC.LLMaskROI.X;
        //                        lMaskMp.Y = tempMp.Y + _AlignC.LLMaskROI.Y;
        //                        lMaskMp.Score = tempMp.Score;
        //                    }
        //                    else
        //                    {
        //                        // 單十字模式：全影像搜尋
        //                        if (_AlignC.LLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
        //                        {
        //                            GV.matcherLLM.MatMatchWithAlgo(0, LeftMaskMat, ref lMaskMp, _AlignC.LLMaskAlgorithm, lMaskClassId);
        //                        }
        //                        else
        //                        {
        //                            GV.matcherLLM.MatMatchWithAlgo(0, LeftMaskMat, ref lMaskMp, _AlignC.LLMaskAlgorithm);
        //                        }
        //                    }

        //                    // ✅ 右下光罩搜尋（支援多十字 ROI）
        //                    if (_AlignC.RLMaskROI.Width > 0 && _AlignC.RLMaskROI.Height > 0)
        //                    {
        //                        Mat rightMaskROI = new Mat(RightMaskMat, _AlignC.RLMaskROI);
        //                        MatchPosition tempMp = new MatchPosition();

        //                        if (_AlignC.RLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
        //                        {
        //                            GV.matcherRLM.MatMatchWithAlgo(0, rightMaskROI, ref tempMp, _AlignC.RLMaskAlgorithm, rMaskClassId);
        //                        }
        //                        else
        //                        {
        //                            GV.matcherRLM.MatMatchWithAlgo(0, rightMaskROI, ref tempMp, _AlignC.RLMaskAlgorithm);
        //                        }

        //                        rMaskMp.X = tempMp.X + _AlignC.RLMaskROI.X;
        //                        rMaskMp.Y = tempMp.Y + _AlignC.RLMaskROI.Y;
        //                        rMaskMp.Score = tempMp.Score;
        //                    }
        //                    else
        //                    {
        //                        if (_AlignC.RLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
        //                        {
        //                            GV.matcherRLM.MatMatchWithAlgo(0, RightMaskMat, ref rMaskMp, _AlignC.RLMaskAlgorithm, rMaskClassId);
        //                        }
        //                        else
        //                        {
        //                            GV.matcherRLM.MatMatchWithAlgo(0, RightMaskMat, ref rMaskMp, _AlignC.RLMaskAlgorithm);
        //                        }
        //                    }

        //                    // ✅ 左下晶圓搜尋（支援多十字 ROI）
        //                    if (_AlignC.LHWaferROI.Width > 0 && _AlignC.LHWaferROI.Height > 0)
        //                    {
        //                        Mat leftWaferROI = new Mat(leftImage, _AlignC.LHWaferROI);
        //                        MatchPosition tempMp = new MatchPosition();

        //                        if (_AlignC.LHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
        //                        {
        //                            GV.matcherLHW.MatMatchWithAlgo(0, leftWaferROI, ref tempMp, _AlignC.LHWaferAlgorithm, lWaferClassId);
        //                        }
        //                        else
        //                        {
        //                            GV.matcherLHW.MatMatchWithAlgo(0, leftWaferROI, ref tempMp, _AlignC.LHWaferAlgorithm);
        //                        }

        //                        lWaferMp.X = tempMp.X + _AlignC.LHWaferROI.X;
        //                        lWaferMp.Y = tempMp.Y + _AlignC.LHWaferROI.Y;
        //                        lWaferMp.Score = tempMp.Score;
        //                    }
        //                    else
        //                    {
        //                        if (_AlignC.LHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
        //                        {
        //                            GV.matcherLHW.MatMatchWithAlgo(0, leftImage, ref lWaferMp, _AlignC.LHWaferAlgorithm, lWaferClassId);
        //                        }
        //                        else
        //                        {
        //                            GV.matcherLHW.MatMatchWithAlgo(0, leftImage, ref lWaferMp, _AlignC.LHWaferAlgorithm);
        //                        }
        //                    }

        //                    // ✅ 右下晶圓搜尋（支援多十字 ROI）
        //                    if (_AlignC.RHWaferROI.Width > 0 && _AlignC.RHWaferROI.Height > 0)
        //                    {
        //                        Mat rightWaferROI = new Mat(rightImage, _AlignC.RHWaferROI);
        //                        MatchPosition tempMp = new MatchPosition();

        //                        if (_AlignC.RHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
        //                        {
        //                            GV.matcherRHW.MatMatchWithAlgo(0, rightWaferROI, ref tempMp, _AlignC.RHWaferAlgorithm, rWaferClassId);
        //                        }
        //                        else
        //                        {
        //                            GV.matcherRHW.MatMatchWithAlgo(0, rightWaferROI, ref tempMp, _AlignC.RHWaferAlgorithm);
        //                        }

        //                        rWaferMp.X = tempMp.X + _AlignC.RHWaferROI.X;
        //                        rWaferMp.Y = tempMp.Y + _AlignC.RHWaferROI.Y;
        //                        rWaferMp.Score = tempMp.Score;
        //                    }
        //                    else
        //                    {
        //                        if (_AlignC.RHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
        //                        {
        //                            GV.matcherRHW.MatMatchWithAlgo(0, rightImage, ref rWaferMp, _AlignC.RHWaferAlgorithm, rWaferClassId);
        //                        }
        //                        else
        //                        {
        //                            GV.matcherRHW.MatMatchWithAlgo(0, rightImage, ref rWaferMp, _AlignC.RHWaferAlgorithm);
        //                        }
        //                    }
        //                }
        //                BLivePmps.LMaskMp = lMaskMp;
        //                BLivePmps.LWaferMp = lWaferMp;
        //                BLivePmps.RMaskMp = rMaskMp;
        //                BLivePmps.RWaferMp = rWaferMp;
        //                Invoke((MethodInvoker)delegate ()
        //                {
        //                    skLeft.MaskMp = lMaskMp;
        //                    skLeft.WaferMp = lWaferMp;
        //                    skRight.MaskMp = rMaskMp;
        //                    skRight.WaferMp = rWaferMp;
        //                    //skLeft.MaskMp = new MatchPosition { X = lMaskMp.X, Y = lMaskMp.Y, Score = lMaskMp.Score };
        //                    //skLeft.WaferMp = new MatchPosition { X = lWaferMp.X, Y = lWaferMp.Y, Score = lWaferMp.Score };
        //                    //skRight.MaskMp = new MatchPosition { X = rMaskMp.X, Y = rMaskMp.Y, Score = rMaskMp.Score };
        //                    //skRight.WaferMp = new MatchPosition { X = rWaferMp.X, Y = rWaferMp.Y, Score = rWaferMp.Score };
        //                    double dLX = lMaskMp.X - lWaferMp.X;
        //                    double dLY = lMaskMp.Y - lWaferMp.Y;
        //                    double dRX = rMaskMp.X - rWaferMp.X;
        //                    double dRY = rMaskMp.Y - rWaferMp.Y;

        //                    string msgLM = string.Format("LMX = {0:N3}, LMY = {1:N3}, dX = {2:N3}, dY = {3:N3}, MScore = {4:N2}",
        //                        lMaskMp.X, lMaskMp.Y, dLX, dLY, lMaskMp.Score);
        //                    string msgLW = string.Format("LWX = {0:N3}, LWY = {1:N3}, WScore = {2:N2}",
        //                        lWaferMp.X, lWaferMp.Y, lWaferMp.Score);
        //                    string msgRM = string.Format("RMX = {0:N3}, RMY = {1:N3}, dX = {2:N3}, dY = {3:N3}, MScore = {4:N2}",
        //                        rMaskMp.X, rMaskMp.Y, dRX, dRY, rMaskMp.Score);
        //                    string msgRW = string.Format("RWX = {0:N3}, RWY = {1:N3}, WScore = {2:N2}",
        //                        rWaferMp.X, rWaferMp.Y, rWaferMp.Score);

        //                    lbLeftXYM.Text = msgLM;
        //                    lbRightXYM.Text = msgRM;
        //                    lbLeftXYW.Text = msgLW;
        //                    lbRightXYW.Text = msgRW;

        //                    Update();
        //                });
        //            }
        //            Thread.Sleep(250);
        //        }
        //        Thread.Sleep(1000);
        //    }
        //}
        public void DrawMatchPosition()
        {
            while (!GV.AppEnding)
            {
                while (GV.TabOption == GV.Tab.BackLearn)
                {
                    if (GV.AppSettingParm.Emulation == true || GV.matcherLLM == null)
                    {
                        Thread.Sleep(2000);
                        continue;
                    }

                    Mat leftImageSrc = GV.LeftBackCam.IsFileImage ? LViewMat : GV.LeftBackCam.Grab();
                    Mat rightImageSrc = GV.RightBackCam.IsFileImage ? RViewMat : GV.RightBackCam.Grab();

                    using (Mat leftImage = leftImageSrc?.Clone())
                    using (Mat rightImage = rightImageSrc?.Clone())
                    {
                        int lWaferClassId = _AlignC.LHWaferAIClassId;
                        int rWaferClassId = _AlignC.RHWaferAIClassId;
                        int lMaskClassId = _AlignC.LLMaskAIClassId;
                        int rMaskClassId = _AlignC.RLMaskAIClassId;

                        if (rbLBackMask.Checked || rbLWafer.Checked)
                        {
                            Invoke((MethodInvoker)delegate ()
                            {
                                if (_AlignC.LHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && cbLBackWaferClassList.SelectedIndex >= 0)
                                {
                                    lWaferClassId = cbLBackWaferClassList.SelectedIndex;
                                }
                                if (_AlignC.RHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && cbRBackWaferClassList.SelectedIndex >= 0)
                                {
                                    rWaferClassId = cbRBackWaferClassList.SelectedIndex;
                                }
                                if (_AlignC.LLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && cbLBackMaskClassList.SelectedIndex >= 0)
                                {
                                    lMaskClassId = cbLBackMaskClassList.SelectedIndex;
                                }
                                if (_AlignC.RLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && cbRBackMaskClassList.SelectedIndex >= 0)
                                {
                                    rMaskClassId = cbRBackMaskClassList.SelectedIndex;
                                }
                            });

                            // ✅ 左下光罩搜尋（支援偏移量）
                            if (_AlignC.LLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
                            {
                                GV.matcherLLM.MatMatchWithAlgo(0, LeftMaskMat, ref lMaskMp, _AlignC.LLMaskAlgorithm, lMaskClassId);
                            }
                            else
                            {
                                GV.matcherLLM.MatMatchWithAlgo(0, LeftMaskMat, ref lMaskMp, _AlignC.LLMaskAlgorithm);
                            }

                            // ✅ 套用偏移量校正座標
                            lMaskMp.X += _AlignC.LLMaskOffsetX;
                            lMaskMp.Y += _AlignC.LLMaskOffsetY;

                            // ✅ 右下光罩搜尋（支援偏移量）
                            if (_AlignC.RLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
                            {
                                GV.matcherRLM.MatMatchWithAlgo(0, RightMaskMat, ref rMaskMp, _AlignC.RLMaskAlgorithm, rMaskClassId);
                            }
                            else
                            {
                                GV.matcherRLM.MatMatchWithAlgo(0, RightMaskMat, ref rMaskMp, _AlignC.RLMaskAlgorithm);
                            }

                            // ✅ 套用偏移量校正座標
                            rMaskMp.X += _AlignC.RLMaskOffsetX;
                            rMaskMp.Y += _AlignC.RLMaskOffsetY;

                            // ✅ 左下晶圓搜尋（支援偏移量）
                            if (_AlignC.LHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
                            {
                                GV.matcherLHW.MatMatchWithAlgo(0, leftImage, ref lWaferMp, _AlignC.LHWaferAlgorithm, lWaferClassId);
                            }
                            else
                            {
                                GV.matcherLHW.MatMatchWithAlgo(0, leftImage, ref lWaferMp, _AlignC.LHWaferAlgorithm);
                            }

                            // ✅ 套用偏移量校正座標
                            lWaferMp.X += _AlignC.LHWaferOffsetX;
                            lWaferMp.Y += _AlignC.LHWaferOffsetY;

                            // ✅ 右下晶圓搜尋（支援偏移量）
                            if (_AlignC.RHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
                            {
                                GV.matcherRHW.MatMatchWithAlgo(0, rightImage, ref rWaferMp, _AlignC.RHWaferAlgorithm, rWaferClassId);
                            }
                            else
                            {
                                GV.matcherRHW.MatMatchWithAlgo(0, rightImage, ref rWaferMp, _AlignC.RHWaferAlgorithm);
                            }

                            // ✅ 套用偏移量校正座標
                            rWaferMp.X += _AlignC.RHWaferOffsetX;
                            rWaferMp.Y += _AlignC.RHWaferOffsetY;
                        }

                        BLivePmps.LMaskMp = lMaskMp;
                        BLivePmps.LWaferMp = lWaferMp;
                        BLivePmps.RMaskMp = rMaskMp;
                        BLivePmps.RWaferMp = rWaferMp;

                        Invoke((MethodInvoker)delegate ()
                        {
                            skLeft.MaskMp = lMaskMp;
                            skLeft.WaferMp = lWaferMp;
                            skRight.MaskMp = rMaskMp;
                            skRight.WaferMp = rWaferMp;

                            double dLX = lMaskMp.X - lWaferMp.X;
                            double dLY = lMaskMp.Y - lWaferMp.Y;
                            double dRX = rMaskMp.X - rWaferMp.X;
                            double dRY = rMaskMp.Y - rWaferMp.Y;

                            string msgLM = string.Format("LMX = {0:N3}, LMY = {1:N3}, dX = {2:N3}, dY = {3:N3}, MScore = {4:N2}",
                                lMaskMp.X, lMaskMp.Y, dLX, dLY, lMaskMp.Score);
                            string msgLW = string.Format("LWX = {0:N3}, LWY = {1:N3}, WScore = {2:N2}",
                                lWaferMp.X, lWaferMp.Y, lWaferMp.Score);
                            string msgRM = string.Format("RMX = {0:N3}, RMY = {1:N3}, dX = {2:N3}, dY = {3:N3}, MScore = {4:N2}",
                                rMaskMp.X, rMaskMp.Y, dRX, dRY, rMaskMp.Score);
                            string msgRW = string.Format("RWX = {0:N3}, RWY = {1:N3}, WScore = {2:N2}",
                                rWaferMp.X, rWaferMp.Y, rWaferMp.Score);

                            lbLeftXYM.Text = msgLM;
                            lbRightXYM.Text = msgRM;
                            lbLeftXYW.Text = msgLW;
                            lbRightXYW.Text = msgRW;

                            Update();
                        });
                    }

                    Thread.Sleep(250);
                }

                Thread.Sleep(1000);
            }
        }
        public void UpdateUI()
        {
            if ((GV.NowRecipeNumber < 1) || (GV.NowRecipeNumber > 100))
            {
                GV.NowRecipeNumber = 1;

            }

            EditRecipe = GV.NowRecipeNumber;

            ChangeRecipe();

            lbRecipeNumber.Text = GV.Dlang.strRecipeNumber + EditRecipe.ToString();
            //GV.HwndFormMain.writeStatus(lbRecipeNumber.Text + "  " + GV.NowRecipeNumber + "  " + EditRecipe.ToString());
            pbLTopMask.Image = null;
            pbLBackMask.Image = null;
            pbLWafer.Image = null;
            pbRTopMask.Image = null;
            pbRBackMask.Image = null;
            pbRWafer.Image = null;

            //    if (rBLowMagnification.Checked)
            //    {
            //        if (_recipe.LeftLowMaskMat != null && !_recipe.LeftLowMaskMat.Empty() &&
            //    _recipe.LeftLowMaskMat.Width > 0 && _recipe.LeftLowMaskMat.Height > 0)
            //        {
            //            try
            //            {
            //                pbLBackMask.Image = _recipe.LeftLowMaskMat.ToBitmap();
            //            }
            //            catch 
            //            {

            //            }
            //        }

            //        if (_recipe.RightLowMaskMat != null && !_recipe.RightLowMaskMat.Empty() &&
            //            _recipe.RightLowMaskMat.Width > 0 && _recipe.RightLowMaskMat.Height > 0)
            //        {
            //            try
            //            {
            //                pbRBackMask.Image = _recipe.RightLowMaskMat.ToBitmap();
            //            }
            //            catch 
            //            {

            //            }
            //        }
            //        cbLTopMaskAlgo.SelectedIndex = (int)_AlignC.LLMaskAlgorithm;
            //        cbRTopMaskAlgo.SelectedIndex = (int)_AlignC.RLMaskAlgorithm;
            //    }
            //    else
            //    {
            //        if (_recipe.LeftHighMaskMat != null && !_recipe.LeftHighMaskMat.Empty() &&
            //    _recipe.LeftHighMaskMat.Width > 0 && _recipe.LeftHighMaskMat.Height > 0)
            //        {
            //            try
            //            {
            //                pbLTopMask.Image = _recipe.LeftHighMaskMat.ToBitmap();
            //            }
            //            catch 
            //            {
            //            }
            //        }

            //        if (_recipe.RightHighMaskMat != null && !_recipe.RightHighMaskMat.Empty() &&
            //            _recipe.RightHighMaskMat.Width > 0 && _recipe.RightHighMaskMat.Height > 0)
            //        {
            //            try
            //            {
            //                pbRTopMask.Image = _recipe.RightHighMaskMat.ToBitmap();
            //            }
            //            catch 
            //            {
            //            }
            //        }

            //        cbLTopMaskAlgo.SelectedIndex = (int)_AlignC.LHMaskAlgorithm;
            //        cbRTopMaskAlgo.SelectedIndex = (int)_AlignC.RHMaskAlgorithm;
            //    }

            //    if (_recipe.LeftLowWaferMat != null && !_recipe.LeftLowWaferMat.Empty() &&
            //_recipe.LeftLowWaferMat.Width > 0 && _recipe.LeftLowWaferMat.Height > 0)
            //    {
            //        try
            //        {
            //            pbLBackMask.Image = _recipe.LeftLowMaskMat.ToBitmap();
            //        }
            //        catch 
            //        { 
            //        }
            //        //using(var bitmap=_recipe.LeftLowMaskMat.ToBitmap())
            //        //{
            //        //    pbLBackMask.Image?.Dispose();
            //        //    pbLBackMask.Image =(Bitmap)bitmap.Clone();
            //        //}
            //    }

            //    if (_recipe.LeftHighWaferMat != null && !_recipe.LeftHighWaferMat.Empty() &&
            //        _recipe.LeftHighWaferMat.Width > 0 && _recipe.LeftHighWaferMat.Height > 0)
            //    {
            //        try
            //        {
            //            pbLWafer.Image = _recipe.LeftHighWaferMat.ToBitmap();
            //        }
            //        catch 
            //        {
            //        }
            //        //using (var bitmap = _recipe.LeftHighWaferMat.ToBitmap())
            //        //{
            //        //    pbLWafer.Image?.Dispose();
            //        //    pbLWafer.Image = (Bitmap)bitmap.Clone();
            //        //}
            //    }

            //    if (_recipe.RightLowWaferMat != null && !_recipe.RightLowWaferMat.Empty() &&
            //        _recipe.RightLowWaferMat.Width > 0 && _recipe.RightLowWaferMat.Height > 0)
            //    {
            //        try
            //        {
            //            pbRBackMask.Image = _recipe.RightLowMaskMat.ToBitmap();
            //        }
            //        catch 
            //        {
            //        }
            //        //using (var bitmap = _recipe.RightLowMaskMat.ToBitmap())
            //        //{
            //        //    pbRBackMask.Image?.Dispose();
            //        //    pbRBackMask.Image = (Bitmap)bitmap.Clone();
            //        //}
            //    }

            //    if (_recipe.RightHighWaferMat != null && !_recipe.RightHighWaferMat.Empty() &&
            //        _recipe.RightHighWaferMat.Width > 0 && _recipe.RightHighWaferMat.Height > 0)
            //    {
            //        try
            //        {
            //            pbRWafer.Image = _recipe.RightHighWaferMat.ToBitmap();
            //        }
            //        catch 
            //        {
            //        }
            //        //using (var bitmap = _recipe.RightHighWaferMat.ToBitmap())
            //        //{
            //        //    pbRWafer.Image?.Dispose();
            //        //    pbRWafer.Image = (Bitmap)bitmap.Clone();
            //        //}
            //    }
            if (rBLowMagnification.Checked)
            {
                // ✅ 左下光罩 - 顯示帶綠點的預覽影像
                if (_recipe.LeftLowMaskMat != null && !_recipe.LeftLowMaskMat.Empty() &&
                    _recipe.LeftLowMaskMat.Width > 0 && _recipe.LeftLowMaskMat.Height > 0)
                {
                    try
                    {
                        pbLBackMask.Image?.Dispose();

                        // ✅ 檢查是否為多十字模式（偏移量不為 0）
                        if (_AlignC.LLMaskOffsetX != 0 || _AlignC.LLMaskOffsetY != 0)
                        {
                            // ✅ 多十字模式：產生帶綠點的預覽影像
                            pbLBackMask.Image = CreatePreviewWithGreenDot(
                                _recipe.LeftLowMaskMat,
                                _AlignC.LLMaskOffsetX,
                                _AlignC.LLMaskOffsetY
                            );
                        }
                        else
                        {
                            // ✅ 單十字模式：顯示原始灰階影像
                            pbLBackMask.Image = _recipe.LeftLowMaskMat.ToBitmap();
                        }
                    }
                    catch
                    {
                    }
                }

                // ✅ 右下光罩 - 顯示帶綠點的預覽影像
                if (_recipe.RightLowMaskMat != null && !_recipe.RightLowMaskMat.Empty() &&
                    _recipe.RightLowMaskMat.Width > 0 && _recipe.RightLowMaskMat.Height > 0)
                {
                    try
                    {
                        pbRBackMask.Image?.Dispose();

                        if (_AlignC.RLMaskOffsetX != 0 || _AlignC.RLMaskOffsetY != 0)
                        {
                            pbRBackMask.Image = CreatePreviewWithGreenDot(
                                _recipe.RightLowMaskMat,
                                _AlignC.RLMaskOffsetX,
                                _AlignC.RLMaskOffsetY
                            );
                        }
                        else
                        {
                            pbRBackMask.Image = _recipe.RightLowMaskMat.ToBitmap();
                        }
                    }
                    catch
                    {
                    }
                }
                cbLTopMaskAlgo.SelectedIndex = (int)_AlignC.LLMaskAlgorithm;
                cbRTopMaskAlgo.SelectedIndex = (int)_AlignC.RLMaskAlgorithm;
            }
            else
            {
                if (_recipe.LeftHighMaskMat != null && !_recipe.LeftHighMaskMat.Empty() &&
                    _recipe.LeftHighMaskMat.Width > 0 && _recipe.LeftHighMaskMat.Height > 0)
                {
                    try
                    {
                        pbLTopMask.Image = _recipe.LeftHighMaskMat.ToBitmap();
                    }
                    catch
                    {
                    }
                }

                if (_recipe.RightHighMaskMat != null && !_recipe.RightHighMaskMat.Empty() &&
                    _recipe.RightHighMaskMat.Width > 0 && _recipe.RightHighMaskMat.Height > 0)
                {
                    try
                    {
                        pbRTopMask.Image = _recipe.RightHighMaskMat.ToBitmap();
                    }
                    catch
                    {
                    }
                }

                cbLTopMaskAlgo.SelectedIndex = (int)_AlignC.LHMaskAlgorithm;
                cbRTopMaskAlgo.SelectedIndex = (int)_AlignC.RHMaskAlgorithm;
            }

            // ✅ 左下晶圓 - 顯示帶綠點的預覽影像
            if (_recipe.LeftHighWaferMat != null && !_recipe.LeftHighWaferMat.Empty() &&
                _recipe.LeftHighWaferMat.Width > 0 && _recipe.LeftHighWaferMat.Height > 0)
            {
                try
                {
                    pbLWafer.Image?.Dispose();

                    if (_AlignC.LHWaferOffsetX != 0 || _AlignC.LHWaferOffsetY != 0)
                    {
                        pbLWafer.Image = CreatePreviewWithGreenDot(
                            _recipe.LeftHighWaferMat,
                            _AlignC.LHWaferOffsetX,
                            _AlignC.LHWaferOffsetY
                        );
                    }
                    else
                    {
                        pbLWafer.Image = _recipe.LeftHighWaferMat.ToBitmap();
                    }
                }
                catch
                {
                }
            }

            // ✅ 右下晶圓 - 顯示帶綠點的預覽影像
            if (_recipe.RightHighWaferMat != null && !_recipe.RightHighWaferMat.Empty() &&
                _recipe.RightHighWaferMat.Width > 0 && _recipe.RightHighWaferMat.Height > 0)
            {
                try
                {
                    pbRWafer.Image?.Dispose();

                    if (_AlignC.RHWaferOffsetX != 0 || _AlignC.RHWaferOffsetY != 0)
                    {
                        pbRWafer.Image = CreatePreviewWithGreenDot(
                            _recipe.RightHighWaferMat,
                            _AlignC.RHWaferOffsetX,
                            _AlignC.RHWaferOffsetY
                        );
                    }
                    else
                    {
                        pbRWafer.Image = _recipe.RightHighWaferMat.ToBitmap();
                    }
                }
                catch
                {
                }
            }

            cbLBackMaskAlgo.SelectedIndex = (int)_AlignC.LLMaskAlgorithm;
            cbLWaferAlgo.SelectedIndex = (int)_AlignC.LHWaferAlgorithm;
            cbRBackMaskAlgo.SelectedIndex = (int)_AlignC.RLMaskAlgorithm;
            cbRWaferAlgo.SelectedIndex = (int)_AlignC.RHWaferAlgorithm;
            if (_AlignC.LLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && _AlignC.LLMaskAIClassId >= 0)
            {
                SetComboBoxByClassId(cbLBackMaskClassList, _AlignC.LLMaskAIClassId);
            }
            else
            {
                cbLBackMaskClassList.SelectedIndex = -1;
            }
            if (_AlignC.RLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && _AlignC.RLMaskAIClassId >= 0)
            {
                SetComboBoxByClassId(cbRBackMaskClassList, _AlignC.RLMaskAIClassId);
            }
            else
            {
                cbRBackMaskClassList.SelectedIndex = -1;
            }
            if (_AlignC.LHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && _AlignC.LHWaferAIClassId >= 0)
            {
                SetComboBoxByClassId(cbLBackWaferClassList, _AlignC.LHWaferAIClassId);
            }
            else
            {
                cbLBackWaferClassList.SelectedIndex = -1;
            }
            if (_AlignC.RHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && _AlignC.RHWaferAIClassId >= 0)
            {
                SetComboBoxByClassId(cbRBackWaferClassList, _AlignC.RHWaferAIClassId);
            }
            else
            {
                cbRBackWaferClassList.SelectedIndex = -1;
            }
                int iL = (int)(_AlignC.dLalpha * 100);
            if (iL > 100) iL = 100;
            if (iL < 0) iL = 0;
            int iR = (int)(_AlignC.dRalpha * 100);
            if (iR > 100) iR = 100;
            if (iR < 0) iR = 0;

            iLalpha = iL;
            iRalpha = iR;

            tBLShowImage.Value = iLalpha;
            tBRShowImage.Value = iRalpha;

            _dLalpha = (double)tBLShowImage.Value / 100;
            _dRalpha = (double)tBRShowImage.Value / 100;

            if (rbLTopMask.Checked == false)
            {
                skLeft.SetShowMask(true, _dLalpha, LeftMaskMat);
                skRight.SetShowMask(true, _dRalpha, RightMaskMat);
            }
            else
            {
                skLeft.SetShowMask(false, 0, LeftMaskMat);
                skRight.SetShowMask(false, 0, RightMaskMat);
            }

            if ((rbLTopMask.Enabled) && (rbLTopMask.Checked) && (GV.UserLevel != GV.User.Operator))
            {
                btLTopMaskLightPlus.Enabled = true;
                btLTopMaskLightMinus.Enabled = true;
                tBLeftTopMaskLight.Enabled = true;
                btLTopMaskLSave.Enabled = true;
            }
            else
            {
                btLTopMaskLightPlus.Enabled = false;
                btLTopMaskLightMinus.Enabled = false;
                tBLeftTopMaskLight.Enabled = false;
                btLTopMaskLSave.Enabled = false;
            }

            if ((rbLBackMask.Enabled) && (rbLBackMask.Checked) && (GV.UserLevel != GV.User.Operator))
            {
                btLBackMaskLightPlus.Enabled = true;
                btLBackMaskLightMinus.Enabled = true;
                tBLeftBackMaskLight.Enabled = true;
                btLBackMaskLSave.Enabled = true;
            }
            else
            {
                btLBackMaskLightPlus.Enabled = false;
                btLBackMaskLightMinus.Enabled = false;
                tBLeftBackMaskLight.Enabled = false;
                btLBackMaskLSave.Enabled = false;
            }

            if ((rbLWafer.Enabled) && (rbLWafer.Checked) && (GV.UserLevel != GV.User.Operator))
            {
                btLWaferLightPlus.Enabled = true;
                btLWaferLightMinus.Enabled = true;
                tBLeftBackWaferLight.Enabled = true;
                btLWaferLSave.Enabled = true;
            }
            else
            {
                btLWaferLightPlus.Enabled = false;
                btLWaferLightMinus.Enabled = false;
                tBLeftBackWaferLight.Enabled = false;
                btLWaferLSave.Enabled = false;
            }

            if ((rbRTopMask.Enabled) && (rbRTopMask.Checked) && (GV.UserLevel != GV.User.Operator))
            {
                btRTopMaskLightPlus.Enabled = true;
                btRTopMaskLightMinus.Enabled = true;
                tBRightTopMaskLight.Enabled = true;
                btRTopMaskLSave.Enabled = true;
            }
            else
            {
                btRTopMaskLightPlus.Enabled = false;
                btRTopMaskLightMinus.Enabled = false;
                tBRightTopMaskLight.Enabled = false;
                btRTopMaskLSave.Enabled = false;
            }

            if ((rbRBackMask.Enabled) && (rbRBackMask.Checked) && (GV.UserLevel != GV.User.Operator))
            {
                btRBackMaskLightPlus.Enabled = true;
                btRBackMaskLightMinus.Enabled = true;
                tBRightBackMaskLight.Enabled = true;
                btRBackMaskLSave.Enabled = true;
            }
            else
            {
                btRBackMaskLightPlus.Enabled = false;
                btRBackMaskLightMinus.Enabled = false;
                tBRightBackMaskLight.Enabled = false;
                btRBackMaskLSave.Enabled = false;
            }

            if ((rbRWafer.Enabled) && (rbRWafer.Checked) && (GV.UserLevel != GV.User.Operator))
            {
                btRWaferLightPlus.Enabled = true;
                btRWaferLightMinus.Enabled = true;
                tBRightBackWaferLight.Enabled = true;
                btRWaferLSave.Enabled = true;
            }
            else
            {
                btRWaferLightPlus.Enabled = false;
                btRWaferLightMinus.Enabled = false;
                tBRightBackWaferLight.Enabled = false;
                btRWaferLSave.Enabled = false;
            }

            if (rbLTopMask.Checked)
            {
                GV.Light.ChangeBrightness("left", tBLeftTopMaskLight.Value);
            }
            else
            {
                GV.Light.ChangeBrightness("left", 0);
            }

            if (rbLBackMask.Checked)
            {
                GV.Light.ChangeBrightness("leftback", tBLeftBackMaskLight.Value);
            }
            else if (rbLWafer.Checked)
            {
                GV.Light.ChangeBrightness("leftback", tBLeftBackWaferLight.Value);
            }
            else
            {
                GV.Light.ChangeBrightness("leftback", 0);
            }

            if (rbRTopMask.Checked)
            {
                GV.Light.ChangeBrightness("right", tBRightTopMaskLight.Value);
            }
            else
            {
                GV.Light.ChangeBrightness("right", 0);
            }

            if (rbRBackMask.Checked)
            {
                GV.Light.ChangeBrightness("rightback", tBRightBackMaskLight.Value);
            }
            else if (rbRWafer.Checked)
            {
                GV.Light.ChangeBrightness("rightback", tBRightBackWaferLight.Value);
            }
            else
            {
                GV.Light.ChangeBrightness("rightback", 0);
            }
            Update();
        }
        private Bitmap CreatePreviewWithGreenDot(Mat sourceImage, int offsetX, int offsetY)
        {
            if (sourceImage == null || sourceImage.Empty())
            {
                return null;
            }

            try
            {
                // ✅ 複製原始影像並轉換為彩色
                Mat previewMat = sourceImage.Clone();
                Cv2.CvtColor(previewMat, previewMat, ColorConversionCodes.GRAY2BGR);

                // ✅ 計算中心點位置（offsetX 和 offsetY 就是中心點座標）
                int centerX = offsetX;
                int centerY = offsetY;

                // ✅ 繪製綠色圓點
                Cv2.Circle(previewMat, new OpenCvSharp.Point(centerX, centerY), 5, new Scalar(0, 255, 0), -1);

                // ✅ 繪製綠色十字線
                Cv2.Line(previewMat,
                         new OpenCvSharp.Point(centerX - 20, centerY),
                         new OpenCvSharp.Point(centerX + 20, centerY),
                         new Scalar(0, 255, 0), 2);
                Cv2.Line(previewMat,
                         new OpenCvSharp.Point(centerX, centerY - 20),
                         new OpenCvSharp.Point(centerX, centerY + 20),
                         new Scalar(0, 255, 0), 2);

                Bitmap result = previewMat.ToBitmap();
                previewMat.Dispose();

                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"CreatePreviewWithGreenDot 錯誤: {ex.Message}");
                return sourceImage.ToBitmap(); // 發生錯誤時回傳原始影像
            }
        }
        private void ChangeRecipe()
        {
            _recipe = GV._recipe;
            _AlignC = _recipe.AlignC;
            UpdateTargetPositionDisplay();
        }
        private void ApplyLanguage()
        {
            if (GV.AppSettingParm.Language == "default" || GV.Dlang == null)
                return;

            // === WEC Location (groupBox2) ===
            groupBoxB2.Text = GV.Dlang.gbWECLocation;
            label22.Text = GV.Dlang.lbTargetPosition;   // "Setting" -> 目標位置
            label23.Text = GV.Dlang.lbCurrentPosition;    // "Now Located" -> 現在位置
            label24.Text = GV.Dlang.lbChuckZ;            // "WEC Z"
            label25.Text = GV.Dlang.lbChuckX;            // "XYY-X"
            label20.Text = GV.Dlang.lbChuckY1;           // "XYY-Y1"
            label21.Text = GV.Dlang.lbChuckY2;           // "XYY-Y2"
            btBackChuckUpdate.Caption = GV.Dlang.btChuckUpdate;  // UCButton 使用 Caption
            btChuckGo.Caption = GV.Dlang.btChuckGo;              // UCButton 使用 Caption

            // === CCD Location (groupBox1) ===
            groupBoxB1.Text = GV.Dlang.gbCCDLocation;
            label10.Text = GV.Dlang.lbTargetPosition;   // "Setting" -> 目標位置
            label11.Text = GV.Dlang.lbCurrentPosition;    // "Now Located" -> 現在位置
                                                         // label12.Text = GV.Dlang.lbBigY;           // "Big Y" (目前隱藏)
            label13.Text = GV.Dlang.lbLeftX;             // "Left X"
            label8.Text = GV.Dlang.lbLeftY;              // "Left Y"
            label9.Text = GV.Dlang.lbLeftZ;              // "Left Z"
            label14.Text = GV.Dlang.lbRightX;            // "Right X"
            label18.Text = GV.Dlang.lbRightY;            // "Right Y"
            label19.Text = GV.Dlang.lbRightZ;            // "Right Z"
            btBackCCDUpdate.Caption = GV.Dlang.btCCDUpdate;      // UCButton 使用 Caption
            btUpCCDGo.Caption = GV.Dlang.btCCDGo;                // UCButton 使用 Caption
        }

        public void CheckLevel()
        {
            if (LeftWaferMp == null)
            {
                LeftWaferMp = new MatchPosition();
                LeftMaskMp = new MatchPosition();
                RightWaferMp = new MatchPosition();
                RightMaskMp = new MatchPosition();
            }

            int nLevel = (int)GV.UserLevel;
            if (NowLevel != nLevel)
            {
                NowLevel = nLevel;
                Invoke((MethodInvoker)delegate ()
                {
                    GV.RingLight.ChangeBrightness("left", 0);
                    GV.RingLight.ChangeBrightness("right", 0);
                    // tBLShowImage.Enabled = true;
                    // tBRShowImage.Enabled = true;

                    if (GV.UserLevel == GV.User.Operator)
                    {
                        // rbLTopMask.Enabled = true;
                        // rbRTopMask.Enabled = true;
                        // rbLBackMask.Enabled = true;
                        // rbRBackMask.Enabled = true;
                        // rbLWafer.Enabled = true;
                        btSave.Enabled = false;
                        cbLowMagnification.Enabled = false;
                        cbHighMagnification.Enabled = false;
                        cbLBackMaskAlgo.Enabled = false;
                        cbRBackMaskAlgo.Enabled = false;
                        cbLWaferAlgo.Enabled = false;
                        cbRWaferAlgo.Enabled = false;
                        btMagSave.Enabled = false;
                        btLShowImageSave.Enabled = false;
                        btRShowImageSave.Enabled = false;
                        btLTopMaskLSave.Enabled = false;
                        btLBackMaskLSave.Enabled = false;
                        btLWaferLSave.Enabled = false;
                        btRTopMaskLSave.Enabled = false;
                        btRBackMaskLSave.Enabled = false;
                        btRWaferLSave.Enabled = false;
                        btDelete.Enabled = false;
                        btDefault.Enabled = false;
                        groupBox4.Visible = false;
                        btLWaferMask.Enabled = false;
                        btRWaferMask.Enabled = false;
                        btLBackMask.Enabled = false;
                        btRBackMask.Enabled = false;
                        btFindLBMaskCenter.Visible = false;
                        btFindRBMaskCenter.Visible = false;
                        btFindLWaferCenter.Visible = false;
                        btFindRWaferCenter.Visible = false;
                    }
                    else
                    {
                        gbLWafer.Enabled = true;
                        gbLBackMask.Enabled = true;
                        gbRWafer.Enabled = true;
                        gbRBackMask.Enabled = true;
                        gbLTopMask.Enabled = true;
                        gbRTopMask.Enabled = true;
                        cbLowMagnification.Enabled = true;
                        cbHighMagnification.Enabled = true;
                        btMagSave.Enabled = true;
                        btLShowImageSave.Enabled = true;
                        btRShowImageSave.Enabled = true;
                        btDelete.Enabled = true;
                        btDefault.Enabled = true;
                        groupBox4.Visible = false;
                        btLWaferMask.Enabled = true;
                        btRWaferMask.Enabled = true;
                        btLBackMask.Enabled = true;
                        btRBackMask.Enabled = true;
                        btSave.Enabled = true;
                        btFindLBMaskCenter.Visible = true;
                        btFindLWaferCenter.Visible = true;
                        btFindRBMaskCenter.Visible = true;
                        btFindRWaferCenter.Visible = true;
                    }

                    if (GV.UserLevel == GV.User.Administrator)
                    {
                        btFindLMaskCenter.Visible = true;
                        btFindLWaferCenter.Visible = true;
                        btFindRMaskCenter.Visible = true;
                        lbLeftXYM.Visible = true;
                        lbRightXYM.Visible = true;
                        btReportLocation.Visible = true;
                        btLMaskSave.Visible = true;
                        btRMaskSave.Visible = true;
                        bBTtMaskAlign.Visible = true;
                        btBTAlignWafer.Visible = true;
                        btBTAlignWaferRotate.Visible = true;
                        btSave.Enabled = true;
                        btReadMaskMat.Visible = true;
                        btNG.Visible = true;
                        NUDLMaskX.Visible = false;
                        NUDLMaskY.Visible = false;
                        NUDRMaskX.Visible = false;
                        NUDRMaskY.Visible = false;
                        BtLMaskAdjSave.Visible = false;
                        BtRMaskAdjSave.Visible = false;
                        lblLMaskX.Visible = false;
                        lblLMaskY.Visible = false;
                        lblRMaskX.Visible = false;
                        lblRMaskY.Visible = false;
                    }
                    else
                    {
                        btFindLMaskCenter.Visible = false;
                        btFindRMaskCenter.Visible = false;
                        // lbLeftXYM.Visible = false;
                        // lbRightXYM.Visible = false;
                        btReportLocation.Visible = false;
                        btLMaskSave.Visible = false;
                        btRMaskSave.Visible = false;
                        bBTtMaskAlign.Visible = false;
                        btBTAlignWafer.Visible = false;
                        btBTAlignWaferRotate.Visible = false;
                        // groupBox4.Visible = false;
                        btReadMaskMat.Visible = false;
                        btNG.Visible = false;
                        NUDLMaskX.Visible = false;
                        NUDLMaskY.Visible = false;
                        NUDRMaskX.Visible = false;
                        NUDRMaskY.Visible = false;
                        BtLMaskAdjSave.Visible = false;
                        BtRMaskAdjSave.Visible = false;
                        lblLMaskX.Visible = false;
                        lblLMaskY.Visible = false;
                        lblRMaskX.Visible = false;
                        lblRMaskY.Visible = false;
                    }
                });

                rBLowMagnification.Enabled = true;
                rBHighMagnification.Enabled = true;
            }
            UpdateUI();
        }

        private string GetTemplateFileName(string patternAndSide)
        {
            string fileName = "Templates//" + patternAndSide + EditRecipe.ToString() + ".bmp";
            return fileName;
        }

        private string GetTemplateFileNameD(string patternAndSide)
        {
            string fileName = "Templates//D" + patternAndSide + EditRecipe.ToString() + ".bmp";
            return fileName;
        }
        private OpenCV3MatchUMat.AlignAlgorithm Algorithm(int a)
        {
            if (a == 0)
                return OpenCV3MatchUMat.AlignAlgorithm.TemplateMatch;
            else if (a == 1)
                return OpenCV3MatchUMat.AlignAlgorithm.EdgeMatch;
            else
                return OpenCV3MatchUMat.AlignAlgorithm.AIMatch;

        }

        private void BtLWaferSave_Click(object sender, EventArgs e)
        {
            GV.TickCount = 0;
            Mat m;
            Mat fullImage;
            fullImage = GV.LeftBackCam.Grab();

            if (fullImage == null || fullImage.Empty())
            {
                MessageBox.Show("Capture image fail!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            OpenCvSharp.Rect roi = skLeft.GetWaferRect();
            if (roi.Width <= 0 || roi.Height <= 0 ||
                roi.X < 0 || roi.Y < 0 ||
                roi.X + roi.Width > fullImage.Width ||
                roi.Y + roi.Height > fullImage.Height)
            {
                MessageBox.Show("晶圓區域無效或超出影像範圍", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                fullImage?.Dispose();
                return;
            }
            m = new Mat(fullImage, roi).Clone();
            if (cbLBackMaskAlgo.SelectedIndex == 1)
            {
                Cv2.GaussianBlur(m, m, new OpenCvSharp.Size(7, 7), 0, 0, BorderTypes.Default);
                Cv2.AdaptiveThreshold(m, m, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.Binary, 27, 2);
                Mat invertedImg = new Mat();
                Cv2.BitwiseNot(m, invertedImg);
                m = invertedImg.Clone();
                invertedImg.Dispose();
            }
            pbLWafer.Image = m.ToBitmap();

            if (MessageBox.Show(GV.Dlang.strLWafer, GV.Dlang.strLWaferSave, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                pbLWafer.Image = _recipe.LeftHighWaferMat.ToBitmap();
                if (cbLWaferAlgo.SelectedIndex == 2)
                {
                    cbLBackWaferClassList.Visible = true;
                    btBackLabelPatternL.Visible = true;
                    pbLWafer.Visible = false;
                }
                return;
            }

            _AlignC.LBWaferBright = tBLeftBackWaferLight.Value;
            _AlignC.LHWaferOffsetX = 0;
            _AlignC.LHWaferOffsetY = 0;
            _recipe.SetLeftHighWaferMat(m);
            GrayImage d = new GrayImage(m.Width, m.Height);
            d.Fill(255);
            _recipe.LeftHighWaferMask = new GrayImage(d);
            _AlignC.LHWaferAlgorithm = Algorithm(cbLWaferAlgo.SelectedIndex);
            if (cbLWaferAlgo.SelectedIndex == 2)
            {
                if (cbLBackWaferClassList.SelectedIndex >= 0 && cbLBackWaferClassList.SelectedItem != null)
                {
                    string selectedClassName = cbLBackWaferClassList.SelectedItem.ToString();
                    Dictionary<string, Int16> ClassList = GV.AIClassList.GetClassList();
                    if (ClassList.ContainsKey(selectedClassName))
                    {
                        _AlignC.LHWaferAIClassId = ClassList[selectedClassName];
                        _AlignC.LHBitwiseNot = ClassList[selectedClassName];
                    }
                    else
                    {
                        MessageBox.Show($"找不到類別 '{selectedClassName}'，請重新選擇", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    // 未選擇類別時，使用 -1 表示追蹤所有特徵
                    _AlignC.LHWaferAIClassId = -1;
                    _AlignC.LHBitwiseNot = -1;
                }
            }
            else
            {
                _AlignC.LHWaferAIClassId = -1;
                _AlignC.LHBitwiseNot = 0;
            }
            if (GV.AppSettingParm.Emulation != true && GV.matcherLHW != null)
            {
                GV.matcherLHW.LearnWithAlgo(m, d, _AlignC.LHWaferAlgorithm);
            }

            _AlignC.LastModifyTime = DateTime.Now;
            GM.WriteRecipeXml(EditRecipe);
            //if (cbLWaferAlgo.SelectedIndex == 2)
            //{
            //    try
            //    {
            //        string trainPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Python", "Train", GV._recipe.RecipeName);
            //        DateTime now = DateTime.Now;
            //        Directory.CreateDirectory(trainPath);
            //        string fn = string.Concat(trainPath, "\\LWB", now.ToString("HH_mm_ss"));
            //        Cv2.ImWrite(string.Concat(fn, ".bmp"), fullImage);
            //        var labelPath = string.Concat(fn, ".txt");

            //        float xCenter = (roi.X + roi.Width / 2f) / fullImage.Width;
            //        float yCenter = (roi.Y + roi.Height / 2f) / fullImage.Height;
            //        float w = roi.Width / (float)fullImage.Width;
            //        float h = roi.Height / (float)fullImage.Height;
            //        int classId = cbLBackWaferClassList.SelectedIndex;
            //        List<YoloBox> boxes = new List<YoloBox>();
            //        boxes.Add(new YoloBox { ClassId = classId, X = xCenter, Y = yCenter, W = w, H = h });
            //        System.IO.File.WriteAllLines(labelPath, boxes.Select(b => $"{b.ClassId} {b.X:F6} {b.Y:F6} {b.W:F6} {b.H:F6}"));
            //    }
            //    catch
            //    {

            //    }
            //}
        }

        private void BtLMaskTemplateSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Sure to save left back mask template ?", "LMaskTemplateSave", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            Mat m = new Mat(GV.LeftBackCam.Grab(), skLeft.GetMaskRect());

            _recipe.LeftLowMaskMat = m;
            pbLBackMask.Image = m.ToBitmap();
            Cv2.ImWrite(GetTemplateFileName("LLM"), m);
            GrayImage d = new GrayImage(m.Width, m.Height);
            d.Fill(255);
            _recipe.LeftLowMaskMask = d;
            d.Save(GetTemplateFileName("MLLM"), System.Drawing.Imaging.ImageFormat.Bmp);
            _AlignC.LLMaskAlgorithm = Algorithm(cbLTopMaskAlgo.SelectedIndex);

            GM.WriteAlignConditionsAcarToXml("AlignConditions.xml");
        }

        private void BtRWaferTempalteSave_Click(object sender, EventArgs e)
        {
            GV.TickCount = 0;
            Mat m;
            Mat fullImage;
            fullImage = GV.RightBackCam.Grab();

            if (fullImage == null || fullImage.Empty())
            {
                MessageBox.Show("Capture image fail!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            OpenCvSharp.Rect roi = skRight.GetWaferRect();
            if (roi.Width <= 0 || roi.Height <= 0 ||
                roi.X < 0 || roi.Y < 0 ||
                roi.X + roi.Width > fullImage.Width ||
                roi.Y + roi.Height > fullImage.Height)
            {
                MessageBox.Show("晶圓區域無效或超出影像範圍", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                fullImage?.Dispose();
                return;
            }
            m = new Mat(fullImage, roi);


            pbRWafer.Image = m.ToBitmap();

            if (MessageBox.Show(GV.Dlang.strRWafer, GV.Dlang.strRWaferSave, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                pbRWafer.Image = _recipe.RightHighWaferMat.ToBitmap();
                if (cbRWaferAlgo.SelectedIndex == 2)
                {
                    cbRBackWaferClassList.Visible = true;
                    btBackLabelPatternR.Visible = false;
                    pbRWafer.Visible = false;
                }
                return;
            }

            _AlignC.RBWaferBright = tBRightBackWaferLight.Value;
            _AlignC.RHWaferOffsetX = 0;
            _AlignC.RHWaferOffsetY = 0;
            _recipe.SetRightHighWaferMat(m);
            GrayImage d = new GrayImage(m.Width, m.Height);
            d.Fill(255);
            _recipe.RightHighWaferMask = d;
            _AlignC.RHWaferAlgorithm = Algorithm(cbRWaferAlgo.SelectedIndex);
            if (cbRBackMaskAlgo.SelectedIndex == 2)
            {
                if(cbRBackWaferClassList.SelectedIndex>=0 && cbRBackWaferClassList.SelectedItem != null)
                {
                    string selectedClassName = cbRBackWaferClassList.SelectedItem.ToString();
                    Dictionary<string, Int16> ClassList = GV.AIClassList.GetClassList();
                    if (ClassList.ContainsKey(selectedClassName))
                    {
                        _AlignC.RHWaferAIClassId = ClassList[selectedClassName];
                        _AlignC.RHBitwiseNot = ClassList[selectedClassName];
                    }
                    else
                    {
                        MessageBox.Show($"找不到類別 '{selectedClassName}'，請重新選擇", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    _AlignC.RHWaferAIClassId = -1;
                    _AlignC.RHBitwiseNot = -1;
                }
            }
            else
            {
                _AlignC.RHWaferAIClassId = -1;
                _AlignC.RHBitwiseNot = -1;
            }
            if(GV.AppSettingParm.Emulation != true && GV.matcherRHW != null)
            {
                GV.matcherRHW.LearnWithAlgo(m, d, _AlignC.RHWaferAlgorithm);
            }                
            _AlignC.LastModifyTime = DateTime.Now;
            GM.WriteRecipeXml(EditRecipe);
            //if (cbRWaferAlgo.SelectedIndex == 2)
            //{
            //    try
            //    {
            //        string trainPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Python", "Train", GV._recipe.RecipeName);
            //        DateTime now = DateTime.Now;
            //        Directory.CreateDirectory(trainPath);
            //        string fn = string.Concat(trainPath, "\\RBW", now.ToString("HH_mm_ss"));
            //        Cv2.ImWrite(string.Concat(fn, ".bmp"), fullImage);
            //        var labelPath = string.Concat(fn, ".txt");
            //        float xCenter = (roi.X + roi.Width / 2f) / fullImage.Width;
            //        float yCenter = (roi.Y + roi.Height / 2f) / fullImage.Height;
            //        float w = roi.Width / (float)fullImage.Width;
            //        float h = roi.Height / (float)fullImage.Height;
            //        int classId = cbRBackWaferClassList.SelectedIndex;
            //        List<YoloBox> boxes = new List<YoloBox>();
            //        boxes.Add(new YoloBox { ClassId = classId, X = xCenter, Y = yCenter, W = w, H = h });
            //        System.IO.File.WriteAllLines(labelPath, boxes.Select(b => $"{b.ClassId} {b.X:F6} {b.Y:F6} {b.W:F6} {b.H:F6}"));
            //    }
            //    catch
            //    {
            //    }
                
            //}
        }

        private void btRMaskTemplateSave_Click(object sender, EventArgs e)
        {
            GV.TickCount = 0;
            Mat m;
            Mat fullImage;
            fullImage = GV.RightBackCam.Grab();

            if (fullImage == null || fullImage.Empty())
            {
                MessageBox.Show("Capture image fail!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            OpenCvSharp.Rect roi = skRight.GetMaskRect();
            if (roi.Width <= 0 || roi.Height <= 0 ||
                roi.X < 0 || roi.Y < 0 ||
                roi.X + roi.Width > fullImage.Width ||
                roi.Y + roi.Height > fullImage.Height)
            {
                MessageBox.Show("晶圓區域無效或超出影像範圍", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                fullImage?.Dispose();
                return;
            }
            m = new Mat(fullImage, roi).Clone();
            if (cbRBackMaskAlgo.SelectedIndex == 1)
            {
                Cv2.GaussianBlur(m, m, new OpenCvSharp.Size(7, 7), 0, 0, BorderTypes.Default);
                Cv2.AdaptiveThreshold(m, m, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.Binary, 27, 2);
                Mat invertedImg = new Mat();
                Cv2.BitwiseNot(m, invertedImg);
                m = invertedImg.Clone();
                invertedImg.Dispose();
            }
            pbRBackMask.Image = m.ToBitmap();

            if (MessageBox.Show(GV.Dlang.strRBottomMask, GV.Dlang.strRBottomMaskSave, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                if (_recipe.RightLowMaskMat != null && !_recipe.RightLowMaskMat.Empty())
                {
                    pbRBackMask.Image = _recipe.RightLowMaskMat.ToBitmap();
                }
                else
                {
                    pbRBackMask.Image = null;
                }

                return;
            }

            _AlignC.RBMaskBright = tBRightBackMaskLight.Value;
            _AlignC.RLMaskOffsetX = 0;
            _AlignC.RLMaskOffsetY = 0;
            RightMaskMat = GV.RightBackCam.Grab();
            GV.RightMaskMat = RightMaskMat;
            _recipe.SetRightLowMaskMat(m);
            GrayImage d = new GrayImage(m.Width, m.Height);
            d.Fill(255);
            _recipe.RightLowMaskMask = d;
            _AlignC.RLMaskAlgorithm = Algorithm(cbRBackMaskAlgo.SelectedIndex);
            if (cbRBackMaskAlgo.SelectedIndex == 2)
            {
                if(cbRBackMaskClassList.SelectedIndex >=0 && cbRBackMaskClassList.SelectedItem != null)
                {
                    string selectedClassName = cbRBackMaskClassList.SelectedItem.ToString();
                    Dictionary<string, Int16> ClassList = GV.AIClassList.GetClassList();
                    if (ClassList.ContainsKey(selectedClassName))
                    {
                        _AlignC.RLMaskAIClassId = ClassList[selectedClassName];
                    }
                    else
                    {
                        MessageBox.Show($"找不到類別 '{selectedClassName}'，請重新選擇", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    _AlignC.RLMaskAIClassId = -1;
                }
            }
            else
            {
                _AlignC.RLMaskAIClassId = -1;
            }
            if(GV.AppSettingParm.Emulation != true && GV.matcherRLM != null)
            {
                GV.matcherRLM.LearnWithAlgo(m, d, _AlignC.RLMaskAlgorithm);
            }
                
            if (!BtRMaskAdjSave.Visible)
            {
                if (GV.Plc.ReadData16(13028) == 0)
                {
                    _AlignC.RMaskAdjX = GV.AppSettingParm.RxShift4;
                    _AlignC.RMaskAdjY = GV.AppSettingParm.RyShift4;
                }
                else
                {
                    _AlignC.RMaskAdjX = GV.AppSettingParm.RxShift6;
                    _AlignC.RMaskAdjY = GV.AppSettingParm.RyShift6;
                }
            }
            _AlignC.LastModifyTime = DateTime.Now;
            GM.WriteRecipeXml(EditRecipe);
            //if (cbRBackMaskAlgo.SelectedIndex == 2)
            //{
            //    try
            //    {
            //        string trainPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Python", "Train", GV._recipe.RecipeName);
            //        DateTime now = DateTime.Now;
            //        Directory.CreateDirectory(trainPath);
            //        string fn = string.Concat(trainPath, "\\RBM", now.ToString("HH_mm_ss"));
            //        Cv2.ImWrite(string.Concat(fn, ".bmp"), fullImage);
            //        var labelPath = string.Concat(fn, ".txt");
            //        float xCenter = (roi.X + roi.Width / 2f) / fullImage.Width;
            //        float yCenter = (roi.Y + roi.Height / 2f) / fullImage.Height;
            //        float w = roi.Width / (float)fullImage.Width;
            //        float h = roi.Height / (float)fullImage.Height;
            //        int classId = cbRBackMaskClassList.SelectedIndex;
            //        List<YoloBox> boxes = new List<YoloBox>();
            //        boxes.Add(new YoloBox { ClassId = classId, X = xCenter, Y = yCenter, W = w, H = h });
            //        System.IO.File.WriteAllLines(labelPath, boxes.Select(b => $"{b.ClassId} {b.X:F6} {b.Y:F6} {b.W:F6} {b.H:F6}"));
            //    }
            //    catch
            //    {
            //    }
            //}
            pbRBackMask.Image = _recipe.RightLowMaskMat.ToBitmap();
        }

        //private void BtLMaskLocationUp_Click(object sender, EventArgs e)
        //{
        //    // LeftLearnPatternVideoWindow.MaskMove(0, -1);
        //}

        //private void BtRWaferLocationUp_Click(object sender, EventArgs e)
        //{
        //    // RightLearnPatternVideoWindow.WaferMove(0, -1);
        //}

        private void btRWaferLocationDown_Click(object sender, EventArgs e)
        {
            // RightLearnPatternVideoWindow.WaferMove(0, 1);
        }

        private void btRWaferLocationLeft_Click(object sender, EventArgs e)
        {
            // RightLearnPatternVideoWindow.WaferMove(-1, 0);
        }

        private void btRWaferLocationRight_Click(object sender, EventArgs e)
        {
            // RightLearnPatternVideoWindow.WaferMove(1, 0);
        }

        private void ButtonCreateLWaferPatternMask_Click(object sender, EventArgs e)
        {
            if (_recipe.LeftHighWaferMat != null)
            {
                if (_recipe.LeftHighWaferMask == null)
                {
                    _recipe.LeftHighWaferMask = new GrayImage(_recipe.LeftHighWaferMat.Width, _recipe.LeftHighWaferMat.Height);
                    _recipe.LeftHighWaferMask.Fill(255);
                }
                DialogPaintMask dialogPaintMask = new DialogPaintMask()
                {
                    Image = _recipe.LeftHighWaferMat,
                    Mask = _recipe.LeftHighWaferMask
                };
                if (dialogPaintMask.ShowDialog() == DialogResult.OK)
                {
                    _recipe.LeftHighWaferMask = new GrayImage(dialogPaintMask.Mask);
                    GV.matcherLHW.LearnWithAlgo(_recipe.LeftHighWaferMat, _recipe.LeftHighWaferMask, _AlignC.LHWaferAlgorithm); ;
                    _AlignC.LastModifyTime = DateTime.Now;
                    GM.WriteRecipeXml(EditRecipe);
                }
            }
        }

        private void RbLTopMask_CheckedChanged(object sender, EventArgs e)
        {
            if (rbLTopMask.Checked)
            {
                GV.LeftUpCam.Freeze();
                GV.LeftBackCam.Freeze();
                GV.skView = 0;
                tBLShowImage.Enabled = false;
                tBLeftTopMaskLight.Value = _AlignC.LeftBrightness[NowMagni];
                btLBackMaskSave.Enabled = false;
                // btLBackMask.Enabled = false;
                btLWaferSave.Enabled = false;
                btLBackMaskLSave.Enabled = false;
                btLBackMaskLightMinus.Enabled = false;
                btLBackMaskLightPlus.Enabled = false;
                btLWaferLightMinus.Enabled = false;
                btLWaferLightPlus.Enabled = false;
                btLWaferLSave.Enabled = false;
                ucNavigatorWaferL.Enabled = false;
                ucNavigatorBackMaskL.Enabled = false;
                // btLWaferMask.Enabled = false;
                rbLBackMask.Checked = false;
                rbLWafer.Checked = false;
                if (GV.UserLevel == GV.User.Administrator)
                {
                }
                else
                {
                    tBLeftBackMaskLight.Value = 0;
                    tBLeftBackWaferLight.Value = 0;
                    tBLeftBackMaskLight.Enabled = false;
                    tBLeftBackWaferLight.Enabled = false;
                }
                cbLBackMaskAlgo.Enabled = false;
                cbLWaferAlgo.Enabled = false;
                Thread.Sleep(SleepTime);
                // gBMag.Enabled = true;
                rBLowMagnification.Enabled = true;
                rBHighMagnification.Enabled = true;
                if (GV.UserLevel == GV.User.Operator)
                {
                    ucNavigatorTopMaskL.Enabled = false;
                    btLTopMaskSave.Enabled = false;
                    cbLTopMaskAlgo.Enabled = false;
                    btLTopMaskLSave.Enabled = false;
                    btLTopMaskLightMinus.Enabled = false;
                    btLTopMaskLightPlus.Enabled = false;
                    tBLeftTopMaskLight.Enabled = false;
                    btLTopMaskLSave.Enabled = false;
                    cbHighMagnification.Enabled = false;
                    cbLowMagnification.Enabled = false;
                    btSave.Enabled = false;
                }
                else
                {
                    btSave.Enabled = true;
                    ucNavigatorTopMaskL.Enabled = true;
                    btLTopMaskSave.Enabled = true;
                    cbLTopMaskAlgo.Enabled = true;
                    btLTopMaskLSave.Enabled = true;
                    btLTopMaskLightMinus.Enabled = true;
                    btLTopMaskLightPlus.Enabled = true;
                    tBLeftTopMaskLight.Enabled = true;
                    btLTopMaskLSave.Enabled = true;
                    if (rBHighMagnification.Checked) cbHighMagnification.Enabled = true;
                    if (rBLowMagnification.Checked) cbLowMagnification.Enabled = true;
                }
                btLTopPhotoMask.Enabled = true;
                GV.Light.ChangeBrightness("left", tBLeftTopMaskLight.Value);
                if (LeftMaskMat != null)
                    skLeft.SetShowMask(false, _dLalpha, LeftMaskMat);
                GV.LeftUpCam.SetWindow(skLeft);
                GV.LeftUpCam.Live();

                if (!rbRTopMask.Checked)
                {
                    rbRTopMask.Checked = true;
                }

                Update();
            }
        }

        private void rbLBackMask_CheckedChanged(object sender, EventArgs e)
        {
            if (rbLBackMask.Checked)
            {
                GV.LeftUpCam.Freeze();
                GV.LeftBackCam.Freeze();
                GV.skView = 1;
                tBLeftBackMaskLight.Value = _AlignC.LBMaskBright;
                btLTopMaskSave.Enabled = false;
                btLTopMaskLSave.Enabled = false;
                btLWaferSave.Enabled = false;
                btLWaferMask.Enabled = false;
                btFindLWaferCenter.Enabled = false;
                btLWaferLSave.Enabled = false;
                btLTopPhotoMask.Enabled = false;
                // btLWaferMask.Enabled = false;
                btLTopMaskLightPlus.Enabled = false;
                btLTopMaskLightMinus.Enabled = false;
                btLWaferLightPlus.Enabled = false;
                btLWaferLightMinus.Enabled = false;
                rbLTopMask.Checked = false;
                rbLWafer.Checked = false;
                tBLeftTopMaskLight.Value = 0;
                tBLeftTopMaskLight.Enabled = false;
                tBLeftBackWaferLight.Enabled = false;
                ucNavigatorTopMaskL.Enabled = false;
                ucNavigatorWaferL.Enabled = false;
                // gBMag.Enabled = false;
                cbLTopMaskAlgo.Enabled = false;
                cbLWaferAlgo.Enabled = false;
                // Thread.Sleep(SleepTime);
                if (GV.UserLevel == GV.User.Operator)
                {
                    ucNavigatorBackMaskL.Enabled = false;
                    btLBackMaskSave.Enabled = false;
                    cbLBackMaskAlgo.Enabled = false;
                    btLBackMaskLSave.Enabled = false;
                    btLBackMaskLightMinus.Enabled = false;
                    btLBackMaskLightPlus.Enabled = false;
                    tBLeftBackMaskLight.Enabled = false;
                    btLBackMaskLSave.Enabled = false;
                }
                else
                {
                    ucNavigatorBackMaskL.Enabled = true;
                    btLBackMaskSave.Enabled = true;
                    cbLBackMaskAlgo.Enabled = true;
                    btLBackMaskLSave.Enabled = true;
                    btLBackMaskLightMinus.Enabled = true;
                    btLBackMaskLightPlus.Enabled = true;
                    tBLeftBackMaskLight.Enabled = true;
                    btLBackMaskLSave.Enabled = true;
                }
                tBLShowImage.Enabled = true;
                GV.Light.ChangeBrightness("leftback", tBLeftBackMaskLight.Value);
                GV.LeftBackCam.SetWindow(skLeft);
                skLeft.SetShowMask(true, _dLalpha, LeftMaskMat);

                GV.LeftBackCam.Live();
                if (!rbRBackMask.Checked)
                {
                    rbRBackMask.Checked = true;
                }

                Update();
            }
        }

        private void rbWafer_Click(object sender, EventArgs e)
        {
            rbLWafer.Checked = true;
            rbRWafer.Checked = true;
            rbLWaferChanged();
            rbRWaferChanged();
        }

        private void rbLWafer_CheckedChanged(object sender, EventArgs e)
        {
            if (rbLWafer.Checked)
            {
                GV.LeftUpCam.Freeze();
                GV.LeftBackCam.Freeze();
                tBLeftBackWaferLight.Value = _AlignC.LBWaferBright;
                btLBackMaskSave.Enabled = false;
                // btLBackMask.Enabled = false;
                btLTopMaskSave.Enabled = false;
                btLTopPhotoMask.Enabled = false;
                btLTopMaskLSave.Enabled = false;
                btLBackMaskLSave.Enabled = false;
                btLTopMaskLightPlus.Enabled = false;
                btLTopMaskLightMinus.Enabled = false;
                btLBackMaskLightPlus.Enabled = false;
                btLBackMaskLightMinus.Enabled = false;
                rbLTopMask.Checked = false;
                rbLBackMask.Checked = false;
                tBLeftTopMaskLight.Value = 0;
                tBLeftTopMaskLight.Enabled = false;
                tBLeftBackMaskLight.Enabled = false;
                ucNavigatorTopMaskL.Enabled = false;
                ucNavigatorBackMaskL.Enabled = false;
                rBHighMagnification.Enabled = false;
                rBLowMagnification.Enabled = false;
                cbHighMagnification.Enabled = false;
                cbLowMagnification.Enabled = false;
                cbLTopMaskAlgo.Enabled = false;
                cbLBackMaskAlgo.Enabled = false;
                // Thread.Sleep(SleepTime);
                if (GV.UserLevel == GV.User.Operator)
                {
                    ucNavigatorWaferL.Enabled = false;
                    btLWaferSave.Enabled = false;
                    cbLWaferAlgo.Enabled = false;
                    btLWaferLSave.Enabled = false;
                    btLWaferLightMinus.Enabled = false;
                    btLWaferLightPlus.Enabled = false;
                    tBLeftBackWaferLight.Enabled = false;
                    btLWaferLSave.Enabled = false;
                    btMagSave.Enabled = false;
                    btSave.Enabled = false;
                }
                else
                {
                    btSave.Enabled = true;
                    ucNavigatorWaferL.Enabled = true;
                    btLWaferSave.Enabled = true;
                    cbLWaferAlgo.Enabled = true;
                    btLWaferLSave.Enabled = true;
                    btLWaferLightMinus.Enabled = true;
                    btLWaferLightPlus.Enabled = true;
                    tBLeftBackWaferLight.Enabled = true;
                    btLWaferLSave.Enabled = true;
                    btMagSave.Enabled = true;
                }
                tBLShowImage.Enabled = true;
                GV.Light.ChangeBrightness("leftback", tBLeftBackWaferLight.Value);
                GV.LeftBackCam.SetWindow(skLeft);
                skLeft.SetShowMask(true, _dLalpha, LeftMaskMat);
                GV.LeftBackCam.Live();

                if (!rbRWafer.Checked)
                {
                    rbRWafer.Checked = true;
                }

                Update();
            }
        }

        private void rbLWaferChanged()
        {
            GV.LeftUpCam.Freeze();
            GV.LeftBackCam.Freeze();
            tBLeftBackWaferLight.Value = _AlignC.LBWaferBright;
            btLBackMaskSave.Enabled = false;
            btLBackMask.Enabled = false;
            btFindLBMaskCenter.Enabled = false;
            btLTopMaskSave.Enabled = false;
            btLTopPhotoMask.Enabled = false;
            btLTopMaskLSave.Enabled = false;
            btLBackMaskLSave.Enabled = false;
            btLTopMaskLightPlus.Enabled = false;
            btLTopMaskLightMinus.Enabled = false;
            btLBackMaskLightPlus.Enabled = false;
            btLBackMaskLightMinus.Enabled = false;
            rbLTopMask.Checked = false;
            rbLBackMask.Checked = false;
            tBLeftTopMaskLight.Value = 0;
            tBLeftTopMaskLight.Enabled = false;
            tBLeftBackMaskLight.Enabled = false;
            ucNavigatorTopMaskL.Enabled = false;
            ucNavigatorBackMaskL.Enabled = false;
            // gBMag.Enabled = false;
            cbLTopMaskAlgo.Enabled = false;
            cbLBackMaskAlgo.Enabled = false;
            if (GV.UserLevel == GV.User.Operator)
            {
                ucNavigatorWaferL.Enabled = false;
                btLWaferSave.Enabled = false;
                cbLWaferAlgo.Enabled = false;
                btLWaferLSave.Enabled = false;
                btLWaferLightMinus.Enabled = false;
                btLWaferLightPlus.Enabled = false;
                tBLeftBackWaferLight.Enabled = false;
                btLWaferLSave.Enabled = false;
                btLWaferMask.Enabled = false;
                btFindLWaferCenter.Enabled = false;
                btSave.Enabled = false;
            }
            else
            {
                btSave.Enabled = true;
                ucNavigatorWaferL.Enabled = true;
                btLWaferSave.Enabled = true;
                cbLWaferAlgo.Enabled = true;
                btLWaferLSave.Enabled = true;
                btLWaferLightMinus.Enabled = true;
                btLWaferLightPlus.Enabled = true;
                tBLeftBackWaferLight.Enabled = true;
                btLWaferLSave.Enabled = true;
                btLWaferMask.Enabled = true;
                btFindLWaferCenter.Enabled = true;
            }
            tBLShowImage.Enabled = true;
            GV.Light.ChangeBrightness("leftback", tBLeftBackWaferLight.Value);
            GV.LeftBackCam.SetWindow(skLeft);
            skLeft.SetShowMask(true, _dLalpha, LeftMaskMat);
            GV.LeftBackCam.Live();
        }

        private void rbRWaferChanged()
        {
            GV.RightUpCam.Freeze();
            GV.RightBackCam.Freeze();
            tBRightBackWaferLight.Value = _AlignC.RBWaferBright;
            btRBackMaskSave.Enabled = false;
            btRBackMask.Enabled = false;
            btFindRBMaskCenter.Enabled = false;
            btRTopMaskSave.Enabled = false;
            btRTopPhotoMask.Enabled = false;
            btRTopMaskLSave.Enabled = false;
            btRBackMaskLSave.Enabled = false;
            btRBackMaskLightPlus.Enabled = false;
            btRBackMaskLightMinus.Enabled = false;
            btRTopMaskLightPlus.Enabled = false;
            btRTopMaskLightMinus.Enabled = false;
            rbRTopMask.Checked = false;
            rbRBackMask.Checked = false;
            tBRightTopMaskLight.Value = 0;
            tBRightTopMaskLight.Enabled = false;
            tBRightBackMaskLight.Enabled = false;
            ucNavigatorTopMaskR.Enabled = false;
            ucNavigatorBackMaskR.Enabled = false;
            cbRBackMaskAlgo.Enabled = false;
            cbRTopMaskAlgo.Enabled = false;
            rBHighMagnification.Enabled = false;
            rBLowMagnification.Enabled = false;
            cbHighMagnification.Enabled = false;
            cbLowMagnification.Enabled = false;
            if (GV.UserLevel == GV.User.Operator)
            {
                ucNavigatorWaferR.Enabled = false;
                btRWaferSave.Enabled = false;
                cbRWaferAlgo.Enabled = false;
                btRWaferLSave.Enabled = false;
                btRWaferLightMinus.Enabled = false;
                btRWaferLightPlus.Enabled = false;
                tBRightBackWaferLight.Enabled = false;
                btRWaferLSave.Enabled = false;
                btMagSave.Enabled = false;
                btRWaferMask.Enabled = false;
                btFindRWaferCenter.Enabled = false;
            }
            else
            {
                ucNavigatorWaferR.Enabled = true;
                btRWaferSave.Enabled = true;
                cbRWaferAlgo.Enabled = true;
                btRWaferLSave.Enabled = true;
                btRWaferLightMinus.Enabled = true;
                btRWaferLightPlus.Enabled = true;
                tBRightBackWaferLight.Enabled = true;
                btRWaferLSave.Enabled = true;
                btMagSave.Enabled = true;
                btRWaferMask.Enabled = true;
                btFindRWaferCenter.Enabled = true;
            }
            tBRShowImage.Enabled = true;
            GV.Light.ChangeBrightness("rightback", tBRightBackWaferLight.Value);
            GV.RightBackCam.SetWindow(skRight);
            skRight.SetShowMask(true, _dRalpha, RightMaskMat);
            GV.RightBackCam.Live();
        }

        private void rbBackMask_Click(object sender, EventArgs e)
        {
            rbLBackMask.Checked = true;
            rbRBackMask.Checked = true;
            rbLBackMaskChanged();
            rbRBackMaskChanged();
        }

        private void rbLBackMaskChanged()
        {
            GV.LeftUpCam.Freeze();
            GV.LeftBackCam.Freeze();
            GV.skView = 1;
            tBLeftBackMaskLight.Value = _AlignC.LBMaskBright;
            btLTopMaskSave.Enabled = false;
            btLTopMaskLSave.Enabled = false;
            btLWaferSave.Enabled = false;
            btLWaferMask.Enabled = false;
            btFindLWaferCenter.Enabled = false;
            btLWaferLSave.Enabled = false;
            btLTopPhotoMask.Enabled = false;
            // btLWaferMask.Enabled = false;
            btLTopMaskLightPlus.Enabled = false;
            btLTopMaskLightMinus.Enabled = false;
            btLWaferLightPlus.Enabled = false;
            btLWaferLightMinus.Enabled = false;
            rbLTopMask.Checked = false;
            rbLWafer.Checked = false;
            tBLeftTopMaskLight.Value = 0;
            tBLeftTopMaskLight.Enabled = false;
            tBLeftBackWaferLight.Enabled = false;
            ucNavigatorTopMaskL.Enabled = false;
            ucNavigatorWaferL.Enabled = false;
            // gBMag.Enabled = false;
            rBHighMagnification.Enabled = false;
            rBLowMagnification.Enabled = false;
            cbHighMagnification.Enabled = false;
            cbLowMagnification.Enabled = false;
            cbLTopMaskAlgo.Enabled = false;
            cbLWaferAlgo.Enabled = false;
            // Thread.Sleep(SleepTime);
            if (GV.UserLevel == GV.User.Operator)
            {
                ucNavigatorBackMaskL.Enabled = false;
                btLBackMaskSave.Enabled = false;
                cbLBackMaskAlgo.Enabled = false;
                btLBackMaskLSave.Enabled = false;
                btLBackMaskLightMinus.Enabled = false;
                btLBackMaskLightPlus.Enabled = false;
                tBLeftBackMaskLight.Enabled = false;
                btLBackMaskLSave.Enabled = false;
                btMagSave.Enabled = false;
                btLBackMask.Enabled = false;
                btFindLBMaskCenter.Enabled = false;
                btSave.Enabled = false;
            }
            else
            {
                btSave.Enabled = true;
                ucNavigatorBackMaskL.Enabled = true;
                btLBackMaskSave.Enabled = true;
                cbLBackMaskAlgo.Enabled = true;
                btLBackMaskLSave.Enabled = true;
                btLBackMaskLightMinus.Enabled = true;
                btLBackMaskLightPlus.Enabled = true;
                tBLeftBackMaskLight.Enabled = true;
                btLBackMaskLSave.Enabled = true;
                btMagSave.Enabled = true;
                btLBackMask.Enabled = true;
                btFindLBMaskCenter.Enabled = true;
            }
            tBLShowImage.Enabled = true;
            GV.Light.ChangeBrightness("leftback", tBLeftBackMaskLight.Value);
            GV.LeftBackCam.SetWindow(skLeft);
            skLeft.SetShowMask(true, _dLalpha, LeftMaskMat);

            GV.LeftBackCam.Live();
        }

        private void rbRBackMaskChanged()
        {
            GV.RightUpCam.Freeze();
            GV.RightBackCam.Freeze();
            tBRightBackMaskLight.Value = _AlignC.RBMaskBright;
            btRTopMaskSave.Enabled = false;
            btRWaferSave.Enabled = false;
            btRWaferMask.Enabled = false;
            btFindRWaferCenter.Enabled = false;
            btRTopPhotoMask.Enabled = false;
            // btRWaferMask.Enabled = false;
            btRWaferLSave.Enabled = false;
            btRTopMaskLSave.Enabled = false;
            btRTopMaskLightPlus.Enabled = false;
            btRTopMaskLightMinus.Enabled = false;
            btRWaferLightPlus.Enabled = false;
            btRWaferLightMinus.Enabled = false;
            rbRTopMask.Checked = false;
            rbRWafer.Checked = false;
            tBRightTopMaskLight.Value = 0;
            tBRightTopMaskLight.Enabled = false;
            tBRightBackWaferLight.Enabled = false;
            ucNavigatorTopMaskR.Enabled = false;
            ucNavigatorWaferR.Enabled = false;
            cbRTopMaskAlgo.Enabled = false;
            cbRWaferAlgo.Enabled = false;
            rBHighMagnification.Enabled = false;
            rBLowMagnification.Enabled = false;
            cbHighMagnification.Enabled = false;
            cbLowMagnification.Enabled = false;
            if (GV.UserLevel == GV.User.Operator)
            {
                ucNavigatorBackMaskR.Enabled = false;
                btRBackMaskSave.Enabled = false;
                cbRBackMaskAlgo.Enabled = false;
                btRBackMaskLSave.Enabled = false;
                btRBackMaskLightMinus.Enabled = false;
                btRBackMaskLightPlus.Enabled = false;
                tBRightBackMaskLight.Enabled = false;
                btRBackMaskLSave.Enabled = false;
                btMagSave.Enabled = false;
                btRBackMask.Enabled = false;
                btFindRBMaskCenter.Enabled = false;
            }
            else
            {
                ucNavigatorBackMaskR.Enabled = true;
                btRBackMaskSave.Enabled = true;
                cbRBackMaskAlgo.Enabled = true;
                btRBackMaskLSave.Enabled = true;
                btRBackMaskLightMinus.Enabled = true;
                btRBackMaskLightPlus.Enabled = true;
                tBRightBackMaskLight.Enabled = true;
                btRBackMaskLSave.Enabled = true;
                btMagSave.Enabled = true;
                btRBackMask.Enabled = true;
                btFindRBMaskCenter.Enabled = true;
            }
            tBRShowImage.Enabled = true;
            GV.Light.ChangeBrightness("rightback", tBRightBackMaskLight.Value);
            GV.RightBackCam.SetWindow(skRight);
            skRight.SetShowMask(true, _dRalpha, RightMaskMat);
            GV.RightBackCam.Live();
        }

        private void rbRWafer_CheckedChanged(object sender, EventArgs e)
        {
            if (rbRWafer.Checked)
            {
                GV.RightUpCam.Freeze();
                GV.RightBackCam.Freeze();
                tBRightBackWaferLight.Value = _AlignC.RBWaferBright;
                btRBackMaskSave.Enabled = false;
                // btRBackMask.Enabled = false;
                btRTopMaskSave.Enabled = false;
                btRTopPhotoMask.Enabled = false;
                btRTopMaskLSave.Enabled = false;
                btRBackMaskLSave.Enabled = false;
                btRBackMaskLightPlus.Enabled = false;
                btRBackMaskLightMinus.Enabled = false;
                btRTopMaskLightPlus.Enabled = false;
                btRTopMaskLightMinus.Enabled = false;
                rbRTopMask.Checked = false;
                rbRBackMask.Checked = false;
                tBRightTopMaskLight.Value = 0;
                // tBRightBackMaskLight.Value = 0;
                tBRightTopMaskLight.Enabled = false;
                tBRightBackMaskLight.Enabled = false;
                ucNavigatorTopMaskR.Enabled = false;
                ucNavigatorBackMaskR.Enabled = false;
                cbRBackMaskAlgo.Enabled = false;
                cbRTopMaskAlgo.Enabled = false;
                // Thread.Sleep(SleepTime);
                if (GV.UserLevel == GV.User.Operator)
                {
                    ucNavigatorWaferR.Enabled = false;
                    btRWaferSave.Enabled = false;
                    cbRWaferAlgo.Enabled = false;
                    btRWaferLSave.Enabled = false;
                    btRWaferLightMinus.Enabled = false;
                    btRWaferLightPlus.Enabled = false;
                    tBRightBackWaferLight.Enabled = false;
                    btRWaferLSave.Enabled = false;
                }
                else
                {
                    ucNavigatorWaferR.Enabled = true;
                    btRWaferSave.Enabled = true;
                    cbRWaferAlgo.Enabled = true;
                    btRWaferLSave.Enabled = true;
                    btRWaferLightMinus.Enabled = true;
                    btRWaferLightPlus.Enabled = true;
                    tBRightBackWaferLight.Enabled = true;
                    btRWaferLSave.Enabled = true;
                }
                tBRShowImage.Enabled = true;
                GV.Light.ChangeBrightness("rightback", tBRightBackWaferLight.Value);
                GV.RightBackCam.SetWindow(skRight);
                skRight.SetShowMask(true, _dRalpha, RightMaskMat);
                GV.RightBackCam.Live();

                if (!rbLWafer.Checked)
                {
                    rbLWafer.Checked = true;
                }

                Update();
            }
        }

        private void rbRBackMask_CheckedChanged(object sender, EventArgs e)
        {
            if (rbRBackMask.Checked)
            {
                GV.RightUpCam.Freeze();
                GV.RightBackCam.Freeze();
                tBRightBackMaskLight.Value = _AlignC.RBMaskBright;
                btRTopMaskSave.Enabled = false;
                btRWaferSave.Enabled = false;
                btRTopPhotoMask.Enabled = false;
                // btRWaferMask.Enabled = false;
                btRWaferLSave.Enabled = false;
                btRTopMaskLSave.Enabled = false;
                btRTopMaskLightPlus.Enabled = false;
                btRTopMaskLightMinus.Enabled = false;
                btRWaferLightPlus.Enabled = false;
                btRWaferLightMinus.Enabled = false;
                rbRTopMask.Checked = false;
                rbRWafer.Checked = false;
                tBRightTopMaskLight.Value = 0;
                tBRightTopMaskLight.Enabled = false;
                tBRightBackWaferLight.Enabled = false;
                ucNavigatorTopMaskR.Enabled = false;
                ucNavigatorWaferR.Enabled = false;
                cbRTopMaskAlgo.Enabled = false;
                cbRWaferAlgo.Enabled = false;
                // Thread.Sleep(SleepTime);
                if (GV.UserLevel == GV.User.Operator)
                {
                    ucNavigatorBackMaskR.Enabled = false;
                    btRBackMaskSave.Enabled = false;
                    cbRBackMaskAlgo.Enabled = false;
                    btRBackMaskLSave.Enabled = false;
                    btRBackMaskLightMinus.Enabled = false;
                    btRBackMaskLightPlus.Enabled = false;
                    tBRightBackMaskLight.Enabled = false;
                    btRBackMaskLSave.Enabled = false;
                }
                else
                {
                    ucNavigatorBackMaskR.Enabled = true;
                    btRBackMaskSave.Enabled = true;
                    cbRBackMaskAlgo.Enabled = true;
                    btRBackMaskLSave.Enabled = true;
                    btRBackMaskLightMinus.Enabled = true;
                    btRBackMaskLightPlus.Enabled = true;
                    tBRightBackMaskLight.Enabled = true;
                    btRBackMaskLSave.Enabled = true;
                }
                tBRShowImage.Enabled = true;
                GV.Light.ChangeBrightness("rightback", tBRightBackMaskLight.Value);
                GV.RightBackCam.SetWindow(skRight);
                skRight.SetShowMask(true, _dRalpha, RightMaskMat);
                GV.RightBackCam.Live();

                if (!rbLBackMask.Checked)
                {
                    rbLBackMask.Checked = true;
                }

                Update();
            }
        }

        private void rbRTopMask_CheckedChanged(object sender, EventArgs e)
        {
            if (rbRTopMask.Checked)
            {
                GV.RightUpCam.Freeze();
                GV.RightBackCam.Freeze();
                tBRShowImage.Enabled = false;
                tBRightTopMaskLight.Value = _AlignC.RightBrightness[NowMagni];
                btRBackMaskSave.Enabled = false;
                // btRBackMask.Enabled = false;
                btRWaferSave.Enabled = false;
                // btRWaferMask.Enabled = false;
                btRBackMaskLSave.Enabled = false;
                btRWaferLSave.Enabled = false;
                btRWaferLightPlus.Enabled = false;
                btRWaferLightMinus.Enabled = false;
                btRBackMaskLightPlus.Enabled = false;
                btRBackMaskLightMinus.Enabled = false;
                rbRBackMask.Checked = false;
                rbRWafer.Checked = false;
                if (GV.UserLevel == GV.User.Administrator)
                {
                }
                else
                {
                    tBRightBackMaskLight.Value = 0;
                    tBRightBackWaferLight.Value = 0;
                    tBRightBackMaskLight.Enabled = false;
                    tBRightBackWaferLight.Enabled = false;
                }
                ucNavigatorBackMaskR.Enabled = false;
                ucNavigatorWaferR.Enabled = false;
                cbRWaferAlgo.Enabled = false;
                cbRBackMaskAlgo.Enabled = false;
                // Thread.Sleep(SleepTime);
                if (GV.UserLevel == GV.User.Operator)
                {
                    ucNavigatorTopMaskR.Enabled = false;
                    btRTopMaskSave.Enabled = false;
                    cbRTopMaskAlgo.Enabled = false;
                    btRTopMaskLSave.Enabled = false;
                    btRTopMaskLightMinus.Enabled = false;
                    btRTopMaskLightPlus.Enabled = false;
                    tBRightTopMaskLight.Enabled = false;
                    btRTopMaskLSave.Enabled = false;
                }
                else
                {
                    ucNavigatorTopMaskR.Enabled = true;
                    btRTopMaskSave.Enabled = true;
                    cbRTopMaskAlgo.Enabled = true;
                    btRTopMaskLSave.Enabled = true;
                    btRTopMaskLightMinus.Enabled = true;
                    btRTopMaskLightPlus.Enabled = true;
                    tBRightTopMaskLight.Enabled = true;
                    btRTopMaskLSave.Enabled = true;
                }
                btRTopPhotoMask.Enabled = true;
                GV.Light.ChangeBrightness("right", tBRightTopMaskLight.Value);
                GV.RightUpCam.SetWindow(skRight);
                if (RightMaskMat != null)
                    skRight.SetShowMask(false, _dRalpha, RightMaskMat);
                GV.RightUpCam.Live();

                if (!rbLTopMask.Checked)
                {
                    rbLTopMask.Checked = true;
                }

                Update();
            }
        }

        private void TBLeftTopMaskLight_ValueChanged(object sender, EventArgs e)
        {
            _AlignC.LeftBrightness[NowMagni] = tBLeftTopMaskLight.Value;
            GV.Light.ChangeBrightness("left", tBLeftTopMaskLight.Value);
        }

        private void TBLeftBackMaskLight_ValueChanged(object sender, EventArgs e)
        {
            _AlignC.LBMaskBright = tBLeftBackMaskLight.Value;
            GV.Light.ChangeBrightness("leftback", tBLeftBackMaskLight.Value);
        }

        private void tBLeftBackWaferLight_ValueChanged(object sender, EventArgs e)
        {
            _AlignC.LBWaferBright = tBLeftBackWaferLight.Value;
            GV.Light.ChangeBrightness("leftback", tBLeftBackWaferLight.Value);
        }

        private void TBRightBackWaferLight_ValueChanged(object sender, EventArgs e)
        {
            _AlignC.RBWaferBright = tBRightBackWaferLight.Value;
            GV.Light.ChangeBrightness("rightback", tBRightBackWaferLight.Value);
        }

        private void TBRightBackMaskLight_ValueChanged(object sender, EventArgs e)
        {
            _AlignC.RBMaskBright = tBRightBackMaskLight.Value;
            GV.Light.ChangeBrightness("rightback", tBRightBackMaskLight.Value);
        }

        private void tBRightTopMaskLight_ValueChanged(object sender, EventArgs e)
        {
            _AlignC.RightBrightness[NowMagni] = tBRightTopMaskLight.Value;
            GV.Light.ChangeBrightness("right", tBRightTopMaskLight.Value);
        }

        private void btLTopMaskSave_Click(object sender, EventArgs e)
        {
            GV.TickCount = 0;

            Mat m = new Mat(GV.LeftUpCam.Grab(), skLeft.GetMaskRect());

            if (rBLowMagnification.Checked)
            {
                // MessageBox1 msg1 = new MessageBox1(GV.Dlang.strLLTopMask, GV.Dlang.strLLTopMaskSave, MessageBoxIcon.Question);
                // DialogResult m1 = msg1.ShowDialog();
                // if (m1 != DialogResult.OK)
                //    return;
                if (MessageBox.Show(GV.Dlang.strLLTopMask, GV.Dlang.strLLTopMaskSave, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                _recipe.LeftLowMaskMat = m;
                pbLTopMask.Image = m.ToBitmap();
                Cv2.ImWrite(GetTemplateFileName("LLM"), m);
                GrayImage d = new GrayImage(m.Width, m.Height);
                d.Fill(255);
                _recipe.LeftLowMaskMask = d;
                d.Save(GetTemplateFileName("MLLM"), System.Drawing.Imaging.ImageFormat.Bmp);
                _AlignC.LLMaskAlgorithm = Algorithm(cbLTopMaskAlgo.SelectedIndex);
            }
            else if (rBHighMagnification.Checked)
            {
                // MessageBox1 msg1 = new MessageBox1(GV.Dlang.strLHTopMask, GV.Dlang.strLHTopMaskSave, MessageBoxIcon.Question);
                // DialogResult m1 = msg1.ShowDialog();
                // if (m1 != DialogResult.OK)
                //    return;
                if (MessageBox.Show(GV.Dlang.strLHTopMask, GV.Dlang.strLHTopMaskSave, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
                _recipe.LeftHighMaskMat = m;
                pbLTopMask.Image = m.ToBitmap();
                Cv2.ImWrite(GetTemplateFileName("LHM"), m);
                GrayImage d = new GrayImage(m.Width, m.Height);
                d.Fill(255);
                _recipe.LeftHighMaskMask = d;
                d.Save(GetTemplateFileName("MLHM"), System.Drawing.Imaging.ImageFormat.Bmp);
                _AlignC.LHMaskAlgorithm = Algorithm(cbLTopMaskAlgo.SelectedIndex);
            }

            _AlignC.LastModifyTime = DateTime.Now;
            GM.WriteAlignConditionsAcarToXml("AlignConditions.xml");
        }

        private void btRTopMaskSave_Click(object sender, EventArgs e)
        {
            GV.TickCount = 0;

            Mat m = new Mat(GV.RightUpCam.Grab(), skRight.GetMaskRect());

            GrayImage d = new GrayImage(m.Width, m.Height);
            d.Fill(255);

            if (rBLowMagnification.Checked)
            {
                // MessageBox1 msg1 = new MessageBox1(GV.Dlang.strRLTopMask, GV.Dlang.strRLTopMaskSave, MessageBoxIcon.Question);
                // DialogResult m1 = msg1.ShowDialog();
                // if (m1 != DialogResult.OK)
                //    return;
                if (MessageBox.Show(GV.Dlang.strRLTopMask, GV.Dlang.strRLTopMaskSave, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                _recipe.RightLowMaskMat = m;
                Cv2.ImWrite(GetTemplateFileName("RLM"), m);
                _recipe.RightLowMaskMask = d;
                d.Save(GetTemplateFileName("MRLM"), System.Drawing.Imaging.ImageFormat.Bmp);
                _AlignC.RLMaskAlgorithm = Algorithm(cbRTopMaskAlgo.SelectedIndex);
            }
            else if (rBHighMagnification.Checked)
            {
                // MessageBox1 msg1 = new MessageBox1(GV.Dlang.strRHTopMask, GV.Dlang.strRHTopMaskSave, MessageBoxIcon.Question);
                // DialogResult m1 = msg1.ShowDialog();
                // if (m1 != DialogResult.OK)
                //    return;
                if (MessageBox.Show(GV.Dlang.strRHTopMask, GV.Dlang.strRHTopMaskSave, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                _recipe.RightHighMaskMat = m;
                Cv2.ImWrite(GetTemplateFileName("RHM"), m);
                _recipe.RightHighMaskMask = d;
                d.Save(GetTemplateFileName("MRHM"), System.Drawing.Imaging.ImageFormat.Bmp);
                _AlignC.RHMaskAlgorithm = Algorithm(cbRTopMaskAlgo.SelectedIndex);
            }

            pbRTopMask.Image = m.ToBitmap();
            _AlignC.LastModifyTime = DateTime.Now;
            GM.WriteAlignConditionsAcarToXml("AlignConditions.xml");
        }

        private void btLRingLightMinus_Click(object sender, EventArgs e)
        {
            if (tBLeftTopMaskLight.Value > 1) tBLeftTopMaskLight.Value--;
            CheckParam();
        }

        private void btLRingLightPlus_Click(object sender, EventArgs e)
        {
            if (tBLeftTopMaskLight.Value < 254) tBLeftTopMaskLight.Value++;
            CheckParam();
        }

        private void btLBackMaskLightPlus_Click(object sender, EventArgs e)
        {
            if (tBLeftBackMaskLight.Value < 254) tBLeftBackMaskLight.Value++;
            CheckParam();
        }

        private void btLWaferLightPlus_Click(object sender, EventArgs e)
        {
            if (tBLeftBackWaferLight.Value < 254) tBLeftBackWaferLight.Value++;
            CheckParam();
        }

        private void btLWaferLightMinus_Click(object sender, EventArgs e)
        {
            if (tBLeftBackWaferLight.Value > 1) tBLeftBackWaferLight.Value--;
            CheckParam();
        }

        private void btLBackMaskLightMinus_Click(object sender, EventArgs e)
        {
            if (tBLeftBackMaskLight.Value > 1) tBLeftBackMaskLight.Value--;
            CheckParam();
        }

        private void btRWaferLightPlus_Click(object sender, EventArgs e)
        {
            if (tBRightBackWaferLight.Value < 254) tBRightBackWaferLight.Value++;
            CheckParam();
        }

        private void btRWaferLightMinus_Click(object sender, EventArgs e)
        {
            if (tBRightBackWaferLight.Value > 1) tBRightBackWaferLight.Value--;
            CheckParam();
        }

        private void btRBackMaskLightPlus_Click(object sender, EventArgs e)
        {
            if (tBRightBackMaskLight.Value < 254) tBRightBackMaskLight.Value++;
            CheckParam();
        }

        private void btRBackMaskLightMinus_Click(object sender, EventArgs e)
        {
            if (tBRightBackMaskLight.Value > 1) tBRightBackMaskLight.Value--;
            CheckParam();
        }

        private void btRTopMaskLightPlus_Click(object sender, EventArgs e)
        {
            if (tBRightTopMaskLight.Value < 254) tBRightTopMaskLight.Value++;
            CheckParam();
        }

        private void BtRTopMaskLightMinus_Click(object sender, EventArgs e)
        {
            if (tBRightTopMaskLight.Value > 1) tBRightTopMaskLight.Value -= 1;
            CheckParam();
        }

        private void BtLBackMaskSave_Click(object sender, EventArgs e)
        {
            GV.TickCount = 0;
            Mat m;
            Mat fullImage = GV.LeftBackCam.Grab();


            if (fullImage == null || fullImage.Empty())
            {
                MessageBox.Show("Capture image fail!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            OpenCvSharp.Rect roi = skLeft.GetMaskRect();
            if (roi.Width <= 0 || roi.Height <= 0 ||
                roi.X < 0 || roi.Y < 0 ||
                roi.X + roi.Width > fullImage.Width ||
                roi.Y + roi.Height > fullImage.Height)
            {
                MessageBox.Show("晶圓區域無效或超出影像範圍", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                fullImage?.Dispose();
                return;
            }
            m = new Mat(fullImage, roi).Clone();
            if (cbLBackMaskAlgo.SelectedIndex == 1)
            {
                Cv2.GaussianBlur(m, m, new OpenCvSharp.Size(7, 7), 0, 0, BorderTypes.Default);
                Cv2.AdaptiveThreshold(m, m, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.Binary, 27, 2);
                Mat invertedImg = new Mat();
                Cv2.BitwiseNot(m, invertedImg);
                m = invertedImg.Clone();
                invertedImg.Dispose();
            }
            pbLBackMask.Image = m.ToBitmap();

            if (MessageBox.Show(GV.Dlang.strLBottomMask, GV.Dlang.strLBottomMaskSave, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                //if(pbLBackMask.Image != null)
                //{
                //    pbLBackMask.Image.Dispose();
                //}
                if (_recipe.LeftLowMaskMat != null && !_recipe.LeftLowMaskMat.Empty())
                {
                    pbLBackMask.Image = _recipe.LeftLowMaskMat.ToBitmap();
                }
                else
                {
                    pbLBackMask.Image = null;
                }
                return;
            }
            _AlignC.LBMaskBright = tBLeftBackMaskLight.Value;
            _AlignC.LLMaskOffsetX = 0;
            _AlignC.LLMaskOffsetY = 0;
            LeftMaskMat = GV.LeftBackCam.Grab();
            GV.LeftMaskMat = LeftMaskMat;
            _recipe.SetLeftLowMaskMat(m);
            GrayImage d = new GrayImage(m.Width, m.Height);
            d.Fill(255);
            _recipe.LeftLowMaskMask = new GrayImage(d);
            _AlignC.LLMaskAlgorithm = Algorithm(cbLBackMaskAlgo.SelectedIndex);
            if (cbLBackMaskAlgo.SelectedIndex == 2)
            {
                if (cbLBackMaskClassList.SelectedIndex >= 0 && cbLBackMaskClassList.SelectedItem != null)
                {
                    string selectedClassName = cbLBackMaskClassList.SelectedItem.ToString();
                    Dictionary<string, Int16> ClassList = GV.AIClassList.GetClassList();
                    if (ClassList.ContainsKey(selectedClassName))
                    {
                        _AlignC.LLMaskAIClassId = ClassList[selectedClassName];
                    }
                    else
                    {
                        MessageBox.Show($"找不到類別 '{selectedClassName}'，請重新選擇", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    _AlignC.LLMaskAIClassId = -1;
                }
            }
            else
            {
                _AlignC.LLMaskAIClassId = -1;
            }
            if (GV.AppSettingParm.Emulation != true && GV.matcherLLM != null)
            {
                GV.matcherLLM.LearnWithAlgo(m, d, _AlignC.LLMaskAlgorithm);
            }
            if (!BtLMaskAdjSave.Visible)
            {
                if (GV.Plc.ReadData16(13028) == 0)
                {
                    _AlignC.LMaskAdjX = GV.AppSettingParm.LxShift4;
                    _AlignC.LMaskAdjY = GV.AppSettingParm.LyShift4;
                }
                else
                {
                    _AlignC.LMaskAdjX = GV.AppSettingParm.LxShift6;
                    _AlignC.LMaskAdjY = GV.AppSettingParm.LyShift6;
                }
            }
            //if (cbLBackMaskAlgo.SelectedIndex == 2)
            //{
            //    try
            //    {
            //        string trainPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Python", "Train", GV._recipe.RecipeName);
            //        DateTime now = DateTime.Now;
            //        Directory.CreateDirectory(trainPath);
            //        string fn = string.Concat(trainPath, "\\LBM", now.ToString("HH_mm_ss"));
            //        Cv2.ImWrite(string.Concat(fn, ".bmp"), fullImage);
            //        var labelPath = string.Concat(fn, ".txt");
            //        float xCenter = (roi.X + roi.Width / 2f) / fullImage.Width;
            //        float yCenter = (roi.Y + roi.Height / 2f) / fullImage.Height;
            //        float w = roi.Width / (float)fullImage.Width;
            //        float h = roi.Height / (float)fullImage.Height;
            //        int classId = cbLBackMaskClassList.SelectedIndex;
            //        List<YoloBox> boxes = new List<YoloBox>();
            //        boxes.Add(new YoloBox { ClassId = classId, X = xCenter, Y = yCenter, W = w, H = h });
            //        System.IO.File.WriteAllLines(labelPath, boxes.Select(b => $"{b.ClassId} {b.X:F6} {b.Y:F6} {b.W:F6} {b.H:F6}"));
            //    }
            //    catch
            //    {

            //    }

            //}
            _AlignC.LastModifyTime = DateTime.Now;
            pbLBackMask.Image = _recipe.LeftLowMaskMat.ToBitmap();
            GM.WriteRecipeXml(EditRecipe);
        }

        //public void ShowMe()
        //{
        //    if (GV.AppSettingParm.Author == 0)
        //    {
        //        buttonF.Visible = false;
        //        buttonL.Visible = false;
        //        buttonR.Visible = false;
        //    }
        //    else
        //    {
        //        buttonF.Visible = true;
        //        buttonL.Visible = true;
        //        buttonR.Visible = true;
        //        groupBox4.Visible = true;
        //        btCaptureN.Visible = true;
        //    }

        //    if (GMPBackLeft == null)
        //    {
        //        GMPBackLeft = new Mat();
        //        GMPBackRight = new Mat();

        //        Dictionary<string, Int16> ClassList = GV.AIClassList.GetClassList();
        //        UpdateClassList(ClassList);
        //    }
        //    if(_AlignC.LLMaskAlgorithm==OpenCV3MatchUMat.AlignAlgorithm.AIMatch && _AlignC.LLMaskAIClassId >= 0)
        //    {
        //        SetComboBoxByClassId(cbLBackMaskClassList, _AlignC.LLMaskAIClassId);
        //    }
        //    if(_AlignC.RLMaskAlgorithm==OpenCV3MatchUMat.AlignAlgorithm.AIMatch && _AlignC.RLMaskAIClassId >= 0)
        //    {
        //        SetComboBoxByClassId(cbRBackMaskClassList, _AlignC.RLMaskAIClassId);
        //    }
        //    if (_AlignC.LHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && _AlignC.LHWaferAIClassId >= 0)
        //    {
        //        SetComboBoxByClassId(cbLBackWaferClassList, _AlignC.LHWaferAIClassId);
        //    }
        //    if(_AlignC.RHWaferAlgorithm== OpenCV3MatchUMat.AlignAlgorithm.AIMatch && _AlignC.RHWaferAIClassId>=0)
        //    {
        //        SetComboBoxByClassId(cbRBackWaferClassList, _AlignC.RHWaferAIClassId);
        //    }

        //    if (GV.AppSettingParm.Emulation != true)
        //    {
        //        if (_recipe.LeftLowMaskMat != null && !_recipe.LeftLowMaskMat.Empty())
        //        {
        //            GV.matcherLLM.LearnWithAlgo(_recipe.LeftLowMaskMat, _recipe.LeftLowMaskMask, _AlignC.LLMaskAlgorithm);
        //        }

        //        if (_recipe.RightLowMaskMat != null && !_recipe.RightLowMaskMat.Empty())
        //        {
        //            // ✅ 關鍵:確保右邊 Mask matcher 學習模板
        //            GV.matcherRLM.LearnWithAlgo(_recipe.RightLowMaskMat, _recipe.RightLowMaskMask, _AlignC.RLMaskAlgorithm);
        //        }

        //        if (_recipe.LeftHighWaferMat != null && !_recipe.LeftHighWaferMat.Empty())
        //        {
        //            GV.matcherLHW.LearnWithAlgo(_recipe.LeftHighWaferMat, _recipe.LeftHighWaferMask, _AlignC.LHWaferAlgorithm);
        //        }

        //        if (_recipe.RightHighWaferMat != null && !_recipe.RightHighWaferMat.Empty())
        //        {
        //            // ✅ 關鍵:確保右邊 Wafer matcher 學習模板
        //            GV.matcherRHW.LearnWithAlgo(_recipe.RightHighWaferMat, _recipe.RightHighWaferMask, _AlignC.RHWaferAlgorithm);
        //        }
        //    }
        //    skLeft.CanCenterLine = true;
        //    skLeft.CanRectMaskAndWafer = true;
        //    skLeft.CanTrackPattern = true;
        //    skLeft.CanZoom = true;

        //    skRight.CanCenterLine = true;
        //    skRight.CanRectMaskAndWafer = true;
        //    skRight.CanTrackPattern = true;
        //    skRight.CanZoom = true;

        //    LeftMaskMat = GV.LeftMaskMat.Clone();
        //    RightMaskMat = GV.RightMaskMat.Clone();
        //    GV.LeftUpCam?.Freeze();
        //    GV.RightUpCam?.Freeze();
        //    GV.RingLight.ChangeBrightness("left", 0);
        //    GV.RingLight.ChangeBrightness("right", 0);
        //    iAdmin = GV.HwndFormMain.iAdmin;
        //    GV.LeftBackCam.SetWindow(skLeft);
        //    GV.RightBackCam.SetWindow(skRight);
        //    tBLeftTopMaskLight.Value = _AlignC.LeftBrightness[NowMagni];
        //    tBRightTopMaskLight.Value = _AlignC.RightBrightness[NowMagni];
        //    tBLeftBackMaskLight.Value = _AlignC.LBMaskBright;
        //    tBRightBackMaskLight.Value = _AlignC.RBMaskBright;
        //    tBLeftBackWaferLight.Value = _AlignC.LBWaferBright;
        //    tBRightBackWaferLight.Value = _AlignC.RBWaferBright;
        //    if (rbLTopMask.Checked)
        //    {
        //        GV.Light.ChangeBrightness("left", tBLeftTopMaskLight.Value);
        //    }
        //    else if (rbLBackMask.Checked)
        //    {
        //        GV.Light.ChangeBrightness("leftback", tBLeftBackMaskLight.Value);
        //    }
        //    else if (rbLWafer.Checked)
        //    {
        //        GV.Light.ChangeBrightness("leftback", tBLeftBackWaferLight.Value);
        //    }

        //    if (rbRTopMask.Checked)
        //    {
        //        GV.Light.ChangeBrightness("right", tBRightTopMaskLight.Value);
        //    }
        //    else if (rbRBackMask.Checked)
        //    {
        //        GV.Light.ChangeBrightness("rightback", tBRightBackMaskLight.Value);
        //    }
        //    else if (rbRWafer.Checked)
        //    {
        //        GV.Light.ChangeBrightness("rightback", tBRightBackWaferLight.Value);
        //    }

        //    if (GV.AppSettingParm.LeftBackCamEnable)
        //    {
        //        lbEmulationModeL.Visible = false;
        //    }

        //    if (GV.AppSettingParm.RightBackCamEnable)
        //    {
        //        lbEmulationModeR.Visible = false;
        //    }

        //    GV.LeftBackCam.Live();

        //    GV.RightBackCam.Live();
        //    if (DrawMatchThread == null)
        //    {
        //        DrawMatchThread = new Thread(DrawMatchPosition);

        //        DrawMatchThread.Start();
        //    }
        //    if (GV.skView == 0)
        //    {
        //        rbLBackMask.Checked = true;
        //        rbRBackMask.Checked = true;
        //        rbLBackMaskChanged();
        //        rbRBackMaskChanged();
        //        GV.NowWaferMask = 1;
        //    }
        //    else if (GV.skView == 1)
        //    {
        //        if (GV.NowWaferMask == 1)
        //        {
        //            rbLBackMask.Checked = true;
        //            rbRBackMask.Checked = true;
        //            rbLBackMaskChanged();
        //            rbRBackMaskChanged();
        //        }
        //        else if (GV.NowWaferMask == 2)
        //        {
        //            rbLWafer.Checked = true;
        //            rbRWafer.Checked = true;
        //            rbLWaferChanged();
        //            rbRWaferChanged();
        //        }
        //    }

        //    if (_AlignC.UpBotMask == 1)
        //    {
        //        cBBottomMask.Checked = false;
        //        cBTopMask.Checked = true;
        //    }
        //    else
        //    {
        //        cBBottomMask.Checked = true;
        //        cBTopMask.Checked = false;
        //    }

        //    if (_AlignC.AdjuestZ == 1)
        //    {
        //        cBAdjZ.Checked = true;
        //    }
        //    else
        //    {
        //        cBAdjZ.Checked = false;
        //    }
        //    NUDLMaskX.Value = _AlignC.LMaskAdjX;
        //    NUDLMaskY.Value = _AlignC.LMaskAdjY;
        //    NUDRMaskX.Value = _AlignC.RMaskAdjX;
        //    NUDRMaskY.Value = _AlignC.RMaskAdjY;
        //    groupBox4.Location = new System.Drawing.Point(560, 15);
        //    ApplyLanguage();
        //    CheckParam();
        //}
        public void ShowMe()
        {
            try
            {
                
                // ✅ 初始化基本可見性設定
                if (GV.AppSettingParm.Author == 0)
                {
                    buttonF.Visible = false;
                    buttonL.Visible = false;
                    buttonR.Visible = false;
                }
                else
                {
                    buttonF.Visible = true;
                    buttonL.Visible = true;
                    buttonR.Visible = true;
                    
                }

                // ✅ 初始化 Mat 物件
                if (GMPBackLeft == null)
                {
                    GMPBackLeft = new Mat();
                    GMPBackRight = new Mat();
                }

                // ✅ 非同步更新 AI ClassList（不阻塞 UI）
                Task.Run(async () =>
                {
                    try
                    {
                        var classNames = await GV.AIClassList.GetClassNamesAsync();
                        var classList = new Dictionary<string, Int16>();
                        for (int i = 0; i < classNames.Count; i++)
                        {
                            classList.Add(classNames[i], (Int16)i);
                        }

                        // 回到 UI 執行緒更新 ComboBox
                        if (!IsDisposed && IsHandleCreated)
                        {
                            BeginInvoke(new Action(() =>
                            {
                                try
                                {
                                    UpdateClassList(classList);
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine($"UpdateClassList in UI thread failed: {ex.Message}");
                                }
                            }));
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"更新 AI 類別列表失敗: {ex.Message}");
                        GM.WriteToStatusTextBox($"更新 AI 類別列表失敗: {ex.Message}");
                    }
                });

                // ✅ 根據已儲存的 AI ClassId 設定 ComboBox 選項
                if (_AlignC.LLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && _AlignC.LLMaskAIClassId >= 0)
                {
                    SetComboBoxByClassId(cbLBackMaskClassList, _AlignC.LLMaskAIClassId);
                }
                if (_AlignC.RLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && _AlignC.RLMaskAIClassId >= 0)
                {
                    SetComboBoxByClassId(cbRBackMaskClassList, _AlignC.RLMaskAIClassId);
                }
                if (_AlignC.LHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && _AlignC.LHWaferAIClassId >= 0)
                {
                    SetComboBoxByClassId(cbLBackWaferClassList, _AlignC.LHWaferAIClassId);
                }
                if (_AlignC.RHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && _AlignC.RHWaferAIClassId >= 0)
                {
                    SetComboBoxByClassId(cbRBackWaferClassList, _AlignC.RHWaferAIClassId);
                }

                // ✅ 非 Emulation 模式：讓 matcher 學習模板
                if (GV.AppSettingParm.Emulation != true)
                {
                    if (_recipe.LeftLowMaskMat != null && !_recipe.LeftLowMaskMat.Empty() && GV.matcherLLM != null)
                    {
                        GV.matcherLLM.LearnWithAlgo(_recipe.LeftLowMaskMat, _recipe.LeftLowMaskMask, _AlignC.LLMaskAlgorithm);
                    }

                    if (_recipe.RightLowMaskMat != null && !_recipe.RightLowMaskMat.Empty() && GV.matcherRLM != null)
                    {
                        GV.matcherRLM.LearnWithAlgo(_recipe.RightLowMaskMat, _recipe.RightLowMaskMask, _AlignC.RLMaskAlgorithm);
                    }

                    if (_recipe.LeftHighWaferMat != null && !_recipe.LeftHighWaferMat.Empty() && GV.matcherLHW != null)
                    {
                        GV.matcherLHW.LearnWithAlgo(_recipe.LeftHighWaferMat, _recipe.LeftHighWaferMask, _AlignC.LHWaferAlgorithm);
                    }

                    if (_recipe.RightHighWaferMat != null && !_recipe.RightHighWaferMat.Empty() && GV.matcherRHW != null)
                    {
                        GV.matcherRHW.LearnWithAlgo(_recipe.RightHighWaferMat, _recipe.RightHighWaferMask, _AlignC.RHWaferAlgorithm);
                    }
                }

                // ✅ 設定 SKWindow 功能
                skLeft.CanCenterLine = true;
                skLeft.CanRectMaskAndWafer = true;
                skLeft.CanTrackPattern = true;
                skLeft.CanZoom = true;

                skRight.CanCenterLine = true;
                skRight.CanRectMaskAndWafer = true;
                skRight.CanTrackPattern = true;
                skRight.CanZoom = true;

                // ✅ 設定 Mask Mat
                if (GV.LeftMaskMat != null && !GV.LeftMaskMat.Empty())
                {
                    LeftMaskMat = GV.LeftMaskMat.Clone();
                }
                if (GV.RightMaskMat != null && !GV.RightMaskMat.Empty())
                {
                    RightMaskMat = GV.RightMaskMat.Clone();
                }

                // ✅ 停止上層相機
                GV.LeftUpCam?.Freeze();
                GV.RightUpCam?.Freeze();
                GV.RingLight.ChangeBrightness("left", 0);
                GV.RingLight.ChangeBrightness("right", 0);

                // ✅ 設定相機視窗
                iAdmin = GV.HwndFormMain.iAdmin;
                GV.LeftBackCam.SetWindow(skLeft);
                GV.RightBackCam.SetWindow(skRight);

                // ✅ 設定亮度
                tBLeftTopMaskLight.Value = _AlignC.LeftBrightness[NowMagni];
                tBRightTopMaskLight.Value = _AlignC.RightBrightness[NowMagni];
                tBLeftBackMaskLight.Value = _AlignC.LBMaskBright;
                tBRightBackMaskLight.Value = _AlignC.RBMaskBright;
                tBLeftBackWaferLight.Value = _AlignC.LBWaferBright;
                tBRightBackWaferLight.Value = _AlignC.RBWaferBright;

                // ✅ 根據當前選擇的 RadioButton 設定光源
                if (rbLTopMask.Checked)
                {
                    GV.Light.ChangeBrightness("left", tBLeftTopMaskLight.Value);
                }
                else if (rbLBackMask.Checked)
                {
                    GV.Light.ChangeBrightness("leftback", tBLeftBackMaskLight.Value);
                }
                else if (rbLWafer.Checked)
                {
                    GV.Light.ChangeBrightness("leftback", tBLeftBackWaferLight.Value);
                }

                if (rbRTopMask.Checked)
                {
                    GV.Light.ChangeBrightness("right", tBRightTopMaskLight.Value);
                }
                else if (rbRBackMask.Checked)
                {
                    GV.Light.ChangeBrightness("rightback", tBRightBackMaskLight.Value);
                }
                else if (rbRWafer.Checked)
                {
                    GV.Light.ChangeBrightness("rightback", tBRightBackWaferLight.Value);
                }

                // ✅ 相機模擬模式檢查
                if (GV.AppSettingParm.LeftBackCamEnable)
                {
                    lbEmulationModeL.Visible = false;
                }

                if (GV.AppSettingParm.RightBackCamEnable)
                {
                    lbEmulationModeR.Visible = false;
                }

                // ✅ 啟動相機 Live
                GV.LeftBackCam.Live();
                GV.RightBackCam.Live();

                // ✅ 啟動 DrawMatchPosition 執行緒
                if (DrawMatchThread == null)
                {
                    DrawMatchThread = new Thread(DrawMatchPosition) { IsBackground = true };
                    DrawMatchThread.Start();
                }

                // ✅ 根據 skView 設定初始顯示模式
                if (GV.skView == 0)
                {
                    rbLBackMask.Checked = true;
                    rbRBackMask.Checked = true;
                    rbLBackMaskChanged();
                    rbRBackMaskChanged();
                    GV.NowWaferMask = 1;
                }
                else if (GV.skView == 1)
                {
                    if (GV.NowWaferMask == 1)
                    {
                        rbLBackMask.Checked = true;
                        rbRBackMask.Checked = true;
                        rbLBackMaskChanged();
                        rbRBackMaskChanged();
                    }
                    else if (GV.NowWaferMask == 2)
                    {
                        rbLWafer.Checked = true;
                        rbRWafer.Checked = true;
                        rbLWaferChanged();
                        rbRWaferChanged();
                    }
                }

                // ✅ 設定 Mask 類型選項
                if (_AlignC.UpBotMask == 1)
                {
                    cBBottomMask.Checked = false;
                    cBTopMask.Checked = true;
                }
                else
                {
                    cBBottomMask.Checked = true;
                    cBTopMask.Checked = false;
                }

                // ✅ 設定 Z 軸調整選項
                if (_AlignC.AdjuestZ == 1)
                {
                    cBAdjZ.Checked = true;
                }
                else
                {
                    cBAdjZ.Checked = false;
                }

                // ✅ 設定 Mask 調整參數
                NUDLMaskX.Value = _AlignC.LMaskAdjX;
                NUDLMaskY.Value = _AlignC.LMaskAdjY;
                NUDRMaskX.Value = _AlignC.RMaskAdjX;
                NUDRMaskY.Value = _AlignC.RMaskAdjY;

                // ✅ 設定 groupBox4 位置
                groupBox4.Location = new System.Drawing.Point(560, 15);

                // ✅ 套用語言設定
                ApplyLanguage();

                // ✅ 檢查參數
                CheckParam();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ShowMe failed: {ex.Message}");
                GM.WriteToStatusTextBox($"ShowMe 初始化失敗: {ex.Message}");
            }
        }

        private void rBLowMagnification_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void rBHighMagnification_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void cbLowMagnification_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rBLowMagnification.Checked)
            {
                rBHighMagnification.Enabled = false;
                rBLowMagnification.Enabled = false;
                cbHighMagnification.Enabled = false;
                cbLowMagnification.Enabled = false;

                NowMagni = cbLowMagnification.SelectedIndex;
                cbHLMagni_Change(NowMagni);
                tBLeftTopMaskLight.Value = _AlignC.LeftBrightness[NowMagni];
                tBRightTopMaskLight.Value = _AlignC.RightBrightness[NowMagni];

                Thread.Sleep(250);

                rBHighMagnification.Enabled = true;
                rBLowMagnification.Enabled = true;
                if (GV.UserLevel != GV.User.Operator)
                {
                    cbHighMagnification.Enabled = true;
                    cbLowMagnification.Enabled = true;
                }
            }
            CheckParam();
        }

        private void cbHighMagnification_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rBHighMagnification.Checked)
            {
                rBLowMagnification.Enabled = false;
                rBHighMagnification.Enabled = false;
                cbLowMagnification.Enabled = false;
                cbHighMagnification.Enabled = false;

                NowMagni = cbHighMagnification.SelectedIndex;
                cbHLMagni_Change(NowMagni);
                tBLeftTopMaskLight.Value = _AlignC.LeftBrightness[NowMagni];
                tBRightTopMaskLight.Value = _AlignC.RightBrightness[NowMagni];

                Thread.Sleep(250);
                rBLowMagnification.Enabled = true;
                rBHighMagnification.Enabled = true;
                if (GV.UserLevel != GV.User.Operator)
                {
                    cbLowMagnification.Enabled = true;
                    cbHighMagnification.Enabled = true;
                }
            }
            CheckParam();
        }

        private async void cbHLMagni_Change(int NowMagn)
        {
            await Task.Run(() => ChangeLensMagnificationWait(NowMagn));
        }

        private void ChangeLensMagnification(string side, int motormagnification)
        {
            try
            {
                if (side == "left")
                {
                    if (!GV.LeftZoomLens.MoveGoto(motormagnification))
                    {
                        // GV.Plc.Send("WR DM6400.S 1");
                        throw new MRException("Error : Zoom lens fail.");
                    }
                }

                if (side == "right")
                {
                    if (!GV.RightZoomLens.MoveGoto(motormagnification))
                    {
                        // GV.Plc.Send("WR DM6400.S 1");
                        throw new MRException("Error : Zoom lens fail.");
                    }
                }
            }
            catch
            {
                Debug.WriteLine("ChangeLensMagnification fail");
                throw;
            }
        }

        private void ChangeLensMagnificationWait(int motormagnification)
        {
            try
            {
                GV.LeftZoomLens.Magnification = motormagnification;
                GV.RightZoomLens.Magnification = motormagnification;

                GV.LeftZoomLens.MoveGoto(GV.ZoomLensInfo.LeftMagnificationMotorSteps[motormagnification]);
                GV.RightZoomLens.MoveGoto(GV.ZoomLensInfo.RightMagnificationMotorSteps[motormagnification]);

                Stopwatch sw = Stopwatch.StartNew();
                while (true)
                {
                    Thread.Sleep(100);
                    if (GV.LeftZoomLens.GetStatus() && GV.RightZoomLens.GetStatus())
                        return;
                    if (sw.ElapsedMilliseconds > 4500)
                    {
                        //GV.LeftZoomLens.SetStatus();
                        //GV.RightZoomLens.SetStatus();
                        break;
                    }
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("ChangeLensMagnification fail");
                throw;
            }
        }

        private void tBLShowImage_ValueChanged(object sender, EventArgs e)
        {
            // iLalpha = tBLShowImage.Value;
            _dLalpha = (double)tBLShowImage.Value / 100;
            _AlignC.dLalpha = _dLalpha;
            if (rbLTopMask.Checked == false)
            {
                skLeft.SetShowMask(true, _dLalpha, LeftMaskMat);
            }
            else
            {
                skLeft.SetShowMask(false, 0, LeftMaskMat);
            }
        }

        private void ucNavigatorTopMaskL_CommandPressed(int dir)
        {
            int x = 0;
            int y = 0;
            switch (dir)
            {
                case 0:
                    x++;
                    break;
                case 1:
                    y--;
                    break;
                case 2:
                    x--;
                    break;
                case 3:
                    y++;
                    break;
            }
            skLeft.MaskMove(x, y);
        }

        private void ucNavigatorWaferL_CommandPressed(int dir)
        {
            int x = 0;
            int y = 0;
            switch (dir)
            {
                case 0:
                    x++;
                    break;
                case 1:
                    y--;
                    break;
                case 2:
                    x--;
                    break;
                case 3:
                    y++;
                    break;
            }
            skLeft.WaferMove(x, y);
        }

        private void ucNavigatorWaferR_CommandPressed(int dir)
        {
            int x = 0;
            int y = 0;
            switch (dir)
            {
                case 0:
                    x++;
                    break;
                case 1:
                    y--;
                    break;
                case 2:
                    x--;
                    break;
                case 3:
                    y++;
                    break;
            }
            skRight.WaferMove(x, y);
        }

        private void ucNavigatorTopMaskR_CommandPressed(int dir)
        {
            int x = 0;
            int y = 0;
            switch (dir)
            {
                case 0:
                    x++;
                    break;
                case 1:
                    y--;
                    break;
                case 2:
                    x--;
                    break;
                case 3:
                    y++;
                    break;
            }
            skRight.MaskMove(x, y);
        }

        private void btLTopMaskView_Click(object sender, EventArgs e)
        {
            if (rBLowMagnification.Checked)
            {
                if (MessageBox.Show("Sure to save Left Low Magnification Light ?", "Left Low Mag Light", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                _AlignC.LeftLight[NowMagni] = true;
                _AlignC.LeftBrightness[NowMagni] = tBLeftTopMaskLight.Value;
                _AlignC.AlignLowMagnification = NowMagni;
            }
            else
            {
                if (MessageBox.Show("Sure to save Left High Magnification Light ?", "Left High Mag Light", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                _AlignC.LeftLight[NowMagni] = true;
                _AlignC.LeftBrightness[NowMagni] = tBLeftTopMaskLight.Value;
                _AlignC.AlignHighMagnification = NowMagni;
            }

            _AlignC.LastModifyTime = DateTime.Now;
            GM.WriteAlignConditionsAcarToXml("AlignConditions.xml");
        }

        private void btLBackMaskView_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Sure to save Left Bottom Mask Light ?", "Left Bottom Mask Light", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            _AlignC.LBMaskBright = tBLeftBackMaskLight.Value;
            _AlignC.LastModifyTime = DateTime.Now;
            GM.WriteAlignConditionsAcarToXml("AlignConditions.xml");
        }

        private void btLWaferView_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Sure to save Left Bottom Wafer Light ?", "Left Bottom Wafer Light", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            _AlignC.LBWaferBright = tBLeftBackWaferLight.Value;
            _AlignC.LastModifyTime = DateTime.Now;
            GM.WriteAlignConditionsAcarToXml("AlignConditions.xml");
        }

        private void btRTopMaskLSave_Click(object sender, EventArgs e)
        {
            if (rBLowMagnification.Checked)
            {
                if (MessageBox.Show("Sure to save Right Low Magnification Light ?", "Right Low Mag Light", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                _AlignC.RightLight[NowMagni] = true;
                _AlignC.RightBrightness[NowMagni] = tBRightTopMaskLight.Value;
                _AlignC.AlignLowMagnification = NowMagni;
            }
            else
            {
                if (MessageBox.Show("Sure to save Right High Magnification Light ?", "Right High Mag Light", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                _AlignC.RightLight[NowMagni] = true;
                _AlignC.RightBrightness[NowMagni] = tBRightTopMaskLight.Value;
                _AlignC.AlignHighMagnification = NowMagni;
            }

            _AlignC.LastModifyTime = DateTime.Now;
            GM.WriteAlignConditionsAcarToXml("AlignConditions.xml");
        }

        private void btRBackMaskLSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Sure to save Right Bottom Mask Light ?", "Right Bottom Mask Light", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            _AlignC.RBMaskBright = tBRightBackMaskLight.Value;
            _AlignC.LastModifyTime = DateTime.Now;
            GM.WriteAlignConditionsAcarToXml("AlignConditions.xml");
        }

        private void btRWaferLSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Sure to save Right Bottom Wafer Light ?", "Right Bottom Wafer Light", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            _AlignC.RBWaferBright = tBRightBackWaferLight.Value;
            _AlignC.LastModifyTime = DateTime.Now;
            GM.WriteAlignConditionsAcarToXml("AlignConditions.xml");
        }

        private void btMagSave_Click(object sender, EventArgs e)
        {
            // int Lmag = cbLowMagnification.SelectedIndex + 1;
            // int Hmag = cbHighMagnification.SelectedIndex + 1;

            // string smsg = "Sure to save \r\nLow Magnification = " + Lmag.ToString() + "\r\nHigh Magnification = " + Hmag.ToString() + "\r\nAnd Set Recipe to BOT Alignmode!!";


            if (rbLTopMask.Checked)
            {
                if (rBHighMagnification.Checked)
                {
                    // MessageBox1 msg1 = new MessageBox1(GV.Dlang.strLightHTopMask, GV.Dlang.strLightHTopMaskSave, MessageBoxIcon.Question);
                    // DialogResult m1 = msg1.ShowDialog();
                    // if (m1 != DialogResult.OK)
                    //     return;
                    if (MessageBox.Show(GV.Dlang.strLightHTopMask, GV.Dlang.strLightHTopMaskSave, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return;

                    _AlignC.AlignHighMagnification = cbHighMagnification.SelectedIndex;
                    _AlignC.LeftLight[NowMagni] = true;
                    _AlignC.LeftBrightness[NowMagni] = tBLeftTopMaskLight.Value;
                    _AlignC.RightLight[NowMagni] = true;
                    _AlignC.RightBrightness[NowMagni] = tBRightTopMaskLight.Value;
                    _AlignC.LHMaskAlgorithm = Algorithm(cbLTopMaskAlgo.SelectedIndex);
                    _AlignC.RHMaskAlgorithm = Algorithm(cbRTopMaskAlgo.SelectedIndex);
                }
                else if (rBLowMagnification.Checked)
                {
                    // MessageBox1 msg1 = new MessageBox1(GV.Dlang.strLightLTopMask, GV.Dlang.strLightLTopMaskSave, MessageBoxIcon.Question);
                    // DialogResult m1 = msg1.ShowDialog();
                    // if (m1 != DialogResult.OK)
                    //   return;
                    if (MessageBox.Show(GV.Dlang.strLightLTopMask, GV.Dlang.strLightLTopMaskSave, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return;
                    _AlignC.AlignLowMagnification = cbLowMagnification.SelectedIndex;
                    _AlignC.LeftLight[NowMagni] = true;
                    _AlignC.LeftBrightness[NowMagni] = tBLeftTopMaskLight.Value;
                    _AlignC.RightLight[NowMagni] = true;
                    _AlignC.RightBrightness[NowMagni] = tBRightTopMaskLight.Value;
                    _AlignC.LLMaskAlgorithm = Algorithm(cbLTopMaskAlgo.SelectedIndex);
                    _AlignC.RLMaskAlgorithm = Algorithm(cbRTopMaskAlgo.SelectedIndex);
                }
            }
            else if (rbLBackMask.Checked)
            {
                // MessageBox1 msg1 = new MessageBox1(GV.Dlang.strLightBottomMask, GV.Dlang.strLightBottomMaskSave, MessageBoxIcon.Question);
                // DialogResult m1 = msg1.ShowDialog();
                // if (m1 != DialogResult.OK)
                //    return;
                if (MessageBox.Show(GV.Dlang.strLightBottomMask, GV.Dlang.strLightBottomMaskSave, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                _AlignC.LBMaskBright = tBLeftBackMaskLight.Value;
                _AlignC.RBMaskBright = tBRightBackMaskLight.Value;
                _AlignC.dLalpha = (double)tBLShowImage.Value / 100;
                _AlignC.dRalpha = (double)tBRShowImage.Value / 100;
                iLalpha = tBLShowImage.Value;
                iRalpha = tBRShowImage.Value;
                _AlignC.LLWaferAlgorithm = Algorithm(cbLBackMaskAlgo.SelectedIndex);
                _AlignC.RLWaferAlgorithm = Algorithm(cbRBackMaskAlgo.SelectedIndex);
            }
            else if (rbLWafer.Checked)
            {
                // MessageBox1 msg1 = new MessageBox1(GV.Dlang.strLightWafer, GV.Dlang.strLightWaferSave, MessageBoxIcon.Question);
                // DialogResult m1 = msg1.ShowDialog();
                // if (m1 != DialogResult.OK)
                //     return;
                if (MessageBox.Show(GV.Dlang.strLightWafer, GV.Dlang.strLightWaferSave, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                _AlignC.LBWaferBright = tBLeftBackWaferLight.Value;
                _AlignC.RBWaferBright = tBRightBackWaferLight.Value;
                _AlignC.dLalpha = (double)tBLShowImage.Value / 100;
                _AlignC.dRalpha = (double)tBRShowImage.Value / 100;
                iLalpha = tBLShowImage.Value;
                iRalpha = tBRShowImage.Value;
                _AlignC.LHWaferAlgorithm = Algorithm(cbLWaferAlgo.SelectedIndex);
                _AlignC.RHWaferAlgorithm = Algorithm(cbRWaferAlgo.SelectedIndex);
            }

            _AlignC.UpBackAlign = 2;
            _AlignC.LastModifyTime = DateTime.Now;
            GM.WriteAlignConditionsAcarToXml("AlignConditions.xml");
            CheckParam();
        }
        private void CheckParam()
        {
            GV.TickCount = 0;

            Color Ocolor = btSave.BackColor;
            Color Ncolor = btSave.BackColor;

            if ((rbLBackMask.Checked) && (_AlignC.LBMaskBright != tBLeftBackMaskLight.Value))
            {
                Ncolor = Color.LightGreen;
            }
            else if ((rbLWafer.Checked) && (_AlignC.LBWaferBright != tBLeftBackWaferLight.Value))
            {
                Ncolor = Color.LightGreen;
            }
            else if ((rbRBackMask.Checked) && (_AlignC.RBMaskBright != tBRightBackMaskLight.Value))
            {
                Ncolor = Color.LightGreen;
            }
            else if ((rbRWafer.Checked) && (_AlignC.RBWaferBright != tBRightBackWaferLight.Value))
            {
                Ncolor = Color.LightGreen;
            }
            else if ((tBLShowImage.Enabled) && (iLalpha != tBLShowImage.Value))
            {
                Ncolor = Color.LightGreen;
            }
            else if ((tBRShowImage.Enabled) && (iRalpha != tBRShowImage.Value))
            {
                Ncolor = Color.LightGreen;
            }
            else if ((rbLBackMask.Checked) && (_AlignC.LLWaferAlgorithm != Algorithm(cbLBackMaskAlgo.SelectedIndex)))
            {
                Ncolor = Color.LightGreen;
            }
            else if ((rbRBackMask.Checked) && (_AlignC.RLWaferAlgorithm != Algorithm(cbRBackMaskAlgo.SelectedIndex)))
            {
                Ncolor = Color.LightGreen;
            }
            else if ((rbLWafer.Checked) && (_AlignC.LHWaferAlgorithm != Algorithm(cbLWaferAlgo.SelectedIndex)))
            {
                Ncolor = Color.LightGreen;
            }
            else if ((rbRWafer.Checked) && (_AlignC.RHWaferAlgorithm != Algorithm(cbRWaferAlgo.SelectedIndex)))
            {
                Ncolor = Color.LightGreen;
            }
            else if (_AlignC.UpBackAlign != 2)
            {
                Ncolor = Color.LightGreen;
            }
            else
            {
                Ncolor = Color.MintCream;
            }

            if (Ncolor != Ocolor)
            {
                btSave.BackColor = Ncolor;
                Update();
            }
        }
        private void btLShowImageSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Sure to save Left Show Mask Image ?", "Left Show Mask Image", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            _AlignC.dLalpha = (double)tBLShowImage.Value / 100;
            _AlignC.LastModifyTime = DateTime.Now;
            GM.WriteAlignConditionsAcarToXml("AlignConditions.xml");
        }

        private void btRShowImageSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Sure to save Left Show Mask Image ?", "Left Show Mask Image", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            _AlignC.dRalpha = (double)tBRShowImage.Value / 100;
            _AlignC.LastModifyTime = DateTime.Now;
            GM.WriteAlignConditionsAcarToXml("AlignConditions.xml");
        }
        private void tBRShowImage_ValueChanged(object sender, EventArgs e)
        {
            // iRalpha = tBRShowImage.Value;
            _dRalpha = (double)tBRShowImage.Value / 100;
            _AlignC.dRalpha = _dRalpha;
            if (rbRTopMask.Checked == false)
            {
                skRight.SetShowMask(true, _dRalpha, RightMaskMat);
            }
            else
            {
                skRight.SetShowMask(false, 0, RightMaskMat);
            }
        }
        private void btDelete_Click(object sender, EventArgs e)
        {
            string smsg = String.Format(GV.Dlang.strSureDeleteRecipe, EditRecipe.ToString());
            if (MessageBox.Show(smsg, GV.Dlang.strDeleteRecipe, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            // MessageBox1 msg1 = new MessageBox1(smsg, GV.Dlang.strDeleteRecipe, MessageBoxIcon.Question);
            // DialogResult m1 = msg1.ShowDialog();
            // if (m1 != DialogResult.OK)
            //    return;

            AlignCondition newOne = new AlignCondition();
            newOne.CopyTo(EditRecipe);

            _AlignC.UpBackAlign = 2;
            _AlignC.LastModifyTime = DateTime.Now;

            _recipe.LeftLowMaskMat = null;
            _recipe.LeftLowWaferMat = null;
            _recipe.RightLowMaskMat = null;
            _recipe.RightLowWaferMat = null;

            _recipe.LeftHighMaskMat = null;
            _recipe.LeftHighWaferMat = null;
            _recipe.RightHighMaskMat = null;
            _recipe.RightHighWaferMat = null;

            deleteTemplateFile("LLM");
            deleteTemplateFile("LHM");
            deleteTemplateFile("RLM");
            deleteTemplateFile("RHM");
            deleteTemplateFile("LLW");
            deleteTemplateFile("LHW");
            deleteTemplateFile("RLW");
            deleteTemplateFile("RHW");
            deleteTemplateFile("LeftMask");
            deleteTemplateFile("RightMask");

            rBLowMagnification.Checked = false;
            rBHighMagnification.Checked = false;
            cbHighMagnification.SelectedIndex = _AlignC.AlignHighMagnification;
            cbLowMagnification.SelectedIndex = _AlignC.AlignLowMagnification;

            GM.WriteAlignConditionsAcarToXml("AlignConditions.xml");

            rBLowMagnification.Checked = true;

            CheckLevel();
            ShowMe();
        }

        /*
        private void timerNowTime_Tick(object sender, EventArgs e)
        {
            lbNowTime.Text = DateTime.Now.ToShortDateString() + DateTime.Now.ToString("    HH:mm:ss ") + GV.UserLevel.ToString();
        }
        */

        public string setTimeString
        {
            get
            {
                return _ClockTime;
            }

            set
            {
                _ClockTime = value;
                lbNowTime.Text = value;
            }
        }

        private void cbLTopMaskAlgo_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckParam();
        }

        private void cbLBackMaskAlgo_SelectedIndexChanged(object sender, EventArgs e)
        {
            _AlignC.LLMaskAlgorithm = Algorithm(cbLBackMaskAlgo.SelectedIndex);
            if (cbLBackMaskAlgo.SelectedIndex == 2)
            {
                cbLBackMaskClassList.Visible = true;
                btBackLabelPatternLM.Visible = false;
                pbLBackMask.Visible = false;
            }
            else
            {
                cbLBackMaskClassList.Visible = false;
                btBackLabelPatternLM.Visible = false;
                pbLBackMask.Visible = true;
            }

            if (_recipe.LeftLowMaskMat != null && !_recipe.LeftLowMaskMat.Empty())
            {
                if (GV.AppSettingParm.Emulation != true)
                {
                    GV.matcherLLM.LearnWithAlgo(_recipe.LeftLowMaskMat, _recipe.LeftLowMaskMask, _AlignC.LLMaskAlgorithm);
                }
            }
            CheckParam();
        }

        private void cbLWaferAlgo_SelectedIndexChanged(object sender, EventArgs e)
        {
            _AlignC.LHWaferAlgorithm = Algorithm(cbLWaferAlgo.SelectedIndex);
            if (cbLWaferAlgo.SelectedIndex == 2)
            {
                cbLBackWaferClassList.Visible = true;
                btBackLabelPatternL.Visible = true;
                pbLWafer.Visible = false;

            }
            else
            {
                cbLBackWaferClassList.Visible = false;
                btBackLabelPatternL.Visible = false;
                pbLWafer.Visible = true;

            }
            if (_recipe.LeftHighWaferMat != null && !_recipe.LeftHighWaferMat.Empty())
            {
                if (GV.AppSettingParm.Emulation != true)
                {
                    GV.matcherLHW.LearnWithAlgo(_recipe.LeftHighWaferMat, _recipe.LeftHighWaferMask, _AlignC.LHWaferAlgorithm);
                }
            }
            CheckParam();
        }

        private void cbRWaferAlgo_SelectedIndexChanged(object sender, EventArgs e)
        {
            _AlignC.RHWaferAlgorithm = Algorithm(cbRWaferAlgo.SelectedIndex);
            if (cbRWaferAlgo.SelectedIndex == 2)
            {
                cbRBackWaferClassList.Visible = true;
                btBackLabelPatternR.Visible = true;
                pbRWafer.Visible = false;
            }
            else
            {
                cbRBackWaferClassList.Visible = false;
                btBackLabelPatternR.Visible = false;
                pbRWafer.Visible = true;
            }
            if (_recipe.RightHighWaferMat != null && !_recipe.RightHighWaferMat.Empty())
            {
                if (GV.AppSettingParm.Emulation != true)
                {
                    GV.matcherRHW.LearnWithAlgo(_recipe.RightHighWaferMat, _recipe.RightHighWaferMask, _AlignC.RHWaferAlgorithm);
                }
            }
            CheckParam();
        }

        private void cbRBackMaskAlgo_SelectedIndexChanged(object sender, EventArgs e)
        {
            _AlignC.RLMaskAlgorithm = Algorithm(cbRBackMaskAlgo.SelectedIndex);
            if (cbRBackMaskAlgo.SelectedIndex == 2)
            {
                cbRBackMaskClassList.Visible = true;
                btBackLabelPatternRM.Visible = false;
                pbRBackMask.Visible = false;
            }
            else
            {
                cbRBackMaskClassList.Visible = false;
                btBackLabelPatternRM.Visible = false;
                pbRBackMask.Visible = true;
            }
            if (_recipe.RightLowMaskMat != null && !_recipe.RightLowMaskMat.Empty())
            {
                if (GV.AppSettingParm.Emulation != true)
                {
                    GV.matcherRLM.LearnWithAlgo(_recipe.RightLowMaskMat, _recipe.RightLowMaskMask, _AlignC.RLMaskAlgorithm);
                }
            }
            CheckParam();
        }

        private void cbRTopMaskAlgo_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckParam();
        }

        private void tBLShowImage_KeyUp(object sender, KeyEventArgs e)
        {
            CheckParam();
        }

        private void tBLShowImage_MouseUp(object sender, MouseEventArgs e)
        {
            CheckParam();
        }

        private void tBRShowImage_KeyUp(object sender, KeyEventArgs e)
        {
            CheckParam();
        }

        private void tBRShowImage_MouseUp(object sender, MouseEventArgs e)
        {
            CheckParam();
        }

        private void tBLeftTopMaskLight_MouseUp(object sender, MouseEventArgs e)
        {
            CheckParam();
        }

        private void tBLeftTopMaskLight_KeyUp(object sender, KeyEventArgs e)
        {
            CheckParam();
        }

        private void tBLeftBackMaskLight_KeyUp(object sender, KeyEventArgs e)
        {
            CheckParam();
        }

        private void tBLeftBackMaskLight_MouseUp(object sender, MouseEventArgs e)
        {
            CheckParam();
        }

        private void tBLeftBackWaferLight_KeyUp(object sender, KeyEventArgs e)
        {
            CheckParam();
        }

        private void tBLeftBackWaferLight_MouseUp(object sender, MouseEventArgs e)
        {
            CheckParam();
        }

        private void tBRightBackWaferLight_KeyUp(object sender, KeyEventArgs e)
        {
            CheckParam();
        }

        private void tBRightBackWaferLight_MouseUp(object sender, MouseEventArgs e)
        {
            CheckParam();
        }

        private void tBRightBackMaskLight_KeyUp(object sender, KeyEventArgs e)
        {
            CheckParam();
        }

        private void tBRightBackMaskLight_MouseUp(object sender, MouseEventArgs e)
        {
            CheckParam();
        }

        private void tBRightTopMaskLight_KeyUp(object sender, KeyEventArgs e)
        {
            CheckParam();
        }

        private void tBRightTopMaskLight_MouseUp(object sender, MouseEventArgs e)
        {
            CheckParam();
        }

        private void btDefault_Click(object sender, EventArgs e)
        {
            int[] Brightness = { 10, 10, 10, 10, 10, 10, 11, 31, 26, 77, 110, 130 };

            string smsg = string.Format(GV.Dlang.strSureDefaultRecipe, EditRecipe.ToString());
            if (MessageBox.Show(smsg, GV.Dlang.strSetRecipe, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            for (int i = 0; i < 12; i++)
            {
                _AlignC.LeftLight[i] = true;
                _AlignC.RightLight[i] = true;
                _AlignC.LRingLight[i] = false;
                _AlignC.RRingLight[i] = false;
                _AlignC.LeftBrightness[i] = Brightness[i];
                _AlignC.RightBrightness[i] = Brightness[i];
                _AlignC.LeftRingBrightness[i] = Brightness[i];
                _AlignC.RightRingBrightness[i] = Brightness[i];
            }

            _AlignC.LBMaskBright = 80;
            _AlignC.RBMaskBright = 80;
            _AlignC.LBWaferBright = 80;
            _AlignC.RBWaferBright = 80;

            GM.WriteAlignConditionsAcarToXml("AlignConditions.xml");

            if (rbLTopMask.Checked)
            {
                if (rBHighMagnification.Checked)
                {
                    tBLeftTopMaskLight.Value = _AlignC.LeftBrightness[_AlignC.AlignHighMagnification];
                    tBRightTopMaskLight.Value = _AlignC.RightBrightness[_AlignC.AlignHighMagnification];
                }
                else if (rBLowMagnification.Checked)
                {
                    tBLeftTopMaskLight.Value = _AlignC.LeftBrightness[_AlignC.AlignLowMagnification];
                    tBRightTopMaskLight.Value = _AlignC.RightBrightness[_AlignC.AlignLowMagnification];
                }
            }
            else if (rbLBackMask.Checked)
            {
                tBLeftBackMaskLight.Value = _AlignC.LBMaskBright;
                tBRightBackMaskLight.Value = _AlignC.RBMaskBright;
            }
            else if (rbLWafer.Checked)
            {
                tBLeftBackWaferLight.Value = _AlignC.LBWaferBright;
                tBRightBackMaskLight.Value = _AlignC.RBWaferBright;
            }

            CheckParam();
        }

        private void rBHighMagnification_Click(object sender, EventArgs e)
        {
            if (rBHighMagnification.Checked)
            {
                cbLowMagnification.Enabled = false;
                cbHighMagnification.Enabled = false;
                rBLowMagnification.Checked = false;
                NowMagni = cbHighMagnification.SelectedIndex;
                ChangeLensMagnificationWait(NowMagni);
                tBLeftTopMaskLight.Value = _AlignC.LeftBrightness[NowMagni];
                tBRightTopMaskLight.Value = _AlignC.RightBrightness[NowMagni];
                if (_recipe.LeftHighMaskMat != null)
                {
                    pbLTopMask.Image = _recipe.LeftHighMaskMat.ToBitmap();
                }
                else { pbLTopMask.Image = null; }

                if (_recipe.RightHighMaskMat != null)
                {
                    pbRTopMask.Image = _recipe.RightHighMaskMat.ToBitmap();
                }
                else { pbRTopMask.Image = null; }

                cbLTopMaskAlgo.SelectedIndex = (int)_AlignC.LHMaskAlgorithm;
                cbRTopMaskAlgo.SelectedIndex = (int)_AlignC.RHMaskAlgorithm;

                Thread.Sleep(250);
                Update();

                if (GV.UserLevel != GV.User.Operator)
                    cbHighMagnification.Enabled = true;
                else
                    cbHighMagnification.Enabled = false;

                Application.DoEvents();

                rBHighMagnification.Enabled = true;
                rBLowMagnification.Enabled = true;
                CheckParam();
            }
        }

        private void rBLowMagnification_Click(object sender, EventArgs e)
        {
            if (rBLowMagnification.Checked)
            {
                cbLowMagnification.Enabled = false;
                cbHighMagnification.Enabled = false;
                rBHighMagnification.Checked = false;
                NowMagni = cbLowMagnification.SelectedIndex;
                ChangeLensMagnificationWait(NowMagni);
                tBLeftTopMaskLight.Value = _AlignC.LeftBrightness[NowMagni];
                tBRightTopMaskLight.Value = _AlignC.RightBrightness[NowMagni];
                if (_recipe.LeftLowMaskMat != null)
                {
                    pbLTopMask.Image = _recipe.LeftLowMaskMat.ToBitmap();
                }
                else { pbLTopMask.Image = null; }
                if (_recipe.RightLowMaskMat != null)
                {
                    pbRTopMask.Image = _recipe.RightLowMaskMat.ToBitmap();
                }
                else { pbRTopMask.Image = null; }

                cbLTopMaskAlgo.SelectedIndex = (int)_AlignC.LLMaskAlgorithm;
                cbRTopMaskAlgo.SelectedIndex = (int)_AlignC.RLMaskAlgorithm;

                Update();
                Thread.Sleep(250);
                // gBMag.Enabled = true;
                if (GV.UserLevel != GV.User.Operator)
                    cbLowMagnification.Enabled = true;
                else
                    cbLowMagnification.Enabled = false;

                Application.DoEvents();

                rBLowMagnification.Enabled = true;
                rBHighMagnification.Enabled = true;
                CheckParam();
            }
        }

        private void btOpen_Click(object sender, EventArgs e)
        {
            if (GV.thCCDPad == null)
            {
                GV.thCCDPad = new Thread(delegate ()
                {
                    GV.CCDPadForm = new CCDPad();
                    GV.CCDPadForm.Show();
                    System.Windows.Threading.Dispatcher.Run();
                });

                // StopCheckPLc = true;
                GV.thCCDPad.SetApartmentState(ApartmentState.STA); // needs to be STA or throws exception
                GV.thCCDPad.Start();
            }
            else
            {
                Invoke(new Action(() => GV.CCDPadForm.BringToFront()));
            }
        }

        private void btFindLMaskCenter_Click(object sender, EventArgs e)
        {
            Mat m = new Mat(GV.LeftUpCam.Grab(), skLeft.GetMaskRect());

            OpenCvSharp.Point f = GV.matcherLLM.FindPatternCenter1(m);

            if ((f.X > 0) && (f.X < 2592) && (f.Y > 0) && (f.Y < 1944))
            {
                int x0 = f.X - m.Width / 2;
                int y0 = f.Y - m.Height / 2;

                skLeft.MaskMove(x0, y0);
            }
        }

        //private void btFindLBMaskCenter_Click(object sender, EventArgs e)
        //{
        //    Mat m;
        //    Rect roi;

        //    if (_recipe.LeftLowMaskMat.Empty())
        //    {
        //        MessageBox.Show("請先學習光罩圖型，再找中心!!", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return;
        //    }
        //    m = _recipe.LeftLowMaskMat;
        //    DialogFindCenter FindCenter = new DialogFindCenter()
        //    {
        //        img = m.Clone(),
        //        iMaskWaferRadio = 1.2,
        //        imgM = new GrayImage(_recipe.LeftLowMaskMask)
        //    };

        //    if (FindCenter.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //    {
        //        roi = FindCenter.rect;
        //    }
        //    else
        //    {
        //        return;
        //    }
        //    Mat newm = new Mat(m, roi);
        //    pbLBackMask.Image = newm.ToBitmap();

        //    if (MessageBox.Show(GV.Dlang.strbtnSaveLLM, GV.Dlang.strSureSaveLLM, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
        //    {
        //        pbLBackMask.Image = m.ToBitmap();
        //        return;
        //    }
        //    _recipe.SetLeftLowMaskMat(newm);
        //    GrayImage editedMask = FindCenter.imgM;
        //    GrayImage d = CropGrayImage(editedMask, roi);
        //    _recipe.LeftLowMaskMask = new GrayImage(d);
        //    _AlignC.LLMaskAlgorithm = Algorithm(cbLBackMaskAlgo.SelectedIndex);
        //    GV.matcherLLM.LearnWithAlgo(newm, d, _AlignC.LLMaskAlgorithm);
        //    _AlignC.LastModifyTime = DateTime.Now;
        //    GM.WriteRecipeXml(EditRecipe);
        //}
        private void btFindLBMaskCenter_Click(object sender, EventArgs e)
        {
            Mat m;
            OpenCvSharp.Rect roi;
            GV.TickCount = 0;

            // ✅ 1. 檢查是否已學習影像
            if (_recipe.LeftLowMaskMat.Empty())
            {
                MessageBox.Show("請先學習光罩圖型,再找中心!!", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            m = _recipe.LeftLowMaskMat;

            // ✅ 2. 開啟 DialogFindCenter
            GrayImage currentMask = _recipe.LeftLowMaskMask;

            DialogFindCenter FindCenter = new DialogFindCenter()
            {
                img = m.Clone(),
                iMaskWaferRadio = 1.2,
                imgM = currentMask != null ? new GrayImage(currentMask) : new GrayImage(m.Width, m.Height)
            };

            if (FindCenter.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            // ✅ 3. 使用 IsMultiCrossMode 判斷模式
            OpenCvSharp.Rect resultRect = FindCenter.rect;
            Mat imageToSave;
            GrayImage maskToSave;
            bool isMultiCross = FindCenter.IsMultiCrossMode;  // ← 直接使用標記

            // ✅ 關鍵修正:使用臨時變數儲存偏移量
            int offsetX = 0;
            int offsetY = 0;

            if (isMultiCross)
            {
                // ✅ 多十字模式:保留完整影像
                imageToSave = m.Clone();

                GrayImage editedMask = FindCenter.imgM;
                maskToSave = new GrayImage(editedMask);

                // ⚠️ 從 resultRect 取出偏移量
                offsetX = resultRect.Width;   // 這是 DialogFindCenter 計算的偏移量
                offsetY = resultRect.Height;

                // 預覽:顯示完整影像 + 中心點標記
                Mat previewMat = m.Clone();
                Cv2.CvtColor(previewMat, previewMat, ColorConversionCodes.GRAY2BGR);
                Cv2.Circle(previewMat, new OpenCvSharp.Point(resultRect.X, resultRect.Y), 5, new Scalar(0, 255, 0), -1);
                Cv2.Line(previewMat, new OpenCvSharp.Point(resultRect.X - 20, resultRect.Y),
                         new OpenCvSharp.Point(resultRect.X + 20, resultRect.Y), new Scalar(0, 255, 0), 2);
                Cv2.Line(previewMat, new OpenCvSharp.Point(resultRect.X, resultRect.Y - 20),
                         new OpenCvSharp.Point(resultRect.X, resultRect.Y + 20), new Scalar(0, 255, 0), 2);
                pbLBackMask.Image = previewMat.ToBitmap();
                previewMat.Dispose();
            }
            else
            {
                // ✅ 單十字模式:裁切影像
                imageToSave = new Mat(m, resultRect);

                GrayImage editedMask = FindCenter.imgM;
                maskToSave = CropGrayImage(editedMask, resultRect);

                // 清除偏移量
                offsetX = 0;
                offsetY = 0;

                pbLBackMask.Image = imageToSave.ToBitmap();
            }

            // ✅ 4. 確認是否儲存
            string confirmMsg = GV.Dlang.strLBottomMask;
            string confirmTitle = GV.Dlang.strLBottomMaskSave;

            if (MessageBox.Show(confirmMsg, confirmTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                pbLBackMask.Image = m.ToBitmap();
                return;
            }

            // ✅ 5. 儲存到 Recipe
            _recipe.SetLeftLowMaskMat(imageToSave);
            _recipe.LeftLowMaskMask = maskToSave;
            _AlignC.LLMaskAlgorithm = Algorithm(cbLBackMaskAlgo.SelectedIndex);

            // ✅ 儲存偏移量到 AlignC (會一起被寫入 Recipe XML)
            _AlignC.LLMaskOffsetX = offsetX;
            _AlignC.LLMaskOffsetY = offsetY;

            if (GV.AppSettingParm.Emulation != true && GV.matcherLLM != null)
            {
                GV.matcherLLM.LearnWithAlgo(imageToSave, maskToSave, _AlignC.LLMaskAlgorithm);
            }

            // ✅ 6. 更新 Mask 調整值
            if (!BtLMaskAdjSave.Visible)
            {
                if (GV.Plc.ReadData16(13028) == 0)
                {
                    _AlignC.LMaskAdjX = GV.AppSettingParm.LxShift4;
                    _AlignC.LMaskAdjY = GV.AppSettingParm.LyShift4;
                }
                else
                {
                    _AlignC.LMaskAdjX = GV.AppSettingParm.LxShift6;
                    _AlignC.LMaskAdjY = GV.AppSettingParm.LyShift6;
                }
            }

            // ✅ 7. 更新最後修改時間
            _AlignC.LastModifyTime = DateTime.Now;

            // ✅ 8. 【關鍵!】將 Recipe 寫入 XML,確保偏移量持久化儲存
            GM.WriteRecipeXml(EditRecipe);

            // ✅ 9. 根據模式顯示不同訊息
            if (isMultiCross)
            {
                //Mat finalPreview = imageToSave.Clone();
                //Cv2.CvtColor(finalPreview, finalPreview, ColorConversionCodes.GRAY2BGR);
                //Cv2.Circle(finalPreview, new OpenCvSharp.Point(resultRect.X, resultRect.Y), 5, new Scalar(0, 255, 0), -1);
                //pbLBackMask.Image = finalPreview.ToBitmap();
                //finalPreview.Dispose();

                MessageBox.Show($"光罩中心已更新並儲存 (多十字模式)\n偏移量: ({offsetX}, {offsetY})",
                                "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                pbLBackMask.Image = imageToSave.ToBitmap();
                MessageBox.Show("光罩中心已更新並儲存 (單十字模式)", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //private void BtFindLWaferCenter_Click(object sender, EventArgs e)
        //{
        //    Mat m;
        //    Rect roi;

        //    if (_recipe.LeftHighWaferMat.Empty())
        //    {
        //        MessageBox.Show("請先學習光罩圖型，再找中心!!", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return;
        //    }
        //    m = _recipe.LeftHighWaferMat;

        //    DialogFindCenter FindCenter = new DialogFindCenter()
        //    {
        //        img = m.Clone(),
        //        iMaskWaferRadio = 0.8,
        //        imgM = new GrayImage(_recipe.LeftHighWaferMask)
        //    };

        //    if (FindCenter.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //    {
        //        roi = FindCenter.rect;
        //    }
        //    else
        //    {
        //        return;
        //    }
        //    Mat newm = new Mat(m, roi);
        //    pbLWafer.Image = newm.ToBitmap();

        //    if (MessageBox.Show(GV.Dlang.strbtnSaveLLW, GV.Dlang.strSureSaveLLW, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
        //    {
        //        pbLWafer.Image = m.ToBitmap();
        //        return;
        //    }

        //    _recipe.SetLeftHighWaferMat(newm);
        //    GrayImage editedMask = FindCenter.imgM;
        //    GrayImage d = CropGrayImage(editedMask, roi);
        //    _recipe.LeftHighWaferMask = new GrayImage(d);
        //    _AlignC.LHWaferAlgorithm = Algorithm(cbLWaferAlgo.SelectedIndex);
        //    GV.matcherLHW.LearnWithAlgo(newm, d, _AlignC.LHWaferAlgorithm);
        //    _AlignC.LastModifyTime = DateTime.Now;
        //    GM.WriteRecipeXml(EditRecipe);
        //}
        private void BtFindLWaferCenter_Click(object sender, EventArgs e)
        {
            Mat m;
            OpenCvSharp.Rect roi;
            GV.TickCount = 0;

            // ✅ 1. 檢查是否已學習影像
            if (_recipe.LeftHighWaferMat.Empty())
            {
                MessageBox.Show("請先學習晶圓圖型,再找中心!!", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            m = _recipe.LeftHighWaferMat;

            // ✅ 2. 開啟 DialogFindCenter
            GrayImage currentMask = _recipe.LeftHighWaferMask;

            DialogFindCenter FindCenter = new DialogFindCenter()
            {
                img = m.Clone(),
                iMaskWaferRadio = 0.8,  // Wafer 用 0.8
                iBlockSize = _AlignC.LHWaferBlockSize,
                iBitwiseNot = _AlignC.LHBitwiseNot,
                iAlgo = (int)_AlignC.LHWaferAlgorithm,
                imgM = currentMask != null ? new GrayImage(currentMask) : new GrayImage(m.Width, m.Height)
            };

            if (FindCenter.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            // ✅ 3. 使用 IsMultiCrossMode 判斷模式
            OpenCvSharp.Rect resultRect = FindCenter.rect;
            Mat imageToSave;
            GrayImage maskToSave;
            bool isMultiCross = FindCenter.IsMultiCrossMode;

            // ✅ 關鍵修正:使用臨時變數儲存偏移量
            int offsetX = 0;
            int offsetY = 0;

            if (isMultiCross)
            {
                // ✅ 多十字模式:保留完整影像
                imageToSave = m.Clone();

                GrayImage editedMask = FindCenter.imgM;
                maskToSave = new GrayImage(editedMask);

                // ⚠️ 從 resultRect 取出偏移量
                offsetX = resultRect.Width;   // 這是 DialogFindCenter 計算的偏移量
                offsetY = resultRect.Height;

                // 預覽:顯示完整影像 + 中心點標記
                Mat previewMat = m.Clone();
                Cv2.CvtColor(previewMat, previewMat, ColorConversionCodes.GRAY2BGR);
                Cv2.Circle(previewMat, new OpenCvSharp.Point(resultRect.X, resultRect.Y), 5, new Scalar(0, 255, 0), -1);
                Cv2.Line(previewMat, new OpenCvSharp.Point(resultRect.X - 20, resultRect.Y),
                         new OpenCvSharp.Point(resultRect.X + 20, resultRect.Y), new Scalar(0, 255, 0), 2);
                Cv2.Line(previewMat, new OpenCvSharp.Point(resultRect.X, resultRect.Y - 20),
                         new OpenCvSharp.Point(resultRect.X, resultRect.Y + 20), new Scalar(0, 255, 0), 2);
                pbLWafer.Image = previewMat.ToBitmap();
                previewMat.Dispose();
            }
            else
            {
                // ✅ 單十字模式:裁切影像
                imageToSave = new Mat(m, resultRect);

                GrayImage editedMask = FindCenter.imgM;
                maskToSave = CropGrayImage(editedMask, resultRect);

                // 清除偏移量
                offsetX = 0;
                offsetY = 0;

                pbLWafer.Image = imageToSave.ToBitmap();
            }

            // ✅ 4. 確認是否儲存
            string confirmMsg = GV.Dlang.strbtnSaveLLW;
            string confirmTitle = GV.Dlang.strSureSaveLLW;

            if (MessageBox.Show(confirmMsg, confirmTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                pbLWafer.Image = m.ToBitmap();
                return;
            }

            // ✅ 5. 儲存到 Recipe
            _recipe.SetLeftHighWaferMat(imageToSave);
            _recipe.LeftHighWaferMask = maskToSave;
            _AlignC.LHWaferAlgorithm = Algorithm(cbLWaferAlgo.SelectedIndex);

            // ✅ 儲存偏移量到 AlignC (會一起被寫入 Recipe XML)
            _AlignC.LHWaferOffsetX = offsetX;
            _AlignC.LHWaferOffsetY = offsetY;

            if (GV.AppSettingParm.Emulation != true && GV.matcherLHW != null)
            {
                GV.matcherLHW.LearnWithAlgo(imageToSave, maskToSave, _AlignC.LHWaferAlgorithm);
            }

            // ✅ 6. 更新最後修改時間
            _AlignC.LastModifyTime = DateTime.Now;

            // ✅ 7. 【關鍵!】將 Recipe 寫入 XML,確保偏移量持久化儲存
            GM.WriteRecipeXml(EditRecipe);

            // ✅ 8. 根據模式顯示不同訊息
            if (isMultiCross)
            {
                //Mat finalPreview = imageToSave.Clone();
                //Cv2.CvtColor(finalPreview, finalPreview, ColorConversionCodes.GRAY2BGR);
                //Cv2.Circle(finalPreview, new OpenCvSharp.Point(resultRect.X, resultRect.Y), 5, new Scalar(0, 255, 0), -1);
                //pbLWafer.Image = finalPreview.ToBitmap();
                //finalPreview.Dispose();

                MessageBox.Show($"晶圓中心已更新並儲存 (多十字模式)\n偏移量: ({offsetX}, {offsetY})",
                                "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                pbLWafer.Image = imageToSave.ToBitmap();
                MessageBox.Show("晶圓中心已更新並儲存 (單十字模式)", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //private void BtFindRWaferCenter_Click(object sender, EventArgs e)
        //{
        //    Rect roi;

        //    if (_recipe.RightHighWaferMat.Empty())
        //    {
        //        MessageBox.Show("請先學習光罩圖型，再找中心!!", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return;
        //    }
        //    Mat m = _recipe.RightHighWaferMat;

        //    DialogFindCenter FindCenter = new DialogFindCenter()
        //    {
        //        img = m.Clone(),
        //        iMaskWaferRadio = 0.8,
        //        imgM = new GrayImage(_recipe.RightHighWaferMask)
        //    };

        //    if (FindCenter.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //    {
        //        roi = FindCenter.rect;
        //    }
        //    else
        //    {
        //        return;
        //    }
        //    Mat newm = new Mat(m, roi);
        //    pbRWafer.Image = newm.ToBitmap();

        //    if (MessageBox.Show(GV.Dlang.strbtnSaveRLW, GV.Dlang.strSureSaveRLW, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
        //    {
        //        pbRWafer.Image = m.ToBitmap();
        //        return;
        //    }
        //    _recipe.SetRightHighWaferMat(newm);
        //    GrayImage grayImage = FindCenter.imgM;
        //    GrayImage d = CropGrayImage(grayImage, roi);
        //    _recipe.RightHighWaferMask = new GrayImage(d);
        //    _AlignC.RHWaferAlgorithm = Algorithm(cbRWaferAlgo.SelectedIndex);
        //    GV.matcherRHW.LearnWithAlgo(newm, d, _AlignC.RHWaferAlgorithm);
        //    _AlignC.LastModifyTime = DateTime.Now;
        //    GM.WriteRecipeXml(EditRecipe);
        //}
        private void BtFindRWaferCenter_Click(object sender, EventArgs e)
        {
            OpenCvSharp.Rect roi;
            GV.TickCount = 0;

            // ✅ 1. 檢查是否已學習影像
            if (_recipe.RightHighWaferMat.Empty())
            {
                MessageBox.Show("請先學習晶圓圖型，再找中心!!", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Mat m = _recipe.RightHighWaferMat;

            // ✅ 2. 開啟 DialogFindCenter
            GrayImage currentMask = _recipe.RightHighWaferMask;

            DialogFindCenter FindCenter = new DialogFindCenter()
            {
                img = m.Clone(),
                iMaskWaferRadio = 0.8,  // Wafer 用 0.8
                iBlockSize = _AlignC.RHWaferBlockSize,
                iBitwiseNot = _AlignC.RHBitwiseNot,
                iAlgo = (int)_AlignC.RHWaferAlgorithm,
                imgM = currentMask != null ? new GrayImage(currentMask) : new GrayImage(m.Width, m.Height)
            };

            if (FindCenter.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            // ✅ 3. 使用 IsMultiCrossMode 判斷模式
            OpenCvSharp.Rect resultRect = FindCenter.rect;
            Mat imageToSave;
            GrayImage maskToSave;
            bool isMultiCross = FindCenter.IsMultiCrossMode;

            // ✅ 關鍵修正:使用臨時變數儲存偏移量
            int offsetX = 0;
            int offsetY = 0;

            if (isMultiCross)
            {
                // ✅ 多十字模式:保留完整影像
                imageToSave = m.Clone();

                GrayImage editedMask = FindCenter.imgM;
                maskToSave = new GrayImage(editedMask);

                // ⚠️ 從 resultRect 取出偏移量
                offsetX = resultRect.Width;   // 這是 DialogFindCenter 計算的偏移量
                offsetY = resultRect.Height;

                // 預覽:顯示完整影像 + 中心點標記
                Mat previewMat = m.Clone();
                Cv2.CvtColor(previewMat, previewMat, ColorConversionCodes.GRAY2BGR);
                Cv2.Circle(previewMat, new OpenCvSharp.Point(resultRect.X, resultRect.Y), 5, new Scalar(0, 255, 0), -1);
                Cv2.Line(previewMat, new OpenCvSharp.Point(resultRect.X - 20, resultRect.Y),
                         new OpenCvSharp.Point(resultRect.X + 20, resultRect.Y), new Scalar(0, 255, 0), 2);
                Cv2.Line(previewMat, new OpenCvSharp.Point(resultRect.X, resultRect.Y - 20),
                         new OpenCvSharp.Point(resultRect.X, resultRect.Y + 20), new Scalar(0, 255, 0), 2);
                pbRWafer.Image = previewMat.ToBitmap();
                previewMat.Dispose();
            }
            else
            {
                // ✅ 單十字模式:裁切影像
                imageToSave = new Mat(m, resultRect);

                GrayImage editedMask = FindCenter.imgM;
                maskToSave = CropGrayImage(editedMask, resultRect);

                // 清除偏移量
                offsetX = 0;
                offsetY = 0;

                pbRWafer.Image = imageToSave.ToBitmap();
            }

            // ✅ 4. 確認是否儲存
            if (MessageBox.Show(GV.Dlang.strbtnSaveRLW, GV.Dlang.strSureSaveRLW, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                pbRWafer.Image = m.ToBitmap();
                return;
            }

            // ✅ 5. 儲存到 Recipe
            _recipe.SetRightHighWaferMat(imageToSave);
            _recipe.RightHighWaferMask = maskToSave;
            _AlignC.RHWaferAlgorithm = Algorithm(cbRWaferAlgo.SelectedIndex);

            // ✅ 儲存偏移量到 AlignC (會一起被寫入 Recipe XML)
            _AlignC.RHWaferOffsetX = offsetX;
            _AlignC.RHWaferOffsetY = offsetY;

            if (GV.AppSettingParm.Emulation != true && GV.matcherRHW != null)
            {
                GV.matcherRHW.LearnWithAlgo(imageToSave, maskToSave, _AlignC.RHWaferAlgorithm);
            }

            // ✅ 6. 更新最後修改時間
            _AlignC.LastModifyTime = DateTime.Now;

            // ✅ 7. 【關鍵!】將 Recipe 寫入 XML,確保偏移量持久化儲存
            GM.WriteRecipeXml(EditRecipe);

            // ✅ 8. 根據模式顯示不同訊息
            if (isMultiCross)
            {
                //Mat finalPreview = imageToSave.Clone();
                //Cv2.CvtColor(finalPreview, finalPreview, ColorConversionCodes.GRAY2BGR);
                //Cv2.Circle(finalPreview, new OpenCvSharp.Point(resultRect.X, resultRect.Y), 5, new Scalar(0, 255, 0), -1);
                //pbRWafer.Image = finalPreview.ToBitmap();
                //finalPreview.Dispose();

                MessageBox.Show($"晶圓中心已更新並儲存 (多十字模式)\n偏移量: ({offsetX}, {offsetY})",
                                "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                pbRWafer.Image = imageToSave.ToBitmap();
                MessageBox.Show("晶圓中心已更新並儲存 (單十字模式)", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //private void BtFindRBMaskCenter_Click(object sender, EventArgs e)
        //{
        //    Mat m;
        //    Rect roi;
        //    GV.TickCount = 0;

        //    if (_recipe.RightLowMaskMat.Empty())
        //    {
        //        MessageBox.Show("請先學習光罩圖型，再找中心!!", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return;
        //    }
        //    m = _recipe.RightLowMaskMat;

        //    DialogFindCenter FindCenter = new DialogFindCenter()
        //    {
        //        img = m.Clone(),
        //        iMaskWaferRadio = 1.2,
        //        imgM = new GrayImage(_recipe.RightLowMaskMask)
        //    };

        //    if (FindCenter.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //    {
        //        roi = FindCenter.rect;
        //    }
        //    else
        //    {
        //        return;
        //    }
        //    Mat newm = new Mat(m, roi);
        //    pbRBackMask.Image = newm.ToBitmap();

        //    if (MessageBox.Show(GV.Dlang.strbtnSaveRLM, GV.Dlang.strSureSaveRLM, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
        //    {
        //        pbRBackMask.Image = m.ToBitmap();
        //        return;
        //    }
        //    _recipe.SetRightLowMaskMat(newm);
        //    GrayImage editedMask = FindCenter.imgM;
        //    GrayImage d = CropGrayImage(editedMask, roi);
        //    _recipe.RightLowMaskMask = new GrayImage(d);
        //    _AlignC.RLMaskAlgorithm = Algorithm(cbRBackMaskAlgo.SelectedIndex);
        //    GV.matcherRLM.LearnWithAlgo(newm, d, _AlignC.RLMaskAlgorithm);
        //    _AlignC.LastModifyTime = DateTime.Now;
        //    GM.WriteRecipeXml(EditRecipe);
        //}
        private void BtFindRBMaskCenter_Click(object sender, EventArgs e)
        {
            Mat m;
            OpenCvSharp.Rect roi;
            GV.TickCount = 0;

            // ✅ 1. 檢查是否已學習影像
            if (_recipe.RightLowMaskMat.Empty())
            {
                MessageBox.Show("請先學習光罩圖型，再找中心!!", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            m = _recipe.RightLowMaskMat;

            // ✅ 2. 開啟 DialogFindCenter
            GrayImage currentMask = _recipe.RightLowMaskMask;

            DialogFindCenter FindCenter = new DialogFindCenter()
            {
                img = m.Clone(),
                iMaskWaferRadio = 1.2,  // Mask 用 1.2
                imgM = currentMask != null ? new GrayImage(currentMask) : new GrayImage(m.Width, m.Height)
            };

            if (FindCenter.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            // ✅ 3. 使用 IsMultiCrossMode 判斷模式
            OpenCvSharp.Rect resultRect = FindCenter.rect;
            Mat imageToSave;
            GrayImage maskToSave;
            bool isMultiCross = FindCenter.IsMultiCrossMode;

            // ✅ 關鍵修正:使用臨時變數儲存偏移量
            int offsetX = 0;
            int offsetY = 0;

            if (isMultiCross)
            {
                // ✅ 多十字模式:保留完整影像
                imageToSave = m.Clone();

                GrayImage editedMask = FindCenter.imgM;
                maskToSave = new GrayImage(editedMask);

                // ⚠️ 從 resultRect 取出偏移量
                offsetX = resultRect.Width;   // 這是 DialogFindCenter 計算的偏移量
                offsetY = resultRect.Height;

                // 預覽:顯示完整影像 + 中心點標記
                Mat previewMat = m.Clone();
                Cv2.CvtColor(previewMat, previewMat, ColorConversionCodes.GRAY2BGR);
                Cv2.Circle(previewMat, new OpenCvSharp.Point(resultRect.X, resultRect.Y), 5, new Scalar(0, 255, 0), -1);
                Cv2.Line(previewMat, new OpenCvSharp.Point(resultRect.X - 20, resultRect.Y),
                         new OpenCvSharp.Point(resultRect.X + 20, resultRect.Y), new Scalar(0, 255, 0), 2);
                Cv2.Line(previewMat, new OpenCvSharp.Point(resultRect.X, resultRect.Y - 20),
                         new OpenCvSharp.Point(resultRect.X, resultRect.Y + 20), new Scalar(0, 255, 0), 2);
                pbRBackMask.Image = previewMat.ToBitmap();
                previewMat.Dispose();
            }
            else
            {
                // ✅ 單十字模式:裁切影像
                imageToSave = new Mat(m, resultRect);

                GrayImage editedMask = FindCenter.imgM;
                maskToSave = CropGrayImage(editedMask, resultRect);

                // 清除偏移量
                offsetX = 0;
                offsetY = 0;

                pbRBackMask.Image = imageToSave.ToBitmap();
            }

            // ✅ 4. 確認是否儲存
            if (MessageBox.Show(GV.Dlang.strbtnSaveRLM, GV.Dlang.strSureSaveRLM, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                pbRBackMask.Image = m.ToBitmap();
                return;
            }

            // ✅ 5. 儲存到 Recipe
            _recipe.SetRightLowMaskMat(imageToSave);
            _recipe.RightLowMaskMask = new GrayImage(maskToSave);
            _AlignC.RLMaskAlgorithm = Algorithm(cbRBackMaskAlgo.SelectedIndex);

            // ✅ 儲存偏移量到 AlignC (會一起被寫入 Recipe XML)
            _AlignC.RLMaskOffsetX = offsetX;
            _AlignC.RLMaskOffsetY = offsetY;

            if (GV.AppSettingParm.Emulation != true && GV.matcherRLM != null)
            {
                GV.matcherRLM.LearnWithAlgo(imageToSave, maskToSave, _AlignC.RLMaskAlgorithm);
            }

            // ✅ 6. 更新 Mask 調整值
            if (!BtRMaskAdjSave.Visible)
            {
                if (GV.Plc.ReadData16(13028) == 0)
                {
                    _AlignC.RMaskAdjX = GV.AppSettingParm.RxShift4;
                    _AlignC.RMaskAdjY = GV.AppSettingParm.RyShift4;
                }
                else
                {
                    _AlignC.RMaskAdjX = GV.AppSettingParm.RxShift6;
                    _AlignC.RMaskAdjY = GV.AppSettingParm.RyShift6;
                }
            }

            // ✅ 7. 更新最後修改時間
            _AlignC.LastModifyTime = DateTime.Now;

            // ✅ 8. 【關鍵!】將 Recipe 寫入 XML,確保偏移量持久化儲存
            GM.WriteRecipeXml(EditRecipe);

            // ✅ 9. 根據模式顯示不同訊息
            if (isMultiCross)
            {
                //Mat finalPreview = imageToSave.Clone();
                //Cv2.CvtColor(finalPreview, finalPreview, ColorConversionCodes.GRAY2BGR);
                //Cv2.Circle(finalPreview, new OpenCvSharp.Point(resultRect.X, resultRect.Y), 5, new Scalar(0, 255, 0), -1);
                //pbRBackMask.Image = finalPreview.ToBitmap();
                //finalPreview.Dispose();

                MessageBox.Show($"光罩中心已更新並儲存 (多十字模式)\n偏移量: ({offsetX}, {offsetY})",
                                "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                pbRBackMask.Image = imageToSave.ToBitmap();
                MessageBox.Show("光罩中心已更新並儲存 (單十字模式)", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtFindRMaskCenter_Click(object sender, EventArgs e)
        {
            Mat m = new Mat(GV.RightBackCam.Grab(), skRight.GetMaskRect());

            OpenCvSharp.Point f = GV.matcherRLM.FindPatternCenter1(m);

            if ((f.X > 0) && (f.X < 2592) && (f.Y > 0) && (f.Y < 1944))
            {
                int x0 = (int)f.X - m.Width / 2;
                int y0 = (int)f.Y - m.Height / 2;

                skRight.MaskMove(x0, y0);
            }
        }

        //private void btReportLocation_Click(object sender, EventArgs e)
        //{
        //    skLeft.CanTrackPattern = true;
        //    skRight.CanTrackPattern = true;

        //    // skLeft.MaskMp = BAlignMatcher.Match(_recipe.LeftBackCam.Grab(), _recipe.LeftLowMaskMat);
        //    skLeft.WaferMp = GV.matcher.Match(_recipe.LeftBackCam.Grab(), _recipe.LeftHighWaferMat);
        //    // skRight.MaskMp = BAlignMatcher.Match(GV.RightUpCam.Grab(), GV.RightLowMaskMat);
        //    skRight.WaferMp = GV.matcher.Match(GV.RightBackCam.Grab(), GV.RightHighWaferMat);

        //    string msg1 = String.Format("Wafer Location X = {0}, Y = {1}, Score = {2}", skLeft.WaferMp.X, skLeft.WaferMp.Y, skLeft.WaferMp.Score) +
        //        "\r\n" + String.Format("Mask Location X = {0}, Y = {1}, Score = {2}", LeftMaskMp.X, LeftMaskMp.Y, LeftMaskMp.Score);
        //    lbLeftXYM.Text = msg1;

        //    string msg2 = String.Format("Wafer Location X = {0}, Y = {1}, Score = {2}", skRight.WaferMp.X, skRight.WaferMp.Y, skRight.WaferMp.Score) +
        //    "\r\n" + String.Format("Mask Location X = {0}, Y = {1}, Score = {2}", RightMaskMp.X, RightMaskMp.Y, RightMaskMp.Score);
        //    lbRightXYM.Text = msg2;

        //    GV.HwndFormMain.writeStatus("Left : " + msg1);
        //    GV.HwndFormMain.writeStatus("Right : " + msg2);

        //}

        private void btLMaskSave_Click(object sender, EventArgs e)
        {
            GV.TickCount = 0;

            Mat m = new Mat(GV.LeftBackCam.Grab(), skLeft.GetMaskRect());
            GV.LeftCheckMat = m;
            pbLBackMask.Image = m.ToBitmap();
            Cv2.ImWrite(GetTemplateFileName("LCheck"), m);
        }

        private void btRMaskSave_Click(object sender, EventArgs e)
        {
            GV.TickCount = 0;

            Mat m = new Mat(GV.RightBackCam.Grab(), skRight.GetMaskRect());
            GV.RightCheckMat = m;
            pbRBackMask.Image = m.ToBitmap();
            Cv2.ImWrite(GetTemplateFileName("RCheck"), m);
        }

        private void bBTtMaskAlign_Click(object sender, EventArgs e)
        {
            MoveCameraBack(0);
        }

        public void deleteTemplateFile(string strf)
        {
            try
            {
                System.IO.File.Copy(GetTemplateFileName(strf), GetTemplateFileNameD(strf));
                System.IO.File.Delete(GetTemplateFileName(strf));
            }
            catch (Exception)
            {
            }
        }

        private void btBTAlignWafer_Click(object sender, EventArgs e)
        {
            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, true);

            ProductMatchPositions pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "back");

            pmps.LMaskMp.X = BLivePmps.LMaskMp.X;
            pmps.LMaskMp.Y = BLivePmps.LMaskMp.Y;
            pmps.LMaskMp.Score = BLivePmps.LMaskMp.Score;
            pmps.RMaskMp.X = BLivePmps.RMaskMp.X;
            pmps.RMaskMp.Y = BLivePmps.RMaskMp.Y;
            pmps.RMaskMp.Score = BLivePmps.RMaskMp.Score;

            // PointF center = new Point(GV.LeftBackCam.Grab().Cols / 2, GV.LeftBackCam.Grab().Rows / 2);

            double beforeMoveLShiftXum = (pmps.LWaferMp.X - pmps.LMaskMp.X) * GV.ZoomLensInfo.LeftDownUmPerPixelX;
            double beforeMoveLShiftYum = (pmps.LWaferMp.Y - pmps.LMaskMp.Y) * GV.ZoomLensInfo.LeftDownUmPerPixelY;
            double beforeMoveRShiftXum = (pmps.RWaferMp.X - pmps.RMaskMp.X) * GV.ZoomLensInfo.RightDownUmPerPixelX;
            double beforeMoveRShiftYum = (pmps.RWaferMp.Y - pmps.RMaskMp.Y) * GV.ZoomLensInfo.RightDownUmPerPixelY;
            // writeStatus("L shift(um) : (" + beforeMoveLShiftXum.ToString("F2") + " , " + beforeMoveLShiftYum.ToString("F2") + ")");
            // writeStatus("R shift(um) : (" + beforeMoveRShiftXum.ToString("F2") + " , " + beforeMoveRShiftYum.ToString("F2") + ")");
            GV.HwndFormMain.writeStatus(string.Format(GV.Dlang.strMessageLShiftum, beforeMoveLShiftXum.ToString("F2"), beforeMoveLShiftYum.ToString("F2")));
            GV.HwndFormMain.writeStatus(string.Format(GV.Dlang.strMessageRShiftum, beforeMoveRShiftXum.ToString("F2"), beforeMoveRShiftYum.ToString("F2")));

            if (pmps.LWaferMp.Score < _AlignC.WaferScore || pmps.RWaferMp.Score < _AlignC.WaferScore)
            {
                GV.Plc.SendAlignBackNG();
                iAlignOK = 0;
                GV.HwndFormMain.writeStatus(GV.Dlang.strErrorCantBottomWafer);
                return;
                // throw new MRException("Error : Can't find wafer template at high magnification");
            }

            //int[] motorSteps = CalcuteMaskWaferShiftAndReturnMotorStepsBack(pmps.LMaskMp, pmps.LWaferMp, pmps.RMaskMp, pmps.RWaferMp, pmps.PatternCenterDistance);

            int[] motorSteps = XyyMotorSteps(pmps.LMaskMp, pmps.LWaferMp, pmps.RMaskMp, pmps.RWaferMp);

            if (!GV.Plc.ReadMemory(GV.Plc.iUpCCDStart))
            {
                iAlignOK = 0;
                //iStartAlign = 0;
                return;
            }
            GV.Plc.AlignXyyTableMove(2, motorSteps[0], motorSteps[1], motorSteps[2]);

            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, false);
        }

        private bool MoveCameraBack(int iMask)
        {
            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, true);

            //ProductMatchPositions pmps = null;

            ProductMatchPositions pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "backmask");

            if ((pmps.LMaskMp.Score < _AlignC.MaskSocre) || (pmps.RMaskMp.Score < _AlignC.MaskSocre))
            {
                GV.HwndFormMain.writeStatus("Bottom L Mask / R Mask score : " + pmps.LMaskMp.Score.ToString("F2") + " , " + pmps.RMaskMp.Score.ToString("F2"));
                GV.Plc.WriteMemory(GV.Plc.iBackMaskNG, true);
                iAlignOK = 0;
                GV.HwndFormMain.writeStatus(GV.Dlang.strErrorCantBottomMask);
                return false;
            }

            PointF center = GV.DownCenter;

            double lOffsetXSteps = ((double)(center.X) - pmps.LMaskMp.X) * GV.ZoomLensInfo.LeftDownUmPerPixelX / GV.ZoomLensInfo.LeftDownUmPerStepX;
            double lOffsetYSteps = ((double)(center.Y) - pmps.LMaskMp.Y) * GV.ZoomLensInfo.LeftDownUmPerPixelY / GV.ZoomLensInfo.LeftDownUmPerStepY;
            double rOffsetXSteps = ((double)(center.X) - pmps.RMaskMp.X) * GV.ZoomLensInfo.RightDownUmPerPixelX / GV.ZoomLensInfo.RightDownUmPerStepX;
            double rOffsetYSteps = ((double)(center.Y) - pmps.RMaskMp.Y) * GV.ZoomLensInfo.RightDownUmPerPixelY / GV.ZoomLensInfo.RightDownUmPerStepY;

            GV.Plc.AlignDownCameraMove(1, -lOffsetXSteps, rOffsetXSteps, -lOffsetYSteps, -rOffsetYSteps);

            Thread.Sleep(250);

            pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "backmask");

            lOffsetXSteps = ((double)(center.X) - pmps.LMaskMp.X) * GV.ZoomLensInfo.LeftDownUmPerPixelX / GV.ZoomLensInfo.LeftDownUmPerStepX;
            lOffsetYSteps = ((double)(center.Y) - pmps.LMaskMp.Y) * GV.ZoomLensInfo.LeftDownUmPerPixelY / GV.ZoomLensInfo.LeftDownUmPerStepY;
            rOffsetXSteps = ((double)(center.X) - pmps.RMaskMp.X) * GV.ZoomLensInfo.RightDownUmPerPixelX / GV.ZoomLensInfo.RightDownUmPerStepX;
            rOffsetYSteps = ((double)(center.Y) - pmps.RMaskMp.Y) * GV.ZoomLensInfo.RightDownUmPerPixelY / GV.ZoomLensInfo.RightDownUmPerStepY;

            GV.Plc.AlignDownCameraMove(0, -lOffsetXSteps, rOffsetXSteps, -lOffsetYSteps, -rOffsetYSteps);

            Thread.Sleep(250);

            pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "backmask");

            if ((pmps.LMaskMp.Score < _AlignC.MaskSocre) || (pmps.RMaskMp.Score < _AlignC.MaskSocre))
            {
                GV.HwndFormMain.writeStatus("Bottom L Mask / R Mask score : " + pmps.LMaskMp.Score.ToString("F2") + " , " + pmps.RMaskMp.Score.ToString("F2"));
                GV.Plc.WriteMemory(GV.Plc.iBackMaskNG, true);
                GV.HwndFormMain.writeStatus(GV.Dlang.strErrorCantBottomMask);
                iAlignOK = 0;
                return false;
            }

            lOffsetXSteps = ((double)(center.X) - pmps.LMaskMp.X) * GV.ZoomLensInfo.LeftDownUmPerPixelX;
            lOffsetYSteps = ((double)(center.Y) - pmps.LMaskMp.Y) * GV.ZoomLensInfo.LeftDownUmPerPixelY;
            rOffsetXSteps = ((double)(center.X) - pmps.RMaskMp.X) * GV.ZoomLensInfo.RightDownUmPerPixelX;
            rOffsetYSteps = ((double)(center.Y) - pmps.RMaskMp.Y) * GV.ZoomLensInfo.RightDownUmPerPixelY;

            double precisionX = (lOffsetXSteps + rOffsetXSteps) / 2.0f;
            double precisionY = (lOffsetYSteps + rOffsetYSteps) / 2.0f;
            double degree = Math.Atan((lOffsetYSteps - rOffsetYSteps) / ((pmps.PatternCenterDistance))) * 180 / Math.PI;
            double expansion = (rOffsetXSteps - lOffsetXSteps) / 2.0f;

            // if ((precisionX < _AlignC.XPrecision ) && (precisionY < _AlignC.YPrecision) && (expansion < _AlignC.MaxExpansion))
            if (!((pmps.LMaskMp.Score < _AlignC.MaskSocre) || (pmps.RMaskMp.Score < _AlignC.MaskSocre)))
            {
                Thread.Sleep(250);
                LeftMask = GV.LeftBackCam.Grab();
                RightMask = GV.RightBackCam.Grab();
                pmps = GetAllMatchPostion(LeftMask, RightMask, "backmask");

                //TopMaskPmps.LMaskMp.X = pmps.LMaskMp.X;
                //TopMaskPmps.LMaskMp.Y = pmps.LMaskMp.Y;
                //TopMaskPmps.LMaskMp.Score = pmps.LMaskMp.Score;
                //TopMaskPmps.RMaskMp.X = pmps.RMaskMp.X;
                //TopMaskPmps.RMaskMp.Y = pmps.RMaskMp.Y;
                //TopMaskPmps.RMaskMp.Score = pmps.RMaskMp.Score;
                BLivePmps.LMaskMp.X = pmps.LMaskMp.X;
                BLivePmps.LMaskMp.Y = pmps.LMaskMp.Y;
                BLivePmps.LMaskMp.Score = pmps.LMaskMp.Score;
                BLivePmps.RMaskMp.X = pmps.RMaskMp.X;
                BLivePmps.RMaskMp.Y = pmps.RMaskMp.Y;
                BLivePmps.RMaskMp.Score = pmps.RMaskMp.Score;
                string TopMaskCheck3 = string.Format(GV.Dlang.strMessageMaskShift, BLastPmps.LMaskMp.X, BLastPmps.RMaskMp.X, BLastPmps.LMaskMp.Y, BLastPmps.RMaskMp.Y);
                GM.WriteToStatusTextBox1(iAdmin, "0:" + TopMaskCheck3);

                // CountDistance();
                // ReportPLCStatus();
                // DownCCDMove();

                GV.Plc.GetLocation();

                _AlignC.PatternCenterDistanceUm = GV.ZoomLensInfo.LRBackCenterDistance
                    - (int)(GV.NowLocation[GV.Plc.iiDDownLeftX] * GV.ZoomLensInfo.LeftDownUmPerStepX) - (int)(GV.NowLocation[GV.Plc.iiDDownRightX] * GV.ZoomLensInfo.RightDownUmPerStepX);

                GV.Plc.WriteMemory(GV.Plc.iBackMaskOK, true);
            }
            else
            {
                GV.HwndFormMain.writeStatus("Bottom L Mask / R Mask score : " + pmps.LMaskMp.Score.ToString("F2") + " , " + pmps.RMaskMp.Score.ToString("F2"));
                // writeStatus("Bottom precisionX : {0} precisionY : {1} degree : {2} exp : {3}", precisionX.)
                GV.Plc.WriteMemory(GV.Plc.iBackMaskNG, true);
                GV.HwndFormMain.writeStatus(GV.Dlang.strErrorCantBottomMask);
                iAlignOK = 0;
                return false;
            }

            GV.LeftMaskMat = LeftMask.Clone();
            GV.RightMaskMat = RightMask.Clone();
            //if (MaskImage.Checked)
            //{
            //    skLeftAlign.SetShowMask(true, _AlignC.dLalpha, LeftMask);
            //    skRightAlign.SetShowMask(true, _AlignC.dRalpha, RightMask);
            //}
            Cv2.ImWrite(GetTemplateFileName("LeftMask"), LeftMask);
            Cv2.ImWrite(GetTemplateFileName("RightMask"), RightMask);
            GV.NowWaferMask = 3;

            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, false);

            return true;
        }

        private void btReadMaskMat_Click(object sender, EventArgs e)
        {
            LeftMask = Cv2.ImRead(GetTemplateFileName("LeftMask"), ImreadModes.Grayscale);
            RightMask = Cv2.ImRead(GetTemplateFileName("RightMask"), ImreadModes.Grayscale);

            ProductMatchPositions pmps = GetAllMatchPostion(LeftMask, RightMask, "backmask");

            BLivePmps.LMaskMp.X = pmps.LMaskMp.X;
            BLivePmps.LMaskMp.Y = pmps.LMaskMp.Y;
            BLivePmps.LMaskMp.Score = pmps.LMaskMp.Score;
            BLivePmps.RMaskMp.X = pmps.RMaskMp.X;
            BLivePmps.RMaskMp.Y = pmps.RMaskMp.Y;
            BLivePmps.RMaskMp.Score = pmps.RMaskMp.Score;

            GV.Plc.GetLocation();

            _AlignC.PatternCenterDistanceUm = GV.ZoomLensInfo.LRBackCenterDistance
                - (int)(GV.NowLocation[GV.Plc.iiDDownLeftX] * GV.ZoomLensInfo.LeftDownUmPerStepX)
                - (int)(GV.NowLocation[GV.Plc.iiDDownRightX] * GV.ZoomLensInfo.RightDownUmPerPixelX);

        }

        private void btNG_Click(object sender, EventArgs e)
        {
            GV.Plc.SendAlignNG();
        }

        private void btUpdate_Click(object sender, EventArgs e)
        {
            nUDXyyX.Value = GV.NowLocation[GV.Plc.iiDChuckX];
            nUDXyyY1.Value = GV.NowLocation[GV.Plc.iiDChuckY1];
            nUDXyyY2.Value = GV.NowLocation[GV.Plc.iiDChuckY2];
        }

        private void btGo_Click(object sender, EventArgs e)
        {
            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, true);

            Thread.Sleep(100);

            bool bRet = GV.Plc.XyyTableMoveAbs((int)nUDXyyX.Value, (int)nUDXyyY1.Value, (int)nUDXyyY2.Value);
            if (bRet == false)
            {
                string msg = string.Format("Xyy Roll back Error = Plc Error !!!");
                lbDebugMsg.Text = msg;
            }

            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, false);
        }

        private void btCapture_Click(object sender, EventArgs e)
        {
            MessageBox.Show(GM.SaveCaptureImageXyy(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab()) + GV.Dlang.strSave);

            ProductMatchPositions pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "back");

            OrgPmps.LWaferMp.X = pmps.LWaferMp.X;
            OrgPmps.LWaferMp.Y = pmps.LWaferMp.Y;
            OrgPmps.RWaferMp.X = pmps.RWaferMp.X;
            OrgPmps.RWaferMp.Y = pmps.RWaferMp.Y;

            GV.Plc.GetLocation();

            OrgMotors[0] = GV.NowLocation[GV.Plc.iiDChuckX];
            OrgMotors[1] = GV.NowLocation[GV.Plc.iiDChuckY1];
            OrgMotors[2] = GV.NowLocation[GV.Plc.iiDChuckY2];

        }

        //private async Task<ProductMatchPositions> GetAllMatchPositionAsync(Mat lSrc, Mat rSrc, string alignMagnification, CancellationToken ct = default)
        //{

        //    int i = 0;
        //    double patternCenterDistance = _AlignC.PatternCenterDistanceUm;
        //    while (i < 3)
        //    {
        //        lSrc.CopyTo(GMPBackLeft);
        //        lSrc.CopyTo(GMPBackRight);
        //        if (GV.AppSettingParm.Emulation != true)
        //        {
        //            if (alignMagnification == "low")
        //            {
        //                var t1 = GV.matcherLLM.MatMatchWithAlgoAsync(0, GMPBackLeft, GMPBacklMaskMp, _AlignC.LLMaskAlgorithm, ct);
        //                var t2 = GV.matcherRLM.MatMatchWithAlgoAsync(0, GMPBackRight, GMPBackrMaskMp, _AlignC.RLMaskAlgorithm, ct);
        //                var t3 = GV.matcherLLW.MatMatchWithAlgoAsync(0, GMPBackLeft, GMPBacklWaferMp, _AlignC.LLWaferAlgorithm, ct);
        //                var t4 = GV.matcherRLW.MatMatchWithAlgoAsync(0, GMPBackRight, GMPBackrWaferMp, _AlignC.RLWaferAlgorithm, ct);
        //                await Task.WhenAll(t1, t2, t3, t4).ConfigureAwait(false);
        //                GMPBacklWaferMp.X -= (float)(_AlignC.XOffset / GV.ZoomLensInfo.LeftUmPerPixelX[_AlignC.AlignLowMagnification]);
        //                GMPBackrWaferMp.X -= (float)(_AlignC.XOffset / GV.ZoomLensInfo.RightUmPerPixelX[_AlignC.AlignLowMagnification]);
        //                GMPBacklWaferMp.Y -= (float)(_AlignC.YLOffset / GV.ZoomLensInfo.LeftUmPerPixelY[_AlignC.AlignLowMagnification]);
        //                GMPBackrWaferMp.Y -= (float)(_AlignC.YROffset / GV.ZoomLensInfo.RightUmPerPixelY[_AlignC.AlignLowMagnification]);
        //            }
        //            else if (alignMagnification == "high")
        //            {
        //                var t1 = GV.matcherLHM.MatMatchWithAlgoAsync(0, GMPBackLeft, GMPBacklMaskMp, _AlignC.LHMaskAlgorithm, ct);
        //                var t2 = GV.matcherRHM.MatMatchWithAlgoAsync(0, GMPBackRight, GMPBackrMaskMp, _AlignC.RHMaskAlgorithm, ct);
        //                var t3 = GV.matcherLHW.MatMatchWithAlgoAsync(0, GMPBackLeft, GMPBacklWaferMp, _AlignC.LHWaferAlgorithm, ct);
        //                var t4 = GV.matcherRHW.MatMatchWithAlgoAsync(0, GMPBackRight, GMPBackrWaferMp, _AlignC.RHWaferAlgorithm, ct);
        //                await Task.WhenAll(t1, t2, t3, t4).ConfigureAwait(false);
        //                GMPBacklWaferMp.X -= (float)(_AlignC.XOffset / GV.ZoomLensInfo.LeftUmPerPixelX[_AlignC.AlignHighMagnification]);
        //                GMPBackrWaferMp.X -= (float)(_AlignC.XOffset / GV.ZoomLensInfo.RightUmPerPixelX[_AlignC.AlignHighMagnification]);
        //                GMPBacklWaferMp.Y -= (float)(_AlignC.YLOffset / GV.ZoomLensInfo.LeftUmPerPixelY[_AlignC.AlignHighMagnification]);
        //                GMPBackrWaferMp.Y -= (float)(_AlignC.YROffset / GV.ZoomLensInfo.RightUmPerPixelY[_AlignC.AlignHighMagnification]);
        //            }
        //            else if (alignMagnification == "back")
        //            {
        //                var t1 = GV.matcherLLW.MatMatchWithAlgoAsync(0, LeftMaskMat, GMPBacklMaskMp, _AlignC.LLWaferAlgorithm);
        //                var t2 = GV.matcherRLW.MatMatchWithAlgoAsync(0, RightMaskMat, GMPBackrMaskMp, _AlignC.RLWaferAlgorithm);
        //                var t3 = GV.matcherLHW.MatMatchWithAlgoAsync(0, GMPBackLeft, GMPBacklWaferMp, _AlignC.LHWaferAlgorithm);
        //                var t4 = GV.matcherRHW.MatMatchWithAlgoAsync(0, GMPBackRight, GMPBackrWaferMp, _AlignC.RHWaferAlgorithm);
        //                await Task.WhenAll(t1, t2, t3, t4).ConfigureAwait(false);
        //                GMPBacklWaferMp.X -= (float)(_AlignC.XOffset / GV.ZoomLensInfo.LeftDownUmPerPixelX);
        //                GMPBackrWaferMp.X -= (float)(_AlignC.XOffset / GV.ZoomLensInfo.RightDownUmPerPixelX);
        //                GMPBacklWaferMp.Y -= (float)(_AlignC.YLOffset / GV.ZoomLensInfo.LeftDownUmPerPixelY);
        //                GMPBackrWaferMp.Y -= (float)(_AlignC.YROffset / GV.ZoomLensInfo.RightDownUmPerPixelY);
        //                // patternCenterDistancePixel = _AlignC.PatternCenterDistanceUm / ((GV.ZoomLensInfo.LeftDownUmPerPixelX + GV.ZoomLensInfo.RightDownUmPerPixelX) / 2);
        //            }
        //            else if (alignMagnification == "backmask")
        //            {
        //                var t1 = GV.matcherLLW.MatMatchWithAlgoAsync(0, GMPBackLeft, GMPBacklMaskMp, _AlignC.LLWaferAlgorithm);
        //                var t2 = GV.matcherRLW.MatMatchWithAlgoAsync(0, GMPBackRight, GMPBackrMaskMp, _AlignC.RLWaferAlgorithm);
        //                await Task.WhenAll(t1, t2).ConfigureAwait(false);
        //            }
        //            else if (alignMagnification == "checkmask")
        //            {
        //                var t1 = GV.matcherLHM.MatMatchWithAlgoAsync(0, GMPBackLeft, GMPBacklMaskMp, _AlignC.LHMaskAlgorithm);
        //                var t2 = GV.matcherRHM.MatMatchWithAlgoAsync(0, GMPBackRight, GMPBackrMaskMp, _AlignC.RHMaskAlgorithm);
        //                await Task.WhenAll(t1, t2).ConfigureAwait(false);
        //            }
        //            //else if (alignMagnification == "backcheck")
        //            //{
        //            //   var t1= GV.matcherLLW.MatMatchWithAlgo(0, lSrc.Clone(), ref lWaferMp, _AlignC.LLWaferAlgorithm);
        //            //    GV.matcherRLW.MatMatchWithAlgo(0, rSrc.Clone(), ref rWaferMp, _AlignC.RLWaferAlgorithm);
        //            //    GV.matcherLHW.MatMatchWithAlgo(0, lSrc.Clone(), ref lWaferMp, _AlignC.LHWaferAlgorithm);
        //            //    GV.matcherRHW.MatMatchWithAlgo(0, rSrc.Clone(), ref rWaferMp, _AlignC.RHWaferAlgorithm);
        //            //}
        //            skLeft.MaskMp = GMPBacklMaskMp;
        //            skLeft.WaferMp = GMPBacklWaferMp;
        //            skRight.MaskMp = GMPBackrMaskMp;
        //            skRight.WaferMp = GMPBackrWaferMp;
        //        }

        //    }
        //    return new ProductMatchPositions(GMPBacklMaskMp, GMPBacklWaferMp, GMPBackrMaskMp, GMPBackrWaferMp, patternCenterDistance);
        //}
        private async Task<ProductMatchPositions> GetAllMatchPositionAsync(Mat lSrc, Mat rSrc, string alignMagnification, CancellationToken ct = default)
        {
            int i = 0;
            double patternCenterDistance = _AlignC.PatternCenterDistanceUm;

            while (i < 3)
            {
                lSrc.CopyTo(GMPBackLeft);
                rSrc.CopyTo(GMPBackRight);

                if (GV.AppSettingParm.Emulation != true)
                {
                    if (alignMagnification == "low")
                    {
                        var t1 = GV.matcherLLM.MatMatchWithAlgoAsync(0, GMPBackLeft, GMPBacklMaskMp, _AlignC.LLMaskAlgorithm, ct);
                        var t2 = GV.matcherRLM.MatMatchWithAlgoAsync(0, GMPBackRight, GMPBackrMaskMp, _AlignC.RLMaskAlgorithm, ct);
                        var t3 = GV.matcherLLW.MatMatchWithAlgoAsync(0, GMPBackLeft, GMPBacklWaferMp, _AlignC.LLWaferAlgorithm, ct);
                        var t4 = GV.matcherRLW.MatMatchWithAlgoAsync(0, GMPBackRight, GMPBackrWaferMp, _AlignC.RLWaferAlgorithm, ct);
                        await Task.WhenAll(t1, t2, t3, t4).ConfigureAwait(false);

                        // ✅ 套用偏移量校正座標
                        GMPBacklMaskMp.X += _AlignC.LLMaskOffsetX;
                        GMPBacklMaskMp.Y += _AlignC.LLMaskOffsetY;

                        GMPBacklWaferMp.X += _AlignC.LLWaferOffsetX;
                        GMPBacklWaferMp.Y += _AlignC.LLWaferOffsetY;

                        GMPBackrMaskMp.X += _AlignC.RLMaskOffsetX;
                        GMPBackrMaskMp.Y += _AlignC.RLMaskOffsetY;

                        GMPBackrWaferMp.X += _AlignC.RLWaferOffsetX;
                        GMPBackrWaferMp.Y += _AlignC.RLWaferOffsetY;

                        // 套用原有的 Offset 補償
                        GMPBacklWaferMp.X -= (float)(_AlignC.XOffset / GV.ZoomLensInfo.LeftUmPerPixelX[_AlignC.AlignLowMagnification]);
                        GMPBackrWaferMp.X -= (float)(_AlignC.XOffset / GV.ZoomLensInfo.RightUmPerPixelX[_AlignC.AlignLowMagnification]);
                        GMPBacklWaferMp.Y -= (float)(_AlignC.YLOffset / GV.ZoomLensInfo.LeftUmPerPixelY[_AlignC.AlignLowMagnification]);
                        GMPBackrWaferMp.Y -= (float)(_AlignC.YROffset / GV.ZoomLensInfo.RightUmPerPixelY[_AlignC.AlignLowMagnification]);
                    }
                    else if (alignMagnification == "high")
                    {
                        var t1 = GV.matcherLHM.MatMatchWithAlgoAsync(0, GMPBackLeft, GMPBacklMaskMp, _AlignC.LHMaskAlgorithm, ct);
                        var t2 = GV.matcherRHM.MatMatchWithAlgoAsync(0, GMPBackRight, GMPBackrMaskMp, _AlignC.RHMaskAlgorithm, ct);
                        var t3 = GV.matcherLHW.MatMatchWithAlgoAsync(0, GMPBackLeft, GMPBacklWaferMp, _AlignC.LHWaferAlgorithm, ct);
                        var t4 = GV.matcherRHW.MatMatchWithAlgoAsync(0, GMPBackRight, GMPBackrWaferMp, _AlignC.RHWaferAlgorithm, ct);
                        await Task.WhenAll(t1, t2, t3, t4).ConfigureAwait(false);

                        // ✅ 套用偏移量校正座標
                        GMPBacklMaskMp.X += _AlignC.LHMaskOffsetX;
                        GMPBacklMaskMp.Y += _AlignC.LHMaskOffsetY;

                        GMPBacklWaferMp.X += _AlignC.LHWaferOffsetX;
                        GMPBacklWaferMp.Y += _AlignC.LHWaferOffsetY;

                        GMPBackrMaskMp.X += _AlignC.RHMaskOffsetX;
                        GMPBackrMaskMp.Y += _AlignC.RHMaskOffsetY;

                        GMPBackrWaferMp.X += _AlignC.RHWaferOffsetX;
                        GMPBackrWaferMp.Y += _AlignC.RHWaferOffsetY;

                        // 套用原有的 Offset 補償
                        GMPBacklWaferMp.X -= (float)(_AlignC.XOffset / GV.ZoomLensInfo.LeftUmPerPixelX[_AlignC.AlignHighMagnification]);
                        GMPBackrWaferMp.X -= (float)(_AlignC.XOffset / GV.ZoomLensInfo.RightUmPerPixelX[_AlignC.AlignHighMagnification]);
                        GMPBacklWaferMp.Y -= (float)(_AlignC.YLOffset / GV.ZoomLensInfo.LeftUmPerPixelY[_AlignC.AlignHighMagnification]);
                        GMPBackrWaferMp.Y -= (float)(_AlignC.YROffset / GV.ZoomLensInfo.RightUmPerPixelY[_AlignC.AlignHighMagnification]);
                    }
                    else if (alignMagnification == "back")
                    {
                        // ✅ 底部對齊模式:使用完整影像搜尋
                        var t1 = GV.matcherLLM.MatMatchWithAlgoAsync(0, LeftMaskMat, GMPBacklMaskMp, _AlignC.LLMaskAlgorithm);
                        var t2 = GV.matcherRLM.MatMatchWithAlgoAsync(0, RightMaskMat, GMPBackrMaskMp, _AlignC.RLMaskAlgorithm);
                        var t3 = GV.matcherLHW.MatMatchWithAlgoAsync(0, GMPBackLeft, GMPBacklWaferMp, _AlignC.LHWaferAlgorithm);
                        var t4 = GV.matcherRHW.MatMatchWithAlgoAsync(0, GMPBackRight, GMPBackrWaferMp, _AlignC.RHWaferAlgorithm);
                        await Task.WhenAll(t1, t2, t3, t4).ConfigureAwait(false);

                        // ✅ 套用偏移量校正座標
                        GMPBacklMaskMp.X += _AlignC.LLMaskOffsetX;
                        GMPBacklMaskMp.Y += _AlignC.LLMaskOffsetY;

                        GMPBacklWaferMp.X += _AlignC.LHWaferOffsetX;
                        GMPBacklWaferMp.Y += _AlignC.LHWaferOffsetY;

                        GMPBackrMaskMp.X += _AlignC.RLMaskOffsetX;
                        GMPBackrMaskMp.Y += _AlignC.RLMaskOffsetY;

                        GMPBackrWaferMp.X += _AlignC.RHWaferOffsetX;
                        GMPBackrWaferMp.Y += _AlignC.RHWaferOffsetY;

                        // 套用原有的 Offset 補償
                        GMPBacklWaferMp.X -= (float)(_AlignC.XOffset / GV.ZoomLensInfo.LeftDownUmPerPixelX);
                        GMPBackrWaferMp.X -= (float)(_AlignC.XOffset / GV.ZoomLensInfo.RightDownUmPerPixelX);
                        GMPBacklWaferMp.Y -= (float)(_AlignC.YLOffset / GV.ZoomLensInfo.LeftDownUmPerPixelY);
                        GMPBackrWaferMp.Y -= (float)(_AlignC.YROffset / GV.ZoomLensInfo.RightDownUmPerPixelY);
                    }
                    else if (alignMagnification == "backmask")
                    {
                        // ✅ 只搜尋光罩:使用完整影像搜尋
                        var t1 = GV.matcherLLM.MatMatchWithAlgoAsync(0, GMPBackLeft, GMPBacklMaskMp, _AlignC.LLMaskAlgorithm);
                        var t2 = GV.matcherRLM.MatMatchWithAlgoAsync(0, GMPBackRight, GMPBackrMaskMp, _AlignC.RLMaskAlgorithm);
                        await Task.WhenAll(t1, t2).ConfigureAwait(false);

                        // ✅ 套用偏移量校正座標
                        GMPBacklMaskMp.X += _AlignC.LLMaskOffsetX;
                        GMPBacklMaskMp.Y += _AlignC.LLMaskOffsetY;

                        GMPBackrMaskMp.X += _AlignC.RLMaskOffsetX;
                        GMPBackrMaskMp.Y += _AlignC.RLMaskOffsetY;
                    }
                    else if (alignMagnification == "checkmask")
                    {
                        var t1 = GV.matcherLHM.MatMatchWithAlgoAsync(0, GMPBackLeft, GMPBacklMaskMp, _AlignC.LHMaskAlgorithm, ct);
                        var t2 = GV.matcherRHM.MatMatchWithAlgoAsync(0, GMPBackRight, GMPBackrMaskMp, _AlignC.RHMaskAlgorithm, ct);
                        await Task.WhenAll(t1, t2).ConfigureAwait(false);

                        // ✅ 套用偏移量校正座標
                        GMPBacklMaskMp.X += _AlignC.LHMaskOffsetX;
                        GMPBacklMaskMp.Y += _AlignC.LHMaskOffsetY;

                        GMPBackrMaskMp.X += _AlignC.RHMaskOffsetX;
                        GMPBackrMaskMp.Y += _AlignC.RHMaskOffsetY;
                    }

                    skLeft.MaskMp = GMPBacklMaskMp;
                    skLeft.WaferMp = GMPBacklWaferMp;
                    skRight.MaskMp = GMPBackrMaskMp;
                    skRight.WaferMp = GMPBackrWaferMp;
                }
                break; // 跳出 while 迴圈
            }

            return new ProductMatchPositions(GMPBacklMaskMp, GMPBacklWaferMp, GMPBackrMaskMp, GMPBackrWaferMp, patternCenterDistance);
        }
        //private ProductMatchPositions GetAllMatchPostion(Mat lSrc, Mat rSrc, string alignMmagnification)
        //{

        //    MatchPosition lMaskMp = new MatchPosition();
        //    MatchPosition rMaskMp = new MatchPosition();
        //    MatchPosition lWaferMp = new MatchPosition();
        //    MatchPosition rWaferMp = new MatchPosition();
        //    double patternCenterDistance = _AlignC.PatternCenterDistanceUm;

        //    if (GV.AppSettingParm.Emulation != true)
        //    {
        //        if (alignMmagnification == "low")
        //        {
        //            GV.matcherLLM.MatMatchWithAlgo(0, lSrc.Clone(), ref lMaskMp, _AlignC.LLMaskAlgorithm);
        //            GV.matcherRLM.MatMatchWithAlgo(0, rSrc.Clone(), ref rMaskMp, _AlignC.RLMaskAlgorithm);
        //            GV.matcherLLW.MatMatchWithAlgo(0, lSrc.Clone(), ref lWaferMp, _AlignC.LLWaferAlgorithm);
        //            GV.matcherRLW.MatMatchWithAlgo(0, rSrc.Clone(), ref rWaferMp, _AlignC.RLWaferAlgorithm);

        //            lWaferMp.X -= (float)(_AlignC.XOffset / GV.ZoomLensInfo.LeftUmPerPixelX[_AlignC.AlignLowMagnification]);
        //            rWaferMp.X -= (float)(_AlignC.XOffset / GV.ZoomLensInfo.RightUmPerPixelX[_AlignC.AlignLowMagnification]);
        //            lWaferMp.Y -= (float)(_AlignC.YLOffset / GV.ZoomLensInfo.LeftUmPerPixelY[_AlignC.AlignLowMagnification]);
        //            rWaferMp.Y -= (float)(_AlignC.YROffset / GV.ZoomLensInfo.RightUmPerPixelY[_AlignC.AlignLowMagnification]);
        //        }
        //        else if (alignMmagnification == "high")
        //        {
        //            GV.matcherLHM.MatMatchWithAlgo(0, lSrc.Clone(), ref lMaskMp, _AlignC.LHMaskAlgorithm);
        //            GV.matcherRHM.MatMatchWithAlgo(0, rSrc.Clone(), ref rMaskMp, _AlignC.RHMaskAlgorithm);
        //            GV.matcherLHW.MatMatchWithAlgo(0, lSrc.Clone(), ref lWaferMp, _AlignC.LHWaferAlgorithm);
        //            GV.matcherRHW.MatMatchWithAlgo(0, rSrc.Clone(), ref rWaferMp, _AlignC.RHWaferAlgorithm);
        //            lWaferMp.X -= (float)(_AlignC.XOffset / GV.ZoomLensInfo.LeftUmPerPixelX[_AlignC.AlignHighMagnification]);
        //            rWaferMp.X -= (float)(_AlignC.XOffset / GV.ZoomLensInfo.RightUmPerPixelX[_AlignC.AlignHighMagnification]);
        //            lWaferMp.Y -= (float)(_AlignC.YLOffset / GV.ZoomLensInfo.LeftUmPerPixelY[_AlignC.AlignHighMagnification]);
        //            rWaferMp.Y -= (float)(_AlignC.YROffset / GV.ZoomLensInfo.RightUmPerPixelY[_AlignC.AlignHighMagnification]);
        //        }
        //        else if (alignMmagnification == "back")
        //        {
        //            GV.matcherLLW.MatMatchWithAlgo(0, LeftMaskMat, ref lMaskMp, _AlignC.LLWaferAlgorithm);
        //            GV.matcherRLW.MatMatchWithAlgo(0, RightMaskMat, ref rMaskMp, _AlignC.RLWaferAlgorithm);
        //            GV.matcherLHW.MatMatchWithAlgo(0, lSrc.Clone(), ref lWaferMp, _AlignC.LHWaferAlgorithm);
        //            GV.matcherRHW.MatMatchWithAlgo(0, rSrc.Clone(), ref rWaferMp, _AlignC.RHWaferAlgorithm);
        //            lWaferMp.X -= (float)(_AlignC.XOffset / GV.ZoomLensInfo.LeftDownUmPerPixelX);
        //            rWaferMp.X -= (float)(_AlignC.XOffset / GV.ZoomLensInfo.RightDownUmPerPixelX);
        //            lWaferMp.Y -= (float)(_AlignC.YLOffset / GV.ZoomLensInfo.LeftDownUmPerPixelY);
        //            rWaferMp.Y -= (float)(_AlignC.YROffset / GV.ZoomLensInfo.RightDownUmPerPixelY);
        //            // patternCenterDistancePixel = _AlignC.PatternCenterDistanceUm / ((GV.ZoomLensInfo.LeftDownUmPerPixelX + GV.ZoomLensInfo.RightDownUmPerPixelX) / 2);
        //        }
        //        else if (alignMmagnification == "backmask")
        //        {
        //            GV.matcherLLW.MatMatchWithAlgo(0, lSrc.Clone(), ref lMaskMp, _AlignC.LLWaferAlgorithm);
        //            GV.matcherRLW.MatMatchWithAlgo(0, rSrc.Clone(), ref rMaskMp, _AlignC.RLWaferAlgorithm);
        //        }
        //        else if (alignMmagnification == "checkmask")
        //        {
        //            GV.matcherLHM.MatMatchWithAlgo(0, lSrc.Clone(), ref lMaskMp, _AlignC.LHMaskAlgorithm);
        //            GV.matcherRHM.MatMatchWithAlgo(0, rSrc.Clone(), ref rMaskMp, _AlignC.RHMaskAlgorithm);
        //        }
        //        else if (alignMmagnification == "backcheck")
        //        {
        //            GV.matcherLLW.MatMatchWithAlgo(0, lSrc.Clone(), ref lWaferMp, _AlignC.LLWaferAlgorithm);
        //            GV.matcherRLW.MatMatchWithAlgo(0, rSrc.Clone(), ref rWaferMp, _AlignC.RLWaferAlgorithm);
        //            GV.matcherLHW.MatMatchWithAlgo(0, lSrc.Clone(), ref lWaferMp, _AlignC.LHWaferAlgorithm);
        //            GV.matcherRHW.MatMatchWithAlgo(0, rSrc.Clone(), ref rWaferMp, _AlignC.RHWaferAlgorithm);
        //        }

        //        skLeft.MaskMp = lMaskMp;
        //        skLeft.WaferMp = lWaferMp;
        //        skRight.MaskMp = rMaskMp;
        //        skRight.WaferMp = rWaferMp;
        //    }
        //    return new ProductMatchPositions(lMaskMp, lWaferMp, rMaskMp, rWaferMp, patternCenterDistance);
        //}
        private ProductMatchPositions GetAllMatchPostion(Mat lSrc, Mat rSrc, string alignMagnification)
        {
            MatchPosition lMaskMp = new MatchPosition();
            MatchPosition rMaskMp = new MatchPosition();
            MatchPosition lWaferMp = new MatchPosition();
            MatchPosition rWaferMp = new MatchPosition();
            double patternCenterDistance = _AlignC.PatternCenterDistanceUm;

            if (GV.AppSettingParm.Emulation != true)
            {
                if (alignMagnification == "low")
                {
                    // ✅ 使用完整影像搜尋
                    GV.matcherLLM.MatMatchWithAlgo(0, lSrc.Clone(), ref lMaskMp, _AlignC.LLMaskAlgorithm);
                    GV.matcherRLM.MatMatchWithAlgo(0, rSrc.Clone(), ref rMaskMp, _AlignC.RLMaskAlgorithm);
                    GV.matcherLLW.MatMatchWithAlgo(0, lSrc.Clone(), ref lWaferMp, _AlignC.LLWaferAlgorithm);
                    GV.matcherRLW.MatMatchWithAlgo(0, rSrc.Clone(), ref rWaferMp, _AlignC.RLWaferAlgorithm);

                    // ✅ 套用偏移量校正座標
                    lMaskMp.X += _AlignC.LLMaskOffsetX;
                    lMaskMp.Y += _AlignC.LLMaskOffsetY;

                    lWaferMp.X += _AlignC.LLWaferOffsetX;
                    lWaferMp.Y += _AlignC.LLWaferOffsetY;

                    rMaskMp.X += _AlignC.RLMaskOffsetX;
                    rMaskMp.Y += _AlignC.RLMaskOffsetY;

                    rWaferMp.X += _AlignC.RLWaferOffsetX;
                    rWaferMp.Y += _AlignC.RLWaferOffsetY;

                    // 套用原有的 Offset 補償
                    lWaferMp.X -= (float)(_AlignC.XOffset / GV.ZoomLensInfo.LeftUmPerPixelX[_AlignC.AlignLowMagnification]);
                    rWaferMp.X -= (float)(_AlignC.XOffset / GV.ZoomLensInfo.RightUmPerPixelX[_AlignC.AlignLowMagnification]);
                    lWaferMp.Y -= (float)(_AlignC.YLOffset / GV.ZoomLensInfo.LeftUmPerPixelY[_AlignC.AlignLowMagnification]);
                    rWaferMp.Y -= (float)(_AlignC.YROffset / GV.ZoomLensInfo.RightUmPerPixelY[_AlignC.AlignLowMagnification]);
                }
                else if (alignMagnification == "high")
                {
                    // ✅ 使用完整影像搜尋
                    GV.matcherLHM.MatMatchWithAlgo(0, lSrc.Clone(), ref lMaskMp, _AlignC.LHMaskAlgorithm);
                    GV.matcherRHM.MatMatchWithAlgo(0, rSrc.Clone(), ref rMaskMp, _AlignC.RHMaskAlgorithm);
                    GV.matcherLHW.MatMatchWithAlgo(0, lSrc.Clone(), ref lWaferMp, _AlignC.LHWaferAlgorithm);
                    GV.matcherRHW.MatMatchWithAlgo(0, rSrc.Clone(), ref rWaferMp, _AlignC.RHWaferAlgorithm);

                    // ✅ 套用偏移量校正座標
                    lMaskMp.X += _AlignC.LHMaskOffsetX;
                    lMaskMp.Y += _AlignC.LHMaskOffsetY;

                    lWaferMp.X += _AlignC.LHWaferOffsetX;
                    lWaferMp.Y += _AlignC.LHWaferOffsetY;

                    rMaskMp.X += _AlignC.RHMaskOffsetX;
                    rMaskMp.Y += _AlignC.RHMaskOffsetY;

                    rWaferMp.X += _AlignC.RHWaferOffsetX;
                    rWaferMp.Y += _AlignC.RHWaferOffsetY;

                    // 套用原有的 Offset 補償
                    lWaferMp.X -= (float)(_AlignC.XOffset / GV.ZoomLensInfo.LeftUmPerPixelX[_AlignC.AlignHighMagnification]);
                    rWaferMp.X -= (float)(_AlignC.XOffset / GV.ZoomLensInfo.RightUmPerPixelX[_AlignC.AlignHighMagnification]);
                    lWaferMp.Y -= (float)(_AlignC.YLOffset / GV.ZoomLensInfo.LeftUmPerPixelY[_AlignC.AlignHighMagnification]);
                    rWaferMp.Y -= (float)(_AlignC.YROffset / GV.ZoomLensInfo.RightUmPerPixelY[_AlignC.AlignHighMagnification]);
                }
                else if (alignMagnification == "back")
                {
                    // ✅ 底部對齊模式:光罩用 LeftMaskMat/RightMaskMat,晶圓用 lSrc/rSrc
                    GV.matcherLLM.MatMatchWithAlgo(0, LeftMaskMat, ref lMaskMp, _AlignC.LLMaskAlgorithm);
                    GV.matcherRLM.MatMatchWithAlgo(0, RightMaskMat, ref rMaskMp, _AlignC.RLMaskAlgorithm);
                    GV.matcherLHW.MatMatchWithAlgo(0, lSrc.Clone(), ref lWaferMp, _AlignC.LHWaferAlgorithm);
                    GV.matcherRHW.MatMatchWithAlgo(0, rSrc.Clone(), ref rWaferMp, _AlignC.RHWaferAlgorithm);

                    // ✅ 套用偏移量校正座標
                    lMaskMp.X += _AlignC.LLMaskOffsetX;
                    lMaskMp.Y += _AlignC.LLMaskOffsetY;

                    lWaferMp.X += _AlignC.LHWaferOffsetX;
                    lWaferMp.Y += _AlignC.LHWaferOffsetY;

                    rMaskMp.X += _AlignC.RLMaskOffsetX;
                    rMaskMp.Y += _AlignC.RLMaskOffsetY;

                    rWaferMp.X += _AlignC.RHWaferOffsetX;
                    rWaferMp.Y += _AlignC.RHWaferOffsetY;

                    // 套用原有的 Offset 補償
                    lWaferMp.X -= (float)(_AlignC.XOffset / GV.ZoomLensInfo.LeftDownUmPerPixelX);
                    rWaferMp.X -= (float)(_AlignC.XOffset / GV.ZoomLensInfo.RightDownUmPerPixelX);
                    lWaferMp.Y -= (float)(_AlignC.YLOffset / GV.ZoomLensInfo.LeftDownUmPerPixelY);
                    rWaferMp.Y -= (float)(_AlignC.YROffset / GV.ZoomLensInfo.RightDownUmPerPixelY);
                }
                else if (alignMagnification == "backmask")
                {
                    // ✅ 只搜尋光罩:使用完整影像搜尋
                    GV.matcherLLM.MatMatchWithAlgo(0, lSrc.Clone(), ref lMaskMp, _AlignC.LLMaskAlgorithm);
                    GV.matcherRLM.MatMatchWithAlgo(0, rSrc.Clone(), ref rMaskMp, _AlignC.RLMaskAlgorithm);

                    // ✅ 套用偏移量校正座標
                    lMaskMp.X += _AlignC.LLMaskOffsetX;
                    lMaskMp.Y += _AlignC.LLMaskOffsetY;

                    rMaskMp.X += _AlignC.RLMaskOffsetX;
                    rMaskMp.Y += _AlignC.RLMaskOffsetY;
                }
                else if (alignMagnification == "checkmask")
                {
                    // ✅ 使用完整影像搜尋
                    GV.matcherLHM.MatMatchWithAlgo(0, lSrc.Clone(), ref lMaskMp, _AlignC.LHMaskAlgorithm);
                    GV.matcherRHM.MatMatchWithAlgo(0, rSrc.Clone(), ref rMaskMp, _AlignC.RHMaskAlgorithm);

                    // ✅ 套用偏移量校正座標
                    lMaskMp.X += _AlignC.LHMaskOffsetX;
                    lMaskMp.Y += _AlignC.LHMaskOffsetY;

                    rMaskMp.X += _AlignC.RHMaskOffsetX;
                    rMaskMp.Y += _AlignC.RHMaskOffsetY;
                }
                else if (alignMagnification == "backcheck")
                {
                    // ✅ 使用完整影像搜尋
                    GV.matcherLLW.MatMatchWithAlgo(0, lSrc.Clone(), ref lWaferMp, _AlignC.LLWaferAlgorithm);
                    GV.matcherRLW.MatMatchWithAlgo(0, rSrc.Clone(), ref rWaferMp, _AlignC.RLWaferAlgorithm);
                    GV.matcherLHW.MatMatchWithAlgo(0, lSrc.Clone(), ref lWaferMp, _AlignC.LHWaferAlgorithm);
                    GV.matcherRHW.MatMatchWithAlgo(0, rSrc.Clone(), ref rWaferMp, _AlignC.RHWaferAlgorithm);

                    // ✅ 套用偏移量校正座標
                    lWaferMp.X += _AlignC.LHWaferOffsetX;
                    lWaferMp.Y += _AlignC.LHWaferOffsetY;

                    rWaferMp.X += _AlignC.RHWaferOffsetX;
                    rWaferMp.Y += _AlignC.RHWaferOffsetY;
                }

                skLeft.MaskMp = lMaskMp;
                skLeft.WaferMp = lWaferMp;
                skRight.MaskMp = rMaskMp;
                skRight.WaferMp = rWaferMp;
            }

            return new ProductMatchPositions(lMaskMp, lWaferMp, rMaskMp, rWaferMp, patternCenterDistance);
        }

        private int[] XyyMotorSteps(MatchPosition lMaskMp, MatchPosition lWaferMp, MatchPosition rMaskMp, MatchPosition rWaferMp)
        {
            int[] motorSteps = new int[3];

            double patternCenterDistancePixel = _AlignC.PatternCenterDistanceUm / 2;

            double ly = ((double)(lWaferMp.Y - lMaskMp.Y)) * GV.ZoomLensInfo.LeftDownUmPerPixelY;
            double ry = ((double)(rWaferMp.Y - rMaskMp.Y)) * GV.ZoomLensInfo.RightDownUmPerPixelY;
            double waferSin = (ry - ly) / patternCenterDistancePixel;
            double waferRad = Math.Asin(waferSin);
            double waferDegree = waferSin * 180 / Math.PI;
            double rotateMotorSteps = waferDegree * GV.ZoomLensInfo.MotorStepsPerPixelDegree;

            double a, b, c;
            a = b = patternCenterDistancePixel;
            c = Math.Sqrt(a * a + b * b - 2 * a * b * Math.Cos(waferRad));
            double deltaX = c * Math.Sin(waferRad);
            double deltaY = c * Math.Cos(waferRad);

            if (waferSin < 0)
            {
                lWaferMp.X -= (float)deltaX;
                lWaferMp.Y += (float)deltaY;
                rWaferMp.X += (float)deltaX;
                rWaferMp.Y -= (float)deltaY;
            }
            if (waferSin > 0)
            {
                lWaferMp.X -= (float)deltaX;
                lWaferMp.Y -= (float)deltaY;
                rWaferMp.X += (float)deltaX;
                rWaferMp.Y += (float)deltaY;
            }

            double xMotorStepsl = (lMaskMp.X - lWaferMp.X) * GV.ZoomLensInfo.LeftDownUmPerPixelX * 10;
            double xMotorStepsr = (rMaskMp.X - rWaferMp.X) * GV.ZoomLensInfo.RightDownUmPerPixelX * 10;
            double xMotorSteps = (xMotorStepsl + xMotorStepsr) / 2;
            double yMotorStepsl = (lMaskMp.Y - lWaferMp.Y) * GV.ZoomLensInfo.LeftDownUmPerPixelY * 10;
            double yMotorStepsr = (rMaskMp.Y - rWaferMp.Y) * GV.ZoomLensInfo.RightDownUmPerPixelY * 10;
            double yMotorSteps = (yMotorStepsl + yMotorStepsr) / 2;

            motorSteps[0] = (int)Math.Round(xMotorSteps);
            motorSteps[1] = (int)Math.Round(yMotorSteps);
            motorSteps[2] = (int)Math.Round(rotateMotorSteps);
            return motorSteps;
        }

        private void btBTAlignWaferRotate_Click(object sender, EventArgs e)
        {
            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, true);

            ProductMatchPositions pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "back");

            pmps.LMaskMp.X = BLivePmps.LMaskMp.X;
            pmps.LMaskMp.Y = BLivePmps.LMaskMp.Y;
            pmps.LMaskMp.Score = BLivePmps.LMaskMp.Score;
            pmps.RMaskMp.X = BLivePmps.RMaskMp.X;
            pmps.RMaskMp.Y = BLivePmps.RMaskMp.Y;
            pmps.RMaskMp.Score = BLivePmps.RMaskMp.Score;

            // PointF center = new Point(GV.LeftBackCam.Grab().Cols / 2, GV.LeftBackCam.Grab().Rows / 2);

            double beforeMoveLShiftXum = (pmps.LWaferMp.X - pmps.LMaskMp.X) * GV.ZoomLensInfo.LeftDownUmPerPixelX;
            double beforeMoveLShiftYum = (pmps.LWaferMp.Y - pmps.LMaskMp.Y) * GV.ZoomLensInfo.LeftDownUmPerPixelY;
            double beforeMoveRShiftXum = (pmps.RWaferMp.X - pmps.RMaskMp.X) * GV.ZoomLensInfo.RightDownUmPerPixelX;
            double beforeMoveRShiftYum = (pmps.RWaferMp.Y - pmps.RMaskMp.Y) * GV.ZoomLensInfo.RightDownUmPerPixelY;
            string msgL = string.Format(GV.Dlang.strMessageLShiftum, beforeMoveLShiftXum.ToString("F2"), beforeMoveLShiftYum.ToString("F2"));
            string msgR = string.Format(GV.Dlang.strMessageRShiftum, beforeMoveRShiftXum.ToString("F2"), beforeMoveRShiftYum.ToString("F2"));
            GV.HwndFormMain.writeStatus(msgL);
            GV.HwndFormMain.writeStatus(msgR);
            lbLeftXYW.Text = msgL;
            lbRightXYW.Text = msgR;

            if (pmps.LWaferMp.Score < _AlignC.WaferScore || pmps.RWaferMp.Score < _AlignC.WaferScore)
            {
                GV.Plc.SendAlignBackNG();
                iAlignOK = 0;
                GV.HwndFormMain.writeStatus(GV.Dlang.strErrorCantBottomWafer);
                return;
                // throw new MRException("Error : Can't find wafer template at high magnification");
            }

            //int[] motorSteps = CalcuteMaskWaferShiftAndReturnMotorStepsBack(pmps.LMaskMp, pmps.LWaferMp, pmps.RMaskMp, pmps.RWaferMp, pmps.PatternCenterDistance);

            int[] motorSteps = XyyRotateSteps(pmps.LMaskMp, pmps.LWaferMp, pmps.RMaskMp, pmps.RWaferMp);

            if (!GV.Plc.ReadMemory(GV.Plc.iUpCCDStart))
            {
                iAlignOK = 0;
                //iStartAlign = 0;
                return;
            }
            GV.Plc.XyyTableMove(motorSteps[0], motorSteps[1], motorSteps[2]);

            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, false);

            Thread.Sleep(1000);

            pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "back");

            pmps.LMaskMp.X = BLivePmps.LMaskMp.X;
            pmps.LMaskMp.Y = BLivePmps.LMaskMp.Y;
            pmps.LMaskMp.Score = BLivePmps.LMaskMp.Score;
            pmps.RMaskMp.X = BLivePmps.RMaskMp.X;
            pmps.RMaskMp.Y = BLivePmps.RMaskMp.Y;
            pmps.RMaskMp.Score = BLivePmps.RMaskMp.Score;

            double NLdisX = pmps.LWaferMp.X - pmps.LMaskMp.X;
            double NLdisY = pmps.LWaferMp.Y - pmps.LMaskMp.Y;
            double NRdisX = pmps.RWaferMp.X - pmps.RMaskMp.X;
            double NRdisY = pmps.RWaferMp.Y - pmps.RMaskMp.Y;

            double NLdisXum = NLdisX * GV.ZoomLensInfo.LeftDownUmPerPixelX;
            double NLdisYum = NLdisY * GV.ZoomLensInfo.LeftDownUmPerPixelY;
            double NRdisXum = NRdisX * GV.ZoomLensInfo.RightDownUmPerPixelX;
            double NRdisYum = NRdisY * GV.ZoomLensInfo.RightDownUmPerPixelY;

            string msgLW = string.Format("LWXum = {0:N8}, LWYum = {1:N8}", NLdisXum, NLdisYum);
            string msgRW = string.Format("RWXum = {0:N8}, RWYum = {1:N8}", NRdisXum, NRdisYum);

            lbLeftXYW.Text = msgLW;
            lbRightXYW.Text = msgRW;
        }

        private void btGoDis_Click(object sender, EventArgs e)
        {
            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, true);

            Thread.Sleep(100);

            ProductMatchPositions pmps;

            if (!((_recipe.LeftHighWaferMat == null) || (_recipe.RightHighWaferMat) == null))
            {
                pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "back");
            }
            else if (!((_recipe.LeftLowWaferMat == null) || (_recipe.RightLowWaferMat) == null))
            {
                pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "backmask");
            }
            else
            {
                pmps = new ProductMatchPositions();
            }

            OrgPmps.LWaferMp.X = pmps.LWaferMp.X;
            OrgPmps.LWaferMp.Y = pmps.LWaferMp.Y;
            OrgPmps.RWaferMp.X = pmps.RWaferMp.X;
            OrgPmps.RWaferMp.Y = pmps.RWaferMp.Y;

            int Xdis = (int)nUDXyyX.Value + 1;
            int Y1dis = (int)nUDXyyY1.Value + 1;
            // int Y2dis = (int)nUDXyyY2.Value;

            GV.Plc.GetLocation();

            OrgMotors[0] = GV.NowLocation[GV.Plc.iiDChuckX];
            OrgMotors[1] = GV.NowLocation[GV.Plc.iiDChuckY1];
            OrgMotors[2] = GV.NowLocation[GV.Plc.iiDChuckY2];

            int x = GV.NowLocation[GV.Plc.iiDChuckX] + Xdis;
            int y1 = GV.NowLocation[GV.Plc.iiDChuckY1] + Y1dis;
            int y2 = GV.NowLocation[GV.Plc.iiDChuckY2] + Y1dis;

            bool bRet = GV.Plc.XyyTableMoveAbs(x, y1, y2);

            if (bRet == false)
            {
                string msg = string.Format("Xyy Move Abs Error = Plc Error !!!");
                lbDebugMsg.Text = msg;
            }

            Thread.Sleep(1000);

            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, false);

            //pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "back");

            if (!((_recipe.LeftHighWaferMat == null) || (_recipe.RightHighWaferMat) == null))
            {
                pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "back");
            }
            else if (!((_recipe.LeftLowWaferMat == null) || (_recipe.RightLowWaferMat) == null))
            {
                pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "backmask");
            }
            else
            {
                pmps = new ProductMatchPositions();
            }

            double NLdisX = pmps.LWaferMp.X - OrgPmps.LWaferMp.X;
            double NLdisY = pmps.LWaferMp.Y - OrgPmps.LWaferMp.Y;
            double NRdisX = pmps.RWaferMp.X - OrgPmps.RWaferMp.X;
            double NRdisY = pmps.RWaferMp.Y - OrgPmps.RWaferMp.Y;

            double NLdisXum = NLdisX * GV.ZoomLensInfo.LeftDownUmPerPixelX;
            double NLdisYum = NLdisY * GV.ZoomLensInfo.LeftDownUmPerPixelY;
            double NRdisXum = NRdisX * GV.ZoomLensInfo.RightDownUmPerPixelX;
            double NRdisYum = NRdisY * GV.ZoomLensInfo.RightDownUmPerPixelY;

            double LStepUm = NLdisXum / Xdis;
            double RStepUm = NRdisXum / Xdis;
            double LYStepUm = NLdisYum / Y1dis;
            double RYStepUm = NRdisYum / Y1dis;

            string msgLW = string.Format("LWX = {0:N8}, LWXum = {2:N8}, LWY = {1:N8}, LWYum = {3:N8}", LStepUm, LYStepUm, NLdisXum, NLdisYum);
            string msgRW = string.Format("RWX = {0:N8}, RWXum = {2:N8}, RWY = {1:N8}, RWYum = {3:N8}", RStepUm, RYStepUm, NRdisXum, NRdisYum);

            lbLeftXYW.Text = msgLW;
            lbRightXYW.Text = msgRW;
        }

        private void btGoDisN_Click(object sender, EventArgs e)
        {
            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, true);

            Thread.Sleep(100);

            //ProductMatchPositions pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "back");
            ProductMatchPositions pmps;

            if (!((_recipe.LeftHighWaferMat == null) || (_recipe.RightHighWaferMat) == null))
            {
                pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "back");
            }
            else if (!((_recipe.LeftLowWaferMat == null) || (_recipe.RightLowWaferMat) == null))
            {
                pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "backmask");
            }
            else
            {
                pmps = new ProductMatchPositions();
            }


            OrgPmps.LWaferMp.X = pmps.LWaferMp.X;
            OrgPmps.LWaferMp.Y = pmps.LWaferMp.Y;
            OrgPmps.RWaferMp.X = pmps.RWaferMp.X;
            OrgPmps.RWaferMp.Y = pmps.RWaferMp.Y;

            int Xdis = (int)nUDXyyX.Value;
            int Y1dis = (int)nUDXyyY1.Value;
            // int Y2dis = (int)nUDXyyY2.Value;

            GV.Plc.GetLocation();

            OrgMotors[0] = GV.NowLocation[GV.Plc.iiDChuckX];
            OrgMotors[1] = GV.NowLocation[GV.Plc.iiDChuckY1];
            OrgMotors[2] = GV.NowLocation[GV.Plc.iiDChuckY2];

            int x = GV.NowLocation[GV.Plc.iiDChuckX] - Xdis;
            int y1 = GV.NowLocation[GV.Plc.iiDChuckY1] - Y1dis;
            int y2 = GV.NowLocation[GV.Plc.iiDChuckY2] - Y1dis;

            bool bRet = GV.Plc.XyyTableMoveAbs(x, y1, y2);

            if (bRet == false)
            {
                string msg = string.Format("Xyy Move Abs Error = Plc Error !!!");
                lbDebugMsg.Text = msg;
            }

            Thread.Sleep(1000);

            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, false);

            if (!((_recipe.LeftHighWaferMat == null) || (_recipe.RightHighWaferMat) == null))
            {
                pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "back");
            }
            else if (!((_recipe.LeftLowWaferMat == null) || (_recipe.RightLowWaferMat) == null))
            {
                pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "backmask");
            }
            else
            {
                pmps = new ProductMatchPositions();
            }

            double NLdisX = pmps.LWaferMp.X - OrgPmps.LWaferMp.X;
            double NLdisY = pmps.LWaferMp.Y - OrgPmps.LWaferMp.Y;
            double NRdisX = pmps.RWaferMp.X - OrgPmps.RWaferMp.X;
            double NRdisY = pmps.RWaferMp.Y - OrgPmps.RWaferMp.Y;

            double NLdisXum = NLdisX * GV.ZoomLensInfo.LeftDownUmPerPixelX;
            double NLdisYum = NLdisY * GV.ZoomLensInfo.LeftDownUmPerPixelY;
            double NRdisXum = NRdisX * GV.ZoomLensInfo.RightDownUmPerPixelX;
            double NRdisYum = NRdisY * GV.ZoomLensInfo.RightDownUmPerPixelY;

            double LStepUm = NLdisXum / Xdis;
            double RStepUm = NRdisXum / Xdis;
            double LYStepUm = NLdisYum / Y1dis;
            double RYStepUm = NRdisYum / Y1dis;

            string msgLW = string.Format("LWX = {0:N8}, LWXum = {2:N8}, LWY = {1:N8}, LWYum = {3:N8}", LStepUm, LYStepUm, NLdisXum, NLdisYum);
            string msgRW = string.Format("RWX = {0:N8}, RWXum = {2:N8}, RWY = {1:N8}, RWYum = {3:N8}", RStepUm, RYStepUm, NRdisXum, NRdisYum);

            lbLeftXYW.Text = msgLW;
            lbRightXYW.Text = msgRW;

        }

        private void BtLBackMask_Click(object sender, EventArgs e)
        {
            if (_recipe.LeftLowMaskMat != null)
            {
                if (_recipe.LeftLowMaskMask == null)
                {
                    _recipe.LeftLowMaskMask = new GrayImage(_recipe.LeftLowMaskMat.Width, _recipe.LeftLowMaskMat.Height);
                    _recipe.LeftLowMaskMask.Fill(255);
                }
                DialogPaintMask dialogPaintMask = new DialogPaintMask()
                {
                    Image = _recipe.LeftLowMaskMat,
                    Mask = _recipe.LeftLowMaskMask
                };
                if (dialogPaintMask.ShowDialog() == DialogResult.OK)
                {
                    _recipe.LeftLowMaskMask = new GrayImage(dialogPaintMask.Mask);
                    if (GV.AppSettingParm.Emulation != true)
                    {
                        GV.matcherLLM.LearnWithAlgo(_recipe.LeftLowMaskMat, _recipe.LeftLowMaskMask, _AlignC.LLMaskAlgorithm);
                    }
                    _AlignC.LastModifyTime = DateTime.Now;
                    GM.WriteRecipeXml(EditRecipe);
                }
            }
        }

        private void BtRWaferMask_Click(object sender, EventArgs e)
        {
            if (_recipe.RightHighWaferMat != null)
            {
                if (_recipe.RightHighWaferMask == null)
                {
                    _recipe.RightHighWaferMask = new GrayImage(_recipe.RightHighWaferMat.Width, _recipe.RightHighWaferMat.Height);
                    _recipe.RightHighWaferMask.Fill(255);
                }
                DialogPaintMask dialogPaintMask = new DialogPaintMask()
                {
                    Image = _recipe.RightHighWaferMat,
                    Mask = _recipe.RightHighWaferMask
                };
                if (dialogPaintMask.ShowDialog() == DialogResult.OK)
                {
                    _recipe.RightHighWaferMask = new GrayImage(dialogPaintMask.Mask);
                    GV.matcherRHW.LearnWithAlgo(_recipe.RightHighWaferMat, _recipe.RightHighWaferMask, _AlignC.RHWaferAlgorithm);
                    _AlignC.LastModifyTime = DateTime.Now;
                    GM.WriteRecipeXml(EditRecipe);
                }
            }
        }

        private void BtRBackMask_Click(object sender, EventArgs e)
        {
            if (_recipe.RightLowMaskMat != null)
            {
                if (_recipe.RightLowMaskMask == null)
                {
                    _recipe.RightLowMaskMask = new GrayImage(_recipe.RightLowMaskMat.Width, _recipe.RightLowMaskMat.Height);
                    _recipe.RightLowMaskMask.Fill(255);
                }
                DialogPaintMask dialogPaintMask = new DialogPaintMask()
                {
                    Image = _recipe.RightLowMaskMat,
                    Mask = _recipe.RightLowMaskMask
                };
                if (dialogPaintMask.ShowDialog() == DialogResult.OK)
                {
                    _recipe.RightLowMaskMask = new GrayImage(dialogPaintMask.Mask);
                    GV.matcherRLM.LearnWithAlgo(_recipe.RightLowMaskMat, _recipe.RightLowMaskMask, _AlignC.RLMaskAlgorithm);
                    _AlignC.LastModifyTime = DateTime.Now;
                    GM.WriteRecipeXml(EditRecipe);
                }
            }
        }

        private void CBTopMask_Click(object sender, EventArgs e)
        {
            ChangeTopBottomMask(1);
        }

        private void ChangeTopBottomMask(int iTopBottom)
        {
            if (iTopBottom == 1)
            {
                cBTopMask.Checked = true;
                cBBottomMask.Checked = false;
                _AlignC.UpBotMask = iTopBottom;
            }
            else
            {
                cBTopMask.Checked = false;
                cBBottomMask.Checked = true;
                _AlignC.UpBotMask = iTopBottom;
            }
            _AlignC.LastModifyTime = DateTime.Now;
            GM.WriteRecipeXml(EditRecipe);
        }

        private void CBBottomMask_Click(object sender, EventArgs e)
        {
            ChangeTopBottomMask(2);
        }

        private void CBAdjZ_CheckedChanged(object sender, EventArgs e)
        {
            if (cBAdjZ.Checked)
            {
                _AlignC.AdjuestZ = 1;
            }
            else
            {
                _AlignC.AdjuestZ = 0;
            }
            _AlignC.LastModifyTime = DateTime.Now;
            GM.WriteRecipeXml(EditRecipe);
        }

        private void BtMask_Click(object sender, EventArgs e)
        {
            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, true);
            GV.Plc.DownCameraZToHigh(1);
            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, false);
        }

        private void BtWafer_Click(object sender, EventArgs e)
        {
            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, true);
            GV.Plc.DownCameraZToHigh(0);
            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, false);
        }

        private void BtCapMask_Click(object sender, EventArgs e)
        {
            Thread.Sleep(250);
            LeftMaskMat = GV.LeftBackCam.Grab();
            RightMaskMat = GV.RightBackCam.Grab();
            GV.LeftMaskMat = LeftMaskMat;
            GV.RightMaskMat = RightMaskMat;
            Cv2.ImWrite("LeftMask.bmp", LeftMaskMat);
            Cv2.ImWrite("RightMask.bmp", RightMaskMat);
        }

        private async void BtAlignTest_ClickAsync(object sender, EventArgs e)
        {

            string msg;


            lbDebugMsg.Visible = true;
            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, true);

            Thread.Sleep(250);
            ProductMatchPositions pmps = await GetAllMatchPositionAsync(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "back");

            double LShiftXum = (pmps.LMaskMp.X - pmps.LWaferMp.X) * GV.ZoomLensInfo.LeftDownUmPerPixelX / GV.ZoomLensInfo.LeftDownUmPerStepX;
            double LShiftYum = (pmps.LMaskMp.Y - pmps.LWaferMp.Y) * GV.ZoomLensInfo.LeftDownUmPerPixelY / GV.ZoomLensInfo.LeftDownUmPerStepY;
            double RShiftXum = (pmps.RMaskMp.X - pmps.RWaferMp.X) * GV.ZoomLensInfo.RightDownUmPerPixelX / GV.ZoomLensInfo.RightDownUmPerStepX;
            double RShiftYum = (pmps.RMaskMp.Y - pmps.RWaferMp.Y) * GV.ZoomLensInfo.RightDownUmPerPixelY / GV.ZoomLensInfo.RightDownUmPerStepY;
            double rollDistance = LShiftYum - RShiftYum;
            GV.Plc.GetLocation();
            _AlignC.PatternCenterDistanceUm = GV.ZoomLensInfo.LRBackCenterDistance - GV.NowLocation[GV.Plc.iiDDownLeftX] / 10 - GV.NowLocation[GV.Plc.iiDDownRightX] / 10;
            int[] motorSteps = XyyMotorSteps(pmps.LMaskMp, pmps.LWaferMp, pmps.RMaskMp, pmps.RWaferMp);
            msg = string.Format("Roll dis {3:N3} TableMove {0}, {1}, {2}", motorSteps[0], motorSteps[1], motorSteps[2], rollDistance);
            lbDebugMsg.Text = msg;
            bool bRet = GV.Plc.AlignXyyTableMove(motorSteps[0], motorSteps[1], motorSteps[2]);
            if (bRet == false)
            {
                msg = string.Format("XyyMove Error = Plc Error !!!");
                lbDebugMsg.Text = msg;
            }

            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, false);

        }


        private void BtAlignTest_Click(object sender, EventArgs e)
        {
            string msg;

            lbDebugMsg.Visible = true;

            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, true);

            Thread.Sleep(250);

            ProductMatchPositions pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "back");

            double LShiftXum = (pmps.LMaskMp.X - pmps.LWaferMp.X) * GV.ZoomLensInfo.LeftDownUmPerPixelX / GV.ZoomLensInfo.LeftDownUmPerStepX;
            double LShiftYum = (pmps.LMaskMp.Y - pmps.LWaferMp.Y) * GV.ZoomLensInfo.LeftDownUmPerPixelY / GV.ZoomLensInfo.LeftDownUmPerStepY;
            double RShiftXum = (pmps.RMaskMp.X - pmps.RWaferMp.X) * GV.ZoomLensInfo.RightDownUmPerPixelX / GV.ZoomLensInfo.RightDownUmPerStepX;
            double RShiftYum = (pmps.RMaskMp.Y - pmps.RWaferMp.Y) * GV.ZoomLensInfo.RightDownUmPerPixelY / GV.ZoomLensInfo.RightDownUmPerStepY;

            double Rolldis = LShiftYum - RShiftYum;

            GV.Plc.GetLocation();

            _AlignC.PatternCenterDistanceUm = GV.ZoomLensInfo.LRBackCenterDistance
                - GV.NowLocation[GV.Plc.iiDDownLeftX] / 10 - GV.NowLocation[GV.Plc.iiDDownRightX] / 10;

            int[] motorSteps = XyyMotorSteps(pmps.LMaskMp, pmps.LWaferMp, pmps.RMaskMp, pmps.RWaferMp);

            msg = string.Format("Roll dis {3:N3} TableMove {0}, {1}, {2}", motorSteps[0], motorSteps[1], motorSteps[2], Rolldis);

            lbDebugMsg.Text = msg;

            bool bRet = GV.Plc.AlignXyyTableMove(motorSteps[0], motorSteps[1], motorSteps[2]);
            if (bRet == false)
            {
                msg = string.Format("XyyMove Error = Plc Error !!!");
                lbDebugMsg.Text = msg;
            }

            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, false);
        }
        public void UpdateClassList(Dictionary<string, Int16> classList)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<Dictionary<string, Int16>>(UpdateClassList), classList);
                return;
            }

            try
            {
                cbLBackMaskClassList.Items.Clear();
                cbLBackWaferClassList.Items.Clear();
                cbRBackWaferClassList.Items.Clear();
                cbRBackMaskClassList.Items.Clear();

                foreach (var className in classList)
                {
                    cbLBackMaskClassList.Items.Add(className.Key);
                    cbLBackWaferClassList.Items.Add(className.Key);
                    cbRBackWaferClassList.Items.Add(className.Key);
                    cbRBackMaskClassList.Items.Add(className.Key);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"更新 ClassList 失敗: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtMaskCenter_Click(object sender, EventArgs e)
        {
            //ProductMatchPositions pmps = null;
            string msg;

            lbDebugMsg.Visible = true;

            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, true);

            Thread.Sleep(250);
            ProductMatchPositions pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "backmask");

            PointF center = GV.DownCenter;

            double leftX = ((double)(center.X) - pmps.LMaskMp.X) * GV.ZoomLensInfo.LeftDownUmPerPixelX / GV.ZoomLensInfo.LeftDownUmPerStepX;
            double leftY = ((double)(center.Y) - pmps.LMaskMp.Y) * GV.ZoomLensInfo.LeftDownUmPerPixelY / GV.ZoomLensInfo.LeftDownUmPerStepY;
            double rightX = ((double)(center.X) - pmps.RMaskMp.X) * GV.ZoomLensInfo.RightDownUmPerPixelX / GV.ZoomLensInfo.RightDownUmPerStepX;
            double rightY = ((double)(center.Y) - pmps.RMaskMp.Y) * GV.ZoomLensInfo.RightDownUmPerPixelY / GV.ZoomLensInfo.RightDownUmPerStepY;

            bool bRet = GV.Plc.AlignCameraMove(2, -leftX, rightX, -leftY, -rightY);

            if (bRet == false)
            {
                // msg = string.Format("CameraMove Error = {0}", GV.Plc.PlcErrorMessage);
                msg = string.Format("CameraMove Error = Plc Error");
                lbDebugMsg.Text = msg;
                GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, false);
                return;
            }

            Thread.Sleep(250);

            pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "backmask");

            double LShiftXum = (pmps.LMaskMp.X - pmps.LWaferMp.X) * GV.ZoomLensInfo.LeftDownUmPerPixelX;
            double LShiftYum = (pmps.LMaskMp.Y - pmps.LWaferMp.Y) * GV.ZoomLensInfo.LeftDownUmPerPixelY;
            double RShiftXum = (pmps.RMaskMp.X - pmps.RWaferMp.X) * GV.ZoomLensInfo.RightDownUmPerPixelX;
            double RShiftYum = (pmps.RMaskMp.Y - pmps.RWaferMp.Y) * GV.ZoomLensInfo.RightDownUmPerPixelY;

            double Rolldis = LShiftYum - RShiftYum;

            _AlignC.PatternCenterDistanceUm = GV.ZoomLensInfo.LRUpCenterDistance
                - GV.NowLocation[GV.Plc.iiDUpLeftX] / 10 - GV.NowLocation[GV.Plc.iiDUpRightX] / 10;

            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, false);
        }


        public void Focus(SentechNetToMat cam, int iLeftRight)
        {
            int nowLocZ;
            double bestFocus = 0;
            GV.Plc.GetLocation();
            int MinPosition;
            int MaxPosition;
            int leftNowLoc = GV.NowLocation[GV.Plc.iiDDownLeftZ];
            int rightNowLoc = GV.NowLocation[GV.Plc.iiDDownRightZ];
            Mat mat;
            if (iLeftRight == 0)
            {
                nowLocZ = GV.NowLocation[GV.Plc.iiDDownLeftZ];
                MinPosition = GV.Plc.iLimit[GV.Plc.iLimitLeftDownZmin];
                MaxPosition = GV.Plc.iLimit[GV.Plc.iLimitLeftDownZmax];
            }
            else
            {
                nowLocZ = GV.NowLocation[GV.Plc.iiDDownRightZ];
                MinPosition = GV.Plc.iLimit[GV.Plc.iLimitRightDownZmin];
                MaxPosition = GV.Plc.iLimit[GV.Plc.iLimitRightDownZmax];
            }
            int bestPosition = nowLocZ;

            for (double pos = MinPosition; pos <= MaxPosition; pos += 100) // 假設 0.1 是步進量
            {
                if (iLeftRight == 0)
                {
                    GV.Plc.AlignDownCameraMoveZAbsW(0, 0, 0, 0, 0, pos, rightNowLoc);
                    System.Threading.Thread.Sleep(100); // 等待機構穩定
                    mat = GV.LeftBackCam.Grab();
                }
                else
                {
                    GV.Plc.AlignDownCameraMoveZAbsW(0, 0, 0, 0, 0, leftNowLoc, pos);
                    System.Threading.Thread.Sleep(100); // 等待機構穩定
                    mat = GV.RightBackCam.Grab();
                }

                double focusValue = CalculateFocus(mat);

                if (focusValue > bestFocus)
                {
                    bestFocus = focusValue;
                    //bestPosition = pos;
                }
            }

            //_zController.MoveTo(bestPosition); // 移到最佳焦距
        }

        //private double CalculateFocusLaplacian(Mat image)
        //{
        //    var laplacian = new Mat();
        //    Cv2.Laplacian(image, laplacian, MatType.CV_8UC1);
        //    var stdDev = new Scalar();
        //    var mean = new Scalar();
        //    Cv2.MeanStdDev(laplacian, ref mean, ref stdDev);
        //    return stdDev.V0 * stdDev.V0;
        //}

        //private double CalculateFocus(Mat image)
        //{
        //    Mat gray = new Mat();
        //    if (image.NumberOfChannels == 3)
        //    {
        //        CvInvoke.CvtColor(image, gray, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);
        //    }
        //    else
        //    {
        //        gray = image;
        //    }

        //    Mat sobelX = new Mat();
        //    Mat sobelY = new Mat();
        //    CvInvoke.Sobel(gray, sobelX, Emgu.CV.CvEnum.DepthType.Cv16S, 1, 0);
        //    CvInvoke.Sobel(gray, sobelY, Emgu.CV.CvEnum.DepthType.Cv16S, 0, 1);

        //    Mat sobel = new Mat();
        //    CvInvoke.Magnitude(sobelX, sobelY, sobel);

        //    double focusValue = CvInvoke.Mean(sobel).V0;
        //    return focusValue;
        //}

        private double CalculateFocus(Mat image)
        {
            Mat gray = new Mat();
            if (image.Channels() == 3)
            {
                Cv2.CvtColor(image, gray, ColorConversionCodes.BGR2GRAY);
            }
            else
            {
                gray = image;
            }

            Mat sobelX = new Mat();
            Mat sobelY = new Mat();
            Cv2.Sobel(gray, sobelX, MatType.CV_16S, 1, 0);
            Cv2.Sobel(gray, sobelY, MatType.CV_16S, 0, 1);

            Mat sobel = new Mat(sobelX.Size(), MatType.CV_16S);
            Cv2.AddWeighted(sobelX, 0.5, sobelY, 0.5, 0, sobel);

            double focusValue = Cv2.Mean(sobel)[0];
            return focusValue;
        }

        private void BtLeftWaferFocus_Click(object sender, EventArgs e)
        {
            AutoFocus(GV.LeftBackCam, 0);
        }

        private void BtLMaskAdjSave_Click(object sender, EventArgs e)
        {
            _AlignC.LMaskAdjX = (int)NUDLMaskX.Value;
            _AlignC.LMaskAdjY = (int)NUDLMaskY.Value;
            _AlignC.LastModifyTime = DateTime.Now;
            GM.WriteAlignConditionsAcarToXml("AlignConditions.xml");
        }

        private void BtRMaskAdjSave_Click(object sender, EventArgs e)
        {
            _AlignC.RMaskAdjX = (int)NUDRMaskX.Value;
            _AlignC.RMaskAdjY = (int)NUDRMaskY.Value;
            _AlignC.LastModifyTime = DateTime.Now;
            GM.WriteAlignConditionsAcarToXml("AlignConditions.xml");
        }

        private void btChuckAlignN_Click(object sender, EventArgs e)
        {

        }

        private int[] XyyRotateSteps(MatchPosition lMaskMp, MatchPosition lWaferMp, MatchPosition rMaskMp, MatchPosition rWaferMp)
        {
            int[] motorSteps = new int[3];
            //double patternCenterDistancePixel = _AlignC.PatternCenterDistanceUm;

            RotationTranslation rot = GetRotation(lMaskMp, lWaferMp, rMaskMp, rWaferMp);

            double _dx = rot.Translation.X;
            double _dy = rot.Translation.Y;

            double dY = GV.NowLocation[GV.Plc.iiDChuckY1] * GV.ZoomLensInfo.LeftDownUmPerPixelY - GV.NowLocation[GV.Plc.iiDChuckY2] * GV.ZoomLensInfo.RightDownUmPerPixelY;
            double Theta0 = Math.Atan2(dY, GV.AppSettingParm.XYYTableSize);

            double dx1 = GV.R * Math.Sin(rot.Rotation + GV.ThetaX + Theta0) - GV.R * Math.Sin(GV.ThetaX + Theta0);
            double dy1 = GV.R * Math.Cos(rot.Rotation + GV.ThetaY1 + Theta0) - GV.R * Math.Cos(GV.ThetaY1 + Theta0);
            double dy2 = GV.R * Math.Cos(rot.Rotation + GV.ThetaY2 + Theta0) - GV.R * Math.Cos(GV.ThetaY2 + Theta0);

            double mx = _dx + dx1;
            double my1 = _dy + dy1;
            double my2 = _dy + dy2;

            motorSteps[0] = (int)(-mx / GV.AppSettingParm.XYYStepX);
            motorSteps[1] = (int)(my1 / GV.AppSettingParm.XYYStepY1);
            motorSteps[2] = (int)(my2 / GV.AppSettingParm.XYYStepY2);

            return motorSteps;
        }

        private void BtRightWaferFocus_Click(object sender, EventArgs e)
        {
            AutoFocus(GV.LeftBackCam, 1);
        }

        public RotationTranslation GetRotation(MatchPosition lMaskMp, MatchPosition lWaferMp, MatchPosition rMaskMp, MatchPosition rWaferMp)
        {
            double patternCenterDistanceUm = _AlignC.PatternCenterDistanceUm / 2;

            RotationTranslation rotat = new RotationTranslation();
            double lwx = lWaferMp.X * GV.ZoomLensInfo.LeftDownUmPerPixelX;
            double lwy = lWaferMp.Y * GV.ZoomLensInfo.LeftDownUmPerPixelY;
            double rwx = rWaferMp.X * GV.ZoomLensInfo.RightDownUmPerPixelX;
            double rwy = rWaferMp.Y * GV.ZoomLensInfo.RightDownUmPerPixelY;
            double lmx = lMaskMp.X * GV.ZoomLensInfo.LeftDownUmPerPixelX;
            double lmy = lMaskMp.Y * GV.ZoomLensInfo.LeftDownUmPerPixelY;
            double rmx = rMaskMp.X * GV.ZoomLensInfo.RightDownUmPerPixelX;
            double rmy = rMaskMp.Y * GV.ZoomLensInfo.RightDownUmPerPixelY;

            PointD pLw = new PointD(lwx, lwy) + new PointD(-patternCenterDistanceUm, 0);
            PointD pRw = new PointD(rwx, rwy) + new PointD(patternCenterDistanceUm, 0);
            PointD pLm = new PointD(lmx, lmy) + new PointD(-patternCenterDistanceUm, 0);
            PointD pRm = new PointD(rmx, rmy) + new PointD(patternCenterDistanceUm, 0);
            LineD lineW = new LineD(pLw, pRw);
            LineD lineM = new LineD(pLm, pRm);
            rotat.Translation = lineM.Center - lineW.Center;

            rotat.Rotation = lineM.Theta - lineW.Theta;

            return rotat;
        }

        private void btCaptureN_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(GM.SaveCaptureImage(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab()) + " " + GV.Dlang.strSave, GV.Dlang.strSave + "Pattern too ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            GM.SaveRecipeTemplate(_recipe, 2);
            try
            {
                string trainBasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Python", "Train");
                string recipePath = Path.Combine(trainBasePath, $"{EditRecipe}");
                if (!Directory.Exists(recipePath))
                {
                    Directory.CreateDirectory(recipePath);
                }
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string leftImagePath = Path.Combine(recipePath, $"Left_{timestamp}.bmp");
                string rightImagePath = Path.Combine(recipePath, $"Right_{timestamp}.bmp");
                Cv2.ImWrite(leftImagePath, GV.LeftBackCam.Grab());
                Cv2.ImWrite(rightImagePath, GV.RightBackCam.Grab());
            }
            catch
            {
            }
        }

        private void BtSave_Click(object sender, EventArgs e)
        {
            _AlignC.LastModifyTime = DateTime.Now;
            GM.WriteRecipeXml(EditRecipe);
        }

        private void btBackLabelPatternL_Click(object sender, EventArgs e)
        {
            DialogLabelImage labelImage = new DialogLabelImage()
            {

            };
            if (labelImage.ShowDialog() == DialogResult.OK)
            {

            }
            else
            {

            }
        }

        private void btBackLabelPatternR_Click(object sender, EventArgs e)
        {
            DialogLabelImage labelImage = new DialogLabelImage()
            {

            };
            if (labelImage.ShowDialog() == DialogResult.OK)
            {

            }
            else
            {

            }
        }

        private void buttonL_Click(object sender, EventArgs e)
        {
            ReadViewMat(0);

        }

        private void ReadViewMat(int iRL)
        {
            OpenFileDialogRead.InitialDirectory = Environment.CurrentDirectory;

            if (OpenFileDialogRead.ShowDialog() != DialogResult.OK) return;

            string filePath = OpenFileDialogRead.FileName;

            if (iRL == 0)
            {
                GV.LeftBackCam.Freeze();
                Thread.Sleep(250);
                lock (_locker)
                {
                    LViewMat = Cv2.ImRead(filePath, ImreadModes.Grayscale);

                }
                if (LViewMat.Empty())
                {
                    Console.WriteLine("圖像讀取失敗!");
                }
                GV.LeftBackCam.IsFileImage = true;
                GV.LeftBackCam.SetSimulationImage(LViewMat);
                skLeft.SetImage(LViewMat);
                GV.LeftBackCam.Live();
            }
            else
            {
                GV.RightBackCam.Freeze();
                Thread.Sleep(250);
                lock (_locker)
                {
                    RViewMat = Cv2.ImRead(filePath, ImreadModes.Grayscale);
                }

                if (RViewMat.Empty())
                {
                    Console.WriteLine("圖像讀取失敗!");
                }

                skRight.SetImage(RViewMat);
                GV.RightBackCam.SetSimulationImage(RViewMat);
                GV.RightBackCam.IsFileImage = true;
                GV.RightBackCam.Live();
            }
        }


        private void buttonR_Click(object sender, EventArgs e)
        {
            ReadViewMat(1);
        }

        private void buttonF_Click(object sender, EventArgs e)
        {
            isLive = false;
            //BtFile.Enabled = false;
            //BtLive.Enabled = true;
            Thread thWorkImage = new Thread(WorkingImage);
            thWorkImage.Start();
            GV.LeftBackCam.IsFileImage = true;
            GV.RightBackCam.IsFileImage = true;
            lbEmulationModeR.Visible = false;
            lbEmulationModeL.Visible = false;
        }
        private void WorkingImage()
        {
            while (!isLive)
            {

                Thread.Sleep(30);
            }
        }
        public void AutoFocusBinary(SentechNetToMat cam, int iLeftRight)
        {
            int nowLocZ;
            double bestFocus = 0;
            GV.Plc.GetLocation();
            int MinPosition;
            int MaxPosition;
            int leftNowLoc = GV.NowLocation[GV.Plc.iiDDownLeftZ];
            int rightNowLoc = GV.NowLocation[GV.Plc.iiDDownRightZ];
            Mat mat;

            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, true);

            if (iLeftRight == 0)
            {
                nowLocZ = GV.NowLocation[GV.Plc.iiDDownLeftZ];
                MinPosition = GV.Plc.iLimit[GV.Plc.iLimitLeftDownZmin];
                MaxPosition = GV.Plc.iLimit[GV.Plc.iLimitLeftDownZmax];
            }
            else
            {
                nowLocZ = GV.NowLocation[GV.Plc.iiDDownRightZ];
                MinPosition = GV.Plc.iLimit[GV.Plc.iLimitRightDownZmin];
                MaxPosition = GV.Plc.iLimit[GV.Plc.iLimitRightDownZmax];
            }
            int bestPosition = nowLocZ;

            while (MaxPosition - MinPosition > 100) // 假設 100 是最小步進量
            {
                int mid1 = MinPosition + (MaxPosition - MinPosition) / 3;
                int mid2 = MaxPosition - (MaxPosition - MinPosition) / 3;

                double focusValue1, focusValue2;

                if (iLeftRight == 0)
                {
                    GV.Plc.AlignDownCameraMoveZAbsW(0, 0, 0, 0, 0, mid1, rightNowLoc);
                    System.Threading.Thread.Sleep(100); // 等待機構穩定
                    mat = GV.LeftBackCam.Grab();
                    focusValue1 = CalculateFocus(mat);

                    GV.Plc.AlignDownCameraMoveZAbsW(0, 0, 0, 0, 0, mid2, rightNowLoc);
                    System.Threading.Thread.Sleep(100); // 等待機構穩定
                    mat = GV.LeftBackCam.Grab();
                    focusValue2 = CalculateFocus(mat);
                }
                else
                {
                    GV.Plc.AlignDownCameraMoveZAbsW(0, 0, 0, 0, 0, leftNowLoc, mid1);
                    System.Threading.Thread.Sleep(100); // 等待機構穩定
                    mat = GV.RightBackCam.Grab();
                    focusValue1 = CalculateFocus(mat);

                    GV.Plc.AlignDownCameraMoveZAbsW(0, 0, 0, 0, 0, leftNowLoc, mid2);
                    System.Threading.Thread.Sleep(100); // 等待機構穩定
                    mat = GV.RightBackCam.Grab();
                    focusValue2 = CalculateFocus(mat);
                }

                if (focusValue1 > focusValue2)
                {
                    MaxPosition = mid2;
                    if (focusValue1 > bestFocus)
                    {
                        bestFocus = focusValue1;
                        bestPosition = mid1;
                    }
                }
                else
                {
                    MinPosition = mid1;
                    if (focusValue2 > bestFocus)
                    {
                        bestFocus = focusValue2;
                        bestPosition = mid2;
                    }
                }
            }

            if (iLeftRight == 0)
            {
                GV.Plc.AlignDownCameraMoveZAbsW(0, 0, 0, 0, 0, bestPosition, rightNowLoc);
            }
            else
            {
                GV.Plc.AlignDownCameraMoveZAbsW(0, 0, 0, 0, 0, leftNowLoc, bestPosition);
            }

            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, false);
        }

        public void AutoFocus(SentechNetToMat cam, int iLeftRight)
        {
            int nowLocZ;
            double bestFocus = 0;
            GV.Plc.GetLocation();
            int MinPosition;
            int MaxPosition;
            int leftNowLoc = GV.NowLocation[GV.Plc.iiDDownLeftZ];
            int rightNowLoc = GV.NowLocation[GV.Plc.iiDDownRightZ];
            Mat mat;

            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, true);

            if (iLeftRight == 0)
            {
                nowLocZ = GV.NowLocation[GV.Plc.iiDDownLeftZ];
                MinPosition = GV.Plc.iLimit[GV.Plc.iLimitLeftDownZmin];
                MaxPosition = GV.Plc.iLimit[GV.Plc.iLimitLeftDownZmax];
            }
            else
            {
                nowLocZ = GV.NowLocation[GV.Plc.iiDDownRightZ];
                MinPosition = GV.Plc.iLimit[GV.Plc.iLimitRightDownZmin];
                MaxPosition = GV.Plc.iLimit[GV.Plc.iLimitRightDownZmax];
            }
            int bestPosition = nowLocZ;

            for (double pos = MinPosition; pos <= MaxPosition; pos += 1000) // 假設 0.1 是步進量
            {
                if (iLeftRight == 0)
                {
                    GV.Plc.AlignDownCameraMoveZAbsW(0, 0, 0, 0, 0, pos, rightNowLoc);
                    System.Threading.Thread.Sleep(100); // 等待機構穩定
                    mat = GV.LeftBackCam.Grab();
                }
                else
                {
                    GV.Plc.AlignDownCameraMoveZAbsW(0, 0, 0, 0, 0, leftNowLoc, pos);
                    System.Threading.Thread.Sleep(100); // 等待機構穩定
                    mat = GV.RightBackCam.Grab();
                }

                double focusValue = CalculateFocus(mat);

                if (focusValue > bestFocus)
                {
                    bestFocus = focusValue;
                    bestPosition = (int)pos;
                }
            }

            if (iLeftRight == 0)
            {
                GV.Plc.AlignDownCameraMoveZAbsW(0, 0, 0, 0, 0, bestPosition, rightNowLoc);
            }
            else
            {
                GV.Plc.AlignDownCameraMoveZAbsW(0, 0, 0, 0, 0, leftNowLoc, bestPosition);
            }

            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, false);
        }
        public void ShowGroupBox4(bool visible)
        {
            groupBox4.Visible = visible;
        }
        private GrayImage CropGrayImage(GrayImage src, Rect roi)
        {
            // 確保 ROI 在有效範圍內
            int x = Math.Max(0, roi.X);
            int y = Math.Max(0, roi.Y);
            int width = Math.Min(roi.Width, src.Width - x);
            int height = Math.Min(roi.Height, src.Height - y);

            GrayImage result = new GrayImage(width, height);

            for (int row = 0; row < height; row++)
            {
                int srcIndex = (y + row) * src.Width + x;
                int dstIndex = row * width;
                Array.Copy(src.Bits, srcIndex, result.Bits, dstIndex, width);
            }

            return result;
        }

        private void ucBackPosition_Click(object sender, EventArgs e)
        {
            if (locationUpdateTimer == null)
            {
                // 如果 Timer 未初始化，先初始化
                locationUpdateTimer = new System.Windows.Forms.Timer();
                locationUpdateTimer.Interval = 500;
                locationUpdateTimer.Tick += LocationUpdateTimer_Tick;
            }

            if (!locationUpdateTimer.Enabled)
            {
                // ✅ 1. 開啟位置顯示
                groupBoxB1.Visible = true;  // 顯示 CCD Location (Now Location 欄位)
                groupBoxB2.Visible = true;  // 顯示 WEC Location (Now Location 欄位)
                locationUpdateTimer.Start();
                ucBackPosition.Caption = "Close Position";

                // 立即更新一次位置資訊
                UpdateCurrentLocationUI();
            }
            else
            {
                // ✅ 2. 關閉位置顯示
                locationUpdateTimer.Stop();
                groupBoxB1.Visible = false;  // 隱藏 CCD Location
                groupBoxB2.Visible = false;  // 隱藏 WEC Location
                ucBackPosition.Caption = "Open Position";
            }
        }

        private void SetComboBoxByClassId(ComboBox comboBox, int classId)
        {
            Dictionary<string, Int16> ClassList = GV.AIClassList.GetClassList();
            string className = ClassList.FirstOrDefault(x => x.Value == classId).Key;
            if (!string.IsNullOrEmpty(className))
            {
                int index = comboBox.Items.IndexOf(className);
                if (index >= 0)
                {
                    comboBox.SelectedIndex = index;
                }
                else
                {
                    comboBox.SelectedIndex = -1;
                }
            }
            else
            {
                comboBox.SelectedIndex = -1;
            }
        }
        public void OnPageLeave()
        {
            try
            {
                // 根據當前選擇的 RadioButton 更新對應的亮度值到 AlignC
                if (rbLTopMask.Checked)
                {
                    _AlignC.LeftBrightness[NowMagni] = tBLeftTopMaskLight.Value;
                    _AlignC.RightBrightness[NowMagni] = tBRightTopMaskLight.Value;
                }
                else if (rbLBackMask.Checked)
                {
                    _AlignC.LBMaskBright = tBLeftBackMaskLight.Value;
                    _AlignC.RBMaskBright = tBRightBackMaskLight.Value;
                }
                else if (rbLWafer.Checked)
                {
                    _AlignC.LBWaferBright = tBLeftBackWaferLight.Value;
                    _AlignC.RBWaferBright = tBRightBackWaferLight.Value;
                }

                _AlignC.dLalpha = (double)tBLShowImage.Value / 100;
                _AlignC.dRalpha = (double)tBRShowImage.Value / 100;

                // 停止 DrawMatchPosition 執行緒
                isLive = false;

                // 儲存到 XML
                _AlignC.LastModifyTime = DateTime.Now;
                GM.WriteRecipeXml(EditRecipe);

                GM.WriteToStatusTextBox1(iAdmin, "已自動儲存底部光源亮度設定");
            }
            catch (Exception ex)
            {
                GM.WriteToStatusTextBox($"儲存亮度設定時發生錯誤: {ex.Message}");
            }
            if (locationUpdateTimer != null)
            {
                locationUpdateTimer.Stop();
            }
        }
    }
}
