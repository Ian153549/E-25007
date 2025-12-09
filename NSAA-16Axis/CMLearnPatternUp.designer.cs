using System;

namespace NSAA_16Axis
{
    partial class CMLearnPatternUp
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gbLMask = new System.Windows.Forms.GroupBox();
            this.btFindLMaskCenter = new System.Windows.Forms.Button();
            this.btCreateLMaskPatternMask = new System.Windows.Forms.Button();
            this.pbLMaskImage = new System.Windows.Forms.PictureBox();
            this.btLMaskLocationSave = new System.Windows.Forms.Button();
            this.cbLMaskAlgorithm = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.gbLWafer = new System.Windows.Forms.GroupBox();
            this.btLabelPatternL = new System.Windows.Forms.Button();
            this.cbLWaferClassList = new System.Windows.Forms.ComboBox();
            this.btFindLWaferCenter = new System.Windows.Forms.Button();
            this.btCreateLWaferPatternMask = new System.Windows.Forms.Button();
            this.pbLWaferImage = new System.Windows.Forms.PictureBox();
            this.btLWaferLoactionSave = new System.Windows.Forms.Button();
            this.cbLWaferAlgorithm = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gbRWafer = new System.Windows.Forms.GroupBox();
            this.btLabelPatternR = new System.Windows.Forms.Button();
            this.cbRWaferClassList = new System.Windows.Forms.ComboBox();
            this.BtLeftWaferFocus = new System.Windows.Forms.Button();
            this.btFindRWaferCenter = new System.Windows.Forms.Button();
            this.btCreateRWaferPatternMask = new System.Windows.Forms.Button();
            this.pbRWaferImage = new System.Windows.Forms.PictureBox();
            this.btRWaferLocationSave = new System.Windows.Forms.Button();
            this.cbRWaferAlgorithm = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.gbRMask = new System.Windows.Forms.GroupBox();
            this.btFindRMaskCenter = new System.Windows.Forms.Button();
            this.btCreateRMaskPatternMask = new System.Windows.Forms.Button();
            this.pbRMaskImage = new System.Windows.Forms.PictureBox();
            this.btRMaskLocationSave = new System.Windows.Forms.Button();
            this.cbRMaskAlgorithm = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lbRecipeNumber = new System.Windows.Forms.Label();
            this.gBMag = new System.Windows.Forms.GroupBox();
            this.btMagSaveHigh = new System.Windows.Forms.Button();
            this.btMagSaveLow = new System.Windows.Forms.Button();
            this.rBHighMagnification = new System.Windows.Forms.RadioButton();
            this.rBLowMagnification = new System.Windows.Forms.RadioButton();
            this.cbHighMagnification = new System.Windows.Forms.ComboBox();
            this.cbLowMagnification = new System.Windows.Forms.ComboBox();
            this.cbPatterhShift = new System.Windows.Forms.ComboBox();
            this.tBRightRingLight = new System.Windows.Forms.TrackBar();
            this.tBLeftRingLight = new System.Windows.Forms.TrackBar();
            this.tBLeftCoaLight = new System.Windows.Forms.TrackBar();
            this.tBRightCoaLight = new System.Windows.Forms.TrackBar();
            this.LRingLight = new System.Windows.Forms.CheckBox();
            this.LCoaLight = new System.Windows.Forms.CheckBox();
            this.RCoaLight = new System.Windows.Forms.CheckBox();
            this.RRingLight = new System.Windows.Forms.CheckBox();
            this.btLCoaLightPlus = new System.Windows.Forms.Button();
            this.btLCoaLightMinus = new System.Windows.Forms.Button();
            this.btLRingLightMinus = new System.Windows.Forms.Button();
            this.btLRingLightPlus = new System.Windows.Forms.Button();
            this.btRCoaLightMinus = new System.Windows.Forms.Button();
            this.btRCoaLightPlus = new System.Windows.Forms.Button();
            this.btRRingLightMinus = new System.Windows.Forms.Button();
            this.btRRingLightPlus = new System.Windows.Forms.Button();
            this.lbEmulationModeL = new System.Windows.Forms.Label();
            this.lbEmulationModeR = new System.Windows.Forms.Label();
            this.btRRingSave = new System.Windows.Forms.Button();
            this.btRCoaSave = new System.Windows.Forms.Button();
            this.btLRingSave = new System.Windows.Forms.Button();
            this.btLCoaSave = new System.Windows.Forms.Button();
            this.lbNowTime = new System.Windows.Forms.Label();
            this.lbDebugMsg = new System.Windows.Forms.Label();
            this.btDelete = new System.Windows.Forms.Button();
            this.btDefault = new System.Windows.Forms.Button();
            this.lbRMsgW = new System.Windows.Forms.Label();
            this.lbLMsgW = new System.Windows.Forms.Label();
            this.lbRMsgM = new System.Windows.Forms.Label();
            this.lbLMsgM = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.nuDRotate = new System.Windows.Forms.NumericUpDown();
            this.btGo = new MRLibrary.UCButton();
            this.btUpdate = new MRLibrary.UCButton();
            this.nUDXyyY2 = new System.Windows.Forms.NumericUpDown();
            this.nUDXyyY1 = new System.Windows.Forms.NumericUpDown();
            this.nUDXyyX = new System.Windows.Forms.NumericUpDown();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.btChuckAlignN = new MRLibrary.UCButton();
            this.btCapture = new MRLibrary.UCButton();
            this.btRollBack = new MRLibrary.UCButton();
            this.btAlignTest = new MRLibrary.UCButton();
            this.skRPattern = new MRLibrary.SKZoomAndPanWindow();
            this.skLPattern = new MRLibrary.SKZoomAndPanWindow();
            this.btTfile = new System.Windows.Forms.Button();
            this.btTLRead = new System.Windows.Forms.Button();
            this.btTRRead = new System.Windows.Forms.Button();
            this.OpenFileDialogReadT = new System.Windows.Forms.OpenFileDialog();
            this.ucRightMaskNavigator = new NSAA_16Axis.UCNavigator();
            this.ucRightWaferNavigator = new NSAA_16Axis.UCNavigator();
            this.ucLeftWaferNavigator = new NSAA_16Axis.UCNavigator();
            this.ucLeftMaskNavigator = new NSAA_16Axis.UCNavigator();
            this.gbLMask.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLMaskImage)).BeginInit();
            this.gbLWafer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLWaferImage)).BeginInit();
            this.gbRWafer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbRWaferImage)).BeginInit();
            this.gbRMask.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbRMaskImage)).BeginInit();
            this.gBMag.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tBRightRingLight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tBLeftRingLight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tBLeftCoaLight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tBRightCoaLight)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nuDRotate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUDXyyY2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUDXyyY1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUDXyyX)).BeginInit();
            this.SuspendLayout();
            // 
            // gbLMask
            // 
            this.gbLMask.BackColor = System.Drawing.Color.AliceBlue;
            this.gbLMask.Controls.Add(this.btFindLMaskCenter);
            this.gbLMask.Controls.Add(this.ucLeftMaskNavigator);
            this.gbLMask.Controls.Add(this.btCreateLMaskPatternMask);
            this.gbLMask.Controls.Add(this.pbLMaskImage);
            this.gbLMask.Controls.Add(this.btLMaskLocationSave);
            this.gbLMask.Controls.Add(this.cbLMaskAlgorithm);
            this.gbLMask.Controls.Add(this.label2);
            this.gbLMask.Font = new System.Drawing.Font("Cambria", 15.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbLMask.ForeColor = System.Drawing.Color.Green;
            this.gbLMask.Location = new System.Drawing.Point(8, 3);
            this.gbLMask.Name = "gbLMask";
            this.gbLMask.Size = new System.Drawing.Size(181, 321);
            this.gbLMask.TabIndex = 8;
            this.gbLMask.TabStop = false;
            this.gbLMask.Text = "Mask";
            // 
            // btFindLMaskCenter
            // 
            this.btFindLMaskCenter.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.btFindLMaskCenter.Font = new System.Drawing.Font("Arial", 12F);
            this.btFindLMaskCenter.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btFindLMaskCenter.Location = new System.Drawing.Point(13, 143);
            this.btFindLMaskCenter.Name = "btFindLMaskCenter";
            this.btFindLMaskCenter.Size = new System.Drawing.Size(76, 30);
            this.btFindLMaskCenter.TabIndex = 70;
            this.btFindLMaskCenter.Text = "FindCenter";
            this.btFindLMaskCenter.UseVisualStyleBackColor = false;
            this.btFindLMaskCenter.Click += new System.EventHandler(this.BtFindLMaskCenter_Click);
            // 
            // btCreateLMaskPatternMask
            // 
            this.btCreateLMaskPatternMask.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.btCreateLMaskPatternMask.Font = new System.Drawing.Font("Arial", 12F);
            this.btCreateLMaskPatternMask.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btCreateLMaskPatternMask.Location = new System.Drawing.Point(89, 143);
            this.btCreateLMaskPatternMask.Name = "btCreateLMaskPatternMask";
            this.btCreateLMaskPatternMask.Size = new System.Drawing.Size(76, 30);
            this.btCreateLMaskPatternMask.TabIndex = 9;
            this.btCreateLMaskPatternMask.Text = "Create mask";
            this.btCreateLMaskPatternMask.UseVisualStyleBackColor = false;
            this.btCreateLMaskPatternMask.Click += new System.EventHandler(this.BtCreateLMaskPatternMask_Click);
            // 
            // pbLMaskImage
            // 
            this.pbLMaskImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbLMaskImage.Location = new System.Drawing.Point(13, 184);
            this.pbLMaskImage.Name = "pbLMaskImage";
            this.pbLMaskImage.Size = new System.Drawing.Size(153, 126);
            this.pbLMaskImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbLMaskImage.TabIndex = 7;
            this.pbLMaskImage.TabStop = false;
            // 
            // btLMaskLocationSave
            // 
            this.btLMaskLocationSave.BackColor = System.Drawing.Color.LightCoral;
            this.btLMaskLocationSave.Font = new System.Drawing.Font("Arial", 12F);
            this.btLMaskLocationSave.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btLMaskLocationSave.Location = new System.Drawing.Point(11, 107);
            this.btLMaskLocationSave.Name = "btLMaskLocationSave";
            this.btLMaskLocationSave.Size = new System.Drawing.Size(155, 30);
            this.btLMaskLocationSave.TabIndex = 6;
            this.btLMaskLocationSave.Text = "SAVE";
            this.btLMaskLocationSave.UseVisualStyleBackColor = false;
            this.btLMaskLocationSave.Click += new System.EventHandler(this.BtLMaskTemplateSave_Click);
            // 
            // cbLMaskAlgorithm
            // 
            this.cbLMaskAlgorithm.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.cbLMaskAlgorithm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLMaskAlgorithm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbLMaskAlgorithm.Font = new System.Drawing.Font("Arial", 12F);
            this.cbLMaskAlgorithm.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cbLMaskAlgorithm.FormattingEnabled = true;
            this.cbLMaskAlgorithm.Items.AddRange(new object[] {
            "Template",
            "Edge"});
            this.cbLMaskAlgorithm.Location = new System.Drawing.Point(66, 71);
            this.cbLMaskAlgorithm.Name = "cbLMaskAlgorithm";
            this.cbLMaskAlgorithm.Size = new System.Drawing.Size(100, 31);
            this.cbLMaskAlgorithm.TabIndex = 1;
            this.cbLMaskAlgorithm.SelectedIndexChanged += new System.EventHandler(this.CbLMaskAlgorithm_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(10, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 23);
            this.label2.TabIndex = 0;
            this.label2.Text = "ALGO.";
            // 
            // gbLWafer
            // 
            this.gbLWafer.BackColor = System.Drawing.Color.LightYellow;
            this.gbLWafer.Controls.Add(this.btLabelPatternL);
            this.gbLWafer.Controls.Add(this.cbLWaferClassList);
            this.gbLWafer.Controls.Add(this.btFindLWaferCenter);
            this.gbLWafer.Controls.Add(this.ucLeftWaferNavigator);
            this.gbLWafer.Controls.Add(this.btCreateLWaferPatternMask);
            this.gbLWafer.Controls.Add(this.pbLWaferImage);
            this.gbLWafer.Controls.Add(this.btLWaferLoactionSave);
            this.gbLWafer.Controls.Add(this.cbLWaferAlgorithm);
            this.gbLWafer.Controls.Add(this.label1);
            this.gbLWafer.Font = new System.Drawing.Font("Cambria", 15.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbLWafer.ForeColor = System.Drawing.Color.Red;
            this.gbLWafer.Location = new System.Drawing.Point(195, 3);
            this.gbLWafer.Name = "gbLWafer";
            this.gbLWafer.Size = new System.Drawing.Size(181, 321);
            this.gbLWafer.TabIndex = 9;
            this.gbLWafer.TabStop = false;
            this.gbLWafer.Text = "Wafer";
            // 
            // btLabelPatternL
            // 
            this.btLabelPatternL.AutoSize = true;
            this.btLabelPatternL.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.btLabelPatternL.Font = new System.Drawing.Font("Arial", 12F);
            this.btLabelPatternL.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btLabelPatternL.Location = new System.Drawing.Point(30, 248);
            this.btLabelPatternL.Name = "btLabelPatternL";
            this.btLabelPatternL.Size = new System.Drawing.Size(129, 33);
            this.btLabelPatternL.TabIndex = 76;
            this.btLabelPatternL.Text = "Label Image";
            this.btLabelPatternL.UseVisualStyleBackColor = false;
            this.btLabelPatternL.Click += new System.EventHandler(this.btLabelPatternL_Click);
            // 
            // cbLWaferClassList
            // 
            this.cbLWaferClassList.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbLWaferClassList.FormattingEnabled = true;
            this.cbLWaferClassList.Items.AddRange(new object[] {
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
            this.cbLWaferClassList.Location = new System.Drawing.Point(13, 205);
            this.cbLWaferClassList.Name = "cbLWaferClassList";
            this.cbLWaferClassList.Size = new System.Drawing.Size(153, 31);
            this.cbLWaferClassList.TabIndex = 75;
            this.cbLWaferClassList.Visible = false;
            // 
            // btFindLWaferCenter
            // 
            this.btFindLWaferCenter.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.btFindLWaferCenter.Font = new System.Drawing.Font("Arial", 12F);
            this.btFindLWaferCenter.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btFindLWaferCenter.Location = new System.Drawing.Point(6, 143);
            this.btFindLWaferCenter.Name = "btFindLWaferCenter";
            this.btFindLWaferCenter.Size = new System.Drawing.Size(76, 30);
            this.btFindLWaferCenter.TabIndex = 84;
            this.btFindLWaferCenter.Text = "FindCenter";
            this.btFindLWaferCenter.UseVisualStyleBackColor = false;
            this.btFindLWaferCenter.Click += new System.EventHandler(this.BtFindLWaferCenter_Click);
            // 
            // btCreateLWaferPatternMask
            // 
            this.btCreateLWaferPatternMask.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.btCreateLWaferPatternMask.Font = new System.Drawing.Font("Arial", 12F);
            this.btCreateLWaferPatternMask.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btCreateLWaferPatternMask.Location = new System.Drawing.Point(88, 143);
            this.btCreateLWaferPatternMask.Name = "btCreateLWaferPatternMask";
            this.btCreateLWaferPatternMask.Size = new System.Drawing.Size(76, 30);
            this.btCreateLWaferPatternMask.TabIndex = 8;
            this.btCreateLWaferPatternMask.Text = "Create mask";
            this.btCreateLWaferPatternMask.UseVisualStyleBackColor = false;
            this.btCreateLWaferPatternMask.Click += new System.EventHandler(this.BtCreateLWaferPatternMask_Click);
            // 
            // pbLWaferImage
            // 
            this.pbLWaferImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbLWaferImage.Location = new System.Drawing.Point(13, 184);
            this.pbLWaferImage.Name = "pbLWaferImage";
            this.pbLWaferImage.Size = new System.Drawing.Size(153, 126);
            this.pbLWaferImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbLWaferImage.TabIndex = 7;
            this.pbLWaferImage.TabStop = false;
            // 
            // btLWaferLoactionSave
            // 
            this.btLWaferLoactionSave.BackColor = System.Drawing.Color.LightCoral;
            this.btLWaferLoactionSave.Font = new System.Drawing.Font("Arial", 12F);
            this.btLWaferLoactionSave.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btLWaferLoactionSave.Location = new System.Drawing.Point(11, 107);
            this.btLWaferLoactionSave.Name = "btLWaferLoactionSave";
            this.btLWaferLoactionSave.Size = new System.Drawing.Size(155, 30);
            this.btLWaferLoactionSave.TabIndex = 6;
            this.btLWaferLoactionSave.Text = "SAVE";
            this.btLWaferLoactionSave.UseVisualStyleBackColor = false;
            this.btLWaferLoactionSave.Click += new System.EventHandler(this.BtLWaferTemplateSave_Click);
            // 
            // cbLWaferAlgorithm
            // 
            this.cbLWaferAlgorithm.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.cbLWaferAlgorithm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLWaferAlgorithm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbLWaferAlgorithm.Font = new System.Drawing.Font("Arial", 12F);
            this.cbLWaferAlgorithm.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cbLWaferAlgorithm.FormattingEnabled = true;
            this.cbLWaferAlgorithm.Items.AddRange(new object[] {
            "Template",
            "Edge",
            "AI Identify"});
            this.cbLWaferAlgorithm.Location = new System.Drawing.Point(66, 76);
            this.cbLWaferAlgorithm.Name = "cbLWaferAlgorithm";
            this.cbLWaferAlgorithm.Size = new System.Drawing.Size(100, 31);
            this.cbLWaferAlgorithm.TabIndex = 1;
            this.cbLWaferAlgorithm.SelectedIndexChanged += new System.EventHandler(this.CbLWaferAlgorithm_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(10, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "ALGO.";
            // 
            // gbRWafer
            // 
            this.gbRWafer.BackColor = System.Drawing.Color.LightYellow;
            this.gbRWafer.Controls.Add(this.btLabelPatternR);
            this.gbRWafer.Controls.Add(this.cbRWaferClassList);
            this.gbRWafer.Controls.Add(this.BtLeftWaferFocus);
            this.gbRWafer.Controls.Add(this.btFindRWaferCenter);
            this.gbRWafer.Controls.Add(this.ucRightWaferNavigator);
            this.gbRWafer.Controls.Add(this.btCreateRWaferPatternMask);
            this.gbRWafer.Controls.Add(this.pbRWaferImage);
            this.gbRWafer.Controls.Add(this.btRWaferLocationSave);
            this.gbRWafer.Controls.Add(this.cbRWaferAlgorithm);
            this.gbRWafer.Controls.Add(this.label3);
            this.gbRWafer.Font = new System.Drawing.Font("Cambria", 15.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbRWafer.ForeColor = System.Drawing.Color.Red;
            this.gbRWafer.Location = new System.Drawing.Point(1536, 3);
            this.gbRWafer.Name = "gbRWafer";
            this.gbRWafer.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.gbRWafer.Size = new System.Drawing.Size(181, 321);
            this.gbRWafer.TabIndex = 9;
            this.gbRWafer.TabStop = false;
            this.gbRWafer.Text = "Wafer";
            // 
            // btLabelPatternR
            // 
            this.btLabelPatternR.AutoSize = true;
            this.btLabelPatternR.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.btLabelPatternR.Font = new System.Drawing.Font("Arial", 12F);
            this.btLabelPatternR.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btLabelPatternR.Location = new System.Drawing.Point(26, 248);
            this.btLabelPatternR.Name = "btLabelPatternR";
            this.btLabelPatternR.Size = new System.Drawing.Size(129, 33);
            this.btLabelPatternR.TabIndex = 75;
            this.btLabelPatternR.Text = "Label Image";
            this.btLabelPatternR.UseVisualStyleBackColor = false;
            this.btLabelPatternR.Click += new System.EventHandler(this.btLabelPatternR_Click);
            // 
            // cbRWaferClassList
            // 
            this.cbRWaferClassList.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbRWaferClassList.FormattingEnabled = true;
            this.cbRWaferClassList.Items.AddRange(new object[] {
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
            this.cbRWaferClassList.Location = new System.Drawing.Point(13, 205);
            this.cbRWaferClassList.Name = "cbRWaferClassList";
            this.cbRWaferClassList.Size = new System.Drawing.Size(153, 31);
            this.cbRWaferClassList.TabIndex = 74;
            this.cbRWaferClassList.Visible = false;
            // 
            // BtLeftWaferFocus
            // 
            this.BtLeftWaferFocus.BackColor = System.Drawing.Color.LightCoral;
            this.BtLeftWaferFocus.Enabled = false;
            this.BtLeftWaferFocus.Font = new System.Drawing.Font("Arial", 12F);
            this.BtLeftWaferFocus.ForeColor = System.Drawing.SystemColors.ControlText;
            this.BtLeftWaferFocus.Location = new System.Drawing.Point(90, 107);
            this.BtLeftWaferFocus.Name = "BtLeftWaferFocus";
            this.BtLeftWaferFocus.Size = new System.Drawing.Size(76, 30);
            this.BtLeftWaferFocus.TabIndex = 85;
            this.BtLeftWaferFocus.Text = "Focus";
            this.BtLeftWaferFocus.UseVisualStyleBackColor = false;
            this.BtLeftWaferFocus.Visible = false;
            this.BtLeftWaferFocus.Click += new System.EventHandler(this.BtLeftWaferFocus_Click);
            // 
            // btFindRWaferCenter
            // 
            this.btFindRWaferCenter.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.btFindRWaferCenter.Font = new System.Drawing.Font("Arial", 12F);
            this.btFindRWaferCenter.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btFindRWaferCenter.Location = new System.Drawing.Point(13, 143);
            this.btFindRWaferCenter.Name = "btFindRWaferCenter";
            this.btFindRWaferCenter.Size = new System.Drawing.Size(76, 30);
            this.btFindRWaferCenter.TabIndex = 84;
            this.btFindRWaferCenter.Text = "FindCenter";
            this.btFindRWaferCenter.UseVisualStyleBackColor = false;
            this.btFindRWaferCenter.Click += new System.EventHandler(this.BtFindRWaferCenter_Click);
            // 
            // btCreateRWaferPatternMask
            // 
            this.btCreateRWaferPatternMask.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.btCreateRWaferPatternMask.Font = new System.Drawing.Font("Arial", 12F);
            this.btCreateRWaferPatternMask.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btCreateRWaferPatternMask.Location = new System.Drawing.Point(89, 143);
            this.btCreateRWaferPatternMask.Name = "btCreateRWaferPatternMask";
            this.btCreateRWaferPatternMask.Size = new System.Drawing.Size(76, 30);
            this.btCreateRWaferPatternMask.TabIndex = 10;
            this.btCreateRWaferPatternMask.Text = "Create mask";
            this.btCreateRWaferPatternMask.UseVisualStyleBackColor = false;
            this.btCreateRWaferPatternMask.Click += new System.EventHandler(this.BtCreateRWaferPatternMask_Click);
            // 
            // pbRWaferImage
            // 
            this.pbRWaferImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbRWaferImage.Location = new System.Drawing.Point(13, 184);
            this.pbRWaferImage.Name = "pbRWaferImage";
            this.pbRWaferImage.Size = new System.Drawing.Size(153, 126);
            this.pbRWaferImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbRWaferImage.TabIndex = 7;
            this.pbRWaferImage.TabStop = false;
            // 
            // btRWaferLocationSave
            // 
            this.btRWaferLocationSave.BackColor = System.Drawing.Color.LightCoral;
            this.btRWaferLocationSave.Font = new System.Drawing.Font("Arial", 12F);
            this.btRWaferLocationSave.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btRWaferLocationSave.Location = new System.Drawing.Point(13, 107);
            this.btRWaferLocationSave.Name = "btRWaferLocationSave";
            this.btRWaferLocationSave.Size = new System.Drawing.Size(155, 30);
            this.btRWaferLocationSave.TabIndex = 6;
            this.btRWaferLocationSave.Text = "SAVE";
            this.btRWaferLocationSave.UseVisualStyleBackColor = false;
            this.btRWaferLocationSave.Click += new System.EventHandler(this.BtRWaferTempalteSave_Click);
            // 
            // cbRWaferAlgorithm
            // 
            this.cbRWaferAlgorithm.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.cbRWaferAlgorithm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRWaferAlgorithm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbRWaferAlgorithm.Font = new System.Drawing.Font("Arial", 12F);
            this.cbRWaferAlgorithm.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cbRWaferAlgorithm.FormattingEnabled = true;
            this.cbRWaferAlgorithm.Items.AddRange(new object[] {
            "Template",
            "Edge",
            "AI Identify"});
            this.cbRWaferAlgorithm.Location = new System.Drawing.Point(68, 75);
            this.cbRWaferAlgorithm.Name = "cbRWaferAlgorithm";
            this.cbRWaferAlgorithm.Size = new System.Drawing.Size(100, 31);
            this.cbRWaferAlgorithm.TabIndex = 1;
            this.cbRWaferAlgorithm.SelectedIndexChanged += new System.EventHandler(this.CbRWaferAlgorithm_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(12, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 23);
            this.label3.TabIndex = 0;
            this.label3.Text = "ALGO.";
            // 
            // gbRMask
            // 
            this.gbRMask.BackColor = System.Drawing.Color.AliceBlue;
            this.gbRMask.Controls.Add(this.btFindRMaskCenter);
            this.gbRMask.Controls.Add(this.ucRightMaskNavigator);
            this.gbRMask.Controls.Add(this.btCreateRMaskPatternMask);
            this.gbRMask.Controls.Add(this.pbRMaskImage);
            this.gbRMask.Controls.Add(this.btRMaskLocationSave);
            this.gbRMask.Controls.Add(this.cbRMaskAlgorithm);
            this.gbRMask.Controls.Add(this.label4);
            this.gbRMask.Font = new System.Drawing.Font("Cambria", 15.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbRMask.ForeColor = System.Drawing.Color.Green;
            this.gbRMask.Location = new System.Drawing.Point(1723, 3);
            this.gbRMask.Name = "gbRMask";
            this.gbRMask.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.gbRMask.Size = new System.Drawing.Size(181, 321);
            this.gbRMask.TabIndex = 10;
            this.gbRMask.TabStop = false;
            this.gbRMask.Text = "Mask";
            // 
            // btFindRMaskCenter
            // 
            this.btFindRMaskCenter.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.btFindRMaskCenter.Font = new System.Drawing.Font("Arial", 12F);
            this.btFindRMaskCenter.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btFindRMaskCenter.Location = new System.Drawing.Point(13, 143);
            this.btFindRMaskCenter.Name = "btFindRMaskCenter";
            this.btFindRMaskCenter.Size = new System.Drawing.Size(76, 30);
            this.btFindRMaskCenter.TabIndex = 84;
            this.btFindRMaskCenter.Text = "FindCenter";
            this.btFindRMaskCenter.UseVisualStyleBackColor = false;
            this.btFindRMaskCenter.Click += new System.EventHandler(this.BtFindRMaskCenter_Click);
            // 
            // btCreateRMaskPatternMask
            // 
            this.btCreateRMaskPatternMask.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.btCreateRMaskPatternMask.Font = new System.Drawing.Font("Arial", 12F);
            this.btCreateRMaskPatternMask.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btCreateRMaskPatternMask.Location = new System.Drawing.Point(89, 143);
            this.btCreateRMaskPatternMask.Name = "btCreateRMaskPatternMask";
            this.btCreateRMaskPatternMask.Size = new System.Drawing.Size(76, 30);
            this.btCreateRMaskPatternMask.TabIndex = 11;
            this.btCreateRMaskPatternMask.Text = "Create mask";
            this.btCreateRMaskPatternMask.UseVisualStyleBackColor = false;
            this.btCreateRMaskPatternMask.Click += new System.EventHandler(this.BtCreateRMaskPatternMask_Click);
            // 
            // pbRMaskImage
            // 
            this.pbRMaskImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbRMaskImage.Location = new System.Drawing.Point(13, 184);
            this.pbRMaskImage.Name = "pbRMaskImage";
            this.pbRMaskImage.Size = new System.Drawing.Size(153, 126);
            this.pbRMaskImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbRMaskImage.TabIndex = 7;
            this.pbRMaskImage.TabStop = false;
            // 
            // btRMaskLocationSave
            // 
            this.btRMaskLocationSave.BackColor = System.Drawing.Color.LightCoral;
            this.btRMaskLocationSave.Font = new System.Drawing.Font("Arial", 12F);
            this.btRMaskLocationSave.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btRMaskLocationSave.Location = new System.Drawing.Point(13, 107);
            this.btRMaskLocationSave.Name = "btRMaskLocationSave";
            this.btRMaskLocationSave.Size = new System.Drawing.Size(155, 30);
            this.btRMaskLocationSave.TabIndex = 6;
            this.btRMaskLocationSave.Text = "SAVE";
            this.btRMaskLocationSave.UseVisualStyleBackColor = false;
            this.btRMaskLocationSave.Click += new System.EventHandler(this.BtRMaskTemplateSave_Click);
            // 
            // cbRMaskAlgorithm
            // 
            this.cbRMaskAlgorithm.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.cbRMaskAlgorithm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRMaskAlgorithm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbRMaskAlgorithm.Font = new System.Drawing.Font("Arial", 12F);
            this.cbRMaskAlgorithm.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cbRMaskAlgorithm.FormattingEnabled = true;
            this.cbRMaskAlgorithm.Items.AddRange(new object[] {
            "Template",
            "Edge"});
            this.cbRMaskAlgorithm.Location = new System.Drawing.Point(66, 75);
            this.cbRMaskAlgorithm.Name = "cbRMaskAlgorithm";
            this.cbRMaskAlgorithm.Size = new System.Drawing.Size(100, 31);
            this.cbRMaskAlgorithm.TabIndex = 1;
            this.cbRMaskAlgorithm.SelectedIndexChanged += new System.EventHandler(this.CbRMaskAlgorithm_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Location = new System.Drawing.Point(10, 78);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 23);
            this.label4.TabIndex = 0;
            this.label4.Text = "ALGO.";
            // 
            // lbRecipeNumber
            // 
            this.lbRecipeNumber.AutoSize = true;
            this.lbRecipeNumber.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbRecipeNumber.Location = new System.Drawing.Point(812, 68);
            this.lbRecipeNumber.Name = "lbRecipeNumber";
            this.lbRecipeNumber.Size = new System.Drawing.Size(347, 46);
            this.lbRecipeNumber.TabIndex = 11;
            this.lbRecipeNumber.Text = "Recipe Number : ";
            // 
            // gBMag
            // 
            this.gBMag.Controls.Add(this.btMagSaveHigh);
            this.gBMag.Controls.Add(this.btMagSaveLow);
            this.gBMag.Controls.Add(this.rBHighMagnification);
            this.gBMag.Controls.Add(this.rBLowMagnification);
            this.gBMag.Controls.Add(this.cbHighMagnification);
            this.gBMag.Controls.Add(this.cbLowMagnification);
            this.gBMag.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gBMag.Location = new System.Drawing.Point(819, 132);
            this.gBMag.Name = "gBMag";
            this.gBMag.Size = new System.Drawing.Size(269, 128);
            this.gBMag.TabIndex = 12;
            this.gBMag.TabStop = false;
            this.gBMag.Text = "Magnification";
            // 
            // btMagSaveHigh
            // 
            this.btMagSaveHigh.BackColor = System.Drawing.Color.MintCream;
            this.btMagSaveHigh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btMagSaveHigh.Font = new System.Drawing.Font("Arial", 12F);
            this.btMagSaveHigh.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btMagSaveHigh.Location = new System.Drawing.Point(171, 82);
            this.btMagSaveHigh.Name = "btMagSaveHigh";
            this.btMagSaveHigh.Size = new System.Drawing.Size(80, 30);
            this.btMagSaveHigh.TabIndex = 27;
            this.btMagSaveHigh.Text = "SAVE";
            this.btMagSaveHigh.UseVisualStyleBackColor = false;
            this.btMagSaveHigh.Click += new System.EventHandler(this.BtMagSave_Click);
            // 
            // btMagSaveLow
            // 
            this.btMagSaveLow.BackColor = System.Drawing.Color.MintCream;
            this.btMagSaveLow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btMagSaveLow.Font = new System.Drawing.Font("Arial", 12F);
            this.btMagSaveLow.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btMagSaveLow.Location = new System.Drawing.Point(171, 32);
            this.btMagSaveLow.Name = "btMagSaveLow";
            this.btMagSaveLow.Size = new System.Drawing.Size(80, 30);
            this.btMagSaveLow.TabIndex = 26;
            this.btMagSaveLow.Text = "SAVE";
            this.btMagSaveLow.UseVisualStyleBackColor = false;
            this.btMagSaveLow.Click += new System.EventHandler(this.BtMagSave_Click);
            // 
            // rBHighMagnification
            // 
            this.rBHighMagnification.AutoSize = true;
            this.rBHighMagnification.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rBHighMagnification.Location = new System.Drawing.Point(25, 86);
            this.rBHighMagnification.Name = "rBHighMagnification";
            this.rBHighMagnification.Size = new System.Drawing.Size(69, 27);
            this.rBHighMagnification.TabIndex = 21;
            this.rBHighMagnification.Text = "High";
            this.rBHighMagnification.UseVisualStyleBackColor = true;
            this.rBHighMagnification.Click += new System.EventHandler(this.RBHighMagnification_Click);
            // 
            // rBLowMagnification
            // 
            this.rBLowMagnification.AutoSize = true;
            this.rBLowMagnification.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rBLowMagnification.Location = new System.Drawing.Point(25, 36);
            this.rBLowMagnification.Name = "rBLowMagnification";
            this.rBLowMagnification.Size = new System.Drawing.Size(68, 27);
            this.rBLowMagnification.TabIndex = 20;
            this.rBLowMagnification.Text = "Low";
            this.rBLowMagnification.UseVisualStyleBackColor = true;
            this.rBLowMagnification.Click += new System.EventHandler(this.RBLowMagnification_Click);
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
            this.cbHighMagnification.Location = new System.Drawing.Point(95, 84);
            this.cbHighMagnification.Name = "cbHighMagnification";
            this.cbHighMagnification.Size = new System.Drawing.Size(60, 31);
            this.cbHighMagnification.TabIndex = 17;
            this.cbHighMagnification.SelectedIndexChanged += new System.EventHandler(this.CbHighMagnification_SelectedIndexChanged);
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
            this.cbLowMagnification.Location = new System.Drawing.Point(95, 34);
            this.cbLowMagnification.Name = "cbLowMagnification";
            this.cbLowMagnification.Size = new System.Drawing.Size(60, 31);
            this.cbLowMagnification.TabIndex = 16;
            this.cbLowMagnification.SelectedIndexChanged += new System.EventHandler(this.CbLowMagnification_SelectedIndexChanged);
            // 
            // cbPatterhShift
            // 
            this.cbPatterhShift.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPatterhShift.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbPatterhShift.FormattingEnabled = true;
            this.cbPatterhShift.Items.AddRange(new object[] {
            "Up",
            "Down",
            "Left",
            "Right",
            "Center"});
            this.cbPatterhShift.Location = new System.Drawing.Point(1097, 215);
            this.cbPatterhShift.Name = "cbPatterhShift";
            this.cbPatterhShift.Size = new System.Drawing.Size(80, 31);
            this.cbPatterhShift.TabIndex = 25;
            this.cbPatterhShift.SelectedIndexChanged += new System.EventHandler(this.CbPatterhShift_SelectedIndexChanged);
            // 
            // tBRightRingLight
            // 
            this.tBRightRingLight.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tBRightRingLight.LargeChange = 1;
            this.tBRightRingLight.Location = new System.Drawing.Point(1473, 82);
            this.tBRightRingLight.Maximum = 255;
            this.tBRightRingLight.Name = "tBRightRingLight";
            this.tBRightRingLight.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tBRightRingLight.Size = new System.Drawing.Size(56, 161);
            this.tBRightRingLight.TabIndex = 50;
            this.tBRightRingLight.Visible = false;
            this.tBRightRingLight.ValueChanged += new System.EventHandler(this.TBRightRingLight_ValueChanged);
            // 
            // tBLeftRingLight
            // 
            this.tBLeftRingLight.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tBLeftRingLight.LargeChange = 1;
            this.tBLeftRingLight.Location = new System.Drawing.Point(408, 87);
            this.tBLeftRingLight.Maximum = 255;
            this.tBLeftRingLight.Name = "tBLeftRingLight";
            this.tBLeftRingLight.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tBLeftRingLight.Size = new System.Drawing.Size(56, 156);
            this.tBLeftRingLight.TabIndex = 51;
            this.tBLeftRingLight.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.tBLeftRingLight.Visible = false;
            this.tBLeftRingLight.ValueChanged += new System.EventHandler(this.TBLeftRingLight_ValueChanged);
            // 
            // tBLeftCoaLight
            // 
            this.tBLeftCoaLight.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tBLeftCoaLight.LargeChange = 1;
            this.tBLeftCoaLight.Location = new System.Drawing.Point(512, 89);
            this.tBLeftCoaLight.Maximum = 255;
            this.tBLeftCoaLight.Name = "tBLeftCoaLight";
            this.tBLeftCoaLight.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tBLeftCoaLight.Size = new System.Drawing.Size(56, 154);
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
            this.tBRightCoaLight.Location = new System.Drawing.Point(1364, 82);
            this.tBRightCoaLight.Maximum = 255;
            this.tBRightCoaLight.Name = "tBRightCoaLight";
            this.tBRightCoaLight.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tBRightCoaLight.Size = new System.Drawing.Size(56, 161);
            this.tBRightCoaLight.TabIndex = 56;
            this.tBRightCoaLight.ValueChanged += new System.EventHandler(this.TBRightCoaLight_ValueChanged);
            // 
            // LRingLight
            // 
            this.LRingLight.Appearance = System.Windows.Forms.Appearance.Button;
            this.LRingLight.AutoSize = true;
            this.LRingLight.Checked = true;
            this.LRingLight.CheckState = System.Windows.Forms.CheckState.Checked;
            this.LRingLight.ForeColor = System.Drawing.Color.Green;
            this.LRingLight.Location = new System.Drawing.Point(392, 20);
            this.LRingLight.Name = "LRingLight";
            this.LRingLight.Size = new System.Drawing.Size(101, 33);
            this.LRingLight.TabIndex = 57;
            this.LRingLight.Text = "RingLight";
            this.LRingLight.UseVisualStyleBackColor = true;
            this.LRingLight.Visible = false;
            this.LRingLight.CheckedChanged += new System.EventHandler(this.LRingLight_CheckedChanged);
            // 
            // LCoaLight
            // 
            this.LCoaLight.Appearance = System.Windows.Forms.Appearance.Button;
            this.LCoaLight.AutoSize = true;
            this.LCoaLight.Checked = true;
            this.LCoaLight.CheckState = System.Windows.Forms.CheckState.Checked;
            this.LCoaLight.ForeColor = System.Drawing.Color.Red;
            this.LCoaLight.Location = new System.Drawing.Point(491, 20);
            this.LCoaLight.Name = "LCoaLight";
            this.LCoaLight.Size = new System.Drawing.Size(98, 33);
            this.LCoaLight.TabIndex = 59;
            this.LCoaLight.Text = "CoaLight";
            this.LCoaLight.UseVisualStyleBackColor = true;
            this.LCoaLight.CheckedChanged += new System.EventHandler(this.LCoaLight_CheckedChanged);
            // 
            // RCoaLight
            // 
            this.RCoaLight.Appearance = System.Windows.Forms.Appearance.Button;
            this.RCoaLight.AutoSize = true;
            this.RCoaLight.Checked = true;
            this.RCoaLight.CheckState = System.Windows.Forms.CheckState.Checked;
            this.RCoaLight.ForeColor = System.Drawing.Color.Red;
            this.RCoaLight.Location = new System.Drawing.Point(1341, 20);
            this.RCoaLight.Name = "RCoaLight";
            this.RCoaLight.Size = new System.Drawing.Size(98, 33);
            this.RCoaLight.TabIndex = 60;
            this.RCoaLight.Text = "CoaLight";
            this.RCoaLight.UseVisualStyleBackColor = true;
            this.RCoaLight.CheckedChanged += new System.EventHandler(this.RCoaLight_CheckedChanged);
            // 
            // RRingLight
            // 
            this.RRingLight.Appearance = System.Windows.Forms.Appearance.Button;
            this.RRingLight.AutoSize = true;
            this.RRingLight.Checked = true;
            this.RRingLight.CheckState = System.Windows.Forms.CheckState.Checked;
            this.RRingLight.ForeColor = System.Drawing.Color.Green;
            this.RRingLight.Location = new System.Drawing.Point(1446, 20);
            this.RRingLight.Name = "RRingLight";
            this.RRingLight.Size = new System.Drawing.Size(101, 33);
            this.RRingLight.TabIndex = 61;
            this.RRingLight.Text = "RingLight";
            this.RRingLight.UseVisualStyleBackColor = true;
            this.RRingLight.Visible = false;
            this.RRingLight.CheckedChanged += new System.EventHandler(this.RRingLight_CheckedChanged);
            // 
            // btLCoaLightPlus
            // 
            this.btLCoaLightPlus.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btLCoaLightPlus.Location = new System.Drawing.Point(512, 53);
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
            this.btLCoaLightMinus.Location = new System.Drawing.Point(512, 244);
            this.btLCoaLightMinus.Name = "btLCoaLightMinus";
            this.btLCoaLightMinus.Size = new System.Drawing.Size(38, 30);
            this.btLCoaLightMinus.TabIndex = 63;
            this.btLCoaLightMinus.Text = "-";
            this.btLCoaLightMinus.UseVisualStyleBackColor = true;
            this.btLCoaLightMinus.Click += new System.EventHandler(this.BtLCoaLightMinus_Click);
            // 
            // btLRingLightMinus
            // 
            this.btLRingLightMinus.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btLRingLightMinus.Location = new System.Drawing.Point(408, 243);
            this.btLRingLightMinus.Name = "btLRingLightMinus";
            this.btLRingLightMinus.Size = new System.Drawing.Size(38, 30);
            this.btLRingLightMinus.TabIndex = 65;
            this.btLRingLightMinus.Text = "-";
            this.btLRingLightMinus.UseVisualStyleBackColor = true;
            this.btLRingLightMinus.Visible = false;
            this.btLRingLightMinus.Click += new System.EventHandler(this.BtLRingLightMinus_Click);
            // 
            // btLRingLightPlus
            // 
            this.btLRingLightPlus.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btLRingLightPlus.Location = new System.Drawing.Point(408, 54);
            this.btLRingLightPlus.Name = "btLRingLightPlus";
            this.btLRingLightPlus.Size = new System.Drawing.Size(38, 30);
            this.btLRingLightPlus.TabIndex = 64;
            this.btLRingLightPlus.Text = "+";
            this.btLRingLightPlus.UseVisualStyleBackColor = true;
            this.btLRingLightPlus.Visible = false;
            this.btLRingLightPlus.Click += new System.EventHandler(this.BtLRingLightPlus_Click);
            // 
            // btRCoaLightMinus
            // 
            this.btRCoaLightMinus.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btRCoaLightMinus.Location = new System.Drawing.Point(1364, 243);
            this.btRCoaLightMinus.Name = "btRCoaLightMinus";
            this.btRCoaLightMinus.Size = new System.Drawing.Size(38, 30);
            this.btRCoaLightMinus.TabIndex = 67;
            this.btRCoaLightMinus.Text = "-";
            this.btRCoaLightMinus.UseVisualStyleBackColor = true;
            this.btRCoaLightMinus.Click += new System.EventHandler(this.BtRCoaLightMinus_Click);
            // 
            // btRCoaLightPlus
            // 
            this.btRCoaLightPlus.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btRCoaLightPlus.Location = new System.Drawing.Point(1364, 50);
            this.btRCoaLightPlus.Name = "btRCoaLightPlus";
            this.btRCoaLightPlus.Size = new System.Drawing.Size(38, 30);
            this.btRCoaLightPlus.TabIndex = 66;
            this.btRCoaLightPlus.Text = "+";
            this.btRCoaLightPlus.UseVisualStyleBackColor = true;
            this.btRCoaLightPlus.Click += new System.EventHandler(this.BtRCoaLightPlus_Click);
            // 
            // btRRingLightMinus
            // 
            this.btRRingLightMinus.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btRRingLightMinus.Location = new System.Drawing.Point(1473, 243);
            this.btRRingLightMinus.Name = "btRRingLightMinus";
            this.btRRingLightMinus.Size = new System.Drawing.Size(38, 30);
            this.btRRingLightMinus.TabIndex = 69;
            this.btRRingLightMinus.Text = "-";
            this.btRRingLightMinus.UseVisualStyleBackColor = true;
            this.btRRingLightMinus.Visible = false;
            this.btRRingLightMinus.Click += new System.EventHandler(this.BtRRingLightMinus_Click);
            // 
            // btRRingLightPlus
            // 
            this.btRRingLightPlus.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btRRingLightPlus.Location = new System.Drawing.Point(1473, 50);
            this.btRRingLightPlus.Name = "btRRingLightPlus";
            this.btRRingLightPlus.Size = new System.Drawing.Size(38, 30);
            this.btRRingLightPlus.TabIndex = 68;
            this.btRRingLightPlus.Text = "+";
            this.btRRingLightPlus.UseVisualStyleBackColor = true;
            this.btRRingLightPlus.Visible = false;
            this.btRRingLightPlus.Click += new System.EventHandler(this.BtRRingLightPlus_Click);
            // 
            // lbEmulationModeL
            // 
            this.lbEmulationModeL.AutoSize = true;
            this.lbEmulationModeL.Font = new System.Drawing.Font("標楷體", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbEmulationModeL.Location = new System.Drawing.Point(306, 672);
            this.lbEmulationModeL.Name = "lbEmulationModeL";
            this.lbEmulationModeL.Size = new System.Drawing.Size(327, 44);
            this.lbEmulationModeL.TabIndex = 71;
            this.lbEmulationModeL.Text = "Emulation Mode";
            // 
            // lbEmulationModeR
            // 
            this.lbEmulationModeR.AutoSize = true;
            this.lbEmulationModeR.Font = new System.Drawing.Font("標楷體", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbEmulationModeR.Location = new System.Drawing.Point(1313, 672);
            this.lbEmulationModeR.Name = "lbEmulationModeR";
            this.lbEmulationModeR.Size = new System.Drawing.Size(327, 44);
            this.lbEmulationModeR.TabIndex = 72;
            this.lbEmulationModeR.Text = "Emulation Mode";
            // 
            // btRRingSave
            // 
            this.btRRingSave.BackColor = System.Drawing.Color.LightCoral;
            this.btRRingSave.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btRRingSave.Location = new System.Drawing.Point(1465, 288);
            this.btRRingSave.Name = "btRRingSave";
            this.btRRingSave.Size = new System.Drawing.Size(60, 30);
            this.btRRingSave.TabIndex = 76;
            this.btRRingSave.Text = "Save";
            this.btRRingSave.UseVisualStyleBackColor = false;
            this.btRRingSave.Visible = false;
            this.btRRingSave.Click += new System.EventHandler(this.BtRRingSave_Click);
            // 
            // btRCoaSave
            // 
            this.btRCoaSave.BackColor = System.Drawing.Color.LightCoral;
            this.btRCoaSave.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btRCoaSave.Location = new System.Drawing.Point(1355, 288);
            this.btRCoaSave.Name = "btRCoaSave";
            this.btRCoaSave.Size = new System.Drawing.Size(60, 30);
            this.btRCoaSave.TabIndex = 75;
            this.btRCoaSave.Text = "Save";
            this.btRCoaSave.UseVisualStyleBackColor = false;
            this.btRCoaSave.Visible = false;
            this.btRCoaSave.Click += new System.EventHandler(this.BtRCoaSave_Click);
            // 
            // btLRingSave
            // 
            this.btLRingSave.BackColor = System.Drawing.Color.LightCoral;
            this.btLRingSave.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btLRingSave.Location = new System.Drawing.Point(397, 288);
            this.btLRingSave.Name = "btLRingSave";
            this.btLRingSave.Size = new System.Drawing.Size(60, 30);
            this.btLRingSave.TabIndex = 74;
            this.btLRingSave.Text = "Save";
            this.btLRingSave.UseVisualStyleBackColor = false;
            this.btLRingSave.Visible = false;
            this.btLRingSave.Click += new System.EventHandler(this.BtLRingSave_Click);
            // 
            // btLCoaSave
            // 
            this.btLCoaSave.BackColor = System.Drawing.Color.LightCoral;
            this.btLCoaSave.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btLCoaSave.Location = new System.Drawing.Point(502, 289);
            this.btLCoaSave.Name = "btLCoaSave";
            this.btLCoaSave.Size = new System.Drawing.Size(60, 30);
            this.btLCoaSave.TabIndex = 73;
            this.btLCoaSave.Text = "Save";
            this.btLCoaSave.UseVisualStyleBackColor = false;
            this.btLCoaSave.Visible = false;
            this.btLCoaSave.Click += new System.EventHandler(this.BtLCoaSave_Click);
            // 
            // lbNowTime
            // 
            this.lbNowTime.AutoSize = true;
            this.lbNowTime.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbNowTime.Location = new System.Drawing.Point(840, 30);
            this.lbNowTime.Name = "lbNowTime";
            this.lbNowTime.Size = new System.Drawing.Size(62, 29);
            this.lbNowTime.TabIndex = 77;
            this.lbNowTime.Text = "time";
            // 
            // lbDebugMsg
            // 
            this.lbDebugMsg.AutoSize = true;
            this.lbDebugMsg.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDebugMsg.Location = new System.Drawing.Point(382, 292);
            this.lbDebugMsg.Name = "lbDebugMsg";
            this.lbDebugMsg.Size = new System.Drawing.Size(62, 29);
            this.lbDebugMsg.TabIndex = 78;
            this.lbDebugMsg.Text = "time";
            this.lbDebugMsg.Visible = false;
            // 
            // btDelete
            // 
            this.btDelete.BackColor = System.Drawing.Color.LightCoral;
            this.btDelete.Font = new System.Drawing.Font("Arial", 12F);
            this.btDelete.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btDelete.Location = new System.Drawing.Point(990, 283);
            this.btDelete.Name = "btDelete";
            this.btDelete.Size = new System.Drawing.Size(80, 30);
            this.btDelete.TabIndex = 79;
            this.btDelete.Text = "Delete";
            this.btDelete.UseVisualStyleBackColor = false;
            this.btDelete.Click += new System.EventHandler(this.BtDelete_Click);
            // 
            // btDefault
            // 
            this.btDefault.BackColor = System.Drawing.Color.LightCoral;
            this.btDefault.Font = new System.Drawing.Font("Arial", 12F);
            this.btDefault.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btDefault.Location = new System.Drawing.Point(894, 283);
            this.btDefault.Name = "btDefault";
            this.btDefault.Size = new System.Drawing.Size(80, 30);
            this.btDefault.TabIndex = 80;
            this.btDefault.Text = "Default";
            this.btDefault.UseVisualStyleBackColor = false;
            this.btDefault.Click += new System.EventHandler(this.BtDefault_Click);
            // 
            // lbRMsgW
            // 
            this.lbRMsgW.AutoSize = true;
            this.lbRMsgW.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbRMsgW.Location = new System.Drawing.Point(986, 369);
            this.lbRMsgW.Name = "lbRMsgW";
            this.lbRMsgW.Size = new System.Drawing.Size(164, 25);
            this.lbRMsgW.TabIndex = 133;
            this.lbRMsgW.Text = "Debug Message";
            // 
            // lbLMsgW
            // 
            this.lbLMsgW.AutoSize = true;
            this.lbLMsgW.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbLMsgW.Location = new System.Drawing.Point(36, 369);
            this.lbLMsgW.Name = "lbLMsgW";
            this.lbLMsgW.Size = new System.Drawing.Size(164, 25);
            this.lbLMsgW.TabIndex = 132;
            this.lbLMsgW.Text = "Debug Message";
            // 
            // lbRMsgM
            // 
            this.lbRMsgM.AutoSize = true;
            this.lbRMsgM.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbRMsgM.Location = new System.Drawing.Point(986, 340);
            this.lbRMsgM.Name = "lbRMsgM";
            this.lbRMsgM.Size = new System.Drawing.Size(164, 25);
            this.lbRMsgM.TabIndex = 131;
            this.lbRMsgM.Text = "Debug Message";
            // 
            // lbLMsgM
            // 
            this.lbLMsgM.AutoSize = true;
            this.lbLMsgM.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbLMsgM.Location = new System.Drawing.Point(36, 340);
            this.lbLMsgM.Name = "lbLMsgM";
            this.lbLMsgM.Size = new System.Drawing.Size(164, 25);
            this.lbLMsgM.TabIndex = 130;
            this.lbLMsgM.Text = "Debug Message";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.nuDRotate);
            this.groupBox4.Controls.Add(this.btGo);
            this.groupBox4.Controls.Add(this.btUpdate);
            this.groupBox4.Controls.Add(this.nUDXyyY2);
            this.groupBox4.Controls.Add(this.nUDXyyY1);
            this.groupBox4.Controls.Add(this.nUDXyyX);
            this.groupBox4.Controls.Add(this.label17);
            this.groupBox4.Controls.Add(this.label16);
            this.groupBox4.Controls.Add(this.label15);
            this.groupBox4.Location = new System.Drawing.Point(1183, 79);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(169, 197);
            this.groupBox4.TabIndex = 141;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Now Table Location";
            this.groupBox4.Visible = false;
            // 
            // nuDRotate
            // 
            this.nuDRotate.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.nuDRotate.Enabled = false;
            this.nuDRotate.Location = new System.Drawing.Point(73, 118);
            this.nuDRotate.Maximum = new decimal(new int[] {
            2500000,
            0,
            0,
            0});
            this.nuDRotate.Minimum = new decimal(new int[] {
            2500000,
            0,
            0,
            -2147483648});
            this.nuDRotate.Name = "nuDRotate";
            this.nuDRotate.Size = new System.Drawing.Size(90, 30);
            this.nuDRotate.TabIndex = 143;
            this.nuDRotate.ValueChanged += new System.EventHandler(this.NuDRotate_ValueChanged);
            // 
            // btGo
            // 
            this.btGo.BackColor = System.Drawing.Color.Transparent;
            this.btGo.Caption = "Go";
            this.btGo.FaceColor = System.Drawing.Color.Lime;
            this.btGo.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btGo.Image = null;
            this.btGo.Location = new System.Drawing.Point(94, 154);
            this.btGo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btGo.Name = "btGo";
            this.btGo.Radius = 5;
            this.btGo.Size = new System.Drawing.Size(66, 30);
            this.btGo.TabIndex = 142;
            this.btGo.Click += new System.EventHandler(this.BtGo_Click);
            // 
            // btUpdate
            // 
            this.btUpdate.BackColor = System.Drawing.Color.Transparent;
            this.btUpdate.Caption = "Update";
            this.btUpdate.FaceColor = System.Drawing.Color.Khaki;
            this.btUpdate.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btUpdate.Image = null;
            this.btUpdate.Location = new System.Drawing.Point(6, 154);
            this.btUpdate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btUpdate.Name = "btUpdate";
            this.btUpdate.Radius = 5;
            this.btUpdate.Size = new System.Drawing.Size(74, 30);
            this.btUpdate.TabIndex = 141;
            this.btUpdate.Click += new System.EventHandler(this.BtUpdate_Click);
            // 
            // nUDXyyY2
            // 
            this.nUDXyyY2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.nUDXyyY2.Enabled = false;
            this.nUDXyyY2.Location = new System.Drawing.Point(73, 86);
            this.nUDXyyY2.Maximum = new decimal(new int[] {
            2500000,
            0,
            0,
            0});
            this.nUDXyyY2.Minimum = new decimal(new int[] {
            2500000,
            0,
            0,
            -2147483648});
            this.nUDXyyY2.Name = "nUDXyyY2";
            this.nUDXyyY2.Size = new System.Drawing.Size(90, 30);
            this.nUDXyyY2.TabIndex = 119;
            // 
            // nUDXyyY1
            // 
            this.nUDXyyY1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.nUDXyyY1.Enabled = false;
            this.nUDXyyY1.Location = new System.Drawing.Point(73, 55);
            this.nUDXyyY1.Maximum = new decimal(new int[] {
            2500000,
            0,
            0,
            0});
            this.nUDXyyY1.Minimum = new decimal(new int[] {
            2500000,
            0,
            0,
            -2147483648});
            this.nUDXyyY1.Name = "nUDXyyY1";
            this.nUDXyyY1.Size = new System.Drawing.Size(90, 30);
            this.nUDXyyY1.TabIndex = 118;
            // 
            // nUDXyyX
            // 
            this.nUDXyyX.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.nUDXyyX.Enabled = false;
            this.nUDXyyX.Location = new System.Drawing.Point(73, 24);
            this.nUDXyyX.Maximum = new decimal(new int[] {
            2500000,
            0,
            0,
            0});
            this.nUDXyyX.Minimum = new decimal(new int[] {
            2500000,
            0,
            0,
            -2147483648});
            this.nUDXyyX.Name = "nUDXyyX";
            this.nUDXyyX.Size = new System.Drawing.Size(90, 30);
            this.nUDXyyX.TabIndex = 117;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Enabled = false;
            this.label17.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label17.Location = new System.Drawing.Point(8, 59);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(74, 24);
            this.label17.TabIndex = 98;
            this.label17.Text = "XYY-Y1";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Enabled = false;
            this.label16.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label16.Location = new System.Drawing.Point(8, 90);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(74, 24);
            this.label16.TabIndex = 96;
            this.label16.Text = "XYY-Y2";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Enabled = false;
            this.label15.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label15.Location = new System.Drawing.Point(16, 28);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(64, 24);
            this.label15.TabIndex = 94;
            this.label15.Text = "XYY-X";
            // 
            // btChuckAlignN
            // 
            this.btChuckAlignN.BackColor = System.Drawing.Color.Transparent;
            this.btChuckAlignN.Caption = "ChuckAlign";
            this.btChuckAlignN.FaceColor = System.Drawing.Color.Khaki;
            this.btChuckAlignN.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btChuckAlignN.Image = null;
            this.btChuckAlignN.Location = new System.Drawing.Point(691, 127);
            this.btChuckAlignN.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btChuckAlignN.Name = "btChuckAlignN";
            this.btChuckAlignN.Radius = 5;
            this.btChuckAlignN.Size = new System.Drawing.Size(90, 30);
            this.btChuckAlignN.TabIndex = 140;
            this.btChuckAlignN.Visible = false;
            this.btChuckAlignN.Click += new System.EventHandler(this.BtChuckAlignN_Click);
            // 
            // btCapture
            // 
            this.btCapture.BackColor = System.Drawing.Color.Transparent;
            this.btCapture.Caption = "Capture";
            this.btCapture.FaceColor = System.Drawing.Color.Khaki;
            this.btCapture.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btCapture.Image = null;
            this.btCapture.Location = new System.Drawing.Point(691, 89);
            this.btCapture.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btCapture.Name = "btCapture";
            this.btCapture.Radius = 5;
            this.btCapture.Size = new System.Drawing.Size(90, 30);
            this.btCapture.TabIndex = 138;
            this.btCapture.Click += new System.EventHandler(this.BtCapture_Click);
            // 
            // btRollBack
            // 
            this.btRollBack.BackColor = System.Drawing.Color.Transparent;
            this.btRollBack.Caption = "RollBack";
            this.btRollBack.FaceColor = System.Drawing.Color.Khaki;
            this.btRollBack.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btRollBack.Image = null;
            this.btRollBack.Location = new System.Drawing.Point(691, 165);
            this.btRollBack.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btRollBack.Name = "btRollBack";
            this.btRollBack.Radius = 5;
            this.btRollBack.Size = new System.Drawing.Size(90, 30);
            this.btRollBack.TabIndex = 136;
            this.btRollBack.Visible = false;
            this.btRollBack.Click += new System.EventHandler(this.BtRollBack_Click);
            // 
            // btAlignTest
            // 
            this.btAlignTest.BackColor = System.Drawing.Color.Transparent;
            this.btAlignTest.Caption = "AlignTest";
            this.btAlignTest.FaceColor = System.Drawing.Color.Khaki;
            this.btAlignTest.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btAlignTest.Image = null;
            this.btAlignTest.Location = new System.Drawing.Point(691, 50);
            this.btAlignTest.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btAlignTest.Name = "btAlignTest";
            this.btAlignTest.Radius = 5;
            this.btAlignTest.Size = new System.Drawing.Size(90, 30);
            this.btAlignTest.TabIndex = 134;
            this.btAlignTest.Click += new System.EventHandler(this.BtAlignTest_ClickAsync);
            // 
            // skRPattern
            // 
            this.skRPattern.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.skRPattern.Location = new System.Drawing.Point(959, 325);
            this.skRPattern.Margin = new System.Windows.Forms.Padding(4);
            this.skRPattern.Name = "skRPattern";
            this.skRPattern.Size = new System.Drawing.Size(944, 708);
            this.skRPattern.TabIndex = 1;
            // 
            // skLPattern
            // 
            this.skLPattern.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.skLPattern.Location = new System.Drawing.Point(8, 325);
            this.skLPattern.Margin = new System.Windows.Forms.Padding(4);
            this.skLPattern.Name = "skLPattern";
            this.skLPattern.Size = new System.Drawing.Size(944, 708);
            this.skLPattern.TabIndex = 0;
            // 
            // btTfile
            // 
            this.btTfile.Location = new System.Drawing.Point(628, 202);
            this.btTfile.Name = "btTfile";
            this.btTfile.Size = new System.Drawing.Size(106, 37);
            this.btTfile.TabIndex = 142;
            this.btTfile.Text = "file";
            this.btTfile.UseVisualStyleBackColor = true;
            this.btTfile.Click += new System.EventHandler(this.btTfile_Click);
            // 
            // btTLRead
            // 
            this.btTLRead.Location = new System.Drawing.Point(628, 243);
            this.btTLRead.Name = "btTLRead";
            this.btTLRead.Size = new System.Drawing.Size(106, 33);
            this.btTLRead.TabIndex = 142;
            this.btTLRead.Text = "buttonL";
            this.btTLRead.UseVisualStyleBackColor = true;
            this.btTLRead.Click += new System.EventHandler(this.btTLRead_Click);
            // 
            // btTRRead
            // 
            this.btTRRead.Location = new System.Drawing.Point(628, 283);
            this.btTRRead.Name = "btTRRead";
            this.btTRRead.Size = new System.Drawing.Size(106, 36);
            this.btTRRead.TabIndex = 142;
            this.btTRRead.Text = "buttonR";
            this.btTRRead.UseVisualStyleBackColor = true;
            this.btTRRead.Click += new System.EventHandler(this.btTRRead_Click);
            // 
            // OpenFileDialogReadT
            // 
            this.OpenFileDialogReadT.FileName = "openFileDialogT1";
            // 
            // ucRightMaskNavigator
            // 
            this.ucRightMaskNavigator.AutoSize = true;
            this.ucRightMaskNavigator.Location = new System.Drawing.Point(36, 30);
            this.ucRightMaskNavigator.Name = "ucRightMaskNavigator";
            this.ucRightMaskNavigator.Size = new System.Drawing.Size(107, 40);
            this.ucRightMaskNavigator.TabIndex = 55;
            this.ucRightMaskNavigator.CommandPressed += new NSAA_16Axis.UCNavigator.CommandPressedEvent(this.UcRightMaskNavigator_CommandPressed);
            // 
            // ucRightWaferNavigator
            // 
            this.ucRightWaferNavigator.AutoSize = true;
            this.ucRightWaferNavigator.Location = new System.Drawing.Point(37, 31);
            this.ucRightWaferNavigator.Name = "ucRightWaferNavigator";
            this.ucRightWaferNavigator.Size = new System.Drawing.Size(107, 40);
            this.ucRightWaferNavigator.TabIndex = 55;
            this.ucRightWaferNavigator.CommandPressed += new NSAA_16Axis.UCNavigator.CommandPressedEvent(this.UcRightWaferNavigator_CommandPressed);
            // 
            // ucLeftWaferNavigator
            // 
            this.ucLeftWaferNavigator.AutoSize = true;
            this.ucLeftWaferNavigator.Location = new System.Drawing.Point(30, 30);
            this.ucLeftWaferNavigator.Name = "ucLeftWaferNavigator";
            this.ucLeftWaferNavigator.Size = new System.Drawing.Size(107, 40);
            this.ucLeftWaferNavigator.TabIndex = 54;
            this.ucLeftWaferNavigator.CommandPressed += new NSAA_16Axis.UCNavigator.CommandPressedEvent(this.UcLeftWaferNavigator_CommandPressed);
            // 
            // ucLeftMaskNavigator
            // 
            this.ucLeftMaskNavigator.AutoSize = true;
            this.ucLeftMaskNavigator.Location = new System.Drawing.Point(32, 30);
            this.ucLeftMaskNavigator.Name = "ucLeftMaskNavigator";
            this.ucLeftMaskNavigator.Size = new System.Drawing.Size(107, 40);
            this.ucLeftMaskNavigator.TabIndex = 55;
            this.ucLeftMaskNavigator.CommandPressed += new NSAA_16Axis.UCNavigator.CommandPressedEvent(this.UcLeftMaskNavigator_CommandPressed);
            // 
            // CMLearnPatternUp
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.btTRRead);
            this.Controls.Add(this.btTLRead);
            this.Controls.Add(this.btTfile);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.cbPatterhShift);
            this.Controls.Add(this.btChuckAlignN);
            this.Controls.Add(this.btCapture);
            this.Controls.Add(this.btRollBack);
            this.Controls.Add(this.btAlignTest);
            this.Controls.Add(this.lbRMsgW);
            this.Controls.Add(this.lbLMsgW);
            this.Controls.Add(this.lbRMsgM);
            this.Controls.Add(this.lbLMsgM);
            this.Controls.Add(this.btDefault);
            this.Controls.Add(this.btDelete);
            this.Controls.Add(this.lbDebugMsg);
            this.Controls.Add(this.lbNowTime);
            this.Controls.Add(this.btRRingSave);
            this.Controls.Add(this.btRCoaSave);
            this.Controls.Add(this.btLRingSave);
            this.Controls.Add(this.btLCoaSave);
            this.Controls.Add(this.lbEmulationModeR);
            this.Controls.Add(this.lbEmulationModeL);
            this.Controls.Add(this.btRRingLightMinus);
            this.Controls.Add(this.btRRingLightPlus);
            this.Controls.Add(this.btRCoaLightMinus);
            this.Controls.Add(this.btRCoaLightPlus);
            this.Controls.Add(this.btLRingLightMinus);
            this.Controls.Add(this.btLRingLightPlus);
            this.Controls.Add(this.btLCoaLightMinus);
            this.Controls.Add(this.btLCoaLightPlus);
            this.Controls.Add(this.RRingLight);
            this.Controls.Add(this.RCoaLight);
            this.Controls.Add(this.LCoaLight);
            this.Controls.Add(this.LRingLight);
            this.Controls.Add(this.tBRightCoaLight);
            this.Controls.Add(this.tBLeftCoaLight);
            this.Controls.Add(this.tBLeftRingLight);
            this.Controls.Add(this.tBRightRingLight);
            this.Controls.Add(this.gBMag);
            this.Controls.Add(this.lbRecipeNumber);
            this.Controls.Add(this.gbRMask);
            this.Controls.Add(this.gbRWafer);
            this.Controls.Add(this.gbLWafer);
            this.Controls.Add(this.gbLMask);
            this.Controls.Add(this.skRPattern);
            this.Controls.Add(this.skLPattern);
            this.Font = new System.Drawing.Font("Arial", 12F);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "CMLearnPatternUp";
            this.Size = new System.Drawing.Size(1912, 1080);
            this.Load += new System.EventHandler(this.CMLearnPattern_Load);
            this.gbLMask.ResumeLayout(false);
            this.gbLMask.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLMaskImage)).EndInit();
            this.gbLWafer.ResumeLayout(false);
            this.gbLWafer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbLWaferImage)).EndInit();
            this.gbRWafer.ResumeLayout(false);
            this.gbRWafer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbRWaferImage)).EndInit();
            this.gbRMask.ResumeLayout(false);
            this.gbRMask.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbRMaskImage)).EndInit();
            this.gBMag.ResumeLayout(false);
            this.gBMag.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tBRightRingLight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tBLeftRingLight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tBLeftCoaLight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tBRightCoaLight)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nuDRotate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUDXyyY2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUDXyyY1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUDXyyX)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MRLibrary.SKZoomAndPanWindow skLPattern;
        private MRLibrary.SKZoomAndPanWindow skRPattern;
        private System.Windows.Forms.GroupBox gbLMask;
        private System.Windows.Forms.PictureBox pbLMaskImage;
        private System.Windows.Forms.Button btLMaskLocationSave;
        private System.Windows.Forms.ComboBox cbLMaskAlgorithm;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox gbLWafer;
        private System.Windows.Forms.PictureBox pbLWaferImage;
        private System.Windows.Forms.Button btLWaferLoactionSave;
        private System.Windows.Forms.ComboBox cbLWaferAlgorithm;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbRWafer;
        private System.Windows.Forms.PictureBox pbRWaferImage;
        private System.Windows.Forms.Button btRWaferLocationSave;
        private System.Windows.Forms.ComboBox cbRWaferAlgorithm;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox gbRMask;
        private System.Windows.Forms.PictureBox pbRMaskImage;
        private System.Windows.Forms.Button btRMaskLocationSave;
        private System.Windows.Forms.ComboBox cbRMaskAlgorithm;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbRecipeNumber;
        private System.Windows.Forms.GroupBox gBMag;
        private System.Windows.Forms.Button btCreateLWaferPatternMask;
        private System.Windows.Forms.Button btCreateRWaferPatternMask;
        private System.Windows.Forms.Button btCreateRMaskPatternMask;
        private System.Windows.Forms.Button btCreateLMaskPatternMask;
        private System.Windows.Forms.ComboBox cbHighMagnification;
        private System.Windows.Forms.ComboBox cbLowMagnification;
        private System.Windows.Forms.TrackBar tBRightRingLight;
        private System.Windows.Forms.TrackBar tBLeftRingLight;
        private System.Windows.Forms.RadioButton rBHighMagnification;
        private System.Windows.Forms.RadioButton rBLowMagnification;
        private UCNavigator ucLeftMaskNavigator;
        private UCNavigator ucLeftWaferNavigator;
        private UCNavigator ucRightWaferNavigator;
        private UCNavigator ucRightMaskNavigator;
        private System.Windows.Forms.TrackBar tBLeftCoaLight;
        private System.Windows.Forms.TrackBar tBRightCoaLight;
        private System.Windows.Forms.CheckBox LRingLight;
        private System.Windows.Forms.CheckBox LCoaLight;
        private System.Windows.Forms.CheckBox RCoaLight;
        private System.Windows.Forms.CheckBox RRingLight;
        private System.Windows.Forms.Button btLCoaLightPlus;
        private System.Windows.Forms.Button btLCoaLightMinus;
        private System.Windows.Forms.Button btLRingLightMinus;
        private System.Windows.Forms.Button btLRingLightPlus;
        private System.Windows.Forms.Button btRCoaLightMinus;
        private System.Windows.Forms.Button btRCoaLightPlus;
        private System.Windows.Forms.Button btRRingLightMinus;
        private System.Windows.Forms.Button btRRingLightPlus;
        private System.Windows.Forms.Label lbEmulationModeL;
        private System.Windows.Forms.Label lbEmulationModeR;
        private System.Windows.Forms.ComboBox cbPatterhShift;
        private System.Windows.Forms.Button btMagSaveLow;
        private System.Windows.Forms.Button btRRingSave;
        private System.Windows.Forms.Button btRCoaSave;
        private System.Windows.Forms.Button btLRingSave;
        private System.Windows.Forms.Button btLCoaSave;
        private System.Windows.Forms.Label lbNowTime;
        private System.Windows.Forms.Label lbDebugMsg;
        private System.Windows.Forms.Button btDelete;
        private System.Windows.Forms.Button btDefault;
        private System.Windows.Forms.Label lbRMsgW;
        private System.Windows.Forms.Label lbLMsgW;
        private System.Windows.Forms.Label lbRMsgM;
        private System.Windows.Forms.Label lbLMsgM;
        private MRLibrary.UCButton btAlignTest;
        private MRLibrary.UCButton btRollBack;
        private MRLibrary.UCButton btCapture;
        private MRLibrary.UCButton btChuckAlignN;
        public System.Windows.Forms.GroupBox groupBox4;
        private MRLibrary.UCButton btGo;
        private MRLibrary.UCButton btUpdate;
        private System.Windows.Forms.NumericUpDown nUDXyyY2;
        private System.Windows.Forms.NumericUpDown nUDXyyY1;
        private System.Windows.Forms.NumericUpDown nUDXyyX;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.NumericUpDown nuDRotate;
        private System.Windows.Forms.Button btFindLMaskCenter;
        private System.Windows.Forms.Button btFindLWaferCenter;
        private System.Windows.Forms.Button btFindRWaferCenter;
        private System.Windows.Forms.Button btFindRMaskCenter;
        private System.Windows.Forms.Button BtLeftWaferFocus;
        private System.Windows.Forms.Button btMagSaveHigh;
        private System.Windows.Forms.ComboBox cbLWaferClassList;
        private System.Windows.Forms.Button btLabelPatternL;
        private System.Windows.Forms.Button btLabelPatternR;
        private System.Windows.Forms.ComboBox cbRWaferClassList;
        private System.Windows.Forms.Button btTfile;
        private System.Windows.Forms.Button btTLRead;
        private System.Windows.Forms.Button btTRRead;
        private System.Windows.Forms.OpenFileDialog OpenFileDialogReadT;
    }
}
