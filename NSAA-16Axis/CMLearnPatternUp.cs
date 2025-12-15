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
        public CMLearnPatternUp()
        {
            InitializeComponent();
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
        }

        private void CMLearnPattern_Load(object sender, EventArgs e)
        {
            if (GV.AppSettingParm.DebugMode)
            {
                lbDebugMsg.Visible = true;
            }

            EditRecipe = GV.NowRecipeNumber;

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
                cbRMaskAlgorithm.Items.Add(GV.Dlang.strTemplate);
                cbRMaskAlgorithm.Items.Add(GV.Dlang.strEdge);
                _recipe = GV._recipe;
                _AlignC = _recipe.AlignC;
                cbPatterhShift.SelectedIndex = 4;
                cbLWaferAlgorithm.SelectedIndex = 0;
                cbRWaferAlgorithm.SelectedIndex = 0;
                cbLMaskAlgorithm.SelectedIndex = 0;
                cbRMaskAlgorithm.SelectedIndex = 0;
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
        }

        public void DrawMatchPosition()
        {

            while (!GV.AppEnding)
            {
                while (GV.TabOption == GV.Tab.Learn)
                {
                    try 
                    { 
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
                                if(_AlignC.LLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && cbLMaskClassList.SelectedIndex >= 0)
                                {
                                    lMaskClassId = cbLMaskClassList.SelectedIndex;
                                }
                                if(_AlignC.RLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && cbRMaskClassList.SelectedIndex >= 0)
                                {
                                    rMaskClassId = cbRMaskClassList.SelectedIndex;
                                }
                            });
                            if (_AlignC.LLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
                            {
                                GV.matcherLLM.MatMatchWithAlgo(0, GV.LeftUpCam.Grab(), ref LMaskMp, _AlignC.LLMaskAlgorithm, lMaskClassId);
                            }
                            else
                            {
                                GV.matcherLLM.MatMatchWithAlgo(0, GV.LeftUpCam.Grab(), ref LMaskMp, _AlignC.LLMaskAlgorithm);
                            }

                            if (_AlignC.LLWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
                            {
                                GV.matcherLLW.MatMatchWithAlgo(0, GV.LeftUpCam.Grab(), ref LWaferMp, _AlignC.LLWaferAlgorithm, lWaferClassId);
                            }
                            else
                            {
                                GV.matcherLLW.MatMatchWithAlgo(0, GV.LeftUpCam.Grab(), ref LWaferMp, _AlignC.LLWaferAlgorithm);
                            }

                            if (_AlignC.RLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
                            {
                                GV.matcherRLM.MatMatchWithAlgo(0, GV.RightUpCam.Grab(), ref RMaskMp, _AlignC.RLMaskAlgorithm, rMaskClassId);
                            }
                            else
                            {
                                GV.matcherRLM.MatMatchWithAlgo(0, GV.RightUpCam.Grab(), ref RMaskMp, _AlignC.RLMaskAlgorithm);
                            }

                            if (_AlignC.RLWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
                            {
                                GV.matcherRLW.MatMatchWithAlgo(0, GV.RightUpCam.Grab(), ref RWaferMp, _AlignC.RLWaferAlgorithm, rWaferClassId);
                            }
                            else
                            {
                                GV.matcherRLW.MatMatchWithAlgo(0, GV.RightUpCam.Grab(), ref RWaferMp, _AlignC.RLWaferAlgorithm);
                            }
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
                                if(_AlignC.LHMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && cbLMaskClassList.SelectedIndex >= 0)
                                {
                                    lMaskClassId = cbLMaskClassList.SelectedIndex;
                                }
                                if(_AlignC.RHMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch && cbRMaskClassList.SelectedIndex >= 0)
                                {
                                    rMaskClassId = cbRMaskClassList.SelectedIndex;
                                }
                            });
                            if (_AlignC.LHMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
                            {
                                GV.matcherLHM.MatMatchWithAlgo(0, GV.LeftUpCam.Grab(), ref LMaskMp, _AlignC.LHMaskAlgorithm, lMaskClassId);
                            }
                            else
                            {
                                GV.matcherLHM.MatMatchWithAlgo(0, GV.LeftUpCam.Grab(), ref LMaskMp, _AlignC.LHMaskAlgorithm);
                            }

                            if (_AlignC.LHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
                            {
                                GV.matcherLHW.MatMatchWithAlgo(0, GV.LeftUpCam.Grab(), ref LWaferMp, _AlignC.LHWaferAlgorithm, lWaferClassId);
                            }
                            else
                            {
                                GV.matcherLHW.MatMatchWithAlgo(0, GV.LeftUpCam.Grab(), ref LWaferMp, _AlignC.LHWaferAlgorithm);
                            }

                            if (_AlignC.RHMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
                            {
                                GV.matcherRHM.MatMatchWithAlgo(0, GV.RightUpCam.Grab(), ref RMaskMp, _AlignC.RHMaskAlgorithm, rMaskClassId);
                            }
                            else
                            {
                                GV.matcherRHM.MatMatchWithAlgo(0, GV.RightUpCam.Grab(), ref RMaskMp, _AlignC.RHMaskAlgorithm);
                            }

                            if (_AlignC.RHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
                            {
                                GV.matcherRHW.MatMatchWithAlgo(0, GV.RightUpCam.Grab(), ref RWaferMp, _AlignC.RHWaferAlgorithm, rWaferClassId);
                            }
                            else
                            {
                                GV.matcherRHW.MatMatchWithAlgo(0, GV.RightUpCam.Grab(), ref RWaferMp, _AlignC.RHWaferAlgorithm);
                            }
                        }

                        Invoke((MethodInvoker)delegate ()
                        {
                            double dLX = skLPattern.MaskMp.X - skLPattern.WaferMp.X;
                            double dLY = skLPattern.MaskMp.Y - skLPattern.WaferMp.Y;
                            double dRX = skRPattern.MaskMp.X - skRPattern.WaferMp.X;
                            double dRY = skRPattern.MaskMp.Y - skRPattern.WaferMp.Y;

                            string msgLM = string.Format("LMX = {0:N3}, LMY = {1:N3}, dX = {2:N3}, dY = {3:N3}, MScore = {4:N2}",
                                skLPattern.MaskMp.X, skLPattern.MaskMp.Y, dLX, dLY, skLPattern.MaskMp.Score);
                            string msgLW = string.Format("LWX = {0:N3}, LWY = {1:N3}, WScore = {2:N2}",
                                skLPattern.WaferMp.X, skLPattern.WaferMp.Y, skLPattern.WaferMp.Score);
                            string msgRM = string.Format("RMX = {0:N3}, RMY = {1:N3}, dX = {2:N3}, dY = {3:N3}, MScore = {4:N2}",
                                skRPattern.MaskMp.X, skRPattern.MaskMp.Y, dRX, dRY, skRPattern.MaskMp.Score);
                            string msgRW = string.Format("RWX = {0:N3}, RWY = {1:N3}, WScore = {2:N2}",
                                skRPattern.WaferMp.X, skRPattern.WaferMp.Y, skRPattern.WaferMp.Score);

                            lbLMsgM.Text = msgLM;
                            lbRMsgM.Text = msgRM;
                            lbLMsgW.Text = msgLW;
                            lbRMsgW.Text = msgRW;

                            Update();
                        });
                    }
                    catch(Exception ex)
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

        public void UpdateUI()
        {
            if ((GV.NowRecipeNumber < 1) || (GV.NowRecipeNumber > 100))
            {
                GV.NowRecipeNumber = 1;

            }

            EditRecipe = GV.NowRecipeNumber;

            ChangeRecipe();

            lbRecipeNumber.Text = GV.Dlang.strRecipeNumber + EditRecipe.ToString();

            cbLowMagnification.SelectedIndex = _AlignC.AlignLowMagnification;
            cbHighMagnification.SelectedIndex = _AlignC.AlignHighMagnification;
            cbPatterhShift.SelectedIndex = _AlignC.PatternShift;

            if (GV.AppSettingParm.DebugMode)
            {
                lbDebugMsg.Text = "UpdateUI = " + _AlignC.AlignLowMagnification.ToString() + " EditRecipe = " + EditRecipe.ToString();
            }

            pbLMaskImage.Image = null;
            pbLWaferImage.Image = null;
            pbRMaskImage.Image = null;
            pbRMaskImage.Image = null;
            pbRWaferImage.Image = null;

            if (rBLowMagnification.Checked)
            {
                if (_recipe.LeftLowMaskMat != null && !_recipe.LeftLowMaskMat.Empty() &&_recipe.LeftLowMaskMat.Width > 0 && _recipe.LeftLowMaskMat.Height > 0)
                {
                    try
                    {
                        pbLMaskImage.Image?.Dispose(); // 先釋放舊的 Image
                        pbLMaskImage.Image = _recipe.LeftLowMaskMat.ToBitmap();
                    }
                    catch (Exception ex)
                    {
                        if (GV.AppSettingParm.DebugMode)
                        {
                            lbDebugMsg.Text = "LLMaskMat ToBitmap Error: " + ex.Message;
                        }
                        pbLMaskImage.Image = null;
                    }
                }
                else
                {
                    pbLMaskImage.Image = null;
                }

                // 對其他圖像做同樣處理
                if (_recipe.LeftLowWaferMat != null && !_recipe.LeftLowWaferMat.Empty() &&
                    _recipe.LeftLowWaferMat.Width > 0 && _recipe.LeftLowWaferMat.Height > 0)
                {
                    try
                    {
                        pbLWaferImage.Image?.Dispose();
                        pbLWaferImage.Image = _recipe.LeftLowWaferMat.ToBitmap();
                    }
                    catch (Exception ex)
                    {
                        if (GV.AppSettingParm.DebugMode)
                        {
                            lbDebugMsg.Text = "LLWaferMat ToBitmap Error: " + ex.Message;
                        }
                        pbLWaferImage.Image = null;
                    }
                }
                else
                {
                    pbLWaferImage.Image = null;
                }
                //if (_recipe.RightLowMaskMat != null)
                //    pbRMaskImage.Image = _recipe.RightLowMaskMat.ToBitmap();
                if (_recipe.RightLowMaskMat != null && !_recipe.RightLowMaskMat.Empty() &&_recipe.RightLowMaskMat.Width > 0 && _recipe.RightLowMaskMat.Height > 0)
                {
                    try
                    {
                        pbRMaskImage.Image?.Dispose(); // 先釋放舊的 Image
                        pbRMaskImage.Image = _recipe.RightLowMaskMat.ToBitmap();
                    }
                    catch (Exception ex)
                    {
                        if (GV.AppSettingParm.DebugMode)
                        {
                            lbDebugMsg.Text = "RLMaskMat ToBitmap Error: " + ex.Message;
                        }
                        pbRMaskImage.Image = null;
                    }
                }
                else
                {
                    pbRMaskImage.Image = null;
                }
                //if (_recipe.RightLowWaferMat != null)
                //    pbRWaferImage.Image = _recipe.RightLowWaferMat.ToBitmap();
                if (_recipe.RightLowWaferMat != null && !_recipe.RightLowWaferMat.Empty() && _recipe.RightLowWaferMat.Width > 0 && _recipe.RightLowWaferMat.Height > 0)
                {
                    try
                    {
                        pbRWaferImage.Image?.Dispose();
                        pbRWaferImage.Image = _recipe.RightLowWaferMat.ToBitmap();
                    }
                    catch (Exception ex)
                    {
                        if (GV.AppSettingParm.DebugMode)
                        {
                            lbDebugMsg.Text = "RLWaferMat ToBitmap Error: " + ex.Message;
                        }
                        pbRWaferImage.Image = null;
                    }
                }
                else
                {
                    pbRWaferImage.Image = null;
                }
                cbLMaskAlgorithm.SelectedIndex = (int)_AlignC.LLMaskAlgorithm;
                cbLWaferAlgorithm.SelectedIndex = (int)_AlignC.LLWaferAlgorithm;
                cbRMaskAlgorithm.SelectedIndex = (int)_AlignC.RLMaskAlgorithm;
                cbRWaferAlgorithm.SelectedIndex = (int)_AlignC.RLWaferAlgorithm;

                if(_AlignC.LLWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
                {
                    cbLWaferClassList.SelectedIndex=_AlignC.LLWaferAIClassId;
                }
                if(_AlignC.RLWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
                {
                    cbRWaferClassList.SelectedIndex=_AlignC.RLWaferAIClassId;
                }
                if(_AlignC.LLMaskAlgorithm== OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
                {
                    cbLMaskClassList.SelectedIndex=_AlignC.LLMaskAIClassId;
                }
                if(_AlignC.RLMaskAlgorithm== OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
                {
                    cbRMaskClassList.SelectedIndex=_AlignC.RLMaskAIClassId;
                }
                btMagSaveLow.Enabled = true;
                btMagSaveHigh.Enabled = false;

                
            }

            if (rBHighMagnification.Checked)
            {
                if (_recipe.LeftHighMaskMat != null)
                    pbLMaskImage.Image = _recipe.LeftHighMaskMat.ToBitmap();
                if (_recipe.LeftHighWaferMat != null)
                    pbLWaferImage.Image = _recipe.LeftHighWaferMat.ToBitmap();
                if (_recipe.RightHighMaskMat != null)
                    pbRMaskImage.Image = _recipe.RightHighMaskMat.ToBitmap();
                if (_recipe.RightHighWaferMat != null)
                    pbRWaferImage.Image = _recipe.RightHighWaferMat.ToBitmap();
                cbLMaskAlgorithm.SelectedIndex = (int)_AlignC.LHMaskAlgorithm;
                cbLWaferAlgorithm.SelectedIndex = (int)_AlignC.LHWaferAlgorithm;
                cbRMaskAlgorithm.SelectedIndex = (int)_AlignC.RHMaskAlgorithm;
                cbRWaferAlgorithm.SelectedIndex = (int)_AlignC.RHWaferAlgorithm;

                if(_AlignC.LHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
                {
                    cbLWaferClassList.SelectedIndex=_AlignC.LHWaferAIClassId;
                }
                if(_AlignC.RHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
                {
                    cbRWaferClassList.SelectedIndex=_AlignC.RHWaferAIClassId;
                }
                if(_AlignC.LHMaskAlgorithm== OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
                {
                    cbLMaskClassList.SelectedIndex=_AlignC.LHMaskAIClassId;
                }
                if(_AlignC.RHMaskAlgorithm== OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
                {
                    cbRMaskClassList.SelectedIndex=_AlignC.RHMaskAIClassId;
                }
                btMagSaveLow.Enabled = false;
                btMagSaveHigh.Enabled = true;
            }

            if ((LCoaLight.Enabled) && (LCoaLight.Checked))
            {
                btLCoaLightMinus.Enabled = true;
                btLCoaLightPlus.Enabled = true;
                tBLeftCoaLight.Enabled = true;
            }
            else
            {
                btLCoaLightMinus.Enabled = false;
                btLCoaLightPlus.Enabled = false;
                tBLeftCoaLight.Enabled = false;
            }

            if ((RCoaLight.Enabled) && (RCoaLight.Checked))
            {
                btRCoaLightMinus.Enabled = true;
                btRCoaLightPlus.Enabled = true;
                tBRightCoaLight.Enabled = true;
            }
            else
            {
                btRCoaLightMinus.Enabled = false;
                btRCoaLightPlus.Enabled = false;
                tBRightCoaLight.Enabled = false;
            }

            if ((LRingLight.Enabled) && (LRingLight.Checked))
            {
                btLRingLightMinus.Enabled = true;
                btLRingLightPlus.Enabled = true;
                tBLeftRingLight.Enabled = true;
            }
            else
            {
                btLRingLightMinus.Enabled = false;
                btLRingLightPlus.Enabled = false;
                tBLeftRingLight.Enabled = false;
            }

            if ((RRingLight.Enabled) && (RRingLight.Checked))
            {
                btRRingLightMinus.Enabled = true;
                btRRingLightPlus.Enabled = true;
                tBRightRingLight.Enabled = true;
            }
            else
            {
                btRRingLightMinus.Enabled = false;
                btRRingLightPlus.Enabled = false;
                tBRightRingLight.Enabled = false;
            }

            if (LCoaLight.Checked)
            {
                GV.Light.ChangeBrightness("left", tBLeftCoaLight.Value);
            }
            else
            {
                GV.Light.ChangeBrightness("left", 0);
            }

            if (RCoaLight.Checked)
            {
                GV.Light.ChangeBrightness("right", tBRightCoaLight.Value);
            }
            else
            {
                GV.Light.ChangeBrightness("right", 0);
            }

            if (LRingLight.Checked)
            {
                GV.RingLight.ChangeBrightness("left", tBLeftRingLight.Value);
            }
            else
            {
                GV.RingLight.ChangeBrightness("left", 0);
            }

            if (RRingLight.Checked)
            {
                GV.RingLight.ChangeBrightness("right", tBRightRingLight.Value);
            }
            else
            {
                GV.RingLight.ChangeBrightness("right", 0);
            }

            Update();
        }

        private void ChangeRecipe()
        {
            _recipe = GV._recipe;
            _AlignC = _recipe.AlignC;

            //_AlignC = GV.acar[EditRecipe];

            rBLowMagnification.Checked = true;
            rBHighMagnification.Checked = false;

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
        }

        public void ShowTime(string NowTime)
        {
            Invoke((MethodInvoker)delegate ()
            {
                lbNowTime.Text = NowTime;
            });
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
            Mat fullImage = GV.LeftUpCam.Grab();
            if (fullImage == null || fullImage.Empty())
            {
                MessageBox.Show("左側相機未取得影像，請確認相機連線狀態", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 取得 ROI 並檢查是否有效
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

            // 建立 ROI Mat
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
                    if (cbLWaferAlgorithm.SelectedIndex == 2)
                    {
                        cbLWaferClassList.Visible = true;
                        btLabelPatternL.Visible = true;
                        pbLWaferImage.Visible = false;
                    }
                    return;
                }

                GV.Plc.GetRecipe();
                GV.Plc.GetLocation();

                _AlignC.LRingLight[NowMageni] = LRingLight.Checked;
                _AlignC.LeftRingBrightness[NowMageni] = tBLeftRingLight.Value;
                _AlignC.LeftLight[NowMageni] = LCoaLight.Checked;
                _AlignC.LeftBrightness[NowMageni] = tBLeftCoaLight.Value;
                _recipe.SetLeftLowWaferMat(m);
                pbLWaferImage.Image = m.ToBitmap();
                GrayImage d = new GrayImage(m.Width, m.Height);
                d.Fill(255);
                _recipe.LeftLowWaferMask = new GrayImage(d);
                _AlignC.LowMagnificationLZ = GV.NowLocation[GV.Plc.iiDSUpLeftZ];
                
                _AlignC.LLWaferAlgorithm = Algoritm(cbLWaferAlgorithm.SelectedIndex);
                if (cbLWaferAlgorithm.SelectedIndex == 2)
                {
                    _AlignC.LLWaferAIClassId=cbLWaferClassList.SelectedIndex;
                    _AlignC.LLBitwiseNot = cbLWaferClassList.SelectedIndex;
                }
                else
                {
                    _AlignC.LLWaferAIClassId = -1;
                    _AlignC.LLBitwiseNot = 0;
                }
                GV.matcherLLW.LearnWithAlgo(m, d, _AlignC.LLWaferAlgorithm);
            }
            else if (rBHighMagnification.Checked)
            {
                if (MessageBox.Show(GV.Dlang.strbtnSaveLHW, GV.Dlang.strSureSaveLHW, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    if (cbLWaferAlgorithm.SelectedIndex == 2)
                    {
                        cbLWaferClassList.Visible = true;
                        btLabelPatternL.Visible = true;
                        pbLWaferImage.Visible = false;
                    }
                    return;
                }
                GV.Plc.GetRecipe();
                GV.Plc.GetLocation();

                _AlignC.LRingLight[NowMageni] = LRingLight.Checked;
                _AlignC.LeftRingBrightness[NowMageni] = tBLeftRingLight.Value;
                _AlignC.LeftLight[NowMageni] = LCoaLight.Checked;
                _recipe.SetLeftHighWaferMat(m);
                pbLWaferImage.Image = m.ToBitmap();
                GrayImage d = new GrayImage(m.Width, m.Height);
                d.Fill(255);
                _recipe.LeftHighWaferMask = new GrayImage(d);
                _AlignC.HighMagnificationLZ = GV.NowLocation[GV.Plc.iiDSUpLeftZ];
                _AlignC.LHWaferAlgorithm = Algoritm(cbLWaferAlgorithm.SelectedIndex);
                if (cbLWaferAlgorithm.SelectedIndex == 2)
                {
                    _AlignC.LHWaferAIClassId = cbLWaferClassList.SelectedIndex;
                    _AlignC.LHBitwiseNot = cbLWaferClassList.SelectedIndex;
                }
                else
                {
                    _AlignC.LHWaferAIClassId = -1;
                    _AlignC.LHBitwiseNot = 0;
                }
                GV.matcherLHW.LearnWithAlgo(m, d, _AlignC.LHWaferAlgorithm);
            }
            else
            {
                MessageBox.Show("Select magnification first.", "LWaferTemplateSave", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _AlignC.LastModifyTime = DateTime.Now;
            GM.WriteRecipeXml(EditRecipe);
            if (cbLWaferAlgorithm.SelectedIndex == 2)
            {
                try
                {
                    string trainPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Python", "Train", GV._recipe.RecipeName);
                    DateTime now = DateTime.Now;
                    Directory.CreateDirectory(trainPath);
                    string fn = string.Concat(trainPath, "\\LW", now.ToString("HH_mm_ss"));
                    Cv2.ImWrite(string.Concat(fn, ".bmp"), fullImage);
                    var labelPath = string.Concat(fn, ".txt");

                    float xCenter = (roi.X + roi.Width / 2f) / fullImage.Width;
                    float yCenter = (roi.Y + roi.Height / 2f) / fullImage.Height;
                    float w = roi.Width / (float)fullImage.Width;
                    float h = roi.Height / (float)fullImage.Height;

                    int classId = cbLWaferClassList.SelectedIndex;
                    List<YoloBox> boxes = new List<YoloBox>();
                    boxes.Add(new YoloBox { ClassId = classId, X = xCenter, Y = yCenter, W = w, H = h });
                    File.WriteAllLines(labelPath, boxes.Select(b => $"{b.ClassId} {b.X:F6} {b.Y:F6} {b.W:F6} {b.H:F6}"));
                }
                catch
                {
                }
                //cbLWaferClassList.Visible = true;
                //btLabelPatternL.Visible = true;
                //pbLWaferImage.Visible = false;
            }
        }

        internal void ShowMe()
        {
            if (GV.AppSettingParm.Author == 0)
            {
                btTfile.Visible = false;    
                btTLRead.Visible = false;
                btTRRead.Visible = false;
            }
            else
            {
                btTfile.Visible = true;
                btTLRead.Visible = true;
                btTRRead.Visible = true;
                groupBox4.Visible = true;
            }
            if (GMPLeft == null)
            {
                GMPLeft = new Mat();
                GMPRight = new Mat();
                cbRMaskClassList.Items.Clear();
                cbRWaferClassList.Items.Clear();
                cbLWaferClassList.Items.Clear();
                cbLMaskClassList.Items.Clear();
                Dictionary<string, Int16> ClassList = GV.AIClassList.GetClassList();
                foreach (var className in ClassList)
                {
                    cbRMaskClassList.Items.Add(className.Key);
                    cbRWaferClassList.Items.Add(className.Key);
                    cbLWaferClassList.Items.Add(className.Key);
                    cbLMaskClassList.Items.Add(className.Key);
                }
            }
            if (_recipe.LeftLowMaskMat != null && !_recipe.LeftLowMaskMat.Empty())
            {
                GV.matcherLLM.LearnWithAlgo(_recipe.LeftLowMaskMat, _recipe.LeftLowMaskMask, _AlignC.LLMaskAlgorithm);
            }
            if (_recipe.RightLowMaskMat != null && !_recipe.RightLowMaskMat.Empty())
            {
                GV.matcherRLM.LearnWithAlgo(_recipe.RightLowMaskMat, _recipe.RightLowMaskMask, _AlignC.RLMaskAlgorithm);
            }
            if (_recipe.LeftLowWaferMat != null && !_recipe.LeftLowWaferMat.Empty())
            {
                GV.matcherLLW.LearnWithAlgo(_recipe.LeftLowWaferMat, _recipe.LeftLowWaferMask, _AlignC.LLWaferAlgorithm);
            }
            if (_recipe.RightLowWaferMat != null && !_recipe.RightLowWaferMat.Empty())
            {
                GV.matcherRLW.LearnWithAlgo(_recipe.RightLowWaferMat, _recipe.RightLowWaferMask, _AlignC.RLWaferAlgorithm);
            }
            if (_recipe.LeftHighMaskMat != null && !_recipe.LeftHighMaskMat.Empty())
            {
                GV.matcherLHM.LearnWithAlgo(_recipe.LeftHighMaskMat, _recipe.LeftHighMaskMask, _AlignC.LHMaskAlgorithm);
            }
            if (_recipe.RightHighMaskMat != null && !_recipe.RightHighMaskMat.Empty())
            {
                GV.matcherRHM.LearnWithAlgo(_recipe.RightHighMaskMat, _recipe.RightHighMaskMask, _AlignC.RHMaskAlgorithm);
            }
            if (_recipe.LeftHighWaferMat != null && !_recipe.LeftHighWaferMat.Empty())
            {
                GV.matcherLHW.LearnWithAlgo(_recipe.LeftHighWaferMat, _recipe.LeftHighWaferMask, _AlignC.LHWaferAlgorithm);
            }
            if (_recipe.RightHighWaferMat != null && !_recipe.RightHighWaferMat.Empty())
            {
                GV.matcherRHW.LearnWithAlgo(_recipe.RightHighWaferMat, _recipe.RightHighWaferMask, _AlignC.RHWaferAlgorithm);
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
                MatchThread = new Thread(DrawMatchPosition);
                MatchThread.Start();
            }

            Invoke((MethodInvoker)delegate ()
            {
                CbHLMagni_Change(NowMageni);

                Changelight();

                CheckParam();

                if (GV.UserLevel != GV.User.Operator)
                {
                    nUDXyyX.Enabled = true;
                    nUDXyyY1.Enabled = true;
                    nUDXyyY2.Enabled = true;
                    nuDRotate.Enabled = true;
                }

                Update();
            });
        }

        private void BtLMaskTemplateSave_Click(object sender, EventArgs e)
        {
            GV.TickCount = 0;

            Mat fullImage = GV.LeftUpCam.Grab();
            if (fullImage == null || fullImage.Empty())
            {
                MessageBox.Show("相機未取得影像，請確認相機連線狀態", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 取得 ROI 並檢查是否有效
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

            // 建立 ROI Mat
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
                // MessageBox1 msg1 = new MessageBox1(GV.Dlang.strbtnSaveLLM, GV.Dlang.strSureSaveLLM, MessageBoxIcon.Question);
                // DialogResult m1 = msg1.ShowDialog();
                // if (m1 != DialogResult.OK)
                if (MessageBox.Show(GV.Dlang.strbtnSaveLLM, GV.Dlang.strSureSaveLLM, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    if (cbLMaskAlgorithm.SelectedIndex == 2)
                    {
                        cbLMaskClassList.Visible = true;
                        btLabelPatternLM.Visible = false;
                        pbLMaskImage.Visible = false;
                    }
                    return;
                }

                GV.Plc.GetRecipe();
                GV.Plc.GetLocation();

                _AlignC.LRingLight[NowMageni] = LRingLight.Checked;
                _AlignC.LeftRingBrightness[NowMageni] = tBLeftRingLight.Value;
                _AlignC.LeftLight[NowMageni] = LCoaLight.Checked;
                _AlignC.LeftBrightness[NowMageni] = tBLeftCoaLight.Value;

                //GV.LeftLowMaskMat[EditRecipe] = m;
                //CvInvoke.Imwrite(GetTemplateFileName("LLM"), m);
                _recipe.SetLeftLowMaskMat(m);
                pbLMaskImage.Image = m.ToBitmap();
                GrayImage d = new GrayImage(m.Width, m.Height);
                d.Fill(255);
                //GV.LeftLowMaskMask[EditRecipe] = d;
                //d.Save(GetTemplateFileName("MLLM"), System.Drawing.Imaging.ImageFormat.Bmp);
                _recipe.LeftLowMaskMask = new GrayImage(d);
                _AlignC.LowMagnificationLZ = GV.NowLocation[GV.Plc.iiDSUpLeftZ];
                _AlignC.LLMaskAlgorithm = Algoritm(cbLMaskAlgorithm.SelectedIndex);
                if (cbLMaskAlgorithm.SelectedIndex == 2)
                {
                    _AlignC.LLMaskAIClassId = cbLMaskClassList.SelectedIndex;

                }
                else
                {
                    _AlignC.LLMaskAIClassId = -1;
                }
                GV.matcherLLM.LearnWithAlgo(m, d, _AlignC.LLMaskAlgorithm);
            }
            else if (rBHighMagnification.Checked)
            {
                if (MessageBox.Show(GV.Dlang.strbtnSaveLHM, GV.Dlang.strSureSaveLHM, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    if (cbLMaskAlgorithm.SelectedIndex == 2)
                    {
                        cbLMaskClassList.Visible = true;
                        btLabelPatternLM.Visible = false;
                        pbLMaskImage.Visible = false;
                    }
                    return;
                }

                GV.Plc.GetRecipe();
                GV.Plc.GetLocation();

                _AlignC.LRingLight[NowMageni] = LRingLight.Checked;
                _AlignC.LeftRingBrightness[NowMageni] = tBLeftRingLight.Value;
                _AlignC.LeftLight[NowMageni] = LCoaLight.Checked;
                _AlignC.LeftBrightness[NowMageni] = tBLeftCoaLight.Value;

                //GV.LeftHighMaskMat[EditRecipe] = m;               
                //CvInvoke.Imwrite(GetTemplateFileName("LHM"), m);
                _recipe.SetLeftHighMaskMat(m);
                pbLMaskImage.Image = m.ToBitmap();
                GrayImage d = new GrayImage(m.Width, m.Height);
                d.Fill(255);
                //GV.LeftHighMaskMask[EditRecipe] = d;
                //d.Save(GetTemplateFileName("MLHM"), System.Drawing.Imaging.ImageFormat.Bmp);
                _recipe.LeftHighMaskMask = new GrayImage(d);
                _AlignC.HighMagnificationLZ = GV.NowLocation[GV.Plc.iiDSUpLeftZ];
                _AlignC.LHMaskAlgorithm = Algoritm(cbLMaskAlgorithm.SelectedIndex);
                if (cbLMaskAlgorithm.SelectedIndex == 2)
                {
                    _AlignC.LHMaskAIClassId = cbLMaskClassList.SelectedIndex;

                }
                else
                {
                    _AlignC.LHMaskAIClassId = -1;
                }
                GV.matcherLHM.LearnWithAlgo(m, d, _AlignC.LHMaskAlgorithm);
            }
            else
            {
                MessageBox.Show("Select magnification first.", "LMaskTemplateSave", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _AlignC.LastModifyTime = DateTime.Now;
            GM.WriteRecipeXml(EditRecipe);
            if (cbLWaferAlgorithm.SelectedIndex == 2)
            {
                try
                {
                    string trainPath= Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Python", "Train", GV._recipe.RecipeName);
                    DateTime now = DateTime.Now;
                    Directory.CreateDirectory(trainPath);
                    string fn=string.Concat(trainPath, "\\LM", now.ToString("HH_mm_ss"));
                    Cv2.ImWrite(string.Concat(fn, ".bmp"), fullImage);
                    var labelPath = string.Concat(fn, ".txt");
                    float xCenter = (roi.X + roi.Width / 2f) / fullImage.Width;
                    float yCenter = (roi.Y + roi.Height / 2f) / fullImage.Height;
                    float w = roi.Width / (float)fullImage.Width;
                    float h = roi.Height / (float)fullImage.Height;
                    int classId = cbLMaskClassList.SelectedIndex;
                    List<YoloBox> boxes = new List<YoloBox>();
                    boxes.Add(new YoloBox { ClassId = classId, X = xCenter, Y = yCenter, W = w, H = h });
                    File.WriteAllLines(labelPath, boxes.Select(b => $"{b.ClassId} {b.X:F6} {b.Y:F6} {b.W:F6} {b.H:F6}"));
                }
                catch
                {

                }
            }
        }

        private void BtRWaferTempalteSave_Click(object sender, EventArgs e)
        {
            GV.TickCount = 0;
            Mat fullImage = GV.RightUpCam.Grab();
            if (fullImage == null || fullImage.Empty())
            {
                MessageBox.Show("右側相機未取得影像，請確認相機連線狀態", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 取得 ROI 並檢查是否有效
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

            // 建立 ROI Mat
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
                    if (cbRWaferAlgorithm.SelectedIndex == 2)
                    {
                        cbRWaferClassList.Visible = true;
                        btLabelPatternR.Visible = true;
                        pbRWaferImage.Visible = false;
                    }
                    return;
                }
                GV.Plc.GetRecipe();
                GV.Plc.GetLocation();

                _AlignC.RightLight[NowMageni] = RCoaLight.Checked;
                _AlignC.RightBrightness[NowMageni] = tBRightCoaLight.Value;
                _AlignC.RRingLight[NowMageni] = RRingLight.Checked;
                _AlignC.RightRingBrightness[NowMageni] = tBRightRingLight.Value;
                _recipe.SetRightLowWaferMat(m);
                pbRWaferImage.Image = m.ToBitmap();
                GrayImage d = new GrayImage(m.Width, m.Height);
                d.Fill(255);
                _recipe.RightLowWaferMask = new GrayImage(d);
                _AlignC.LowMagnificationRZ = GV.NowLocation[GV.Plc.iiDSUpRightZ];
                _AlignC.RLWaferAlgorithm = Algoritm(cbRWaferAlgorithm.SelectedIndex);
                if (cbRWaferAlgorithm.SelectedIndex == 2)
                {
                    _AlignC.RLWaferAIClassId = cbRWaferClassList.SelectedIndex;
                    _AlignC.RLBitwiseNot = cbRWaferClassList.SelectedIndex;
                }
                else
                {
                    _AlignC.RLWaferAIClassId = -1;
                    _AlignC.RLBitwiseNot = 0;
                }
                GV.matcherRLW.LearnWithAlgo(m, d, _AlignC.RLWaferAlgorithm);
            }
            else if (rBHighMagnification.Checked)
            {
                if (MessageBox.Show(GV.Dlang.strbtnSaveRHW, GV.Dlang.strSureSaveRHW, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    if (cbRWaferAlgorithm.SelectedIndex == 2)
                    {
                        cbRWaferClassList.Visible = true;
                        btLabelPatternR.Visible = true;
                        pbRWaferImage.Visible = false;
                    }
                    return;
                }
                GV.Plc.GetRecipe();
                GV.Plc.GetLocation();

                _AlignC.RightLight[NowMageni] = RCoaLight.Checked;
                _AlignC.RightBrightness[NowMageni] = tBRightCoaLight.Value;
                _AlignC.RRingLight[NowMageni] = RRingLight.Checked;
                _AlignC.RightRingBrightness[NowMageni] = tBRightRingLight.Value;

                _recipe.SetRightHighWaferMat(m);
                pbRWaferImage.Image = m.ToBitmap();
                GrayImage d = new GrayImage(m.Width, m.Height);
                d.Fill(255);

                _recipe.RightHighWaferMask = new GrayImage(d);
                _AlignC.HighMagnificationRZ = GV.NowLocation[GV.Plc.iiDSUpRightZ];
                _AlignC.RHWaferAlgorithm = Algoritm(cbRWaferAlgorithm.SelectedIndex);
                if (cbRWaferAlgorithm.SelectedIndex == 2)
                {
                    _AlignC.RHWaferAIClassId = cbRWaferClassList.SelectedIndex;
                    _AlignC.RHBitwiseNot = cbRWaferClassList.SelectedIndex;
                }
                else
                {
                    _AlignC.RHWaferAIClassId = -1;
                    _AlignC.RHBitwiseNot = 0;
                }
                GV.matcherRHW.LearnWithAlgo(m, d, _AlignC.RHWaferAlgorithm);
            }
            else
            {
                MessageBox.Show("Select magnification first.", "RWaferTemplateSave", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _AlignC.LastModifyTime = DateTime.Now;
            //GM.WriteAlignConditionsAcarToXml("AlignConditions.xml");
            GM.WriteRecipeXml(EditRecipe);
            if (cbRWaferAlgorithm.SelectedIndex == 2)
            {
                string trainPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Python", "Train", GV._recipe.RecipeName);
                DateTime now = DateTime.Now;
                Directory.CreateDirectory(trainPath);
                string fn = string.Concat(trainPath, "\\R", now.ToString("HH_mm_ss"));
                Cv2.ImWrite(string.Concat(fn, ".bmp"), fullImage);
                var labelPath = string.Concat(fn, ".txt");

                float xCenter = (roi.X + roi.Width / 2f) / fullImage.Width;
                float yCenter = (roi.X + roi.Height / 2f) / fullImage.Height;
                float w = roi.Width / (float)fullImage.Width;
                float h = roi.Height / (float)fullImage.Height;

                int classId = cbRWaferClassList.SelectedIndex;
                List<YoloBox> boxes = new List<YoloBox>();
                boxes.Add(new YoloBox { ClassId = classId, X = xCenter, Y = yCenter, W = w, H = h });
                File.WriteAllLines(labelPath, boxes.Select(b => $"{b.ClassId} {b.X:F6} {b.Y:F6} {b.W:F6} {b.H:F6}"));
                //cbRWaferClassList.Visible = true;
                //btLabelPatternR.Visible = true;
                //pbLWaferImage.Visible = false;
            }
        }

        private void BtRMaskTemplateSave_Click(object sender, EventArgs e)
        {
            GV.TickCount = 0;
            Mat fullImage = GV.RightUpCam.Grab();
            if (fullImage == null || fullImage.Empty())
            {
                MessageBox.Show("右側相機未取得影像，請確認相機連線狀態", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 取得 ROI 並檢查是否有效
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

            // 建立 ROI Mat
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
                    if (cbRMaskAlgorithm.SelectedIndex == 2)
                    {
                        cbRMaskClassList.Visible = true;
                        btLabelPatternRM.Visible = false;
                        pbRMaskImage.Visible = false;
                    }
                    return;
                }

                GV.Plc.GetRecipe();
                GV.Plc.GetLocation();

                _AlignC.RightLight[NowMageni] = RCoaLight.Checked;
                _AlignC.RightBrightness[NowMageni] = tBRightCoaLight.Value;
                _AlignC.RRingLight[NowMageni] = RRingLight.Checked;
                _AlignC.RightRingBrightness[NowMageni] = tBRightRingLight.Value;
                _recipe.SetRightLowMaskMat(m);
                pbRMaskImage.Image = m.ToBitmap();
                GrayImage d = new GrayImage(m.Width, m.Height);
                d.Fill(255);
                _recipe.RightLowMaskMask = new GrayImage(d);
                _AlignC.LowMagnificationRZ = GV.NowLocation[GV.Plc.iiDSUpRightZ];
                _AlignC.RLMaskAlgorithm = Algoritm(cbRMaskAlgorithm.SelectedIndex);
                if (cbRMaskAlgorithm.SelectedIndex == 2)
                {
                    _AlignC.RLMaskAIClassId = cbRMaskClassList.SelectedIndex;
                }
                else
                {
                    _AlignC.RLMaskAIClassId = -1;
                }
                GV.matcherRLM.LearnWithAlgo(m, d, _AlignC.RLMaskAlgorithm);
            }
            else if (rBHighMagnification.Checked)
            {
                if (MessageBox.Show(GV.Dlang.strbtnSaveRHM, GV.Dlang.strSureSaveRHM, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    if( cbRMaskAlgorithm.SelectedIndex == 2)
                    {
                        cbRMaskClassList.Visible = true;
                        btLabelPatternRM.Visible = false;
                        pbRMaskImage.Visible = false;
                    }
                    return;
                }

                GV.Plc.GetRecipe();
                GV.Plc.GetLocation();

                _AlignC.RightLight[NowMageni] = RCoaLight.Checked;
                _AlignC.RightBrightness[NowMageni] = tBRightCoaLight.Value;
                _AlignC.RRingLight[NowMageni] = RRingLight.Checked;
                _AlignC.RightRingBrightness[NowMageni] = tBRightRingLight.Value;
                _recipe.SetRightHighMaskMat(m);
                pbRMaskImage.Image = m.ToBitmap();
                GrayImage d = new GrayImage(m.Width, m.Height);
                d.Fill(255);
                _recipe.RightHighMaskMask = new GrayImage(d);
                _AlignC.HighMagnificationRZ = GV.NowLocation[GV.Plc.iiDSUpRightZ];
                _AlignC.RHMaskAlgorithm = Algoritm(cbRMaskAlgorithm.SelectedIndex);
                if (cbRMaskAlgorithm.SelectedIndex == 2)
                {
                    _AlignC.RHMaskAIClassId = cbRMaskClassList.SelectedIndex;
                }
                else
                {
                    _AlignC.RHMaskAIClassId = -1;
                }
                GV.matcherRHM.LearnWithAlgo(m, d, _AlignC.RHMaskAlgorithm);
            }
            else
            {
                MessageBox.Show("Select magnification first.", "RMaskTemplateSave", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _AlignC.LastModifyTime = DateTime.Now;
            //GM.WriteAlignConditionsAcarToXml("AlignConditions.xml");
            GM.WriteRecipeXml(EditRecipe);
            if(cbRWaferAlgorithm.SelectedIndex == 2)
            {
                try
                {
                    string trainPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Python", "Train", GV._recipe.RecipeName);
                    DateTime now = DateTime.Now;
                    Directory.CreateDirectory(trainPath);
                    string fn = string.Concat(trainPath, "\\RM", now.ToString("HH_mm_ss"));
                    Cv2.ImWrite(string.Concat(fn, ".bmp"), fullImage);
                    var labelPath = string.Concat(fn, ".txt");
                    float xCenter = (roi.X + roi.Width / 2f) / fullImage.Width;
                    float yCenter = (roi.Y + roi.Height / 2f) / fullImage.Height;
                    float w = roi.Width / (float)fullImage.Width;
                    float h = roi.Height / (float)fullImage.Height;
                    int classId = cbRMaskClassList.SelectedIndex;
                    List<YoloBox> boxes = new List<YoloBox>();
                    boxes.Add(new YoloBox { ClassId = classId, X = xCenter, Y = yCenter, W = w, H = h });
                    File.WriteAllLines(labelPath, boxes.Select(b => $"{b.ClassId} {b.X:F6} {b.Y:F6} {b.W:F6} {b.H:F6}"));
                }
                catch
                {
                }
            }
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
            if(rBLowMagnification.Checked)
            {
                _AlignC.LLMaskAlgorithm = Algoritm(cbLMaskAlgorithm.SelectedIndex);
                if(_recipe.LeftLowMaskMat !=null && !_recipe.LeftLowMaskMat.Empty())
                {
                    GV.matcherLLM.LearnWithAlgo(_recipe.LeftLowMaskMat, _recipe.LeftLowMaskMask, _AlignC.LLMaskAlgorithm);
                }
            }
            else if (rBHighMagnification.Checked)
            {
                _AlignC.LHMaskAlgorithm = Algoritm(cbLMaskAlgorithm.SelectedIndex);
                if(_recipe.LeftHighMaskMat !=null && !_recipe.LeftHighMaskMat.Empty())
                {
                    GV.matcherLHM.LearnWithAlgo(_recipe.LeftHighMaskMat, _recipe.LeftHighMaskMask, _AlignC.LHMaskAlgorithm);
                }
            }
            if (cbLMaskAlgorithm.SelectedIndex == 2)
            {
                
                cbLMaskClassList.Visible = true;
                btLabelPatternLM.Visible = false;
                pbLMaskImage.Visible = false;
            }
            else
            {
                cbLMaskClassList.Visible = false;
                btLabelPatternLM.Visible = false;
                pbLMaskImage.Visible = true;
            }
            CheckParam();
        }

        private void CbRWaferAlgorithm_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rBLowMagnification.Checked)
            {
                _AlignC.RLWaferAlgorithm = Algoritm(cbRWaferAlgorithm.SelectedIndex);
                if(_recipe.RightLowWaferMat !=null && !_recipe.RightLowMaskMat.Empty())
                {
                    GV.matcherRLW.LearnWithAlgo(_recipe.RightLowWaferMat, _recipe.RightLowWaferMask, _AlignC.RLWaferAlgorithm);
                }
            }
            else if (rBHighMagnification.Checked)
            {
                _AlignC.RHWaferAlgorithm = Algoritm(cbRWaferAlgorithm.SelectedIndex);
                if(_recipe.RightHighWaferMat !=null && !_recipe.RightHighMaskMat.Empty())
                {
                    GV.matcherRHW.LearnWithAlgo(_recipe.RightHighWaferMat, _recipe.RightHighWaferMask, _AlignC.RHWaferAlgorithm);
                }
            }
            if (cbRWaferAlgorithm.SelectedIndex == 2)
            {
                cbRWaferClassList.Visible = true;
                btLabelPatternR.Visible = true;
                pbRWaferImage.Visible = false;
            }
            else
            {
                cbRWaferClassList.Visible = false;
                btLabelPatternR.Visible = false;
                pbRWaferImage.Visible = true;
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
                    catch(Exception ex)
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
                if (_recipe.RightHighMaskMat != null && !_recipe.RightHighMaskMat.Empty() && _recipe.RightHighMaskMask.Height>0 && _recipe.RightHighMaskMask.Width > 0)
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
                if (_recipe.RightHighWaferMat != null && !_recipe.RightHighWaferMat.Empty() && _recipe.RightHighWaferMask.Width>0 && _recipe.RightHighWaferMask.Height > 0)
                {
                    try
                    {
                        pbRWaferImage.Image?.Dispose();
                        pbRWaferImage.Image = _recipe.RightHighWaferMat.ToBitmap();
                    }
                    catch(Exception ex)
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

                CheckParam();
            }
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
                    GV.matcherLLW.LearnWithAlgo(_recipe.LeftLowWaferMat, _recipe.LeftLowWaferMask, _AlignC.LLWaferAlgorithm);
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
                btLabelPatternL.Visible = true;
                pbLWaferImage.Visible = false;
            }
            else
            {
                cbLWaferClassList.Visible = false;
                btLabelPatternL.Visible = false;
                pbLWaferImage.Visible = true;
            }
            
            CheckParam();
        }

        private void CbRMaskAlgorithm_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rBLowMagnification.Checked)
            {
                _AlignC.RLMaskAlgorithm = Algoritm(cbRMaskAlgorithm.SelectedIndex);
                if(_recipe.RightLowMaskMat != null && !_recipe.RightLowMaskMat.Empty())
                {
                    GV.matcherRLM.LearnWithAlgo(_recipe.RightLowMaskMat, _recipe.RightLowMaskMask, _AlignC.RLMaskAlgorithm);
                }
            }                
            else if (rBHighMagnification.Checked)
            {
                _AlignC.RHMaskAlgorithm = Algoritm(cbRMaskAlgorithm.SelectedIndex);
                if(_recipe.RightHighMaskMask != null && !_recipe.RightHighMaskMat.Empty())
                {
                    GV.matcherRHM.LearnWithAlgo(_recipe.RightHighMaskMat, _recipe.RightHighMaskMask, _AlignC.RHMaskAlgorithm);
                }
            }
            if (cbRMaskAlgorithm.SelectedIndex == 2)
            {
                
                cbRMaskClassList.Visible = true;
                btLabelPatternRM.Visible = false;
                pbRMaskImage.Visible = false;
            }
            else
            {
                cbRMaskClassList.Visible = false;
                btLabelPatternRM.Visible = false;
                pbRMaskImage.Visible = true;
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
            }
        }
        private async void BtAlignTest_ClickAsync(object sender, EventArgs e)
        {
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
            Thread.Sleep(250);
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
                    var t1 = GV.matcherLLM.MatMatchWithAlgoAsync(0, GMPLeft, GMPlMaskMp, _AlignC.LLMaskAlgorithm, ct);
                    var t2 = GV.matcherLLW.MatMatchWithAlgoAsync(0, GMPLeft, GMPlWaferMp, _AlignC.LLWaferAlgorithm, ct);
                    var t3 = GV.matcherRLM.MatMatchWithAlgoAsync(0, GMPRight, GMPrMaskMp, _AlignC.RLMaskAlgorithm, ct);
                    var t4 = GV.matcherRLW.MatMatchWithAlgoAsync(0, GMPRight, GMPrWaferMp, _AlignC.RLWaferAlgorithm, ct);
                    await Task.WhenAll(t1, t2, t3, t4).ConfigureAwait(false);
                }
                else if (alignMagnification == "high")
                {
                    var t1 = GV.matcherLHM.MatMatchWithAlgoAsync(0, GMPLeft, GMPlMaskMp, _AlignC.LHMaskAlgorithm, ct);
                    var t2 = GV.matcherLHW.MatMatchWithAlgoAsync(0, GMPLeft, GMPlWaferMp, _AlignC.LHWaferAlgorithm, ct);
                    var t3 = GV.matcherRHM.MatMatchWithAlgoAsync(0, GMPRight, GMPrMaskMp, _AlignC.RHMaskAlgorithm, ct);
                    var t4 = GV.matcherRHW.MatMatchWithAlgoAsync(0, GMPRight, GMPrWaferMp, _AlignC.RHWaferAlgorithm, ct);
                    await Task.WhenAll(t1, t2, t3, t4).ConfigureAwait(false);
                }

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
                    i = 4;
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
                    GV.matcherLLM.MatMatch(lSrc.Clone(), ref LMaskMp);
                    GV.matcherRLM.MatMatch(rSrc.Clone(), ref RMaskMp);
                    GV.matcherLLW.MatMatch(lSrc.Clone(), ref LWaferMp);
                    GV.matcherRLW.MatMatch(rSrc.Clone(), ref RWaferMp);

                }
                else if (alignMmagnification == "high")
                {
                    GV.matcherLHM.MatMatch(lSrc.Clone(), ref LMaskMp);
                    GV.matcherRHM.MatMatch(rSrc.Clone(), ref RMaskMp);
                    GV.matcherLHW.MatMatch(lSrc.Clone(), ref LWaferMp);
                    GV.matcherRHW.MatMatch(rSrc.Clone(), ref RWaferMp);
                }

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
                    i = 4;
                }
            }

            return new NMatchPosition(LMaskMp, LWaferMp, RMaskMp, RWaferMp, i);
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
            nUDXyyX.Value = GV.NowLocation[GV.Plc.iiDChuckX];
            nUDXyyY1.Value = GV.NowLocation[GV.Plc.iiDChuckY1];
            nUDXyyY2.Value = GV.NowLocation[GV.Plc.iiDChuckY2];
            nuDRotate.Value = (Decimal)GV.ZoomLensInfo.MotorStepsPerPixelDegree;
        }

        private void BtGo_Click(object sender, EventArgs e)
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

        private void NuDRotate_ValueChanged(object sender, EventArgs e)
        {
            GV.ZoomLensInfo.MotorStepsPerPixelDegree = (double)nuDRotate.Value;
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
        private void BtFindLMaskCenter_Click(object sender, EventArgs e)
        {
            Mat m;
            Rect roi;
            GV.TickCount = 0;

            if (rBLowMagnification.Checked)
            {
                if (_recipe.LeftLowMaskMat.Empty())
                {
                    MessageBox.Show("請先學習光罩圖型，再找中心!!", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                m = _recipe.LeftLowMaskMat;
                DialogFindCenter FindCenter = new DialogFindCenter()
                {
                    img = m.Clone(),
                    iMaskWaferRadio = 1.2,
                    imgM = new GrayImage(_recipe.LeftLowMaskMask)
                };

                if (FindCenter.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    roi = FindCenter.rect;
                }
                else
                {
                    return;
                }

                Mat newm = new Mat(m, roi);
                pbLMaskImage.Image = newm.ToBitmap();

                if (MessageBox.Show(GV.Dlang.strbtnSaveLLM, GV.Dlang.strSureSaveLLM, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    pbLMaskImage.Image = m.ToBitmap();
                    return;
                }

                _recipe.SetLeftLowMaskMat(newm);

                // 取得 DialogFindCenter 中修改後的遮罩
                GrayImage editedMask = FindCenter.imgM;

                // 裁切遮罩到 ROI 範圍
                GrayImage d = CropGrayImage(editedMask, roi);

                _recipe.LeftLowMaskMask = new GrayImage(d);
                _AlignC.LLMaskAlgorithm = Algoritm(cbLMaskAlgorithm.SelectedIndex);
                GV.matcherLLM.LearnWithAlgo(newm, d, _AlignC.LLMaskAlgorithm);
            }
            else if (rBHighMagnification.Checked)
            {
                if (_recipe.LeftHighMaskMat.Empty())
                {
                    MessageBox.Show("請先學習光罩圖型，再找中心!!", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                m = _recipe.LeftHighMaskMat;
                DialogFindCenter FindCenter = new DialogFindCenter()
                {
                    img = m.Clone(),
                    iMaskWaferRadio = 1.2,
                    imgM = new GrayImage(_recipe.LeftHighMaskMask)
                };

                if (FindCenter.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    roi = FindCenter.rect;
                }
                else
                {
                    return;
                }

                Mat newm = new Mat(m, roi);
                pbLMaskImage.Image = newm.ToBitmap();

                if (MessageBox.Show(GV.Dlang.strbtnSaveLHM, GV.Dlang.strSureSaveLHM, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    pbLMaskImage.Image = m.ToBitmap();
                    return;
                }

                _recipe.SetLeftHighMaskMat(newm);

                // 取得 DialogFindCenter 中修改後的遮罩
                GrayImage editedMask = FindCenter.imgM;

                // 裁切遮罩到 ROI 範圍
                GrayImage d = CropGrayImage(editedMask, roi);

                _recipe.LeftHighMaskMask = new GrayImage(d);
                _AlignC.LHMaskAlgorithm = Algoritm(cbLMaskAlgorithm.SelectedIndex);
                GV.matcherLHM.LearnWithAlgo(newm, d, _AlignC.LHMaskAlgorithm);
            }
            else
            {
                MessageBox.Show("Select magnification first.", "LMaskTemplateSave", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _AlignC.LastModifyTime = DateTime.Now;
            GM.WriteRecipeXml(EditRecipe);
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

        private void BtFindLWaferCenter_Click(object sender, EventArgs e)
        {
            Mat m;
            Rect roi;
            GV.TickCount = 0;

            if (rBLowMagnification.Checked)
            {
                if (_recipe.LeftLowWaferMat.Empty())
                {
                    MessageBox.Show("請先學習晶圓圖型，再找中心!!", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                m = _recipe.LeftLowWaferMat;
                DialogFindCenter FindCenter = new DialogFindCenter()
                {
                    img = m,
                    iMaskWaferRadio = 0.8,
                    iBlockSize = _AlignC.LLWaferBlockSize,
                    iBitwiseNot = _AlignC.LLBitwiseNot,
                    iAlgo = (int)_AlignC.LLWaferAlgorithm,
                    imgM = new GrayImage(_recipe.LeftLowWaferMask)
                };

                if (FindCenter.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    roi = FindCenter.rect;
                   
                }
                else
                {
                    return;
                }
                Mat newm = new Mat(m, roi);
                pbLWaferImage.Image = newm.ToBitmap();

                if (MessageBox.Show(GV.Dlang.strbtnSaveLLW, GV.Dlang.strSureSaveLLW, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    pbLWaferImage.Image = m.ToBitmap();
                    return;
                }
                               
                _recipe.SetLeftLowWaferMat(newm);
                GrayImage editedMask = FindCenter.imgM;               
                GrayImage d = CropGrayImage(editedMask, roi);
                _recipe.LeftLowWaferMask = new GrayImage(d);
                _AlignC.LLWaferAlgorithm = Algoritm(cbLWaferAlgorithm.SelectedIndex);
                GV.matcherLLW.LearnWithAlgo(newm, d, _AlignC.LLWaferAlgorithm);
            }
            else if (rBHighMagnification.Checked)
            {
                if (_recipe.LeftHighWaferMat.Empty())
                {
                    MessageBox.Show("請先學習晶圓圖型，再找中心!!", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                m = _recipe.LeftHighWaferMat;
                DialogFindCenter FindCenter = new DialogFindCenter()
                {
                    img = m.Clone(),
                    iMaskWaferRadio = 0.8,
                    iBlockSize = _AlignC.LHWaferBlockSize,
                    iBitwiseNot = _AlignC.LHBitwiseNot,
                    iAlgo = (int)_AlignC.LHWaferAlgorithm,
                    imgM = new GrayImage(_recipe.LeftHighWaferMask)
                };

                if (FindCenter.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    roi = FindCenter.rect;
                    
                }
                else
                {
                    return;
                }
                Mat newm = new Mat(m, roi);
                pbLWaferImage.Image = newm.ToBitmap();

                if (MessageBox.Show(GV.Dlang.strbtnSaveLHM, GV.Dlang.strSureSaveLHM, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    pbLWaferImage.Image = m.ToBitmap();
                    return;
                }
                
                _recipe.SetLeftHighWaferMat(newm);

                GrayImage editedMask = FindCenter.imgM;
                GrayImage d = CropGrayImage(editedMask, roi);
                _recipe.LeftHighWaferMask = new GrayImage(d);
                _AlignC.LHWaferAlgorithm = Algoritm(cbLWaferAlgorithm.SelectedIndex);
                GV.matcherLHW.LearnWithAlgo(newm, d, _AlignC.LHWaferAlgorithm);
            }
            else
            {
                MessageBox.Show("Select magnification first.", "LWaferTemplateSave", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _AlignC.LastModifyTime = DateTime.Now;
            GM.WriteRecipeXml(EditRecipe);
        }

        private void BtFindRWaferCenter_Click(object sender, EventArgs e)
        {
            Mat m;
            Rect roi;
            GV.TickCount = 0;

            if (rBLowMagnification.Checked)
            {
                if (_recipe.RightLowWaferMat.Empty())
                {
                    MessageBox.Show("請先學習晶圓圖型，再找中心!!", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                m = _recipe.RightLowWaferMat;
                DialogFindCenter FindCenter = new DialogFindCenter()
                {
                    img = m.Clone(),
                    iMaskWaferRadio = 0.8,
                    iBlockSize = _AlignC.RLWaferBlockSize,
                    iBitwiseNot = _AlignC.RLBitwiseNot,
                    iAlgo = (int)_AlignC.RLWaferAlgorithm,
                    imgM = new GrayImage(_recipe.RightLowWaferMask)
                };

                if (FindCenter.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    roi = FindCenter.rect;
                    if (cbRWaferAlgorithm.SelectedIndex == 3)
                    {
                        _AlignC.RLWaferBlockSize = FindCenter.iBlockSize;
                        _AlignC.RLBitwiseNot = FindCenter.iBitwiseNot;
                        // GV.matcherRLW.SetAlgoParameter(FindCenter.iBlockSize, FindCenter.iBitwiseNot);
                    }
                }
                else
                {
                    return;
                }
                //roi = new Rectangle(0, 0, m.Width, m.Height);
                //FindTargetCenterCG findTargetCenterCG = new FindTargetCenterCG();
                //roi = findTargetCenterCG.AutoCenterCGWafer(m, roi);
                Mat newm = new Mat(m, roi);
                pbRWaferImage.Image = newm.ToBitmap();

                if (MessageBox.Show(GV.Dlang.strbtnSaveRLW, GV.Dlang.strSureSaveRLW, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    pbRWaferImage.Image = m.ToBitmap();
                    return;
                }
                
                _recipe.SetRightLowWaferMat(newm);
                GrayImage editedMask = FindCenter.imgM;
                GrayImage d = CropGrayImage(editedMask, roi);
                _recipe.RightLowWaferMask = new GrayImage(d);
                _AlignC.RLWaferAlgorithm = Algoritm(cbRWaferAlgorithm.SelectedIndex);
                GV.matcherRLW.LearnWithAlgo(newm, d, _AlignC.RLWaferAlgorithm);
            }
            else if (rBHighMagnification.Checked)
            {
                if (_recipe.RightHighWaferMat.Empty())
                {
                    MessageBox.Show("請先學習晶圓圖型，再找中心!!", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                m = _recipe.RightHighWaferMat;
                DialogFindCenter FindCenter = new DialogFindCenter()
                {
                    img = m.Clone(),
                    iMaskWaferRadio = 0.8,
                    iBlockSize = _AlignC.RHWaferBlockSize,
                    iAlgo = (int)_AlignC.RHWaferAlgorithm,
                    iBitwiseNot = _AlignC.RHBitwiseNot,
                    imgM = new GrayImage(_recipe.RightHighWaferMask)
                };

                if (FindCenter.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    roi = FindCenter.rect;
                    if (cbRWaferAlgorithm.SelectedIndex == 3)
                    {
                        _AlignC.RHWaferBlockSize = FindCenter.iBlockSize;
                        _AlignC.RHBitwiseNot = FindCenter.iBitwiseNot;
                        //GV.matcherRHW.SetAlgoParameter(FindCenter.iBlockSize, FindCenter.iBitwiseNot);
                    }
                }
                else
                {
                    return;
                }
                //roi = new Rectangle(0, 0, m.Width, m.Height);
                //FindTargetCenterCG findTargetCenterCG = new FindTargetCenterCG();
                //roi = findTargetCenterCG.AutoCenterCGWafer(m, roi);
                Mat newm = new Mat(m, roi);
                pbRWaferImage.Image = newm.ToBitmap();

                if (MessageBox.Show(GV.Dlang.strbtnSaveLHM, GV.Dlang.strSureSaveLHM, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    pbRWaferImage.Image = m.ToBitmap();
                    return;
                }
                
                _recipe.SetRightHighWaferMat(newm);
                GrayImage editedMask = FindCenter.imgM;
                GrayImage d = CropGrayImage(editedMask, roi);
                _recipe.RightHighWaferMask = new GrayImage(d);
                // GM.Learn(GV.m_MatcherRW, ImageConvert.MatToGrayImage(GV.RightHighWaferMat[EditRecipe]), GV.RightHighWaferMask[EditRecipe]);
                _AlignC.RHWaferAlgorithm = Algoritm(cbRWaferAlgorithm.SelectedIndex);
                GV.matcherRHW.LearnWithAlgo(newm, d, _AlignC.RHWaferAlgorithm);
            }
            else
            {
                MessageBox.Show("Select magnification first.", "RWaferTemplateSave", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _AlignC.LastModifyTime = DateTime.Now;
            GM.WriteRecipeXml(EditRecipe);
        }

        private void BtFindRMaskCenter_Click(object sender, EventArgs e)
        {
            Mat m;
            Rect roi;
            GV.TickCount = 0;

            if (rBLowMagnification.Checked)
            {
                if (_recipe.RightLowMaskMat.Empty())
                {
                    MessageBox.Show("請先學習光罩圖型，再找中心!!", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                m = _recipe.RightLowMaskMat;
                DialogFindCenter FindCenter = new DialogFindCenter()
                {
                    img = m.Clone(),
                    iMaskWaferRadio = 1.2,
                    imgM = new GrayImage(_recipe.RightLowMaskMask)
                };

                if (FindCenter.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    roi = FindCenter.rect;
                }
                else
                {
                    return;
                }
                Mat newm = new Mat(m, roi);
                pbRMaskImage.Image = newm.ToBitmap();

                if (MessageBox.Show(GV.Dlang.strbtnSaveRLM, GV.Dlang.strSureSaveRLM, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    pbRMaskImage.Image = m.ToBitmap();
                    return;
                }

                _recipe.SetRightLowMaskMat(newm);
                GrayImage editedMask = FindCenter.imgM;
                GrayImage d = CropGrayImage(editedMask, roi);
                _recipe.RightLowMaskMask = new GrayImage(d);
                _AlignC.RLMaskAlgorithm = Algoritm(cbRMaskAlgorithm.SelectedIndex);
                GV.matcherRLM.LearnWithAlgo(newm, d, _AlignC.RLMaskAlgorithm);
            }
            else if (rBHighMagnification.Checked)
            {
                if (_recipe.RightHighMaskMat.Empty())
                {
                    MessageBox.Show("請先學習光罩圖型，再找中心!!", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                m = _recipe.RightHighMaskMat;
                DialogFindCenter FindCenter = new DialogFindCenter()
                {
                    img = m.Clone(),
                    iMaskWaferRadio = 1.2,
                    imgM = new GrayImage(_recipe.RightHighMaskMask)
                };

                if (FindCenter.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    roi = FindCenter.rect;
                }
                else
                {
                    return;
                }
                Mat newm = new Mat(m, roi);
                pbRMaskImage.Image = newm.ToBitmap();

                if (MessageBox.Show(GV.Dlang.strbtnSaveRHM, GV.Dlang.strSureSaveRHM, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    pbRMaskImage.Image = m.ToBitmap();
                    return;
                }

                _recipe.SetRightHighMaskMat(newm);
                GrayImage editedMask = FindCenter.imgM;
                GrayImage d = CropGrayImage(editedMask, roi);
                _recipe.RightHighMaskMask = new GrayImage(d);
                _AlignC.RHMaskAlgorithm = Algoritm(cbRMaskAlgorithm.SelectedIndex);
                GV.matcherRHM.LearnWithAlgo(newm, d, _AlignC.RHMaskAlgorithm);
            }
            else
            {
                MessageBox.Show("Select magnification first.", "RMaskTemplateSave", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _AlignC.LastModifyTime = DateTime.Now;
            GM.WriteRecipeXml(EditRecipe);
        }

        private void BtLeftWaferFocus_Click(object sender, EventArgs e)
        {

        }

        private void btLabelPatternL_Click(object sender, EventArgs e)
        {
            Bitmap bmp = GV.LeftUpCam.Grab().ToBitmap();
            DialogLabelImage labelImage = new DialogLabelImage(bmp,this.NowMageni);
            labelImage.ShowDialog();
        }

        private void btLabelPatternR_Click(object sender, EventArgs e)
        {
            Bitmap bmp= GV.RightUpCam.Grab().ToBitmap();
            DialogLabelImage labelImage = new DialogLabelImage(bmp,NowMageni);
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
            groupBox4.Visible = visible;
        }
        
    }
}
