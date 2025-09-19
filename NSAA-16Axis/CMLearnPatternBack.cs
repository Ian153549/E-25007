//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Drawing;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using System.Threading;
//using MRLibrary;
//using OpenCvSharp;
//using OpenCvSharp.Extensions;
//using System.IO;
//using System.Diagnostics;
//using System.Drawing.Imaging;
//using static MRLibrary.OpenCV3MatchUMat;


//namespace NSAA_16Axis
//{
//    public partial class CMLearnPatternBack : UserControl
//    {
//        public int EditRecipe;
//        public int NowMagni;
//        public int SleepTime = 200;
//        public int NowLevel;

//        public double _dLalpha;
//        public double _dRalpha;

//        public int iLalpha;
//        public int iRalpha;

//        public Mat LeftMaskMat, RightMaskMat;
//        public Mat LeftMask, RightMask;

//        public MatchPosition LeftWaferMp = null;
//        public MatchPosition LeftMaskMp = null;
//        public MatchPosition RightWaferMp = null;
//        public MatchPosition RightMaskMp = null;

//        ProductMatchPositions BLastPmps = new ProductMatchPositions();
//        ProductMatchPositions BLivePmps = new ProductMatchPositions();

//        public Mat LViewMat;
//        public Mat RViewMat;
//        public Recipe _recipe = null;
//        public RecipeTemplate _recipeT = null;
//        public AlignCondition _AlignC = null;
//        public bool isLive = true;
//        ProductMatchPositions OrgPmps = new ProductMatchPositions();
//        public int[] OrgMotors = new int[3];
//        private string _ClockTime;
//        public int iAlignOK = 0;
//        public int iAdmin = 0;
//        public Thread DrawMatchThread = null;

//        public Mat LHWaferMat;
//        public Mat LHMaskMat;
//        public Mat RHWaferMat;
//        public Mat RHMaskMat;
//        public Mat LLMaskMat;
//        public Mat LLWaferMat;
//        public Mat RLMaskMat;
//        public Mat RLWaferMat;
//        //private Mat LLMaskMat
//        //{
//        //    get => _recipeT.LeftLowMaskMat;
//        //    set
//        //    {
//        //        if (_recipeT != null) _recipeT.LeftLowMaskMat = value;
//        //    }
//        //}
//        //private Mat LHMaskMat
//        //{
//        //    get => _recipeT?.LeftHighMaskMat;
//        //    set
//        //    {
//        //        if (_recipeT != null) _recipeT.LeftHighMaskMat = value;
//        //    }
//        //}
//        //private Mat RLMaskMat
//        //{
//        //    get => _recipeT.RightLowMaskMat;
//        //    set
//        //    {
//        //        if (_recipeT != null) _recipeT.RightLowMaskMat = value;
//        //    }
//        //}
//        //private Mat RHMaskMat
//        //{
//        //    get => _recipeT.RightHighMaskMat;
//        //    set
//        //    {
//        //        if (_recipeT != null) _recipeT.RightHighMaskMat = value;
//        //    }
//        //}
//        //private Mat LLWaferMat
//        //{
//        //    get => _recipeT?.LeftLowWaferMat;
//        //    set
//        //    {
//        //        if (_recipeT != null)
//        //            _recipeT.LeftLowWaferMat = value;
//        //    }
//        //}
//        //private Mat LHWaferMat
//        //{
//        //    get => _recipeT?.LeftHighWaferMat;
//        //    set
//        //    {
//        //        if (_recipeT != null)
//        //            _recipeT.LeftHighWaferMat = value;
//        //    }
//        //}
//        //private Mat RLWaferMat
//        //{
//        //    get => _recipeT?.RightLowWaferMat;
//        //    set
//        //    {
//        //        if (_recipeT != null)
//        //            _recipeT.RightLowWaferMat = value;
//        //    }
//        //}
//        //private Mat RHWaferMat
//        //{
//        //    get => _recipeT?.RightHighWaferMat;
//        //    set
//        //    {
//        //        if (_recipeT != null)
//        //            _recipeT.RightHighWaferMat = value;
//        //    }
//        //}
//        public CMLearnPatternBack()
//        {
//            InitializeComponent();
//            SetStyle(ControlStyles.UserPaint, true);
//            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
//            SetStyle(ControlStyles.DoubleBuffer, true);
//            skLeft.CanCenterLine = true;
//            skLeft.CanRectMaskAndWafer = true;
//            skLeft.CanTrackPattern = true;
//            skLeft.CanZoom = true;
//            skRight.CanCenterLine = true;
//            skRight.CanRectMaskAndWafer = true;
//            skRight.CanTrackPattern = true;
//            skRight.CanZoom = true;
//            GV.BackLearnLeft = skLeft;
//            GV.BackLearnRight = skRight;
//        }

//        private void CMLearnPattern_Load(object sender, EventArgs e)
//        {
//            if (GV.AppSettingParm.DebugMode)
//            {
//                lbDebugMsg.Visible = true;
//            }

//            EditRecipe = GV.NowRecipeNumber;

//            if (GV.AppSettingParm.Language != "default")
//            {
//                btLTopMaskSave.Text = GV.Dlang.strSave;
//                btRTopMaskSave.Text = GV.Dlang.strSave;
//                btLBackMaskSave.Text = GV.Dlang.strSave;
//                btRBackMaskSave.Text = GV.Dlang.strSave;
//                btLWaferSave.Text = GV.Dlang.strSave;
//                btRWaferSave.Text = GV.Dlang.strSave;
//                btMagSave.Text = GV.Dlang.strSave;
//                btDelete.Text = GV.Dlang.strDelete;
//                btDefault.Text = GV.Dlang.strDefault;
//                gbLWafer.Text = GV.Dlang.strWafer;
//                gbRWafer.Text = GV.Dlang.strWafer;
//                gbLTopMask.Text = GV.Dlang.strTop + GV.Dlang.strMask;
//                gbRTopMask.Text = GV.Dlang.strTop + GV.Dlang.strMask;
//                gbLBackMask.Text = GV.Dlang.strBack + GV.Dlang.strMask;
//                gbRBackMask.Text = GV.Dlang.strBack + GV.Dlang.strMask;
//                gBMag.Text = GV.Dlang.groupBoxMagnification;
//                rBHighMagnification.Text = GV.Dlang.radioButtonHighMagnification;
//                rBLowMagnification.Text = GV.Dlang.radioButtonLowMagnification;
//                rbLTopMask.Text = GV.Dlang.strLight;
//                rbRTopMask.Text = GV.Dlang.strLight;
//                rbLBackMask.Text = GV.Dlang.strLight;
//                rbRBackMask.Text = GV.Dlang.strLight;
//                rbLWafer.Text = GV.Dlang.strLight;
//                rbRWafer.Text = GV.Dlang.strLight;
//                label1.Text = GV.Dlang.strAlgo;
//                label2.Text = GV.Dlang.strAlgo;
//                label3.Text = GV.Dlang.strAlgo;
//                label4.Text = GV.Dlang.strAlgo;
//                label5.Text = GV.Dlang.strAlgo;
//                label6.Text = GV.Dlang.strAlgo;
//                lbLLive.Text = GV.Dlang.strLive;
//                lbRLive.Text = GV.Dlang.strLive;
//                lbLRecord.Text = GV.Dlang.strRecord;
//                lbRRecord.Text = GV.Dlang.strRecord;
//                cBTopMask.Text = GV.Dlang.strTopMask;
//                cBBottomMask.Text = GV.Dlang.strBottomMask;
//                btCapMask.Caption = GV.Dlang.strCapMask;
//                btAlignTest.Caption = GV.Dlang.strAlignTest;
//                BtMaskCenter.Caption = GV.Dlang.strMaskCenter;
//                btCaptureN.Caption = GV.Dlang.strCapture;
//                cbLWaferAlgo.Items.Clear();
//                cbRWaferAlgo.Items.Clear();
//                cbLBackMaskAlgo.Items.Clear();
//                cbRBackMaskAlgo.Items.Clear();
//                cbLWaferAlgo.Items.Add(GV.Dlang.strTemplate);
//                cbLWaferAlgo.Items.Add(GV.Dlang.strEdge);
//                cbRWaferAlgo.Items.Add(GV.Dlang.strTemplate);
//                cbRWaferAlgo.Items.Add(GV.Dlang.strEdge);
//                cbLBackMaskAlgo.Items.Add(GV.Dlang.strTemplate);
//                cbLBackMaskAlgo.Items.Add(GV.Dlang.strEdge);
//                cbRBackMaskAlgo.Items.Add(GV.Dlang.strTemplate);
//                cbRBackMaskAlgo.Items.Add(GV.Dlang.strEdge);
//                _recipe = GV._recipe;
//                _AlignC = _recipe.AlignC;
//                cbLWaferAlgo.SelectedIndex = 0;
//                cbRWaferAlgo.SelectedIndex = 0;
//                cbLBackMaskAlgo.SelectedIndex = 0;
//                cbRBackMaskAlgo.SelectedIndex = 0;
//            }

//            LeftMaskMat = GV.LeftMaskMat;
//            RightMaskMat = GV.RightMaskMat;
//            tBLShowImage.Enabled = false;
//            tBRShowImage.Enabled = false;
//            skLeft.MaskMp = new MatchPosition();
//            skLeft.WaferMp = new MatchPosition();
//            skRight.MaskMp = new MatchPosition();
//            skRight.WaferMp = new MatchPosition();

//            if (GV.AppSettingParm.LeftUpCamEnable == true)
//            {
//                lbEmulationModeL.Visible = false;
//            }
//            else
//            {
//                skLeft.CanRectMaskAndWafer = false;
//            }
//            if (GV.AppSettingParm.RightUpCamEnable == true)
//            {
//                lbEmulationModeR.Visible = false;
//            }
//            else
//            {
//                skRight.CanRectMaskAndWafer = false;
//            }

//            if (GV.AppSettingParm.DebugMode == true)
//            {
//                btOpen.Visible = true;
//            }

//            NUDLMaskX.Visible = false;
//            NUDLMaskY.Visible = false;
//            NUDRMaskX.Visible = false;
//            NUDRMaskY.Visible = false;
//            BtLMaskAdjSave.Visible = false;
//            BtRMaskAdjSave.Visible = false;
//            lblLMaskX.Visible = false;
//            lblLMaskY.Visible = false;
//            lblRMaskX.Visible = false;
//            lblRMaskY.Visible = false;
//        }

//        public void DrawMatchPosition()
//        {
//            MatchPosition lMaskMp = new MatchPosition();
//            MatchPosition rMaskMp = new MatchPosition();
//            MatchPosition lWaferMp = new MatchPosition();
//            MatchPosition rWaferMp = new MatchPosition();
//            while (!GV.AppEnding)
//            {
//                while (GV.TabOption == GV.Tab.BackLearn)
//                {
//                    if (rbLBackMask.Checked)
//                    {
//                        GV.matcherLLW.MatMatchWithAlgo(0, GV.LeftBackCam.Grab(), ref lMaskMp, _AlignC.LLWaferAlgorithm);
//                        GV.matcherRLW.MatMatchWithAlgo(0, GV.RightBackCam.Grab(), ref rMaskMp, _AlignC.RLWaferAlgorithm);
//                    }
//                    else
//                    {
//                        GV.matcherLLW.MatMatchWithAlgo(0, LeftMaskMat, ref lMaskMp, _AlignC.LLWaferAlgorithm);
//                        GV.matcherRLW.MatMatchWithAlgo(0, RightMaskMat, ref rMaskMp, _AlignC.RLWaferAlgorithm);
//                        GV.matcherLHW.MatMatchWithAlgo(0, GV.LeftBackCam.Grab(), ref lWaferMp, _AlignC.LHWaferAlgorithm);
//                        GV.matcherRHW.MatMatchWithAlgo(0, GV.RightBackCam.Grab(), ref rWaferMp, _AlignC.RHWaferAlgorithm);
//                    }

//                    Invoke((MethodInvoker)delegate ()
//                    {
//                        skLeft.MaskMp = lMaskMp;
//                        skLeft.WaferMp = lWaferMp;
//                        skRight.MaskMp = rMaskMp;
//                        skRight.WaferMp = rWaferMp;

//                        double dLX = lMaskMp.X - lWaferMp.X;
//                        double dLY = lMaskMp.Y - lWaferMp.Y;
//                        double dRX = rMaskMp.X - rWaferMp.X;
//                        double dRY = rMaskMp.Y - rWaferMp.Y;

//                        string msgLM = string.Format("LMX = {0:N3}, LMY = {1:N3}, dX = {2:N3}, dY = {3:N3}, MScore = {4:N2}",
//                            lMaskMp.X, lMaskMp.Y, dLX, dLY, lMaskMp.Score);
//                        string msgLW = string.Format("LWX = {0:N3}, LWY = {1:N3}, WScore = {2:N2}",
//                            lWaferMp.X, lWaferMp.Y, lWaferMp.Score);
//                        string msgRM = string.Format("RMX = {0:N3}, RMY = {1:N3}, dX = {2:N3}, dY = {3:N3}, MScore = {4:N2}",
//                            rMaskMp.X, rMaskMp.Y, dRX, dRY, rMaskMp.Score);
//                        string msgRW = string.Format("RWX = {0:N3}, RWY = {1:N3}, WScore = {2:N2}",
//                            rWaferMp.X, rWaferMp.Y, rWaferMp.Score);

//                        lbLeftXYM.Text = msgLM;
//                        lbRightXYM.Text = msgRM;
//                        lbLeftXYW.Text = msgLW;
//                        lbRightXYW.Text = msgRW;

//                        Update();
//                    });
//                    Thread.Sleep(250);
//                }
//                Thread.Sleep(1000);
//            }
//        }

//        public void UpdateUI()
//        {
//            if ((GV.NowRecipeNumber < 1) || (GV.NowRecipeNumber > 100))
//            {
//                GV.NowRecipeNumber = 1;

//            }

//            EditRecipe = GV.NowRecipeNumber;

//            ChangeRecipe();

//            lbRecipeNumber.Text = GV.Dlang.strRecipeNumber + EditRecipe.ToString();
//            //GV.HwndFormMain.writeStatus(lbRecipeNumber.Text + "  " + GV.NowRecipeNumber + "  " + EditRecipe.ToString());
//            pbLTopMask.Image?.Dispose();
//            pbLTopMask.Image = null;
//            pbLBackMask.Image?.Dispose();
//            pbLBackMask.Image = null;
//            pbLWafer.Image?.Dispose();
//            pbLWafer.Image = null;
//            pbRTopMask.Image?.Dispose();
//            pbRTopMask.Image = null;
//            pbRBackMask.Image?.Dispose();
//            pbRBackMask.Image = null;
//            pbRWafer.Image?.Dispose();
//            pbRWafer.Image = null;

//            if (rBLowMagnification.Checked)
//            {
//                if (LLMaskMat != null && !LLMaskMat.Empty())
//                {
//                    pbLTopMask.Image = LLMaskMat.ToBitmap();
//                }
//                if (RLMaskMat != null && !RLMaskMat.Empty())
//                {
//                    pbRTopMask.Image = RLMaskMat.ToBitmap();
//                }
//                cbLTopMaskAlgo.SelectedIndex = (int)_AlignC.LLMaskAlgorithm;
//                cbRTopMaskAlgo.SelectedIndex = (int)_AlignC.RLMaskAlgorithm;
//            }
//            else
//            {
//                //if (_recipe.LeftHighMaskMat != null)
//                //{
//                //    LHMaskMat = _recipeT.LeftHighMaskMat.Clone();
//                //    pbLTopMask.Image = LHMaskMat.ToBitmap();
//                //}
//                //else { pbLTopMask.Image = null; }

//                //if (_recipe.RightHighMaskMat != null)
//                //{
//                //    RHMaskMat = _recipeT.RightHighMaskMat.Clone();
//                //    pbRTopMask.Image = RHMaskMat.ToBitmap();
//                //}
//                //else { pbRTopMask.Image = null; }
//                if (LHMaskMat != null && !LHMaskMat.Empty())
//                {
//                    pbLTopMask.Image = LHMaskMat.ToBitmap();
//                }
//                if (RHMaskMat != null && !RHMaskMat.Empty())
//                {
//                    pbRTopMask.Image = RHMaskMat.ToBitmap();
//                }

//                cbLTopMaskAlgo.SelectedIndex = (int)_AlignC.LHMaskAlgorithm;
//                cbRTopMaskAlgo.SelectedIndex = (int)_AlignC.RHMaskAlgorithm;
//            }

//            if (LLWaferMat != null && !LLWaferMat.Empty())
//            {
//                pbLBackMask.Image = LLWaferMat.ToBitmap();
//            }

//            if (LHWaferMat != null && !LHWaferMat.Empty())
//            {
//                pbLWafer.Image = LHWaferMat.ToBitmap();
//            }

//            if (RLWaferMat != null && !RLWaferMat.Empty())
//            {
//                pbRBackMask.Image = RLWaferMat.ToBitmap();
//            }
//            if (RHWaferMat != null && !RHWaferMat.Empty())
//            {
//                pbRWafer.Image = RHWaferMat.ToBitmap();
//            }


//            cbLBackMaskAlgo.SelectedIndex = (int)_AlignC.LLWaferAlgorithm;
//            cbLWaferAlgo.SelectedIndex = (int)_AlignC.LHWaferAlgorithm;
//            cbRBackMaskAlgo.SelectedIndex = (int)_AlignC.RLWaferAlgorithm;
//            cbRWaferAlgo.SelectedIndex = (int)_AlignC.RHWaferAlgorithm;

//            int iL = (int)(_AlignC.dLalpha * 100);
//            if (iL > 100) iL = 100;
//            if (iL < 0) iL = 0;
//            int iR = (int)(_AlignC.dRalpha * 100);
//            if (iR > 100) iR = 100;
//            if (iR < 0) iR = 0;

//            iLalpha = iL;
//            iRalpha = iR;

//            tBLShowImage.Value = iLalpha;
//            tBRShowImage.Value = iRalpha;

//            _dLalpha = (double)tBLShowImage.Value / 100;
//            _dRalpha = (double)tBRShowImage.Value / 100;

//            if (rbLTopMask.Checked == false)
//            {
//                skLeft.SetShowMask(true, _dLalpha, LeftMaskMat);
//                skRight.SetShowMask(true, _dRalpha, RightMaskMat);
//            }
//            else
//            {
//                skLeft.SetShowMask(false, 0, LeftMaskMat);
//                skRight.SetShowMask(false, 0, RightMaskMat);
//            }

//            if ((rbLTopMask.Enabled) && (rbLTopMask.Checked) && (GV.UserLevel != GV.User.Operator))
//            {
//                btLTopMaskLightPlus.Enabled = true;
//                btLTopMaskLightMinus.Enabled = true;
//                tBLeftTopMaskLight.Enabled = true;
//                btLTopMaskLSave.Enabled = true;
//            }
//            else
//            {
//                btLTopMaskLightPlus.Enabled = false;
//                btLTopMaskLightMinus.Enabled = false;
//                tBLeftTopMaskLight.Enabled = false;
//                btLTopMaskLSave.Enabled = false;
//            }

//            if ((rbLBackMask.Enabled) && (rbLBackMask.Checked) && (GV.UserLevel != GV.User.Operator))
//            {
//                btLBackMaskLightPlus.Enabled = true;
//                btLBackMaskLightMinus.Enabled = true;
//                tBLeftBackMaskLight.Enabled = true;
//                btLBackMaskLSave.Enabled = true;
//            }
//            else
//            {
//                btLBackMaskLightPlus.Enabled = false;
//                btLBackMaskLightMinus.Enabled = false;
//                tBLeftBackMaskLight.Enabled = false;
//                btLBackMaskLSave.Enabled = false;
//            }

//            if ((rbLWafer.Enabled) && (rbLWafer.Checked) && (GV.UserLevel != GV.User.Operator))
//            {
//                btLWaferLightPlus.Enabled = true;
//                btLWaferLightMinus.Enabled = true;
//                tBLeftBackWaferLight.Enabled = true;
//                btLWaferLSave.Enabled = true;
//            }
//            else
//            {
//                btLWaferLightPlus.Enabled = false;
//                btLWaferLightMinus.Enabled = false;
//                tBLeftBackWaferLight.Enabled = false;
//                btLWaferLSave.Enabled = false;
//            }

//            if ((rbRTopMask.Enabled) && (rbRTopMask.Checked) && (GV.UserLevel != GV.User.Operator))
//            {
//                btRTopMaskLightPlus.Enabled = true;
//                btRTopMaskLightMinus.Enabled = true;
//                tBRightTopMaskLight.Enabled = true;
//                btRTopMaskLSave.Enabled = true;
//            }
//            else
//            {
//                btRTopMaskLightPlus.Enabled = false;
//                btRTopMaskLightMinus.Enabled = false;
//                tBRightTopMaskLight.Enabled = false;
//                btRTopMaskLSave.Enabled = false;
//            }

//            if ((rbRBackMask.Enabled) && (rbRBackMask.Checked) && (GV.UserLevel != GV.User.Operator))
//            {
//                btRBackMaskLightPlus.Enabled = true;
//                btRBackMaskLightMinus.Enabled = true;
//                tBRightBackMaskLight.Enabled = true;
//                btRBackMaskLSave.Enabled = true;
//            }
//            else
//            {
//                btRBackMaskLightPlus.Enabled = false;
//                btRBackMaskLightMinus.Enabled = false;
//                tBRightBackMaskLight.Enabled = false;
//                btRBackMaskLSave.Enabled = false;
//            }

//            if ((rbRWafer.Enabled) && (rbRWafer.Checked) && (GV.UserLevel != GV.User.Operator))
//            {
//                btRWaferLightPlus.Enabled = true;
//                btRWaferLightMinus.Enabled = true;
//                tBRightBackWaferLight.Enabled = true;
//                btRWaferLSave.Enabled = true;
//            }
//            else
//            {
//                btRWaferLightPlus.Enabled = false;
//                btRWaferLightMinus.Enabled = false;
//                tBRightBackWaferLight.Enabled = false;
//                btRWaferLSave.Enabled = false;
//            }

//            if (rbLTopMask.Checked)
//            {
//                GV.Light.ChangeBrightness("left", tBLeftTopMaskLight.Value);
//            }
//            else
//            {
//                GV.Light.ChangeBrightness("left", 0);
//            }

//            if (rbLBackMask.Checked)
//            {
//                GV.Light.ChangeBrightness("leftback", tBLeftBackMaskLight.Value);
//            }
//            else if (rbLWafer.Checked)
//            {
//                GV.Light.ChangeBrightness("leftback", tBLeftBackWaferLight.Value);
//            }
//            else
//            {
//                GV.Light.ChangeBrightness("leftback", 0);
//            }

//            if (rbRTopMask.Checked)
//            {
//                GV.Light.ChangeBrightness("right", tBRightTopMaskLight.Value);
//            }
//            else
//            {
//                GV.Light.ChangeBrightness("right", 0);
//            }

//            if (rbRBackMask.Checked)
//            {
//                GV.Light.ChangeBrightness("rightback", tBRightBackMaskLight.Value);
//            }
//            else if (rbRWafer.Checked)
//            {
//                GV.Light.ChangeBrightness("rightback", tBRightBackWaferLight.Value);
//            }
//            else
//            {
//                GV.Light.ChangeBrightness("rightback", 0);
//            }
//            Update();
//        }

//        private void ChangeRecipe()
//        {
//            _recipe = GV._recipe;
//            _AlignC = _recipe.AlignC;
//        }


//        public void CheckLevel()
//        {
//            if (LeftWaferMp == null)
//            {
//                LeftWaferMp = new MatchPosition();
//                LeftMaskMp = new MatchPosition();
//                RightWaferMp = new MatchPosition();
//                RightMaskMp = new MatchPosition();
//            }

//            int nLevel = (int)GV.UserLevel;
//            if (NowLevel != nLevel)
//            {
//                NowLevel = nLevel;
//                Invoke((MethodInvoker)delegate ()
//                {
//                    GV.RingLight.ChangeBrightness("left", 0);
//                    GV.RingLight.ChangeBrightness("right", 0);
//                    // tBLShowImage.Enabled = true;
//                    // tBRShowImage.Enabled = true;

//                    if (GV.UserLevel == GV.User.Operator)
//                    {
//                        // rbLTopMask.Enabled = true;
//                        // rbRTopMask.Enabled = true;
//                        // rbLBackMask.Enabled = true;
//                        // rbRBackMask.Enabled = true;
//                        // rbLWafer.Enabled = true;
//                        btSave.Enabled = false;
//                        cbLowMagnification.Enabled = false;
//                        cbHighMagnification.Enabled = false;
//                        cbLBackMaskAlgo.Enabled = false;
//                        cbRBackMaskAlgo.Enabled = false;
//                        cbLWaferAlgo.Enabled = false;
//                        cbRWaferAlgo.Enabled = false;
//                        btMagSave.Enabled = false;
//                        btLShowImageSave.Enabled = false;
//                        btRShowImageSave.Enabled = false;
//                        btLTopMaskLSave.Enabled = false;
//                        btLBackMaskLSave.Enabled = false;
//                        btLWaferLSave.Enabled = false;
//                        btRTopMaskLSave.Enabled = false;
//                        btRBackMaskLSave.Enabled = false;
//                        btRWaferLSave.Enabled = false;
//                        btDelete.Enabled = false;
//                        btDefault.Enabled = false;
//                        groupBox4.Visible = false;
//                        btLWaferMask.Enabled = false;
//                        btRWaferMask.Enabled = false;
//                        btLBackMask.Enabled = false;
//                        btRBackMask.Enabled = false;
//                        btFindLBMaskCenter.Visible = false;
//                        btFindRBMaskCenter.Visible = false;
//                        btFindLWaferCenter.Visible = false;
//                        btFindRWaferCenter.Visible = false;
//                    }
//                    else
//                    {
//                        gbLWafer.Enabled = true;
//                        gbLBackMask.Enabled = true;
//                        gbRWafer.Enabled = true;
//                        gbRBackMask.Enabled = true;
//                        gbLTopMask.Enabled = true;
//                        gbRTopMask.Enabled = true;
//                        cbLowMagnification.Enabled = true;
//                        cbHighMagnification.Enabled = true;
//                        btMagSave.Enabled = true;
//                        btLShowImageSave.Enabled = true;
//                        btRShowImageSave.Enabled = true;
//                        btDelete.Enabled = true;
//                        btDefault.Enabled = true;
//                        groupBox4.Visible = false;
//                        btLWaferMask.Enabled = true;
//                        btRWaferMask.Enabled = true;
//                        btLBackMask.Enabled = true;
//                        btRBackMask.Enabled = true;
//                        btSave.Enabled = true;
//                        btFindLBMaskCenter.Visible = true;
//                        btFindLWaferCenter.Visible = true;
//                        btFindRBMaskCenter.Visible = true;
//                        btFindRWaferCenter.Visible = true;
//                    }

//                    if (GV.UserLevel == GV.User.Administrator)
//                    {
//                        btFindLMaskCenter.Visible = true;
//                        btFindLWaferCenter.Visible = true;
//                        btFindRMaskCenter.Visible = true;
//                        lbLeftXYM.Visible = true;
//                        lbRightXYM.Visible = true;
//                        btReportLocation.Visible = true;
//                        btLMaskSave.Visible = true;
//                        btRMaskSave.Visible = true;
//                        bBTtMaskAlign.Visible = true;
//                        btBTAlignWafer.Visible = true;
//                        btBTAlignWaferRotate.Visible = true;
//                        btSave.Enabled = true;
//                        btReadMaskMat.Visible = true;
//                        btNG.Visible = true;
//                        NUDLMaskX.Visible = true;
//                        NUDLMaskY.Visible = true;
//                        NUDRMaskX.Visible = true;
//                        NUDRMaskY.Visible = true;
//                        BtLMaskAdjSave.Visible = true;
//                        BtRMaskAdjSave.Visible = true;
//                        lblLMaskX.Visible = true;
//                        lblLMaskY.Visible = true;
//                        lblRMaskX.Visible = true;
//                        lblRMaskY.Visible = true;
//                    }
//                    else
//                    {
//                        btFindLMaskCenter.Visible = false;
//                        btFindRMaskCenter.Visible = false;
//                        // lbLeftXYM.Visible = false;
//                        // lbRightXYM.Visible = false;
//                        btReportLocation.Visible = false;
//                        btLMaskSave.Visible = false;
//                        btRMaskSave.Visible = false;
//                        bBTtMaskAlign.Visible = false;
//                        btBTAlignWafer.Visible = false;
//                        btBTAlignWaferRotate.Visible = false;
//                        // groupBox4.Visible = false;
//                        btReadMaskMat.Visible = false;
//                        btNG.Visible = false;
//                        NUDLMaskX.Visible = false;
//                        NUDLMaskY.Visible = false;
//                        NUDRMaskX.Visible = false;
//                        NUDRMaskY.Visible = false;
//                        BtLMaskAdjSave.Visible = false;
//                        BtRMaskAdjSave.Visible = false;
//                        lblLMaskX.Visible = false;
//                        lblLMaskY.Visible = false;
//                        lblRMaskX.Visible = false;
//                        lblRMaskY.Visible = false;
//                    }
//                });

//                rBLowMagnification.Enabled = true;
//                rBHighMagnification.Enabled = true;
//            }
//            UpdateUI();
//        }

//        private string GetTemplateFileName(string patternAndSide)
//        {
//            string fileName = "Templates//" + patternAndSide + EditRecipe.ToString() + ".bmp";
//            return fileName;
//        }

//        private string GetTemplateFileNameD(string patternAndSide)
//        {
//            string fileName = "Templates//D" + patternAndSide + EditRecipe.ToString() + ".bmp";
//            return fileName;
//        }
//        private OpenCV3MatchUMat.AlignAlgorithm Algoritm(int a)
//        {
//            if (a == 0)
//                return OpenCV3MatchUMat.AlignAlgorithm.TemplateMatch;
//            else if (a == 1)
//                return OpenCV3MatchUMat.AlignAlgorithm.EdgeMatch;
//            else
//                return OpenCV3MatchUMat.AlignAlgorithm.TemplateMatch;

//        }

//        private void BtLWaferSave_Click(object sender, EventArgs e)
//        {
//            Mat m;
//            Mat fullImage;
//            GV.TickCount = 0;
//            //Mat m = new Mat(GV.LeftBackCam.Grab(), skLeft.GetWaferRect());
//            if (isLive)
//            {
//                fullImage = GV.LeftBackCam.Grab();
//            }
//            else
//            {
//                fullImage = LViewMat?.Clone();
//            }
//            if (fullImage == null || fullImage.Empty())
//            {
//                MessageBox.Show("相機尚未取得有效影像");
//                return;
//            }
//            OpenCvSharp.Rect roi = skLeft.GetWaferRect();
//            if (roi.Width <= 0 || roi.Height <= 0 ||
//                roi.X < 0 || roi.Y < 0 || roi.X + roi.Width > fullImage.Width || roi.Y + roi.Height > fullImage.Height)
//            {
//                MessageBox.Show("遮罩區域無效或超出影像範圍");
//                return;
//            }
//            m = new Mat(fullImage, roi);
//            pbLWafer.Image?.Dispose();
//            pbLWafer.Image = m.ToBitmap();
//            if (MessageBox.Show(GV.Dlang.strLWafer, GV.Dlang.strLWaferSave, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
//            {
//                pbLWafer.Image?.Dispose();
//                if (!LHWaferMat.Empty() && LHWaferMat != null)
//                {
//                    pbLWafer.Image = LHWaferMat.ToBitmap();
//                }
//                m.Dispose();
//                fullImage.Dispose();
//                return;
//            }

//            _AlignC.LBWaferBright = tBLeftBackWaferLight.Value;
//            LHWaferMat = m;
//            Cv2.ImWrite(GetTemplateFileName("LHW"), m);
//            GrayImage d = new GrayImage(m.Width, m.Height);
//            d.Fill(255);
//            _recipeT.LeftHighWaferMask = d;
//            d.Save(GetTemplateFileName("MLHW"), System.Drawing.Imaging.ImageFormat.Bmp);
//            _AlignC.LHWaferAlgorithm = Algoritm(cbLWaferAlgo.SelectedIndex);
//            GV.matcherLHW.LearnWithAlgo(m, d, _AlignC.LHWaferAlgorithm);
//            _AlignC.LastModifyTime = DateTime.Now;
//            GM.WriteRecipeXml(GV._recipe.FileName, GV._recipe);
//        }

//        private void BtLMaskTemplateSave_Click(object sender, EventArgs e)
//        {
//            //if (MessageBox.Show("Sure to save left back mask template ?", "LMaskTemplateSave", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
//            //    return;

//            //Mat m = new Mat(GV.LeftBackCam.Grab(), skLeft.GetMaskRectangle());

//            //_recipe.LeftLowMaskMat = m;
//            //pbLBackMask.Image = m.ToBitmap();
//            //CvInvoke.Imwrite(GetTemplateFileName("LLM"), m);
//            //GrayImage d = new GrayImage(m.Width, m.Height);
//            //d.Fill(255);
//            //_recipe.LeftLowMaskMask = d;
//            //d.Save(GetTemplateFileName("MLLM"), System.Drawing.Imaging.ImageFormat.Bmp);
//            //_AlignC.LLMaskAlgorithm = Algoritm(cbLTopMaskAlgo.SelectedIndex);

//            //GM.WriteAlignConditionsAcarToXml("AlignConditions.xml");
//        }

//        private void BtRWaferTempalteSave_Click(object sender, EventArgs e)
//        {
//            Mat m;
//            Mat fullImage;
//            GV.TickCount = 0;
//            if (isLive)
//            {
//                fullImage = GV.RightBackCam.Grab();
//            }
//            else
//            {
//                fullImage = RViewMat?.Clone();
//            }
//            if (fullImage == null || fullImage.Empty())
//            {
//                MessageBox.Show("相機尚未取得有效影像");
//                return;
//            }
//            OpenCvSharp.Rect roi = skRight.GetWaferRect();
//            if (roi.Width <= 0 || roi.Height <= 0 ||
//                roi.X < 0 || roi.Y < 0 || roi.X + roi.Width > fullImage.Width || roi.Y + roi.Height > fullImage.Height)
//            {
//                MessageBox.Show("遮罩區域無效或超出影像範圍");
//                return;
//            }
//            m = new Mat(fullImage, roi);
//            //Mat m = new Mat(GV.RightBackCam.Grab(), skRight.GetWaferRectangle());
//            pbRWafer.Image?.Dispose();
//            pbRWafer.Image = m.ToBitmap();

//            if (MessageBox.Show(GV.Dlang.strRWafer, GV.Dlang.strRWaferSave, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
//            {
//                pbRWafer.Image?.Dispose();
//                if (!RHWaferMat.Empty() && RHWaferMat != null)
//                {
//                    pbRWafer.Image = RHWaferMat.ToBitmap();
//                }
//                m.Dispose();
//                fullImage.Dispose();
//                //pbRWafer.Image = _recipe.RightHighWaferMat.ToBitmap();
//                return;
//            }

//            _AlignC.RBWaferBright = tBRightBackWaferLight.Value;
//            RHWaferMat = m;
//            Cv2.ImWrite(GetTemplateFileName("RHW"), m);
//            GrayImage d = new GrayImage(m.Width, m.Height);
//            d.Fill(255);
//            _recipeT.RightHighWaferMask = d;
//            d.Save(GetTemplateFileName("MRHW"), System.Drawing.Imaging.ImageFormat.Bmp);
//            _AlignC.RHWaferAlgorithm = Algoritm(cbRWaferAlgo.SelectedIndex);
//            GV.matcherRHW.LearnWithAlgo(m, d, _AlignC.RHWaferAlgorithm);
//            _AlignC.LastModifyTime = DateTime.Now;
//            GM.WriteRecipeXml(GV._recipe.FileName, GV._recipe);
//        }

//        private void btRMaskTemplateSave_Click(object sender, EventArgs e)
//        {
//            Mat m;
//            Mat fullImage;
//            GV.TickCount = 0;
//            if (isLive)
//            {
//                fullImage = GV.RightBackCam.Grab();
//            }
//            else
//            {
//                fullImage = RViewMat?.Clone();
//            }
//            if (fullImage == null || fullImage.Empty())
//            {
//                MessageBox.Show("相機尚未取得有效影像");
//                return;
//            }
//            OpenCvSharp.Rect roi = skRight.GetMaskRect();
//            if (roi.Width <= 0 || roi.Height <= 0 ||
//                roi.X < 0 || roi.Y < 0 || roi.X + roi.Width > fullImage.Width || roi.Y + roi.Height > fullImage.Height)
//            {
//                MessageBox.Show("遮罩區域無效或超出影像範圍");
//                return;
//            }
//            m = new Mat(fullImage, roi);
//            pbRBackMask.Image?.Dispose();
//            //Mat m = new Mat(GV.RightBackCam.Grab(), skRight.GetMaskRect());
//            pbRBackMask.Image = m.ToBitmap();

//            if (MessageBox.Show(GV.Dlang.strRBottomMask, GV.Dlang.strRBottomMaskSave, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
//            {
//                pbRBackMask.Image?.Dispose();
//                if (!RLWaferMat.Empty() && RLWaferMat != null)
//                {
//                    pbRBackMask.Image = RLWaferMat.ToBitmap();
//                }
//                //pbRBackMask.Image = _recipe.RightLowWaferMat.ToBitmap();
//                m.Dispose();
//                fullImage.Dispose();
//                return;
//            }

//            _AlignC.RBMaskBright = tBRightBackMaskLight.Value;
//            RHMaskMat = m;
//            Cv2.ImWrite(GetTemplateFileName("RHM"), m);
//            //RightMaskMat = GV.RightBackCam.Grab();
//            //GV.RightMaskMat = RightMaskMat;
//            //_recipe.SetRightLowWaferMat(m);
//            GrayImage d = new GrayImage(m.Width, m.Height);
//            d.Fill(255);
//            _recipeT.RightHighMaskMask = d;
//            d.Save(GetTemplateFileName("MRHM"), System.Drawing.Imaging.ImageFormat.Bmp);
//            _AlignC.RLWaferAlgorithm = Algoritm(cbRBackMaskAlgo.SelectedIndex);
//            GV.matcherRLW.LearnWithAlgo(m, d, _AlignC.RLWaferAlgorithm);
//            if (!BtRMaskAdjSave.Visible)
//            {
//                if (GV.Plc.ReadData16(13028) == 0)
//                {
//                    _AlignC.RMaskAdjX = GV.AppSettingParm.RxShift4;
//                    _AlignC.RMaskAdjY = GV.AppSettingParm.RyShift4;
//                }
//                else
//                {
//                    _AlignC.RMaskAdjX = GV.AppSettingParm.RxShift6;
//                    _AlignC.RMaskAdjY = GV.AppSettingParm.RyShift6;
//                }
//            }
//            _AlignC.LastModifyTime = DateTime.Now;
//            GM.WriteRecipeXml(GV._recipe.FileName, GV._recipe);
//        }

//        //private void BtLMaskLocationUp_Click(object sender, EventArgs e)
//        //{
//        //    // LeftLearnPatternVideoWindow.MaskMove(0, -1);
//        //}

//        //private void BtRWaferLocationUp_Click(object sender, EventArgs e)
//        //{
//        //    // RightLearnPatternVideoWindow.WaferMove(0, -1);
//        //}

//        private void btRWaferLocationDown_Click(object sender, EventArgs e)
//        {
//            // RightLearnPatternVideoWindow.WaferMove(0, 1);
//        }

//        private void btRWaferLocationLeft_Click(object sender, EventArgs e)
//        {
//            // RightLearnPatternVideoWindow.WaferMove(-1, 0);
//        }

//        private void btRWaferLocationRight_Click(object sender, EventArgs e)
//        {
//            // RightLearnPatternVideoWindow.WaferMove(1, 0);
//        }

//        private void ButtonCreateLWaferPatternMask_Click(object sender, EventArgs e)
//        {
//            if (_recipeT.LeftHighWaferMat != null)
//            {
//                if (_recipeT.LeftHighWaferMask == null)
//                {
//                    _recipeT.LeftHighWaferMask = new GrayImage(_recipeT.LeftHighWaferMat.Width, _recipeT.LeftHighWaferMat.Height);
//                    _recipeT.LeftHighWaferMask.Fill(255);
//                }
//                DialogPaintMask dialogPaintMask = new DialogPaintMask()
//                {
//                    Image = _recipeT.LeftHighWaferMat,
//                    Mask = _recipeT.LeftHighWaferMask
//                };
//                if (dialogPaintMask.ShowDialog() == DialogResult.OK)
//                {
//                    _recipeT.LeftHighWaferMask = dialogPaintMask.Mask;
//                    GV.matcherLHW.LearnWithAlgo(_recipeT.LeftHighWaferMat, _recipeT.LeftHighWaferMask, _AlignC.LHWaferAlgorithm); ;
//                    _AlignC.LastModifyTime = DateTime.Now;
//                    GM.WriteRecipeXml(GV._recipe.FileName,GV._recipe);
//                }
//            }
//        }

//        private void RbLTopMask_CheckedChanged(object sender, EventArgs e)
//        {
//            if (rbLTopMask.Checked)
//            {
//                GV.LeftUpCam.Freeze();
//                GV.LeftBackCam.Freeze();
//                GV.skView = 0;
//                tBLShowImage.Enabled = false;
//                tBLeftTopMaskLight.Value = _AlignC.LeftBrightness[NowMagni];
//                btLBackMaskSave.Enabled = false;
//                // btLBackMask.Enabled = false;
//                btLWaferSave.Enabled = false;
//                btLBackMaskLSave.Enabled = false;
//                btLBackMaskLightMinus.Enabled = false;
//                btLBackMaskLightPlus.Enabled = false;
//                btLWaferLightMinus.Enabled = false;
//                btLWaferLightPlus.Enabled = false;
//                btLWaferLSave.Enabled = false;
//                ucNavigatorWaferL.Enabled = false;
//                ucNavigatorBackMaskL.Enabled = false;
//                // btLWaferMask.Enabled = false;
//                rbLBackMask.Checked = false;
//                rbLWafer.Checked = false;
//                if (GV.UserLevel == GV.User.Administrator)
//                {
//                }
//                else
//                {
//                    tBLeftBackMaskLight.Value = 0;
//                    tBLeftBackWaferLight.Value = 0;
//                    tBLeftBackMaskLight.Enabled = false;
//                    tBLeftBackWaferLight.Enabled = false;
//                }
//                cbLBackMaskAlgo.Enabled = false;
//                cbLWaferAlgo.Enabled = false;
//                Thread.Sleep(SleepTime);
//                // gBMag.Enabled = true;
//                rBLowMagnification.Enabled = true;
//                rBHighMagnification.Enabled = true;
//                if (GV.UserLevel == GV.User.Operator)
//                {
//                    ucNavigatorTopMaskL.Enabled = false;
//                    btLTopMaskSave.Enabled = false;
//                    cbLTopMaskAlgo.Enabled = false;
//                    btLTopMaskLSave.Enabled = false;
//                    btLTopMaskLightMinus.Enabled = false;
//                    btLTopMaskLightPlus.Enabled = false;
//                    tBLeftTopMaskLight.Enabled = false;
//                    btLTopMaskLSave.Enabled = false;
//                    cbHighMagnification.Enabled = false;
//                    cbLowMagnification.Enabled = false;
//                    btSave.Enabled = false;
//                }
//                else
//                {
//                    btSave.Enabled = true;
//                    ucNavigatorTopMaskL.Enabled = true;
//                    btLTopMaskSave.Enabled = true;
//                    cbLTopMaskAlgo.Enabled = true;
//                    btLTopMaskLSave.Enabled = true;
//                    btLTopMaskLightMinus.Enabled = true;
//                    btLTopMaskLightPlus.Enabled = true;
//                    tBLeftTopMaskLight.Enabled = true;
//                    btLTopMaskLSave.Enabled = true;
//                    if (rBHighMagnification.Checked) cbHighMagnification.Enabled = true;
//                    if (rBLowMagnification.Checked) cbLowMagnification.Enabled = true;
//                }
//                btLTopPhotoMask.Enabled = true;
//                GV.Light.ChangeBrightness("left", tBLeftTopMaskLight.Value);
//                if (LeftMaskMat != null)
//                    skLeft.SetShowMask(false, _dLalpha, LeftMaskMat);
//                GV.LeftUpCam.SetWindow(skLeft);
//                GV.LeftUpCam.Live();

//                if (!rbRTopMask.Checked)
//                {
//                    rbRTopMask.Checked = true;
//                }

//                Update();
//            }
//        }

//        private void rbLBackMask_CheckedChanged(object sender, EventArgs e)
//        {
//            if (rbLBackMask.Checked)
//            {
//                GV.LeftUpCam.Freeze();
//                GV.LeftBackCam.Freeze();
//                GV.skView = 1;
//                tBLeftBackMaskLight.Value = _AlignC.LBMaskBright;
//                btLTopMaskSave.Enabled = false;
//                btLTopMaskLSave.Enabled = false;
//                btLWaferSave.Enabled = false;
//                btLWaferMask.Enabled = false;
//                btFindLWaferCenter.Enabled = false;
//                btLWaferLSave.Enabled = false;
//                btLTopPhotoMask.Enabled = false;
//                // btLWaferMask.Enabled = false;
//                btLTopMaskLightPlus.Enabled = false;
//                btLTopMaskLightMinus.Enabled = false;
//                btLWaferLightPlus.Enabled = false;
//                btLWaferLightMinus.Enabled = false;
//                rbLTopMask.Checked = false;
//                rbLWafer.Checked = false;
//                tBLeftTopMaskLight.Value = 0;
//                tBLeftTopMaskLight.Enabled = false;
//                tBLeftBackWaferLight.Enabled = false;
//                ucNavigatorTopMaskL.Enabled = false;
//                ucNavigatorWaferL.Enabled = false;
//                // gBMag.Enabled = false;
//                cbLTopMaskAlgo.Enabled = false;
//                cbLWaferAlgo.Enabled = false;
//                // Thread.Sleep(SleepTime);
//                if (GV.UserLevel == GV.User.Operator)
//                {
//                    ucNavigatorBackMaskL.Enabled = false;
//                    btLBackMaskSave.Enabled = false;
//                    cbLBackMaskAlgo.Enabled = false;
//                    btLBackMaskLSave.Enabled = false;
//                    btLBackMaskLightMinus.Enabled = false;
//                    btLBackMaskLightPlus.Enabled = false;
//                    tBLeftBackMaskLight.Enabled = false;
//                    btLBackMaskLSave.Enabled = false;
//                }
//                else
//                {
//                    ucNavigatorBackMaskL.Enabled = true;
//                    btLBackMaskSave.Enabled = true;
//                    cbLBackMaskAlgo.Enabled = true;
//                    btLBackMaskLSave.Enabled = true;
//                    btLBackMaskLightMinus.Enabled = true;
//                    btLBackMaskLightPlus.Enabled = true;
//                    tBLeftBackMaskLight.Enabled = true;
//                    btLBackMaskLSave.Enabled = true;
//                }
//                tBLShowImage.Enabled = true;
//                GV.Light.ChangeBrightness("leftback", tBLeftBackMaskLight.Value);
//                GV.LeftBackCam.SetWindow(skLeft);
//                skLeft.SetShowMask(true, _dLalpha, LeftMaskMat);

//                GV.LeftBackCam.Live();
//                if (!rbRBackMask.Checked)
//                {
//                    rbRBackMask.Checked = true;
//                }

//                Update();
//            }
//        }

//        private void rbWafer_Click(object sender, EventArgs e)
//        {
//            rbLWafer.Checked = true;
//            rbRWafer.Checked = true;
//            rbLWaferChanged();
//            rbRWaferChanged();
//        }

//        private void rbLWafer_CheckedChanged(object sender, EventArgs e)
//        {
//            if (rbLWafer.Checked)
//            {
//                GV.LeftUpCam.Freeze();
//                GV.LeftBackCam.Freeze();
//                tBLeftBackWaferLight.Value = _AlignC.LBWaferBright;
//                btLBackMaskSave.Enabled = false;
//                // btLBackMask.Enabled = false;
//                btLTopMaskSave.Enabled = false;
//                btLTopPhotoMask.Enabled = false;
//                btLTopMaskLSave.Enabled = false;
//                btLBackMaskLSave.Enabled = false;
//                btLTopMaskLightPlus.Enabled = false;
//                btLTopMaskLightMinus.Enabled = false;
//                btLBackMaskLightPlus.Enabled = false;
//                btLBackMaskLightMinus.Enabled = false;
//                rbLTopMask.Checked = false;
//                rbLBackMask.Checked = false;
//                tBLeftTopMaskLight.Value = 0;
//                tBLeftTopMaskLight.Enabled = false;
//                tBLeftBackMaskLight.Enabled = false;
//                ucNavigatorTopMaskL.Enabled = false;
//                ucNavigatorBackMaskL.Enabled = false;
//                rBHighMagnification.Enabled = false;
//                rBLowMagnification.Enabled = false;
//                cbHighMagnification.Enabled = false;
//                cbLowMagnification.Enabled = false;
//                cbLTopMaskAlgo.Enabled = false;
//                cbLBackMaskAlgo.Enabled = false;
//                // Thread.Sleep(SleepTime);
//                if (GV.UserLevel == GV.User.Operator)
//                {
//                    ucNavigatorWaferL.Enabled = false;
//                    btLWaferSave.Enabled = false;
//                    cbLWaferAlgo.Enabled = false;
//                    btLWaferLSave.Enabled = false;
//                    btLWaferLightMinus.Enabled = false;
//                    btLWaferLightPlus.Enabled = false;
//                    tBLeftBackWaferLight.Enabled = false;
//                    btLWaferLSave.Enabled = false;
//                    btMagSave.Enabled = false;
//                    btSave.Enabled = false;
//                }
//                else
//                {
//                    btSave.Enabled = true;
//                    ucNavigatorWaferL.Enabled = true;
//                    btLWaferSave.Enabled = true;
//                    cbLWaferAlgo.Enabled = true;
//                    btLWaferLSave.Enabled = true;
//                    btLWaferLightMinus.Enabled = true;
//                    btLWaferLightPlus.Enabled = true;
//                    tBLeftBackWaferLight.Enabled = true;
//                    btLWaferLSave.Enabled = true;
//                    btMagSave.Enabled = true;
//                }
//                tBLShowImage.Enabled = true;
//                GV.Light.ChangeBrightness("leftback", tBLeftBackWaferLight.Value);
//                GV.LeftBackCam.SetWindow(skLeft);
//                skLeft.SetShowMask(true, _dLalpha, LeftMaskMat);
//                GV.LeftBackCam.Live();

//                if (!rbRWafer.Checked)
//                {
//                    rbRWafer.Checked = true;
//                }

//                Update();
//            }
//        }

//        private void rbLWaferChanged()
//        {
//            GV.LeftUpCam.Freeze();
//            GV.LeftBackCam.Freeze();
//            tBLeftBackWaferLight.Value = _AlignC.LBWaferBright;
//            btLBackMaskSave.Enabled = false;
//            btLBackMask.Enabled = false;
//            btFindLBMaskCenter.Enabled = false;
//            btLTopMaskSave.Enabled = false;
//            btLTopPhotoMask.Enabled = false;
//            btLTopMaskLSave.Enabled = false;
//            btLBackMaskLSave.Enabled = false;
//            btLTopMaskLightPlus.Enabled = false;
//            btLTopMaskLightMinus.Enabled = false;
//            btLBackMaskLightPlus.Enabled = false;
//            btLBackMaskLightMinus.Enabled = false;
//            rbLTopMask.Checked = false;
//            rbLBackMask.Checked = false;
//            tBLeftTopMaskLight.Value = 0;
//            tBLeftTopMaskLight.Enabled = false;
//            tBLeftBackMaskLight.Enabled = false;
//            ucNavigatorTopMaskL.Enabled = false;
//            ucNavigatorBackMaskL.Enabled = false;
//            // gBMag.Enabled = false;
//            cbLTopMaskAlgo.Enabled = false;
//            cbLBackMaskAlgo.Enabled = false;
//            if (GV.UserLevel == GV.User.Operator)
//            {
//                ucNavigatorWaferL.Enabled = false;
//                btLWaferSave.Enabled = false;
//                cbLWaferAlgo.Enabled = false;
//                btLWaferLSave.Enabled = false;
//                btLWaferLightMinus.Enabled = false;
//                btLWaferLightPlus.Enabled = false;
//                tBLeftBackWaferLight.Enabled = false;
//                btLWaferLSave.Enabled = false;
//                btLWaferMask.Enabled = false;
//                btFindLWaferCenter.Enabled = false;
//                btSave.Enabled = false;
//            }
//            else
//            {
//                btSave.Enabled = true;
//                ucNavigatorWaferL.Enabled = true;
//                btLWaferSave.Enabled = true;
//                cbLWaferAlgo.Enabled = true;
//                btLWaferLSave.Enabled = true;
//                btLWaferLightMinus.Enabled = true;
//                btLWaferLightPlus.Enabled = true;
//                tBLeftBackWaferLight.Enabled = true;
//                btLWaferLSave.Enabled = true;
//                btLWaferMask.Enabled = true;
//                btFindLWaferCenter.Enabled = true;
//            }
//            tBLShowImage.Enabled = true;
//            GV.Light.ChangeBrightness("leftback", tBLeftBackWaferLight.Value);
//            GV.LeftBackCam.SetWindow(skLeft);
//            skLeft.SetShowMask(true, _dLalpha, LeftMaskMat);
//            GV.LeftBackCam.Live();
//        }

//        private void rbRWaferChanged()
//        {
//            GV.RightUpCam.Freeze();
//            GV.RightBackCam.Freeze();
//            tBRightBackWaferLight.Value = _AlignC.RBWaferBright;
//            btRBackMaskSave.Enabled = false;
//            btRBackMask.Enabled = false;
//            btFindRBMaskCenter.Enabled = false;
//            btRTopMaskSave.Enabled = false;
//            btRTopPhotoMask.Enabled = false;
//            btRTopMaskLSave.Enabled = false;
//            btRBackMaskLSave.Enabled = false;
//            btRBackMaskLightPlus.Enabled = false;
//            btRBackMaskLightMinus.Enabled = false;
//            btRTopMaskLightPlus.Enabled = false;
//            btRTopMaskLightMinus.Enabled = false;
//            rbRTopMask.Checked = false;
//            rbRBackMask.Checked = false;
//            tBRightTopMaskLight.Value = 0;
//            // tBRightBackMaskLight.Value = 0;
//            tBRightTopMaskLight.Enabled = false;
//            tBRightBackMaskLight.Enabled = false;
//            ucNavigatorTopMaskR.Enabled = false;
//            ucNavigatorBackMaskR.Enabled = false;
//            cbRBackMaskAlgo.Enabled = false;
//            cbRTopMaskAlgo.Enabled = false;
//            rBHighMagnification.Enabled = false;
//            rBLowMagnification.Enabled = false;
//            cbHighMagnification.Enabled = false;
//            cbLowMagnification.Enabled = false;
//            if (GV.UserLevel == GV.User.Operator)
//            {
//                ucNavigatorWaferR.Enabled = false;
//                btRWaferSave.Enabled = false;
//                cbRWaferAlgo.Enabled = false;
//                btRWaferLSave.Enabled = false;
//                btRWaferLightMinus.Enabled = false;
//                btRWaferLightPlus.Enabled = false;
//                tBRightBackWaferLight.Enabled = false;
//                btRWaferLSave.Enabled = false;
//                btMagSave.Enabled = false;
//                btRWaferMask.Enabled = false;
//                btFindRWaferCenter.Enabled = false;
//            }
//            else
//            {
//                ucNavigatorWaferR.Enabled = true;
//                btRWaferSave.Enabled = true;
//                cbRWaferAlgo.Enabled = true;
//                btRWaferLSave.Enabled = true;
//                btRWaferLightMinus.Enabled = true;
//                btRWaferLightPlus.Enabled = true;
//                tBRightBackWaferLight.Enabled = true;
//                btRWaferLSave.Enabled = true;
//                btMagSave.Enabled = true;
//                btRWaferMask.Enabled = true;
//                btFindRWaferCenter.Enabled = true;
//            }
//            tBRShowImage.Enabled = true;
//            GV.Light.ChangeBrightness("rightback", tBRightBackWaferLight.Value);
//            GV.RightBackCam.SetWindow(skRight);
//            skRight.SetShowMask(true, _dRalpha, RightMaskMat);
//            GV.RightBackCam.Live();
//        }

//        private void rbBackMask_Click(object sender, EventArgs e)
//        {
//            rbLBackMask.Checked = true;
//            rbRBackMask.Checked = true;
//            rbLBackMaskChanged();
//            rbRBackMaskChanged();
//        }

//        private void rbLBackMaskChanged()
//        {
//            GV.LeftUpCam.Freeze();
//            GV.LeftBackCam.Freeze();
//            GV.skView = 1;
//            tBLeftBackMaskLight.Value = _AlignC.LBMaskBright;
//            btLTopMaskSave.Enabled = false;
//            btLTopMaskLSave.Enabled = false;
//            btLWaferSave.Enabled = false;
//            btLWaferMask.Enabled = false;
//            btFindLWaferCenter.Enabled = false;
//            btLWaferLSave.Enabled = false;
//            btLTopPhotoMask.Enabled = false;
//            // btLWaferMask.Enabled = false;
//            btLTopMaskLightPlus.Enabled = false;
//            btLTopMaskLightMinus.Enabled = false;
//            btLWaferLightPlus.Enabled = false;
//            btLWaferLightMinus.Enabled = false;
//            rbLTopMask.Checked = false;
//            rbLWafer.Checked = false;
//            tBLeftTopMaskLight.Value = 0;
//            tBLeftTopMaskLight.Enabled = false;
//            tBLeftBackWaferLight.Enabled = false;
//            ucNavigatorTopMaskL.Enabled = false;
//            ucNavigatorWaferL.Enabled = false;
//            // gBMag.Enabled = false;
//            rBHighMagnification.Enabled = false;
//            rBLowMagnification.Enabled = false;
//            cbHighMagnification.Enabled = false;
//            cbLowMagnification.Enabled = false;
//            cbLTopMaskAlgo.Enabled = false;
//            cbLWaferAlgo.Enabled = false;
//            // Thread.Sleep(SleepTime);
//            if (GV.UserLevel == GV.User.Operator)
//            {
//                ucNavigatorBackMaskL.Enabled = false;
//                btLBackMaskSave.Enabled = false;
//                cbLBackMaskAlgo.Enabled = false;
//                btLBackMaskLSave.Enabled = false;
//                btLBackMaskLightMinus.Enabled = false;
//                btLBackMaskLightPlus.Enabled = false;
//                tBLeftBackMaskLight.Enabled = false;
//                btLBackMaskLSave.Enabled = false;
//                btMagSave.Enabled = false;
//                btLBackMask.Enabled = false;
//                btFindLBMaskCenter.Enabled = false;
//                btSave.Enabled = false;
//            }
//            else
//            {
//                btSave.Enabled = true;
//                ucNavigatorBackMaskL.Enabled = true;
//                btLBackMaskSave.Enabled = true;
//                cbLBackMaskAlgo.Enabled = true;
//                btLBackMaskLSave.Enabled = true;
//                btLBackMaskLightMinus.Enabled = true;
//                btLBackMaskLightPlus.Enabled = true;
//                tBLeftBackMaskLight.Enabled = true;
//                btLBackMaskLSave.Enabled = true;
//                btMagSave.Enabled = true;
//                btLBackMask.Enabled = true;
//                btFindLBMaskCenter.Enabled = true;
//            }
//            tBLShowImage.Enabled = true;
//            GV.Light.ChangeBrightness("leftback", tBLeftBackMaskLight.Value);
//            GV.LeftBackCam.SetWindow(skLeft);
//            skLeft.SetShowMask(true, _dLalpha, LeftMaskMat);

//            GV.LeftBackCam.Live();
//        }

//        private void rbRBackMaskChanged()
//        {
//            GV.RightUpCam.Freeze();
//            GV.RightBackCam.Freeze();
//            tBRightBackMaskLight.Value = _AlignC.RBMaskBright;
//            btRTopMaskSave.Enabled = false;
//            btRWaferSave.Enabled = false;
//            btRWaferMask.Enabled = false;
//            btFindRWaferCenter.Enabled = false;
//            btRTopPhotoMask.Enabled = false;
//            // btRWaferMask.Enabled = false;
//            btRWaferLSave.Enabled = false;
//            btRTopMaskLSave.Enabled = false;
//            btRTopMaskLightPlus.Enabled = false;
//            btRTopMaskLightMinus.Enabled = false;
//            btRWaferLightPlus.Enabled = false;
//            btRWaferLightMinus.Enabled = false;
//            rbRTopMask.Checked = false;
//            rbRWafer.Checked = false;
//            tBRightTopMaskLight.Value = 0;
//            tBRightTopMaskLight.Enabled = false;
//            tBRightBackWaferLight.Enabled = false;
//            ucNavigatorTopMaskR.Enabled = false;
//            ucNavigatorWaferR.Enabled = false;
//            cbRTopMaskAlgo.Enabled = false;
//            cbRWaferAlgo.Enabled = false;
//            rBHighMagnification.Enabled = false;
//            rBLowMagnification.Enabled = false;
//            cbHighMagnification.Enabled = false;
//            cbLowMagnification.Enabled = false;
//            if (GV.UserLevel == GV.User.Operator)
//            {
//                ucNavigatorBackMaskR.Enabled = false;
//                btRBackMaskSave.Enabled = false;
//                cbRBackMaskAlgo.Enabled = false;
//                btRBackMaskLSave.Enabled = false;
//                btRBackMaskLightMinus.Enabled = false;
//                btRBackMaskLightPlus.Enabled = false;
//                tBRightBackMaskLight.Enabled = false;
//                btRBackMaskLSave.Enabled = false;
//                btMagSave.Enabled = false;
//                btRBackMask.Enabled = false;
//                btFindRBMaskCenter.Enabled = false;
//            }
//            else
//            {
//                ucNavigatorBackMaskR.Enabled = true;
//                btRBackMaskSave.Enabled = true;
//                cbRBackMaskAlgo.Enabled = true;
//                btRBackMaskLSave.Enabled = true;
//                btRBackMaskLightMinus.Enabled = true;
//                btRBackMaskLightPlus.Enabled = true;
//                tBRightBackMaskLight.Enabled = true;
//                btRBackMaskLSave.Enabled = true;
//                btMagSave.Enabled = true;
//                btRBackMask.Enabled = true;
//                btFindRBMaskCenter.Enabled = true;
//            }
//            tBRShowImage.Enabled = true;
//            GV.Light.ChangeBrightness("rightback", tBRightBackMaskLight.Value);
//            GV.RightBackCam.SetWindow(skRight);
//            skRight.SetShowMask(true, _dRalpha, RightMaskMat);
//            GV.RightBackCam.Live();
//        }

//        private void rbRWafer_CheckedChanged(object sender, EventArgs e)
//        {
//            if (rbRWafer.Checked)
//            {
//                GV.RightUpCam.Freeze();
//                GV.RightBackCam.Freeze();
//                tBRightBackWaferLight.Value = _AlignC.RBWaferBright;
//                btRBackMaskSave.Enabled = false;
//                // btRBackMask.Enabled = false;
//                btRTopMaskSave.Enabled = false;
//                btRTopPhotoMask.Enabled = false;
//                btRTopMaskLSave.Enabled = false;
//                btRBackMaskLSave.Enabled = false;
//                btRBackMaskLightPlus.Enabled = false;
//                btRBackMaskLightMinus.Enabled = false;
//                btRTopMaskLightPlus.Enabled = false;
//                btRTopMaskLightMinus.Enabled = false;
//                rbRTopMask.Checked = false;
//                rbRBackMask.Checked = false;
//                tBRightTopMaskLight.Value = 0;
//                // tBRightBackMaskLight.Value = 0;
//                tBRightTopMaskLight.Enabled = false;
//                tBRightBackMaskLight.Enabled = false;
//                ucNavigatorTopMaskR.Enabled = false;
//                ucNavigatorBackMaskR.Enabled = false;
//                cbRBackMaskAlgo.Enabled = false;
//                cbRTopMaskAlgo.Enabled = false;
//                // Thread.Sleep(SleepTime);
//                if (GV.UserLevel == GV.User.Operator)
//                {
//                    ucNavigatorWaferR.Enabled = false;
//                    btRWaferSave.Enabled = false;
//                    cbRWaferAlgo.Enabled = false;
//                    btRWaferLSave.Enabled = false;
//                    btRWaferLightMinus.Enabled = false;
//                    btRWaferLightPlus.Enabled = false;
//                    tBRightBackWaferLight.Enabled = false;
//                    btRWaferLSave.Enabled = false;
//                }
//                else
//                {
//                    ucNavigatorWaferR.Enabled = true;
//                    btRWaferSave.Enabled = true;
//                    cbRWaferAlgo.Enabled = true;
//                    btRWaferLSave.Enabled = true;
//                    btRWaferLightMinus.Enabled = true;
//                    btRWaferLightPlus.Enabled = true;
//                    tBRightBackWaferLight.Enabled = true;
//                    btRWaferLSave.Enabled = true;
//                }
//                tBRShowImage.Enabled = true;
//                GV.Light.ChangeBrightness("rightback", tBRightBackWaferLight.Value);
//                GV.RightBackCam.SetWindow(skRight);
//                skRight.SetShowMask(true, _dRalpha, RightMaskMat);
//                GV.RightBackCam.Live();

//                if (!rbLWafer.Checked)
//                {
//                    rbLWafer.Checked = true;
//                }

//                Update();
//            }
//        }

//        private void rbRBackMask_CheckedChanged(object sender, EventArgs e)
//        {
//            if (rbRBackMask.Checked)
//            {
//                GV.RightUpCam.Freeze();
//                GV.RightBackCam.Freeze();
//                tBRightBackMaskLight.Value = _AlignC.RBMaskBright;
//                btRTopMaskSave.Enabled = false;
//                btRWaferSave.Enabled = false;
//                btRTopPhotoMask.Enabled = false;
//                // btRWaferMask.Enabled = false;
//                btRWaferLSave.Enabled = false;
//                btRTopMaskLSave.Enabled = false;
//                btRTopMaskLightPlus.Enabled = false;
//                btRTopMaskLightMinus.Enabled = false;
//                btRWaferLightPlus.Enabled = false;
//                btRWaferLightMinus.Enabled = false;
//                rbRTopMask.Checked = false;
//                rbRWafer.Checked = false;
//                tBRightTopMaskLight.Value = 0;
//                tBRightTopMaskLight.Enabled = false;
//                tBRightBackWaferLight.Enabled = false;
//                ucNavigatorTopMaskR.Enabled = false;
//                ucNavigatorWaferR.Enabled = false;
//                cbRTopMaskAlgo.Enabled = false;
//                cbRWaferAlgo.Enabled = false;
//                // Thread.Sleep(SleepTime);
//                if (GV.UserLevel == GV.User.Operator)
//                {
//                    ucNavigatorBackMaskR.Enabled = false;
//                    btRBackMaskSave.Enabled = false;
//                    cbRBackMaskAlgo.Enabled = false;
//                    btRBackMaskLSave.Enabled = false;
//                    btRBackMaskLightMinus.Enabled = false;
//                    btRBackMaskLightPlus.Enabled = false;
//                    tBRightBackMaskLight.Enabled = false;
//                    btRBackMaskLSave.Enabled = false;
//                }
//                else
//                {
//                    ucNavigatorBackMaskR.Enabled = true;
//                    btRBackMaskSave.Enabled = true;
//                    cbRBackMaskAlgo.Enabled = true;
//                    btRBackMaskLSave.Enabled = true;
//                    btRBackMaskLightMinus.Enabled = true;
//                    btRBackMaskLightPlus.Enabled = true;
//                    tBRightBackMaskLight.Enabled = true;
//                    btRBackMaskLSave.Enabled = true;
//                }
//                tBRShowImage.Enabled = true;
//                GV.Light.ChangeBrightness("rightback", tBRightBackMaskLight.Value);
//                GV.RightBackCam.SetWindow(skRight);
//                skRight.SetShowMask(true, _dRalpha, RightMaskMat);
//                GV.RightBackCam.Live();

//                if (!rbLBackMask.Checked)
//                {
//                    rbLBackMask.Checked = true;
//                }

//                Update();
//            }
//        }

//        private void rbRTopMask_CheckedChanged(object sender, EventArgs e)
//        {
//            if (rbRTopMask.Checked)
//            {
//                GV.RightUpCam.Freeze();
//                GV.RightBackCam.Freeze();
//                tBRShowImage.Enabled = false;
//                tBRightTopMaskLight.Value = _AlignC.RightBrightness[NowMagni];
//                btRBackMaskSave.Enabled = false;
//                // btRBackMask.Enabled = false;
//                btRWaferSave.Enabled = false;
//                // btRWaferMask.Enabled = false;
//                btRBackMaskLSave.Enabled = false;
//                btRWaferLSave.Enabled = false;
//                btRWaferLightPlus.Enabled = false;
//                btRWaferLightMinus.Enabled = false;
//                btRBackMaskLightPlus.Enabled = false;
//                btRBackMaskLightMinus.Enabled = false;
//                rbRBackMask.Checked = false;
//                rbRWafer.Checked = false;
//                if (GV.UserLevel == GV.User.Administrator)
//                {
//                }
//                else
//                {
//                    tBRightBackMaskLight.Value = 0;
//                    tBRightBackWaferLight.Value = 0;
//                    tBRightBackMaskLight.Enabled = false;
//                    tBRightBackWaferLight.Enabled = false;
//                }
//                ucNavigatorBackMaskR.Enabled = false;
//                ucNavigatorWaferR.Enabled = false;
//                cbRWaferAlgo.Enabled = false;
//                cbRBackMaskAlgo.Enabled = false;
//                // Thread.Sleep(SleepTime);
//                if (GV.UserLevel == GV.User.Operator)
//                {
//                    ucNavigatorTopMaskR.Enabled = false;
//                    btRTopMaskSave.Enabled = false;
//                    cbRTopMaskAlgo.Enabled = false;
//                    btRTopMaskLSave.Enabled = false;
//                    btRTopMaskLightMinus.Enabled = false;
//                    btRTopMaskLightPlus.Enabled = false;
//                    tBRightTopMaskLight.Enabled = false;
//                    btRTopMaskLSave.Enabled = false;
//                }
//                else
//                {
//                    ucNavigatorTopMaskR.Enabled = true;
//                    btRTopMaskSave.Enabled = true;
//                    cbRTopMaskAlgo.Enabled = true;
//                    btRTopMaskLSave.Enabled = true;
//                    btRTopMaskLightMinus.Enabled = true;
//                    btRTopMaskLightPlus.Enabled = true;
//                    tBRightTopMaskLight.Enabled = true;
//                    btRTopMaskLSave.Enabled = true;
//                }
//                btRTopPhotoMask.Enabled = true;
//                GV.Light.ChangeBrightness("right", tBRightTopMaskLight.Value);
//                GV.RightUpCam.SetWindow(skRight);
//                if (RightMaskMat != null)
//                    skRight.SetShowMask(false, _dRalpha, RightMaskMat);
//                GV.RightUpCam.Live();

//                if (!rbLTopMask.Checked)
//                {
//                    rbLTopMask.Checked = true;
//                }

//                Update();
//            }
//        }

//        private void TBLeftTopMaskLight_ValueChanged(object sender, EventArgs e)
//        {
//            GV.Light.ChangeBrightness("left", tBLeftTopMaskLight.Value);
//        }

//        private void TBLeftBackMaskLight_ValueChanged(object sender, EventArgs e)
//        {
//            GV.Light.ChangeBrightness("leftback", tBLeftBackMaskLight.Value);
//        }

//        private void tBLeftBackWaferLight_ValueChanged(object sender, EventArgs e)
//        {
//            GV.Light.ChangeBrightness("leftback", tBLeftBackWaferLight.Value);
//        }

//        private void TBRightBackWaferLight_ValueChanged(object sender, EventArgs e)
//        {
//            GV.Light.ChangeBrightness("rightback", tBRightBackWaferLight.Value);
//        }

//        private void TBRightBackMaskLight_ValueChanged(object sender, EventArgs e)
//        {
//            GV.Light.ChangeBrightness("rightback", tBRightBackMaskLight.Value);
//        }

//        private void tBRightTopMaskLight_ValueChanged(object sender, EventArgs e)
//        {
//            GV.Light.ChangeBrightness("right", tBRightTopMaskLight.Value);
//        }

//        private void btLTopMaskSave_Click(object sender, EventArgs e)
//        {
//            //GV.TickCount = 0;

//            //Mat m = new Mat(GV.LeftUpCam.Grab(), skLeft.GetMaskRectangle());

//            //if (rBLowMagnification.Checked)
//            //{
//            //    // MessageBox1 msg1 = new MessageBox1(GV.Dlang.strLLTopMask, GV.Dlang.strLLTopMaskSave, MessageBoxIcon.Question);
//            //    // DialogResult m1 = msg1.ShowDialog();
//            //    // if (m1 != DialogResult.OK)
//            //    //    return;
//            //    if (MessageBox.Show(GV.Dlang.strLLTopMask, GV.Dlang.strLLTopMaskSave, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
//            //        return;

//            //    _recipe.LeftLowMaskMat = m;
//            //    pbLTopMask.Image = m.ToBitmap();
//            //    Cv2.ImWrite(GetTemplateFileName("LLM"), m);
//            //    GrayImage d = new GrayImage(m.Width, m.Height);
//            //    d.Fill(255);
//            //    _recipe.LeftLowMaskMask = d;
//            //    d.Save(GetTemplateFileName("MLLM"), System.Drawing.Imaging.ImageFormat.Bmp);
//            //    _AlignC.LLMaskAlgorithm = Algoritm(cbLTopMaskAlgo.SelectedIndex);
//            //}
//            //else if (rBHighMagnification.Checked)
//            //{
//            //    // MessageBox1 msg1 = new MessageBox1(GV.Dlang.strLHTopMask, GV.Dlang.strLHTopMaskSave, MessageBoxIcon.Question);
//            //    // DialogResult m1 = msg1.ShowDialog();
//            //    // if (m1 != DialogResult.OK)
//            //    //    return;
//            //    if (MessageBox.Show(GV.Dlang.strLHTopMask, GV.Dlang.strLHTopMaskSave, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
//            //        return;
//            //    _recipe.LeftHighMaskMat = m;
//            //    pbLTopMask.Image = m.ToBitmap();
//            //    Cv2.ImWrite(GetTemplateFileName("LHM"), m);
//            //    GrayImage d = new GrayImage(m.Width, m.Height);
//            //    d.Fill(255);
//            //    _recipe.LeftHighMaskMask = d;
//            //    d.Save(GetTemplateFileName("MLHM"), System.Drawing.Imaging.ImageFormat.Bmp);
//            //    _AlignC.LHMaskAlgorithm = Algoritm(cbLTopMaskAlgo.SelectedIndex);
//            //}

//            //_AlignC.LastModifyTime = DateTime.Now;
//            //GM.WriteAlignConditionsAcarToXml("AlignConditions.xml");
//        }

//        private void btRTopMaskSave_Click(object sender, EventArgs e)
//        {
//            //GV.TickCount = 0;

//            //Mat m = new Mat(GV.RightUpCam.Grab(), skRight.GetMaskRectangle());

//            //GrayImage d = new GrayImage(m.Width, m.Height);
//            //d.Fill(255);

//            //if (rBLowMagnification.Checked)
//            //{
//            //    // MessageBox1 msg1 = new MessageBox1(GV.Dlang.strRLTopMask, GV.Dlang.strRLTopMaskSave, MessageBoxIcon.Question);
//            //    // DialogResult m1 = msg1.ShowDialog();
//            //    // if (m1 != DialogResult.OK)
//            //    //    return;
//            //    if (MessageBox.Show(GV.Dlang.strRLTopMask, GV.Dlang.strRLTopMaskSave, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
//            //        return;

//            //    _recipe.RightLowMaskMat = m;
//            //    Cv2.ImWrite(GetTemplateFileName("RLM"), m);
//            //    _recipe.RightLowMaskMask = d;
//            //    d.Save(GetTemplateFileName("MRLM"), System.Drawing.Imaging.ImageFormat.Bmp);
//            //    _AlignC.RLMaskAlgorithm = Algoritm(cbRTopMaskAlgo.SelectedIndex);
//            //}
//            //else if (rBHighMagnification.Checked)
//            //{
//            //    // MessageBox1 msg1 = new MessageBox1(GV.Dlang.strRHTopMask, GV.Dlang.strRHTopMaskSave, MessageBoxIcon.Question);
//            //    // DialogResult m1 = msg1.ShowDialog();
//            //    // if (m1 != DialogResult.OK)
//            //    //    return;
//            //    if (MessageBox.Show(GV.Dlang.strRHTopMask, GV.Dlang.strRHTopMaskSave, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
//            //        return;

//            //    _recipe.RightHighMaskMat = m;
//            //    Cv2.ImWrite(GetTemplateFileName("RHM"), m);
//            //    _recipe.RightHighMaskMask = d;
//            //    d.Save(GetTemplateFileName("MRHM"), System.Drawing.Imaging.ImageFormat.Bmp);
//            //    _AlignC.RHMaskAlgorithm = Algoritm(cbRTopMaskAlgo.SelectedIndex);
//            //}

//            //pbRTopMask.Image = m.ToBitmap();
//            //_AlignC.LastModifyTime = DateTime.Now;
//            //GM.WriteAlignConditionsAcarToXml("AlignConditions.xml");
//        }

//        private void btLRingLightMinus_Click(object sender, EventArgs e)
//        {
//            if (tBLeftTopMaskLight.Value > 1) tBLeftTopMaskLight.Value--;
//            CheckParam();
//        }

//        private void btLRingLightPlus_Click(object sender, EventArgs e)
//        {
//            if (tBLeftTopMaskLight.Value < 254) tBLeftTopMaskLight.Value++;
//            CheckParam();
//        }

//        private void btLBackMaskLightPlus_Click(object sender, EventArgs e)
//        {
//            if (tBLeftBackMaskLight.Value < 254) tBLeftBackMaskLight.Value++;
//            CheckParam();
//        }

//        private void btLWaferLightPlus_Click(object sender, EventArgs e)
//        {
//            if (tBLeftBackWaferLight.Value < 254) tBLeftBackWaferLight.Value++;
//            CheckParam();
//        }

//        private void btLWaferLightMinus_Click(object sender, EventArgs e)
//        {
//            if (tBLeftBackWaferLight.Value > 1) tBLeftBackWaferLight.Value--;
//            CheckParam();
//        }

//        private void btLBackMaskLightMinus_Click(object sender, EventArgs e)
//        {
//            if (tBLeftBackMaskLight.Value > 1) tBLeftBackMaskLight.Value--;
//            CheckParam();
//        }

//        private void btRWaferLightPlus_Click(object sender, EventArgs e)
//        {
//            if (tBRightBackWaferLight.Value < 254) tBRightBackWaferLight.Value++;
//            CheckParam();
//        }

//        private void btRWaferLightMinus_Click(object sender, EventArgs e)
//        {
//            if (tBRightBackWaferLight.Value > 1) tBRightBackWaferLight.Value--;
//            CheckParam();
//        }

//        private void btRBackMaskLightPlus_Click(object sender, EventArgs e)
//        {
//            if (tBRightBackMaskLight.Value < 254) tBRightBackMaskLight.Value++;
//            CheckParam();
//        }

//        private void btRBackMaskLightMinus_Click(object sender, EventArgs e)
//        {
//            if (tBRightBackMaskLight.Value > 1) tBRightBackMaskLight.Value--;
//            CheckParam();
//        }

//        private void btRTopMaskLightPlus_Click(object sender, EventArgs e)
//        {
//            if (tBRightTopMaskLight.Value < 254) tBRightTopMaskLight.Value++;
//            CheckParam();
//        }

//        private void BtRTopMaskLightMinus_Click(object sender, EventArgs e)
//        {
//            if (tBRightTopMaskLight.Value > 1) tBRightTopMaskLight.Value -= 1;
//            CheckParam();
//        }

//        private void BtLBackMaskSave_Click(object sender, EventArgs e)
//        {
//            GV.TickCount = 0;
//            Mat m = new Mat(GV.LeftBackCam.Grab(), skLeft.GetMaskRect());
//            pbLBackMask.Image = m.ToBitmap();

//            if (MessageBox.Show(GV.Dlang.strLBottomMask, GV.Dlang.strLBottomMaskSave, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
//            {
//                pbLBackMask.Image = _recipe.LeftLowWaferMat.ToBitmap();
//                return;
//            }

//            _AlignC.LBMaskBright = tBLeftBackMaskLight.Value;

//            LeftMaskMat = GV.LeftBackCam.Grab();
//            GV.LeftMaskMat = LeftMaskMat;
//            _recipe.SetLeftLowWaferMat(m);
//            GrayImage d = new GrayImage(m.Width, m.Height);
//            d.Fill(255);
//            _recipe.LeftLowWaferMask = d;
//            _AlignC.LLWaferAlgorithm = Algoritm(cbLBackMaskAlgo.SelectedIndex);
//            GV.matcherLLW.LearnWithAlgo(m, d, _AlignC.LLWaferAlgorithm);
//            if (!BtLMaskAdjSave.Visible)
//            {
//                if (GV.Plc.ReadData16(13028) == 0)
//                {
//                    _AlignC.LMaskAdjX = GV.AppSettingParm.LxShift4;
//                    _AlignC.LMaskAdjY = GV.AppSettingParm.LyShift4;
//                }
//                else
//                {
//                    _AlignC.LMaskAdjX = GV.AppSettingParm.LxShift6;
//                    _AlignC.LMaskAdjY = GV.AppSettingParm.LyShift6;
//                }
//            }
//            _AlignC.LastModifyTime = DateTime.Now;
//            GM.WriteRecipeXml(EditRecipe);
//        }

//        public void ShowMe()
//        {
//            LeftMaskMat = GV.LeftMaskMat.Clone();
//            RightMaskMat = GV.RightMaskMat.Clone();
//            GV.RingLight.ChangeBrightness("left", 0);
//            GV.RingLight.ChangeBrightness("right", 0);
//            iAdmin = GV.HwndFormMain.iAdmin;

//            if (DrawMatchThread == null)
//            {
//                DrawMatchThread = new Thread(DrawMatchPosition);
//                DrawMatchThread.Start();
//            }
//            GV.LeftBackCam.SetWindow(skLeft);
//            GV.RightBackCam.SetWindow(skRight);

//            // 啟動相機即時預覽
//            GV.LeftBackCam.Live();
//            GV.RightBackCam.Live();

//            if (GV.skView == 0)
//            {
//                //rbLTopMask.Checked = false;
//                //rbRTopMask.Checked = false;
//                //// Thread.Sleep(100);
//                //rbLTopMask.Checked = true;
//                //if (rBLowMagnification.Checked == true)
//                //{
//                //    NowMagni = cbLowMagnification.SelectedIndex;
//                //    cbHighMagnification.Enabled = false;
//                //} else
//                //{
//                //    NowMagni = cbHighMagnification.SelectedIndex;
//                //    cbLowMagnification.Enabled = false;
//                //}
//                // ChangeLensMagnificationWait(NowMagni);
//                // tBLeftTopMaskLight.Value = _AlignC.LeftBrightness[NowMagni];
//                // tBRightTopMaskLight.Value = _AlignC.RightBrightness[NowMagni];

//                rbLBackMask.Checked = true;
//                rbRBackMask.Checked = true;
//                rbLBackMaskChanged();
//                rbRBackMaskChanged();
//                GV.NowWaferMask = 1;
//            }
//            else if (GV.skView == 1)
//            {
//                if (GV.NowWaferMask == 1)
//                {
//                    rbLBackMask.Checked = true;
//                    rbRBackMask.Checked = true;
//                    rbLBackMaskChanged();
//                    rbRBackMaskChanged();
//                }
//                else if (GV.NowWaferMask == 2)
//                {
//                    rbLWafer.Checked = true;
//                    rbRWafer.Checked = true;
//                    rbLWaferChanged();
//                    rbRWaferChanged();
//                }
//            }

//            if (_AlignC.UpBotMask == 1)
//            {
//                cBBottomMask.Checked = false;
//                cBTopMask.Checked = true;
//            }
//            else
//            {
//                cBBottomMask.Checked = true;
//                cBTopMask.Checked = false;
//            }

//            if (_AlignC.AdjuestZ == 1)
//            {
//                cBAdjZ.Checked = true;
//            }
//            else
//            {
//                cBAdjZ.Checked = false;
//            }

//            //cBAdjZ.Checked = true;
//            NUDLMaskX.Value = _AlignC.LMaskAdjX;
//            NUDLMaskY.Value = _AlignC.LMaskAdjY;
//            NUDRMaskX.Value = _AlignC.RMaskAdjX;
//            NUDRMaskY.Value = _AlignC.RMaskAdjY;

//            groupBox4.Location = new System.Drawing.Point(560, 60);

//            CheckParam();
//        }

//        private void rBLowMagnification_CheckedChanged(object sender, EventArgs e)
//        {

//        }

//        private void rBHighMagnification_CheckedChanged(object sender, EventArgs e)
//        {
//        }

//        private void cbLowMagnification_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            if (rBLowMagnification.Checked)
//            {
//                rBHighMagnification.Enabled = false;
//                rBLowMagnification.Enabled = false;
//                cbHighMagnification.Enabled = false;
//                cbLowMagnification.Enabled = false;

//                NowMagni = cbLowMagnification.SelectedIndex;
//                cbHLMagni_Change(NowMagni);
//                tBLeftTopMaskLight.Value = _AlignC.LeftBrightness[NowMagni];
//                tBRightTopMaskLight.Value = _AlignC.RightBrightness[NowMagni];

//                Thread.Sleep(250);

//                rBHighMagnification.Enabled = true;
//                rBLowMagnification.Enabled = true;
//                if (GV.UserLevel != GV.User.Operator)
//                {
//                    cbHighMagnification.Enabled = true;
//                    cbLowMagnification.Enabled = true;
//                }
//            }
//            CheckParam();
//        }

//        private void cbHighMagnification_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            if (rBHighMagnification.Checked)
//            {
//                rBLowMagnification.Enabled = false;
//                rBHighMagnification.Enabled = false;
//                cbLowMagnification.Enabled = false;
//                cbHighMagnification.Enabled = false;

//                NowMagni = cbHighMagnification.SelectedIndex;
//                cbHLMagni_Change(NowMagni);
//                tBLeftTopMaskLight.Value = _AlignC.LeftBrightness[NowMagni];
//                tBRightTopMaskLight.Value = _AlignC.RightBrightness[NowMagni];

//                Thread.Sleep(250);
//                rBLowMagnification.Enabled = true;
//                rBHighMagnification.Enabled = true;
//                if (GV.UserLevel != GV.User.Operator)
//                {
//                    cbLowMagnification.Enabled = true;
//                    cbHighMagnification.Enabled = true;
//                }
//            }
//            CheckParam();
//        }

//        private async void cbHLMagni_Change(int NowMagn)
//        {
//            await Task.Run(() => ChangeLensMagnificationWait(NowMagn));
//        }

//        private void ChangeLensMagnification(string side, int motormagnification)
//        {
//            try
//            {
//                if (side == "left")
//                {
//                    if (!GV.LeftZoomLens.MoveGoto(motormagnification))
//                    {
//                        // GV.Plc.Send("WR DM6400.S 1");
//                        throw new MRException("Error : Zoom lens fail.");
//                    }
//                }

//                if (side == "right")
//                {
//                    if (!GV.RightZoomLens.MoveGoto(motormagnification))
//                    {
//                        // GV.Plc.Send("WR DM6400.S 1");
//                        throw new MRException("Error : Zoom lens fail.");
//                    }
//                }
//            }
//            catch
//            {
//                Debug.WriteLine("ChangeLensMagnification fail");
//                throw;
//            }
//        }

//        private void ChangeLensMagnificationWait(int motormagnification)
//        {
//            try
//            {
//                GV.LeftZoomLens.Magnification = motormagnification;
//                GV.RightZoomLens.Magnification = motormagnification;

//                GV.LeftZoomLens.MoveGoto(GV.ZoomLensInfo.LeftMagnificationMotorSteps[motormagnification]);
//                GV.RightZoomLens.MoveGoto(GV.ZoomLensInfo.RightMagnificationMotorSteps[motormagnification]);

//                Stopwatch sw = Stopwatch.StartNew();
//                while (true)
//                {
//                    Thread.Sleep(100);
//                    if (GV.LeftZoomLens.GetStatus() && GV.RightZoomLens.GetStatus())
//                        return;
//                    if (sw.ElapsedMilliseconds > 4500)
//                    {
//                        //GV.LeftZoomLens.SetStatus();
//                        //GV.RightZoomLens.SetStatus();
//                        break;
//                    }
//                }
//            }
//            catch (Exception)
//            {
//                Debug.WriteLine("ChangeLensMagnification fail");
//                throw;
//            }
//        }

//        private void tBLShowImage_ValueChanged(object sender, EventArgs e)
//        {
//            // iLalpha = tBLShowImage.Value;
//            _dLalpha = (double)tBLShowImage.Value / 100;
//            _AlignC.dLalpha = _dLalpha;
//            if (rbLTopMask.Checked == false)
//            {
//                skLeft.SetShowMask(true, _dLalpha, LeftMaskMat);
//            }
//            else
//            {
//                skLeft.SetShowMask(false, 0, LeftMaskMat);
//            }
//        }

//        private void ucNavigatorTopMaskL_CommandPressed(int dir)
//        {
//            int x = 0;
//            int y = 0;
//            switch (dir)
//            {
//                case 0:
//                    x++;
//                    break;
//                case 1:
//                    y--;
//                    break;
//                case 2:
//                    x--;
//                    break;
//                case 3:
//                    y++;
//                    break;
//            }
//            skLeft.MaskMove(x, y);
//        }

//        private void ucNavigatorWaferL_CommandPressed(int dir)
//        {
//            int x = 0;
//            int y = 0;
//            switch (dir)
//            {
//                case 0:
//                    x++;
//                    break;
//                case 1:
//                    y--;
//                    break;
//                case 2:
//                    x--;
//                    break;
//                case 3:
//                    y++;
//                    break;
//            }
//            skLeft.WaferMove(x, y);
//        }

//        private void ucNavigatorWaferR_CommandPressed(int dir)
//        {
//            int x = 0;
//            int y = 0;
//            switch (dir)
//            {
//                case 0:
//                    x++;
//                    break;
//                case 1:
//                    y--;
//                    break;
//                case 2:
//                    x--;
//                    break;
//                case 3:
//                    y++;
//                    break;
//            }
//            skRight.WaferMove(x, y);
//        }

//        private void ucNavigatorTopMaskR_CommandPressed(int dir)
//        {
//            int x = 0;
//            int y = 0;
//            switch (dir)
//            {
//                case 0:
//                    x++;
//                    break;
//                case 1:
//                    y--;
//                    break;
//                case 2:
//                    x--;
//                    break;
//                case 3:
//                    y++;
//                    break;
//            }
//            skRight.MaskMove(x, y);
//        }

//        private void btLTopMaskView_Click(object sender, EventArgs e)
//        {
//            if (rBLowMagnification.Checked)
//            {
//                if (MessageBox.Show("Sure to save Left Low Magnification Light ?", "Left Low Mag Light", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
//                    return;

//                _AlignC.LeftLight[NowMagni] = true;
//                _AlignC.LeftBrightness[NowMagni] = tBLeftTopMaskLight.Value;
//                _AlignC.AlignLowMagnification = NowMagni;
//            }
//            else
//            {
//                if (MessageBox.Show("Sure to save Left High Magnification Light ?", "Left High Mag Light", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
//                    return;

//                _AlignC.LeftLight[NowMagni] = true;
//                _AlignC.LeftBrightness[NowMagni] = tBLeftTopMaskLight.Value;
//                _AlignC.AlignHighMagnification = NowMagni;
//            }

//            _AlignC.LastModifyTime = DateTime.Now;
//            GM.WriteAlignConditionsAcarToXml("AlignConditions.xml");
//        }

//        private void btLBackMaskView_Click(object sender, EventArgs e)
//        {
//            if (MessageBox.Show("Sure to save Left Bottom Mask Light ?", "Left Bottom Mask Light", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
//                return;

//            _AlignC.LBMaskBright = tBLeftBackMaskLight.Value;
//            _AlignC.LastModifyTime = DateTime.Now;
//            GM.WriteAlignConditionsAcarToXml("AlignConditions.xml");
//        }

//        private void btLWaferView_Click(object sender, EventArgs e)
//        {
//            if (MessageBox.Show("Sure to save Left Bottom Wafer Light ?", "Left Bottom Wafer Light", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
//                return;

//            _AlignC.LBWaferBright = tBLeftBackWaferLight.Value;
//            _AlignC.LastModifyTime = DateTime.Now;
//            GM.WriteAlignConditionsAcarToXml("AlignConditions.xml");
//        }

//        private void btRTopMaskLSave_Click(object sender, EventArgs e)
//        {
//            if (rBLowMagnification.Checked)
//            {
//                if (MessageBox.Show("Sure to save Right Low Magnification Light ?", "Right Low Mag Light", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
//                    return;

//                _AlignC.RightLight[NowMagni] = true;
//                _AlignC.RightBrightness[NowMagni] = tBRightTopMaskLight.Value;
//                _AlignC.AlignLowMagnification = NowMagni;
//            }
//            else
//            {
//                if (MessageBox.Show("Sure to save Right High Magnification Light ?", "Right High Mag Light", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
//                    return;

//                _AlignC.RightLight[NowMagni] = true;
//                _AlignC.RightBrightness[NowMagni] = tBRightTopMaskLight.Value;
//                _AlignC.AlignHighMagnification = NowMagni;
//            }

//            _AlignC.LastModifyTime = DateTime.Now;
//            GM.WriteAlignConditionsAcarToXml("AlignConditions.xml");
//        }

//        private void btRBackMaskLSave_Click(object sender, EventArgs e)
//        {
//            if (MessageBox.Show("Sure to save Right Bottom Mask Light ?", "Right Bottom Mask Light", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
//                return;

//            _AlignC.RBMaskBright = tBRightBackMaskLight.Value;
//            _AlignC.LastModifyTime = DateTime.Now;
//            GM.WriteAlignConditionsAcarToXml("AlignConditions.xml");
//        }

//        private void btRWaferLSave_Click(object sender, EventArgs e)
//        {
//            if (MessageBox.Show("Sure to save Right Bottom Wafer Light ?", "Right Bottom Wafer Light", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
//                return;

//            _AlignC.RBWaferBright = tBRightBackWaferLight.Value;
//            _AlignC.LastModifyTime = DateTime.Now;
//            GM.WriteAlignConditionsAcarToXml("AlignConditions.xml");
//        }

//        private void btMagSave_Click(object sender, EventArgs e)
//        {
//            // int Lmag = cbLowMagnification.SelectedIndex + 1;
//            // int Hmag = cbHighMagnification.SelectedIndex + 1;

//            // string smsg = "Sure to save \r\nLow Magnification = " + Lmag.ToString() + "\r\nHigh Magnification = " + Hmag.ToString() + "\r\nAnd Set Recipe to BOT Alignmode!!";


//            if (rbLTopMask.Checked)
//            {
//                if (rBHighMagnification.Checked)
//                {
//                    // MessageBox1 msg1 = new MessageBox1(GV.Dlang.strLightHTopMask, GV.Dlang.strLightHTopMaskSave, MessageBoxIcon.Question);
//                    // DialogResult m1 = msg1.ShowDialog();
//                    // if (m1 != DialogResult.OK)
//                    //     return;
//                    if (MessageBox.Show(GV.Dlang.strLightHTopMask, GV.Dlang.strLightHTopMaskSave, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
//                        return;

//                    _AlignC.AlignHighMagnification = cbHighMagnification.SelectedIndex;
//                    _AlignC.LeftLight[NowMagni] = true;
//                    _AlignC.LeftBrightness[NowMagni] = tBLeftTopMaskLight.Value;
//                    _AlignC.RightLight[NowMagni] = true;
//                    _AlignC.RightBrightness[NowMagni] = tBRightTopMaskLight.Value;
//                    _AlignC.LHMaskAlgorithm = Algoritm(cbLTopMaskAlgo.SelectedIndex);
//                    _AlignC.RHMaskAlgorithm = Algoritm(cbRTopMaskAlgo.SelectedIndex);
//                }
//                else if (rBLowMagnification.Checked)
//                {
//                    // MessageBox1 msg1 = new MessageBox1(GV.Dlang.strLightLTopMask, GV.Dlang.strLightLTopMaskSave, MessageBoxIcon.Question);
//                    // DialogResult m1 = msg1.ShowDialog();
//                    // if (m1 != DialogResult.OK)
//                    //   return;
//                    if (MessageBox.Show(GV.Dlang.strLightLTopMask, GV.Dlang.strLightLTopMaskSave, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
//                        return;
//                    _AlignC.AlignLowMagnification = cbLowMagnification.SelectedIndex;
//                    _AlignC.LeftLight[NowMagni] = true;
//                    _AlignC.LeftBrightness[NowMagni] = tBLeftTopMaskLight.Value;
//                    _AlignC.RightLight[NowMagni] = true;
//                    _AlignC.RightBrightness[NowMagni] = tBRightTopMaskLight.Value;
//                    _AlignC.LLMaskAlgorithm = Algoritm(cbLTopMaskAlgo.SelectedIndex);
//                    _AlignC.RLMaskAlgorithm = Algoritm(cbRTopMaskAlgo.SelectedIndex);
//                }
//            }
//            else if (rbLBackMask.Checked)
//            {
//                // MessageBox1 msg1 = new MessageBox1(GV.Dlang.strLightBottomMask, GV.Dlang.strLightBottomMaskSave, MessageBoxIcon.Question);
//                // DialogResult m1 = msg1.ShowDialog();
//                // if (m1 != DialogResult.OK)
//                //    return;
//                if (MessageBox.Show(GV.Dlang.strLightBottomMask, GV.Dlang.strLightBottomMaskSave, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
//                    return;

//                _AlignC.LBMaskBright = tBLeftBackMaskLight.Value;
//                _AlignC.RBMaskBright = tBRightBackMaskLight.Value;
//                _AlignC.dLalpha = (double)tBLShowImage.Value / 100;
//                _AlignC.dRalpha = (double)tBRShowImage.Value / 100;
//                iLalpha = tBLShowImage.Value;
//                iRalpha = tBRShowImage.Value;
//                _AlignC.LLWaferAlgorithm = Algoritm(cbLBackMaskAlgo.SelectedIndex);
//                _AlignC.RLWaferAlgorithm = Algoritm(cbRBackMaskAlgo.SelectedIndex);
//            }
//            else if (rbLWafer.Checked)
//            {
//                // MessageBox1 msg1 = new MessageBox1(GV.Dlang.strLightWafer, GV.Dlang.strLightWaferSave, MessageBoxIcon.Question);
//                // DialogResult m1 = msg1.ShowDialog();
//                // if (m1 != DialogResult.OK)
//                //     return;
//                if (MessageBox.Show(GV.Dlang.strLightWafer, GV.Dlang.strLightWaferSave, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
//                    return;

//                _AlignC.LBWaferBright = tBLeftBackWaferLight.Value;
//                _AlignC.RBWaferBright = tBRightBackWaferLight.Value;
//                _AlignC.dLalpha = (double)tBLShowImage.Value / 100;
//                _AlignC.dRalpha = (double)tBRShowImage.Value / 100;
//                iLalpha = tBLShowImage.Value;
//                iRalpha = tBRShowImage.Value;
//                _AlignC.LHWaferAlgorithm = Algoritm(cbLWaferAlgo.SelectedIndex);
//                _AlignC.RHWaferAlgorithm = Algoritm(cbRWaferAlgo.SelectedIndex);
//            }

//            _AlignC.UpBackAlign = 2;
//            _AlignC.LastModifyTime = DateTime.Now;
//            GM.WriteAlignConditionsAcarToXml("AlignConditions.xml");
//            CheckParam();
//        }

//        private void CheckParam()
//        {
//            GV.TickCount = 0;

//            Color Ocolor = btSave.BackColor;
//            Color Ncolor = btSave.BackColor;

//            if ((rbLBackMask.Checked) && (_AlignC.LBMaskBright != tBLeftBackMaskLight.Value))
//            {
//                Ncolor = Color.LightGreen;
//            }
//            else if ((rbLWafer.Checked) && (_AlignC.LBWaferBright != tBLeftBackWaferLight.Value))
//            {
//                Ncolor = Color.LightGreen;
//            }
//            else if ((rbRBackMask.Checked) && (_AlignC.RBMaskBright != tBRightBackMaskLight.Value))
//            {
//                Ncolor = Color.LightGreen;
//            }
//            else if ((rbRWafer.Checked) && (_AlignC.RBWaferBright != tBRightBackWaferLight.Value))
//            {
//                Ncolor = Color.LightGreen;
//            }
//            else if ((tBLShowImage.Enabled) && (iLalpha != tBLShowImage.Value))
//            {
//                Ncolor = Color.LightGreen;
//            }
//            else if ((tBRShowImage.Enabled) && (iRalpha != tBRShowImage.Value))
//            {
//                Ncolor = Color.LightGreen;
//            }
//            else if ((rbLBackMask.Checked) && (_AlignC.LLWaferAlgorithm != Algoritm(cbLBackMaskAlgo.SelectedIndex)))
//            {
//                Ncolor = Color.LightGreen;
//            }
//            else if ((rbRBackMask.Checked) && (_AlignC.RLWaferAlgorithm != Algoritm(cbRBackMaskAlgo.SelectedIndex)))
//            {
//                Ncolor = Color.LightGreen;
//            }
//            else if ((rbLWafer.Checked) && (_AlignC.LHWaferAlgorithm != Algoritm(cbLWaferAlgo.SelectedIndex)))
//            {
//                Ncolor = Color.LightGreen;
//            }
//            else if ((rbRWafer.Checked) && (_AlignC.RHWaferAlgorithm != Algoritm(cbRWaferAlgo.SelectedIndex)))
//            {
//                Ncolor = Color.LightGreen;
//            }
//            else if (_AlignC.UpBackAlign != 2)
//            {
//                Ncolor = Color.LightGreen;
//            }
//            else
//            {
//                Ncolor = Color.MintCream;
//            }

//            if (Ncolor != Ocolor)
//            {
//                btSave.BackColor = Ncolor;
//                Update();
//            }
//        }

//        private void btLShowImageSave_Click(object sender, EventArgs e)
//        {
//            if (MessageBox.Show("Sure to save Left Show Mask Image ?", "Left Show Mask Image", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
//                return;

//            _AlignC.dLalpha = (double)tBLShowImage.Value / 100;
//            _AlignC.LastModifyTime = DateTime.Now;
//            GM.WriteAlignConditionsAcarToXml("AlignConditions.xml");
//        }

//        private void btRShowImageSave_Click(object sender, EventArgs e)
//        {
//            if (MessageBox.Show("Sure to save Left Show Mask Image ?", "Left Show Mask Image", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
//                return;

//            _AlignC.dRalpha = (double)tBRShowImage.Value / 100;
//            _AlignC.LastModifyTime = DateTime.Now;
//            GM.WriteAlignConditionsAcarToXml("AlignConditions.xml");
//        }
//        private void tBRShowImage_ValueChanged(object sender, EventArgs e)
//        {
//            // iRalpha = tBRShowImage.Value;
//            _dRalpha = (double)tBRShowImage.Value / 100;
//            _AlignC.dRalpha = _dRalpha;
//            if (rbRTopMask.Checked == false)
//            {
//                skRight.SetShowMask(true, _dRalpha, RightMaskMat);
//            }
//            else
//            {
//                skRight.SetShowMask(false, 0, RightMaskMat);
//            }
//        }

//        private void btDelete_Click(object sender, EventArgs e)
//        {
//            string smsg = String.Format(GV.Dlang.strSureDeleteRecipe, EditRecipe.ToString());
//            if (MessageBox.Show(smsg, GV.Dlang.strDeleteRecipe, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
//                return;

//            // MessageBox1 msg1 = new MessageBox1(smsg, GV.Dlang.strDeleteRecipe, MessageBoxIcon.Question);
//            // DialogResult m1 = msg1.ShowDialog();
//            // if (m1 != DialogResult.OK)
//            //    return;

//            AlignCondition newOne = new AlignCondition();
//            newOne.CopyTo(EditRecipe);

//            _AlignC.UpBackAlign = 2;
//            _AlignC.LastModifyTime = DateTime.Now;

//            _recipe.LeftLowMaskMat = null;
//            _recipe.LeftLowWaferMat = null;
//            _recipe.RightLowMaskMat = null;
//            _recipe.RightLowWaferMat = null;

//            _recipe.LeftHighMaskMat = null;
//            _recipe.LeftHighWaferMat = null;
//            _recipe.RightHighMaskMat = null;
//            _recipe.RightHighWaferMat = null;

//            deleteTemplateFile("LLM");
//            deleteTemplateFile("LHM");
//            deleteTemplateFile("RLM");
//            deleteTemplateFile("RHM");
//            deleteTemplateFile("LLW");
//            deleteTemplateFile("LHW");
//            deleteTemplateFile("RLW");
//            deleteTemplateFile("RHW");
//            deleteTemplateFile("LeftMask");
//            deleteTemplateFile("RightMask");

//            rBLowMagnification.Checked = false;
//            rBHighMagnification.Checked = false;
//            cbHighMagnification.SelectedIndex = _AlignC.AlignHighMagnification;
//            cbLowMagnification.SelectedIndex = _AlignC.AlignLowMagnification;

//            GM.WriteAlignConditionsAcarToXml("AlignConditions.xml");

//            rBLowMagnification.Checked = true;

//            CheckLevel();
//            ShowMe();
//        }

//        /*
//        private void timerNowTime_Tick(object sender, EventArgs e)
//        {
//            lbNowTime.Text = DateTime.Now.ToShortDateString() + DateTime.Now.ToString("    HH:mm:ss ") + GV.UserLevel.ToString();
//        }
//        */

//        public string setTimeString
//        {
//            get
//            {
//                return _ClockTime;
//            }

//            set
//            {
//                _ClockTime = value;
//                lbNowTime.Text = value;
//            }
//        }

//        private void cbLTopMaskAlgo_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            CheckParam();
//        }

//        private void cbLBackMaskAlgo_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            CheckParam();
//        }

//        private void cbLWaferAlgo_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            CheckParam();
//        }

//        private void cbRWaferAlgo_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            CheckParam();
//        }

//        private void cbRBackMaskAlgo_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            CheckParam();
//        }

//        private void cbRTopMaskAlgo_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            CheckParam();
//        }

//        private void tBLShowImage_KeyUp(object sender, KeyEventArgs e)
//        {
//            CheckParam();
//        }

//        private void tBLShowImage_MouseUp(object sender, MouseEventArgs e)
//        {
//            CheckParam();
//        }

//        private void tBRShowImage_KeyUp(object sender, KeyEventArgs e)
//        {
//            CheckParam();
//        }

//        private void tBRShowImage_MouseUp(object sender, MouseEventArgs e)
//        {
//            CheckParam();
//        }

//        private void tBLeftTopMaskLight_MouseUp(object sender, MouseEventArgs e)
//        {
//            CheckParam();
//        }

//        private void tBLeftTopMaskLight_KeyUp(object sender, KeyEventArgs e)
//        {
//            CheckParam();
//        }

//        private void tBLeftBackMaskLight_KeyUp(object sender, KeyEventArgs e)
//        {
//            CheckParam();
//        }

//        private void tBLeftBackMaskLight_MouseUp(object sender, MouseEventArgs e)
//        {
//            CheckParam();
//        }

//        private void tBLeftBackWaferLight_KeyUp(object sender, KeyEventArgs e)
//        {
//            CheckParam();
//        }

//        private void tBLeftBackWaferLight_MouseUp(object sender, MouseEventArgs e)
//        {
//            CheckParam();
//        }

//        private void tBRightBackWaferLight_KeyUp(object sender, KeyEventArgs e)
//        {
//            CheckParam();
//        }

//        private void tBRightBackWaferLight_MouseUp(object sender, MouseEventArgs e)
//        {
//            CheckParam();
//        }

//        private void tBRightBackMaskLight_KeyUp(object sender, KeyEventArgs e)
//        {
//            CheckParam();
//        }

//        private void tBRightBackMaskLight_MouseUp(object sender, MouseEventArgs e)
//        {
//            CheckParam();
//        }

//        private void tBRightTopMaskLight_KeyUp(object sender, KeyEventArgs e)
//        {
//            CheckParam();
//        }

//        private void tBRightTopMaskLight_MouseUp(object sender, MouseEventArgs e)
//        {
//            CheckParam();
//        }

//        private void btDefault_Click(object sender, EventArgs e)
//        {
//            int[] Brightness = { 10, 10, 10, 10, 10, 10, 11, 31, 26, 77, 110, 130 };

//            string smsg = string.Format(GV.Dlang.strSureDefaultRecipe, EditRecipe.ToString());
//            if (MessageBox.Show(smsg, GV.Dlang.strSetRecipe, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
//                return;
//            // MessageBox1 msg1 = new MessageBox1(smsg, GV.Dlang.strSetRecipe, MessageBoxIcon.Question);
//            // DialogResult m1 = msg1.ShowDialog();
//            // if (m1 != DialogResult.OK)
//            //     return;

//            for (int i = 0; i < 12; i++)
//            {
//                _AlignC.LeftLight[i] = true;
//                _AlignC.RightLight[i] = true;
//                _AlignC.LRingLight[i] = false;
//                _AlignC.RRingLight[i] = false;
//                _AlignC.LeftBrightness[i] = Brightness[i];
//                _AlignC.RightBrightness[i] = Brightness[i];
//                _AlignC.LeftRingBrightness[i] = Brightness[i];
//                _AlignC.RightRingBrightness[i] = Brightness[i];
//            }

//            _AlignC.LBMaskBright = 80;
//            _AlignC.RBMaskBright = 80;
//            _AlignC.LBWaferBright = 80;
//            _AlignC.RBWaferBright = 80;

//            GM.WriteAlignConditionsAcarToXml("AlignConditions.xml");

//            if (rbLTopMask.Checked)
//            {
//                if (rBHighMagnification.Checked)
//                {
//                    tBLeftTopMaskLight.Value = _AlignC.LeftBrightness[_AlignC.AlignHighMagnification];
//                    tBRightTopMaskLight.Value = _AlignC.RightBrightness[_AlignC.AlignHighMagnification];
//                }
//                else if (rBLowMagnification.Checked)
//                {
//                    tBLeftTopMaskLight.Value = _AlignC.LeftBrightness[_AlignC.AlignLowMagnification];
//                    tBRightTopMaskLight.Value = _AlignC.RightBrightness[_AlignC.AlignLowMagnification];
//                }
//            }
//            else if (rbLBackMask.Checked)
//            {
//                tBLeftBackMaskLight.Value = _AlignC.LBMaskBright;
//                tBRightBackMaskLight.Value = _AlignC.RBMaskBright;
//            }
//            else if (rbLWafer.Checked)
//            {
//                tBLeftBackWaferLight.Value = _AlignC.LBWaferBright;
//                tBRightBackMaskLight.Value = _AlignC.RBWaferBright;
//            }

//            CheckParam();
//        }

//        private void rBHighMagnification_Click(object sender, EventArgs e)
//        {
//            if (rBHighMagnification.Checked)
//            {
//                cbLowMagnification.Enabled = false;
//                cbHighMagnification.Enabled = false;

//                // gBMag.Enabled = false;

//                rBLowMagnification.Checked = false;
//                // rBLowMagnification.Enabled = false;
//                // rBHighMagnification.Enabled = false;
//                // if (!rbLTopMask.Checked) rbLTopMask.Checked = true;
//                NowMagni = cbHighMagnification.SelectedIndex;
//                ChangeLensMagnificationWait(NowMagni);
//                // await Task.Run(() => cbHLMagni_Change(NowMagni));
//                tBLeftTopMaskLight.Value = _AlignC.LeftBrightness[NowMagni];
//                tBRightTopMaskLight.Value = _AlignC.RightBrightness[NowMagni];
//                if (_recipe.LeftHighMaskMat != null)
//                {
//                    pbLTopMask.Image = _recipe.LeftHighMaskMat.ToBitmap();
//                }
//                else { pbLTopMask.Image = null; }

//                if (_recipe.RightHighMaskMat != null)
//                {
//                    pbRTopMask.Image = _recipe.RightHighMaskMat.ToBitmap();
//                }
//                else { pbRTopMask.Image = null; }

//                cbLTopMaskAlgo.SelectedIndex = (int)_AlignC.LHMaskAlgorithm;
//                cbRTopMaskAlgo.SelectedIndex = (int)_AlignC.RHMaskAlgorithm;

//                Thread.Sleep(250);
//                Update();

//                if (GV.UserLevel != GV.User.Operator)
//                    cbHighMagnification.Enabled = true;
//                else
//                    cbHighMagnification.Enabled = false;

//                Application.DoEvents();

//                rBHighMagnification.Enabled = true;
//                rBLowMagnification.Enabled = true;
//                CheckParam();
//            }
//        }

//        private void rBLowMagnification_Click(object sender, EventArgs e)
//        {
//            if (rBLowMagnification.Checked)
//            {
//                cbLowMagnification.Enabled = false;
//                cbHighMagnification.Enabled = false;
//                // gBMag.Enabled = false;

//                rBHighMagnification.Checked = false;
//                // rBHighMagnification.Enabled = false;
//                // rBLowMagnification.Enabled = false;
//                // if (!rbLTopMask.Checked) rbLTopMask.Checked = true;
//                NowMagni = cbLowMagnification.SelectedIndex;
//                ChangeLensMagnificationWait(NowMagni);
//                // await Task.Run(() => cbHLMagni_Change(NowMagni));
//                tBLeftTopMaskLight.Value = _AlignC.LeftBrightness[NowMagni];
//                tBRightTopMaskLight.Value = _AlignC.RightBrightness[NowMagni];
//                if (_recipe.LeftLowMaskMat != null)
//                {
//                    pbLTopMask.Image = _recipe.LeftLowMaskMat.ToBitmap();
//                }
//                else { pbLTopMask.Image = null; }
//                if (_recipe.RightLowMaskMat != null)
//                {
//                    pbRTopMask.Image = _recipe.RightLowMaskMat.ToBitmap();
//                }
//                else { pbRTopMask.Image = null; }

//                cbLTopMaskAlgo.SelectedIndex = (int)_AlignC.LLMaskAlgorithm;
//                cbRTopMaskAlgo.SelectedIndex = (int)_AlignC.RLMaskAlgorithm;

//                Update();
//                Thread.Sleep(250);
//                // gBMag.Enabled = true;
//                if (GV.UserLevel != GV.User.Operator)
//                    cbLowMagnification.Enabled = true;
//                else
//                    cbLowMagnification.Enabled = false;

//                Application.DoEvents();

//                rBLowMagnification.Enabled = true;
//                rBHighMagnification.Enabled = true;
//                CheckParam();
//            }
//        }

//        private void btOpen_Click(object sender, EventArgs e)
//        {
//            if (GV.thCCDPad == null)
//            {
//                GV.thCCDPad = new Thread(delegate ()
//                {
//                    GV.CCDPadForm = new CCDPad();
//                    GV.CCDPadForm.Show();
//                    System.Windows.Threading.Dispatcher.Run();
//                });

//                // StopCheckPLc = true;
//                GV.thCCDPad.SetApartmentState(ApartmentState.STA); // needs to be STA or throws exception
//                GV.thCCDPad.Start();
//            }
//            else
//            {
//                Invoke(new Action(() => GV.CCDPadForm.BringToFront()));
//            }
//        }

//        private void btFindLMaskCenter_Click(object sender, EventArgs e)
//        {
//            //Mat m = new Mat(GV.LeftUpCam.Grab(), skLeft.GetMaskRectangle());

//            //Point f = GV.matcherLLM.FindPatternCenter1(m);

//            //if ((f.X > 0) && (f.X < 2592) && (f.Y > 0) && (f.Y < 1944))
//            //{
//            //    int x0 = f.X - m.Width / 2;
//            //    int y0 = f.Y - m.Height / 2;

//            //    skLeft.MaskMove(x0, y0);
//            //}
//        }

//        private void btFindLBMaskCenter_Click(object sender, EventArgs e)
//        {
//            Mat m;
//            OpenCvSharp.Rect roi;
//            GV.TickCount = 0;

//            //if (_recipe.LeftLowWaferMat.IsEmpty)
//            //{
//            //    MessageBox.Show("請先學習光罩圖型，再找中心!!", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            //    return;
//            //}
//            if(LHMaskMat == null || LHMaskMat.Empty())
//            {
//                MessageBox.Show("請先學習光罩圖型，再找中心!!", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                return;
//            }
//            m = LHMaskMat.Clone();
//            DialogFindCenter FindCenter = new DialogFindCenter()
//            {
//                img = m.Clone(),
//                iMaskWaferRadio = 1.2
//            };

//            if (FindCenter.ShowDialog() == System.Windows.Forms.DialogResult.OK)
//            {
//                roi = FindCenter.rect;
//            }
//            else
//            {
//                m.Dispose();
//                return;
//            }
//            Mat newm = new Mat(m, roi);
//            pbLBackMask.Image?.Dispose();
//            pbLBackMask.Image = newm.ToBitmap();

//            if (MessageBox.Show(GV.Dlang.strbtnSaveLLM, GV.Dlang.strSureSaveLLM, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
//            {
//                pbLBackMask.Image?.Dispose();
//                pbLBackMask.Image = m.ToBitmap();
//                newm.Dispose();
//                m.Dispose();
//                return;
//            }
//            LHMaskMat = newm;
//            Cv2.ImWrite(GetTemplateFileName("LHM"), newm);
//            GrayImage d = new GrayImage(newm.Width, newm.Height);
//            d.Fill(255);
//            _recipeT.LeftHighMaskMask= d;
//            d.Save(GetTemplateFileName("MLHM"), System.Drawing.Imaging.ImageFormat.Bmp);
//            //_recipe.LeftLowWaferMask = d;
//            //_recipe.SetLeftLowWaferMat(newm);
//            _AlignC.LLWaferAlgorithm = Algoritm(cbLBackMaskAlgo.SelectedIndex);
//            GV.matcherLLW.LearnWithAlgo(newm, d, _AlignC.LLWaferAlgorithm);
//            _AlignC.LastModifyTime = DateTime.Now;
//            GM.WriteRecipeXml(GV._recipe.FileName,GV._recipe);
//        }

//        private void BtFindLWaferCenter_Click(object sender, EventArgs e)
//        {
//            Mat m;
//            OpenCvSharp.Rect roi;
//            GV.TickCount = 0;

//            if(LHWaferMat == null|| LHWaferMat.Empty())
//            {
//                MessageBox.Show("請先學習光罩圖型，再找中心!!", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                return;
//            }
//            m = LHWaferMat.Clone();

//            DialogFindCenter FindCenter = new DialogFindCenter()
//            {
//                img = m.Clone(),
//                iMaskWaferRadio = 0.8
//            };

//            if (FindCenter.ShowDialog() == System.Windows.Forms.DialogResult.OK)
//            {
//                roi = FindCenter.rect;
//            }
//            else
//            {
//                m.Dispose();
//                return;
//            }
//            Mat newm = new Mat(m, roi);
//            pbLWafer.Image?.Dispose();
//            pbLWafer.Image = newm.ToBitmap();

//            if (MessageBox.Show(GV.Dlang.strbtnSaveLLW, GV.Dlang.strSureSaveLLW, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
//            {
//                pbLWafer.Image?.Dispose();
//                pbLWafer.Image = m.ToBitmap();
//                newm.Dispose();
//                m.Dispose();
//                return;
//            }
//            LHWaferMat = newm;
//            Cv2.ImWrite(GetTemplateFileName("LHW"), newm);
//            //_recipe.SetLeftHighWaferMat(newm);
//            GrayImage d = new GrayImage(newm.Width, newm.Height);
//            d.Fill(255);
//            _recipeT.LeftHighWaferMask = d;
//            d.Save(GetTemplateFileName("MLHW"), System.Drawing.Imaging.ImageFormat.Bmp);
//            _AlignC.LHWaferAlgorithm = Algoritm(cbLWaferAlgo.SelectedIndex);
//            GV.matcherLHW.LearnWithAlgo(newm, d, _AlignC.LHWaferAlgorithm);
//            _AlignC.LastModifyTime = DateTime.Now;
//            GM.WriteRecipeXml(GV._recipe.FileName,GV._recipe);
//        }

//        private void BtFindRWaferCenter_Click(object sender, EventArgs e)
//        {
//            Mat m;
//            OpenCvSharp.Rect roi;
//            GV.TickCount = 0;

//            if (RHWaferMat == null || RHWaferMat.Empty())
//            {
//                MessageBox.Show("請先學習光罩圖型，再找中心!!", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                return;
//            }

//            m = RHWaferMat.Clone();

//            DialogFindCenter FindCenter = new DialogFindCenter()
//            {
//                img = m.Clone(),
//                iMaskWaferRadio = 0.8
//            };

//            if (FindCenter.ShowDialog() == System.Windows.Forms.DialogResult.OK)
//            {
//                roi = FindCenter.rect;
//            }
//            else
//            {
//                m.Dispose();
//                return;
//            }
//            Mat newm = new Mat(m, roi);
//            pbRWafer.Image?.Dispose();
//            pbRWafer.Image = newm.ToBitmap();

//            if (MessageBox.Show(GV.Dlang.strbtnSaveRLW, GV.Dlang.strSureSaveRLW, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
//            {
//                pbRWafer.Image?.Dispose();
//                pbRWafer.Image = m.ToBitmap();
//                newm.Dispose();
//                m.Dispose();
//                return;
//            }
//            RHWaferMat = newm;
//            Cv2.ImWrite(GetTemplateFileName("RHW"), newm);
//            //_recipe.SetRightHighWaferMat(newm);
//            GrayImage d = new GrayImage(newm.Width, newm.Height);
//            d.Fill(255);
//            _recipeT.RightHighWaferMask = d;
//            d.Save(GetTemplateFileName("MRHW"), System.Drawing.Imaging.ImageFormat.Bmp);
//            _AlignC.RHWaferAlgorithm = Algoritm(cbRWaferAlgo.SelectedIndex);
//            GV.matcherRHW.LearnWithAlgo(newm, d, _AlignC.RHWaferAlgorithm);
//            _AlignC.LastModifyTime = DateTime.Now;
//            GM.WriteRecipeXml(GV._recipe.FileName, GV._recipe);
//        }

//        private void BtFindRBMaskCenter_Click(object sender, EventArgs e)
//        {
//            Mat m;
//            OpenCvSharp.Rect roi;
//            GV.TickCount = 0;

//            //if (_recipe.RightLowWaferMat.IsEmpty)
//            //{
//            //    MessageBox.Show("請先學習光罩圖型，再找中心!!", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            //    return;
//            //}
//            if(RHMaskMat==null || RHMaskMat.Empty())
//            {
//                MessageBox.Show("請先學習光罩圖型，再找中心!!", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                return;
//            }
//            //m = _recipe.RightLowWaferMat;
//            m= RHMaskMat.Clone();
//            DialogFindCenter FindCenter = new DialogFindCenter()
//            {
//                img = m.Clone(),
//                iMaskWaferRadio = 1.2
//            };

//            if (FindCenter.ShowDialog() == System.Windows.Forms.DialogResult.OK)
//            {
//                roi = FindCenter.rect;
//            }
//            else
//            {
//                m.Dispose();
//                return;
//            }
//            Mat newm = new Mat(m, roi);
//            pbRBackMask.Image?.Dispose();
//            pbRBackMask.Image = newm.ToBitmap();

//            if (MessageBox.Show(GV.Dlang.strbtnSaveRLM, GV.Dlang.strSureSaveRLM, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
//            {
//                pbRBackMask.Image?.Dispose();
//                pbRBackMask.Image = m.ToBitmap();
//                newm.Dispose();
//                m.Dispose();
//                return;
//            }
//            RHMaskMat = newm;
//            Cv2.ImWrite(GetTemplateFileName("RHM"), newm);
//            GrayImage d = new GrayImage(newm.Width, newm.Height);
//            d.Fill(255);
//            _recipeT.RightHighMaskMask = d;
//            //_recipe.SetRightLowWaferMat(newm);
//            d.Save(GetTemplateFileName("MRHM"), System.Drawing.Imaging.ImageFormat.Bmp);
//            _AlignC.RLWaferAlgorithm = Algoritm(cbRBackMaskAlgo.SelectedIndex);
//            GV.matcherRLW.LearnWithAlgo(newm, d, _AlignC.RLWaferAlgorithm);
//            _AlignC.LastModifyTime = DateTime.Now;
//            GM.WriteRecipeXml(GV._recipe.FileName,GV._recipe);
//        }

//        private void BtFindRMaskCenter_Click(object sender, EventArgs e)
//        {
//            //Mat m = new Mat(GV.RightUpCam.Grab(), skRight.GetMaskRect());

//            //PointF f = GV.matcherRLM.FindPatternCenter1(m);

//            //if ((f.X > 0) && (f.X < 2592) && (f.Y > 0) && (f.Y < 1944))
//            //{
//            //    int x0 = (int)f.X - m.Width / 2;
//            //    int y0 = (int)f.Y - m.Height / 2;

//            //    skRight.MaskMove(x0, y0);
//            //}
//        }

//        //private void btReportLocation_Click(object sender, EventArgs e)
//        //{
//        //    skLeft.CanTrackPattern = true;
//        //    skRight.CanTrackPattern = true;

//        //    // skLeft.MaskMp = BAlignMatcher.Match(_recipe.LeftBackCam.Grab(), _recipe.LeftLowMaskMat);
//        //    skLeft.WaferMp = GV.matcher.Match(_recipe.LeftBackCam.Grab(), _recipe.LeftHighWaferMat);
//        //    // skRight.MaskMp = BAlignMatcher.Match(GV.RightUpCam.Grab(), GV.RightLowMaskMat);
//        //    skRight.WaferMp = GV.matcher.Match(GV.RightBackCam.Grab(), GV.RightHighWaferMat);

//        //    string msg1 = String.Format("Wafer Location X = {0}, Y = {1}, Score = {2}", skLeft.WaferMp.X, skLeft.WaferMp.Y, skLeft.WaferMp.Score) +
//        //        "\r\n" + String.Format("Mask Location X = {0}, Y = {1}, Score = {2}", LeftMaskMp.X, LeftMaskMp.Y, LeftMaskMp.Score);
//        //    lbLeftXYM.Text = msg1;

//        //    string msg2 = String.Format("Wafer Location X = {0}, Y = {1}, Score = {2}", skRight.WaferMp.X, skRight.WaferMp.Y, skRight.WaferMp.Score) +
//        //    "\r\n" + String.Format("Mask Location X = {0}, Y = {1}, Score = {2}", RightMaskMp.X, RightMaskMp.Y, RightMaskMp.Score);
//        //    lbRightXYM.Text = msg2;

//        //    GV.HwndFormMain.writeStatus("Left : " + msg1);
//        //    GV.HwndFormMain.writeStatus("Right : " + msg2);

//        //}

//        private void btLMaskSave_Click(object sender, EventArgs e)
//        {
//            GV.TickCount = 0;

//            Mat m = new Mat(GV.LeftBackCam.Grab(), skLeft.GetMaskRect());
//            GV.LeftCheckMat = m;
//            pbLBackMask.Image = m.ToBitmap();
//            Cv2.ImWrite(GetTemplateFileName("LCheck"), m);
//        }

//        private void btRMaskSave_Click(object sender, EventArgs e)
//        {
//            GV.TickCount = 0;

//            Mat m = new Mat(GV.RightBackCam.Grab(), skRight.GetMaskRect());
//            GV.RightCheckMat = m;
//            pbRBackMask.Image = m.ToBitmap();
//            Cv2.ImWrite(GetTemplateFileName("RCheck"), m);
//        }

//        private void bBTtMaskAlign_Click(object sender, EventArgs e)
//        {
//            MoveCameraBack(0);
//        }

//        public void deleteTemplateFile(string strf)
//        {
//            try
//            {
//                File.Copy(GetTemplateFileName(strf), GetTemplateFileNameD(strf));
//                File.Delete(GetTemplateFileName(strf));
//            }
//            catch (Exception)
//            {
//            }
//        }

//        private void btBTAlignWafer_Click(object sender, EventArgs e)
//        {
//            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, true);

//            ProductMatchPositions pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "back");

//            pmps.LMaskMp.X = BLivePmps.LMaskMp.X;
//            pmps.LMaskMp.Y = BLivePmps.LMaskMp.Y;
//            pmps.LMaskMp.Score = BLivePmps.LMaskMp.Score;
//            pmps.RMaskMp.X = BLivePmps.RMaskMp.X;
//            pmps.RMaskMp.Y = BLivePmps.RMaskMp.Y;
//            pmps.RMaskMp.Score = BLivePmps.RMaskMp.Score;

//            // PointF center = new Point(GV.LeftBackCam.Grab().Cols / 2, GV.LeftBackCam.Grab().Rows / 2);

//            double beforeMoveLShiftXum = (pmps.LWaferMp.X - pmps.LMaskMp.X) * GV.ZoomLensInfo.LeftDownUmPerPixelX;
//            double beforeMoveLShiftYum = (pmps.LWaferMp.Y - pmps.LMaskMp.Y) * GV.ZoomLensInfo.LeftDownUmPerPixelY;
//            double beforeMoveRShiftXum = (pmps.RWaferMp.X - pmps.RMaskMp.X) * GV.ZoomLensInfo.RightDownUmPerPixelX;
//            double beforeMoveRShiftYum = (pmps.RWaferMp.Y - pmps.RMaskMp.Y) * GV.ZoomLensInfo.RightDownUmPerPixelY;
//            // writeStatus("L shift(um) : (" + beforeMoveLShiftXum.ToString("F2") + " , " + beforeMoveLShiftYum.ToString("F2") + ")");
//            // writeStatus("R shift(um) : (" + beforeMoveRShiftXum.ToString("F2") + " , " + beforeMoveRShiftYum.ToString("F2") + ")");
//            GV.HwndFormMain.writeStatus(string.Format(GV.Dlang.strMessageLShiftum, beforeMoveLShiftXum.ToString("F2"), beforeMoveLShiftYum.ToString("F2")));
//            GV.HwndFormMain.writeStatus(string.Format(GV.Dlang.strMessageRShiftum, beforeMoveRShiftXum.ToString("F2"), beforeMoveRShiftYum.ToString("F2")));

//            if (pmps.LWaferMp.Score < _AlignC.WaferScore || pmps.RWaferMp.Score < _AlignC.WaferScore)
//            {
//                GV.Plc.SendAlignBackNG();
//                iAlignOK = 0;
//                GV.HwndFormMain.writeStatus(GV.Dlang.strErrorCantBottomWafer);
//                return;
//                // throw new MRException("Error : Can't find wafer template at high magnification");
//            }

//            //int[] motorSteps = CalcuteMaskWaferShiftAndReturnMotorStepsBack(pmps.LMaskMp, pmps.LWaferMp, pmps.RMaskMp, pmps.RWaferMp, pmps.PatternCenterDistance);

//            int[] motorSteps = XyyMotorSteps(pmps.LMaskMp, pmps.LWaferMp, pmps.RMaskMp, pmps.RWaferMp);

//            if (!GV.Plc.ReadMemory(GV.Plc.iUpCCDStart))
//            {
//                iAlignOK = 0;
//                //iStartAlign = 0;
//                return;
//            }
//            GV.Plc.AlignXyyTableMove(2, motorSteps[0], motorSteps[1], motorSteps[2]);

//            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, false);
//        }

//        private bool MoveCameraBack(int iMask)
//        {
//            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, true);

//            //ProductMatchPositions pmps = null;

//            ProductMatchPositions pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "backmask");

//            if ((pmps.LMaskMp.Score < _AlignC.MaskSocre) || (pmps.RMaskMp.Score < _AlignC.MaskSocre))
//            {
//                GV.HwndFormMain.writeStatus("Bottom L Mask / R Mask score : " + pmps.LMaskMp.Score.ToString("F2") + " , " + pmps.RMaskMp.Score.ToString("F2"));
//                GV.Plc.WriteMemory(GV.Plc.iBackMaskNG, true);
//                iAlignOK = 0;
//                GV.HwndFormMain.writeStatus(GV.Dlang.strErrorCantBottomMask);
//                return false;
//            }

//            PointF center = GV.DownCenter;

//            double lOffsetXSteps = ((double)(center.X) - pmps.LMaskMp.X) * GV.ZoomLensInfo.LeftDownUmPerPixelX / GV.ZoomLensInfo.LeftDownUmPerStepX;
//            double lOffsetYSteps = ((double)(center.Y) - pmps.LMaskMp.Y) * GV.ZoomLensInfo.LeftDownUmPerPixelY / GV.ZoomLensInfo.LeftDownUmPerStepY;
//            double rOffsetXSteps = ((double)(center.X) - pmps.RMaskMp.X) * GV.ZoomLensInfo.RightDownUmPerPixelX / GV.ZoomLensInfo.RightDownUmPerStepX;
//            double rOffsetYSteps = ((double)(center.Y) - pmps.RMaskMp.Y) * GV.ZoomLensInfo.RightDownUmPerPixelY / GV.ZoomLensInfo.RightDownUmPerStepY;

//            GV.Plc.AlignDownCameraMove(1, -lOffsetXSteps, rOffsetXSteps, -lOffsetYSteps, -rOffsetYSteps);

//            Thread.Sleep(250);

//            pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "backmask");

//            lOffsetXSteps = ((double)(center.X) - pmps.LMaskMp.X) * GV.ZoomLensInfo.LeftDownUmPerPixelX / GV.ZoomLensInfo.LeftDownUmPerStepX;
//            lOffsetYSteps = ((double)(center.Y) - pmps.LMaskMp.Y) * GV.ZoomLensInfo.LeftDownUmPerPixelY / GV.ZoomLensInfo.LeftDownUmPerStepY;
//            rOffsetXSteps = ((double)(center.X) - pmps.RMaskMp.X) * GV.ZoomLensInfo.RightDownUmPerPixelX / GV.ZoomLensInfo.RightDownUmPerStepX;
//            rOffsetYSteps = ((double)(center.Y) - pmps.RMaskMp.Y) * GV.ZoomLensInfo.RightDownUmPerPixelY / GV.ZoomLensInfo.RightDownUmPerStepY;

//            GV.Plc.AlignDownCameraMove(0, -lOffsetXSteps, rOffsetXSteps, -lOffsetYSteps, -rOffsetYSteps);

//            Thread.Sleep(250);

//            pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "backmask");

//            if ((pmps.LMaskMp.Score < _AlignC.MaskSocre) || (pmps.RMaskMp.Score < _AlignC.MaskSocre))
//            {
//                GV.HwndFormMain.writeStatus("Bottom L Mask / R Mask score : " + pmps.LMaskMp.Score.ToString("F2") + " , " + pmps.RMaskMp.Score.ToString("F2"));
//                GV.Plc.WriteMemory(GV.Plc.iBackMaskNG, true);
//                GV.HwndFormMain.writeStatus(GV.Dlang.strErrorCantBottomMask);
//                iAlignOK = 0;
//                return false;
//            }

//            lOffsetXSteps = ((double)(center.X) - pmps.LMaskMp.X) * GV.ZoomLensInfo.LeftDownUmPerPixelX;
//            lOffsetYSteps = ((double)(center.Y) - pmps.LMaskMp.Y) * GV.ZoomLensInfo.LeftDownUmPerPixelY;
//            rOffsetXSteps = ((double)(center.X) - pmps.RMaskMp.X) * GV.ZoomLensInfo.RightDownUmPerPixelX;
//            rOffsetYSteps = ((double)(center.Y) - pmps.RMaskMp.Y) * GV.ZoomLensInfo.RightDownUmPerPixelY;

//            double precisionX = (lOffsetXSteps + rOffsetXSteps) / 2.0f;
//            double precisionY = (lOffsetYSteps + rOffsetYSteps) / 2.0f;
//            double degree = Math.Atan((lOffsetYSteps - rOffsetYSteps) / ((pmps.PatternCenterDistance))) * 180 / Math.PI;
//            double expansion = (rOffsetXSteps - lOffsetXSteps) / 2.0f;

//            // if ((precisionX < _AlignC.XPrecision ) && (precisionY < _AlignC.YPrecision) && (expansion < _AlignC.MaxExpansion))
//            if (!((pmps.LMaskMp.Score < _AlignC.MaskSocre) || (pmps.RMaskMp.Score < _AlignC.MaskSocre)))
//            {
//                Thread.Sleep(250);
//                LeftMask = GV.LeftBackCam.Grab();
//                RightMask = GV.RightBackCam.Grab();
//                pmps = GetAllMatchPostion(LeftMask, RightMask, "backmask");

//                //TopMaskPmps.LMaskMp.X = pmps.LMaskMp.X;
//                //TopMaskPmps.LMaskMp.Y = pmps.LMaskMp.Y;
//                //TopMaskPmps.LMaskMp.Score = pmps.LMaskMp.Score;
//                //TopMaskPmps.RMaskMp.X = pmps.RMaskMp.X;
//                //TopMaskPmps.RMaskMp.Y = pmps.RMaskMp.Y;
//                //TopMaskPmps.RMaskMp.Score = pmps.RMaskMp.Score;
//                BLivePmps.LMaskMp.X = pmps.LMaskMp.X;
//                BLivePmps.LMaskMp.Y = pmps.LMaskMp.Y;
//                BLivePmps.LMaskMp.Score = pmps.LMaskMp.Score;
//                BLivePmps.RMaskMp.X = pmps.RMaskMp.X;
//                BLivePmps.RMaskMp.Y = pmps.RMaskMp.Y;
//                BLivePmps.RMaskMp.Score = pmps.RMaskMp.Score;
//                string TopMaskCheck3 = string.Format(GV.Dlang.strMessageMaskShift, BLastPmps.LMaskMp.X, BLastPmps.RMaskMp.X, BLastPmps.LMaskMp.Y, BLastPmps.RMaskMp.Y);
//                GM.WriteToStatusTextBox1(iAdmin, "0:" + TopMaskCheck3);

//                // CountDistance();
//                // ReportPLCStatus();
//                // DownCCDMove();

//                GV.Plc.GetLocation();

//                _AlignC.PatternCenterDistanceUm = GV.ZoomLensInfo.LRBackCenterDistance
//                    - (int)(GV.NowLocation[GV.Plc.iiDDownLeftX] * GV.ZoomLensInfo.LeftDownUmPerStepX) - (int)(GV.NowLocation[GV.Plc.iiDDownRightX] * GV.ZoomLensInfo.RightDownUmPerStepX);

//                GV.Plc.WriteMemory(GV.Plc.iBackMaskOK, true);
//            }
//            else
//            {
//                GV.HwndFormMain.writeStatus("Bottom L Mask / R Mask score : " + pmps.LMaskMp.Score.ToString("F2") + " , " + pmps.RMaskMp.Score.ToString("F2"));
//                // writeStatus("Bottom precisionX : {0} precisionY : {1} degree : {2} exp : {3}", precisionX.)
//                GV.Plc.WriteMemory(GV.Plc.iBackMaskNG, true);
//                GV.HwndFormMain.writeStatus(GV.Dlang.strErrorCantBottomMask);
//                iAlignOK = 0;
//                return false;
//            }

//            GV.LeftMaskMat = LeftMask.Clone();
//            GV.RightMaskMat = RightMask.Clone();
//            //if (MaskImage.Checked)
//            //{
//            //    skLeftAlign.SetShowMask(true, _AlignC.dLalpha, LeftMask);
//            //    skRightAlign.SetShowMask(true, _AlignC.dRalpha, RightMask);
//            //}
//            Cv2.ImWrite(GetTemplateFileName("LeftMask"), LeftMask);
//            Cv2.ImWrite(GetTemplateFileName("RightMask"), RightMask);
//            GV.NowWaferMask = 3;

//            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, false);

//            return true;
//        }

//        private void btReadMaskMat_Click(object sender, EventArgs e)
//        {
//            LeftMask = Cv2.ImRead(GetTemplateFileName("LeftMask"), ImreadModes.Grayscale);
//            RightMask = Cv2.ImRead(GetTemplateFileName("RightMask"), ImreadModes.Grayscale);

//            ProductMatchPositions pmps = GetAllMatchPostion(LeftMask, RightMask, "backmask");

//            BLivePmps.LMaskMp.X = pmps.LMaskMp.X;
//            BLivePmps.LMaskMp.Y = pmps.LMaskMp.Y;
//            BLivePmps.LMaskMp.Score = pmps.LMaskMp.Score;
//            BLivePmps.RMaskMp.X = pmps.RMaskMp.X;
//            BLivePmps.RMaskMp.Y = pmps.RMaskMp.Y;
//            BLivePmps.RMaskMp.Score = pmps.RMaskMp.Score;

//            GV.Plc.GetLocation();

//            _AlignC.PatternCenterDistanceUm = GV.ZoomLensInfo.LRBackCenterDistance
//                - (int)(GV.NowLocation[GV.Plc.iiDDownLeftX] * GV.ZoomLensInfo.LeftDownUmPerStepX)
//                - (int)(GV.NowLocation[GV.Plc.iiDDownRightX] * GV.ZoomLensInfo.RightDownUmPerPixelX);

//        }

//        private void btNG_Click(object sender, EventArgs e)
//        {
//            GV.Plc.SendAlignNG();
//        }

//        private void btUpdate_Click(object sender, EventArgs e)
//        {
//            nUDXyyX.Value = GV.NowLocation[GV.Plc.iiDChuckX];
//            nUDXyyY1.Value = GV.NowLocation[GV.Plc.iiDChuckY1];
//            nUDXyyY2.Value = GV.NowLocation[GV.Plc.iiDChuckY2];
//        }

//        private void btGo_Click(object sender, EventArgs e)
//        {
//            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, true);

//            Thread.Sleep(100);

//            bool bRet = GV.Plc.XyyTableMoveAbs((int)nUDXyyX.Value, (int)nUDXyyY1.Value, (int)nUDXyyY2.Value);
//            if (bRet == false)
//            {
//                string msg = string.Format("Xyy Roll back Error = Plc Error !!!");
//                lbDebugMsg.Text = msg;
//            }

//            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, false);
//        }

//        private void btCapture_Click(object sender, EventArgs e)
//        {
//            MessageBox.Show(GM.SaveCaptureImageXyy(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab()) + GV.Dlang.strSave);

//            ProductMatchPositions pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "back");

//            OrgPmps.LWaferMp.X = pmps.LWaferMp.X;
//            OrgPmps.LWaferMp.Y = pmps.LWaferMp.Y;
//            OrgPmps.RWaferMp.X = pmps.RWaferMp.X;
//            OrgPmps.RWaferMp.Y = pmps.RWaferMp.Y;

//            GV.Plc.GetLocation();

//            OrgMotors[0] = GV.NowLocation[GV.Plc.iiDChuckX];
//            OrgMotors[1] = GV.NowLocation[GV.Plc.iiDChuckY1];
//            OrgMotors[2] = GV.NowLocation[GV.Plc.iiDChuckY2];

//        }

//        private ProductMatchPositions GetAllMatchPostion(Mat lSrc, Mat rSrc, string alignMmagnification)
//        {

//            MatchPosition lMaskMp = new MatchPosition();
//            MatchPosition rMaskMp = new MatchPosition();
//            MatchPosition lWaferMp = new MatchPosition();
//            MatchPosition rWaferMp = new MatchPosition();
//            double patternCenterDistance = _AlignC.PatternCenterDistanceUm;

//            if (GV.AppSettingParm.Emulation != true)
//            {
//                if (alignMmagnification == "low")
//                {
//                    GV.matcherLLM.MatMatchWithAlgo(0, lSrc.Clone(), ref lMaskMp, _AlignC.LLMaskAlgorithm);
//                    GV.matcherRLM.MatMatchWithAlgo(0, rSrc.Clone(), ref rMaskMp, _AlignC.RLMaskAlgorithm);
//                    GV.matcherLLW.MatMatchWithAlgo(0, lSrc.Clone(), ref lWaferMp, _AlignC.LLWaferAlgorithm);
//                    GV.matcherRLW.MatMatchWithAlgo(0, rSrc.Clone(), ref rWaferMp, _AlignC.RLWaferAlgorithm);

//                    lWaferMp.X -= (float)(_AlignC.XOffset / GV.ZoomLensInfo.LeftUmPerPixelX[_AlignC.AlignLowMagnification]);
//                    rWaferMp.X -= (float)(_AlignC.XOffset / GV.ZoomLensInfo.RightUmPerPixelX[_AlignC.AlignLowMagnification]);
//                    lWaferMp.Y -= (float)(_AlignC.YLOffset / GV.ZoomLensInfo.LeftUmPerPixelY[_AlignC.AlignLowMagnification]);
//                    rWaferMp.Y -= (float)(_AlignC.YROffset / GV.ZoomLensInfo.RightUmPerPixelY[_AlignC.AlignLowMagnification]);
//                }
//                else if (alignMmagnification == "high")
//                {
//                    GV.matcherLHM.MatMatchWithAlgo(0, lSrc.Clone(), ref lMaskMp, _AlignC.LHMaskAlgorithm);
//                    GV.matcherRHM.MatMatchWithAlgo(0, rSrc.Clone(), ref rMaskMp, _AlignC.RHMaskAlgorithm);
//                    GV.matcherLHW.MatMatchWithAlgo(0, lSrc.Clone(), ref lWaferMp, _AlignC.LHWaferAlgorithm);
//                    GV.matcherRHW.MatMatchWithAlgo(0, rSrc.Clone(), ref rWaferMp, _AlignC.RHWaferAlgorithm);
//                    lWaferMp.X -= (float)(_AlignC.XOffset / GV.ZoomLensInfo.LeftUmPerPixelX[_AlignC.AlignHighMagnification]);
//                    rWaferMp.X -= (float)(_AlignC.XOffset / GV.ZoomLensInfo.RightUmPerPixelX[_AlignC.AlignHighMagnification]);
//                    lWaferMp.Y -= (float)(_AlignC.YLOffset / GV.ZoomLensInfo.LeftUmPerPixelY[_AlignC.AlignHighMagnification]);
//                    rWaferMp.Y -= (float)(_AlignC.YROffset / GV.ZoomLensInfo.RightUmPerPixelY[_AlignC.AlignHighMagnification]);
//                }
//                else if (alignMmagnification == "back")
//                {
//                    GV.matcherLLW.MatMatchWithAlgo(0, LeftMaskMat, ref lMaskMp, _AlignC.LLWaferAlgorithm);
//                    GV.matcherRLW.MatMatchWithAlgo(0, RightMaskMat, ref rMaskMp, _AlignC.RLWaferAlgorithm);
//                    GV.matcherLHW.MatMatchWithAlgo(0, lSrc.Clone(), ref lWaferMp, _AlignC.LHWaferAlgorithm);
//                    GV.matcherRHW.MatMatchWithAlgo(0, rSrc.Clone(), ref rWaferMp, _AlignC.RHWaferAlgorithm);
//                    lWaferMp.X -= (float)(_AlignC.XOffset / GV.ZoomLensInfo.LeftDownUmPerPixelX);
//                    rWaferMp.X -= (float)(_AlignC.XOffset / GV.ZoomLensInfo.RightDownUmPerPixelX);
//                    lWaferMp.Y -= (float)(_AlignC.YLOffset / GV.ZoomLensInfo.LeftDownUmPerPixelY);
//                    rWaferMp.Y -= (float)(_AlignC.YROffset / GV.ZoomLensInfo.RightDownUmPerPixelY);
//                    // patternCenterDistancePixel = _AlignC.PatternCenterDistanceUm / ((GV.ZoomLensInfo.LeftDownUmPerPixelX + GV.ZoomLensInfo.RightDownUmPerPixelX) / 2);
//                }
//                else if (alignMmagnification == "backmask")
//                {
//                    GV.matcherLLW.MatMatchWithAlgo(0, lSrc.Clone(), ref lMaskMp, _AlignC.LLWaferAlgorithm);
//                    GV.matcherRLW.MatMatchWithAlgo(0, rSrc.Clone(), ref rMaskMp, _AlignC.RLWaferAlgorithm);
//                }
//                else if (alignMmagnification == "checkmask")
//                {
//                    GV.matcherLHM.MatMatchWithAlgo(0, lSrc.Clone(), ref lMaskMp, _AlignC.LHMaskAlgorithm);
//                    GV.matcherRHM.MatMatchWithAlgo(0, rSrc.Clone(), ref rMaskMp, _AlignC.RHMaskAlgorithm);
//                }
//                else if (alignMmagnification == "backcheck")
//                {
//                    GV.matcherLLW.MatMatchWithAlgo(0, lSrc.Clone(), ref lWaferMp, _AlignC.LLWaferAlgorithm);
//                    GV.matcherRLW.MatMatchWithAlgo(0, rSrc.Clone(), ref rWaferMp, _AlignC.RLWaferAlgorithm);
//                    GV.matcherLHW.MatMatchWithAlgo(0, lSrc.Clone(), ref lWaferMp, _AlignC.LHWaferAlgorithm);
//                    GV.matcherRHW.MatMatchWithAlgo(0, rSrc.Clone(), ref rWaferMp, _AlignC.RHWaferAlgorithm);
//                }

//                skLeft.MaskMp = lMaskMp;
//                skLeft.WaferMp = lWaferMp;
//                skRight.MaskMp = rMaskMp;
//                skRight.WaferMp = rWaferMp;
//            }
//            return new ProductMatchPositions(lMaskMp, lWaferMp, rMaskMp, rWaferMp, patternCenterDistance);
//        }

//        private int[] XyyMotorSteps(MatchPosition lMaskMp, MatchPosition lWaferMp, MatchPosition rMaskMp, MatchPosition rWaferMp)
//        {
//            int[] motorSteps = new int[3];

//            double patternCenterDistancePixel = _AlignC.PatternCenterDistanceUm / 2;

//            double ly = ((double)(lWaferMp.Y - lMaskMp.Y)) * GV.ZoomLensInfo.LeftDownUmPerPixelY;
//            double ry = ((double)(rWaferMp.Y - rMaskMp.Y)) * GV.ZoomLensInfo.RightDownUmPerPixelY;
//            double waferSin = (ry - ly) / patternCenterDistancePixel;
//            double waferRad = Math.Asin(waferSin);
//            double waferDegree = waferSin * 180 / Math.PI;
//            double rotateMotorSteps = waferDegree * GV.ZoomLensInfo.MotorStepsPerPixelDegree;

//            double a, b, c;
//            a = b = patternCenterDistancePixel;
//            c = Math.Sqrt(a * a + b * b - 2 * a * b * Math.Cos(waferRad));
//            double deltaX = c * Math.Sin(waferRad);
//            double deltaY = c * Math.Cos(waferRad);

//            if (waferSin < 0)
//            {
//                lWaferMp.X -= (float)deltaX;
//                lWaferMp.Y += (float)deltaY;
//                rWaferMp.X += (float)deltaX;
//                rWaferMp.Y -= (float)deltaY;
//            }
//            if (waferSin > 0)
//            {
//                lWaferMp.X -= (float)deltaX;
//                lWaferMp.Y -= (float)deltaY;
//                rWaferMp.X += (float)deltaX;
//                rWaferMp.Y += (float)deltaY;
//            }

//            double xMotorStepsl = (lMaskMp.X - lWaferMp.X) * GV.ZoomLensInfo.LeftDownUmPerPixelX * 10;
//            double xMotorStepsr = (rMaskMp.X - rWaferMp.X) * GV.ZoomLensInfo.RightDownUmPerPixelX * 10;
//            double xMotorSteps = (xMotorStepsl + xMotorStepsr) / 2;
//            double yMotorStepsl = (lMaskMp.Y - lWaferMp.Y) * GV.ZoomLensInfo.LeftDownUmPerPixelY * 10;
//            double yMotorStepsr = (rMaskMp.Y - rWaferMp.Y) * GV.ZoomLensInfo.RightDownUmPerPixelY * 10;
//            double yMotorSteps = (yMotorStepsl + yMotorStepsr) / 2;

//            motorSteps[0] = (int)Math.Round(xMotorSteps);
//            motorSteps[1] = (int)Math.Round(yMotorSteps);
//            motorSteps[2] = (int)Math.Round(rotateMotorSteps);
//            return motorSteps;
//        }

//        private void btBTAlignWaferRotate_Click(object sender, EventArgs e)
//        {
//            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, true);

//            ProductMatchPositions pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "back");

//            pmps.LMaskMp.X = BLivePmps.LMaskMp.X;
//            pmps.LMaskMp.Y = BLivePmps.LMaskMp.Y;
//            pmps.LMaskMp.Score = BLivePmps.LMaskMp.Score;
//            pmps.RMaskMp.X = BLivePmps.RMaskMp.X;
//            pmps.RMaskMp.Y = BLivePmps.RMaskMp.Y;
//            pmps.RMaskMp.Score = BLivePmps.RMaskMp.Score;

//            // PointF center = new Point(GV.LeftBackCam.Grab().Cols / 2, GV.LeftBackCam.Grab().Rows / 2);

//            double beforeMoveLShiftXum = (pmps.LWaferMp.X - pmps.LMaskMp.X) * GV.ZoomLensInfo.LeftDownUmPerPixelX;
//            double beforeMoveLShiftYum = (pmps.LWaferMp.Y - pmps.LMaskMp.Y) * GV.ZoomLensInfo.LeftDownUmPerPixelY;
//            double beforeMoveRShiftXum = (pmps.RWaferMp.X - pmps.RMaskMp.X) * GV.ZoomLensInfo.RightDownUmPerPixelX;
//            double beforeMoveRShiftYum = (pmps.RWaferMp.Y - pmps.RMaskMp.Y) * GV.ZoomLensInfo.RightDownUmPerPixelY;
//            string msgL = string.Format(GV.Dlang.strMessageLShiftum, beforeMoveLShiftXum.ToString("F2"), beforeMoveLShiftYum.ToString("F2"));
//            string msgR = string.Format(GV.Dlang.strMessageRShiftum, beforeMoveRShiftXum.ToString("F2"), beforeMoveRShiftYum.ToString("F2"));
//            GV.HwndFormMain.writeStatus(msgL);
//            GV.HwndFormMain.writeStatus(msgR);
//            lbLeftXYW.Text = msgL;
//            lbRightXYW.Text = msgR;

//            if (pmps.LWaferMp.Score < _AlignC.WaferScore || pmps.RWaferMp.Score < _AlignC.WaferScore)
//            {
//                GV.Plc.SendAlignBackNG();
//                iAlignOK = 0;
//                GV.HwndFormMain.writeStatus(GV.Dlang.strErrorCantBottomWafer);
//                return;
//                // throw new MRException("Error : Can't find wafer template at high magnification");
//            }

//            //int[] motorSteps = CalcuteMaskWaferShiftAndReturnMotorStepsBack(pmps.LMaskMp, pmps.LWaferMp, pmps.RMaskMp, pmps.RWaferMp, pmps.PatternCenterDistance);

//            int[] motorSteps = XyyRotateSteps(pmps.LMaskMp, pmps.LWaferMp, pmps.RMaskMp, pmps.RWaferMp);

//            if (!GV.Plc.ReadMemory(GV.Plc.iUpCCDStart))
//            {
//                iAlignOK = 0;
//                //iStartAlign = 0;
//                return;
//            }
//            GV.Plc.XyyTableMove(motorSteps[0], motorSteps[1], motorSteps[2]);

//            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, false);

//            Thread.Sleep(1000);

//            pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "back");

//            pmps.LMaskMp.X = BLivePmps.LMaskMp.X;
//            pmps.LMaskMp.Y = BLivePmps.LMaskMp.Y;
//            pmps.LMaskMp.Score = BLivePmps.LMaskMp.Score;
//            pmps.RMaskMp.X = BLivePmps.RMaskMp.X;
//            pmps.RMaskMp.Y = BLivePmps.RMaskMp.Y;
//            pmps.RMaskMp.Score = BLivePmps.RMaskMp.Score;

//            double NLdisX = pmps.LWaferMp.X - pmps.LMaskMp.X;
//            double NLdisY = pmps.LWaferMp.Y - pmps.LMaskMp.Y;
//            double NRdisX = pmps.RWaferMp.X - pmps.RMaskMp.X;
//            double NRdisY = pmps.RWaferMp.Y - pmps.RMaskMp.Y;

//            double NLdisXum = NLdisX * GV.ZoomLensInfo.LeftDownUmPerPixelX;
//            double NLdisYum = NLdisY * GV.ZoomLensInfo.LeftDownUmPerPixelY;
//            double NRdisXum = NRdisX * GV.ZoomLensInfo.RightDownUmPerPixelX;
//            double NRdisYum = NRdisY * GV.ZoomLensInfo.RightDownUmPerPixelY;

//            string msgLW = string.Format("LWXum = {0:N8}, LWYum = {1:N8}", NLdisXum, NLdisYum);
//            string msgRW = string.Format("RWXum = {0:N8}, RWYum = {1:N8}", NRdisXum, NRdisYum);

//            lbLeftXYW.Text = msgLW;
//            lbRightXYW.Text = msgRW;
//        }

//        private void btGoDis_Click(object sender, EventArgs e)
//        {
//            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, true);

//            Thread.Sleep(100);

//            ProductMatchPositions pmps;

//            if (!((_recipe.LeftHighWaferMat == null) || (_recipe.RightHighWaferMat) == null))
//            {
//                pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "back");
//            }
//            else if (!((_recipe.LeftLowWaferMat == null) || (_recipe.RightLowWaferMat) == null))
//            {
//                pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "backmask");
//            }
//            else
//            {
//                pmps = new ProductMatchPositions();
//            }

//            OrgPmps.LWaferMp.X = pmps.LWaferMp.X;
//            OrgPmps.LWaferMp.Y = pmps.LWaferMp.Y;
//            OrgPmps.RWaferMp.X = pmps.RWaferMp.X;
//            OrgPmps.RWaferMp.Y = pmps.RWaferMp.Y;

//            int Xdis = (int)nUDXyyX.Value + 1;
//            int Y1dis = (int)nUDXyyY1.Value + 1;
//            // int Y2dis = (int)nUDXyyY2.Value;

//            GV.Plc.GetLocation();

//            OrgMotors[0] = GV.NowLocation[GV.Plc.iiDChuckX];
//            OrgMotors[1] = GV.NowLocation[GV.Plc.iiDChuckY1];
//            OrgMotors[2] = GV.NowLocation[GV.Plc.iiDChuckY2];

//            int x = GV.NowLocation[GV.Plc.iiDChuckX] + Xdis;
//            int y1 = GV.NowLocation[GV.Plc.iiDChuckY1] + Y1dis;
//            int y2 = GV.NowLocation[GV.Plc.iiDChuckY2] + Y1dis;

//            bool bRet = GV.Plc.XyyTableMoveAbs(x, y1, y2);

//            if (bRet == false)
//            {
//                string msg = string.Format("Xyy Move Abs Error = Plc Error !!!");
//                lbDebugMsg.Text = msg;
//            }

//            Thread.Sleep(1000);

//            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, false);

//            //pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "back");

//            if (!((_recipe.LeftHighWaferMat == null) || (_recipe.RightHighWaferMat) == null))
//            {
//                pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "back");
//            }
//            else if (!((_recipe.LeftLowWaferMat == null) || (_recipe.RightLowWaferMat) == null))
//            {
//                pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "backmask");
//            }
//            else
//            {
//                pmps = new ProductMatchPositions();
//            }

//            double NLdisX = pmps.LWaferMp.X - OrgPmps.LWaferMp.X;
//            double NLdisY = pmps.LWaferMp.Y - OrgPmps.LWaferMp.Y;
//            double NRdisX = pmps.RWaferMp.X - OrgPmps.RWaferMp.X;
//            double NRdisY = pmps.RWaferMp.Y - OrgPmps.RWaferMp.Y;

//            double NLdisXum = NLdisX * GV.ZoomLensInfo.LeftDownUmPerPixelX;
//            double NLdisYum = NLdisY * GV.ZoomLensInfo.LeftDownUmPerPixelY;
//            double NRdisXum = NRdisX * GV.ZoomLensInfo.RightDownUmPerPixelX;
//            double NRdisYum = NRdisY * GV.ZoomLensInfo.RightDownUmPerPixelY;

//            double LStepUm = NLdisXum / Xdis;
//            double RStepUm = NRdisXum / Xdis;
//            double LYStepUm = NLdisYum / Y1dis;
//            double RYStepUm = NRdisYum / Y1dis;

//            string msgLW = string.Format("LWX = {0:N8}, LWXum = {2:N8}, LWY = {1:N8}, LWYum = {3:N8}", LStepUm, LYStepUm, NLdisXum, NLdisYum);
//            string msgRW = string.Format("RWX = {0:N8}, RWXum = {2:N8}, RWY = {1:N8}, RWYum = {3:N8}", RStepUm, RYStepUm, NRdisXum, NRdisYum);

//            lbLeftXYW.Text = msgLW;
//            lbRightXYW.Text = msgRW;
//        }

//        private void btGoDisN_Click(object sender, EventArgs e)
//        {
//            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, true);

//            Thread.Sleep(100);

//            //ProductMatchPositions pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "back");
//            ProductMatchPositions pmps;

//            if (!((_recipe.LeftHighWaferMat == null) || (_recipe.RightHighWaferMat) == null))
//            {
//                pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "back");
//            }
//            else if (!((_recipe.LeftLowWaferMat == null) || (_recipe.RightLowWaferMat) == null))
//            {
//                pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "backmask");
//            }
//            else
//            {
//                pmps = new ProductMatchPositions();
//            }


//            OrgPmps.LWaferMp.X = pmps.LWaferMp.X;
//            OrgPmps.LWaferMp.Y = pmps.LWaferMp.Y;
//            OrgPmps.RWaferMp.X = pmps.RWaferMp.X;
//            OrgPmps.RWaferMp.Y = pmps.RWaferMp.Y;

//            int Xdis = (int)nUDXyyX.Value;
//            int Y1dis = (int)nUDXyyY1.Value;
//            // int Y2dis = (int)nUDXyyY2.Value;

//            GV.Plc.GetLocation();

//            OrgMotors[0] = GV.NowLocation[GV.Plc.iiDChuckX];
//            OrgMotors[1] = GV.NowLocation[GV.Plc.iiDChuckY1];
//            OrgMotors[2] = GV.NowLocation[GV.Plc.iiDChuckY2];

//            int x = GV.NowLocation[GV.Plc.iiDChuckX] - Xdis;
//            int y1 = GV.NowLocation[GV.Plc.iiDChuckY1] - Y1dis;
//            int y2 = GV.NowLocation[GV.Plc.iiDChuckY2] - Y1dis;

//            bool bRet = GV.Plc.XyyTableMoveAbs(x, y1, y2);

//            if (bRet == false)
//            {
//                string msg = string.Format("Xyy Move Abs Error = Plc Error !!!");
//                lbDebugMsg.Text = msg;
//            }

//            Thread.Sleep(1000);

//            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, false);

//            if (!((_recipe.LeftHighWaferMat == null) || (_recipe.RightHighWaferMat) == null))
//            {
//                pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "back");
//            }
//            else if (!((_recipe.LeftLowWaferMat == null) || (_recipe.RightLowWaferMat) == null))
//            {
//                pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "backmask");
//            }
//            else
//            {
//                pmps = new ProductMatchPositions();
//            }

//            double NLdisX = pmps.LWaferMp.X - OrgPmps.LWaferMp.X;
//            double NLdisY = pmps.LWaferMp.Y - OrgPmps.LWaferMp.Y;
//            double NRdisX = pmps.RWaferMp.X - OrgPmps.RWaferMp.X;
//            double NRdisY = pmps.RWaferMp.Y - OrgPmps.RWaferMp.Y;

//            double NLdisXum = NLdisX * GV.ZoomLensInfo.LeftDownUmPerPixelX;
//            double NLdisYum = NLdisY * GV.ZoomLensInfo.LeftDownUmPerPixelY;
//            double NRdisXum = NRdisX * GV.ZoomLensInfo.RightDownUmPerPixelX;
//            double NRdisYum = NRdisY * GV.ZoomLensInfo.RightDownUmPerPixelY;

//            double LStepUm = NLdisXum / Xdis;
//            double RStepUm = NRdisXum / Xdis;
//            double LYStepUm = NLdisYum / Y1dis;
//            double RYStepUm = NRdisYum / Y1dis;

//            string msgLW = string.Format("LWX = {0:N8}, LWXum = {2:N8}, LWY = {1:N8}, LWYum = {3:N8}", LStepUm, LYStepUm, NLdisXum, NLdisYum);
//            string msgRW = string.Format("RWX = {0:N8}, RWXum = {2:N8}, RWY = {1:N8}, RWYum = {3:N8}", RStepUm, RYStepUm, NRdisXum, NRdisYum);

//            lbLeftXYW.Text = msgLW;
//            lbRightXYW.Text = msgRW;

//        }

//        private void BtLBackMask_Click(object sender, EventArgs e)
//        {
//            if (_recipeT.LeftLowWaferMat != null)
//            {
//                if (_recipeT.LeftLowWaferMask == null)
//                {
//                    _recipeT.LeftLowWaferMask = new GrayImage(_recipeT.LeftLowWaferMat.Width, _recipeT.LeftLowWaferMat.Height);
//                    _recipeT.LeftLowWaferMask.Fill(255);
//                }
//                DialogPaintMask dialogPaintMask = new DialogPaintMask()
//                {
//                    Image = _recipeT.LeftLowWaferMat,
//                    Mask = _recipeT.LeftLowWaferMask
//                };
//                if (dialogPaintMask.ShowDialog() == DialogResult.OK)
//                {
//                    _recipeT.LeftLowWaferMask = dialogPaintMask.Mask;
//                    GV.matcherLLW.LearnWithAlgo(_recipeT.LeftLowWaferMat, _recipeT.LeftLowWaferMask, _AlignC.LLWaferAlgorithm);
//                    _AlignC.LastModifyTime = DateTime.Now;
//                    GM.WriteRecipeXml(GV._recipe.FileName,GV._recipe);
//                }
//            }
//        }

//        private void BtRWaferMask_Click(object sender, EventArgs e)
//        {
//            if (_recipeT.RightHighWaferMat != null)
//            {
//                if (_recipeT.RightHighWaferMask == null)
//                {
//                    _recipeT.RightHighWaferMask = new GrayImage(_recipeT.RightHighWaferMat.Width, _recipeT.RightHighWaferMat.Height);
//                    _recipeT.RightHighWaferMask.Fill(255);
//                }
//                DialogPaintMask dialogPaintMask = new DialogPaintMask()
//                {
//                    Image = _recipeT.RightHighWaferMat,
//                    Mask = _recipeT.RightHighWaferMask
//                };
//                if (dialogPaintMask.ShowDialog() == DialogResult.OK)
//                {
//                    _recipeT.RightHighWaferMask = dialogPaintMask.Mask;
//                    GV.matcherRHW.LearnWithAlgo(_recipeT.RightHighWaferMat, _recipeT.RightHighWaferMask, _AlignC.RHWaferAlgorithm);
//                    _AlignC.LastModifyTime = DateTime.Now;
//                    GM.WriteRecipeXml(GV._recipe.FileName,GV._recipe);
//                }
//            }
//        }

//        private void BtRBackMask_Click(object sender, EventArgs e)
//        {
//            if (_recipeT.RightLowWaferMat != null)
//            {
//                if (_recipeT.RightLowWaferMask == null)
//                {
//                    _recipeT.RightLowWaferMask = new GrayImage(_recipeT.RightLowWaferMat.Width, _recipeT.RightLowWaferMat.Height);
//                    _recipeT.RightLowWaferMask.Fill(255);
//                }
//                DialogPaintMask dialogPaintMask = new DialogPaintMask()
//                {
//                    Image = _recipeT.RightLowWaferMat,
//                    Mask = _recipeT.RightLowWaferMask
//                };
//                if (dialogPaintMask.ShowDialog() == DialogResult.OK)
//                {
//                    _recipeT.RightLowWaferMask = dialogPaintMask.Mask;
//                    GV.matcherRLW.LearnWithAlgo(_recipeT.RightLowWaferMat, _recipeT.RightLowWaferMask, _AlignC.RLWaferAlgorithm);
//                    _AlignC.LastModifyTime = DateTime.Now;
//                    GM.WriteRecipeXml(GV._recipe.FileName, GV._recipe);
//                }
//            }
//        }

//        private void CBTopMask_Click(object sender, EventArgs e)
//        {
//            ChangeTopBottomMask(1);
//        }

//        private void ChangeTopBottomMask(int iTopBottom)
//        {
//            if (iTopBottom == 1)
//            {
//                cBTopMask.Checked = true;
//                cBBottomMask.Checked = false;
//                _AlignC.UpBotMask = iTopBottom;
//            }
//            else
//            {
//                cBTopMask.Checked = false;
//                cBBottomMask.Checked = true;
//                _AlignC.UpBotMask = iTopBottom;
//            }
//            _AlignC.LastModifyTime = DateTime.Now;
//            GM.WriteRecipeXml(EditRecipe);
//        }

//        private void CBBottomMask_Click(object sender, EventArgs e)
//        {
//            ChangeTopBottomMask(2);
//        }

//        private void CBAdjZ_CheckedChanged(object sender, EventArgs e)
//        {
//            if (cBAdjZ.Checked)
//            {
//                _AlignC.AdjuestZ = 1;
//            }
//            else
//            {
//                _AlignC.AdjuestZ = 0;
//            }
//            _AlignC.LastModifyTime = DateTime.Now;
//            GM.WriteRecipeXml(EditRecipe);
//        }

//        private void BtMask_Click(object sender, EventArgs e)
//        {
//            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, true);
//            GV.Plc.DownCameraZToHigh(1);
//            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, false);
//        }

//        private void BtWafer_Click(object sender, EventArgs e)
//        {
//            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, true);
//            GV.Plc.DownCameraZToHigh(0);
//            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, false);
//        }

//        private void BtCapMask_Click(object sender, EventArgs e)
//        {
//            Thread.Sleep(250);
//            LeftMaskMat = GV.LeftBackCam.Grab();
//            RightMaskMat = GV.RightBackCam.Grab();
//            GV.LeftMaskMat = LeftMaskMat;
//            GV.RightMaskMat = RightMaskMat;
//            Cv2.ImWrite("LeftMask.bmp", LeftMaskMat);
//            Cv2.ImWrite("RightMask.bmp", RightMaskMat);
//        }

//        private void BtAlignTest_Click(object sender, EventArgs e)
//        {
//            string msg;

//            lbDebugMsg.Visible = true;

//            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, true);

//            Thread.Sleep(250);

//            ProductMatchPositions pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "back");

//            double LShiftXum = (pmps.LMaskMp.X - pmps.LWaferMp.X) * GV.ZoomLensInfo.LeftDownUmPerPixelX / GV.ZoomLensInfo.LeftDownUmPerStepX;
//            double LShiftYum = (pmps.LMaskMp.Y - pmps.LWaferMp.Y) * GV.ZoomLensInfo.LeftDownUmPerPixelY / GV.ZoomLensInfo.LeftDownUmPerStepY;
//            double RShiftXum = (pmps.RMaskMp.X - pmps.RWaferMp.X) * GV.ZoomLensInfo.RightDownUmPerPixelX / GV.ZoomLensInfo.RightDownUmPerStepX;
//            double RShiftYum = (pmps.RMaskMp.Y - pmps.RWaferMp.Y) * GV.ZoomLensInfo.RightDownUmPerPixelY / GV.ZoomLensInfo.RightDownUmPerStepY;

//            double Rolldis = LShiftYum - RShiftYum;

//            GV.Plc.GetLocation();

//            _AlignC.PatternCenterDistanceUm = GV.ZoomLensInfo.LRBackCenterDistance
//                - GV.NowLocation[GV.Plc.iiDDownLeftX] / 10 - GV.NowLocation[GV.Plc.iiDDownRightX] / 10;

//            int[] motorSteps = XyyMotorSteps(pmps.LMaskMp, pmps.LWaferMp, pmps.RMaskMp, pmps.RWaferMp);

//            msg = string.Format("Roll dis {3:N3} TableMove {0}, {1}, {2}", motorSteps[0], motorSteps[1], motorSteps[2], Rolldis);

//            lbDebugMsg.Text = msg;

//            bool bRet = GV.Plc.AlignXyyTableMove(motorSteps[0], motorSteps[1], motorSteps[2]);
//            if (bRet == false)
//            {
//                msg = string.Format("XyyMove Error = Plc Error !!!");
//                lbDebugMsg.Text = msg;
//            }

//            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, false);
//        }

//        private void BtMaskCenter_Click(object sender, EventArgs e)
//        {
//            //ProductMatchPositions pmps = null;
//            string msg;

//            lbDebugMsg.Visible = true;

//            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, true);

//            Thread.Sleep(250);
//            ProductMatchPositions pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "backmask");

//            PointF center = GV.DownCenter;

//            double leftX = ((double)(center.X) - pmps.LMaskMp.X) * GV.ZoomLensInfo.LeftDownUmPerPixelX / GV.ZoomLensInfo.LeftDownUmPerStepX;
//            double leftY = ((double)(center.Y) - pmps.LMaskMp.Y) * GV.ZoomLensInfo.LeftDownUmPerPixelY / GV.ZoomLensInfo.LeftDownUmPerStepY;
//            double rightX = ((double)(center.X) - pmps.RMaskMp.X) * GV.ZoomLensInfo.RightDownUmPerPixelX / GV.ZoomLensInfo.RightDownUmPerStepX;
//            double rightY = ((double)(center.Y) - pmps.RMaskMp.Y) * GV.ZoomLensInfo.RightDownUmPerPixelY / GV.ZoomLensInfo.RightDownUmPerStepY;

//            bool bRet = GV.Plc.AlignCameraMove(2, -leftX, rightX, -leftY, -rightY);

//            if (bRet == false)
//            {
//                // msg = string.Format("CameraMove Error = {0}", GV.Plc.PlcErrorMessage);
//                msg = string.Format("CameraMove Error = Plc Error");
//                lbDebugMsg.Text = msg;
//                GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, false);
//                return;
//            }

//            Thread.Sleep(250);

//            pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "backmask");

//            double LShiftXum = (pmps.LMaskMp.X - pmps.LWaferMp.X) * GV.ZoomLensInfo.LeftDownUmPerPixelX;
//            double LShiftYum = (pmps.LMaskMp.Y - pmps.LWaferMp.Y) * GV.ZoomLensInfo.LeftDownUmPerPixelY;
//            double RShiftXum = (pmps.RMaskMp.X - pmps.RWaferMp.X) * GV.ZoomLensInfo.RightDownUmPerPixelX;
//            double RShiftYum = (pmps.RMaskMp.Y - pmps.RWaferMp.Y) * GV.ZoomLensInfo.RightDownUmPerPixelY;

//            double Rolldis = LShiftYum - RShiftYum;

//            _AlignC.PatternCenterDistanceUm = GV.ZoomLensInfo.LRUpCenterDistance
//                - GV.NowLocation[GV.Plc.iiDUpLeftX] / 10 - GV.NowLocation[GV.Plc.iiDUpRightX] / 10;

//            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, false);
//        }


//        public void Focus(SentechNetToMat cam, int iLeftRight)
//        {
//            int nowLocZ;
//            double bestFocus = 0;
//            GV.Plc.GetLocation();
//            int MinPosition;
//            int MaxPosition;
//            int leftNowLoc = GV.NowLocation[GV.Plc.iiDDownLeftZ];
//            int rightNowLoc = GV.NowLocation[GV.Plc.iiDDownRightZ];
//            Mat mat;
//            if (iLeftRight == 0)
//            {
//                nowLocZ = GV.NowLocation[GV.Plc.iiDDownLeftZ];
//                MinPosition = GV.Plc.iLimit[GV.Plc.iLimitLeftDownZmin];
//                MaxPosition = GV.Plc.iLimit[GV.Plc.iLimitLeftDownZmax];
//            }
//            else
//            {
//                nowLocZ = GV.NowLocation[GV.Plc.iiDDownRightZ];
//                MinPosition = GV.Plc.iLimit[GV.Plc.iLimitRightDownZmin];
//                MaxPosition = GV.Plc.iLimit[GV.Plc.iLimitRightDownZmax];
//            }
//            int bestPosition = nowLocZ;

//            for (double pos = MinPosition; pos <= MaxPosition; pos += 100) // 假設 0.1 是步進量
//            {
//                if (iLeftRight == 0)
//                {
//                    GV.Plc.AlignDownCameraMoveZAbsW(0, 0, 0, 0, 0, pos, rightNowLoc);
//                    System.Threading.Thread.Sleep(100); // 等待機構穩定
//                    mat = GV.LeftBackCam.Grab();
//                }
//                else
//                {
//                    GV.Plc.AlignDownCameraMoveZAbsW(0, 0, 0, 0, 0, leftNowLoc, pos);
//                    System.Threading.Thread.Sleep(100); // 等待機構穩定
//                    mat = GV.RightBackCam.Grab();
//                }

//                double focusValue = CalculateFocus(mat);

//                if (focusValue > bestFocus)
//                {
//                    bestFocus = focusValue;
//                    //bestPosition = pos;
//                }
//            }

//            //_zController.MoveTo(bestPosition); // 移到最佳焦距
//        }

//        //private double CalculateFocusLaplacian(Mat image)
//        //{
//        //    var laplacian = new Mat();
//        //    Cv2.Laplacian(image, laplacian, MatType.CV_8U);
//        //    var stdDev = new Scalar();
//        //    var mean = new Scalar();
//        //    Cv2.MeanStdDev(laplacian, ref mean, ref stdDev);
//        //    return stdDev.V0 * stdDev.V0;
//        //}

//        //private double CalculateFocus(Mat image)
//        //{
//        //    Mat gray = new Mat();
//        //    if (image.NumberOfChannels == 3)
//        //    {
//        //        CvInvoke.CvtColor(image, gray, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);
//        //    }
//        //    else
//        //    {
//        //        gray = image;
//        //    }

//        //    Mat sobelX = new Mat();
//        //    Mat sobelY = new Mat();
//        //    CvInvoke.Sobel(gray, sobelX, Emgu.CV.CvEnum.DepthType.Cv16S, 1, 0);
//        //    CvInvoke.Sobel(gray, sobelY, Emgu.CV.CvEnum.DepthType.Cv16S, 0, 1);

//        //    Mat sobel = new Mat();
//        //    CvInvoke.Magnitude(sobelX, sobelY, sobel);

//        //    double focusValue = CvInvoke.Mean(sobel).V0;
//        //    return focusValue;
//        //}

//        private double CalculateFocus(Mat image)
//        {
//            Mat gray = new Mat();
//            if (image.Channels() == 3)
//            {
//                Cv2.CvtColor(image, gray, ColorConversionCodes.BGR2GRAY);
//            }
//            else
//            {
//                gray = image;
//            }

//            Mat sobelX = new Mat();
//            Mat sobelY = new Mat();
//            Cv2.Sobel(gray, sobelX, MatType.CV_16S, 1, 0);
//            Cv2.Sobel(gray, sobelY, MatType.CV_16S, 0, 1);

//            Mat sobel = new Mat(sobelX.Size(), MatType.CV_16S);
//            Cv2.AddWeighted(sobelX, 0.5, sobelY, 0.5, 0, sobel);

//            double focusValue = Cv2.Mean(sobel)[0];
//            return focusValue;
//        }

//        private void BtLeftWaferFocus_Click(object sender, EventArgs e)
//        {
//            AutoFocus(GV.LeftBackCam, 0);
//        }

//        private void BtLMaskAdjSave_Click(object sender, EventArgs e)
//        {
//            _AlignC.LMaskAdjX = (int)NUDLMaskX.Value;
//            _AlignC.LMaskAdjY = (int)NUDLMaskY.Value;
//            _AlignC.LastModifyTime = DateTime.Now;
//            GM.WriteAlignConditionsAcarToXml("AlignConditions.xml");
//        }

//        private void BtRMaskAdjSave_Click(object sender, EventArgs e)
//        {
//            _AlignC.RMaskAdjX = (int)NUDRMaskX.Value;
//            _AlignC.RMaskAdjY = (int)NUDRMaskY.Value;
//            _AlignC.LastModifyTime = DateTime.Now;
//            GM.WriteAlignConditionsAcarToXml("AlignConditions.xml");
//        }

//        private void btChuckAlignN_Click(object sender, EventArgs e)
//        {

//        }

//        private int[] XyyRotateSteps(MatchPosition lMaskMp, MatchPosition lWaferMp, MatchPosition rMaskMp, MatchPosition rWaferMp)
//        {
//            int[] motorSteps = new int[3];
//            //double patternCenterDistancePixel = _AlignC.PatternCenterDistanceUm;

//            RotationTranslation rot = GetRotation(lMaskMp, lWaferMp, rMaskMp, rWaferMp);

//            double _dx = rot.Translation.X;
//            double _dy = rot.Translation.Y;

//            double dY = GV.NowLocation[GV.Plc.iiDChuckY1] * GV.ZoomLensInfo.LeftDownUmPerPixelY - GV.NowLocation[GV.Plc.iiDChuckY2] * GV.ZoomLensInfo.RightDownUmPerPixelY;
//            double Theta0 = Math.Atan2(dY, GV.AppSettingParm.XYYTableSize);

//            double dx1 = GV.R * Math.Sin(rot.Rotation + GV.ThetaX + Theta0) - GV.R * Math.Sin(GV.ThetaX + Theta0);
//            double dy1 = GV.R * Math.Cos(rot.Rotation + GV.ThetaY1 + Theta0) - GV.R * Math.Cos(GV.ThetaY1 + Theta0);
//            double dy2 = GV.R * Math.Cos(rot.Rotation + GV.ThetaY2 + Theta0) - GV.R * Math.Cos(GV.ThetaY2 + Theta0);

//            double mx = _dx + dx1;
//            double my1 = _dy + dy1;
//            double my2 = _dy + dy2;

//            motorSteps[0] = (int)(-mx / GV.AppSettingParm.XYYStepX);
//            motorSteps[1] = (int)(my1 / GV.AppSettingParm.XYYStepY1);
//            motorSteps[2] = (int)(my2 / GV.AppSettingParm.XYYStepY2);

//            return motorSteps;
//        }

//        private void BtRightWaferFocus_Click(object sender, EventArgs e)
//        {
//            AutoFocus(GV.LeftBackCam, 1);
//        }

//        public RotationTranslation GetRotation(MatchPosition lMaskMp, MatchPosition lWaferMp, MatchPosition rMaskMp, MatchPosition rWaferMp)
//        {
//            double patternCenterDistanceUm = _AlignC.PatternCenterDistanceUm / 2;

//            RotationTranslation rotat = new RotationTranslation();
//            double lwx = lWaferMp.X * GV.ZoomLensInfo.LeftDownUmPerPixelX;
//            double lwy = lWaferMp.Y * GV.ZoomLensInfo.LeftDownUmPerPixelY;
//            double rwx = rWaferMp.X * GV.ZoomLensInfo.RightDownUmPerPixelX;
//            double rwy = rWaferMp.Y * GV.ZoomLensInfo.RightDownUmPerPixelY;
//            double lmx = lMaskMp.X * GV.ZoomLensInfo.LeftDownUmPerPixelX;
//            double lmy = lMaskMp.Y * GV.ZoomLensInfo.LeftDownUmPerPixelY;
//            double rmx = rMaskMp.X * GV.ZoomLensInfo.RightDownUmPerPixelX;
//            double rmy = rMaskMp.Y * GV.ZoomLensInfo.RightDownUmPerPixelY;

//            PointD pLw = new PointD(lwx, lwy) + new PointD(-patternCenterDistanceUm, 0);
//            PointD pRw = new PointD(rwx, rwy) + new PointD(patternCenterDistanceUm, 0);
//            PointD pLm = new PointD(lmx, lmy) + new PointD(-patternCenterDistanceUm, 0);
//            PointD pRm = new PointD(rmx, rmy) + new PointD(patternCenterDistanceUm, 0);
//            LineD lineW = new LineD(pLw, pRw);
//            LineD lineM = new LineD(pLm, pRm);
//            rotat.Translation = lineM.Center - lineW.Center;

//            rotat.Rotation = lineM.Theta - lineW.Theta;

//            return rotat;
//        }

//        private void btCaptureN_Click(object sender, EventArgs e)
//        {
//            if (MessageBox.Show(GM.SaveCaptureImage(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab()) + " " + GV.Dlang.strSave, GV.Dlang.strSave + "Pattern too ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
//                return;
//            GM.SaveRecipeTemplate(_recipe, 2);
//        }

//        private void BtSave_Click(object sender, EventArgs e)
//        {
//            _AlignC.LastModifyTime = DateTime.Now;
//            GM.WriteRecipeXml(EditRecipe);
//        }

//        public void AutoFocusBinary(SentechNetToMat cam, int iLeftRight)
//        {
//            int nowLocZ;
//            double bestFocus = 0;
//            GV.Plc.GetLocation();
//            int MinPosition;
//            int MaxPosition;
//            int leftNowLoc = GV.NowLocation[GV.Plc.iiDDownLeftZ];
//            int rightNowLoc = GV.NowLocation[GV.Plc.iiDDownRightZ];
//            Mat mat;

//            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, true);

//            if (iLeftRight == 0)
//            {
//                nowLocZ = GV.NowLocation[GV.Plc.iiDDownLeftZ];
//                MinPosition = GV.Plc.iLimit[GV.Plc.iLimitLeftDownZmin];
//                MaxPosition = GV.Plc.iLimit[GV.Plc.iLimitLeftDownZmax];
//            }
//            else
//            {
//                nowLocZ = GV.NowLocation[GV.Plc.iiDDownRightZ];
//                MinPosition = GV.Plc.iLimit[GV.Plc.iLimitRightDownZmin];
//                MaxPosition = GV.Plc.iLimit[GV.Plc.iLimitRightDownZmax];
//            }
//            int bestPosition = nowLocZ;

//            while (MaxPosition - MinPosition > 100) // 假設 100 是最小步進量
//            {
//                int mid1 = MinPosition + (MaxPosition - MinPosition) / 3;
//                int mid2 = MaxPosition - (MaxPosition - MinPosition) / 3;

//                double focusValue1, focusValue2;

//                if (iLeftRight == 0)
//                {
//                    GV.Plc.AlignDownCameraMoveZAbsW(0, 0, 0, 0, 0, mid1, rightNowLoc);
//                    System.Threading.Thread.Sleep(100); // 等待機構穩定
//                    mat = GV.LeftBackCam.Grab();
//                    focusValue1 = CalculateFocus(mat);

//                    GV.Plc.AlignDownCameraMoveZAbsW(0, 0, 0, 0, 0, mid2, rightNowLoc);
//                    System.Threading.Thread.Sleep(100); // 等待機構穩定
//                    mat = GV.LeftBackCam.Grab();
//                    focusValue2 = CalculateFocus(mat);
//                }
//                else
//                {
//                    GV.Plc.AlignDownCameraMoveZAbsW(0, 0, 0, 0, 0, leftNowLoc, mid1);
//                    System.Threading.Thread.Sleep(100); // 等待機構穩定
//                    mat = GV.RightBackCam.Grab();
//                    focusValue1 = CalculateFocus(mat);

//                    GV.Plc.AlignDownCameraMoveZAbsW(0, 0, 0, 0, 0, leftNowLoc, mid2);
//                    System.Threading.Thread.Sleep(100); // 等待機構穩定
//                    mat = GV.RightBackCam.Grab();
//                    focusValue2 = CalculateFocus(mat);
//                }

//                if (focusValue1 > focusValue2)
//                {
//                    MaxPosition = mid2;
//                    if (focusValue1 > bestFocus)
//                    {
//                        bestFocus = focusValue1;
//                        bestPosition = mid1;
//                    }
//                }
//                else
//                {
//                    MinPosition = mid1;
//                    if (focusValue2 > bestFocus)
//                    {
//                        bestFocus = focusValue2;
//                        bestPosition = mid2;
//                    }
//                }
//            }

//            if (iLeftRight == 0)
//            {
//                GV.Plc.AlignDownCameraMoveZAbsW(0, 0, 0, 0, 0, bestPosition, rightNowLoc);
//            }
//            else
//            {
//                GV.Plc.AlignDownCameraMoveZAbsW(0, 0, 0, 0, 0, leftNowLoc, bestPosition);
//            }

//            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, false);
//        }

//        public void AutoFocus(SentechNetToMat cam, int iLeftRight)
//        {
//            int nowLocZ;
//            double bestFocus = 0;
//            GV.Plc.GetLocation();
//            int MinPosition;
//            int MaxPosition;
//            int leftNowLoc = GV.NowLocation[GV.Plc.iiDDownLeftZ];
//            int rightNowLoc = GV.NowLocation[GV.Plc.iiDDownRightZ];
//            Mat mat;

//            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, true);

//            if (iLeftRight == 0)
//            {
//                nowLocZ = GV.NowLocation[GV.Plc.iiDDownLeftZ];
//                MinPosition = GV.Plc.iLimit[GV.Plc.iLimitLeftDownZmin];
//                MaxPosition = GV.Plc.iLimit[GV.Plc.iLimitLeftDownZmax];
//            }
//            else
//            {
//                nowLocZ = GV.NowLocation[GV.Plc.iiDDownRightZ];
//                MinPosition = GV.Plc.iLimit[GV.Plc.iLimitRightDownZmin];
//                MaxPosition = GV.Plc.iLimit[GV.Plc.iLimitRightDownZmax];
//            }
//            int bestPosition = nowLocZ;

//            for (double pos = MinPosition; pos <= MaxPosition; pos += 1000) // 假設 0.1 是步進量
//            {
//                if (iLeftRight == 0)
//                {
//                    GV.Plc.AlignDownCameraMoveZAbsW(0, 0, 0, 0, 0, pos, rightNowLoc);
//                    System.Threading.Thread.Sleep(100); // 等待機構穩定
//                    mat = GV.LeftBackCam.Grab();
//                }
//                else
//                {
//                    GV.Plc.AlignDownCameraMoveZAbsW(0, 0, 0, 0, 0, leftNowLoc, pos);
//                    System.Threading.Thread.Sleep(100); // 等待機構穩定
//                    mat = GV.RightBackCam.Grab();
//                }

//                double focusValue = CalculateFocus(mat);

//                if (focusValue > bestFocus)
//                {
//                    bestFocus = focusValue;
//                    bestPosition = (int)pos;
//                }
//            }

//            if (iLeftRight == 0)
//            {
//                GV.Plc.AlignDownCameraMoveZAbsW(0, 0, 0, 0, 0, bestPosition, rightNowLoc);
//            }
//            else
//            {
//                GV.Plc.AlignDownCameraMoveZAbsW(0, 0, 0, 0, 0, leftNowLoc, bestPosition);
//            }

//            GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, false);
//        }
//    }
//}

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

        public CMLearnPatternBack()
        {
            InitializeComponent();
            GV.LeftDownWindowOnAlignPage = skLeft;
            GV.RightDownWindowOnAlignPage = skRight;
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

        private void CMLearnPattern_Load(object sender, EventArgs e)
        {
            if (GV.AppSettingParm.DebugMode)
            {
                lbDebugMsg.Visible = true;
            }

            EditRecipe = GV.NowRecipeNumber;

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
                cbRBackMaskAlgo.Items.Add(GV.Dlang.strTemplate);
                cbRBackMaskAlgo.Items.Add(GV.Dlang.strEdge);
                _recipe = GV._recipe;
                _AlignC = _recipe.AlignC;
                cbLWaferAlgo.SelectedIndex = 0;
                cbRWaferAlgo.SelectedIndex = 0;
                cbLBackMaskAlgo.SelectedIndex = 0;
                cbRBackMaskAlgo.SelectedIndex = 0;
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
            skLeft.MaskMp = lMaskMp;
            skLeft.WaferMp = lWaferMp;
            skRight.MaskMp = rMaskMp;
            skRight.WaferMp = rWaferMp;
        }
        public void DrawMatchPosition()
        {

            while (!GV.AppEnding)
            {
                while (GV.TabOption == GV.Tab.BackLearn)
                {
                    Mat leftImageSrc = GV.LeftBackCam.IsFileImage ? LViewMat : GV.LeftBackCam.Grab();
                    Mat rightImageSrc = GV.RightBackCam.IsFileImage ? RViewMat : GV.RightBackCam.Grab();
                    using (Mat leftImage = leftImageSrc?.Clone())
                    using (Mat rightImage = rightImageSrc?.Clone())
                    {
                        if (rbLBackMask.Checked)
                        {
                            //GV.matcherLLW.MatMatchWithAlgo(0, leftImage, ref lMaskMp, _AlignC.LLWaferAlgorithm);
                            //GV.matcherRLW.MatMatchWithAlgo(0, rightImage, ref rMaskMp, _AlignC.RLWaferAlgorithm);
                            GV.matcherLLM.MatMatchWithAlgo(0, LeftMaskMat, ref lMaskMp, _AlignC.LLMaskAlgorithm);
                            GV.matcherRLM.MatMatchWithAlgo(0, RightMaskMat, ref rMaskMp, _AlignC.RLMaskAlgorithm);
                            GV.matcherLHW.MatMatchWithAlgo(0, leftImage, ref lWaferMp, _AlignC.LHWaferAlgorithm);
                            GV.matcherRHW.MatMatchWithAlgo(0, rightImage, ref rWaferMp, _AlignC.RHWaferAlgorithm);
                        }
                        else
                        {
                            GV.matcherLLM.MatMatchWithAlgo(0, LeftMaskMat, ref lMaskMp, _AlignC.LLMaskAlgorithm);
                            GV.matcherRLM.MatMatchWithAlgo(0, RightMaskMat, ref rMaskMp, _AlignC.RLMaskAlgorithm);
                            GV.matcherLHW.MatMatchWithAlgo(0, leftImage, ref lWaferMp, _AlignC.LHWaferAlgorithm);
                            GV.matcherRHW.MatMatchWithAlgo(0, rightImage, ref rWaferMp, _AlignC.RHWaferAlgorithm);
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
                            //skLeft.MaskMp = new MatchPosition { X = lMaskMp.X, Y = lMaskMp.Y, Score = lMaskMp.Score };
                            //skLeft.WaferMp = new MatchPosition { X = lWaferMp.X, Y = lWaferMp.Y, Score = lWaferMp.Score };
                            //skRight.MaskMp = new MatchPosition { X = rMaskMp.X, Y = rMaskMp.Y, Score = rMaskMp.Score };
                            //skRight.WaferMp = new MatchPosition { X = rWaferMp.X, Y = rWaferMp.Y, Score = rWaferMp.Score };
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

            if (rBLowMagnification.Checked)
            {
                if (_recipe.LeftLowMaskMat != null && !_recipe.LeftLowMaskMat.Empty() &&
            _recipe.LeftLowMaskMat.Width > 0 && _recipe.LeftLowMaskMat.Height > 0)
                {
                    try
                    {
                        pbLTopMask.Image = _recipe.LeftLowMaskMat.ToBitmap();
                    }
                    catch (Exception ex)
                    {

                    }
                }

                if (_recipe.RightLowMaskMat != null && !_recipe.RightLowMaskMat.Empty() &&
                    _recipe.RightLowMaskMat.Width > 0 && _recipe.RightLowMaskMat.Height > 0)
                {
                    try
                    {
                        pbRTopMask.Image = _recipe.RightLowMaskMat.ToBitmap();
                    }
                    catch (Exception ex)
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
                    catch (Exception ex)
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
                    catch (Exception ex)
                    {
                    }
                }

                cbLTopMaskAlgo.SelectedIndex = (int)_AlignC.LHMaskAlgorithm;
                cbRTopMaskAlgo.SelectedIndex = (int)_AlignC.RHMaskAlgorithm;
            }

            if (_recipe.LeftLowWaferMat != null && !_recipe.LeftLowWaferMat.Empty() &&
        _recipe.LeftLowWaferMat.Width > 0 && _recipe.LeftLowWaferMat.Height > 0)
            {
                try
                {
                    pbLBackMask.Image = _recipe.LeftLowMaskMat.ToBitmap();
                }
                catch { }
                //using(var bitmap=_recipe.LeftLowMaskMat.ToBitmap())
                //{
                //    pbLBackMask.Image?.Dispose();
                //    pbLBackMask.Image =(Bitmap)bitmap.Clone();
                //}
            }

            if (_recipe.LeftHighWaferMat != null && !_recipe.LeftHighWaferMat.Empty() &&
                _recipe.LeftHighWaferMat.Width > 0 && _recipe.LeftHighWaferMat.Height > 0)
            {
                try
                {
                    pbLWafer.Image = _recipe.LeftHighWaferMat.ToBitmap();
                }
                catch { }
                //using (var bitmap = _recipe.LeftHighWaferMat.ToBitmap())
                //{
                //    pbLWafer.Image?.Dispose();
                //    pbLWafer.Image = (Bitmap)bitmap.Clone();
                //}
            }

            if (_recipe.RightLowWaferMat != null && !_recipe.RightLowWaferMat.Empty() &&
                _recipe.RightLowWaferMat.Width > 0 && _recipe.RightLowWaferMat.Height > 0)
            {
                try
                {
                    pbRBackMask.Image = _recipe.RightLowMaskMat.ToBitmap();
                }
                catch { }
                //using (var bitmap = _recipe.RightLowMaskMat.ToBitmap())
                //{
                //    pbRBackMask.Image?.Dispose();
                //    pbRBackMask.Image = (Bitmap)bitmap.Clone();
                //}
            }

            if (_recipe.RightHighWaferMat != null && !_recipe.RightHighWaferMat.Empty() &&
                _recipe.RightHighWaferMat.Width > 0 && _recipe.RightHighWaferMat.Height > 0)
            {
                try
                {
                    pbRWafer.Image = _recipe.RightHighWaferMat.ToBitmap();
                }
                catch { }
                //using (var bitmap = _recipe.RightHighWaferMat.ToBitmap())
                //{
                //    pbRWafer.Image?.Dispose();
                //    pbRWafer.Image = (Bitmap)bitmap.Clone();
                //}
            }

            cbLBackMaskAlgo.SelectedIndex = (int)_AlignC.LLMaskAlgorithm;
            cbLWaferAlgo.SelectedIndex = (int)_AlignC.LHWaferAlgorithm;
            cbRBackMaskAlgo.SelectedIndex = (int)_AlignC.RLMaskAlgorithm;
            cbRWaferAlgo.SelectedIndex = (int)_AlignC.RHWaferAlgorithm;

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

        private void ChangeRecipe()
        {
            _recipe = GV._recipe;
            _AlignC = _recipe.AlignC;
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
                        NUDLMaskX.Visible = true;
                        NUDLMaskY.Visible = true;
                        NUDRMaskX.Visible = true;
                        NUDRMaskY.Visible = true;
                        BtLMaskAdjSave.Visible = true;
                        BtRMaskAdjSave.Visible = true;
                        lblLMaskX.Visible = true;
                        lblLMaskY.Visible = true;
                        lblRMaskX.Visible = true;
                        lblRMaskY.Visible = true;
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
            m = new Mat(fullImage, roi);
            pbLWafer.Image = m.ToBitmap();

            if (MessageBox.Show(GV.Dlang.strLWafer, GV.Dlang.strLWaferSave, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                pbLWafer.Image = _recipe.LeftHighWaferMat.ToBitmap();
                if (cbLWaferAlgo.SelectedIndex == 2)
                {
                    pbLBackWaferClassList.Visible = true;
                    btBackLabelPatternL.Visible = true;
                    pbLWafer.Visible = false;
                }
                return;
            }

            _AlignC.LBWaferBright = tBLeftBackWaferLight.Value;
            _recipe.SetLeftHighWaferMat(m);
            GrayImage d = new GrayImage(m.Width, m.Height);
            d.Fill(255);
            _recipe.LeftHighWaferMask = d;
            _AlignC.LHWaferAlgorithm = Algorithm(cbLWaferAlgo.SelectedIndex);
            GV.matcherLHW.LearnWithAlgo(m, d, _AlignC.LHWaferAlgorithm);
            _AlignC.LastModifyTime = DateTime.Now;
            GM.WriteRecipeXml(EditRecipe);
            if (cbLWaferAlgo.SelectedIndex == 2)
            {
                try
                {
                    string trainPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Python", "Train", GV._recipe.RecipeName);
                    DateTime now = DateTime.Now;
                    Directory.CreateDirectory(trainPath);
                    string fn = string.Concat(trainPath, "\\LB", now.ToString("HH_mm_ss"));
                    Cv2.ImWrite(string.Concat(fn, ".bmp"), fullImage);
                    var labelPath = string.Concat(fn, ".txt");

                    float xCenter = (roi.X + roi.Width / 2f) / fullImage.Width;
                    float yCenter = (roi.Y + roi.Height / 2f) / fullImage.Height;
                    float w = roi.Width / (float)fullImage.Width;
                    float h = roi.Height / (float)fullImage.Height;
                    int classId = pbLBackWaferClassList.SelectedIndex;
                    List<YoloBox> boxes = new List<YoloBox>();
                    boxes.Add(new YoloBox { ClassId = classId, X = xCenter, Y = yCenter, W = w, H = h });
                    System.IO.File.WriteAllLines(labelPath, boxes.Select(b => $"{b.ClassId} {b.X:F6} {b.Y:F6} {b.W:F6} {b.H:F6}"));
                }
                catch
                {

                }
                pbLBackWaferClassList.Visible = true;
                btBackLabelPatternL.Visible = true;
                pbLWafer.Visible = false;
            }
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
                    pbRBackWaferClassList.Visible = true;
                    btBackLabelPatternR.Visible = true;
                    pbRWafer.Visible = false;
                }
                return;
            }

            _AlignC.RBWaferBright = tBRightBackWaferLight.Value;
            _recipe.SetRightHighWaferMat(m);
            GrayImage d = new GrayImage(m.Width, m.Height);
            d.Fill(255);
            _recipe.RightHighWaferMask = d;
            _AlignC.RHWaferAlgorithm = Algorithm(cbRWaferAlgo.SelectedIndex);
            GV.matcherRHW.LearnWithAlgo(m, d, _AlignC.RHWaferAlgorithm);
            _AlignC.LastModifyTime = DateTime.Now;
            GM.WriteRecipeXml(EditRecipe);
            if (cbRWaferAlgo.SelectedIndex == 2)
            {
                try
                {
                    string trainPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Python", "Train", GV._recipe.RecipeName);
                    DateTime now = DateTime.Now;
                    Directory.CreateDirectory(trainPath);
                    string fn = string.Concat(trainPath, "\\RB", now.ToString("HH_mm_ss"));
                    Cv2.ImWrite(string.Concat(fn, ".bmp"), fullImage);
                    var labelPath = string.Concat(fn, ".txt");
                    float xCenter = (roi.X + roi.Width / 2f) / fullImage.Width;
                    float yCenter = (roi.Y + roi.Height / 2f) / fullImage.Height;
                    float w = roi.Width / (float)fullImage.Width;
                    float h = roi.Height / (float)fullImage.Height;
                    int classId = pbRBackWaferClassList.SelectedIndex;
                    List<YoloBox> boxes = new List<YoloBox>();
                    boxes.Add(new YoloBox { ClassId = classId, X = xCenter, Y = yCenter, W = w, H = h });
                    System.IO.File.WriteAllLines(labelPath, boxes.Select(b => $"{b.ClassId} {b.X:F6} {b.Y:F6} {b.W:F6} {b.H:F6}"));
                }
                catch
                {
                }
                pbRBackWaferClassList.Visible = true;
                btBackLabelPatternR.Visible = true;
                pbRWafer.Visible = false;
            }
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
            m = new Mat(fullImage, roi);
            pbRBackMask.Image = m.ToBitmap();

            if (MessageBox.Show(GV.Dlang.strRBottomMask, GV.Dlang.strRBottomMaskSave, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                pbRBackMask.Image = _recipe.RightLowWaferMat.ToBitmap();

                return;
            }

            _AlignC.RBMaskBright = tBRightBackMaskLight.Value;

            RightMaskMat = GV.RightBackCam.Grab();
            GV.RightMaskMat = RightMaskMat;
            _recipe.SetRightLowMaskMat(m);
            GrayImage d = new GrayImage(m.Width, m.Height);
            d.Fill(255);
            _recipe.RightLowMaskMask = d;
            _AlignC.RLMaskAlgorithm = Algorithm(cbRBackMaskAlgo.SelectedIndex);
            GV.matcherRLM.LearnWithAlgo(m, d, _AlignC.RLMaskAlgorithm);
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
                    _recipe.LeftHighWaferMask = dialogPaintMask.Mask;
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
            GV.Light.ChangeBrightness("left", tBLeftTopMaskLight.Value);
        }

        private void TBLeftBackMaskLight_ValueChanged(object sender, EventArgs e)
        {
            GV.Light.ChangeBrightness("leftback", tBLeftBackMaskLight.Value);
        }

        private void tBLeftBackWaferLight_ValueChanged(object sender, EventArgs e)
        {
            GV.Light.ChangeBrightness("leftback", tBLeftBackWaferLight.Value);
        }

        private void TBRightBackWaferLight_ValueChanged(object sender, EventArgs e)
        {
            GV.Light.ChangeBrightness("rightback", tBRightBackWaferLight.Value);
        }

        private void TBRightBackMaskLight_ValueChanged(object sender, EventArgs e)
        {
            GV.Light.ChangeBrightness("rightback", tBRightBackMaskLight.Value);
        }

        private void tBRightTopMaskLight_ValueChanged(object sender, EventArgs e)
        {
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
            Mat fullImage;
            fullImage = GV.LeftBackCam.Grab();

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
            m = new Mat(fullImage, roi);
            pbLBackMask.Image = m.ToBitmap();

            if (MessageBox.Show(GV.Dlang.strLBottomMask, GV.Dlang.strLBottomMaskSave, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                pbLBackMask.Image = _recipe.LeftLowMaskMat.ToBitmap();
                return;
            }
            _AlignC.LBMaskBright = tBLeftBackMaskLight.Value;
            LeftMaskMat = GV.LeftBackCam.Grab();
            GV.LeftMaskMat = LeftMaskMat;
            _recipe.SetLeftLowMaskMat(m);
            GrayImage d = new GrayImage(m.Width, m.Height);
            d.Fill(255);
            _recipe.LeftLowMaskMask = d;
            _AlignC.LLMaskAlgorithm = Algorithm(cbLBackMaskAlgo.SelectedIndex);
            GV.matcherLLM.LearnWithAlgo(m, d, _AlignC.LLMaskAlgorithm);
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
            _AlignC.LastModifyTime = DateTime.Now;
            GM.WriteRecipeXml(EditRecipe);
        }

        public void ShowMe()
        {
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

            if (GMPBackLeft == null)
            {
                GMPBackLeft = new Mat();
                GMPBackRight = new Mat();
                pbLBackWaferClassList.Items.Clear();
                pbRBackWaferClassList.Items.Clear();
                Dictionary<string, Int16> ClassList = GV.AIClassList.GetClassList();
                foreach (var className in ClassList)
                {
                    pbRBackWaferClassList.Items.Add(className);
                    pbLBackWaferClassList.Items.Add(className);
                }
            }
            if (_recipe.LeftLowMaskMat != null && !_recipe.LeftLowMaskMat.Empty())
            {
                GV.matcherLLM.LearnWithAlgo(_recipe.LeftLowMaskMat, _recipe.LeftLowMaskMask, _AlignC.LLMaskAlgorithm);
            }

            if (_recipe.RightLowMaskMat != null && !_recipe.RightLowMaskMat.Empty())
            {
                // ✅ 關鍵:確保右邊 Mask matcher 學習模板
                GV.matcherRLM.LearnWithAlgo(_recipe.RightLowMaskMat, _recipe.RightLowMaskMask, _AlignC.RLMaskAlgorithm);
            }

            if (_recipe.LeftHighWaferMat != null && !_recipe.LeftHighWaferMat.Empty())
            {
                GV.matcherLHW.LearnWithAlgo(_recipe.LeftHighWaferMat, _recipe.LeftHighWaferMask, _AlignC.LHWaferAlgorithm);
            }

            if (_recipe.RightHighWaferMat != null && !_recipe.RightHighWaferMat.Empty())
            {
                // ✅ 關鍵:確保右邊 Wafer matcher 學習模板
                GV.matcherRHW.LearnWithAlgo(_recipe.RightHighWaferMat, _recipe.RightHighWaferMask, _AlignC.RHWaferAlgorithm);
            }
            skLeft.CanCenterLine = true;
            skLeft.CanRectMaskAndWafer = true;
            skLeft.CanTrackPattern = true;
            skLeft.CanZoom = true;

            skRight.CanCenterLine = true;
            skRight.CanRectMaskAndWafer = true;
            skRight.CanTrackPattern = true;
            skRight.CanZoom = true;

            LeftMaskMat = GV.LeftMaskMat.Clone();
            RightMaskMat = GV.RightMaskMat.Clone();
            GV.LeftUpCam?.Freeze();
            GV.RightUpCam?.Freeze();
            GV.RingLight.ChangeBrightness("left", 0);
            GV.RingLight.ChangeBrightness("right", 0);
            iAdmin = GV.HwndFormMain.iAdmin;
            GV.LeftBackCam.SetWindow(skLeft);
            GV.RightBackCam.SetWindow(skRight);

            if (GV.AppSettingParm.LeftBackCamEnable)
            {
                lbEmulationModeL.Visible = false;
            }

            if (GV.AppSettingParm.RightBackCamEnable)
            {
                lbEmulationModeR.Visible = false;
            }

            GV.LeftBackCam.Live();

            GV.RightBackCam.Live();
            if (DrawMatchThread == null)
            {
                DrawMatchThread = new Thread(DrawMatchPosition);

                DrawMatchThread.Start();
            }
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

            if (_AlignC.AdjuestZ == 1)
            {
                cBAdjZ.Checked = true;
            }
            else
            {
                cBAdjZ.Checked = false;
            }
            NUDLMaskX.Value = _AlignC.LMaskAdjX;
            NUDLMaskY.Value = _AlignC.LMaskAdjY;
            NUDRMaskX.Value = _AlignC.RMaskAdjX;
            NUDRMaskY.Value = _AlignC.RMaskAdjY;
            groupBox4.Location = new System.Drawing.Point(560, 60);
            CheckParam();
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
            CheckParam();
        }

        private void cbLWaferAlgo_SelectedIndexChanged(object sender, EventArgs e)
        {
            _AlignC.LHWaferAlgorithm = Algorithm(cbLWaferAlgo.SelectedIndex);
            if (cbLWaferAlgo.SelectedIndex == 2)
            {
                pbLBackWaferClassList.Visible = true;
                btBackLabelPatternL.Visible = true;
                pbLWafer.Visible = false;

            }
            else
            {
                pbLBackWaferClassList.Visible = false;
                btBackLabelPatternL.Visible = false;
                pbLWafer.Visible = true;

            }
            CheckParam();
        }

        private void cbRWaferAlgo_SelectedIndexChanged(object sender, EventArgs e)
        {
            _AlignC.RHWaferAlgorithm = Algorithm(cbRWaferAlgo.SelectedIndex);
            if (cbRWaferAlgo.SelectedIndex == 2)
            {
                pbRBackWaferClassList.Visible = true;
                btBackLabelPatternR.Visible = true;
                pbRWafer.Visible = false;
            }
            else
            {
                pbRBackWaferClassList.Visible = false;
                btBackLabelPatternR.Visible = false;
                pbRWafer.Visible = true;
            }
            CheckParam();
        }

        private void cbRBackMaskAlgo_SelectedIndexChanged(object sender, EventArgs e)
        {
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

        private void btFindLBMaskCenter_Click(object sender, EventArgs e)
        {
            Mat m;
            Rect roi;

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
            pbLBackMask.Image = newm.ToBitmap();

            if (MessageBox.Show(GV.Dlang.strbtnSaveLLM, GV.Dlang.strSureSaveLLM, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                pbLBackMask.Image = m.ToBitmap();
                return;
            }

            GrayImage d = new GrayImage(newm.Width, newm.Height);
            d.Fill(255);
            _recipe.LeftLowMaskMask = d;
            _recipe.SetLeftLowMaskMat(newm);
            _AlignC.LLMaskAlgorithm = Algorithm(cbLBackMaskAlgo.SelectedIndex);
            GV.matcherLLM.LearnWithAlgo(newm, d, _AlignC.LLMaskAlgorithm);
            _AlignC.LastModifyTime = DateTime.Now;
            GM.WriteRecipeXml(EditRecipe);
        }

        private void BtFindLWaferCenter_Click(object sender, EventArgs e)
        {
            Mat m;
            Rect roi;

            if (_recipe.LeftHighWaferMat.Empty())
            {
                MessageBox.Show("請先學習光罩圖型，再找中心!!", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            m = _recipe.LeftHighWaferMat;

            DialogFindCenter FindCenter = new DialogFindCenter()
            {
                img = m.Clone(),
                iMaskWaferRadio = 0.8,
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
            pbLWafer.Image = newm.ToBitmap();

            if (MessageBox.Show(GV.Dlang.strbtnSaveLLW, GV.Dlang.strSureSaveLLW, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                pbLWafer.Image = m.ToBitmap();
                return;
            }

            _recipe.SetLeftHighWaferMat(newm);
            GrayImage d = new GrayImage(newm.Width, newm.Height);
            d.Fill(255);
            _recipe.LeftHighWaferMask = d;
            _AlignC.LHWaferAlgorithm = Algorithm(cbLWaferAlgo.SelectedIndex);
            GV.matcherLHW.LearnWithAlgo(newm, d, _AlignC.LHWaferAlgorithm);
            _AlignC.LastModifyTime = DateTime.Now;
            GM.WriteRecipeXml(EditRecipe);
        }

        private void BtFindRWaferCenter_Click(object sender, EventArgs e)
        {
            Rect roi;

            if (_recipe.RightHighWaferMat.Empty())
            {
                MessageBox.Show("請先學習光罩圖型，再找中心!!", "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Mat m = _recipe.RightHighWaferMat;

            DialogFindCenter FindCenter = new DialogFindCenter()
            {
                img = m.Clone(),
                iMaskWaferRadio = 0.8,
                imgM = new GrayImage(_recipe.RightHighWaferMask)
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
            pbRWafer.Image = newm.ToBitmap();

            if (MessageBox.Show(GV.Dlang.strbtnSaveRLW, GV.Dlang.strSureSaveRLW, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                pbRWafer.Image = m.ToBitmap();
                return;
            }

            _recipe.SetRightHighWaferMat(newm);
            GrayImage d = new GrayImage(newm.Width, newm.Height);
            d.Fill(255);
            _recipe.RightHighWaferMask = d;
            _AlignC.RHWaferAlgorithm = Algorithm(cbRWaferAlgo.SelectedIndex);
            GV.matcherRHW.LearnWithAlgo(newm, d, _AlignC.RHWaferAlgorithm);
            _AlignC.LastModifyTime = DateTime.Now;
            GM.WriteRecipeXml(EditRecipe);
        }

        private void BtFindRBMaskCenter_Click(object sender, EventArgs e)
        {
            Mat m;
            Rect roi;
            GV.TickCount = 0;

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
            pbRBackMask.Image = newm.ToBitmap();

            if (MessageBox.Show(GV.Dlang.strbtnSaveRLM, GV.Dlang.strSureSaveRLM, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                pbRBackMask.Image = m.ToBitmap();
                return;
            }

            GrayImage d = new GrayImage(newm.Width, newm.Height);
            d.Fill(255);
            _recipe.RightLowMaskMask = d;
            _recipe.SetRightLowMaskMat(newm);
            _AlignC.RLMaskAlgorithm = Algorithm(cbRBackMaskAlgo.SelectedIndex);
            GV.matcherRLM.LearnWithAlgo(newm, d, _AlignC.RLMaskAlgorithm);
            _AlignC.LastModifyTime = DateTime.Now;
            GM.WriteRecipeXml(EditRecipe);
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

        private async Task<ProductMatchPositions> GetAllMatchPositionAsync(Mat lSrc, Mat rSrc, string alignMagnification, CancellationToken ct = default)
        {

            int i = 0;
            double patternCenterDistance = _AlignC.PatternCenterDistanceUm;
            while (i < 3)
            {
                lSrc.CopyTo(GMPBackLeft);
                lSrc.CopyTo(GMPBackRight);
                if (GV.AppSettingParm.Emulation != true)
                {
                    if (alignMagnification == "low")
                    {
                        var t1 = GV.matcherLLM.MatMatchWithAlgoAsync(0, GMPBackLeft, GMPBacklMaskMp, _AlignC.LLMaskAlgorithm, ct);
                        var t2 = GV.matcherRLM.MatMatchWithAlgoAsync(0, GMPBackRight, GMPBackrMaskMp, _AlignC.RLMaskAlgorithm, ct);
                        var t3 = GV.matcherLLW.MatMatchWithAlgoAsync(0, GMPBackLeft, GMPBacklWaferMp, _AlignC.LLWaferAlgorithm, ct);
                        var t4 = GV.matcherRLW.MatMatchWithAlgoAsync(0, GMPBackRight, GMPBackrWaferMp, _AlignC.RLWaferAlgorithm, ct);
                        await Task.WhenAll(t1, t2, t3, t4).ConfigureAwait(false);
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
                        GMPBacklWaferMp.X -= (float)(_AlignC.XOffset / GV.ZoomLensInfo.LeftUmPerPixelX[_AlignC.AlignHighMagnification]);
                        GMPBackrWaferMp.X -= (float)(_AlignC.XOffset / GV.ZoomLensInfo.RightUmPerPixelX[_AlignC.AlignHighMagnification]);
                        GMPBacklWaferMp.Y -= (float)(_AlignC.YLOffset / GV.ZoomLensInfo.LeftUmPerPixelY[_AlignC.AlignHighMagnification]);
                        GMPBackrWaferMp.Y -= (float)(_AlignC.YROffset / GV.ZoomLensInfo.RightUmPerPixelY[_AlignC.AlignHighMagnification]);
                    }
                    else if (alignMagnification == "back")
                    {
                        var t1 = GV.matcherLLW.MatMatchWithAlgoAsync(0, LeftMaskMat, GMPBacklMaskMp, _AlignC.LLWaferAlgorithm);
                        var t2 = GV.matcherRLW.MatMatchWithAlgoAsync(0, RightMaskMat, GMPBackrMaskMp, _AlignC.RLWaferAlgorithm);
                        var t3 = GV.matcherLHW.MatMatchWithAlgoAsync(0, GMPBackLeft, GMPBacklWaferMp, _AlignC.LHWaferAlgorithm);
                        var t4 = GV.matcherRHW.MatMatchWithAlgoAsync(0, GMPBackRight, GMPBackrWaferMp, _AlignC.RHWaferAlgorithm);
                        await Task.WhenAll(t1, t2, t3, t4).ConfigureAwait(false);
                        GMPBacklWaferMp.X -= (float)(_AlignC.XOffset / GV.ZoomLensInfo.LeftDownUmPerPixelX);
                        GMPBackrWaferMp.X -= (float)(_AlignC.XOffset / GV.ZoomLensInfo.RightDownUmPerPixelX);
                        GMPBacklWaferMp.Y -= (float)(_AlignC.YLOffset / GV.ZoomLensInfo.LeftDownUmPerPixelY);
                        GMPBackrWaferMp.Y -= (float)(_AlignC.YROffset / GV.ZoomLensInfo.RightDownUmPerPixelY);
                        // patternCenterDistancePixel = _AlignC.PatternCenterDistanceUm / ((GV.ZoomLensInfo.LeftDownUmPerPixelX + GV.ZoomLensInfo.RightDownUmPerPixelX) / 2);
                    }
                    else if (alignMagnification == "backmask")
                    {
                        var t1 = GV.matcherLLW.MatMatchWithAlgoAsync(0, GMPBackLeft, GMPBacklMaskMp, _AlignC.LLWaferAlgorithm);
                        var t2 = GV.matcherRLW.MatMatchWithAlgoAsync(0, GMPBackRight, GMPBackrMaskMp, _AlignC.RLWaferAlgorithm);
                        await Task.WhenAll(t1, t2).ConfigureAwait(false);
                    }
                    else if (alignMagnification == "checkmask")
                    {
                        var t1 = GV.matcherLHM.MatMatchWithAlgoAsync(0, GMPBackLeft, GMPBacklMaskMp, _AlignC.LHMaskAlgorithm);
                        var t2 = GV.matcherRHM.MatMatchWithAlgoAsync(0, GMPBackRight, GMPBackrMaskMp, _AlignC.RHMaskAlgorithm);
                        await Task.WhenAll(t1, t2).ConfigureAwait(false);
                    }
                    //else if (alignMagnification == "backcheck")
                    //{
                    //   var t1= GV.matcherLLW.MatMatchWithAlgo(0, lSrc.Clone(), ref lWaferMp, _AlignC.LLWaferAlgorithm);
                    //    GV.matcherRLW.MatMatchWithAlgo(0, rSrc.Clone(), ref rWaferMp, _AlignC.RLWaferAlgorithm);
                    //    GV.matcherLHW.MatMatchWithAlgo(0, lSrc.Clone(), ref lWaferMp, _AlignC.LHWaferAlgorithm);
                    //    GV.matcherRHW.MatMatchWithAlgo(0, rSrc.Clone(), ref rWaferMp, _AlignC.RHWaferAlgorithm);
                    //}
                    skLeft.MaskMp = GMPBacklMaskMp;
                    skLeft.WaferMp = GMPBacklWaferMp;
                    skRight.MaskMp = GMPBackrMaskMp;
                    skRight.WaferMp = GMPBackrWaferMp;
                }

            }
            return new ProductMatchPositions(GMPBacklMaskMp, GMPBacklWaferMp, GMPBackrMaskMp, GMPBackrWaferMp, patternCenterDistance);
        }

        private ProductMatchPositions GetAllMatchPostion(Mat lSrc, Mat rSrc, string alignMmagnification)
        {

            MatchPosition lMaskMp = new MatchPosition();
            MatchPosition rMaskMp = new MatchPosition();
            MatchPosition lWaferMp = new MatchPosition();
            MatchPosition rWaferMp = new MatchPosition();
            double patternCenterDistance = _AlignC.PatternCenterDistanceUm;

            if (GV.AppSettingParm.Emulation != true)
            {
                if (alignMmagnification == "low")
                {
                    GV.matcherLLM.MatMatchWithAlgo(0, lSrc.Clone(), ref lMaskMp, _AlignC.LLMaskAlgorithm);
                    GV.matcherRLM.MatMatchWithAlgo(0, rSrc.Clone(), ref rMaskMp, _AlignC.RLMaskAlgorithm);
                    GV.matcherLLW.MatMatchWithAlgo(0, lSrc.Clone(), ref lWaferMp, _AlignC.LLWaferAlgorithm);
                    GV.matcherRLW.MatMatchWithAlgo(0, rSrc.Clone(), ref rWaferMp, _AlignC.RLWaferAlgorithm);

                    lWaferMp.X -= (float)(_AlignC.XOffset / GV.ZoomLensInfo.LeftUmPerPixelX[_AlignC.AlignLowMagnification]);
                    rWaferMp.X -= (float)(_AlignC.XOffset / GV.ZoomLensInfo.RightUmPerPixelX[_AlignC.AlignLowMagnification]);
                    lWaferMp.Y -= (float)(_AlignC.YLOffset / GV.ZoomLensInfo.LeftUmPerPixelY[_AlignC.AlignLowMagnification]);
                    rWaferMp.Y -= (float)(_AlignC.YROffset / GV.ZoomLensInfo.RightUmPerPixelY[_AlignC.AlignLowMagnification]);
                }
                else if (alignMmagnification == "high")
                {
                    GV.matcherLHM.MatMatchWithAlgo(0, lSrc.Clone(), ref lMaskMp, _AlignC.LHMaskAlgorithm);
                    GV.matcherRHM.MatMatchWithAlgo(0, rSrc.Clone(), ref rMaskMp, _AlignC.RHMaskAlgorithm);
                    GV.matcherLHW.MatMatchWithAlgo(0, lSrc.Clone(), ref lWaferMp, _AlignC.LHWaferAlgorithm);
                    GV.matcherRHW.MatMatchWithAlgo(0, rSrc.Clone(), ref rWaferMp, _AlignC.RHWaferAlgorithm);
                    lWaferMp.X -= (float)(_AlignC.XOffset / GV.ZoomLensInfo.LeftUmPerPixelX[_AlignC.AlignHighMagnification]);
                    rWaferMp.X -= (float)(_AlignC.XOffset / GV.ZoomLensInfo.RightUmPerPixelX[_AlignC.AlignHighMagnification]);
                    lWaferMp.Y -= (float)(_AlignC.YLOffset / GV.ZoomLensInfo.LeftUmPerPixelY[_AlignC.AlignHighMagnification]);
                    rWaferMp.Y -= (float)(_AlignC.YROffset / GV.ZoomLensInfo.RightUmPerPixelY[_AlignC.AlignHighMagnification]);
                }
                else if (alignMmagnification == "back")
                {
                    GV.matcherLLW.MatMatchWithAlgo(0, LeftMaskMat, ref lMaskMp, _AlignC.LLWaferAlgorithm);
                    GV.matcherRLW.MatMatchWithAlgo(0, RightMaskMat, ref rMaskMp, _AlignC.RLWaferAlgorithm);
                    GV.matcherLHW.MatMatchWithAlgo(0, lSrc.Clone(), ref lWaferMp, _AlignC.LHWaferAlgorithm);
                    GV.matcherRHW.MatMatchWithAlgo(0, rSrc.Clone(), ref rWaferMp, _AlignC.RHWaferAlgorithm);
                    lWaferMp.X -= (float)(_AlignC.XOffset / GV.ZoomLensInfo.LeftDownUmPerPixelX);
                    rWaferMp.X -= (float)(_AlignC.XOffset / GV.ZoomLensInfo.RightDownUmPerPixelX);
                    lWaferMp.Y -= (float)(_AlignC.YLOffset / GV.ZoomLensInfo.LeftDownUmPerPixelY);
                    rWaferMp.Y -= (float)(_AlignC.YROffset / GV.ZoomLensInfo.RightDownUmPerPixelY);
                    // patternCenterDistancePixel = _AlignC.PatternCenterDistanceUm / ((GV.ZoomLensInfo.LeftDownUmPerPixelX + GV.ZoomLensInfo.RightDownUmPerPixelX) / 2);
                }
                else if (alignMmagnification == "backmask")
                {
                    GV.matcherLLW.MatMatchWithAlgo(0, lSrc.Clone(), ref lMaskMp, _AlignC.LLWaferAlgorithm);
                    GV.matcherRLW.MatMatchWithAlgo(0, rSrc.Clone(), ref rMaskMp, _AlignC.RLWaferAlgorithm);
                }
                else if (alignMmagnification == "checkmask")
                {
                    GV.matcherLHM.MatMatchWithAlgo(0, lSrc.Clone(), ref lMaskMp, _AlignC.LHMaskAlgorithm);
                    GV.matcherRHM.MatMatchWithAlgo(0, rSrc.Clone(), ref rMaskMp, _AlignC.RHMaskAlgorithm);
                }
                else if (alignMmagnification == "backcheck")
                {
                    GV.matcherLLW.MatMatchWithAlgo(0, lSrc.Clone(), ref lWaferMp, _AlignC.LLWaferAlgorithm);
                    GV.matcherRLW.MatMatchWithAlgo(0, rSrc.Clone(), ref rWaferMp, _AlignC.RLWaferAlgorithm);
                    GV.matcherLHW.MatMatchWithAlgo(0, lSrc.Clone(), ref lWaferMp, _AlignC.LHWaferAlgorithm);
                    GV.matcherRHW.MatMatchWithAlgo(0, rSrc.Clone(), ref rWaferMp, _AlignC.RHWaferAlgorithm);
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
                    _recipe.LeftLowMaskMask = dialogPaintMask.Mask;
                    GV.matcherLLM.LearnWithAlgo(_recipe.LeftLowMaskMat, _recipe.LeftLowMaskMask, _AlignC.LLMaskAlgorithm);
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
                    _recipe.RightHighWaferMask = dialogPaintMask.Mask;
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
                    _recipe.RightLowMaskMask = dialogPaintMask.Mask;
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
    }
}
