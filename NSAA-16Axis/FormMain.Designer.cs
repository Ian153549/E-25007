
using System;

namespace NSAA_16Axis
{
    partial class FormMain
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.BtGetMask = new System.Windows.Forms.Button();
            this.btCapMask = new System.Windows.Forms.Button();
            this.lbMagnification = new System.Windows.Forms.Label();
            this.cbMagnification = new System.Windows.Forms.ComboBox();
            this.tBLeftCoaLight = new System.Windows.Forms.TrackBar();
            this.tBRightCoaLight = new System.Windows.Forms.TrackBar();
            this.btLCoaLightPlus = new System.Windows.Forms.Button();
            this.btLCoaLightMinus = new System.Windows.Forms.Button();
            this.btRCoaLightPlus = new System.Windows.Forms.Button();
            this.btRCoaLightMinus = new System.Windows.Forms.Button();
            this.btACK = new System.Windows.Forms.Button();
            this.ckAutoAlign = new System.Windows.Forms.CheckBox();
            this.btNG = new System.Windows.Forms.Button();
            this.btZCa = new System.Windows.Forms.Button();
            this.lbVerson = new System.Windows.Forms.Label();
            this.lbMin = new System.Windows.Forms.Label();
            this.lbMax = new System.Windows.Forms.Label();
            this.nudMinAlignTimes = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.nudMaxAlignTimes = new System.Windows.Forms.NumericUpDown();
            this.cBDown = new System.Windows.Forms.CheckBox();
            this.lbRightCCD = new System.Windows.Forms.Label();
            this.lbLeftCCD = new System.Windows.Forms.Label();
            this.lbEmulationModeR = new System.Windows.Forms.Label();
            this.lbEmulationModeL = new System.Windows.Forms.Label();
            this.skRightAlign = new MRLibrary.SKZoomAndPanWindow();
            this.skLeftAlign = new MRLibrary.SKZoomAndPanWindow();
            this.skZoomAndPanWindow1 = new MRLibrary.SKZoomAndPanWindow();
            this.labelCycleTestCycleTestTimes = new System.Windows.Forms.Label();
            this.gbAlignCondition = new System.Windows.Forms.GroupBox();
            this.cbPixelShift = new System.Windows.Forms.ComboBox();
            this.chbPixelShift = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.nUDMarkDistance = new System.Windows.Forms.NumericUpDown();
            this.nudMaskScore = new System.Windows.Forms.NumericUpDown();
            this.nudExpansion = new System.Windows.Forms.NumericUpDown();
            this.lbYOffset = new System.Windows.Forms.Label();
            this.lbStdXpre = new System.Windows.Forms.Label();
            this.lbWaferScore = new System.Windows.Forms.Label();
            this.nudXOffset = new System.Windows.Forms.NumericUpDown();
            this.lbXOffset = new System.Windows.Forms.Label();
            this.btSaveAlignParm = new System.Windows.Forms.Button();
            this.lbMaskScore = new System.Windows.Forms.Label();
            this.nudXPrecision = new System.Windows.Forms.NumericUpDown();
            this.nudYROffest = new System.Windows.Forms.NumericUpDown();
            this.lbStdThetaPre = new System.Windows.Forms.Label();
            this.lbMarkDist = new System.Windows.Forms.Label();
            this.lbThetaOffset = new System.Windows.Forms.Label();
            this.nudYLOffest = new System.Windows.Forms.NumericUpDown();
            this.lbMaxAlignTime = new System.Windows.Forms.Label();
            this.lbStdMaxExpansion = new System.Windows.Forms.Label();
            this.lbStdYPre = new System.Windows.Forms.Label();
            this.nudWaferScore = new System.Windows.Forms.NumericUpDown();
            this.nudYPrecision = new System.Windows.Forms.NumericUpDown();
            this.nudThetaPrecision = new System.Windows.Forms.NumericUpDown();
            this.lbNowRecipeNumber = new System.Windows.Forms.Label();
            this.MaskImage = new System.Windows.Forms.CheckBox();
            this.lbCompany = new System.Windows.Forms.Label();
            this.lbNowTime = new System.Windows.Forms.Label();
            this.btQuit = new System.Windows.Forms.Button();
            this.btLogIn = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tbStatus = new System.Windows.Forms.TextBox();
            this.ucSaveImage = new MRLibrary.UCButton();
            this.ucSaveRecipe = new MRLibrary.UCButton();
            this.ucGetRecipe = new MRLibrary.UCButton();
            this.ucGoRecipe = new MRLibrary.UCButton();
            this.ucAlign = new MRLibrary.UCButton();
            this.btSystemSetting = new System.Windows.Forms.Button();
            this.buttonTestMode = new System.Windows.Forms.Button();
            this.buttonCapture = new System.Windows.Forms.Button();
            this.lbRShowImage = new System.Windows.Forms.Label();
            this.btRShowImageSave = new System.Windows.Forms.Button();
            this.btRShowImageMinus = new System.Windows.Forms.Button();
            this.btRShowImagePlus = new System.Windows.Forms.Button();
            this.tBRShowImageAlign = new System.Windows.Forms.TrackBar();
            this.lbLShowImage = new System.Windows.Forms.Label();
            this.btLShowImageSave = new System.Windows.Forms.Button();
            this.btLShowImageMinus = new System.Windows.Forms.Button();
            this.btLShowImagePlus = new System.Windows.Forms.Button();
            this.tBLShowImageAlign = new System.Windows.Forms.TrackBar();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.cmLearnPatternUp1 = new NSAA_16Axis.CMLearnPatternUp();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.btReadMask = new MRLibrary.UCButton();
            this.ucOpenPad = new MRLibrary.UCButton();
            this.lbEmulationModePR = new System.Windows.Forms.Label();
            this.lbEmulationModePL = new System.Windows.Forms.Label();
            this.trackBar2 = new System.Windows.Forms.TrackBar();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.groupBoxMeasurePatternDistance = new System.Windows.Forms.GroupBox();
            this.lbStep = new System.Windows.Forms.Label();
            this.buttonCalPatternDistance = new System.Windows.Forms.Button();
            this.numericUpDownMeasurePatternDistanceSteps = new System.Windows.Forms.NumericUpDown();
            this.label19 = new System.Windows.Forms.Label();
            this.buttonRightBrightnessIncrease = new System.Windows.Forms.Button();
            this.buttonLeftBrightnessIncrease = new System.Windows.Forms.Button();
            this.buttonRightBrightnessDecrease = new System.Windows.Forms.Button();
            this.lbRCCDLight = new System.Windows.Forms.Label();
            this.lbLCCDLight = new System.Windows.Forms.Label();
            this.buttonLeftBrightnessDecrease = new System.Windows.Forms.Button();
            this.gbMachingSetting = new System.Windows.Forms.GroupBox();
            this.btnManualPlc = new System.Windows.Forms.Button();
            this.buttonSetting = new System.Windows.Forms.Button();
            this.btTuneLens = new System.Windows.Forms.Button();
            this.buttonInitialLens = new System.Windows.Forms.Button();
            this.btnTuneXyyTable = new System.Windows.Forms.Button();
            this.buttonTuneLight = new System.Windows.Forms.Button();
            this.label22 = new System.Windows.Forms.Label();
            this.gbZoomLensParameters = new System.Windows.Forms.GroupBox();
            this.btnSaveAlignMagnification = new System.Windows.Forms.Button();
            this.lbLowMagnification = new System.Windows.Forms.Label();
            this.cbLowMagnification = new System.Windows.Forms.ComboBox();
            this.lbHighMagnification = new System.Windows.Forms.Label();
            this.cbHighMagnification = new System.Windows.Forms.ComboBox();
            this.gbCameraParm = new System.Windows.Forms.GroupBox();
            this.radioButtonLeftBackCam = new System.Windows.Forms.RadioButton();
            this.radioButtonRightBackCam = new System.Windows.Forms.RadioButton();
            this.buttonCameraParameterResotre = new System.Windows.Forms.Button();
            this.buttonCameraParmSave = new System.Windows.Forms.Button();
            this.trackBarCameraParmeters = new System.Windows.Forms.TrackBar();
            this.comboBoxCameraParameter = new System.Windows.Forms.ComboBox();
            this.radioButtonLeftCamera = new System.Windows.Forms.RadioButton();
            this.radioButtonRightCamera = new System.Windows.Forms.RadioButton();
            this.gbRightZoomLens = new System.Windows.Forms.GroupBox();
            this.btRightX12 = new System.Windows.Forms.Button();
            this.btRightX11 = new System.Windows.Forms.Button();
            this.btRightX6 = new System.Windows.Forms.Button();
            this.btRightX10 = new System.Windows.Forms.Button();
            this.btRightX7 = new System.Windows.Forms.Button();
            this.btRightX9 = new System.Windows.Forms.Button();
            this.btRightX8 = new System.Windows.Forms.Button();
            this.btRightX1 = new System.Windows.Forms.Button();
            this.btRightX5 = new System.Windows.Forms.Button();
            this.btRightX2 = new System.Windows.Forms.Button();
            this.btRightX4 = new System.Windows.Forms.Button();
            this.btRightX3 = new System.Windows.Forms.Button();
            this.gbLeftZoomLens = new System.Windows.Forms.GroupBox();
            this.btLeftX12 = new System.Windows.Forms.Button();
            this.btLeftX11 = new System.Windows.Forms.Button();
            this.btLeftX6 = new System.Windows.Forms.Button();
            this.btLeftX10 = new System.Windows.Forms.Button();
            this.btLeftX7 = new System.Windows.Forms.Button();
            this.btLeftX9 = new System.Windows.Forms.Button();
            this.btLeftX8 = new System.Windows.Forms.Button();
            this.btLeftX1 = new System.Windows.Forms.Button();
            this.btLeftX5 = new System.Windows.Forms.Button();
            this.btLeftX2 = new System.Windows.Forms.Button();
            this.btLeftX4 = new System.Windows.Forms.Button();
            this.btLeftX3 = new System.Windows.Forms.Button();
            this.btnQuit = new System.Windows.Forms.Button();
            this.btZoomInfoWrite = new MRLibrary.UCButton();
            this.btZoomInfoRead = new MRLibrary.UCButton();
            this.skRightParam = new MRLibrary.SKZoomAndPanWindow();
            this.skLeftParam = new MRLibrary.SKZoomAndPanWindow();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.cmLearnPatternBack1 = new NSAA_16Axis.CMLearnPatternBack();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.cmzCalibration1 = new NSAA_16Axis.CMZCalibration();
            this.skZoomAndPanWindow6 = new MRLibrary.SKZoomAndPanWindow();
            this.timerUpdateInfomationAccordingRecipeNumber = new System.Windows.Forms.Timer(this.components);
            this.timerCheckPlcDram = new System.Windows.Forms.Timer(this.components);
            this.timerNowTime = new System.Windows.Forms.Timer(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tBLeftCoaLight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tBRightCoaLight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinAlignTimes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxAlignTimes)).BeginInit();
            this.gbAlignCondition.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nUDMarkDistance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaskScore)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudExpansion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudXOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudXPrecision)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudYROffest)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudYLOffest)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWaferScore)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudYPrecision)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThetaPrecision)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tBRShowImageAlign)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tBLShowImageAlign)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.groupBoxMeasurePatternDistance.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMeasurePatternDistanceSteps)).BeginInit();
            this.gbMachingSetting.SuspendLayout();
            this.gbZoomLensParameters.SuspendLayout();
            this.gbCameraParm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarCameraParmeters)).BeginInit();
            this.gbRightZoomLens.SuspendLayout();
            this.gbLeftZoomLens.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1920, 1080);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.TabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tabPage1.Controls.Add(this.BtGetMask);
            this.tabPage1.Controls.Add(this.btCapMask);
            this.tabPage1.Controls.Add(this.lbMagnification);
            this.tabPage1.Controls.Add(this.cbMagnification);
            this.tabPage1.Controls.Add(this.tBLeftCoaLight);
            this.tabPage1.Controls.Add(this.tBRightCoaLight);
            this.tabPage1.Controls.Add(this.btLCoaLightPlus);
            this.tabPage1.Controls.Add(this.btLCoaLightMinus);
            this.tabPage1.Controls.Add(this.btRCoaLightPlus);
            this.tabPage1.Controls.Add(this.btRCoaLightMinus);
            this.tabPage1.Controls.Add(this.btACK);
            this.tabPage1.Controls.Add(this.ckAutoAlign);
            this.tabPage1.Controls.Add(this.btNG);
            this.tabPage1.Controls.Add(this.btZCa);
            this.tabPage1.Controls.Add(this.lbVerson);
            this.tabPage1.Controls.Add(this.lbMin);
            this.tabPage1.Controls.Add(this.lbMax);
            this.tabPage1.Controls.Add(this.nudMinAlignTimes);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.nudMaxAlignTimes);
            this.tabPage1.Controls.Add(this.cBDown);
            this.tabPage1.Controls.Add(this.lbRightCCD);
            this.tabPage1.Controls.Add(this.lbLeftCCD);
            this.tabPage1.Controls.Add(this.lbEmulationModeR);
            this.tabPage1.Controls.Add(this.lbEmulationModeL);
            this.tabPage1.Controls.Add(this.skRightAlign);
            this.tabPage1.Controls.Add(this.skLeftAlign);
            this.tabPage1.Controls.Add(this.skZoomAndPanWindow1);
            this.tabPage1.Controls.Add(this.labelCycleTestCycleTestTimes);
            this.tabPage1.Controls.Add(this.gbAlignCondition);
            this.tabPage1.Controls.Add(this.lbCompany);
            this.tabPage1.Controls.Add(this.lbNowTime);
            this.tabPage1.Controls.Add(this.btQuit);
            this.tabPage1.Controls.Add(this.btLogIn);
            this.tabPage1.Controls.Add(this.pictureBox1);
            this.tabPage1.Controls.Add(this.tbStatus);
            this.tabPage1.Controls.Add(this.ucSaveImage);
            this.tabPage1.Controls.Add(this.ucSaveRecipe);
            this.tabPage1.Controls.Add(this.ucGetRecipe);
            this.tabPage1.Controls.Add(this.ucGoRecipe);
            this.tabPage1.Controls.Add(this.ucAlign);
            this.tabPage1.Controls.Add(this.btSystemSetting);
            this.tabPage1.Controls.Add(this.buttonTestMode);
            this.tabPage1.Controls.Add(this.buttonCapture);
            this.tabPage1.Controls.Add(this.lbRShowImage);
            this.tabPage1.Controls.Add(this.btRShowImageSave);
            this.tabPage1.Controls.Add(this.btRShowImageMinus);
            this.tabPage1.Controls.Add(this.btRShowImagePlus);
            this.tabPage1.Controls.Add(this.tBRShowImageAlign);
            this.tabPage1.Controls.Add(this.lbLShowImage);
            this.tabPage1.Controls.Add(this.btLShowImageSave);
            this.tabPage1.Controls.Add(this.btLShowImageMinus);
            this.tabPage1.Controls.Add(this.btLShowImagePlus);
            this.tabPage1.Controls.Add(this.tBLShowImageAlign);
            this.tabPage1.Location = new System.Drawing.Point(4, 38);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1912, 1038);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Tag = "1";
            this.tabPage1.Text = "|  Align  |";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // BtGetMask
            // 
            this.BtGetMask.Location = new System.Drawing.Point(1092, 248);
            this.BtGetMask.Name = "BtGetMask";
            this.BtGetMask.Size = new System.Drawing.Size(100, 30);
            this.BtGetMask.TabIndex = 86;
            this.BtGetMask.Text = "CapMask";
            this.BtGetMask.UseVisualStyleBackColor = true;
            this.BtGetMask.Visible = false;
            this.BtGetMask.Click += new System.EventHandler(this.BtGetMask_Click);
            // 
            // btCapMask
            // 
            this.btCapMask.Location = new System.Drawing.Point(1092, 56);
            this.btCapMask.Name = "btCapMask";
            this.btCapMask.Size = new System.Drawing.Size(100, 30);
            this.btCapMask.TabIndex = 85;
            this.btCapMask.Text = "CapMask";
            this.btCapMask.UseVisualStyleBackColor = true;
            this.btCapMask.Visible = false;
            this.btCapMask.Click += new System.EventHandler(this.BtCapMask_Click);
            // 
            // lbMagnification
            // 
            this.lbMagnification.AutoSize = true;
            this.lbMagnification.Location = new System.Drawing.Point(1088, 58);
            this.lbMagnification.Name = "lbMagnification";
            this.lbMagnification.Size = new System.Drawing.Size(66, 29);
            this.lbMagnification.TabIndex = 78;
            this.lbMagnification.Text = "倍率:";
            // 
            // cbMagnification
            // 
            this.cbMagnification.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMagnification.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbMagnification.FormattingEnabled = true;
            this.cbMagnification.Items.AddRange(new object[] {
            "X1",
            "X2",
            "X3",
            "X4",
            "X5",
            "X6",
            "X7",
            "X8",
            "X9",
            "X10",
            "X11",
            "X12"});
            this.cbMagnification.Location = new System.Drawing.Point(1140, 58);
            this.cbMagnification.Name = "cbMagnification";
            this.cbMagnification.Size = new System.Drawing.Size(60, 31);
            this.cbMagnification.TabIndex = 16;
            this.cbMagnification.SelectedIndexChanged += new System.EventHandler(this.CbMagnification_SelectedIndexChanged);
            // 
            // tBLeftCoaLight
            // 
            this.tBLeftCoaLight.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tBLeftCoaLight.LargeChange = 1;
            this.tBLeftCoaLight.Location = new System.Drawing.Point(1087, 118);
            this.tBLeftCoaLight.Maximum = 255;
            this.tBLeftCoaLight.Name = "tBLeftCoaLight";
            this.tBLeftCoaLight.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tBLeftCoaLight.Size = new System.Drawing.Size(56, 164);
            this.tBLeftCoaLight.TabIndex = 54;
            this.tBLeftCoaLight.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.tBLeftCoaLight.ValueChanged += new System.EventHandler(this.TBLeftCoaLight_ValueChanged);
            // 
            // tBRightCoaLight
            // 
            this.tBRightCoaLight.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tBRightCoaLight.LargeChange = 1;
            this.tBRightCoaLight.Location = new System.Drawing.Point(1150, 118);
            this.tBRightCoaLight.Maximum = 255;
            this.tBRightCoaLight.Name = "tBRightCoaLight";
            this.tBRightCoaLight.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tBRightCoaLight.Size = new System.Drawing.Size(56, 164);
            this.tBRightCoaLight.TabIndex = 56;
            this.tBRightCoaLight.ValueChanged += new System.EventHandler(this.TBRightCoaLight_ValueChanged);
            // 
            // btLCoaLightPlus
            // 
            this.btLCoaLightPlus.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btLCoaLightPlus.Location = new System.Drawing.Point(1095, 88);
            this.btLCoaLightPlus.Name = "btLCoaLightPlus";
            this.btLCoaLightPlus.Size = new System.Drawing.Size(38, 30);
            this.btLCoaLightPlus.TabIndex = 62;
            this.btLCoaLightPlus.Text = "+";
            this.btLCoaLightPlus.UseVisualStyleBackColor = true;
            this.btLCoaLightPlus.Click += new System.EventHandler(this.BtLCoaLightPlus_Click);
            // 
            // btLCoaLightMinus
            // 
            this.btLCoaLightMinus.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btLCoaLightMinus.Location = new System.Drawing.Point(1095, 278);
            this.btLCoaLightMinus.Name = "btLCoaLightMinus";
            this.btLCoaLightMinus.Size = new System.Drawing.Size(38, 30);
            this.btLCoaLightMinus.TabIndex = 63;
            this.btLCoaLightMinus.Text = "-";
            this.btLCoaLightMinus.UseVisualStyleBackColor = true;
            this.btLCoaLightMinus.Click += new System.EventHandler(this.BtLCoaLightMinus_Click);
            // 
            // btRCoaLightPlus
            // 
            this.btRCoaLightPlus.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btRCoaLightPlus.Location = new System.Drawing.Point(1158, 88);
            this.btRCoaLightPlus.Name = "btRCoaLightPlus";
            this.btRCoaLightPlus.Size = new System.Drawing.Size(38, 30);
            this.btRCoaLightPlus.TabIndex = 66;
            this.btRCoaLightPlus.Text = "+";
            this.btRCoaLightPlus.UseVisualStyleBackColor = true;
            this.btRCoaLightPlus.Click += new System.EventHandler(this.BtRCoaLightPlus_Click);
            // 
            // btRCoaLightMinus
            // 
            this.btRCoaLightMinus.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btRCoaLightMinus.Location = new System.Drawing.Point(1158, 278);
            this.btRCoaLightMinus.Name = "btRCoaLightMinus";
            this.btRCoaLightMinus.Size = new System.Drawing.Size(38, 30);
            this.btRCoaLightMinus.TabIndex = 67;
            this.btRCoaLightMinus.Text = "-";
            this.btRCoaLightMinus.UseVisualStyleBackColor = true;
            this.btRCoaLightMinus.Click += new System.EventHandler(this.BtRCoaLightMinus_Click);
            // 
            // btACK
            // 
            this.btACK.Location = new System.Drawing.Point(1114, 226);
            this.btACK.Name = "btACK";
            this.btACK.Size = new System.Drawing.Size(60, 30);
            this.btACK.TabIndex = 84;
            this.btACK.Text = "ACK";
            this.btACK.UseVisualStyleBackColor = true;
            this.btACK.Visible = false;
            this.btACK.Click += new System.EventHandler(this.BtACK_Click);
            // 
            // ckAutoAlign
            // 
            this.ckAutoAlign.AutoSize = true;
            this.ckAutoAlign.Location = new System.Drawing.Point(1088, 0);
            this.ckAutoAlign.Name = "ckAutoAlign";
            this.ckAutoAlign.Size = new System.Drawing.Size(118, 33);
            this.ckAutoAlign.TabIndex = 83;
            this.ckAutoAlign.Text = "AutoAlign";
            this.ckAutoAlign.UseVisualStyleBackColor = true;
            this.ckAutoAlign.Visible = false;
            // 
            // btNG
            // 
            this.btNG.Location = new System.Drawing.Point(1114, 179);
            this.btNG.Name = "btNG";
            this.btNG.Size = new System.Drawing.Size(60, 30);
            this.btNG.TabIndex = 82;
            this.btNG.Text = "NG";
            this.btNG.UseVisualStyleBackColor = true;
            this.btNG.Visible = false;
            this.btNG.Click += new System.EventHandler(this.BtNG_Click);
            // 
            // btZCa
            // 
            this.btZCa.Location = new System.Drawing.Point(1114, 129);
            this.btZCa.Name = "btZCa";
            this.btZCa.Size = new System.Drawing.Size(60, 30);
            this.btZCa.TabIndex = 81;
            this.btZCa.Text = "btZ";
            this.btZCa.UseVisualStyleBackColor = true;
            this.btZCa.Visible = false;
            this.btZCa.Click += new System.EventHandler(this.BtZCa_Click);
            // 
            // lbVerson
            // 
            this.lbVerson.AutoSize = true;
            this.lbVerson.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbVerson.Location = new System.Drawing.Point(162, 145);
            this.lbVerson.Name = "lbVerson";
            this.lbVerson.Size = new System.Drawing.Size(102, 16);
            this.lbVerson.TabIndex = 79;
            this.lbVerson.Text = "202505232115";
            // 
            // lbMin
            // 
            this.lbMin.AutoSize = true;
            this.lbMin.BackColor = System.Drawing.SystemColors.Control;
            this.lbMin.Location = new System.Drawing.Point(946, 66);
            this.lbMin.Name = "lbMin";
            this.lbMin.Size = new System.Drawing.Size(55, 29);
            this.lbMin.TabIndex = 78;
            this.lbMin.Text = "Min :";
            // 
            // lbMax
            // 
            this.lbMax.AutoSize = true;
            this.lbMax.BackColor = System.Drawing.SystemColors.Control;
            this.lbMax.Location = new System.Drawing.Point(830, 66);
            this.lbMax.Name = "lbMax";
            this.lbMax.Size = new System.Drawing.Size(61, 29);
            this.lbMax.TabIndex = 77;
            this.lbMax.Text = "Max :";
            // 
            // nudMinAlignTimes
            // 
            this.nudMinAlignTimes.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.nudMinAlignTimes.Location = new System.Drawing.Point(1000, 60);
            this.nudMinAlignTimes.Name = "nudMinAlignTimes";
            this.nudMinAlignTimes.Size = new System.Drawing.Size(50, 35);
            this.nudMinAlignTimes.TabIndex = 76;
            this.nudMinAlignTimes.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMinAlignTimes.ValueChanged += new System.EventHandler(this.ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(241, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 29);
            this.label1.TabIndex = 75;
            this.label1.Text = "label1";
            this.label1.Visible = false;
            // 
            // nudMaxAlignTimes
            // 
            this.nudMaxAlignTimes.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.nudMaxAlignTimes.Location = new System.Drawing.Point(890, 60);
            this.nudMaxAlignTimes.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMaxAlignTimes.Name = "nudMaxAlignTimes";
            this.nudMaxAlignTimes.Size = new System.Drawing.Size(50, 35);
            this.nudMaxAlignTimes.TabIndex = 39;
            this.nudMaxAlignTimes.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudMaxAlignTimes.ValueChanged += new System.EventHandler(this.ValueChanged);
            // 
            // cBDown
            // 
            this.cBDown.AutoSize = true;
            this.cBDown.Location = new System.Drawing.Point(261, 35);
            this.cBDown.Name = "cBDown";
            this.cBDown.Size = new System.Drawing.Size(18, 17);
            this.cBDown.TabIndex = 74;
            this.cBDown.UseVisualStyleBackColor = true;
            // 
            // lbRightCCD
            // 
            this.lbRightCCD.AutoSize = true;
            this.lbRightCCD.BackColor = System.Drawing.Color.Transparent;
            this.lbRightCCD.Location = new System.Drawing.Point(1810, 337);
            this.lbRightCCD.Name = "lbRightCCD";
            this.lbRightCCD.Size = new System.Drawing.Size(94, 29);
            this.lbRightCCD.TabIndex = 73;
            this.lbRightCCD.Text = "Top CCD";
            // 
            // lbLeftCCD
            // 
            this.lbLeftCCD.AutoSize = true;
            this.lbLeftCCD.Location = new System.Drawing.Point(6, 337);
            this.lbLeftCCD.Name = "lbLeftCCD";
            this.lbLeftCCD.Size = new System.Drawing.Size(94, 29);
            this.lbLeftCCD.TabIndex = 72;
            this.lbLeftCCD.Text = "Top CCD";
            // 
            // lbEmulationModeR
            // 
            this.lbEmulationModeR.AutoSize = true;
            this.lbEmulationModeR.Font = new System.Drawing.Font("標楷體", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbEmulationModeR.Location = new System.Drawing.Point(1292, 654);
            this.lbEmulationModeR.Name = "lbEmulationModeR";
            this.lbEmulationModeR.Size = new System.Drawing.Size(327, 44);
            this.lbEmulationModeR.TabIndex = 71;
            this.lbEmulationModeR.Text = "Emulation Mode";
            this.lbEmulationModeR.Visible = false;
            // 
            // lbEmulationModeL
            // 
            this.lbEmulationModeL.AutoSize = true;
            this.lbEmulationModeL.Font = new System.Drawing.Font("標楷體", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbEmulationModeL.Location = new System.Drawing.Point(317, 654);
            this.lbEmulationModeL.Name = "lbEmulationModeL";
            this.lbEmulationModeL.Size = new System.Drawing.Size(327, 44);
            this.lbEmulationModeL.TabIndex = 70;
            this.lbEmulationModeL.Text = "Emulation Mode";
            this.lbEmulationModeL.Visible = false;
            // 
            // skRightAlign
            // 
            this.skRightAlign.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.skRightAlign.Location = new System.Drawing.Point(955, 319);
            this.skRightAlign.Name = "skRightAlign";
            this.skRightAlign.Size = new System.Drawing.Size(944, 708);
            this.skRightAlign.TabIndex = 33;
            // 
            // skLeftAlign
            // 
            this.skLeftAlign.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.skLeftAlign.Location = new System.Drawing.Point(10, 319);
            this.skLeftAlign.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.skLeftAlign.Name = "skLeftAlign";
            this.skLeftAlign.Size = new System.Drawing.Size(944, 708);
            this.skLeftAlign.TabIndex = 32;
            // 
            // skZoomAndPanWindow1
            // 
            this.skZoomAndPanWindow1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.skZoomAndPanWindow1.Location = new System.Drawing.Point(176, 4776);
            this.skZoomAndPanWindow1.Margin = new System.Windows.Forms.Padding(21, 84, 21, 84);
            this.skZoomAndPanWindow1.Name = "skZoomAndPanWindow1";
            this.skZoomAndPanWindow1.Size = new System.Drawing.Size(4050, 6068);
            this.skZoomAndPanWindow1.TabIndex = 31;
            // 
            // labelCycleTestCycleTestTimes
            // 
            this.labelCycleTestCycleTestTimes.AutoSize = true;
            this.labelCycleTestCycleTestTimes.ForeColor = System.Drawing.Color.DarkRed;
            this.labelCycleTestCycleTestTimes.Location = new System.Drawing.Point(1116, 90);
            this.labelCycleTestCycleTestTimes.Name = "labelCycleTestCycleTestTimes";
            this.labelCycleTestCycleTestTimes.Size = new System.Drawing.Size(74, 29);
            this.labelCycleTestCycleTestTimes.TabIndex = 30;
            this.labelCycleTestCycleTestTimes.Text = "( 0 / 0 )";
            this.labelCycleTestCycleTestTimes.Visible = false;
            // 
            // gbAlignCondition
            // 
            this.gbAlignCondition.BackColor = System.Drawing.SystemColors.Control;
            this.gbAlignCondition.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.gbAlignCondition.Controls.Add(this.cbPixelShift);
            this.gbAlignCondition.Controls.Add(this.chbPixelShift);
            this.gbAlignCondition.Controls.Add(this.tableLayoutPanel1);
            this.gbAlignCondition.Controls.Add(this.MaskImage);
            this.gbAlignCondition.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gbAlignCondition.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbAlignCondition.Location = new System.Drawing.Point(309, 10);
            this.gbAlignCondition.Name = "gbAlignCondition";
            this.gbAlignCondition.Size = new System.Drawing.Size(764, 304);
            this.gbAlignCondition.TabIndex = 27;
            this.gbAlignCondition.TabStop = false;
            this.gbAlignCondition.Text = "Align Condition";
            // 
            // cbPixelShift
            // 
            this.cbPixelShift.FormattingEnabled = true;
            this.cbPixelShift.Items.AddRange(new object[] {
            "4X4",
            "9X9"});
            this.cbPixelShift.Location = new System.Drawing.Point(51, 269);
            this.cbPixelShift.Name = "cbPixelShift";
            this.cbPixelShift.Size = new System.Drawing.Size(104, 31);
            this.cbPixelShift.TabIndex = 71;
            this.cbPixelShift.Visible = false;
            // 
            // chbPixelShift
            // 
            this.chbPixelShift.AutoSize = true;
            this.chbPixelShift.Location = new System.Drawing.Point(28, 269);
            this.chbPixelShift.Name = "chbPixelShift";
            this.chbPixelShift.Size = new System.Drawing.Size(18, 17);
            this.chbPixelShift.TabIndex = 70;
            this.chbPixelShift.UseVisualStyleBackColor = true;
            this.chbPixelShift.Visible = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 52.14008F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 47.85992F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 239F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 218F));
            this.tableLayoutPanel1.Controls.Add(this.nUDMarkDistance, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.nudMaskScore, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.nudExpansion, 3, 4);
            this.tableLayoutPanel1.Controls.Add(this.lbYOffset, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lbStdXpre, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.lbWaferScore, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.nudXOffset, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lbXOffset, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btSaveAlignParm, 3, 5);
            this.tableLayoutPanel1.Controls.Add(this.lbMaskScore, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.nudXPrecision, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.nudYROffest, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.lbStdThetaPre, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.lbMarkDist, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.lbThetaOffset, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.nudYLOffest, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.lbMaxAlignTime, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbStdMaxExpansion, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.lbStdYPre, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.nudWaferScore, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.nudYPrecision, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.nudThetaPrecision, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.lbNowRecipeNumber, 2, 5);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(26, 33);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 51.35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 48.65F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(720, 230);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // nUDMarkDistance
            // 
            this.nUDMarkDistance.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.nUDMarkDistance.Location = new System.Drawing.Point(146, 195);
            this.nUDMarkDistance.Maximum = new decimal(new int[] {
            25000000,
            0,
            0,
            0});
            this.nUDMarkDistance.Name = "nUDMarkDistance";
            this.nUDMarkDistance.Size = new System.Drawing.Size(107, 30);
            this.nUDMarkDistance.TabIndex = 48;
            this.nUDMarkDistance.Visible = false;
            this.nUDMarkDistance.ValueChanged += new System.EventHandler(this.ValueChanged);
            // 
            // nudMaskScore
            // 
            this.nudMaskScore.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.nudMaskScore.DecimalPlaces = 2;
            this.nudMaskScore.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudMaskScore.Location = new System.Drawing.Point(155, 8);
            this.nudMaskScore.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            131072});
            this.nudMaskScore.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudMaskScore.Name = "nudMaskScore";
            this.nudMaskScore.Size = new System.Drawing.Size(89, 30);
            this.nudMaskScore.TabIndex = 24;
            this.nudMaskScore.Value = new decimal(new int[] {
            7,
            0,
            0,
            65536});
            this.nudMaskScore.ValueChanged += new System.EventHandler(this.ValueChanged);
            // 
            // nudExpansion
            // 
            this.nudExpansion.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.nudExpansion.DecimalPlaces = 1;
            this.nudExpansion.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudExpansion.Location = new System.Drawing.Point(566, 159);
            this.nudExpansion.Name = "nudExpansion";
            this.nudExpansion.Size = new System.Drawing.Size(89, 30);
            this.nudExpansion.TabIndex = 42;
            this.nudExpansion.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudExpansion.ValueChanged += new System.EventHandler(this.ValueChanged);
            // 
            // lbYOffset
            // 
            this.lbYOffset.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbYOffset.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbYOffset.Location = new System.Drawing.Point(16, 131);
            this.lbYOffset.Name = "lbYOffset";
            this.lbYOffset.Size = new System.Drawing.Size(104, 18);
            this.lbYOffset.TabIndex = 29;
            this.lbYOffset.Text = "YL Offest(um)";
            this.lbYOffset.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbStdXpre
            // 
            this.lbStdXpre.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbStdXpre.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbStdXpre.ForeColor = System.Drawing.Color.LightCoral;
            this.lbStdXpre.Location = new System.Drawing.Point(283, 58);
            this.lbStdXpre.Name = "lbStdXpre";
            this.lbStdXpre.Size = new System.Drawing.Size(197, 18);
            this.lbStdXpre.TabIndex = 32;
            this.lbStdXpre.Text = "Standard X Precision(um)";
            this.lbStdXpre.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbWaferScore
            // 
            this.lbWaferScore.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbWaferScore.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbWaferScore.Location = new System.Drawing.Point(21, 58);
            this.lbWaferScore.Name = "lbWaferScore";
            this.lbWaferScore.Size = new System.Drawing.Size(95, 18);
            this.lbWaferScore.TabIndex = 26;
            this.lbWaferScore.Text = "Wafer Score";
            this.lbWaferScore.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nudXOffset
            // 
            this.nudXOffset.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.nudXOffset.DecimalPlaces = 1;
            this.nudXOffset.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudXOffset.Location = new System.Drawing.Point(155, 92);
            this.nudXOffset.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.nudXOffset.Name = "nudXOffset";
            this.nudXOffset.Size = new System.Drawing.Size(89, 30);
            this.nudXOffset.TabIndex = 36;
            this.nudXOffset.ValueChanged += new System.EventHandler(this.ValueChanged);
            // 
            // lbXOffset
            // 
            this.lbXOffset.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbXOffset.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbXOffset.Location = new System.Drawing.Point(21, 97);
            this.lbXOffset.Name = "lbXOffset";
            this.lbXOffset.Size = new System.Drawing.Size(95, 18);
            this.lbXOffset.TabIndex = 28;
            this.lbXOffset.Text = "X Offset(um)";
            this.lbXOffset.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btSaveAlignParm
            // 
            this.btSaveAlignParm.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btSaveAlignParm.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btSaveAlignParm.BackColor = System.Drawing.Color.MintCream;
            this.btSaveAlignParm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btSaveAlignParm.ForeColor = System.Drawing.Color.Black;
            this.btSaveAlignParm.Location = new System.Drawing.Point(559, 194);
            this.btSaveAlignParm.Name = "btSaveAlignParm";
            this.btSaveAlignParm.Size = new System.Drawing.Size(102, 33);
            this.btSaveAlignParm.TabIndex = 43;
            this.btSaveAlignParm.Text = "SAVE";
            this.btSaveAlignParm.UseVisualStyleBackColor = false;
            this.btSaveAlignParm.Click += new System.EventHandler(this.BtSaveAlignParm_Click);
            // 
            // lbMaskScore
            // 
            this.lbMaskScore.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbMaskScore.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbMaskScore.Location = new System.Drawing.Point(22, 14);
            this.lbMaskScore.Name = "lbMaskScore";
            this.lbMaskScore.Size = new System.Drawing.Size(92, 18);
            this.lbMaskScore.TabIndex = 25;
            this.lbMaskScore.Text = "Mask Score";
            this.lbMaskScore.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nudXPrecision
            // 
            this.nudXPrecision.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.nudXPrecision.DecimalPlaces = 1;
            this.nudXPrecision.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudXPrecision.Location = new System.Drawing.Point(566, 52);
            this.nudXPrecision.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudXPrecision.Name = "nudXPrecision";
            this.nudXPrecision.Size = new System.Drawing.Size(89, 30);
            this.nudXPrecision.TabIndex = 40;
            this.nudXPrecision.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nudXPrecision.ValueChanged += new System.EventHandler(this.ValueChanged);
            // 
            // nudYROffest
            // 
            this.nudYROffest.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.nudYROffest.DecimalPlaces = 1;
            this.nudYROffest.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudYROffest.Location = new System.Drawing.Point(155, 159);
            this.nudYROffest.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.nudYROffest.Name = "nudYROffest";
            this.nudYROffest.Size = new System.Drawing.Size(89, 30);
            this.nudYROffest.TabIndex = 38;
            this.nudYROffest.ValueChanged += new System.EventHandler(this.ValueChanged);
            // 
            // lbStdThetaPre
            // 
            this.lbStdThetaPre.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbStdThetaPre.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbStdThetaPre.ForeColor = System.Drawing.Color.LightCoral;
            this.lbStdThetaPre.Location = new System.Drawing.Point(271, 131);
            this.lbStdThetaPre.Name = "lbStdThetaPre";
            this.lbStdThetaPre.Size = new System.Drawing.Size(220, 18);
            this.lbStdThetaPre.TabIndex = 34;
            this.lbStdThetaPre.Text = "Standard Theta Precision(um)";
            this.lbStdThetaPre.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbMarkDist
            // 
            this.lbMarkDist.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbMarkDist.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbMarkDist.Location = new System.Drawing.Point(8, 201);
            this.lbMarkDist.Name = "lbMarkDist";
            this.lbMarkDist.Size = new System.Drawing.Size(121, 18);
            this.lbMarkDist.TabIndex = 47;
            this.lbMarkDist.Text = "Mark DIST(um)";
            this.lbMarkDist.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbMarkDist.Visible = false;
            // 
            // lbThetaOffset
            // 
            this.lbThetaOffset.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbThetaOffset.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbThetaOffset.Location = new System.Drawing.Point(16, 164);
            this.lbThetaOffset.Name = "lbThetaOffset";
            this.lbThetaOffset.Size = new System.Drawing.Size(104, 18);
            this.lbThetaOffset.TabIndex = 31;
            this.lbThetaOffset.Text = "YR Offset(um)";
            this.lbThetaOffset.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nudYLOffest
            // 
            this.nudYLOffest.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.nudYLOffest.DecimalPlaces = 1;
            this.nudYLOffest.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudYLOffest.Location = new System.Drawing.Point(155, 127);
            this.nudYLOffest.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.nudYLOffest.Name = "nudYLOffest";
            this.nudYLOffest.Size = new System.Drawing.Size(89, 30);
            this.nudYLOffest.TabIndex = 37;
            this.nudYLOffest.ValueChanged += new System.EventHandler(this.ValueChanged);
            // 
            // lbMaxAlignTime
            // 
            this.lbMaxAlignTime.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbMaxAlignTime.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbMaxAlignTime.Location = new System.Drawing.Point(321, 14);
            this.lbMaxAlignTime.Name = "lbMaxAlignTime";
            this.lbMaxAlignTime.Size = new System.Drawing.Size(121, 18);
            this.lbMaxAlignTime.TabIndex = 30;
            this.lbMaxAlignTime.Text = "Align Times";
            this.lbMaxAlignTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbStdMaxExpansion
            // 
            this.lbStdMaxExpansion.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbStdMaxExpansion.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbStdMaxExpansion.ForeColor = System.Drawing.Color.LightCoral;
            this.lbStdMaxExpansion.Location = new System.Drawing.Point(269, 164);
            this.lbStdMaxExpansion.Name = "lbStdMaxExpansion";
            this.lbStdMaxExpansion.Size = new System.Drawing.Size(224, 18);
            this.lbStdMaxExpansion.TabIndex = 33;
            this.lbStdMaxExpansion.Text = "Standard Max Expansion(um)";
            this.lbStdMaxExpansion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbStdYPre
            // 
            this.lbStdYPre.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbStdYPre.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbStdYPre.ForeColor = System.Drawing.Color.LightCoral;
            this.lbStdYPre.Location = new System.Drawing.Point(283, 97);
            this.lbStdYPre.Name = "lbStdYPre";
            this.lbStdYPre.Size = new System.Drawing.Size(197, 18);
            this.lbStdYPre.TabIndex = 45;
            this.lbStdYPre.Text = "Standard Y Precision(um)";
            this.lbStdYPre.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nudWaferScore
            // 
            this.nudWaferScore.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.nudWaferScore.DecimalPlaces = 2;
            this.nudWaferScore.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudWaferScore.Location = new System.Drawing.Point(155, 52);
            this.nudWaferScore.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            131072});
            this.nudWaferScore.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudWaferScore.Name = "nudWaferScore";
            this.nudWaferScore.Size = new System.Drawing.Size(89, 30);
            this.nudWaferScore.TabIndex = 35;
            this.nudWaferScore.Value = new decimal(new int[] {
            70,
            0,
            0,
            131072});
            this.nudWaferScore.ValueChanged += new System.EventHandler(this.ValueChanged);
            // 
            // nudYPrecision
            // 
            this.nudYPrecision.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.nudYPrecision.DecimalPlaces = 1;
            this.nudYPrecision.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudYPrecision.Location = new System.Drawing.Point(566, 92);
            this.nudYPrecision.Name = "nudYPrecision";
            this.nudYPrecision.Size = new System.Drawing.Size(89, 30);
            this.nudYPrecision.TabIndex = 46;
            this.nudYPrecision.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nudYPrecision.ValueChanged += new System.EventHandler(this.ValueChanged);
            // 
            // nudThetaPrecision
            // 
            this.nudThetaPrecision.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.nudThetaPrecision.DecimalPlaces = 1;
            this.nudThetaPrecision.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudThetaPrecision.Location = new System.Drawing.Point(566, 127);
            this.nudThetaPrecision.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudThetaPrecision.Name = "nudThetaPrecision";
            this.nudThetaPrecision.Size = new System.Drawing.Size(89, 30);
            this.nudThetaPrecision.TabIndex = 41;
            this.nudThetaPrecision.Value = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.nudThetaPrecision.ValueChanged += new System.EventHandler(this.ValueChanged);
            // 
            // lbNowRecipeNumber
            // 
            this.lbNowRecipeNumber.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbNowRecipeNumber.AutoSize = true;
            this.lbNowRecipeNumber.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbNowRecipeNumber.Location = new System.Drawing.Point(268, 191);
            this.lbNowRecipeNumber.Name = "lbNowRecipeNumber";
            this.lbNowRecipeNumber.Size = new System.Drawing.Size(226, 39);
            this.lbNowRecipeNumber.TabIndex = 44;
            this.lbNowRecipeNumber.Text = "Recipe Number  : ";
            // 
            // MaskImage
            // 
            this.MaskImage.AutoSize = true;
            this.MaskImage.Location = new System.Drawing.Point(622, 261);
            this.MaskImage.Name = "MaskImage";
            this.MaskImage.Size = new System.Drawing.Size(135, 27);
            this.MaskImage.TabIndex = 69;
            this.MaskImage.Text = "MaskImage";
            this.MaskImage.UseVisualStyleBackColor = true;
            this.MaskImage.CheckedChanged += new System.EventHandler(this.MaskImage_CheckedChanged);
            // 
            // lbCompany
            // 
            this.lbCompany.AutoSize = true;
            this.lbCompany.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCompany.Location = new System.Drawing.Point(26, 162);
            this.lbCompany.Name = "lbCompany";
            this.lbCompany.Size = new System.Drawing.Size(298, 24);
            this.lbCompany.TabIndex = 19;
            this.lbCompany.Text = "M&&R Nano Technology co. Ltd";
            this.lbCompany.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbNowTime
            // 
            this.lbNowTime.AutoSize = true;
            this.lbNowTime.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbNowTime.Location = new System.Drawing.Point(31, 294);
            this.lbNowTime.Name = "lbNowTime";
            this.lbNowTime.Size = new System.Drawing.Size(62, 29);
            this.lbNowTime.TabIndex = 18;
            this.lbNowTime.Text = "time";
            // 
            // btQuit
            // 
            this.btQuit.BackColor = System.Drawing.Color.MintCream;
            this.btQuit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btQuit.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btQuit.Location = new System.Drawing.Point(35, 249);
            this.btQuit.Name = "btQuit";
            this.btQuit.Size = new System.Drawing.Size(182, 33);
            this.btQuit.TabIndex = 17;
            this.btQuit.Text = "Quit";
            this.btQuit.UseVisualStyleBackColor = false;
            this.btQuit.Click += new System.EventHandler(this.BtQuit_Click);
            // 
            // btLogIn
            // 
            this.btLogIn.BackColor = System.Drawing.Color.MintCream;
            this.btLogIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btLogIn.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btLogIn.Location = new System.Drawing.Point(35, 194);
            this.btLogIn.Name = "btLogIn";
            this.btLogIn.Size = new System.Drawing.Size(182, 33);
            this.btLogIn.TabIndex = 16;
            this.btLogIn.Text = "Log in";
            this.btLogIn.UseVisualStyleBackColor = false;
            this.btLogIn.Click += new System.EventHandler(this.BtLogIn_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(35, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(182, 141);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 15;
            this.pictureBox1.TabStop = false;
            // 
            // tbStatus
            // 
            this.tbStatus.BackColor = System.Drawing.Color.GhostWhite;
            this.tbStatus.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbStatus.Location = new System.Drawing.Point(1216, 10);
            this.tbStatus.Multiline = true;
            this.tbStatus.Name = "tbStatus";
            this.tbStatus.ReadOnly = true;
            this.tbStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbStatus.Size = new System.Drawing.Size(688, 290);
            this.tbStatus.TabIndex = 14;
            // 
            // ucSaveImage
            // 
            this.ucSaveImage.BackColor = System.Drawing.Color.Transparent;
            this.ucSaveImage.Caption = "CaptureMask";
            this.ucSaveImage.FaceColor = System.Drawing.Color.Empty;
            this.ucSaveImage.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ucSaveImage.ForeColor = System.Drawing.Color.Red;
            this.ucSaveImage.Image = null;
            this.ucSaveImage.Location = new System.Drawing.Point(1095, 242);
            this.ucSaveImage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ucSaveImage.Name = "ucSaveImage";
            this.ucSaveImage.Radius = 5;
            this.ucSaveImage.Size = new System.Drawing.Size(100, 30);
            this.ucSaveImage.TabIndex = 68;
            this.ucSaveImage.Visible = false;
            this.ucSaveImage.Click += new System.EventHandler(this.UcSaveImage_Click);
            // 
            // ucSaveRecipe
            // 
            this.ucSaveRecipe.BackColor = System.Drawing.Color.Transparent;
            this.ucSaveRecipe.Caption = "SaveRecipe";
            this.ucSaveRecipe.FaceColor = System.Drawing.Color.Empty;
            this.ucSaveRecipe.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ucSaveRecipe.ForeColor = System.Drawing.Color.Red;
            this.ucSaveRecipe.Image = null;
            this.ucSaveRecipe.Location = new System.Drawing.Point(1095, 204);
            this.ucSaveRecipe.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ucSaveRecipe.Name = "ucSaveRecipe";
            this.ucSaveRecipe.Radius = 5;
            this.ucSaveRecipe.Size = new System.Drawing.Size(100, 30);
            this.ucSaveRecipe.TabIndex = 67;
            this.ucSaveRecipe.Visible = false;
            this.ucSaveRecipe.Click += new System.EventHandler(this.UcSaveRecipe_Click);
            // 
            // ucGetRecipe
            // 
            this.ucGetRecipe.BackColor = System.Drawing.Color.Transparent;
            this.ucGetRecipe.Caption = "GetRecipe";
            this.ucGetRecipe.FaceColor = System.Drawing.Color.Empty;
            this.ucGetRecipe.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ucGetRecipe.ForeColor = System.Drawing.Color.Red;
            this.ucGetRecipe.Image = null;
            this.ucGetRecipe.Location = new System.Drawing.Point(1095, 166);
            this.ucGetRecipe.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ucGetRecipe.Name = "ucGetRecipe";
            this.ucGetRecipe.Radius = 5;
            this.ucGetRecipe.Size = new System.Drawing.Size(100, 30);
            this.ucGetRecipe.TabIndex = 66;
            this.ucGetRecipe.Visible = false;
            this.ucGetRecipe.Click += new System.EventHandler(this.UcGetRecipe_Click);
            // 
            // ucGoRecipe
            // 
            this.ucGoRecipe.BackColor = System.Drawing.Color.Transparent;
            this.ucGoRecipe.Caption = "GoRecipe";
            this.ucGoRecipe.FaceColor = System.Drawing.Color.Empty;
            this.ucGoRecipe.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ucGoRecipe.ForeColor = System.Drawing.Color.Red;
            this.ucGoRecipe.Image = null;
            this.ucGoRecipe.Location = new System.Drawing.Point(1095, 128);
            this.ucGoRecipe.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ucGoRecipe.Name = "ucGoRecipe";
            this.ucGoRecipe.Radius = 5;
            this.ucGoRecipe.Size = new System.Drawing.Size(100, 30);
            this.ucGoRecipe.TabIndex = 65;
            this.ucGoRecipe.Visible = false;
            this.ucGoRecipe.Click += new System.EventHandler(this.UcGoRecipe_Click);
            // 
            // ucAlign
            // 
            this.ucAlign.BackColor = System.Drawing.Color.Transparent;
            this.ucAlign.Caption = "Align";
            this.ucAlign.FaceColor = System.Drawing.Color.Empty;
            this.ucAlign.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ucAlign.ForeColor = System.Drawing.Color.Red;
            this.ucAlign.Image = null;
            this.ucAlign.Location = new System.Drawing.Point(1095, 90);
            this.ucAlign.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ucAlign.Name = "ucAlign";
            this.ucAlign.Radius = 5;
            this.ucAlign.Size = new System.Drawing.Size(100, 30);
            this.ucAlign.TabIndex = 64;
            this.ucAlign.Visible = false;
            this.ucAlign.Click += new System.EventHandler(this.UcAlign_Click);
            // 
            // btSystemSetting
            // 
            this.btSystemSetting.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btSystemSetting.Location = new System.Drawing.Point(1095, 110);
            this.btSystemSetting.Name = "btSystemSetting";
            this.btSystemSetting.Size = new System.Drawing.Size(89, 27);
            this.btSystemSetting.TabIndex = 80;
            this.btSystemSetting.Text = "Setting";
            this.btSystemSetting.UseVisualStyleBackColor = true;
            this.btSystemSetting.Visible = false;
            this.btSystemSetting.Click += new System.EventHandler(this.BtSystemSetting_Click);
            // 
            // buttonTestMode
            // 
            this.buttonTestMode.Location = new System.Drawing.Point(1095, 93);
            this.buttonTestMode.Name = "buttonTestMode";
            this.buttonTestMode.Size = new System.Drawing.Size(106, 30);
            this.buttonTestMode.TabIndex = 20;
            this.buttonTestMode.Text = "Cycle test";
            this.buttonTestMode.UseVisualStyleBackColor = true;
            this.buttonTestMode.Visible = false;
            this.buttonTestMode.Click += new System.EventHandler(this.ButtonTestMode_Click);
            // 
            // buttonCapture
            // 
            this.buttonCapture.Location = new System.Drawing.Point(1092, 29);
            this.buttonCapture.Name = "buttonCapture";
            this.buttonCapture.Size = new System.Drawing.Size(100, 30);
            this.buttonCapture.TabIndex = 21;
            this.buttonCapture.Text = "Capture";
            this.buttonCapture.UseVisualStyleBackColor = true;
            this.buttonCapture.Visible = false;
            this.buttonCapture.Click += new System.EventHandler(this.ButtonCapture_Click);
            // 
            // lbRShowImage
            // 
            this.lbRShowImage.AutoSize = true;
            this.lbRShowImage.Location = new System.Drawing.Point(955, 875);
            this.lbRShowImage.Name = "lbRShowImage";
            this.lbRShowImage.Size = new System.Drawing.Size(138, 29);
            this.lbRShowImage.TabIndex = 96;
            this.lbRShowImage.Text = "右側透明度:";
            // 
            // btRShowImageSave
            // 
            this.btRShowImageSave.BackColor = System.Drawing.Color.LightCoral;
            this.btRShowImageSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btRShowImageSave.Font = new System.Drawing.Font("Arial", 10F);
            this.btRShowImageSave.Location = new System.Drawing.Point(1580, 870);
            this.btRShowImageSave.Name = "btRShowImageSave";
            this.btRShowImageSave.Size = new System.Drawing.Size(60, 65);
            this.btRShowImageSave.TabIndex = 94;
            this.btRShowImageSave.Text = "Save";
            this.btRShowImageSave.UseVisualStyleBackColor = false;
            this.btRShowImageSave.Visible = false;
            this.btRShowImageSave.Click += new System.EventHandler(this.BtRShowImageSave_Click);
            // 
            // btRShowImageMinus
            // 
            this.btRShowImageMinus.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.btRShowImageMinus.Location = new System.Drawing.Point(1535, 905);
            this.btRShowImageMinus.Name = "btRShowImageMinus";
            this.btRShowImageMinus.Size = new System.Drawing.Size(38, 30);
            this.btRShowImageMinus.TabIndex = 92;
            this.btRShowImageMinus.Text = "-";
            this.btRShowImageMinus.UseVisualStyleBackColor = true;
            this.btRShowImageMinus.Click += new System.EventHandler(this.BtRShowImageMinus_Click);
            // 
            // btRShowImagePlus
            // 
            this.btRShowImagePlus.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.btRShowImagePlus.Location = new System.Drawing.Point(1535, 870);
            this.btRShowImagePlus.Name = "btRShowImagePlus";
            this.btRShowImagePlus.Size = new System.Drawing.Size(38, 30);
            this.btRShowImagePlus.TabIndex = 91;
            this.btRShowImagePlus.Text = "+";
            this.btRShowImagePlus.UseVisualStyleBackColor = true;
            this.btRShowImagePlus.Click += new System.EventHandler(this.BtRShowImagePlus_Click);
            // 
            // tBRShowImageAlign
            // 
            this.tBRShowImageAlign.Enabled = false;
            this.tBRShowImageAlign.LargeChange = 10;
            this.tBRShowImageAlign.Location = new System.Drawing.Point(1080, 870);
            this.tBRShowImageAlign.Maximum = 100;
            this.tBRShowImageAlign.Name = "tBRShowImageAlign";
            this.tBRShowImageAlign.Size = new System.Drawing.Size(450, 56);
            this.tBRShowImageAlign.TabIndex = 88;
            this.tBRShowImageAlign.TickFrequency = 10;
            this.tBRShowImageAlign.Value = 100;
            this.tBRShowImageAlign.Scroll += new System.EventHandler(this.tBRShowImageAlign_Scroll);
            this.tBRShowImageAlign.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tBRShowImageAlign_KeyUp);
            this.tBRShowImageAlign.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tBRShowImageAlign_MouseUp);
            // 
            // lbLShowImage
            // 
            this.lbLShowImage.AutoSize = true;
            this.lbLShowImage.Location = new System.Drawing.Point(10, 875);
            this.lbLShowImage.Name = "lbLShowImage";
            this.lbLShowImage.Size = new System.Drawing.Size(138, 29);
            this.lbLShowImage.TabIndex = 95;
            this.lbLShowImage.Text = "左側透明度:";
            // 
            // btLShowImageSave
            // 
            this.btLShowImageSave.BackColor = System.Drawing.Color.LightCoral;
            this.btLShowImageSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btLShowImageSave.Font = new System.Drawing.Font("Arial", 10F);
            this.btLShowImageSave.Location = new System.Drawing.Point(635, 870);
            this.btLShowImageSave.Name = "btLShowImageSave";
            this.btLShowImageSave.Size = new System.Drawing.Size(60, 65);
            this.btLShowImageSave.TabIndex = 93;
            this.btLShowImageSave.Text = "Save";
            this.btLShowImageSave.UseVisualStyleBackColor = false;
            this.btLShowImageSave.Visible = false;
            this.btLShowImageSave.Click += new System.EventHandler(this.BtLShowImageSave_Click);
            // 
            // btLShowImageMinus
            // 
            this.btLShowImageMinus.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.btLShowImageMinus.Location = new System.Drawing.Point(590, 905);
            this.btLShowImageMinus.Name = "btLShowImageMinus";
            this.btLShowImageMinus.Size = new System.Drawing.Size(38, 30);
            this.btLShowImageMinus.TabIndex = 90;
            this.btLShowImageMinus.Text = "-";
            this.btLShowImageMinus.UseVisualStyleBackColor = true;
            this.btLShowImageMinus.Click += new System.EventHandler(this.BtLShowImageMinus_Click);
            // 
            // btLShowImagePlus
            // 
            this.btLShowImagePlus.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.btLShowImagePlus.Location = new System.Drawing.Point(590, 870);
            this.btLShowImagePlus.Name = "btLShowImagePlus";
            this.btLShowImagePlus.Size = new System.Drawing.Size(38, 30);
            this.btLShowImagePlus.TabIndex = 89;
            this.btLShowImagePlus.Text = "+";
            this.btLShowImagePlus.UseVisualStyleBackColor = true;
            this.btLShowImagePlus.Click += new System.EventHandler(this.BtLShowImagePlus_Click);
            // 
            // tBLShowImageAlign
            // 
            this.tBLShowImageAlign.Enabled = false;
            this.tBLShowImageAlign.LargeChange = 10;
            this.tBLShowImageAlign.Location = new System.Drawing.Point(135, 870);
            this.tBLShowImageAlign.Maximum = 100;
            this.tBLShowImageAlign.Name = "tBLShowImageAlign";
            this.tBLShowImageAlign.Size = new System.Drawing.Size(450, 56);
            this.tBLShowImageAlign.TabIndex = 87;
            this.tBLShowImageAlign.TickFrequency = 10;
            this.tBLShowImageAlign.Value = 100;
            this.tBLShowImageAlign.Scroll += new System.EventHandler(this.tBLShowImageAlign_Scroll);
            this.tBLShowImageAlign.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tBLShowImageAlign_KeyUp);
            this.tBLShowImageAlign.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tBLShowImageAlign_MouseUp);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.cmLearnPatternUp1);
            this.tabPage2.Location = new System.Drawing.Point(4, 38);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(192, 58);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Tag = "2";
            this.tabPage2.Text = "|  Pattern Edit Top  |";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // cmLearnPatternUp1
            // 
            this.cmLearnPatternUp1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.cmLearnPatternUp1.Font = new System.Drawing.Font("Arial", 12F);
            this.cmLearnPatternUp1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmLearnPatternUp1.Location = new System.Drawing.Point(0, 0);
            this.cmLearnPatternUp1.Name = "cmLearnPatternUp1";
            this.cmLearnPatternUp1.SetTimeString = null;
            this.cmLearnPatternUp1.Size = new System.Drawing.Size(1912, 1080);
            this.cmLearnPatternUp1.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.btReadMask);
            this.tabPage3.Controls.Add(this.ucOpenPad);
            this.tabPage3.Controls.Add(this.lbEmulationModePR);
            this.tabPage3.Controls.Add(this.lbEmulationModePL);
            this.tabPage3.Controls.Add(this.trackBar2);
            this.tabPage3.Controls.Add(this.trackBar1);
            this.tabPage3.Controls.Add(this.groupBoxMeasurePatternDistance);
            this.tabPage3.Controls.Add(this.label19);
            this.tabPage3.Controls.Add(this.buttonRightBrightnessIncrease);
            this.tabPage3.Controls.Add(this.buttonLeftBrightnessIncrease);
            this.tabPage3.Controls.Add(this.buttonRightBrightnessDecrease);
            this.tabPage3.Controls.Add(this.lbRCCDLight);
            this.tabPage3.Controls.Add(this.lbLCCDLight);
            this.tabPage3.Controls.Add(this.buttonLeftBrightnessDecrease);
            this.tabPage3.Controls.Add(this.gbMachingSetting);
            this.tabPage3.Controls.Add(this.label22);
            this.tabPage3.Controls.Add(this.gbZoomLensParameters);
            this.tabPage3.Controls.Add(this.gbCameraParm);
            this.tabPage3.Controls.Add(this.gbRightZoomLens);
            this.tabPage3.Controls.Add(this.gbLeftZoomLens);
            this.tabPage3.Controls.Add(this.btnQuit);
            this.tabPage3.Controls.Add(this.btZoomInfoWrite);
            this.tabPage3.Controls.Add(this.btZoomInfoRead);
            this.tabPage3.Controls.Add(this.skRightParam);
            this.tabPage3.Controls.Add(this.skLeftParam);
            this.tabPage3.Location = new System.Drawing.Point(4, 38);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(192, 58);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Tag = "3";
            this.tabPage3.Text = "|  Parameters Setting  |";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // btReadMask
            // 
            this.btReadMask.BackColor = System.Drawing.Color.Transparent;
            this.btReadMask.Caption = "Read Back Mask";
            this.btReadMask.FaceColor = System.Drawing.Color.Empty;
            this.btReadMask.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btReadMask.ForeColor = System.Drawing.Color.Red;
            this.btReadMask.Image = null;
            this.btReadMask.Location = new System.Drawing.Point(1774, 101);
            this.btReadMask.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btReadMask.Name = "btReadMask";
            this.btReadMask.Radius = 5;
            this.btReadMask.Size = new System.Drawing.Size(118, 35);
            this.btReadMask.TabIndex = 75;
            this.btReadMask.Click += new System.EventHandler(this.BtReadMask_Click);
            // 
            // ucOpenPad
            // 
            this.ucOpenPad.BackColor = System.Drawing.Color.Transparent;
            this.ucOpenPad.Caption = "OpenPad";
            this.ucOpenPad.FaceColor = System.Drawing.Color.Empty;
            this.ucOpenPad.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ucOpenPad.ForeColor = System.Drawing.Color.Red;
            this.ucOpenPad.Image = null;
            this.ucOpenPad.Location = new System.Drawing.Point(1773, 219);
            this.ucOpenPad.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ucOpenPad.Name = "ucOpenPad";
            this.ucOpenPad.Radius = 5;
            this.ucOpenPad.Size = new System.Drawing.Size(118, 35);
            this.ucOpenPad.TabIndex = 74;
            this.ucOpenPad.Click += new System.EventHandler(this.UcOpenPad_Click);
            // 
            // lbEmulationModePR
            // 
            this.lbEmulationModePR.AutoSize = true;
            this.lbEmulationModePR.Font = new System.Drawing.Font("標楷體", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbEmulationModePR.Location = new System.Drawing.Point(1292, 648);
            this.lbEmulationModePR.Name = "lbEmulationModePR";
            this.lbEmulationModePR.Size = new System.Drawing.Size(327, 44);
            this.lbEmulationModePR.TabIndex = 73;
            this.lbEmulationModePR.Text = "Emulation Mode";
            // 
            // lbEmulationModePL
            // 
            this.lbEmulationModePL.AutoSize = true;
            this.lbEmulationModePL.Font = new System.Drawing.Font("標楷體", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbEmulationModePL.Location = new System.Drawing.Point(279, 648);
            this.lbEmulationModePL.Name = "lbEmulationModePL";
            this.lbEmulationModePL.Size = new System.Drawing.Size(327, 44);
            this.lbEmulationModePL.TabIndex = 72;
            this.lbEmulationModePL.Text = "Emulation Mode";
            // 
            // trackBar2
            // 
            this.trackBar2.Location = new System.Drawing.Point(1192, 267);
            this.trackBar2.Name = "trackBar2";
            this.trackBar2.Size = new System.Drawing.Size(635, 56);
            this.trackBar2.TabIndex = 62;
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(213, 266);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(662, 56);
            this.trackBar1.TabIndex = 61;
            // 
            // groupBoxMeasurePatternDistance
            // 
            this.groupBoxMeasurePatternDistance.Controls.Add(this.lbStep);
            this.groupBoxMeasurePatternDistance.Controls.Add(this.buttonCalPatternDistance);
            this.groupBoxMeasurePatternDistance.Controls.Add(this.numericUpDownMeasurePatternDistanceSteps);
            this.groupBoxMeasurePatternDistance.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxMeasurePatternDistance.Location = new System.Drawing.Point(1496, 128);
            this.groupBoxMeasurePatternDistance.Name = "groupBoxMeasurePatternDistance";
            this.groupBoxMeasurePatternDistance.Size = new System.Drawing.Size(245, 100);
            this.groupBoxMeasurePatternDistance.TabIndex = 60;
            this.groupBoxMeasurePatternDistance.TabStop = false;
            this.groupBoxMeasurePatternDistance.Text = "Meas. pattern distance";
            // 
            // lbStep
            // 
            this.lbStep.AutoSize = true;
            this.lbStep.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbStep.Location = new System.Drawing.Point(28, 30);
            this.lbStep.Name = "lbStep";
            this.lbStep.Size = new System.Drawing.Size(73, 23);
            this.lbStep.TabIndex = 15;
            this.lbStep.Text = "Steps :";
            // 
            // buttonCalPatternDistance
            // 
            this.buttonCalPatternDistance.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCalPatternDistance.Location = new System.Drawing.Point(31, 61);
            this.buttonCalPatternDistance.Name = "buttonCalPatternDistance";
            this.buttonCalPatternDistance.Size = new System.Drawing.Size(172, 28);
            this.buttonCalPatternDistance.TabIndex = 13;
            this.buttonCalPatternDistance.Text = "Measure";
            this.buttonCalPatternDistance.UseVisualStyleBackColor = true;
            // 
            // numericUpDownMeasurePatternDistanceSteps
            // 
            this.numericUpDownMeasurePatternDistanceSteps.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownMeasurePatternDistanceSteps.Location = new System.Drawing.Point(98, 28);
            this.numericUpDownMeasurePatternDistanceSteps.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownMeasurePatternDistanceSteps.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.numericUpDownMeasurePatternDistanceSteps.Name = "numericUpDownMeasurePatternDistanceSteps";
            this.numericUpDownMeasurePatternDistanceSteps.Size = new System.Drawing.Size(105, 30);
            this.numericUpDownMeasurePatternDistanceSteps.TabIndex = 16;
            this.numericUpDownMeasurePatternDistanceSteps.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Arial", 14F);
            this.label19.Location = new System.Drawing.Point(960, 289);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(210, 27);
            this.label19.TabIndex = 50;
            this.label19.Text = "Right brightness : ";
            this.label19.Visible = false;
            // 
            // buttonRightBrightnessIncrease
            // 
            this.buttonRightBrightnessIncrease.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRightBrightnessIncrease.Location = new System.Drawing.Point(1833, 266);
            this.buttonRightBrightnessIncrease.Name = "buttonRightBrightnessIncrease";
            this.buttonRightBrightnessIncrease.Size = new System.Drawing.Size(58, 45);
            this.buttonRightBrightnessIncrease.TabIndex = 57;
            this.buttonRightBrightnessIncrease.Text = "+";
            this.buttonRightBrightnessIncrease.UseVisualStyleBackColor = true;
            this.buttonRightBrightnessIncrease.Visible = false;
            // 
            // buttonLeftBrightnessIncrease
            // 
            this.buttonLeftBrightnessIncrease.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonLeftBrightnessIncrease.Location = new System.Drawing.Point(889, 266);
            this.buttonLeftBrightnessIncrease.Name = "buttonLeftBrightnessIncrease";
            this.buttonLeftBrightnessIncrease.Size = new System.Drawing.Size(58, 45);
            this.buttonLeftBrightnessIncrease.TabIndex = 56;
            this.buttonLeftBrightnessIncrease.Text = "+";
            this.buttonLeftBrightnessIncrease.UseVisualStyleBackColor = true;
            this.buttonLeftBrightnessIncrease.Visible = false;
            // 
            // buttonRightBrightnessDecrease
            // 
            this.buttonRightBrightnessDecrease.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRightBrightnessDecrease.Location = new System.Drawing.Point(1128, 266);
            this.buttonRightBrightnessDecrease.Name = "buttonRightBrightnessDecrease";
            this.buttonRightBrightnessDecrease.Size = new System.Drawing.Size(58, 45);
            this.buttonRightBrightnessDecrease.TabIndex = 55;
            this.buttonRightBrightnessDecrease.Text = "-";
            this.buttonRightBrightnessDecrease.UseVisualStyleBackColor = true;
            this.buttonRightBrightnessDecrease.Visible = false;
            // 
            // lbRCCDLight
            // 
            this.lbRCCDLight.AutoSize = true;
            this.lbRCCDLight.Font = new System.Drawing.Font("Arial", 14F);
            this.lbRCCDLight.Location = new System.Drawing.Point(960, 266);
            this.lbRCCDLight.Name = "lbRCCDLight";
            this.lbRCCDLight.Size = new System.Drawing.Size(156, 27);
            this.lbRCCDLight.TabIndex = 54;
            this.lbRCCDLight.Text = "右CCD光源 : ";
            this.lbRCCDLight.Visible = false;
            // 
            // lbLCCDLight
            // 
            this.lbLCCDLight.AutoSize = true;
            this.lbLCCDLight.Font = new System.Drawing.Font("Arial", 14F);
            this.lbLCCDLight.Location = new System.Drawing.Point(3, 266);
            this.lbLCCDLight.Name = "lbLCCDLight";
            this.lbLCCDLight.Size = new System.Drawing.Size(156, 27);
            this.lbLCCDLight.TabIndex = 53;
            this.lbLCCDLight.Text = "左CCD光源 : ";
            this.lbLCCDLight.Visible = false;
            // 
            // buttonLeftBrightnessDecrease
            // 
            this.buttonLeftBrightnessDecrease.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonLeftBrightnessDecrease.Location = new System.Drawing.Point(149, 266);
            this.buttonLeftBrightnessDecrease.Name = "buttonLeftBrightnessDecrease";
            this.buttonLeftBrightnessDecrease.Size = new System.Drawing.Size(58, 45);
            this.buttonLeftBrightnessDecrease.TabIndex = 52;
            this.buttonLeftBrightnessDecrease.Text = "-";
            this.buttonLeftBrightnessDecrease.UseVisualStyleBackColor = true;
            this.buttonLeftBrightnessDecrease.Visible = false;
            // 
            // gbMachingSetting
            // 
            this.gbMachingSetting.Controls.Add(this.btnManualPlc);
            this.gbMachingSetting.Controls.Add(this.buttonSetting);
            this.gbMachingSetting.Controls.Add(this.btTuneLens);
            this.gbMachingSetting.Controls.Add(this.buttonInitialLens);
            this.gbMachingSetting.Controls.Add(this.btnTuneXyyTable);
            this.gbMachingSetting.Controls.Add(this.buttonTuneLight);
            this.gbMachingSetting.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbMachingSetting.Location = new System.Drawing.Point(1129, 113);
            this.gbMachingSetting.Name = "gbMachingSetting";
            this.gbMachingSetting.Size = new System.Drawing.Size(338, 122);
            this.gbMachingSetting.TabIndex = 51;
            this.gbMachingSetting.TabStop = false;
            this.gbMachingSetting.Text = "Maching setting";
            // 
            // btnManualPlc
            // 
            this.btnManualPlc.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnManualPlc.Location = new System.Drawing.Point(26, 34);
            this.btnManualPlc.Name = "btnManualPlc";
            this.btnManualPlc.Size = new System.Drawing.Size(89, 25);
            this.btnManualPlc.TabIndex = 25;
            this.btnManualPlc.Text = "PLC";
            this.btnManualPlc.UseVisualStyleBackColor = true;
            this.btnManualPlc.Click += new System.EventHandler(this.ButtonManualPlc_Click);
            // 
            // buttonSetting
            // 
            this.buttonSetting.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSetting.Location = new System.Drawing.Point(26, 77);
            this.buttonSetting.Name = "buttonSetting";
            this.buttonSetting.Size = new System.Drawing.Size(89, 27);
            this.buttonSetting.TabIndex = 29;
            this.buttonSetting.Text = "Setting";
            this.buttonSetting.UseVisualStyleBackColor = true;
            this.buttonSetting.Click += new System.EventHandler(this.ButtonSetting_Click);
            // 
            // btTuneLens
            // 
            this.btTuneLens.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btTuneLens.Location = new System.Drawing.Point(229, 34);
            this.btTuneLens.Name = "btTuneLens";
            this.btTuneLens.Size = new System.Drawing.Size(88, 25);
            this.btTuneLens.TabIndex = 22;
            this.btTuneLens.Text = "Tune Lens";
            this.btTuneLens.UseVisualStyleBackColor = true;
            this.btTuneLens.Click += new System.EventHandler(this.BtTuneLens_Click);
            // 
            // buttonInitialLens
            // 
            this.buttonInitialLens.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonInitialLens.Location = new System.Drawing.Point(229, 77);
            this.buttonInitialLens.Name = "buttonInitialLens";
            this.buttonInitialLens.Size = new System.Drawing.Size(88, 27);
            this.buttonInitialLens.TabIndex = 28;
            this.buttonInitialLens.Text = "Initial lens";
            this.buttonInitialLens.UseVisualStyleBackColor = true;
            this.buttonInitialLens.Click += new System.EventHandler(this.ButtonInitialLens_Click);
            // 
            // btnTuneXyyTable
            // 
            this.btnTuneXyyTable.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTuneXyyTable.Location = new System.Drawing.Point(121, 34);
            this.btnTuneXyyTable.Name = "btnTuneXyyTable";
            this.btnTuneXyyTable.Size = new System.Drawing.Size(101, 25);
            this.btnTuneXyyTable.TabIndex = 24;
            this.btnTuneXyyTable.Text = "Tune Table";
            this.btnTuneXyyTable.UseVisualStyleBackColor = true;
            this.btnTuneXyyTable.Click += new System.EventHandler(this.ButtonTuneXyyTable_Click);
            // 
            // buttonTuneLight
            // 
            this.buttonTuneLight.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonTuneLight.Location = new System.Drawing.Point(121, 78);
            this.buttonTuneLight.Name = "buttonTuneLight";
            this.buttonTuneLight.Size = new System.Drawing.Size(101, 27);
            this.buttonTuneLight.TabIndex = 26;
            this.buttonTuneLight.Text = "TuneLight";
            this.buttonTuneLight.UseVisualStyleBackColor = true;
            this.buttonTuneLight.Visible = false;
            this.buttonTuneLight.Click += new System.EventHandler(this.ButtonTuneLight_Click);
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Arial", 14F);
            this.label22.Location = new System.Drawing.Point(2, 289);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(193, 27);
            this.label22.TabIndex = 48;
            this.label22.Text = "Left brightness : ";
            this.label22.Visible = false;
            // 
            // gbZoomLensParameters
            // 
            this.gbZoomLensParameters.Controls.Add(this.btnSaveAlignMagnification);
            this.gbZoomLensParameters.Controls.Add(this.lbLowMagnification);
            this.gbZoomLensParameters.Controls.Add(this.cbLowMagnification);
            this.gbZoomLensParameters.Controls.Add(this.lbHighMagnification);
            this.gbZoomLensParameters.Controls.Add(this.cbHighMagnification);
            this.gbZoomLensParameters.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbZoomLensParameters.Location = new System.Drawing.Point(573, 113);
            this.gbZoomLensParameters.Name = "gbZoomLensParameters";
            this.gbZoomLensParameters.Size = new System.Drawing.Size(538, 120);
            this.gbZoomLensParameters.TabIndex = 45;
            this.gbZoomLensParameters.TabStop = false;
            this.gbZoomLensParameters.Text = "Align Magnification And Mask Searching Area";
            this.gbZoomLensParameters.Visible = false;
            // 
            // btnSaveAlignMagnification
            // 
            this.btnSaveAlignMagnification.BackColor = System.Drawing.Color.LightCoral;
            this.btnSaveAlignMagnification.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveAlignMagnification.Location = new System.Drawing.Point(334, 52);
            this.btnSaveAlignMagnification.Name = "btnSaveAlignMagnification";
            this.btnSaveAlignMagnification.Size = new System.Drawing.Size(189, 30);
            this.btnSaveAlignMagnification.TabIndex = 19;
            this.btnSaveAlignMagnification.Text = "Save Align Magnification";
            this.btnSaveAlignMagnification.UseVisualStyleBackColor = false;
            this.btnSaveAlignMagnification.Click += new System.EventHandler(this.ButtonSaveAlignMagnification_Click);
            // 
            // lbLowMagnification
            // 
            this.lbLowMagnification.AutoSize = true;
            this.lbLowMagnification.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbLowMagnification.Location = new System.Drawing.Point(35, 37);
            this.lbLowMagnification.Name = "lbLowMagnification";
            this.lbLowMagnification.Size = new System.Drawing.Size(222, 23);
            this.lbLowMagnification.TabIndex = 16;
            this.lbLowMagnification.Text = "Align LowMagnification :";
            // 
            // cbLowMagnification
            // 
            this.cbLowMagnification.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLowMagnification.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbLowMagnification.FormattingEnabled = true;
            this.cbLowMagnification.Items.AddRange(new object[] {
            "X1",
            "X2",
            "X3",
            "X4",
            "X5",
            "X6",
            "X7",
            "X8",
            "X9",
            "X10",
            "X11",
            "X12"});
            this.cbLowMagnification.Location = new System.Drawing.Point(228, 32);
            this.cbLowMagnification.Name = "cbLowMagnification";
            this.cbLowMagnification.Size = new System.Drawing.Size(60, 31);
            this.cbLowMagnification.TabIndex = 15;
            // 
            // lbHighMagnification
            // 
            this.lbHighMagnification.AutoSize = true;
            this.lbHighMagnification.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbHighMagnification.Location = new System.Drawing.Point(37, 82);
            this.lbHighMagnification.Name = "lbHighMagnification";
            this.lbHighMagnification.Size = new System.Drawing.Size(223, 23);
            this.lbHighMagnification.TabIndex = 17;
            this.lbHighMagnification.Text = "Align HighMagnification :";
            // 
            // cbHighMagnification
            // 
            this.cbHighMagnification.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbHighMagnification.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbHighMagnification.FormattingEnabled = true;
            this.cbHighMagnification.Items.AddRange(new object[] {
            "X1",
            "X2",
            "X3",
            "X4",
            "X5",
            "X6",
            "X7",
            "X8",
            "X9",
            "X10",
            "X11",
            "X12"});
            this.cbHighMagnification.Location = new System.Drawing.Point(228, 78);
            this.cbHighMagnification.Name = "cbHighMagnification";
            this.cbHighMagnification.Size = new System.Drawing.Size(60, 31);
            this.cbHighMagnification.TabIndex = 18;
            // 
            // gbCameraParm
            // 
            this.gbCameraParm.Controls.Add(this.radioButtonLeftBackCam);
            this.gbCameraParm.Controls.Add(this.radioButtonRightBackCam);
            this.gbCameraParm.Controls.Add(this.buttonCameraParameterResotre);
            this.gbCameraParm.Controls.Add(this.buttonCameraParmSave);
            this.gbCameraParm.Controls.Add(this.trackBarCameraParmeters);
            this.gbCameraParm.Controls.Add(this.comboBoxCameraParameter);
            this.gbCameraParm.Controls.Add(this.radioButtonLeftCamera);
            this.gbCameraParm.Controls.Add(this.radioButtonRightCamera);
            this.gbCameraParm.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbCameraParm.Location = new System.Drawing.Point(3, 100);
            this.gbCameraParm.Name = "gbCameraParm";
            this.gbCameraParm.Size = new System.Drawing.Size(555, 160);
            this.gbCameraParm.TabIndex = 44;
            this.gbCameraParm.TabStop = false;
            this.gbCameraParm.Text = "Camera Parmeters";
            // 
            // radioButtonLeftBackCam
            // 
            this.radioButtonLeftBackCam.AutoSize = true;
            this.radioButtonLeftBackCam.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonLeftBackCam.Location = new System.Drawing.Point(13, 65);
            this.radioButtonLeftBackCam.Name = "radioButtonLeftBackCam";
            this.radioButtonLeftBackCam.Size = new System.Drawing.Size(162, 27);
            this.radioButtonLeftBackCam.TabIndex = 25;
            this.radioButtonLeftBackCam.TabStop = true;
            this.radioButtonLeftBackCam.Text = "Left Back Cam";
            this.radioButtonLeftBackCam.UseVisualStyleBackColor = true;
            this.radioButtonLeftBackCam.CheckedChanged += new System.EventHandler(this.RadioButtonLeftBackCam_CheckedChanged);
            // 
            // radioButtonRightBackCam
            // 
            this.radioButtonRightBackCam.AutoSize = true;
            this.radioButtonRightBackCam.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonRightBackCam.Location = new System.Drawing.Point(160, 65);
            this.radioButtonRightBackCam.Name = "radioButtonRightBackCam";
            this.radioButtonRightBackCam.Size = new System.Drawing.Size(173, 27);
            this.radioButtonRightBackCam.TabIndex = 26;
            this.radioButtonRightBackCam.TabStop = true;
            this.radioButtonRightBackCam.Text = "Right Back Cam";
            this.radioButtonRightBackCam.UseVisualStyleBackColor = true;
            this.radioButtonRightBackCam.CheckedChanged += new System.EventHandler(this.radioButtonRightBackCam_CheckedChanged);
            // 
            // buttonCameraParameterResotre
            // 
            this.buttonCameraParameterResotre.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCameraParameterResotre.Location = new System.Drawing.Point(447, 31);
            this.buttonCameraParameterResotre.Name = "buttonCameraParameterResotre";
            this.buttonCameraParameterResotre.Size = new System.Drawing.Size(96, 29);
            this.buttonCameraParameterResotre.TabIndex = 24;
            this.buttonCameraParameterResotre.Text = "RESTORE";
            this.buttonCameraParameterResotre.UseVisualStyleBackColor = true;
            this.buttonCameraParameterResotre.Click += new System.EventHandler(this.ButtonCameraParameterResotre_Click);
            // 
            // buttonCameraParmSave
            // 
            this.buttonCameraParmSave.BackColor = System.Drawing.Color.LightCoral;
            this.buttonCameraParmSave.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCameraParmSave.Location = new System.Drawing.Point(447, 75);
            this.buttonCameraParmSave.Name = "buttonCameraParmSave";
            this.buttonCameraParmSave.Size = new System.Drawing.Size(96, 29);
            this.buttonCameraParmSave.TabIndex = 23;
            this.buttonCameraParmSave.Text = "SAVE";
            this.buttonCameraParmSave.UseVisualStyleBackColor = false;
            this.buttonCameraParmSave.Click += new System.EventHandler(this.ButtonCameraParmSave_Click);
            // 
            // trackBarCameraParmeters
            // 
            this.trackBarCameraParmeters.LargeChange = 1;
            this.trackBarCameraParmeters.Location = new System.Drawing.Point(13, 104);
            this.trackBarCameraParmeters.Maximum = 100;
            this.trackBarCameraParmeters.Name = "trackBarCameraParmeters";
            this.trackBarCameraParmeters.Size = new System.Drawing.Size(412, 56);
            this.trackBarCameraParmeters.TabIndex = 22;
            this.trackBarCameraParmeters.Scroll += new System.EventHandler(this.trackBarCameraParmeters_Scroll);
            // 
            // comboBoxCameraParameter
            // 
            this.comboBoxCameraParameter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCameraParameter.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxCameraParameter.FormattingEnabled = true;
            this.comboBoxCameraParameter.Items.AddRange(new object[] {
            "Gain",
            "Gamma",
            "BlackLevel",
            "ExposureTime"});
            this.comboBoxCameraParameter.Location = new System.Drawing.Point(304, 28);
            this.comboBoxCameraParameter.Name = "comboBoxCameraParameter";
            this.comboBoxCameraParameter.Size = new System.Drawing.Size(121, 31);
            this.comboBoxCameraParameter.TabIndex = 21;
            this.comboBoxCameraParameter.SelectedIndexChanged += new System.EventHandler(this.comboBoxCameraParameter_SelectedIndexChanged);
            // 
            // radioButtonLeftCamera
            // 
            this.radioButtonLeftCamera.AutoSize = true;
            this.radioButtonLeftCamera.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonLeftCamera.Location = new System.Drawing.Point(13, 32);
            this.radioButtonLeftCamera.Name = "radioButtonLeftCamera";
            this.radioButtonLeftCamera.Size = new System.Drawing.Size(141, 27);
            this.radioButtonLeftCamera.TabIndex = 19;
            this.radioButtonLeftCamera.TabStop = true;
            this.radioButtonLeftCamera.Text = "Left Camera";
            this.radioButtonLeftCamera.UseVisualStyleBackColor = true;
            this.radioButtonLeftCamera.CheckedChanged += new System.EventHandler(this.RadioButtonLeftCamera_CheckedChanged);
            // 
            // radioButtonRightCamera
            // 
            this.radioButtonRightCamera.AutoSize = true;
            this.radioButtonRightCamera.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonRightCamera.Location = new System.Drawing.Point(160, 32);
            this.radioButtonRightCamera.Name = "radioButtonRightCamera";
            this.radioButtonRightCamera.Size = new System.Drawing.Size(152, 27);
            this.radioButtonRightCamera.TabIndex = 20;
            this.radioButtonRightCamera.TabStop = true;
            this.radioButtonRightCamera.Text = "Right Camera";
            this.radioButtonRightCamera.UseVisualStyleBackColor = true;
            this.radioButtonRightCamera.CheckedChanged += new System.EventHandler(this.radioButtonRightCamera_CheckedChanged);
            // 
            // gbRightZoomLens
            // 
            this.gbRightZoomLens.Controls.Add(this.btRightX12);
            this.gbRightZoomLens.Controls.Add(this.btRightX11);
            this.gbRightZoomLens.Controls.Add(this.btRightX6);
            this.gbRightZoomLens.Controls.Add(this.btRightX10);
            this.gbRightZoomLens.Controls.Add(this.btRightX7);
            this.gbRightZoomLens.Controls.Add(this.btRightX9);
            this.gbRightZoomLens.Controls.Add(this.btRightX8);
            this.gbRightZoomLens.Controls.Add(this.btRightX1);
            this.gbRightZoomLens.Controls.Add(this.btRightX5);
            this.gbRightZoomLens.Controls.Add(this.btRightX2);
            this.gbRightZoomLens.Controls.Add(this.btRightX4);
            this.gbRightZoomLens.Controls.Add(this.btRightX3);
            this.gbRightZoomLens.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbRightZoomLens.Location = new System.Drawing.Point(955, 3);
            this.gbRightZoomLens.Name = "gbRightZoomLens";
            this.gbRightZoomLens.Size = new System.Drawing.Size(886, 91);
            this.gbRightZoomLens.TabIndex = 43;
            this.gbRightZoomLens.TabStop = false;
            this.gbRightZoomLens.Text = "Right Zoom Lens";
            // 
            // btRightX12
            // 
            this.btRightX12.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btRightX12.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btRightX12.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btRightX12.Location = new System.Drawing.Point(800, 28);
            this.btRightX12.Name = "btRightX12";
            this.btRightX12.Size = new System.Drawing.Size(70, 40);
            this.btRightX12.TabIndex = 11;
            this.btRightX12.Text = "X12";
            this.btRightX12.UseVisualStyleBackColor = false;
            this.btRightX12.Click += new System.EventHandler(this.BtRight_Click);
            // 
            // btRightX11
            // 
            this.btRightX11.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btRightX11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btRightX11.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btRightX11.Location = new System.Drawing.Point(728, 28);
            this.btRightX11.Name = "btRightX11";
            this.btRightX11.Size = new System.Drawing.Size(70, 40);
            this.btRightX11.TabIndex = 10;
            this.btRightX11.Text = "X11";
            this.btRightX11.UseVisualStyleBackColor = false;
            this.btRightX11.Click += new System.EventHandler(this.BtRight_Click);
            // 
            // btRightX6
            // 
            this.btRightX6.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btRightX6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btRightX6.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btRightX6.Location = new System.Drawing.Point(368, 28);
            this.btRightX6.Name = "btRightX6";
            this.btRightX6.Size = new System.Drawing.Size(70, 40);
            this.btRightX6.TabIndex = 5;
            this.btRightX6.Text = "X6";
            this.btRightX6.UseVisualStyleBackColor = false;
            this.btRightX6.Click += new System.EventHandler(this.BtRight_Click);
            // 
            // btRightX10
            // 
            this.btRightX10.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btRightX10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btRightX10.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btRightX10.Location = new System.Drawing.Point(656, 28);
            this.btRightX10.Name = "btRightX10";
            this.btRightX10.Size = new System.Drawing.Size(70, 40);
            this.btRightX10.TabIndex = 9;
            this.btRightX10.Text = "X10";
            this.btRightX10.UseVisualStyleBackColor = false;
            this.btRightX10.Click += new System.EventHandler(this.BtRight_Click);
            // 
            // btRightX7
            // 
            this.btRightX7.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btRightX7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btRightX7.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btRightX7.Location = new System.Drawing.Point(440, 28);
            this.btRightX7.Name = "btRightX7";
            this.btRightX7.Size = new System.Drawing.Size(70, 40);
            this.btRightX7.TabIndex = 6;
            this.btRightX7.Text = "X7";
            this.btRightX7.UseVisualStyleBackColor = false;
            this.btRightX7.Click += new System.EventHandler(this.BtRight_Click);
            // 
            // btRightX9
            // 
            this.btRightX9.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btRightX9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btRightX9.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btRightX9.Location = new System.Drawing.Point(584, 28);
            this.btRightX9.Name = "btRightX9";
            this.btRightX9.Size = new System.Drawing.Size(70, 40);
            this.btRightX9.TabIndex = 8;
            this.btRightX9.Text = "X9";
            this.btRightX9.UseVisualStyleBackColor = false;
            this.btRightX9.Click += new System.EventHandler(this.BtRight_Click);
            // 
            // btRightX8
            // 
            this.btRightX8.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btRightX8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btRightX8.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btRightX8.Location = new System.Drawing.Point(512, 28);
            this.btRightX8.Name = "btRightX8";
            this.btRightX8.Size = new System.Drawing.Size(70, 40);
            this.btRightX8.TabIndex = 7;
            this.btRightX8.Text = "X8";
            this.btRightX8.UseVisualStyleBackColor = false;
            this.btRightX8.Click += new System.EventHandler(this.BtRight_Click);
            // 
            // btRightX1
            // 
            this.btRightX1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btRightX1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btRightX1.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btRightX1.Location = new System.Drawing.Point(8, 28);
            this.btRightX1.Name = "btRightX1";
            this.btRightX1.Size = new System.Drawing.Size(70, 40);
            this.btRightX1.TabIndex = 0;
            this.btRightX1.Text = "X1";
            this.btRightX1.UseVisualStyleBackColor = false;
            this.btRightX1.Click += new System.EventHandler(this.BtRight_Click);
            // 
            // btRightX5
            // 
            this.btRightX5.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btRightX5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btRightX5.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btRightX5.Location = new System.Drawing.Point(296, 28);
            this.btRightX5.Name = "btRightX5";
            this.btRightX5.Size = new System.Drawing.Size(70, 40);
            this.btRightX5.TabIndex = 4;
            this.btRightX5.Text = "X5";
            this.btRightX5.UseVisualStyleBackColor = false;
            this.btRightX5.Click += new System.EventHandler(this.BtRight_Click);
            // 
            // btRightX2
            // 
            this.btRightX2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btRightX2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btRightX2.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btRightX2.Location = new System.Drawing.Point(80, 28);
            this.btRightX2.Name = "btRightX2";
            this.btRightX2.Size = new System.Drawing.Size(70, 40);
            this.btRightX2.TabIndex = 1;
            this.btRightX2.Text = "X2";
            this.btRightX2.UseVisualStyleBackColor = false;
            this.btRightX2.Click += new System.EventHandler(this.BtRight_Click);
            // 
            // btRightX4
            // 
            this.btRightX4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btRightX4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btRightX4.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btRightX4.Location = new System.Drawing.Point(224, 28);
            this.btRightX4.Name = "btRightX4";
            this.btRightX4.Size = new System.Drawing.Size(70, 40);
            this.btRightX4.TabIndex = 3;
            this.btRightX4.Text = "X4";
            this.btRightX4.UseVisualStyleBackColor = false;
            this.btRightX4.Click += new System.EventHandler(this.BtRight_Click);
            // 
            // btRightX3
            // 
            this.btRightX3.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btRightX3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btRightX3.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btRightX3.Location = new System.Drawing.Point(152, 28);
            this.btRightX3.Name = "btRightX3";
            this.btRightX3.Size = new System.Drawing.Size(70, 40);
            this.btRightX3.TabIndex = 2;
            this.btRightX3.Text = "X3";
            this.btRightX3.UseVisualStyleBackColor = false;
            this.btRightX3.Click += new System.EventHandler(this.BtRight_Click);
            // 
            // gbLeftZoomLens
            // 
            this.gbLeftZoomLens.Controls.Add(this.btLeftX12);
            this.gbLeftZoomLens.Controls.Add(this.btLeftX11);
            this.gbLeftZoomLens.Controls.Add(this.btLeftX6);
            this.gbLeftZoomLens.Controls.Add(this.btLeftX10);
            this.gbLeftZoomLens.Controls.Add(this.btLeftX7);
            this.gbLeftZoomLens.Controls.Add(this.btLeftX9);
            this.gbLeftZoomLens.Controls.Add(this.btLeftX8);
            this.gbLeftZoomLens.Controls.Add(this.btLeftX1);
            this.gbLeftZoomLens.Controls.Add(this.btLeftX5);
            this.gbLeftZoomLens.Controls.Add(this.btLeftX2);
            this.gbLeftZoomLens.Controls.Add(this.btLeftX4);
            this.gbLeftZoomLens.Controls.Add(this.btLeftX3);
            this.gbLeftZoomLens.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbLeftZoomLens.Location = new System.Drawing.Point(3, 3);
            this.gbLeftZoomLens.Name = "gbLeftZoomLens";
            this.gbLeftZoomLens.Size = new System.Drawing.Size(886, 91);
            this.gbLeftZoomLens.TabIndex = 42;
            this.gbLeftZoomLens.TabStop = false;
            this.gbLeftZoomLens.Text = "Left Zoom Lens";
            // 
            // btLeftX12
            // 
            this.btLeftX12.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btLeftX12.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btLeftX12.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btLeftX12.Location = new System.Drawing.Point(809, 28);
            this.btLeftX12.Name = "btLeftX12";
            this.btLeftX12.Size = new System.Drawing.Size(70, 40);
            this.btLeftX12.TabIndex = 11;
            this.btLeftX12.Text = "X12";
            this.btLeftX12.UseVisualStyleBackColor = false;
            this.btLeftX12.Click += new System.EventHandler(this.BtLeft_Click);
            // 
            // btLeftX11
            // 
            this.btLeftX11.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btLeftX11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btLeftX11.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btLeftX11.Location = new System.Drawing.Point(736, 28);
            this.btLeftX11.Name = "btLeftX11";
            this.btLeftX11.Size = new System.Drawing.Size(70, 40);
            this.btLeftX11.TabIndex = 10;
            this.btLeftX11.Text = "X11";
            this.btLeftX11.UseVisualStyleBackColor = false;
            this.btLeftX11.Click += new System.EventHandler(this.BtLeft_Click);
            // 
            // btLeftX6
            // 
            this.btLeftX6.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btLeftX6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btLeftX6.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btLeftX6.Location = new System.Drawing.Point(371, 28);
            this.btLeftX6.Name = "btLeftX6";
            this.btLeftX6.Size = new System.Drawing.Size(70, 40);
            this.btLeftX6.TabIndex = 5;
            this.btLeftX6.Text = "X6";
            this.btLeftX6.UseVisualStyleBackColor = false;
            this.btLeftX6.Click += new System.EventHandler(this.BtLeft_Click);
            // 
            // btLeftX10
            // 
            this.btLeftX10.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btLeftX10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btLeftX10.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btLeftX10.Location = new System.Drawing.Point(663, 28);
            this.btLeftX10.Name = "btLeftX10";
            this.btLeftX10.Size = new System.Drawing.Size(70, 40);
            this.btLeftX10.TabIndex = 9;
            this.btLeftX10.Text = "X10";
            this.btLeftX10.UseVisualStyleBackColor = false;
            this.btLeftX10.Click += new System.EventHandler(this.BtLeft_Click);
            // 
            // btLeftX7
            // 
            this.btLeftX7.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btLeftX7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btLeftX7.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btLeftX7.Location = new System.Drawing.Point(444, 28);
            this.btLeftX7.Name = "btLeftX7";
            this.btLeftX7.Size = new System.Drawing.Size(70, 40);
            this.btLeftX7.TabIndex = 6;
            this.btLeftX7.Text = "X7";
            this.btLeftX7.UseVisualStyleBackColor = false;
            this.btLeftX7.Click += new System.EventHandler(this.BtLeft_Click);
            // 
            // btLeftX9
            // 
            this.btLeftX9.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btLeftX9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btLeftX9.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btLeftX9.Location = new System.Drawing.Point(590, 28);
            this.btLeftX9.Name = "btLeftX9";
            this.btLeftX9.Size = new System.Drawing.Size(70, 40);
            this.btLeftX9.TabIndex = 8;
            this.btLeftX9.Text = "X9";
            this.btLeftX9.UseVisualStyleBackColor = false;
            this.btLeftX9.Click += new System.EventHandler(this.BtLeft_Click);
            // 
            // btLeftX8
            // 
            this.btLeftX8.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btLeftX8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btLeftX8.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btLeftX8.Location = new System.Drawing.Point(517, 28);
            this.btLeftX8.Name = "btLeftX8";
            this.btLeftX8.Size = new System.Drawing.Size(70, 40);
            this.btLeftX8.TabIndex = 7;
            this.btLeftX8.Text = "X8";
            this.btLeftX8.UseVisualStyleBackColor = false;
            this.btLeftX8.Click += new System.EventHandler(this.BtLeft_Click);
            // 
            // btLeftX1
            // 
            this.btLeftX1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btLeftX1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btLeftX1.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btLeftX1.Location = new System.Drawing.Point(6, 28);
            this.btLeftX1.Name = "btLeftX1";
            this.btLeftX1.Size = new System.Drawing.Size(70, 40);
            this.btLeftX1.TabIndex = 0;
            this.btLeftX1.Text = "X1";
            this.btLeftX1.UseVisualStyleBackColor = false;
            this.btLeftX1.Click += new System.EventHandler(this.BtLeft_Click);
            // 
            // btLeftX5
            // 
            this.btLeftX5.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btLeftX5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btLeftX5.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btLeftX5.Location = new System.Drawing.Point(298, 28);
            this.btLeftX5.Name = "btLeftX5";
            this.btLeftX5.Size = new System.Drawing.Size(70, 40);
            this.btLeftX5.TabIndex = 4;
            this.btLeftX5.Text = "X5";
            this.btLeftX5.UseVisualStyleBackColor = false;
            this.btLeftX5.Click += new System.EventHandler(this.BtLeft_Click);
            // 
            // btLeftX2
            // 
            this.btLeftX2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btLeftX2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btLeftX2.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btLeftX2.Location = new System.Drawing.Point(79, 28);
            this.btLeftX2.Name = "btLeftX2";
            this.btLeftX2.Size = new System.Drawing.Size(70, 40);
            this.btLeftX2.TabIndex = 1;
            this.btLeftX2.Text = "X2";
            this.btLeftX2.UseVisualStyleBackColor = false;
            this.btLeftX2.Click += new System.EventHandler(this.BtLeft_Click);
            // 
            // btLeftX4
            // 
            this.btLeftX4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btLeftX4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btLeftX4.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btLeftX4.Location = new System.Drawing.Point(225, 28);
            this.btLeftX4.Name = "btLeftX4";
            this.btLeftX4.Size = new System.Drawing.Size(70, 40);
            this.btLeftX4.TabIndex = 3;
            this.btLeftX4.Text = "X4";
            this.btLeftX4.UseVisualStyleBackColor = false;
            this.btLeftX4.Click += new System.EventHandler(this.BtLeft_Click);
            // 
            // btLeftX3
            // 
            this.btLeftX3.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btLeftX3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btLeftX3.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btLeftX3.Location = new System.Drawing.Point(152, 28);
            this.btLeftX3.Name = "btLeftX3";
            this.btLeftX3.Size = new System.Drawing.Size(70, 40);
            this.btLeftX3.TabIndex = 2;
            this.btLeftX3.Text = "X3";
            this.btLeftX3.UseVisualStyleBackColor = false;
            this.btLeftX3.Click += new System.EventHandler(this.BtLeft_Click);
            // 
            // btnQuit
            // 
            this.btnQuit.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnQuit.Location = new System.Drawing.Point(1773, 195);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(118, 43);
            this.btnQuit.TabIndex = 46;
            this.btnQuit.Text = "QUIT";
            this.btnQuit.UseVisualStyleBackColor = true;
            this.btnQuit.Visible = false;
            // 
            // btZoomInfoWrite
            // 
            this.btZoomInfoWrite.BackColor = System.Drawing.Color.Transparent;
            this.btZoomInfoWrite.Caption = "ZoomInfo Write";
            this.btZoomInfoWrite.FaceColor = System.Drawing.Color.Empty;
            this.btZoomInfoWrite.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btZoomInfoWrite.ForeColor = System.Drawing.Color.Red;
            this.btZoomInfoWrite.Image = null;
            this.btZoomInfoWrite.Location = new System.Drawing.Point(1773, 181);
            this.btZoomInfoWrite.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btZoomInfoWrite.Name = "btZoomInfoWrite";
            this.btZoomInfoWrite.Radius = 5;
            this.btZoomInfoWrite.Size = new System.Drawing.Size(118, 35);
            this.btZoomInfoWrite.TabIndex = 71;
            this.btZoomInfoWrite.Click += new System.EventHandler(this.BtZoomInfoWrite_Click);
            // 
            // btZoomInfoRead
            // 
            this.btZoomInfoRead.BackColor = System.Drawing.Color.Transparent;
            this.btZoomInfoRead.Caption = "ZoomInfo Read";
            this.btZoomInfoRead.FaceColor = System.Drawing.Color.Empty;
            this.btZoomInfoRead.Font = new System.Drawing.Font("微軟正黑體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btZoomInfoRead.ForeColor = System.Drawing.Color.Red;
            this.btZoomInfoRead.Image = null;
            this.btZoomInfoRead.Location = new System.Drawing.Point(1773, 143);
            this.btZoomInfoRead.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btZoomInfoRead.Name = "btZoomInfoRead";
            this.btZoomInfoRead.Radius = 5;
            this.btZoomInfoRead.Size = new System.Drawing.Size(118, 35);
            this.btZoomInfoRead.TabIndex = 70;
            this.btZoomInfoRead.Click += new System.EventHandler(this.BtZoomInfoRead_Click);
            // 
            // skRightParam
            // 
            this.skRightParam.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.skRightParam.Location = new System.Drawing.Point(955, 325);
            this.skRightParam.Name = "skRightParam";
            this.skRightParam.Size = new System.Drawing.Size(944, 708);
            this.skRightParam.TabIndex = 64;
            // 
            // skLeftParam
            // 
            this.skLeftParam.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.skLeftParam.Location = new System.Drawing.Point(6, 325);
            this.skLeftParam.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.skLeftParam.Name = "skLeftParam";
            this.skLeftParam.Size = new System.Drawing.Size(944, 708);
            this.skLeftParam.TabIndex = 63;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.cmLearnPatternBack1);
            this.tabPage4.Location = new System.Drawing.Point(4, 38);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(192, 58);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Tag = "4";
            this.tabPage4.Text = "|  Pattern Edit Bottom  |";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // cmLearnPatternBack1
            // 
            this.cmLearnPatternBack1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.cmLearnPatternBack1.Font = new System.Drawing.Font("Arial", 12F);
            this.cmLearnPatternBack1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmLearnPatternBack1.Location = new System.Drawing.Point(0, 0);
            this.cmLearnPatternBack1.Name = "cmLearnPatternBack1";
            this.cmLearnPatternBack1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmLearnPatternBack1.setTimeString = null;
            this.cmLearnPatternBack1.Size = new System.Drawing.Size(1912, 1080);
            this.cmLearnPatternBack1.TabIndex = 0;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.cmzCalibration1);
            this.tabPage5.Controls.Add(this.skZoomAndPanWindow6);
            this.tabPage5.Location = new System.Drawing.Point(4, 38);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(192, 58);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Tag = "5";
            this.tabPage5.Text = "| BackSide Z Calibration  |";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // cmzCalibration1
            // 
            this.cmzCalibration1.Location = new System.Drawing.Point(0, 0);
            this.cmzCalibration1.Name = "cmzCalibration1";
            this.cmzCalibration1.Size = new System.Drawing.Size(1912, 1020);
            this.cmzCalibration1.TabIndex = 1;
            // 
            // skZoomAndPanWindow6
            // 
            this.skZoomAndPanWindow6.Location = new System.Drawing.Point(3, 3);
            this.skZoomAndPanWindow6.Name = "skZoomAndPanWindow6";
            this.skZoomAndPanWindow6.Size = new System.Drawing.Size(800, 450);
            this.skZoomAndPanWindow6.TabIndex = 0;
            // 
            // timerNowTime
            // 
            this.timerNowTime.Tick += new System.EventHandler(this.TimerNowTime_Tick);
            // 
            // FormMain
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1536, 864);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormMain";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.Shown += new System.EventHandler(this.FormMain_Shown);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tBLeftCoaLight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tBRightCoaLight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinAlignTimes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxAlignTimes)).EndInit();
            this.gbAlignCondition.ResumeLayout(false);
            this.gbAlignCondition.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nUDMarkDistance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaskScore)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudExpansion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudXOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudXPrecision)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudYROffest)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudYLOffest)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWaferScore)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudYPrecision)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThetaPrecision)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tBRShowImageAlign)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tBLShowImageAlign)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.groupBoxMeasurePatternDistance.ResumeLayout(false);
            this.groupBoxMeasurePatternDistance.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMeasurePatternDistanceSteps)).EndInit();
            this.gbMachingSetting.ResumeLayout(false);
            this.gbZoomLensParameters.ResumeLayout(false);
            this.gbZoomLensParameters.PerformLayout();
            this.gbCameraParm.ResumeLayout(false);
            this.gbCameraParm.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarCameraParmeters)).EndInit();
            this.gbRightZoomLens.ResumeLayout(false);
            this.gbLeftZoomLens.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void FormMain_FormClosing(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        private System.Windows.Forms.TrackBar tBLShowImageAlign;
        private System.Windows.Forms.TrackBar tBRShowImageAlign;
        private System.Windows.Forms.Button btLShowImagePlus;
        private System.Windows.Forms.Button btLShowImageMinus;
        private System.Windows.Forms.Button btRShowImagePlus;
        private System.Windows.Forms.Button btRShowImageMinus;
        private System.Windows.Forms.Button btLShowImageSave;
        private System.Windows.Forms.Button btRShowImageSave;
        private System.Windows.Forms.Label lbLShowImage;
        private System.Windows.Forms.Label lbRShowImage;

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox gbAlignCondition;
        private System.Windows.Forms.NumericUpDown nUDMarkDistance;
        private System.Windows.Forms.Label lbMarkDist;
        private System.Windows.Forms.Label lbMaskScore;
        private System.Windows.Forms.NumericUpDown nudMaskScore;
        private System.Windows.Forms.Label lbStdXpre;
        private System.Windows.Forms.Label lbWaferScore;
        private System.Windows.Forms.Label lbMaxAlignTime;
        private System.Windows.Forms.Label lbThetaOffset;
        private System.Windows.Forms.Label lbXOffset;
        private System.Windows.Forms.Label lbYOffset;
        private System.Windows.Forms.NumericUpDown nudWaferScore;
        private System.Windows.Forms.NumericUpDown nudXOffset;
        private System.Windows.Forms.NumericUpDown nudYLOffest;
        private System.Windows.Forms.NumericUpDown nudYROffest;
        private System.Windows.Forms.NumericUpDown nudMaxAlignTimes;
        private System.Windows.Forms.NumericUpDown nudXPrecision;
        private System.Windows.Forms.Button btSaveAlignParm;
        private System.Windows.Forms.Label lbStdMaxExpansion;
        private System.Windows.Forms.NumericUpDown nudExpansion;
        private System.Windows.Forms.Label lbStdThetaPre;
        private System.Windows.Forms.NumericUpDown nudThetaPrecision;
        private System.Windows.Forms.Label lbStdYPre;
        private System.Windows.Forms.NumericUpDown nudYPrecision;
        private System.Windows.Forms.Label lbNowRecipeNumber;
        private System.Windows.Forms.Button buttonCapture;
        private System.Windows.Forms.Button buttonTestMode;
        private System.Windows.Forms.Label lbCompany;
        private System.Windows.Forms.Label lbNowTime;
        private System.Windows.Forms.Button btQuit;
        private System.Windows.Forms.Button btLogIn;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox tbStatus;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Button buttonRightBrightnessIncrease;
        private System.Windows.Forms.Button buttonLeftBrightnessIncrease;
        private System.Windows.Forms.Button buttonRightBrightnessDecrease;
        private System.Windows.Forms.Label lbRCCDLight;
        private System.Windows.Forms.Label lbLCCDLight;
        private System.Windows.Forms.Button buttonLeftBrightnessDecrease;
        private System.Windows.Forms.GroupBox gbMachingSetting;
        private System.Windows.Forms.Button btnManualPlc;
        private System.Windows.Forms.Button buttonSetting;
        private System.Windows.Forms.Button btTuneLens;
        private System.Windows.Forms.Button buttonInitialLens;
        private System.Windows.Forms.Button btnTuneXyyTable;
        private System.Windows.Forms.Button buttonTuneLight;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Button btnQuit;
        private System.Windows.Forms.GroupBox gbZoomLensParameters;
        private System.Windows.Forms.Button btnSaveAlignMagnification;
        private System.Windows.Forms.Label lbLowMagnification;
        private System.Windows.Forms.ComboBox cbLowMagnification;
        private System.Windows.Forms.Label lbHighMagnification;
        private System.Windows.Forms.ComboBox cbHighMagnification;
        private System.Windows.Forms.GroupBox gbCameraParm;
        private System.Windows.Forms.Button buttonCameraParameterResotre;
        private System.Windows.Forms.Button buttonCameraParmSave;
        private System.Windows.Forms.TrackBar trackBarCameraParmeters;
        private System.Windows.Forms.ComboBox comboBoxCameraParameter;
        private System.Windows.Forms.RadioButton radioButtonLeftCamera;
        private System.Windows.Forms.RadioButton radioButtonRightCamera;
        private System.Windows.Forms.GroupBox gbRightZoomLens;
        private System.Windows.Forms.Button btRightX6;
        private System.Windows.Forms.Button btRightX10;
        private System.Windows.Forms.Button btRightX7;
        private System.Windows.Forms.Button btRightX9;
        private System.Windows.Forms.Button btRightX8;
        private System.Windows.Forms.Button btRightX1;
        private System.Windows.Forms.Button btRightX5;
        private System.Windows.Forms.Button btRightX2;
        private System.Windows.Forms.Button btRightX4;
        private System.Windows.Forms.Button btRightX3;
        private System.Windows.Forms.GroupBox gbLeftZoomLens;
        private System.Windows.Forms.Button btLeftX6;
        private System.Windows.Forms.Button btLeftX10;
        private System.Windows.Forms.Button btLeftX7;
        private System.Windows.Forms.Button btLeftX9;
        private System.Windows.Forms.Button btLeftX8;
        private System.Windows.Forms.Button btLeftX1;
        private System.Windows.Forms.Button btLeftX5;
        private System.Windows.Forms.Button btLeftX2;
        private System.Windows.Forms.Button btLeftX4;
        private System.Windows.Forms.Button btLeftX3;
        private System.Windows.Forms.Label labelCycleTestCycleTestTimes;
        private System.Windows.Forms.Timer timerUpdateInfomationAccordingRecipeNumber;
        private System.Windows.Forms.Timer timerCheckPlcDram;
        private System.Windows.Forms.Timer timerNowTime;
        private System.Windows.Forms.RadioButton radioButtonLeftBackCam;
        private System.Windows.Forms.RadioButton radioButtonRightBackCam;
        private System.Windows.Forms.GroupBox groupBoxMeasurePatternDistance;
        private System.Windows.Forms.Label lbStep;
        private System.Windows.Forms.Button buttonCalPatternDistance;
        private System.Windows.Forms.NumericUpDown numericUpDownMeasurePatternDistanceSteps;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TrackBar trackBar2;
        private System.Windows.Forms.TrackBar trackBar1;
        private MRLibrary.SKZoomAndPanWindow skZoomAndPanWindow1;
        private CMLearnPatternUp cmLearnPatternUp1;
        private CMLearnPatternBack cmLearnPatternBack1;
        private MRLibrary.SKZoomAndPanWindow skLeftAlign;
        private MRLibrary.SKZoomAndPanWindow skRightAlign;
        private MRLibrary.SKZoomAndPanWindow skRightParam;
        private MRLibrary.SKZoomAndPanWindow skLeftParam;
        private System.Windows.Forms.TabPage tabPage5;
        private MRLibrary.SKZoomAndPanWindow skZoomAndPanWindow6;
        private CMZCalibration cmzCalibration1;
        private System.Windows.Forms.Button btRightX12;
        private System.Windows.Forms.Button btRightX11;
        private System.Windows.Forms.Button btLeftX12;
        private System.Windows.Forms.Button btLeftX11;
        private MRLibrary.UCButton ucAlign;
        private MRLibrary.UCButton ucGoRecipe;
        private MRLibrary.UCButton ucSaveRecipe;
        private MRLibrary.UCButton ucGetRecipe;
        private MRLibrary.UCButton btZoomInfoRead;
        private System.Windows.Forms.CheckBox MaskImage;
        private MRLibrary.UCButton btZoomInfoWrite;
        private System.Windows.Forms.Label lbEmulationModeL;
        private System.Windows.Forms.Label lbEmulationModeR;
        private System.Windows.Forms.Label lbEmulationModePR;
        private System.Windows.Forms.Label lbEmulationModePL;
        private MRLibrary.UCButton ucOpenPad;
        private System.Windows.Forms.Label lbRightCCD;
        private System.Windows.Forms.Label lbLeftCCD;
        private System.Windows.Forms.CheckBox cBDown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudMinAlignTimes;
        private System.Windows.Forms.Label lbMin;
        private System.Windows.Forms.Label lbMax;
        private System.Windows.Forms.Label lbVerson;
        private System.Windows.Forms.Button btSystemSetting;
        private System.Windows.Forms.Button btZCa;
        private System.Windows.Forms.Button btNG;
        private System.Windows.Forms.CheckBox ckAutoAlign;
        private System.Windows.Forms.Button btACK;
        private MRLibrary.UCButton ucSaveImage;
        private System.Windows.Forms.Button btCapMask;
        private MRLibrary.UCButton btReadMask;
        private System.Windows.Forms.Button BtGetMask;
        private System.Windows.Forms.Label lbMagnification;
        private System.Windows.Forms.ComboBox cbMagnification;
        private System.Windows.Forms.TrackBar tBLeftCoaLight;
        private System.Windows.Forms.TrackBar tBRightCoaLight;
        private System.Windows.Forms.Button btLCoaLightPlus;
        private System.Windows.Forms.Button btLCoaLightMinus;
        private System.Windows.Forms.Button btRCoaLightPlus;
        private System.Windows.Forms.Button btRCoaLightMinus;
        private System.Windows.Forms.CheckBox chbPixelShift;
        private System.Windows.Forms.ComboBox cbPixelShift;
    }
}

