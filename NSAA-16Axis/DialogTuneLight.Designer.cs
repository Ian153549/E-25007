namespace NSAA_16Axis
{
    partial class DialogTuneLight
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
            this.trackBarLightLeft = new System.Windows.Forms.TrackBar();
            this.trackBarLightRight = new System.Windows.Forms.TrackBar();
            this.lbLeftLight = new System.Windows.Forms.Label();
            this.lbRightLight = new System.Windows.Forms.Label();
            this.btnRightLightSave = new System.Windows.Forms.Button();
            this.comboBoxRightMagnification = new System.Windows.Forms.ComboBox();
            this.cBLeftMagnification = new System.Windows.Forms.ComboBox();
            this.btnLeftLightSave = new System.Windows.Forms.Button();
            this.buttonLeftDecrease = new System.Windows.Forms.Button();
            this.buttonRightDecrease = new System.Windows.Forms.Button();
            this.buttonLeftIncrease = new System.Windows.Forms.Button();
            this.buttonRightIncrease = new System.Windows.Forms.Button();
            this.buttonRightBackIncrease = new System.Windows.Forms.Button();
            this.buttonLeftBackIncrease = new System.Windows.Forms.Button();
            this.buttonRightBackDecrease = new System.Windows.Forms.Button();
            this.buttonLeftBackDecrease = new System.Windows.Forms.Button();
            this.btnLeftBackLightSave = new System.Windows.Forms.Button();
            this.btnRightBackLightSave = new System.Windows.Forms.Button();
            this.lbRightBackLight = new System.Windows.Forms.Label();
            this.lbLeftBackLight = new System.Windows.Forms.Label();
            this.trackBarLightRightBack = new System.Windows.Forms.TrackBar();
            this.trackBarLightLeftBack = new System.Windows.Forms.TrackBar();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.cBLeftRingMagnification = new System.Windows.Forms.ComboBox();
            this.button5 = new System.Windows.Forms.Button();
            this.cBRightRingMagnification = new System.Windows.Forms.ComboBox();
            this.button6 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tBRightRingLight = new System.Windows.Forms.TrackBar();
            this.tBLeftRingLight = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLightLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLightRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLightRightBack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLightLeftBack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tBRightRingLight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tBLeftRingLight)).BeginInit();
            this.SuspendLayout();
            // 
            // trackBarLightLeft
            // 
            this.trackBarLightLeft.LargeChange = 1;
            this.trackBarLightLeft.Location = new System.Drawing.Point(191, 47);
            this.trackBarLightLeft.Maximum = 255;
            this.trackBarLightLeft.Name = "trackBarLightLeft";
            this.trackBarLightLeft.Size = new System.Drawing.Size(442, 45);
            this.trackBarLightLeft.TabIndex = 0;
            this.trackBarLightLeft.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trackBarLightChannel1_MouseUp);
            // 
            // trackBarLightRight
            // 
            this.trackBarLightRight.LargeChange = 1;
            this.trackBarLightRight.Location = new System.Drawing.Point(191, 136);
            this.trackBarLightRight.Maximum = 255;
            this.trackBarLightRight.Name = "trackBarLightRight";
            this.trackBarLightRight.Size = new System.Drawing.Size(442, 45);
            this.trackBarLightRight.TabIndex = 1;
            this.trackBarLightRight.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trackBarLightChannel2_MouseUp);
            // 
            // lbLeftLight
            // 
            this.lbLeftLight.AutoSize = true;
            this.lbLeftLight.Font = new System.Drawing.Font("Arial", 12F);
            this.lbLeftLight.Location = new System.Drawing.Point(12, 53);
            this.lbLeftLight.Name = "lbLeftLight";
            this.lbLeftLight.Size = new System.Drawing.Size(78, 18);
            this.lbLeftLight.TabIndex = 2;
            this.lbLeftLight.Text = "Left light : ";
            // 
            // lbRightLight
            // 
            this.lbRightLight.AutoSize = true;
            this.lbRightLight.Font = new System.Drawing.Font("Arial", 12F);
            this.lbRightLight.Location = new System.Drawing.Point(12, 143);
            this.lbRightLight.Name = "lbRightLight";
            this.lbRightLight.Size = new System.Drawing.Size(78, 18);
            this.lbRightLight.TabIndex = 3;
            this.lbRightLight.Text = "right light :";
            // 
            // btnRightLightSave
            // 
            this.btnRightLightSave.BackColor = System.Drawing.Color.LightCoral;
            this.btnRightLightSave.Font = new System.Drawing.Font("Arial", 12F);
            this.btnRightLightSave.Location = new System.Drawing.Point(786, 143);
            this.btnRightLightSave.Name = "btnRightLightSave";
            this.btnRightLightSave.Size = new System.Drawing.Size(88, 29);
            this.btnRightLightSave.TabIndex = 4;
            this.btnRightLightSave.Text = "SAVE";
            this.btnRightLightSave.UseVisualStyleBackColor = false;
            this.btnRightLightSave.Click += new System.EventHandler(this.ButtonRightLightSave_Click);
            // 
            // comboBoxRightMagnification
            // 
            this.comboBoxRightMagnification.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRightMagnification.Font = new System.Drawing.Font("Arial", 12F);
            this.comboBoxRightMagnification.FormattingEnabled = true;
            this.comboBoxRightMagnification.Items.AddRange(new object[] {
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
            this.comboBoxRightMagnification.Location = new System.Drawing.Point(702, 145);
            this.comboBoxRightMagnification.Name = "comboBoxRightMagnification";
            this.comboBoxRightMagnification.Size = new System.Drawing.Size(67, 26);
            this.comboBoxRightMagnification.TabIndex = 5;
            this.comboBoxRightMagnification.SelectedIndexChanged += new System.EventHandler(this.ComboBoxRightMagnification_SelectedIndexChanged);
            // 
            // cBLeftMagnification
            // 
            this.cBLeftMagnification.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBLeftMagnification.Font = new System.Drawing.Font("Arial", 12F);
            this.cBLeftMagnification.FormattingEnabled = true;
            this.cBLeftMagnification.Items.AddRange(new object[] {
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
            this.cBLeftMagnification.Location = new System.Drawing.Point(702, 55);
            this.cBLeftMagnification.Name = "cBLeftMagnification";
            this.cBLeftMagnification.Size = new System.Drawing.Size(67, 26);
            this.cBLeftMagnification.TabIndex = 7;
            this.cBLeftMagnification.SelectedIndexChanged += new System.EventHandler(this.ComboBoxLeftMagnification_SelectedIndexChanged);
            // 
            // btnLeftLightSave
            // 
            this.btnLeftLightSave.BackColor = System.Drawing.Color.LightCoral;
            this.btnLeftLightSave.Font = new System.Drawing.Font("Arial", 12F);
            this.btnLeftLightSave.Location = new System.Drawing.Point(786, 53);
            this.btnLeftLightSave.Name = "btnLeftLightSave";
            this.btnLeftLightSave.Size = new System.Drawing.Size(88, 29);
            this.btnLeftLightSave.TabIndex = 6;
            this.btnLeftLightSave.Text = "SAVE";
            this.btnLeftLightSave.UseVisualStyleBackColor = false;
            this.btnLeftLightSave.Click += new System.EventHandler(this.ButtonLeftLightSave_Click);
            // 
            // buttonLeftDecrease
            // 
            this.buttonLeftDecrease.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonLeftDecrease.Location = new System.Drawing.Point(142, 47);
            this.buttonLeftDecrease.Name = "buttonLeftDecrease";
            this.buttonLeftDecrease.Size = new System.Drawing.Size(38, 42);
            this.buttonLeftDecrease.TabIndex = 8;
            this.buttonLeftDecrease.Text = "-";
            this.buttonLeftDecrease.UseVisualStyleBackColor = true;
            this.buttonLeftDecrease.Click += new System.EventHandler(this.buttonLeftDecrease_Click);
            // 
            // buttonRightDecrease
            // 
            this.buttonRightDecrease.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRightDecrease.Location = new System.Drawing.Point(142, 136);
            this.buttonRightDecrease.Name = "buttonRightDecrease";
            this.buttonRightDecrease.Size = new System.Drawing.Size(38, 42);
            this.buttonRightDecrease.TabIndex = 9;
            this.buttonRightDecrease.Text = "-";
            this.buttonRightDecrease.UseVisualStyleBackColor = true;
            this.buttonRightDecrease.Click += new System.EventHandler(this.buttonRightDecrease_Click);
            // 
            // buttonLeftIncrease
            // 
            this.buttonLeftIncrease.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonLeftIncrease.Location = new System.Drawing.Point(648, 47);
            this.buttonLeftIncrease.Name = "buttonLeftIncrease";
            this.buttonLeftIncrease.Size = new System.Drawing.Size(38, 42);
            this.buttonLeftIncrease.TabIndex = 10;
            this.buttonLeftIncrease.Text = "+";
            this.buttonLeftIncrease.UseVisualStyleBackColor = true;
            this.buttonLeftIncrease.Click += new System.EventHandler(this.buttonLeftIncrease_Click);
            // 
            // buttonRightIncrease
            // 
            this.buttonRightIncrease.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRightIncrease.Location = new System.Drawing.Point(648, 136);
            this.buttonRightIncrease.Name = "buttonRightIncrease";
            this.buttonRightIncrease.Size = new System.Drawing.Size(38, 42);
            this.buttonRightIncrease.TabIndex = 11;
            this.buttonRightIncrease.Text = "+";
            this.buttonRightIncrease.UseVisualStyleBackColor = true;
            this.buttonRightIncrease.Click += new System.EventHandler(this.buttonRightIncrease_Click);
            // 
            // buttonRightBackIncrease
            // 
            this.buttonRightBackIncrease.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRightBackIncrease.Location = new System.Drawing.Point(648, 480);
            this.buttonRightBackIncrease.Name = "buttonRightBackIncrease";
            this.buttonRightBackIncrease.Size = new System.Drawing.Size(38, 42);
            this.buttonRightBackIncrease.TabIndex = 23;
            this.buttonRightBackIncrease.Text = "+";
            this.buttonRightBackIncrease.UseVisualStyleBackColor = true;
            // 
            // buttonLeftBackIncrease
            // 
            this.buttonLeftBackIncrease.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonLeftBackIncrease.Location = new System.Drawing.Point(648, 385);
            this.buttonLeftBackIncrease.Name = "buttonLeftBackIncrease";
            this.buttonLeftBackIncrease.Size = new System.Drawing.Size(38, 42);
            this.buttonLeftBackIncrease.TabIndex = 22;
            this.buttonLeftBackIncrease.Text = "+";
            this.buttonLeftBackIncrease.UseVisualStyleBackColor = true;
            // 
            // buttonRightBackDecrease
            // 
            this.buttonRightBackDecrease.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRightBackDecrease.Location = new System.Drawing.Point(142, 480);
            this.buttonRightBackDecrease.Name = "buttonRightBackDecrease";
            this.buttonRightBackDecrease.Size = new System.Drawing.Size(38, 42);
            this.buttonRightBackDecrease.TabIndex = 21;
            this.buttonRightBackDecrease.Text = "-";
            this.buttonRightBackDecrease.UseVisualStyleBackColor = true;
            // 
            // buttonLeftBackDecrease
            // 
            this.buttonLeftBackDecrease.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonLeftBackDecrease.Location = new System.Drawing.Point(142, 385);
            this.buttonLeftBackDecrease.Name = "buttonLeftBackDecrease";
            this.buttonLeftBackDecrease.Size = new System.Drawing.Size(38, 42);
            this.buttonLeftBackDecrease.TabIndex = 20;
            this.buttonLeftBackDecrease.Text = "-";
            this.buttonLeftBackDecrease.UseVisualStyleBackColor = true;
            // 
            // btnLeftBackLightSave
            // 
            this.btnLeftBackLightSave.BackColor = System.Drawing.Color.LightCoral;
            this.btnLeftBackLightSave.Font = new System.Drawing.Font("Arial", 12F);
            this.btnLeftBackLightSave.Location = new System.Drawing.Point(786, 391);
            this.btnLeftBackLightSave.Name = "btnLeftBackLightSave";
            this.btnLeftBackLightSave.Size = new System.Drawing.Size(88, 29);
            this.btnLeftBackLightSave.TabIndex = 18;
            this.btnLeftBackLightSave.Text = "SAVE";
            this.btnLeftBackLightSave.UseVisualStyleBackColor = false;
            this.btnLeftBackLightSave.Click += new System.EventHandler(this.buttonLeftBackLightSave_Click);
            // 
            // btnRightBackLightSave
            // 
            this.btnRightBackLightSave.BackColor = System.Drawing.Color.LightCoral;
            this.btnRightBackLightSave.Font = new System.Drawing.Font("Arial", 12F);
            this.btnRightBackLightSave.Location = new System.Drawing.Point(786, 487);
            this.btnRightBackLightSave.Name = "btnRightBackLightSave";
            this.btnRightBackLightSave.Size = new System.Drawing.Size(88, 29);
            this.btnRightBackLightSave.TabIndex = 16;
            this.btnRightBackLightSave.Text = "SAVE";
            this.btnRightBackLightSave.UseVisualStyleBackColor = false;
            this.btnRightBackLightSave.Click += new System.EventHandler(this.buttonRightBackLightSave_Click);
            // 
            // lbRightBackLight
            // 
            this.lbRightBackLight.AutoSize = true;
            this.lbRightBackLight.Font = new System.Drawing.Font("Arial", 12F);
            this.lbRightBackLight.Location = new System.Drawing.Point(12, 487);
            this.lbRightBackLight.Name = "lbRightBackLight";
            this.lbRightBackLight.Size = new System.Drawing.Size(95, 18);
            this.lbRightBackLight.TabIndex = 15;
            this.lbRightBackLight.Text = "RBack light :";
            // 
            // lbLeftBackLight
            // 
            this.lbLeftBackLight.AutoSize = true;
            this.lbLeftBackLight.Font = new System.Drawing.Font("Arial", 12F);
            this.lbLeftBackLight.Location = new System.Drawing.Point(12, 391);
            this.lbLeftBackLight.Name = "lbLeftBackLight";
            this.lbLeftBackLight.Size = new System.Drawing.Size(97, 18);
            this.lbLeftBackLight.TabIndex = 14;
            this.lbLeftBackLight.Text = "LBack light : ";
            // 
            // trackBarLightRightBack
            // 
            this.trackBarLightRightBack.LargeChange = 1;
            this.trackBarLightRightBack.Location = new System.Drawing.Point(191, 487);
            this.trackBarLightRightBack.Maximum = 255;
            this.trackBarLightRightBack.Name = "trackBarLightRightBack";
            this.trackBarLightRightBack.Size = new System.Drawing.Size(442, 45);
            this.trackBarLightRightBack.TabIndex = 13;
            this.trackBarLightRightBack.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trackBarLightChannel4_MouseUp);
            // 
            // trackBarLightLeftBack
            // 
            this.trackBarLightLeftBack.LargeChange = 1;
            this.trackBarLightLeftBack.Location = new System.Drawing.Point(191, 391);
            this.trackBarLightLeftBack.Maximum = 255;
            this.trackBarLightLeftBack.Name = "trackBarLightLeftBack";
            this.trackBarLightLeftBack.Size = new System.Drawing.Size(442, 45);
            this.trackBarLightLeftBack.TabIndex = 12;
            this.trackBarLightLeftBack.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trackBarLightChannel3_MouseUp);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(648, 292);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(38, 42);
            this.button1.TabIndex = 35;
            this.button1.Text = "+";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(648, 214);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(38, 42);
            this.button2.TabIndex = 34;
            this.button2.Text = "+";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(142, 292);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(38, 42);
            this.button3.TabIndex = 33;
            this.button3.Text = "-";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.Location = new System.Drawing.Point(142, 214);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(38, 42);
            this.button4.TabIndex = 32;
            this.button4.Text = "-";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // cBLeftRingMagnification
            // 
            this.cBLeftRingMagnification.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBLeftRingMagnification.Font = new System.Drawing.Font("Arial", 12F);
            this.cBLeftRingMagnification.FormattingEnabled = true;
            this.cBLeftRingMagnification.Items.AddRange(new object[] {
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
            this.cBLeftRingMagnification.Location = new System.Drawing.Point(702, 222);
            this.cBLeftRingMagnification.Name = "cBLeftRingMagnification";
            this.cBLeftRingMagnification.Size = new System.Drawing.Size(67, 26);
            this.cBLeftRingMagnification.TabIndex = 31;
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.LightCoral;
            this.button5.Font = new System.Drawing.Font("Arial", 12F);
            this.button5.Location = new System.Drawing.Point(786, 220);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(88, 29);
            this.button5.TabIndex = 30;
            this.button5.Text = "SAVE";
            this.button5.UseVisualStyleBackColor = false;
            // 
            // cBRightRingMagnification
            // 
            this.cBRightRingMagnification.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cBRightRingMagnification.Font = new System.Drawing.Font("Arial", 12F);
            this.cBRightRingMagnification.FormattingEnabled = true;
            this.cBRightRingMagnification.Items.AddRange(new object[] {
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
            this.cBRightRingMagnification.Location = new System.Drawing.Point(702, 301);
            this.cBRightRingMagnification.Name = "cBRightRingMagnification";
            this.cBRightRingMagnification.Size = new System.Drawing.Size(67, 26);
            this.cBRightRingMagnification.TabIndex = 29;
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.LightCoral;
            this.button6.Font = new System.Drawing.Font("Arial", 12F);
            this.button6.Location = new System.Drawing.Point(786, 299);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(88, 29);
            this.button6.TabIndex = 28;
            this.button6.Text = "SAVE";
            this.button6.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 12F);
            this.label1.Location = new System.Drawing.Point(12, 305);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 18);
            this.label1.TabIndex = 27;
            this.label1.Text = "Right Ring Light :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 12F);
            this.label2.Location = new System.Drawing.Point(12, 229);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 18);
            this.label2.TabIndex = 26;
            this.label2.Text = "Left Ring Light : ";
            // 
            // tBRightRingLight
            // 
            this.tBRightRingLight.LargeChange = 1;
            this.tBRightRingLight.Location = new System.Drawing.Point(191, 299);
            this.tBRightRingLight.Maximum = 255;
            this.tBRightRingLight.Name = "tBRightRingLight";
            this.tBRightRingLight.Size = new System.Drawing.Size(442, 45);
            this.tBRightRingLight.TabIndex = 25;
            this.tBRightRingLight.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tBRightRingLight_MouseUp);
            // 
            // tBLeftRingLight
            // 
            this.tBLeftRingLight.LargeChange = 1;
            this.tBLeftRingLight.Location = new System.Drawing.Point(191, 220);
            this.tBLeftRingLight.Maximum = 255;
            this.tBLeftRingLight.Name = "tBLeftRingLight";
            this.tBLeftRingLight.Size = new System.Drawing.Size(442, 45);
            this.tBLeftRingLight.TabIndex = 24;
            this.tBLeftRingLight.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tBLeftRingLight_MouseUp);
            // 
            // DialogTuneLight
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(913, 565);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.cBLeftRingMagnification);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.cBRightRingMagnification);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tBRightRingLight);
            this.Controls.Add(this.tBLeftRingLight);
            this.Controls.Add(this.buttonRightBackIncrease);
            this.Controls.Add(this.buttonLeftBackIncrease);
            this.Controls.Add(this.buttonRightBackDecrease);
            this.Controls.Add(this.buttonLeftBackDecrease);
            this.Controls.Add(this.btnLeftBackLightSave);
            this.Controls.Add(this.btnRightBackLightSave);
            this.Controls.Add(this.lbRightBackLight);
            this.Controls.Add(this.lbLeftBackLight);
            this.Controls.Add(this.trackBarLightRightBack);
            this.Controls.Add(this.trackBarLightLeftBack);
            this.Controls.Add(this.buttonRightIncrease);
            this.Controls.Add(this.buttonLeftIncrease);
            this.Controls.Add(this.buttonRightDecrease);
            this.Controls.Add(this.buttonLeftDecrease);
            this.Controls.Add(this.cBLeftMagnification);
            this.Controls.Add(this.btnLeftLightSave);
            this.Controls.Add(this.comboBoxRightMagnification);
            this.Controls.Add(this.btnRightLightSave);
            this.Controls.Add(this.lbRightLight);
            this.Controls.Add(this.lbLeftLight);
            this.Controls.Add(this.trackBarLightRight);
            this.Controls.Add(this.trackBarLightLeft);
            this.Name = "DialogTuneLight";
            this.Text = "DialogTuneLight";
            this.Load += new System.EventHandler(this.DialogTuneLight_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLightLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLightRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLightRightBack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLightLeftBack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tBRightRingLight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tBLeftRingLight)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TrackBar trackBarLightRight;
        private System.Windows.Forms.Label lbLeftLight;
        private System.Windows.Forms.Label lbRightLight;
        private System.Windows.Forms.Button btnRightLightSave;
        private System.Windows.Forms.ComboBox comboBoxRightMagnification;
        private System.Windows.Forms.ComboBox cBLeftMagnification;
        private System.Windows.Forms.Button btnLeftLightSave;
        private System.Windows.Forms.Button buttonLeftDecrease;
        private System.Windows.Forms.Button buttonRightDecrease;
        private System.Windows.Forms.Button buttonLeftIncrease;
        private System.Windows.Forms.Button buttonRightIncrease;
        private System.Windows.Forms.TrackBar trackBarLightLeft;
        private System.Windows.Forms.Button buttonRightBackIncrease;
        private System.Windows.Forms.Button buttonLeftBackIncrease;
        private System.Windows.Forms.Button buttonRightBackDecrease;
        private System.Windows.Forms.Button buttonLeftBackDecrease;
        private System.Windows.Forms.Button btnLeftBackLightSave;
        private System.Windows.Forms.Button btnRightBackLightSave;
        private System.Windows.Forms.Label lbRightBackLight;
        private System.Windows.Forms.Label lbLeftBackLight;
        private System.Windows.Forms.TrackBar trackBarLightRightBack;
        private System.Windows.Forms.TrackBar trackBarLightLeftBack;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.ComboBox cBLeftRingMagnification;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.ComboBox cBRightRingMagnification;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar tBRightRingLight;
        private System.Windows.Forms.TrackBar tBLeftRingLight;
    }
}