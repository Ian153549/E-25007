using MRLibrary;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections;
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
using System.Windows.Interop;
using System.Xml;
using static System.Windows.Forms.AxHost;

namespace NSAA_16Axis
{
    public partial class CMLearnPatternUp : UserControl
    {
        public int EditRecipe;
        public int NowMageni;
        public int iFirstRun = 0;
        public int iDelayT = 100;

        public int _LeftRingBrightness;
        public int _RightRingBrightness;
        public int _LeftBrightness;
        public int _RightBrightness;
        private string _ClockTime;
        private readonly object _locker = new object();
        public bool isLive = true;

        private System.Windows.Forms.Timer locationUpdateTimer;
        private bool isLocationUpdateEnabled = false;

        private int _lastShowRecipe = -1;
        private int _lastLearnRecipe = -1;
        private int _lastClassListRecipe = -1;
        private int _showMeBusy = 0;

        public Thread MatchThread = null;
        public AlignCondition _AlignC = null;
        public Recipe _recipe = null;
        public Mat MoveCCDLeft = null;
        public Mat MoveCCDRight = null;
        public Mat GMPLeft;
        public Mat GMPRight;
        public Mat LTViewMat;
        public Mat RTViewMat;

        public MatchPosition GMPlMaskMp;
        public MatchPosition GMPrMaskMp;
        public MatchPosition GMPlWaferMp;
        public MatchPosition GMPrWaferMp;

        public Stack RollbackStack = new Stack();
        public int DebugMsgCount = 0;

        public MatchPosition LMaskMp = new MatchPosition();
        public MatchPosition LWaferMp = new MatchPosition();
        public MatchPosition RMaskMp = new MatchPosition();
        public MatchPosition RWaferMp = new MatchPosition();

        private int _locationRefreshBusy = 0;
        private bool _runtimeInitialized = false;

        public CMLearnPatternUp()
        {
            InitializeComponent();
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime || DesignMode)
                return;
            //GV.LeftUpWindowOnLearnPage = skLPattern;
            //GV.RightUpWindowOnLearnPage = skRPattern;
            //skLPattern.MaskMp = LMaskMp;
            //skLPattern.WaferMp = LWaferMp;
            //skRPattern.MaskMp = RMaskMp;
            //skRPattern.WaferMp = RWaferMp;
            //SetStyle(ControlStyles.UserPaint, true);
            //SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            //SetStyle(ControlStyles.DoubleBuffer, true);
            //skLPattern.CanRectMaskAndWafer = true;
            //skLPattern.CanTrackPattern = true;
            //skLPattern.CanZoom = true;
            //skLPattern.CanCenterLine = true;
            //skRPattern.CanRectMaskAndWafer = true;
            //skRPattern.CanTrackPattern = true;
            //skRPattern.CanZoom = true;
            //skRPattern.CanCenterLine = true;

            //locationUpdateTimer = new System.Windows.Forms.Timer();
            //locationUpdateTimer.Interval = 500; // 每 500ms 更新一次
            //locationUpdateTimer.Tick += LocationUpdateTimer_Tick;

            //// 初始狀態：隱藏 groupBox
            //groupBoxT4.Visible = false;
            //groupBoxT1.Visible = false;
        }

        private void CMLearnPattern_Load(object sender, EventArgs e)
        {
            if (GV.AppSettingParm.DebugMode)
            {
                lbDebugMsg.Visible = true;
            }

            EditRecipe = GV.NowRecipeNumber;
            _recipe = GV._recipe;
            _AlignC = _recipe.AlignC;
            cbPatterhShift.SelectedIndex = 4;
            cbLWaferAlgorithm.SelectedIndex = 0;
            cbRWaferAlgorithm.SelectedIndex = 0;
            cbLMaskAlgorithm.SelectedIndex = 0;
            cbRMaskAlgorithm.SelectedIndex = 0;
            if (GV.AppSettingParm.Language != "default")
            {
                btLWaferLoactionSave.Text = GV.Dlang.strSave;
                btLMaskLocationSave.Text = GV.Dlang.strSave;
                btRWaferLocationSave.Text = GV.Dlang.strSave;
                btRMaskLocationSave.Text = GV.Dlang.strSave;
                btMagSaveLow.Text = GV.Dlang.strSave;
                btMagSaveHigh.Text = GV.Dlang.strSave;
                btDelete.Text = GV.Dlang.strDelete;
                btDefault.Text = GV.Dlang.strDefault;
                gbLWafer.Text = GV.Dlang.strWafer;
                gbRWafer.Text = GV.Dlang.strWafer;
                gbLMask.Text = GV.Dlang.strMask;
                gbRMask.Text = GV.Dlang.strMask;
                gBMag.Text = GV.Dlang.groupBoxMagnification;
                rBHighMagnification.Text = GV.Dlang.radioButtonHighMagnification;
                rBLowMagnification.Text = GV.Dlang.radioButtonLowMagnification;
                // lbOrgRecipe.Text = GV.Dlang.lbOrgRecipe;
                // btRecipeCopy.Text = GV.Dlang.btRecipeCopy;
                label1.Text = GV.Dlang.strAlgo;
                label2.Text = GV.Dlang.strAlgo;
                label3.Text = GV.Dlang.strAlgo;
                label4.Text = GV.Dlang.strAlgo;
                LRingLight.Text = GV.Dlang.strRingLight;
                RRingLight.Text = GV.Dlang.strRingLight;
                LCoaLight.Text = GV.Dlang.strCoaLight;
                RCoaLight.Text = GV.Dlang.strCoaLight;
                cbPatterhShift.Items.Clear();
                cbPatterhShift.Items.Add(GV.Dlang.strDown);
                cbPatterhShift.Items.Add(GV.Dlang.strUp);
                cbPatterhShift.Items.Add(GV.Dlang.strLeft);
                cbPatterhShift.Items.Add(GV.Dlang.strRight);
                cbPatterhShift.Items.Add(GV.Dlang.strCenter);
                btCreateLMaskPatternMask.Text = GV.Dlang.strCreateMark;
                btCreateRMaskPatternMask.Text = GV.Dlang.strCreateMark;
                btCreateLWaferPatternMask.Text = GV.Dlang.strCreateMark;
                btCreateRWaferPatternMask.Text = GV.Dlang.strCreateMark;
                btFindRWaferCenter.Text = GV.Dlang.strFindCenter;
                btFindLWaferCenter.Text = GV.Dlang.strFindCenter;
                btFindRMaskCenter.Text = GV.Dlang.strFindCenter;
                btFindLMaskCenter.Text = GV.Dlang.strFindCenter;
                btAlignTest.Caption = GV.Dlang.strAlignTest;
                btCapture.Caption = GV.Dlang.strCapture;
                btLabelPatternL.Text = GV.Dlang.strImageLabel;
                btLabelPatternR.Text = GV.Dlang.strImageLabel;
                btChuckAlignN.Caption = GV.Dlang.strChuckAlign;
                cbLWaferAlgorithm.Items.Clear();
                cbRWaferAlgorithm.Items.Clear();
                cbLMaskAlgorithm.Items.Clear();
                cbRMaskAlgorithm.Items.Clear();
                cbLWaferAlgorithm.Items.Add(GV.Dlang.strTemplate);
                cbLWaferAlgorithm.Items.Add(GV.Dlang.strEdge);
                cbLWaferAlgorithm.Items.Add(GV.Dlang.strAI);
                cbRWaferAlgorithm.Items.Add(GV.Dlang.strTemplate);
                cbRWaferAlgorithm.Items.Add(GV.Dlang.strEdge);
                cbRWaferAlgorithm.Items.Add(GV.Dlang.strAI);
                cbLMaskAlgorithm.Items.Add(GV.Dlang.strTemplate);
                cbLMaskAlgorithm.Items.Add(GV.Dlang.strEdge);
                cbLMaskAlgorithm.Items.Add(GV.Dlang.strAI);
                cbRMaskAlgorithm.Items.Add(GV.Dlang.strTemplate);
                cbRMaskAlgorithm.Items.Add(GV.Dlang.strEdge);
                cbRMaskAlgorithm.Items.Add(GV.Dlang.strAI);

            }

            NowMageni = 1;
            // cbLowMagnification.SelectedIndex = _AlignC.AlignLowMagnification;
            // cbHighMagnification.SelectedIndex = _AlignC.AlignHighMagnification;
            // cbPatterhShift.SelectedIndex = _AlignC.PatternShift;
            rBLowMagnification.Checked = true;
            if (GV.AppSettingParm.LeftUpCamEnable == true)
            {
                lbEmulationModeL.Visible = false;
            }
            else
            {
                skLPattern.CanRectMaskAndWafer = false;
            }
            if (GV.AppSettingParm.RightUpCamEnable == true)
            {
                lbEmulationModeR.Visible = false;
            }
            else
            {
                skRPattern.CanRectMaskAndWafer = false;
            }

            skLPattern.WaferMp = LWaferMp;
            skLPattern.MaskMp = LMaskMp;
            skRPattern.WaferMp = RWaferMp;
            skRPattern.MaskMp = RMaskMp;
            groupBoxT4.Visible = false;
            groupBoxT1.Visible = false;
            isLocationUpdateEnabled = false;
            locationUpdateTimer.Stop();
            //locationUpdateTimer = new System.Windows.Forms.Timer();
            //locationUpdateTimer.Interval = 500; // 每 500ms 更新一次
            //locationUpdateTimer.Tick += LocationUpdateTimer_Tick;
            //locationUpdateTimer.Start();

            //UpdateTargetPositionDisplay();
        }

        public void InitializeRuntime()
        {
            if (_runtimeInitialized)
                return;

            _runtimeInitialized = true;

            GV.LeftUpWindowOnLearnPage = skLPattern;
            GV.RightUpWindowOnLearnPage = skRPattern;

            skLPattern.MaskMp = LMaskMp;
            skLPattern.WaferMp = LWaferMp;
            skRPattern.MaskMp = RMaskMp;
            skRPattern.WaferMp = RWaferMp;

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            skLPattern.CanRectMaskAndWafer = true;
            skLPattern.CanTrackPattern = true;
            skLPattern.CanZoom = true;
            skLPattern.CanCenterLine = true;

            skRPattern.CanRectMaskAndWafer = true;
            skRPattern.CanTrackPattern = true;
            skRPattern.CanZoom = true;
            skRPattern.CanCenterLine = true;

            locationUpdateTimer = new System.Windows.Forms.Timer();
            locationUpdateTimer.Interval = 500;
            locationUpdateTimer.Tick += LocationUpdateTimer_Tick;

            groupBoxT4.Visible = false;
            groupBoxT1.Visible = false;
        }

        private async void LocationUpdateTimer_Tick(object sender, EventArgs e)
        {
            if (!isLocationUpdateEnabled) return;
            await RefreshCurrentLocationAsync();
        }

        private async Task RefreshCurrentLocationAsync()
        {
            if (Interlocked.Exchange(ref _locationRefreshBusy, 1) == 1) return;
            try
            {
                await Task.Run(() =>
                {
                    GV.Plc.GetLocation();
                });
                int[] location = (int[])GV.NowLocation.Clone();
                SafeBeginInvoke(() =>
                {
                    ApplyCurrentLocationToUI(location);
                });
            }
            catch (Exception ex)
            {
                SafeBeginInvoke(() =>
                 {
                     locationUpdateTimer.Stop();
                     isLocationUpdateEnabled = false;
                     GM.WriteToStatusTextBox($"更新位置失敗: {ex.Message}");
                 });
            }
            finally
            {
                Interlocked.Exchange(ref _locationRefreshBusy, 0);
            }
        }

        private void ApplyCurrentLocationToUI(int[] location)
        {
            nudReadChuckZ.Value = (decimal)(location[GV.Plc.iiDChuckZ] / 10.0);
            nudReadChuckX.Value = (decimal)(location[GV.Plc.iiDChuckX] / 10.0);
            nudReadChuckY1.Value = (decimal)(location[GV.Plc.iiDChuckY1] / 10.0);
            nudReadChuckY2.Value = (decimal)(location[GV.Plc.iiDChuckY2] / 10.0);
            nudReadBigY.Value = (decimal)(location[GV.Plc.iiDUpBigY] / 10.0);
            nudReadUpLeftX.Value = (decimal)(location[GV.Plc.iiDUpLeftX] / 10.0);
            nudReadUpLeftY.Value = (decimal)(location[GV.Plc.iiDUpLeftY] / 10.0);
            nudReadUpLeftZ.Value = (decimal)(location[GV.Plc.iiDSUpLeftZ] / 10.0);
            nudReadUpRightX.Value = (decimal)(location[GV.Plc.iiDUpRightX] / 10.0);
            nudReadUpRightY.Value = (decimal)(location[GV.Plc.iiDUpRightY] / 10.0);
            nudReadUpRightZ.Value = (decimal)(location[GV.Plc.iiDSUpRightZ] / 10.0);
        }

        private void UpdateCurrentLocationUI()
        {
            //try
            //{
            //    // 從 PLC 讀取當前位置
            //    GV.Plc.GetLocation();

            //    // 更新 UI（使用 InvokeRequired 確保執行緒安全）
            //    if (InvokeRequired)
            //    {
            //        Invoke(new Action(UpdateCurrentLocationUI));
            //        return;
            //    }

            //    //  將 PLC 數值除以 10 後顯示 - WEC Location
            //    nudReadChuckZ.Value = (decimal)(GV.NowLocation[GV.Plc.iiDChuckZ] / 10.0);
            //    nudReadChuckX.Value = (decimal)(GV.NowLocation[GV.Plc.iiDChuckX] / 10.0);
            //    nudReadChuckY1.Value = (decimal)(GV.NowLocation[GV.Plc.iiDChuckY1] / 10.0);
            //    nudReadChuckY2.Value = (decimal)(GV.NowLocation[GV.Plc.iiDChuckY2] / 10.0);

            //    //  將 PLC 數值除以 10 後顯示 - CCD Location
            //    nudReadBigY.Value = (decimal)(GV.NowLocation[GV.Plc.iiDUpBigY] / 10.0);
            //    nudReadUpLeftX.Value = (decimal)(GV.NowLocation[GV.Plc.iiDUpLeftX] / 10.0);
            //    nudReadUpLeftY.Value = (decimal)(GV.NowLocation[GV.Plc.iiDUpLeftY] / 10.0);
            //    nudReadUpLeftZ.Value = (decimal)(GV.NowLocation[GV.Plc.iiDSUpLeftZ] / 10.0);
            //    nudReadUpRightX.Value = (decimal)(GV.NowLocation[GV.Plc.iiDUpRightX] / 10.0);
            //    nudReadUpRightY.Value = (decimal)(GV.NowLocation[GV.Plc.iiDUpRightY] / 10.0);
            //    nudReadUpRightZ.Value = (decimal)(GV.NowLocation[GV.Plc.iiDSUpRightZ] / 10.0);
            //}
            //catch (Exception ex)
            //{
            //    // 發生錯誤時停止計時器
            //    locationUpdateTimer.Stop();
            //    isLocationUpdateEnabled = false;
            //    GM.WriteToStatusTextBox($"更新位置失敗: {ex.Message}");
            //}
            _ = RefreshCurrentLocationAsync();
        }
        private void UpdateTargetPositionDisplay()
        {
            if (_recipe == null) return;

            if (rBLowMagnification.Checked)
            {
                // WEC Location - 低倍率目標位置
                var wecPos = _recipe.UpperLowResWECPos;
                nudWriteChuckZ.Text = wecPos.Z.ToString("F0"); // WEC Z 需確認對應欄位
                nudWriteChuckX.Text = wecPos.X.ToString("F0");
                nudWriteChuckY1.Text = wecPos.Y.ToString("F0");
                nudWriteChuckY2.Text = wecPos.A.ToString("F0");

                // CCD Location - 低倍率目標位置
                var ccdPos = _recipe.UpperMLowResPosition;
                nudWriteUpBigY.Text = ccdPos.Y.ToString("F0");
                nudWriteUpLeftX.Text = ccdPos.XL.ToString("F0");
                nudWriteUpLeftY.Text = ccdPos.YL.ToString("F0");
                nudWriteUpLeftZ.Text = ccdPos.ZL.ToString("F0");
                nudWriteUpRightX.Text = ccdPos.XR.ToString("F0");
                nudWriteUpRightY.Text = ccdPos.YR.ToString("F0");
                nudWriteUpRightZ.Text = ccdPos.ZR.ToString("F0");
            }
            else if (rBHighMagnification.Checked)
            {
                // WEC Location - 高倍率目標位置
                var wecPos = _recipe.UpperWECPos;
                nudWriteChuckZ.Text = wecPos.Z.ToString("F0");
                nudWriteChuckX.Text = wecPos.X.ToString("F0");
                nudWriteChuckY1.Text = wecPos.Y.ToString("F0");
                nudWriteChuckY2.Text = wecPos.A.ToString("F0");

                // CCD Location - 高倍率目標位置
                var ccdPos = _recipe.UpperMPosition;
                nudWriteUpBigY.Text = ccdPos.Y.ToString("F0");
                nudWriteUpLeftX.Text = ccdPos.XL.ToString("F0");
                nudWriteUpLeftY.Text = ccdPos.YL.ToString("F0");
                nudWriteUpLeftZ.Text = ccdPos.ZL.ToString("F0");
                nudWriteUpRightX.Text = ccdPos.XR.ToString("F0");
                nudWriteUpRightY.Text = ccdPos.YR.ToString("F0");
                nudWriteUpRightZ.Text = ccdPos.ZR.ToString("F0");
            }
        }
        public void DrawMatchPosition()
        {
            while (!GV.AppEnding)
            {
                while (GV.TabOption == GV.Tab.Learn)
                {
                    if (GV.AppSettingParm.Emulation == true || GV.matcherLLM == null)
                    {
                        Thread.Sleep(2000);
                        continue;
                    }
                    try
                    {
                        Mat leftFullImage = GV.LeftUpCam.Grab();
                        Mat rightFullImage = GV.RightUpCam.Grab();

                        if (rBLowMagnification.Checked)
                        {
                            int lWaferClassId = _AlignC.LLWaferAIClassId;
                            int rWaferClassId = _AlignC.RLWaferAIClassId;
                            int lMaskClassId = _AlignC.LLMaskAIClassId;
                            int rMaskClassId = _AlignC.RLMaskAIClassId;

                            Invoke((MethodInvoker)delegate ()
                            {
                                if (_AlignC.LLWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && cbLWaferClassList.SelectedIndex >= 0)
                                {
                                    lWaferClassId = cbLWaferClassList.SelectedIndex;
                                }
                                if (_AlignC.RLWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && cbRWaferClassList.SelectedIndex >= 0)
                                {
                                    rWaferClassId = cbRWaferClassList.SelectedIndex;
                                }
                                if (_AlignC.LLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && cbLMaskClassList.SelectedIndex >= 0)
                                {
                                    lMaskClassId = cbLMaskClassList.SelectedIndex;
                                }
                                if (_AlignC.RLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && cbRMaskClassList.SelectedIndex >= 0)
                                {
                                    rMaskClassId = cbRMaskClassList.SelectedIndex;
                                }
                            });

                            //  左低倍率光罩 - 使用完整影像搜尋
                            if (_AlignC.LLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
                            {
                                GV.matcherLLM.MatMatchWithAlgo(0, leftFullImage, ref LMaskMp, _AlignC.LLMaskAlgorithm, lMaskClassId);
                            }
                            else
                            {
                                GV.matcherLLM.MatMatchWithAlgo(0, leftFullImage, ref LMaskMp, _AlignC.LLMaskAlgorithm);
                            }

                            //  套用偏移量校正座標
                            LMaskMp.X += _AlignC.LLMaskOffsetX;
                            LMaskMp.Y += _AlignC.LLMaskOffsetY;

                            //  左低倍率晶圓 - 使用完整影像搜尋
                            if (_AlignC.LLWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
                            {
                                GV.matcherLLW.MatMatchWithAlgo(0, leftFullImage, ref LWaferMp, _AlignC.LLWaferAlgorithm, lWaferClassId);
                            }
                            else
                            {
                                GV.matcherLLW.MatMatchWithAlgo(0, leftFullImage, ref LWaferMp, _AlignC.LLWaferAlgorithm);
                            }

                            //  套用偏移量校正座標
                            LWaferMp.X += _AlignC.LLWaferOffsetX;
                            LWaferMp.Y += _AlignC.LLWaferOffsetY;

                            //  右低倍率光罩 - 使用完整影像搜尋
                            if (_AlignC.RLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
                            {
                                GV.matcherRLM.MatMatchWithAlgo(0, rightFullImage, ref RMaskMp, _AlignC.RLMaskAlgorithm, rMaskClassId);
                            }
                            else
                            {
                                GV.matcherRLM.MatMatchWithAlgo(0, rightFullImage, ref RMaskMp, _AlignC.RLMaskAlgorithm);
                            }

                            //  套用偏移量校正座標
                            RMaskMp.X += _AlignC.RLMaskOffsetX;
                            RMaskMp.Y += _AlignC.RLMaskOffsetY;

                            //  右低倍率晶圓 - 使用完整影像搜尋
                            if (_AlignC.RLWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
                            {
                                GV.matcherRLW.MatMatchWithAlgo(0, rightFullImage, ref RWaferMp, _AlignC.RLWaferAlgorithm, rWaferClassId);
                            }
                            else
                            {
                                GV.matcherRLW.MatMatchWithAlgo(0, rightFullImage, ref RWaferMp, _AlignC.RLWaferAlgorithm);
                            }

                            //  套用偏移量校正座標
                            RWaferMp.X += _AlignC.RLWaferOffsetX;
                            RWaferMp.Y += _AlignC.RLWaferOffsetY;
                        }

                        if (rBHighMagnification.Checked)
                        {
                            int lWaferClassId = _AlignC.LHWaferAIClassId;
                            int rWaferClassId = _AlignC.RHWaferAIClassId;
                            int lMaskClassId = _AlignC.LHMaskAIClassId;
                            int rMaskClassId = _AlignC.RHMaskAIClassId;

                            Invoke((MethodInvoker)delegate ()
                            {
                                if (_AlignC.LHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && cbLWaferClassList.SelectedIndex >= 0)
                                {
                                    lWaferClassId = cbLWaferClassList.SelectedIndex;
                                }
                                if (_AlignC.RHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && cbRWaferClassList.SelectedIndex >= 0)
                                {
                                    rWaferClassId = cbRWaferClassList.SelectedIndex;
                                }
                                if (_AlignC.LHMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && cbLMaskClassList.SelectedIndex >= 0)
                                {
                                    lMaskClassId = cbLMaskClassList.SelectedIndex;
                                }
                                if (_AlignC.RHMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && cbRMaskClassList.SelectedIndex >= 0)
                                {
                                    rMaskClassId = cbRMaskClassList.SelectedIndex;
                                }
                            });

                            //  左高倍率光罩 - 使用完整影像搜尋
                            if (_AlignC.LHMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
                            {
                                GV.matcherLHM.MatMatchWithAlgo(0, leftFullImage, ref LMaskMp, _AlignC.LHMaskAlgorithm, lMaskClassId);
                            }
                            else
                            {
                                GV.matcherLHM.MatMatchWithAlgo(0, leftFullImage, ref LMaskMp, _AlignC.LHMaskAlgorithm);
                            }

                            //  套用偏移量校正座標
                            LMaskMp.X += _AlignC.LHMaskOffsetX;
                            LMaskMp.Y += _AlignC.LHMaskOffsetY;

                            //  左高倍率晶圓 - 使用完整影像搜尋
                            if (_AlignC.LHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
                            {
                                GV.matcherLHW.MatMatchWithAlgo(0, leftFullImage, ref LWaferMp, _AlignC.LHWaferAlgorithm, lWaferClassId);
                            }
                            else
                            {
                                GV.matcherLHW.MatMatchWithAlgo(0, leftFullImage, ref LWaferMp, _AlignC.LHWaferAlgorithm);
                            }

                            //  套用偏移量校正座標
                            LWaferMp.X += _AlignC.LHWaferOffsetX;
                            LWaferMp.Y += _AlignC.LHWaferOffsetY;

                            //  右高倍率光罩 - 使用完整影像搜尋
                            if (_AlignC.RHMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
                            {
                                GV.matcherRHM.MatMatchWithAlgo(0, rightFullImage, ref RMaskMp, _AlignC.RHMaskAlgorithm, rMaskClassId);
                            }
                            else
                            {
                                GV.matcherRHM.MatMatchWithAlgo(0, rightFullImage, ref RMaskMp, _AlignC.RHMaskAlgorithm);
                            }

                            //  套用偏移量校正座標
                            RMaskMp.X += _AlignC.RHMaskOffsetX;
                            RMaskMp.Y += _AlignC.RHMaskOffsetY;

                            //  右高倍率晶圓 - 使用完整影像搜尋
                            if (_AlignC.RHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
                            {
                                GV.matcherRHW.MatMatchWithAlgo(0, rightFullImage, ref RWaferMp, _AlignC.RHWaferAlgorithm, rWaferClassId);
                            }
                            else
                            {
                                GV.matcherRHW.MatMatchWithAlgo(0, rightFullImage, ref RWaferMp, _AlignC.RHWaferAlgorithm);
                            }

                            //  套用偏移量校正座標
                            RWaferMp.X += _AlignC.RHWaferOffsetX;
                            RWaferMp.Y += _AlignC.RHWaferOffsetY;
                        }

                        //  更新 UI 顯示
                        //Invoke((MethodInvoker)delegate ()
                        //{
                        //    double dLX = skLPattern.MaskMp.X - skLPattern.WaferMp.X;
                        //    double dLY = skLPattern.MaskMp.Y - skLPattern.WaferMp.Y;
                        //    double dRX = skRPattern.MaskMp.X - skRPattern.WaferMp.X;
                        //    double dRY = skRPattern.MaskMp.Y - skRPattern.WaferMp.Y;

                        //    string msgLM = string.Format("LMX = {0:N3}, LMY = {1:N3}, dX = {2:N3}, dY = {3:N3}, MScore = {4:N2}",
                        //        skLPattern.MaskMp.X, skLPattern.MaskMp.Y, dLX, dLY, skLPattern.MaskMp.Score);
                        //    string msgLW = string.Format("LWX = {0:N3}, LWY = {1:N3}, WScore = {2:N2}",
                        //        skLPattern.WaferMp.X, skLPattern.WaferMp.Y, skLPattern.WaferMp.Score);
                        //    string msgRM = string.Format("RMX = {0:N3}, RMY = {1:N3}, dX = {2:N3}, dY = {3:N3}, MScore = {4:N2}",
                        //        skRPattern.MaskMp.X, skRPattern.MaskMp.Y, dRX, dRY, skRPattern.MaskMp.Score);
                        //    string msgRW = string.Format("RWX = {0:N3}, RWY = {1:N3}, WScore = {2:N2}",
                        //        skRPattern.WaferMp.X, skRPattern.WaferMp.Y, skRPattern.WaferMp.Score);

                        //    lbLMsgM.Text = msgLM;
                        //    lbRMsgM.Text = msgRM;
                        //    lbLMsgW.Text = msgLW;
                        //    lbRMsgW.Text = msgRW;

                        //    Update();
                        //});
                        SafeBeginInvoke(() =>
                        {
                            double dLX = skLPattern.MaskMp.X - skLPattern.WaferMp.X;
                            double dLY = skLPattern.MaskMp.Y - skLPattern.WaferMp.Y;
                            double dRX = skRPattern.MaskMp.X - skRPattern.WaferMp.X;
                            double dRY = skRPattern.MaskMp.Y - skRPattern.WaferMp.Y;

                            lbLMsgM.Text = string.Format(
                                "LMX = {0:N3}, LMY = {1:N3}, dX = {2:N3}, dY = {3:N3}, MScore = {4:N2}",
                                skLPattern.MaskMp.X,
                                skLPattern.MaskMp.Y,
                                dLX,
                                dLY,
                                skLPattern.MaskMp.Score);

                            lbLMsgW.Text = string.Format(
                                "LWX = {0:N3}, LWY = {1:N3}, WScore = {2:N2}",
                                skLPattern.WaferMp.X,
                                skLPattern.WaferMp.Y,
                                skLPattern.WaferMp.Score);

                            lbRMsgM.Text = string.Format(
                                "RMX = {0:N3}, RMY = {1:N3}, dX = {2:N3}, dY = {3:N3}, MScore = {4:N2}",
                                skRPattern.MaskMp.X,
                                skRPattern.MaskMp.Y,
                                dRX,
                                dRY,
                                skRPattern.MaskMp.Score);

                            lbRMsgW.Text = string.Format(
                                "RWX = {0:N3}, RWY = {1:N3}, WScore = {2:N2}",
                                skRPattern.WaferMp.X,
                                skRPattern.WaferMp.Y,
                                skRPattern.WaferMp.Score);

                            Invalidate();
                        });

                    }
                    catch (Exception ex)
                    {
                        WriteStatus(ex.Message);
                    }

                    Thread.Sleep(2000);
                }
                Thread.Sleep(2000);
            }
        }

        public void WriteStatus(string message)
        {
            Invoke((MethodInvoker)delegate { GM.WriteToStatusTextBox(message); });
        }

        //public void UpdateUI()
        //{
        //    if ((GV.NowRecipeNumber < 1) || (GV.NowRecipeNumber > 100))
        //    {
        //        GV.NowRecipeNumber = 1;

        //    }

        //    EditRecipe = GV.NowRecipeNumber;

        //    ChangeRecipe();

        //    lbRecipeNumber.Text = GV.Dlang.strRecipeNumber + EditRecipe.ToString();

        //    cbLowMagnification.SelectedIndex = _AlignC.AlignLowMagnification;
        //    cbHighMagnification.SelectedIndex = _AlignC.AlignHighMagnification;
        //    cbPatterhShift.SelectedIndex = _AlignC.PatternShift;

        //    if (GV.AppSettingParm.DebugMode)
        //    {
        //        lbDebugMsg.Text = "UpdateUI = " + _AlignC.AlignLowMagnification.ToString() + " EditRecipe = " + EditRecipe.ToString();
        //    }

        //    pbLMaskImage.Image = null;
        //    pbLWaferImage.Image = null;
        //    pbRMaskImage.Image = null;
        //    pbRMaskImage.Image = null;
        //    pbRWaferImage.Image = null;

        //    _ = Task.Run(async () =>
        //    {
        //        if (rBLowMagnification.Checked)
        //        {
        //            //  左低倍率光罩 - 顯示帶綠點的預覽影像
        //            if (_recipe.LeftLowMaskMat != null && !_recipe.LeftLowMaskMat.Empty() &&
        //                _recipe.LeftLowMaskMat.Width > 0 && _recipe.LeftLowMaskMat.Height > 0)
        //            {
        //                try
        //                {
        //                    pbLMaskImage.Image?.Dispose();

        //                    //  檢查是否為多十字模式（偏移量不為 0）
        //                    if (_AlignC.LLMaskOffsetX != 0 || _AlignC.LLMaskOffsetY != 0)
        //                    {
        //                        //  多十字模式：產生帶綠點的預覽影像
        //                        pbLMaskImage.Image = CreatePreviewWithGreenDot(
        //                            _recipe.LeftLowMaskMat,
        //                            _AlignC.LLMaskOffsetX,
        //                            _AlignC.LLMaskOffsetY
        //                        );
        //                    }
        //                    else
        //                    {
        //                        //  單十字模式：顯示原始灰階影像
        //                        pbLMaskImage.Image = _recipe.LeftLowMaskMat.ToBitmap();
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    if (GV.AppSettingParm.DebugMode)
        //                    {
        //                        lbDebugMsg.Text = "LLMaskMat ToBitmap Error: " + ex.Message;
        //                    }
        //                    pbLMaskImage.Image = null;
        //                }
        //            }
        //            else
        //            {
        //                pbLMaskImage.Image = null;
        //            }

        //            //  左低倍率晶圓 - 顯示帶綠點的預覽影像
        //            if (_recipe.LeftLowWaferMat != null && !_recipe.LeftLowWaferMat.Empty() &&
        //                _recipe.LeftLowWaferMat.Width > 0 && _recipe.LeftLowWaferMat.Height > 0)
        //            {
        //                try
        //                {
        //                    pbLWaferImage.Image?.Dispose();

        //                    //  檢查是否為多十字模式
        //                    if (_AlignC.LLWaferOffsetX != 0 || _AlignC.LLWaferOffsetY != 0)
        //                    {
        //                        pbLWaferImage.Image = CreatePreviewWithGreenDot(
        //                            _recipe.LeftLowWaferMat,
        //                            _AlignC.LLWaferOffsetX,
        //                            _AlignC.LLWaferOffsetY
        //                        );
        //                    }
        //                    else
        //                    {
        //                        pbLWaferImage.Image = _recipe.LeftLowWaferMat.ToBitmap();
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    if (GV.AppSettingParm.DebugMode)
        //                    {
        //                        lbDebugMsg.Text = "LLWaferMat ToBitmap Error: " + ex.Message;
        //                    }
        //                    pbLWaferImage.Image = null;
        //                }
        //            }
        //            else
        //            {
        //                pbLWaferImage.Image = null;
        //            }

        //            //  右低倍率光罩 - 顯示帶綠點的預覽影像
        //            if (_recipe.RightLowMaskMat != null && !_recipe.RightLowMaskMat.Empty() &&
        //                _recipe.RightLowMaskMat.Width > 0 && _recipe.RightLowMaskMat.Height > 0)
        //            {
        //                try
        //                {
        //                    pbRMaskImage.Image?.Dispose();

        //                    if (_AlignC.RLMaskOffsetX != 0 || _AlignC.RLMaskOffsetY != 0)
        //                    {
        //                        pbRMaskImage.Image = CreatePreviewWithGreenDot(
        //                            _recipe.RightLowMaskMat,
        //                            _AlignC.RLMaskOffsetX,
        //                            _AlignC.RLMaskOffsetY
        //                        );
        //                    }
        //                    else
        //                    {
        //                        pbRMaskImage.Image = _recipe.RightLowMaskMat.ToBitmap();
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    if (GV.AppSettingParm.DebugMode)
        //                    {
        //                        lbDebugMsg.Text = "RLMaskMat ToBitmap Error: " + ex.Message;
        //                    }
        //                    pbRMaskImage.Image = null;
        //                }
        //            }
        //            else
        //            {
        //                pbRMaskImage.Image = null;
        //            }

        //            //  右低倍率晶圓 - 顯示帶綠點的預覽影像
        //            if (_recipe.RightLowWaferMat != null && !_recipe.RightLowWaferMat.Empty() &&
        //                _recipe.RightLowWaferMat.Width > 0 && _recipe.RightLowWaferMat.Height > 0)
        //            {
        //                try
        //                {
        //                    pbRWaferImage.Image?.Dispose();

        //                    if (_AlignC.RLWaferOffsetX != 0 || _AlignC.RLWaferOffsetY != 0)
        //                    {
        //                        pbRWaferImage.Image = CreatePreviewWithGreenDot(
        //                            _recipe.RightLowWaferMat,
        //                            _AlignC.RLWaferOffsetX,
        //                            _AlignC.RLWaferOffsetY
        //                        );
        //                    }
        //                    else
        //                    {
        //                        pbRWaferImage.Image = _recipe.RightLowWaferMat.ToBitmap();
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    if (GV.AppSettingParm.DebugMode)
        //                    {
        //                        lbDebugMsg.Text = "RLWaferMat ToBitmap Error: " + ex.Message;
        //                    }
        //                    pbRWaferImage.Image = null;
        //                }
        //            }
        //            else
        //            {
        //                pbRWaferImage.Image = null;
        //            }
        //            cbLMaskAlgorithm.SelectedIndex = (int)_AlignC.LLMaskAlgorithm;
        //            cbLWaferAlgorithm.SelectedIndex = (int)_AlignC.LLWaferAlgorithm;
        //            cbRMaskAlgorithm.SelectedIndex = (int)_AlignC.RLMaskAlgorithm;
        //            cbRWaferAlgorithm.SelectedIndex = (int)_AlignC.RLWaferAlgorithm;

        //            if (_AlignC.LLWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && _AlignC.LLWaferAIClassId >= 0)
        //            {
        //                SetComboBoxByClassId(cbLWaferClassList, _AlignC.LLWaferAIClassId);
        //            }
        //            else
        //            {
        //                cbLWaferClassList.SelectedIndex = -1;
        //            }

        //            // 恢復 Right Wafer AI 類別選擇
        //            if (_AlignC.RLWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && _AlignC.RLWaferAIClassId >= 0)
        //            {
        //                SetComboBoxByClassId(cbRWaferClassList, _AlignC.RLWaferAIClassId);
        //            }
        //            else
        //            {
        //                cbRWaferClassList.SelectedIndex = -1;
        //            }

        //            // 恢復 Left Mask AI 類別選擇
        //            if (_AlignC.LLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && _AlignC.LLMaskAIClassId >= 0)
        //            {
        //                SetComboBoxByClassId(cbLMaskClassList, _AlignC.LLMaskAIClassId);
        //            }
        //            else
        //            {
        //                cbLMaskClassList.SelectedIndex = -1;
        //            }

        //            // 恢復 Right Mask AI 類別選擇
        //            if (_AlignC.RLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && _AlignC.RLMaskAIClassId >= 0)
        //            {
        //                SetComboBoxByClassId(cbRMaskClassList, _AlignC.RLMaskAIClassId);
        //            }
        //            else
        //            {
        //                cbRMaskClassList.SelectedIndex = -1;
        //            }
        //            btMagSaveLow.Enabled = true;
        //            btMagSaveHigh.Enabled = false;


        //        }

        //        if (rBHighMagnification.Checked)
        //        {
        //            if (_recipe.LeftHighMaskMat != null && !_recipe.LeftHighMaskMat.Empty())
        //            {
        //                pbLMaskImage.Image?.Dispose();

        //                if (_AlignC.LHMaskOffsetX != 0 || _AlignC.LHMaskOffsetY != 0)
        //                {
        //                    pbLMaskImage.Image = CreatePreviewWithGreenDot(
        //                        _recipe.LeftHighMaskMat,
        //                        _AlignC.LHMaskOffsetX,
        //                        _AlignC.LHMaskOffsetY
        //                    );
        //                }
        //                else
        //                {
        //                    pbLMaskImage.Image = _recipe.LeftHighMaskMat.ToBitmap();
        //                }
        //            }

        //            if (_recipe.LeftHighWaferMat != null && !_recipe.LeftHighWaferMat.Empty())
        //            {
        //                pbLWaferImage.Image?.Dispose();

        //                if (_AlignC.LHWaferOffsetX != 0 || _AlignC.LHWaferOffsetY != 0)
        //                {
        //                    pbLWaferImage.Image = CreatePreviewWithGreenDot(
        //                        _recipe.LeftHighWaferMat,
        //                        _AlignC.LHWaferOffsetX,
        //                        _AlignC.LHWaferOffsetY
        //                    );
        //                }
        //                else
        //                {
        //                    pbLWaferImage.Image = _recipe.LeftHighWaferMat.ToBitmap();
        //                }
        //            }

        //            if (_recipe.RightHighMaskMat != null && !_recipe.RightHighMaskMat.Empty())
        //            {
        //                pbRMaskImage.Image?.Dispose();

        //                if (_AlignC.RHMaskOffsetX != 0 || _AlignC.RHMaskOffsetY != 0)
        //                {
        //                    pbRMaskImage.Image = CreatePreviewWithGreenDot(
        //                        _recipe.RightHighMaskMat,
        //                        _AlignC.RHMaskOffsetX,
        //                        _AlignC.RHMaskOffsetY
        //                    );
        //                }
        //                else
        //                {
        //                    pbRMaskImage.Image = _recipe.RightHighMaskMat.ToBitmap();
        //                }
        //            }

        //            if (_recipe.RightHighWaferMat != null && !_recipe.RightHighWaferMat.Empty())
        //            {
        //                pbRWaferImage.Image?.Dispose();

        //                if (_AlignC.RHWaferOffsetX != 0 || _AlignC.RHWaferOffsetY != 0)
        //                {
        //                    pbRWaferImage.Image = CreatePreviewWithGreenDot(
        //                        _recipe.RightHighWaferMat,
        //                        _AlignC.RHWaferOffsetX,
        //                        _AlignC.RHWaferOffsetY
        //                    );
        //                }
        //                else
        //                {
        //                    pbRWaferImage.Image = _recipe.RightHighWaferMat.ToBitmap();
        //                }
        //            }
        //            cbLMaskAlgorithm.SelectedIndex = (int)_AlignC.LHMaskAlgorithm;
        //            cbLWaferAlgorithm.SelectedIndex = (int)_AlignC.LHWaferAlgorithm;
        //            cbRMaskAlgorithm.SelectedIndex = (int)_AlignC.RHMaskAlgorithm;
        //            cbRWaferAlgorithm.SelectedIndex = (int)_AlignC.RHWaferAlgorithm;

        //            if (_AlignC.LHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && _AlignC.LHWaferAIClassId >= 0)
        //            {
        //                SetComboBoxByClassId(cbLWaferClassList, _AlignC.LHWaferAIClassId);
        //            }
        //            else
        //            {
        //                cbLWaferClassList.SelectedIndex = -1;
        //            }

        //            if (_AlignC.RHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && _AlignC.RHWaferAIClassId >= 0)
        //            {
        //                SetComboBoxByClassId(cbRWaferClassList, _AlignC.RHWaferAIClassId);
        //            }
        //            else
        //            {
        //                cbRWaferClassList.SelectedIndex = -1;
        //            }

        //            if (_AlignC.LHMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && _AlignC.LHMaskAIClassId >= 0)
        //            {
        //                SetComboBoxByClassId(cbLMaskClassList, _AlignC.LHMaskAIClassId);
        //            }
        //            else
        //            {
        //                cbLMaskClassList.SelectedIndex = -1;
        //            }

        //            if (_AlignC.RHMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && _AlignC.RHMaskAIClassId >= 0)
        //            {
        //                SetComboBoxByClassId(cbRMaskClassList, _AlignC.RHMaskAIClassId);
        //            }
        //            else
        //            {
        //                cbRMaskClassList.SelectedIndex = -1;
        //            }
        //            btMagSaveLow.Enabled = false;
        //            btMagSaveHigh.Enabled = true;
        //        }
        //    });

        //    _ = Task.Run(async () =>
        //    {
        //        if ((LCoaLight.Enabled) && (LCoaLight.Checked))
        //        {
        //            btLCoaLightMinus.Enabled = true;
        //            btLCoaLightPlus.Enabled = true;
        //            tBLeftCoaLight.Enabled = true;
        //        }
        //        else
        //        {
        //            btLCoaLightMinus.Enabled = false;
        //            btLCoaLightPlus.Enabled = false;
        //            tBLeftCoaLight.Enabled = false;
        //        }

        //        if ((RCoaLight.Enabled) && (RCoaLight.Checked))
        //        {
        //            btRCoaLightMinus.Enabled = true;
        //            btRCoaLightPlus.Enabled = true;
        //            tBRightCoaLight.Enabled = true;
        //        }
        //        else
        //        {
        //            btRCoaLightMinus.Enabled = false;
        //            btRCoaLightPlus.Enabled = false;
        //            tBRightCoaLight.Enabled = false;
        //        }

        //        if ((LRingLight.Enabled) && (LRingLight.Checked))
        //        {
        //            btLRingLightMinus.Enabled = true;
        //            btLRingLightPlus.Enabled = true;
        //            tBLeftRingLight.Enabled = true;
        //        }
        //        else
        //        {
        //            btLRingLightMinus.Enabled = false;
        //            btLRingLightPlus.Enabled = false;
        //            tBLeftRingLight.Enabled = false;
        //        }

        //        if ((RRingLight.Enabled) && (RRingLight.Checked))
        //        {
        //            btRRingLightMinus.Enabled = true;
        //            btRRingLightPlus.Enabled = true;
        //            tBRightRingLight.Enabled = true;
        //        }
        //        else
        //        {
        //            btRRingLightMinus.Enabled = false;
        //            btRRingLightPlus.Enabled = false;
        //            tBRightRingLight.Enabled = false;
        //        }
        //    });

        //    _ = Task.Run(async () =>
        //    {
        //        if (LCoaLight.Checked)
        //        {
        //            GV.Light.ChangeBrightness("left", tBLeftCoaLight.Value);
        //        }
        //        else
        //        {
        //            GV.Light.ChangeBrightness("left", 0);
        //        }

        //        if (RCoaLight.Checked)
        //        {
        //            GV.Light.ChangeBrightness("right", tBRightCoaLight.Value);
        //        }
        //        else
        //        {
        //            GV.Light.ChangeBrightness("right", 0);
        //        }

        //        if (LRingLight.Checked)
        //        {
        //            GV.RingLight.ChangeBrightness("left", tBLeftRingLight.Value);
        //        }
        //        else
        //        {
        //            GV.RingLight.ChangeBrightness("left", 0);
        //        }

        //        if (RRingLight.Checked)
        //        {
        //            GV.RingLight.ChangeBrightness("right", tBRightRingLight.Value);
        //        }
        //        else
        //        {
        //            GV.RingLight.ChangeBrightness("right", 0);
        //        }
        //    });
        //    if (groupBoxT4.Visible || groupBoxT1.Visible)
        //    {
        //        isLocationUpdateEnabled = true;
        //        if (!locationUpdateTimer.Enabled)
        //            locationUpdateTimer.Start();
        //        UpdateCurrentLocationUI();
        //    }

        //    Update();
        //}
        public void UpdateUI()
        {
            _ = UpdateUIAsync();
        }

        private async Task UpdateUIAsync()
        {
            if ((GV.NowRecipeNumber < 1) || (GV.NowRecipeNumber > 100)) GV.NowRecipeNumber = 1;
            EditRecipe = GV.NowRecipeNumber;
            ChangeRecipe();
            lbRecipeNumber.Text = GV.Dlang.strRecipeNumber + EditRecipe.ToString();
            cbLowMagnification.SelectedIndex = _AlignC.AlignLowMagnification;
            cbHighMagnification.SelectedIndex = _AlignC.AlignHighMagnification;
            cbPatterhShift.SelectedIndex = _AlignC.PatternShift;
            bool isLow = rBLowMagnification.Checked;
            bool isHigh = rBHighMagnification.Checked;
            ApplyLightControlEnableState();
            var lightState = CaptureLightState();
            _ = UpdateLightHardwareAsync(lightState);
            await RefreshPreviewImagesAsync(isLow, isHigh);

            if (groupBoxT4.Visible || groupBoxT1.Visible)
            {
                isLocationUpdateEnabled = true;
                if (!locationUpdateTimer.Enabled)
                    locationUpdateTimer.Start();
                _ = RefreshCurrentLocationAsync();
            }
            Invalidate();
        }

        private void ApplyLightControlEnableState()
        {
            bool leftCoa = LCoaLight.Enabled && LCoaLight.Checked;
            btLCoaLightMinus.Enabled = leftCoa;
            btLCoaLightPlus.Enabled = leftCoa;
            tBLeftCoaLight.Enabled = leftCoa;

            bool rightCoa = RCoaLight.Enabled && RCoaLight.Checked;
            btRCoaLightMinus.Enabled = rightCoa;
            btRCoaLightPlus.Enabled = rightCoa;
            tBRightCoaLight.Enabled = rightCoa;

            bool leftRing = LRingLight.Enabled && LRingLight.Checked;
            btLRingLightMinus.Enabled = leftRing;
            btLRingLightPlus.Enabled = leftRing;
            tBLeftRingLight.Enabled = leftRing;

            bool rightRing = RRingLight.Enabled && RRingLight.Checked;
            btRRingLightMinus.Enabled = rightRing;
            btRRingLightPlus.Enabled = rightRing;
            tBRightRingLight.Enabled = rightRing;
        }
        private sealed class LightUiState
        {
            public bool LCoaChecked;
            public bool RCoaChecked;
            public bool LRingChecked;
            public bool RRingChecked;
            public int LCoaValue;
            public int RCoaValue;
            public int LRingValue;
            public int RRingValue;
        }

        private LightUiState CaptureLightState()
        {
            return new LightUiState
            {
                LCoaChecked = LCoaLight.Checked,
                RCoaChecked = RCoaLight.Checked,
                LRingChecked = LRingLight.Checked,
                RRingChecked = RRingLight.Checked,
                LCoaValue = tBLeftCoaLight.Value,
                RCoaValue = tBRightCoaLight.Value,
                LRingValue = tBLeftRingLight.Value,
                RRingValue = tBRightRingLight.Value
            };
        }
        private async Task UpdateLightHardwareAsync(LightUiState state)
        {
            try
            {
                await Task.Run(() =>
                {
                    GV.Light.ChangeBrightness("left", state.LCoaChecked ? state.LCoaValue : 0);
                    GV.Light.ChangeBrightness("right", state.RCoaChecked ? state.RCoaValue : 0);
                    GV.RingLight.ChangeBrightness("left", state.LRingChecked ? state.LRingValue : 0);
                    GV.RingLight.ChangeBrightness("right", state.RRingChecked ? state.RRingValue : 0);
                });
            }
            catch (Exception ex)
            {
                SafeBeginInvoke(() =>
                {
                    GM.WriteToStatusTextBox($"光源更新失敗:{ex.Message}");

                });
            }
        }
        private sealed class PreviewImageSet : IDisposable
        {
            public Bitmap LMask;
            public Bitmap LWafer;
            public Bitmap RMask;
            public Bitmap RWafer;
            public void Dispose()
            {
                LMask?.Dispose();
                LWafer?.Dispose();
                RMask?.Dispose();
                RWafer?.Dispose();
            }
        }

        private async Task RefreshPreviewImagesAsync(bool isLow, bool isHigh)
        {
            PreviewImageSet images = null;
            try
            {
                images = await Task.Run(() =>
                {
                    var result = new PreviewImageSet();
                    if (isLow)
                    {
                        result.LMask = BuildPreviewBitmap(_recipe.LeftLowMaskMat, _AlignC.LLMaskOffsetX, _AlignC.LLMaskOffsetY);
                        result.LWafer = BuildPreviewBitmap(_recipe.LeftLowWaferMat, _AlignC.LLWaferOffsetX, _AlignC.LLWaferOffsetY);
                        result.RMask = BuildPreviewBitmap(_recipe.RightLowMaskMat, _AlignC.RLMaskOffsetX, _AlignC.RLMaskOffsetY);
                        result.RWafer = BuildPreviewBitmap(_recipe.RightLowWaferMat, _AlignC.RLWaferOffsetX, _AlignC.RLWaferOffsetY);
                    }
                    else if (isHigh)
                    {
                        result.LMask = BuildPreviewBitmap(_recipe.LeftHighMaskMat, _AlignC.LHMaskOffsetX, _AlignC.LHMaskOffsetY);
                        result.LWafer = BuildPreviewBitmap(_recipe.LeftHighWaferMat, _AlignC.LHWaferOffsetX, _AlignC.LHWaferOffsetY);
                        result.RMask = BuildPreviewBitmap(_recipe.RightHighMaskMat, _AlignC.RHMaskOffsetX, _AlignC.RHMaskOffsetY);
                        result.RWafer = BuildPreviewBitmap(_recipe.RightHighWaferMat, _AlignC.RHWaferOffsetX, _AlignC.RHWaferOffsetY);
                    }
                    return result;
                });
                var uiImage = images;
                images = null;
                SafeBeginInvoke(() =>
                {
                    SetPictureBoxImage(pbLMaskImage, uiImage.LMask);
                    SetPictureBoxImage(pbLWaferImage, uiImage.LWafer);
                    SetPictureBoxImage(pbRMaskImage, uiImage.RMask);
                    SetPictureBoxImage(pbRWaferImage, uiImage.RWafer);
                    if (isLow)
                    {
                        cbLMaskAlgorithm.SelectedIndex = (int)_AlignC.LLMaskAlgorithm;
                        cbLWaferAlgorithm.SelectedIndex = (int)_AlignC.LLWaferAlgorithm;
                        cbRMaskAlgorithm.SelectedIndex = (int)_AlignC.RLMaskAlgorithm;
                        cbRWaferAlgorithm.SelectedIndex = (int)_AlignC.RLWaferAlgorithm;
                        RestoreAiClassSelection
                        (
                            _AlignC.LLWaferAlgorithm, _AlignC.LLWaferAIClassId,
                            _AlignC.RLWaferAlgorithm, _AlignC.RLWaferAIClassId,
                            _AlignC.LLMaskAlgorithm, _AlignC.LLMaskAIClassId,
                            _AlignC.RLMaskAlgorithm, _AlignC.RLMaskAIClassId
                        );
                        btMagSaveLow.Enabled = true;
                        btMagSaveHigh.Enabled = false;
                    }
                    else if (isHigh)
                    {
                        cbLMaskAlgorithm.SelectedIndex = (int)_AlignC.LHMaskAlgorithm;
                        cbLWaferAlgorithm.SelectedIndex = (int)_AlignC.LHWaferAlgorithm;
                        cbRMaskAlgorithm.SelectedIndex = (int)_AlignC.RHMaskAlgorithm;
                        cbRWaferAlgorithm.SelectedIndex = (int)_AlignC.RHWaferAlgorithm;

                        RestoreAiClassSelection
                        (
                            _AlignC.LHWaferAlgorithm, _AlignC.LHWaferAIClassId,
                            _AlignC.RHWaferAlgorithm, _AlignC.RHWaferAIClassId,
                            _AlignC.LHMaskAlgorithm, _AlignC.LHMaskAIClassId,
                            _AlignC.RHMaskAlgorithm, _AlignC.RHMaskAIClassId
                        );

                        btMagSaveLow.Enabled = false;
                        btMagSaveHigh.Enabled = true;
                    }
                });
            }
            catch (Exception ex)
            {
                SafeBeginInvoke(() =>
                {
                    if (GV.AppSettingParm.DebugMode)
                        lbDebugMsg.Text = "Preview image update failed: " + ex.Message;
                });
            }
            finally
            {
                images?.Dispose();
            }
        }
        private Bitmap BuildPreviewBitmap(Mat sourceImage, int offsetX, int offsetY)
        {
            if (sourceImage == null || sourceImage.Empty() || sourceImage.Width <= 0 || sourceImage.Height <= 0)
                return null;

            if (offsetX != 0 || offsetY != 0)
                return CreatePreviewWithGreenDot(sourceImage, offsetX, offsetY);

            return sourceImage.ToBitmap();
        }
        private void SetPictureBoxImage(PictureBox pictureBox, Image newImage)
        {
            var oldImage = pictureBox.Image;
            pictureBox.Image = newImage;
            if (oldImage != null && !ReferenceEquals(oldImage, newImage)) oldImage.Dispose();
        }

        private void RestoreAiClassSelection(
            OpenCV3MatchUMat.AlignAlgorithm lWaferAlgo, int lWaferClassId,
            OpenCV3MatchUMat.AlignAlgorithm rWaferAlgo, int rWaferClassId,
            OpenCV3MatchUMat.AlignAlgorithm lMaskAlgo, int lMaskClassId,
            OpenCV3MatchUMat.AlignAlgorithm rMaskAlgo, int rMaskClassId)
        {
            if (lWaferAlgo == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && lWaferClassId >= 0)
                SetComboBoxByClassId(cbLWaferClassList, lWaferClassId);
            else
                cbLWaferClassList.SelectedIndex = -1;
            if (rWaferAlgo == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && rWaferClassId >= 0)
                SetComboBoxByClassId(cbRWaferClassList, rWaferClassId);
            else
                cbRWaferClassList.SelectedIndex = -1;

            if (lMaskAlgo == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && lMaskClassId >= 0)
                SetComboBoxByClassId(cbLMaskClassList, lMaskClassId);
            else
                cbLMaskClassList.SelectedIndex = -1;

            if (rMaskAlgo == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && rMaskClassId >= 0)
                SetComboBoxByClassId(cbRMaskClassList, rMaskClassId);
            else
                cbRMaskClassList.SelectedIndex = -1;
        }

        private Bitmap CreatePreviewWithGreenDot(Mat sourceImage, int offsetX, int offsetY)
        {
            if (sourceImage == null || sourceImage.Empty())
            {
                return null;
            }

            try
            {
                //  複製原始影像並轉換為彩色
                Mat previewMat = sourceImage.Clone();
                Cv2.CvtColor(previewMat, previewMat, ColorConversionCodes.GRAY2BGR);

                //  計算中心點位置（offsetX 和 offsetY 就是中心點座標）
                int centerX = offsetX;
                int centerY = offsetY;

                //  繪製綠色圓點
                Cv2.Circle(previewMat, new OpenCvSharp.Point(centerX, centerY), 5, new Scalar(0, 255, 0), -1);

                //  繪製綠色十字線
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
            if (EditRecipe != GV.NowRecipeNumber)
            {
                Debug.WriteLine($"[ChangeRecipe] EditRecipe ({EditRecipe}) 與 GV.NowRecipeNumber ({GV.NowRecipeNumber}) 不一致，已修正");
                EditRecipe = GV.NowRecipeNumber;
            }
            _recipe = GV._recipe;
            _AlignC = _recipe.AlignC;

            //_AlignC = GV.acar[EditRecipe];

            rBLowMagnification.Checked = true;
            rBHighMagnification.Checked = false;
            if (_AlignC.AlignLowMagnification < 0 || _AlignC.AlignLowMagnification >= _AlignC.LeftBrightness.Length) _AlignC.AlignLowMagnification = 0;
            NowMageni = _AlignC.AlignLowMagnification;

            if (_AlignC.LeftLight[NowMageni])
            {
                LCoaLight.Checked = true;
                tBLeftCoaLight.Value = _AlignC.LeftBrightness[NowMageni];
            }
            else
            {
                LCoaLight.Checked = false;
                tBLeftCoaLight.Value = 0;
            }

            if (_AlignC.RightLight[NowMageni])
            {
                RCoaLight.Checked = true;
                tBRightCoaLight.Value = _AlignC.RightBrightness[NowMageni];
            }
            else
            {
                RCoaLight.Checked = false;
                tBRightCoaLight.Value = 0;
            }

            if (_AlignC.LRingLight[NowMageni])
            {
                LRingLight.Checked = true;
                tBLeftRingLight.Value = _AlignC.LeftRingBrightness[NowMageni];
            }
            else
            {
                LRingLight.Checked = false;
                tBLeftRingLight.Value = 0;
            }

            if (_AlignC.RRingLight[NowMageni])
            {
                RRingLight.Checked = true;
                tBRightRingLight.Value = _AlignC.RightRingBrightness[NowMageni];
            }
            else
            {
                RRingLight.Checked = false;
                tBRightRingLight.Value = 0;
            }
            UpdateTargetPositionDisplay();
        }

        public void ShowTime(string NowTime)
        {
            Invoke((MethodInvoker)delegate ()
            {
                lbNowTime.Text = NowTime;
            });
        }
        private void ApplyLanguage()
        {
            if (GV.AppSettingParm.Language != "default" && GV.Dlang != null)
            {

                groupBoxT4.Text = GV.Dlang.gbWECLocation;
                label6.Text = GV.Dlang.lbTargetPosition;
                label5.Text = GV.Dlang.lbCurrentPosition;
                label8.Text = GV.Dlang.lbChuckZ;
                label15.Text = GV.Dlang.lbChuckX;
                label17.Text = GV.Dlang.lbChuckY1;
                label16.Text = GV.Dlang.lbChuckY2;
                btChuckUpdate.Caption = GV.Dlang.btChuckUpdate;
                btChuckGo.Caption = GV.Dlang.btChuckGo;

                groupBoxT1.Text = GV.Dlang.gbCCDLocation;
                label10.Text = GV.Dlang.lbTargetPosition;
                label11.Text = GV.Dlang.lbCurrentPosition;
                label12.Text = GV.Dlang.lbBigY;
                label13.Text = GV.Dlang.lbLeftX;
                label7.Text = GV.Dlang.lbLeftY;
                label9.Text = GV.Dlang.lbLeftZ;
                label14.Text = GV.Dlang.lbRightX;
                label18.Text = GV.Dlang.lbRightY;
                label19.Text = GV.Dlang.lbRightZ;
                btUpCCDUpdate.Caption = GV.Dlang.btCCDUpdate;
                btUpCCDGo.Caption = GV.Dlang.btCCDGo;
            }
        }

        //public void CheckLevel()
        //{
        //    Invoke((MethodInvoker)delegate ()
        //    {
        //        if (GV.UserLevel == GV.User.Operator)
        //        {
        //            gbLWafer.Enabled = false;
        //            gbLMask.Enabled = false;
        //            gbRWafer.Enabled = false;
        //            gbRMask.Enabled = false;
        //            LRingLight.Enabled = false;
        //            LCoaLight.Enabled = false;
        //            RRingLight.Enabled = false;
        //            RCoaLight.Enabled = false;
        //            cbLowMagnification.Enabled = false;
        //            cbHighMagnification.Enabled = false;
        //            cbPatterhShift.Enabled = false;
        //            btLCoaSave.Enabled = false;
        //            btRCoaSave.Enabled = false;
        //            btLRingSave.Enabled = false;
        //            btRRingSave.Enabled = false;
        //            btMagSaveLow.Enabled = false;
        //            btMagSaveHigh.Enabled = false;
        //            btDelete.Enabled = false;
        //            btDefault.Enabled = false;
        //        }
        //        else
        //        {
        //            gbLWafer.Enabled = true;
        //            gbLMask.Enabled = true;
        //            gbRWafer.Enabled = true;
        //            gbRMask.Enabled = true;
        //            LRingLight.Enabled = true;
        //            LCoaLight.Enabled = true;
        //            RRingLight.Enabled = true;
        //            RCoaLight.Enabled = true;
        //            cbLowMagnification.Enabled = true;
        //            cbHighMagnification.Enabled = false;
        //            cbPatterhShift.Enabled = false;
        //            btLCoaSave.Enabled = true;
        //            btRCoaSave.Enabled = true;
        //            btLRingSave.Enabled = true;
        //            btRRingSave.Enabled = true;
        //            // btMagSaveLow.Enabled = true;
        //            btDelete.Enabled = true;
        //            btDefault.Enabled = true;
        //        }
        //        rBHighMagnification.Enabled = true;
        //        rBLowMagnification.Enabled = true;

        //        if (GV.MachineStatus == GV.Status.Align)
        //        {
        //        }
        //        else if (GV.MachineStatus == GV.Status.Standby)
        //        {
        //            gBMag.Enabled = true;
        //        }

        //        UpdateUI();
        //    });
        //}
        public void CheckLevel()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate ()
                {
                    CheckLevelInternal();
                });
            }
            else
            {
                CheckLevelInternal();
            }
        }

        private void CheckLevelInternal()
        {
            if (GV.UserLevel == GV.User.Operator)
            {
                gbLWafer.Enabled = false;
                gbLMask.Enabled = false;
                gbRWafer.Enabled = false;
                gbRMask.Enabled = false;
                LRingLight.Enabled = false;
                LCoaLight.Enabled = false;
                RRingLight.Enabled = false;
                RCoaLight.Enabled = false;
                cbLowMagnification.Enabled = false;
                cbHighMagnification.Enabled = false;
                cbPatterhShift.Enabled = false;
                btLCoaSave.Enabled = false;
                btRCoaSave.Enabled = false;
                btLRingSave.Enabled = false;
                btRRingSave.Enabled = false;
                btMagSaveLow.Enabled = false;
                btMagSaveHigh.Enabled = false;
                btDelete.Enabled = false;
                btDefault.Enabled = false;
            }
            else
            {
                gbLWafer.Enabled = true;
                gbLMask.Enabled = true;
                gbRWafer.Enabled = true;
                gbRMask.Enabled = true;
                LRingLight.Enabled = true;
                LCoaLight.Enabled = true;
                RRingLight.Enabled = true;
                RCoaLight.Enabled = true;
                cbLowMagnification.Enabled = true;
                cbHighMagnification.Enabled = false;
                cbPatterhShift.Enabled = false;
                btLCoaSave.Enabled = true;
                btRCoaSave.Enabled = true;
                btLRingSave.Enabled = true;
                btRRingSave.Enabled = true;
                btDelete.Enabled = true;
                btDefault.Enabled = true;
            }
            rBHighMagnification.Enabled = true;
            rBLowMagnification.Enabled = true;

            if (GV.MachineStatus == GV.Status.Align)
            {
            }
            else if (GV.MachineStatus == GV.Status.Standby)
            {
                gBMag.Enabled = true;
            }

            UpdateUI();
            //SyncAiClassListVisibility();
        }
        private void ApplyAlgorithmSelectionForCurrentMagnification()
        {
            if (_AlignC == null) return;

            if (rBLowMagnification.Checked)
            {
                cbLMaskAlgorithm.SelectedIndex = (int)_AlignC.LLMaskAlgorithm;
                cbLWaferAlgorithm.SelectedIndex = (int)_AlignC.LLWaferAlgorithm;
                cbRMaskAlgorithm.SelectedIndex = (int)_AlignC.RLMaskAlgorithm;
                cbRWaferAlgorithm.SelectedIndex = (int)_AlignC.RLWaferAlgorithm;
            }
            else if (rBHighMagnification.Checked)
            {
                cbLMaskAlgorithm.SelectedIndex = (int)_AlignC.LHMaskAlgorithm;
                cbLWaferAlgorithm.SelectedIndex = (int)_AlignC.LHWaferAlgorithm;
                cbRMaskAlgorithm.SelectedIndex = (int)_AlignC.RHMaskAlgorithm;
                cbRWaferAlgorithm.SelectedIndex = (int)_AlignC.RHWaferAlgorithm;
            }
        }

        private void SyncAiClassListVisibility()
        {
            bool lWaferAi = cbLWaferAlgorithm.SelectedIndex == 2;
            cbLWaferClassList.Visible = lWaferAi;
            pbLWaferImage.Visible = !lWaferAi;
            btFindLWaferCenter.Visible = !lWaferAi;
            btCreateLWaferPatternMask.Visible = !lWaferAi;

            bool rWaferAi = cbRWaferAlgorithm.SelectedIndex == 2;
            cbRWaferClassList.Visible = rWaferAi;
            pbRWaferImage.Visible = !rWaferAi;
            btFindRWaferCenter.Visible = !rWaferAi;
            btCreateRWaferPatternMask.Visible = !rWaferAi;

            bool lMaskAi = cbLMaskAlgorithm.SelectedIndex == 2;
            cbLMaskClassList.Visible = lMaskAi;
            pbLMaskImage.Visible = !lMaskAi;
            btFindLMaskCenter.Visible = !lMaskAi;
            btCreateLMaskPatternMask.Visible = !lMaskAi;

            bool rMaskAi = cbRMaskAlgorithm.SelectedIndex == 2;
            cbRMaskClassList.Visible = rMaskAi;
            pbRMaskImage.Visible = !rMaskAi;
            btFindRMaskCenter.Visible = !rMaskAi;
            btCreateRMaskPatternMask.Visible = !rMaskAi;
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

        private OpenCV3MatchUMat.AlignAlgorithm Algoritm(int a)
        {
            if (a == 0)
                return OpenCV3MatchUMat.AlignAlgorithm.TemplateMatch;
            else if (a == 1)
                return OpenCV3MatchUMat.AlignAlgorithm.EdgeMatch;
            else if (a == 2)
                return OpenCV3MatchUMat.AlignAlgorithm.AIMatch;
            else
                return OpenCV3MatchUMat.AlignAlgorithm.TemplateMatch;

        }

        private void BtLWaferTemplateSave_Click(object sender, EventArgs e)
        {
            GV.TickCount = 0;
            if (cbLWaferAlgorithm.SelectedIndex == 2)  // AIMatch
            {
                if (cbLWaferClassList.SelectedIndex < 0 || cbLWaferClassList.SelectedItem == null)
                {
                    MessageBox.Show("請先選擇 AI 類別", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string confirmMsg = rBLowMagnification.Checked ? "確定儲存左側低倍晶圓 AI 類別?" : "確定儲存左側高倍晶圓 AI 類別?";
                if (MessageBox.Show(confirmMsg, "儲存 AI 類別", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                string selectedClassName = cbLWaferClassList.SelectedItem.ToString();
                Dictionary<string, Int16> ClassList = GV.AIClassList.GetClassList();

                if (!ClassList.ContainsKey(selectedClassName))
                {
                    MessageBox.Show($"找不到類別 '{selectedClassName}'", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int classId = ClassList[selectedClassName];

                if (rBLowMagnification.Checked)
                {
                    _AlignC.LLWaferAlgorithm = OpenCV3MatchUMat.AlignAlgorithm.AIMatch;
                    _AlignC.LLWaferAIClassId = classId;
                    _AlignC.LLBitwiseNot = classId;
                }
                else if (rBHighMagnification.Checked)
                {
                    _AlignC.LHWaferAlgorithm = OpenCV3MatchUMat.AlignAlgorithm.AIMatch;
                    _AlignC.LHWaferAIClassId = classId;
                    _AlignC.LHBitwiseNot = classId;
                }

                _AlignC.LastModifyTime = DateTime.Now;
                GM.WriteRecipeXml(EditRecipe);

                MessageBox.Show($"已儲存 AI 類別: {selectedClassName} (ID: {classId})", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //  TemplateMatch/EdgeMatch 模式：原有的影像處理流程
            Mat fullImage = GV.LeftUpCam.Grab();
            if (fullImage == null || fullImage.Empty())
            {
                MessageBox.Show("左側相機未取得影像，請確認相機連線狀態", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            OpenCvSharp.Rect roi = skLPattern.GetWaferRect();
            if (roi.Width <= 0 || roi.Height <= 0 ||
                roi.X < 0 || roi.Y < 0 ||
                roi.X + roi.Width > fullImage.Width ||
                roi.Y + roi.Height > fullImage.Height)
            {
                MessageBox.Show("Wafer ROI 超出影像範圍或無效", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                fullImage.Dispose();
                return;
            }

            Mat m = new Mat(fullImage, roi).Clone();

            if (cbLWaferAlgorithm.SelectedIndex == 1)
            {
                Cv2.GaussianBlur(m, m, new OpenCvSharp.Size(7, 7), 0, 0, BorderTypes.Default);
                Cv2.AdaptiveThreshold(m, m, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.Binary, 27, 2);
                Mat invertedImg = new Mat();
                Cv2.BitwiseNot(m, invertedImg);
                m = invertedImg.Clone();
                invertedImg.Dispose();
            }

            if (rBLowMagnification.Checked)
            {
                if (MessageBox.Show(GV.Dlang.strbtnSaveLLW, GV.Dlang.strSureSaveLLW, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                GV.Plc.GetRecipe();
                GV.Plc.GetLocation();

                _AlignC.LRingLight[NowMageni] = LRingLight.Checked;
                _AlignC.LeftRingBrightness[NowMageni] = tBLeftRingLight.Value;
                _AlignC.LeftLight[NowMageni] = LCoaLight.Checked;
                _AlignC.LeftBrightness[NowMageni] = tBLeftCoaLight.Value;
                _AlignC.LLWaferOffsetX = 0;
                _AlignC.LLWaferOffsetY = 0;
                _recipe.SetLeftLowWaferMat(m);
                pbLWaferImage.Image = m.ToBitmap();
                GrayImage d = new GrayImage(m.Width, m.Height);
                d.Fill(255);
                _recipe.LeftLowWaferMask = new GrayImage(d);
                _AlignC.LowMagnificationLZ = GV.NowLocation[GV.Plc.iiDSUpLeftZ];
                _AlignC.LLWaferAlgorithm = Algoritm(cbLWaferAlgorithm.SelectedIndex);
                _AlignC.LLWaferAIClassId = -1;
                _AlignC.LLBitwiseNot = 0;

                if (GV.AppSettingParm.Emulation != true && GV.matcherLLW != null)
                {
                    GV.matcherLLW.LearnWithAlgo(m, d, _AlignC.LLWaferAlgorithm);
                }
            }
            else if (rBHighMagnification.Checked)
            {
                if (MessageBox.Show(GV.Dlang.strbtnSaveLHW, GV.Dlang.strSureSaveLHW, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                GV.Plc.GetRecipe();
                GV.Plc.GetLocation();

                _AlignC.LRingLight[NowMageni] = LRingLight.Checked;
                _AlignC.LeftRingBrightness[NowMageni] = tBLeftRingLight.Value;
                _AlignC.LeftLight[NowMageni] = LCoaLight.Checked;
                _AlignC.LHWaferOffsetX = 0;
                _AlignC.LHWaferOffsetY = 0;
                _recipe.SetLeftHighWaferMat(m);
                pbLWaferImage.Image = m.ToBitmap();
                GrayImage d = new GrayImage(m.Width, m.Height);
                d.Fill(255);
                _recipe.LeftHighWaferMask = new GrayImage(d);
                _AlignC.HighMagnificationLZ = GV.NowLocation[GV.Plc.iiDSUpLeftZ];
                _AlignC.LHWaferAlgorithm = Algoritm(cbLWaferAlgorithm.SelectedIndex);
                _AlignC.LHWaferAIClassId = -1;
                _AlignC.LHBitwiseNot = 0;

                if (GV.AppSettingParm.Emulation != true && GV.matcherLHW != null)
                {
                    GV.matcherLHW.LearnWithAlgo(m, d, _AlignC.LHWaferAlgorithm);
                }
            }
            else
            {
                MessageBox.Show("Select magnification first.", "LWaferTemplateSave", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _AlignC.LastModifyTime = DateTime.Now;
            GM.WriteRecipeXml(EditRecipe);
        }

        //internal void ShowMe()
        //{
        //    if (Interlocked.Exchange(ref _showMeBusy, 1) == 1) return;
        //    Stopwatch sw = Stopwatch.StartNew();
        //    try
        //    {

        //        EditRecipe = GV.NowRecipeNumber;
        //        _recipe = GV._recipe;
        //        _AlignC = _recipe.AlignC;
        //        //EditRecipe = _recipe.RecipeNumber;
        //        bool recipeChanged = (_lastShowRecipe != EditRecipe);
        //        _lastShowRecipe = EditRecipe;

        //        lbRecipeNumber.Text = GV.Dlang.strRecipeNumber + EditRecipe.ToString();
        //        btLabelPatternL.Visible = true;

        //        if (GV.AppSettingParm.Author == 0)
        //        {
        //            btTfile.Visible = false;
        //            btTLRead.Visible = false;
        //            btTRRead.Visible = false;
        //        }
        //        else
        //        {
        //            btTfile.Visible = true;
        //            btTLRead.Visible = true;
        //            btTRRead.Visible = true;
        //        }


        //        if (GMPLeft == null)
        //        {
        //            GMPLeft = new Mat();
        //            GMPRight = new Mat();
        //        }

        //        if (MoveCCDLeft == null)
        //        {
        //            MoveCCDLeft = new Mat();
        //            MoveCCDRight = new Mat();
        //            GMPlMaskMp = new MatchPosition();
        //            GMPrMaskMp = new MatchPosition();
        //            GMPlWaferMp = new MatchPosition();
        //            GMPrWaferMp = new MatchPosition();
        //        }

        //        if (MatchThread == null)
        //        {
        //            MatchThread = new Thread(DrawMatchPosition) { IsBackground = true };
        //            MatchThread.Start();
        //        }

        //        //_ = Task.Run(async () => { CbHLMagni_Change(NowMageni); });
        //        //_ = Task.Run(async () => { Changelight(); });
        //        //_ = Task.Run(async () => { CheckParam(); });
        //        CbHLMagni_Change(NowMageni);
        //        Changelight();
        //        CheckParam();
        //        //Update();
        //        //ApplyLanguage();

        //        _ = Task.Run(async () =>
        //        {
        //            try
        //            {
        //                try
        //                {
        //                    var classNames = await GV.AIClassList.GetClassNamesAsync();
        //                    var classList = new Dictionary<string, Int16>();
        //                    for (int i = 0; i < classNames.Count; i++)
        //                    {
        //                        classList.Add(classNames[i], (Int16)i);
        //                    }

        //                    await InvokeAsync(() => UpdateClassList(classList));
        //                }
        //                catch (Exception ex)
        //                {
        //                    Debug.WriteLine($"更新 AI 類別列表失敗: {ex.Message}");
        //                }


        //                await InvokeAsync(() =>
        //                {
        //                    try
        //                    {
        //                        if (rBLowMagnification.Checked)
        //                        {
        //                            if (_AlignC.LLWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && _AlignC.LLWaferAIClassId >= 0)
        //                            {
        //                                SetComboBoxByClassId(cbLWaferClassList, _AlignC.LLWaferAIClassId);
        //                            }
        //                            if (_AlignC.RLWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && _AlignC.RLWaferAIClassId >= 0)
        //                            {
        //                                SetComboBoxByClassId(cbRWaferClassList, _AlignC.RLWaferAIClassId);
        //                            }
        //                            if (_AlignC.LLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && _AlignC.LLMaskAIClassId >= 0)
        //                            {
        //                                SetComboBoxByClassId(cbLMaskClassList, _AlignC.LLMaskAIClassId);
        //                            }
        //                            if (_AlignC.RLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && _AlignC.RLMaskAIClassId >= 0)
        //                            {
        //                                SetComboBoxByClassId(cbRMaskClassList, _AlignC.RLMaskAIClassId);
        //                            }
        //                        }
        //                        else if (rBHighMagnification.Checked)
        //                        {
        //                            if (_AlignC.LHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && _AlignC.LHWaferAIClassId >= 0)
        //                            {
        //                                SetComboBoxByClassId(cbLWaferClassList, _AlignC.LHWaferAIClassId);
        //                            }
        //                            if (_AlignC.RHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && _AlignC.RHWaferAIClassId >= 0)
        //                            {
        //                                SetComboBoxByClassId(cbRWaferClassList, _AlignC.RHWaferAIClassId);
        //                            }
        //                            if (_AlignC.LHMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && _AlignC.LHMaskAIClassId >= 0)
        //                            {
        //                                SetComboBoxByClassId(cbLMaskClassList, _AlignC.LHMaskAIClassId);
        //                            }
        //                            if (_AlignC.RHMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && _AlignC.RHMaskAIClassId >= 0)
        //                            {
        //                                SetComboBoxByClassId(cbRMaskClassList, _AlignC.RHMaskAIClassId);
        //                            }
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        Debug.WriteLine($"恢復 AI 類別選擇失敗: {ex.Message}");
        //                    }
        //                });

        //                if (GV.AppSettingParm?.Emulation != true)
        //                {
        //                    await Task.Run(() =>
        //                    {
        //                        try
        //                        {
        //                            // 並行學習所有 Matcher
        //                            var learnTasks = new List<Task>();

        //                            if (_recipe.LeftLowMaskMat != null && !_recipe.LeftLowMaskMat.Empty() && GV.matcherLLM != null)
        //                            {
        //                                learnTasks.Add(Task.Run(() =>
        //                                {
        //                                    try
        //                                    {
        //                                        GV.matcherLLM.LearnWithAlgo(_recipe.LeftLowMaskMat, _recipe.LeftLowMaskMask, _AlignC.LLMaskAlgorithm);
        //                                    }
        //                                    catch (Exception ex) { Debug.WriteLine($"LLM Learn 失敗: {ex.Message}"); }
        //                                }));
        //                            }

        //                            if (_recipe.RightLowMaskMat != null && !_recipe.RightLowMaskMat.Empty() && GV.matcherRLM != null)
        //                            {
        //                                learnTasks.Add(Task.Run(() =>
        //                                {
        //                                    try
        //                                    {
        //                                        GV.matcherRLM.LearnWithAlgo(_recipe.RightLowMaskMat, _recipe.RightLowMaskMask, _AlignC.RLMaskAlgorithm);
        //                                    }
        //                                    catch (Exception ex) { Debug.WriteLine($"RLM Learn 失敗: {ex.Message}"); }
        //                                }));
        //                            }

        //                            if (_recipe.LeftLowWaferMat != null && !_recipe.LeftLowWaferMat.Empty() && GV.matcherLLW != null)
        //                            {
        //                                learnTasks.Add(Task.Run(() =>
        //                                {
        //                                    try
        //                                    {
        //                                        GV.matcherLLW.LearnWithAlgo(_recipe.LeftLowWaferMat, _recipe.LeftLowWaferMask, _AlignC.LLWaferAlgorithm);
        //                                    }
        //                                    catch (Exception ex) { Debug.WriteLine($"LLW Learn 失敗: {ex.Message}"); }
        //                                }));
        //                            }

        //                            if (_recipe.RightLowWaferMat != null && !_recipe.RightLowWaferMat.Empty() && GV.matcherRLW != null)
        //                            {
        //                                learnTasks.Add(Task.Run(() =>
        //                                {
        //                                    try
        //                                    {
        //                                        GV.matcherRLW.LearnWithAlgo(_recipe.RightLowWaferMat, _recipe.RightLowWaferMask, _AlignC.RLWaferAlgorithm);
        //                                    }
        //                                    catch (Exception ex) { Debug.WriteLine($"RLW Learn 失敗: {ex.Message}"); }
        //                                }));
        //                            }

        //                            if (_recipe.LeftHighMaskMat != null && !_recipe.LeftHighMaskMat.Empty() && GV.matcherLHM != null)
        //                            {
        //                                learnTasks.Add(Task.Run(() =>
        //                                {
        //                                    try
        //                                    {
        //                                        GV.matcherLHM.LearnWithAlgo(_recipe.LeftHighMaskMat, _recipe.LeftHighMaskMask, _AlignC.LHMaskAlgorithm);
        //                                    }
        //                                    catch (Exception ex) { Debug.WriteLine($"LHM Learn 失敗: {ex.Message}"); }
        //                                }));
        //                            }

        //                            if (_recipe.RightHighMaskMat != null && !_recipe.RightHighMaskMat.Empty() && GV.matcherRHM != null)
        //                            {
        //                                learnTasks.Add(Task.Run(() =>
        //                                {
        //                                    try
        //                                    {
        //                                        GV.matcherRHM.LearnWithAlgo(_recipe.RightHighMaskMat, _recipe.RightHighMaskMask, _AlignC.RHMaskAlgorithm);
        //                                    }
        //                                    catch (Exception ex) { Debug.WriteLine($"RHM Learn 失敗: {ex.Message}"); }
        //                                }));
        //                            }

        //                            if (_recipe.LeftHighWaferMat != null && !_recipe.LeftHighWaferMat.Empty() && GV.matcherLHW != null)
        //                            {
        //                                learnTasks.Add(Task.Run(() =>
        //                                {
        //                                    try
        //                                    {
        //                                        GV.matcherLHW.LearnWithAlgo(_recipe.LeftHighWaferMat, _recipe.LeftHighWaferMask, _AlignC.LHWaferAlgorithm);
        //                                    }
        //                                    catch (Exception ex) { Debug.WriteLine($"LHW Learn 失敗: {ex.Message}"); }
        //                                }));
        //                            }

        //                            if (_recipe.RightHighWaferMat != null && !_recipe.RightHighWaferMat.Empty() && GV.matcherRHW != null)
        //                            {
        //                                learnTasks.Add(Task.Run(() =>
        //                                {
        //                                    try
        //                                    {
        //                                        GV.matcherRHW.LearnWithAlgo(_recipe.RightHighWaferMat, _recipe.RightHighWaferMask, _AlignC.RHWaferAlgorithm);
        //                                    }
        //                                    catch (Exception ex) { Debug.WriteLine($"RHW Learn 失敗: {ex.Message}"); }
        //                                }));
        //                            }

        //                            if (learnTasks.Count > 0)
        //                            {
        //                                Task.WaitAll(learnTasks.ToArray(), 5000); // 最多等 5 秒
        //                            }
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            Debug.WriteLine($"Matcher 學習失敗: {ex.Message}");
        //                        }
        //                    });
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Debug.WriteLine($"ShowMe 背景任務失敗: {ex.Message}");
        //            }
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine($"ShowMe 錯誤: {ex.Message}");
        //    }
        //}
        internal void ShowMe()
        {
            if (Interlocked.Exchange(ref _showMeBusy, 1) == 1) return;

            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                EditRecipe = GV.NowRecipeNumber;
                _recipe = GV._recipe;
                _AlignC = _recipe.AlignC;

                bool recipeChanged = (_lastShowRecipe != EditRecipe);
                _lastShowRecipe = EditRecipe;

                lbRecipeNumber.Text = GV.Dlang.strRecipeNumber + EditRecipe.ToString();
                btLabelPatternL.Visible = true;

                bool author = GV.AppSettingParm.Author != 0;
                btTfile.Visible = author;
                btTLRead.Visible = author;
                btTRRead.Visible = author;

                if (GMPLeft == null)
                {
                    GMPLeft = new Mat();
                    GMPRight = new Mat();
                }
                if (MoveCCDLeft == null)
                {
                    MoveCCDLeft = new Mat();
                    MoveCCDRight = new Mat();
                    GMPlMaskMp = new MatchPosition();
                    GMPrMaskMp = new MatchPosition();
                    GMPlWaferMp = new MatchPosition();
                    GMPrWaferMp = new MatchPosition();
                }
                if (MatchThread == null)
                {
                    MatchThread = new Thread(DrawMatchPosition) { IsBackground = true };
                    MatchThread.Start();
                }

                // UI 立即可互動：輕量設定同步做
                CbHLMagni_Change(NowMageni);
                Changelight();
                CheckParam();
                ApplyAlgorithmSelectionForCurrentMagnification();
                SyncAiClassListVisibility();

                // class list：Recipe 切換或目前清單為空時重抓，避免首次失敗後不再更新
                bool needRefreshClassList = recipeChanged || _lastClassListRecipe != EditRecipe || cbLWaferClassList.Items.Count == 0;
                if (needRefreshClassList)
                {
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            var classNames = await GV.AIClassList.GetClassNamesAsync();
                            var classList = new Dictionary<string, short>();
                            for (int i = 0; i < classNames.Count; i++) classList[classNames[i]] = (short)i;

                            await InvokeAsync(() =>
                            {
                                UpdateClassList(classList);
                                ApplyAlgorithmSelectionForCurrentMagnification();
                                SyncAiClassListVisibility();
                            });
                            await InvokeAsync(() =>
                            {
                                if (rBLowMagnification.Checked)
                                {
                                    if (_AlignC.LLWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && _AlignC.LLWaferAIClassId >= 0) SetComboBoxByClassId(cbLWaferClassList, _AlignC.LLWaferAIClassId);
                                    if (_AlignC.RLWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && _AlignC.RLWaferAIClassId >= 0) SetComboBoxByClassId(cbRWaferClassList, _AlignC.RLWaferAIClassId);
                                    if (_AlignC.LLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && _AlignC.LLMaskAIClassId >= 0) SetComboBoxByClassId(cbLMaskClassList, _AlignC.LLMaskAIClassId);
                                    if (_AlignC.RLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && _AlignC.RLMaskAIClassId >= 0) SetComboBoxByClassId(cbRMaskClassList, _AlignC.RLMaskAIClassId);
                                }
                                else
                                {
                                    if (_AlignC.LHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && _AlignC.LHWaferAIClassId >= 0) SetComboBoxByClassId(cbLWaferClassList, _AlignC.LHWaferAIClassId);
                                    if (_AlignC.RHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && _AlignC.RHWaferAIClassId >= 0) SetComboBoxByClassId(cbRWaferClassList, _AlignC.RHWaferAIClassId);
                                    if (_AlignC.LHMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && _AlignC.LHMaskAIClassId >= 0) SetComboBoxByClassId(cbLMaskClassList, _AlignC.LHMaskAIClassId);
                                    if (_AlignC.RHMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && _AlignC.RHMaskAIClassId >= 0) SetComboBoxByClassId(cbRMaskClassList, _AlignC.RHMaskAIClassId);
                                }

                                SyncAiClassListVisibility();
                            });

                            if (classList.Count > 0)
                            {
                                _lastClassListRecipe = EditRecipe;
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("Up.ShowMe classlist error: " + ex.Message);
                        }
                    });
                }

                // matcher 學習只在 recipe 改變時做一次
                if (GV.AppSettingParm?.Emulation != true && _lastLearnRecipe != EditRecipe)
                {
                    _lastLearnRecipe = EditRecipe;
                    _ = Task.Run(() =>
                    {
                        try
                        {
                            var learnTasks = new List<Task>();
                            if (_recipe.LeftLowMaskMat != null && !_recipe.LeftLowMaskMat.Empty() && GV.matcherLLM != null) learnTasks.Add(Task.Run(() => GV.matcherLLM.LearnWithAlgo(_recipe.LeftLowMaskMat, _recipe.LeftLowMaskMask, _AlignC.LLMaskAlgorithm)));
                            if (_recipe.RightLowMaskMat != null && !_recipe.RightLowMaskMat.Empty() && GV.matcherRLM != null) learnTasks.Add(Task.Run(() => GV.matcherRLM.LearnWithAlgo(_recipe.RightLowMaskMat, _recipe.RightLowMaskMask, _AlignC.RLMaskAlgorithm)));
                            if (_recipe.LeftLowWaferMat != null && !_recipe.LeftLowWaferMat.Empty() && GV.matcherLLW != null) learnTasks.Add(Task.Run(() => GV.matcherLLW.LearnWithAlgo(_recipe.LeftLowWaferMat, _recipe.LeftLowWaferMask, _AlignC.LLWaferAlgorithm)));
                            if (_recipe.RightLowWaferMat != null && !_recipe.RightLowWaferMat.Empty() && GV.matcherRLW != null) learnTasks.Add(Task.Run(() => GV.matcherRLW.LearnWithAlgo(_recipe.RightLowWaferMat, _recipe.RightLowWaferMask, _AlignC.RLWaferAlgorithm)));
                            if (_recipe.LeftHighMaskMat != null && !_recipe.LeftHighMaskMat.Empty() && GV.matcherLHM != null) learnTasks.Add(Task.Run(() => GV.matcherLHM.LearnWithAlgo(_recipe.LeftHighMaskMat, _recipe.LeftHighMaskMask, _AlignC.LHMaskAlgorithm)));
                            if (_recipe.RightHighMaskMat != null && !_recipe.RightHighMaskMat.Empty() && GV.matcherRHM != null) learnTasks.Add(Task.Run(() => GV.matcherRHM.LearnWithAlgo(_recipe.RightHighMaskMat, _recipe.RightHighMaskMask, _AlignC.RHMaskAlgorithm)));
                            if (_recipe.LeftHighWaferMat != null && !_recipe.LeftHighWaferMat.Empty() && GV.matcherLHW != null) learnTasks.Add(Task.Run(() => GV.matcherLHW.LearnWithAlgo(_recipe.LeftHighWaferMat, _recipe.LeftHighWaferMask, _AlignC.LHWaferAlgorithm)));
                            if (_recipe.RightHighWaferMat != null && !_recipe.RightHighWaferMat.Empty() && GV.matcherRHW != null) learnTasks.Add(Task.Run(() => GV.matcherRHW.LearnWithAlgo(_recipe.RightHighWaferMat, _recipe.RightHighWaferMask, _AlignC.RHWaferAlgorithm)));
                            Task.WaitAll(learnTasks.ToArray(), 5000);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("Up.ShowMe learn error: " + ex.Message);
                        }
                    });
                }

                Debug.WriteLine("CMLearnPatternUp.ShowMe ms=" + sw.ElapsedMilliseconds);
            }
            finally
            {
                Interlocked.Exchange(ref _showMeBusy, 0);
            }
        }

        private Task InvokeAsync(Action action)
        {
            if (IsDisposed) return Task.CompletedTask;
            if (!InvokeRequired)
            {
                action();
                return Task.CompletedTask;
            }

            var tcs = new TaskCompletionSource<bool>();
            BeginInvoke((MethodInvoker)delegate
            {
                try { action(); tcs.SetResult(true); }
                catch (Exception ex) { tcs.SetException(ex); }
            });
            return tcs.Task;
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
                cbRMaskClassList.Items.Clear();
                cbRWaferClassList.Items.Clear();
                cbLWaferClassList.Items.Clear();
                cbLMaskClassList.Items.Clear();

                foreach (var className in classList)
                {
                    cbRMaskClassList.Items.Add(className.Key);
                    cbRWaferClassList.Items.Add(className.Key);
                    cbLWaferClassList.Items.Add(className.Key);
                    cbLMaskClassList.Items.Add(className.Key);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"更新 ClassList 失敗: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void BtLMaskTemplateSave_Click(object sender, EventArgs e)
        {
            GV.TickCount = 0;

            if (cbLMaskAlgorithm.SelectedIndex == 2)  // AIMatch
            {
                if (cbLMaskClassList.SelectedIndex < 0 || cbLMaskClassList.SelectedItem == null)
                {
                    MessageBox.Show("請先選擇 AI 類別", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string confirmMsg = rBLowMagnification.Checked ? "確定儲存左側低倍光罩 AI 類別?" : "確定儲存左側高倍光罩 AI 類別?";
                if (MessageBox.Show(confirmMsg, "儲存 AI 類別", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                // 取得選擇的類別 ID
                string selectedClassName = cbLMaskClassList.SelectedItem.ToString();
                Dictionary<string, Int16> ClassList = GV.AIClassList.GetClassList();

                if (!ClassList.ContainsKey(selectedClassName))
                {
                    MessageBox.Show($"找不到類別 '{selectedClassName}'", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int classId = ClassList[selectedClassName];

                // 儲存 ClassId 到 AlignCondition
                if (rBLowMagnification.Checked)
                {
                    _AlignC.LLMaskAlgorithm = OpenCV3MatchUMat.AlignAlgorithm.AIMatch;
                    _AlignC.LLMaskAIClassId = classId;
                }
                else if (rBHighMagnification.Checked)
                {
                    _AlignC.LHMaskAlgorithm = OpenCV3MatchUMat.AlignAlgorithm.AIMatch;
                    _AlignC.LHMaskAIClassId = classId;
                }

                _AlignC.LastModifyTime = DateTime.Now;
                GM.WriteRecipeXml(EditRecipe);

                MessageBox.Show($"已儲存 AI 類別: {selectedClassName} (ID: {classId})", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //  TemplateMatch/EdgeMatch 模式：原有的影像處理流程
            Mat fullImage = GV.LeftUpCam.Grab();
            if (fullImage == null || fullImage.Empty())
            {
                MessageBox.Show("相機未取得影像，請確認相機連線狀態", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            OpenCvSharp.Rect roi = skLPattern.GetMaskRect();
            if (roi.Width <= 0 || roi.Height <= 0 ||
                roi.X < 0 || roi.Y < 0 ||
                roi.X + roi.Width > fullImage.Width ||
                roi.Y + roi.Height > fullImage.Height)
            {
                MessageBox.Show("ROI 超出影像範圍或無效", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                fullImage.Dispose();
                return;
            }

            Mat m = new Mat(fullImage, roi).Clone();

            if (cbLMaskAlgorithm.SelectedIndex == 1)
            {
                Cv2.GaussianBlur(m, m, new OpenCvSharp.Size(7, 7), 0, 0, BorderTypes.Default);
                Cv2.AdaptiveThreshold(m, m, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.Binary, 27, 2);
                Mat invertedImg = new Mat();
                Cv2.BitwiseNot(m, invertedImg);
                m = invertedImg.Clone();
                invertedImg.Dispose();
            }

            if (rBLowMagnification.Checked)
            {
                if (MessageBox.Show(GV.Dlang.strbtnSaveLLM, GV.Dlang.strSureSaveLLM, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                GV.Plc.GetRecipe();
                GV.Plc.GetLocation();

                _AlignC.LRingLight[NowMageni] = LRingLight.Checked;
                _AlignC.LeftRingBrightness[NowMageni] = tBLeftRingLight.Value;
                _AlignC.LeftLight[NowMageni] = LCoaLight.Checked;
                _AlignC.LeftBrightness[NowMageni] = tBLeftCoaLight.Value;
                _AlignC.LLMaskOffsetX = 0;
                _AlignC.LLMaskOffsetY = 0;
                _recipe.SetLeftLowMaskMat(m);
                pbLMaskImage.Image = m.ToBitmap();
                GrayImage d = new GrayImage(m.Width, m.Height);
                d.Fill(255);
                _recipe.LeftLowMaskMask = new GrayImage(d);
                _AlignC.LowMagnificationLZ = GV.NowLocation[GV.Plc.iiDSUpLeftZ];
                _AlignC.LLMaskAlgorithm = Algoritm(cbLMaskAlgorithm.SelectedIndex);
                _AlignC.LLMaskAIClassId = -1;

                if (GV.AppSettingParm.Emulation != true && GV.matcherLLM != null)
                {
                    GV.matcherLLM.LearnWithAlgo(m, d, _AlignC.LLMaskAlgorithm);
                }
            }
            else if (rBHighMagnification.Checked)
            {
                if (MessageBox.Show(GV.Dlang.strbtnSaveLHM, GV.Dlang.strSureSaveLHM, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                GV.Plc.GetRecipe();
                GV.Plc.GetLocation();

                _AlignC.LRingLight[NowMageni] = LRingLight.Checked;
                _AlignC.LeftRingBrightness[NowMageni] = tBLeftRingLight.Value;
                _AlignC.LeftLight[NowMageni] = LCoaLight.Checked;
                _AlignC.LeftBrightness[NowMageni] = tBLeftCoaLight.Value;
                _AlignC.LHMaskOffsetX = 0;
                _AlignC.LHMaskOffsetY = 0;
                _recipe.SetLeftHighMaskMat(m);
                pbLMaskImage.Image = m.ToBitmap();
                GrayImage d = new GrayImage(m.Width, m.Height);
                d.Fill(255);
                _recipe.LeftHighMaskMask = new GrayImage(d);
                _AlignC.HighMagnificationLZ = GV.NowLocation[GV.Plc.iiDSUpLeftZ];
                _AlignC.LHMaskAlgorithm = Algoritm(cbLMaskAlgorithm.SelectedIndex);
                _AlignC.LHMaskAIClassId = -1;

                if (GV.AppSettingParm.Emulation != true && GV.matcherLHM != null)
                {
                    GV.matcherLHM.LearnWithAlgo(m, d, _AlignC.LHMaskAlgorithm);
                }
            }
            else
            {
                MessageBox.Show("Select magnification first.", "LMaskTemplateSave", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _AlignC.LastModifyTime = DateTime.Now;
            GM.WriteRecipeXml(EditRecipe);
        }

        private void BtRWaferTempalteSave_Click(object sender, EventArgs e)
        {
            GV.TickCount = 0;
            if (cbRWaferAlgorithm.SelectedIndex == 2)  // AIMatch
            {
                if (cbRWaferClassList.SelectedIndex < 0 || cbRWaferClassList.SelectedItem == null)
                {
                    MessageBox.Show("請先選擇 AI 類別", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string confirmMsg = rBLowMagnification.Checked ? "確定儲存右側低倍晶圓 AI 類別?" : "確定儲存右側高倍晶圓 AI 類別?";
                if (MessageBox.Show(confirmMsg, "儲存 AI 類別", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                string selectedClassName = cbRWaferClassList.SelectedItem.ToString();
                Dictionary<string, Int16> ClassList = GV.AIClassList.GetClassList();

                if (!ClassList.ContainsKey(selectedClassName))
                {
                    MessageBox.Show($"找不到類別 '{selectedClassName}'", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int classId = ClassList[selectedClassName];

                if (rBLowMagnification.Checked)
                {
                    _AlignC.RLWaferAlgorithm = OpenCV3MatchUMat.AlignAlgorithm.AIMatch;
                    _AlignC.RLWaferAIClassId = classId;
                    _AlignC.RLBitwiseNot = classId;
                }
                else if (rBHighMagnification.Checked)
                {
                    _AlignC.RHWaferAlgorithm = OpenCV3MatchUMat.AlignAlgorithm.AIMatch;
                    _AlignC.RHWaferAIClassId = classId;
                    _AlignC.RHBitwiseNot = classId;
                }

                _AlignC.LastModifyTime = DateTime.Now;
                GM.WriteRecipeXml(EditRecipe);

                MessageBox.Show($"已儲存 AI 類別: {selectedClassName} (ID: {classId})", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //  TemplateMatch/EdgeMatch 模式：原有的影像處理流程
            Mat fullImage = GV.RightUpCam.Grab();
            if (fullImage == null || fullImage.Empty())
            {
                MessageBox.Show("右側相機未取得影像，請確認相機連線狀態", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            OpenCvSharp.Rect roi = skRPattern.GetWaferRect();
            if (roi.Width <= 0 || roi.Height <= 0 ||
                roi.X < 0 || roi.Y < 0 ||
                roi.X + roi.Width > fullImage.Width ||
                roi.Y + roi.Height > fullImage.Height)
            {
                MessageBox.Show("Wafer ROI 超出影像範圍或無效", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                fullImage.Dispose();
                return;
            }

            Mat m = new Mat(fullImage, roi).Clone();

            if (cbRWaferAlgorithm.SelectedIndex == 1)
            {
                Cv2.GaussianBlur(m, m, new OpenCvSharp.Size(7, 7), 0, 0, BorderTypes.Default);
                Cv2.AdaptiveThreshold(m, m, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.Binary, 27, 2);
                Mat invertedImg = new Mat();
                Cv2.BitwiseNot(m, invertedImg);
                m = invertedImg.Clone();
                invertedImg.Dispose();
            }

            if (rBLowMagnification.Checked)
            {
                if (MessageBox.Show(GV.Dlang.strbtnSaveRLW, GV.Dlang.strSureSaveRLW, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                GV.Plc.GetRecipe();
                GV.Plc.GetLocation();

                _AlignC.RightLight[NowMageni] = RCoaLight.Checked;
                _AlignC.RightBrightness[NowMageni] = tBRightCoaLight.Value;
                _AlignC.RRingLight[NowMageni] = RRingLight.Checked;
                _AlignC.RightRingBrightness[NowMageni] = tBRightRingLight.Value;
                _AlignC.RLWaferOffsetX = 0;
                _AlignC.RLWaferOffsetY = 0;
                _recipe.SetRightLowWaferMat(m);
                pbRWaferImage.Image = m.ToBitmap();
                GrayImage d = new GrayImage(m.Width, m.Height);
                d.Fill(255);
                _recipe.RightLowWaferMask = new GrayImage(d);
                _AlignC.LowMagnificationRZ = GV.NowLocation[GV.Plc.iiDSUpRightZ];
                _AlignC.RLWaferAlgorithm = Algoritm(cbRWaferAlgorithm.SelectedIndex);
                _AlignC.RLWaferAIClassId = -1;
                _AlignC.RLBitwiseNot = 0;

                if (GV.AppSettingParm.Emulation != true && GV.matcherRLW != null)
                {
                    GV.matcherRLW.LearnWithAlgo(m, d, _AlignC.RLWaferAlgorithm);
                }
            }
            else if (rBHighMagnification.Checked)
            {
                if (MessageBox.Show(GV.Dlang.strbtnSaveRHW, GV.Dlang.strSureSaveRHW, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                GV.Plc.GetRecipe();
                GV.Plc.GetLocation();

                _AlignC.RightLight[NowMageni] = RCoaLight.Checked;
                _AlignC.RightBrightness[NowMageni] = tBRightCoaLight.Value;
                _AlignC.RRingLight[NowMageni] = RRingLight.Checked;
                _AlignC.RightRingBrightness[NowMageni] = tBRightRingLight.Value;
                _AlignC.RHWaferOffsetX = 0;
                _AlignC.RHWaferOffsetY = 0;
                _recipe.SetRightHighWaferMat(m);
                pbRWaferImage.Image = m.ToBitmap();
                GrayImage d = new GrayImage(m.Width, m.Height);
                d.Fill(255);
                _recipe.RightHighWaferMask = new GrayImage(d);
                _AlignC.HighMagnificationRZ = GV.NowLocation[GV.Plc.iiDSUpRightZ];
                _AlignC.RHWaferAlgorithm = Algoritm(cbRWaferAlgorithm.SelectedIndex);
                _AlignC.RHWaferAIClassId = -1;
                _AlignC.RHBitwiseNot = 0;

                if (GV.AppSettingParm.Emulation != true && GV.matcherRHW != null)
                {
                    GV.matcherRHW.LearnWithAlgo(m, d, _AlignC.RHWaferAlgorithm);
                }
            }
            else
            {
                MessageBox.Show("Select magnification first.", "RWaferTemplateSave", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _AlignC.LastModifyTime = DateTime.Now;
            GM.WriteRecipeXml(EditRecipe);
        }

        private void BtRMaskTemplateSave_Click(object sender, EventArgs e)
        {
            GV.TickCount = 0;
            if (cbRMaskAlgorithm.SelectedIndex == 2)  // AIMatch
            {
                if (cbRMaskClassList.SelectedIndex < 0 || cbRMaskClassList.SelectedItem == null)
                {
                    MessageBox.Show("請先選擇 AI 類別", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string confirmMsg = rBLowMagnification.Checked ? "確定儲存右側低倍光罩 AI 類別?" : "確定儲存右側高倍光罩 AI 類別?";
                if (MessageBox.Show(confirmMsg, "儲存 AI 類別", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                string selectedClassName = cbRMaskClassList.SelectedItem.ToString();
                Dictionary<string, Int16> ClassList = GV.AIClassList.GetClassList();

                if (!ClassList.ContainsKey(selectedClassName))
                {
                    MessageBox.Show($"找不到類別 '{selectedClassName}'", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int classId = ClassList[selectedClassName];

                if (rBLowMagnification.Checked)
                {
                    _AlignC.RLMaskAlgorithm = OpenCV3MatchUMat.AlignAlgorithm.AIMatch;
                    _AlignC.RLMaskAIClassId = classId;
                }
                else if (rBHighMagnification.Checked)
                {
                    _AlignC.RHMaskAlgorithm = OpenCV3MatchUMat.AlignAlgorithm.AIMatch;
                    _AlignC.RHMaskAIClassId = classId;
                }

                _AlignC.LastModifyTime = DateTime.Now;
                GM.WriteRecipeXml(EditRecipe);

                MessageBox.Show($"已儲存 AI 類別: {selectedClassName} (ID: {classId})", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //  TemplateMatch/EdgeMatch 模式：原有的影像處理流程
            Mat fullImage = GV.RightUpCam.Grab();
            if (fullImage == null || fullImage.Empty())
            {
                MessageBox.Show("右側相機未取得影像，請確認相機連線狀態", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            OpenCvSharp.Rect roi = skRPattern.GetMaskRect();
            if (roi.Width <= 0 || roi.Height <= 0 ||
                roi.X < 0 || roi.Y < 0 ||
                roi.X + roi.Width > fullImage.Width ||
                roi.Y + roi.Height > fullImage.Height)
            {
                MessageBox.Show("Mask ROI 超出影像範圍或無效", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                fullImage.Dispose();
                return;
            }

            Mat m = new Mat(fullImage, roi).Clone();

            if (cbRMaskAlgorithm.SelectedIndex == 1)
            {
                Cv2.GaussianBlur(m, m, new OpenCvSharp.Size(7, 7), 0, 0, BorderTypes.Default);
                Cv2.AdaptiveThreshold(m, m, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.Binary, 27, 2);
                Mat invertedImg = new Mat();
                Cv2.BitwiseNot(m, invertedImg);
                m = invertedImg.Clone();
                invertedImg.Dispose();
            }

            if (rBLowMagnification.Checked)
            {
                if (MessageBox.Show(GV.Dlang.strbtnSaveRLM, GV.Dlang.strSureSaveRLM, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                GV.Plc.GetRecipe();
                GV.Plc.GetLocation();

                _AlignC.RightLight[NowMageni] = RCoaLight.Checked;
                _AlignC.RightBrightness[NowMageni] = tBRightCoaLight.Value;
                _AlignC.RRingLight[NowMageni] = RRingLight.Checked;
                _AlignC.RightRingBrightness[NowMageni] = tBRightRingLight.Value;
                _AlignC.RLMaskOffsetX = 0;
                _AlignC.RLMaskOffsetY = 0;
                _recipe.SetRightLowMaskMat(m);
                pbRMaskImage.Image = m.ToBitmap();
                GrayImage d = new GrayImage(m.Width, m.Height);
                d.Fill(255);
                _recipe.RightLowMaskMask = new GrayImage(d);
                _AlignC.LowMagnificationRZ = GV.NowLocation[GV.Plc.iiDSUpRightZ];
                _AlignC.RLMaskAlgorithm = Algoritm(cbRMaskAlgorithm.SelectedIndex);
                _AlignC.RLMaskAIClassId = -1;

                if (GV.AppSettingParm.Emulation != true && GV.matcherRLM != null)
                {
                    GV.matcherRLM.LearnWithAlgo(m, d, _AlignC.RLMaskAlgorithm);
                }
            }
            else if (rBHighMagnification.Checked)
            {
                if (MessageBox.Show(GV.Dlang.strbtnSaveRHM, GV.Dlang.strSureSaveRHM, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }

                GV.Plc.GetRecipe();
                GV.Plc.GetLocation();

                _AlignC.RightLight[NowMageni] = RCoaLight.Checked;
                _AlignC.RightBrightness[NowMageni] = tBRightCoaLight.Value;
                _AlignC.RRingLight[NowMageni] = RRingLight.Checked;
                _AlignC.RightRingBrightness[NowMageni] = tBRightRingLight.Value;
                _AlignC.RHMaskOffsetX = 0;
                _AlignC.RHMaskOffsetY = 0;
                _recipe.SetRightHighMaskMat(m);
                pbRMaskImage.Image = m.ToBitmap();
                GrayImage d = new GrayImage(m.Width, m.Height);
                d.Fill(255);
                _recipe.RightHighMaskMask = new GrayImage(d);
                _AlignC.HighMagnificationRZ = GV.NowLocation[GV.Plc.iiDSUpRightZ];
                _AlignC.RHMaskAlgorithm = Algoritm(cbRMaskAlgorithm.SelectedIndex);
                _AlignC.RHMaskAIClassId = -1;

                if (GV.AppSettingParm.Emulation != true && GV.matcherRHM != null)
                {
                    GV.matcherRHM.LearnWithAlgo(m, d, _AlignC.RHMaskAlgorithm);
                }
            }
            else
            {
                MessageBox.Show("Select magnification first.", "RMaskTemplateSave", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _AlignC.LastModifyTime = DateTime.Now;
            GM.WriteRecipeXml(EditRecipe);
        }

        private void BtLMaskLocationUp_Click(object sender, EventArgs e)
        {
            skLPattern.MaskMove(0, -1);
        }

        private void BtLMaskLocationDown_Click(object sender, EventArgs e)
        {
            skLPattern.MaskMove(0, 1);
        }

        private void BtLMaskLocationLeft_Click(object sender, EventArgs e)
        {
            skLPattern.MaskMove(-1, 0);
        }

        private void BtLMaskLocationRight_Click(object sender, EventArgs e)
        {
            skLPattern.MaskMove(1, 0);
        }

        private void BtLWaferLocationUp_Click(object sender, EventArgs e)
        {
            skLPattern.WaferMove(0, -1);
        }

        private void BtLWaferLocationDown_Click(object sender, EventArgs e)
        {
            skLPattern.WaferMove(0, 1);
        }

        private void BtLWaferLocationLeft_Click(object sender, EventArgs e)
        {
            skLPattern.WaferMove(-1, 0);
        }

        private void BtLWaferLocationRight_Click(object sender, EventArgs e)
        {
            skLPattern.WaferMove(1, 0);
        }

        private void BtRMaskLocationUp_Click(object sender, EventArgs e)
        {
            skRPattern.MaskMove(0, -1);
        }

        private void BtRMaskLocationDown_Click(object sender, EventArgs e)
        {
            skRPattern.MaskMove(0, 1);
        }

        private void BtRMaskLocationLeft_Click(object sender, EventArgs e)
        {
            skRPattern.MaskMove(-1, 0);
        }

        private void BtRMaskLocationRight_Click(object sender, EventArgs e)
        {
            skRPattern.MaskMove(1, 0);
        }

        private void BtRWaferLocationUp_Click(object sender, EventArgs e)
        {
            skRPattern.WaferMove(0, -1);
        }

        private void BtRWaferLocationDown_Click(object sender, EventArgs e)
        {
            skRPattern.WaferMove(0, 1);
        }

        private void BtRWaferLocationLeft_Click(object sender, EventArgs e)
        {
            skRPattern.WaferMove(-1, 0);
        }

        private void BtRWaferLocationRight_Click(object sender, EventArgs e)
        {
            skRPattern.WaferMove(1, 0);
        }

        private void BtCreateLWaferPatternMask_Click(object sender, EventArgs e)
        {
            if (!rBHighMagnification.Checked && !rBLowMagnification.Checked)
                return;

            Mat mat = rBHighMagnification.Checked ? _recipe.LeftHighWaferMat : _recipe.LeftLowWaferMat;

            if (mat.Empty()) return;

            GrayImage gmas = rBHighMagnification.Checked ? _recipe.LeftHighWaferMask : _recipe.LeftLowWaferMask;

            if ((gmas == null) || (gmas.Width != mat.Width || gmas.Height != mat.Height))
            {
                gmas = new GrayImage(mat.Width, mat.Height);
            }

            DialogPaintMask d = new DialogPaintMask()
            {
                Image = mat,
                Mask = gmas
            };
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (rBHighMagnification.Checked)
                {
                    //GV.LeftHighWaferMask[EditRecipe] = d.Mask;
                    //GV.LeftHighWaferMask[EditRecipe].Save(GetTemplateFileName("MLHW"), ImageFormat.Bmp);
                    _recipe.LeftHighWaferMask = new GrayImage(d.Mask);
                    GV.matcherLHW.LearnWithAlgo(mat, d.Mask, _AlignC.LHWaferAlgorithm);
                    _AlignC.LastModifyTime = DateTime.Now;
                }
                else
                {
                    //GV.LeftLowWaferMask[EditRecipe] = d.Mask;
                    //GV.LeftLowWaferMask[EditRecipe].Save(GetTemplateFileName("MLLW"), ImageFormat.Bmp);
                    _recipe.LeftLowWaferMask = new GrayImage(d.Mask);
                    GV.matcherLLW.LearnWithAlgo(mat, d.Mask, _AlignC.LLWaferAlgorithm);
                    _AlignC.LastModifyTime = DateTime.Now;
                }
                GM.WriteRecipeXml(EditRecipe);
            }
        }

        private void BtCreateLMaskPatternMask_Click(object sender, EventArgs e)
        {
            if (!rBHighMagnification.Checked && !rBLowMagnification.Checked)
                return;

            Mat mat = rBHighMagnification.Checked ? _recipe.LeftHighMaskMat : _recipe.LeftLowMaskMat;

            if (mat.Empty()) return;

            GrayImage gmas = rBHighMagnification.Checked ? _recipe.LeftHighMaskMask : _recipe.LeftLowMaskMask;

            if ((gmas == null) || (gmas.Width != mat.Width || gmas.Height != mat.Height))
            {
                gmas = new GrayImage(mat.Width, mat.Height);
            }
            DialogPaintMask d = new DialogPaintMask()
            {
                Image = mat,
                Mask = gmas
            };
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (rBHighMagnification.Checked)
                {
                    //GV.LeftHighMaskMask[EditRecipe] = d.Mask;
                    //GV.LeftHighMaskMask[EditRecipe].Save(GetTemplateFileName("MLHM"), ImageFormat.Bmp);
                    _recipe.LeftHighMaskMask = new GrayImage(d.Mask);
                    GV.matcherLHM.LearnWithAlgo(mat, d.Mask, _AlignC.LHMaskAlgorithm);
                    _AlignC.LastModifyTime = DateTime.Now;
                }
                else
                {
                    //GV.LeftLowMaskMask[EditRecipe] = d.Mask;
                    //GV.LeftLowMaskMask[EditRecipe].Save(GetTemplateFileName("MLLM"), ImageFormat.Bmp);
                    _recipe.LeftLowMaskMask = new GrayImage(d.Mask);
                    GV.matcherLLM.LearnWithAlgo(mat, d.Mask, _AlignC.LLMaskAlgorithm);
                    _AlignC.LastModifyTime = DateTime.Now;
                }
                GM.WriteRecipeXml(EditRecipe);
            }
        }

        private void BtCreateRWaferPatternMask_Click(object sender, EventArgs e)
        {
            if (!rBHighMagnification.Checked && !rBLowMagnification.Checked)
                return;

            Mat mat = rBHighMagnification.Checked ? _recipe.RightHighWaferMat : _recipe.RightLowWaferMat;

            if (mat.Empty()) return;

            GrayImage gmas = rBHighMagnification.Checked ? _recipe.RightHighWaferMask : _recipe.RightLowWaferMask;

            if ((gmas == null) || (gmas.Width != mat.Width || gmas.Height != mat.Height))
            {
                gmas = new GrayImage(mat.Width, mat.Height);
            }

            DialogPaintMask d = new DialogPaintMask()
            {
                Image = mat,
                Mask = gmas
            };
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (rBHighMagnification.Checked)
                {
                    //GV.RightHighWaferMask[EditRecipe] = d.Mask;
                    //GV.RightHighWaferMask[EditRecipe].Save(GetTemplateFileName("MRHW"), ImageFormat.Bmp);
                    _recipe.RightHighWaferMask = new GrayImage(d.Mask);
                    GV.matcherRHW.LearnWithAlgo(mat, d.Mask, _AlignC.RHWaferAlgorithm);
                    _AlignC.LastModifyTime = DateTime.Now;
                }
                else
                {
                    //GV.RightLowWaferMask[EditRecipe] = d.Mask;
                    //GV.RightLowWaferMask[EditRecipe].Save(GetTemplateFileName("MRLW"), ImageFormat.Bmp);
                    _recipe.RightLowWaferMask = new GrayImage(d.Mask);
                    GV.matcherRLW.LearnWithAlgo(mat, d.Mask, _AlignC.RLWaferAlgorithm);
                    _AlignC.LastModifyTime = DateTime.Now;
                }
                GM.WriteRecipeXml(EditRecipe);
            }
        }

        private void BtCreateRMaskPatternMask_Click(object sender, EventArgs e)
        {
            if (!rBHighMagnification.Checked && !rBLowMagnification.Checked)
                return;

            Mat mat = rBHighMagnification.Checked ? _recipe.RightHighMaskMat : _recipe.RightLowMaskMat;

            if (mat.Empty()) return;

            GrayImage gmas = rBHighMagnification.Checked ? _recipe.RightHighMaskMask : _recipe.RightLowMaskMask;

            if ((gmas == null) || (gmas.Width != mat.Width || gmas.Height != mat.Height))
            {
                gmas = new GrayImage(mat.Width, mat.Height);
            }
            DialogPaintMask d = new DialogPaintMask()
            {
                Image = mat,
                Mask = gmas
            };
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (rBHighMagnification.Checked)
                {
                    //GV.RightHighMaskMask[EditRecipe] = d.Mask;
                    //GV.RightHighMaskMask[EditRecipe].Save(GetTemplateFileName("MRHM"), ImageFormat.Bmp);
                    _recipe.RightHighMaskMask = new GrayImage(d.Mask);
                    GV.matcherRHM.LearnWithAlgo(mat, d.Mask, _AlignC.RHMaskAlgorithm);
                    _AlignC.LastModifyTime = DateTime.Now;
                }
                else
                {
                    //GV.RightLowMaskMask[EditRecipe] = d.Mask;
                    //GV.RightLowMaskMask[EditRecipe].Save(GetTemplateFileName("MRLM"), ImageFormat.Bmp);
                    _recipe.RightLowMaskMask = new GrayImage(d.Mask);
                    GV.matcherRLM.LearnWithAlgo(mat, d.Mask, _AlignC.RLMaskAlgorithm);
                    _AlignC.LastModifyTime = DateTime.Now;
                }
                GM.WriteRecipeXml(EditRecipe);
            }
        }

        private void CbLMaskAlgorithm_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rBLowMagnification.Checked)
            {
                _AlignC.LLMaskAlgorithm = Algoritm(cbLMaskAlgorithm.SelectedIndex);
                if (_recipe.LeftLowMaskMat != null && !_recipe.LeftLowMaskMat.Empty())
                {
                    if (GV.AppSettingParm.Emulation != true)
                    {
                        GV.matcherLLM.LearnWithAlgo(_recipe.LeftLowMaskMat, _recipe.LeftLowMaskMask, _AlignC.LLMaskAlgorithm);
                    }
                }
            }
            else if (rBHighMagnification.Checked)
            {
                _AlignC.LHMaskAlgorithm = Algoritm(cbLMaskAlgorithm.SelectedIndex);
                if (_recipe.LeftHighMaskMat != null && !_recipe.LeftHighMaskMat.Empty())
                {
                    if (GV.AppSettingParm.Emulation != true)
                    {
                        GV.matcherLHM.LearnWithAlgo(_recipe.LeftHighMaskMat, _recipe.LeftHighMaskMask, _AlignC.LHMaskAlgorithm);
                    }

                }
            }
            if (cbLMaskAlgorithm.SelectedIndex == 2)
            {

                cbLMaskClassList.Visible = true;
                btLabelPatternLM.Visible = false;
                pbLMaskImage.Visible = false;
                btFindLMaskCenter.Visible = false;
                btCreateLMaskPatternMask.Visible = false;
            }
            else
            {
                cbLMaskClassList.Visible = false;
                btLabelPatternLM.Visible = false;
                pbLMaskImage.Visible = true;
                btFindLMaskCenter.Visible = true;
                btCreateLMaskPatternMask.Visible = true;
            }
            CheckParam();
        }

        private void CbRWaferAlgorithm_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rBLowMagnification.Checked)
            {
                _AlignC.RLWaferAlgorithm = Algoritm(cbRWaferAlgorithm.SelectedIndex);
                if (_recipe.RightLowWaferMat != null && !_recipe.RightLowMaskMat.Empty())
                {
                    if (GV.AppSettingParm.Emulation != true)
                    {
                        GV.matcherRLW.LearnWithAlgo(_recipe.RightLowWaferMat, _recipe.RightLowWaferMask, _AlignC.RLWaferAlgorithm);
                    }
                }
            }
            else if (rBHighMagnification.Checked)
            {
                _AlignC.RHWaferAlgorithm = Algoritm(cbRWaferAlgorithm.SelectedIndex);
                if (_recipe.RightHighWaferMat != null && !_recipe.RightHighMaskMat.Empty())
                {
                    GV.matcherRHW.LearnWithAlgo(_recipe.RightHighWaferMat, _recipe.RightHighWaferMask, _AlignC.RHWaferAlgorithm);
                }
            }
            if (cbRWaferAlgorithm.SelectedIndex == 2)
            {
                cbRWaferClassList.Visible = true;
                btLabelPatternR.Visible = false;
                pbRWaferImage.Visible = false;
                btFindRWaferCenter.Visible = false;
                btCreateRWaferPatternMask.Visible = false;
            }
            else
            {
                cbRWaferClassList.Visible = false;
                btLabelPatternR.Visible = false;
                pbRWaferImage.Visible = true;
                btFindRWaferCenter.Visible = true;
                btCreateRWaferPatternMask.Visible = true;
            }
            CheckParam();
        }

        private void CbLowMagnification_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rBLowMagnification.Checked)
            {
                rBHighMagnification.Enabled = false;
                rBLowMagnification.Enabled = false;
                cbHighMagnification.Enabled = false;
                cbLowMagnification.Enabled = false;

                if (GV.AppSettingParm.DebugMode)
                {
                    lbDebugMsg.Text = "NowMageni be = " + NowMageni.ToString();
                }

                NowMageni = cbLowMagnification.SelectedIndex;

                if (GV.AppSettingParm.DebugMode)
                {
                    lbDebugMsg.Text = "NowMageni af = " + NowMageni.ToString();
                }

                // cbHLMagni_Change(NowMageni);
                // Task t1 = Task.Run(() => ChangeLensMagnification(NowMageni));
                // await t1;
                ChangeLensMagnification(NowMageni);

                Changelight();

                Thread.Sleep(iDelayT);

                rBHighMagnification.Enabled = true;
                rBLowMagnification.Enabled = true;
                if (GV.UserLevel != GV.User.Operator)
                {
                    cbHighMagnification.Enabled = false;
                    cbLowMagnification.Enabled = true;
                }

                CheckParam();
            }
        }

        private void CbHighMagnification_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rBHighMagnification.Checked)
            {
                rBHighMagnification.Enabled = false;
                rBLowMagnification.Enabled = false;
                cbHighMagnification.Enabled = false;
                cbLowMagnification.Enabled = false;

                NowMageni = cbHighMagnification.SelectedIndex;

                // cbHLMagni_Change(NowMageni);
                // Task t1 = Task.Run(() => ChangeLensMagnification(NowMageni));
                // await t1;

                ChangeLensMagnification(NowMageni);

                Changelight();

                Thread.Sleep(iDelayT);

                rBHighMagnification.Enabled = true;
                rBLowMagnification.Enabled = true;
                if (GV.UserLevel != GV.User.Operator)
                {
                    cbHighMagnification.Enabled = true;
                    cbLowMagnification.Enabled = false;
                }

                CheckParam();
            }
        }

        private async void CbHLMagni_Change(int NowMagn)
        {
            Task t1 = Task.Run(() => ChangeLensMagnification(NowMagn));
            await t1;
        }

        private void LRingLight_CheckedChanged(object sender, EventArgs e)
        {
            if (LRingLight.Checked)
            {
                LRingLight.ForeColor = Color.Green;
                tBLeftRingLight.Enabled = true;
                btLRingLightPlus.Enabled = true;
                btLRingLightMinus.Enabled = true;
                tBLeftRingLight.Value = _LeftRingBrightness;
            }
            else
            {
                LRingLight.ForeColor = Color.DarkSeaGreen;
                tBLeftRingLight.Enabled = false;
                btLRingLightPlus.Enabled = false;
                btLRingLightMinus.Enabled = false;
                tBLeftRingLight.Value = 0;
            }
            CheckParam();
        }

        private void RRingLight_CheckedChanged(object sender, EventArgs e)
        {
            if (RRingLight.Checked)
            {
                RRingLight.ForeColor = Color.Green;
                tBRightRingLight.Enabled = true;
                btRRingLightPlus.Enabled = true;
                btRRingLightMinus.Enabled = true;
                tBRightRingLight.Value = _RightRingBrightness;
            }
            else
            {
                RRingLight.ForeColor = Color.DarkSeaGreen;
                tBRightRingLight.Enabled = false;
                btRRingLightPlus.Enabled = false;
                btRRingLightMinus.Enabled = false;
                tBRightRingLight.Value = 0;
            }
            CheckParam();
        }

        private void LCoaLight_CheckedChanged(object sender, EventArgs e)
        {
            if (LCoaLight.Checked)
            {
                LCoaLight.ForeColor = Color.Red;
                tBLeftCoaLight.Enabled = true;
                btLCoaLightPlus.Enabled = true;
                btLCoaLightMinus.Enabled = true;
                tBLeftCoaLight.Value = _LeftBrightness;
            }
            else
            {
                LCoaLight.ForeColor = Color.RosyBrown;
                tBLeftCoaLight.Enabled = false;
                btLCoaLightPlus.Enabled = false;
                btLCoaLightMinus.Enabled = false;
                tBLeftCoaLight.Value = 0;
            }
            CheckParam();
        }

        private void RCoaLight_CheckedChanged(object sender, EventArgs e)
        {
            if (RCoaLight.Checked)
            {
                RCoaLight.ForeColor = Color.Red;
                tBRightCoaLight.Enabled = true;
                btRCoaLightPlus.Enabled = true;
                btRCoaLightMinus.Enabled = true;
                tBRightCoaLight.Value = _RightBrightness;
            }
            else
            {
                RCoaLight.ForeColor = Color.RosyBrown;
                tBRightCoaLight.Enabled = false;
                btRCoaLightPlus.Enabled = false;
                btRCoaLightMinus.Enabled = false;
                tBRightCoaLight.Value = 0;
            }
            CheckParam();
        }

        public void Changelight()
        {
            if (_AlignC.LeftLight[NowMageni])
            {
                LCoaLight.Checked = true;
                tBLeftCoaLight.Value = _AlignC.LeftBrightness[NowMageni];
            }
            else
            {
                LCoaLight.Checked = false;
                tBLeftCoaLight.Value = 0;
            }
            if (_AlignC.LRingLight[NowMageni])
            {
                LRingLight.Checked = true;
                tBLeftRingLight.Value = _AlignC.LeftRingBrightness[NowMageni];
            }
            else
            {
                LRingLight.Checked = false;
                tBLeftRingLight.Value = 0;
            }
            if (_AlignC.RightLight[NowMageni])
            {
                RCoaLight.Checked = true;
                tBRightCoaLight.Value = _AlignC.RightBrightness[NowMageni];
            }
            else
            {
                RCoaLight.Checked = false;
                tBRightCoaLight.Value = 0;
            }
            if (_AlignC.RRingLight[NowMageni])
            {
                RRingLight.Checked = true;
                tBRightRingLight.Value = _AlignC.RightRingBrightness[NowMageni];
            }
            else
            {
                RRingLight.Checked = false;
                tBRightRingLight.Value = 0;
            }
        }

        private void ChangeLensMagnification(int motormagnification)
        {
            try
            {
                // CheckIfPlcStop();

                lock (_locker)
                {
                    int oMagni = GV.LeftZoomLens.Magnification;

                    GV.LeftZoomLens.Magnification = motormagnification;
                    GV.RightZoomLens.Magnification = motormagnification;


                    Task tZoomAdj = Task.Factory.StartNew(() =>
                    {
                        GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, true);

                        GV.Plc.ZoomAdj(oMagni, motormagnification);

                        GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, false);
                    });

                    Task tLeft = Task.Factory.StartNew(() =>
                    {
                        GV.LeftZoomLens.MoveGoto(GV.ZoomLensInfo.LeftMagnificationMotorSteps[motormagnification]);
                    });

                    Task tRight = Task.Factory.StartNew(() =>
                    {
                        GV.RightZoomLens.MoveGoto(GV.ZoomLensInfo.RightMagnificationMotorSteps[motormagnification]);
                    });

                    Stopwatch sw = Stopwatch.StartNew();
                    while (true)
                    {
                        Thread.Sleep(100);
                        if (GV.AppSettingParm.DebugMode)
                        {
                            //int leftCount = GV.LeftZoomLens.GetPulseCount();
                            //int rightCount4 = GV.RightZoomLens.GetPulseCount4();
                            int leftCount = 0;
                            int rightCount4 = 0;
                            lbDebugMsg.Text = String.Format("left count = {0:d} Right count4 = {0:d}", leftCount, rightCount4);
                        }
                        if (GV.LeftZoomLens.GetStatus() && GV.RightZoomLens.GetStatus())
                        {
                            if (!GV.AppSettingParm.DebugMode)
                            {
                                int leftCount = GV.LeftZoomLens.GetPulseCount();
                                int rightCount4 = GV.RightZoomLens.GetPulseCount4();
                                //int leftCount = 0;
                                //int rightCount4 = 0;
                                lbDebugMsg.Text = String.Format("left count = {0:d} Right count4 = {0:d}", leftCount, rightCount4);
                            }

                            return;
                        }
                        if (sw.ElapsedMilliseconds > 3000)
                        {
                            //GV.LeftZoomLens.SetStatus();
                            //GV.RightZoomLens.SetStatus();
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("ChangeLensMagnification fail");
                // throw;
            }
        }

        public void CheckIfPlcStop()
        {
            /*
            if (GV.Plc.Send("RD MR26304") == "1\r\n" || GV.Plc.Send("RD MR21803") == "1\r\n")
            {
                // throw new MRException("Plc abort");
                // Debug.WriteLine("Plc abort");
            }
            */
        }

        private void BtLCoaLightMinus_Click(object sender, EventArgs e)
        {
            if (tBLeftCoaLight.Value > 1)
                tBLeftCoaLight.Value--;
        }

        private void BtLCoaLightPlus_Click(object sender, EventArgs e)
        {
            if (tBLeftCoaLight.Value < 254)
                tBLeftCoaLight.Value++;
        }

        private void BtRCoaLightPlus_Click(object sender, EventArgs e)
        {
            if (tBRightCoaLight.Value < 254)
                tBRightCoaLight.Value++;
        }

        private void BtRCoaLightMinus_Click(object sender, EventArgs e)
        {
            if (tBRightCoaLight.Value > 1)
                tBRightCoaLight.Value--;
        }

        private void TBLeftCoaLight_ValueChanged(object sender, EventArgs e)
        {
            _LeftBrightness = tBLeftCoaLight.Value;
            GV.Light.ChangeBrightness("left", _LeftBrightness);
            CheckParam();
        }

        private void TBRightCoaLight_ValueChanged(object sender, EventArgs e)
        {
            _RightBrightness = tBRightCoaLight.Value;
            GV.Light.ChangeBrightness("right", _RightBrightness);
            CheckParam();
        }

        private void TBLeftRingLight_ValueChanged(object sender, EventArgs e)
        {
            _LeftRingBrightness = tBLeftRingLight.Value;
            GV.RingLight.ChangeBrightness("left", _LeftRingBrightness);
            CheckParam();
        }

        private void TBRightRingLight_ValueChanged(object sender, EventArgs e)
        {
            _RightRingBrightness = tBRightRingLight.Value;
            GV.RingLight.ChangeBrightness("right", _RightRingBrightness);
            CheckParam();
        }

        private void UcLeftWaferNavigator_CommandPressed(int dir)
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
            skLPattern.WaferMove(x, y);
        }

        private void UcLeftMaskNavigator_CommandPressed(int dir)
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
            skLPattern.MaskMove(x, y);
        }

        private void UcRightWaferNavigator_CommandPressed(int dir)
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
            skRPattern.WaferMove(x, y);
        }

        private void UcRightMaskNavigator_CommandPressed(int dir)
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
            skRPattern.MaskMove(x, y);
        }

        private void CbPatterhShift_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckParam();
        }

        private void BtLRingLightPlus_Click(object sender, EventArgs e)
        {
            if (tBLeftRingLight.Value < 254)
                tBLeftRingLight.Value++;
        }

        private void BtLRingLightMinus_Click(object sender, EventArgs e)
        {
            if (tBLeftRingLight.Value > 1)
                tBLeftRingLight.Value--;
        }

        private void BtRRingLightPlus_Click(object sender, EventArgs e)
        {
            if (tBRightRingLight.Value < 254)
                tBRightRingLight.Value++;
        }

        private void BtRRingLightMinus_Click(object sender, EventArgs e)
        {
            if (tBRightRingLight.Value > 1)
                tBRightRingLight.Value--;
        }

        private void BtLRingSave_Click(object sender, EventArgs e)
        {
            if (rBLowMagnification.Checked)
            {
                if (MessageBox.Show("Sure to save Left Low Mag Ring Light ?", "Left Low Mag Ring Light", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                _AlignC.LRingLight[NowMageni] = LRingLight.Checked;
                _AlignC.LeftRingBrightness[NowMageni] = tBLeftRingLight.Value;
            }
            else
            {
                if (MessageBox.Show("Sure to save Left High Mag Ring Light ?", "Left High Mag Ring Light", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                _AlignC.LRingLight[NowMageni] = LRingLight.Checked;
                _AlignC.LeftRingBrightness[NowMageni] = tBLeftRingLight.Value;
            }

            _AlignC.LastModifyTime = DateTime.Now;
            //GM.WriteAlignConditionsAcarToXml("AlignConditions.xml");
            GM.WriteRecipeXml(EditRecipe);
        }

        private void BtLCoaSave_Click(object sender, EventArgs e)
        {
            if (rBLowMagnification.Checked)
            {
                if (MessageBox.Show("Sure to save Left Low Mag Coaxial Light ?", "Left Low Mag Coaxial Light", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                _AlignC.LeftLight[NowMageni] = LCoaLight.Checked;
                _AlignC.LeftBrightness[NowMageni] = tBLeftCoaLight.Value;
            }
            else
            {
                if (MessageBox.Show("Sure to save Left High Mag Coaxial Light ?", "Left High Mag Coaxial Light", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                _AlignC.LeftLight[NowMageni] = LCoaLight.Checked;
                _AlignC.LeftBrightness[NowMageni] = tBLeftCoaLight.Value;
            }

            _AlignC.LastModifyTime = DateTime.Now;
            GM.WriteRecipeXml(EditRecipe);
        }

        private void BtRCoaSave_Click(object sender, EventArgs e)
        {
            if (rBLowMagnification.Checked)
            {
                if (MessageBox.Show("Sure to save Right Low Mag Coaxial Light ?", "Right Low Mag Coaxial Light", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                _AlignC.RightLight[NowMageni] = RCoaLight.Checked;
                _AlignC.RightBrightness[NowMageni] = tBRightCoaLight.Value;
            }
            else
            {
                if (MessageBox.Show("Sure to save Right High Mag Coaxial Light ?", "Right High Mag Coaxial Light", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                _AlignC.RightLight[NowMageni] = RCoaLight.Checked;
                _AlignC.RightBrightness[NowMageni] = tBRightCoaLight.Value;
            }

            _AlignC.LastModifyTime = DateTime.Now;
            GM.WriteRecipeXml(EditRecipe);
        }

        private void BtRRingSave_Click(object sender, EventArgs e)
        {

            if (rBLowMagnification.Checked)
            {
                if (MessageBox.Show("Sure to save Right Low Mag Ring Light ?", "Right Low Mag Ring Light", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                _AlignC.RRingLight[NowMageni] = RRingLight.Checked;
                _AlignC.RightRingBrightness[NowMageni] = tBRightRingLight.Value;
            }
            else
            {
                if (MessageBox.Show("Sure to save Right High Mag Ring Light ?", "Right High Mag Ring Light", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                _AlignC.RRingLight[NowMageni] = RRingLight.Checked;
                _AlignC.RightRingBrightness[NowMageni] = tBRightRingLight.Value;
            }

            _AlignC.LastModifyTime = DateTime.Now;
            GM.WriteRecipeXml(EditRecipe);
        }

        private void BtMagSave_Click(object sender, EventArgs e)
        {
            int mag;
            string msg = null;

            if (rBLowMagnification.Checked)
            {
                mag = cbLowMagnification.SelectedIndex + 1;
                msg = string.Format(GV.Dlang.strLowMagnification, mag.ToString());
            }
            else if (rBHighMagnification.Checked)
            {
                mag = cbHighMagnification.SelectedIndex + 1;
                msg = string.Format(GV.Dlang.strHighMagnification, mag);
                msg += "\r\n";
                msg += string.Format(GV.Dlang.strPatternShift, cbPatterhShift.SelectedItem.ToString());
            }

            string smsg = GV.Dlang.strSureSave + "\r\n" + msg + "\r\n" + GV.Dlang.strTopAlignMode;
            if (MessageBox.Show(smsg, GV.Dlang.strSaveMagnification, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            // MessageBox1 msg1 = new MessageBox1(smsg, GV.Dlang.strSaveMagnification, MessageBoxIcon.Question);
            // DialogResult m1 = msg1.ShowDialog();
            // if (m1 != DialogResult.OK)
            //     return;

            if (rBLowMagnification.Checked)
            {
                _AlignC.AlignLowMagnification = cbLowMagnification.SelectedIndex;
                _AlignC.LLMaskAlgorithm = Algoritm(cbLMaskAlgorithm.SelectedIndex);
                _AlignC.RLMaskAlgorithm = Algoritm(cbRMaskAlgorithm.SelectedIndex);
                _AlignC.LLWaferAlgorithm = Algoritm(cbLWaferAlgorithm.SelectedIndex);
                _AlignC.RLWaferAlgorithm = Algoritm(cbRWaferAlgorithm.SelectedIndex);
            }
            else if (rBHighMagnification.Checked)
            {
                _AlignC.AlignHighMagnification = cbHighMagnification.SelectedIndex;
                _AlignC.PatternShift = cbPatterhShift.SelectedIndex;
                _AlignC.LHMaskAlgorithm = Algoritm(cbLMaskAlgorithm.SelectedIndex);
                _AlignC.RHMaskAlgorithm = Algoritm(cbRMaskAlgorithm.SelectedIndex);
                _AlignC.LHWaferAlgorithm = Algoritm(cbLWaferAlgorithm.SelectedIndex);
                _AlignC.RHWaferAlgorithm = Algoritm(cbRWaferAlgorithm.SelectedIndex);
            }

            _AlignC.LRingLight[NowMageni] = LRingLight.Checked;
            _AlignC.LeftRingBrightness[NowMageni] = tBLeftRingLight.Value;
            _AlignC.LeftLight[NowMageni] = LCoaLight.Checked;
            _AlignC.LeftBrightness[NowMageni] = tBLeftCoaLight.Value;
            _AlignC.RightLight[NowMageni] = RCoaLight.Checked;
            _AlignC.RightBrightness[NowMageni] = tBRightCoaLight.Value;
            _AlignC.RRingLight[NowMageni] = RRingLight.Checked;
            _AlignC.RightRingBrightness[NowMageni] = tBRightRingLight.Value;

            _AlignC.UpBackAlign = 1;
            _AlignC.LastModifyTime = DateTime.Now;
            GM.WriteRecipeXml(EditRecipe);
            CheckParam();
        }

        private void CheckParam()
        {
            GV.TickCount = 0;
            Color Ocolor = btMagSaveLow.BackColor;
            Color Ncolor;

            if (_AlignC.LRingLight[NowMageni] != LRingLight.Checked)
            {
                Ncolor = Color.LightGreen;
            }
            else if ((LRingLight.Checked) && (_AlignC.LeftRingBrightness[NowMageni] != tBLeftRingLight.Value))
            {
                Ncolor = Color.LightGreen;
            }
            else if (_AlignC.LeftLight[NowMageni] != LCoaLight.Checked)
            {
                Ncolor = Color.LightGreen;
            }
            else if ((LCoaLight.Checked) && (_AlignC.LeftBrightness[NowMageni] != tBLeftCoaLight.Value))
            {
                Ncolor = Color.LightGreen;
            }
            else if (_AlignC.RightLight[NowMageni] != RCoaLight.Checked)
            {
                Ncolor = Color.LightGreen;
            }
            else if ((RCoaLight.Checked) && (_AlignC.RightBrightness[NowMageni] != tBRightCoaLight.Value))
            {
                Ncolor = Color.LightGreen;
            }
            else if (_AlignC.RRingLight[NowMageni] != RRingLight.Checked)
            {
                Ncolor = Color.LightGreen;
            }
            else if ((RRingLight.Checked) && (_AlignC.RightRingBrightness[NowMageni] != tBRightRingLight.Value))
            {
                Ncolor = Color.LightGreen;
            }
            else if ((rBLowMagnification.Checked) && (_AlignC.AlignLowMagnification != cbLowMagnification.SelectedIndex))
            {
                Ncolor = Color.LightGreen;
            }
            else if ((rBHighMagnification.Checked) && (_AlignC.AlignHighMagnification != cbHighMagnification.SelectedIndex))
            {
                Ncolor = Color.LightGreen;
            }
            else if ((rBHighMagnification.Checked) && (_AlignC.PatternShift != cbPatterhShift.SelectedIndex))
            {
                Ncolor = Color.LightGreen;
            }
            else if ((rBLowMagnification.Checked) && (_AlignC.RLMaskAlgorithm != Algoritm(cbRMaskAlgorithm.SelectedIndex)))
            {
                Ncolor = Color.LightGreen;
            }
            else if ((rBLowMagnification.Checked) && (_AlignC.RLMaskAlgorithm != Algoritm(cbRMaskAlgorithm.SelectedIndex)))
            {
                Ncolor = Color.LightGreen;
            }
            else if ((rBLowMagnification.Checked) && (_AlignC.LLMaskAlgorithm != Algoritm(cbLMaskAlgorithm.SelectedIndex)))
            {
                Ncolor = Color.LightGreen;
            }
            else if ((rBLowMagnification.Checked) && (_AlignC.RLWaferAlgorithm != Algoritm(cbRWaferAlgorithm.SelectedIndex)))
            {
                Ncolor = Color.LightGreen;
            }
            else if ((rBLowMagnification.Checked) && (_AlignC.LLWaferAlgorithm != Algoritm(cbLWaferAlgorithm.SelectedIndex)))
            {
                Ncolor = Color.LightGreen;
            }
            else if ((rBHighMagnification.Checked) && (_AlignC.RHMaskAlgorithm != Algoritm(cbRMaskAlgorithm.SelectedIndex)))
            {
                Ncolor = Color.LightGreen;
            }
            else if ((rBHighMagnification.Checked) && (_AlignC.LHMaskAlgorithm != Algoritm(cbLMaskAlgorithm.SelectedIndex)))
            {
                Ncolor = Color.LightGreen;
            }
            else if ((rBHighMagnification.Checked) && (_AlignC.RHWaferAlgorithm != Algoritm(cbRWaferAlgorithm.SelectedIndex)))
            {
                Ncolor = Color.LightGreen;
            }
            else if ((rBHighMagnification.Checked) && (_AlignC.LHWaferAlgorithm != Algoritm(cbLWaferAlgorithm.SelectedIndex)))
            {
                Ncolor = Color.LightGreen;
            }
            else if (_AlignC.UpBackAlign != 1)
            {
                Ncolor = Color.LightGreen;
            }
            else
            {
                Ncolor = Color.MintCream;
            }

            if (Ncolor != Ocolor)
            {
                btMagSaveLow.BackColor = Ncolor;
                btMagSaveHigh.BackColor = Ncolor;
                Update();
            }
        }

        public string SetTimeString
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

        private void RBHighMagnification_Click(object sender, EventArgs e)
        {
            if (rBHighMagnification.Checked)
            {
                GV.TickCount = 0;
                gBMag.Enabled = false;
                cbHighMagnification.Enabled = false;
                cbLowMagnification.Enabled = false;
                rBHighMagnification.Enabled = false;
                rBLowMagnification.Enabled = false;
                if (GV.UserLevel != GV.User.Operator)
                    btMagSaveHigh.Enabled = true;
                btMagSaveLow.Enabled = false;

                GV.NowMagnification = 2;

                NowMageni = cbHighMagnification.SelectedIndex;
                cbLMaskAlgorithm.SelectedIndex = (int)_AlignC.LHMaskAlgorithm;
                cbRMaskAlgorithm.SelectedIndex = (int)_AlignC.RHMaskAlgorithm;
                cbLWaferAlgorithm.SelectedIndex = (int)_AlignC.LHWaferAlgorithm;
                cbRWaferAlgorithm.SelectedIndex = (int)_AlignC.RHWaferAlgorithm;

                Changelight();

                ChangeLensMagnification(NowMageni);

                //if (_recipe.LeftHighMaskMat != null)
                //{
                //    pbLMaskImage.Image = _recipe.LeftHighMaskMat.ToBitmap();
                //}
                //else
                //{
                //    pbLMaskImage.Image = null;
                //}
                if (_recipe.LeftHighMaskMat != null && !_recipe.LeftHighMaskMat.Empty() && _recipe.LeftHighMaskMat.Width > 0 && _recipe.LeftHighMaskMat.Height > 0)
                {
                    try
                    {
                        pbLMaskImage.Image?.Dispose(); // 先釋放舊的 Image
                        pbLMaskImage.Image = _recipe.LeftHighMaskMat.ToBitmap();
                    }
                    catch (Exception ex)
                    {
                        if (GV.AppSettingParm.DebugMode)
                        {
                            lbDebugMsg.Text = "LeftHighMaskMat ToBitmap Error: " + ex.Message;
                        }
                        pbLMaskImage.Image = null;
                    }
                }
                else
                {
                    pbLMaskImage.Image = null;
                }

                //if (_recipe.LeftHighWaferMat != null)
                //{
                //    pbLWaferImage.Image = _recipe.LeftHighWaferMat.ToBitmap();
                //}
                //else
                //{
                //    pbLWaferImage.Image = null;
                //}
                if (_recipe.LeftHighWaferMat != null && !_recipe.LeftHighWaferMat.Empty() && _recipe.LeftHighWaferMask.Width > 0 && _recipe.LeftHighWaferMask.Height > 0)
                {
                    try
                    {
                        pbLWaferImage.Image?.Dispose();
                        pbLWaferImage.Image = _recipe.LeftHighWaferMat.ToBitmap();
                    }
                    catch (Exception ex)
                    {
                        if (GV.AppSettingParm.DebugMode)
                        {
                            lbDebugMsg.Text = "LeftHighWaferMat ToBitmap Error: " + ex.Message;
                        }
                        pbLWaferImage.Image = null;
                    }
                }
                else
                {
                    pbLWaferImage.Image = null;
                }
                if (_recipe.RightHighMaskMat != null && !_recipe.RightHighMaskMat.Empty() && _recipe.RightHighMaskMask.Height > 0 && _recipe.RightHighMaskMask.Width > 0)
                {
                    try
                    {
                        pbRMaskImage.Image?.Dispose();
                        pbRMaskImage.Image = _recipe.RightHighMaskMat.ToBitmap();
                    }
                    catch (Exception ex)
                    {
                        if (GV.AppSettingParm.DebugMode)
                        {
                            lbDebugMsg.Text = "RightHighMaskMat ToBitmap Error: " + ex.Message;
                        }
                        pbRMaskImage.Image = null;
                    }
                }
                else
                {
                    pbRMaskImage.Image = null;
                }
                if (_recipe.RightHighWaferMat != null && !_recipe.RightHighWaferMat.Empty() && _recipe.RightHighWaferMask.Width > 0 && _recipe.RightHighWaferMask.Height > 0)
                {
                    try
                    {
                        pbRWaferImage.Image?.Dispose();
                        pbRWaferImage.Image = _recipe.RightHighWaferMat.ToBitmap();
                    }
                    catch (Exception ex)
                    {
                        if (GV.AppSettingParm.DebugMode)
                        {
                            lbDebugMsg.Text = "RightHighWaferMat ToBitmap Error: " + ex.Message;
                        }
                        pbRWaferImage.Image = null;
                    }
                }
                else
                {
                    pbRWaferImage.Image = null;
                }

                //if (_recipe.RightHighMaskMat != null)
                //{
                //    pbRMaskImage.Image = _recipe.RightHighMaskMat.ToBitmap();
                //}
                //else { pbRMaskImage.Image = null; }

                //if (_recipe.RightHighWaferMat != null)
                //{
                //    pbRWaferImage.Image = _recipe.RightHighWaferMat.ToBitmap();
                //}
                //else { pbRWaferImage.Image = null; }

                Thread.Sleep(iDelayT);
                gBMag.Enabled = true;
                cbLowMagnification.Enabled = false;
                if (GV.UserLevel != GV.User.Operator)
                {
                    cbHighMagnification.Enabled = true;
                    cbPatterhShift.Enabled = true;
                }
                else
                {
                    cbHighMagnification.Enabled = false;
                    cbPatterhShift.Enabled = false;
                }

                Application.DoEvents();

                rBLowMagnification.Enabled = true;
                rBHighMagnification.Enabled = true;
                Update();
                UpdateTargetPositionDisplay();
                CheckParam();
            }
        }

        private void BtChuckUpdate_Click(object sender, EventArgs e)
        {
            if (_recipe == null) return;

            if (MessageBox.Show("確定將現在 WEC 位置儲存為目標位置?", "更新 WEC 位置",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            GV.Plc.GetLocation();

            if (rBLowMagnification.Checked)
            {
                _recipe.UpperLowResWECPos.Z = GV.NowLocation[GV.Plc.iiDChuckZ];
                _recipe.UpperLowResWECPos.X = GV.NowLocation[GV.Plc.iiDChuckX];
                _recipe.UpperLowResWECPos.Y = GV.NowLocation[GV.Plc.iiDChuckY1];
                _recipe.UpperLowResWECPos.A = GV.NowLocation[GV.Plc.iiDChuckY2];
            }
            else if (rBHighMagnification.Checked)
            {
                _recipe.UpperWECPos.Z = GV.NowLocation[GV.Plc.iiDChuckZ];
                _recipe.UpperWECPos.X = GV.NowLocation[GV.Plc.iiDChuckX];
                _recipe.UpperWECPos.Y = GV.NowLocation[GV.Plc.iiDChuckY1];
                _recipe.UpperWECPos.A = GV.NowLocation[GV.Plc.iiDChuckY2];
            }

            GM.WriteRecipeXml(EditRecipe);
            UpdateTargetPositionDisplay();

            MessageBox.Show("WEC 位置已更新", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void BtUpCCDUpdate_Click(object sender, EventArgs e)
        {
            if (_recipe == null) return;

            if (MessageBox.Show("確定將現在 CCD 位置儲存為目標位置?", "更新 CCD 位置",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            GV.Plc.GetLocation();

            if (rBLowMagnification.Checked)
            {
                _recipe.UpperMLowResPosition.Y = GV.NowLocation[GV.Plc.iiDUpBigY];
                _recipe.UpperMLowResPosition.XL = GV.NowLocation[GV.Plc.iiDUpLeftX];
                _recipe.UpperMLowResPosition.YL = GV.NowLocation[GV.Plc.iiDUpLeftY];
                _recipe.UpperMLowResPosition.ZL = GV.NowLocation[GV.Plc.iiDSUpLeftZ];
                _recipe.UpperMLowResPosition.XR = GV.NowLocation[GV.Plc.iiDUpRightX];
                _recipe.UpperMLowResPosition.YR = GV.NowLocation[GV.Plc.iiDUpRightY];
                _recipe.UpperMLowResPosition.ZR = GV.NowLocation[GV.Plc.iiDSUpRightZ];
            }
            else if (rBHighMagnification.Checked)
            {
                _recipe.UpperMPosition.Y = GV.NowLocation[GV.Plc.iiDUpBigY];
                _recipe.UpperMPosition.XL = GV.NowLocation[GV.Plc.iiDUpLeftX];
                _recipe.UpperMPosition.YL = GV.NowLocation[GV.Plc.iiDUpLeftY];
                _recipe.UpperMPosition.ZL = GV.NowLocation[GV.Plc.iiDSUpLeftZ];
                _recipe.UpperMPosition.XR = GV.NowLocation[GV.Plc.iiDUpRightX];
                _recipe.UpperMPosition.YR = GV.NowLocation[GV.Plc.iiDUpRightY];
                _recipe.UpperMPosition.ZR = GV.NowLocation[GV.Plc.iiDSUpRightZ];
            }

            GM.WriteRecipeXml(EditRecipe);
            UpdateTargetPositionDisplay();

            MessageBox.Show("CCD 位置已更新", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtDelete_Click(object sender, EventArgs e)
        {
            string smsg = String.Format(GV.Dlang.strSureDeleteRecipe, EditRecipe.ToString());
            if (MessageBox.Show(smsg, GV.Dlang.strDeleteRecipe, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            // MessageBox1 msg1 = new MessageBox1(smsg, GV.Dlang.strDeleteRecipe, MessageBoxIcon.Question);
            // DialogResult m1 = msg1.ShowDialog();
            // if (m1 != DialogResult.OK)
            //     return;

            AlignCondition newOne = new AlignCondition();
            newOne.CopyTo(EditRecipe);

            _AlignC.LastModifyTime = DateTime.Now;

            GV.LeftLowMaskMat[EditRecipe] = null;
            GV.LeftLowWaferMat[EditRecipe] = null;
            GV.RightLowMaskMat[EditRecipe] = null;
            GV.RightLowWaferMat[EditRecipe] = null;

            GV.LeftHighMaskMat[EditRecipe] = null;
            GV.LeftHighWaferMat[EditRecipe] = null;
            GV.RightHighMaskMat[EditRecipe] = null;
            GV.RightHighWaferMat[EditRecipe] = null;

            DeleteTemplateFile("LLM");
            DeleteTemplateFile("LHM");
            DeleteTemplateFile("RLM");
            DeleteTemplateFile("RHM");
            DeleteTemplateFile("LLW");
            DeleteTemplateFile("LHW");
            DeleteTemplateFile("RLW");
            DeleteTemplateFile("RHW");
            DeleteTemplateFile("LeftMask");
            DeleteTemplateFile("RightMask");

            rBLowMagnification.Checked = false;
            rBHighMagnification.Checked = false;
            cbHighMagnification.SelectedIndex = _AlignC.AlignHighMagnification;
            cbLowMagnification.SelectedIndex = _AlignC.AlignLowMagnification;

            GM.WriteRecipeXml(EditRecipe);

            CheckLevel();
            ShowMe();
        }

        public void DeleteTemplateFile(string strf)
        {
            try
            {
                File.Copy(GetTemplateFileName(strf), GetTemplateFileNameD(strf));
                File.Delete(GetTemplateFileName(strf));
            }
            catch (Exception)
            {
            }
        }

        private void CbLWaferAlgorithm_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rBLowMagnification.Checked)
            {
                _AlignC.LLWaferAlgorithm = Algoritm(cbLWaferAlgorithm.SelectedIndex);
                if (_recipe.LeftLowWaferMat != null && !_recipe.LeftLowWaferMat.Empty())
                {
                    if (GV.AppSettingParm.Emulation != true)
                    {
                        GV.matcherLLW.LearnWithAlgo(_recipe.LeftLowWaferMat, _recipe.LeftLowWaferMask, _AlignC.LLWaferAlgorithm);
                    }
                }
            }

            else if (rBHighMagnification.Checked)
            {
                _AlignC.LHWaferAlgorithm = Algoritm(cbLWaferAlgorithm.SelectedIndex);
                if (_recipe.LeftHighWaferMat != null && !_recipe.LeftHighWaferMat.Empty())
                {
                    GV.matcherLHW.LearnWithAlgo(_recipe.LeftHighWaferMat, _recipe.LeftHighWaferMask, _AlignC.LHWaferAlgorithm);
                }

            }

            if (cbLWaferAlgorithm.SelectedIndex == 2)
            {

                cbLWaferClassList.Visible = true;
                //btLabelPatternL.Visible = true;
                pbLWaferImage.Visible = false;
                btFindLWaferCenter.Visible = false;
                btCreateLWaferPatternMask.Visible = false;

            }
            else
            {
                cbLWaferClassList.Visible = false;
                //btLabelPatternL.Visible = false;
                pbLWaferImage.Visible = true;
                btFindLWaferCenter.Visible = true;
                btCreateLWaferPatternMask.Visible = true;
            }

            CheckParam();
        }

        private void CbRMaskAlgorithm_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rBLowMagnification.Checked)
            {
                _AlignC.RLMaskAlgorithm = Algoritm(cbRMaskAlgorithm.SelectedIndex);
                if (_recipe.RightLowMaskMat != null && !_recipe.RightLowMaskMat.Empty())
                {
                    if (GV.AppSettingParm.Emulation != true)
                    {
                        GV.matcherRLM.LearnWithAlgo(_recipe.RightLowMaskMat, _recipe.RightLowMaskMask, _AlignC.RLMaskAlgorithm);
                    }
                }
            }
            else if (rBHighMagnification.Checked)
            {
                _AlignC.RHMaskAlgorithm = Algoritm(cbRMaskAlgorithm.SelectedIndex);
                if (_recipe.RightHighMaskMask != null && !_recipe.RightHighMaskMat.Empty())
                {
                    if (GV.AppSettingParm.Emulation != true)
                    {
                        GV.matcherRHM.LearnWithAlgo(_recipe.RightHighMaskMat, _recipe.RightHighMaskMask, _AlignC.RHMaskAlgorithm);
                    }

                }
            }
            if (cbRMaskAlgorithm.SelectedIndex == 2)
            {

                cbRMaskClassList.Visible = true;
                btLabelPatternRM.Visible = false;
                pbRMaskImage.Visible = false;
                btFindRMaskCenter.Visible = false;
                btCreateRMaskPatternMask.Visible = false;
            }
            else
            {
                cbRMaskClassList.Visible = false;
                btLabelPatternRM.Visible = false;
                pbRMaskImage.Visible = true;
                btFindRMaskCenter.Visible = true;
                btCreateRMaskPatternMask.Visible = true;
            }
            CheckParam();
        }

        private void BtDefault_Click(object sender, EventArgs e)
        {
            int[] Brightness = { 10, 10, 10, 10, 10, 10, 11, 31, 26, 77, 110, 130 };

            string smsg = string.Format(GV.Dlang.strSureDefaultRecipe, EditRecipe.ToString());
            if (MessageBox.Show(smsg, GV.Dlang.strSetRecipe, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            // MessageBox1 msg1 = new MessageBox1(smsg, GV.Dlang.strSetRecipe, MessageBoxIcon.Question);
            // DialogResult m1 = msg1.ShowDialog();
            // if (m1 != DialogResult.OK)
            //     return;

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

            GM.WriteRecipeXml(EditRecipe);

            Changelight();

            CheckParam();

            Update();
        }

        private void RBLowMagnification_Click(object sender, EventArgs e)
        {
            if (rBLowMagnification.Checked)
            {
                try
                {
                    GV.TickCount = 0;
                    gBMag.Enabled = false;
                    rBHighMagnification.Enabled = false;
                    rBLowMagnification.Enabled = false;
                    cbLowMagnification.Enabled = false;
                    cbHighMagnification.Enabled = false;
                    cbPatterhShift.Enabled = false;
                    if (GV.UserLevel != GV.User.Operator)
                        btMagSaveLow.Enabled = true;
                    btMagSaveHigh.Enabled = false;
                    GV.NowMagnification = 1;
                    NowMageni = cbLowMagnification.SelectedIndex;

                    cbLMaskAlgorithm.SelectedIndex = (int)_AlignC.LLMaskAlgorithm;
                    cbRMaskAlgorithm.SelectedIndex = (int)_AlignC.RLMaskAlgorithm;
                    cbLWaferAlgorithm.SelectedIndex = (int)_AlignC.LLWaferAlgorithm;
                    cbRWaferAlgorithm.SelectedIndex = (int)_AlignC.RLWaferAlgorithm;

                    Changelight();

                    ChangeLensMagnification(NowMageni);

                    if (_recipe.LeftLowMaskMat != null)
                    {
                        pbLMaskImage.Image = _recipe.LeftLowMaskMat.ToBitmap();
                    }
                    else
                    {
                        pbLMaskImage.Image = null;
                    }
                    if (_recipe.LeftLowWaferMat != null)
                    {
                        pbLWaferImage.Image = _recipe.LeftLowWaferMat.ToBitmap();
                    }
                    else
                    {
                        pbLWaferImage.Image = null;
                    }
                    if (_recipe.RightLowMaskMat != null)
                    {
                        pbRMaskImage.Image = _recipe.RightLowMaskMat.ToBitmap();
                    }
                    else
                    {
                        pbRMaskImage.Image = null;
                    }
                    if (_recipe.RightLowWaferMat != null)
                    {
                        pbRWaferImage.Image = _recipe.RightLowWaferMat.ToBitmap();
                    }
                    else
                    {
                        pbRWaferImage.Image = null;
                    }

                    Thread.Sleep(iDelayT);
                    gBMag.Enabled = true;
                    if (GV.UserLevel != GV.User.Operator)
                        cbLowMagnification.Enabled = true;
                    else
                        cbLowMagnification.Enabled = false;

                    cbHighMagnification.Enabled = false;

                    Application.DoEvents();

                    rBLowMagnification.Enabled = true;
                    rBHighMagnification.Enabled = true;
                    CheckParam();
                }
                catch (Exception ex)
                {
                    if (GV.AppSettingParm.DebugMode)
                    {
                        lbDebugMsg.Text = ex.Message;
                    }
                }
                UpdateTargetPositionDisplay();
            }
        }
        private async void BtAlignTest_ClickAsync(object sender, EventArgs e)
        {
            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, true);
            NMatchPosition pmps = null;
            string msg;
            GV.TestAlign = true;
            Thread.Sleep(250);
            lbDebugMsg.Visible = true;
            DebugMsgCount = 1;

            if (rBHighMagnification.Checked)
            {
                pmps = await GetMatchPositionAsync(GV.LeftUpCam.Grab(), GV.RightUpCam.Grab(), "high");
            }
            else if (rBLowMagnification.Checked)
            {
                pmps = await GetMatchPositionAsync(GV.LeftUpCam.Grab(), GV.RightUpCam.Grab(), "low");
            }

            PointF center = GV.UpCenter;
            double leftX = ((double)(center.X) - pmps.LMaskMp.X) * GV.ZoomLensInfo.LeftCameraMotorStepsPerPixelX[NowMageni];
            double leftY = ((double)(center.Y) - pmps.LMaskMp.Y) * GV.ZoomLensInfo.LeftCameraMotorStepsPerPixelY[NowMageni];
            double rightX = ((double)(center.X) - pmps.RMaskMp.X) * GV.ZoomLensInfo.RightCameraMotorStepsPerPixelX[NowMageni];
            double rightY = ((double)(center.Y) - pmps.RMaskMp.Y) * GV.ZoomLensInfo.RightCameraMotorStepsPerPixelY[NowMageni];

            bool bRet = GV.Plc.AlignCameraMove(0, -leftX, rightX, -leftY, -rightY);
            if (bRet == false)
            {
                msg = string.Format("CameraMove Error ={0}", GV.Plc.PlcErrorMessage);
                lbDebugMsg.Text = msg;
                return;
            }
            //Thread.Sleep(1000);
            if (rBHighMagnification.Checked)
            {
                pmps = await GetMatchPositionAsync(GV.LeftUpCam.Grab(), GV.RightUpCam.Grab(), "high");
            }
            else if (rBLowMagnification.Checked)
            {
                pmps = await GetMatchPositionAsync(GV.LeftUpCam.Grab(), GV.RightUpCam.Grab(), "low");
            }
            double LShiftXum = (pmps.LMaskMp.X - pmps.LWaferMp.X) * GV.ZoomLensInfo.LeftUmPerPixelX[NowMageni];
            double LShiftYum = (pmps.LMaskMp.Y - pmps.LWaferMp.Y) * GV.ZoomLensInfo.LeftUmPerPixelY[NowMageni];
            double RShiftXum = (pmps.RMaskMp.X - pmps.RWaferMp.X) * GV.ZoomLensInfo.RightUmPerPixelX[NowMageni];
            double RShiftYum = (pmps.RMaskMp.Y - pmps.RWaferMp.Y) * GV.ZoomLensInfo.RightUmPerPixelY[NowMageni];
            double rollDistance = LShiftYum - RShiftYum;
            GV.Plc.GetLocation();
            GV._recipe.AlignC.PatternCenterDistanceUm = GV.ZoomLensInfo.LRUpCenterDistance - (int)((double)GV.NowLocation[GV.Plc.iiDUpLeftX] *
                GV.ZoomLensInfo.LeftMotorStepsPerumX) - (int)((double)GV.NowLocation[GV.Plc.iiDUpRightX] * GV.ZoomLensInfo.RightMotorStepsPerumX);
            int[] motorSteps = XyyMotorSteps(pmps.LMaskMp, pmps.LWaferMp, pmps.RMaskMp, pmps.RWaferMp);
            msg = string.Format("Roll dis {3:N3} TableMove {0}, {1}, {2}", motorSteps[0], motorSteps[1], motorSteps[2], rollDistance);
            lbDebugMsg.Text = msg;
            bRet = GV.Plc.AlignXyyTableMove(motorSteps[0], motorSteps[1], motorSteps[2]);
            //Thread.Sleep(1000);
            if (bRet == false)
            {
                msg = string.Format("XyyMove Error = Plc Error!!");
            }
            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, false);
            GV.TestAlign = false;
        }

        private void BtAlignTest_Click(object sender, EventArgs e)
        {
            NMatchPosition pmps = null;
            string msg;

            lbDebugMsg.Visible = true;

            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, true);

            if (rBHighMagnification.Checked)
            {
                pmps = GetMatchPostion(GV.LeftUpCam.Grab(), GV.RightUpCam.Grab(), "high");
            }
            else if (rBLowMagnification.Checked)
            {
                pmps = GetMatchPostion(GV.LeftUpCam.Grab(), GV.RightUpCam.Grab(), "low");
            }


            PointF center = GV.UpCenter;

            double leftX = ((double)(center.X) - pmps.LMaskMp.X) * GV.ZoomLensInfo.LeftCameraMotorStepsPerPixelX[NowMageni];
            double leftY = ((double)(center.Y) - pmps.LMaskMp.Y) * GV.ZoomLensInfo.LeftCameraMotorStepsPerPixelY[NowMageni];
            double rightX = ((double)(center.X) - pmps.RMaskMp.X) * GV.ZoomLensInfo.RightCameraMotorStepsPerPixelX[NowMageni];
            double rightY = ((double)(center.Y) - pmps.RMaskMp.Y) * GV.ZoomLensInfo.RightCameraMotorStepsPerPixelY[NowMageni];

            bool bRet = GV.Plc.AlignCameraMove(0, -leftX, rightX, -leftY, -rightY);

            if (bRet == false)
            {
                // msg = string.Format("CameraMove Error = {0}", GV.Plc.PlcErrorMessage);
                msg = string.Format("CameraMove Error = Plc Error");
                lbDebugMsg.Text = msg;
                return;
            }

            Thread.Sleep(250);

            if (rBHighMagnification.Checked)
            {
                pmps = GetMatchPostion(GV.LeftUpCam.Grab(), GV.RightUpCam.Grab(), "high");
            }
            else if (rBLowMagnification.Checked)
            {
                pmps = GetMatchPostion(GV.LeftUpCam.Grab(), GV.RightUpCam.Grab(), "low");
            }

            //double LShiftXum = (pmps.LMaskMp.X - pmps.LWaferMp.X) * GV.ZoomLensInfo.LeftUmPerPixelX[NowMageni];
            double LShiftYum = (pmps.LMaskMp.Y - pmps.LWaferMp.Y) * GV.ZoomLensInfo.LeftUmPerPixelY[NowMageni];
            //double RShiftXum = (pmps.RMaskMp.X - pmps.RWaferMp.X) * GV.ZoomLensInfo.RightUmPerPixelX[NowMageni];
            double RShiftYum = (pmps.RMaskMp.Y - pmps.RWaferMp.Y) * GV.ZoomLensInfo.RightUmPerPixelY[NowMageni];

            double Rolldis = LShiftYum - RShiftYum;

            GV.Plc.GetLocation();

            _AlignC.PatternCenterDistanceUm = GV.ZoomLensInfo.LRUpCenterDistance
                - GV.NowLocation[GV.Plc.iiDUpLeftX] / 10 - GV.NowLocation[GV.Plc.iiDUpRightX] / 10;

            int[] motorSteps = XyyMotorSteps(pmps.LMaskMp, pmps.LWaferMp, pmps.RMaskMp, pmps.RWaferMp);

            msg = string.Format("Roll dis {3:N3} TableMove {0}, {1}, {2}", motorSteps[0], motorSteps[1], motorSteps[2], Rolldis);

            lbDebugMsg.Text = msg;

            bRet = GV.Plc.AlignXyyTableMove(motorSteps[0], motorSteps[1], motorSteps[2]);
            if (bRet == false)
            {
                msg = string.Format("XyyMove Error = Plc Error !!!");
                lbDebugMsg.Text = msg;
            }

            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, false);
        }

        private async Task<NMatchPosition> GetMatchPositionAsync(Mat lSrc, Mat rSrc, string alignMagnification, CancellationToken ct = default)
        {
            int i = 0;
            while (i < 3)
            {
                lSrc.CopyTo(GMPLeft);
                rSrc.CopyTo(GMPRight);

                if (alignMagnification == "low")
                {
                    //  使用完整影像搜尋
                    var t1 = GV.matcherLLM.MatMatchWithAlgoAsync(0, GMPLeft, GMPlMaskMp, _AlignC.LLMaskAlgorithm, ct);
                    var t2 = GV.matcherLLW.MatMatchWithAlgoAsync(0, GMPLeft, GMPlWaferMp, _AlignC.LLWaferAlgorithm, ct);
                    var t3 = GV.matcherRLM.MatMatchWithAlgoAsync(0, GMPRight, GMPrMaskMp, _AlignC.RLMaskAlgorithm, ct);
                    var t4 = GV.matcherRLW.MatMatchWithAlgoAsync(0, GMPRight, GMPrWaferMp, _AlignC.RLWaferAlgorithm, ct);

                    await Task.WhenAll(t1, t2, t3, t4).ConfigureAwait(false);

                    //  套用偏移量校正座標
                    GMPlMaskMp.X += _AlignC.LLMaskOffsetX;
                    GMPlMaskMp.Y += _AlignC.LLMaskOffsetY;

                    GMPlWaferMp.X += _AlignC.LLWaferOffsetX;
                    GMPlWaferMp.Y += _AlignC.LLWaferOffsetY;

                    GMPrMaskMp.X += _AlignC.RLMaskOffsetX;
                    GMPrMaskMp.Y += _AlignC.RLMaskOffsetY;

                    GMPrWaferMp.X += _AlignC.RLWaferOffsetX;
                    GMPrWaferMp.Y += _AlignC.RLWaferOffsetY;
                }
                else if (alignMagnification == "high")
                {
                    //  使用完整影像搜尋
                    var t1 = GV.matcherLHM.MatMatchWithAlgoAsync(0, GMPLeft, GMPlMaskMp, _AlignC.LHMaskAlgorithm, ct);
                    var t2 = GV.matcherLHW.MatMatchWithAlgoAsync(0, GMPLeft, GMPlWaferMp, _AlignC.LHWaferAlgorithm, ct);
                    var t3 = GV.matcherRHM.MatMatchWithAlgoAsync(0, GMPRight, GMPrMaskMp, _AlignC.RHMaskAlgorithm, ct);
                    var t4 = GV.matcherRHW.MatMatchWithAlgoAsync(0, GMPRight, GMPrWaferMp, _AlignC.RHWaferAlgorithm, ct);

                    await Task.WhenAll(t1, t2, t3, t4).ConfigureAwait(false);

                    //  套用偏移量校正座標
                    GMPlMaskMp.X += _AlignC.LHMaskOffsetX;
                    GMPlMaskMp.Y += _AlignC.LHMaskOffsetY;

                    GMPlWaferMp.X += _AlignC.LHWaferOffsetX;
                    GMPlWaferMp.Y += _AlignC.LHWaferOffsetY;

                    GMPrMaskMp.X += _AlignC.RHMaskOffsetX;
                    GMPrMaskMp.Y += _AlignC.RHMaskOffsetY;

                    GMPrWaferMp.X += _AlignC.RHWaferOffsetX;
                    GMPrWaferMp.Y += _AlignC.RHWaferOffsetY;
                }

                //  檢查 Score 是否符合標準
                if (GMPlWaferMp.Score < GV.NowAlignCondition.WaferScore || GMPrWaferMp.Score < GV.NowAlignCondition.WaferScore)
                {
                    i++;
                }
                else if (GMPlMaskMp.Score < GV.NowAlignCondition.MaskSocre || GMPrMaskMp.Score < GV.NowAlignCondition.MaskSocre)
                {
                    i++;
                }
                else
                {
                    i = 4;  // 成功,跳出迴圈
                }
            }
            return new NMatchPosition(GMPlMaskMp, GMPlWaferMp, GMPrMaskMp, GMPrWaferMp, i);
        }

        private NMatchPosition GetMatchPostion(Mat lSrc, Mat rSrc, string alignMmagnification)
        {
            int i = 0;

            while (i < 3)
            {
                if (alignMmagnification == "low")
                {
                    //  使用完整影像搜尋
                    if (_AlignC.LLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
                    {
                        GV.matcherLLM.MatMatchWithAlgo(0, lSrc.Clone(), ref LMaskMp, _AlignC.LLMaskAlgorithm, _AlignC.LLMaskAIClassId);
                    }
                    else
                    {
                        GV.matcherLLM.MatMatchWithAlgo(0, lSrc.Clone(), ref LMaskMp, _AlignC.LLMaskAlgorithm);
                    }

                    if (_AlignC.RLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
                    {
                        GV.matcherRLM.MatMatchWithAlgo(0, rSrc.Clone(), ref RMaskMp, _AlignC.RLMaskAlgorithm, _AlignC.RLMaskAIClassId);
                    }
                    else
                    {
                        GV.matcherRLM.MatMatchWithAlgo(0, rSrc.Clone(), ref RMaskMp, _AlignC.RLMaskAlgorithm);
                    }

                    if (_AlignC.LLWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
                    {
                        GV.matcherLLW.MatMatchWithAlgo(0, lSrc.Clone(), ref LWaferMp, _AlignC.LLWaferAlgorithm, _AlignC.LLWaferAIClassId);
                    }
                    else
                    {
                        GV.matcherLLW.MatMatchWithAlgo(0, lSrc.Clone(), ref LWaferMp, _AlignC.LLWaferAlgorithm);
                    }

                    if (_AlignC.RLWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
                    {
                        GV.matcherRLW.MatMatchWithAlgo(0, rSrc.Clone(), ref RWaferMp, _AlignC.RLWaferAlgorithm, _AlignC.RLWaferAIClassId);
                    }
                    else
                    {
                        GV.matcherRLW.MatMatchWithAlgo(0, rSrc.Clone(), ref RWaferMp, _AlignC.RLWaferAlgorithm);
                    }

                    //  套用偏移量校正座標
                    LMaskMp.X += _AlignC.LLMaskOffsetX;
                    LMaskMp.Y += _AlignC.LLMaskOffsetY;

                    LWaferMp.X += _AlignC.LLWaferOffsetX;
                    LWaferMp.Y += _AlignC.LLWaferOffsetY;

                    RMaskMp.X += _AlignC.RLMaskOffsetX;
                    RMaskMp.Y += _AlignC.RLMaskOffsetY;

                    RWaferMp.X += _AlignC.RLWaferOffsetX;
                    RWaferMp.Y += _AlignC.RLWaferOffsetY;
                    LWaferMp.X -= (float)(_AlignC.XOffset / GV.ZoomLensInfo.LeftUmPerPixelX[_AlignC.AlignLowMagnification]);
                    RWaferMp.X -= (float)(_AlignC.XOffset / GV.ZoomLensInfo.RightUmPerPixelX[_AlignC.AlignLowMagnification]);
                    LWaferMp.Y -= (float)(_AlignC.YLOffset / GV.ZoomLensInfo.LeftUmPerPixelY[_AlignC.AlignLowMagnification]);
                    RWaferMp.Y -= (float)(_AlignC.YROffset / GV.ZoomLensInfo.RightUmPerPixelY[_AlignC.AlignLowMagnification]);

                }
                else if (alignMmagnification == "high")
                {
                    //  使用完整影像搜尋
                    if (_AlignC.LHMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
                    {
                        GV.matcherLHM.MatMatchWithAlgo(0, lSrc.Clone(), ref LMaskMp, _AlignC.LHMaskAlgorithm, _AlignC.LHMaskAIClassId);
                    }
                    else
                    {
                        GV.matcherLHM.MatMatchWithAlgo(0, lSrc.Clone(), ref LMaskMp, _AlignC.LHMaskAlgorithm);
                    }

                    if (_AlignC.RHMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
                    {
                        GV.matcherRHM.MatMatchWithAlgo(0, rSrc.Clone(), ref RMaskMp, _AlignC.RHMaskAlgorithm, _AlignC.RHMaskAIClassId);
                    }
                    else
                    {
                        GV.matcherRHM.MatMatchWithAlgo(0, rSrc.Clone(), ref RMaskMp, _AlignC.RHMaskAlgorithm);
                    }

                    if (_AlignC.LHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
                    {
                        GV.matcherLHW.MatMatchWithAlgo(0, lSrc.Clone(), ref LWaferMp, _AlignC.LHWaferAlgorithm, _AlignC.LHWaferAIClassId);
                    }
                    else
                    {
                        GV.matcherLHW.MatMatchWithAlgo(0, lSrc.Clone(), ref LWaferMp, _AlignC.LHWaferAlgorithm);
                    }

                    if (_AlignC.RHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
                    {
                        GV.matcherRHW.MatMatchWithAlgo(0, rSrc.Clone(), ref RWaferMp, _AlignC.RHWaferAlgorithm, _AlignC.RHWaferAIClassId);
                    }
                    else
                    {
                        GV.matcherRHW.MatMatchWithAlgo(0, rSrc.Clone(), ref RWaferMp, _AlignC.RHWaferAlgorithm);
                    }

                    //  套用偏移量校正座標
                    LMaskMp.X += _AlignC.LHMaskOffsetX;
                    LMaskMp.Y += _AlignC.LHMaskOffsetY;

                    LWaferMp.X += _AlignC.LHWaferOffsetX;
                    LWaferMp.Y += _AlignC.LHWaferOffsetY;

                    RMaskMp.X += _AlignC.RHMaskOffsetX;
                    RMaskMp.Y += _AlignC.RHMaskOffsetY;

                    RWaferMp.X += _AlignC.RHWaferOffsetX;
                    RWaferMp.Y += _AlignC.RHWaferOffsetY;
                    LWaferMp.X -= (float)(_AlignC.XOffset / GV.ZoomLensInfo.LeftUmPerPixelX[_AlignC.AlignHighMagnification]);
                    RWaferMp.X -= (float)(_AlignC.XOffset / GV.ZoomLensInfo.RightUmPerPixelX[_AlignC.AlignHighMagnification]);
                    LWaferMp.Y -= (float)(_AlignC.YLOffset / GV.ZoomLensInfo.LeftUmPerPixelY[_AlignC.AlignHighMagnification]);
                    RWaferMp.Y -= (float)(_AlignC.YROffset / GV.ZoomLensInfo.RightUmPerPixelY[_AlignC.AlignHighMagnification]);

                }

                //  檢查 Score 是否符合標準
                if ((LMaskMp.Score < GV.NowAlignCondition.MaskSocre) || (RMaskMp.Score < GV.NowAlignCondition.MaskSocre))
                {
                    i++;
                }
                else if ((LWaferMp.Score < GV.NowAlignCondition.WaferScore) || (RWaferMp.Score < GV.NowAlignCondition.WaferScore))
                {
                    i++;
                }
                else
                {
                    i = 4;  // 成功,跳出迴圈
                }
                skLPattern.MaskMp = LMaskMp;
                skLPattern.WaferMp = LWaferMp;
                skRPattern.MaskMp = RMaskMp;
                skRPattern.WaferMp = RWaferMp;
            }

            return new NMatchPosition(LMaskMp, LWaferMp, RMaskMp, RWaferMp, i);
        }
        private Mat GetROIImageForMatch(Mat fullImage, OpenCvSharp.Rect roi)
        {
            if (roi.Width <= 0 || roi.Height <= 0)
            {
                return fullImage.Clone();
            }

            // 確保 ROI 在範圍內
            int x = Math.Max(0, Math.Min(roi.X, fullImage.Width - 1));
            int y = Math.Max(0, Math.Min(roi.Y, fullImage.Height - 1));
            int width = Math.Min(roi.Width, fullImage.Width - x);
            int height = Math.Min(roi.Height, fullImage.Height - y);

            return new Mat(fullImage, new OpenCvSharp.Rect(x, y, width, height)).Clone();
        }

        private void BtCapture_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(GM.SaveCaptureImage(GV.LeftUpCam.Grab(), GV.RightUpCam.Grab()) + " " + GV.Dlang.strSave, GV.Dlang.strSave + "Pattern too ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            int iHighLow = 1;
            if (rBLowMagnification.Checked)
            {
                iHighLow = 0;
            }
            GM.SaveRecipeTemplate(_recipe, iHighLow);
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
                Cv2.ImWrite(leftImagePath, GV.LeftUpCam.Grab());
                Cv2.ImWrite(rightImagePath, GV.RightUpCam.Grab());
            }
            catch
            {
            }
            //MessageBox.Show(GM.SaveCaptureImage(GV.LeftUpCam.Grab(), GV.RightUpCam.Grab()) + " " + GV.Dlang.strSave);
        }

        private int[] XyyMotorSteps(MatchPosition lMaskMp, MatchPosition lWaferMp, MatchPosition rMaskMp, MatchPosition rWaferMp)
        {
            int[] motorSteps = new int[3];

            double patternCenterDistancePixel = _AlignC.PatternCenterDistanceUm / 2;

            double ly = ((double)(lWaferMp.Y - lMaskMp.Y)) * GV.ZoomLensInfo.LeftUmPerPixelY[NowMageni];
            double ry = ((double)(rWaferMp.Y - rMaskMp.Y)) * GV.ZoomLensInfo.RightUmPerPixelY[NowMageni];
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

            double xMotorStepsl = (lMaskMp.X - lWaferMp.X) * GV.ZoomLensInfo.LeftMotorStepsPerPixelX[NowMageni];
            double xMotorStepsr = (rMaskMp.X - rWaferMp.X) * GV.ZoomLensInfo.RightMotorStepsPerPixelX[NowMageni];
            double xMotorSteps = (xMotorStepsl + xMotorStepsr) / 2;
            double yMotorStepsl = (lMaskMp.Y - lWaferMp.Y) * GV.ZoomLensInfo.LeftMotorStepsPerPixelY[NowMageni];
            double yMotorStepsr = (rMaskMp.Y - rWaferMp.Y) * GV.ZoomLensInfo.RightMotorStepsPerPixelY[NowMageni];
            double yMotorSteps = (yMotorStepsl + yMotorStepsr) / 2;

            motorSteps[0] = (int)Math.Round(xMotorSteps);
            motorSteps[1] = (int)Math.Round(yMotorSteps);
            motorSteps[2] = (int)Math.Round(rotateMotorSteps);
            return motorSteps;
        }

        private int[] XyyRotateSteps(MatchPosition lMaskMp, MatchPosition lWaferMp, MatchPosition rMaskMp, MatchPosition rWaferMp)
        {
            int[] motorSteps = new int[3];
            //double patternCenterDistancePixel = _AlignC.PatternCenterDistanceUm;

            RotationTranslation rot = GetRotation(lMaskMp, lWaferMp, rMaskMp, rWaferMp);

            double dx = rot.Translation.X;
            double dy = rot.Translation.Y;

            double dY = (GV.NowLocation[GV.Plc.iiDChuckY1] - GV.NowLocation[GV.Plc.iiDChuckY2]) * 0.1;
            double Theta0 = Math.Atan2(dY, GV.AppSettingParm.XYYTableSize);

            double dx1 = GV.R * Math.Sin(rot.Rotation + GV.ThetaX + Theta0) - GV.R * Math.Sin(GV.ThetaX + Theta0);
            double dy1 = GV.R * Math.Cos(rot.Rotation + GV.ThetaY1 + Theta0) - GV.R * Math.Cos(GV.ThetaY1 + Theta0);
            double dy2 = GV.R * Math.Cos(rot.Rotation + GV.ThetaY2 + Theta0) - GV.R * Math.Cos(GV.ThetaY2 + Theta0);

            double mx = dx + dx1;
            double my1 = dy + dy1;
            double my2 = dy + dy2;

            motorSteps[0] = (int)(mx / GV.AppSettingParm.XYYStepX);
            motorSteps[1] = (int)(my1 / GV.AppSettingParm.XYYStepY1);
            motorSteps[2] = (int)(my2 / GV.AppSettingParm.XYYStepY2);

            return motorSteps;
        }

        public RotationTranslation GetRotation(MatchPosition lMaskMp, MatchPosition lWaferMp, MatchPosition rMaskMp, MatchPosition rWaferMp)
        {
            double patternCenterDistancePixel = _AlignC.PatternCenterDistanceUm;

            RotationTranslation rotat = new RotationTranslation();
            double lwx = lWaferMp.X * GV.ZoomLensInfo.LeftUmPerPixelX[NowMageni];
            double lwy = lWaferMp.Y * GV.ZoomLensInfo.LeftUmPerPixelY[NowMageni];
            double rwx = rWaferMp.X * GV.ZoomLensInfo.RightUmPerPixelX[NowMageni];
            double rwy = rWaferMp.Y * GV.ZoomLensInfo.RightUmPerPixelY[NowMageni];
            double lmx = lMaskMp.X * GV.ZoomLensInfo.LeftUmPerPixelX[NowMageni];
            double lmy = lMaskMp.Y * GV.ZoomLensInfo.LeftUmPerPixelY[NowMageni];
            double rmx = rMaskMp.X * GV.ZoomLensInfo.RightUmPerPixelX[NowMageni];
            double rmy = rMaskMp.Y * GV.ZoomLensInfo.RightUmPerPixelY[NowMageni];

            PointD pLw = new PointD(lwx, lwy) + new PointD(-patternCenterDistancePixel, 0);
            PointD pRw = new PointD(rwx, rwy) + new PointD(patternCenterDistancePixel, 0);
            PointD pLm = new PointD(lmx, lmy) + new PointD(-patternCenterDistancePixel, 0);
            PointD pRm = new PointD(rmx, rmy) + new PointD(patternCenterDistancePixel, 0);
            LineD lineW = new LineD(pLw, pRw);
            LineD lineM = new LineD(pLm, pRm);
            rotat.Translation = lineM.Center - lineW.Center;

            rotat.Rotation = lineM.Theta - lineW.Theta;

            return rotat;
        }

        private void BtChuckAlign_Click(object sender, EventArgs e)
        {
            bool bRet;
            NMatchPosition pmps = null;
            string msg;

            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, true);

            if (rBHighMagnification.Checked)
            {
                pmps = GetMatchPostion(GV.LeftUpCam.Grab(), GV.RightUpCam.Grab(), "high");
            }
            else if (rBLowMagnification.Checked)
            {
                pmps = GetMatchPostion(GV.LeftUpCam.Grab(), GV.RightUpCam.Grab(), "low");
            }

            //double LShiftXum = (pmps.LMaskMp.X - pmps.LWaferMp.X) * GV.ZoomLensInfo.LeftUmPerPixelX[NowMageni];
            double LShiftYum = (pmps.LMaskMp.Y - pmps.LWaferMp.Y) * GV.ZoomLensInfo.LeftUmPerPixelY[NowMageni];
            //double RShiftXum = (pmps.RMaskMp.X - pmps.RWaferMp.X) * GV.ZoomLensInfo.RightUmPerPixelX[NowMageni];
            double RShiftYum = (pmps.RMaskMp.Y - pmps.RWaferMp.Y) * GV.ZoomLensInfo.RightUmPerPixelY[NowMageni];

            double Rolldis = LShiftYum - RShiftYum;

            int[] motorSteps = XyyRotateSteps(pmps.LMaskMp, pmps.LWaferMp, pmps.RMaskMp, pmps.RWaferMp);

            msg = string.Format("Roll dis {3:N3} TableMove {0}, {1}, {2}", motorSteps[0], motorSteps[1], motorSteps[2], Rolldis);

            lbDebugMsg.Text = msg;

            bRet = GV.Plc.XyyTableMove(motorSteps[0], motorSteps[1], motorSteps[2]);
            if (bRet == false)
            {
                msg = string.Format("XyyMove Error = Plc Error !!!");
                lbDebugMsg.Text = msg;
            }

            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, false);
        }

        private void BtChuckAlignN_Click(object sender, EventArgs e)
        {
            bool bRet;
            NMatchPosition pmps = null;
            string msg;

            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, true);

            if (rBHighMagnification.Checked)
            {
                pmps = GetMatchPostion(GV.LeftUpCam.Grab(), GV.RightUpCam.Grab(), "high");
            }
            else if (rBLowMagnification.Checked)
            {
                pmps = GetMatchPostion(GV.LeftUpCam.Grab(), GV.RightUpCam.Grab(), "low");
            }

            //double LShiftXum = (pmps.LMaskMp.X - pmps.LWaferMp.X) * GV.ZoomLensInfo.LeftUmPerPixelX[NowMageni];
            double LShiftYum = (pmps.LMaskMp.Y - pmps.LWaferMp.Y) * GV.ZoomLensInfo.LeftUmPerPixelY[NowMageni];
            //double RShiftXum = (pmps.RMaskMp.X - pmps.RWaferMp.X) * GV.ZoomLensInfo.RightUmPerPixelX[NowMageni];
            double RShiftYum = (pmps.RMaskMp.Y - pmps.RWaferMp.Y) * GV.ZoomLensInfo.RightUmPerPixelY[NowMageni];

            double Rolldis = LShiftYum - RShiftYum;

            int[] motorSteps = XyyMotorSteps(pmps.LMaskMp, pmps.LWaferMp, pmps.RMaskMp, pmps.RWaferMp);

            msg = string.Format("Roll dis {3:N3} TableMove {0}, {1}, {2}", motorSteps[0], motorSteps[1], motorSteps[2], Rolldis);

            lbDebugMsg.Text = msg;

            int[] Roll = new int[3];
            Roll[0] = motorSteps[0];
            Roll[1] = motorSteps[1];
            Roll[2] = motorSteps[2];

            RollbackStack.Push(Roll);

            bRet = GV.Plc.AlignXyyTableMove(motorSteps[0], motorSteps[1], motorSteps[2]);

            if (bRet == false)
            {
                msg = string.Format("XyyMove Error = Plc Error !!!");
                lbDebugMsg.Text = msg;
            }

            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, false);
        }

        private void BtRollBack_Click(object sender, EventArgs e)
        {
            if (RollbackStack.Count > 0)
            {
                GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, true);

                Thread.Sleep(100);

                int[] roll = (int[])RollbackStack.Pop();

                bool bRet = GV.Plc.AlignXyyTableMove(-roll[0], -roll[1], -roll[2]);
                if (bRet == false)
                {
                    string msg = string.Format("Xyy Roll back Error = Plc Error !!!");
                    lbDebugMsg.Text = msg;
                }

                GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, false);
            }
        }

        private void BtUpdate_Click(object sender, EventArgs e)
        {
            nudReadChuckX.Value = GV.NowLocation[GV.Plc.iiDChuckX];
            nudReadChuckY1.Value = GV.NowLocation[GV.Plc.iiDChuckY1];
            nudReadChuckY2.Value = GV.NowLocation[GV.Plc.iiDChuckY2];
            nudReadChuckR.Value = (Decimal)GV.ZoomLensInfo.MotorStepsPerPixelDegree;
        }

        private void BtGo_Click(object sender, EventArgs e)
        {
            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, true);

            Thread.Sleep(100);

            bool bRet = GV.Plc.XyyTableMoveAbs((int)nudReadChuckX.Value, (int)nudReadChuckY1.Value, (int)nudReadChuckY2.Value);
            if (bRet == false)
            {
                string msg = string.Format("Xyy Roll back Error = Plc Error !!!");
                lbDebugMsg.Text = msg;
            }

            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, false);
        }

        private void NuDRotate_ValueChanged(object sender, EventArgs e)
        {
            GV.ZoomLensInfo.MotorStepsPerPixelDegree = (double)nudReadChuckR.Value;
        }

        //private void BtFindLMaskCenter_Click(object sender, EventArgs e)
        //{
        //    Mat m;
        //    Rect roi;
        //    GV.TickCount = 0;

        //    if (rBLowMagnification.Checked)
        //    {
        //        if (_recipe.LeftLowMaskMat.Empty())
        //        {
        //            MessageBox.Show("請先學習光罩圖型，再找中心!!", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return;
        //        }
        //        m = _recipe.LeftLowMaskMat;
        //        DialogFindCenter FindCenter = new DialogFindCenter()
        //        {
        //            img = m.Clone(),
        //            iMaskWaferRadio = 1.2,
        //            imgM = new GrayImage(_recipe.LeftLowMaskMask)
        //        };

        //        if (FindCenter.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //        {
        //            roi = FindCenter.rect;
        //            Mat newm = new Mat(m, roi);

        //            // 從 FindCenter 取回修改後的 imgM，並裁切相同 ROI
        //            GrayImage editedMask = FindCenter.imgM;  // 假設 imgM 是 public 屬性
        //            GrayImage d = editedMask.GetSubImage(roi);  // 需要實作裁切方法

        //            _recipe.LeftLowMaskMask = new GrayImage(d);
        //        }

        //        else
        //        {
        //            return;
        //        }
        //        //roi = new Rectangle(0, 0, m.Width, m.Height);
        //        //FindTargetCenterCG findTargetCenterCG = new FindTargetCenterCG();
        //        //roi = findTargetCenterCG.AutoCenterCG(m, roi);
        //        Mat newm = new Mat(m, roi);
        //        pbLMaskImage.Image = newm.ToBitmap();

        //        if (MessageBox.Show(GV.Dlang.strbtnSaveLLM, GV.Dlang.strSureSaveLLM, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
        //        {
        //            pbLMaskImage.Image = m.ToBitmap();
        //            return;
        //        }

        //        //GV.LeftLowMaskMat[EditRecipe] = newm;
        //        //CvInvoke.Imwrite(GetTemplateFileName("LLM"), newm);
        //        GrayImage d = new GrayImage(newm.Width, newm.Height);
        //        d.Fill(255);
        //        //GV.LeftLowMaskMask[EditRecipe] = d;
        //        //d.Save(GetTemplateFileName("MLLM"), System.Drawing.Imaging.ImageFormat.Bmp);
        //        _recipe.SetLeftLowMaskMat(newm);
        //        _recipe.LeftLowMaskMask = new GrayImage(d);
        //        _AlignC.LLMaskAlgorithm = Algoritm(cbLMaskAlgorithm.SelectedIndex);
        //        GV.matcherLLM.LearnWithAlgo(newm, d, _AlignC.LLMaskAlgorithm);
        //    }
        //    else if (rBHighMagnification.Checked)
        //    {
        //        if (_recipe.LeftHighMaskMat.Empty())
        //        {
        //            MessageBox.Show("請先學習光罩圖型，再找中心!!", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return;
        //        }
        //        m = _recipe.LeftHighMaskMat;
        //        DialogFindCenter FindCenter = new DialogFindCenter()
        //        {
        //            img = m.Clone(),
        //            iMaskWaferRadio = 1.2,
        //            imgM = new GrayImage(_recipe.LeftHighMaskMask)
        //        };

        //        if (FindCenter.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //        {
        //            roi = FindCenter.rect;
        //        }
        //        else
        //        {
        //            return;
        //        }
        //        //roi = new Rectangle(0, 0, m.Width, m.Height);
        //        //FindTargetCenterCG findTargetCenterCG = new FindTargetCenterCG();
        //        //roi = findTargetCenterCG.AutoCenterCG(m, roi);
        //        Mat newm = new Mat(m, roi);
        //        pbLMaskImage.Image = newm.ToBitmap();

        //        if (MessageBox.Show(GV.Dlang.strbtnSaveLHM, GV.Dlang.strSureSaveLHM, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
        //        {
        //            pbLMaskImage.Image = m.ToBitmap();
        //            return;
        //        }

        //        //GV.LeftHighMaskMat[EditRecipe] = newm;
        //        //CvInvoke.Imwrite(GetTemplateFileName("LHM"), newm);
        //        GrayImage d = new GrayImage(newm.Width, newm.Height);
        //        d.Fill(255);
        //        //GV.LeftHighMaskMask[EditRecipe] = d;
        //        //d.Save(GetTemplateFileName("MLHM"), System.Drawing.Imaging.ImageFormat.Bmp);
        //        _recipe.SetLeftHighMaskMat(newm);
        //        _recipe.LeftHighMaskMask = new GrayImage(d);
        //        _AlignC.LHMaskAlgorithm = Algoritm(cbLMaskAlgorithm.SelectedIndex);
        //        GV.matcherLHM.LearnWithAlgo(newm, d, _AlignC.LHMaskAlgorithm);
        //    }
        //    else
        //    {
        //        MessageBox.Show("Select magnification first.", "LMaskTemplateSave", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return;
        //    }

        //    _AlignC.LastModifyTime = DateTime.Now;
        //    GM.WriteRecipeXml(EditRecipe);
        //}
        //private void BtFindLMaskCenter_Click(object sender, EventArgs e)
        //{
        //    Mat m;
        //    Rect roi;
        //    GV.TickCount = 0;

        //    if (rBLowMagnification.Checked)
        //    {
        //        if (_recipe.LeftLowMaskMat.Empty())
        //        {
        //            MessageBox.Show("請先學習光罩圖型，再找中心!!", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return;
        //        }
        //        m = _recipe.LeftLowMaskMat;
        //        DialogFindCenter FindCenter = new DialogFindCenter()
        //        {
        //            img = m.Clone(),
        //            iMaskWaferRadio = 1.2,
        //            imgM = new GrayImage(_recipe.LeftLowMaskMask)
        //        };

        //        if (FindCenter.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //        {
        //            roi = FindCenter.rect;
        //        }
        //        else
        //        {
        //            return;
        //        }

        //        Mat newm = new Mat(m, roi);
        //        pbLMaskImage.Image = newm.ToBitmap();

        //        if (MessageBox.Show(GV.Dlang.strbtnSaveLLM, GV.Dlang.strSureSaveLLM, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
        //        {
        //            pbLMaskImage.Image = m.ToBitmap();
        //            return;
        //        }

        //        _recipe.SetLeftLowMaskMat(newm);

        //        // 取得 DialogFindCenter 中修改後的遮罩
        //        GrayImage editedMask = FindCenter.imgM;

        //        // 裁切遮罩到 ROI 範圍
        //        GrayImage d = CropGrayImage(editedMask, roi);

        //        _recipe.LeftLowMaskMask = new GrayImage(d);
        //        _AlignC.LLMaskAlgorithm = Algoritm(cbLMaskAlgorithm.SelectedIndex);
        //        GV.matcherLLM.LearnWithAlgo(newm, d, _AlignC.LLMaskAlgorithm);
        //    }
        //    else if (rBHighMagnification.Checked)
        //    {
        //        if (_recipe.LeftHighMaskMat.Empty())
        //        {
        //            MessageBox.Show("請先學習光罩圖型，再找中心!!", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return;
        //        }
        //        m = _recipe.LeftHighMaskMat;
        //        DialogFindCenter FindCenter = new DialogFindCenter()
        //        {
        //            img = m.Clone(),
        //            iMaskWaferRadio = 1.2,
        //            imgM = new GrayImage(_recipe.LeftHighMaskMask)
        //        };

        //        if (FindCenter.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //        {
        //            roi = FindCenter.rect;
        //        }
        //        else
        //        {
        //            return;
        //        }

        //        Mat newm = new Mat(m, roi);
        //        pbLMaskImage.Image = newm.ToBitmap();

        //        if (MessageBox.Show(GV.Dlang.strbtnSaveLHM, GV.Dlang.strSureSaveLHM, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
        //        {
        //            pbLMaskImage.Image = m.ToBitmap();
        //            return;
        //        }

        //        _recipe.SetLeftHighMaskMat(newm);

        //        // 取得 DialogFindCenter 中修改後的遮罩
        //        GrayImage editedMask = FindCenter.imgM;

        //        // 裁切遮罩到 ROI 範圍
        //        GrayImage d = CropGrayImage(editedMask, roi);

        //        _recipe.LeftHighMaskMask = new GrayImage(d);
        //        _AlignC.LHMaskAlgorithm = Algoritm(cbLMaskAlgorithm.SelectedIndex);
        //        GV.matcherLHM.LearnWithAlgo(newm, d, _AlignC.LHMaskAlgorithm);
        //    }
        //    else
        //    {
        //        MessageBox.Show("Select magnification first.", "LMaskTemplateSave", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return;
        //    }

        //    _AlignC.LastModifyTime = DateTime.Now;
        //    GM.WriteRecipeXml(EditRecipe);
        //}

        //v2
        //private void BtFindLMaskCenter_Click(object sender, EventArgs e)
        //{
        //    Mat m;
        //    OpenCvSharp.Rect roi;
        //    GV.TickCount = 0;

        //    //  1. 檢查是否已學習影像
        //    if (rBLowMagnification.Checked)
        //    {
        //        if (_recipe.LeftLowMaskMat.Empty())
        //        {
        //            MessageBox.Show("請先學習光罩圖型，再找中心!!", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return;
        //        }
        //        m = _recipe.LeftLowMaskMat;
        //    }
        //    else if (rBHighMagnification.Checked)
        //    {
        //        if (_recipe.LeftHighMaskMat.Empty())
        //        {
        //            MessageBox.Show("請先學習光罩圖型，再找中心!!", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return;
        //        }
        //        m = _recipe.LeftHighMaskMat;
        //    }
        //    else
        //    {
        //        MessageBox.Show("請先選擇倍率（低倍或高倍）", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return;
        //    }

        //    //  2. 開啟 DialogFindCenter
        //    GrayImage currentMask = rBLowMagnification.Checked
        //        ? _recipe.LeftLowMaskMask
        //        : _recipe.LeftHighMaskMask;

        //    DialogFindCenter FindCenter = new DialogFindCenter()
        //    {
        //        img = m.Clone(),
        //        iMaskWaferRadio = 1.2,
        //        imgM = currentMask != null ? new GrayImage(currentMask) : new GrayImage(m.Width, m.Height)
        //    };

        //    if (FindCenter.ShowDialog() != DialogResult.OK)
        //    {
        //        return;
        //    }

        //    //  3. 檢查是單十字還是多十字模式
        //    var selectedROIs = FindCenter.GetSelectedROIs();

        //    if (selectedROIs != null && selectedROIs.Count > 0)
        //    {
        //        //  多十字模式：使用第一個有效的 ROI
        //        var validROI = selectedROIs.FirstOrDefault(r => r.isValid);
        //        if (validROI == null)
        //        {
        //            MessageBox.Show("所有選取區域都無效，請重新選擇", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            pbLMaskImage.Image = m.ToBitmap();
        //            return;
        //        }

        //        roi = validROI.Region;
        //        if (rBLowMagnification.Checked)
        //        {
        //            _AlignC.LLMaskROI = roi;
        //        }
        //        else if (rBHighMagnification.Checked)
        //        {
        //            _AlignC.LHMaskROI = roi;
        //        }
        //    }
        //    else
        //    {
        //        //  單十字模式：使用原本的 rect
        //        roi = FindCenter.rect;
        //        if (rBLowMagnification.Checked)
        //        {
        //            _AlignC.LLMaskROI = new OpenCvSharp.Rect(0, 0, 0, 0);
        //        }
        //        else if (rBHighMagnification.Checked)
        //        {
        //            _AlignC.LHMaskROI = new OpenCvSharp.Rect(0, 0, 0, 0);
        //        }
        //    }

        //    //  4. 裁切影像並顯示預覽
        //    Mat newm = new Mat(m, roi);
        //    pbLMaskImage.Image = newm.ToBitmap();

        //    //  5. 確認是否儲存
        //    string confirmMsg = rBLowMagnification.Checked
        //        ? GV.Dlang.strbtnSaveLLM
        //        : GV.Dlang.strbtnSaveLHM;
        //    string confirmTitle = rBLowMagnification.Checked
        //        ? GV.Dlang.strSureSaveLLM
        //        : GV.Dlang.strSureSaveLHM;

        //    if (MessageBox.Show(confirmMsg, confirmTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
        //    {
        //        pbLMaskImage.Image = m.ToBitmap();
        //        return;
        //    }

        //    //  6. 裁切遮罩並儲存
        //    GrayImage editedMask = FindCenter.imgM;
        //    GrayImage d = CropGrayImage(editedMask, roi);

        //    if (rBLowMagnification.Checked)
        //    {
        //        _recipe.SetLeftLowMaskMat(newm);
        //        _recipe.LeftLowMaskMask = new GrayImage(d);
        //        _AlignC.LLMaskAlgorithm = Algoritm(cbLMaskAlgorithm.SelectedIndex);

        //        if (GV.AppSettingParm.Emulation != true && GV.matcherLLM != null)
        //        {
        //            GV.matcherLLM.LearnWithAlgo(newm, d, _AlignC.LLMaskAlgorithm);
        //        }
        //    }
        //    else if (rBHighMagnification.Checked)
        //    {
        //        _recipe.SetLeftHighMaskMat(newm);
        //        _recipe.LeftHighMaskMask = new GrayImage(d);
        //        _AlignC.LHMaskAlgorithm = Algoritm(cbLMaskAlgorithm.SelectedIndex);

        //        if (GV.AppSettingParm.Emulation != true && GV.matcherLHM != null)
        //        {
        //            GV.matcherLHM.LearnWithAlgo(newm, d, _AlignC.LHMaskAlgorithm);
        //        }
        //    }

        //    _AlignC.LastModifyTime = DateTime.Now;
        //    GM.WriteRecipeXml(EditRecipe);

        //    MessageBox.Show("光罩中心已更新", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //}
        private void BtFindLMaskCenter_Click(object sender, EventArgs e)
        {
            Mat m;
            GV.TickCount = 0;

            //  1. 檢查是否已學習影像
            if (rBLowMagnification.Checked)
            {
                if (_recipe.LeftLowMaskMat.Empty())
                {
                    MessageBox.Show("請先學習光罩圖型,再找中心!!", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                m = _recipe.LeftLowMaskMat;
            }
            else if (rBHighMagnification.Checked)
            {
                if (_recipe.LeftHighMaskMat.Empty())
                {
                    MessageBox.Show("請先學習光罩圖型,再找中心!!", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                m = _recipe.LeftHighMaskMat;
            }
            else
            {
                MessageBox.Show("請先選擇倍率(低倍或高倍)", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //  2. 開啟 DialogFindCenter
            GrayImage currentMask = rBLowMagnification.Checked
                ? _recipe.LeftLowMaskMask
                : _recipe.LeftHighMaskMask;

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

            //  3. 使用 IsMultiCrossMode 判斷模式
            OpenCvSharp.Rect resultRect = FindCenter.rect;
            Mat imageToSave;
            GrayImage maskToSave;
            bool isMultiCross = FindCenter.IsMultiCrossMode;  // ← 直接使用標記

            //  關鍵修正:使用臨時變數儲存偏移量
            int offsetX = 0;
            int offsetY = 0;

            if (isMultiCross)
            {
                //  多十字模式:保留完整影像
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
                pbLMaskImage.Image = previewMat.ToBitmap();
                previewMat.Dispose();
            }
            else
            {
                //  單十字模式:裁切影像
                imageToSave = new Mat(m, resultRect);

                GrayImage editedMask = FindCenter.imgM;
                maskToSave = CropGrayImage(editedMask, resultRect);

                // 清除偏移量
                offsetX = 0;
                offsetY = 0;

                pbLMaskImage.Image = imageToSave.ToBitmap();
            }

            //  4. 確認是否儲存
            string confirmMsg = rBLowMagnification.Checked ? GV.Dlang.strbtnSaveLLM : GV.Dlang.strbtnSaveLHM;
            string confirmTitle = rBLowMagnification.Checked ? GV.Dlang.strSureSaveLLM : GV.Dlang.strSureSaveLHM;

            if (MessageBox.Show(confirmMsg, confirmTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                pbLMaskImage.Image = m.ToBitmap();
                return;
            }

            //  5. 儲存到 Recipe
            if (rBLowMagnification.Checked)
            {
                _recipe.SetLeftLowMaskMat(imageToSave);
                _recipe.LeftLowMaskMask = maskToSave;
                _AlignC.LLMaskAlgorithm = Algoritm(cbLMaskAlgorithm.SelectedIndex);

                //  儲存偏移量到 AlignC (會一起被寫入 Recipe XML)
                _AlignC.LLMaskOffsetX = offsetX;
                _AlignC.LLMaskOffsetY = offsetY;

                if (GV.AppSettingParm.Emulation != true && GV.matcherLLM != null)
                {
                    GV.matcherLLM.LearnWithAlgo(imageToSave, maskToSave, _AlignC.LLMaskAlgorithm);
                }
            }
            else if (rBHighMagnification.Checked)
            {
                _recipe.SetLeftHighMaskMat(imageToSave);
                _recipe.LeftHighMaskMask = maskToSave;
                _AlignC.LHMaskAlgorithm = Algoritm(cbLMaskAlgorithm.SelectedIndex);

                //  儲存偏移量到 AlignC (會一起被寫入 Recipe XML)
                _AlignC.LHMaskOffsetX = offsetX;
                _AlignC.LHMaskOffsetY = offsetY;

                if (GV.AppSettingParm.Emulation != true && GV.matcherLHM != null)
                {
                    GV.matcherLHM.LearnWithAlgo(imageToSave, maskToSave, _AlignC.LHMaskAlgorithm);
                }
            }

            //  6. 更新最後修改時間
            _AlignC.LastModifyTime = DateTime.Now;

            //  7. 【關鍵!】將 Recipe 寫入 XML,確保偏移量持久化儲存
            GM.WriteRecipeXml(EditRecipe);

            //  8. 根據模式顯示不同訊息
            if (isMultiCross)
            {
                //Mat finalPreview = imageToSave.Clone();
                //Cv2.CvtColor(finalPreview, finalPreview, ColorConversionCodes.GRAY2BGR);
                //Cv2.Circle(finalPreview, new OpenCvSharp.Point(resultRect.X, resultRect.Y), 5, new Scalar(0, 255, 0), -1);
                //pbLMaskImage.Image = finalPreview.ToBitmap();
                //finalPreview.Dispose();

                MessageBox.Show($"光罩中心已更新並儲存 (多十字模式)\n偏移量: ({offsetX:F2}, {offsetY:F2})",
                                "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                pbLMaskImage.Image = imageToSave.ToBitmap();
                MessageBox.Show("光罩中心已更新並儲存 (單十字模式)", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        /// <summary>
        /// 裁切 GrayImage 到指定的 ROI 範圍
        /// </summary>
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

        //private void BtFindLWaferCenter_Click(object sender, EventArgs e)
        //{
        //    Mat m;
        //    Rect roi;
        //    GV.TickCount = 0;

        //    if (rBLowMagnification.Checked)
        //    {
        //        if (_recipe.LeftLowWaferMat.Empty())
        //        {
        //            MessageBox.Show("請先學習晶圓圖型，再找中心!!", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return;
        //        }
        //        m = _recipe.LeftLowWaferMat;
        //        DialogFindCenter FindCenter = new DialogFindCenter()
        //        {
        //            img = m,
        //            iMaskWaferRadio = 0.8,
        //            iBlockSize = _AlignC.LLWaferBlockSize,
        //            iBitwiseNot = _AlignC.LLBitwiseNot,
        //            iAlgo = (int)_AlignC.LLWaferAlgorithm,
        //            imgM = new GrayImage(_recipe.LeftLowWaferMask)
        //        };

        //        if (FindCenter.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //        {
        //            roi = FindCenter.rect;

        //        }
        //        else
        //        {
        //            return;
        //        }
        //        Mat newm = new Mat(m, roi);
        //        pbLWaferImage.Image = newm.ToBitmap();

        //        if (MessageBox.Show(GV.Dlang.strbtnSaveLLW, GV.Dlang.strSureSaveLLW, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
        //        {
        //            pbLWaferImage.Image = m.ToBitmap();
        //            return;
        //        }

        //        _recipe.SetLeftLowWaferMat(newm);
        //        GrayImage editedMask = FindCenter.imgM;
        //        GrayImage d = CropGrayImage(editedMask, roi);
        //        _recipe.LeftLowWaferMask = new GrayImage(d);
        //        _AlignC.LLWaferAlgorithm = Algoritm(cbLWaferAlgorithm.SelectedIndex);
        //        GV.matcherLLW.LearnWithAlgo(newm, d, _AlignC.LLWaferAlgorithm);
        //    }
        //    else if (rBHighMagnification.Checked)
        //    {
        //        if (_recipe.LeftHighWaferMat.Empty())
        //        {
        //            MessageBox.Show("請先學習晶圓圖型，再找中心!!", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return;
        //        }
        //        m = _recipe.LeftHighWaferMat;
        //        DialogFindCenter FindCenter = new DialogFindCenter()
        //        {
        //            img = m.Clone(),
        //            iMaskWaferRadio = 0.8,
        //            iBlockSize = _AlignC.LHWaferBlockSize,
        //            iBitwiseNot = _AlignC.LHBitwiseNot,
        //            iAlgo = (int)_AlignC.LHWaferAlgorithm,
        //            imgM = new GrayImage(_recipe.LeftHighWaferMask)
        //        };

        //        if (FindCenter.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //        {
        //            roi = FindCenter.rect;

        //        }
        //        else
        //        {
        //            return;
        //        }
        //        Mat newm = new Mat(m, roi);
        //        pbLWaferImage.Image = newm.ToBitmap();

        //        if (MessageBox.Show(GV.Dlang.strbtnSaveLHM, GV.Dlang.strSureSaveLHM, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
        //        {
        //            pbLWaferImage.Image = m.ToBitmap();
        //            return;
        //        }

        //        _recipe.SetLeftHighWaferMat(newm);

        //        GrayImage editedMask = FindCenter.imgM;
        //        GrayImage d = CropGrayImage(editedMask, roi);
        //        _recipe.LeftHighWaferMask = new GrayImage(d);
        //        _AlignC.LHWaferAlgorithm = Algoritm(cbLWaferAlgorithm.SelectedIndex);
        //        GV.matcherLHW.LearnWithAlgo(newm, d, _AlignC.LHWaferAlgorithm);
        //    }
        //    else
        //    {
        //        MessageBox.Show("Select magnification first.", "LWaferTemplateSave", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return;
        //    }

        //    _AlignC.LastModifyTime = DateTime.Now;
        //    GM.WriteRecipeXml(EditRecipe);
        //}
        private void BtFindLWaferCenter_Click(object sender, EventArgs e)
        {
            Mat m;
            GV.TickCount = 0;

            //  1. 檢查是否已學習影像
            if (rBLowMagnification.Checked)
            {
                if (_recipe.LeftLowWaferMat.Empty())
                {
                    MessageBox.Show("請先學習晶圓圖型,再找中心!!", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                m = _recipe.LeftLowWaferMat;
            }
            else if (rBHighMagnification.Checked)
            {
                if (_recipe.LeftHighWaferMat.Empty())
                {
                    MessageBox.Show("請先學習晶圓圖型,再找中心!!", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                m = _recipe.LeftHighWaferMat;
            }
            else
            {
                MessageBox.Show("請先選擇倍率(低倍或高倍)", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //  2. 開啟 DialogFindCenter
            GrayImage currentMask = rBLowMagnification.Checked
                ? _recipe.LeftLowWaferMask
                : _recipe.LeftHighWaferMask;

            DialogFindCenter FindCenter = new DialogFindCenter()
            {
                img = m.Clone(),
                iMaskWaferRadio = 0.8,  // Wafer 用 0.8
                iBlockSize = rBLowMagnification.Checked ? _AlignC.LLWaferBlockSize : _AlignC.LHWaferBlockSize,
                iBitwiseNot = rBLowMagnification.Checked ? _AlignC.LLBitwiseNot : _AlignC.LHBitwiseNot,
                iAlgo = (int)(rBLowMagnification.Checked ? _AlignC.LLWaferAlgorithm : _AlignC.LHWaferAlgorithm),
                imgM = currentMask != null ? new GrayImage(currentMask) : new GrayImage(m.Width, m.Height)
            };

            if (FindCenter.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            //  3. 使用 IsMultiCrossMode 判斷模式
            OpenCvSharp.Rect resultRect = FindCenter.rect;
            Mat imageToSave;
            GrayImage maskToSave;
            bool isMultiCross = FindCenter.IsMultiCrossMode;

            //  關鍵修正:使用臨時變數儲存偏移量
            int offsetX = 0;
            int offsetY = 0;

            if (isMultiCross)
            {
                //  多十字模式:保留完整影像
                imageToSave = m.Clone();

                GrayImage editedMask = FindCenter.imgM;
                maskToSave = new GrayImage(editedMask);

                // ⚠️ 從 resultRect 取出偏移量
                offsetX = resultRect.Width;
                offsetY = resultRect.Height;

                // 預覽:顯示完整影像 + 中心點標記
                Mat previewMat = m.Clone();
                Cv2.CvtColor(previewMat, previewMat, ColorConversionCodes.GRAY2BGR);
                Cv2.Circle(previewMat, new OpenCvSharp.Point(resultRect.X, resultRect.Y), 5, new Scalar(0, 255, 0), -1);
                Cv2.Line(previewMat, new OpenCvSharp.Point(resultRect.X - 20, resultRect.Y),
                         new OpenCvSharp.Point(resultRect.X + 20, resultRect.Y), new Scalar(0, 255, 0), 2);
                Cv2.Line(previewMat, new OpenCvSharp.Point(resultRect.X, resultRect.Y - 20),
                         new OpenCvSharp.Point(resultRect.X, resultRect.Y + 20), new Scalar(0, 255, 0), 2);
                pbLWaferImage.Image = previewMat.ToBitmap();
                previewMat.Dispose();
            }
            else
            {
                //  單十字模式:裁切影像
                imageToSave = new Mat(m, resultRect);

                GrayImage editedMask = FindCenter.imgM;
                maskToSave = CropGrayImage(editedMask, resultRect);

                // 清除偏移量
                offsetX = 0;
                offsetY = 0;

                pbLWaferImage.Image = imageToSave.ToBitmap();
            }

            //  4. 確認是否儲存
            string confirmMsg = rBLowMagnification.Checked ? GV.Dlang.strbtnSaveLLW : GV.Dlang.strbtnSaveLHW;
            string confirmTitle = rBLowMagnification.Checked ? GV.Dlang.strSureSaveLLW : GV.Dlang.strSureSaveLHW;

            if (MessageBox.Show(confirmMsg, confirmTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                pbLWaferImage.Image = m.ToBitmap();
                return;
            }

            //  5. 儲存到 Recipe
            if (rBLowMagnification.Checked)
            {
                _recipe.SetLeftLowWaferMat(imageToSave);
                _recipe.LeftLowWaferMask = maskToSave;
                _AlignC.LLWaferAlgorithm = Algoritm(cbLWaferAlgorithm.SelectedIndex);

                //  儲存偏移量到 AlignC (會一起被寫入 Recipe XML)
                _AlignC.LLWaferOffsetX = offsetX;
                _AlignC.LLWaferOffsetY = offsetY;

                if (GV.AppSettingParm.Emulation != true && GV.matcherLLW != null)
                {
                    GV.matcherLLW.LearnWithAlgo(imageToSave, maskToSave, _AlignC.LLWaferAlgorithm);
                }
            }
            else if (rBHighMagnification.Checked)
            {
                _recipe.SetLeftHighWaferMat(imageToSave);
                _recipe.LeftHighWaferMask = maskToSave;
                _AlignC.LHWaferAlgorithm = Algoritm(cbLWaferAlgorithm.SelectedIndex);

                //  儲存偏移量到 AlignC (會一起被寫入 Recipe XML)
                _AlignC.LHWaferOffsetX = offsetX;
                _AlignC.LHWaferOffsetY = offsetY;

                if (GV.AppSettingParm.Emulation != true && GV.matcherLHW != null)
                {
                    GV.matcherLHW.LearnWithAlgo(imageToSave, maskToSave, _AlignC.LHWaferAlgorithm);
                }
            }

            //  6. 更新最後修改時間
            _AlignC.LastModifyTime = DateTime.Now;

            //  7. 【關鍵!】將 Recipe 寫入 XML,確保偏移量持久化儲存
            GM.WriteRecipeXml(EditRecipe);

            //  8. 根據模式顯示不同訊息
            if (isMultiCross)
            {
                //Mat finalPreview = imageToSave.Clone();
                //Cv2.CvtColor(finalPreview, finalPreview, ColorConversionCodes.GRAY2BGR);
                //Cv2.Circle(finalPreview, new OpenCvSharp.Point(resultRect.X, resultRect.Y), 5, new Scalar(0, 255, 0), -1);
                //pbLWaferImage.Image = finalPreview.ToBitmap();
                //finalPreview.Dispose();

                MessageBox.Show($"晶圓中心已更新並儲存 (多十字模式)\n偏移量: ({offsetX:F2}, {offsetY:F2})",
                                "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                pbLWaferImage.Image = imageToSave.ToBitmap();
                MessageBox.Show("晶圓中心已更新並儲存 (單十字模式)", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //private void BtFindRWaferCenter_Click(object sender, EventArgs e)
        //{
        //    Mat m;
        //    Rect roi;
        //    GV.TickCount = 0;

        //    if (rBLowMagnification.Checked)
        //    {
        //        if (_recipe.RightLowWaferMat.Empty())
        //        {
        //            MessageBox.Show("請先學習晶圓圖型，再找中心!!", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return;
        //        }
        //        m = _recipe.RightLowWaferMat;
        //        DialogFindCenter FindCenter = new DialogFindCenter()
        //        {
        //            img = m.Clone(),
        //            iMaskWaferRadio = 0.8,
        //            iBlockSize = _AlignC.RLWaferBlockSize,
        //            iBitwiseNot = _AlignC.RLBitwiseNot,
        //            iAlgo = (int)_AlignC.RLWaferAlgorithm,
        //            imgM = new GrayImage(_recipe.RightLowWaferMask)
        //        };

        //        if (FindCenter.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //        {
        //            roi = FindCenter.rect;
        //            if (cbRWaferAlgorithm.SelectedIndex == 3)
        //            {
        //                _AlignC.RLWaferBlockSize = FindCenter.iBlockSize;
        //                _AlignC.RLBitwiseNot = FindCenter.iBitwiseNot;
        //                // GV.matcherRLW.SetAlgoParameter(FindCenter.iBlockSize, FindCenter.iBitwiseNot);
        //            }
        //        }
        //        else
        //        {
        //            return;
        //        }
        //        //roi = new Rectangle(0, 0, m.Width, m.Height);
        //        //FindTargetCenterCG findTargetCenterCG = new FindTargetCenterCG();
        //        //roi = findTargetCenterCG.AutoCenterCGWafer(m, roi);
        //        Mat newm = new Mat(m, roi);
        //        pbRWaferImage.Image = newm.ToBitmap();

        //        if (MessageBox.Show(GV.Dlang.strbtnSaveRLW, GV.Dlang.strSureSaveRLW, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
        //        {
        //            pbRWaferImage.Image = m.ToBitmap();
        //            return;
        //        }

        //        _recipe.SetRightLowWaferMat(newm);
        //        GrayImage editedMask = FindCenter.imgM;
        //        GrayImage d = CropGrayImage(editedMask, roi);
        //        _recipe.RightLowWaferMask = new GrayImage(d);
        //        _AlignC.RLWaferAlgorithm = Algoritm(cbRWaferAlgorithm.SelectedIndex);
        //        GV.matcherRLW.LearnWithAlgo(newm, d, _AlignC.RLWaferAlgorithm);
        //    }
        //    else if (rBHighMagnification.Checked)
        //    {
        //        if (_recipe.RightHighWaferMat.Empty())
        //        {
        //            MessageBox.Show("請先學習晶圓圖型，再找中心!!", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return;
        //        }
        //        m = _recipe.RightHighWaferMat;
        //        DialogFindCenter FindCenter = new DialogFindCenter()
        //        {
        //            img = m.Clone(),
        //            iMaskWaferRadio = 0.8,
        //            iBlockSize = _AlignC.RHWaferBlockSize,
        //            iAlgo = (int)_AlignC.RHWaferAlgorithm,
        //            iBitwiseNot = _AlignC.RHBitwiseNot,
        //            imgM = new GrayImage(_recipe.RightHighWaferMask)
        //        };

        //        if (FindCenter.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //        {
        //            roi = FindCenter.rect;
        //            if (cbRWaferAlgorithm.SelectedIndex == 3)
        //            {
        //                _AlignC.RHWaferBlockSize = FindCenter.iBlockSize;
        //                _AlignC.RHBitwiseNot = FindCenter.iBitwiseNot;
        //                //GV.matcherRHW.SetAlgoParameter(FindCenter.iBlockSize, FindCenter.iBitwiseNot);
        //            }
        //        }
        //        else
        //        {
        //            return;
        //        }
        //        //roi = new Rectangle(0, 0, m.Width, m.Height);
        //        //FindTargetCenterCG findTargetCenterCG = new FindTargetCenterCG();
        //        //roi = findTargetCenterCG.AutoCenterCGWafer(m, roi);
        //        Mat newm = new Mat(m, roi);
        //        pbRWaferImage.Image = newm.ToBitmap();

        //        if (MessageBox.Show(GV.Dlang.strbtnSaveLHM, GV.Dlang.strSureSaveLHM, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
        //        {
        //            pbRWaferImage.Image = m.ToBitmap();
        //            return;
        //        }

        //        _recipe.SetRightHighWaferMat(newm);
        //        GrayImage editedMask = FindCenter.imgM;
        //        GrayImage d = CropGrayImage(editedMask, roi);
        //        _recipe.RightHighWaferMask = new GrayImage(d);
        //        // GM.Learn(GV.m_MatcherRW, ImageConvert.MatToGrayImage(GV.RightHighWaferMat[EditRecipe]), GV.RightHighWaferMask[EditRecipe]);
        //        _AlignC.RHWaferAlgorithm = Algoritm(cbRWaferAlgorithm.SelectedIndex);
        //        GV.matcherRHW.LearnWithAlgo(newm, d, _AlignC.RHWaferAlgorithm);
        //    }
        //    else
        //    {
        //        MessageBox.Show("Select magnification first.", "RWaferTemplateSave", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return;
        //    }

        //    _AlignC.LastModifyTime = DateTime.Now;
        //    GM.WriteRecipeXml(EditRecipe);
        //}
        private void BtFindRWaferCenter_Click(object sender, EventArgs e)
        {
            Mat m;
            GV.TickCount = 0;

            //  1. 檢查是否已學習影像
            if (rBLowMagnification.Checked)
            {
                if (_recipe.RightLowWaferMat.Empty())
                {
                    MessageBox.Show("請先學習晶圓圖型,再找中心!!", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                m = _recipe.RightLowWaferMat;
            }
            else if (rBHighMagnification.Checked)
            {
                if (_recipe.RightHighWaferMat.Empty())
                {
                    MessageBox.Show("請先學習晶圓圖型,再找中心!!", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                m = _recipe.RightHighWaferMat;
            }
            else
            {
                MessageBox.Show("請先選擇倍率(低倍或高倍)", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //  2. 開啟 DialogFindCenter
            GrayImage currentMask = rBLowMagnification.Checked
                ? _recipe.RightLowWaferMask
                : _recipe.RightHighWaferMask;

            DialogFindCenter FindCenter = new DialogFindCenter()
            {
                img = m.Clone(),
                iMaskWaferRadio = 0.8,  // Wafer 用 0.8
                iBlockSize = rBLowMagnification.Checked ? _AlignC.RLWaferBlockSize : _AlignC.RHWaferBlockSize,
                iBitwiseNot = rBLowMagnification.Checked ? _AlignC.RLBitwiseNot : _AlignC.RHBitwiseNot,
                iAlgo = (int)(rBLowMagnification.Checked ? _AlignC.RLWaferAlgorithm : _AlignC.RHWaferAlgorithm),
                imgM = currentMask != null ? new GrayImage(currentMask) : new GrayImage(m.Width, m.Height)
            };

            if (FindCenter.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            //  3. 使用 IsMultiCrossMode 判斷模式
            OpenCvSharp.Rect resultRect = FindCenter.rect;
            Mat imageToSave;
            GrayImage maskToSave;
            bool isMultiCross = FindCenter.IsMultiCrossMode;

            //  關鍵修正:使用臨時變數儲存偏移量
            int offsetX = 0;
            int offsetY = 0;

            if (isMultiCross)
            {
                //  多十字模式:保留完整影像
                imageToSave = m.Clone();

                GrayImage editedMask = FindCenter.imgM;
                maskToSave = new GrayImage(editedMask);

                // ⚠️ 從 resultRect 取出偏移量
                offsetX = resultRect.Width;
                offsetY = resultRect.Height;

                // 預覽:顯示完整影像 + 中心點標記
                Mat previewMat = m.Clone();
                Cv2.CvtColor(previewMat, previewMat, ColorConversionCodes.GRAY2BGR);
                Cv2.Circle(previewMat, new OpenCvSharp.Point(resultRect.X, resultRect.Y), 5, new Scalar(0, 255, 0), -1);
                Cv2.Line(previewMat, new OpenCvSharp.Point(resultRect.X - 20, resultRect.Y),
                         new OpenCvSharp.Point(resultRect.X + 20, resultRect.Y), new Scalar(0, 255, 0), 2);
                Cv2.Line(previewMat, new OpenCvSharp.Point(resultRect.X, resultRect.Y - 20),
                         new OpenCvSharp.Point(resultRect.X, resultRect.Y + 20), new Scalar(0, 255, 0), 2);
                pbRWaferImage.Image = previewMat.ToBitmap();
                previewMat.Dispose();
            }
            else
            {
                //  單十字模式:裁切影像
                imageToSave = new Mat(m, resultRect);

                GrayImage editedMask = FindCenter.imgM;
                maskToSave = CropGrayImage(editedMask, resultRect);

                // 清除偏移量
                offsetX = 0;
                offsetY = 0;

                pbRWaferImage.Image = imageToSave.ToBitmap();
            }

            //  4. 確認是否儲存
            string confirmMsg = rBLowMagnification.Checked ? GV.Dlang.strbtnSaveRLW : GV.Dlang.strbtnSaveRHW;
            string confirmTitle = rBLowMagnification.Checked ? GV.Dlang.strSureSaveRLW : GV.Dlang.strSureSaveRHW;

            if (MessageBox.Show(confirmMsg, confirmTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                pbRWaferImage.Image = m.ToBitmap();
                return;
            }

            //  5. 儲存到 Recipe
            if (rBLowMagnification.Checked)
            {
                _recipe.SetRightLowWaferMat(imageToSave);
                _recipe.RightLowWaferMask = maskToSave;
                _AlignC.RLWaferAlgorithm = Algoritm(cbRWaferAlgorithm.SelectedIndex);

                //  儲存偏移量到 AlignC (會一起被寫入 Recipe XML)
                _AlignC.RLWaferOffsetX = offsetX;
                _AlignC.RLWaferOffsetY = offsetY;

                if (GV.AppSettingParm.Emulation != true && GV.matcherRLW != null)
                {
                    GV.matcherRLW.LearnWithAlgo(imageToSave, maskToSave, _AlignC.RLWaferAlgorithm);
                }
            }
            else if (rBHighMagnification.Checked)
            {
                _recipe.SetRightHighWaferMat(imageToSave);
                _recipe.RightHighWaferMask = maskToSave;
                _AlignC.RHWaferAlgorithm = Algoritm(cbRWaferAlgorithm.SelectedIndex);

                //  儲存偏移量到 AlignC (會一起被寫入 Recipe XML)
                _AlignC.RHWaferOffsetX = offsetX;
                _AlignC.RHWaferOffsetY = offsetY;

                if (GV.AppSettingParm.Emulation != true && GV.matcherRHW != null)
                {
                    GV.matcherRHW.LearnWithAlgo(imageToSave, maskToSave, _AlignC.RHWaferAlgorithm);
                }
            }

            //  6. 更新最後修改時間
            _AlignC.LastModifyTime = DateTime.Now;

            //  7. 【關鍵!】將 Recipe 寫入 XML,確保偏移量持久化儲存
            GM.WriteRecipeXml(EditRecipe);

            //  8. 根據模式顯示不同訊息
            if (isMultiCross)
            {
                //Mat finalPreview = imageToSave.Clone();
                //Cv2.CvtColor(finalPreview, finalPreview, ColorConversionCodes.GRAY2BGR);
                //Cv2.Circle(finalPreview, new OpenCvSharp.Point(resultRect.X, resultRect.Y), 5, new Scalar(0, 255, 0), -1);
                //pbRWaferImage.Image = finalPreview.ToBitmap();
                //finalPreview.Dispose();

                MessageBox.Show($"晶圓中心已更新並儲存 (多十字模式)\n偏移量: ({offsetX:F2}, {offsetY:F2})",
                                "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                pbRWaferImage.Image = imageToSave.ToBitmap();
                MessageBox.Show("晶圓中心已更新並儲存 (單十字模式)", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //private void BtFindRMaskCenter_Click(object sender, EventArgs e)
        //{
        //    Mat m;
        //    Rect roi;
        //    GV.TickCount = 0;

        //    if (rBLowMagnification.Checked)
        //    {
        //        if (_recipe.RightLowMaskMat.Empty())
        //        {
        //            MessageBox.Show("請先學習光罩圖型，再找中心!!", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return;
        //        }
        //        m = _recipe.RightLowMaskMat;
        //        DialogFindCenter FindCenter = new DialogFindCenter()
        //        {
        //            img = m.Clone(),
        //            iMaskWaferRadio = 1.2,
        //            imgM = new GrayImage(_recipe.RightLowMaskMask)
        //        };

        //        if (FindCenter.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //        {
        //            roi = FindCenter.rect;
        //        }
        //        else
        //        {
        //            return;
        //        }
        //        Mat newm = new Mat(m, roi);
        //        pbRMaskImage.Image = newm.ToBitmap();

        //        if (MessageBox.Show(GV.Dlang.strbtnSaveRLM, GV.Dlang.strSureSaveRLM, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
        //        {
        //            pbRMaskImage.Image = m.ToBitmap();
        //            return;
        //        }

        //        _recipe.SetRightLowMaskMat(newm);
        //        GrayImage editedMask = FindCenter.imgM;
        //        GrayImage d = CropGrayImage(editedMask, roi);
        //        _recipe.RightLowMaskMask = new GrayImage(d);
        //        _AlignC.RLMaskAlgorithm = Algoritm(cbRMaskAlgorithm.SelectedIndex);
        //        GV.matcherRLM.LearnWithAlgo(newm, d, _AlignC.RLMaskAlgorithm);
        //    }
        //    else if (rBHighMagnification.Checked)
        //    {
        //        if (_recipe.RightHighMaskMat.Empty())
        //        {
        //            MessageBox.Show("請先學習光罩圖型，再找中心!!", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return;
        //        }
        //        m = _recipe.RightHighMaskMat;
        //        DialogFindCenter FindCenter = new DialogFindCenter()
        //        {
        //            img = m.Clone(),
        //            iMaskWaferRadio = 1.2,
        //            imgM = new GrayImage(_recipe.RightHighMaskMask)
        //        };

        //        if (FindCenter.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //        {
        //            roi = FindCenter.rect;
        //        }
        //        else
        //        {
        //            return;
        //        }
        //        Mat newm = new Mat(m, roi);
        //        pbRMaskImage.Image = newm.ToBitmap();

        //        if (MessageBox.Show(GV.Dlang.strbtnSaveRHM, GV.Dlang.strSureSaveRHM, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
        //        {
        //            pbRMaskImage.Image = m.ToBitmap();
        //            return;
        //        }

        //        _recipe.SetRightHighMaskMat(newm);
        //        GrayImage editedMask = FindCenter.imgM;
        //        GrayImage d = CropGrayImage(editedMask, roi);
        //        _recipe.RightHighMaskMask = new GrayImage(d);
        //        _AlignC.RHMaskAlgorithm = Algoritm(cbRMaskAlgorithm.SelectedIndex);
        //        GV.matcherRHM.LearnWithAlgo(newm, d, _AlignC.RHMaskAlgorithm);
        //    }
        //    else
        //    {
        //        MessageBox.Show("Select magnification first.", "RMaskTemplateSave", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return;
        //    }

        //    _AlignC.LastModifyTime = DateTime.Now;
        //    GM.WriteRecipeXml(EditRecipe);
        //}
        private void BtFindRMaskCenter_Click(object sender, EventArgs e)
        {
            Mat m;
            GV.TickCount = 0;

            //  1. 檢查是否已學習影像
            if (rBLowMagnification.Checked)
            {
                if (_recipe.RightLowMaskMat.Empty())
                {
                    MessageBox.Show("請先學習光罩圖型,再找中心!!", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                m = _recipe.RightLowMaskMat;
            }
            else if (rBHighMagnification.Checked)
            {
                if (_recipe.RightHighMaskMat.Empty())
                {
                    MessageBox.Show("請先學習光罩圖型,再找中心!!", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                m = _recipe.RightHighMaskMat;
            }
            else
            {
                MessageBox.Show("請先選擇倍率(低倍或高倍)", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //  2. 開啟 DialogFindCenter
            GrayImage currentMask = rBLowMagnification.Checked
                ? _recipe.RightLowMaskMask
                : _recipe.RightHighMaskMask;

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

            //  3. 使用 IsMultiCrossMode 判斷模式
            OpenCvSharp.Rect resultRect = FindCenter.rect;
            Mat imageToSave;
            GrayImage maskToSave;
            bool isMultiCross = FindCenter.IsMultiCrossMode;

            //  關鍵修正:使用臨時變數儲存偏移量
            int offsetX = 0;
            int offsetY = 0;

            if (isMultiCross)
            {
                //  多十字模式:保留完整影像
                imageToSave = m.Clone();

                GrayImage editedMask = FindCenter.imgM;
                maskToSave = new GrayImage(editedMask);

                // ⚠️ 從 resultRect 取出偏移量
                offsetX = resultRect.Width;
                offsetY = resultRect.Height;

                // 預覽:顯示完整影像 + 中心點標記
                Mat previewMat = m.Clone();
                Cv2.CvtColor(previewMat, previewMat, ColorConversionCodes.GRAY2BGR);
                Cv2.Circle(previewMat, new OpenCvSharp.Point(resultRect.X, resultRect.Y), 5, new Scalar(0, 255, 0), -1);
                Cv2.Line(previewMat, new OpenCvSharp.Point(resultRect.X - 20, resultRect.Y),
                         new OpenCvSharp.Point(resultRect.X + 20, resultRect.Y), new Scalar(0, 255, 0), 2);
                Cv2.Line(previewMat, new OpenCvSharp.Point(resultRect.X, resultRect.Y - 20),
                         new OpenCvSharp.Point(resultRect.X, resultRect.Y + 20), new Scalar(0, 255, 0), 2);
                pbRMaskImage.Image = previewMat.ToBitmap();
                previewMat.Dispose();
            }
            else
            {
                //  單十字模式:裁切影像
                imageToSave = new Mat(m, resultRect);

                GrayImage editedMask = FindCenter.imgM;
                maskToSave = CropGrayImage(editedMask, resultRect);

                // 清除偏移量
                offsetX = 0;
                offsetY = 0;

                pbRMaskImage.Image = imageToSave.ToBitmap();
            }

            //  4. 確認是否儲存
            string confirmMsg = rBLowMagnification.Checked ? GV.Dlang.strbtnSaveRLM : GV.Dlang.strbtnSaveRHM;
            string confirmTitle = rBLowMagnification.Checked ? GV.Dlang.strSureSaveRLM : GV.Dlang.strSureSaveRHM;

            if (MessageBox.Show(confirmMsg, confirmTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                pbRMaskImage.Image = m.ToBitmap();
                return;
            }

            //  5. 儲存到 Recipe
            if (rBLowMagnification.Checked)
            {
                _recipe.SetRightLowMaskMat(imageToSave);
                _recipe.RightLowMaskMask = maskToSave;
                _AlignC.RLMaskAlgorithm = Algoritm(cbRMaskAlgorithm.SelectedIndex);

                //  儲存偏移量到 AlignC (會一起被寫入 Recipe XML)
                _AlignC.RLMaskOffsetX = offsetX;
                _AlignC.RLMaskOffsetY = offsetY;

                if (GV.AppSettingParm.Emulation != true && GV.matcherRLM != null)
                {
                    GV.matcherRLM.LearnWithAlgo(imageToSave, maskToSave, _AlignC.RLMaskAlgorithm);
                }
            }
            else if (rBHighMagnification.Checked)
            {
                _recipe.SetRightHighMaskMat(imageToSave);
                _recipe.RightHighMaskMask = maskToSave;
                _AlignC.RHMaskAlgorithm = Algoritm(cbRMaskAlgorithm.SelectedIndex);

                //  儲存偏移量到 AlignC (會一起被寫入 Recipe XML)
                _AlignC.RHMaskOffsetX = offsetX;
                _AlignC.RHMaskOffsetY = offsetY;

                if (GV.AppSettingParm.Emulation != true && GV.matcherRHM != null)
                {
                    GV.matcherRHM.LearnWithAlgo(imageToSave, maskToSave, _AlignC.RHMaskAlgorithm);
                }
            }

            //  6. 更新最後修改時間
            _AlignC.LastModifyTime = DateTime.Now;

            //  7. 【關鍵!】將 Recipe 寫入 XML,確保偏移量持久化儲存
            GM.WriteRecipeXml(EditRecipe);

            //  8. 根據模式顯示不同訊息
            if (isMultiCross)
            {
                //Mat finalPreview = imageToSave.Clone();
                //Cv2.CvtColor(finalPreview, finalPreview, ColorConversionCodes.GRAY2BGR);
                //Cv2.Circle(finalPreview, new OpenCvSharp.Point(resultRect.X, resultRect.Y), 5, new Scalar(0, 255, 0), -1);
                //pbRMaskImage.Image = finalPreview.ToBitmap();
                //finalPreview.Dispose();

                MessageBox.Show($"光罩中心已更新並儲存 (多十字模式)\n偏移量: ({offsetX:F2}, {offsetY:F2})",
                                "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                pbRMaskImage.Image = imageToSave.ToBitmap();
                MessageBox.Show("光罩中心已更新並儲存 (單十字模式)", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtLeftWaferFocus_Click(object sender, EventArgs e)
        {

        }

        private void btLabelPatternL_Click(object sender, EventArgs e)
        {
            Bitmap bmp = GV.LeftUpCam.Grab().ToBitmap();
            DialogLabelImage labelImage = new DialogLabelImage(bmp, this.NowMageni);
            labelImage.ShowDialog();
        }

        private void btLabelPatternR_Click(object sender, EventArgs e)
        {
            Bitmap bmp = GV.RightUpCam.Grab().ToBitmap();
            DialogLabelImage labelImage = new DialogLabelImage(bmp, NowMageni);
            labelImage.ShowDialog();
        }

        private void btTfile_Click(object sender, EventArgs e)
        {
            isLive = false;
            Thread thWorkImageT = new Thread(WorkImageT);
            thWorkImageT.Start();
            GV.LeftUpCam.IsFileImage = true;
            GV.RightUpCam.IsFileImage = true;
            lbEmulationModeL.Visible = false;
            lbEmulationModeR.Visible = false;
        }

        private void WorkImageT()
        {
            while (!isLive)
            {
                Thread.Sleep(100);
            }
        }

        private void btTLRead_Click(object sender, EventArgs e)
        {
            ReadViewMat(1);
        }

        private void btTRRead_Click(object sender, EventArgs e)
        {
            ReadViewMat(0);
        }
        private void ReadViewMat(int iRL)
        {
            OpenFileDialogReadT.InitialDirectory = Environment.CurrentDirectory;
            if (OpenFileDialogReadT.ShowDialog() != DialogResult.OK) return;
            string filePath = OpenFileDialogReadT.FileName;
            if (iRL == 0)
            {
                GV.RightUpCam.Freeze();
                Thread.Sleep(250);
                lock (_locker)
                {
                    RTViewMat = Cv2.ImRead(filePath, ImreadModes.Grayscale);
                }
                if (RTViewMat.Empty())
                {
                    Console.WriteLine("Read Right Image Fail!");
                }
                skRPattern.SetImage(RTViewMat);
                GV.RightUpCam.SetSimulationImage(RTViewMat);
                GV.RightUpCam.IsFileImage = true;

                GV.RightUpCam.Live();
            }
            else
            {
                GV.LeftUpCam.Freeze();
                Thread.Sleep(250);
                lock (_locker)
                {
                    LTViewMat = Cv2.ImRead(filePath, ImreadModes.Grayscale);
                }
                if (LTViewMat.Empty())
                {
                    Console.WriteLine("Read Left Image Fail!");
                }
                skLPattern.SetImage(LTViewMat);
                GV.LeftUpCam.SetSimulationImage(LTViewMat);
                GV.LeftUpCam.IsFileImage = true;

                GV.LeftUpCam.Live();
            }
        }
        public void ShowGroupBox4(bool visible)
        {
            groupBoxT4.Visible = visible;
        }
        private void SetComboBoxByClassId(ComboBox comboBox, int classId)
        {
            Dictionary<string, Int16> ClassList = GV.AIClassList.GetClassList();

            // 反查類別名稱
            string className = ClassList.FirstOrDefault(x => x.Value == classId).Key;

            if (!string.IsNullOrEmpty(className))
            {
                // 在 ComboBox 中尋找對應的項目
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

        private void ucPosition_Click(object sender, EventArgs e)
        {
            ApplyLanguage();
            if (!isLocationUpdateEnabled)
            {
                // 開啟位置顯示
                groupBoxT4.Visible = true;
                groupBoxT1.Visible = true;
                locationUpdateTimer.Start();
                isLocationUpdateEnabled = true;
                ucPosition.Text = "Close Position";

                // 立即更新一次
                UpdateCurrentLocationUI();
            }
            else
            {
                // 關閉位置顯示
                locationUpdateTimer.Stop();
                groupBoxT4.Visible = false;
                groupBoxT1.Visible = false;
                isLocationUpdateEnabled = false;
                ucPosition.Text = "Open Position";
            }
        }
        public void OnPageLeave()
        {
            try
            {
                //   1. 儲存亮度設定
                if (rBLowMagnification.Checked)
                {
                    _AlignC.LRingLight[NowMageni] = LRingLight.Checked;
                    _AlignC.LeftRingBrightness[NowMageni] = tBLeftRingLight.Value;
                    _AlignC.LeftLight[NowMageni] = LCoaLight.Checked;
                    _AlignC.LeftBrightness[NowMageni] = tBLeftCoaLight.Value;
                    _AlignC.RightLight[NowMageni] = RCoaLight.Checked;
                    _AlignC.RightBrightness[NowMageni] = tBRightCoaLight.Value;
                    _AlignC.RRingLight[NowMageni] = RRingLight.Checked;
                    _AlignC.RightRingBrightness[NowMageni] = tBRightRingLight.Value;
                }
                else if (rBHighMagnification.Checked)
                {
                    _AlignC.LRingLight[NowMageni] = LRingLight.Checked;
                    _AlignC.LeftRingBrightness[NowMageni] = tBLeftRingLight.Value;
                    _AlignC.LeftLight[NowMageni] = LCoaLight.Checked;
                    _AlignC.LeftBrightness[NowMageni] = tBLeftCoaLight.Value;
                    _AlignC.RightLight[NowMageni] = RCoaLight.Checked;
                    _AlignC.RightBrightness[NowMageni] = tBRightCoaLight.Value;
                    _AlignC.RRingLight[NowMageni] = RRingLight.Checked;
                    _AlignC.RightRingBrightness[NowMageni] = tBRightRingLight.Value;
                }

                //   2. 儲存演算法選擇
                if (rBLowMagnification.Checked)
                {
                    _AlignC.LLMaskAlgorithm = Algoritm(cbLMaskAlgorithm.SelectedIndex);
                    _AlignC.RLMaskAlgorithm = Algoritm(cbRMaskAlgorithm.SelectedIndex);
                    _AlignC.LLWaferAlgorithm = Algoritm(cbLWaferAlgorithm.SelectedIndex);
                    _AlignC.RLWaferAlgorithm = Algoritm(cbRWaferAlgorithm.SelectedIndex);
                }
                else if (rBHighMagnification.Checked)
                {
                    _AlignC.LHMaskAlgorithm = Algoritm(cbLMaskAlgorithm.SelectedIndex);
                    _AlignC.RHMaskAlgorithm = Algoritm(cbRMaskAlgorithm.SelectedIndex);
                    _AlignC.LHWaferAlgorithm = Algoritm(cbLWaferAlgorithm.SelectedIndex);
                    _AlignC.RHWaferAlgorithm = Algoritm(cbRWaferAlgorithm.SelectedIndex);
                }

                //   3. 儲存 AI ClassId（如果選擇 AIMatch 且有選擇類別）
                if (rBLowMagnification.Checked)
                {
                    if (_AlignC.LLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && cbLMaskClassList.SelectedIndex >= 0)
                    {
                        Dictionary<string, Int16> ClassList = GV.AIClassList.GetClassList();
                        string selectedClassName = cbLMaskClassList.SelectedItem?.ToString();
                        if (!string.IsNullOrEmpty(selectedClassName) && ClassList.ContainsKey(selectedClassName))
                        {
                            _AlignC.LLMaskAIClassId = ClassList[selectedClassName];
                        }
                    }

                    if (_AlignC.RLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && cbRMaskClassList.SelectedIndex >= 0)
                    {
                        Dictionary<string, Int16> ClassList = GV.AIClassList.GetClassList();
                        string selectedClassName = cbRMaskClassList.SelectedItem?.ToString();
                        if (!string.IsNullOrEmpty(selectedClassName) && ClassList.ContainsKey(selectedClassName))
                        {
                            _AlignC.RLMaskAIClassId = ClassList[selectedClassName];
                        }
                    }

                    if (_AlignC.LLWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && cbLWaferClassList.SelectedIndex >= 0)
                    {
                        Dictionary<string, Int16> ClassList = GV.AIClassList.GetClassList();
                        string selectedClassName = cbLWaferClassList.SelectedItem?.ToString();
                        if (!string.IsNullOrEmpty(selectedClassName) && ClassList.ContainsKey(selectedClassName))
                        {
                            _AlignC.LLWaferAIClassId = ClassList[selectedClassName];
                        }
                    }

                    if (_AlignC.RLWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && cbRWaferClassList.SelectedIndex >= 0)
                    {
                        Dictionary<string, Int16> ClassList = GV.AIClassList.GetClassList();
                        string selectedClassName = cbRWaferClassList.SelectedItem?.ToString();
                        if (!string.IsNullOrEmpty(selectedClassName) && ClassList.ContainsKey(selectedClassName))
                        {
                            _AlignC.RLWaferAIClassId = ClassList[selectedClassName];
                        }
                    }
                }
                else if (rBHighMagnification.Checked)
                {
                    if (_AlignC.LHMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && cbLMaskClassList.SelectedIndex >= 0)
                    {
                        Dictionary<string, Int16> ClassList = GV.AIClassList.GetClassList();
                        string selectedClassName = cbLMaskClassList.SelectedItem?.ToString();
                        if (!string.IsNullOrEmpty(selectedClassName) && ClassList.ContainsKey(selectedClassName))
                        {
                            _AlignC.LHMaskAIClassId = ClassList[selectedClassName];
                        }
                    }

                    if (_AlignC.RHMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && cbRMaskClassList.SelectedIndex >= 0)
                    {
                        Dictionary<string, Int16> ClassList = GV.AIClassList.GetClassList();
                        string selectedClassName = cbRMaskClassList.SelectedItem?.ToString();
                        if (!string.IsNullOrEmpty(selectedClassName) && ClassList.ContainsKey(selectedClassName))
                        {
                            _AlignC.RHMaskAIClassId = ClassList[selectedClassName];
                        }
                    }

                    if (_AlignC.LHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && cbLWaferClassList.SelectedIndex >= 0)
                    {
                        Dictionary<string, Int16> ClassList = GV.AIClassList.GetClassList();
                        string selectedClassName = cbLWaferClassList.SelectedItem?.ToString();
                        if (!string.IsNullOrEmpty(selectedClassName) && ClassList.ContainsKey(selectedClassName))
                        {
                            _AlignC.LHWaferAIClassId = ClassList[selectedClassName];
                        }
                    }

                    if (_AlignC.RHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && cbRWaferClassList.SelectedIndex >= 0)
                    {
                        Dictionary<string, Int16> ClassList = GV.AIClassList.GetClassList();
                        string selectedClassName = cbRWaferClassList.SelectedItem?.ToString();
                        if (!string.IsNullOrEmpty(selectedClassName) && ClassList.ContainsKey(selectedClassName))
                        {
                            _AlignC.RHWaferAIClassId = ClassList[selectedClassName];
                        }
                    }
                }

                //   4. 儲存 UpBackAlign 模式（表示使用上對齊）
                _AlignC.UpBackAlign = 1;

                //   5. 儲存倍率設定
                if (rBLowMagnification.Checked)
                {
                    _AlignC.AlignLowMagnification = cbLowMagnification.SelectedIndex;
                }
                else if (rBHighMagnification.Checked)
                {
                    _AlignC.AlignHighMagnification = cbHighMagnification.SelectedIndex;
                    _AlignC.PatternShift = cbPatterhShift.SelectedIndex;
                }

                //   6. 停止執行緒和計時器
                isLive = false;

                if (locationUpdateTimer != null && locationUpdateTimer.Enabled)
                {
                    locationUpdateTimer.Stop();
                }

                //   7. 儲存 Recipe 到 XML
                _AlignC.LastModifyTime = DateTime.Now;
                GM.WriteRecipeXml(EditRecipe);

                GM.WriteToStatusTextBox1(GV.UserLevel == GV.User.Administrator ? 1 : 0,
                    $"已自動儲存上對齊頁面設定到 Recipe {EditRecipe}");
            }
            catch (Exception ex)
            {
                GM.WriteToStatusTextBox($"OnPageLeave 儲存設定時發生錯誤: {ex.Message}");
                Debug.WriteLine($"OnPageLeave Error: {ex.Message}\n{ex.StackTrace}");
            }
        }

        private void SafeBeginInvoke(Action action)
        {
            if (IsDisposed || Disposing || !IsHandleCreated) return;
            if (InvokeRequired)
            {
                try
                {
                    BeginInvoke((MethodInvoker)(() =>
                    {
                        if (!IsDisposed && !Disposing && IsHandleCreated)
                            action();
                    }));
                }
                catch (InvalidOperationException)
                {

                }
                return;
            }
            action();
        }
    }
}
