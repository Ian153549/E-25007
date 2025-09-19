namespace NSAA_16Axis
{
    partial class DialogTuneTable
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBoxLeftPattern = new System.Windows.Forms.PictureBox();
            this.pictureBoxRightPattern = new System.Windows.Forms.PictureBox();
            this.buttonSaveLEetPattern = new System.Windows.Forms.Button();
            this.SaveRightPattern = new System.Windows.Forms.Button();
            this.radioButtonTableX = new System.Windows.Forms.RadioButton();
            this.radioButtonTableY = new System.Windows.Forms.RadioButton();
            this.radioButtonTableTheta = new System.Windows.Forms.RadioButton();
            this.buttonXYYTableMove = new System.Windows.Forms.Button();
            this.comboBoxZoomLensGoto = new System.Windows.Forms.ComboBox();
            this.groupBoxZoomLens = new System.Windows.Forms.GroupBox();
            this.lbMovGoto = new System.Windows.Forms.Label();
            this.gbXYYTable = new System.Windows.Forms.GroupBox();
            this.checkBoxTableManual = new System.Windows.Forms.CheckBox();
            this.labelRightResult = new System.Windows.Forms.Label();
            this.lbTableStep = new System.Windows.Forms.Label();
            this.numericUpDownTableSteps = new System.Windows.Forms.NumericUpDown();
            this.XYYTableSave = new System.Windows.Forms.Button();
            this.labelLeftResult = new System.Windows.Forms.Label();
            this.gbCamera = new System.Windows.Forms.GroupBox();
            this.radioButtonCameraRightY = new System.Windows.Forms.RadioButton();
            this.checkBoxCameraManual = new System.Windows.Forms.CheckBox();
            this.labelRightCameraResult = new System.Windows.Forms.Label();
            this.buttonCameraSave = new System.Windows.Forms.Button();
            this.buttonCameraMove = new System.Windows.Forms.Button();
            this.labelLeftCameraResult = new System.Windows.Forms.Label();
            this.lbCameraStep = new System.Windows.Forms.Label();
            this.numericUpDownCameraSteps = new System.Windows.Forms.NumericUpDown();
            this.radioButtonCameraLeftY = new System.Windows.Forms.RadioButton();
            this.radioButtonrCameraRghtX = new System.Windows.Forms.RadioButton();
            this.radioButtonCameraLeftX = new System.Windows.Forms.RadioButton();
            this.numericUpDownPatternCenterDistance = new System.Windows.Forms.NumericUpDown();
            this.lbPettrnCenterDis = new System.Windows.Forms.Label();
            this.RightCameraWindow = new MRLibrary.SKZoomAndPanWindow();
            this.LeftCameraWindow = new MRLibrary.SKZoomAndPanWindow();
            this.lbRecord = new System.Windows.Forms.Label();
            this.radioButtonCamera = new System.Windows.Forms.RadioButton();
            this.radioButtonTable = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLeftPattern)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRightPattern)).BeginInit();
            this.groupBoxZoomLens.SuspendLayout();
            this.gbXYYTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTableSteps)).BeginInit();
            this.gbCamera.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCameraSteps)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPatternCenterDistance)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxLeftPattern
            // 
            this.pictureBoxLeftPattern.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxLeftPattern.Location = new System.Drawing.Point(12, 12);
            this.pictureBoxLeftPattern.Name = "pictureBoxLeftPattern";
            this.pictureBoxLeftPattern.Size = new System.Drawing.Size(150, 150);
            this.pictureBoxLeftPattern.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxLeftPattern.TabIndex = 2;
            this.pictureBoxLeftPattern.TabStop = false;
            // 
            // pictureBoxRightPattern
            // 
            this.pictureBoxRightPattern.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxRightPattern.Location = new System.Drawing.Point(206, 12);
            this.pictureBoxRightPattern.Name = "pictureBoxRightPattern";
            this.pictureBoxRightPattern.Size = new System.Drawing.Size(150, 150);
            this.pictureBoxRightPattern.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxRightPattern.TabIndex = 3;
            this.pictureBoxRightPattern.TabStop = false;
            // 
            // buttonSaveLEetPattern
            // 
            this.buttonSaveLEetPattern.Font = new System.Drawing.Font("Arial", 12F);
            this.buttonSaveLEetPattern.Location = new System.Drawing.Point(12, 179);
            this.buttonSaveLEetPattern.Name = "buttonSaveLEetPattern";
            this.buttonSaveLEetPattern.Size = new System.Drawing.Size(150, 31);
            this.buttonSaveLEetPattern.TabIndex = 4;
            this.buttonSaveLEetPattern.Text = "SAVE";
            this.buttonSaveLEetPattern.UseVisualStyleBackColor = true;
            this.buttonSaveLEetPattern.Click += new System.EventHandler(this.buttonSaveLEetPattern_Click);
            // 
            // SaveRightPattern
            // 
            this.SaveRightPattern.Font = new System.Drawing.Font("Arial", 12F);
            this.SaveRightPattern.Location = new System.Drawing.Point(206, 179);
            this.SaveRightPattern.Name = "SaveRightPattern";
            this.SaveRightPattern.Size = new System.Drawing.Size(150, 31);
            this.SaveRightPattern.TabIndex = 5;
            this.SaveRightPattern.Text = "SAVE";
            this.SaveRightPattern.UseVisualStyleBackColor = true;
            this.SaveRightPattern.Click += new System.EventHandler(this.SaveRightPattern_Click);
            // 
            // radioButtonTableX
            // 
            this.radioButtonTableX.AutoSize = true;
            this.radioButtonTableX.Font = new System.Drawing.Font("Arial", 12F);
            this.radioButtonTableX.Location = new System.Drawing.Point(15, 30);
            this.radioButtonTableX.Name = "radioButtonTableX";
            this.radioButtonTableX.Size = new System.Drawing.Size(37, 22);
            this.radioButtonTableX.TabIndex = 6;
            this.radioButtonTableX.TabStop = true;
            this.radioButtonTableX.Text = "X";
            this.radioButtonTableX.UseVisualStyleBackColor = true;
            // 
            // radioButtonTableY
            // 
            this.radioButtonTableY.AutoSize = true;
            this.radioButtonTableY.Font = new System.Drawing.Font("Arial", 12F);
            this.radioButtonTableY.Location = new System.Drawing.Point(58, 30);
            this.radioButtonTableY.Name = "radioButtonTableY";
            this.radioButtonTableY.Size = new System.Drawing.Size(35, 22);
            this.radioButtonTableY.TabIndex = 7;
            this.radioButtonTableY.TabStop = true;
            this.radioButtonTableY.Text = "Y";
            this.radioButtonTableY.UseVisualStyleBackColor = true;
            // 
            // radioButtonTableTheta
            // 
            this.radioButtonTableTheta.AutoSize = true;
            this.radioButtonTableTheta.Font = new System.Drawing.Font("Arial", 12F);
            this.radioButtonTableTheta.Location = new System.Drawing.Point(99, 30);
            this.radioButtonTableTheta.Name = "radioButtonTableTheta";
            this.radioButtonTableTheta.Size = new System.Drawing.Size(65, 22);
            this.radioButtonTableTheta.TabIndex = 8;
            this.radioButtonTableTheta.TabStop = true;
            this.radioButtonTableTheta.Text = "Theta";
            this.radioButtonTableTheta.UseVisualStyleBackColor = true;
            // 
            // buttonXYYTableMove
            // 
            this.buttonXYYTableMove.Font = new System.Drawing.Font("Arial", 12F);
            this.buttonXYYTableMove.Location = new System.Drawing.Point(328, 28);
            this.buttonXYYTableMove.Name = "buttonXYYTableMove";
            this.buttonXYYTableMove.Size = new System.Drawing.Size(75, 26);
            this.buttonXYYTableMove.TabIndex = 10;
            this.buttonXYYTableMove.Text = "Go";
            this.buttonXYYTableMove.UseVisualStyleBackColor = true;
            this.buttonXYYTableMove.Click += new System.EventHandler(this.buttonXYYTableMove_Click);
            // 
            // comboBoxZoomLensGoto
            // 
            this.comboBoxZoomLensGoto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxZoomLensGoto.Font = new System.Drawing.Font("Arial", 12F);
            this.comboBoxZoomLensGoto.FormattingEnabled = true;
            this.comboBoxZoomLensGoto.Items.AddRange(new object[] {
            "X1",
            "X2",
            "X3",
            "X4",
            "X5",
            "X6",
            "X7",
            "X8",
            "X9",
            "X10"});
            this.comboBoxZoomLensGoto.Location = new System.Drawing.Point(77, 34);
            this.comboBoxZoomLensGoto.Name = "comboBoxZoomLensGoto";
            this.comboBoxZoomLensGoto.Size = new System.Drawing.Size(60, 26);
            this.comboBoxZoomLensGoto.TabIndex = 11;
            this.comboBoxZoomLensGoto.SelectedIndexChanged += new System.EventHandler(this.comboBoxZoomLensGoto_SelectedIndexChanged);
            // 
            // groupBoxZoomLens
            // 
            this.groupBoxZoomLens.Controls.Add(this.lbMovGoto);
            this.groupBoxZoomLens.Controls.Add(this.comboBoxZoomLensGoto);
            this.groupBoxZoomLens.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxZoomLens.Location = new System.Drawing.Point(464, 12);
            this.groupBoxZoomLens.Name = "groupBoxZoomLens";
            this.groupBoxZoomLens.Size = new System.Drawing.Size(154, 74);
            this.groupBoxZoomLens.TabIndex = 13;
            this.groupBoxZoomLens.TabStop = false;
            this.groupBoxZoomLens.Text = "Zoom Lens";
            // 
            // lbMovGoto
            // 
            this.lbMovGoto.AutoSize = true;
            this.lbMovGoto.Font = new System.Drawing.Font("Arial", 12F);
            this.lbMovGoto.Location = new System.Drawing.Point(20, 37);
            this.lbMovGoto.Name = "lbMovGoto";
            this.lbMovGoto.Size = new System.Drawing.Size(54, 18);
            this.lbMovGoto.TabIndex = 12;
            this.lbMovGoto.Text = "Goto : ";
            // 
            // gbXYYTable
            // 
            this.gbXYYTable.Controls.Add(this.checkBoxTableManual);
            this.gbXYYTable.Controls.Add(this.labelRightResult);
            this.gbXYYTable.Controls.Add(this.lbTableStep);
            this.gbXYYTable.Controls.Add(this.numericUpDownTableSteps);
            this.gbXYYTable.Controls.Add(this.XYYTableSave);
            this.gbXYYTable.Controls.Add(this.labelLeftResult);
            this.gbXYYTable.Controls.Add(this.radioButtonTableX);
            this.gbXYYTable.Controls.Add(this.radioButtonTableY);
            this.gbXYYTable.Controls.Add(this.buttonXYYTableMove);
            this.gbXYYTable.Controls.Add(this.radioButtonTableTheta);
            this.gbXYYTable.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbXYYTable.Location = new System.Drawing.Point(464, 97);
            this.gbXYYTable.Name = "gbXYYTable";
            this.gbXYYTable.Size = new System.Drawing.Size(419, 118);
            this.gbXYYTable.TabIndex = 14;
            this.gbXYYTable.TabStop = false;
            this.gbXYYTable.Text = "XYY Table";
            this.gbXYYTable.Visible = false;
            // 
            // checkBoxTableManual
            // 
            this.checkBoxTableManual.AutoSize = true;
            this.checkBoxTableManual.Font = new System.Drawing.Font("Arial", 12F);
            this.checkBoxTableManual.Location = new System.Drawing.Point(243, 72);
            this.checkBoxTableManual.Name = "checkBoxTableManual";
            this.checkBoxTableManual.Size = new System.Drawing.Size(77, 22);
            this.checkBoxTableManual.TabIndex = 16;
            this.checkBoxTableManual.Text = "Manual";
            this.checkBoxTableManual.UseVisualStyleBackColor = true;
            // 
            // labelRightResult
            // 
            this.labelRightResult.AutoSize = true;
            this.labelRightResult.Font = new System.Drawing.Font("Arial", 12F);
            this.labelRightResult.Location = new System.Drawing.Point(15, 94);
            this.labelRightResult.Name = "labelRightResult";
            this.labelRightResult.Size = new System.Drawing.Size(161, 18);
            this.labelRightResult.TabIndex = 15;
            this.labelRightResult.Text = "Right steps per pixel : ";
            // 
            // lbTableStep
            // 
            this.lbTableStep.AutoSize = true;
            this.lbTableStep.Font = new System.Drawing.Font("Arial", 12F);
            this.lbTableStep.Location = new System.Drawing.Point(186, 32);
            this.lbTableStep.Name = "lbTableStep";
            this.lbTableStep.Size = new System.Drawing.Size(61, 18);
            this.lbTableStep.TabIndex = 14;
            this.lbTableStep.Text = "Steps : ";
            // 
            // numericUpDownTableSteps
            // 
            this.numericUpDownTableSteps.Font = new System.Drawing.Font("Arial", 12F);
            this.numericUpDownTableSteps.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.numericUpDownTableSteps.Location = new System.Drawing.Point(250, 29);
            this.numericUpDownTableSteps.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numericUpDownTableSteps.Minimum = new decimal(new int[] {
            5000,
            0,
            0,
            -2147483648});
            this.numericUpDownTableSteps.Name = "numericUpDownTableSteps";
            this.numericUpDownTableSteps.Size = new System.Drawing.Size(70, 26);
            this.numericUpDownTableSteps.TabIndex = 13;
            this.numericUpDownTableSteps.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ChangeMotorSteps);
            this.numericUpDownTableSteps.MouseUp += new System.Windows.Forms.MouseEventHandler(this.numericUpDownSteps_MouseUp);
            // 
            // XYYTableSave
            // 
            this.XYYTableSave.BackColor = System.Drawing.Color.LightCoral;
            this.XYYTableSave.Font = new System.Drawing.Font("Arial", 12F);
            this.XYYTableSave.Location = new System.Drawing.Point(328, 72);
            this.XYYTableSave.Name = "XYYTableSave";
            this.XYYTableSave.Size = new System.Drawing.Size(75, 29);
            this.XYYTableSave.TabIndex = 12;
            this.XYYTableSave.Text = "SAVE";
            this.XYYTableSave.UseVisualStyleBackColor = false;
            this.XYYTableSave.Click += new System.EventHandler(this.Save_Click);
            // 
            // labelLeftResult
            // 
            this.labelLeftResult.AutoSize = true;
            this.labelLeftResult.Font = new System.Drawing.Font("Arial", 12F);
            this.labelLeftResult.Location = new System.Drawing.Point(15, 63);
            this.labelLeftResult.Name = "labelLeftResult";
            this.labelLeftResult.Size = new System.Drawing.Size(151, 18);
            this.labelLeftResult.TabIndex = 11;
            this.labelLeftResult.Text = "Left steps per pixel : ";
            // 
            // gbCamera
            // 
            this.gbCamera.Controls.Add(this.radioButtonCameraRightY);
            this.gbCamera.Controls.Add(this.checkBoxCameraManual);
            this.gbCamera.Controls.Add(this.labelRightCameraResult);
            this.gbCamera.Controls.Add(this.buttonCameraSave);
            this.gbCamera.Controls.Add(this.buttonCameraMove);
            this.gbCamera.Controls.Add(this.labelLeftCameraResult);
            this.gbCamera.Controls.Add(this.lbCameraStep);
            this.gbCamera.Controls.Add(this.numericUpDownCameraSteps);
            this.gbCamera.Controls.Add(this.radioButtonCameraLeftY);
            this.gbCamera.Controls.Add(this.radioButtonrCameraRghtX);
            this.gbCamera.Controls.Add(this.radioButtonCameraLeftX);
            this.gbCamera.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbCamera.Location = new System.Drawing.Point(464, 97);
            this.gbCamera.Name = "gbCamera";
            this.gbCamera.Size = new System.Drawing.Size(507, 118);
            this.gbCamera.TabIndex = 17;
            this.gbCamera.TabStop = false;
            this.gbCamera.Text = "CameraAxisMotor";
            this.gbCamera.Visible = false;
            // 
            // radioButtonCameraRightY
            // 
            this.radioButtonCameraRightY.AutoSize = true;
            this.radioButtonCameraRightY.Font = new System.Drawing.Font("Arial", 12F);
            this.radioButtonCameraRightY.Location = new System.Drawing.Point(219, 28);
            this.radioButtonCameraRightY.Name = "radioButtonCameraRightY";
            this.radioButtonCameraRightY.Size = new System.Drawing.Size(71, 22);
            this.radioButtonCameraRightY.TabIndex = 12;
            this.radioButtonCameraRightY.TabStop = true;
            this.radioButtonCameraRightY.Text = "RightY";
            this.radioButtonCameraRightY.UseVisualStyleBackColor = true;
            // 
            // checkBoxCameraManual
            // 
            this.checkBoxCameraManual.AutoSize = true;
            this.checkBoxCameraManual.Font = new System.Drawing.Font("Arial", 12F);
            this.checkBoxCameraManual.Location = new System.Drawing.Point(346, 72);
            this.checkBoxCameraManual.Name = "checkBoxCameraManual";
            this.checkBoxCameraManual.Size = new System.Drawing.Size(77, 22);
            this.checkBoxCameraManual.TabIndex = 11;
            this.checkBoxCameraManual.Text = "Manual";
            this.checkBoxCameraManual.UseVisualStyleBackColor = true;
            // 
            // labelRightCameraResult
            // 
            this.labelRightCameraResult.AutoSize = true;
            this.labelRightCameraResult.Font = new System.Drawing.Font("Arial", 12F);
            this.labelRightCameraResult.Location = new System.Drawing.Point(13, 90);
            this.labelRightCameraResult.Name = "labelRightCameraResult";
            this.labelRightCameraResult.Size = new System.Drawing.Size(161, 18);
            this.labelRightCameraResult.TabIndex = 10;
            this.labelRightCameraResult.Text = "Right steps per pixel : ";
            // 
            // buttonCameraSave
            // 
            this.buttonCameraSave.Font = new System.Drawing.Font("Arial", 12F);
            this.buttonCameraSave.Location = new System.Drawing.Point(437, 72);
            this.buttonCameraSave.Name = "buttonCameraSave";
            this.buttonCameraSave.Size = new System.Drawing.Size(59, 26);
            this.buttonCameraSave.TabIndex = 9;
            this.buttonCameraSave.Text = "Save";
            this.buttonCameraSave.UseVisualStyleBackColor = true;
            this.buttonCameraSave.Click += new System.EventHandler(this.ButtonCameraSave_Click);
            // 
            // buttonCameraMove
            // 
            this.buttonCameraMove.Font = new System.Drawing.Font("Arial", 12F);
            this.buttonCameraMove.Location = new System.Drawing.Point(437, 28);
            this.buttonCameraMove.Name = "buttonCameraMove";
            this.buttonCameraMove.Size = new System.Drawing.Size(59, 27);
            this.buttonCameraMove.TabIndex = 8;
            this.buttonCameraMove.Text = "Go";
            this.buttonCameraMove.UseVisualStyleBackColor = true;
            this.buttonCameraMove.Click += new System.EventHandler(this.ButtonCameraMove_Click);
            // 
            // labelLeftCameraResult
            // 
            this.labelLeftCameraResult.AutoSize = true;
            this.labelLeftCameraResult.Font = new System.Drawing.Font("Arial", 12F);
            this.labelLeftCameraResult.Location = new System.Drawing.Point(13, 60);
            this.labelLeftCameraResult.Name = "labelLeftCameraResult";
            this.labelLeftCameraResult.Size = new System.Drawing.Size(151, 18);
            this.labelLeftCameraResult.TabIndex = 7;
            this.labelLeftCameraResult.Text = "Left steps per pixel : ";
            // 
            // lbCameraStep
            // 
            this.lbCameraStep.AutoSize = true;
            this.lbCameraStep.Font = new System.Drawing.Font("Arial", 12F);
            this.lbCameraStep.Location = new System.Drawing.Point(290, 30);
            this.lbCameraStep.Name = "lbCameraStep";
            this.lbCameraStep.Size = new System.Drawing.Size(61, 18);
            this.lbCameraStep.TabIndex = 5;
            this.lbCameraStep.Text = "Steps : ";
            // 
            // numericUpDownCameraSteps
            // 
            this.numericUpDownCameraSteps.Font = new System.Drawing.Font("Arial", 12F);
            this.numericUpDownCameraSteps.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.numericUpDownCameraSteps.Location = new System.Drawing.Point(352, 28);
            this.numericUpDownCameraSteps.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownCameraSteps.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.numericUpDownCameraSteps.Name = "numericUpDownCameraSteps";
            this.numericUpDownCameraSteps.Size = new System.Drawing.Size(74, 26);
            this.numericUpDownCameraSteps.TabIndex = 3;
            // 
            // radioButtonCameraLeftY
            // 
            this.radioButtonCameraLeftY.AutoSize = true;
            this.radioButtonCameraLeftY.Font = new System.Drawing.Font("Arial", 12F);
            this.radioButtonCameraLeftY.Location = new System.Drawing.Point(159, 28);
            this.radioButtonCameraLeftY.Name = "radioButtonCameraLeftY";
            this.radioButtonCameraLeftY.Size = new System.Drawing.Size(61, 22);
            this.radioButtonCameraLeftY.TabIndex = 2;
            this.radioButtonCameraLeftY.TabStop = true;
            this.radioButtonCameraLeftY.Text = "LeftY";
            this.radioButtonCameraLeftY.UseVisualStyleBackColor = true;
            // 
            // radioButtonrCameraRghtX
            // 
            this.radioButtonrCameraRghtX.AutoSize = true;
            this.radioButtonrCameraRghtX.Font = new System.Drawing.Font("Arial", 12F);
            this.radioButtonrCameraRghtX.Location = new System.Drawing.Point(80, 28);
            this.radioButtonrCameraRghtX.Name = "radioButtonrCameraRghtX";
            this.radioButtonrCameraRghtX.Size = new System.Drawing.Size(73, 22);
            this.radioButtonrCameraRghtX.TabIndex = 1;
            this.radioButtonrCameraRghtX.TabStop = true;
            this.radioButtonrCameraRghtX.Text = "RightX";
            this.radioButtonrCameraRghtX.UseVisualStyleBackColor = true;
            // 
            // radioButtonCameraLeftX
            // 
            this.radioButtonCameraLeftX.AutoSize = true;
            this.radioButtonCameraLeftX.Font = new System.Drawing.Font("Arial", 12F);
            this.radioButtonCameraLeftX.Location = new System.Drawing.Point(11, 27);
            this.radioButtonCameraLeftX.Name = "radioButtonCameraLeftX";
            this.radioButtonCameraLeftX.Size = new System.Drawing.Size(63, 22);
            this.radioButtonCameraLeftX.TabIndex = 0;
            this.radioButtonCameraLeftX.TabStop = true;
            this.radioButtonCameraLeftX.Text = "LeftX";
            this.radioButtonCameraLeftX.UseVisualStyleBackColor = true;
            // 
            // numericUpDownPatternCenterDistance
            // 
            this.numericUpDownPatternCenterDistance.DecimalPlaces = 2;
            this.numericUpDownPatternCenterDistance.Font = new System.Drawing.Font("Arial", 12F);
            this.numericUpDownPatternCenterDistance.Location = new System.Drawing.Point(873, 30);
            this.numericUpDownPatternCenterDistance.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownPatternCenterDistance.Name = "numericUpDownPatternCenterDistance";
            this.numericUpDownPatternCenterDistance.Size = new System.Drawing.Size(98, 26);
            this.numericUpDownPatternCenterDistance.TabIndex = 16;
            // 
            // lbPettrnCenterDis
            // 
            this.lbPettrnCenterDis.AutoSize = true;
            this.lbPettrnCenterDis.Location = new System.Drawing.Point(644, 32);
            this.lbPettrnCenterDis.Name = "lbPettrnCenterDis";
            this.lbPettrnCenterDis.Size = new System.Drawing.Size(223, 18);
            this.lbPettrnCenterDis.TabIndex = 17;
            this.lbPettrnCenterDis.Text = "Pattern Center Distance(mm) : ";
            // 
            // RightCameraWindow
            // 
            this.RightCameraWindow.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.RightCameraWindow.Location = new System.Drawing.Point(503, 234);
            this.RightCameraWindow.Name = "RightCameraWindow";
            this.RightCameraWindow.Size = new System.Drawing.Size(472, 354);
            this.RightCameraWindow.TabIndex = 1;
            // 
            // LeftCameraWindow
            // 
            this.LeftCameraWindow.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.LeftCameraWindow.Location = new System.Drawing.Point(12, 234);
            this.LeftCameraWindow.Name = "LeftCameraWindow";
            this.LeftCameraWindow.Size = new System.Drawing.Size(472, 354);
            this.LeftCameraWindow.TabIndex = 0;
            // 
            // lbRecord
            // 
            this.lbRecord.AutoSize = true;
            this.lbRecord.Location = new System.Drawing.Point(644, 68);
            this.lbRecord.Name = "lbRecord";
            this.lbRecord.Size = new System.Drawing.Size(71, 18);
            this.lbRecord.TabIndex = 18;
            this.lbRecord.Text = "Record : ";
            // 
            // radioButtonCamera
            // 
            this.radioButtonCamera.AutoSize = true;
            this.radioButtonCamera.Location = new System.Drawing.Point(371, 30);
            this.radioButtonCamera.Name = "radioButtonCamera";
            this.radioButtonCamera.Size = new System.Drawing.Size(83, 22);
            this.radioButtonCamera.TabIndex = 19;
            this.radioButtonCamera.TabStop = true;
            this.radioButtonCamera.Text = "Camera";
            this.radioButtonCamera.UseVisualStyleBackColor = true;
            this.radioButtonCamera.CheckedChanged += new System.EventHandler(this.RadioButtonCamera_CheckedChanged);
            // 
            // radioButtonTable
            // 
            this.radioButtonTable.AutoSize = true;
            this.radioButtonTable.Location = new System.Drawing.Point(371, 69);
            this.radioButtonTable.Name = "radioButtonTable";
            this.radioButtonTable.Size = new System.Drawing.Size(63, 22);
            this.radioButtonTable.TabIndex = 20;
            this.radioButtonTable.TabStop = true;
            this.radioButtonTable.Text = "Table";
            this.radioButtonTable.UseVisualStyleBackColor = true;
            this.radioButtonTable.CheckedChanged += new System.EventHandler(this.RadioButtonTable_CheckedChanged);
            // 
            // DialogTuneTable
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(991, 604);
            this.Controls.Add(this.radioButtonTable);
            this.Controls.Add(this.radioButtonCamera);
            this.Controls.Add(this.lbRecord);
            this.Controls.Add(this.lbPettrnCenterDis);
            this.Controls.Add(this.numericUpDownPatternCenterDistance);
            this.Controls.Add(this.groupBoxZoomLens);
            this.Controls.Add(this.SaveRightPattern);
            this.Controls.Add(this.buttonSaveLEetPattern);
            this.Controls.Add(this.pictureBoxRightPattern);
            this.Controls.Add(this.pictureBoxLeftPattern);
            this.Controls.Add(this.RightCameraWindow);
            this.Controls.Add(this.LeftCameraWindow);
            this.Controls.Add(this.gbXYYTable);
            this.Controls.Add(this.gbCamera);
            this.Font = new System.Drawing.Font("Arial", 12F);
            this.Name = "DialogTuneTable";
            this.Text = "DialogTuneTable";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DialogTuneTable_FormClosing);
            this.Load += new System.EventHandler(this.DialogTuneTable_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLeftPattern)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRightPattern)).EndInit();
            this.groupBoxZoomLens.ResumeLayout(false);
            this.groupBoxZoomLens.PerformLayout();
            this.gbXYYTable.ResumeLayout(false);
            this.gbXYYTable.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTableSteps)).EndInit();
            this.gbCamera.ResumeLayout(false);
            this.gbCamera.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCameraSteps)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPatternCenterDistance)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MRLibrary.SKZoomAndPanWindow LeftCameraWindow;
        private MRLibrary.SKZoomAndPanWindow RightCameraWindow;
        private System.Windows.Forms.PictureBox pictureBoxLeftPattern;
        private System.Windows.Forms.PictureBox pictureBoxRightPattern;
        private System.Windows.Forms.Button buttonSaveLEetPattern;
        private System.Windows.Forms.Button SaveRightPattern;
        private System.Windows.Forms.RadioButton radioButtonTableX;
        private System.Windows.Forms.RadioButton radioButtonTableY;
        private System.Windows.Forms.RadioButton radioButtonTableTheta;
        private System.Windows.Forms.Button buttonXYYTableMove;
        private System.Windows.Forms.ComboBox comboBoxZoomLensGoto;
        private System.Windows.Forms.GroupBox groupBoxZoomLens;
        private System.Windows.Forms.GroupBox gbXYYTable;
        private System.Windows.Forms.Label lbMovGoto;
        private System.Windows.Forms.Label lbTableStep;
        private System.Windows.Forms.NumericUpDown numericUpDownTableSteps;
        private System.Windows.Forms.Button XYYTableSave;
        private System.Windows.Forms.Label labelLeftResult;
        private System.Windows.Forms.Label labelRightResult;
        private System.Windows.Forms.NumericUpDown numericUpDownPatternCenterDistance;
        private System.Windows.Forms.Label lbPettrnCenterDis;
        private System.Windows.Forms.CheckBox checkBoxTableManual;
        private System.Windows.Forms.Label lbRecord;
        private System.Windows.Forms.GroupBox gbCamera;
        private System.Windows.Forms.Label labelLeftCameraResult;
        private System.Windows.Forms.Label lbCameraStep;
        private System.Windows.Forms.NumericUpDown numericUpDownCameraSteps;
        private System.Windows.Forms.RadioButton radioButtonCameraLeftY;
        private System.Windows.Forms.RadioButton radioButtonrCameraRghtX;
        private System.Windows.Forms.RadioButton radioButtonCameraLeftX;
        private System.Windows.Forms.Button buttonCameraSave;
        private System.Windows.Forms.Button buttonCameraMove;
        private System.Windows.Forms.RadioButton radioButtonCamera;
        private System.Windows.Forms.RadioButton radioButtonTable;
        private System.Windows.Forms.Label labelRightCameraResult;
        private System.Windows.Forms.CheckBox checkBoxCameraManual;
        private System.Windows.Forms.RadioButton radioButtonCameraRightY;
    }
}