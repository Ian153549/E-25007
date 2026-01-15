using Microsoft.SqlServer.Server;
using MRLibrary;
using OpenCvSharp;
using OpenCvSharp.Aruco;
using Sentech.StApiDotNET;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace NSAA_16Axis
{
    public partial class FormMain : Form
    {
        private double _dLalphaAlign = 1.0;  // 左側透明度 (預設 100% 實時影像)
        private double _dRalphaAlign = 1.0;  // 右側透明度
        private int iLalphaAlign = 100;      // 左側 TrackBar 初始值
        private int iRalphaAlign = 100;



        //object _locker = new object();
        Recipe _recipe;
        AlignCondition AlignC;
        int _cycleTestTarget = 0;
        int _cycleTestTimes = 0;
        //int _Magnification = 0;
        bool _isTestMode = false;
        bool IsMouseUp = false;
        // int TestAlign = 0;
        public int iNowRecipe = 1;
        public int iNowRecipeP = 1;

        public double SearchingStepX = 8000;
        public double SearchingStepY = 8000;
        public int iDelay = 2000;
        public static int SleepTime = 200;

        public int iTabIndex = 0;
        public int iAlignOK = 0;
        public int iStartAlign = 0;
        public int iWaferSearchOK = 0;
        public int iShowMask = 0;
        public int iUpMaskPmps = 0;
        public int[] iAlignCode;
        private readonly object Llock = new object();
        readonly object Rlock = new object();
        readonly DialogInitial DlgInitial = new DialogInitial();

        ProductMatchPositions LastPmps = new ProductMatchPositions();
        //ProductMatchPositions MaskPmps = null;
        readonly ProductMatchPositions TopMaskPmps = new ProductMatchPositions();
        readonly ProductMatchPositions UpMaskPmps = new ProductMatchPositions();

        readonly ProductMatchPositions BLastPmps = new ProductMatchPositions();
        readonly ProductMatchPositions BLivePmps = new ProductMatchPositions();
        //ProductMatchPositions CheckPmps = new ProductMatchPositions();

        public MatchPosition _LMaskMp = new MatchPosition();
        public MatchPosition _RMaskMp = new MatchPosition();

        public MatchPosition lMaskMp = new MatchPosition();
        public MatchPosition lWaferMp = new MatchPosition();
        public MatchPosition rMaskMp = new MatchPosition();
        public MatchPosition rWaferMp = new MatchPosition();

        // GV.Status TempStatus = GV.Status.Standby;

        public int iAlignMatcherRun = 0;
        public int iDAlignMatcherRun = 0;

        public static FormMain HwndFormMain;
        public Mat DialogMat = null;
        public Mat DialogCenter = null;
        public Mat LeftMask = null;
        public Mat RightMask = null;
        public Mat tLeftMask = null;
        public Mat tRightMask = null;
        public Mat BeforeLeftMask = null;
        public Mat BeforeRightMask = null;
        public Mat LeftUpMask = null;
        public Mat RightUpMask = null;
        public int iMaskGetOK = 0;
        public Mat LCaputerMat = null;
        public Mat RCaputerMat = null;
        public Mat LCapMixMat = null;
        public Mat RCapMixMat = null;

        public Mat ULeft = null;
        public Mat URight = null;
        public Mat BLeft = null;
        public Mat BRight = null;

        public delegate void GetActiveHigh();
        public delegate void GetActiveLow();
        public GetActiveHigh ActHighDelegate;
        public GetActiveLow ActLowDelegate;

        //ProductMatchPositions pmpsFistHigh = null;

        public int[] PlcMemroyIn = new int[50];
        public int[] PlcMemoryOut = new int[50];

        //int[] bMotor = new int[3];
        //double bMoveLShiftXum;
        //double bMoveLShiftYum;
        //double bMoveRShiftXum;
        //double bMoveRShiftYum;

        // CCDPad CCDPadForm = null;
        // public Thread thCCDPad = null;

        public Thread thZoomToLow = null;

        public Thread thLeftZoom = null;
        public Thread thRightZoom = null;
        public Thread thChengeLensWaitRun = null;

        public bool StopCheckPLc = false;

        public double _dMoveLShiftXum;
        public double _dMoveLShiftYum;
        public double _dMoveRShiftXum;
        public double _dMoveRShiftYum;

        public double _dDifLXum;
        public double _dDifLYum;
        public double _dDifRXum;
        public double _dDifRYum;

        public double _dMoveLShiftXPixel;
        public double _dMoveLShiftYPixel;
        public double _dMoveRShiftXPixel;
        public double _dMoveRShiftYPixel;
        private Mat _LeftMask;
        private Mat _RightMask;

        public int iLWaferHigh = 0;
        public int iRWaferHigh = 0;

        public int iAdmin = 0;
        public int iUpDownAlign = 0;
        public int iZoomAdj = 0;

        public int backWidth = 4000;
        public int backHeight = 3000;

        public int CameraCount;
        public CStDevice[] camera = new CStDevice[4];
        public IStDeviceInfo[] iInfo = new IStDeviceInfo[4];

        public int iExposure = 0;
        private HashSet<string> _savedImageHashes = new HashSet<string>();
        private string _lastStartedModel = null;
        private readonly object _aiRestartLock = new object();
        public FormMain()
        {
            InitializeComponent();

            //Easy.Initialize();

            HwndFormMain = this;
            GV.HwndFormMain = this;

            ActHighDelegate = new GetActiveHigh(GetActiveHighMethod);
            ActLowDelegate = new GetActiveLow(GetActiveLowMethod);

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            skLeftAlign.CanCenterLine = true;
            skLeftAlign.CanTrackPattern = true;
            skLeftAlign.CanZoom = true;

            skRightAlign.CanCenterLine = true;
            skRightAlign.CanTrackPattern = true;
            skRightAlign.CanZoom = true;

            skLeftParam.CanCenterLine = true;
            skLeftParam.CanTrackPattern = true;

            skRightParam.CanCenterLine = true;
            skRightParam.CanTrackPattern = true;

            GV.LeftUpWindowOnAlignPage = skLeftAlign;
            GV.RightUpWindowOnAlignPage = skRightAlign;
            GV.LeftUpWindowOnParameterSettingPage = skLeftParam;
            GV.RightUpWindowOnParameterSettingPage = skRightParam;

            GV.LeftDownWindowOnAlignPage = skLeftAlign;
            GV.RightDownWindowOnAlignPage = skRightAlign;

            GV.TbStatus = tbStatus;
            GV.skView = 0;

            timerNowTime.Enabled = true;
            timerNowTime.Interval = 1000;
            timerNowTime.Start();

            this.tabPage3.Parent = null;
            this.tabPage4.Parent = null;
            this.tabPage5.Parent = null;

            btZoomInfoRead.Visible = false;
            groupBoxMeasurePatternDistance.Visible = false;
            gbCameraParm.Visible = false;

            trackBar1.Visible = false;
            trackBar2.Visible = false;
            skLeftAlign.Controls.Add(lbLeftCCD);
            lbLeftCCD.Location = new System.Drawing.Point(10, 20);
            skRightAlign.Controls.Add(lbRightCCD);
            lbRightCCD.Location = new System.Drawing.Point(skRightAlign.Width - 140, 20);
        }

        private void TBLShowImage_ValueChanged(object sender, EventArgs e)
        {
            _dLalphaAlign = (double)tBLShowImage.Value / 100;

            // 只有在 MaskImage 勾選時才更新顯示
            if (MaskImage.Checked && _LeftMask != null && !_LeftMask.Empty())
            {
                skLeftAlign.SetShowMask(true, _dLalphaAlign, _LeftMask);
            }

            // 更新標籤顯示
            lbLShowImage.Text = $"左側透明度: {tBLShowImage.Value}%";
        }

        private void TBRShowImage_ValueChanged(object sender, EventArgs e)
        {
            _dRalphaAlign = (double)tBRShowImage.Value / 100;

            if (MaskImage.Checked && _RightMask != null && !_RightMask.Empty())
            {
                skRightAlign.SetShowMask(true, _dRalphaAlign, _RightMask);
            }

            lbRShowImage.Text = $"右側透明度: {tBRShowImage.Value}%";
        }

        // ===== MouseUp/KeyUp 事件（檢查是否需要儲存）=====
        private void TBLShowImage_MouseUp(object sender, MouseEventArgs e)
        {
            CheckAlignMaskParam();
        }

        private void TBLShowImage_KeyUp(object sender, KeyEventArgs e)
        {
            CheckAlignMaskParam();
        }

        private void TBRShowImage_MouseUp(object sender, MouseEventArgs e)
        {
            CheckAlignMaskParam();
        }

        private void TBRShowImage_KeyUp(object sender, KeyEventArgs e)
        {
            CheckAlignMaskParam();
        }

        // ===== +/- 按鈕事件 =====
        private void BtLShowImagePlus_Click(object sender, EventArgs e)
        {
            if (tBLShowImage.Value < 100)
            {
                tBLShowImage.Value += 1;
            }
        }

        private void BtLShowImageMinus_Click(object sender, EventArgs e)
        {
            if (tBLShowImage.Value > 0)
            {
                tBLShowImage.Value -= 1;
            }
        }

        private void BtRShowImagePlus_Click(object sender, EventArgs e)
        {
            if (tBRShowImage.Value < 100)
            {
                tBRShowImage.Value += 1;
            }
        }

        private void BtRShowImageMinus_Click(object sender, EventArgs e)
        {
            if (tBRShowImage.Value > 0)
            {
                tBRShowImage.Value -= 1;
            }
        }

        // ===== Save 按鈕事件 =====
        private void BtLShowImageSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("確定要儲存左側 Mask 顯示透明度?", "儲存左側透明度",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            iLalphaAlign = tBLShowImage.Value;

            // 儲存到 AlignCondition
            if (GV.acar != null && GV.NowRecipeNumber >= 0 && GV.NowRecipeNumber < GV.acar.Length)
            {
                GV.acar[GV.NowRecipeNumber].dLalphaAlign = _dLalphaAlign;
                GV.acar[GV.NowRecipeNumber].LastModifyTime = DateTime.Now;
                GM.WriteAlignConditionsAcarToXml("AlignConditions.xml");
            }

            btLShowImageSave.BackColor = Color.MintCream;
            MessageBox.Show("已儲存左側 Mask 透明度", "儲存成功",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtRShowImageSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("確定要儲存右側 Mask 顯示透明度?", "儲存右側透明度",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            iRalphaAlign = tBRShowImage.Value;

            if (GV.acar != null && GV.NowRecipeNumber >= 0 && GV.NowRecipeNumber < GV.acar.Length)
            {
                GV.acar[GV.NowRecipeNumber].dRalphaAlign = _dRalphaAlign;
                GV.acar[GV.NowRecipeNumber].LastModifyTime = DateTime.Now;
                GM.WriteAlignConditionsAcarToXml("AlignConditions.xml");
            }

            btRShowImageSave.BackColor = Color.MintCream;
            MessageBox.Show("已儲存右側 Mask 透明度", "儲存成功",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ===== 檢查參數是否變更 =====
        private void CheckAlignMaskParam()
        {
            // 檢查左側透明度是否變更
            if (tBLShowImage.Enabled && iLalphaAlign != tBLShowImage.Value)
            {
                btLShowImageSave.BackColor = Color.LightCoral;
            }
            else
            {
                btLShowImageSave.BackColor = Color.MintCream;
            }

            // 檢查右側透明度是否變更
            if (tBRShowImage.Enabled && iRalphaAlign != tBRShowImage.Value)
            {
                btRShowImageSave.BackColor = Color.LightCoral;
            }
            else
            {
                btRShowImageSave.BackColor = Color.MintCream;
            }
        }
        public void GetActiveHighMethod()
        {
            GV.NowMagnification = 2;
        }
        public void GetActiveLowMethod()
        {
            GV.NowMagnification = 1;
        }

        private void BtQuit_Click(object sender, EventArgs e)
        {
            GV.TickCount = 0;
            if (GV.UserLevel == GV.User.Administrator)
            {
                if (MessageBox.Show("Are you want to save AppSettingParm.xml", "Save Parm.xml", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    GM.WriteAppSettingParmXml("AppSettingParm.xml", GV.AppSettingParm);
            }
            GM.Quit();
        }

        private void BtLogIn_Click(object sender, EventArgs e)
        {
            GV.TickCount = 0;

            string UserName;

            if (GV.UserLevel != GV.User.Operator)
            {
                // MessageBox1 msg1 = new MessageBox1(GV.Dlang.btSureLogout, GV.Dlang.btLogout, MessageBoxIcon.Question);
                // DialogResult m1 = msg1.ShowDialog();
                // if (m1 == DialogResult.OK)
                if (MessageBox.Show(GV.Dlang.btSureLogout, GV.Dlang.btLogout, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    btLogIn.Text = GV.Dlang.btLogin;
                    GV.UserLevel = GV.User.Operator;
                    GM.WriteToStatusTextBox(GV.Dlang.btLogout);
                    // CMAlignCheckLevel();
                }
                else
                {
                    return;
                }
            }
            else
            {
                DialogLogIn login = new DialogLogIn(DialogLogIn.UseEnum.Permission)
                {
                    StartPosition = FormStartPosition.CenterParent
                };
                login.ShowDialog();
            }

            if (GV.UserLevel == GV.User.Engineer)
            {
                btLogIn.Text = GV.Dlang.strEngineerLogout;
                tabPage2.Parent = null;
                tabPage3.Parent = null;
                tabPage4.Parent = null;
                tabPage5.Parent = null;

                int iRecipe = GV.Plc.ReadData16(GV.Plc.iNowRecipeNumber);
                bool test = GV.Plc.IsOpen;
                if (iRecipe != iNowRecipe)
                {
                    iNowRecipe = iRecipe + 1;
                    DebugMessage("Engineer Level iNowRecipe = " + iNowRecipe.ToString());
                    ChangeRecipeNumber(iNowRecipe);
                }

                if (AlignC.UpBackAlign == 2)
                {
                    GV.UpBackAlign = 2;
                    tabPage4.Parent = tabControl1;
                    //cmLearnPatternBack1.CheckLevel();
                }
                else
                {
                    GV.UpBackAlign = 1;
                    tabPage2.Parent = tabControl1;
                    //cmLearnPatternUp1.CheckLevel();
                }

                gbAlignCondition.Enabled = true;
                lbMax.Enabled = true;
                lbMin.Enabled = true;
                nudMaxAlignTimes.Enabled = true;
                nudMinAlignTimes.Enabled = true;
                // tabPage5.Parent = tabControl1;
                ucOpenPad.Visible = false;
                btSystemSetting.Visible = false;
                UserName = GV.Dlang.strEngineer;
                iAdmin = 0;
            }
            else if (GV.UserLevel == GV.User.Administrator)
            {
                btLogIn.Text = GV.Dlang.strAdminLogout;
                tabPage2.Parent = null;
                tabPage3.Parent = null;
                tabPage4.Parent = null;
                tabPage5.Parent = null;
                tabPage2.Parent = tabControl1;
                tabPage4.Parent = tabControl1;
                tabPage3.Parent = tabControl1;
                gbAlignCondition.Enabled = true;
                lbMax.Enabled = true;
                lbMin.Enabled = true;
                nudMaxAlignTimes.Enabled = true;
                nudMinAlignTimes.Enabled = true;
                DebugMessage("Administrator Level iNowRecipe = " + iNowRecipe.ToString());
                iAdmin = 1;
                ucOpenPad.Visible = true;
                btZoomInfoRead.Visible = true;
                cmLearnPatternBack1.groupBox4.Visible = true;
                cmLearnPatternUp1.groupBoxT4.Visible = true;
                UserName = GV.Dlang.strAdmin;
            }
            else
            {
                int iRecipe = GV.Plc.ReadData16(GV.Plc.iNowRecipeNumber);

                if (iRecipe != iNowRecipe)
                {
                    iNowRecipe = iRecipe;
                    DebugMessage("user Level iNowRecipe = " + iNowRecipe.ToString());
                    ChangeRecipeNumber(iNowRecipe);
                }

                if (AlignC.UpBackAlign == 2)
                {
                    tabPage2.Parent = null;
                    tabPage3.Parent = null;
                    tabPage4.Parent = null;
                    tabPage5.Parent = null;
                    tabPage4.Parent = tabControl1;
                    //cmLearnPatternBack1.CheckLevel();
                    GV.UpBackAlign = 2;
                }
                else
                {
                    tabPage2.Parent = null;
                    tabPage3.Parent = null;
                    tabPage4.Parent = null;
                    tabPage5.Parent = null;
                    tabPage2.Parent = tabControl1;
                    //cmLearnPatternUp1.CheckLevel();
                    GV.UpBackAlign = 1;
                }
                gbAlignCondition.Enabled = false;
                lbMax.Enabled = false;
                lbMin.Enabled = false;
                nudMaxAlignTimes.Enabled = false;
                nudMinAlignTimes.Enabled = false;
                ucOpenPad.Visible = false;
                btSystemSetting.Visible = false;
                UserName = GV.Dlang.strOperator;
                iAdmin = 0;
            }

            GM.WriteToStatusTextBox(UserName + GV.Dlang.btLogin);

            DisplayAlignCondition();
        }

        public void CMAlignCheckLevel()
        {
            Invoke((MethodInvoker)delegate ()
            {
                iUpDownAlign = GV.Plc.ReadData16(GV.Plc.iUpDownAlign);
                if (GV.UserLevel == GV.User.Operator)
                {
                    gbAlignCondition.Enabled = false;
                    lbMax.Enabled = false;
                    lbMin.Enabled = false;
                    nudMaxAlignTimes.Enabled = false;
                    nudMinAlignTimes.Enabled = false;
                    buttonTestMode.Visible = false;
                    buttonCapture.Visible = true;
                    if (iUpDownAlign == 0)
                    {
                        tabPage2.Parent = tabControl1;
                        tabPage3.Parent = null;
                        tabPage4.Parent = null;
                        tabPage5.Parent = null;
                    }
                    else
                    {
                        tabPage2.Parent = null;
                        tabPage3.Parent = null;
                        tabPage4.Parent = tabControl1;
                        tabPage5.Parent = null;
                    }
                    //else
                    //{
                    //    tabPage2.Parent = tabControl1;
                    //    tabPage3.Parent = null;
                    //    tabPage4.Parent = null;
                    //    tabPage5.Parent = null;
                    //}
                }
                if (GV.UserLevel == GV.User.Engineer)
                {
                    gbAlignCondition.Enabled = true;
                    lbMax.Enabled = true;
                    lbMin.Enabled = true;
                    nudMaxAlignTimes.Enabled = true;
                    nudMinAlignTimes.Enabled = true;
                    buttonTestMode.Visible = false;
                    buttonCapture.Visible = true;
                    if (iUpDownAlign == 1)
                    {
                        tabPage2.Parent = null;
                        tabPage3.Parent = null;
                        tabPage4.Parent = tabControl1;
                        tabPage5.Parent = null;
                    }
                    else
                    {
                        tabPage2.Parent = tabControl1;
                        tabPage3.Parent = null;
                        tabPage4.Parent = null;
                        tabPage5.Parent = null;
                    }
                }
                if (GV.UserLevel == GV.User.Administrator)
                {
                    gbAlignCondition.Enabled = true;
                    lbMax.Enabled = true;
                    lbMin.Enabled = true;
                    nudMaxAlignTimes.Enabled = true;
                    nudMinAlignTimes.Enabled = true;
                    // buttonTestMode.Visible = true;
                    buttonCapture.Visible = true;
                }
                //tbStatus.ScrollToCaret();
                //tbStatus.Refresh();
                DisplayAlignCondition();
            });
        }

        public void CMLearnPatternCheckLevel()
        {
            Invoke((MethodInvoker)delegate ()
            {
                // UpdateUI();
                if (GV.UserLevel == GV.User.Operator)
                {
                    // gbLWafer.Enabled = false; gbLMask.Enabled = false; gbRWafer.Enabled = false; gbRMask.Enabled = false;
                    numericUpDownMeasurePatternDistanceSteps.Enabled = false;
                    groupBoxMeasurePatternDistance.Enabled = false;
                    numericUpDownMeasurePatternDistanceSteps.Enabled = false;
                }
                else
                {
                    // gbLWafer.Enabled = true; gbLMask.Enabled = true; gbRWafer.Enabled = true; gbRMask.Enabled = true;
                    numericUpDownMeasurePatternDistanceSteps.Enabled = true;
                    groupBoxMeasurePatternDistance.Enabled = true;
                    numericUpDownMeasurePatternDistanceSteps.Enabled = true;
                }

                if (GV.MachineStatus == GV.Status.Align)
                {
                    // gbMagnification.Enabled = false;
                }
                if (GV.MachineStatus == GV.Status.Standby)
                {
                    // gbMagnification.Enabled = true;
                }
            });
        }

        public void ParameterCheckLevel()
        {
            Invoke((MethodInvoker)delegate ()
            {
                foreach (Button ctl in gbLeftZoomLens.Controls)
                    if (ctl.TabIndex == GV.LeftZoomLens.Magnification)
                        ctl.BackColor = Color.Honeydew;
                    else
                        ctl.BackColor = Color.Empty;

                foreach (Button ctl in gbRightZoomLens.Controls)
                    if (ctl.TabIndex == GV.RightZoomLens.Magnification)
                        ctl.BackColor = Color.Honeydew;
                    else
                        ctl.BackColor = Color.Empty;

                cbLowMagnification.SelectedIndex = AlignC.AlignLowMagnification;
                cbHighMagnification.SelectedIndex = AlignC.AlignHighMagnification;

                if (GV.MachineStatus == GV.Status.Align)
                {
                    gbLeftZoomLens.Enabled = false;
                    gbRightZoomLens.Enabled = false;
                }
                if (GV.MachineStatus == GV.Status.Standby)
                {
                    gbLeftZoomLens.Enabled = true;
                    gbRightZoomLens.Enabled = true;
                }

                switch (GV.UserLevel)
                {
                    case GV.User.Administrator:
                        btTuneLens.Visible = true;
                        btnManualPlc.Visible = true;
                        btnTuneXyyTable.Visible = true;
                        buttonSetting.Visible = true;
                        buttonTuneLight.Visible = true;
                        buttonInitialLens.Visible = true;
                        break;
                    case GV.User.Engineer:
                        btTuneLens.Visible = false;
                        btnManualPlc.Visible = false;
                        btnTuneXyyTable.Visible = false;
                        buttonSetting.Visible = true;
                        // buttonTuneLight.Visible = true;
                        buttonInitialLens.Visible = true;
                        break;
                    case GV.User.Operator:
                        btTuneLens.Visible = false;
                        btnManualPlc.Visible = false;
                        btnTuneXyyTable.Visible = false;
                        buttonSetting.Visible = false;
                        // buttonTuneLight.Visible = true;
                        buttonInitialLens.Visible = true;
                        int iUpDownAlign = GV.Plc.ReadData16(GV.Plc.iUpDownAlign);
                        if (iUpDownAlign == 0)
                        {
                            tabPage2.Parent = tabControl1;
                            tabPage4.Parent = null;
                        }
                        else if (iUpDownAlign == 1)
                        {
                            tabPage2.Parent = null;
                            tabPage4.Parent = tabControl1;
                        }
                        else
                        {

                        }
                        break;
                }
            });
        }

        private void ChangeRecipeNumber(int iNowReci)
        {
            if ((iNowReci < 1) || (iNowReci > 100))
            {
                iNowReci = 1;
            }

            DebugMessage("Change Recipe Number in func " + iNowReci.ToString());

            // GM.ReadRecpeTemplate(iNowReci);
            GV.NowRecipeNumber = iNowReci;

            GV._recipe = GM.ReadRecipeXml(iNowReci);
            _recipe = GV._recipe;
            AlignC = _recipe.AlignC;

            LearnTemplate();

            Invoke((MethodInvoker)delegate ()
            {
                GV.UpBackAlign = AlignC.UpBackAlign;
                nudMaskScore.Value = (decimal)AlignC.MaskSocre;
                nudWaferScore.Value = (decimal)AlignC.WaferScore;
                nudXOffset.Value = (decimal)AlignC.XOffset;
                nudYLOffest.Value = (decimal)AlignC.YLOffset;
                nudYROffest.Value = (decimal)AlignC.YROffset;
                nudMaxAlignTimes.Value = (decimal)AlignC.MaxAlignTimes;
                nudMinAlignTimes.Value = (decimal)(AlignC.MinAlignTimes);
                nudXPrecision.Value = (decimal)AlignC.XPrecision;
                nudYPrecision.Value = (decimal)AlignC.YPrecision;
                nudThetaPrecision.Value = (decimal)AlignC.ThetaPrecision;
                nudExpansion.Value = (decimal)AlignC.MaxExpansion;
                if (AlignC.UpBackAlign == 2)
                {
                    if (GV.AppSettingParm.DebugMode == true)
                        writeStatus("12: " + AlignC.PatternCenterDistanceUm.ToString() + " R: " + iNowReci.ToString());

                    //gbAlignCondition.Text = "BOT CCD Align Condition  " + AlignC.LastModifyTime.ToString();
                    gbAlignCondition.Text = GV.Dlang.gbAlignCondition + "  " + AlignC.LastModifyTime.ToString();
                    nUDMarkDistance.Value = (decimal)AlignC.PatternCenterDistanceUm * 2;

                    try
                    {
                        LeftMask = Cv2.ImRead("LeftMask.bmp", ImreadModes.Grayscale);
                        RightMask = Cv2.ImRead("RightMask.bmp", ImreadModes.Grayscale);
                    }
                    catch (Exception)
                    {
                        LeftMask = new Mat(new OpenCvSharp.Size(4000, 3000), MatType.CV_8UC1);
                        RightMask = new Mat(new OpenCvSharp.Size(4000, 3000), MatType.CV_8UC1);
                    }
                    GV.LeftMaskMat = LeftMask;
                    GV.RightMaskMat = RightMask;

                    DebugMessage("Botttom Align!");
                    btCapMask.Visible = true;
                    BtGetMask.Visible = false;
                    cbMagnification.Visible = false;
                    lbMagnification.Visible = false;

                    ProductMatchPositions pmps = GetAllMatchPostion(LeftMask, RightMask, "backmask");
                    BLastPmps.LMaskMp.X = pmps.LMaskMp.X;
                    BLastPmps.LMaskMp.Y = pmps.LMaskMp.Y;
                    BLastPmps.LMaskMp.Score = pmps.LMaskMp.Score;
                    BLastPmps.RMaskMp.X = pmps.RMaskMp.X;
                    BLastPmps.RMaskMp.Y = pmps.RMaskMp.Y;
                    BLastPmps.RMaskMp.Score = pmps.RMaskMp.Score;

                    ChangeUpDown(2, 1);
                }
                else
                {
                    if (GV.AppSettingParm.DebugMode == true)
                        writeStatus("11: " + AlignC.PatternCenterDistanceUm.ToString() + " R: " + iNowReci.ToString());

                    // gbAlignCondition.Text = "TOP CCD Align Condition  " + AlignC.LastModifyTime.ToString();
                    gbAlignCondition.Text = GV.Dlang.gbAlignCondition + "  " + AlignC.LastModifyTime.ToString();
                    nUDMarkDistance.Value = (decimal)AlignC.PatternCenterDistanceUm * 2;

                    DebugMessage("Top Align!");
                    cbMagnification.Visible = true;
                    lbMagnification.Visible = true;
                    btCapMask.Visible = false;
                    BtGetMask.Visible = false;
                    ChangeUpDown(1, 1);
                }
                lbNowRecipeNumber.Text = GV.Dlang.strRecipeNumber + iNowReci.ToString();

                GV.Plc.GetRecipe();

                Update();
            });
        }

        private void LearnTemplate()
        {
            try
            {
                GV.matcherLLW.iBlockSize = AlignC.LLWaferBlockSize;
                GV.matcherLHW.iBlockSize = AlignC.LHWaferBlockSize;
                GV.matcherRLW.iBlockSize = AlignC.RLWaferBlockSize;
                GV.matcherRHW.iBlockSize = AlignC.RHWaferBlockSize;
                if (_recipe.LeftLowWaferMat != null && !_recipe.LeftLowWaferMat.Empty())
                    GV.matcherLLW.LearnWithAlgo(_recipe.LeftLowWaferMat, _recipe.LeftLowWaferMask, AlignC.LLWaferAlgorithm);

                if (_recipe.LeftHighWaferMat != null && !_recipe.LeftHighWaferMat.Empty())
                    GV.matcherLHW.LearnWithAlgo(_recipe.LeftHighWaferMat, _recipe.LeftHighWaferMask, AlignC.LHWaferAlgorithm);

                if (_recipe.LeftLowMaskMat != null && !_recipe.LeftLowMaskMat.Empty())
                    GV.matcherLLM.LearnWithAlgo(_recipe.LeftLowMaskMat, _recipe.LeftLowMaskMask, AlignC.LLMaskAlgorithm);

                if (_recipe.LeftHighMaskMat != null && !_recipe.LeftHighMaskMat.Empty())
                    GV.matcherLHM.LearnWithAlgo(_recipe.LeftHighMaskMat, _recipe.LeftHighMaskMask, AlignC.LHMaskAlgorithm);

                if (_recipe.RightLowWaferMat != null && !_recipe.RightLowWaferMat.Empty())
                    GV.matcherRLW.LearnWithAlgo(_recipe.RightLowWaferMat, _recipe.RightLowWaferMask, AlignC.RLWaferAlgorithm);

                if (_recipe.RightHighWaferMat != null && !_recipe.RightHighWaferMat.Empty())
                    GV.matcherRHW.LearnWithAlgo(_recipe.RightHighWaferMat, _recipe.RightHighWaferMask, AlignC.RHWaferAlgorithm);

                if (_recipe.RightLowMaskMat != null && !_recipe.RightLowMaskMat.Empty())
                    GV.matcherRLM.LearnWithAlgo(_recipe.RightLowMaskMat, _recipe.RightLowMaskMask, AlignC.RLMaskAlgorithm);

                if (_recipe.RightHighMaskMat != null && !_recipe.RightHighMaskMat.Empty())
                    GV.matcherRHM.LearnWithAlgo(_recipe.RightHighMaskMat, _recipe.RightHighMaskMask, AlignC.RHMaskAlgorithm);
            }
            catch (Exception ex)
            {
                string message = GV.AppSettingParm.Language != "default"
            ? string.Format(GV.Dlang.strLoadImageError, ex.Message)
            : $"載入模板時發生錯誤: {ex.Message}";
                string title = GV.AppSettingParm.Language != "default"
                    ? GV.Dlang.strError
                    : "錯誤";
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void DisplayAlignCondition()
        {
            nudMaskScore.Value = (decimal)AlignC.MaskSocre;
            nudWaferScore.Value = (decimal)AlignC.WaferScore;
            nudXOffset.Value = (decimal)AlignC.XOffset;
            nudYLOffest.Value = (decimal)AlignC.YLOffset;
            nudYROffest.Value = (decimal)AlignC.YROffset;
            nudMaxAlignTimes.Value = (decimal)AlignC.MaxAlignTimes;
            nudMinAlignTimes.Value = (decimal)AlignC.MinAlignTimes;
            nudXPrecision.Value = (decimal)AlignC.XPrecision;
            nudYPrecision.Value = (decimal)AlignC.YPrecision;
            nudThetaPrecision.Value = (decimal)AlignC.ThetaPrecision;
            nudExpansion.Value = (decimal)AlignC.MaxExpansion;
            if (AlignC.UpBackAlign == 2)
            {
                if (GV.AppSettingParm.DebugMode == true)
                    writeStatus("2: " + AlignC.PatternCenterDistanceUm.ToString());
                // gbAlignCondition.Text = "BOT CCD Align Condition  " + AlignC.LastModifyTime.ToString();
                gbAlignCondition.Text = GV.Dlang.gbAlignCondition + "  " + AlignC.LastModifyTime.ToString();
                nUDMarkDistance.Value = (decimal)AlignC.PatternCenterDistanceUm;
                MaskImage.Visible = true;
            }
            else
            {
                if (GV.AppSettingParm.DebugMode == true)
                    writeStatus("1: " + AlignC.PatternCenterDistanceUm.ToString());
                // gbAlignCondition.Text = "TOP CCD Align Condition  " + AlignC.LastModifyTime.ToString();
                gbAlignCondition.Text = GV.Dlang.gbAlignCondition + "  " + AlignC.LastModifyTime.ToString();
                nUDMarkDistance.Value = (decimal)AlignC.PatternCenterDistanceUm;
                MaskImage.Visible = false;
            }
            lbNowRecipeNumber.Text = GV.Dlang.strRecipeNumber + iNowRecipe.ToString();
            Update();
        }

        private void BtSaveAlignParm_Click(object sender, EventArgs e)
        {
            GV.TickCount = 0;

            if (MessageBox.Show(GV.Dlang.btSureSaveAlignCondition, GV.Dlang.btSaveAlignCon, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            AlignC.MaskSocre = (float)nudMaskScore.Value;
            AlignC.WaferScore = (float)nudWaferScore.Value;
            AlignC.XOffset = (double)nudXOffset.Value;
            AlignC.YLOffset = (double)nudYLOffest.Value;
            AlignC.YROffset = (double)nudYROffest.Value;
            AlignC.MaxAlignTimes = (int)nudMaxAlignTimes.Value;
            AlignC.MinAlignTimes = (int)nudMinAlignTimes.Value;
            AlignC.XPrecision = (double)nudXPrecision.Value;
            AlignC.YPrecision = (double)nudYPrecision.Value;
            AlignC.ThetaPrecision = (double)nudThetaPrecision.Value;
            AlignC.MaxExpansion = (double)nudExpansion.Value;
            AlignC.LastModifyTime = DateTime.Now;

            int iUpDownAlign = GV.Plc.ReadData16(GV.Plc.iUpDownAlign);
            if (iUpDownAlign == 0)
            {
                AlignC.UpBackAlign = 1;
                if (GV.AppSettingParm.DebugMode == true)
                    writeStatus("3: " + nUDMarkDistance.Value.ToString());

                gbAlignCondition.Text = GV.Dlang.gbAlignCondition + "  " + AlignC.LastModifyTime.ToString();

                //AlignC.PatternCenterDistanceUm = (int)nUDMarkDistance.Value / 2;
                //AlignC.DPatternCenterDistanceUm = (int)nUDMarkDistance.Value;
            }
            else if (iUpDownAlign == 1)
            {
                AlignC.UpBackAlign = 2;
                if (GV.AppSettingParm.DebugMode == true)
                    writeStatus("4: " + nUDMarkDistance.Value.ToString());

                gbAlignCondition.Text = GV.Dlang.gbAlignCondition + "  " + AlignC.LastModifyTime.ToString();

                //AlignC.PatternCenterDistanceUm = (int)nUDMarkDistance.Value / 2;
                //AlignC.DPatternCenterDistanceUm = (int)nUDMarkDistance.Value;
            }
            else
            {
                if (GV.AppSettingParm.Emulation)
                {
                    if (cBDown.Checked)
                    {
                        AlignC.UpBackAlign = 2;
                        if (GV.AppSettingParm.DebugMode == true)
                            writeStatus("5: " + nUDMarkDistance.Value.ToString());
                        //AlignC.PatternCenterDistanceUm = (int)nUDMarkDistance.Value / 2;
                        //AlignC.DPatternCenterDistanceUm = (int)nUDMarkDistance.Value;
                    }
                    else
                    {
                        AlignC.UpBackAlign = 1;
                        if (GV.AppSettingParm.DebugMode == true)
                            writeStatus("6: " + nUDMarkDistance.Value.ToString());
                        //AlignC.PatternCenterDistanceUm = (int)nUDMarkDistance.Value / 2;
                        //AlignC.DPatternCenterDistanceUm = (int)nUDMarkDistance.Value;
                    }
                }
                else
                {
                    AlignC.UpBackAlign = 1;
                    if (GV.AppSettingParm.DebugMode == true)
                        writeStatus("7: " + nUDMarkDistance.Value.ToString());
                    //AlignC.PatternCenterDistanceUm = (int)nUDMarkDistance.Value / 2;
                    //AlignC.DPatternCenterDistanceUm = (int)nUDMarkDistance.Value;
                }
            }

            btSaveAlignParm.BackColor = Color.MintCream;
            //GM.WriteAlignConditionsAcarToXml("AlignConditions.xml");
            GM.WriteRecipeXml(iNowRecipe);
            Update();
        }

        //private AlignCondition ReadAlignCondition(AlignCondition alignConditions)
        //{
        //    alignConditions.RecipeNumber = iNowRecipe;
        //    alignConditions.MaskSocre = (float)nudMaskScore.Value;
        //    alignConditions.WaferScore = (float)nudWaferScore.Value;
        //    alignConditions.XOffset = (double)nudXOffset.Value;
        //    alignConditions.YLOffset = (double)nudYLOffest.Value;
        //    alignConditions.YROffset = (double)nudYROffest.Value;
        //    alignConditions.MaxAlignTimes = (int)nudMaxAlignTimes.Value;
        //    alignConditions.MinAlignTimes = (int)nudMinAlignTimes.Value;
        //    alignConditions.XPrecision = (double)nudXPrecision.Value;
        //    alignConditions.YPrecision = (double)nudYPrecision.Value;
        //    alignConditions.ThetaPrecision = (double)nudThetaPrecision.Value;
        //    alignConditions.MaxExpansion = (double)nudExpansion.Value;
        //    alignConditions.RecipeNumber = iNowRecipe;
        //    if (GV.Plc.ReadMemory(GV.Plc.iUpAlign))
        //    {
        //        alignConditions.UpBackAlign = 1;
        //        if (GV.AppSettingParm.DebugMode == true)
        //            writeStatus("3: " + nUDMarkDistance.Value.ToString());
        //        alignConditions.PatternCenterDistanceUm = (int)nUDMarkDistance.Value / 2;
        //        alignConditions.DPatternCenterDistanceUm = (int)nUDMarkDistance.Value;
        //    }
        //    else if (GV.Plc.ReadMemory(GV.Plc.iDownAlign))
        //    {
        //        alignConditions.UpBackAlign = 2;
        //        if (GV.AppSettingParm.DebugMode == true)
        //            writeStatus("4: " + nUDMarkDistance.Value.ToString());
        //        alignConditions.PatternCenterDistanceUm = (int)nUDMarkDistance.Value / 2;
        //        alignConditions.DPatternCenterDistanceUm = (int)nUDMarkDistance.Value;
        //    } else
        //    {
        //        if (GV.AppSettingParm.Emulation)
        //        {
        //            if (cBDown.Checked)
        //            {
        //                alignConditions.UpBackAlign = 2;
        //                if (GV.AppSettingParm.DebugMode == true)
        //                    writeStatus("5: " + nUDMarkDistance.Value.ToString());
        //                alignConditions.PatternCenterDistanceUm = (int)nUDMarkDistance.Value / 2;
        //                alignConditions.DPatternCenterDistanceUm = (int)nUDMarkDistance.Value;
        //            }
        //            else
        //            {
        //                alignConditions.UpBackAlign = 1;
        //                if (GV.AppSettingParm.DebugMode == true)
        //                    writeStatus("6: " + nUDMarkDistance.Value.ToString());
        //                alignConditions.PatternCenterDistanceUm = (int)nUDMarkDistance.Value / 2;
        //                alignConditions.DPatternCenterDistanceUm = (int)nUDMarkDistance.Value;
        //            }
        //        } else
        //        {
        //            alignConditions.UpBackAlign = 1;
        //            writeStatus("7: " + nUDMarkDistance.Value.ToString());
        //            alignConditions.PatternCenterDistanceUm = (int)nUDMarkDistance.Value / 2;
        //            alignConditions.DPatternCenterDistanceUm = (int)nUDMarkDistance.Value;
        //        }
        //    }
        //    return alignConditions;
        //}

        private void ButtonCapture_Click(object sender, EventArgs e)
        {
            try
            {
                Mat leftImage = null;
                Mat rightImage = null;

                if (GV.skView == 0)
                {
                    leftImage = GV.LeftUpCam.Grab();
                    rightImage = GV.RightUpCam.Grab();
                }
                else if (GV.skView == 1)
                {
                    leftImage = GV.LeftBackCam.Grab();
                    rightImage = GV.RightBackCam.Grab();
                }

                if (leftImage == null || rightImage == null)
                {
                    MessageBox.Show("相機擷取影像失敗,請檢查相機連接狀態", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string result = GM.SaveCaptureImage(leftImage, rightImage);
                MessageBox.Show(result + GV.Dlang.strSave, "擷取影像", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                //MessageBox.Show($"擷取影像時發生錯誤: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                GM.WriteToStatusTextBox($"ButtonCapture_Click 錯誤: {ex.Message}");
            }
        }

        private void ButtonTestMode_Click(object sender, EventArgs e)
        {
            if (_isTestMode)
            {
                GV.Plc.Send("WR MR37000 0");
                _isTestMode = false;
                buttonTestMode.Text = GV.Dlang.btTestMode;
                labelCycleTestCycleTestTimes.Visible = false;
                labelCycleTestCycleTestTimes.Text = "( 0 / 0 )";
                return;
            }
            if (!_isTestMode)
            {
                DialogCycleTest dialog = new DialogCycleTest
                {
                    StartPosition = FormStartPosition.CenterScreen
                };
                dialog.ShowDialog();
                _cycleTestTarget = dialog.CycleTestTarget;
                if (_cycleTestTarget <= 0) return;

                _cycleTestTimes = 1;
                labelCycleTestCycleTestTimes.Text = "( " + _cycleTestTarget.ToString() + " / " + _cycleTestTimes.ToString() + " )";
                labelCycleTestCycleTestTimes.Visible = true;
                //GV.Plc.Send("WR MR37000 1");
                _isTestMode = true;
                buttonTestMode.Text = GV.Dlang.btAbort;
                return;
            }
        }

        //private async Task FormMain_Load(object sender, EventArgs e)
        //{
        //    try
        //    {

        //        writeStatus("Program Start " + lbVerson.Text);
        //        var configLoadTask = Task.WhenAll(
        //            Task.Run(() => GV.AlignContionsList = GM.ReadAlignConditionsXmlToList("AlignConditions.xml")),
        //            Task.Run(() => GV.ZoomLensInfo = GM.ReadZoomLensInfoXml("ZoomLensInfo.xml")),
        //            Task.Run(() => GV.BaslerCamParm = GM.ReadBalserCamParmXml("BaslerCamParm.xml")),
        //            Task.Run(() => GV.AppSettingParm = GM.ReadAppSettingParmXml("AppSettingParm.xml")),
        //            Task.Run(() => GV.PlcAlarmState = GM.ReadPLCAlarmCodeXml("AlarmCode.xml"))
        //            );

        //        GV.LeftUpCam = new SentechNetToMat();
        //        GV.RightUpCam = new SentechNetToMat();
        //        GV.LeftBackCam = new SentechNetToMat();
        //        GV.RightBackCam = new SentechNetToMat();
        //        await configLoadTask;
        //        LogActivities.RemoveAgedFiles(GV.AppSettingParm.LogAge);
        //        GV.PLCTimeOut = GV.AppSettingParm.PLCTimeOut * 1000;
        //        await Task.Run(() => {
        //            GetRecipeXml();
        //            SetOffset();
        //        });
        //        SetNewCam();
        //        //SetNewMatcher();
        //        if (GV.AppSettingParm.DefaultUserLevel == 1)
        //        {
        //            GV.UserLevel = GV.User.Administrator;
        //        }
        //        else if (GV.AppSettingParm.DefaultUserLevel == 2)
        //        {
        //            GV.UserLevel = GV.User.Engineer;
        //        }
        //        else
        //        {
        //            GV.UserLevel = GV.User.Operator;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "LoadParm", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        //        GM.WriteToStatusTextBox(ex.Message);
        //        GM.Quit();
        //    }

        //    if (GV.AppSettingParm.Emulation)
        //    {
        //        if (GV.AppSettingParm.ProgramMode == true)
        //        {
        //            cBDown.Visible = true;
        //        }
        //        else
        //        {
        //            cBDown.Visible = false;
        //        }
        //    }
        //    else
        //    {
        //        cBDown.Visible = false;
        //    }

        //    //GM.ReadRecpeTemplate(iNowRecipe);

        //    GV._recipe = GM.ReadRecipeXml(iNowRecipe);

        //    _recipe = GV._recipe;
        //    AlignC = GV._recipe.AlignC;

        //    DisplayAlignCondition();

        //    if (GV.AppSettingParm.Language != "default")
        //    {
        //        ChangeLanguage();
        //    }
        //    else
        //    {
        //        GV.Dlang = new DisplayLanguage();
        //    }

        //    this.Text = GV.Dlang.ProgramName;

        //    //skLeftAlign.MaskMp = new MatchPosition();
        //    //skLeftAlign.WaferMp = new MatchPosition();
        //    //skRightAlign.MaskMp = new MatchPosition();
        //    //skRightAlign.WaferMp = new MatchPosition();
        //    skLeftAlign.MaskMp = lMaskMp;
        //    skLeftAlign.WaferMp = lWaferMp;
        //    skRightAlign.MaskMp = rMaskMp;
        //    skRightAlign.WaferMp = rWaferMp;


        //    BLeft = new Mat();
        //    BRight = new Mat();
        //    ULeft = new Mat();
        //    URight = new Mat();

        //    if (InitialDevice())
        //    {
        //        await Task.Run(() => LaunchDevice());
        //    }
        //    EnsureServiceRestart();
        //    CMAlignCheckLevel();

        //    GV.LeftUpCam.Freeze();
        //    GV.RightUpCam.Freeze();
        //    GV.TabOption = GV.Tab.Align;

        //    _ = Task.Run(() => ZoomToLow());

        //    Update();
        //    //GV.matcher = new OpenCV3MatchUMat("matcher");

        //    if (GV.AppSettingParm.Emulation != true)
        //    {
        //        // 改為背景載入，不阻塞 UI
        //        _ = Task.Run(() => LoadTemplatesAsync());
        //    }

        //    GV.LeftUpCam.SetWindow(GV.LeftUpWindowOnAlignPage);
        //    GV.RightUpCam.SetWindow(GV.RightUpWindowOnAlignPage);
        //    GV.LeftUpCam.Live();
        //    GV.RightUpCam.Live();
        //    //GV.LeftBackCam.SetWindow(GV.LeftDownWindowOnAlignPage);
        //    //GV.RightBackCam.SetWindow(GV.RightDownWindowOnAlignPage);
        //    //GV.LeftBackCam.Live();
        //    //GV.RightBackCam.Live();

        //    if (AlignC.LeftLight[AlignC.AlignLowMagnification])
        //    {
        //        GV.Light.ChangeBrightness("left", AlignC.LeftBrightness[AlignC.AlignLowMagnification]);
        //    }
        //    else
        //    {
        //        GV.Light.ChangeBrightness("left", 0);
        //    }
        //    if (AlignC.RightLight[AlignC.AlignLowMagnification])
        //    {
        //        GV.Light.ChangeBrightness("right", AlignC.RightBrightness[AlignC.AlignLowMagnification]);
        //    }
        //    else
        //    {
        //        GV.Light.ChangeBrightness("right", 0);
        //    }
        //    GV.Light.ChangeBrightness("leftback", 0);
        //    GV.Light.ChangeBrightness("rightback", 0);
        //    if (AlignC.LRingLight[AlignC.AlignLowMagnification])
        //    {
        //        GV.RingLight.ChangeBrightness("left", AlignC.LeftRingBrightness[AlignC.AlignLowMagnification]);
        //    }
        //    else
        //    {
        //        GV.RingLight.ChangeBrightness("left", 0);
        //    }
        //    if (AlignC.RRingLight[AlignC.AlignLowMagnification])
        //    {
        //        GV.RingLight.ChangeBrightness("right", AlignC.RightRingBrightness[AlignC.AlignLowMagnification]);
        //    }
        //    else
        //    {
        //        GV.RingLight.ChangeBrightness("right", 0);
        //    }
        //    comboBoxCameraParameter.SelectedIndex = 0;
        //    radioButtonLeftCamera.Checked = true;

        //    if (iAlignMatcherRun == 0)
        //    {
        //        GV.OnLearnPattern = false;
        //        //GV.OnAlign = true; //20251128
        //        GV.OnAlign = false;
        //        if (GV.drawAlignMatch == null)
        //        {
        //            GV.drawAlignMatch = new Thread(DrawAlignMatchPosition);
        //            GV.drawAlignMatch.Start();
        //        }
        //    }

        //    GV.UpMat = GV.LeftUpCam.Grab();
        //    GV.DownMat = GV.LeftBackCam.Grab();

        //    GV.UpCenter = new System.Drawing.Point(GV.UpMat.Width / 2, GV.UpMat.Height / 2);
        //    GV.DownCenter = new System.Drawing.Point(GV.DownMat.Width / 2, GV.DownMat.Height / 2);

        //    LastPmps = GetAllMatchPostion(GV.LeftUpCam.Grab(), GV.RightUpCam.Grab(), "first");
        //    if (GV.bBacksideCCD == false)
        //    {
        //        // BacksideCCD.Visible = false;
        //        radioButtonRightBackCam.Visible = false;
        //        radioButtonLeftBackCam.Visible = false;
        //    }

        //    LeftMask = GV.LeftBackCam.Grab();
        //    RightMask = GV.RightBackCam.Grab();

        //    Thread.Sleep(500);
        //    SearchingStepX = GV.ZoomLensInfo.LeftDownUmPerPixelX * LeftMask.Width * 8.0;
        //    SearchingStepY = GV.ZoomLensInfo.LeftDownUmPerPixelY * LeftMask.Height * 8.0;

        //    if (File.Exists("LeftMask.bmp"))
        //        LeftMask = Cv2.ImRead("LeftMask.bmp", ImreadModes.Grayscale);
        //    if (File.Exists("RightMask.bmp"))
        //        RightMask = Cv2.ImRead("RightMask.bmp", ImreadModes.Grayscale);

        //    GV.LeftMaskMat = LeftMask;
        //    GV.RightMaskMat = RightMask;

        //    cmLearnPatternBack1.LeftMaskMat = LeftMask;
        //    cmLearnPatternBack1.RightMaskMat = RightMask;

        //    tBLeftCoaLight.Visible = false;
        //    tBRightCoaLight.Visible = false;

        //    Thread.Sleep(1000);
        //    GV.AIClassList.initialize();
        //    GV.scanPlcThread = new Thread(CheckPlcDram);
        //    GV.scanPlcThread.Start();

        //    lbEmulationModeL.Visible = !(GV.AppSettingParm.LeftUpCamEnable || GV.AppSettingParm.LeftBackCamEnable);
        //    lbEmulationModeR.Visible = !(GV.AppSettingParm.RightUpCamEnable || GV.AppSettingParm.RightBackCamEnable);

        //}
        private void FormMain_Load(object sender, EventArgs e)
        {
            try
            {
                DlgInitial.Show();
                DlgInitial.BringToFront();
                Application.DoEvents(); // 確保視窗完全顯示
                Thread.Sleep(200);
                writeStatus("Program Start " + lbVerson.Text);
                
                // ✅ 背景載入配置（不等待）
                LoadConfigurationsAsync();

                // 初始化相機物件（快速，不阻塞）
                InitializeCameras();

                SetNewCam();
                // 設定使用者權限
                SetUserLevel();

                // UI 初始化
                InitializeUI();

                // 設備初始化（可能耗時）
                if (InitialDevice())
                {
                    _ = LaunchDeviceAsync();
                }

                // 其他初始化
                PostInitialize();
            }
            catch (Exception ex)
            {
                HandleLoadError(ex);
            }
        }

        // ✅ 非同步載入配置檔
        private void LoadConfigurationsAsync()
        {
            try
            {
                writeStatus("正在載入配置檔...");

                GV.AlignContionsList = GM.ReadAlignConditionsXmlToList("AlignConditions.xml");
                GV.ZoomLensInfo = GM.ReadZoomLensInfoXml("ZoomLensInfo.xml");
                GV.BaslerCamParm = GM.ReadBalserCamParmXml("BaslerCamParm.xml");
                GV.AppSettingParm = GM.ReadAppSettingParmXml("AppSettingParm.xml");
                GV.PlcAlarmState = GM.ReadPLCAlarmCodeXml("AlarmCode.xml");

                LogActivities.RemoveAgedFiles(GV.AppSettingParm.LogAge);
                GV.PLCTimeOut = GV.AppSettingParm.PLCTimeOut * 1000;

                GetRecipeXml();
                SetOffset();

                writeStatus("配置檔載入完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"載入配置檔失敗: {ex.Message}", "LoadParm", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                GM.WriteToStatusTextBox(ex.Message);
                throw;
            }
        }

        // ✅ 初始化相機物件
        private void InitializeCameras()
        {
            GV.LeftUpCam = new SentechNetToMat();
            GV.RightUpCam = new SentechNetToMat();
            GV.LeftBackCam = new SentechNetToMat();
            GV.RightBackCam = new SentechNetToMat();
        }

        // ✅ 設定使用者權限
        private void SetUserLevel()
        {
            if (GV.AppSettingParm.DefaultUserLevel == 1)
            {
                GV.UserLevel = GV.User.Administrator;
            }
            else if (GV.AppSettingParm.DefaultUserLevel == 2)
            {
                GV.UserLevel = GV.User.Engineer;
            }
            else
            {
                GV.UserLevel = GV.User.Operator;
            }
        }

        // ✅ UI 初始化
        private void InitializeUI()
        {
            if (GV.AppSettingParm.Emulation)
            {
                if (GV.AppSettingParm.ProgramMode == true)
                {
                    cBDown.Visible = true;
                }
                else
                {
                    cBDown.Visible = false;
                }
            }
            else
            {
                cBDown.Visible = false;
            }

            // 載入 Recipe
            GV._recipe = GM.ReadRecipeXml(iNowRecipe);
            _recipe = GV._recipe;
            AlignC = GV._recipe.AlignC;

            DisplayAlignCondition();

            // 設定語言
            if (GV.AppSettingParm.Language != "default")
            {
                ChangeLanguage();
            }
            else
            {
                GV.Dlang = new DisplayLanguage();
            }

            this.Text = GV.Dlang.ProgramName;

            // 設定 MatchPosition
            skLeftAlign.MaskMp = lMaskMp;
            skLeftAlign.WaferMp = lWaferMp;
            skRightAlign.MaskMp = rMaskMp;
            skRightAlign.WaferMp = rWaferMp;

            // 初始化 Mat
            BLeft = new Mat();
            BRight = new Mat();
            ULeft = new Mat();
            URight = new Mat();
        }

        // ✅ 非同步啟動設備
        private async Task LaunchDeviceAsync()
        {
            await Task.Run(() => {
                try
                {
                    GV.Plc.GetLimit();

                    // ✅ 在 UI 執行緒更新 DialogInitial
                    Invoke((MethodInvoker)delegate {
                        if (DlgInitial != null && !DlgInitial.IsDisposed && DlgInitial.IsHandleCreated)
                        {
                            DlgInitial.WriteToInitialTextBox(GV.Dlang.strLaunchSucc);
                        }
                        GM.WriteToStatusTextBox(GV.Dlang.strWelcome);
                    });

                    Thread.Sleep(250);

                    GV.ModeSelected = GV.Mode.Production;

                    if (GV.ModeSelected == GV.Mode.Educational)
                    {
                        Invoke((MethodInvoker)delegate {
                            HideTabAndSelectTabInEducationalMode();
                            HideToolsInEducationalMode();
                            GV.LeftUpCam.SetWindow(GV.LeftUpWindowOnParameterSettingPage);
                            GV.RightUpCam.SetWindow(GV.RightUpWindowOnParameterSettingPage);
                        });
                    }
                    else if (GV.ModeSelected == GV.Mode.Production)
                    {
                        Invoke((MethodInvoker)delegate {
                            GV.LeftUpCam.SetWindow(GV.LeftUpWindowOnAlignPage);
                            GV.RightUpCam.SetWindow(GV.RightUpWindowOnAlignPage);
                        });
                    }
                }
                catch (Exception ex)
                {
                    Invoke((MethodInvoker)delegate {
                        MessageBox.Show(ex.ToString(), "LaunchDevice", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        GM.WriteToStatusTextBox(ex.Message);
                    });
                }
            });
        }

        // ✅ 其他初始化
        private void PostInitialize()
        {
            EnsureServiceRestart();
            CMAlignCheckLevel();

            GV.LeftUpCam.Freeze();
            GV.RightUpCam.Freeze();
            GV.TabOption = GV.Tab.Align;

            _ = Task.Run(() => ZoomToLow());

            Update();

            // ✅ 先載入模板，再啟動搜尋執行緒
            if (GV.AppSettingParm.Emulation != true)
            {
                _ = Task.Run(async () =>
                {
                    // ✅ 等待模板載入完成
                    await LoadTemplatesAsync();
                    Thread.Sleep(500);
                    // ✅ 模板載入完成後再啟動對齊匹配執行緒
                    if (iAlignMatcherRun == 0)
                    {
                        GV.OnLearnPattern = false;
                        GV.OnAlign = false;
                        if (GV.drawAlignMatch == null)
                        {
                            GV.drawAlignMatch = new Thread(DrawAlignMatchPosition);
                            GV.drawAlignMatch.Start();
                        }
                    }
                });
            }
            else
            {
                // ✅ Emulation 模式也啟動執行緒
                if (iAlignMatcherRun == 0)
                {
                    GV.OnLearnPattern = false;
                    GV.OnAlign = false;
                    if (GV.drawAlignMatch == null)
                    {
                        GV.drawAlignMatch = new Thread(DrawAlignMatchPosition);
                        GV.drawAlignMatch.Start();
                    }
                }
            }

            // 設定相機視窗
            GV.LeftUpCam.SetWindow(GV.LeftUpWindowOnAlignPage);
            GV.RightUpCam.SetWindow(GV.RightUpWindowOnAlignPage);
            GV.LeftUpCam.Live();
            GV.RightUpCam.Live();

            // 設定光源
            if (AlignC.LeftLight[AlignC.AlignLowMagnification])
            {
                GV.Light.ChangeBrightness("left", AlignC.LeftBrightness[AlignC.AlignLowMagnification]);
            }
            else
            {
                GV.Light.ChangeBrightness("left", 0);
            }

            if (AlignC.RightLight[AlignC.AlignLowMagnification])
            {
                GV.Light.ChangeBrightness("right", AlignC.RightBrightness[AlignC.AlignLowMagnification]);
            }
            else
            {
                GV.Light.ChangeBrightness("right", 0);
            }

            GV.Light.ChangeBrightness("leftback", 0);
            GV.Light.ChangeBrightness("rightback", 0);

            if (AlignC.LRingLight[AlignC.AlignLowMagnification])
            {
                GV.RingLight.ChangeBrightness("left", AlignC.LeftRingBrightness[AlignC.AlignLowMagnification]);
            }
            else
            {
                GV.RingLight.ChangeBrightness("left", 0);
            }

            if (AlignC.RRingLight[AlignC.AlignLowMagnification])
            {
                GV.RingLight.ChangeBrightness("right", AlignC.RightRingBrightness[AlignC.AlignLowMagnification]);
            }
            else
            {
                GV.RingLight.ChangeBrightness("right", 0);
            }

            comboBoxCameraParameter.SelectedIndex = 0;
            radioButtonLeftCamera.Checked = true;

            GV.UpMat = GV.LeftUpCam.Grab();
            GV.DownMat = GV.LeftBackCam.Grab();

            GV.UpCenter = new System.Drawing.Point(GV.UpMat.Width / 2, GV.UpMat.Height / 2);
            GV.DownCenter = new System.Drawing.Point(GV.DownMat.Width / 2, GV.DownMat.Height / 2);

            LastPmps = GetAllMatchPostion(GV.LeftUpCam.Grab(), GV.RightUpCam.Grab(), "first");

            if (GV.bBacksideCCD == false)
            {
                radioButtonRightBackCam.Visible = false;
                radioButtonLeftBackCam.Visible = false;
            }

            LeftMask = GV.LeftBackCam.Grab();
            RightMask = GV.RightBackCam.Grab();

            Thread.Sleep(500);
            SearchingStepX = GV.ZoomLensInfo.LeftDownUmPerPixelX * LeftMask.Width * 8.0;
            SearchingStepY = GV.ZoomLensInfo.LeftDownUmPerPixelY * LeftMask.Height * 8.0;

            if (File.Exists("LeftMask.bmp"))
                LeftMask = Cv2.ImRead("LeftMask.bmp", ImreadModes.Grayscale);
            if (File.Exists("RightMask.bmp"))
                RightMask = Cv2.ImRead("RightMask.bmp", ImreadModes.Grayscale);

            GV.LeftMaskMat = LeftMask;
            GV.RightMaskMat = RightMask;

            cmLearnPatternBack1.LeftMaskMat = LeftMask;
            cmLearnPatternBack1.RightMaskMat = RightMask;

            tBLeftCoaLight.Visible = false;
            tBRightCoaLight.Visible = false;

            Thread.Sleep(1000);
            GV.AIClassList.initialize();
            GV.scanPlcThread = new Thread(CheckPlcDram);
            GV.scanPlcThread.Start();

            lbEmulationModeL.Visible = !(GV.AppSettingParm.LeftUpCamEnable || GV.AppSettingParm.LeftBackCamEnable);
            lbEmulationModeR.Visible = !(GV.AppSettingParm.RightUpCamEnable || GV.AppSettingParm.RightBackCamEnable);
        }

        // ✅ 錯誤處理
        private void HandleLoadError(Exception ex)
        {
            MessageBox.Show(ex.Message, "LoadParm", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            GM.WriteToStatusTextBox(ex.Message);
            GM.Quit();
        }

        private async Task LoadTemplatesAsync()
        {
            GV.matcherLLW = new OpenCV3MatchUMat("LLW");
            GV.matcherLHW = new OpenCV3MatchUMat("LHW");
            GV.matcherLLM = new OpenCV3MatchUMat("LLM");
            GV.matcherLHM = new OpenCV3MatchUMat("LHM");
            GV.matcherRLW = new OpenCV3MatchUMat("RLW");
            GV.matcherRHW = new OpenCV3MatchUMat("RHW");
            GV.matcherRLM = new OpenCV3MatchUMat("RLM");
            GV.matcherRHM = new OpenCV3MatchUMat("RHM");

            try
            {
                GV.matcherLLW.iBlockSize = AlignC.LLWaferBlockSize;
                GV.matcherLHW.iBlockSize = AlignC.LHWaferBlockSize;
                GV.matcherRLW.iBlockSize = AlignC.RLWaferBlockSize;
                GV.matcherRHW.iBlockSize = AlignC.RHWaferBlockSize;

                // ✅ 並行學習模板
                var learnTasks = new List<Task>();

                if (_recipe.LeftLowWaferMat != null && !_recipe.LeftLowWaferMat.Empty())
                    learnTasks.Add(Task.Run(() => GV.matcherLLW.LearnWithAlgo(_recipe.LeftLowWaferMat, _recipe.LeftLowWaferMask, AlignC.LLWaferAlgorithm)));

                if (_recipe.LeftHighWaferMat != null && !_recipe.LeftHighWaferMat.Empty())
                    learnTasks.Add(Task.Run(() => GV.matcherLHW.LearnWithAlgo(_recipe.LeftHighWaferMat, _recipe.LeftHighWaferMask, AlignC.LHWaferAlgorithm)));

                if (_recipe.LeftLowMaskMat != null && !_recipe.LeftLowMaskMat.Empty())

                    learnTasks.Add(Task.Run(() => GV.matcherLLM.LearnWithAlgo(_recipe.LeftLowMaskMat, _recipe.LeftLowMaskMask, AlignC.LLMaskAlgorithm)));
                
                if (_recipe.LeftHighMaskMat != null && !_recipe.LeftHighMaskMat.Empty())

                    learnTasks.Add(Task.Run(() => GV.matcherLHM.LearnWithAlgo(_recipe.LeftHighMaskMat, _recipe.LeftHighMaskMask, AlignC.LHMaskAlgorithm)));
               
                if (_recipe.RightLowWaferMat != null && !_recipe.RightLowWaferMat.Empty())

                    learnTasks.Add(Task.Run(() => GV.matcherRLW.LearnWithAlgo(_recipe.RightLowWaferMat, _recipe.RightLowWaferMask, AlignC.RLWaferAlgorithm)));
                
                if (_recipe.RightHighWaferMat != null && !_recipe.RightHighWaferMat.Empty())

                    learnTasks.Add(Task.Run(() => GV.matcherRHW.LearnWithAlgo(_recipe.RightHighWaferMat, _recipe.RightHighWaferMask, AlignC.RHWaferAlgorithm)));
               
                if (_recipe.RightLowMaskMat != null && !_recipe.RightLowMaskMat.Empty())

                    learnTasks.Add(Task.Run(() => GV.matcherRLM.LearnWithAlgo(_recipe.RightLowMaskMat, _recipe.RightLowMaskMask, AlignC.RLMaskAlgorithm)));
                
                if (_recipe.RightHighMaskMat != null && !_recipe.RightHighMaskMat.Empty())

                    learnTasks.Add(Task.Run(() => GV.matcherRHM.LearnWithAlgo(_recipe.RightHighMaskMat, _recipe.RightHighMaskMask, AlignC.RHMaskAlgorithm)));
               

                await Task.WhenAll(learnTasks);
            }
            catch (Exception ex)
            {
                Invoke((MethodInvoker)delegate {
                    MessageBox.Show(ex.Message, "Pattern Learn", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                });
            }
        }

        string serviceExePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Python");
        private volatile bool _isAIServiceRestarting = false;

        void EnsureServiceRestart()
        {
            var existingProcess = Process.GetProcessesByName(GV.serviceName);

            foreach (var process in existingProcess)
            {
                try
                {
                    if (!process.HasExited)
                    {
                        process.Kill();
                        process.WaitForExit();
                    }
                    process.Dispose();

                }
                catch
                {
                }
            }
            StartSeviceProcess();
        }
        //void EnsureServiceRestart()
        //{
        //    try
        //    {
        //        writeStatus("正在停止 AI 服務...");

        //        // ✅ 方法 1: 通過 Port 找到進程並終止
        //        bool portKilled = KillProcessByPort(9300);

        //        if (!portKilled)
        //        {
        //            // ✅ 方法 2: 通過進程名稱終止
        //            string[] processNames = { "waitress-serve", "python", "pythonw" };

        //            foreach (string processName in processNames)
        //            {
        //                try
        //                {
        //                    var processes = Process.GetProcessesByName(processName);

        //                    foreach (var process in processes)
        //                    {
        //                        try
        //                        {
        //                            writeStatus($"正在停止 {processName} (PID: {process.Id})...");
        //                            process.Kill();

        //                            if (process.WaitForExit(5000))
        //                            {
        //                                writeStatus($"進程 {process.Id} 已停止");
        //                            }
        //                            else
        //                            {
        //                                writeStatus($"進程 {process.Id} 強制終止逾時");
        //                            }
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            writeStatus($"終止進程失敗: {ex.Message}");
        //                        }
        //                    }
        //                }
        //                catch
        //                {
        //                    // 進程不存在，繼續
        //                }
        //            }
        //        }

        //        // 等待 Port 完全釋放
        //        writeStatus("等待 Port 釋放...");
        //        Thread.Sleep(2000);

        //        // 啟動新服務
        //        writeStatus("正在啟動 AI 服務...");
        //        StartSeviceProcess();
        //    }
        //    catch (Exception ex)
        //    {
        //        writeStatus($"重啟服務時發生錯誤: {ex.Message}");
        //    }
        //}

        /// <summary>
        /// 通過 Port 找到並終止占用該 Port 的進程
        /// </summary>
        private bool KillProcessByPort(int port)
        {
            try
            {
                // 使用 netstat 找到占用 Port 的進程
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "netstat",
                    Arguments = "-ano",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(psi))
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    // 解析 netstat 輸出
                    foreach (string line in output.Split('\n'))
                    {
                        if (line.Contains($":{port}") && line.Contains("LISTENING"))
                        {
                            // 提取 PID
                            string[] parts = line.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            if (parts.Length > 0)
                            {
                                string pidStr = parts[parts.Length - 1];
                                if (int.TryParse(pidStr, out int pid))
                                {
                                    try
                                    {
                                        Process targetProcess = Process.GetProcessById(pid);
                                        writeStatus($"找到占用 Port {port} 的進程: {targetProcess.ProcessName} (PID: {pid})");

                                        targetProcess.Kill();

                                        if (targetProcess.WaitForExit(5000))
                                        {
                                            writeStatus($"成功終止 PID {pid}");
                                            return true;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        writeStatus($"終止 PID {pid} 失敗: {ex.Message}");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                writeStatus($"通過 Port 查找進程失敗: {ex.Message}");
            }

            return false;
        }

        //void StartSeviceProcess()
        //{
        //    int plcRecipe = -1; // 預設值
        //    try
        //    {

        //        plcRecipe = GV.Plc.ReadData16(GV.Plc.iNowRecipeNumber);

        //        // 驗證 Recipe Number 範圍
        //        if (plcRecipe < 1 || plcRecipe > 100)
        //        {
        //            writeStatus($"PLC Recipe Number 超出範圍: {plcRecipe}，使用預設值 1");
        //            plcRecipe = -1;
        //        }
        //        else
        //        {
        //            writeStatus($"從 PLC 讀取 Recipe Number: {plcRecipe}");
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        writeStatus($"讀取 PLC Recipe Number 失敗: {ex.Message}，使用預設值 1");
        //    }
        //    string modelsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Python", "Models");
        //    string targetModelFileName = $"best{plcRecipe}.pt";
        //    string targetModelPath = Path.Combine(modelsPath, targetModelFileName);

        //    string loadRecipeValue;
        //    if (File.Exists(targetModelPath))
        //    {
        //        loadRecipeValue = targetModelFileName;
        //    }
        //    else
        //    {
        //        loadRecipeValue = "best.pt";
        //    }
        //    // 建立 AI 配置物件
        //    var aiConfig = new
        //    {
        //        loadRecipe = loadRecipeValue,
        //        recipeNumber = plcRecipe,

        //    };

        //    // 儲存配置到 AIConfig.json
        //    string configPath = Path.Combine(serviceExePath, "AIConfig.json");
        //    try
        //    {
        //        string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(aiConfig, Newtonsoft.Json.Formatting.Indented);
        //        File.WriteAllText(configPath, jsonContent);
        //        writeStatus($"載入模型: {aiConfig.loadRecipe}");
        //    }
        //    catch (Exception ex)
        //    {
        //        writeStatus($"儲存 AIConfig.json 失敗: {ex.Message}");
        //    }
        //    ProcessStartInfo psi = new ProcessStartInfo
        //    {
        //        FileName = "Start.bat",
        //        WorkingDirectory = serviceExePath,
        //        UseShellExecute = true,
        //        WindowStyle = ProcessWindowStyle.Minimized
        //    };
        //    try
        //    {
        //        Process.Start(psi);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"無法啟動背景服務:{ex.Message}");
        //    }
        //}
        void StartSeviceProcess()
        {
            int plcRecipe = -1;

            try
            {
                plcRecipe = GV.Plc.ReadData16(GV.Plc.iNowRecipeNumber);

                if (plcRecipe < 1 || plcRecipe > 100)
                {
                    writeStatus($"PLC Recipe Number 超出範圍: {plcRecipe}，使用預設值 1");
                    plcRecipe = -1;
                }
                else
                {
                    writeStatus($"從 PLC 讀取 Recipe Number: {plcRecipe}");
                }
            }
            catch (Exception ex)
            {
                writeStatus($"讀取 PLC Recipe Number 失敗: {ex.Message}，使用預設值 1");
            }

            // ✅ 快速檢查：模型檔案是否存在
            string modelsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Python", "Models");
            string targetModelFileName = $"best{plcRecipe}.pt";
            string targetModelPath = Path.Combine(modelsPath, targetModelFileName);

            // ✅ 直接使用三元運算，減少變數賦值
            string loadRecipeValue = File.Exists(targetModelPath) ? targetModelFileName : "best.pt";

            // ✅ 只在需要時建立配置物件
            string configPath = Path.Combine(serviceExePath, "AIConfig.json");

            try
            {
                // ✅ 方案 1: 使用更快的 JSON 序列化（System.Text.Json）
                // 如果專案已引用 System.Text.Json，這會比 Newtonsoft.Json 快 2-3 倍

                //string jsonContent = System.Text.Json.JsonSerializer.Serialize(new
                //{
                //    loadRecipe = loadRecipeValue,
                //    recipeNumber = plcRecipe
                //}, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });


                // ✅ 方案 2: 手動建立 JSON（最快，避免序列化開銷）
                string jsonContent = $@"{{
  ""loadRecipe"": ""{loadRecipeValue}"",
  ""recipeNumber"": {plcRecipe}
}}";

                File.WriteAllText(configPath, jsonContent);
                writeStatus($"載入模型: {loadRecipeValue}");
            }
            catch (Exception ex)
            {
                writeStatus($"儲存 AIConfig.json 失敗: {ex.Message}");
            }

            // ✅ 優化：使用 CreateNoWindow 減少視窗建立開銷
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "Start.bat",
                WorkingDirectory = serviceExePath,
                UseShellExecute = true,  // ✅ 改為 false 可加速
                CreateNoWindow = true,     // ✅ 不建立視窗更快
                WindowStyle = ProcessWindowStyle.Minimized
            };

            try
            {
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"無法啟動背景服務:{ex.Message}");
            }
        }
        public async Task<bool> RestartAIServiceAfterTraining(int recipeNumber)
        {
            try
            {
                writeStatus($"正在重新載入 Recipe {recipeNumber} 的 AI 模型...");
                AIService.IsRestarting = true;
                // 1. 停止現有的 Python 服務
                var existingProcess = Process.GetProcessesByName(GV.serviceName);
                foreach (var process in existingProcess)
                {
                    try
                    {
                        if (!process.HasExited)
                        {
                            process.Kill();
                            process.WaitForExit(5000); // 等待最多 5 秒 
                        }
                        process.Dispose();
                    }
                    catch (Exception ex)
                    {
                        writeStatus($"停止舊服務時發生錯誤: {ex.Message}");
                    }
                }

                // 2. 等待一下確保 port 釋放
                await Task.Delay(500);

                // 3. 更新 AIConfig.json（如果需要的話）
                try
                {
                    // 檢查模型檔案是否存在
                    string modelsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Python", "Models");
                    string targetModelFileName = $"best{recipeNumber}.pt";
                    string targetModelPath = Path.Combine(modelsPath, targetModelFileName);

                    string loadRecipeValue;
                    if (File.Exists(targetModelPath))
                    {
                        loadRecipeValue = targetModelFileName;
                        writeStatus($"找到新訓練的模型: {targetModelFileName}");
                    }
                    else
                    {
                        loadRecipeValue = "best.pt";
                        writeStatus($"找不到 {targetModelFileName}，使用預設模型 best.pt");
                    }

                    // 建立 AI 配置物件
                    var aiConfig = new
                    {
                        loadRecipe = loadRecipeValue,
                        recipeNumber = recipeNumber,
                        modelsPath = "Models",
                        port = 9300,
                        timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    };

                    // 儲存配置到 AIConfig.json
                    string configPath = Path.Combine(serviceExePath, "AIConfig.json");
                    string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(aiConfig, Newtonsoft.Json.Formatting.Indented);
                    File.WriteAllText(configPath, jsonContent);
                    writeStatus($"AI 配置已更新: {configPath}");
                }
                catch (Exception ex)
                {
                    writeStatus($"更新 AIConfig.json 失敗: {ex.Message}");
                }

                // 4. 啟動新的 Python Flask 服務
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "Start.bat",
                    WorkingDirectory = serviceExePath,
                    UseShellExecute = true,
                    WindowStyle = ProcessWindowStyle.Minimized
                };

                try
                {
                    Process.Start(psi);
                    writeStatus("Python AI 服務已重新啟動");
                }
                catch (Exception ex)
                {
                    writeStatus($"啟動服務失敗: {ex.Message}");
                    return false;
                }

                // 5. 等待服務啟動完成
                await Task.Delay(1000);
                Dictionary<string, Int16> ClassList = GV.AIClassList.GetClassList();
                cmLearnPatternUp1.UpdateClassList(ClassList);
                cmLearnPatternBack1.UpdateClassList(ClassList);
                AIService.IsRestarting = false;

                writeStatus($"AI 服務重啟完成 (Recipe {recipeNumber})");
                return true;
            }
            catch (Exception ex)
            {
                writeStatus($"重新啟動 AI 服務時發生錯誤: {ex.Message}");
                return false;
            }
        }


        //private void SetNewMatcher()
        //{
        //    GV.m_MatcherLM = new EMatcher();
        //    GV.m_MatcherRM = new EMatcher();
        //    GV.m_MatcherLW = new EMatcher();
        //    GV.m_MatcherRW = new EMatcher();
        //}

        private void SetOffset()
        {
            // GM.ReadOffsetTxT("C:\\Dacian\\AD1.txt");
            // GM.WriteOffsetAllListToXml("ZOffset.xml");
            GM.ReadOffsetListXmlToList("ZOffset.xml");
        }

        private void ChangeLanguage()
        {
            if (GV.AppSettingParm.Language != "default")
            {
                GV.Dlang = GM.ReadLanguageFile(GV.AppSettingParm.Language + ".xml");
                tabControl1.Controls[0].Text = GV.Dlang.mainAlign;
                tabControl1.Controls[1].Text = GV.Dlang.mainLearnPattern;
                tabPage4.Text = GV.Dlang.MemoryPatternBottom;
                tabPage3.Text = GV.Dlang.mainParametersSetting;
                btLogIn.Text = GV.Dlang.btLogin;
                btQuit.Text = GV.Dlang.btQuit;
                gbAlignCondition.Text = GV.Dlang.gbAlignCondition;
                lbMaskScore.Text = GV.Dlang.lbMaskScore;
                lbWaferScore.Text = GV.Dlang.lbWaferScore;
                lbXOffset.Text = GV.Dlang.lbXOffset;
                lbYOffset.Text = GV.Dlang.lbYOffset;
                lbThetaOffset.Text = GV.Dlang.lbThetaOffset;
                lbMarkDist.Text = GV.Dlang.lbMarkDist;
                lbMaxAlignTime.Text = GV.Dlang.lbMaxAlignTimes;
                lbStdXpre.Text = GV.Dlang.lbStdXPre;
                lbStdYPre.Text = GV.Dlang.lbStdYPre;
                lbStdThetaPre.Text = GV.Dlang.lbStdThetaPre;
                lbStdMaxExpansion.Text = GV.Dlang.lbStdMaxExpansion;
                lbNowRecipeNumber.Text = GV.Dlang.strRecipeNumber + iNowRecipe.ToString();
                MaskImage.Text = GV.Dlang.strMaskImage;
                btSaveAlignParm.Text = GV.Dlang.strSave;
                btSystemSetting.Text = GV.Dlang.btSetting;
                // btLMaskLocationSave.Text = GV.Dlang.strSave;
                // btRWaferLocationSave.Text = GV.Dlang.strSave;
                // btRMaskLocationSave.Text= GV.Dlang.strSave;
                buttonCameraParmSave.Text = GV.Dlang.strSave;
                buttonCapture.Text = GV.Dlang.btCapture;
                buttonTestMode.Text = GV.Dlang.btTestMode;
                lbCompany.Text = GV.Dlang.lbCompany;
                lbMax.Text = GV.Dlang.strMax;
                lbMin.Text = GV.Dlang.strMin;
                groupBoxMeasurePatternDistance.Text = GV.Dlang.gbMeasurePatternDistance;
                lbStep.Text = GV.Dlang.strStep;
                buttonCalPatternDistance.Text = GV.Dlang.btCalPatternDistance;
                gbLeftZoomLens.Text = GV.Dlang.gbLeftZoomLens;
                gbRightZoomLens.Text = GV.Dlang.gbRightZoomLens;
                gbCameraParm.Text = GV.Dlang.gpcamaraParameter;
                radioButtonLeftCamera.Text = GV.Dlang.radioButtonLeftCamera;
                radioButtonRightCamera.Text = GV.Dlang.radioButtonRightCamera;
                radioButtonLeftBackCam.Text = GV.Dlang.radioButtonLeftBackCam;
                radioButtonRightBackCam.Text = GV.Dlang.radioButtonRightBackCam;
                buttonCameraParameterResotre.Text = GV.Dlang.btResotre;
                buttonCameraParmSave.Text = GV.Dlang.strSave;
                gbZoomLensParameters.Text = GV.Dlang.gbZoomLensParam;
                lbLowMagnification.Text = GV.Dlang.lbLowMagnification;
                lbHighMagnification.Text = GV.Dlang.lbHighMagnification;
                btnSaveAlignMagnification.Text = GV.Dlang.btnSaveAlignMagnification;
                gbMachingSetting.Text = GV.Dlang.gbMachingSetting;
                btnManualPlc.Text = GV.Dlang.btnManualPlc;
                btnTuneXyyTable.Text = GV.Dlang.btnTuneXyyTable;
                btTuneLens.Text = GV.Dlang.btTuneLens;
                buttonSetting.Text = GV.Dlang.buttonSetting;
                buttonTuneLight.Text = GV.Dlang.buttonTuneLight;
                buttonInitialLens.Text = GV.Dlang.buttonInitialLens;
                btnQuit.Text = GV.Dlang.btnQuit;
                lbLCCDLight.Text = GV.Dlang.lbLCCDLight;
                lbRCCDLight.Text = GV.Dlang.lbRCCDLight;
                lbLeftCCD.Text = GV.Dlang.strTopCCD;
                lbRightCCD.Text = GV.Dlang.strTopCCD;
                btCapMask.Text = GV.Dlang.strCapMask;
                for (int i = 0; i < GV.Dlang.sNowState.Length; i++)
                {
                    if (GV.Dlang.sNowState[i] != "")
                    {
                        GV.AppSettingParm.sNowState[i] = GV.Dlang.sNowState[i];
                    }
                }
                if (GV.AppSettingParm.Language == "TChinese")
                {
                    GV.PlcAlarmState = GM.ReadPLCAlarmCodeXml("AlarmCodeT.xml");
                }
                else if (GV.AppSettingParm.Language == "CChinese")
                {
                    GV.PlcAlarmState = GM.ReadPLCAlarmCodeXml("AlarmCodeC.xml");
                }
            }
        }

        public void SetNewCam()
        {
            GV.CStapi = new CStApiAutoInit();

            GV.Csystem = new CStSystem();

            int CameraEnableCount = 0;
            if (GV.AppSettingParm.LeftUpCamEnable == true)
                CameraEnableCount++;
            if (GV.AppSettingParm.RightUpCamEnable == true)
                CameraEnableCount++;
            if (GV.AppSettingParm.LeftBackCamEnable == true)
                CameraEnableCount++;
            if (GV.AppSettingParm.RightBackCamEnable == true)
                CameraEnableCount++;

            try
            {
                for (int i = 0; i < CameraEnableCount; i++)
                {
                    camera[i] = GV.Csystem.CreateFirstStDevice();
                    CameraCount++;
                }

            }
            catch (Exception)
            {
                //MessageBox.Show("Failed to create one camera");
                string msg = "";
                //if (GV.AppSettingParm.Language == "default")
                //    msg = GV.Dlang.strCameraCreateError;
                //else
                //    msg = GV.DlangRU.strCameraCreateError;
                MessageBox.Show(msg, "InitCamera", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

            //CStDevice camera1 = GV.Csystem.CreateFirstStDevice();
            //CStDevice camera2 = GV.Csystem.CreateFirstStDevice();

            try
            {
                for (int i = 0; i < CameraCount; i++)
                {
                    iInfo[i] = camera[i].GetIStDeviceInfo();
                }
            }
            catch (Exception)
            {
                //MessageBox.Show("Failed to get one camera info!");
                //string msg = "";
                //if (GV.AppSettingParm.Language == "default")
                //    msg = GV.Dlang.strCameraGetError;
                //else
                //    msg = GV.DlangRU.strCameraGetError;
                //MessageBox.Show(msg, "InitCamera", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            //IStDeviceInfo iInfo1 = camera1.GetIStDeviceInfo();
            //IStDeviceInfo iInfo2 = camera2.GetIStDeviceInfo();

            if (CameraCount == 4)
            {
                ////Left Top 
                //if (GV.AppSettingParm.LeftTopCamSerialNumber == iInfo[0].SerialNumber)
                //    GV.LeftTopCam = new SentechNetToMat(camera[0]);
                //else if (GV.AppSettingParm.LeftTopCamSerialNumber == iInfo[1].SerialNumber)
                //    GV.LeftTopCam = new SentechNetToMat(camera[1]);
                //else if (GV.AppSettingParm.LeftTopCamSerialNumber == iInfo[2].SerialNumber)
                //    GV.LeftTopCam = new SentechNetToMat(camera[2]);
                //else
                //    GV.LeftTopCam = new SentechNetToMat(camera[3]);

                ////Right Top
                //if (GV.AppSettingParm.RightTopCamSerialNumber == iInfo[0].SerialNumber)
                //    GV.RightTopCam = new SentechNetToMat(camera[0]);
                //else if (GV.AppSettingParm.RightTopCamSerialNumber == iInfo[1].SerialNumber)
                //    GV.RightTopCam = new SentechNetToMat(camera[1]);
                //else if (GV.AppSettingParm.RightTopCamSerialNumber == iInfo[2].SerialNumber)
                //    GV.RightTopCam = new SentechNetToMat(camera[2]);
                //else
                //    GV.RightTopCam = new SentechNetToMat(camera[3]);

                ////Right Bottom
                //if (GV.AppSettingParm.RightBottomCamSerialNumber == iInfo[0].SerialNumber)
                //    GV.RightBottomCam = new SentechNetToMat(camera[0]);
                //else if (GV.AppSettingParm.RightBottomCamSerialNumber == iInfo[1].SerialNumber)
                //    GV.RightBottomCam = new SentechNetToMat(camera[1]);
                //else if (GV.AppSettingParm.RightBottomCamSerialNumber == iInfo[2].SerialNumber)
                //    GV.RightBottomCam = new SentechNetToMat(camera[2]);
                //else
                //    GV.RightBottomCam = new SentechNetToMat(camera[3]);

                ////Left Bottom
                //if (GV.AppSettingParm.LeftBottomCamSerialNumber == iInfo[0].SerialNumber)
                //    GV.LeftBottomCam = new SentechNetToMat(camera[0]);
                //else if (GV.AppSettingParm.LeftBottomCamSerialNumber == iInfo[1].SerialNumber)
                //    GV.LeftBottomCam = new SentechNetToMat(camera[1]);
                //else if (GV.AppSettingParm.LeftBottomCamSerialNumber == iInfo[2].SerialNumber)
                //    GV.LeftBottomCam = new SentechNetToMat(camera[2]);
                //else
                //    GV.LeftBottomCam = new SentechNetToMat(camera[3]);
            }
            else if (CameraCount == 2)
            {
            }

            //if (GV.AppSettingParm.LeftTopCamSerialNumber == iInfo1.SerialNumber)
            //{
            //    GV.LeftTopCam = new SentechNetToMat(camera1);
            //}
            //else
            //{
            //    GV.RightTopCam = new SentechNetToMat(camera1);
            //}

            //if (GV.AppSettingParm.RightTopCamSerialNumber == iInfo2.SerialNumber)
            //{
            //    GV.RightTopCam = new SentechNetToMat(camera2);
            //}
            //else
            //{
            //    GV.LeftTopCam = new SentechNetToMat(camera2);
            //}

            int cameraRotation = GV.AppSettingParm.LeftCameraRotation;
            if (cameraRotation == 90)
            {
                GV.LeftBackCam.Rotate = SentechNetToMat.ImageRotate.Degree90;
            }
            else if (cameraRotation == 180)
            {
                GV.LeftBackCam.Rotate = SentechNetToMat.ImageRotate.Degree180;
            }
            else if (cameraRotation == 270)
            {
                GV.LeftBackCam.Rotate = SentechNetToMat.ImageRotate.Degree270;
            }
            switch (GV.AppSettingParm.LeftCameraMirror)
            {
                case 0:
                    {
                        GV.LeftBackCam.Mirror = SentechNetToMat.ImageMirror.None;
                        break;
                    }
                case 1:
                    {
                        GV.LeftBackCam.Mirror = SentechNetToMat.ImageMirror.Horizontal;
                        break;
                    }
                case 2:
                    {
                        GV.LeftBackCam.Mirror = SentechNetToMat.ImageMirror.Vertical;
                        break;
                    }
            }

            cameraRotation = GV.AppSettingParm.RightCameraRotation;
            if (cameraRotation == 90)
            {
                GV.RightBackCam.Rotate = SentechNetToMat.ImageRotate.Degree90;
            }
            else if (cameraRotation == 180)
            {
                GV.RightBackCam.Rotate = SentechNetToMat.ImageRotate.Degree180;
            }
            else if (cameraRotation == 270)
            {
                GV.RightBackCam.Rotate = SentechNetToMat.ImageRotate.Degree270;
            }
            switch (GV.AppSettingParm.RightCameraMirror)
            {
                case 0:
                    {
                        GV.RightBackCam.Mirror = SentechNetToMat.ImageMirror.None;
                        break;
                    }
                case 1:
                    {
                        GV.RightBackCam.Mirror = SentechNetToMat.ImageMirror.Horizontal;
                        break;
                    }
                case 2:
                    {
                        GV.RightBackCam.Mirror = SentechNetToMat.ImageMirror.Vertical;
                        break;
                    }
            }
        }

        public bool InitialDevice()
        {

            DlgInitial.StartPosition = FormStartPosition.CenterScreen;
            DlgInitial.Show();
            DlgInitial.BringToFront();

            try
            {
                if (!Directory.Exists("Templates"))
                    Directory.CreateDirectory("Templates");

                //DlgInitial.WriteToInitialTextBox("Read Patten Templates !!");
                //GM.ReadMatTemplates();
                //DlgInitial.WriteToInitialTextBox(GV.Dlang.strOK);

                //DlgInitial.WriteToInitialTextBox("Read Mask Templates !!");
                //GM.ReadMaskTemplates();
                //DlgInitial.WriteToInitialTextBox(GV.Dlang.strOK);

                DlgInitial.WriteToInitialTextBox(GV.Dlang.strConnecting);

                if (GV.AppSettingParm.LeftUpCamEnable == true)
                {
                    if (GV.AppSettingParm.LeftUpCamSerialNumber == iInfo[0].SerialNumber)
                        GV.LeftUpCam = new SentechNetToMat(camera[0]);
                    else if (GV.AppSettingParm.LeftUpCamSerialNumber == iInfo[1].SerialNumber)
                        GV.LeftUpCam = new SentechNetToMat(camera[1]);
                    else if (GV.AppSettingParm.LeftUpCamSerialNumber == iInfo[2].SerialNumber)
                        GV.LeftUpCam = new SentechNetToMat(camera[2]);
                    else if (GV.AppSettingParm.LeftUpCamSerialNumber == iInfo[3].SerialNumber)
                        GV.LeftUpCam = new SentechNetToMat(camera[3]);
                    else
                        throw new MRException("Error : Can't find LeftUpCam !!");
                    GV.LeftUpCam.SetReverseX(GV.BaslerCamParm.LeftUpReserveX);
                    GV.LeftUpCam.SetReverseY(GV.BaslerCamParm.LeftUpReserveY);
                    GV.LeftUpCam.OpenCamera();
                    lbEmulationModeL.Visible = false;
                }
                else
                {
                    lbEmulationModeL.Visible = true;
                }

                if (GV.AppSettingParm.RightUpCamEnable == true)
                {
                    if (GV.AppSettingParm.RightUpCamSerialNumber == iInfo[0].SerialNumber)
                        GV.RightUpCam = new SentechNetToMat(camera[0]);
                    else if (GV.AppSettingParm.RightUpCamSerialNumber == iInfo[1].SerialNumber)
                        GV.RightUpCam = new SentechNetToMat(camera[1]);
                    else if (GV.AppSettingParm.RightUpCamSerialNumber == iInfo[2].SerialNumber)
                        GV.RightUpCam = new SentechNetToMat(camera[2]);
                    else if (GV.AppSettingParm.RightUpCamSerialNumber == iInfo[3].SerialNumber)
                        GV.RightUpCam = new SentechNetToMat(camera[3]);
                    else
                        throw new MRException("Error : Can't find RightUpCam !!");
                    GV.RightUpCam.SetReverseX(GV.BaslerCamParm.RightUpReserveX);
                    GV.RightUpCam.SetReverseY(GV.BaslerCamParm.RightUpReserveY);
                    GV.RightUpCam.OpenCamera();
                    lbEmulationModeR.Visible = false;
                }
                else
                {
                    lbEmulationModeR.Visible = true;
                }

                if (GV.bBacksideCCD == true)
                {
                    if (GV.AppSettingParm.LeftBackCamEnable == true)
                    {
                        if (GV.AppSettingParm.LeftBackCamSerialNumber == iInfo[0].SerialNumber)
                            GV.LeftBackCam = new SentechNetToMat(camera[0]);
                        else if (GV.AppSettingParm.LeftBackCamSerialNumber == iInfo[1].SerialNumber)
                            GV.LeftBackCam = new SentechNetToMat(camera[1]);
                        else if (GV.AppSettingParm.LeftBackCamSerialNumber == iInfo[2].SerialNumber)
                            GV.LeftBackCam = new SentechNetToMat(camera[2]);
                        else if (GV.AppSettingParm.LeftBackCamSerialNumber == iInfo[3].SerialNumber)
                            GV.LeftBackCam = new SentechNetToMat(camera[3]);
                        else
                            throw new MRException("Error : Can't find LeftBackCam !!");
                        GV.LeftBackCam.SetReverseX(GV.BaslerCamParm.LeftBackReserveX);
                        GV.LeftBackCam.SetReverseY(GV.BaslerCamParm.LeftBackReserveY);
                        GV.LeftBackCam.OpenCamera();
                    }

                    if (GV.AppSettingParm.RightBackCamEnable == true)
                    {
                        if (GV.AppSettingParm.RightBackCamSerialNumber == iInfo[0].SerialNumber)
                            GV.RightBackCam = new SentechNetToMat(camera[0]);
                        else if (GV.AppSettingParm.RightBackCamSerialNumber == iInfo[1].SerialNumber)
                            GV.RightBackCam = new SentechNetToMat(camera[1]);
                        else if (GV.AppSettingParm.RightBackCamSerialNumber == iInfo[2].SerialNumber)
                            GV.RightBackCam = new SentechNetToMat(camera[2]);
                        else if (GV.AppSettingParm.RightBackCamSerialNumber == iInfo[3].SerialNumber)
                            GV.RightBackCam = new SentechNetToMat(camera[3]);
                        else
                            throw new MRException("Error : Can't find RightBackCam !!");
                        GV.RightBackCam.SetReverseX(GV.BaslerCamParm.RightBackReserveX);
                        GV.RightBackCam.SetReverseY(GV.BaslerCamParm.RightBackReserveY);
                        GV.RightBackCam.OpenCamera();
                    }
                }

                DlgInitial.WriteToInitialTextBox(GV.Dlang.strOK);

                DlgInitial.WriteToInitialTextBox(GV.Dlang.strConnectPlc);
                if (GV.AppSettingParm.PlcEnable == true)
                {
                    GV.Plc.Open(GV.AppSettingParm.PlcIp, GV.AppSettingParm.PlcPort);
                    if (GV.AppSettingParm.PlcStatusEnable == true)
                    {
                        UcOpenPlcStatus();
                    }
                    GV.Plc.WriteMemory(GV.Plc.iAlignTestStart, false);
                }
                else
                {
                    GV.Plc.MxCreateEmuData();
                    UcOpenPlcTest();
                    if (GV.AppSettingParm.PlcStatusEnable == true)
                    {
                        UcOpenPlcStatus();
                    }
                }
                // GV.Plc.Send("WR MR37000 0"); //關閉cycle test輔助接點
                DlgInitial.WriteToInitialTextBox(GV.Dlang.strOK);

                DlgInitial.WriteToInitialTextBox(GV.Dlang.strConnectLenLight);
                if (GV.AppSettingParm.Emulation != true)
                {
                    GV.Light.Open(GV.AppSettingParm.LightComPort, 19200, 8, System.IO.Ports.Parity.None, System.IO.Ports.StopBits.One, 4);
                }
                DlgInitial.WriteToInitialTextBox(GV.Dlang.strOK);

                DlgInitial.WriteToInitialTextBox(GV.Dlang.strConnectLenRingLight);
                if (GV.AppSettingParm.Emulation != true)
                {
                    if (GV.AppSettingParm.RingLightPortEnable == true)
                    {
                        GV.RingLight.LightNum = 2;
                        GV.RingLight.Open(GV.AppSettingParm.RingLightPort, 19200, 8, System.IO.Ports.Parity.None, System.IO.Ports.StopBits.One, GV.RingLight.LightNum);
                    }
                }
                DlgInitial.WriteToInitialTextBox(GV.Dlang.strOK);

                DlgInitial.WriteToInitialTextBox(GV.Dlang.strConnectLeftZoom);
                //GV.LeftZoomLens.DrvIndex = 0;
                if (GV.AppSettingParm.LeftZoomLensEnable == true)
                {
                    //GV.LeftZoomLens.Open(GV.AppSettingParm.LeftZoomLensComPort);
                    GV.LeftZoomLens.Open(GV.AppSettingParm.LeftZoomLensComPort, 9600, 8, System.IO.Ports.Parity.None, System.IO.Ports.StopBits.One);
                }
                GV.LeftZoomLens.SetDownThruHome(GV.AppSettingParm.EnableDownThruHome);
                DlgInitial.WriteToInitialTextBox(GV.Dlang.strOK);

                DlgInitial.WriteToInitialTextBox(GV.Dlang.strConnectRightZoom);
                //GV.RightZoomLens.DrvIndex = 1;
                if (GV.AppSettingParm.RightZoomLensEnable == true)
                {
                    // GV.RightZoomLens.Open(GV.AppSettingParm.RightZoomLensComPort);
                    GV.RightZoomLens.Open(GV.AppSettingParm.RightZoomLensComPort, 9600, 8, System.IO.Ports.Parity.None, System.IO.Ports.StopBits.One);
                }
                GV.RightZoomLens.SetDownThruHome(GV.AppSettingParm.EnableDownThruHome);
                DlgInitial.WriteToInitialTextBox(GV.Dlang.strOK);

                DlgInitial.WriteToInitialTextBox(GV.Dlang.strInitialLenLight);
                GV.Light.ReadAllChannelBrightness();
                DlgInitial.WriteToInitialTextBox(GV.Dlang.strOK);

                DlgInitial.WriteToInitialTextBox(GV.Dlang.strInitialCameras);

                if (GV.AppSettingParm.LeftUpCamEnable == true)
                {
                    GV.LeftUpCam.CameraGain(GV.BaslerCamParm.LeftUpGain);
                    GV.LeftUpCam.CameraGamma(GV.BaslerCamParm.LeftUpGamma);
                    GV.LeftUpCam.CameraBlackLevel(GV.BaslerCamParm.LeftUpBlackLevel);
                    if (GV.BaslerCamParm.LeftUpExposureTime > 0)
                        GV.LeftUpCam.CameraExposureTime(GV.BaslerCamParm.LeftUpExposureTime);
                    GV.LeftUpCam.SetReverseX(GV.BaslerCamParm.LeftUpReserveX);
                    GV.LeftUpCam.CameraFrameRate(GV.BaslerCamParm.LeftUpFrameRate);
                }

                if (GV.AppSettingParm.RightUpCamEnable == true)
                {
                    GV.RightUpCam.CameraGain(GV.BaslerCamParm.RightUpGain);
                    GV.RightUpCam.CameraGamma(GV.BaslerCamParm.RightUpGamma);
                    GV.RightUpCam.CameraBlackLevel(GV.BaslerCamParm.RightUpBlackLevel);
                    if (GV.BaslerCamParm.RightUpExposureTime > 0)
                        GV.RightUpCam.CameraExposureTime(GV.BaslerCamParm.RightUpExposureTime);
                    GV.RightUpCam.SetReverseX(GV.BaslerCamParm.RightUpReserveX);
                    GV.RightUpCam.CameraFrameRate(GV.BaslerCamParm.RightUpFrameRate);
                }

                if (GV.AppSettingParm.LeftBackCamEnable == true)
                {
                    GV.LeftBackCam.CameraGain(GV.BaslerCamParm.LeftBackGain);
                    GV.LeftBackCam.CameraGamma(GV.BaslerCamParm.LeftBackGamma);
                    GV.LeftBackCam.CameraBlackLevel(GV.BaslerCamParm.LeftBackBlackLevel);
                    if (GV.BaslerCamParm.LeftBackExposureTime > 0)
                        GV.LeftBackCam.CameraExposureTime(GV.BaslerCamParm.LeftBackExposureTime);
                }

                if (GV.AppSettingParm.RightBackCamEnable == true)
                {
                    GV.RightBackCam.CameraGain(GV.BaslerCamParm.RightBackGain);
                    GV.RightBackCam.CameraGamma(GV.BaslerCamParm.RightBackGamma);
                    GV.RightBackCam.CameraBlackLevel(GV.BaslerCamParm.RightBackBlackLevel);
                    if (GV.BaslerCamParm.RightBackExposureTime > 0)
                        GV.RightBackCam.CameraExposureTime(GV.BaslerCamParm.RightBackExposureTime);
                }

                trackBarCameraParmeters.Value = (int)GV.BaslerCamParm.LeftUpGain;

                DlgInitial.WriteToInitialTextBox(GV.Dlang.strOK);

                if (GV.AppSettingParm.EnableInitialZoomLensWhenStart)
                {
                    DlgInitial.WriteToInitialTextBox(GV.Dlang.strInitialZoomLens);
                    Task t1 = Task.Factory.StartNew(() => GV.LeftZoomLens.MoveHome());
                    Task t2 = Task.Factory.StartNew(() => GV.RightZoomLens.MoveHome());
                    t1.Wait(300000);
                    t2.Wait(300000);
                    GV.LeftZoomLens.Magnification = 1;
                    GV.RightZoomLens.Magnification = 1;
                    DlgInitial.WriteToInitialTextBox(GV.Dlang.strOK);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, GV.Dlang.strInitialDevice, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                GM.WriteToStatusTextBox(ex.Message);
                // if (MessageBox.Show(GV.Dlang.strChangeEmulationMode, GV.Dlang.btnQuit, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                // {
                //     GV.AppSettingParm.Emulation = true;
                //     GM.WriteAppSettingParmXml("AppSettingParm.xml", GV.AppSettingParm);
                // }
                GM.Quit();
                return false;
            }

            // GV.LeftUpWindowOnLearnPage.CanZoom = true;
            // GV.LeftUpWindowOnLearnPage.CanRectMaskAndWafer = true;
            // GV.LeftUpWindowOnLearnPage.CanTrackPattern = true;
            // GV.LeftUpWindowOnLearnPage.CanCenterLine = true;

            // GV.RightUpWindowOnLearnPage.CanZoom = true;
            // GV.RightUpWindowOnLearnPage.CanRectMaskAndWafer = true;
            // GV.RightUpWindowOnLearnPage.CanTrackPattern = true;
            // GV.RightUpWindowOnLearnPage.CanCenterLine = true;

            // GV.LeftUpWindowOnAlignPage.CanTrackPattern = true;
            // GV.RightUpWindowOnAlignPage.CanTrackPattern = true;

            // GV.LeftUpWindowOnParameterSettingPage.CanZoom = true;
            // GV.LeftUpWindowOnParameterSettingPage.CanCenterLine = true;

            // GV.RightUpWindowOnParameterSettingPage.CanZoom = true;
            // GV.RightUpWindowOnParameterSettingPage.CanCenterLine = true;

            return true;
        }

        public void LaunchDevice()
        {
            try
            {
                // GV.LeftUpCam.Live();
                // GV.RightUpCam.Live();
                // Thread drawMatchPosition = new Thread(DrawMatchPosition); // if put this to CMLearnPattern designer , the visual studio will crash beacause of an unknow reason
                // drawMatchPosition.Start();

                // Thread updateInfoThread = new Thread(UpdateInfomationAccordingRecipeNumber);
                GV.Plc.GetLimit();

                DlgInitial.WriteToInitialTextBox(GV.Dlang.strLaunchSucc);
                GM.WriteToStatusTextBox(GV.Dlang.strWelcome);
                Thread.Sleep(250);

                //use these code when education mode 
                /*DialogSelectUserMode dialog = new DialogSelectUserMode();
                dialog.StartPosition = FormStartPosition.CenterScreen;
                dialog.ShowDialog();*/

                GV.ModeSelected = GV.Mode.Production;
                if (GV.ModeSelected == GV.Mode.Educational)
                {
                    HideTabAndSelectTabInEducationalMode();
                    HideToolsInEducationalMode();
                    GV.LeftUpCam.SetWindow(GV.LeftUpWindowOnParameterSettingPage);
                    GV.RightUpCam.SetWindow(GV.RightUpWindowOnParameterSettingPage);
                }
                if (GV.ModeSelected == GV.Mode.Production)
                {
                    GV.LeftUpCam.SetWindow(GV.LeftUpWindowOnAlignPage);
                    GV.RightUpCam.SetWindow(GV.RightUpWindowOnAlignPage);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "LaunchDevice", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                GM.WriteToStatusTextBox(ex.Message);
                GM.Quit();
                return;
            }
        }
        //private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string sTag = tabControl1.SelectedTab.Tag.ToString();

        //    GV.TickCount = 0;
        //    if (iTabIndex == 3 && sTag != "4")
        //    {
        //        cmLearnPatternBack1.OnPageLeave();
        //    }
        //    if (sTag == "1")
        //    // if (tabControl1.SelectedIndex == 1)
        //    {
        //        // TestAlign = 0;
        //        iTabIndex = 0;
        //        GV.OnLearnPattern = false;

        //        GV.TabOption = GV.Tab.Align;
        //        CMAlignCheckLevel();
        //        // UpdateUI();
        //        //if (GV.AppSettingParm.Emulation != true && GV.skView == 1)
        //        //{
        //        //    // Bottom CCD 模式時重新學習 Back Mask 模板
        //        //    if (GV.matcherLLM != null && _recipe.LeftLowMaskMat != null && !_recipe.LeftLowMaskMat.Empty())
        //        //    {
        //        //        GV.matcherLLM.LearnWithAlgo(_recipe.LeftLowMaskMat, _recipe.LeftLowMaskMask, AlignC.LLMaskAlgorithm);
        //        //    }
        //        //    if (GV.matcherRLM != null && _recipe.RightLowMaskMat != null && !_recipe.RightLowMaskMat.Empty())
        //        //    {
        //        //        GV.matcherRLM.LearnWithAlgo(_recipe.RightLowMaskMat, _recipe.RightLowMaskMask, AlignC.RLMaskAlgorithm);
        //        //    }
        //        //}
        //        if (iAlignMatcherRun == 0)
        //        {
        //            GV.OnLearnPattern = false;
        //            //GV.OnAlign = true; 20251128
        //            GV.OnAlign = false;

        //            if (GV.drawAlignMatch == null)
        //            {
        //                GV.drawAlignMatch = new Thread(DrawAlignMatchPosition);
        //                GV.drawAlignMatch.Start();
        //            }
        //            /*
        //            if (GV.skView == 0)
        //            {
        //            }
        //            else if (GV.skView == 1)
        //            {
        //                // Thread drawAlignMatch = new Thread(DrawDownAlginMatchPosition);
        //                // drawAlignMatch.Start();
        //            }
        //            */
        //        }

        //        /*   
        //        if (GV.AlignSideMode == GV.AlignSide.Upside)
        //        {
        //            GV.LeftUpCam.SetWindow(GV.LeftUpWindowOnAlignPage);
        //            GV.RightUpCam.SetWindow(GV.RightUpWindowOnAlignPage);
        //            GV.LeftUpCam.Live();
        //            GV.RightUpCam.Live();
        //        } else
        //        {
        //            GV.LeftBackCam.SetWindow(GV.LeftUpWindowOnAlignPage);
        //            GV.RightBackCam.SetWindow(GV.RightUpWindowOnAlignPage);
        //            GV.LeftBackCam.Live();
        //            GV.RightBackCam.Live();
        //        }
        //        */

        //        if (GV.skView == 0)
        //        {
        //            GV.LeftUpCam.Freeze();
        //            GV.RightUpCam.Freeze();
        //            GV.LeftBackCam.Freeze();
        //            GV.RightBackCam.Freeze();
        //            // MaskImage.Checked = false;
        //            // MaskImage.Visible = false;
        //            GV.LeftUpCam.SetWindow(skLeftAlign);
        //            GV.RightUpCam.SetWindow(skRightAlign);
        //            // 重新設定 MatchPosition 參考
        //            skLeftAlign.MaskMp = lMaskMp;
        //            skLeftAlign.WaferMp = lWaferMp;
        //            skRightAlign.MaskMp = rMaskMp;
        //            skRightAlign.WaferMp = rWaferMp;

        //            skLeftAlign.SetShowMask(false, 0, LeftMask);
        //            skRightAlign.SetShowMask(false, 0, RightMask);
        //            lbLeftCCD.Text = GV.Dlang.strTopCCD;
        //            lbRightCCD.Text = GV.Dlang.strTopCCD;
        //            TurnOnLight();
        //            GV.LeftUpCam.Live();
        //            GV.RightUpCam.Live();
        //            lbEmulationModeL.Visible = !(GV.AppSettingParm.LeftUpCamEnable && GV.LeftUpCam.IsLive);
        //            lbEmulationModeR.Visible = !(GV.AppSettingParm.RightUpCamEnable && GV.RightUpCam.IsLive);

        //        }
        //        else if (GV.skView == 1)
        //        {
        //            GV.LeftUpCam.Freeze();
        //            GV.RightUpCam.Freeze();
        //            GV.LeftBackCam.Freeze();
        //            GV.RightBackCam.Freeze();
        //            // MaskImage.Visible = true;
        //            GV.LeftBackCam.SetWindow(skLeftAlign);
        //            GV.RightBackCam.SetWindow(skRightAlign);

        //            skLeftAlign.MaskMp = lMaskMp;
        //            skLeftAlign.WaferMp = lWaferMp;
        //            skRightAlign.MaskMp = rMaskMp;
        //            skRightAlign.WaferMp = rWaferMp;

        //            //if (GV.AppSettingParm.Emulation != true)
        //            //{
        //            //    if (GV.matcherLLM != null && _recipe.LeftLowMaskMat != null && !_recipe.LeftLowMaskMat.Empty())
        //            //    {
        //            //        GV.matcherLLM.LearnWithAlgo(_recipe.LeftLowMaskMat, _recipe.LeftLowMaskMask, AlignC.LLMaskAlgorithm);
        //            //    }
        //            //    if (GV.matcherRLM != null && _recipe.RightLowMaskMat != null && !_recipe.RightLowMaskMat.Empty())
        //            //    {
        //            //        GV.matcherRLM.LearnWithAlgo(_recipe.RightLowMaskMat, _recipe.RightLowMaskMask, AlignC.RLMaskAlgorithm);
        //            //    }
        //            //}
        //            if (MaskImage.Checked)
        //            {
        //                if ((RightMask == null) || (LeftMask == null))
        //                {
        //                    LeftMask = Cv2.ImRead(GetTemplateFileName("LeftMask"), ImreadModes.Grayscale);
        //                    RightMask = Cv2.ImRead(GetTemplateFileName("RightMask"), ImreadModes.Grayscale);
        //                }
        //                skLeftAlign.SetShowMask(true, AlignC.dLalpha, LeftMask);
        //                skRightAlign.SetShowMask(true, AlignC.dRalpha, RightMask);
        //            }
        //            lbLeftCCD.Text = GV.Dlang.strBottomCCD;
        //            lbRightCCD.Text = GV.Dlang.strBottomCCD;
        //            if (GV.NowWaferMask == 1) // Mask Only
        //            {
        //                GV.Light.ChangeBrightness("leftback", AlignC.LBMaskBright);
        //                GV.Light.ChangeBrightness("rightback", AlignC.RBMaskBright);
        //            }
        //            else if (GV.NowWaferMask == 2 || GV.NowWaferMask == 3) // Mask+Wafer
        //            {
        //                GV.Light.ChangeBrightness("leftback", AlignC.LBWaferBright);
        //                GV.Light.ChangeBrightness("rightback", AlignC.RBWaferBright);
        //            }
        //            else
        //            {
        //                // 預設使用 Mask 亮度
        //                GV.Light.ChangeBrightness("leftback", AlignC.LBMaskBright);
        //                GV.Light.ChangeBrightness("rightback", AlignC.RBMaskBright);
        //            }
        //            Thread.Sleep(250);
        //            GV.LeftBackCam.Live();
        //            GV.RightBackCam.Live();
        //            GV.OnAlign = false;
        //            lbEmulationModeL.Visible = !(GV.AppSettingParm.LeftBackCamEnable && GV.LeftBackCam.IsLive);
        //            lbEmulationModeR.Visible = !(GV.AppSettingParm.RightBackCamEnable && GV.RightBackCam.IsLive);
        //        }
        //        NValueChanged();
        //        GM.WriteToStatusTextBox1(iAdmin, "Change to Align");
        //    }
        //    else if (sTag == "2")
        //    {
        //        iTabIndex = 1;
        //        GV.LeftUpCam.Freeze();
        //        GV.RightUpCam.Freeze();
        //        GV.LeftBackCam.Freeze();
        //        GV.RightBackCam.Freeze();

        //        GV.TabOption = GV.Tab.Learn;
        //        GV.OnAlign = false;
        //        GV.OnLearnPattern = true;
        //        // CMLearnPatternCheckLevel();
        //        // UpdateUI();

        //        //if (drawMatcher == null)
        //        //{
        //        //    GV.OnAlign = false;
        //        //    GV.OnLearnPattern = true;
        //        //    // Thread drawMatchPosition = new Thread(DrawMatchPosition);
        //        //    // drawMatchPosition.Start();
        //        //}

        //        // if (GV.AlignSideMode == GV.AlignSide.Upside)
        //        // {
        //        GV.skView = 0;
        //        GV.LeftUpCam.SetWindow(GV.LeftUpWindowOnLearnPage);
        //        GV.RightUpCam.SetWindow(GV.RightUpWindowOnLearnPage);
        //        GV.LeftUpCam.Live();
        //        GV.RightUpCam.Live();
        //        // } 
        //        /*
        //        else
        //        {
        //            GV.LeftBackCam.SetWindow(GV.LeftUpWindowOnLearnPage);
        //            GV.RightBackCam.SetWindow(GV.RightUpWindowOnLearnPage);
        //            GV.LeftBackCam.Live();
        //            GV.RightBackCam.Live();
        //        }
        //        */
        //        cmLearnPatternUp1.CheckLevel();
        //        cmLearnPatternUp1.ShowMe();
        //        GM.WriteToStatusTextBox1(iAdmin, "Change to Pattern Edit Top");
        //    }
        //    // if (tabControl1.SelectedIndex == 2)
        //    else if (sTag == "3")
        //    {
        //        iTabIndex = 2;
        //        GV.OnLearnPattern = false;
        //        GV.LeftUpCam.Freeze();
        //        GV.RightUpCam.Freeze();
        //        GV.LeftBackCam.Freeze();
        //        GV.RightBackCam.Freeze();

        //        GV.TabOption = GV.Tab.Setting;
        //        ParameterCheckLevel();
        //        Update();

        //        // ✅ 修正：使用 GV.skView 或 AlignC.UpBackAlign 來判斷
        //        if (GV.skView == 0 || AlignC.UpBackAlign == 1)
        //        {
        //            // Top CCD 模式
        //            GV.LeftUpCam.SetWindow(GV.LeftUpWindowOnParameterSettingPage);
        //            GV.RightUpCam.SetWindow(GV.RightUpWindowOnParameterSettingPage);
        //            Thread.Sleep(250);
        //            GV.LeftUpCam.Live();
        //            GV.RightUpCam.Live();

        //            // ✅ 更新 Emulation Mode 標籤
        //            lbEmulationModePL.Visible = !GV.AppSettingParm.LeftUpCamEnable;
        //            lbEmulationModePR.Visible = !GV.AppSettingParm.RightUpCamEnable;
        //        }
        //        else
        //        {
        //            // Bottom CCD 模式
        //            GV.LeftBackCam.SetWindow(GV.LeftUpWindowOnParameterSettingPage);
        //            GV.RightBackCam.SetWindow(GV.RightUpWindowOnParameterSettingPage);
        //            Thread.Sleep(250);
        //            GV.LeftBackCam.Live();
        //            GV.RightBackCam.Live();

        //            // ✅ 更新 Emulation Mode 標籤
        //            lbEmulationModePL.Visible = !GV.AppSettingParm.LeftBackCamEnable;
        //            lbEmulationModePR.Visible = !GV.AppSettingParm.RightBackCamEnable;
        //        }

        //        GM.WriteToStatusTextBox1(iAdmin, "Change to Parameters Setting");
        //    }
        //    else if (sTag == "4")
        //    {
        //        iTabIndex = 3;
        //        GV.TabOption = GV.Tab.BackLearn;
        //        GV.LeftUpCam.Freeze();
        //        GV.RightUpCam.Freeze();
        //        GV.LeftBackCam.Freeze();
        //        GV.RightBackCam.Freeze();
        //        // thChengeLensWaitRun = new Thread(ChangeLens);
        //        // thChengeLensWaitRun.Start();

        //        Thread.Sleep(250);
        //        cmLearnPatternBack1.CheckLevel();
        //        Thread.Sleep(250);
        //        GV.LeftBackCam.Live();
        //        Thread.Sleep(250);
        //        GV.RightBackCam.Live();
        //        cmLearnPatternBack1.ShowMe();
        //        //UpdateUI();
        //        Update();
        //        GM.WriteToStatusTextBox1(iAdmin, "Change to Pattern Edit Bottom");
        //    }
        //    else if (sTag == "5")
        //    {
        //        iTabIndex = 4;
        //        GM.WriteToStatusTextBox1(iAdmin, "BackSize Z Calibration");
        //    }
        //}
        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sTag = tabControl1.SelectedTab.Tag.ToString();
            GV.TickCount = 0;

            // ✅ 先處理離開頁面的邏輯（快速）
            if (iTabIndex == 3 && sTag != "4")
            {
                cmLearnPatternBack1.OnPageLeave();
            }

            // ✅ 根據目標頁面執行對應的初始化（非同步化）
            switch (sTag)
            {
                case "1": // Align Page
                    _ = Task.Run(() => InitializeAlignPageAsync());
                    break;
                case "2": // Learn Pattern Up
                    _ = Task.Run(() => InitializeLearnPatternUpPageAsync());
                    break;
                case "3": // Parameter Setting
                    _ = Task.Run(() => InitializeParameterSettingPageAsync());
                    break;
                case "4": // Learn Pattern Back
                    _ = Task.Run(() => InitializeLearnPatternBackPageAsync());
                    break;
                case "5": // Z Calibration
                    iTabIndex = 4;
                    GM.WriteToStatusTextBox1(iAdmin, "BackSize Z Calibration");
                    break;
            }
        }

        // ✅ Align Page 初始化（非同步）
        private async Task InitializeAlignPageAsync()
        {
            try
            {
                iTabIndex = 0;
                GV.OnLearnPattern = false;
                GV.TabOption = GV.Tab.Align;

                // 在 UI 執行緒更新權限檢查
                await InvokeAsync(() => CMAlignCheckLevel());

                // ✅ 啟動對齊匹配執行緒（如果尚未啟動）
                if (iAlignMatcherRun == 0)
                {
                    GV.OnLearnPattern = false;
                    GV.OnAlign = false;

                    if (GV.drawAlignMatch == null)
                    {
                        GV.drawAlignMatch = new Thread(DrawAlignMatchPosition);
                        GV.drawAlignMatch.Start();
                    }
                }

                // ✅ 根據 GV.skView 設定相機（非阻塞）
                if (GV.skView == 0) // Top CCD
                {
                    await SetupTopCCDAsync();
                }
                else if (GV.skView == 1) // Bottom CCD
                {
                    await SetupBottomCCDAsync();
                }

                // ✅ 更新 Emulation Mode 標籤
                await InvokeAsync(() =>
                {
                    if (GV.skView == 0) // Top CCD - 使用 Align 頁面標籤
                    {
                        lbEmulationModeL.Visible = !(GV.AppSettingParm.LeftUpCamEnable && GV.LeftUpCam.IsLive);
                        lbEmulationModeR.Visible = !(GV.AppSettingParm.RightUpCamEnable && GV.RightUpCam.IsLive);
                    }
                    else if (GV.skView == 1) // Bottom CCD - 使用 Align 頁面標籤
                    {
                        lbEmulationModeL.Visible = !(GV.AppSettingParm.LeftBackCamEnable && GV.LeftBackCam.IsLive);
                        lbEmulationModeR.Visible = !(GV.AppSettingParm.RightBackCamEnable && GV.RightBackCam.IsLive);
                    }

                    // ✅ 確保參數頁面的 Emulation Mode 標籤不會顯示在 Align 頁面
                    lbEmulationModePL.Visible = false;
                    lbEmulationModePR.Visible = false;
                });

                await InvokeAsync(() => NValueChanged());
                GM.WriteToStatusTextBox1(iAdmin, "Change to Align");
            }
            catch (Exception ex)
            {
                GM.WriteToStatusTextBox($"InitializeAlignPageAsync 錯誤: {ex.Message}");
            }
        }

        // ✅ 設定 Top CCD（優化版）
        private async Task SetupTopCCDAsync()
        {
            try
            {
                // 1. 先凍結所有相機（快速操作）
                GV.LeftUpCam?.Freeze();
                GV.RightUpCam?.Freeze();
                GV.LeftBackCam?.Freeze();
                GV.RightBackCam?.Freeze();

                // 2. 在 UI 執行緒更新標籤與 Mask 設定
                await InvokeAsync(() =>
                {
                    iShowMask = MaskImage.Checked ? 1 : 0;
                    lbLeftCCD.Text = GV.Dlang.strTopCCD;
                    lbRightCCD.Text = GV.Dlang.strTopCCD;

                    skLeftAlign.SetShowMask(false, 0, LeftMask);
                    skRightAlign.SetShowMask(false, 0, RightMask);
                });

                // ✅ 減少等待時間（從 250ms 降到 100ms）
                await Task.Delay(100);

                // 3. 設定相機視窗（確保在 UI 執行緒）
                await InvokeAsync(() =>
                {
                    GV.LeftUpCam.SetWindow(skLeftAlign);
                    GV.RightUpCam.SetWindow(skRightAlign);

                    skLeftAlign.MaskMp = lMaskMp;
                    skLeftAlign.WaferMp = lWaferMp;
                    skRightAlign.MaskMp = rMaskMp;
                    skRightAlign.WaferMp = rWaferMp;
                });

                // 4. 開啟光源與相機（併行執行）
                var lightTask = Task.Run(() => TurnOnLight());
                var cameraTask = Task.Run(() =>
                {
                    GV.LeftUpCam?.Live();
                    GV.RightUpCam?.Live();
                });

                await Task.WhenAll(lightTask, cameraTask);
            }
            catch (Exception ex)
            {
                GM.WriteToStatusTextBox($"SetupTopCCDAsync 錯誤: {ex.Message}");
            }
        }

        // ✅ 設定 Bottom CCD（優化版）
        private async Task SetupBottomCCDAsync()
        {
            try
            {
                // 1. 先凍結所有相機
                GV.LeftUpCam?.Freeze();
                GV.RightUpCam?.Freeze();
                GV.LeftBackCam?.Freeze();
                GV.RightBackCam?.Freeze();

                // ✅ 減少等待時間
                await Task.Delay(100);

                // 2. 在 UI 執行緒設定視窗
                await InvokeAsync(() =>
                {
                    GV.LeftBackCam.SetWindow(skLeftAlign);
                    GV.RightBackCam.SetWindow(skRightAlign);

                    skLeftAlign.MaskMp = lMaskMp;
                    skLeftAlign.WaferMp = lWaferMp;
                    skRightAlign.MaskMp = rMaskMp;
                    skRightAlign.WaferMp = rWaferMp;

                    lbLeftCCD.Text = GV.Dlang.strBottomCCD;
                    lbRightCCD.Text = GV.Dlang.strBottomCCD;
                });

                // 3. 設定 Mask 顯示
                if (MaskImage.Checked)
                {
                    // ✅ 非同步載入 Mask 影像
                    await Task.Run(() =>
                    {
                        if (LeftMask == null || RightMask == null)
                        {
                            LeftMask = Cv2.ImRead(GetTemplateFileName("LeftMask"), ImreadModes.Grayscale);
                            RightMask = Cv2.ImRead(GetTemplateFileName("RightMask"), ImreadModes.Grayscale);
                        }
                    });

                    await InvokeAsync(() =>
                    {
                        skLeftAlign.SetShowMask(true, AlignC.dLalpha, LeftMask);
                        skRightAlign.SetShowMask(true, AlignC.dRalpha, RightMask);
                    });
                }

                // 4. 設定光源亮度（根據 GV.NowWaferMask）
                await Task.Run(() =>
                {
                    if (GV.NowWaferMask == 1) // Mask Only
                    {
                        GV.Light.ChangeBrightness("leftback", AlignC.LBMaskBright);
                        GV.Light.ChangeBrightness("rightback", AlignC.RBMaskBright);
                    }
                    else if (GV.NowWaferMask == 2 || GV.NowWaferMask == 3) // Mask+Wafer
                    {
                        GV.Light.ChangeBrightness("leftback", AlignC.LBWaferBright);
                        GV.Light.ChangeBrightness("rightback", AlignC.RBWaferBright);
                    }
                    else
                    {
                        GV.Light.ChangeBrightness("leftback", AlignC.LBMaskBright);
                        GV.Light.ChangeBrightness("rightback", AlignC.RBMaskBright);
                    }
                });

                await Task.Delay(100);

                // 5. 開啟相機
                await Task.Run(() =>
                {
                    GV.LeftBackCam?.Live();
                    GV.RightBackCam?.Live();
                });

                GV.OnAlign = false;
            }
            catch (Exception ex)
            {
                GM.WriteToStatusTextBox($"SetupBottomCCDAsync 錯誤: {ex.Message}");
            }
        }

        // ✅ Learn Pattern Up Page 初始化
        private async Task InitializeLearnPatternUpPageAsync()
        {
            try
            {
                iTabIndex = 1;
                GV.LeftUpCam?.Freeze();
                GV.RightUpCam?.Freeze();
                GV.LeftBackCam?.Freeze();
                GV.RightBackCam?.Freeze();

                GV.TabOption = GV.Tab.Learn;
                GV.OnAlign = false;
                GV.OnLearnPattern = true;

                GV.skView = 0;

                await InvokeAsync(() =>
                {
                    GV.LeftUpCam.SetWindow(GV.LeftUpWindowOnLearnPage);
                    GV.RightUpCam.SetWindow(GV.RightUpWindowOnLearnPage);
                });

                await Task.Run(() =>
                {
                    GV.LeftUpCam?.Live();
                    GV.RightUpCam?.Live();
                });

                await InvokeAsync(() =>
                {
                    cmLearnPatternUp1.CheckLevel();
                    cmLearnPatternUp1.ShowMe();
                });

                GM.WriteToStatusTextBox1(iAdmin, "Change to Pattern Edit Top");
            }
            catch (Exception ex)
            {
                GM.WriteToStatusTextBox($"InitializeLearnPatternUpPageAsync 錯誤: {ex.Message}");
            }
        }

        // ✅ Parameter Setting Page 初始化
        private async Task InitializeParameterSettingPageAsync()
        {
            try
            {
                iTabIndex = 2;
                GV.OnLearnPattern = false;
                GV.LeftUpCam?.Freeze();
                GV.RightUpCam?.Freeze();
                GV.LeftBackCam?.Freeze();
                GV.RightBackCam?.Freeze();

                GV.TabOption = GV.Tab.Setting;

                await InvokeAsync(() =>
                {
                    ParameterCheckLevel();
                    Update();
                });

                await Task.Delay(100);

                // ✅ 根據 GV.skView 或 AlignC.UpBackAlign 判斷使用哪組相機
                if (GV.skView == 0 || AlignC.UpBackAlign == 1)
                {
                    await InvokeAsync(() =>
                    {
                        GV.LeftUpCam.SetWindow(GV.LeftUpWindowOnParameterSettingPage);
                        GV.RightUpCam.SetWindow(GV.RightUpWindowOnParameterSettingPage);
                    });

                    await Task.Delay(100);

                    await Task.Run(() =>
                    {
                        GV.LeftUpCam?.Live();
                        GV.RightUpCam?.Live();
                    });

                    await InvokeAsync(() =>
                    {
                        lbEmulationModePL.Visible = !GV.AppSettingParm.LeftUpCamEnable;
                        lbEmulationModePR.Visible = !GV.AppSettingParm.RightUpCamEnable;
                    });
                }
                else
                {
                    await InvokeAsync(() =>
                    {
                        GV.LeftBackCam.SetWindow(GV.LeftUpWindowOnParameterSettingPage);
                        GV.RightBackCam.SetWindow(GV.RightUpWindowOnParameterSettingPage);
                    });

                    await Task.Delay(100);

                    await Task.Run(() =>
                    {
                        GV.LeftBackCam?.Live();
                        GV.RightBackCam?.Live();
                    });

                    await InvokeAsync(() =>
                    {
                        lbEmulationModePL.Visible = !GV.AppSettingParm.LeftBackCamEnable;
                        lbEmulationModePR.Visible = !GV.AppSettingParm.RightBackCamEnable;
                    });
                }

                GM.WriteToStatusTextBox1(iAdmin, "Change to Parameters Setting");
            }
            catch (Exception ex)
            {
                GM.WriteToStatusTextBox($"InitializeParameterSettingPageAsync 錯誤: {ex.Message}");
            }
        }

        // ✅ Learn Pattern Back Page 初始化
        private async Task InitializeLearnPatternBackPageAsync()
        {
            try
            {
                iTabIndex = 3;
                GV.TabOption = GV.Tab.BackLearn;
                GV.LeftUpCam?.Freeze();
                GV.RightUpCam?.Freeze();
                GV.LeftBackCam?.Freeze();
                GV.RightBackCam?.Freeze();

                await Task.Delay(100);

                await InvokeAsync(() => cmLearnPatternBack1.CheckLevel());

                await Task.Delay(100);

                await Task.Run(() =>
                {
                    GV.LeftBackCam?.Live();
                    Thread.Sleep(100);
                    GV.RightBackCam?.Live();
                });

                await InvokeAsync(() =>
                {
                    cmLearnPatternBack1.ShowMe();
                    Update();
                });

                GM.WriteToStatusTextBox1(iAdmin, "Change to Pattern Edit Bottom");
            }
            catch (Exception ex)
            {
                GM.WriteToStatusTextBox($"InitializeLearnPatternBackPageAsync 錯誤: {ex.Message}");
            }
        }

        // ✅ 輔助方法：在 UI 執行緒執行動作
        private Task InvokeAsync(Action action)
        {
            if (InvokeRequired)
            {
                return Task.Run(() => Invoke(action));
            }
            else
            {
                action();
                return Task.CompletedTask;
            }
        }
        //public void DrawAlignMatchPosition()
        //{
        //    iAlignMatcherRun = 1;
        //    //bool bRet = false;

        //    while (!GV.AppEnding)
        //    {
        //        while ((!GV.OnAlign) && (GV.TabOption == GV.Tab.Align))
        //        {
        //            if (AIService.IsRestarting)
        //            {
        //                Thread.Sleep(1000);
        //                continue;
        //            }
        //            if (GV.AppSettingParm.Emulation == true)
        //            {
        //                Thread.Sleep(1000);
        //                continue;
        //            }
        //            if (GV.skView == 0)
        //            {
        //                if (GV.AppSettingParm.DebugMode)
        //                {
        //                    //writeStatus("GV.NowMagi = " + GV.NowMagnification.ToString());
        //                }
        //                if (GV.NowMagnification == 1)
        //                {
        //                    GV.matcherLLM.MatMatchWithAlgo(0, GV.LeftUpCam.Grab(), ref lMaskMp, AlignC.LLMaskAlgorithm, AlignC.LLMaskAIClassId);
        //                    GV.matcherLLW.MatMatchWithAlgo(0, GV.LeftUpCam.Grab(), ref lWaferMp, AlignC.LLWaferAlgorithm, AlignC.LLWaferAIClassId);
        //                    GV.matcherRLM.MatMatchWithAlgo(0, GV.RightUpCam.Grab(), ref rMaskMp, AlignC.RLMaskAlgorithm, AlignC.RLMaskAIClassId);
        //                    GV.matcherRLW.MatMatchWithAlgo(0, GV.RightUpCam.Grab(), ref rWaferMp, AlignC.RLWaferAlgorithm, AlignC.RLWaferAIClassId);

        //                    skLeftAlign.WaferMp.X = lWaferMp.X;
        //                    skLeftAlign.WaferMp.Y = lWaferMp.Y;
        //                    skLeftAlign.WaferMp.Score = lWaferMp.Score;
        //                    skRightAlign.WaferMp.X = rWaferMp.X;
        //                    skRightAlign.WaferMp.Y = rWaferMp.Y;
        //                    skRightAlign.WaferMp.Score = rWaferMp.Score;
        //                    skLeftAlign.MaskMp.X = lMaskMp.X;
        //                    skLeftAlign.MaskMp.Y = lMaskMp.Y;
        //                    skLeftAlign.MaskMp.Score = lMaskMp.Score;
        //                    skRightAlign.MaskMp.X = rMaskMp.X;
        //                    skRightAlign.MaskMp.Y = rMaskMp.Y;
        //                    skRightAlign.MaskMp.Score = rMaskMp.Score;

        //                }
        //                else if (GV.NowMagnification == 2)
        //                {
        //                    if (iMaskGetOK == 1)
        //                    {
        //                        skLeftAlign.MaskMp.X = LastPmps.LMaskMp.X;
        //                        skLeftAlign.MaskMp.Y = LastPmps.LMaskMp.Y;
        //                        skLeftAlign.MaskMp.Score = LastPmps.LMaskMp.Score;
        //                        skRightAlign.MaskMp.X = LastPmps.RMaskMp.X;
        //                        skRightAlign.MaskMp.Y = LastPmps.RMaskMp.Y;
        //                        skRightAlign.MaskMp.Score = LastPmps.RMaskMp.Score;

        //                    }
        //                    else
        //                    {
        //                        GV.matcherLHM.MatMatchWithAlgo(0, GV.LeftUpCam.Grab(), ref lMaskMp, AlignC.LHMaskAlgorithm, AlignC.LLMaskAIClassId);
        //                        GV.matcherRHM.MatMatchWithAlgo(0, GV.RightUpCam.Grab(), ref rMaskMp, AlignC.RHMaskAlgorithm, AlignC.RLMaskAIClassId);

        //                        skLeftAlign.MaskMp.X = lMaskMp.X;
        //                        skLeftAlign.MaskMp.Y = lMaskMp.Y;
        //                        skLeftAlign.MaskMp.Score = lMaskMp.Score;
        //                        skRightAlign.MaskMp.X = rMaskMp.X;
        //                        skRightAlign.MaskMp.Y = rMaskMp.Y;
        //                        skRightAlign.MaskMp.Score = rMaskMp.Score;

        //                    }
        //                    GV.matcherLHW.MatMatchWithAlgo(0, GV.LeftUpCam.Grab(), ref lWaferMp, AlignC.LHWaferAlgorithm, AlignC.LHWaferAIClassId);
        //                    GV.matcherRHW.MatMatchWithAlgo(0, GV.RightUpCam.Grab(), ref rWaferMp, AlignC.RHWaferAlgorithm, AlignC.RHWaferAIClassId);

        //                    skLeftAlign.WaferMp.X = lWaferMp.X;
        //                    skLeftAlign.WaferMp.Y = lWaferMp.Y;
        //                    skLeftAlign.WaferMp.Score = lWaferMp.Score;
        //                    skRightAlign.WaferMp.X = rWaferMp.X;
        //                    skRightAlign.WaferMp.Y = rWaferMp.Y;
        //                    skRightAlign.WaferMp.Score = rWaferMp.Score;

        //                }
        //                Thread.Sleep(250);
        //            }
        //            else if (GV.skView == 1)
        //            {
        //                if (GV.NowWaferMask == 1)//Mask Only
        //                {
        //                    //GV.matcherLLW.MatMatchWithAlgo(0,GV.LeftBackCam.Grab(), ref lMaskMp,AlignC.LLWaferAlgorithm);
        //                    //GV.matcherRLW.MatMatchWithAlgo(0,GV.RightBackCam.Grab(), ref rMaskMp,AlignC.RLWaferAlgorithm);
        //                    GV.matcherLLM.MatMatchWithAlgo(0, GV.LeftBackCam.Grab(), ref lMaskMp, AlignC.LLMaskAlgorithm, AlignC.LLMaskAIClassId);
        //                    GV.matcherRLM.MatMatchWithAlgo(0, GV.RightBackCam.Grab(), ref rMaskMp, AlignC.RLMaskAlgorithm, AlignC.RLMaskAIClassId);
        //                    skLeftAlign.MaskMp.X = lMaskMp.X;
        //                    skLeftAlign.MaskMp.Y = lMaskMp.Y;
        //                    skLeftAlign.MaskMp.Score = lMaskMp.Score;
        //                    skRightAlign.MaskMp.X = rMaskMp.X;
        //                    skRightAlign.MaskMp.Y = rMaskMp.Y;
        //                    skRightAlign.MaskMp.Score = rMaskMp.Score;

        //                }
        //                else if (GV.NowWaferMask == 2)//Mask+Wafer
        //                {
        //                    //GV.matcherLLW.MatMatchWithAlgo(0,LeftMask, ref lMaskMp,AlignC.LLWaferAlgorithm);
        //                    //GV.matcherLHW.MatMatchWithAlgo(0, GV.LeftBackCam.Grab(), ref lWaferMp,AlignC.LHWaferAlgorithm);
        //                    //GV.matcherRLW.MatMatchWithAlgo(0,RightMask, ref rMaskMp, AlignC.RLWaferAlgorithm);
        //                    //GV.matcherRHW.MatMatchWithAlgo(0,GV.RightBackCam.Grab(), ref rWaferMp, AlignC.RHWaferAlgorithm);
        //                    GV.matcherLLM.MatMatchWithAlgo(0, LeftMask, ref lMaskMp, AlignC.LLMaskAlgorithm, AlignC.LLMaskAIClassId);
        //                    GV.matcherLHW.MatMatchWithAlgo(0, GV.LeftBackCam.Grab(), ref lWaferMp, AlignC.LHWaferAlgorithm, AlignC.LHWaferAIClassId);
        //                    GV.matcherRLM.MatMatchWithAlgo(0, RightMask, ref rMaskMp, AlignC.RLMaskAlgorithm, AlignC.RLMaskAIClassId);
        //                    GV.matcherRHW.MatMatchWithAlgo(0, GV.RightBackCam.Grab(), ref rWaferMp, AlignC.RHWaferAlgorithm, AlignC.RHWaferAIClassId);

        //                    skLeftAlign.WaferMp.X = lWaferMp.X;
        //                    skLeftAlign.WaferMp.Y = lWaferMp.Y;
        //                    skLeftAlign.WaferMp.Score = lWaferMp.Score;
        //                    skRightAlign.WaferMp.X = rWaferMp.X;
        //                    skRightAlign.WaferMp.Y = rWaferMp.Y;
        //                    skRightAlign.WaferMp.Score = rWaferMp.Score;
        //                    skLeftAlign.MaskMp.X = lMaskMp.X;
        //                    skLeftAlign.MaskMp.Y = lMaskMp.Y;
        //                    skLeftAlign.MaskMp.Score = lMaskMp.Score;
        //                    skRightAlign.MaskMp.X = rMaskMp.X;
        //                    skRightAlign.MaskMp.Y = rMaskMp.Y;
        //                    skRightAlign.MaskMp.Score = rMaskMp.Score;

        //                }
        //                else if (GV.NowWaferMask == 3)// use BLivePmps for mask
        //                {
        //                    skLeftAlign.MaskMp.X = BLivePmps.LMaskMp.X;
        //                    skLeftAlign.MaskMp.Y = BLivePmps.LMaskMp.Y;
        //                    skLeftAlign.MaskMp.Score = BLivePmps.LMaskMp.Score;

        //                    GV.matcherLHW.MatMatchWithAlgo(0, GV.LeftBackCam.Grab(), ref lWaferMp, AlignC.LHWaferAlgorithm, AlignC.LHWaferAIClassId);
        //                    //GV.matcherLHW.MatMatch(GV.LeftBackCam.Grab(), ref skLeftAlign.WaferMp);
        //                    skLeftAlign.WaferMp.X = lWaferMp.X;
        //                    skLeftAlign.WaferMp.Y = lWaferMp.Y;
        //                    skLeftAlign.WaferMp.Score = lWaferMp.Score;

        //                    // skRightAlign.MaskMp = AlignMatcher.Match(RightMask, GV.RightLowWaferMat[iNowRecipe]);
        //                    // BLivePmps.RMaskMp.CopyTo(ref skRightAlign.MaskMp);
        //                    skRightAlign.MaskMp.X = BLivePmps.RMaskMp.X;
        //                    skRightAlign.MaskMp.Y = BLivePmps.RMaskMp.Y;
        //                    skRightAlign.MaskMp.Score = BLivePmps.RMaskMp.Score;

        //                    GV.matcherRHW.MatMatchWithAlgo(0, GV.RightBackCam.Grab(), ref rWaferMp, AlignC.RHWaferAlgorithm, AlignC.RHWaferAIClassId);
        //                    //GV.matcherRHW.MatMatch(GV.RightBackCam.Grab(), ref skRightAlign.WaferMp);
        //                    skRightAlign.WaferMp.X = rWaferMp.X;
        //                    skRightAlign.WaferMp.Y = rWaferMp.Y;
        //                    skRightAlign.WaferMp.Score = rWaferMp.Score;
        //                }
        //                Thread.Sleep(250);
        //            }
        //        }
        //        Thread.Sleep(1000);
        //    }
        //}
        public void DrawAlignMatchPosition()
        {
            iAlignMatcherRun = 1;

            while (!GV.AppEnding)
            {
                while ((!GV.OnAlign) && (GV.TabOption == GV.Tab.Align))
                {
                    if (AIService.IsRestarting)
                    {
                        Thread.Sleep(1000);
                        continue;
                    }
                    if (GV.AppSettingParm.Emulation == true)
                    {
                        Thread.Sleep(1000);
                        continue;
                    }

                    if (GV.skView == 0)  // Top CCD
                    {
                        if (GV.NowMagnification == 1)  // 低倍率
                        {
                            // ✅ 左上低倍率光罩搜尋（支援偏移量）
                            GV.matcherLLM.MatMatchWithAlgo(0, GV.LeftUpCam.Grab(), ref lMaskMp, AlignC.LLMaskAlgorithm, AlignC.LLMaskAIClassId);
                            lMaskMp.X += AlignC.LLMaskOffsetX;
                            lMaskMp.Y += AlignC.LLMaskOffsetY;

                            // ✅ 左上低倍率晶圓搜尋（支援偏移量）
                            GV.matcherLLW.MatMatchWithAlgo(0, GV.LeftUpCam.Grab(), ref lWaferMp, AlignC.LLWaferAlgorithm, AlignC.LLWaferAIClassId);
                            lWaferMp.X += AlignC.LLWaferOffsetX;
                            lWaferMp.Y += AlignC.LLWaferOffsetY;

                            // ✅ 右上低倍率光罩搜尋（支援偏移量）
                            GV.matcherRLM.MatMatchWithAlgo(0, GV.RightUpCam.Grab(), ref rMaskMp, AlignC.RLMaskAlgorithm, AlignC.RLMaskAIClassId);
                            rMaskMp.X += AlignC.RLMaskOffsetX;
                            rMaskMp.Y += AlignC.RLMaskOffsetY;

                            // ✅ 右上低倍率晶圓搜尋（支援偏移量）
                            GV.matcherRLW.MatMatchWithAlgo(0, GV.RightUpCam.Grab(), ref rWaferMp, AlignC.RLWaferAlgorithm, AlignC.RLWaferAIClassId);
                            rWaferMp.X += AlignC.RLWaferOffsetX;
                            rWaferMp.Y += AlignC.RLWaferOffsetY;

                            skLeftAlign.WaferMp.X = lWaferMp.X;
                            skLeftAlign.WaferMp.Y = lWaferMp.Y;
                            skLeftAlign.WaferMp.Score = lWaferMp.Score;
                            skRightAlign.WaferMp.X = rWaferMp.X;
                            skRightAlign.WaferMp.Y = rWaferMp.Y;
                            skRightAlign.WaferMp.Score = rWaferMp.Score;
                            skLeftAlign.MaskMp.X = lMaskMp.X;
                            skLeftAlign.MaskMp.Y = lMaskMp.Y;
                            skLeftAlign.MaskMp.Score = lMaskMp.Score;
                            skRightAlign.MaskMp.X = rMaskMp.X;
                            skRightAlign.MaskMp.Y = rMaskMp.Y;
                            skRightAlign.MaskMp.Score = rMaskMp.Score;
                        }
                        else if (GV.NowMagnification == 2)  // 高倍率
                        {
                            if (iMaskGetOK == 1)
                            {
                                skLeftAlign.MaskMp.X = LastPmps.LMaskMp.X;
                                skLeftAlign.MaskMp.Y = LastPmps.LMaskMp.Y;
                                skLeftAlign.MaskMp.Score = LastPmps.LMaskMp.Score;
                                skRightAlign.MaskMp.X = LastPmps.RMaskMp.X;
                                skRightAlign.MaskMp.Y = LastPmps.RMaskMp.Y;
                                skRightAlign.MaskMp.Score = LastPmps.RMaskMp.Score;
                            }
                            else
                            {
                                // ✅ 左上高倍率光罩搜尋（支援偏移量）
                                GV.matcherLHM.MatMatchWithAlgo(0, GV.LeftUpCam.Grab(), ref lMaskMp, AlignC.LHMaskAlgorithm, AlignC.LLMaskAIClassId);
                                lMaskMp.X += AlignC.LHMaskOffsetX;
                                lMaskMp.Y += AlignC.LHMaskOffsetY;

                                // ✅ 右上高倍率光罩搜尋（支援偏移量）
                                GV.matcherRHM.MatMatchWithAlgo(0, GV.RightUpCam.Grab(), ref rMaskMp, AlignC.RHMaskAlgorithm, AlignC.RLMaskAIClassId);
                                rMaskMp.X += AlignC.RHMaskOffsetX;
                                rMaskMp.Y += AlignC.RHMaskOffsetY;

                                skLeftAlign.MaskMp.X = lMaskMp.X;
                                skLeftAlign.MaskMp.Y = lMaskMp.Y;
                                skLeftAlign.MaskMp.Score = lMaskMp.Score;
                                skRightAlign.MaskMp.X = rMaskMp.X;
                                skRightAlign.MaskMp.Y = rMaskMp.Y;
                                skRightAlign.MaskMp.Score = rMaskMp.Score;
                            }

                            // ✅ 左上高倍率晶圓搜尋（支援偏移量）
                            GV.matcherLHW.MatMatchWithAlgo(0, GV.LeftUpCam.Grab(), ref lWaferMp, AlignC.LHWaferAlgorithm, AlignC.LHWaferAIClassId);
                            lWaferMp.X += AlignC.LHWaferOffsetX;
                            lWaferMp.Y += AlignC.LHWaferOffsetY;

                            // ✅ 右上高倍率晶圓搜尋（支援偏移量）
                            GV.matcherRHW.MatMatchWithAlgo(0, GV.RightUpCam.Grab(), ref rWaferMp, AlignC.RHWaferAlgorithm, AlignC.RHWaferAIClassId);
                            rWaferMp.X += AlignC.RHWaferOffsetX;
                            rWaferMp.Y += AlignC.RHWaferOffsetY;

                            skLeftAlign.WaferMp.X = lWaferMp.X;
                            skLeftAlign.WaferMp.Y = lWaferMp.Y;
                            skLeftAlign.WaferMp.Score = lWaferMp.Score;
                            skRightAlign.WaferMp.X = rWaferMp.X;
                            skRightAlign.WaferMp.Y = rWaferMp.Y;
                            skRightAlign.WaferMp.Score = rWaferMp.Score;
                        }
                        Thread.Sleep(250);
                    }
                    else if (GV.skView == 1)  // Bottom CCD
                    {
                        if (GV.NowWaferMask == 1)  // Mask Only
                        {
                            // ✅ 左下光罩搜尋（支援偏移量）
                            GV.matcherLLM.MatMatchWithAlgo(0, GV.LeftBackCam.Grab(), ref lMaskMp, AlignC.LLMaskAlgorithm, AlignC.LLMaskAIClassId);
                            lMaskMp.X += AlignC.LLMaskOffsetX;
                            lMaskMp.Y += AlignC.LLMaskOffsetY;

                            // ✅ 右下光罩搜尋（支援偏移量）
                            GV.matcherRLM.MatMatchWithAlgo(0, GV.RightBackCam.Grab(), ref rMaskMp, AlignC.RLMaskAlgorithm, AlignC.RLMaskAIClassId);
                            rMaskMp.X += AlignC.RLMaskOffsetX;
                            rMaskMp.Y += AlignC.RLMaskOffsetY;

                            skLeftAlign.MaskMp.X = lMaskMp.X;
                            skLeftAlign.MaskMp.Y = lMaskMp.Y;
                            skLeftAlign.MaskMp.Score = lMaskMp.Score;
                            skRightAlign.MaskMp.X = rMaskMp.X;
                            skRightAlign.MaskMp.Y = rMaskMp.Y;
                            skRightAlign.MaskMp.Score = rMaskMp.Score;
                        }
                        else if (GV.NowWaferMask == 2)  // Mask+Wafer
                        {
                            // ✅ 使用 LeftMask/RightMask 搜尋光罩（支援偏移量）
                            GV.matcherLLM.MatMatchWithAlgo(0, LeftMask, ref lMaskMp, AlignC.LLMaskAlgorithm, AlignC.LLMaskAIClassId);
                            lMaskMp.X += AlignC.LLMaskOffsetX;
                            lMaskMp.Y += AlignC.LLMaskOffsetY;

                            GV.matcherRLM.MatMatchWithAlgo(0, RightMask, ref rMaskMp, AlignC.RLMaskAlgorithm, AlignC.RLMaskAIClassId);
                            rMaskMp.X += AlignC.RLMaskOffsetX;
                            rMaskMp.Y += AlignC.RLMaskOffsetY;

                            // ✅ 左下晶圓搜尋（支援偏移量）
                            GV.matcherLHW.MatMatchWithAlgo(0, GV.LeftBackCam.Grab(), ref lWaferMp, AlignC.LHWaferAlgorithm, AlignC.LHWaferAIClassId);
                            lWaferMp.X += AlignC.LHWaferOffsetX;
                            lWaferMp.Y += AlignC.LHWaferOffsetY;

                            // ✅ 右下晶圓搜尋（支援偏移量）
                            GV.matcherRHW.MatMatchWithAlgo(0, GV.RightBackCam.Grab(), ref rWaferMp, AlignC.RHWaferAlgorithm, AlignC.RHWaferAIClassId);
                            rWaferMp.X += AlignC.RHWaferOffsetX;
                            rWaferMp.Y += AlignC.RHWaferOffsetY;

                            skLeftAlign.WaferMp.X = lWaferMp.X;
                            skLeftAlign.WaferMp.Y = lWaferMp.Y;
                            skLeftAlign.WaferMp.Score = lWaferMp.Score;
                            skRightAlign.WaferMp.X = rWaferMp.X;
                            skRightAlign.WaferMp.Y = rWaferMp.Y;
                            skRightAlign.WaferMp.Score = rWaferMp.Score;
                            skLeftAlign.MaskMp.X = lMaskMp.X;
                            skLeftAlign.MaskMp.Y = lMaskMp.Y;
                            skLeftAlign.MaskMp.Score = lMaskMp.Score;
                            skRightAlign.MaskMp.X = rMaskMp.X;
                            skRightAlign.MaskMp.Y = rMaskMp.Y;
                            skRightAlign.MaskMp.Score = rMaskMp.Score;
                        }
                        else if (GV.NowWaferMask == 3)  // use BLivePmps for mask
                        {
                            skLeftAlign.MaskMp.X = BLivePmps.LMaskMp.X;
                            skLeftAlign.MaskMp.Y = BLivePmps.LMaskMp.Y;
                            skLeftAlign.MaskMp.Score = BLivePmps.LMaskMp.Score;

                            // ✅ 左下晶圓搜尋（支援偏移量）
                            GV.matcherLHW.MatMatchWithAlgo(0, GV.LeftBackCam.Grab(), ref lWaferMp, AlignC.LHWaferAlgorithm, AlignC.LHWaferAIClassId);
                            lWaferMp.X += AlignC.LHWaferOffsetX;
                            lWaferMp.Y += AlignC.LHWaferOffsetY;

                            skLeftAlign.WaferMp.X = lWaferMp.X;
                            skLeftAlign.WaferMp.Y = lWaferMp.Y;
                            skLeftAlign.WaferMp.Score = lWaferMp.Score;

                            skRightAlign.MaskMp.X = BLivePmps.RMaskMp.X;
                            skRightAlign.MaskMp.Y = BLivePmps.RMaskMp.Y;
                            skRightAlign.MaskMp.Score = BLivePmps.RMaskMp.Score;

                            // ✅ 右下晶圓搜尋（支援偏移量）
                            GV.matcherRHW.MatMatchWithAlgo(0, GV.RightBackCam.Grab(), ref rWaferMp, AlignC.RHWaferAlgorithm, AlignC.RHWaferAIClassId);
                            rWaferMp.X += AlignC.RHWaferOffsetX;
                            rWaferMp.Y += AlignC.RHWaferOffsetY;

                            skRightAlign.WaferMp.X = rWaferMp.X;
                            skRightAlign.WaferMp.Y = rWaferMp.Y;
                            skRightAlign.WaferMp.Score = rWaferMp.Score;
                        }
                        Thread.Sleep(250);
                    }
                }
                Thread.Sleep(1000);
            }
        }


        public async void ZoomToLow()
        {
            Task t1 = Task.Run(() => ChangeLensMagnification("left", AlignC.LeftBrightness[AlignC.AlignLowMagnification], GV.ZoomLensInfo.LeftMagnificationMotorSteps, AlignC.AlignLowMagnification));
            Task t2 = Task.Run(() => ChangeLensMagnification("right", AlignC.RightBrightness[AlignC.AlignLowMagnification], GV.ZoomLensInfo.RightMagnificationMotorSteps, AlignC.AlignLowMagnification));
            await t1; await t2;
        }

        public async void CheckPlcDram()
        {
            int iNowRecipeNumber = 0;
            int iRunState = 0;
            int iHmiState = 0;
            int iAlignMode = -1;
            int iPreviousRecipeNumber = -1;

            while (!GV.AppEnding)
            {
                try
                {
                    GV.Plc.GetLocation();

                    GV.iHmiNum = GV.Plc.ReadData32(GV.Plc.iHmi);

                    if ((GV.iHmiNum > 0) && (GV.iHmiNum < 190))
                    {
                        if (GV.iHmiNum != iHmiState)
                        {
                            iHmiState = GV.iHmiNum;
                            try
                            {
                                writeStatus(GV.HmiCodeD[iHmiState]);
                            }
                            catch (Exception)
                            {

                            }

                        }
                        if (iHmiState == 13)
                        {
                            GV.iAutoRun = GV.Plc.ReadData32(GV.Plc.iAutoRun);
                            if (GV.iAutoRun != iRunState)
                            {
                                iRunState = GV.iAutoRun;
                                writeStatus(GV.AppSettingParm.sNowState[iRunState]);
                                if (iRunState == 0)
                                {
                                    if (GV.skView == 0)
                                    {
                                        Task t = Task.Factory.StartNew(() =>
                                        {
                                            ChangeLensMagnification(AlignC.AlignLowMagnification);
                                        });
                                    }
                                }
                            }
                        }
                    }

                    int iUpDownAlign = GV.Plc.ReadData16(GV.Plc.iUpDownAlign);
                    //int iUpDownAlign = 0;
                    if (iUpDownAlign == 1)
                    {
                        if (GV.UpBackAlign != 2)
                        {
                            GV.UpBackAlign = 2;
                            AlignC.UpBackAlign = 2;
                            ChangeUpDown(2, 1);
                            Invoke((MethodInvoker)delegate ()
                            {
                                if ((GV.UserLevel == GV.User.Operator) || (GV.UserLevel == GV.User.Engineer))
                                {
                                    tabControl1.SelectedTab = tabPage1;
                                    tabPage2.Parent = null;
                                    tabPage3.Parent = null;
                                    tabPage4.Parent = null;
                                    tabPage5.Parent = null;
                                    tabPage4.Parent = tabControl1;
                                    btCapMask.Visible = true;
                                    BtGetMask.Visible = false;
                                    MaskImage.Visible = true;
                                    lbMagnification.Visible = false;
                                    cbMagnification.Visible = false;
                                    tBLeftCoaLight.Visible = false;
                                    tBRightCoaLight.Visible = false;
                                    btLCoaLightPlus.Visible = false;
                                    btLCoaLightMinus.Visible = false;
                                    btRCoaLightPlus.Visible = false;
                                    btRCoaLightMinus.Visible = false;
                                    NValueChanged();
                                    Update();
                                }
                            });
                        }
                        int iMaskBond = GV.Plc.ReadData16(GV.Plc.iMaskBond);
                        AlignC.UpBotMask = iMaskBond + 1;
                    }
                    else
                    {
                        if (GV.UpBackAlign != 1)
                        {
                            GV.UpBackAlign = 1;
                            AlignC.UpBackAlign = 1;
                            ChangeUpDown(1, 1);
                            Invoke((MethodInvoker)delegate ()
                            {
                                if ((GV.UserLevel == GV.User.Operator) || (GV.UserLevel == GV.User.Engineer))
                                {
                                    tabControl1.SelectedTab = tabPage1;
                                    tabPage2.Parent = null;
                                    tabPage3.Parent = null;
                                    tabPage4.Parent = null;
                                    tabPage5.Parent = null;
                                    tabPage2.Parent = tabControl1;
                                    btCapMask.Visible = false;
                                    BtGetMask.Visible = false;
                                    MaskImage.Visible = false;
                                    NValueChanged();
                                    Update();
                                }
                            });
                        }

                        int iMode = GV.Plc.ReadData16(GV.Plc.iAlignMode);
                        if (iMode != iAlignMode)
                        {
                            iAlignMode = iMode;
                            if (iMode == 1)
                            {
                                Invoke((MethodInvoker)delegate ()
                                {
                                    lbMagnification.Visible = false;
                                    cbMagnification.Visible = false;
                                    tBLeftCoaLight.Visible = false;
                                    tBRightCoaLight.Visible = false;
                                    btLCoaLightPlus.Visible = false;
                                    btLCoaLightMinus.Visible = false;
                                    btRCoaLightPlus.Visible = false;
                                    btRCoaLightMinus.Visible = false;
                                    Update();
                                });
                            }
                            else
                            {
                                Invoke((MethodInvoker)delegate ()
                                {
                                    lbMagnification.Visible = true;
                                    cbMagnification.Visible = true;
                                    cbMagnification.SelectedIndex = GV.LeftZoomLens.Magnification;
                                    tBLeftCoaLight.Value = AlignC.LeftBrightness[GV.LeftZoomLens.Magnification];
                                    tBLeftCoaLight.Visible = true;
                                    //tBLeftCoaLight.Visible = false;
                                    tBRightCoaLight.Value = AlignC.RightBrightness[GV.RightZoomLens.Magnification];
                                    tBRightCoaLight.Visible = true;
                                    //tBRightCoaLight.Visible = false;
                                    btLCoaLightPlus.Visible = true;
                                    btLCoaLightMinus.Visible = true;
                                    btRCoaLightPlus.Visible = true;
                                    btRCoaLightMinus.Visible = true;
                                    Update();
                                });
                            }
                        }
                    }

                    iNowRecipeNumber = GV.Plc.ReadData16(GV.Plc.iNowRecipeNumber);
                    if ((iNowRecipeNumber < 1) || (iNowRecipeNumber > 100)) iNowRecipeNumber = 1;

                    if (iNowRecipeNumber != iNowRecipe)
                    {
                        GM.DebugMessage("Change Recipe Number : " + iNowRecipeNumber.ToString());
                        int oldRecipeNumber = iNowRecipe;
                        iNowRecipe = iNowRecipeNumber;
                        //ChangeRecipeNumber(iNowRecipeNumber);
                        GV.NowRecipeNumber = iNowRecipeNumber;
                        GV.MachineStatus = GV.Status.Standby;

                        //    _ = Task.Run(async () =>
                        //    {
                        //        try
                        //        {
                        //            writeStatus($"正在切換 Recipe {oldRecipeNumber} → {iNowRecipeNumber}...");

                        //            // ✅ 1. 先變更 Recipe（快速操作）
                        //            BeginInvoke((MethodInvoker)delegate {
                        //                ChangeRecipeNumber(iNowRecipeNumber);
                        //            });

                        //            // ✅ 2. 檢查是否需要重啟 AI 服務
                        //            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Python", "AIConfig.json");
                        //            bool needRestart = true;

                        //            try
                        //            {
                        //                if (File.Exists(configPath))
                        //                {
                        //                    string jsonContent = File.ReadAllText(configPath);
                        //                    var aiConfig = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(jsonContent);
                        //                    int configRecipeNumber = (int)aiConfig.recipeNumber;

                        //                    if (configRecipeNumber == iNowRecipeNumber)
                        //                    {
                        //                        writeStatus($"AI 模型已載入 Recipe {iNowRecipeNumber}，無需重啟服務");
                        //                        needRestart = false;
                        //                    }
                        //                    else
                        //                    {
                        //                        //writeStatus($"AI 配置 Recipe 不符 (Config: {configRecipeNumber}, Current: {iNowRecipeNumber})，需要重啟服務");
                        //                    }
                        //                }
                        //                else
                        //                {
                        //                    writeStatus("AIConfig.json 不存在，需要重啟 AI 服務");
                        //                }
                        //            }
                        //            catch (Exception ex)
                        //            {
                        //                writeStatus($"讀取 AIConfig.json 失敗: {ex.Message}，將重啟 AI 服務");
                        //            }

                        //            // ✅ 3. 只有在需要時才重啟 AI 服務
                        //            if (needRestart)
                        //            {
                        //                AIService.IsRestarting = true;

                        //                // 等待正在進行的 AI 推論完成
                        //                await Task.Delay(500);

                        //                writeStatus($"正在重新啟動 AI 服務...");
                        //                EnsureServiceRestart();

                        //                // 等待服務啟動
                        //                await Task.Delay(500);

                        //                // ✅ 4. 更新類別列表
                        //                //try
                        //                //{
                        //                //    var classNames = await GV.AIClassList.GetClassNamesAsync();
                        //                //    var classList = new Dictionary<string, Int16>();
                        //                //    for (int i = 0; i < classNames.Count; i++)
                        //                //    {
                        //                //        classList.Add(classNames[i], (Int16)i);
                        //                //    }

                        //                //    // ✅ 在 UI 執行緒更新控制項
                        //                //    Invoke((MethodInvoker)delegate {
                        //                //        cmLearnPatternUp1.UpdateClassList(classList);
                        //                //        cmLearnPatternBack1.UpdateClassList(classList);
                        //                //    });
                        //                //}
                        //                //catch (Exception ex)
                        //                //{
                        //                //    writeStatus($"更新 AI 類別列表失敗: {ex.Message}");
                        //                //}

                        //                AIService.IsRestarting = false;
                        //            }

                        //            writeStatus($"Recipe 切換完成: {iNowRecipeNumber}");
                        //            iPreviousRecipeNumber = iNowRecipeNumber;
                        //        }
                        //        catch (Exception ex)
                        //        {
                        //            writeStatus($"Recipe 變更處理失敗: {ex.Message}");
                        //            AIService.IsRestarting = false;
                        //        }
                        //    });

                        //    // ✅ 不等待背景任務完成，立即繼續 PLC 輪詢
                        //}
                        _ = Task.Run(async () =>
                        {
                            try
                            {
                                writeStatus($"正在切換 Recipe {oldRecipeNumber} → {iNowRecipeNumber}...");

                                // ✅ 1. 停止搜尋執行緒
                                GV.OnAlign = true; // 暫停搜尋
                                await Task.Delay(500); // 等待執行緒暫停

                                // ✅ 2. 在背景執行緒載入 Recipe XML
                                Recipe newRecipe = null;
                                AlignCondition newAlignC = null;

                                await Task.Run(() =>
                                {
                                    newRecipe = GM.ReadRecipeXml(iNowRecipeNumber);
                                    newAlignC = newRecipe.AlignC;
                                });

                                // ✅ 3. 快速更新全域變數
                                await InvokeAsync(() =>
                                {
                                    GV._recipe = newRecipe;
                                    _recipe = newRecipe;
                                    AlignC = newAlignC;

                                    // 快速更新 UI
                                    GV.UpBackAlign = AlignC.UpBackAlign;
                                    nudMaskScore.Value = (decimal)AlignC.MaskSocre;
                                    nudWaferScore.Value = (decimal)AlignC.WaferScore;
                                    nudXOffset.Value = (decimal)AlignC.XOffset;
                                    nudYLOffest.Value = (decimal)AlignC.YLOffset;
                                    nudYROffest.Value = (decimal)AlignC.YROffset;
                                    nudMaxAlignTimes.Value = (decimal)AlignC.MaxAlignTimes;
                                    nudMinAlignTimes.Value = (decimal)(AlignC.MinAlignTimes);
                                    nudXPrecision.Value = (decimal)AlignC.XPrecision;
                                    nudYPrecision.Value = (decimal)AlignC.YPrecision;
                                    nudThetaPrecision.Value = (decimal)AlignC.ThetaPrecision;
                                    nudExpansion.Value = (decimal)AlignC.MaxExpansion;

                                    if (AlignC.UpBackAlign == 2)
                                    {
                                        gbAlignCondition.Text = GV.Dlang.gbAlignCondition + "  " + AlignC.LastModifyTime.ToString();
                                        nUDMarkDistance.Value = (decimal)AlignC.PatternCenterDistanceUm * 2;

                                        try
                                        {
                                            LeftMask = Cv2.ImRead("LeftMask.bmp", ImreadModes.Grayscale);
                                            RightMask = Cv2.ImRead("RightMask.bmp", ImreadModes.Grayscale);
                                        }
                                        catch (Exception)
                                        {
                                            LeftMask = new Mat(new OpenCvSharp.Size(4000, 3000), MatType.CV_8UC1);
                                            RightMask = new Mat(new OpenCvSharp.Size(4000, 3000), MatType.CV_8UC1);
                                        }
                                        GV.LeftMaskMat = LeftMask;
                                        GV.RightMaskMat = RightMask;

                                        btCapMask.Visible = true;
                                        BtGetMask.Visible = false;
                                        cbMagnification.Visible = false;
                                        lbMagnification.Visible = false;
                                    }
                                    else
                                    {
                                        gbAlignCondition.Text = GV.Dlang.gbAlignCondition + "  " + AlignC.LastModifyTime.ToString();
                                        nUDMarkDistance.Value = (decimal)AlignC.PatternCenterDistanceUm * 2;

                                        cbMagnification.Visible = true;
                                        lbMagnification.Visible = true;
                                        btCapMask.Visible = false;
                                        BtGetMask.Visible = false;
                                    }

                                    lbNowRecipeNumber.Text = GV.Dlang.strRecipeNumber + iNowRecipe.ToString();
                                    GV.Plc.GetRecipe();
                                    Update();
                                });

                                // ✅ 4. 重新學習模板（在背景執行緒）
                                await Task.Run(() =>
                                {
                                    try
                                    {
                                        // ✅ 重新初始化 Matcher（確保使用新的 BlockSize）
                                        GV.matcherLLW.iBlockSize = AlignC.LLWaferBlockSize;
                                        GV.matcherLHW.iBlockSize = AlignC.LHWaferBlockSize;
                                        GV.matcherRLW.iBlockSize = AlignC.RLWaferBlockSize;
                                        GV.matcherRHW.iBlockSize = AlignC.RHWaferBlockSize;

                                        // ✅ 重新學習所有模板
                                        if (_recipe.LeftLowWaferMat != null && !_recipe.LeftLowWaferMat.Empty())
                                            GV.matcherLLW.LearnWithAlgo(_recipe.LeftLowWaferMat, _recipe.LeftLowWaferMask, AlignC.LLWaferAlgorithm);

                                        if (_recipe.LeftHighWaferMat != null && !_recipe.LeftHighWaferMat.Empty())
                                            GV.matcherLHW.LearnWithAlgo(_recipe.LeftHighWaferMat, _recipe.LeftHighWaferMask, AlignC.LHWaferAlgorithm);

                                        if (_recipe.LeftLowMaskMat != null && !_recipe.LeftLowMaskMat.Empty())
                                            GV.matcherLLM.LearnWithAlgo(_recipe.LeftLowMaskMat, _recipe.LeftLowMaskMask, AlignC.LLMaskAlgorithm);

                                        if (_recipe.LeftHighMaskMat != null && !_recipe.LeftHighMaskMat.Empty())
                                            GV.matcherLHM.LearnWithAlgo(_recipe.LeftHighMaskMat, _recipe.LeftHighMaskMask, AlignC.LHMaskAlgorithm);

                                        if (_recipe.RightLowWaferMat != null && !_recipe.RightLowWaferMat.Empty())
                                            GV.matcherRLW.LearnWithAlgo(_recipe.RightLowWaferMat, _recipe.RightLowWaferMask, AlignC.RLWaferAlgorithm);

                                        if (_recipe.RightHighWaferMat != null && !_recipe.RightHighWaferMat.Empty())
                                            GV.matcherRHW.LearnWithAlgo(_recipe.RightHighWaferMat, _recipe.RightHighWaferMask, AlignC.RHWaferAlgorithm);

                                        if (_recipe.RightLowMaskMat != null && !_recipe.RightLowMaskMat.Empty())
                                            GV.matcherRLM.LearnWithAlgo(_recipe.RightLowMaskMat, _recipe.RightLowMaskMask, AlignC.RLMaskAlgorithm);

                                        if (_recipe.RightHighMaskMat != null && !_recipe.RightHighMaskMat.Empty())
                                            GV.matcherRHM.LearnWithAlgo(_recipe.RightHighMaskMat, _recipe.RightHighMaskMask, AlignC.RHMaskAlgorithm);

                                        writeStatus("模板學習完成");
                                    }
                                    catch (Exception ex)
                                    {
                                        writeStatus($"模板學習失敗: {ex.Message}");
                                    }
                                });

                                // ✅ 5. 只在 Bottom CCD 模式時才取得 Mask 位置
                                if (AlignC.UpBackAlign == 2)
                                {
                                    await Task.Run(() =>
                                    {
                                        ProductMatchPositions pmps = GetAllMatchPostion(LeftMask, RightMask, "backmask");
                                        BLastPmps.LMaskMp.X = pmps.LMaskMp.X;
                                        BLastPmps.LMaskMp.Y = pmps.LMaskMp.Y;
                                        BLastPmps.LMaskMp.Score = pmps.LMaskMp.Score;
                                        BLastPmps.RMaskMp.X = pmps.RMaskMp.X;
                                        BLastPmps.RMaskMp.Y = pmps.RMaskMp.Y;
                                        BLastPmps.RMaskMp.Score = pmps.RMaskMp.Score;
                                    });

                                    await InvokeAsync(() => ChangeUpDown(2, 1));
                                }
                                else
                                {
                                    await InvokeAsync(() => ChangeUpDown(1, 1));
                                }

                                // ✅ 6. 恢復搜尋執行緒
                                GV.OnAlign = false;

                                writeStatus($"Recipe 切換完成: {iNowRecipeNumber}");
                                iPreviousRecipeNumber = iNowRecipeNumber;

                                // ✅ 7. AI 服務重啟（獨立任務）
                                string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Python", "AIConfig.json");
                                bool needRestart = true;

                                try
                                {
                                    if (File.Exists(configPath))
                                    {
                                        string jsonContent = File.ReadAllText(configPath);
                                        var aiConfig = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(jsonContent);
                                        int configRecipeNumber = (int)aiConfig.recipeNumber;

                                        if (configRecipeNumber == iNowRecipeNumber)
                                        {
                                            writeStatus($"AI 模型已載入 Recipe {iNowRecipeNumber}，無需重啟服務");
                                            needRestart = false;
                                        }
                                    }
                                    else
                                    {
                                        writeStatus("AIConfig.json 不存在，需要重啟 AI 服務");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    writeStatus($"讀取 AIConfig.json 失敗: {ex.Message}，將重啟 AI 服務");
                                }

                                if (needRestart)
                                {
                                    AIService.IsRestarting = true;
                                    await Task.Delay(500);
                                    writeStatus($"正在重新啟動 AI 服務...");
                                    EnsureServiceRestart();
                                    await Task.Delay(500);
                                    AIService.IsRestarting = false;
                                }
                            }
                            catch (Exception ex)
                            {
                                writeStatus($"Recipe 變更處理失敗: {ex.Message}");
                                AIService.IsRestarting = false;
                            }
                        });

                        // ✅ 不等待背景任務完成，立即繼續 PLC 輪詢
                    }
                    Thread.Sleep(250);
                    if (iTabIndex == 0)
                    {
                        iAlignCode = GV.Plc.ReadData16(GV.Plc.iAlign, 4);
                        if (GV.MachineStatus == GV.Status.Standby)
                        {
                            if (iAlignCode[1] == 2)
                            {
                                if (iUpDownAlign == 0)
                                {
                                    GV.MachineStatus = GV.Status.Align;
                                    iStartAlign = 1;
                                    iAlignOK = 0;
                                }
                                else
                                {
                                    GV.MachineStatus = GV.Status.DownWaferAlign;
                                    iStartAlign = 1;
                                    iAlignOK = 0;
                                }
                            }
                            else if (iAlignCode[3] == 2)
                            {
                                GV.MachineStatus = GV.Status.DownAlign;
                                iStartAlign = 1;
                                iAlignOK = 0;
                            }

                            if (GV.MachineStatus == GV.Status.Standby)
                            {
                                if (iStartAlign == 1) iStartAlign = 0;
                            }
                        }
                        else if (GV.MachineStatus == GV.Status.DownStandby)
                        {
                            if (iAlignCode[1] == 2)
                            {
                                GV.MachineStatus = GV.Status.DownWaferAlign;
                                iStartAlign = 1;
                                iAlignOK = 0;
                            }
                            else if (iAlignCode[3] == 2)
                            {
                                if (GV.skView == 1) ChangeUpDown(1, 1);
                                GV.MachineStatus = GV.Status.DownAlign;
                                iStartAlign = 1;
                                iAlignOK = 0;
                            }
                            if (GV.MachineStatus == GV.Status.DownStandby)
                            {
                            }
                        }
                    }
                    else
                    {
                        GV.MachineStatus = GV.Status.Standby;
                    }

                    switch (GV.MachineStatus)
                    {
                        case (GV.Status.Standby):
                            // if ((iTabIndex == 0) && (GV.skView == 1) && GV.Plc.ReadMemory(GV.Plc.iExposure)) changeUpDown(1, 1);
                            break;
                        case (GV.Status.DownStandby):
                            GV.TickCount = 0;
                            break;
                        case (GV.Status.Align):
                            GV.MachineStatus = GV.Status.Align;
                            if (iStartAlign == 1)
                            {
                                GV.TickCount = 0;
                                iExposure = 0;
                                GV.OnAlign = true;
                                await Align();
                                GV.OnAlign = false;
                            }
                            GV.MachineStatus = GV.Status.Standby;
                            if (GV.AppSettingParm.InTestProgram == true)
                            {
                                if (iTabIndex == 0)
                                {
                                    if (iAlignMatcherRun != 0)
                                    {
                                        GV.OnAlign = false;
                                    }
                                }
                            }
                            break;
                        case (GV.Status.DownAlign):
                            if (iStartAlign == 1)
                            {
                                GV.TickCount = 0;
                                DebugMessage("Do Mask Picture!");
                                GV.OnAlign = true;
                                await DAlign();
                                GV.OnAlign = false;
                                if (iAlignOK == 1)
                                {
                                    GV.MachineStatus = GV.Status.DownStandby;
                                }
                                else
                                {
                                    GV.MachineStatus = GV.Status.Standby;
                                }
                            }
                            break;
                        case GV.Status.DownWaferAlign:
                            if ((LeftMask == null) || (RightMask == null))
                            {
                                LeftMask = Cv2.ImRead(GetTemplateFileName("LeftMask"), ImreadModes.Grayscale);
                                RightMask = Cv2.ImRead(GetTemplateFileName("RightMask"), ImreadModes.Grayscale);
                            }
                            if (iStartAlign == 1)
                            {
                                GV.TickCount = 0;
                                iExposure = 0;
                                GV.OnAlign = true;
                                Thread.Sleep(1000);
                                await DAlignWafer();
                                GV.OnAlign = false;
                                if ((iAlignOK == 1) && !(GV.Plc.ReadMemory(490)))
                                {
                                    GV.MachineStatus = GV.Status.Standby;
                                }
                                else
                                {
                                    GV.MachineStatus = GV.Status.DownStandby;
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
                catch (ThreadAbortException)
                {

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    GM.Quit();
                }
            }
        }

        private void ReportPLCStatus()
        {
            if (GV.UserLevel != GV.User.Administrator)
            {
                GV.EnableTbStatus = 0;
            }
            int[] tempR = GV.Plc.ReadData32(GV.Plc.iRMemoryAddress, GV.Plc.iRCount);
            writeStatus(String.Format("MASKY : {0} , BigY : {1}", tempR[GV.Plc.iiDSMaskY], tempR[GV.Plc.iiDSBigY]));
            writeStatus(String.Format("LeftUpCCDX : {0} , LeftUpCCDY: {1} , RightUpCCDX : {2} , RightUpCCDY : {3}",
                tempR[GV.Plc.iiDSUpLeftX], tempR[GV.Plc.iiDSUpLeftY], tempR[GV.Plc.iiDSUpRightX], tempR[GV.Plc.iiDSUpRightY]));
            writeStatus(String.Format("LeftDownCCDX : {0} , LeftDownCCDY: {1} , LeftDownCCDZ : {2} , RightDownCCDX : {3} , RightDownCCDY : {4} , RightDownCCDZ: {5}",
                tempR[GV.Plc.iiDSDownLeftX], tempR[GV.Plc.iiDSDownLeftY], tempR[GV.Plc.iiDSDownLeftZ],
                tempR[GV.Plc.iiDSUpRightX], tempR[GV.Plc.iiDSUpRightY], tempR[GV.Plc.iiDSDownRightZ]));
            writeStatus(String.Format("Chuck X : {0} , Chuck Y1 : {1} , Chuck Y2 : {2}, ChuckZ : {3}",
                tempR[GV.Plc.iiDSChuckX], tempR[GV.Plc.iiDSChuckY1], tempR[GV.Plc.iiDSChuckY2], tempR[GV.Plc.iiDSChuckZ]));

            GV.EnableTbStatus = 1;
        }

        public async Task Align()
        {
            try
            {
                Stopwatch sw = Stopwatch.StartNew();
                iAlignCode = GV.Plc.ReadData16(GV.Plc.iAlign, 4);
                if (iAlignCode[1] == 0)
                {
                    iStartAlign = 0;
                    return;
                }

                Thread.Sleep(500);
                if (_recipe.LeftHighMaskMat == null || _recipe.LeftHighWaferMat == null || _recipe.LeftLowMaskMat == null || _recipe.LeftLowWaferMat == null
                || _recipe.RightHighMaskMat == null || _recipe.RightHighWaferMat == null || _recipe.RightLowMaskMat == null || _recipe.RightLowWaferMat == null)
                {
                    GV.Plc.SendAlignNG();
                    // throw new MRException("Error : Please learn (high/low) pattern first");
                    writeStatus(GV.Dlang.strErrorCreateFirst);
                    iStartAlign = 0;
                    return;
                }

                writeStatus(GV.Dlang.strClear);
                if (_isTestMode)
                    writeStatus("Test mode : ( " + _cycleTestTarget.ToString() + "/" + _cycleTestTimes.ToString() + " )");

                writeStatus(GV.Dlang.strMessageLowMagnification);
                //if (GV.skView != 0) changeUpDown(1, 1);
                writeStatus(GV.Dlang.strMessageZoomOut);
                HwndFormMain.BeginInvoke(ActLowDelegate, new object[] { });
                Task t1 = Task.Run(() => ChangeLensMagnification(AlignC.AlignLowMagnification));
                Thread.Sleep(1500);
                iAlignCode = GV.Plc.ReadData16(GV.Plc.iAlign, 4);
                if (iAlignCode[1] == 0)
                {
                    iStartAlign = 0;
                    return;
                }
                await t1;
                writeStatus(GV.Dlang.strMessageMaskCenter);
                
                await Task.Run(() => MoveCamera(AlignC.AlignLowMagnification));
                if (iStartAlign == 0)
                {
                    return;
                }
                Thread.Sleep(500);
                iAlignCode = GV.Plc.ReadData16(GV.Plc.iAlign, 4);
                if (iAlignCode[1] == 0)
                {
                    iStartAlign = 0;
                    return;
                }
                writeStatus(GV.Dlang.strMessageWaferSearch);
                await Task.Run(() => SearchWafer());
                iAlignCode = GV.Plc.ReadData16(GV.Plc.iAlign, 4);
                if (iAlignCode[1] == 0)
                {
                    iStartAlign = 0;
                    return;
                }
                await Task.Run(() => MoveTableToMatchPatternAtSpecificMagnification("low"));
                Thread.Sleep(200);
                iAlignCode = GV.Plc.ReadData16(GV.Plc.iAlign, 4);
                if (iAlignCode[1] == 0)
                {
                    iStartAlign = 0;
                    return;
                }
                writeStatus(GV.Dlang.strMessagesHighMagnification);
                writeStatus(GV.Dlang.strMessageZoomIn);
                HwndFormMain.BeginInvoke(ActHighDelegate, new object[] { });
                iZoomAdj = 1;
                Task t3 = Task.Run(() =>
                {
                    ChangeLensMagnification(AlignC.AlignHighMagnification);
                    Thread.Sleep(500);
                    MoveCamera(AlignC.AlignHighMagnification);
                    Thread.Sleep(500);
                });
                await t3;
                iZoomAdj = 0;
                iAlignCode = GV.Plc.ReadData16(GV.Plc.iAlign, 4);
                if (iAlignCode[1] == 0)
                {
                    iStartAlign = 0;
                    return;
                }
                await Task.Run(() => MoveTableToMatchPatternAtSpecificMagnification("high"));
                Thread.Sleep(500);
                iAlignCode = GV.Plc.ReadData16(GV.Plc.iAlign, 4);
                if (iAlignCode[1] == 0 || (iStartAlign == 0))
                {
                    iStartAlign = 0;
                    return;
                }

                writeStatus(GV.Dlang.strMessageAlign);
                await Task.Run(() => MoveTableToMatchPattern());
                Thread.Sleep(500);
                iAlignCode = GV.Plc.ReadData16(GV.Plc.iAlign, 4);
                if (iAlignCode[1] == 0 || (iStartAlign == 0))
                {
                    iStartAlign = 0;
                    return;
                }
                await Task.Run(() => MoveTableToMatchPatternAfterContactShift());
                Thread.Sleep(500);
                writeStatus(string.Format(GV.Dlang.strMessageElaspedTime, sw.ElapsedMilliseconds));

                if (iExposure == 1)
                    GM.SaveImageBeforeExposure(GV.LeftUpCam.Grab(), GV.RightUpCam.Grab());

                if (_isTestMode)
                {
                    _cycleTestTimes++;
                    labelCycleTestCycleTestTimes.Text = "( " + _cycleTestTarget.ToString() + " / " + _cycleTestTimes.ToString() + " )";
                    if (_cycleTestTimes == _cycleTestTarget)
                    {
                        //GV.Plc.Send("WR MR37000 0");
                        _isTestMode = false;
                        buttonTestMode.Text = "Cycle test";
                        MessageBox.Show("Cycle test finished");
                    }
                }
            }
            catch (Exception ex)
            {
                writeStatus(ex.Message);
            }

        }

        public async Task DAlign()
        {
            try
            {
                iUpMaskPmps = 0;
                Stopwatch sw = Stopwatch.StartNew();

                // DoTopCCDAlignAsync();
                if (GV.matcherLLM != null && _recipe.LeftLowMaskMat != null)
                {
                    GV.matcherLLM.LearnWithAlgo(_recipe.LeftLowMaskMat, _recipe.LeftLowMaskMask, AlignC.LLMaskAlgorithm);
                }
                if (GV.matcherRLM != null && _recipe.RightLowMaskMat != null)
                {
                    GV.matcherRLM.LearnWithAlgo(_recipe.RightLowMaskMat, _recipe.RightLowMaskMask, AlignC.RLMaskAlgorithm);
                }
                if (GV.matcherLHW != null && _recipe.LeftHighWaferMat != null)
                {
                    GV.matcherLHW.LearnWithAlgo(_recipe.LeftHighWaferMat, _recipe.LeftHighWaferMask, AlignC.LHWaferAlgorithm);
                }
                if (GV.matcherRHW != null && _recipe.RightHighWaferMat != null)
                {
                    GV.matcherRHW.LearnWithAlgo(_recipe.RightHighWaferMat, _recipe.RightHighWaferMask, AlignC.RHWaferAlgorithm);
                }
                ChangeUpDown(2, 1);
                iAlignCode = GV.Plc.ReadData16(GV.Plc.iAlign, 4);
                if (iAlignCode[3] == 0)
                {
                    iStartAlign = 0;
                    iAlignOK = 0;
                    writeStatus("Do Mask Picture faile Step 1!");
                    return;
                }

                await Task.Run(() => MoveCameraBack(0));
                Thread.Sleep(500);
            }
            catch (Exception ex)
            {
                writeStatus(ex.Message);
            }

        }

        private async Task DoTopCCDAlignAsync()
        {
            iLWaferHigh = GV.Plc.ReadData32(GV.Plc.iLWaferHigh);
            iRWaferHigh = GV.Plc.ReadData32(GV.Plc.iRWaferHigh);

            if (!GV.Plc.ReadMemory(GV.Plc.iMaskStart))
            {
                iStartAlign = 0;
                iAlignOK = 0;
                return;
            }

            if (_recipe.LeftHighMaskMat == null || _recipe.LeftHighWaferMat == null || _recipe.LeftLowMaskMat == null || _recipe.LeftLowWaferMat == null
            || _recipe.RightHighMaskMat == null || _recipe.RightHighWaferMat == null || _recipe.RightLowMaskMat == null || _recipe.RightLowWaferMat == null)
            {
                GV.Plc.SendAlignNG();
                writeStatus(GV.Dlang.strErrorCreateBottomFirst);
                // throw new MRException("Error : Please learn (high/low) pattern first");
                iStartAlign = 0;
                iAlignOK = 0;
                return;
            }

            writeStatus(GV.Dlang.strClear);
            if (_isTestMode)
                writeStatus("Test mode : ( " + _cycleTestTarget.ToString() + "/" + _cycleTestTimes.ToString() + " )");

            writeStatus(GV.Dlang.strMessageBottomMaskAlign);
            writeStatus(GV.Dlang.strMessageZoomOut);
            HwndFormMain.BeginInvoke(ActLowDelegate, new object[] { });
            Task t1 = Task.Run(() => ChangeLensMagnification(AlignC.AlignLowMagnification));
            await t1;
            if (!GV.Plc.ReadMemory(GV.Plc.iDownCCDStart))
            {
                iStartAlign = 0;
                iAlignOK = 0;
                return;
            }
            ChangeUpDown(1, 1);
            writeStatus(GV.Dlang.strMessageMaskCenter);
            iAlignOK = 0;
            writeStatus(GV.Dlang.strMessageLowMagnification);
            await Task.Run(() => MoveCameraMaskUp("low"));
            if (iAlignOK == 0) return;
            Task t2 = Task.Run(() => ChangeLensMagnification(AlignC.AlignHighMagnification));
            await t2;
            Thread.Sleep(250);
            if (!GV.Plc.ReadMemory(GV.Plc.iDownCCDStart))
            {
                iStartAlign = 0;
                iAlignOK = 0;
                return;
            }
            iAlignOK = 0;
            HwndFormMain.BeginInvoke(ActHighDelegate, new object[] { });
            writeStatus(GV.Dlang.strMessagesHighMagnification);
            await Task.Run(() => MoveCameraMaskUp("high"));
            if (iAlignOK == 0) return;
            if (!GV.Plc.ReadMemory(GV.Plc.iDownCCDStart))
            {
                iStartAlign = 0;
                iAlignOK = 0;
                return;
            }
        }

        public async Task DAlignWafer()
        {
            try
            {
                writeStatus(GV.Dlang.strMessageCheckMaskAlign);
                if ((_recipe.LeftHighWaferMat == null) || (_recipe.RightHighWaferMat == null))
                {
                    return;
                }

                if (iStartAlign == 1)
                {
                    if (AlignC.UpBotMask == 2)
                    {
                        
                        ProductMatchPositions pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "backmask");
                        Thread.Sleep(500);
                        string TopMaskCheck2 = string.Format(GV.Dlang.strMessageMaskShift, pmps.LMaskMp.X, pmps.RMaskMp.X, pmps.LMaskMp.Y, pmps.RMaskMp.Y);
                        TopMaskCheck2 = string.Format("LMaskScore = {0:F} , RMaskScore = {1:F}", pmps.LMaskMp.Score, pmps.RMaskMp.Score) + TopMaskCheck2; ;
                        DebugMessage("2:" + TopMaskCheck2);
                        if ((pmps.LMaskMp.Score > AlignC.MaskSocre) && (pmps.RMaskMp.Score > AlignC.MaskSocre))
                        {
                            BLivePmps.LMaskMp.X = pmps.LMaskMp.X;
                            BLivePmps.LMaskMp.Y = pmps.LMaskMp.Y;
                            BLivePmps.LMaskMp.Score = BLastPmps.LMaskMp.Score;
                            BLivePmps.RMaskMp.X = pmps.RMaskMp.X;
                            BLivePmps.RMaskMp.Y = pmps.RMaskMp.Y;
                            BLivePmps.RMaskMp.Score = BLastPmps.RMaskMp.Score;
                        }
                    }
                    else
                    {
                        
                        ProductMatchPositions pmps = GetAllMatchPostion(LeftMask, RightMask, "backmask");
                        Thread.Sleep(500);
                        if ((pmps.LMaskMp.Score > GV.AppSettingParm.LMaskScoreW) && (pmps.RMaskMp.Score > GV.AppSettingParm.RMaskScoreW))
                        {
                            string TopMaskCheck3 = string.Format(GV.Dlang.strMessageMaskShift, pmps.LMaskMp.X, pmps.RMaskMp.X, pmps.LMaskMp.Y, pmps.RMaskMp.Y);
                            DebugMessage("3:" + TopMaskCheck3);
                            BLivePmps.LMaskMp.X = pmps.LMaskMp.X;
                            BLivePmps.LMaskMp.Y = pmps.LMaskMp.Y;
                            BLivePmps.LMaskMp.Score = BLastPmps.LMaskMp.Score;
                            BLivePmps.RMaskMp.X = pmps.RMaskMp.X;
                            BLivePmps.RMaskMp.Y = pmps.RMaskMp.Y;
                            BLivePmps.RMaskMp.Score = BLastPmps.RMaskMp.Score;
                        }
                        else
                        {
                            BLivePmps.LMaskMp.X = BLastPmps.LMaskMp.X;
                            BLivePmps.LMaskMp.Y = BLastPmps.LMaskMp.Y;
                            BLivePmps.LMaskMp.Score = BLastPmps.LMaskMp.Score;
                            BLivePmps.RMaskMp.X = BLastPmps.RMaskMp.X;
                            BLivePmps.RMaskMp.Y = BLastPmps.RMaskMp.Y;
                            BLivePmps.RMaskMp.Score = BLastPmps.RMaskMp.Score;
                            string TopMaskCheck3 = string.Format(GV.Dlang.strMessageMaskShift, BLivePmps.LMaskMp.X, BLivePmps.RMaskMp.X, BLivePmps.LMaskMp.Y, BLivePmps.RMaskMp.Y);
                            writeStatus("4: " + TopMaskCheck3);
                            DebugMessage("4: " + TopMaskCheck3);
                        }
                    }

                    if (MaskImage.Checked)
                    {
                        skLeftAlign.SetShowMask(true, AlignC.dLalpha, LeftMask);
                        skRightAlign.SetShowMask(true, AlignC.dRalpha, RightMask);
                    }

                    if (AlignC.AdjuestZ == 1) MoveDownCCD();

                    ChangeUpDown(2, 2);

                    SearchWaferBack();
                }
                if ((iWaferSearchOK == 0) || (iStartAlign == 0)) return;
                writeStatus(GV.Dlang.strMessageBottomWaferAlign);
                await Task.Run(() => MoveTableToMatchPatternBack());
                Thread.Sleep(500);
                if ((iStartAlign == 0) || (iAlignOK == 0)) return;
                await Task.Run(() => MoveTableToMatchPatternAfterContactShiftBack());
                Thread.Sleep(500);
            }
            catch (Exception ex)
            {
                writeStatus(ex.Message);
            }
        }

        private void MoveDownCCD()
        {
            GV.Plc.GetRecipe();

            int lwz = GV.Plc.RecipeData[45];
            int rwz = GV.Plc.RecipeData[46];

            GV.Plc.AlignDownCameraMoveZAbsW(1, 0, 0, 0, 0, lwz, rwz);
        }

        private void CheckMaskAlign()
        {
            if (iUpMaskPmps == 1)
            {
                Thread.Sleep(500);
                ProductMatchPositions pmps = GetAllMatchPostion(GV.LeftUpCam.Grab(), GV.RightUpCam.Grab(), "checkmask");
                double dMoveLShiftXum = (pmps.LMaskMp.X - UpMaskPmps.LMaskMp.X) * GV.ZoomLensInfo.LeftUmPerPixelX[AlignC.AlignHighMagnification];
                double dMoveLShiftYum = (pmps.LMaskMp.Y - UpMaskPmps.LMaskMp.Y) * GV.ZoomLensInfo.LeftUmPerPixelY[AlignC.AlignHighMagnification];
                double dMoveRShiftXum = (pmps.RMaskMp.X - UpMaskPmps.RMaskMp.X) * GV.ZoomLensInfo.RightUmPerPixelX[AlignC.AlignHighMagnification];
                double dMoveRShiftYum = (pmps.RMaskMp.Y - UpMaskPmps.RMaskMp.Y) * GV.ZoomLensInfo.RightUmPerPixelY[AlignC.AlignHighMagnification];

                // Mat Ldimage = Lmat.Clone();
                // Mat Rdimage = Rmat.Clone();
                // LogActivities.GenerateImageLog(iNowRecipe, "BMask", Ldimage, Rdimage);

                // _dMoveLShiftXum = dMoveLShiftXum + GV.AppSettingParm._LxShift;
                // _dMoveLShiftYum = dMoveLShiftYum + GV.AppSettingParm._LyShift;
                // _dMoveRShiftXum = dMoveRShiftXum + GV.AppSettingParm._RxShift;
                // _dMoveRShiftYum = dMoveRShiftYum + GV.AppSettingParm._RyShift;

                _dMoveLShiftXum = GV.AppSettingParm._LxShift;
                _dMoveLShiftYum = GV.AppSettingParm._LyShift;
                _dMoveRShiftXum = GV.AppSettingParm._RxShift;
                _dMoveRShiftYum = GV.AppSettingParm._RyShift;

                _dMoveLShiftXPixel = _dMoveLShiftXum / GV.ZoomLensInfo.LeftDownUmPerPixelX;
                _dMoveLShiftYPixel = _dMoveLShiftYum / GV.ZoomLensInfo.LeftDownUmPerPixelY;
                _dMoveRShiftXPixel = _dMoveRShiftXum / GV.ZoomLensInfo.RightDownUmPerPixelX;
                _dMoveRShiftYPixel = _dMoveRShiftYum / GV.ZoomLensInfo.RightDownUmPerPixelY;

                // BLivePmps.LMaskMp.X = BLastPmps.LMaskMp.X + (float)_dMoveLShiftXPixel;
                // BLivePmps.LMaskMp.Y = BLastPmps.LMaskMp.Y + (float)_dMoveLShiftYPixel;
                // BLivePmps.LMaskMp.Score = BLastPmps.LMaskMp.Score;
                // BLivePmps.RMaskMp.X = BLastPmps.RMaskMp.X + (float)_dMoveRShiftXPixel;
                // BLivePmps.RMaskMp.Y = BLastPmps.RMaskMp.Y + (float)_dMoveRShiftYPixel;
                // BLivePmps.RMaskMp.Score = BLastPmps.RMaskMp.Score;

                BLivePmps.LMaskMp.X = BLastPmps.LMaskMp.X;
                BLivePmps.LMaskMp.Y = BLastPmps.LMaskMp.Y;
                BLivePmps.LMaskMp.Score = BLastPmps.LMaskMp.Score;
                BLivePmps.RMaskMp.X = BLastPmps.RMaskMp.X;
                BLivePmps.RMaskMp.Y = BLastPmps.RMaskMp.Y;
                BLivePmps.RMaskMp.Score = BLastPmps.RMaskMp.Score;

                /*
                LastPmps.LMaskMp.X = UpMaskPmps.LMaskMp.X;
                LastPmps.LMaskMp.Y = UpMaskPmps.LMaskMp.Y;
                LastPmps.RMaskMp.X = UpMaskPmps.RMaskMp.X;
                LastPmps.RMaskMp.Y = UpMaskPmps.RMaskMp.Y;
                */

                double lOffsetXSteps = dMoveLShiftXum / GV.ZoomLensInfo.LeftDownUmPerStepX;
                double lOffsetYSteps = dMoveLShiftYum / GV.ZoomLensInfo.LeftDownUmPerStepY;
                double rOffsetXSteps = dMoveRShiftXum / GV.ZoomLensInfo.RightDownUmPerStepX;
                double rOffsetYSteps = dMoveRShiftYum / GV.ZoomLensInfo.RightDownUmPerStepY;

                if (GV.BaslerCamParm.LeftBackReserveX)
                {
                    lOffsetXSteps *= -1;
                }

                if (GV.BaslerCamParm.RightBackReserveX)
                {
                    rOffsetXSteps *= -1;
                }

                string TopMaskCheck1 = string.Format(GV.Dlang.strMessageMaskShift, -dMoveLShiftXum, dMoveRShiftXum, -dMoveLShiftYum, -dMoveRShiftYum);
                writeStatus("1:" + TopMaskCheck1);
                // string TopMaskCheck = string.Format(GV.Dlang.strMessageMaskShift, -lOffsetXSteps, rOffsetXSteps, -lOffsetYSteps, -rOffsetYSteps);
                // writeStatus(TopMaskCheck);
                // GV.Plc.AlignDownCameraMove(0, -lOffsetXSteps, rOffsetXSteps, -lOffsetYSteps, -rOffsetYSteps);
                // Thread.Sleep(250);
            }
        }

        private bool CheckAlignConditions(ProductMatchPositions pmps, out AlignResultData alignResultData)
        {
            double AfterMoveLShiftXum = (pmps.LWaferMp.X - pmps.LMaskMp.X) * GV.ZoomLensInfo.LeftUmPerPixelX[AlignC.AlignHighMagnification];
            double AfterMoveLShiftYum = (pmps.LWaferMp.Y - pmps.LMaskMp.Y) * GV.ZoomLensInfo.LeftUmPerPixelY[AlignC.AlignHighMagnification];
            double AfterMoveRShiftXum = (pmps.RWaferMp.X - pmps.RMaskMp.X) * GV.ZoomLensInfo.RightUmPerPixelX[AlignC.AlignHighMagnification];
            double AfterMoveRShiftYum = (pmps.RWaferMp.Y - pmps.RMaskMp.Y) * GV.ZoomLensInfo.RightUmPerPixelY[AlignC.AlignHighMagnification];

            double precisionXL = (pmps.LWaferMp.X - pmps.LMaskMp.X) * GV.ZoomLensInfo.LeftUmPerPixelX[AlignC.AlignHighMagnification];
            double precisionXR = (pmps.RWaferMp.X - pmps.RMaskMp.X) * GV.ZoomLensInfo.RightUmPerPixelX[AlignC.AlignHighMagnification];
            double precisionX = (precisionXR + precisionXL) / 2.0f;
            double precisionY = ((pmps.LWaferMp.Y - pmps.LMaskMp.Y) * GV.ZoomLensInfo.LeftUmPerPixelY[AlignC.AlignHighMagnification] + (pmps.RWaferMp.Y - pmps.RMaskMp.Y) * GV.ZoomLensInfo.RightUmPerPixelY[AlignC.AlignHighMagnification]) / 2.0f;
            double degree = Math.Atan((AfterMoveLShiftYum - AfterMoveRShiftYum) / ((pmps.PatternCenterDistance))) * 180 / Math.PI;
            double degreeL = AfterMoveLShiftYum - AfterMoveRShiftYum;
            double expansionN = precisionXR - precisionXL;
            double expansion = expansionN / 2.0f;

            double expan = Math.Abs(expansion);

            double preXL = Math.Abs(precisionXL);
            double preXR = Math.Abs(precisionXR);
            double preX = Math.Abs(preXL - preXR);
            // AlignC.XPrecision

            if (expansionN > 0)
            {
                precisionXL += expan;
                precisionXR -= expan;
            }
            else
            {
                precisionXL -= expan;
                precisionXR += expan;
            }

            writeStatus(string.Format(GV.Dlang.strMessageLShiftum, AfterMoveLShiftXum.ToString("F2"), AfterMoveLShiftYum.ToString("F2")));
            writeStatus(string.Format(GV.Dlang.strMessageRShiftum, AfterMoveRShiftXum.ToString("F2"), AfterMoveRShiftYum.ToString("F2")));
            writeStatus(string.Format(GV.Dlang.strMessageLeftMaskWaferScore, pmps.LMaskMp.Score.ToString("F2"), pmps.LWaferMp.Score.ToString("F2")));
            writeStatus(string.Format(GV.Dlang.strMessageRightMaskWaferScore, pmps.RMaskMp.Score.ToString("F2"), pmps.RWaferMp.Score.ToString("F2")));
            writeStatus(GV.Dlang.strMessagePrecisionX + " L " + preXL.ToString("F2"));
            writeStatus(GV.Dlang.strMessagePrecisionX + " R " + preXR.ToString("F2"));
            writeStatus(GV.Dlang.strMessagePrecisionX + " X " + preX.ToString("F2"));
            writeStatus(GV.Dlang.strMessagePrecisionX + precisionX.ToString("F2"));
            writeStatus(GV.Dlang.strMessagePrecisionY + precisionY.ToString("F2"));
            writeStatus(GV.Dlang.strMessageRotation + Math.Abs(degree).ToString("F8"));
            writeStatus(GV.Dlang.strMessageRotation + " um " + degreeL.ToString("F2"));
            writeStatus(GV.Dlang.strMessageExpansion + expansion.ToString("F2"));

            float lxum = (float)GV.ZoomLensInfo.LeftUmPerPixelX[AlignC.AlignHighMagnification];
            float lyum = (float)GV.ZoomLensInfo.LeftUmPerPixelY[AlignC.AlignHighMagnification];
            float rxum = (float)GV.ZoomLensInfo.RightUmPerPixelX[AlignC.AlignHighMagnification];
            float ryum = (float)GV.ZoomLensInfo.RightUmPerPixelY[AlignC.AlignHighMagnification];
            float fPreX = (float)(AlignC.XPrecision * 2) / 3;
            alignResultData = new AlignResultData((pmps.LWaferMp.X - pmps.LMaskMp.X) * lxum, (pmps.LWaferMp.Y - pmps.LMaskMp.Y) * lyum, (pmps.RWaferMp.X - pmps.RMaskMp.X) * rxum, (pmps.RWaferMp.Y - pmps.RMaskMp.Y) * ryum
                , precisionX, precisionY, expansion);
            // if (Math.Abs(precisionX) < AlignC.XPrecision && Math.Abs(precisionY) < AlignC.YPrecision && Math.Abs(degree) < AlignC.ThetaPrecision && Math.Abs(expansion) < AlignC.MaxExpansion)
            // if (preX < (AlignC.XPrecision / 2) && Math.Abs(precisionXL) < AlignC.XPrecision && Math.Abs(precisionXR) < AlignC.XPrecision && Math.Abs(precisionY) < AlignC.YPrecision && Math.Abs(degreeL) < AlignC.ThetaPrecision && Math.Abs(expansion) < AlignC.MaxExpansion)

            if (preX < fPreX && Math.Abs(precisionX) < AlignC.XPrecision && Math.Abs(precisionY) < AlignC.YPrecision && Math.Abs(degreeL) < AlignC.ThetaPrecision && Math.Abs(expansion) < AlignC.MaxExpansion)
                return true;
            else
                return false;
        }

        private bool CheckAlignConditionsBack(ProductMatchPositions pmps, out AlignResultData alignResultData)
        {
            double lxum = GV.ZoomLensInfo.LeftDownUmPerPixelX;
            double lyum = GV.ZoomLensInfo.LeftDownUmPerPixelY;
            double rxum = GV.ZoomLensInfo.RightDownUmPerPixelX;
            double ryum = GV.ZoomLensInfo.RightDownUmPerPixelY;

            double AfterMoveLShiftXum = (pmps.LWaferMp.X - pmps.LMaskMp.X) * lxum;
            double AfterMoveLShiftYum = (pmps.LWaferMp.Y - pmps.LMaskMp.Y) * lyum;
            double AfterMoveRShiftXum = (pmps.RWaferMp.X - pmps.RMaskMp.X) * rxum;
            double AfterMoveRShiftYum = (pmps.RWaferMp.Y - pmps.RMaskMp.Y) * ryum;

            double precisionXL = (pmps.LWaferMp.X - pmps.LMaskMp.X) * lxum;
            double precisionXR = (pmps.RWaferMp.X - pmps.RMaskMp.X) * rxum;
            double precisionX = (precisionXL + precisionXR) / 2.0f;
            double precisionY = ((pmps.LWaferMp.Y - pmps.LMaskMp.Y) * lyum + (pmps.RWaferMp.Y - pmps.RMaskMp.Y) * ryum) / 2.0f;
            double degreeL = AfterMoveLShiftYum - AfterMoveRShiftYum;
            double degree = Math.Atan((AfterMoveLShiftYum - AfterMoveRShiftYum) / ((pmps.PatternCenterDistance))) * 180 / Math.PI;
            double expansionN = precisionXR - precisionXL;
            double expansion = expansionN / 2.0f;

            double expan = Math.Abs(expansion);
            double LSxum = AfterMoveLShiftXum + expansion;
            double RSxum = AfterMoveRShiftXum - expansion;
            if (expansion > 0)
            {
                LSxum = precisionXL + expan;
                RSxum = precisionXR - expan;
            }
            else
            {
                LSxum = precisionXL - expan;
                RSxum = precisionXR + expan;
            }

            double preXL = Math.Abs(precisionXL);
            double preXR = Math.Abs(precisionXR);
            double preX = Math.Abs(preXL - preXR);
            double dPreX = (float)(AlignC.XPrecision * 2) / 3;
            // writeStatus("check L shift(um) : (" + AfterMoveLShiftXum.ToString("F2") + " , " + AfterMoveLShiftYum.ToString("F2") + ")");
            // writeStatus("check R shift(um) : (" + AfterMoveRShiftXum.ToString("F2") + " , " + AfterMoveRShiftYum.ToString("F2") + ")");
            // writeStatus("L Mask / Wafer score : " + pmps.LMaskMp.Score.ToString("F2") + " , " + pmps.LWaferMp.Score.ToString("F2"));
            // writeStatus("R Mask / Wafer score : " + pmps.RMaskMp.Score.ToString("F2") + " , " + pmps.RWaferMp.Score.ToString("F2"));
            // writeStatus("PrecisionX(um) : " + precisionX.ToString("F2"));
            // writeStatus("PrecisionY(um) : " + precisionY.ToString("F2"));
            // writeStatus("Rotation(deg) : " + Math.Abs(degree).ToString("F8"));
            // writeStatus("Expansion(um) : " + expansion.ToString("F2"));
            writeStatus(string.Format(GV.Dlang.strMessageLShiftum, AfterMoveLShiftXum.ToString("F2"), AfterMoveLShiftYum.ToString("F2")));
            writeStatus(string.Format(GV.Dlang.strMessageRShiftum, AfterMoveRShiftXum.ToString("F2"), AfterMoveRShiftYum.ToString("F2")));
            writeStatus(string.Format(GV.Dlang.strMessageLeftMaskWaferScore, pmps.LMaskMp.Score.ToString("F2"), pmps.LWaferMp.Score.ToString("F2")));
            writeStatus(string.Format(GV.Dlang.strMessageRightMaskWaferScore, pmps.RMaskMp.Score.ToString("F2"), pmps.RWaferMp.Score.ToString("F2")));
            writeStatus(GV.Dlang.strMessagePrecisionX + precisionX.ToString("F2"));
            writeStatus(GV.Dlang.strMessagePrecisionY + precisionY.ToString("F2"));
            writeStatus(GV.Dlang.strMessageRotation + Math.Abs(degree).ToString("F8"));
            writeStatus(GV.Dlang.strMessageRotation + " um " + degreeL.ToString("F2"));
            writeStatus(GV.Dlang.strMessageExpansion + expansion.ToString("F2"));

            alignResultData = new AlignResultData((pmps.LWaferMp.X - pmps.LMaskMp.X) * lxum, (pmps.LWaferMp.Y - pmps.LMaskMp.Y) * lyum, (pmps.RWaferMp.X - pmps.RMaskMp.X) * rxum, (pmps.RWaferMp.Y - pmps.RMaskMp.Y) * ryum
                , precisionX, precisionY, expansion);

            // if ((Math.Abs(LSxum) < AlignC.XPrecision) && (Math.Abs(RSxum) < AlignC.XPrecision) && Math.Abs(precisionY) < AlignC.YPrecision && Math.Abs(degree) < AlignC.ThetaPrecision && Math.Abs(expansion) < AlignC.MaxExpansion)
            // if ((Math.Abs(AfterMoveLShiftXum) < AlignC.XPrecision) && (Math.Abs(AfterMoveRShiftXum) < AlignC.XPrecision) && (Math.Abs(AfterMoveLShiftYum) < AlignC.YPrecision) && (Math.Abs(AfterMoveRShiftYum) < AlignC.YPrecision) 
            //    && (Math.Abs(degree) < AlignC.ThetaPrecision) && (Math.Abs(expansion) < AlignC.MaxExpansion))
            if (preX < dPreX && Math.Abs(precisionX) < AlignC.XPrecision && Math.Abs(precisionY) < AlignC.YPrecision && Math.Abs(degreeL) < AlignC.ThetaPrecision && Math.Abs(expansion) < AlignC.MaxExpansion)
                return true;
            else
                return false;
        }

        private int[] CalcuteMaskWaferShiftAndReturnMotorSteps(MatchPosition lMaskMp, MatchPosition lWaferMp, MatchPosition rMaskMp, MatchPosition rWaferMp, double patternCenterDistance)
        {
            int[] motorSteps = new int[3];

            double pCenterDistance = patternCenterDistance / 2;

            double ly = ((double)(lWaferMp.Y - lMaskMp.Y)) * GV.ZoomLensInfo.LeftUmPerPixelY[GV.LeftZoomLens.Magnification];
            double ry = ((double)(rWaferMp.Y - rMaskMp.Y)) * GV.ZoomLensInfo.RightUmPerPixelY[GV.RightZoomLens.Magnification];
            double waferSin = (ry - ly) / pCenterDistance;
            double waferRad = Math.Asin(waferSin);
            double waferDegree = waferSin * 180 / Math.PI;
            double rotateMotorSteps = waferDegree * GV.ZoomLensInfo.MotorStepsPerPixelDegree;

            double a, b, c;
            a = b = pCenterDistance;
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

            double xMotorStepsl = (lMaskMp.X - lWaferMp.X) * GV.ZoomLensInfo.LeftMotorStepsPerPixelX[GV.LeftZoomLens.Magnification];
            double xMotorStepsr = (rMaskMp.X - rWaferMp.X) * GV.ZoomLensInfo.RightMotorStepsPerPixelX[GV.RightZoomLens.Magnification];
            double xMotorSteps = (xMotorStepsl + xMotorStepsr) / 2;
            double yMotorStepsl = (lMaskMp.Y - lWaferMp.Y) * GV.ZoomLensInfo.LeftMotorStepsPerPixelY[GV.LeftZoomLens.Magnification];
            double yMotorStepsr = (rMaskMp.Y - rWaferMp.Y) * GV.ZoomLensInfo.RightMotorStepsPerPixelY[GV.RightZoomLens.Magnification];
            double yMotorSteps = (yMotorStepsl + yMotorStepsr) / 2;

            motorSteps[0] = (int)Math.Round(xMotorSteps);
            motorSteps[1] = (int)Math.Round(yMotorSteps);
            motorSteps[2] = (int)Math.Round(rotateMotorSteps);
            return motorSteps;
        }

        private int[] CalcuteMaskWaferShiftAndReturnMotorStepsCenter(MatchPosition lMaskMp, MatchPosition lWaferMp, MatchPosition rMaskMp, MatchPosition rWaferMp, double patternCenterDistancePixel)
        {
            int[] motorSteps = new int[3];

            double waferSin = ((lWaferMp.Y - lMaskMp.Y) - (rWaferMp.Y - rMaskMp.Y)) / patternCenterDistancePixel;
            double waferRad = Math.Asin(waferSin);
            double waferDegree = waferSin * 180 / Math.PI;
            double rotateMotorSteps = waferDegree * GV.ZoomLensInfo.MotorStepsPerPixelDegree;
            double a, b, c;
            a = b = patternCenterDistancePixel / 2;
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

            double xl = (lMaskMp.X - lWaferMp.X) * GV.ZoomLensInfo.LeftMotorStepsPerPixelX[GV.LeftZoomLens.Magnification];
            double xr = (rMaskMp.X - rWaferMp.X) * GV.ZoomLensInfo.RightMotorStepsPerPixelX[GV.RightZoomLens.Magnification];

            double yl = (lMaskMp.Y - lWaferMp.Y) * GV.ZoomLensInfo.LeftMotorStepsPerPixelY[GV.LeftZoomLens.Magnification];
            double yr = (rMaskMp.Y - rWaferMp.Y) * GV.ZoomLensInfo.RightMotorStepsPerPixelY[GV.RightZoomLens.Magnification];

            double xMotorSteps = (xl + xr) / 2;
            double yMotorSteps = (yl + yr) / 2;

            motorSteps[0] = (int)Math.Round(xMotorSteps);
            motorSteps[1] = (int)Math.Round(-yMotorSteps);
            motorSteps[2] = (int)Math.Round(rotateMotorSteps);
            return motorSteps;
        }

        private int[] CalcuteMaskWaferShiftAndReturnMotorStepsBack(MatchPosition lMaskMp, MatchPosition lWaferMp, MatchPosition rMaskMp, MatchPosition rWaferMp, double patternCenterDistance)
        {
            int[] motorSteps = new int[3];

            double ly = (lWaferMp.Y - lMaskMp.Y) * GV.ZoomLensInfo.LeftDownUmPerPixelY;
            double ry = (rWaferMp.Y - rMaskMp.Y) * GV.ZoomLensInfo.RightDownUmPerPixelY;
            double waferSin = (ly - ry) / patternCenterDistance;
            double waferRad = Math.Asin(waferSin);
            double waferDegree = waferSin * 180 / Math.PI;
            double rotateMotorSteps = waferDegree * GV.ZoomLensInfo.MotorStepsPerPixelDegree;
            double a, b, c;
            a = b = patternCenterDistance / 2;
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
                rWaferMp.X += (float)deltaX;
                rWaferMp.Y += (float)deltaY;
            }

            double xMotorStepsl = (lMaskMp.X - lWaferMp.X) * GV.ZoomLensInfo.LeftDownUmPerPixelX / GV.ZoomLensInfo.LeftDownUmPerStepX;
            if (GV.BaslerCamParm.LeftBackReserveX) xMotorStepsl *= -1;
            double xMotorStepsr = (rMaskMp.X - rWaferMp.X) * GV.ZoomLensInfo.RightDownUmPerPixelX / GV.ZoomLensInfo.RightDownUmPerStepX;
            if (GV.BaslerCamParm.RightBackReserveX) xMotorStepsr *= -1;
            double xMotorSteps = (xMotorStepsl + xMotorStepsr) / 2;
            double yMotorSteps = ((lMaskMp.Y - lWaferMp.Y) * GV.ZoomLensInfo.RightDownUmPerPixelY / GV.ZoomLensInfo.LeftDownUmPerStepY + (rMaskMp.Y - rWaferMp.Y) * GV.ZoomLensInfo.RightDownUmPerPixelY / GV.ZoomLensInfo.RightDownUmPerStepY) / 2;

            motorSteps[0] = (int)Math.Round(xMotorSteps);
            motorSteps[1] = (int)Math.Round(yMotorSteps);
            motorSteps[2] = (int)Math.Round(rotateMotorSteps);
            return motorSteps;
        }

        private int[] XyyMotorSteps(MatchPosition lMaskMp, MatchPosition lWaferMp, MatchPosition rMaskMp, MatchPosition rWaferMp)
        {
            int[] motorSteps = new int[3];

            double patternCenterDistancePixel = AlignC.PatternCenterDistanceUm / 2;

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

            double xMotorStepsl = (lMaskMp.X - lWaferMp.X) * GV.ZoomLensInfo.LeftDownUmPerPixelX / GV.ZoomLensInfo.StageMotorStepsPerumX;
            double xMotorStepsr = (rMaskMp.X - rWaferMp.X) * GV.ZoomLensInfo.RightDownUmPerPixelX / GV.ZoomLensInfo.StageMotorStepsPerumX;
            double xMotorSteps = (xMotorStepsl + xMotorStepsr) / 2;
            double yMotorStepsl = (lMaskMp.Y - lWaferMp.Y) * GV.ZoomLensInfo.LeftDownUmPerPixelY / GV.ZoomLensInfo.StageMotorStepsPerumY1;
            double yMotorStepsr = (rMaskMp.Y - rWaferMp.Y) * GV.ZoomLensInfo.RightDownUmPerPixelY / GV.ZoomLensInfo.StageMotorStepsPerumY1;
            double yMotorSteps = (yMotorStepsl + yMotorStepsr) / 2;

            motorSteps[0] = (int)Math.Round(xMotorSteps);
            motorSteps[1] = (int)Math.Round(yMotorSteps);
            motorSteps[2] = (int)Math.Round(rotateMotorSteps);
            return motorSteps;
        }

        private void MoveTableToMatchPattern()
        {
            for (int times = 1; times < AlignC.MaxAlignTimes + 1; times++)
            {
                CheckIfPlcStop();
                Thread.Sleep(200);
                writeStatus(GV.Dlang.strMessageAlign1 + times.ToString() + " ========");
               
                ProductMatchPositions pmps = GetAllMatchPostion(GV.LeftUpCam.Grab(), GV.RightUpCam.Grab(), "high");

                double beforeMoveLShiftXum = (pmps.LWaferMp.X - pmps.LMaskMp.X) * GV.ZoomLensInfo.LeftUmPerPixelX[AlignC.AlignHighMagnification];
                double beforeMoveLShiftYum = (pmps.LWaferMp.Y - pmps.LMaskMp.Y) * GV.ZoomLensInfo.LeftUmPerPixelY[AlignC.AlignHighMagnification];
                double beforeMoveRShiftXum = (pmps.RWaferMp.X - pmps.RMaskMp.X) * GV.ZoomLensInfo.RightUmPerPixelX[AlignC.AlignHighMagnification];
                double beforeMoveRShiftYum = (pmps.RWaferMp.Y - pmps.RMaskMp.Y) * GV.ZoomLensInfo.RightUmPerPixelY[AlignC.AlignHighMagnification];
                // writeStatus("L shift(um) : (" + beforeMoveLShiftXum.ToString("F2") + " , " + beforeMoveLShiftYum.ToString("F2") + ")");
                // writeStatus("R shift(um) : (" + beforeMoveRShiftXum.ToString("F2") + " , " + beforeMoveRShiftYum.ToString("F2") + ")");
                writeStatus(string.Format(GV.Dlang.strMessageLShiftum, beforeMoveLShiftXum.ToString("F2"), beforeMoveLShiftYum.ToString("F2")));
                writeStatus(string.Format(GV.Dlang.strMessageRShiftum, beforeMoveRShiftXum.ToString("F2"), beforeMoveRShiftYum.ToString("F2")));

                if (pmps.LWaferMp.Score < AlignC.WaferScore && pmps.RWaferMp.Score < AlignC.WaferScore)
                {
                    GV.Plc.SendAlignNG();
                    iStartAlign = 0;
                    writeStatus(GV.Dlang.strErrorHighCantFindWafer);
                    return;
                    // throw new MRException("Error : Can't find wafer template at high magnification");
                }

                iAlignCode = GV.Plc.ReadData16(GV.Plc.iAlign, 4);
                if (iAlignCode[1] == 0)
                {
                    iStartAlign = 0;
                    return;
                }

                int[] motorSteps = CalcuteMaskWaferShiftAndReturnMotorSteps(pmps.LMaskMp, pmps.LWaferMp, pmps.RMaskMp, pmps.RWaferMp, pmps.PatternCenterDistance);
                GV.Plc.AlignXyyTableMove(motorSteps[0], motorSteps[1], motorSteps[2]);
                Thread.Sleep(500);
                writeStatus(GV.Dlang.strMessageTableStep + " : " + motorSteps[0].ToString() + " , " + motorSteps[1].ToString() + " , " + motorSteps[2].ToString());

                if (times < AlignC.MinAlignTimes) continue;

                AlignResultData alignResultData = new AlignResultData();
                if (CheckAlignConditions(CheckWaferMatchPostion(GV.LeftUpCam.Grab(), GV.RightUpCam.Grab()), out alignResultData))
                {
                    motorSteps = CalcuteMaskWaferShiftAndReturnMotorStepsCenter(pmps.LMaskMp, pmps.LWaferMp, pmps.RMaskMp, pmps.RWaferMp, pmps.PatternCenterDistance);
                    GV.Plc.AlignXyyTableMove(motorSteps[0], motorSteps[1], motorSteps[2]);
                    writeStatus(GV.Dlang.strMessageTableStep + " : " + motorSteps[0].ToString() + " , " + motorSteps[1].ToString() + " , " + motorSteps[2].ToString());
                    iAlignOK = 1;
                    writeStatus(GV.Dlang.strMessageStage1done);
                    return;
                }
            }
            GV.Plc.SendAlignNG();
            iAlignOK = 0;
            iStartAlign = 0;
            writeStatus(GV.Dlang.strErrorMaxAlign);
            // throw new MRException("Error : Over max align times");
        }

        private void MoveTableToMatchPatternBack()
        {
            //bMoveLShiftXum = 0;
            //bMoveLShiftYum = 0;
            //bMoveRShiftXum = 0;
            //bMoveRShiftYum = 0;

            for (int times = 1; times < AlignC.MaxAlignTimes + 1; times++)
            {
                GV.TickCount = 0;

                iAlignCode = GV.Plc.ReadData16(GV.Plc.iAlign, 4);
                if (iAlignCode[1] == 0)
                {
                    iStartAlign = 0;
                    iAlignOK = 0;
                    return;
                }

                writeStatus(GV.Dlang.strMessageBottomAlign1 + times.ToString() + " ========");
                Thread.Sleep(500);
                ProductMatchPositions pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "back");

                double beforeMoveLShiftXum = (pmps.LWaferMp.X - pmps.LMaskMp.X) * GV.ZoomLensInfo.LeftDownUmPerPixelX;
                double beforeMoveLShiftYum = (pmps.LWaferMp.Y - pmps.LMaskMp.Y) * GV.ZoomLensInfo.LeftDownUmPerPixelY;
                double beforeMoveRShiftXum = (pmps.RWaferMp.X - pmps.RMaskMp.X) * GV.ZoomLensInfo.RightDownUmPerPixelX;
                double beforeMoveRShiftYum = (pmps.RWaferMp.Y - pmps.RMaskMp.Y) * GV.ZoomLensInfo.RightDownUmPerPixelY;
                writeStatus(string.Format(GV.Dlang.strMessageLShiftum, beforeMoveLShiftXum.ToString("F2"), beforeMoveLShiftYum.ToString("F2")));
                writeStatus(string.Format(GV.Dlang.strMessageRShiftum, beforeMoveRShiftXum.ToString("F2"), beforeMoveRShiftYum.ToString("F2")));

                if (pmps.LWaferMp.Score < AlignC.WaferScore || pmps.RWaferMp.Score < AlignC.WaferScore)
                {
                    writeStatus(string.Format(GV.Dlang.strMessageLeftMaskWaferScore, pmps.LMaskMp.Score.ToString("F2"), pmps.LWaferMp.Score.ToString("F2")));
                    writeStatus(string.Format(GV.Dlang.strMessageRightMaskWaferScore, pmps.RMaskMp.Score.ToString("F2"), pmps.RWaferMp.Score.ToString("F2")));
                    GV.Plc.SendAlignBackNG();
                    iAlignOK = 0;
                    writeStatus(GV.Dlang.strErrorCantBottomWafer);
                    return;
                }

                GV.Plc.GetLocation();
                DebugMessage(GV.Dlang.strMessageTableStep + GV.NowLocation[5].ToString() + " , " + GV.NowLocation[6].ToString() + " , " + GV.NowLocation[7].ToString());

                int[] motorSteps = XyyMotorSteps(pmps.LMaskMp, pmps.LWaferMp, pmps.RMaskMp, pmps.RWaferMp);

                iAlignCode = GV.Plc.ReadData16(GV.Plc.iAlign, 4);
                if (iAlignCode[1] == 0)
                {
                    iStartAlign = 0;
                    iAlignOK = 0;
                    return;
                }

                //bMoveLShiftXum = beforeMoveLShiftXum;
                //bMoveLShiftYum = beforeMoveLShiftYum;
                //bMoveRShiftXum = beforeMoveRShiftXum;
                //bMoveRShiftYum = beforeMoveRShiftYum;

                //bMotor[0] = motorSteps[0];
                //bMotor[1] = motorSteps[1];
                //bMotor[2] = motorSteps[2];

                GV.Plc.AlignXyyTableMove(false, motorSteps[0], motorSteps[1], motorSteps[2]);
                writeStatus(GV.Dlang.strMessageTableStep + motorSteps[0].ToString() + " , " + motorSteps[1].ToString() + " , " + motorSteps[2].ToString());
                Thread.Sleep(500);
                if (times < AlignC.MinAlignTimes)
                {
                    continue;
                }
                Thread.Sleep(500);

                AlignResultData alignResultData = new AlignResultData();
                if (CheckAlignConditionsBack(CheckWaferMatchPostionBack(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab()), out alignResultData))
                {
                    iAlignOK = 1;

                    writeStatus(GV.Dlang.strMessageStage1done);
                    return;
                }
            }
            GV.Plc.SendAlignBackNG();
            writeStatus(GV.Dlang.strErrorMaxAlign);
            iAlignOK = 0;
            iStartAlign = 0;
        }

        private int MoveTableToMatchPatternAfterContactShift()
        {
            if (!GV.AppSettingParm.EnableContactShiftCompensation)
            {

                GV.Plc.TellPlcToEnterContactUnContactMode();
                //GV.Plc.Contact(1);
                //GV.Plc.Exposure(1);
                iExposure = 1;
                return 0;
            }

            for (int times = 1; times < AlignC.MaxAlignTimes + 1; times++)
            {
                CheckIfPlcStop();

                writeStatus(GV.Dlang.strMessageAlgin2 + times.ToString() + " ========");

                GV.Plc.Contact(1);

                Thread.Sleep(500);
                ProductMatchPositions pmps = GetAllMatchPostion(GV.LeftUpCam.Grab(), GV.RightUpCam.Grab(), "high");

                if (pmps.LMaskMp.Score < AlignC.MaskSocre || pmps.RMaskMp.Score < AlignC.MaskSocre)
                {
                    GV.Plc.ContactFault(1);
                    GV.Plc.SendAlignNG();
                    iStartAlign = 0;
                    writeStatus(string.Format(GV.Dlang.strMessageLeftMaskWaferScore, pmps.LMaskMp.Score.ToString("F2"), pmps.LWaferMp.Score.ToString("F2")));
                    writeStatus(string.Format(GV.Dlang.strMessageRightMaskWaferScore, pmps.RMaskMp.Score.ToString("F2"), pmps.RWaferMp.Score.ToString("F2")));
                    writeStatus(GV.Dlang.strErrorHighCantFindMask);
                    return 1;
                }
                if (pmps.LWaferMp.Score < AlignC.WaferScore || pmps.RWaferMp.Score < AlignC.WaferScore)
                {
                    GV.Plc.ContactFault(1);
                    GV.Plc.SendAlignNG();
                    iStartAlign = 0;
                    writeStatus(string.Format(GV.Dlang.strMessageLeftMaskWaferScore, pmps.LMaskMp.Score.ToString("F2"), pmps.LWaferMp.Score.ToString("F2")));
                    writeStatus(string.Format(GV.Dlang.strMessageRightMaskWaferScore, pmps.RMaskMp.Score.ToString("F2"), pmps.RWaferMp.Score.ToString("F2")));
                    writeStatus(GV.Dlang.strErrorHighCantFindWafer);
                    return 1;
                }

                double beforeMoveLShiftXum = (pmps.LWaferMp.X - pmps.LMaskMp.X) * GV.ZoomLensInfo.LeftUmPerPixelX[AlignC.AlignHighMagnification];
                double beforeMoveLShiftYum = (pmps.LWaferMp.Y - pmps.LMaskMp.Y) * GV.ZoomLensInfo.LeftUmPerPixelY[AlignC.AlignHighMagnification];
                double beforeMoveRShiftXum = (pmps.RWaferMp.X - pmps.RMaskMp.X) * GV.ZoomLensInfo.RightUmPerPixelX[AlignC.AlignHighMagnification];
                double beforeMoveRShiftYum = (pmps.RWaferMp.Y - pmps.RMaskMp.Y) * GV.ZoomLensInfo.RightUmPerPixelY[AlignC.AlignHighMagnification];

                if (times > AlignC.MinAlignTimes)
                {
                    AlignResultData alignResultData = new AlignResultData();
                    if (CheckAlignConditions(pmps, out alignResultData))
                    {
                        GM.ModelToCSV("result.csv", alignResultData);
                        writeStatus(GV.Dlang.strMessageStage2done);
                        GV.Plc.Exposure(1);
                        iExposure = 1;
                        return 0;
                    }
                }

                if (times == AlignC.MaxAlignTimes) break;

                bool bUnc = GV.Plc.Uncontact(1);
                if (GV.AppSettingParm.DebugMode)
                {
                    if (bUnc)
                    {
                        writeStatus("UnContact OK !");
                    }
                    else
                    {
                        writeStatus("UnContact Fault!");
                    }
                }

                int[] motorSteps = CalcuteMaskWaferShiftAndReturnMotorSteps(pmps.LMaskMp, pmps.LWaferMp, pmps.RMaskMp, pmps.RWaferMp, pmps.PatternCenterDistance);
                if (GV.AppSettingParm.DebugMode)
                    writeStatus("Top Contact Move");
                
                writeStatus(GV.Dlang.strMessageTableStep + " : " + motorSteps[0].ToString() + " , " + motorSteps[1].ToString() + " , " + motorSteps[2].ToString());
                GV.Plc.AlignXyyTableMove(motorSteps[0], motorSteps[1], motorSteps[2]);
                Thread.Sleep(200);
            }

            GV.Plc.SendAlignNG();
            writeStatus(GV.Dlang.strErrorMaxAlign);
            return 1;
        }

        private int MoveTableToMatchPatternAfterContactShiftBack()
        {
            if (!GV.AppSettingParm.EnableContactShiftCompensation)
            {

                // GV.Plc.TellPlcToEnterContactUnContactMode();
                GV.Plc.Contact(1);
                GM.SaveImageBeforeExposure(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab());
                GV.Plc.Exposure(1);
                return 0;
            }

            for (int times = 1; times < AlignC.MaxAlignTimes + 1; times++)
            {
                GV.TickCount = 0;

                writeStatus(GV.Dlang.strMessageBottomAlign2 + times.ToString() + " ========");

                if (GV.AppSettingParm.DebugMode)
                    writeStatus("Contact 2");

                if (!GV.Plc.Contact(1))
                {
                    iStartAlign = 0;
                    iAlignOK = 0;
                    GV.Plc.SendAlignBackNG();
                    writeStatus("Contact Fault !!");
                    return 0;
                }

                Thread.Sleep(500);
                ProductMatchPositions pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "back");

                //pmps.LMaskMp.X = BLivePmps.LMaskMp.X;
                //pmps.LMaskMp.Y = BLivePmps.LMaskMp.Y;
                //pmps.LMaskMp.Score = BLivePmps.LMaskMp.Score;
                //pmps.RMaskMp.X = BLivePmps.RMaskMp.X;
                //pmps.RMaskMp.Y = BLivePmps.RMaskMp.Y;
                //pmps.RMaskMp.Score = BLivePmps.RMaskMp.Score;

                if (pmps.LMaskMp.Score < AlignC.MaskSocre || pmps.RMaskMp.Score < AlignC.MaskSocre)
                {
                    GV.Plc.SendAlignBackNG();
                    iStartAlign = 0;
                    iAlignOK = 0;
                    writeStatus(string.Format(GV.Dlang.strMessageLeftMaskWaferScore, pmps.LMaskMp.Score.ToString("F2"), pmps.LWaferMp.Score.ToString("F2")));
                    writeStatus(string.Format(GV.Dlang.strMessageRightMaskWaferScore, pmps.RMaskMp.Score.ToString("F2"), pmps.RWaferMp.Score.ToString("F2")));
                    writeStatus(GV.Dlang.strErrorCantBottomMask);
                    return 0;
                }
                if (pmps.LWaferMp.Score < AlignC.WaferScore || pmps.RWaferMp.Score < AlignC.WaferScore)
                {
                    GV.Plc.SendAlignBackNG();
                    iStartAlign = 0;
                    iAlignOK = 0;
                    writeStatus(string.Format(GV.Dlang.strMessageLeftMaskWaferScore, pmps.LMaskMp.Score.ToString("F2"), pmps.LWaferMp.Score.ToString("F2")));
                    writeStatus(string.Format(GV.Dlang.strMessageRightMaskWaferScore, pmps.RMaskMp.Score.ToString("F2"), pmps.RWaferMp.Score.ToString("F2")));
                    writeStatus(GV.Dlang.strErrorCantBottomWafer);
                    return 0;
                }

                double beforeMoveLShiftXum = (pmps.LWaferMp.X - pmps.LMaskMp.X) * GV.ZoomLensInfo.LeftDownUmPerPixelX;
                double beforeMoveLShiftYum = (pmps.LWaferMp.Y - pmps.LMaskMp.Y) * GV.ZoomLensInfo.LeftDownUmPerPixelY;
                double beforeMoveRShiftXum = (pmps.RWaferMp.X - pmps.RMaskMp.X) * GV.ZoomLensInfo.RightDownUmPerPixelX;
                double beforeMoveRShiftYum = (pmps.RWaferMp.Y - pmps.RMaskMp.Y) * GV.ZoomLensInfo.RightDownUmPerPixelY;
                writeStatus(string.Format(GV.Dlang.strMessageLShiftum, beforeMoveLShiftXum.ToString("F2"), beforeMoveLShiftYum.ToString("F2")));
                writeStatus(string.Format(GV.Dlang.strMessageRShiftum, beforeMoveRShiftXum.ToString("F2"), beforeMoveRShiftYum.ToString("F2")));

                if (times > AlignC.MinAlignTimes)
                {
                    Thread.Sleep(500);
                    AlignResultData alignResultData = new AlignResultData();
                    if (CheckAlignConditionsBack(CheckWaferMatchPostionBack(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab()), out alignResultData))
                    {
                        GM.ModelToCSV("result.csv", alignResultData);
                        writeStatus(GV.Dlang.strMessageStage2done);

                        ProductMatchPositions _pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "backcheck");
                        _pmps.LMaskMp = BLivePmps.LMaskMp;
                        _pmps.RMaskMp = BLivePmps.RMaskMp;
                        double checkMoveLShiftXum = (_pmps.LWaferMp.X - _pmps.LMaskMp.X) * GV.ZoomLensInfo.LeftDownUmPerPixelX;
                        double checkMoveLShiftYum = (_pmps.LWaferMp.Y - _pmps.LMaskMp.Y) * GV.ZoomLensInfo.LeftDownUmPerPixelY;
                        double checkMoveRShiftXum = (_pmps.RWaferMp.X - _pmps.RMaskMp.X) * GV.ZoomLensInfo.RightDownUmPerPixelX;
                        double checkMoveRShiftYum = (_pmps.RWaferMp.Y - _pmps.RMaskMp.Y) * GV.ZoomLensInfo.RightDownUmPerPixelY;
                        writeStatus(string.Format(GV.Dlang.strMessageLShiftum, checkMoveLShiftXum.ToString("F2"), checkMoveLShiftYum.ToString("F2")));
                        writeStatus(string.Format(GV.Dlang.strMessageRShiftum, checkMoveRShiftXum.ToString("F2"), checkMoveRShiftYum.ToString("F2")));

                        GM.SaveImageBeforeExposure(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab());
                        GV.Plc.Exposure(2);
                        iAlignOK = 1;
                        return 0;
                    }
                }

                if (times == AlignC.MaxAlignTimes) break;

                if (GV.AppSettingParm.DebugMode)
                    writeStatus("UnContact 2");
                // GV.Plc.Uncontact(1);
                if (!GV.Plc.Uncontact(1))
                {
                    iStartAlign = 0;
                    iAlignOK = 0;
                    GV.Plc.SendAlignBackNG();
                    writeStatus("Uncontact Fault !!");
                    return 0;
                }

                GV.Plc.GetLocation();
                GM.DebugMessage("XYY Location : " + GV.NowLocation[5].ToString() + " , " + GV.NowLocation[6].ToString() + " , " + GV.NowLocation[7].ToString());

                int[] motorSteps = XyyMotorSteps(pmps.LMaskMp, pmps.LWaferMp, pmps.RMaskMp, pmps.RWaferMp);

                if (GV.AppSettingParm.DebugMode)
                    writeStatus("Contact Move");
                Thread.Sleep(500);
                GV.Plc.AlignXyyTableMove(false, motorSteps[0], motorSteps[1], motorSteps[2]);
                writeStatus(GV.Dlang.strMessageTableStep + motorSteps[0].ToString() + " , " + motorSteps[1].ToString() + " , " + motorSteps[2].ToString());
            }
            GV.Plc.SendAlignBackNG();
            iStartAlign = 0;
            iAlignOK = 0;
            writeStatus(GV.Dlang.strErrorMaxAlign);
            return 0;
            // Debug.WriteLine("Over max align times");
            // throw new MRException("Over max align times");
        }

        private async void ChangeAlignConditionValue(object sender, MouseEventArgs e)
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
                DialogCalculator cal = new DialogCalculator
                {
                    StartPosition = FormStartPosition.CenterScreen
                };
                cal.ShowDialog();
                if (cal.IsDouble) nud.Value = (decimal)cal.NumberDouble;
                if (cal.IsInt) nud.Value = (decimal)cal.NumberInt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MoveTableToMatchPatternAtSpecificMagnification(string alignMagnification)
        {
            bool ack = true;
            int iHeight;
            int iWidth;
            ProductMatchPositions pmps;

            //GV.Plc.ChangeXyyTableSpeed(1000);

            CheckIfPlcStop();
            Thread.Sleep(200);
            if (alignMagnification == "high")
            {
                if (AlignC.PatternShift == 0)
                {
                    iHeight = (_recipe.LeftHighMaskMat.Height + _recipe.LeftHighWaferMat.Height) / 2;
                    iHeight = (int)(iHeight * GV.ZoomLensInfo.LeftMotorStepsPerPixelY[GV.LeftZoomLens.Magnification]);
                    GV.Plc.AlignXyyTableMove(ack, 0, -iHeight, 0);
                    Thread.Sleep(200);
                }
                else if (AlignC.PatternShift == 1)
                {
                    iHeight = (_recipe.LeftHighMaskMat.Height + _recipe.LeftHighWaferMat.Height) / 2;
                    iHeight = (int)(iHeight * GV.ZoomLensInfo.LeftMotorStepsPerPixelY[GV.LeftZoomLens.Magnification]);
                    GV.Plc.AlignXyyTableMove(ack, 0, iHeight, 0);
                    Thread.Sleep(200);
                }
                else if (AlignC.PatternShift == 2)
                {
                    if (_recipe.LeftHighMaskMat.Width > _recipe.LeftHighWaferMat.Width)
                    {
                        iWidth = _recipe.LeftHighMaskMat.Width;
                    }
                    else
                    {
                        iWidth = _recipe.LeftHighWaferMat.Width;
                    }
                    iWidth = (int)(iWidth * GV.ZoomLensInfo.LeftMotorStepsPerPixelX[GV.LeftZoomLens.Magnification]);
                    GV.Plc.AlignXyyTableMove(ack, -iWidth, 0, 0);
                    Thread.Sleep(200);
                }
                else if (AlignC.PatternShift == 3)
                {
                    if (_recipe.LeftHighMaskMat.Width > _recipe.LeftHighWaferMat.Width)
                    {
                        iWidth = _recipe.LeftHighMaskMat.Width;
                    }
                    else
                    {
                        iWidth = _recipe.LeftHighWaferMat.Width;
                    }
                    iWidth = (int)(iWidth * GV.ZoomLensInfo.LeftMotorStepsPerPixelX[GV.LeftZoomLens.Magnification]);
                    GV.Plc.AlignXyyTableMove(ack, iWidth, 0, 0);
                    Thread.Sleep(200);
                }
                else
                {

                }
                //Thread.Sleep(500);
                iAlignCode = GV.Plc.ReadData16(GV.Plc.iAlign, 4);
                if (iAlignCode[1] == 0)
                {
                    iStartAlign = 0;
                    return;
                }
                
                pmps = GetAllMatchPostion(GV.LeftUpCam.Grab(), GV.RightUpCam.Grab(), alignMagnification);
                //Thread.Sleep(500);
                if (pmps.LMaskMp.Score < AlignC.MaskSocre || pmps.RMaskMp.Score < AlignC.MaskSocre)
                {
                    GV.Plc.SendAlignNG();
                    writeStatus(string.Format(GV.Dlang.strMessageLeftMaskWaferScore, pmps.LMaskMp.Score.ToString("F2"), pmps.LWaferMp.Score.ToString("F2")));
                    writeStatus(string.Format(GV.Dlang.strMessageRightMaskWaferScore, pmps.RMaskMp.Score.ToString("F2"), pmps.RWaferMp.Score.ToString("F2")));
                    writeStatus(GV.Dlang.strErrorHighCantFindMask);
                    iStartAlign = 0;
                    return;
                }
                LastPmps.LMaskMp = pmps.LMaskMp;
                LastPmps.RMaskMp = pmps.RMaskMp;
                writeStatus($"Mask位置: L({pmps.LMaskMp.X:F1},{pmps.LMaskMp.Y:F1}) R({pmps.RMaskMp.X:F1},{pmps.RMaskMp.Y:F1})");
                iMaskGetOK = 1;
            }
            else
            {
               
                pmps = GetAllMatchPostion(GV.LeftUpCam.Grab(), GV.RightUpCam.Grab(), alignMagnification);
                //Thread.Sleep(500);
            }

            writeStatus(string.Format(GV.Dlang.strMessageLeftMaskWaferScore, pmps.LMaskMp.Score.ToString("F2"), pmps.LWaferMp.Score.ToString("F2")));
            writeStatus(string.Format(GV.Dlang.strMessageRightMaskWaferScore, pmps.RMaskMp.Score.ToString("F2"), pmps.RWaferMp.Score.ToString("F2")));

            if (pmps.LWaferMp.Score < AlignC.WaferScore && pmps.RWaferMp.Score < AlignC.WaferScore)
            {
                GV.Plc.SendAlignNG();
                iStartAlign = 0;
                writeStatus(GV.Dlang.strErrorCantFindWafer);
                return;
            }

            double beforeMoveLShiftXum = (pmps.LWaferMp.X - pmps.LMaskMp.X) * GV.ZoomLensInfo.LeftUmPerPixelX[GV.LeftZoomLens.Magnification];
            double beforeMoveLShiftYum = (pmps.LWaferMp.Y - pmps.LMaskMp.Y) * GV.ZoomLensInfo.LeftUmPerPixelY[GV.LeftZoomLens.Magnification];
            double beforeMoveRShiftXum = (pmps.RWaferMp.X - pmps.RMaskMp.X) * GV.ZoomLensInfo.RightUmPerPixelX[GV.RightZoomLens.Magnification];
            double beforeMoveRShiftYum = (pmps.RWaferMp.Y - pmps.RMaskMp.Y) * GV.ZoomLensInfo.RightUmPerPixelY[GV.RightZoomLens.Magnification];

            writeStatus(string.Format(GV.Dlang.strMessageLShiftum, beforeMoveLShiftXum.ToString("F2"), beforeMoveLShiftYum.ToString("F2")));
            writeStatus(string.Format(GV.Dlang.strMessageRShiftum, beforeMoveRShiftXum.ToString("F2"), beforeMoveRShiftYum.ToString("F2")));

            int[] motorSteps = CalcuteMaskWaferShiftAndReturnMotorSteps(pmps.LMaskMp, pmps.LWaferMp, pmps.RMaskMp, pmps.RWaferMp, pmps.PatternCenterDistance);
            //Thread.Sleep(500);
            iAlignCode = GV.Plc.ReadData16(GV.Plc.iAlign, 4);
            if (iAlignCode[1] == 0)
            {
                iStartAlign = 0;
                return;
            }
            GV.Plc.AlignXyyTableMove(motorSteps[0], motorSteps[1], motorSteps[2]);
            Thread.Sleep(500);
            writeStatus(GV.Dlang.strMessageTableStep + " : " + motorSteps[0].ToString() + " , " + motorSteps[1].ToString() + " , " + motorSteps[2].ToString());
        }

        private bool MoveCamera(int alignMagnification)
        {
            int i;
            bool ack = true;
            ProductMatchPositions pmps = null;

            if (alignMagnification == AlignC.AlignLowMagnification)
            {
                for (i = 0; i < 2; i++)
                {
                    Thread.Sleep(500);
                    pmps = GetAllMatchPostion(GV.LeftUpCam.Grab(), GV.RightUpCam.Grab(), "low");

                    writeStatus(string.Format(GV.Dlang.strMessageLeftMaskWaferScore, pmps.LMaskMp.Score.ToString("F2"), pmps.LWaferMp.Score.ToString("F2")));
                    writeStatus(string.Format(GV.Dlang.strMessageRightMaskWaferScore, pmps.RMaskMp.Score.ToString("F2"), pmps.RWaferMp.Score.ToString("F2")));

                    if (pmps.LMaskMp.Score < AlignC.MaskSocre || pmps.RMaskMp.Score < AlignC.MaskSocre)
                    {
                        iAlignCode = GV.Plc.ReadData16(GV.Plc.iAlign, 4);
                        if (iAlignCode[1] == 0)
                        {
                            iStartAlign = 0;
                            return false;
                        }
                        int iHeight;
                        if (_recipe.LeftLowMaskMat.Height > _recipe.LeftLowWaferMat.Height)
                        {
                            iHeight = _recipe.LeftLowMaskMat.Height;
                        }
                        else
                        {
                            iHeight = _recipe.LeftLowWaferMat.Height;
                        }
                        iHeight = (int)(iHeight * GV.ZoomLensInfo.LeftMotorStepsPerPixelY[AlignC.AlignLowMagnification]);
                        GV.Plc.AlignXyyTableMove(ack, 0, iHeight, 0);
                        Thread.Sleep(200);
                    }
                    else
                    {
                        break;
                    }
                }
                if (i == 2)
                {
                    // GV.Plc.Send("WR DM6400.S 1");
                    Mat leftFailImage = GV.LeftUpCam.Grab();
                    Mat rightFailImage = GV.RightUpCam.Grab();
                    SaveAlignFailImage(leftFailImage, rightFailImage, "UpMaskFail", saveOnce: true);

                    GV.Plc.SendAlignNG();
                    writeStatus(GV.Dlang.strErrorCantFindMask);
                    iStartAlign = 0;
                    return false;
                    // Debug.WriteLine("Error : Can't find mask");
                    // throw new MRException("Error : Can't find mask");
                }
            }
            else
            {
                pmps = GetAllMatchPostion(GV.LeftUpCam.Grab(), GV.RightUpCam.Grab(), "high");
                //Thread.Sleep(800);
            }

            PointF center = GV.UpCenter;

            double lOffsetXSteps = ((double)(center.X) - pmps.LMaskMp.X) * GV.ZoomLensInfo.LeftCameraMotorStepsPerPixelX[alignMagnification];
            double lOffsetYSteps = ((double)(center.Y) - pmps.LMaskMp.Y) * GV.ZoomLensInfo.LeftCameraMotorStepsPerPixelY[alignMagnification];
            double rOffsetXSteps = ((double)(center.X) - pmps.RMaskMp.X) * GV.ZoomLensInfo.RightCameraMotorStepsPerPixelX[alignMagnification];
            double rOffsetYSteps = ((double)(center.Y) - pmps.RMaskMp.Y) * GV.ZoomLensInfo.RightCameraMotorStepsPerPixelY[alignMagnification];

            iAlignCode = GV.Plc.ReadData16(GV.Plc.iAlign, 4);
            if (iAlignCode[1] == 0)
            {
                iStartAlign = 0;
                return false;
            }

            GV.Plc.AlignCameraMove(0, -lOffsetXSteps, rOffsetXSteps, -lOffsetYSteps, -rOffsetYSteps);
            Thread.Sleep(800);
            GV.Plc.GetLocation();
            Thread.Sleep(200);
            AlignC.PatternCenterDistanceUm = GV.ZoomLensInfo.LRUpCenterDistance
                - GV.NowLocation[GV.Plc.iiDUpLeftX] / 10 - GV.NowLocation[GV.Plc.iiDUpRightX] / 10;

            return true;
        }

        private bool MoveCameraMaskUp(string Magni)
        {
            int i = (Magni == "low") ? AlignC.AlignLowMagnification : AlignC.AlignHighMagnification;

            ProductMatchPositions pmps = GetAllMatchPostion(GV.LeftUpCam.Grab(), GV.RightUpCam.Grab(), Magni);

            if ((pmps.LMaskMp.Score < AlignC.MaskSocre) || (pmps.RMaskMp.Score < AlignC.MaskSocre))
            {
                //MaskPmps = pmps;
                writeStatus("L Mask / R Mask score : " + pmps.LMaskMp.Score.ToString("F2") + " , " + pmps.RMaskMp.Score.ToString("F2"));
                //writeStatus(GV.Dlang.strMessageMaskScore + pmps.LMaskMp.Score.ToString("F2") + " , " + pmps.RMaskMp.Score.ToString("F2"));
                // 
                if (Magni == "low")
                {
                    writeStatus(GV.Dlang.strErrorLowCantFindMask);
                }
                else
                {
                    writeStatus(GV.Dlang.strErrorHighCantFindMask);
                }
                GV.Plc.WriteMemory(GV.Plc.iBackMaskNG, true);
                iAlignOK = 0;
                return false;
            }

            PointF center = new System.Drawing.Point(GV.LeftUpCam.Grab().Cols / 2, GV.LeftUpCam.Grab().Rows / 2);

            double lOffsetXSteps = ((double)(center.X) - pmps.LMaskMp.X) * GV.ZoomLensInfo.LeftCameraMotorStepsPerPixelX[i];
            double lOffsetYSteps = ((double)(center.Y) - pmps.LMaskMp.Y) * GV.ZoomLensInfo.LeftCameraMotorStepsPerPixelY[i];
            double rOffsetXSteps = ((double)(center.X) - pmps.RMaskMp.X) * GV.ZoomLensInfo.RightCameraMotorStepsPerPixelX[i];
            double rOffsetYSteps = ((double)(center.Y) - pmps.RMaskMp.Y) * GV.ZoomLensInfo.RightCameraMotorStepsPerPixelY[i];

            if (GV.BaslerCamParm.LeftUpReserveX)
            {
                lOffsetXSteps *= -1;
            }

            if (GV.BaslerCamParm.RightUpReserveX)
            {
                rOffsetXSteps *= -1;
            }

            if (!GV.Plc.ReadMemory(GV.Plc.iDownCCDStart))
            {
                iStartAlign = 0;
                iAlignOK = 0;
                return false;
            }

            bool brt = GV.Plc.AlignCameraMove(1, lOffsetXSteps, -rOffsetXSteps, -lOffsetYSteps, -rOffsetYSteps);
            if (brt)
            {
                iAlignOK = 1;
                if (Magni == "high")
                {
                    Thread.Sleep(500);
                    LeftUpMask = GV.LeftUpCam.Grab();
                    RightUpMask = GV.RightUpCam.Grab();
                    pmps = GetAllMatchPostion(LeftUpMask, RightUpMask, "checkmask");
                    UpMaskPmps.LMaskMp.X = pmps.LMaskMp.X;
                    UpMaskPmps.LMaskMp.Y = pmps.LMaskMp.Y;
                    UpMaskPmps.LMaskMp.Score = pmps.LMaskMp.Score;
                    UpMaskPmps.RMaskMp.X = pmps.RMaskMp.X;
                    UpMaskPmps.RMaskMp.Y = pmps.RMaskMp.Y;
                    UpMaskPmps.RMaskMp.Score = pmps.RMaskMp.Score;
                    // _LMaskMp.X = UpMaskPmps.LMaskMp.X;
                    // _LMaskMp.Y = UpMaskPmps.LMaskMp.Y;
                    // _LMaskMp.Score = UpMaskPmps.LMaskMp.Score;
                    // _RMaskMp.X = UpMaskPmps.RMaskMp.X;
                    // _RMaskMp.Y = UpMaskPmps.RMaskMp.Y;
                    // _RMaskMp.Score = UpMaskPmps.RMaskMp.Score;
                    iUpMaskPmps = 1;
                }
                return true;
            }
            else
            {
                writeStatus(GV.Dlang.strMessageCameraMoveError);
                iAlignOK = 0;
                return false;
            }
        }

        private void ChangeUpDown(int iUpDown, int iMaskWafer)
        {
            int iLb = 0;
            int iRb = 0;

            if (iUpDown == 1)
            {
                GV.skView = 0;
                GV.LeftUpCam.Freeze();
                GV.RightUpCam.Freeze();
                GV.LeftBackCam.Freeze();
                GV.RightBackCam.Freeze();
                Invoke((MethodInvoker)delegate ()
                {
                    iShowMask = (MaskImage.Checked) ? 1 : 0;
                    lbLeftCCD.Text = GV.Dlang.strTopCCD;
                    lbRightCCD.Text = GV.Dlang.strTopCCD;
                });
                Thread.Sleep(SleepTime);
                if (GV.TabOption == GV.Tab.Learn)
                {
                    GV.LeftUpCam.SetWindow(GV.LeftUpWindowOnLearnPage);
                    GV.RightUpCam.SetWindow(GV.RightUpWindowOnLearnPage);
                }
                else if (GV.TabOption == GV.Tab.BackLearn)
                {
                    GV.LeftUpCam.SetWindow(GV.BackLearnLeft);
                    GV.RightUpCam.SetWindow(GV.BackLearnRight);
                }
                else
                {
                    GV.LeftUpCam.SetWindow(GV.LeftUpWindowOnAlignPage);
                    GV.RightUpCam.SetWindow(GV.RightUpWindowOnAlignPage);
                }

                skLeftAlign.SetShowMask(false, 0, LeftMask);
                skRightAlign.SetShowMask(false, 0, RightMask);
                GV.Light.ChangeBrightness(1, 0, 0);
                GV.RingLight.ChangeBrightness(0, 0, 0);
                GV.LeftUpCam.Live();
                GV.RightUpCam.Live();
            }
            else if (iUpDown == 2)
            {
                GV.skView = 1;
                GV.LeftUpCam.Freeze();
                GV.RightUpCam.Freeze();
                GV.LeftBackCam.Freeze();
                GV.RightBackCam.Freeze();
                Thread.Sleep(SleepTime);
                GV.LeftBackCam.SetWindow(GV.LeftDownWindowOnAlignPage);
                GV.RightBackCam.SetWindow(GV.RightDownWindowOnAlignPage);

                Invoke((MethodInvoker)delegate ()
                {
                    lbLeftCCD.Text = GV.Dlang.strBottomCCD;
                    lbRightCCD.Text = GV.Dlang.strBottomCCD;
                    // MaskImage.Visible = true;
                    if (iShowMask == 1)
                    {
                        MaskImage.Checked = true;
                        // skLeftAlign.SetShowMask(true, AlignC.dLalpha, LeftMask);
                        // skRightAlign.SetShowMask(true, AlignC.dRalpha, RightMask);
                    }
                });
                GV.LeftBackCam.Live();
                GV.RightBackCam.Live();
                if (iMaskWafer == 1)
                {
                    GV.NowWaferMask = 1;
                    iLb = AlignC.LBMaskBright;
                    iRb = AlignC.RBMaskBright;
                }
                else if (iMaskWafer == 2)
                {
                    GV.NowWaferMask = 2;
                    iLb = AlignC.LBWaferBright;
                    iRb = AlignC.RBWaferBright;
                }
                GV.Light.ChangeBrightness(1, iRb, iLb);
                GV.Light.ChangeBrightness(0, 0, 0);
                GV.RingLight.ChangeBrightness(0, 0, 0);

                // Thread drawAlignMatch = new Thread(DrawDownAlginMatchPosition);
                // drawAlignMatch.Start();

                // bDown = true;
            }
            Thread.Sleep(250);
        }

        private bool MoveCameraBack(int iMask)
        {

            bool bRet = false;

            GV.Plc.GetRecipe();

            //int lmz = GV.Plc.RecipeData[34];
            //int rmz = GV.Plc.RecipeData[35];
            //int lwz = GV.Plc.RecipeData[36];
            //int rwz = GV.Plc.RecipeData[37];
            int lmz = GV.Plc.RecipeData[43];
            int rmz = GV.Plc.RecipeData[44];
            int lwz = GV.Plc.RecipeData[45];
            int rwz = GV.Plc.RecipeData[46];


            ProductMatchPositions pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "backmask");
            Thread.Sleep(500);
            //ProductMatchPositions pmps = GetAllMatchPostion(LeftMask, RightMask, "backmask");
            if ((pmps.LMaskMp.Score < AlignC.MaskSocre) || (pmps.RMaskMp.Score < AlignC.MaskSocre))
            {
                Mat leftFailImage = GV.LeftBackCam.Grab();
                Mat rightFailImage = GV.RightBackCam.Grab();
                SaveAlignFailImage(leftFailImage, rightFailImage, "DownMaskFail", saveOnce: true);
                writeStatus("Bottom L Mask / R Mask score : " + pmps.LMaskMp.Score.ToString("F2") + " , " + pmps.RMaskMp.Score.ToString("F2"));
                GV.Plc.WriteMemory(GV.Plc.iBackMaskNG, true);
                iAlignOK = 0;
                writeStatus(GV.Dlang.strErrorCantBottomMask);
                return false;
            }


            PointF center = GV.DownCenter;

            double lOffsetXSteps = ((double)(center.X) - pmps.LMaskMp.X) * GV.ZoomLensInfo.LeftDownUmPerPixelX / GV.ZoomLensInfo.LeftDownUmPerStepX;
            double lOffsetYSteps = ((double)(center.Y) - pmps.LMaskMp.Y) * GV.ZoomLensInfo.LeftDownUmPerPixelY / GV.ZoomLensInfo.LeftDownUmPerStepY;
            double rOffsetXSteps = ((double)(center.X) - pmps.RMaskMp.X) * GV.ZoomLensInfo.RightDownUmPerPixelX / GV.ZoomLensInfo.RightDownUmPerStepX;
            double rOffsetYSteps = ((double)(center.Y) - pmps.RMaskMp.Y) * GV.ZoomLensInfo.RightDownUmPerPixelY / GV.ZoomLensInfo.RightDownUmPerStepY;

            //double lz = (lwz - lmz) / 3;
            //double rz = (rwz - rmz) / 3;
            double lz = (lwz - lmz) / 2;
            double rz = (rwz - rmz) / 2;

            if (AlignC.AdjuestZ == 1)
            {
                bRet = GV.Plc.AlignDownCameraMove(0, -lOffsetXSteps, rOffsetXSteps, -lOffsetYSteps, -rOffsetYSteps);
            }
            else
            {
                bRet = GV.Plc.AlignDownCameraMoveZ(0, -lOffsetXSteps, rOffsetXSteps, -lOffsetYSteps, -rOffsetYSteps, lz, rz);
                Thread.Sleep(1000);
                //bRet = GV.Plc.AlignDownCameraMove(0, -lOffsetXSteps, rOffsetXSteps, -lOffsetYSteps, -rOffsetYSteps);
            }

            if (!bRet)
            {
                writeStatus("Do Mask Picture faile Step 2!");
                iAlignOK = 0;
                return bRet;
            }
            Thread.Sleep(1000);

            pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "backmask");

            lOffsetXSteps = ((double)(center.X) - pmps.LMaskMp.X) * GV.ZoomLensInfo.LeftDownUmPerPixelX / GV.ZoomLensInfo.LeftDownUmPerStepX;
            lOffsetYSteps = ((double)(center.Y) - pmps.LMaskMp.Y) * GV.ZoomLensInfo.LeftDownUmPerPixelY / GV.ZoomLensInfo.LeftDownUmPerStepY;
            rOffsetXSteps = ((double)(center.X) - pmps.RMaskMp.X) * GV.ZoomLensInfo.RightDownUmPerPixelX / GV.ZoomLensInfo.RightDownUmPerStepX;
            rOffsetYSteps = ((double)(center.Y) - pmps.RMaskMp.Y) * GV.ZoomLensInfo.RightDownUmPerPixelY / GV.ZoomLensInfo.RightDownUmPerStepY;

            bRet = GV.Plc.AlignDownCameraMove(0, -lOffsetXSteps, rOffsetXSteps, -lOffsetYSteps, -rOffsetYSteps);
            //bRet = GV.Plc.AlignDownCameraMove(0, -lOffsetXSteps, -rOffsetXSteps, -lOffsetYSteps, -rOffsetYSteps);
            Thread.Sleep(1000);
            if (!bRet)
            {
                iAlignOK = 0;
                writeStatus("Do Mask Picture faile Step 3!");
                return bRet;
            }


            pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "backmask");

            if ((pmps.LMaskMp.Score < AlignC.MaskSocre) || (pmps.RMaskMp.Score < AlignC.MaskSocre))
            {
                Mat leftFailImage2 = GV.LeftBackCam.Grab();
                Mat rightFailImage2 = GV.RightBackCam.Grab();
                SaveAlignFailImage(leftFailImage2, rightFailImage2, "DownMaskFail", saveOnce: true);
                writeStatus("Bottom L Mask / R Mask score : " + pmps.LMaskMp.Score.ToString("F2") + " , " + pmps.RMaskMp.Score.ToString("F2"));
                GV.Plc.WriteMemory(GV.Plc.iBackMaskNG, true);
                writeStatus(GV.Dlang.strErrorCantBottomMask);
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

            // if ((precisionX < AlignC.XPrecision ) && (precisionY < AlignC.YPrecision) && (expansion < AlignC.MaxExpansion))
            if (!((pmps.LMaskMp.Score < AlignC.MaskSocre) || (pmps.RMaskMp.Score < AlignC.MaskSocre)))
            {
                Thread.Sleep(250);
                LeftMask = GV.LeftBackCam.Grab();
                RightMask = GV.RightBackCam.Grab();
                //GV.LeftMaskMat = LeftMask.Clone();
                //GV.RightMaskMat = RightMask.Clone();
                pmps = GetAllMatchPostion(LeftMask, RightMask, "backmask");

                //TopMaskPmps.LMaskMp.X = pmps.LMaskMp.X;
                //TopMaskPmps.LMaskMp.Y = pmps.LMaskMp.Y;
                //TopMaskPmps.LMaskMp.Score = pmps.LMaskMp.Score;
                //TopMaskPmps.RMaskMp.X = pmps.RMaskMp.X;
                //TopMaskPmps.RMaskMp.Y = pmps.RMaskMp.Y;
                //TopMaskPmps.RMaskMp.Score = pmps.RMaskMp.Score;
                BLastPmps.LMaskMp.X = pmps.LMaskMp.X;
                BLastPmps.LMaskMp.Y = pmps.LMaskMp.Y;
                BLastPmps.LMaskMp.Score = pmps.LMaskMp.Score;
                BLastPmps.RMaskMp.X = pmps.RMaskMp.X;
                BLastPmps.RMaskMp.Y = pmps.RMaskMp.Y;
                BLastPmps.RMaskMp.Score = pmps.RMaskMp.Score;
                string TopMaskCheck3 = string.Format(GV.Dlang.strMessageMaskShift, BLastPmps.LMaskMp.X, BLastPmps.RMaskMp.X, BLastPmps.LMaskMp.Y, BLastPmps.RMaskMp.Y);
                DebugMessage("0:" + TopMaskCheck3);

                // CountDistance();
                // ReportPLCStatus();
                // DownCCDMove();

                GV.Plc.GetLocation();

                AlignC.PatternCenterDistanceUm = GV.ZoomLensInfo.LRBackCenterDistance
                    - GV.NowLocation[GV.Plc.iiDDownLeftX] / 10 - GV.NowLocation[GV.Plc.iiDDownRightX] / 10;

                DebugMessage("Back Mask Pattern Center Distance um = " + AlignC.PatternCenterDistanceUm.ToString());

                if (iMask == 0)
                {
                    // GV.Plc.WriteMemory(GV.Plc.iBackMaskOK, true);
                    GV.Plc.WriteData16(GV.Plc.iMaskMemory, 5);
                }
            }
            else
            {
                writeStatus("Bottom L Mask / R Mask score : " + pmps.LMaskMp.Score.ToString("F2") + " , " + pmps.RMaskMp.Score.ToString("F2"));
                // writeStatus("Bottom precisionX : {0} precisionY : {1} degree : {2} exp : {3}", precisionX.)
                GV.Plc.WriteMemory(GV.Plc.iBackMaskNG, true);
                writeStatus(GV.Dlang.strErrorCantBottomMask);
                iAlignOK = 0;
                return false;
            }

            // GV.LeftMaskMat = LeftMask.Clone();
            // GV.RightMaskMat = RightMask.Clone();
            MaskShift(LeftMask, ref GV.LeftMaskMat);
            MaskShift(RightMask, ref GV.RightMaskMat);

            if (MaskImage.Checked)
            {
                skLeftAlign.SetShowMask(true, AlignC.dLalpha, LeftMask);
                skRightAlign.SetShowMask(true, AlignC.dRalpha, RightMask);
            }
            Cv2.ImWrite(GetTemplateFileName("LeftMask"), LeftMask);
            Cv2.ImWrite(GetTemplateFileName("RightMask"), RightMask);
            GV.NowWaferMask = 3;
            writeStatus("Do Mask Picture OK!");
            return true;
        }

        private void DownCCDMove()
        {
            int iLHigh = 0;
            int iRhigh = 0;
            int iLDistance = 0;
            int iRDistance = 0;

            writeStatus(String.Format("Recipe Down CCD High L {0} / R{1}", iLWaferHigh, iRWaferHigh));
            GV.Plc.AlignDownCameraMoveZAbs(0, 0, 0, 0, 0, iLWaferHigh, iRWaferHigh);
            // GV.Light.ChangeBrightness(0, 2, 7);
            // GV.Light.ChangeBrightness(1, 0, 0);

            // writeStatus(String.Format("Move Down CCD to Recipe L {0} / R {1}", iLWaferHigh - 600, iRWaferHigh - 600));
            // GV.Plc.AlignDownCameraMoveZAbs(0, 0, 0, 0, 0, iLWaferHigh - 3000, iRWaferHigh - 3000);
            Thread.Sleep(1000);
            ProductMatchPositions pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "backmask");
            if (pmps.LMaskMp.Score > GV.AppSettingParm.LMaskScore)
            {
                BLastPmps.LMaskMp.X = pmps.LMaskMp.X;
                BLastPmps.LMaskMp.Y = pmps.LMaskMp.Y;

                GM.WriteToStatusTextBox1(iAdmin, "Bottom L Mask Score: " + pmps.LMaskMp.Score.ToString("F2"));
                GM.WriteToStatusTextBox1(iAdmin, "Bottom L Mask X,Y Offset : " + pmps.LMaskMp.X.ToString("F2") + " , " + pmps.LMaskMp.Y.ToString("F2"));

            }
            else
            {
                GM.WriteToStatusTextBox1(iAdmin, "Bottom L Mask score : " + pmps.LMaskMp.Score.ToString("F2"));
                GM.WriteToStatusTextBox1(iAdmin, "Bottom L Mask X,Y Offset: " + pmps.LMaskMp.X.ToString("F2") + " , " + pmps.LMaskMp.Y.ToString("F2"));

                iLHigh = GV.Plc.iDownLHigh;

                iLDistance = iLHigh - iLWaferHigh;

                iLDistance /= 100;

                float tempLxOffset = (float)GV.LxOffset[iLDistance];
                float tempLyOffset = (float)GV.LyOffset[iLDistance];

                GM.WriteToStatusTextBox1(iAdmin, "Bottom L Mask iLdis, X, Y Offset: " + iLDistance.ToString() + " , " + tempLxOffset.ToString("F2") + " , " + tempLyOffset.ToString("F2"));

                BLastPmps.LMaskMp.X += tempLxOffset;
                BLastPmps.LMaskMp.Y += tempLyOffset;
                GM.WriteToStatusTextBox1(iAdmin, "BLast L Mask X, Y : " + BLastPmps.LMaskMp.X.ToString("F2") + " , " + BLastPmps.LMaskMp.Y.ToString("F2"));
            }

            if (pmps.RMaskMp.Score > GV.AppSettingParm.RMaskScore)
            {
                BLastPmps.RMaskMp.X = pmps.RMaskMp.X;
                BLastPmps.RMaskMp.Y = pmps.RMaskMp.Y;
                GM.WriteToStatusTextBox1(iAdmin, "Bottom R Mask score : " + pmps.RMaskMp.Score.ToString("F2"));
                GM.WriteToStatusTextBox1(iAdmin, "Bottom R Mask X,Y : " + pmps.RMaskMp.X.ToString("F2") + " , " + pmps.RMaskMp.Y.ToString("F2"));
            }
            else
            {
                GM.WriteToStatusTextBox1(iAdmin, "Bottom R Mask score : " + pmps.RMaskMp.Score.ToString("F2"));
                GM.WriteToStatusTextBox1(iAdmin, "Bottom R Mask X,Y Offset: " + pmps.RMaskMp.X.ToString("F2") + " , " + pmps.RMaskMp.Y.ToString("F2"));

                iRhigh = GV.Plc.iDownRHigh;

                iRDistance = iRhigh - iRWaferHigh;

                iRDistance /= 100;

                float tempRxOffset = (float)GV.RxOffset[iRDistance];
                float tempRyOffset = (float)GV.RyOffset[iRDistance];

                GM.WriteToStatusTextBox1(iAdmin, "Bottom R Mask iRdis, X, Y : " + iRDistance.ToString() + " , " + tempRxOffset.ToString("F2") + " , " + tempRyOffset.ToString("F2"));

                BLastPmps.RMaskMp.X += tempRxOffset;
                BLastPmps.RMaskMp.Y += tempRyOffset;

                GM.WriteToStatusTextBox1(iAdmin, "BLast R Mask X,Y Offset: " + BLastPmps.RMaskMp.X.ToString("F2") + " , " + BLastPmps.RMaskMp.Y.ToString("F2"));
            }

            MoveMaskMat();

            if (GV.UserLevel == GV.User.Administrator)
            {
                cmLearnPatternBack1.LeftMaskMp.X = BLastPmps.LMaskMp.X;
                cmLearnPatternBack1.LeftMaskMp.Y = BLastPmps.LMaskMp.Y;
                cmLearnPatternBack1.LeftMaskMp.Score = BLastPmps.LMaskMp.Score;
                cmLearnPatternBack1.RightMaskMp.X = BLastPmps.RMaskMp.X;
                cmLearnPatternBack1.RightMaskMp.Y = BLastPmps.RMaskMp.Y;
                cmLearnPatternBack1.RightMaskMp.Score = BLastPmps.RMaskMp.Score;
            }
        }

        private void MoveMaskMat()
        {
            float xs;
            float ys;

            tLeftMask = LeftMask.Clone();
            tRightMask = RightMask.Clone();

            // Image<Gray, Byte> tLimg = tLeftMask.ToImage<Gray, Byte>();
            // Image<Gray, Byte> tLdimg = LeftMask.ToImage<Gray, Byte>();
            // ImgTranslate(tLimg, tLdimg, xs, ys);
            // LeftMask = tLdimg.Mat;

            xs = (float)(BLastPmps.LMaskMp.X - TopMaskPmps.LMaskMp.X);
            ys = (float)(BLastPmps.LMaskMp.Y - TopMaskPmps.LMaskMp.Y);

            //Image<Gray, float> warpImageL = new Image<Gray, float>(new float[,,] { { { 1, 0, xs }, { 0, 1, ys } } });
            //Matrix<float> warpImageL = new Matrix<float>(new float[,] {
            //        { 1, 0, xs },
            //        { 0, 1, ys }
            //    });
            //Mat transMatL = warpImageL.Mat;
            //Cv2.WarpAffine(tLeftMask, LeftMask, transMatL, LeftMask.Size());
            float[,] warpMatrixL = new float[2, 3] { { 1, 0, xs }, { 0, 1, ys } };
            Mat transMatL = Mat.FromArray(warpMatrixL);
            Cv2.WarpAffine(tLeftMask, LeftMask, transMatL, LeftMask.Size());

            // Image<Gray, Byte> tRimg = tRightMask.ToImage<Gray, Byte>();
            // Image<Gray, Byte> tRdimg = RightMask.ToImage<Gray, Byte>();
            //ImgTranslate(tRimg, tRdimg, xs, ys);
            // RightMask = tRdimg.Mat;

            xs = (float)(BLastPmps.RMaskMp.X - TopMaskPmps.RMaskMp.X);
            ys = (float)(BLastPmps.RMaskMp.Y - TopMaskPmps.RMaskMp.Y);
            //Image<Gray, float> warpImageR = new Image<Gray, float>(new float[,,] { { { 1, 0, xs }, { 0, 1, ys } } });
            //Matrix<float> warpImageR = new Matrix<float>(new float[,] {
            //    { 1, 0, xs },
            //    { 0, 1, ys }
            //});

            //Mat transMatR = warpImageL.Mat;
            //Cv2.WarpAffine(tRightMask, RightMask, transMatR, RightMask.Size());
            float[,] warpMatrixR = new float[2, 3] { { 1, 0, xs }, { 0, 1, ys } };
            Mat transMatR = Mat.FromArray(warpMatrixR);
            Cv2.WarpAffine(tRightMask, RightMask, transMatR, RightMask.Size());
        }

        private void CountDistance()
        {
            //int i = 0;
            ProductMatchPositions pmps;

            GV.Light.ChangeBrightness(0, 10, 10);
            GV.Light.ChangeBrightness(1, 0, 0);

            for (int i = 0; i < 200; i++)
            {
                GV.Plc.AlignDownCameraMoveZ(0, 0, 0, 0, 0, -100, -100);
                Thread.Sleep(350);
                _LeftMask = GV.LeftBackCam.Grab();
                _RightMask = GV.RightBackCam.Grab();
                pmps = GetAllMatchPostion(_LeftMask, _RightMask, "backmask");
                // _LeftMask = skLeftAlign.GetWindowImage();
                // _RightMask = skRightAlign.GetWindowImage();
                Cv2.ImWrite(GetTemplateFileName("LeftBackMaski" + i.ToString()), _LeftMask);
                Cv2.ImWrite(GetTemplateFileName("RightBackMaski" + i.ToString()), _RightMask);
                double LMpX = pmps.LMaskMp.X - BLastPmps.LMaskMp.X;
                double LMpY = pmps.LMaskMp.Y - BLastPmps.LMaskMp.Y;
                double RMpX = pmps.RMaskMp.X - BLastPmps.RMaskMp.X;
                double RMpY = pmps.RMaskMp.Y - BLastPmps.RMaskMp.Y;

                writeStatus(String.Format("{4:0} : lx = {0:0.##} ly = {1:0.##} lscore = {5:0.##} rx = {2:0.##} ry = {3:0.##} rscore = {6:0.##}", LMpX, LMpY, RMpX, RMpY, i, pmps.LMaskMp.Score, pmps.RMaskMp.Score));
            }
            Thread.Sleep(5000);
        }

        //private ProductMatchPositions GetAllMatchPostion(Mat lSrc, Mat rSrc, string alignMmagnification)
        //{
        //    double patternCenterDistance = AlignC.PatternCenterDistanceUm;

        //    if (GV.AppSettingParm.Emulation != true)
        //    {
        //        if (alignMmagnification == "low")
        //        {
        //            GV.matcherLLM.MatMatchWithAlgo(0, lSrc.Clone(), ref lMaskMp, AlignC.LLMaskAlgorithm);
        //            GV.matcherRLM.MatMatchWithAlgo(0, rSrc.Clone(), ref rMaskMp, AlignC.RLMaskAlgorithm);
        //            GV.matcherLLW.MatMatchWithAlgo(0, lSrc.Clone(), ref lWaferMp, AlignC.LLWaferAlgorithm);
        //            GV.matcherRLW.MatMatchWithAlgo(0, rSrc.Clone(), ref rWaferMp, AlignC.RLWaferAlgorithm);
        //            lWaferMp.X -= (float)(AlignC.XOffset / GV.ZoomLensInfo.LeftUmPerPixelX[AlignC.AlignLowMagnification]);
        //            rWaferMp.X -= (float)(AlignC.XOffset / GV.ZoomLensInfo.RightUmPerPixelX[AlignC.AlignLowMagnification]);
        //            lWaferMp.Y -= (float)(AlignC.YLOffset / GV.ZoomLensInfo.LeftUmPerPixelY[AlignC.AlignLowMagnification]);
        //            rWaferMp.Y -= (float)(AlignC.YROffset / GV.ZoomLensInfo.RightUmPerPixelY[AlignC.AlignLowMagnification]);
        //        }
        //        else if (alignMmagnification == "high")
        //        {
        //            GV.matcherLHM.MatMatchWithAlgo(0, lSrc.Clone(), ref lMaskMp, AlignC.LHMaskAlgorithm);
        //            GV.matcherRHM.MatMatchWithAlgo(0, rSrc.Clone(), ref rMaskMp, AlignC.RHMaskAlgorithm);
        //            GV.matcherLHW.MatMatchWithAlgo(0, lSrc.Clone(), ref lWaferMp, AlignC.LHWaferAlgorithm);
        //            GV.matcherRHW.MatMatchWithAlgo(0, rSrc.Clone(), ref rWaferMp, AlignC.RHWaferAlgorithm);
        //            lWaferMp.X -= (float)(AlignC.XOffset / GV.ZoomLensInfo.LeftUmPerPixelX[AlignC.AlignHighMagnification]);
        //            rWaferMp.X -= (float)(AlignC.XOffset / GV.ZoomLensInfo.RightUmPerPixelX[AlignC.AlignHighMagnification]);
        //            lWaferMp.Y -= (float)(AlignC.YLOffset / GV.ZoomLensInfo.LeftUmPerPixelY[AlignC.AlignHighMagnification]);
        //            rWaferMp.Y -= (float)(AlignC.YROffset / GV.ZoomLensInfo.RightUmPerPixelY[AlignC.AlignHighMagnification]);
        //        }
        //        else if (alignMmagnification == "back")
        //        {
        //            //if (AlignC.UpBotMask == 2)
        //            //{
        //            //    GV.matcherLLW.MatMatchWithAlgo(0, lSrc.Clone(), ref lMaskMp, AlignC.LLWaferAlgorithm);
        //            //    GV.matcherRLW.MatMatchWithAlgo(0, rSrc.Clone(), ref rMaskMp, AlignC.RLWaferAlgorithm);
        //            //    GM.DebugMessage(string.Format("lMask Score : {0}", lMaskMp.Score));
        //            //    GM.DebugMessage("back match UpBotMask = 2!");
        //            //}
        //            //else
        //            //{
        //            //    //GV.matcherLLW.MatMatchWithAlgo(0, LeftMask, ref lMaskMp, AlignC.LLWaferAlgorithm);
        //            //    //GV.matcherRLW.MatMatchWithAlgo(0, RightMask, ref rMaskMp, AlignC.RLWaferAlgorithm);

        //            //    lMaskMp.X += AlignC.LMaskAdjX;
        //            //    lMaskMp.Y += AlignC.LMaskAdjY;
        //            //    rMaskMp.X += AlignC.RMaskAdjX;
        //            //    rMaskMp.Y += AlignC.RMaskAdjY;

        //            //    GM.DebugMessage("back match UpBotMask != 2");
        //            //}
        //            if (AlignC.UpBotMask == 2)
        //            {
        //                GV.matcherLLM.MatMatchWithAlgo(0, lSrc.Clone(), ref lMaskMp, AlignC.LLMaskAlgorithm);
        //                GV.matcherRLM.MatMatchWithAlgo(0, rSrc.Clone(), ref rMaskMp, AlignC.RLMaskAlgorithm);
        //                GM.DebugMessage(string.Format("lMask Score : {0}", lMaskMp.Score));
        //                GM.DebugMessage("back match UpBotMask = 2!");
        //            }
        //            else
        //            {
        //                if (AlignC.LLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
        //                {
        //                    GV.matcherLLM.MatMatchWithAlgo(0, LeftMask, ref lMaskMp, AlignC.LLMaskAlgorithm, AlignC.LLMaskAIClassId);
        //                }
        //                else
        //                {
        //                    GV.matcherLLM.MatMatchWithAlgo(0, LeftMask, ref lMaskMp, AlignC.LLMaskAlgorithm);
        //                }
        //                if (AlignC.RLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
        //                {
        //                    GV.matcherRLM.MatMatchWithAlgo(0, RightMask, ref rMaskMp, AlignC.RLMaskAlgorithm, AlignC.LLMaskAIClassId);
        //                }
        //                else
        //                {
        //                    GV.matcherRLM.MatMatchWithAlgo(0, RightMask, ref rMaskMp, AlignC.RLMaskAlgorithm);
        //                }

        //                lMaskMp.X += AlignC.LMaskAdjX;
        //                lMaskMp.Y += AlignC.LMaskAdjY;
        //                rMaskMp.X += AlignC.RMaskAdjX;
        //                rMaskMp.Y += AlignC.RMaskAdjY;

        //                GM.DebugMessage("back match UpBotMask != 2");
        //            }

        //            if (AlignC.LHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
        //            {
        //                GV.matcherLHW.MatMatchWithAlgo(0, lSrc.Clone(), ref lWaferMp, AlignC.LHWaferAlgorithm, AlignC.LHWaferAIClassId);
        //            }
        //            else
        //            {
        //                GV.matcherLHW.MatMatchWithAlgo(0, lSrc.Clone(), ref lWaferMp, AlignC.LHWaferAlgorithm);
        //            }
        //            if (AlignC.RHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
        //            {
        //                GV.matcherRHW.MatMatchWithAlgo(0, rSrc.Clone(), ref rWaferMp, AlignC.RHWaferAlgorithm, AlignC.RHWaferAIClassId);
        //            }
        //            else
        //            {
        //                GV.matcherRHW.MatMatchWithAlgo(0, rSrc.Clone(), ref rWaferMp, AlignC.RHWaferAlgorithm);
        //            }


        //            lWaferMp.X -= (float)(AlignC.XOffset / GV.ZoomLensInfo.LeftDownUmPerPixelX);
        //            rWaferMp.X -= (float)(AlignC.XOffset / GV.ZoomLensInfo.RightDownUmPerPixelX);
        //            lWaferMp.Y -= (float)(AlignC.YLOffset / GV.ZoomLensInfo.LeftDownUmPerPixelY);
        //            rWaferMp.Y -= (float)(AlignC.YROffset / GV.ZoomLensInfo.RightDownUmPerPixelY);
        //        }
        //        else if (alignMmagnification == "backmask")
        //        {
        //            //GV.matcherLLW.MatMatchWithAlgo(0, lSrc.Clone(), ref lMaskMp, AlignC.LLWaferAlgorithm);
        //            //GV.matcherRLW.MatMatchWithAlgo(0, rSrc.Clone(), ref rMaskMp, AlignC.RLWaferAlgorithm);
        //            GV.matcherLLM.MatMatchWithAlgo(0, lSrc.Clone(), ref lMaskMp, AlignC.LLMaskAlgorithm);
        //            GV.matcherRLM.MatMatchWithAlgo(0, rSrc.Clone(), ref rMaskMp, AlignC.RLMaskAlgorithm);
        //        }
        //        else if (alignMmagnification == "backwafer")
        //        {
        //            GV.matcherLHW.MatMatchWithAlgo(0, lSrc.Clone(), ref lWaferMp, AlignC.LHWaferAlgorithm);
        //            GV.matcherRHW.MatMatchWithAlgo(0, rSrc.Clone(), ref rWaferMp, AlignC.RHWaferAlgorithm);
        //        }
        //        else if (alignMmagnification == "checkmask")
        //        {
        //            GV.matcherLHM.MatMatchWithAlgo(0, lSrc.Clone(), ref lMaskMp, AlignC.LHMaskAlgorithm);
        //            GV.matcherRHM.MatMatchWithAlgo(0, rSrc.Clone(), ref rMaskMp, AlignC.RHMaskAlgorithm);
        //        }
        //        else if (alignMmagnification == "backcheck")
        //        {
        //            //GV.matcherLLW.MatMatchWithAlgo(0, LeftMask.Clone(), ref lMaskMp, AlignC.LLWaferAlgorithm);
        //            //GV.matcherRLW.MatMatchWithAlgo(0, RightMask.Clone(), ref rMaskMp, AlignC.RLWaferAlgorithm);
        //            GV.matcherLLM.MatMatchWithAlgo(0, LeftMask.Clone(), ref lMaskMp, AlignC.LLMaskAlgorithm);
        //            GV.matcherRLM.MatMatchWithAlgo(0, RightMask.Clone(), ref rMaskMp, AlignC.RLMaskAlgorithm);
        //            GV.matcherLHW.MatMatchWithAlgo(0, lSrc.Clone(), ref lWaferMp, AlignC.LHWaferAlgorithm);
        //            GV.matcherRHW.MatMatchWithAlgo(0, rSrc.Clone(), ref rWaferMp, AlignC.RHWaferAlgorithm);
        //        }
        //        else if (alignMmagnification == "first")
        //        {
        //            GV.matcherLLM.MatMatchWithAlgo(0, lSrc.Clone(), ref lMaskMp, AlignC.LLMaskAlgorithm);
        //            GV.matcherRLM.MatMatchWithAlgo(0, rSrc.Clone(), ref rMaskMp, AlignC.RLMaskAlgorithm);
        //            GV.matcherLLW.MatMatchWithAlgo(0, lSrc.Clone(), ref lWaferMp, AlignC.LLWaferAlgorithm);
        //            GV.matcherRLW.MatMatchWithAlgo(0, rSrc.Clone(), ref rWaferMp, AlignC.RLWaferAlgorithm);
        //        }
        //        else if (alignMmagnification == "CheckMat")
        //        {
        //            GV.matcherLLM.MatMatchWithAlgo(0, lSrc.Clone(), ref lMaskMp, AlignC.LLMaskAlgorithm);
        //            GV.matcherRLM.MatMatchWithAlgo(0, rSrc.Clone(), ref rMaskMp, AlignC.RLMaskAlgorithm);
        //            GV.matcherLLW.MatMatchWithAlgo(0, lSrc.Clone(), ref lWaferMp, AlignC.LLWaferAlgorithm);
        //            GV.matcherRLW.MatMatchWithAlgo(0, rSrc.Clone(), ref rWaferMp, AlignC.RLWaferAlgorithm);
        //        }
        //    }

        //    skLeftAlign.WaferMp.X = lWaferMp.X;
        //    skLeftAlign.WaferMp.Y = lWaferMp.Y;
        //    skLeftAlign.WaferMp.Score = lWaferMp.Score;
        //    skRightAlign.WaferMp.X = rWaferMp.X;
        //    skRightAlign.WaferMp.Y = rWaferMp.Y;
        //    skRightAlign.WaferMp.Score = rWaferMp.Score;
        //    skLeftAlign.MaskMp.X = lMaskMp.X;
        //    skLeftAlign.MaskMp.Y = lMaskMp.Y;
        //    skLeftAlign.MaskMp.Score = lMaskMp.Score;
        //    skRightAlign.MaskMp.X = rMaskMp.X;
        //    skRightAlign.MaskMp.Y = rMaskMp.Y;
        //    skRightAlign.MaskMp.Score = rMaskMp.Score;

        //    return new ProductMatchPositions(lMaskMp, lWaferMp, rMaskMp, rWaferMp, patternCenterDistance);
        //}
        private ProductMatchPositions GetAllMatchPostion(Mat lSrc, Mat rSrc, string alignMmagnification)
        {
            double patternCenterDistance = AlignC.PatternCenterDistanceUm;

            if (GV.AppSettingParm.Emulation != true)
            {
                if (alignMmagnification == "low")
                {
                    // ✅ 使用完整影像搜尋
                    GV.matcherLLM.MatMatchWithAlgo(0, lSrc.Clone(), ref lMaskMp, AlignC.LLMaskAlgorithm);
                    GV.matcherRLM.MatMatchWithAlgo(0, rSrc.Clone(), ref rMaskMp, AlignC.RLMaskAlgorithm);
                    GV.matcherLLW.MatMatchWithAlgo(0, lSrc.Clone(), ref lWaferMp, AlignC.LLWaferAlgorithm);
                    GV.matcherRLW.MatMatchWithAlgo(0, rSrc.Clone(), ref rWaferMp, AlignC.RLWaferAlgorithm);

                    // ✅ 套用偏移量校正座標
                    lMaskMp.X += AlignC.LLMaskOffsetX;
                    lMaskMp.Y += AlignC.LLMaskOffsetY;

                    lWaferMp.X += AlignC.LLWaferOffsetX;
                    lWaferMp.Y += AlignC.LLWaferOffsetY;

                    rMaskMp.X += AlignC.RLMaskOffsetX;
                    rMaskMp.Y += AlignC.RLMaskOffsetY;

                    rWaferMp.X += AlignC.RLWaferOffsetX;
                    rWaferMp.Y += AlignC.RLWaferOffsetY;

                    // 套用原有的 Offset 補償
                    lWaferMp.X -= (float)(AlignC.XOffset / GV.ZoomLensInfo.LeftUmPerPixelX[AlignC.AlignLowMagnification]);
                    rWaferMp.X -= (float)(AlignC.XOffset / GV.ZoomLensInfo.RightUmPerPixelX[AlignC.AlignLowMagnification]);
                    lWaferMp.Y -= (float)(AlignC.YLOffset / GV.ZoomLensInfo.LeftUmPerPixelY[AlignC.AlignLowMagnification]);
                    rWaferMp.Y -= (float)(AlignC.YROffset / GV.ZoomLensInfo.RightUmPerPixelY[AlignC.AlignLowMagnification]);
                }
                else if (alignMmagnification == "high")
                {
                    // ✅ 使用完整影像搜尋
                    GV.matcherLHM.MatMatchWithAlgo(0, lSrc.Clone(), ref lMaskMp, AlignC.LHMaskAlgorithm);
                    GV.matcherRHM.MatMatchWithAlgo(0, rSrc.Clone(), ref rMaskMp, AlignC.RHMaskAlgorithm);
                    GV.matcherLHW.MatMatchWithAlgo(0, lSrc.Clone(), ref lWaferMp, AlignC.LHWaferAlgorithm);
                    GV.matcherRHW.MatMatchWithAlgo(0, rSrc.Clone(), ref rWaferMp, AlignC.RHWaferAlgorithm);

                    // ✅ 套用偏移量校正座標
                    lMaskMp.X += AlignC.LHMaskOffsetX;
                    lMaskMp.Y += AlignC.LHMaskOffsetY;

                    lWaferMp.X += AlignC.LHWaferOffsetX;
                    lWaferMp.Y += AlignC.LHWaferOffsetY;

                    rMaskMp.X += AlignC.RHMaskOffsetX;
                    rMaskMp.Y += AlignC.RHMaskOffsetY;

                    rWaferMp.X += AlignC.RHWaferOffsetX;
                    rWaferMp.Y += AlignC.RHWaferOffsetY;

                    // 套用原有的 Offset 補償
                    lWaferMp.X -= (float)(AlignC.XOffset / GV.ZoomLensInfo.LeftUmPerPixelX[AlignC.AlignHighMagnification]);
                    rWaferMp.X -= (float)(AlignC.XOffset / GV.ZoomLensInfo.RightUmPerPixelX[AlignC.AlignHighMagnification]);
                    lWaferMp.Y -= (float)(AlignC.YLOffset / GV.ZoomLensInfo.LeftUmPerPixelY[AlignC.AlignHighMagnification]);
                    rWaferMp.Y -= (float)(AlignC.YROffset / GV.ZoomLensInfo.RightUmPerPixelY[AlignC.AlignHighMagnification]);
                }
                else if (alignMmagnification == "back")
                {
                    // ✅ 判斷使用哪種光罩來源
                    if (AlignC.UpBotMask == 2)
                    {
                        // 使用即時影像搜尋光罩
                        GV.matcherLLM.MatMatchWithAlgo(0, lSrc.Clone(), ref lMaskMp, AlignC.LLMaskAlgorithm);
                        GV.matcherRLM.MatMatchWithAlgo(0, rSrc.Clone(), ref rMaskMp, AlignC.RLMaskAlgorithm);

                        GM.DebugMessage($"back match UpBotMask = 2, lMask Score: {lMaskMp.Score}");
                    }
                    else
                    {
                        // 使用預先儲存的 Mask 影像搜尋
                        if (AlignC.LLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
                        {
                            GV.matcherLLM.MatMatchWithAlgo(0, LeftMask, ref lMaskMp, AlignC.LLMaskAlgorithm, AlignC.LLMaskAIClassId);
                        }
                        else
                        {
                            GV.matcherLLM.MatMatchWithAlgo(0, LeftMask, ref lMaskMp, AlignC.LLMaskAlgorithm);
                        }

                        if (AlignC.RLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
                        {
                            GV.matcherRLM.MatMatchWithAlgo(0, RightMask, ref rMaskMp, AlignC.RLMaskAlgorithm, AlignC.RLMaskAIClassId);
                        }
                        else
                        {
                            GV.matcherRLM.MatMatchWithAlgo(0, RightMask, ref rMaskMp, AlignC.RLMaskAlgorithm);
                        }

                        // 加上調整偏移
                        lMaskMp.X += AlignC.LMaskAdjX;
                        lMaskMp.Y += AlignC.LMaskAdjY;
                        rMaskMp.X += AlignC.RMaskAdjX;
                        rMaskMp.Y += AlignC.RMaskAdjY;

                        GM.DebugMessage("back match UpBotMask != 2");
                    }

                    // ✅ 套用偏移量校正座標（光罩）
                    lMaskMp.X += AlignC.LLMaskOffsetX;
                    lMaskMp.Y += AlignC.LLMaskOffsetY;

                    rMaskMp.X += AlignC.RLMaskOffsetX;
                    rMaskMp.Y += AlignC.RLMaskOffsetY;

                    // ✅ 搜尋晶圓
                    if (AlignC.LHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
                    {
                        GV.matcherLHW.MatMatchWithAlgo(0, lSrc.Clone(), ref lWaferMp, AlignC.LHWaferAlgorithm, AlignC.LHWaferAIClassId);
                    }
                    else
                    {
                        GV.matcherLHW.MatMatchWithAlgo(0, lSrc.Clone(), ref lWaferMp, AlignC.LHWaferAlgorithm);
                    }

                    if (AlignC.RHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
                    {
                        GV.matcherRHW.MatMatchWithAlgo(0, rSrc.Clone(), ref rWaferMp, AlignC.RHWaferAlgorithm, AlignC.RHWaferAIClassId);
                    }
                    else
                    {
                        GV.matcherRHW.MatMatchWithAlgo(0, rSrc.Clone(), ref rWaferMp, AlignC.RHWaferAlgorithm);
                    }

                    // ✅ 套用偏移量校正座標（晶圓）
                    lWaferMp.X += AlignC.LHWaferOffsetX;
                    lWaferMp.Y += AlignC.LHWaferOffsetY;

                    rWaferMp.X += AlignC.RHWaferOffsetX;
                    rWaferMp.Y += AlignC.RHWaferOffsetY;

                    // 套用原有的 Offset 補償
                    lWaferMp.X -= (float)(AlignC.XOffset / GV.ZoomLensInfo.LeftDownUmPerPixelX);
                    rWaferMp.X -= (float)(AlignC.XOffset / GV.ZoomLensInfo.RightDownUmPerPixelX);
                    lWaferMp.Y -= (float)(AlignC.YLOffset / GV.ZoomLensInfo.LeftDownUmPerPixelY);
                    rWaferMp.Y -= (float)(AlignC.YROffset / GV.ZoomLensInfo.RightDownUmPerPixelY);
                }
                else if (alignMmagnification == "backmask")
                {
                    // ✅ 只搜尋光罩:使用完整影像搜尋
                    GV.matcherLLM.MatMatchWithAlgo(0, lSrc.Clone(), ref lMaskMp, AlignC.LLMaskAlgorithm);
                    GV.matcherRLM.MatMatchWithAlgo(0, rSrc.Clone(), ref rMaskMp, AlignC.RLMaskAlgorithm);

                    // ✅ 套用偏移量校正座標
                    lMaskMp.X += AlignC.LLMaskOffsetX;
                    lMaskMp.Y += AlignC.LLMaskOffsetY;

                    rMaskMp.X += AlignC.RLMaskOffsetX;
                    rMaskMp.Y += AlignC.RLMaskOffsetY;
                }
                else if (alignMmagnification == "backwafer")
                {
                    // ✅ 只搜尋晶圓:使用完整影像搜尋
                    GV.matcherLHW.MatMatchWithAlgo(0, lSrc.Clone(), ref lWaferMp, AlignC.LHWaferAlgorithm);
                    GV.matcherRHW.MatMatchWithAlgo(0, rSrc.Clone(), ref rWaferMp, AlignC.RHWaferAlgorithm);

                    // ✅ 套用偏移量校正座標
                    lWaferMp.X += AlignC.LHWaferOffsetX;
                    lWaferMp.Y += AlignC.LHWaferOffsetY;

                    rWaferMp.X += AlignC.RHWaferOffsetX;
                    rWaferMp.Y += AlignC.RHWaferOffsetY;
                }
                else if (alignMmagnification == "checkmask")
                {
                    // ✅ 檢查光罩:使用完整影像搜尋
                    GV.matcherLHM.MatMatchWithAlgo(0, lSrc.Clone(), ref lMaskMp, AlignC.LHMaskAlgorithm);
                    GV.matcherRHM.MatMatchWithAlgo(0, rSrc.Clone(), ref rMaskMp, AlignC.RHMaskAlgorithm);

                    // ✅ 套用偏移量校正座標
                    lMaskMp.X += AlignC.LHMaskOffsetX;
                    lMaskMp.Y += AlignC.LHMaskOffsetY;

                    rMaskMp.X += AlignC.RHMaskOffsetX;
                    rMaskMp.Y += AlignC.RHMaskOffsetY;
                }
                else if (alignMmagnification == "backcheck")
                {
                    // ✅ 使用預存的 Mask 影像搜尋光罩
                    GV.matcherLLM.MatMatchWithAlgo(0, LeftMask.Clone(), ref lMaskMp, AlignC.LLMaskAlgorithm);
                    GV.matcherRLM.MatMatchWithAlgo(0, RightMask.Clone(), ref rMaskMp, AlignC.RLMaskAlgorithm);

                    // ✅ 套用偏移量校正座標（光罩）
                    lMaskMp.X += AlignC.LLMaskOffsetX;
                    lMaskMp.Y += AlignC.LLMaskOffsetY;

                    rMaskMp.X += AlignC.RLMaskOffsetX;
                    rMaskMp.Y += AlignC.RLMaskOffsetY;

                    // ✅ 搜尋晶圓
                    GV.matcherLHW.MatMatchWithAlgo(0, lSrc.Clone(), ref lWaferMp, AlignC.LHWaferAlgorithm);
                    GV.matcherRHW.MatMatchWithAlgo(0, rSrc.Clone(), ref rWaferMp, AlignC.RHWaferAlgorithm);

                    // ✅ 套用偏移量校正座標（晶圓）
                    lWaferMp.X += AlignC.LHWaferOffsetX;
                    lWaferMp.Y += AlignC.LHWaferOffsetY;

                    rWaferMp.X += AlignC.RHWaferOffsetX;
                    rWaferMp.Y += AlignC.RHWaferOffsetY;
                }
                else if (alignMmagnification == "first")
                {
                    // ✅ 初始化模式:使用完整影像搜尋
                    GV.matcherLLM.MatMatchWithAlgo(0, lSrc.Clone(), ref lMaskMp, AlignC.LLMaskAlgorithm);
                    GV.matcherRLM.MatMatchWithAlgo(0, rSrc.Clone(), ref rMaskMp, AlignC.RLMaskAlgorithm);
                    GV.matcherLLW.MatMatchWithAlgo(0, lSrc.Clone(), ref lWaferMp, AlignC.LLWaferAlgorithm);
                    GV.matcherRLW.MatMatchWithAlgo(0, rSrc.Clone(), ref rWaferMp, AlignC.RLWaferAlgorithm);

                    // ✅ 套用偏移量校正座標
                    lMaskMp.X += AlignC.LLMaskOffsetX;
                    lMaskMp.Y += AlignC.LLMaskOffsetY;

                    lWaferMp.X += AlignC.LLWaferOffsetX;
                    lWaferMp.Y += AlignC.LLWaferOffsetY;

                    rMaskMp.X += AlignC.RLMaskOffsetX;
                    rMaskMp.Y += AlignC.RLMaskOffsetY;

                    rWaferMp.X += AlignC.RLWaferOffsetX;
                    rWaferMp.Y += AlignC.RLWaferOffsetY;
                }
                else if (alignMmagnification == "CheckMat")
                {
                    // CheckMat 模式:保留原有邏輯（與 first 相同）
                    GV.matcherLLM.MatMatchWithAlgo(0, lSrc.Clone(), ref lMaskMp, AlignC.LLMaskAlgorithm);
                    GV.matcherRLM.MatMatchWithAlgo(0, rSrc.Clone(), ref rMaskMp, AlignC.RLMaskAlgorithm);
                    GV.matcherLLW.MatMatchWithAlgo(0, lSrc.Clone(), ref lWaferMp, AlignC.LLWaferAlgorithm);
                    GV.matcherRLW.MatMatchWithAlgo(0, rSrc.Clone(), ref rWaferMp, AlignC.RLWaferAlgorithm);

                    // ✅ 套用偏移量校正座標
                    lMaskMp.X += AlignC.LLMaskOffsetX;
                    lMaskMp.Y += AlignC.LLMaskOffsetY;

                    lWaferMp.X += AlignC.LLWaferOffsetX;
                    lWaferMp.Y += AlignC.LLWaferOffsetY;

                    rMaskMp.X += AlignC.RLMaskOffsetX;
                    rMaskMp.Y += AlignC.RLMaskOffsetY;

                    rWaferMp.X += AlignC.RLWaferOffsetX;
                    rWaferMp.Y += AlignC.RLWaferOffsetY;
                }
            }

            skLeftAlign.WaferMp.X = lWaferMp.X;
            skLeftAlign.WaferMp.Y = lWaferMp.Y;
            skLeftAlign.WaferMp.Score = lWaferMp.Score;
            skRightAlign.WaferMp.X = rWaferMp.X;
            skRightAlign.WaferMp.Y = rWaferMp.Y;
            skRightAlign.WaferMp.Score = rWaferMp.Score;
            skLeftAlign.MaskMp.X = lMaskMp.X;
            skLeftAlign.MaskMp.Y = lMaskMp.Y;
            skLeftAlign.MaskMp.Score = lMaskMp.Score;
            skRightAlign.MaskMp.X = rMaskMp.X;
            skRightAlign.MaskMp.Y = rMaskMp.Y;
            skRightAlign.MaskMp.Score = rMaskMp.Score;

            return new ProductMatchPositions(lMaskMp, lWaferMp, rMaskMp, rWaferMp, patternCenterDistance);
        }

        private void GetAllMatchPostion(Mat lSrc, Mat rSrc, string alignMmagnification, ref ProductMatchPositions rpms)
        {
            rpms.PatternCenterDistance = AlignC.PatternCenterDistanceUm;

            if (GV.AppSettingParm.Emulation != true)
            {
                if (alignMmagnification == "low")
                {
                    GV.matcherLLM.MatMatchWithAlgo(0, lSrc.Clone(), ref rpms.LMaskMp, AlignC.LLMaskAlgorithm);
                    GV.matcherRLM.MatMatchWithAlgo(0, rSrc.Clone(), ref rpms.RMaskMp, AlignC.RLMaskAlgorithm);
                    GV.matcherLLW.MatMatchWithAlgo(0, lSrc.Clone(), ref rpms.LWaferMp, AlignC.LLWaferAlgorithm);
                    GV.matcherRLW.MatMatchWithAlgo(0, rSrc.Clone(), ref rpms.RWaferMp, AlignC.RLWaferAlgorithm);
                    rpms.LWaferMp.X -= (float)(AlignC.XOffset / GV.ZoomLensInfo.LeftUmPerPixelX[AlignC.AlignLowMagnification]);
                    rpms.RWaferMp.X -= (float)(AlignC.XOffset / GV.ZoomLensInfo.RightUmPerPixelX[AlignC.AlignLowMagnification]);
                    rpms.LWaferMp.Y -= (float)(AlignC.YLOffset / GV.ZoomLensInfo.LeftUmPerPixelY[AlignC.AlignLowMagnification]);
                    rpms.RWaferMp.Y -= (float)(AlignC.YROffset / GV.ZoomLensInfo.RightUmPerPixelY[AlignC.AlignLowMagnification]);
                }
                else if (alignMmagnification == "high")
                {
                    GV.matcherLHM.MatMatchWithAlgo(0, lSrc.Clone(), ref rpms.LMaskMp, AlignC.LHMaskAlgorithm);
                    GV.matcherRHM.MatMatchWithAlgo(0, rSrc.Clone(), ref rpms.RMaskMp, AlignC.RHMaskAlgorithm);
                    GV.matcherLHW.MatMatchWithAlgo(0, lSrc.Clone(), ref rpms.LWaferMp, AlignC.LHWaferAlgorithm);
                    GV.matcherRHW.MatMatchWithAlgo(0, rSrc.Clone(), ref rpms.RWaferMp, AlignC.RHWaferAlgorithm);
                    rpms.LWaferMp.X -= (float)(AlignC.XOffset / GV.ZoomLensInfo.LeftUmPerPixelX[AlignC.AlignHighMagnification]);
                    rpms.RWaferMp.X -= (float)(AlignC.XOffset / GV.ZoomLensInfo.RightUmPerPixelX[AlignC.AlignHighMagnification]);
                    rpms.LWaferMp.Y -= (float)(AlignC.YLOffset / GV.ZoomLensInfo.LeftUmPerPixelY[AlignC.AlignHighMagnification]);
                    rpms.RWaferMp.Y -= (float)(AlignC.YROffset / GV.ZoomLensInfo.RightUmPerPixelY[AlignC.AlignHighMagnification]);
                }
                else if (alignMmagnification == "back")
                {
                    BLivePmps.LMaskMp.CopyTo(ref rpms.LMaskMp);
                    BLivePmps.RMaskMp.CopyTo(ref rpms.RMaskMp);

                    GV.matcherLHW.MatMatchWithAlgo(0, lSrc.Clone(), ref rpms.LWaferMp, AlignC.LHWaferAlgorithm);
                    GV.matcherRHW.MatMatchWithAlgo(0, rSrc.Clone(), ref rpms.RWaferMp, AlignC.RHWaferAlgorithm);
                    rpms.LWaferMp.X -= (float)(AlignC.XOffset / GV.ZoomLensInfo.LeftDownUmPerPixelX);
                    rpms.RWaferMp.X -= (float)(AlignC.XOffset / GV.ZoomLensInfo.RightDownUmPerPixelX);
                    rpms.LWaferMp.Y -= (float)(AlignC.YLOffset / GV.ZoomLensInfo.LeftDownUmPerPixelY);
                    rpms.RWaferMp.Y -= (float)(AlignC.YROffset / GV.ZoomLensInfo.RightDownUmPerPixelY);
                }
                else if (alignMmagnification == "backmask")
                {
                    GV.matcherLLW.MatMatchWithAlgo(0, lSrc.Clone(), ref rpms.LMaskMp, AlignC.LLWaferAlgorithm);
                    GV.matcherRLW.MatMatchWithAlgo(0, rSrc.Clone(), ref rpms.RMaskMp, AlignC.RLWaferAlgorithm);
                }
                else if (alignMmagnification == "checkmask")
                {
                    GV.matcherLHM.MatMatchWithAlgo(0, lSrc.Clone(), ref rpms.LMaskMp, AlignC.LHMaskAlgorithm);
                    GV.matcherRHM.MatMatchWithAlgo(0, rSrc.Clone(), ref rpms.RMaskMp, AlignC.RHMaskAlgorithm);
                }
                else if (alignMmagnification == "backcheck")
                {
                    GV.matcherLLW.MatMatchWithAlgo(0, lSrc.Clone(), ref rpms.LMaskMp, AlignC.LLWaferAlgorithm);
                    GV.matcherRLW.MatMatchWithAlgo(0, rSrc.Clone(), ref rpms.RMaskMp, AlignC.RLWaferAlgorithm);
                    GV.matcherLHW.MatMatchWithAlgo(0, lSrc.Clone(), ref rpms.LWaferMp, AlignC.LHWaferAlgorithm);
                    GV.matcherRHW.MatMatchWithAlgo(0, rSrc.Clone(), ref rpms.RWaferMp, AlignC.RHWaferAlgorithm);
                }
                else if (alignMmagnification == "first")
                {

                }
                else if (alignMmagnification == "CheckMat")
                {

                }

                skLeftAlign.MaskMp.X = rpms.LMaskMp.X;
                skLeftAlign.MaskMp.Y = rpms.LMaskMp.Y;
                skLeftAlign.MaskMp.Score = rpms.LMaskMp.Score;
                skLeftAlign.WaferMp.X = rpms.LWaferMp.X;
                skLeftAlign.WaferMp.Y = rpms.LWaferMp.Y;
                skLeftAlign.WaferMp.Score = rpms.LWaferMp.Score;
                skRightAlign.MaskMp.X = rpms.RMaskMp.X;
                skRightAlign.MaskMp.Y = rpms.RMaskMp.Y;
                skRightAlign.MaskMp.Score = rpms.RMaskMp.Score;
                skRightAlign.WaferMp.X = rpms.RWaferMp.X;
                skRightAlign.WaferMp.Y = rpms.RWaferMp.Y;
                skRightAlign.WaferMp.Score = rpms.RWaferMp.Score;
            }
            // return new ProductMatchPositions(lMaskMp, lWaferMp, rMaskMp, rWaferMp, patternCenterDistance);
        }

        //private ProductMatchPositions CheckWaferMatchPostion(Mat lSrc, Mat rSrc)
        //{
        //    //MatchPosition lMaskMp = new MatchPosition();
        //    //MatchPosition rMaskMp = new MatchPosition();
        //    //MatchPosition lWaferMp = new MatchPosition();
        //    //MatchPosition rWaferMp = new MatchPosition();

        //    lMaskMp = LastPmps.LMaskMp;
        //    rMaskMp = LastPmps.RMaskMp;
        //    GV.matcherLHW.MatMatchWithAlgo(0, lSrc.Clone(), ref lWaferMp, AlignC.LHWaferAlgorithm);
        //    GV.matcherRHW.MatMatchWithAlgo(0, rSrc.Clone(), ref rWaferMp, AlignC.RHWaferAlgorithm);

        //    lWaferMp.X -= (float)(AlignC.XOffset / GV.ZoomLensInfo.LeftUmPerPixelX[AlignC.AlignHighMagnification]);
        //    rWaferMp.X -= (float)(AlignC.XOffset / GV.ZoomLensInfo.RightUmPerPixelX[AlignC.AlignHighMagnification]);
        //    lWaferMp.Y -= (float)(AlignC.YLOffset / GV.ZoomLensInfo.LeftUmPerPixelY[AlignC.AlignHighMagnification]);
        //    rWaferMp.Y -= (float)(AlignC.YROffset / GV.ZoomLensInfo.RightUmPerPixelY[AlignC.AlignHighMagnification]);

        //    skLeftAlign.WaferMp.X = lWaferMp.X;
        //    skLeftAlign.WaferMp.Y = lWaferMp.Y;
        //    skLeftAlign.WaferMp.Score = lWaferMp.Score;
        //    skRightAlign.WaferMp.X = rWaferMp.X;
        //    skRightAlign.WaferMp.Y = rWaferMp.Y;
        //    skRightAlign.WaferMp.Score = rWaferMp.Score;
        //    skLeftAlign.MaskMp.X = lMaskMp.X;
        //    skLeftAlign.MaskMp.Y = lMaskMp.Y;
        //    skLeftAlign.MaskMp.Score = lMaskMp.Score;
        //    skRightAlign.MaskMp.X = rMaskMp.X;
        //    skRightAlign.MaskMp.Y = rMaskMp.Y;
        //    skRightAlign.MaskMp.Score = rMaskMp.Score;

        //    double patternCenterDistancePixel = AlignC.PatternCenterDistanceUm / ((GV.ZoomLensInfo.LeftUmPerPixelX[AlignC.AlignHighMagnification] + GV.ZoomLensInfo.RightUmPerPixelX[AlignC.AlignHighMagnification]) / 2);

        //    return new ProductMatchPositions(lMaskMp, lWaferMp, rMaskMp, rWaferMp, patternCenterDistancePixel);
        //}
        private ProductMatchPositions CheckWaferMatchPostion(Mat lSrc, Mat rSrc)
        {
            // ✅ 直接使用 LastPmps 中的光罩位置(已經在前面步驟確定)
            lMaskMp = LastPmps.LMaskMp;
            rMaskMp = LastPmps.RMaskMp;

            // ✅ 搜尋左上高倍率晶圓(支援偏移量)
            GV.matcherLHW.MatMatchWithAlgo(0, lSrc.Clone(), ref lWaferMp, AlignC.LHWaferAlgorithm);

            // ✅ 套用偏移量校正座標
            lWaferMp.X += AlignC.LHWaferOffsetX;
            lWaferMp.Y += AlignC.LHWaferOffsetY;

            // ✅ 搜尋右上高倍率晶圓(支援偏移量)
            GV.matcherRHW.MatMatchWithAlgo(0, rSrc.Clone(), ref rWaferMp, AlignC.RHWaferAlgorithm);

            // ✅ 套用偏移量校正座標
            rWaferMp.X += AlignC.RHWaferOffsetX;
            rWaferMp.Y += AlignC.RHWaferOffsetY;

            // 套用偏移補償
            lWaferMp.X -= (float)(AlignC.XOffset / GV.ZoomLensInfo.LeftUmPerPixelX[AlignC.AlignHighMagnification]);
            rWaferMp.X -= (float)(AlignC.XOffset / GV.ZoomLensInfo.RightUmPerPixelX[AlignC.AlignHighMagnification]);
            lWaferMp.Y -= (float)(AlignC.YLOffset / GV.ZoomLensInfo.LeftUmPerPixelY[AlignC.AlignHighMagnification]);
            rWaferMp.Y -= (float)(AlignC.YROffset / GV.ZoomLensInfo.RightUmPerPixelY[AlignC.AlignHighMagnification]);

            // 更新 UI 顯示
            skLeftAlign.WaferMp.X = lWaferMp.X;
            skLeftAlign.WaferMp.Y = lWaferMp.Y;
            skLeftAlign.WaferMp.Score = lWaferMp.Score;
            skRightAlign.WaferMp.X = rWaferMp.X;
            skRightAlign.WaferMp.Y = rWaferMp.Y;
            skRightAlign.WaferMp.Score = rWaferMp.Score;
            skLeftAlign.MaskMp.X = lMaskMp.X;
            skLeftAlign.MaskMp.Y = lMaskMp.Y;
            skLeftAlign.MaskMp.Score = lMaskMp.Score;
            skRightAlign.MaskMp.X = rMaskMp.X;
            skRightAlign.MaskMp.Y = rMaskMp.Y;
            skRightAlign.MaskMp.Score = rMaskMp.Score;

            double patternCenterDistancePixel = AlignC.PatternCenterDistanceUm / ((GV.ZoomLensInfo.LeftUmPerPixelX[AlignC.AlignHighMagnification] + GV.ZoomLensInfo.RightUmPerPixelX[AlignC.AlignHighMagnification]) / 2);

            return new ProductMatchPositions(lMaskMp, lWaferMp, rMaskMp, rWaferMp, patternCenterDistancePixel);
        }

        //private ProductMatchPositions CheckWaferMatchPostionBack(Mat lSrc, Mat rSrc)
        //{
        //    MatchPosition lMaskMp = new MatchPosition();
        //    MatchPosition rMaskMp = new MatchPosition();
        //    MatchPosition lWaferMp = new MatchPosition();
        //    MatchPosition rWaferMp = new MatchPosition();

        //    if (AlignC.UpBotMask == 2)
        //    {
        //        if (AlignC.LLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
        //        {
        //            GV.matcherLLM.MatMatchWithAlgo(0, lSrc.Clone(), ref lMaskMp, AlignC.LLMaskAlgorithm, AlignC.LLMaskAIClassId);
        //        }
        //        else
        //        {
        //            GV.matcherLLM.MatMatchWithAlgo(0, lSrc.Clone(), ref lMaskMp, AlignC.LLMaskAlgorithm);
        //        }
        //        if (AlignC.RLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
        //        {
        //            GV.matcherRLM.MatMatchWithAlgo(0, rSrc.Clone(), ref rMaskMp, AlignC.RLMaskAlgorithm, AlignC.RLMaskAIClassId);
        //        }
        //        else
        //        {
        //            GV.matcherRLM.MatMatchWithAlgo(0, rSrc.Clone(), ref rMaskMp, AlignC.RLMaskAlgorithm);
        //        }
        //        //GV.matcherLLW.MatMatchWithAlgo(0, lSrc.Clone(), ref lMaskMp, AlignC.LLWaferAlgorithm);
        //        //GV.matcherRLW.MatMatchWithAlgo(0, rSrc.Clone(), ref rMaskMp, AlignC.RLWaferAlgorithm);
        //    }
        //    else
        //    {
        //        if (AlignC.LLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
        //        {
        //            GV.matcherLLM.MatMatchWithAlgo(0, LeftMask, ref lMaskMp, AlignC.LLMaskAlgorithm, AlignC.LLMaskAIClassId);
        //        }
        //        else
        //        {
        //            GV.matcherLLM.MatMatchWithAlgo(0, LeftMask, ref lMaskMp, AlignC.LLMaskAlgorithm);
        //        }
        //        if (AlignC.RLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
        //        {
        //            GV.matcherRLM.MatMatchWithAlgo(0, RightMask, ref rMaskMp, AlignC.RLMaskAlgorithm, AlignC.RLMaskAIClassId);
        //        }
        //        else
        //        {
        //            GV.matcherRLM.MatMatchWithAlgo(0, RightMask, ref rMaskMp, AlignC.RLMaskAlgorithm);
        //        }
        //        //GV.matcherLLM.MatMatchWithAlgo(0, LeftMask, ref lMaskMp, AlignC.LLMaskAlgorithm);
        //        //GV.matcherRLM.MatMatchWithAlgo(0, RightMask, ref rMaskMp, AlignC.RLMaskAlgorithm);
        //    }
        //    lMaskMp.X += AlignC.LMaskAdjX;
        //    lMaskMp.Y += AlignC.LMaskAdjY;
        //    rMaskMp.X += AlignC.RMaskAdjX;
        //    rMaskMp.Y += AlignC.RMaskAdjY;
        //    if (AlignC.LHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
        //    {
        //        GV.matcherLHW.MatMatchWithAlgo(0, lSrc.Clone(), ref lWaferMp, AlignC.LHWaferAlgorithm, AlignC.LHWaferAIClassId);
        //    }
        //    else
        //    {
        //        GV.matcherLHW.MatMatchWithAlgo(0, lSrc.Clone(), ref lWaferMp, AlignC.LHWaferAlgorithm);
        //    }
        //    if (AlignC.RHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
        //    {
        //        GV.matcherRHW.MatMatchWithAlgo(0, rSrc.Clone(), ref rWaferMp, AlignC.RHWaferAlgorithm, AlignC.RHWaferAIClassId);

        //    }
        //    else
        //    {
        //        GV.matcherRHW.MatMatchWithAlgo(0, rSrc.Clone(), ref rWaferMp, AlignC.RHWaferAlgorithm);
        //    }
        //    //GV.matcherRHW.MatMatchWithAlgo(0, rSrc.Clone(), ref rWaferMp, AlignC.RHWaferAlgorithm);
        //    //GV.matcherRHW.MatMatchWithAlgo(0, rSrc.Clone(), ref rWaferMp, AlignC.RHWaferAlgorithm);

        //    lWaferMp.X -= (float)(AlignC.XOffset / GV.ZoomLensInfo.LeftDownUmPerPixelX);
        //    rWaferMp.X -= (float)(AlignC.XOffset / GV.ZoomLensInfo.RightDownUmPerPixelX);
        //    lWaferMp.Y -= (float)(AlignC.YLOffset / GV.ZoomLensInfo.LeftDownUmPerPixelY);
        //    rWaferMp.Y -= (float)(AlignC.YROffset / GV.ZoomLensInfo.RightDownUmPerPixelY);

        //    skLeftAlign.WaferMp.X = lWaferMp.X;
        //    skLeftAlign.WaferMp.Y = lWaferMp.Y;
        //    skLeftAlign.WaferMp.Score = lWaferMp.Score;
        //    skRightAlign.WaferMp.X = rWaferMp.X;
        //    skRightAlign.WaferMp.Y = rWaferMp.Y;
        //    skRightAlign.WaferMp.Score = rWaferMp.Score;
        //    skLeftAlign.MaskMp.X = lMaskMp.X;
        //    skLeftAlign.MaskMp.Y = lMaskMp.Y;
        //    skLeftAlign.MaskMp.Score = lMaskMp.Score;
        //    skRightAlign.MaskMp.X = rMaskMp.X;
        //    skRightAlign.MaskMp.Y = rMaskMp.Y;
        //    skRightAlign.MaskMp.Score = rMaskMp.Score;

        //    double patternCenterDistancePixel = AlignC.PatternCenterDistanceUm / ((GV.ZoomLensInfo.LeftDownUmPerPixelX + GV.ZoomLensInfo.RightDownUmPerPixelX) / 2);

        //    return new ProductMatchPositions(lMaskMp, lWaferMp, rMaskMp, rWaferMp, patternCenterDistancePixel);
        //}
        private ProductMatchPositions CheckWaferMatchPostionBack(Mat lSrc, Mat rSrc)
        {
            MatchPosition lMaskMp = new MatchPosition();
            MatchPosition rMaskMp = new MatchPosition();
            MatchPosition lWaferMp = new MatchPosition();
            MatchPosition rWaferMp = new MatchPosition();

            // ✅ 搜尋光罩（根據 UpBotMask 決定使用即時影像或預存的 Mask）
            if (AlignC.UpBotMask == 2)
            {
                // 使用即時影像搜尋光罩
                if (AlignC.LLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
                {
                    GV.matcherLLM.MatMatchWithAlgo(0, lSrc.Clone(), ref lMaskMp, AlignC.LLMaskAlgorithm, AlignC.LLMaskAIClassId);
                }
                else
                {
                    GV.matcherLLM.MatMatchWithAlgo(0, lSrc.Clone(), ref lMaskMp, AlignC.LLMaskAlgorithm);
                }

                if (AlignC.RLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
                {
                    GV.matcherRLM.MatMatchWithAlgo(0, rSrc.Clone(), ref rMaskMp, AlignC.RLMaskAlgorithm, AlignC.RLMaskAIClassId);
                }
                else
                {
                    GV.matcherRLM.MatMatchWithAlgo(0, rSrc.Clone(), ref rMaskMp, AlignC.RLMaskAlgorithm);
                }
            }
            else
            {
                // 使用預存的 Mask 影像搜尋
                if (AlignC.LLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
                {
                    GV.matcherLLM.MatMatchWithAlgo(0, LeftMask, ref lMaskMp, AlignC.LLMaskAlgorithm, AlignC.LLMaskAIClassId);
                }
                else
                {
                    GV.matcherLLM.MatMatchWithAlgo(0, LeftMask, ref lMaskMp, AlignC.LLMaskAlgorithm);
                }

                if (AlignC.RLMaskAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
                {
                    GV.matcherRLM.MatMatchWithAlgo(0, RightMask, ref rMaskMp, AlignC.RLMaskAlgorithm, AlignC.RLMaskAIClassId);
                }
                else
                {
                    GV.matcherRLM.MatMatchWithAlgo(0, RightMask, ref rMaskMp, AlignC.RLMaskAlgorithm);
                }
            }

            // 加上調整偏移（保留原有邏輯）
            lMaskMp.X += AlignC.LMaskAdjX;
            lMaskMp.Y += AlignC.LMaskAdjY;
            rMaskMp.X += AlignC.RMaskAdjX;
            rMaskMp.Y += AlignC.RMaskAdjY;

            // ✅ 套用偏移量校正座標（光罩）
            lMaskMp.X += AlignC.LLMaskOffsetX;
            lMaskMp.Y += AlignC.LLMaskOffsetY;

            rMaskMp.X += AlignC.RLMaskOffsetX;
            rMaskMp.Y += AlignC.RLMaskOffsetY;

            // ✅ 搜尋晶圓（支援偏移量）
            if (AlignC.LHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
            {
                GV.matcherLHW.MatMatchWithAlgo(0, lSrc.Clone(), ref lWaferMp, AlignC.LHWaferAlgorithm, AlignC.LHWaferAIClassId);
            }
            else
            {
                GV.matcherLHW.MatMatchWithAlgo(0, lSrc.Clone(), ref lWaferMp, AlignC.LHWaferAlgorithm);
            }

            if (AlignC.RHWaferAlgorithm == OpenCV3MatchUMat.AlignAlgorithm.AIMatch)
            {
                GV.matcherRHW.MatMatchWithAlgo(0, rSrc.Clone(), ref rWaferMp, AlignC.RHWaferAlgorithm, AlignC.RHWaferAIClassId);
            }
            else
            {
                GV.matcherRHW.MatMatchWithAlgo(0, rSrc.Clone(), ref rWaferMp, AlignC.RHWaferAlgorithm);
            }

            // ✅ 套用偏移量校正座標（晶圓）
            lWaferMp.X += AlignC.LHWaferOffsetX;
            lWaferMp.Y += AlignC.LHWaferOffsetY;

            rWaferMp.X += AlignC.RHWaferOffsetX;
            rWaferMp.Y += AlignC.RHWaferOffsetY;

            // 套用偏移補償
            lWaferMp.X -= (float)(AlignC.XOffset / GV.ZoomLensInfo.LeftDownUmPerPixelX);
            rWaferMp.X -= (float)(AlignC.XOffset / GV.ZoomLensInfo.RightDownUmPerPixelX);
            lWaferMp.Y -= (float)(AlignC.YLOffset / GV.ZoomLensInfo.LeftDownUmPerPixelY);
            rWaferMp.Y -= (float)(AlignC.YROffset / GV.ZoomLensInfo.RightDownUmPerPixelY);

            // 更新 UI 顯示
            skLeftAlign.WaferMp.X = lWaferMp.X;
            skLeftAlign.WaferMp.Y = lWaferMp.Y;
            skLeftAlign.WaferMp.Score = lWaferMp.Score;
            skRightAlign.WaferMp.X = rWaferMp.X;
            skRightAlign.WaferMp.Y = rWaferMp.Y;
            skRightAlign.WaferMp.Score = rWaferMp.Score;
            skLeftAlign.MaskMp.X = lMaskMp.X;
            skLeftAlign.MaskMp.Y = lMaskMp.Y;
            skLeftAlign.MaskMp.Score = lMaskMp.Score;
            skRightAlign.MaskMp.X = rMaskMp.X;
            skRightAlign.MaskMp.Y = rMaskMp.Y;
            skRightAlign.MaskMp.Score = rMaskMp.Score;

            double patternCenterDistancePixel = AlignC.PatternCenterDistanceUm / ((GV.ZoomLensInfo.LeftDownUmPerPixelX + GV.ZoomLensInfo.RightDownUmPerPixelX) / 2);

            return new ProductMatchPositions(lMaskMp, lWaferMp, rMaskMp, rWaferMp, patternCenterDistancePixel);
        }

        private void SearchWafer()
        {
            int _SearchingStepX = (int)(SearchingStepX - ((_recipe.LeftHighWaferMat.Width + _recipe.RightHighWaferMat.Width) / 4));
            int _SearchingStepY = (int)(SearchingStepY - ((_recipe.LeftHighWaferMat.Height + _recipe.RightHighWaferMat.Height) / 4));

            int findCount = 0;
            int[,] searchXY = {{ -1, -1 }, { 1, 0 }, { 1, 0 }, { 0, 1 }, { 0, 1 }, { -1, 0 }, { -1, 0 }, {0, -1}, { -1, 0 }, { 0, -1 },
                    { 0, -1 }, { 1, 0 }, { 1, 0 }, { 1, 0 }, { 1, 0 }, { 0, 1 }, { 0, 1 }, { 0, 1 }, { 0, 1 },{ -1, 0 }, { -1, 0 }, { -1, 0 }, { -1, 0 },{ 0, -1 } };
            int[] searchTheta = new int[] { 1, 1, 1, 1, 1, -5, -1, -1, -1, -1, -1, 5 };
            int rollBack1 = 5;
            int rollBack2 = 11;
            Thread.Sleep(250);
            ProductMatchPositions pmps = GetAllMatchPostion(GV.LeftUpCam.Grab(), GV.RightUpCam.Grab(), "low");
            if (!(pmps.LWaferMp.Score > AlignC.WaferScore && pmps.RWaferMp.Score > AlignC.WaferScore))
            {
                Mat leftFailImage = GV.LeftUpCam.Grab();
                Mat rightFailImage = GV.RightUpCam.Grab();
                SaveAlignFailImage(leftFailImage, rightFailImage, "UpWaferFail", saveOnce: true);
            }
            // writeStatus("L / R Wafer score : " + pmps.LWaferMp.Score.ToString("F2") + " , " + pmps.RWaferMp.Score.ToString("F2"));
            writeStatus(string.Format(GV.Dlang.strMessageWaferScore, pmps.LWaferMp.Score.ToString("F2"), pmps.RWaferMp.Score.ToString("F2")));
            if (pmps.LWaferMp.Score > AlignC.WaferScore && pmps.RWaferMp.Score > AlignC.WaferScore)
                return;
            if (!GV.AppSettingParm.EnableAutoSearch)
            {
                if (pmps.LWaferMp.Score > AlignC.WaferScore && pmps.RWaferMp.Score > AlignC.WaferScore)
                    return;
                else
                {
                    GV.Plc.SendAlignNG();
                    iStartAlign = 0;
                    writeStatus(GV.Dlang.strErrorCantFindWafer);
                    return;
                }
            }

            for (int i = 0; i < (searchXY.Length / 2) - 1; i++)
            {
                if (pmps.LWaferMp.Score > AlignC.WaferScore || pmps.RWaferMp.Score > AlignC.WaferScore)
                    findCount = 1;
                if (pmps.LWaferMp.Score > AlignC.WaferScore && pmps.RWaferMp.Score > AlignC.WaferScore)
                    findCount = 2;
                if (findCount > 0)
                    break;
                iAlignCode = GV.Plc.ReadData16(GV.Plc.iAlign, 4);
                if (iAlignCode[1] == 0)
                {
                    iStartAlign = 0;
                    return;
                }
                GV.Plc.AlignXyyTableMove(_SearchingStepX * searchXY[i, 0], _SearchingStepY * searchXY[i, 1], 0);
                Thread.Sleep(250);
                Mat leftSearchImage = GV.LeftUpCam.Grab();
                Mat rightSearchImage = GV.RightUpCam.Grab();
                pmps = GetAllMatchPostion(leftSearchImage, rightSearchImage, "low");
                if (!(pmps.LWaferMp.Score > AlignC.WaferScore && pmps.RWaferMp.Score > AlignC.WaferScore))
                {
                    SaveAlignFailImage(leftSearchImage, rightSearchImage, $"UpWaferSearch", saveOnce: false);
                }
                writeStatus(string.Format(GV.Dlang.strMessageWaferScore, pmps.LWaferMp.Score.ToString("F2"), pmps.RWaferMp.Score.ToString("F2")));
            }

            if (findCount == 1)
            {
                if (pmps.LWaferMp.Score > AlignC.WaferScore)
                {
                    int[] motorSteps = CalcuteMaskWaferShiftAndReturnMotorSteps(pmps.LMaskMp, pmps.LWaferMp, pmps.LMaskMp, pmps.LWaferMp, pmps.PatternCenterDistance);
                    iAlignCode = GV.Plc.ReadData16(GV.Plc.iAlign, 4);
                    if (iAlignCode[1] == 0)
                    {
                        iStartAlign = 0;
                        return;
                    }
                    GV.Plc.AlignXyyTableMove(motorSteps[0], motorSteps[1], motorSteps[2]);
                    Thread.Sleep(250);
                }
                else if (pmps.RWaferMp.Score > AlignC.WaferScore)
                {
                    int[] motorSteps = CalcuteMaskWaferShiftAndReturnMotorSteps(pmps.RMaskMp, pmps.RWaferMp, pmps.RMaskMp, pmps.RWaferMp, pmps.PatternCenterDistance);
                    iAlignCode = GV.Plc.ReadData16(GV.Plc.iAlign, 4);
                    if (iAlignCode[1] == 0)
                    {
                        iStartAlign = 0;
                        return;
                    }
                    GV.Plc.AlignXyyTableMove(motorSteps[0], motorSteps[1], motorSteps[2]);
                    Thread.Sleep(250);
                }
            }

            double deltaXSum = 0, deltaYSum = 0;
            for (int i = 0; i < searchTheta.Length; i++)
            {
                Thread.Sleep(iDelay);
                iAlignCode = GV.Plc.ReadData16(GV.Plc.iAlign, 4);
                if (iAlignCode[1] == 0)
                {
                    iStartAlign = 0;
                    return;
                }
                Mat leftThetaImage = GV.LeftUpCam.Grab();
                Mat rightThetaImage = GV.RightUpCam.Grab();
                pmps = GetAllMatchPostion(leftThetaImage, rightThetaImage, "low");
                if (!(pmps.LWaferMp.Score > AlignC.WaferScore && pmps.RWaferMp.Score > AlignC.WaferScore))
                {
                    SaveAlignFailImage(leftThetaImage, rightThetaImage, $"UpWaferTheta", saveOnce: false);
                }

                writeStatus(string.Format(GV.Dlang.strMessageWaferScore, pmps.LWaferMp.Score.ToString("F2"), pmps.RWaferMp.Score.ToString("F2")));
                if (pmps.LWaferMp.Score > AlignC.WaferScore && pmps.RWaferMp.Score > AlignC.WaferScore)
                {
                    writeStatus(GV.Dlang.strMessageFindWaferSucc);
                    return;
                }

                int steps = (int)(_SearchingStepY * searchTheta[i] * 0.5);
                double degree = steps / GV.ZoomLensInfo.MotorStepsPerPixelDegree;
                double rad = degree * Math.PI / 180;

                int side = 0;
                if (pmps.LWaferMp.Score > AlignC.WaferScore) side = -1;
                if (pmps.RWaferMp.Score > AlignC.WaferScore) side = 1;

                double a, b, c;
                a = b = pmps.PatternCenterDistance;
                c = Math.Sqrt(a * a + b * b - 2 * a * b * Math.Cos(rad)) * 2;

                double deltaX = 0, deltaY = 0;

                if (side < 0 && i < rollBack1)
                {
                    deltaX -= c * Math.Sin(rad);
                    deltaY += c * Math.Cos(rad);
                }
                if (side > 0 && i > rollBack1)
                {
                    deltaX += c * Math.Sin(rad);
                    deltaY += c * Math.Cos(rad);
                }
                if (side > 0 && i < rollBack1)
                {
                    deltaX += c * Math.Sin(rad);
                    deltaY -= c * Math.Cos(rad);
                }
                if (side < 0 && i > rollBack1)
                {
                    deltaX -= c * Math.Sin(rad);
                    deltaY -= c * Math.Cos(rad);
                }
                deltaXSum += deltaX;
                deltaYSum += deltaY;
                if (i == rollBack1)
                {
                    GV.Plc.AlignXyyTableMove((int)Math.Round(deltaXSum), (int)Math.Round(deltaYSum), steps);
                    deltaXSum = deltaYSum = 0;
                    continue;
                }
                if (i == rollBack2)
                {
                    GV.Plc.AlignXyyTableMove((int)Math.Round(deltaXSum), (int)Math.Round(deltaYSum), steps);
                    deltaXSum = deltaYSum = 0;
                    continue;
                }
                GV.Plc.AlignXyyTableMove((int)Math.Round(deltaX), (int)Math.Round(deltaY * -1), steps);
            }

            pmps = GetAllMatchPostion(GV.LeftUpCam.Grab(), GV.RightUpCam.Grab(), "low");
            // writeStatus("L / R Wafer score : " + pmps.LWaferMp.Score.ToString("F2") + " , " + pmps.RWaferMp.Score.ToString("F2"));
            writeStatus(string.Format(GV.Dlang.strMessageWaferScore, pmps.LWaferMp.Score.ToString("F2"), pmps.RWaferMp.Score.ToString("F2")));
            if (pmps.LWaferMp.Score > AlignC.WaferScore && pmps.RWaferMp.Score > AlignC.WaferScore)
            {
                writeStatus(GV.Dlang.strMessageFindWaferSucc);
                return;
            }
            GV.Plc.SendAlignNG();
            writeStatus(GV.Dlang.strErrorCantFindMask);
            iStartAlign = 0;
        }

        private void SearchWaferBack()
        {
            int iHeight = (_recipe.LeftHighWaferMat.Height + _recipe.RightHighWaferMat.Height) / 2;
            int iWidth = (_recipe.LeftHighWaferMat.Width + _recipe.RightHighWaferMat.Width) / 2;
            double _SearchY = (backHeight - iHeight) * (GV.ZoomLensInfo.LeftDownUmPerPixelY + GV.ZoomLensInfo.RightDownUmPerPixelY) * 5;
            double _SearchX = (backWidth - iWidth) * (GV.ZoomLensInfo.LeftDownUmPerPixelX + GV.ZoomLensInfo.RightDownUmPerPixelX) * 5;
            int findCount = 0;
            // int[,] searchXY = new int[,] { { -1, -1 }, { 1, 0 }, { 1, 0 }, { 0, 1 }, { -1, 0 }, { -1, 0 }, { 0, 1 }, { 1, 0 }, { 1, 0 }, { -1, -1 } };
            int[,] searchXY = new int[,] {{ -1, -1 }, { 1, 0 }, { 1, 0 }, { 0, 1 }, { 0, 1 }, { -1, 0 }, { -1, 0 }, {0, -1}, { -1, 0 }, { 0, -1 },
                    { 0, -1 }, { 1, 0 }, { 1, 0 }, { 1, 0 }, { 1, 0 }, { 0, 1 }, { 0, 1 }, { 0, 1 }, { 0, 1 },{ -1, 0 }, { -1, 0 }, { -1, 0 }, { -1, 0 },{ 0, -1 } };

            // int[] searchTheta = new int[] { 1, 1, 1, -3, -1, -1, -1, 3 };
            int[] searchTheta = new int[] { 1, 1, 1, 1, 1, 1, -6, -1, -1, -1, -1, -1, -1, 6 };
            int rollBack1 = 6;
            int rollBack2 = 13;
            Mat leftInitImage = GV.LeftBackCam.Grab();
            Mat rightInitImage = GV.RightBackCam.Grab();
            ProductMatchPositions pmps = GetAllMatchPostion(leftInitImage, rightInitImage, "back");

            if (!(pmps.LWaferMp.Score > AlignC.WaferScore && pmps.RWaferMp.Score > AlignC.WaferScore))
            {
                SaveAlignFailImage(leftInitImage, rightInitImage, "DownWaferFail", saveOnce: false);
            }

            writeStatus(string.Format(GV.Dlang.strMessageWaferScore, pmps.LWaferMp.Score.ToString("F2"), pmps.RWaferMp.Score.ToString("F2")));
            if (pmps.LWaferMp.Score > AlignC.WaferScore && pmps.RWaferMp.Score > AlignC.WaferScore)
            {
                iWaferSearchOK = 1;
                return;
            }

            for (int i = 0; i < (searchXY.Length / 2) - 1; i++)
            {
                GV.TickCount = 0;
                writeStatus("Search Stage 1 Step : " + i.ToString());
                if (pmps.LWaferMp.Score > AlignC.WaferScore || pmps.RWaferMp.Score > AlignC.WaferScore)
                    findCount = 1;
                if (pmps.LWaferMp.Score > AlignC.WaferScore && pmps.RWaferMp.Score > AlignC.WaferScore)
                {
                    findCount = 2;
                    iWaferSearchOK = 1;
                    return;
                }

                if (findCount > 0)
                    break;
                iAlignCode = GV.Plc.ReadData16(GV.Plc.iAlign, 4);
                if (iAlignCode[1] == 0)
                {
                    iStartAlign = 0;
                    return;
                }
                int X = (int)_SearchX * searchXY[i, 0];
                int Y = (int)_SearchY * searchXY[i, 1];
                GV.Plc.AlignXyyTableMoveDown(X, Y, 0);
                Thread.Sleep(iDelay);
                Mat leftSearchImage = GV.LeftBackCam.Grab();
                Mat rightSearchImage = GV.RightBackCam.Grab();
                pmps = GetAllMatchPostion(leftSearchImage, rightSearchImage, "back");

                if (!(pmps.LWaferMp.Score > AlignC.WaferScore && pmps.RWaferMp.Score > AlignC.WaferScore))
                {
                    SaveAlignFailImage(leftSearchImage, rightSearchImage, $"DownWaferSearch", saveOnce: false);
                }
                writeStatus(string.Format(GV.Dlang.strMessageWaferScore, pmps.LWaferMp.Score.ToString("F2"), pmps.RWaferMp.Score.ToString("F2")));
            }

            if (pmps.LWaferMp.Score > AlignC.WaferScore)
            {
                // int[] motorSteps = CalcuteMaskWaferShiftAndReturnMotorStepsBack(pmps.LMaskMp, pmps.LWaferMp, pmps.LMaskMp, pmps.LWaferMp, pmps.PatternCenterDistance);
                int[] motorSteps = XyyMotorSteps(pmps.LMaskMp, pmps.LWaferMp, pmps.RMaskMp, pmps.RWaferMp);
                iAlignCode = GV.Plc.ReadData16(GV.Plc.iAlign, 4);
                if (iAlignCode[1] == 0)
                {
                    iStartAlign = 0;
                    return;
                }
                GV.Plc.AlignXyyTableMove(2, motorSteps[0], motorSteps[1], motorSteps[2]);
            }
            else if (pmps.RWaferMp.Score > AlignC.WaferScore)
            {
                // int[] motorSteps = CalcuteMaskWaferShiftAndReturnMotorStepsBack(pmps.RMaskMp, pmps.RWaferMp, pmps.RMaskMp, pmps.RWaferMp, pmps.PatternCenterDistance);
                int[] motorSteps = XyyMotorSteps(pmps.LMaskMp, pmps.LWaferMp, pmps.RMaskMp, pmps.RWaferMp);
                iAlignCode = GV.Plc.ReadData16(GV.Plc.iAlign, 4);
                if (iAlignCode[1] == 0)
                {
                    iStartAlign = 0;
                    return;
                }
                GV.Plc.AlignXyyTableMove(2, motorSteps[0], motorSteps[1], motorSteps[2]);
            }

            double deltaXSum = 0, deltaYSum = 0;
            for (int i = 0; i < searchTheta.Length; i++)
            {
                writeStatus("Search Stage 2 Step : " + i.ToString());
                GV.TickCount = 0;
                Thread.Sleep(iDelay);
                Mat leftThetaImage = GV.LeftBackCam.Grab();
                Mat rightThetaImage = GV.RightBackCam.Grab();
                pmps = GetAllMatchPostion(leftThetaImage, rightThetaImage, "back");

                if (!(pmps.LWaferMp.Score > AlignC.WaferScore && pmps.RWaferMp.Score > AlignC.WaferScore))
                {
                    SaveAlignFailImage(leftThetaImage, rightThetaImage, $"DownWaferTheta", saveOnce: false);
                }
                writeStatus(string.Format(GV.Dlang.strMessageWaferScore, pmps.LWaferMp.Score.ToString("F2"), pmps.RWaferMp.Score.ToString("F2")));
                if (pmps.LWaferMp.Score > AlignC.WaferScore && pmps.RWaferMp.Score > AlignC.WaferScore)
                {
                    iWaferSearchOK = 1;
                    writeStatus(GV.Dlang.strMessageFindWaferSucc);
                    return;
                }

                int steps = (int)((_SearchY * searchTheta[i]) * 0.5);
                double degree = steps / GV.ZoomLensInfo.MotorStepsPerPixelDegree;
                double rad = degree * Math.PI / 180;

                int side = 0;
                double deltaX = 0, deltaY = 0;
                if (pmps.LWaferMp.Score > AlignC.WaferScore)
                {
                    side = -1;
                }
                else if (pmps.RWaferMp.Score > AlignC.WaferScore)
                {
                    side = 1;
                }

                double a, b, c;
                a = b = pmps.PatternCenterDistance;
                c = Math.Sqrt(a * a + b * b - 2 * a * b * Math.Cos(rad)) * 2;

                if (side < 0 && i < rollBack1)
                {
                    deltaX -= c * Math.Sin(rad);
                    deltaY += c * Math.Cos(rad);
                }
                if (side > 0 && i > rollBack1)
                {
                    deltaX += c * Math.Sin(rad);
                    deltaY += c * Math.Cos(rad);
                }
                if (side > 0 && i < rollBack1)
                {
                    deltaX += c * Math.Sin(rad);
                    deltaY -= c * Math.Cos(rad);
                }
                if (side < 0 && i > rollBack1)
                {
                    deltaX -= c * Math.Sin(rad);
                    deltaY -= c * Math.Cos(rad);
                }
                deltaXSum += deltaX;
                deltaYSum += deltaY;
                if (i == rollBack1)
                {
                    iAlignCode = GV.Plc.ReadData16(GV.Plc.iAlign, 4);
                    if (iAlignCode[1] == 0)
                    {
                        iStartAlign = 0;
                        return;
                    }
                    GV.Plc.AlignXyyTableMoveDown((int)Math.Round(deltaXSum), (int)Math.Round(deltaYSum), steps);
                    deltaXSum = deltaYSum = 0;
                    continue;
                }
                if (i == rollBack2)
                {
                    iAlignCode = GV.Plc.ReadData16(GV.Plc.iAlign, 4);
                    if (iAlignCode[1] == 0)
                    {
                        iStartAlign = 0;
                        return;
                    }
                    GV.Plc.AlignXyyTableMoveDown((int)Math.Round(deltaXSum), (int)Math.Round(deltaYSum), steps);
                    deltaXSum = deltaYSum = 0;
                    continue;
                }
                iAlignCode = GV.Plc.ReadData16(GV.Plc.iAlign, 4);
                if (iAlignCode[1] == 0)
                {
                    iStartAlign = 0;
                    return;
                }
                GV.Plc.AlignXyyTableMoveDown((int)Math.Round(deltaX), (int)Math.Round(deltaY * -1), steps);
            }

            pmps = GetAllMatchPostion(GV.LeftBackCam.Grab(), GV.RightBackCam.Grab(), "back");
            writeStatus(string.Format(GV.Dlang.strMessageWaferScore, pmps.LWaferMp.Score.ToString("F2"), pmps.RWaferMp.Score.ToString("F2")));
            if (pmps.LWaferMp.Score > AlignC.WaferScore && pmps.RWaferMp.Score > AlignC.WaferScore)
            {
                iWaferSearchOK = 1;
                writeStatus(GV.Dlang.strMessageFindWaferSucc);
                return;
            }
            // GV.Plc.Send("WR DM6400.S 1");
            GV.MachineStatus = GV.Status.DownStandby;
            iWaferSearchOK = 0;
            iStartAlign = 0;
            GV.Plc.WriteMemory(GV.Plc.iUpAlignNG, true);
            writeStatus(GV.Dlang.strErrorCantFindWafer);
            // Debug.WriteLine("Can't find wafer.");
            // throw new MRException("Can't find wafer.");
        }

        public void writeStatus(string s)
        {
            Invoke((MethodInvoker)delegate { GM.WriteToStatusTextBox(s); });
        }

        public void DebugMessage(string s)
        {
            Invoke((MethodInvoker)delegate { GM.DebugMessage(s); });
        }

        /*
        private async void UpdateInfomationAccordingRecipeNumber()
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                GM.Quit();
            }
        }
        */

        public void HideTabAndSelectTabInEducationalMode()
        {
            tabControl1.SelectedTab = tabPage3;
            Thread.Sleep(500);
            tabPage1.Parent = null;
            tabPage2.Parent = null;
        }

        public void HideToolsInEducationalMode()
        {
            btnQuit.Visible = true;
            gbCameraParm.Visible = false;
            gbZoomLensParameters.Visible = false;
            gbMachingSetting.Visible = false;
            lbYOffset.Visible = true; lbMaxAlignTime.Visible = true; lbThetaOffset.Visible = true; lbStdXpre.Visible = true;
            buttonLeftBrightnessDecrease.Visible = true;
            buttonLeftBrightnessIncrease.Visible = true;
            buttonRightBrightnessDecrease.Visible = true;
            buttonRightBrightnessIncrease.Visible = true;
            //trackBarLeftBrightness1.Visible = true;
            //trackBarRightBrightness1.Visible = true;
        }

        private void ChangeLensMagnification(string side, int brightness, int[] motorSteps, int magnification)
        {
            try
            {
                CheckIfPlcStop();
                GV.Light.ChangeBrightness(side, brightness);
                if (side == "left")
                {
                    if (!GV.LeftZoomLens.MoveGoto(motorSteps, magnification))
                    {
                        GV.Plc.Send("WR DM6400.S 1");
                        throw new MRException("Error : Zoom lens fail.");
                    }
                    else
                    {
                        // GV.Plc.AlignCameraMove(lOffsetXSteps, rOffsetXSteps, lOffsetYSteps, rOffsetYSteps);
                    }
                }

                if (side == "right")
                {
                    if (!GV.RightZoomLens.MoveGoto(motorSteps, magnification))
                    {
                        GV.Plc.Send("WR DM6400.S 1");
                        throw new MRException("Error : Zoom lens fail.");
                    }
                }

            }
            catch
            {
                Debug.WriteLine("ChangeLensMagnification fail");
                // throw;
            }
        }

        private void ChangeLensMagnification(int magnification)
        {
            try
            {
                int oMagni = GV.LeftZoomLens.Magnification;
                if (GV.AppSettingParm.DebugMode == true)
                    writeStatus("iNowRecipe = " + iNowRecipe.ToString());
                GV.LeftZoomLens.Magnification = magnification;
                GV.RightZoomLens.Magnification = magnification;

                if (AlignC.LeftLight[magnification])
                {
                    GV.Light.ChangeBrightness("left", AlignC.LeftBrightness[magnification]);
                }
                else
                {
                    GV.Light.ChangeBrightness("left", 0);
                }
                if (AlignC.RightLight[magnification])
                {
                    GV.Light.ChangeBrightness("right", AlignC.RightBrightness[magnification]);
                }
                else
                {
                    GV.Light.ChangeBrightness("right", 0);
                }

                if ((iZoomAdj == 1) && (magnification == AlignC.AlignHighMagnification))
                {
                    Task tZoomAdj = Task.Factory.StartNew(() =>
                    {
                        GV.Plc.ZoomAdj(oMagni, magnification);
                    });
                }

                Task tLeft = Task.Factory.StartNew(() =>
                {
                    GV.LeftZoomLens.MoveGoto(GV.ZoomLensInfo.LeftMagnificationMotorSteps[magnification]);
                });

                Task tRight = Task.Factory.StartNew(() =>
                {
                    GV.RightZoomLens.MoveGoto(GV.ZoomLensInfo.RightMagnificationMotorSteps[magnification]);
                });

                Stopwatch sw = Stopwatch.StartNew();

                while (true)
                {
                    Thread.Sleep(100);
                    if (GV.LeftZoomLens.GetStatus() && GV.RightZoomLens.GetStatus())
                        return;
                    if (sw.ElapsedMilliseconds > 10000)
                    {
                        break;
                    }
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("ChangeLensMagnification fail");
            }
        }

        public void CheckIfPlcStop()
        {
            /*
            if (GV.Plc.Send("RD MR26304") == "1\r\n" || GV.Plc.Send("RD MR21803") == "1\r\n")
            {
                throw new MRException("Plc abort");
                // Debug.WriteLine("Plc abort");
            }
            */
        }

        private void ButtonManualPlc_Click(object sender, EventArgs e)
        {
            DialogManualPlcCom plc = new DialogManualPlcCom
            {
                StartPosition = FormStartPosition.CenterScreen
            };
            plc.ShowDialog();
            ParameterCheckLevel();
        }

        private void ButtonTuneLight_Click(object sender, EventArgs e)
        {
            DialogTuneLight light = new DialogTuneLight
            {
                StartPosition = FormStartPosition.CenterScreen
            };
            light.ShowDialog();
            ParameterCheckLevel();
        }

        private void ButtonTuneXyyTable_Click(object sender, EventArgs e)
        {

            Thread.Sleep(200);  // 等待相機完全停止
            DialogTuneTable dialog = new DialogTuneTable
            {
                StartPosition = FormStartPosition.CenterScreen
            };
            dialog.ShowDialog();

            GV.LeftUpCam.SetWindow(GV.LeftUpWindowOnParameterSettingPage);
            GV.RightUpCam.SetWindow(GV.RightUpWindowOnParameterSettingPage);
            ParameterCheckLevel();
        }

        private void BtTuneLens_Click(object sender, EventArgs e)
        {
            //DialogTuneLens tuneLens = new DialogTuneLens
            //{
            //    StartPosition = FormStartPosition.CenterParent
            //};
            //tuneLens.ShowDialog();

            //GV.LeftUpCam.SetWindow(GV.LeftUpWindowOnParameterSettingPage);
            //GV.RightUpCam.SetWindow(GV.RightUpWindowOnParameterSettingPage);
            //GV.RightUpCam.Live();
            //GV.LeftUpCam.Live();
            //var prevLeftWindow= GV.LeftUpWindowOnParameterSettingPage;
            //Action showTune = () =>
            //{
            //    using (DialogTuneLens tuneLens = new DialogTuneLens
            //    {
            //        StartPosition = FormStartPosition.CenterParent
            //    })
            //    {
            //        // 以 this 作為 owner (較安全)，確保在 UI 執行緒建立與顯示
            //        tuneLens.ShowDialog(this);
            //    }

            //    // 對話方塊關閉後，回復相機視窗到參數頁面（在 UI 執行緒執行）
            //    try
            //    {
            //        GV.LeftUpCam.SetWindow(GV.LeftUpWindowOnParameterSettingPage);
            //        GV.RightUpCam.SetWindow(GV.RightUpWindowOnParameterSettingPage);
            //        GV.RightUpCam.Live();
            //        GV.LeftUpCam.Live();
            //    }
            //    catch (Exception ex)
            //    {
            //        DebugMessage("BtTuneLens_Click restore camera window failed: " + ex.Message);
            //    }
            //};

            //if (this.InvokeRequired)
            //{
            //    this.Invoke(showTune);
            //}
            //else
            //{
            //    showTune();
            //}
            var prevLeftWindow = GV.LeftUpWindowOnParameterSettingPage;
            var prevRightWindow = GV.RightUpWindowOnParameterSettingPage;
            bool leftWasLive = false;
            bool rightWasLive = false;

            try
            {
                leftWasLive = GV.LeftUpCam?.IsLive ?? false;
                rightWasLive = GV.RightUpCam?.IsLive ?? false;
            }
            catch { /* 若相機物件為 null，略過 */ }

            // 先凍結相機，避免在切換 window 時發生 race condition
            try
            {
                GV.LeftUpCam?.Freeze();
                GV.RightUpCam?.Freeze();
            }
            catch (Exception ex)
            {
                DebugMessage("Freeze camera failed: " + ex.Message);
            }

            Action showTune = () =>
            {
                using (DialogTuneLens tuneLens = new DialogTuneLens
                {
                    StartPosition = FormStartPosition.CenterParent
                })
                {
                    // 以 this 作為 owner，確保在 UI 執行緒建立與顯示
                    tuneLens.ShowDialog(this);
                }
            };

            // 在 UI 執行緒顯示對話方塊
            if (this.InvokeRequired)
                this.Invoke(showTune);
            else
                showTune();

            // 對話方塊關閉後，安全地還原原本的 window 與相機狀態
            Action restoreWindows = () =>
            {
                try
                {
                    // 確保原始控制項仍存在且 handle 已建立
                    if (prevLeftWindow != null && !prevLeftWindow.IsHandleCreated)
                        prevLeftWindow.CreateControl();
                    if (prevRightWindow != null && !prevRightWindow.IsHandleCreated)
                        prevRightWindow.CreateControl();

                    // 還原全域參考（以防途中被變更）
                    GV.LeftUpWindowOnParameterSettingPage = prevLeftWindow;
                    GV.RightUpWindowOnParameterSettingPage = prevRightWindow;

                    // 將相機指回原始參數頁面控制項
                    if (prevLeftWindow != null)
                        GV.LeftUpCam?.SetWindow(prevLeftWindow);
                    if (prevRightWindow != null)
                        GV.RightUpCam?.SetWindow(prevRightWindow);

                    // 恢復相機是否為 Live 的狀態
                    if (leftWasLive)
                        GV.LeftUpCam?.Live();
                    else
                        GV.LeftUpCam?.Freeze();

                    if (rightWasLive)
                        GV.RightUpCam?.Live();
                    else
                        GV.RightUpCam?.Freeze();
                }
                catch (Exception ex)
                {
                    DebugMessage("Restore camera window failed: " + ex.Message);
                }
            };

            if (this.InvokeRequired)
                this.Invoke(restoreWindows);
            else
                restoreWindows();

            // 更新 UI 的參數頁面相關狀態
            ParameterCheckLevel();
        }

        private void ButtonSetting_Click(object sender, EventArgs e)
        {
            DialogAppSetting setting = new DialogAppSetting
            {
                StartPosition = FormStartPosition.CenterScreen
            };
            setting.ShowDialog();
        }

        private async void ButtonInitialLens_Click(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            bt.Enabled = false;
            await Task.Run(() =>
            {
                Task t1 = Task.Run(() =>
                {
                    GV.LeftZoomLens.MoveHome();
                });
                Task t2 = Task.Run(() =>
                {
                    GV.RightZoomLens.MoveHome();
                });
                t1.Wait(30000);
                t2.Wait(30000);
            });
            bt.Enabled = true;
            ParameterCheckLevel();
        }

        private void BtLWaferLocationUp_Click(object sender, EventArgs e)
        {
            // LeftLearnPatternVideoWindow.WaferMove(0, -1);
        }

        private void BtLWaferLocationLeft_Click(object sender, EventArgs e)
        {
            // LeftLearnPatternVideoWindow.WaferMove(-1, 0);
        }

        private void BtLWaferLocationDown_Click(object sender, EventArgs e)
        {
            // LeftLearnPatternVideoWindow.WaferMove(0, 1);
        }

        private void BtLWaferLocationRight_Click(object sender, EventArgs e)
        {
            // LeftLearnPatternVideoWindow.WaferMove(1, 0);
        }

        private void BtLMaskLocationUp_Click(object sender, EventArgs e)
        {
            // LeftLearnPatternVideoWindow.MaskMove(0, -1);
        }

        private void BtLMaskLocationLeft_Click(object sender, EventArgs e)
        {
            // LeftLearnPatternVideoWindow.MaskMove(-1, 0);
        }

        private void BtLMaskLocationDown_Click(object sender, EventArgs e)
        {
            // LeftLearnPatternVideoWindow.MaskMove(0, 1);
        }

        private void BtLMaskLocationRight_Click(object sender, EventArgs e)
        {
            // LeftLearnPatternVideoWindow.MaskMove(1, 0);
        }

        private void BtRWaferLocationUp_Click(object sender, EventArgs e)
        {
            // RightLearnPatternVideoWindow.WaferMove(0, -1);
        }

        private void BtRWaferLocationLeft_Click(object sender, EventArgs e)
        {
            // RightLearnPatternVideoWindow.WaferMove(-1, 0);
        }

        private void BtRWaferLocationDown_Click(object sender, EventArgs e)
        {
            // RightLearnPatternVideoWindow.WaferMove(0, 1);
        }

        private void BtRWaferLocationRight_Click(object sender, EventArgs e)
        {
            // RightLearnPatternVideoWindow.WaferMove(1, 0);
        }

        private void BtRMaskLocationUp_Click(object sender, EventArgs e)
        {
            // RightLearnPatternVideoWindow.MaskMove(0, -1);
        }

        private void BtRMaskLocationLeft_Click(object sender, EventArgs e)
        {
            // RightLearnPatternVideoWindow.MaskMove(-1, 0);
        }

        private void BtRMaskLocationDown_Click(object sender, EventArgs e)
        {
            // RightLearnPatternVideoWindow.MaskMove(0, 1);
        }

        private void BtRMaskLocationRight_Click(object sender, EventArgs e)
        {
            // RightLearnPatternVideoWindow.MaskMove(1, 0);
        }

        private string GetTemplateFileName(string patternAndSide)
        {
            string fileName = "Templates//" + patternAndSide + iNowRecipe.ToString() + ".bmp";
            return fileName;
        }

        private OpenCV3MatchUMat.AlignAlgorithm Algoritm(int a)
        {
            if (a == 0)
                return OpenCV3MatchUMat.AlignAlgorithm.TemplateMatch;
            else if (a == 1)
                return OpenCV3MatchUMat.AlignAlgorithm.EdgeMatch;
            else
                return OpenCV3MatchUMat.AlignAlgorithm.TemplateMatch;

        }

        private async void BtLeft_Click(object sender, EventArgs e)
        {
            bool isSuccess = false;
            Button bt = (Button)sender;
            await Task.Run(() =>
            {
                lock (Llock)
                {
                    GV.Light.ChangeBrightness("left", AlignC.LeftBrightness[bt.TabIndex]);
                    // trackBarLeftBrightness1.Value = GM.GetAlignCondition(iNowRecipe).LeftBrightness[bt.TabIndex];
                    isSuccess = GV.LeftZoomLens.MoveGoto(GV.ZoomLensInfo.LeftMagnificationMotorSteps, bt.TabIndex);
                }

            });
            if (isSuccess)
            {
                foreach (Button ctl in gbLeftZoomLens.Controls)
                    if (ctl.TabIndex == GV.LeftZoomLens.Magnification)
                        ctl.BackColor = Color.Honeydew;
                    else
                        ctl.BackColor = Color.Empty;
            }
        }

        private async void BtRight_Click(object sender, EventArgs e)
        {
            bool isSuccess = false;
            Button bt = (Button)sender;
            await Task.Run(() =>
            {
                lock (Rlock)
                {
                    GV.Light.ChangeBrightness("right", AlignC.RightBrightness[bt.TabIndex]);
                    // trackBarRightBrightness1.Value = GM.GetAlignCondition(iNowRecipe).RightBrightness[bt.TabIndex];
                    isSuccess = true;
                    isSuccess = GV.RightZoomLens.MoveGoto(GV.ZoomLensInfo.RightMagnificationMotorSteps, bt.TabIndex);
                }
            });
            if (isSuccess)
            {
                foreach (Button ctl in gbRightZoomLens.Controls)
                    if (ctl.TabIndex == GV.RightZoomLens.Magnification)
                        ctl.BackColor = Color.Honeydew;
                    else
                        ctl.BackColor = Color.Empty;
            }
        }

        private void TimerNowTime_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            string strTimeNow = now.ToShortDateString() + now.ToString("  HH:mm:ss");
            lbNowTime.Text = strTimeNow;
            cmLearnPatternUp1.SetTimeString = strTimeNow;
            cmLearnPatternBack1.setTimeString = strTimeNow;

            //int Hour = now.Hour;
            //int Min = now.Minute;
            //int Second = now.Second;

            GV.TickCount++;

            //  if ((Hour == 15) && (Min == 4) && (Second == 0))
            //if (GV.TickCount >= GV.AppSettingParm.RestartWait)
            //{
            //    if (GV.thCCDPad != null)
            //    {
            //        GV.thCCDPad.Abort();
            //    }

            //    if (GV.drawAlignMatch != null)
            //    {
            //        GV.drawAlignMatch.Abort();
            //    }

            //    if (GV.scanPlcThread != null)
            //    {
            //        GV.scanPlcThread.Abort();
            //    }

            //    if (GV.thPlcStatus != null)
            //    {
            //        Invoke((MethodInvoker)delegate () {
            //            GV.PlcStatusForm.Close();
            //        });
            //        GV.thPlcStatus.Abort();
            //    }

            //    if (GV.thPlcTest != null)
            //    {
            //        Invoke((MethodInvoker)delegate () {
            //            GV.PlcTestForm.Close();
            //        });
            //        GV.thPlcTest.Abort();
            //    }

            //    if (GV.LeftUpCam.IsOpen)
            //    {
            //        GV.LeftUpCam.Freeze();
            //        GV.LeftUpCam.StopCamera();
            //    }
            //    if (GV.RightUpCam.IsOpen)
            //    {
            //        GV.RightUpCam.Freeze();
            //        GV.RightUpCam.StopCamera();
            //    }

            //    if (GV.LeftBackCam.IsOpen)
            //    {
            //        GV.LeftBackCam.Freeze();
            //        GV.LeftBackCam.StopCamera();
            //    }

            //    if (GV.RightBackCam.IsOpen)
            //    {
            //        GV.RightBackCam.Freeze();
            //        GV.RightBackCam.StopCamera();
            //    }

            //    if (GV.Plc.IsOpen)
            //    {
            //        GV.Plc.Close();
            //    }

            //    GV.LeftZoomLens.Close();
            //    GV.RightZoomLens.Close();
            //    GV.Light.Close();
            //    GV.RingLight.Close();

            //    Thread.Sleep(1000);

            //    Application.Restart();
            //}
        }

        private void RadioButtonLeftCamera_CheckedChanged(object sender, EventArgs e)
        {
            ChangeCamParam();
        }

        private void radioButtonRightCamera_CheckedChanged(object sender, EventArgs e)
        {
            ChangeCamParam();
        }

        private void ButtonCameraParameterResotre_Click(object sender, EventArgs e)
        {
            SentechNetToMat cam;
            if (radioButtonLeftCamera.Checked)
            {
                cam = GV.LeftUpCam;
                cam.CameraGain(GV.BaslerCamParm.LeftUpGain);
                cam.CameraGamma(GV.BaslerCamParm.LeftUpGamma);
                cam.CameraBlackLevel(GV.BaslerCamParm.LeftUpBlackLevel);
                cam.CameraExposureTime(GV.BaslerCamParm.LeftUpExposureTime);
            }

            if (radioButtonRightCamera.Checked)
            {
                cam = GV.RightUpCam;
                cam.CameraGain(GV.BaslerCamParm.RightUpGain);
                cam.CameraGamma(GV.BaslerCamParm.RightUpGamma);
                cam.CameraBlackLevel(GV.BaslerCamParm.RightUpBlackLevel);
                cam.CameraExposureTime(GV.BaslerCamParm.RightUpExposureTime);
            }

            SentechNetToMat scam;
            if (radioButtonLeftBackCam.Checked)
            {
                scam = GV.LeftBackCam;
                scam.CameraGain(GV.BaslerCamParm.LeftBackGain);
                scam.CameraGamma(GV.BaslerCamParm.LeftBackGamma);
                scam.CameraBlackLevel(GV.BaslerCamParm.LeftBackBlackLevel);
                scam.CameraExposureTime(GV.BaslerCamParm.LeftBackExposureTime);
            }

            if (radioButtonRightBackCam.Checked)
            {
                scam = GV.RightBackCam;
                scam.CameraGain(GV.BaslerCamParm.RightBackGain);
                scam.CameraGamma(GV.BaslerCamParm.RightBackGamma);
                scam.CameraBlackLevel(GV.BaslerCamParm.RightBackBlackLevel);
                scam.CameraExposureTime(GV.BaslerCamParm.RightBackExposureTime);
            }
        }

        private void ButtonCameraParmSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Sure to save camera parameters ?", "SAVE", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

            GV.BaslerCamParm.LeftUpGain = GV.LeftUpCam.CameraGainPercentageValue;
            GV.BaslerCamParm.LeftUpGamma = GV.LeftUpCam.CameraGammaPercentageValue;
            GV.BaslerCamParm.LeftUpBlackLevel = GV.LeftUpCam.CameraBlackLevelPercentageValue;
            GV.BaslerCamParm.LeftUpExposureTime = GV.LeftUpCam.CameraExposureTimePercentageValue;

            GV.BaslerCamParm.RightUpGain = GV.RightUpCam.CameraGainPercentageValue;
            GV.BaslerCamParm.RightUpGamma = GV.RightUpCam.CameraGammaPercentageValue;
            GV.BaslerCamParm.RightUpBlackLevel = GV.RightUpCam.CameraBlackLevelPercentageValue;
            GV.BaslerCamParm.RightUpExposureTime = GV.RightUpCam.CameraExposureTimePercentageValue;

            if (GV.bBacksideCCD == true)
            {
                GV.BaslerCamParm.LeftBackGain = GV.LeftBackCam.CameraGainPercentageValue;
                GV.BaslerCamParm.LeftBackGamma = GV.LeftBackCam.CameraGammaPercentageValue;
                GV.BaslerCamParm.LeftBackBlackLevel = GV.LeftBackCam.CameraBlackLevelPercentageValue;
                GV.BaslerCamParm.LeftBackExposureTime = GV.LeftBackCam.CameraExposureTimePercentageValue;

                GV.BaslerCamParm.RightBackGain = GV.RightBackCam.CameraGainPercentageValue;
                GV.BaslerCamParm.RightBackGamma = GV.RightBackCam.CameraGammaPercentageValue;
                GV.BaslerCamParm.RightBackBlackLevel = GV.RightBackCam.CameraBlackLevelPercentageValue;
                GV.BaslerCamParm.RightBackExposureTime = GV.RightBackCam.CameraExposureTimePercentageValue;
            }

            GM.WriteBalserCamParmXml("BaslerCamParm.xml", GV.BaslerCamParm);
        }

        private void RadioButtonLeftBackCam_CheckedChanged(object sender, EventArgs e)
        {
            ChangeCamParam();
        }

        private void trackBarCameraParmeters_Scroll(object sender, EventArgs e)
        {
            if (radioButtonLeftCamera.Checked == true)
            {
                if (comboBoxCameraParameter.SelectedIndex == 0)
                {
                    GV.LeftUpCam.CameraGainPercentageValue = trackBarCameraParmeters.Value;
                }
                else if (comboBoxCameraParameter.SelectedIndex == 1)
                {
                    GV.LeftUpCam.CameraGammaPercentageValue = trackBarCameraParmeters.Value;
                }
                else if (comboBoxCameraParameter.SelectedIndex == 2)
                {
                    GV.LeftUpCam.CameraBlackLevelPercentageValue = trackBarCameraParmeters.Value;
                }
                else if (comboBoxCameraParameter.SelectedIndex == 3)
                {
                    GV.LeftUpCam.CameraExposureTimePercentageValue = trackBarCameraParmeters.Value;
                }
            }
            else if (radioButtonRightCamera.Checked == true)
            {
                if (comboBoxCameraParameter.SelectedIndex == 0)
                {
                    GV.RightUpCam.CameraGainPercentageValue = trackBarCameraParmeters.Value;
                }
                else if (comboBoxCameraParameter.SelectedIndex == 1)
                {
                    GV.RightUpCam.CameraGammaPercentageValue = trackBarCameraParmeters.Value;
                }
                else if (comboBoxCameraParameter.SelectedIndex == 2)
                {
                    GV.RightUpCam.CameraBlackLevelPercentageValue = trackBarCameraParmeters.Value;
                }
                else if (comboBoxCameraParameter.SelectedIndex == 3)
                {
                    GV.RightUpCam.CameraExposureTimePercentageValue = trackBarCameraParmeters.Value;
                }
            }
            else if (radioButtonLeftBackCam.Checked == true)
            {
                if (comboBoxCameraParameter.SelectedIndex == 0)
                {
                    GV.LeftBackCam.CameraGainPercentageValue = trackBarCameraParmeters.Value;
                }
                else if (comboBoxCameraParameter.SelectedIndex == 1)
                {
                    GV.LeftBackCam.CameraGammaPercentageValue = trackBarCameraParmeters.Value;
                }
                else if (comboBoxCameraParameter.SelectedIndex == 2)
                {
                    GV.LeftBackCam.CameraBlackLevelPercentageValue = trackBarCameraParmeters.Value;
                }
                else if (comboBoxCameraParameter.SelectedIndex == 3)
                {
                    GV.LeftBackCam.CameraExposureTimePercentageValue = trackBarCameraParmeters.Value;
                }
            }
            else if (radioButtonRightBackCam.Checked == true)
            {
                if (comboBoxCameraParameter.SelectedIndex == 0)
                {
                    GV.RightBackCam.CameraGainPercentageValue = trackBarCameraParmeters.Value;
                }
                else if (comboBoxCameraParameter.SelectedIndex == 1)
                {
                    GV.RightBackCam.CameraGammaPercentageValue = trackBarCameraParmeters.Value;
                }
                else if (comboBoxCameraParameter.SelectedIndex == 2)
                {
                    GV.RightBackCam.CameraBlackLevelPercentageValue = trackBarCameraParmeters.Value;
                }
                else if (comboBoxCameraParameter.SelectedIndex == 3)
                {
                    GV.RightBackCam.CameraExposureTimePercentageValue = trackBarCameraParmeters.Value;
                }
            }
        }

        private void comboBoxCameraParameter_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeCamParam();
        }

        private void ChangeCamParam()
        {
            if (radioButtonLeftCamera.Checked == true)
            {
                if (comboBoxCameraParameter.SelectedIndex == 0)
                {
                    trackBarCameraParmeters.Value = (int)GV.LeftUpCam.CameraGainPercentageValue;
                }
                else if (comboBoxCameraParameter.SelectedIndex == 1)
                {
                    trackBarCameraParmeters.Value = (int)GV.LeftUpCam.CameraGammaPercentageValue;
                }
                else if (comboBoxCameraParameter.SelectedIndex == 2)
                {
                    trackBarCameraParmeters.Value = (int)GV.LeftUpCam.CameraBlackLevelPercentageValue;
                }
                else if (comboBoxCameraParameter.SelectedIndex == 3)
                {
                    trackBarCameraParmeters.Value = (int)GV.LeftUpCam.CameraExposureTimePercentageValue;
                }
            }
            else if (radioButtonRightCamera.Checked == true)
            {
                if (comboBoxCameraParameter.SelectedIndex == 0)
                {
                    trackBarCameraParmeters.Value = (int)GV.RightUpCam.CameraGainPercentageValue;
                }
                else if (comboBoxCameraParameter.SelectedIndex == 1)
                {
                    trackBarCameraParmeters.Value = (int)GV.RightUpCam.CameraGammaPercentageValue;
                }
                else if (comboBoxCameraParameter.SelectedIndex == 2)
                {
                    trackBarCameraParmeters.Value = (int)GV.RightUpCam.CameraBlackLevelPercentageValue;
                }
                else if (comboBoxCameraParameter.SelectedIndex == 3)
                {
                    trackBarCameraParmeters.Value = (int)GV.RightUpCam.CameraExposureTimePercentageValue;
                }
            }
            else if (radioButtonLeftBackCam.Checked == true)
            {
                if (comboBoxCameraParameter.SelectedIndex == 0)
                {
                    trackBarCameraParmeters.Value = (int)GV.LeftBackCam.CameraGainPercentageValue;
                }
                else if (comboBoxCameraParameter.SelectedIndex == 1)
                {
                    trackBarCameraParmeters.Value = (int)GV.LeftBackCam.CameraGammaPercentageValue;
                }
                else if (comboBoxCameraParameter.SelectedIndex == 2)
                {
                    trackBarCameraParmeters.Value = (int)GV.LeftBackCam.CameraBlackLevelPercentageValue;
                }
                else if (comboBoxCameraParameter.SelectedIndex == 3)
                {
                    trackBarCameraParmeters.Value = (int)GV.LeftBackCam.CameraExposureTimePercentageValue;
                }
            }
            else if (radioButtonRightBackCam.Checked == true)
            {
                if (comboBoxCameraParameter.SelectedIndex == 0)
                {
                    trackBarCameraParmeters.Value = (int)GV.RightBackCam.CameraGainPercentageValue;
                }
                else if (comboBoxCameraParameter.SelectedIndex == 1)
                {
                    trackBarCameraParmeters.Value = (int)GV.RightBackCam.CameraGammaPercentageValue;
                }
                else if (comboBoxCameraParameter.SelectedIndex == 2)
                {
                    trackBarCameraParmeters.Value = (int)GV.RightBackCam.CameraBlackLevelPercentageValue;
                }
                else if (comboBoxCameraParameter.SelectedIndex == 3)
                {
                    trackBarCameraParmeters.Value = (int)GV.RightBackCam.CameraExposureTimePercentageValue;
                }
            }
        }

        private void radioButtonRightBackCam_CheckedChanged(object sender, EventArgs e)
        {
            ChangeCamParam();
        }

        private void btRMaskLocationSave_Click(object sender, EventArgs e)
        {
            /*
            if (radioButtonLowMagnification.Checked)
            {
                if (MessageBox.Show("Sure to save Right Low mask template ?", "RLMaskTemplateSave", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
                GV.RightLowMaskMat[iNowRecipe] = new Mat(GV.RightUpCam.Grab(), RightLearnPatternVideoWindow.GetMaskRectangle());
                pbRMaskImage.Image = GV.RightLowMaskMat[iNowRecipe].ToBitmap();
                CvInvoke.Imwrite(GetTemplateFileName("RLM"), GV.RightLowMaskMat[iNowRecipe]);
                AlignC.RLMaskAlgorithm = Algoritm(cbRMaskAlgorithm.SelectedIndex);
                AlignC.AlignLowMagnification = GV.RightZoomLens.Magnification;
            }
            else if (radioButtonHighMagnification.Checked)
            {
                if (MessageBox.Show("Sure to save Right High mask template ?", "RHMaskTemplateSave", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
                GV.RightHighMaskMat[iNowRecipe] = new Mat(GV.RightUpCam.Grab(), RightLearnPatternVideoWindow.GetMaskRectangle());
                pbRMaskImage.Image = GV.RightHighMaskMat[iNowRecipe].ToBitmap();
                CvInvoke.Imwrite(GetTemplateFileName("RHM"), GV.RightHighMaskMat[iNowRecipe]);
                AlignC.RHMaskAlgorithm = Algoritm(cbRMaskAlgorithm.SelectedIndex);
                AlignC.AlignHighMagnification = GV.RightZoomLens.Magnification;
            }
            else
            {
                MessageBox.Show("Select magnification first.", "RMaskTemplateSave", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            GM.WriteAlignConditionsListToXml("AlignConditions.xml", AlignC, iNowRecipe);
            */
        }


        // private async void radioButtonMagnification_CheckedChanged(object sender, EventArgs e)
        //private void radioButtonMagnification_CheckedChanged(object sender, EventArgs e)
        //{
        //    // bool isSuccess = false;

        //    if (GV.LeftLowWaferMat[iNowRecipe] == null)
        //    {
        //        GV.LeftLowWaferMat[iNowRecipe] = GV.LeftLowWaferMat[1];
        //        GV.LeftLowMaskMat[iNowRecipe] = GV.LeftLowMaskMat[1];
        //        GV.RightLowWaferMat[iNowRecipe] = GV.RightLowWaferMat[1];
        //        GV.RightLowMaskMat[iNowRecipe] = GV.RightLowMaskMat[1];
        //        GV.LeftHighWaferMat[iNowRecipe] = GV.LeftHighWaferMat[1];
        //        GV.LeftHighMaskMat[iNowRecipe] = GV.LeftHighMaskMat[1];
        //        GV.RightHighWaferMat[iNowRecipe] = GV.RightHighWaferMat[1];
        //        GV.RightHighMaskMat[iNowRecipe] = GV.RightHighMaskMat[1];
        //        //             } else
        //    }
        //    {
        //        /*
        //        if (radioButtonLowMagnification.Checked == true)
        //        {
        //            if (GV.LeftLowWaferMat[iNowRecipe] != null)
        //            {
        //                pbLWaferImage.Image = GV.LeftLowWaferMat[iNowRecipe].ToBitmap();
        //                pbLMaskImage.Image = GV.LeftLowMaskMat[iNowRecipe].ToBitmap();
        //                pbRWaferImage.Image = GV.RightLowWaferMat[iNowRecipe].ToBitmap();
        //                pbRMaskImage.Image = GV.RightLowMaskMat[iNowRecipe].ToBitmap();
        //                await Task.Run(() =>
        //                {
        //                    lock (Llock)
        //                    {
        //                        GV.Light.ChangeBrightness("left", GM.GetAlignCondition(iNowRecipe).LeftBrightness[AlignC.AlignLowMagnification]);
        //                        trackBarLeftBrightness.Value = GM.GetAlignCondition(iNowRecipe).LeftBrightness[AlignC.AlignLowMagnification];
        //                        isSuccess = GV.LeftZoomLens.MoveGoto(GV.ZoomLensInfo.LeftMagnificationMotorSteps, AlignC.AlignLowMagnification);
        //                        GV.Light.ChangeBrightness("right", GM.GetAlignCondition(iNowRecipe).RightBrightness[AlignC.AlignLowMagnification]);
        //                        trackBarRightBrightness.Value = GM.GetAlignCondition(iNowRecipe).RightBrightness[AlignC.AlignLowMagnification];
        //                        isSuccess = GV.RightZoomLens.MoveGoto(GV.ZoomLensInfo.RightMagnificationMotorSteps, AlignC.AlignLowMagnification);
        //                    }
        //                });
        //            }
        //        }
        //        else if (radioButtonHighMagnification.Checked == true)
        //        {
        //            if (GV.LeftHighWaferMat[iNowRecipe] != null)
        //            {
        //                pbLWaferImage.Image = GV.LeftHighWaferMat[iNowRecipe].ToBitmap();
        //                pbLMaskImage.Image = GV.LeftHighMaskMat[iNowRecipe].ToBitmap();
        //                pbRWaferImage.Image = GV.RightHighWaferMat[iNowRecipe].ToBitmap();
        //                pbRMaskImage.Image = GV.RightHighMaskMat[iNowRecipe].ToBitmap();
        //                await Task.Run(() =>
        //                {
        //                    lock (Llock)
        //                    {
        //                        GV.Light.ChangeBrightness("left", GM.GetAlignCondition(iNowRecipe).LeftBrightness[AlignC.AlignHighMagnification]);
        //                        trackBarLeftBrightness.Value = GM.GetAlignCondition(iNowRecipe).LeftBrightness[AlignC.AlignHighMagnification];
        //                        isSuccess = GV.LeftZoomLens.MoveGoto(GV.ZoomLensInfo.LeftMagnificationMotorSteps, AlignC.AlignHighMagnification);
        //                        GV.Light.ChangeBrightness("right", GM.GetAlignCondition(iNowRecipe).RightBrightness[AlignC.AlignHighMagnification]);
        //                        trackBarRightBrightness.Value = GM.GetAlignCondition(iNowRecipe).RightBrightness[AlignC.AlignHighMagnification];
        //                        isSuccess = GV.RightZoomLens.MoveGoto(GV.ZoomLensInfo.RightMagnificationMotorSteps, AlignC.AlignHighMagnification);
        //                    }
        //                });
        //            }
        //        }
        //        */
        //    }
        //}

        private void ButtonSaveAlignMagnification_Click(object sender, EventArgs e)
        {
            AlignC.AlignLowMagnification = cbLowMagnification.SelectedIndex;
            AlignC.AlignHighMagnification = cbHighMagnification.SelectedIndex;
            GM.WriteAlignConditionsListToXml("AlignConditions.xml", AlignC, iNowRecipe);
        }

        private void UcOpenPad_Click(object sender, EventArgs e)
        {
            if (GV.thCCDPad == null)
            {
                GV.thCCDPad = new Thread(delegate ()
                {
                    GV.CCDPadForm = new CCDPad();
                    GV.CCDPadForm.Show();
                    System.Windows.Threading.Dispatcher.Run();
                });

                StopCheckPLc = true;
                GV.thCCDPad.SetApartmentState(ApartmentState.STA); // needs to be STA or throws exception
                GV.thCCDPad.Start();
            }
            else
            {
                Invoke(new Action(() => GV.CCDPadForm.BringToFront()));
            }
        }

        public void UcOpenPlcTest()
        {
            if (GV.thPlcTest == null)
            {
                GV.thPlcTest = new Thread(delegate ()
                {
                    GV.PlcTestForm = new PlcTest();
                    GV.PlcTestForm.Show();
                    System.Windows.Threading.Dispatcher.Run();
                });

                GV.thPlcTest.SetApartmentState(ApartmentState.STA); // needs to be STA or throws exception
                GV.thPlcTest.Start();
            }
            else
            {
                Invoke(new Action(() => GV.PlcTestForm.BringToFront()));
            }
        }

        public void UcOpenPlcStatus()
        {
            if (GV.thPlcStatus == null)
            {
                GV.thPlcStatus = new Thread(delegate ()
                {
                    GV.PlcStatusForm = new PlcStatus();
                    GV.PlcStatusForm.Show();
                    System.Windows.Threading.Dispatcher.Run();
                });

                GV.thPlcStatus.SetApartmentState(ApartmentState.STA); // needs to be STA or throws exception
                GV.thPlcStatus.Start();
            }
            else
            {
                Invoke(new Action(() => GV.PlcStatusForm.BringToFront()));
            }
        }

        private async void UcAlign_Click(object sender, EventArgs e)
        {
            if (GV.Plc.ReadMemory(GV.Plc.iUpAlign))
            {
                await Align();
            }
            else if (GV.Plc.ReadMemory(GV.Plc.iDownAlign))
            {
                // TestAlign = 1;
                await DAlign();
            }
        }

        private void UcGoRecipe_Click(object sender, EventArgs e)
        {
            GV.Plc.MoveCCDChuckToRecipe();
        }

        private void UcSaveRecipe_Click(object sender, EventArgs e)
        {
            if (GV.NowRecipe == null)
            {
                GV.NowRecipe = new RecipeInfo();
            }

            int[] tempR = GV.Plc.ReadData32(GV.Plc.iRMemoryAddress, GV.Plc.iRCount);
            GV.NowRecipe.iWECX = tempR[GV.Plc.iiDChuckX];
            GV.NowRecipe.iWECY1 = tempR[GV.Plc.iiDChuckY1];
            GV.NowRecipe.iWECY2 = tempR[GV.Plc.iiDChuckY2];


            GV.NowRecipe.iUpCCDBigY = tempR[GV.Plc.iiDSBigY];
            GV.NowRecipe.iUpCCDLeftX = tempR[GV.Plc.iiDSUpLeftX];
            GV.NowRecipe.iUpCCDLeftY = tempR[GV.Plc.iiDSUpLeftY];
            GV.NowRecipe.iUpCCDRightX = tempR[GV.Plc.iiDSUpRightX];
            GV.NowRecipe.iUpCCDRightY = tempR[GV.Plc.iiDSUpRightY];

            try
            {
                var serializer = new XmlSerializer(typeof(RecipeInfo));
                using (var stream = File.Create("RecipeInfo.xml"))
                {
                    serializer.Serialize(stream, GV.NowRecipe);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "RecipeInfo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                GM.WriteToStatusTextBox(ex.Message);
            }
        }

        private void UcGetRecipe_Click(object sender, EventArgs e)
        {
            GetRecipeXml();
        }

        private void GetRecipeXml()
        {
            RecipeInfo lRecipeInfo = new RecipeInfo();
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(RecipeInfo));
                using (FileStream stream = File.OpenRead("RecipeInfo.xml"))
                {
                    lRecipeInfo = (RecipeInfo)serializer.Deserialize(stream);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "RecipeInfo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                GM.WriteToStatusTextBox(ex.Message);
            }
            GV.NowRecipe = lRecipeInfo;
        }

        private void UcSaveImage_Click(object sender, EventArgs e)
        {

            if (GV.skView == 0)
            {
                Mat m = GV.LeftUpCam.Grab();
                Cv2.ImWrite(GetTemplateFileName("LUV"), m);
                m = GV.RightUpCam.Grab();
                Cv2.ImWrite(GetTemplateFileName("RUV"), m);
            }
            else if (GV.skView == 1)
            {
                Mat m = GV.LeftBackCam.Grab();
                Cv2.ImWrite(GetTemplateFileName("LBV"), m);
                m = GV.RightBackCam.Grab();
                Cv2.ImWrite(GetTemplateFileName("RBV"), m);
            }
        }

        private async void UcWaferAlign_Click(object sender, EventArgs e)
        {
            if ((RightMask == null) || (LeftMask == null))
            {
                LeftMask = Cv2.ImRead(GetTemplateFileName("LeftMask"), ImreadModes.Grayscale);
                RightMask = Cv2.ImRead(GetTemplateFileName("RightMask"), ImreadModes.Grayscale);
            }
            await DAlignWafer();
        }

        //private void MaskImage_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (MaskImage.Checked)
        //    {
        //        if ((RightMask == null) || (LeftMask == null))
        //        {
        //            LeftMask = Cv2.ImRead(GetTemplateFileName("LeftMask"), ImreadModes.Grayscale);
        //            RightMask = Cv2.ImRead(GetTemplateFileName("RightMask"), ImreadModes.Grayscale);
        //            GV.LeftMaskMat = LeftMask;
        //            GV.RightMaskMat = RightMask;
        //        }
        //        skLeftAlign.SetShowMask(true, AlignC.dLalpha, LeftMask);
        //        skRightAlign.SetShowMask(true, AlignC.dRalpha, RightMask);
        //    }
        //    else
        //    {
        //        if ((RightMask == null) || (LeftMask == null))
        //        {
        //            LeftMask = Cv2.ImRead(GetTemplateFileName("LeftMask"), ImreadModes.Grayscale);
        //            RightMask = Cv2.ImRead(GetTemplateFileName("RightMask"), ImreadModes.Grayscale);
        //            skLeftAlign.SetShowMask(true, AlignC.dLalpha, LeftMask);
        //            skRightAlign.SetShowMask(true, AlignC.dRalpha, RightMask);
        //            GV.LeftMaskMat = LeftMask;
        //            GV.RightMaskMat = RightMask;
        //        }
        //        skLeftAlign.SetShowMask(false, 0, LeftMask);
        //        skRightAlign.SetShowMask(false, 0, RightMask);
        //    }
        //}
        private void MaskImage_CheckedChanged(object sender, EventArgs e)
        {
            // ✅ 讀取當前的 Up/Down 模式
            int iUpDownAlign = GV.Plc.ReadData16(GV.Plc.iUpDownAlign);

            // ✅ 判斷是否應該顯示透明度調整控制項
            // 只有在 MaskImage 勾選且為 Bottom CCD 模式 (iUpDownAlign == 1) 時才顯示
            bool shouldShowControls = MaskImage.Checked && (iUpDownAlign == 1);

            // ✅ 設定所有透明度調整控制項的可見性
            tBLShowImage.Visible = shouldShowControls;
            tBRShowImage.Visible = shouldShowControls;
            btLShowImagePlus.Visible = shouldShowControls;
            btLShowImageMinus.Visible = shouldShowControls;
            btRShowImagePlus.Visible = shouldShowControls;
            btRShowImageMinus.Visible = shouldShowControls;
            //btLShowImageSave.Visible = shouldShowControls;
            //btRShowImageSave.Visible = shouldShowControls;
            lbLShowImage.Visible = shouldShowControls;
            lbRShowImage.Visible = shouldShowControls;

            if (MaskImage.Checked)
            {
                // ✅ 只有在 Bottom CCD 模式才啟用 TrackBar
                if (iUpDownAlign == 1)
                {
                    tBLShowImage.Enabled = true;
                    tBRShowImage.Enabled = true;

                    // ✅ 載入儲存的透明度值
                    if (GV.acar != null && GV.NowRecipeNumber >= 0 && GV.NowRecipeNumber < GV.acar.Length)
                    {
                        // 從 AlignCondition 載入透明度設定
                        int iL = (int)(GV.acar[GV.NowRecipeNumber].dLalphaAlign * 100);
                        int iR = (int)(GV.acar[GV.NowRecipeNumber].dRalphaAlign * 100);

                        // 限制範圍 0-100
                        iL = Math.Max(0, Math.Min(100, iL));
                        iR = Math.Max(0, Math.Min(100, iR));

                        tBLShowImage.Value = iL;
                        tBRShowImage.Value = iR;
                        iLalphaAlign = iL;
                        iRalphaAlign = iR;

                        _dLalphaAlign = (double)iL / 100;
                        _dRalphaAlign = (double)iR / 100;
                    }
                }

                // ✅ 設定 Mask 顯示（不論 iUpDownAlign 值都執行）
                if (_LeftMask != null && !_LeftMask.Empty())
                {
                    skLeftAlign.SetShowMask(true, _dLalphaAlign, _LeftMask);
                }

                if (_RightMask != null && !_RightMask.Empty())
                {
                    skRightAlign.SetShowMask(true, _dRalphaAlign, _RightMask);
                }
            }
            else
            {
                // ✅ 停用 TrackBar 並清除 Mask 顯示
                tBLShowImage.Enabled = false;
                tBRShowImage.Enabled = false;
                skLeftAlign.SetShowMask(false, 0, null);
                skRightAlign.SetShowMask(false, 0, null);
            }
        }

        public void TurnOnLight()
        {
            if (AlignC.LeftLight[AlignC.AlignLowMagnification])
            {
                GV.Light.ChangeBrightness("left", AlignC.LeftBrightness[AlignC.AlignLowMagnification]);
            }
            else
            {
                GV.Light.ChangeBrightness("left", 0);
            }

            if (AlignC.RightLight[AlignC.AlignLowMagnification])
            {
                GV.Light.ChangeBrightness("right", AlignC.RightBrightness[AlignC.AlignLowMagnification]);
            }
            else
            {
                GV.Light.ChangeBrightness("right", 0);
            }

            if (GV.skView == 1 && AlignC.UpBackAlign == 2)
            {
                // 根據 GV.NowWaferMask 決定使用哪種亮度
                if (GV.NowWaferMask == 1) // Mask Only
                {
                    GV.Light.ChangeBrightness("leftback", AlignC.LBMaskBright);
                    GV.Light.ChangeBrightness("rightback", AlignC.RBMaskBright);
                }
                else if (GV.NowWaferMask == 2 || GV.NowWaferMask == 3) // Wafer
                {
                    GV.Light.ChangeBrightness("leftback", AlignC.LBWaferBright);
                    GV.Light.ChangeBrightness("rightback", AlignC.RBWaferBright);
                }
                else
                {
                    // 預設使用 Mask 亮度
                    GV.Light.ChangeBrightness("leftback", AlignC.LBMaskBright);
                    GV.Light.ChangeBrightness("rightback", AlignC.RBMaskBright);
                }
            }
            else
            {
                GV.Light.ChangeBrightness("leftback", 0);
                GV.Light.ChangeBrightness("rightback", 0);
            }

            if (AlignC.LRingLight[AlignC.AlignLowMagnification])
            {
                GV.RingLight.ChangeBrightness("left", AlignC.LeftRingBrightness[AlignC.AlignLowMagnification]);
            }
            else
            {
                GV.RingLight.ChangeBrightness("left", 0);
            }

            if (AlignC.RRingLight[AlignC.AlignLowMagnification])
            {
                GV.RingLight.ChangeBrightness("right", AlignC.RightRingBrightness[AlignC.AlignLowMagnification]);
            }
            else
            {
                GV.RingLight.ChangeBrightness("right", 0);
            }
        }

        private void ValueChanged(object sender, EventArgs e)
        {
            NValueChanged();
        }

        private void NValueChanged()
        {
            GV.TickCount = 0;

            Color Ocolor = btSaveAlignParm.BackColor;
            Color Ncolor;

            if (AlignC.MaskSocre != (float)nudMaskScore.Value)
            {
                Ncolor = Color.LightGreen;
            }
            else if (AlignC.WaferScore != (float)nudWaferScore.Value)
            {
                Ncolor = Color.LightGreen;
            }
            else if (AlignC.XOffset != (double)nudXOffset.Value)
            {
                Ncolor = Color.LightGreen;
            }
            else if (AlignC.YLOffset != (double)nudYLOffest.Value)
            {
                Ncolor = Color.LightGreen;
            }
            else if (AlignC.YROffset != (double)nudYROffest.Value)
            {
                Ncolor = Color.LightGreen;
            }
            //else if (AlignC.DPatternCenterDistanceUm != (int)nUDMarkDistance.Value)
            //{
            //    Ncolor = Color.LightGreen;
            //}
            else if (AlignC.MaxAlignTimes != (int)nudMaxAlignTimes.Value)
            {
                Ncolor = Color.LightGreen;
            }
            else if (AlignC.MinAlignTimes != (int)nudMinAlignTimes.Value)
            {
                Ncolor = Color.LightGreen;
            }
            else if (AlignC.XPrecision != (double)nudXPrecision.Value)
            {
                Ncolor = Color.LightGreen;
            }
            else if (AlignC.YPrecision != (double)nudYPrecision.Value)
            {
                Ncolor = Color.LightGreen;
            }
            else if (AlignC.ThetaPrecision != (double)nudThetaPrecision.Value)
            {
                Ncolor = Color.LightGreen;
            }
            else if (AlignC.MaxExpansion != (double)nudExpansion.Value)
            {
                Ncolor = Color.LightGreen;
            }
            else if (AlignC.UpBackAlign != GV.UpBackAlign)
            {
                Ncolor = Color.LightGreen;
            }
            else
            {
                Ncolor = Color.MintCream;
            }

            if (Ncolor != Ocolor)
            {
                btSaveAlignParm.BackColor = Ncolor;
                Update();
            }
        }

        private void BtSystemSetting_Click(object sender, EventArgs e)
        {
            DialogAppSetting setting = new DialogAppSetting
            {
                StartPosition = FormStartPosition.CenterScreen
            };
            setting.ShowDialog();
        }

        private void BtZCa_Click(object sender, EventArgs e)
        {

        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            // mLearnPatternBack1.CheckLevel();
            DlgInitial.Close();
        }

        public void ImgTranslate(Mat srcImage, Mat dstImage, int xOffset, int yOffset)
        {
            for (int i = 0; i < srcImage.Rows; i++)
            {
                for (int j = 0; j < srcImage.Cols; j++)
                {
                    int x = j + xOffset;
                    int y = i + yOffset;
                    if ((x >= 0) && (x < dstImage.Cols) && (y >= 0) && (y < dstImage.Rows))
                    {
                        dstImage.At<byte>(y, x) = srcImage.At<byte>(i, j);
                    }
                }
            }
        }

        private void BtNG_Click(object sender, EventArgs e)
        {
            GV.Plc.SendAlignNG();
        }

        private void BtACK_Click(object sender, EventArgs e)
        {
            GV.Plc.WriteMemory(GV.Plc.iUpCCDAlignACK, true);
        }

        private void BtCapMask_Click(object sender, EventArgs e)
        {
            Thread.Sleep(250);
            LeftMask = GV.LeftBackCam.Grab();
            RightMask = GV.RightBackCam.Grab();
            GV.LeftMaskMat = LeftMask;
            GV.RightMaskMat = RightMask;
        }

        private void BtZoomInfoWrite_Click(object sender, EventArgs e)
        {
            GM.WriteZoomLensInfoToXml("ZoomLensInfo.xml", GV.ZoomLensInfo);
        }

        private void BtZoomInfoRead_Click(object sender, EventArgs e)
        {
            GV.ZoomLensInfo = GM.ReadZoomLensInfoXml("ZoomLensInfo.xml");
        }

        private void BtReadMask_Click(object sender, EventArgs e)
        {
            DebugMessage("Read Back Mask");

            ProductMatchPositions pmps;
            try
            {
                LeftMask = Cv2.ImRead(GetTemplateFileName("LeftMask"), ImreadModes.Grayscale);
                RightMask = Cv2.ImRead(GetTemplateFileName("RightMask"), ImreadModes.Grayscale);
            }
            catch (Exception)
            {
                DebugMessage("read Mask faile");
                return;
            }
            GV.LeftMaskMat = LeftMask.Clone();
            GV.RightMaskMat = RightMask.Clone();

            try
            {
                pmps = GetAllMatchPostion(LeftMask, RightMask, "backmask");
            }
            catch (Exception)
            {
                DebugMessage("Back mask faile");
                return;
            }

            BLastPmps.LMaskMp.X = pmps.LMaskMp.X;
            BLastPmps.LMaskMp.Y = pmps.LMaskMp.Y;
            BLastPmps.LMaskMp.Score = pmps.LMaskMp.Score;
            BLastPmps.RMaskMp.X = pmps.RMaskMp.X;
            BLastPmps.RMaskMp.Y = pmps.RMaskMp.Y;
            BLastPmps.RMaskMp.Score = pmps.RMaskMp.Score;
            string TopMaskCheck3 = string.Format(GV.Dlang.strMessageMaskShift, BLastPmps.LMaskMp.X, BLastPmps.RMaskMp.X, BLastPmps.LMaskMp.Y, BLastPmps.RMaskMp.Y);
            DebugMessage("Read Back Mask : " + TopMaskCheck3);
            GV.NowWaferMask = 3;
        }

        public void MaskShift(Mat img, ref Mat targeimg)
        {
            // 定义平移向量

            PointF translation = new PointF(50, 30); // 在x方向平移50个像素，在y方向平移30个像素

            // 创建平移矩阵
            //Matrix<double> translationMatrix = new Matrix<double>(2, 3);
            //translationMatrix[0, 0] = 1; // 设置旋转矩阵的第一行为[1, 0, tx]
            //translationMatrix[0, 2] = translation.X;
            //translationMatrix[1, 1] = 1; // 设置旋转矩阵的第二行为[0, 1, ty]
            //translationMatrix[1, 2] = translation.Y;
            float[,] translationMatrix = new float[2, 3]
            {
                {1,0,translation.X },
                { 0,1,translation.Y }
            };
            Mat transMat = Mat.FromArray(translationMatrix);

            // 应用平移变换
            Cv2.WarpAffine(img, targeimg, transMat, img.Size());
        }

        private void BtGetMask_Click(object sender, EventArgs e)
        {
            LeftMask = Cv2.ImRead(GetTemplateFileName("LeftMask"), ImreadModes.Grayscale);
            RightMask = Cv2.ImRead(GetTemplateFileName("RightMask"), ImreadModes.Grayscale);
            GV.LeftMaskMat = LeftMask.Clone();
            GV.RightMaskMat = RightMask.Clone();
            ProductMatchPositions pmps = GetAllMatchPostion(LeftMask, RightMask, "backmask");
            BLastPmps.LMaskMp.X = pmps.LMaskMp.X;
            BLastPmps.LMaskMp.Y = pmps.LMaskMp.Y;
            BLastPmps.LMaskMp.Score = pmps.LMaskMp.Score;
            BLastPmps.RMaskMp.X = pmps.RMaskMp.X;
            BLastPmps.RMaskMp.Y = pmps.RMaskMp.Y;
            BLastPmps.RMaskMp.Score = pmps.RMaskMp.Score;
            BLivePmps.LMaskMp.X = pmps.LMaskMp.X;
            BLivePmps.LMaskMp.Y = pmps.LMaskMp.Y;
            BLivePmps.LMaskMp.Score = pmps.LMaskMp.Score;
            BLivePmps.RMaskMp.X = pmps.RMaskMp.X;
            BLivePmps.RMaskMp.Y = pmps.RMaskMp.Y;
            BLivePmps.RMaskMp.Score = pmps.RMaskMp.Score;
            string TopMaskCheck3 = string.Format(GV.Dlang.strMessageMaskShift, BLastPmps.LMaskMp.X, BLastPmps.RMaskMp.X, BLastPmps.LMaskMp.Y, BLastPmps.RMaskMp.Y);
            DebugMessage("0:" + TopMaskCheck3);
        }

        private void CbMagnification_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbMagnification.SelectedIndex != GV.LeftZoomLens.Magnification)
                ChangeLensMagnification(cbMagnification.SelectedIndex);
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

        private void TBRightCoaLight_ValueChanged(object sender, EventArgs e)
        {
            if (AlignC.RightBrightness[GV.RightZoomLens.Magnification] != tBRightCoaLight.Value)
            {
                AlignC.RightBrightness[GV.RightZoomLens.Magnification] = tBRightCoaLight.Value;
                GV.Light.ChangeBrightness("right", AlignC.RightBrightness[GV.RightZoomLens.Magnification]);
                if (GV.UserLevel != GV.User.Operator)
                {
                    AlignC.LastModifyTime = DateTime.Now;
                    GM.WriteRecipeXml(AlignC.RecipeNumber);
                }
            }
        }

        private void TBLeftCoaLight_ValueChanged(object sender, EventArgs e)
        {
            if (AlignC.LeftBrightness[GV.LeftZoomLens.Magnification] != tBLeftCoaLight.Value)
            {
                AlignC.LeftBrightness[GV.LeftZoomLens.Magnification] = tBLeftCoaLight.Value;
                GV.Light.ChangeBrightness("left", AlignC.LeftBrightness[GV.LeftZoomLens.Magnification]);
                if (GV.UserLevel != GV.User.Operator)
                {
                    AlignC.LastModifyTime = DateTime.Now;
                    GM.WriteRecipeXml(AlignC.RecipeNumber);
                }
            }
        }

        private void SaveAlignFailImage(Mat leftImage, Mat rightImage, string prefix, bool saveOnce = false)
        {
            try
            {
                if (leftImage == null || leftImage.Empty() || rightImage == null || rightImage.Empty())
                {
                    return;
                }
                int recipeNumber = GV.Plc.ReadData16(GV.Plc.iNowRecipeNumber);
                string trainPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Python", "Train", recipeNumber.ToString());
                if (!Directory.Exists(trainPath))
                {
                    Directory.CreateDirectory(trainPath);
                }
                if (saveOnce)
                {
                    string imageHash = ComputeImageHash(leftImage, rightImage);
                    if (_savedImageHashes.Contains(imageHash))
                    {
                        return;
                    }
                    _savedImageHashes.Add(imageHash);
                }
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string leftImageSave = Path.Combine(trainPath, $"{prefix}_L_{timestamp}.bmp");
                string rightImageSave = Path.Combine(trainPath, $"{prefix}_R_{timestamp}.bmmp");
                Cv2.ImWrite(leftImageSave, leftImage);
                Cv2.ImWrite(rightImageSave, rightImage);
            }
            catch
            {

            }
        }

        private string ComputeImageHash(Mat leftImage, Mat rightImage)
        {
            try
            {
                Scalar leftMean = Cv2.Mean(leftImage);
                Scalar rightMean = Cv2.Mean(rightImage);
                return $"{leftMean.Val0:F2}_{leftMean.Val1:F2}_{rightMean.Val0:F2}_{rightMean.Val1:F2}";
            }
            catch
            {
                return Guid.NewGuid().ToString();
            }
        }
    }
}

