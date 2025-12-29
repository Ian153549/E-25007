namespace NSAA_16Axis
{
    partial class DialogTuneLens
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
            this.rbLeftLens = new System.Windows.Forms.RadioButton();
            this.rbRightLens = new System.Windows.Forms.RadioButton();
            this.lbMoveTo = new System.Windows.Forms.Label();
            this.comboBoxGoto = new System.Windows.Forms.ComboBox();
            this.rbLive = new System.Windows.Forms.RadioButton();
            this.rbMeasure = new System.Windows.Forms.RadioButton();
            this.gbParameter = new System.Windows.Forms.GroupBox();
            this.btnPreCalculate = new System.Windows.Forms.Button();
            this.btnCalAndSave = new System.Windows.Forms.Button();
            this.numericUpDownPatternWidth = new System.Windows.Forms.NumericUpDown();
            this.lbPatternWidth = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lbX12 = new System.Windows.Forms.Label();
            this.numericUpDownX10 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownX9 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownX8 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownX7 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownX6 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownX5 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownX4 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownX3 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownX2 = new System.Windows.Forms.NumericUpDown();
            this.lbX10 = new System.Windows.Forms.Label();
            this.lbX1 = new System.Windows.Forms.Label();
            this.lbX2 = new System.Windows.Forms.Label();
            this.lbX3 = new System.Windows.Forms.Label();
            this.lbX4 = new System.Windows.Forms.Label();
            this.lbX5 = new System.Windows.Forms.Label();
            this.lbX6 = new System.Windows.Forms.Label();
            this.lbX7 = new System.Windows.Forms.Label();
            this.lbX8 = new System.Windows.Forms.Label();
            this.lbX9 = new System.Windows.Forms.Label();
            this.numericUpDownX1 = new System.Windows.Forms.NumericUpDown();
            this.lbX11 = new System.Windows.Forms.Label();
            this.numericUpDownX11 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownX12 = new System.Windows.Forms.NumericUpDown();
            this.pictureBoxMeasure = new System.Windows.Forms.PictureBox();
            this.skZoomAndPanWindowMeasure = new MRLibrary.SKZoomAndPanWindow();
            this.gbCameraSelect = new System.Windows.Forms.GroupBox();
            this.rbUpperCamera = new System.Windows.Forms.RadioButton();
            this.rbLowerCamera = new System.Windows.Forms.RadioButton();
            this.lbRecipe = new System.Windows.Forms.Label();
            this.comboBoxRecipe = new System.Windows.Forms.ComboBox();
            this.btnMoveToRecipePos = new System.Windows.Forms.Button();
            this.btnSaveCurrentPos = new System.Windows.Forms.Button();
            this.gbParameter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPatternWidth)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMeasure)).BeginInit();
            this.gbCameraSelect.SuspendLayout();
            this.SuspendLayout();
            // 
            // rbLeftLens
            // 
            this.rbLeftLens.AutoSize = true;
            this.rbLeftLens.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbLeftLens.Location = new System.Drawing.Point(1363, 26);
            this.rbLeftLens.Margin = new System.Windows.Forms.Padding(4);
            this.rbLeftLens.Name = "rbLeftLens";
            this.rbLeftLens.Size = new System.Drawing.Size(159, 27);
            this.rbLeftLens.TabIndex = 1;
            this.rbLeftLens.TabStop = true;
            this.rbLeftLens.Text = "Left zoom lens";
            this.rbLeftLens.UseVisualStyleBackColor = true;
            this.rbLeftLens.CheckedChanged += new System.EventHandler(this.rbLeftLens_CheckedChanged);
            // 
            // rbRightLens
            // 
            this.rbRightLens.AutoSize = true;
            this.rbRightLens.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbRightLens.Location = new System.Drawing.Point(1530, 26);
            this.rbRightLens.Margin = new System.Windows.Forms.Padding(4);
            this.rbRightLens.Name = "rbRightLens";
            this.rbRightLens.Size = new System.Drawing.Size(170, 27);
            this.rbRightLens.TabIndex = 2;
            this.rbRightLens.TabStop = true;
            this.rbRightLens.Text = "Right zoom lens";
            this.rbRightLens.UseVisualStyleBackColor = true;
            this.rbRightLens.CheckedChanged += new System.EventHandler(this.rbRightLens_CheckedChanged);
            // 
            // lbMoveTo
            // 
            this.lbMoveTo.AutoSize = true;
            this.lbMoveTo.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbMoveTo.Location = new System.Drawing.Point(59, 102);
            this.lbMoveTo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbMoveTo.Name = "lbMoveTo";
            this.lbMoveTo.Size = new System.Drawing.Size(72, 23);
            this.lbMoveTo.TabIndex = 3;
            this.lbMoveTo.Text = "Goto : ";
            // 
            // comboBoxGoto
            // 
            this.comboBoxGoto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxGoto.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxGoto.FormattingEnabled = true;
            this.comboBoxGoto.Location = new System.Drawing.Point(135, 99);
            this.comboBoxGoto.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxGoto.Name = "comboBoxGoto";
            this.comboBoxGoto.Size = new System.Drawing.Size(160, 31);
            this.comboBoxGoto.TabIndex = 4;
            this.comboBoxGoto.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // rbLive
            // 
            this.rbLive.AutoSize = true;
            this.rbLive.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbLive.Location = new System.Drawing.Point(164, 151);
            this.rbLive.Margin = new System.Windows.Forms.Padding(4);
            this.rbLive.Name = "rbLive";
            this.rbLive.Size = new System.Drawing.Size(66, 27);
            this.rbLive.TabIndex = 5;
            this.rbLive.TabStop = true;
            this.rbLive.Text = "Live";
            this.rbLive.UseVisualStyleBackColor = true;
            this.rbLive.CheckedChanged += new System.EventHandler(this.radioButtonLive_CheckedChanged);
            // 
            // rbMeasure
            // 
            this.rbMeasure.AutoSize = true;
            this.rbMeasure.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbMeasure.Location = new System.Drawing.Point(271, 151);
            this.rbMeasure.Margin = new System.Windows.Forms.Padding(4);
            this.rbMeasure.Name = "rbMeasure";
            this.rbMeasure.Size = new System.Drawing.Size(108, 27);
            this.rbMeasure.TabIndex = 6;
            this.rbMeasure.TabStop = true;
            this.rbMeasure.Text = "Measure";
            this.rbMeasure.UseVisualStyleBackColor = true;
            this.rbMeasure.CheckedChanged += new System.EventHandler(this.radioButtonMeasure_CheckedChanged);
            // 
            // gbParameter
            // 
            this.gbParameter.Controls.Add(this.btnPreCalculate);
            this.gbParameter.Controls.Add(this.btnCalAndSave);
            this.gbParameter.Controls.Add(this.numericUpDownPatternWidth);
            this.gbParameter.Controls.Add(this.lbPatternWidth);
            this.gbParameter.Controls.Add(this.lbMoveTo);
            this.gbParameter.Controls.Add(this.comboBoxGoto);
            this.gbParameter.Controls.Add(this.rbLive);
            this.gbParameter.Controls.Add(this.tableLayoutPanel1);
            this.gbParameter.Controls.Add(this.rbMeasure);
            this.gbParameter.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbParameter.Location = new System.Drawing.Point(1300, 61);
            this.gbParameter.Margin = new System.Windows.Forms.Padding(4);
            this.gbParameter.Name = "gbParameter";
            this.gbParameter.Padding = new System.Windows.Forms.Padding(4);
            this.gbParameter.Size = new System.Drawing.Size(503, 785);
            this.gbParameter.TabIndex = 7;
            this.gbParameter.TabStop = false;
            this.gbParameter.Text = "Parameter";
            // 
            // btnPreCalculate
            // 
            this.btnPreCalculate.Location = new System.Drawing.Point(27, 729);
            this.btnPreCalculate.Margin = new System.Windows.Forms.Padding(4);
            this.btnPreCalculate.Name = "btnPreCalculate";
            this.btnPreCalculate.Size = new System.Drawing.Size(153, 42);
            this.btnPreCalculate.TabIndex = 12;
            this.btnPreCalculate.Text = "Precalculate";
            this.btnPreCalculate.UseVisualStyleBackColor = true;
            this.btnPreCalculate.Click += new System.EventHandler(this.buttonPrecalculate_Click);
            // 
            // btnCalAndSave
            // 
            this.btnCalAndSave.BackColor = System.Drawing.Color.LightCoral;
            this.btnCalAndSave.Location = new System.Drawing.Point(244, 729);
            this.btnCalAndSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnCalAndSave.Name = "btnCalAndSave";
            this.btnCalAndSave.Size = new System.Drawing.Size(221, 42);
            this.btnCalAndSave.TabIndex = 11;
            this.btnCalAndSave.Text = "Calculate and Save";
            this.btnCalAndSave.UseVisualStyleBackColor = false;
            this.btnCalAndSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // numericUpDownPatternWidth
            // 
            this.numericUpDownPatternWidth.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.numericUpDownPatternWidth.Location = new System.Drawing.Point(255, 46);
            this.numericUpDownPatternWidth.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDownPatternWidth.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.numericUpDownPatternWidth.Name = "numericUpDownPatternWidth";
            this.numericUpDownPatternWidth.Size = new System.Drawing.Size(80, 30);
            this.numericUpDownPatternWidth.TabIndex = 10;
            // 
            // lbPatternWidth
            // 
            this.lbPatternWidth.AutoSize = true;
            this.lbPatternWidth.Location = new System.Drawing.Point(59, 49);
            this.lbPatternWidth.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbPatternWidth.Name = "lbPatternWidth";
            this.lbPatternWidth.Size = new System.Drawing.Size(184, 23);
            this.lbPatternWidth.TabIndex = 9;
            this.lbPatternWidth.Text = "Pattern width(um) : ";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.lbX12, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDownX10, 1, 9);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDownX9, 1, 8);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDownX8, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDownX7, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDownX6, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDownX5, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDownX4, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDownX3, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDownX2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lbX10, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.lbX1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbX2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lbX3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lbX4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lbX5, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.lbX6, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.lbX7, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.lbX8, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.lbX9, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDownX1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbX11, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDownX11, 1, 10);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDownX12, 1, 11);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(27, 201);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 12;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(439, 502);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // lbX12
            // 
            this.lbX12.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbX12.AutoSize = true;
            this.lbX12.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbX12.Location = new System.Drawing.Point(52, 465);
            this.lbX12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbX12.Name = "lbX12";
            this.lbX12.Size = new System.Drawing.Size(116, 23);
            this.lbX12.TabIndex = 20;
            this.lbX12.Text = "X12 (pixel) :";
            // 
            // numericUpDownX10
            // 
            this.numericUpDownX10.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numericUpDownX10.DecimalPlaces = 2;
            this.numericUpDownX10.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.numericUpDownX10.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDownX10.Location = new System.Drawing.Point(249, 375);
            this.numericUpDownX10.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDownX10.Maximum = new decimal(new int[] {
            6000,
            0,
            0,
            0});
            this.numericUpDownX10.Name = "numericUpDownX10";
            this.numericUpDownX10.Size = new System.Drawing.Size(160, 30);
            this.numericUpDownX10.TabIndex = 19;
            // 
            // numericUpDownX9
            // 
            this.numericUpDownX9.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numericUpDownX9.DecimalPlaces = 2;
            this.numericUpDownX9.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.numericUpDownX9.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDownX9.Location = new System.Drawing.Point(249, 334);
            this.numericUpDownX9.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDownX9.Maximum = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            this.numericUpDownX9.Name = "numericUpDownX9";
            this.numericUpDownX9.Size = new System.Drawing.Size(160, 30);
            this.numericUpDownX9.TabIndex = 18;
            // 
            // numericUpDownX8
            // 
            this.numericUpDownX8.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numericUpDownX8.DecimalPlaces = 2;
            this.numericUpDownX8.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.numericUpDownX8.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDownX8.Location = new System.Drawing.Point(249, 293);
            this.numericUpDownX8.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDownX8.Maximum = new decimal(new int[] {
            2600,
            0,
            0,
            0});
            this.numericUpDownX8.Name = "numericUpDownX8";
            this.numericUpDownX8.Size = new System.Drawing.Size(160, 30);
            this.numericUpDownX8.TabIndex = 17;
            // 
            // numericUpDownX7
            // 
            this.numericUpDownX7.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numericUpDownX7.DecimalPlaces = 2;
            this.numericUpDownX7.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.numericUpDownX7.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDownX7.Location = new System.Drawing.Point(249, 252);
            this.numericUpDownX7.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDownX7.Maximum = new decimal(new int[] {
            2600,
            0,
            0,
            0});
            this.numericUpDownX7.Name = "numericUpDownX7";
            this.numericUpDownX7.Size = new System.Drawing.Size(160, 30);
            this.numericUpDownX7.TabIndex = 16;
            // 
            // numericUpDownX6
            // 
            this.numericUpDownX6.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numericUpDownX6.DecimalPlaces = 2;
            this.numericUpDownX6.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.numericUpDownX6.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDownX6.Location = new System.Drawing.Point(249, 211);
            this.numericUpDownX6.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDownX6.Maximum = new decimal(new int[] {
            2600,
            0,
            0,
            0});
            this.numericUpDownX6.Name = "numericUpDownX6";
            this.numericUpDownX6.Size = new System.Drawing.Size(160, 30);
            this.numericUpDownX6.TabIndex = 15;
            // 
            // numericUpDownX5
            // 
            this.numericUpDownX5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numericUpDownX5.DecimalPlaces = 2;
            this.numericUpDownX5.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.numericUpDownX5.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDownX5.Location = new System.Drawing.Point(249, 170);
            this.numericUpDownX5.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDownX5.Maximum = new decimal(new int[] {
            2600,
            0,
            0,
            0});
            this.numericUpDownX5.Name = "numericUpDownX5";
            this.numericUpDownX5.Size = new System.Drawing.Size(160, 30);
            this.numericUpDownX5.TabIndex = 14;
            // 
            // numericUpDownX4
            // 
            this.numericUpDownX4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numericUpDownX4.DecimalPlaces = 2;
            this.numericUpDownX4.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.numericUpDownX4.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDownX4.Location = new System.Drawing.Point(249, 129);
            this.numericUpDownX4.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDownX4.Maximum = new decimal(new int[] {
            2600,
            0,
            0,
            0});
            this.numericUpDownX4.Name = "numericUpDownX4";
            this.numericUpDownX4.Size = new System.Drawing.Size(160, 30);
            this.numericUpDownX4.TabIndex = 13;
            // 
            // numericUpDownX3
            // 
            this.numericUpDownX3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numericUpDownX3.DecimalPlaces = 2;
            this.numericUpDownX3.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.numericUpDownX3.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDownX3.Location = new System.Drawing.Point(249, 88);
            this.numericUpDownX3.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDownX3.Maximum = new decimal(new int[] {
            2600,
            0,
            0,
            0});
            this.numericUpDownX3.Name = "numericUpDownX3";
            this.numericUpDownX3.Size = new System.Drawing.Size(160, 30);
            this.numericUpDownX3.TabIndex = 12;
            // 
            // numericUpDownX2
            // 
            this.numericUpDownX2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numericUpDownX2.DecimalPlaces = 2;
            this.numericUpDownX2.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.numericUpDownX2.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDownX2.Location = new System.Drawing.Point(249, 47);
            this.numericUpDownX2.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDownX2.Maximum = new decimal(new int[] {
            2600,
            0,
            0,
            0});
            this.numericUpDownX2.Name = "numericUpDownX2";
            this.numericUpDownX2.Size = new System.Drawing.Size(160, 30);
            this.numericUpDownX2.TabIndex = 11;
            // 
            // lbX10
            // 
            this.lbX10.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbX10.AutoSize = true;
            this.lbX10.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbX10.Location = new System.Drawing.Point(52, 378);
            this.lbX10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbX10.Name = "lbX10";
            this.lbX10.Size = new System.Drawing.Size(116, 23);
            this.lbX10.TabIndex = 9;
            this.lbX10.Text = "X10 (pixel) :";
            // 
            // lbX1
            // 
            this.lbX1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbX1.AutoSize = true;
            this.lbX1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbX1.Location = new System.Drawing.Point(57, 9);
            this.lbX1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbX1.Name = "lbX1";
            this.lbX1.Size = new System.Drawing.Size(105, 23);
            this.lbX1.TabIndex = 0;
            this.lbX1.Text = "X1 (pixel) :";
            // 
            // lbX2
            // 
            this.lbX2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbX2.AutoSize = true;
            this.lbX2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbX2.Location = new System.Drawing.Point(57, 50);
            this.lbX2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbX2.Name = "lbX2";
            this.lbX2.Size = new System.Drawing.Size(105, 23);
            this.lbX2.TabIndex = 1;
            this.lbX2.Text = "X2 (pixel) :";
            // 
            // lbX3
            // 
            this.lbX3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbX3.AutoSize = true;
            this.lbX3.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbX3.Location = new System.Drawing.Point(57, 91);
            this.lbX3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbX3.Name = "lbX3";
            this.lbX3.Size = new System.Drawing.Size(105, 23);
            this.lbX3.TabIndex = 2;
            this.lbX3.Text = "X3 (pixel) :";
            // 
            // lbX4
            // 
            this.lbX4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbX4.AutoSize = true;
            this.lbX4.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbX4.Location = new System.Drawing.Point(57, 132);
            this.lbX4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbX4.Name = "lbX4";
            this.lbX4.Size = new System.Drawing.Size(105, 23);
            this.lbX4.TabIndex = 3;
            this.lbX4.Text = "X4 (pixel) :";
            // 
            // lbX5
            // 
            this.lbX5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbX5.AutoSize = true;
            this.lbX5.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbX5.Location = new System.Drawing.Point(57, 173);
            this.lbX5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbX5.Name = "lbX5";
            this.lbX5.Size = new System.Drawing.Size(105, 23);
            this.lbX5.TabIndex = 4;
            this.lbX5.Text = "X5 (pixel) :";
            // 
            // lbX6
            // 
            this.lbX6.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbX6.AutoSize = true;
            this.lbX6.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbX6.Location = new System.Drawing.Point(57, 214);
            this.lbX6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbX6.Name = "lbX6";
            this.lbX6.Size = new System.Drawing.Size(105, 23);
            this.lbX6.TabIndex = 5;
            this.lbX6.Text = "X6 (pixel) :";
            // 
            // lbX7
            // 
            this.lbX7.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbX7.AutoSize = true;
            this.lbX7.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbX7.Location = new System.Drawing.Point(57, 255);
            this.lbX7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbX7.Name = "lbX7";
            this.lbX7.Size = new System.Drawing.Size(105, 23);
            this.lbX7.TabIndex = 6;
            this.lbX7.Text = "X7 (pixel) :";
            // 
            // lbX8
            // 
            this.lbX8.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbX8.AutoSize = true;
            this.lbX8.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbX8.Location = new System.Drawing.Point(57, 296);
            this.lbX8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbX8.Name = "lbX8";
            this.lbX8.Size = new System.Drawing.Size(105, 23);
            this.lbX8.TabIndex = 7;
            this.lbX8.Text = "X8 (pixel) :";
            // 
            // lbX9
            // 
            this.lbX9.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbX9.AutoSize = true;
            this.lbX9.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbX9.Location = new System.Drawing.Point(57, 337);
            this.lbX9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbX9.Name = "lbX9";
            this.lbX9.Size = new System.Drawing.Size(105, 23);
            this.lbX9.TabIndex = 8;
            this.lbX9.Text = "X9 (pixel) :";
            // 
            // numericUpDownX1
            // 
            this.numericUpDownX1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numericUpDownX1.DecimalPlaces = 2;
            this.numericUpDownX1.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.numericUpDownX1.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDownX1.Location = new System.Drawing.Point(249, 6);
            this.numericUpDownX1.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDownX1.Maximum = new decimal(new int[] {
            2600,
            0,
            0,
            0});
            this.numericUpDownX1.Name = "numericUpDownX1";
            this.numericUpDownX1.Size = new System.Drawing.Size(160, 30);
            this.numericUpDownX1.TabIndex = 10;
            // 
            // lbX11
            // 
            this.lbX11.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbX11.AutoSize = true;
            this.lbX11.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbX11.Location = new System.Drawing.Point(52, 419);
            this.lbX11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbX11.Name = "lbX11";
            this.lbX11.Size = new System.Drawing.Size(115, 23);
            this.lbX11.TabIndex = 21;
            this.lbX11.Text = "X11 (pixel) :";
            // 
            // numericUpDownX11
            // 
            this.numericUpDownX11.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numericUpDownX11.DecimalPlaces = 2;
            this.numericUpDownX11.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.numericUpDownX11.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDownX11.Location = new System.Drawing.Point(249, 416);
            this.numericUpDownX11.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDownX11.Maximum = new decimal(new int[] {
            6000,
            0,
            0,
            0});
            this.numericUpDownX11.Name = "numericUpDownX11";
            this.numericUpDownX11.Size = new System.Drawing.Size(160, 30);
            this.numericUpDownX11.TabIndex = 22;
            // 
            // numericUpDownX12
            // 
            this.numericUpDownX12.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numericUpDownX12.DecimalPlaces = 2;
            this.numericUpDownX12.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.numericUpDownX12.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDownX12.Location = new System.Drawing.Point(249, 461);
            this.numericUpDownX12.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDownX12.Maximum = new decimal(new int[] {
            6000,
            0,
            0,
            0});
            this.numericUpDownX12.Name = "numericUpDownX12";
            this.numericUpDownX12.Size = new System.Drawing.Size(160, 30);
            this.numericUpDownX12.TabIndex = 23;
            // 
            // pictureBoxMeasure
            // 
            this.pictureBoxMeasure.Location = new System.Drawing.Point(17, 15);
            this.pictureBoxMeasure.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBoxMeasure.Name = "pictureBoxMeasure";
            this.pictureBoxMeasure.Size = new System.Drawing.Size(944, 708);
            this.pictureBoxMeasure.TabIndex = 9;
            this.pictureBoxMeasure.TabStop = false;
            this.pictureBoxMeasure.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxMeasure_MouseDown);
            this.pictureBoxMeasure.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxMeasure_MouseMove);
            this.pictureBoxMeasure.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxMeasure_MouseUp);
            // 
            // skZoomAndPanWindowMeasure
            // 
            this.skZoomAndPanWindowMeasure.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.skZoomAndPanWindowMeasure.Location = new System.Drawing.Point(17, 15);
            this.skZoomAndPanWindowMeasure.Margin = new System.Windows.Forms.Padding(4);
            this.skZoomAndPanWindowMeasure.Name = "skZoomAndPanWindowMeasure";
            this.skZoomAndPanWindowMeasure.Size = new System.Drawing.Size(944, 708);
            this.skZoomAndPanWindowMeasure.TabIndex = 0;
            // 
            // gbCameraSelect
            // 
            this.gbCameraSelect.Controls.Add(this.rbUpperCamera);
            this.gbCameraSelect.Controls.Add(this.rbLowerCamera);
            this.gbCameraSelect.Controls.Add(this.lbRecipe);
            this.gbCameraSelect.Controls.Add(this.comboBoxRecipe);
            this.gbCameraSelect.Controls.Add(this.btnMoveToRecipePos);
            this.gbCameraSelect.Controls.Add(this.btnSaveCurrentPos);
            this.gbCameraSelect.Font = new System.Drawing.Font("Arial", 12F);
            this.gbCameraSelect.Location = new System.Drawing.Point(1300, 865);
            this.gbCameraSelect.Margin = new System.Windows.Forms.Padding(4);
            this.gbCameraSelect.Name = "gbCameraSelect";
            this.gbCameraSelect.Padding = new System.Windows.Forms.Padding(4);
            this.gbCameraSelect.Size = new System.Drawing.Size(503, 150);
            this.gbCameraSelect.TabIndex = 100;
            this.gbCameraSelect.TabStop = false;
            this.gbCameraSelect.Text = "Camera / Recipe";
            this.gbCameraSelect.Visible = false;
            // 
            // rbUpperCamera
            // 
            this.rbUpperCamera.AutoSize = true;
            this.rbUpperCamera.Checked = true;
            this.rbUpperCamera.Location = new System.Drawing.Point(20, 31);
            this.rbUpperCamera.Margin = new System.Windows.Forms.Padding(4);
            this.rbUpperCamera.Name = "rbUpperCamera";
            this.rbUpperCamera.Size = new System.Drawing.Size(160, 27);
            this.rbUpperCamera.TabIndex = 0;
            this.rbUpperCamera.TabStop = true;
            this.rbUpperCamera.Text = "Upper Camera";
            this.rbUpperCamera.CheckedChanged += new System.EventHandler(this.rbUpperCamera_CheckedChanged);
            // 
            // rbLowerCamera
            // 
            this.rbLowerCamera.AutoSize = true;
            this.rbLowerCamera.Location = new System.Drawing.Point(200, 31);
            this.rbLowerCamera.Margin = new System.Windows.Forms.Padding(4);
            this.rbLowerCamera.Name = "rbLowerCamera";
            this.rbLowerCamera.Size = new System.Drawing.Size(162, 27);
            this.rbLowerCamera.TabIndex = 1;
            this.rbLowerCamera.Text = "Lower Camera";
            this.rbLowerCamera.CheckedChanged += new System.EventHandler(this.rbLowerCamera_CheckedChanged);
            // 
            // lbRecipe
            // 
            this.lbRecipe.AutoSize = true;
            this.lbRecipe.Location = new System.Drawing.Point(20, 69);
            this.lbRecipe.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbRecipe.Name = "lbRecipe";
            this.lbRecipe.Size = new System.Drawing.Size(77, 23);
            this.lbRecipe.TabIndex = 2;
            this.lbRecipe.Text = "Recipe:";
            // 
            // comboBoxRecipe
            // 
            this.comboBoxRecipe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRecipe.Location = new System.Drawing.Point(107, 65);
            this.comboBoxRecipe.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxRecipe.Name = "comboBoxRecipe";
            this.comboBoxRecipe.Size = new System.Drawing.Size(132, 31);
            this.comboBoxRecipe.TabIndex = 3;
            this.comboBoxRecipe.SelectedIndexChanged += new System.EventHandler(this.comboBoxRecipe_SelectedIndexChanged);
            // 
            // btnMoveToRecipePos
            // 
            this.btnMoveToRecipePos.Location = new System.Drawing.Point(20, 106);
            this.btnMoveToRecipePos.Margin = new System.Windows.Forms.Padding(4);
            this.btnMoveToRecipePos.Name = "btnMoveToRecipePos";
            this.btnMoveToRecipePos.Size = new System.Drawing.Size(227, 35);
            this.btnMoveToRecipePos.TabIndex = 4;
            this.btnMoveToRecipePos.Text = "Move to Recipe Pos";
            this.btnMoveToRecipePos.Click += new System.EventHandler(this.btnMoveToRecipePos_Click);
            // 
            // btnSaveCurrentPos
            // 
            this.btnSaveCurrentPos.BackColor = System.Drawing.Color.LightCoral;
            this.btnSaveCurrentPos.Location = new System.Drawing.Point(260, 106);
            this.btnSaveCurrentPos.Margin = new System.Windows.Forms.Padding(4);
            this.btnSaveCurrentPos.Name = "btnSaveCurrentPos";
            this.btnSaveCurrentPos.Size = new System.Drawing.Size(227, 35);
            this.btnSaveCurrentPos.TabIndex = 5;
            this.btnSaveCurrentPos.Text = "Save Current Pos";
            this.btnSaveCurrentPos.UseVisualStyleBackColor = false;
            this.btnSaveCurrentPos.Click += new System.EventHandler(this.btnSaveCurrentPos_Click);
            // 
            // DialogTuneLens
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1833, 1030);
            this.Controls.Add(this.rbRightLens);
            this.Controls.Add(this.rbLeftLens);
            this.Controls.Add(this.pictureBoxMeasure);
            this.Controls.Add(this.gbParameter);
            this.Controls.Add(this.skZoomAndPanWindowMeasure);
            this.Controls.Add(this.gbCameraSelect);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DialogTuneLens";
            this.Text = "DialogTuneLens";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DialogTuneLens_FormClosing);
            this.Load += new System.EventHandler(this.DialogTuneLens_Load);
            this.gbParameter.ResumeLayout(false);
            this.gbParameter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPatternWidth)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMeasure)).EndInit();
            this.gbCameraSelect.ResumeLayout(false);
            this.gbCameraSelect.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbCameraSelect;
        private System.Windows.Forms.RadioButton rbUpperCamera;
        private System.Windows.Forms.RadioButton rbLowerCamera;
        private System.Windows.Forms.ComboBox comboBoxRecipe;
        private System.Windows.Forms.Label lbRecipe;
        private System.Windows.Forms.Button btnMoveToRecipePos;
        private System.Windows.Forms.Button btnSaveCurrentPos;
        private MRLibrary.SKZoomAndPanWindow skZoomAndPanWindowMeasure;
        private System.Windows.Forms.RadioButton rbLeftLens;
        private System.Windows.Forms.RadioButton rbRightLens;
        private System.Windows.Forms.Label lbMoveTo;
        private System.Windows.Forms.ComboBox comboBoxGoto;
        private System.Windows.Forms.RadioButton rbLive;
        private System.Windows.Forms.RadioButton rbMeasure;
        private System.Windows.Forms.GroupBox gbParameter;
        private System.Windows.Forms.PictureBox pictureBoxMeasure;
        private System.Windows.Forms.Button btnCalAndSave;
        private System.Windows.Forms.NumericUpDown numericUpDownPatternWidth;
        private System.Windows.Forms.Label lbPatternWidth;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.NumericUpDown numericUpDownX10;
        private System.Windows.Forms.NumericUpDown numericUpDownX9;
        private System.Windows.Forms.NumericUpDown numericUpDownX8;
        private System.Windows.Forms.NumericUpDown numericUpDownX7;
        private System.Windows.Forms.NumericUpDown numericUpDownX6;
        private System.Windows.Forms.NumericUpDown numericUpDownX5;
        private System.Windows.Forms.NumericUpDown numericUpDownX4;
        private System.Windows.Forms.NumericUpDown numericUpDownX3;
        private System.Windows.Forms.NumericUpDown numericUpDownX2;
        private System.Windows.Forms.Label lbX10;
        private System.Windows.Forms.Label lbX1;
        private System.Windows.Forms.Label lbX2;
        private System.Windows.Forms.Label lbX3;
        private System.Windows.Forms.Label lbX4;
        private System.Windows.Forms.Label lbX5;
        private System.Windows.Forms.Label lbX6;
        private System.Windows.Forms.Label lbX7;
        private System.Windows.Forms.Label lbX8;
        private System.Windows.Forms.Label lbX9;
        private System.Windows.Forms.NumericUpDown numericUpDownX1;
        private System.Windows.Forms.Button btnPreCalculate;
        private System.Windows.Forms.Label lbX12;
        private System.Windows.Forms.Label lbX11;
        private System.Windows.Forms.NumericUpDown numericUpDownX11;
        private System.Windows.Forms.NumericUpDown numericUpDownX12;
    }
}