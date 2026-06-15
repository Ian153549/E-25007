namespace NSAA_16Axis
{
    partial class DialogLabelImage
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
        public void InitializeComponent()
        {
            this.skPattern = new MRLibrary.SKZoomAndPanWindow();
            this.comboBoxClass = new System.Windows.Forms.ComboBox();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnOpenFolder = new System.Windows.Forms.Button();
            this.btnOrganizeAndYaml = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.lb = new System.Windows.Forms.Label();
            this.tbNewLabel = new System.Windows.Forms.TextBox();
            this.btAdd = new System.Windows.Forms.Button();
            this.btDelete = new System.Windows.Forms.Button();
            this.btTrain = new System.Windows.Forms.Button();
            this.ucLeftWaferNavigator = new NSAA_16Axis.UCNavigator();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btDeleteImage = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cbLabelType = new System.Windows.Forms.ComboBox();
            this.lblPadding = new System.Windows.Forms.Label();
            this.nudPadding = new System.Windows.Forms.NumericUpDown();
            this.cbPaddingDir = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lvInfo = new System.Windows.Forms.ListView();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPadding)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // skPattern
            // 
            this.skPattern.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.skPattern.Location = new System.Drawing.Point(20, 21);
            this.skPattern.Margin = new System.Windows.Forms.Padding(8, 10, 8, 10);
            this.skPattern.Name = "skPattern";
            this.skPattern.Size = new System.Drawing.Size(1259, 885);
            this.skPattern.TabIndex = 33;
            this.skPattern.Paint += new System.Windows.Forms.PaintEventHandler(this.skPattern_Paint);
            this.skPattern.MouseDown += new System.Windows.Forms.MouseEventHandler(this.skPattern_MouseDown);
            this.skPattern.MouseMove += new System.Windows.Forms.MouseEventHandler(this.skPattern_MouseMove);
            this.skPattern.MouseUp += new System.Windows.Forms.MouseEventHandler(this.skPattern_MouseUp);
            // 
            // comboBoxClass
            // 
            this.comboBoxClass.FormattingEnabled = true;
            this.comboBoxClass.Location = new System.Drawing.Point(9, 116);
            this.comboBoxClass.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxClass.Name = "comboBoxClass";
            this.comboBoxClass.Size = new System.Drawing.Size(139, 23);
            this.comboBoxClass.TabIndex = 61;
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(130, 25);
            this.btnNext.Margin = new System.Windows.Forms.Padding(4);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(94, 56);
            this.btnNext.TabIndex = 60;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(9, 25);
            this.btnPrev.Margin = new System.Windows.Forms.Padding(4);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(94, 56);
            this.btnPrev.TabIndex = 59;
            this.btnPrev.Text = "Previous";
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(9, 163);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(94, 56);
            this.btnSave.TabIndex = 58;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnOpenFolder
            // 
            this.btnOpenFolder.Location = new System.Drawing.Point(1339, 21);
            this.btnOpenFolder.Margin = new System.Windows.Forms.Padding(4);
            this.btnOpenFolder.Name = "btnOpenFolder";
            this.btnOpenFolder.Size = new System.Drawing.Size(139, 56);
            this.btnOpenFolder.TabIndex = 57;
            this.btnOpenFolder.Text = "OpenFolder";
            this.btnOpenFolder.UseVisualStyleBackColor = true;
            this.btnOpenFolder.Visible = false;
            this.btnOpenFolder.Click += new System.EventHandler(this.btnOpenFolder_Click);
            // 
            // btnOrganizeAndYaml
            // 
            this.btnOrganizeAndYaml.Location = new System.Drawing.Point(127, 163);
            this.btnOrganizeAndYaml.Margin = new System.Windows.Forms.Padding(4);
            this.btnOrganizeAndYaml.Name = "btnOrganizeAndYaml";
            this.btnOrganizeAndYaml.Size = new System.Drawing.Size(97, 56);
            this.btnOrganizeAndYaml.TabIndex = 56;
            this.btnOrganizeAndYaml.Text = "General Yaml";
            this.btnOrganizeAndYaml.UseVisualStyleBackColor = true;
            this.btnOrganizeAndYaml.Click += new System.EventHandler(this.btnOrganizeAndYaml_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(20, 21);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1259, 885);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 62;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(9, 89);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(94, 56);
            this.btnClear.TabIndex = 63;
            this.btnClear.Text = "Clear Label";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // lb
            // 
            this.lb.AutoSize = true;
            this.lb.Location = new System.Drawing.Point(6, 34);
            this.lb.Name = "lb";
            this.lb.Size = new System.Drawing.Size(69, 15);
            this.lb.TabIndex = 64;
            this.lb.Text = "New Label";
            // 
            // tbNewLabel
            // 
            this.tbNewLabel.Location = new System.Drawing.Point(81, 24);
            this.tbNewLabel.Name = "tbNewLabel";
            this.tbNewLabel.Size = new System.Drawing.Size(100, 25);
            this.tbNewLabel.TabIndex = 65;
            // 
            // btAdd
            // 
            this.btAdd.Location = new System.Drawing.Point(6, 69);
            this.btAdd.Name = "btAdd";
            this.btAdd.Size = new System.Drawing.Size(95, 31);
            this.btAdd.TabIndex = 66;
            this.btAdd.Text = "Add";
            this.btAdd.UseVisualStyleBackColor = true;
            this.btAdd.Click += new System.EventHandler(this.btAdd_Click);
            // 
            // btDelete
            // 
            this.btDelete.Location = new System.Drawing.Point(107, 69);
            this.btDelete.Name = "btDelete";
            this.btDelete.Size = new System.Drawing.Size(93, 31);
            this.btDelete.TabIndex = 67;
            this.btDelete.Text = "Delete";
            this.btDelete.UseVisualStyleBackColor = true;
            this.btDelete.Click += new System.EventHandler(this.btDelete_Click);
            // 
            // btTrain
            // 
            this.btTrain.Location = new System.Drawing.Point(1417, 868);
            this.btTrain.Name = "btTrain";
            this.btTrain.Size = new System.Drawing.Size(130, 49);
            this.btTrain.TabIndex = 68;
            this.btTrain.Text = "Train Model";
            this.btTrain.UseVisualStyleBackColor = true;
            this.btTrain.Visible = false;
            this.btTrain.Click += new System.EventHandler(this.btTrain_Click);
            // 
            // ucLeftWaferNavigator
            // 
            this.ucLeftWaferNavigator.AutoSize = true;
            this.ucLeftWaferNavigator.Location = new System.Drawing.Point(1339, 876);
            this.ucLeftWaferNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.ucLeftWaferNavigator.Name = "ucLeftWaferNavigator";
            this.ucLeftWaferNavigator.Size = new System.Drawing.Size(143, 50);
            this.ucLeftWaferNavigator.TabIndex = 55;
            this.ucLeftWaferNavigator.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbNewLabel);
            this.groupBox1.Controls.Add(this.lb);
            this.groupBox1.Controls.Add(this.btDelete);
            this.groupBox1.Controls.Add(this.btAdd);
            this.groupBox1.Controls.Add(this.comboBoxClass);
            this.groupBox1.Location = new System.Drawing.Point(1290, 84);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(248, 163);
            this.groupBox1.TabIndex = 69;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Label";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btDeleteImage);
            this.groupBox2.Controls.Add(this.btnPrev);
            this.groupBox2.Controls.Add(this.btnNext);
            this.groupBox2.Controls.Add(this.btnClear);
            this.groupBox2.Controls.Add(this.btnSave);
            this.groupBox2.Controls.Add(this.btnOrganizeAndYaml);
            this.groupBox2.Location = new System.Drawing.Point(1290, 257);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(248, 245);
            this.groupBox2.TabIndex = 70;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Image";
            // 
            // btDeleteImage
            // 
            this.btDeleteImage.Location = new System.Drawing.Point(130, 89);
            this.btDeleteImage.Margin = new System.Windows.Forms.Padding(4);
            this.btDeleteImage.Name = "btDeleteImage";
            this.btDeleteImage.Size = new System.Drawing.Size(94, 56);
            this.btDeleteImage.TabIndex = 64;
            this.btDeleteImage.Text = "Delete Image";
            this.btDeleteImage.UseVisualStyleBackColor = true;
            this.btDeleteImage.Click += new System.EventHandler(this.btDeleteImage_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbLabelType);
            this.groupBox3.Controls.Add(this.lblPadding);
            this.groupBox3.Controls.Add(this.nudPadding);
            this.groupBox3.Controls.Add(this.cbPaddingDir);
            this.groupBox3.Location = new System.Drawing.Point(1290, 520);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(247, 135);
            this.groupBox3.TabIndex = 71;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "LabelType";
            // 
            // cbLabelType
            // 
            this.cbLabelType.FormattingEnabled = true;
            this.cbLabelType.Items.AddRange(new object[] {
            "正方形",
            "方形"});
            this.cbLabelType.Location = new System.Drawing.Point(9, 36);
            this.cbLabelType.Name = "cbLabelType";
            this.cbLabelType.Size = new System.Drawing.Size(121, 23);
            this.cbLabelType.TabIndex = 0;
            // 
            // lblPadding
            // 
            this.lblPadding.AutoSize = true;
            this.lblPadding.Location = new System.Drawing.Point(9, 62);
            this.lblPadding.Name = "lblPadding";
            this.lblPadding.Size = new System.Drawing.Size(57, 15);
            this.lblPadding.TabIndex = 1;
            this.lblPadding.Text = "Padding:";
            // 
            // nudPadding
            // 
            this.nudPadding.Location = new System.Drawing.Point(9, 80);
            this.nudPadding.Name = "nudPadding";
            this.nudPadding.Size = new System.Drawing.Size(60, 25);
            this.nudPadding.TabIndex = 2;
            // 
            // cbPaddingDir
            // 
            this.cbPaddingDir.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPaddingDir.FormattingEnabled = true;
            this.cbPaddingDir.Items.AddRange(new object[] {
            "外擴",
            "內縮"});
            this.cbPaddingDir.Location = new System.Drawing.Point(78, 80);
            this.cbPaddingDir.Name = "cbPaddingDir";
            this.cbPaddingDir.Size = new System.Drawing.Size(60, 23);
            this.cbPaddingDir.TabIndex = 3;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lvInfo);
            this.groupBox4.Location = new System.Drawing.Point(1292, 661);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(254, 201);
            this.groupBox4.TabIndex = 72;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Info";
            // 
            // lvInfo
            // 
            this.lvInfo.HideSelection = false;
            this.lvInfo.Location = new System.Drawing.Point(10, 24);
            this.lvInfo.Name = "lvInfo";
            this.lvInfo.Size = new System.Drawing.Size(238, 171);
            this.lvInfo.TabIndex = 0;
            this.lvInfo.UseCompatibleStateImageBehavior = false;
            // 
            // DialogLabelImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1560, 929);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btTrain);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnOpenFolder);
            this.Controls.Add(this.ucLeftWaferNavigator);
            this.Controls.Add(this.skPattern);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DialogLabelImage";
            this.Text = "DialogLabelImage";
            this.Load += new System.EventHandler(this.DialogLabelImage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPadding)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public MRLibrary.SKZoomAndPanWindow skPattern;
        private UCNavigator ucLeftWaferNavigator;
        private System.Windows.Forms.ComboBox comboBoxClass;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnOpenFolder;
        private System.Windows.Forms.Button btnOrganizeAndYaml;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label lb;
        private System.Windows.Forms.TextBox tbNewLabel;
        private System.Windows.Forms.Button btAdd;
        private System.Windows.Forms.Button btDelete;
        private System.Windows.Forms.Button btTrain;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btDeleteImage;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cbLabelType;
        private System.Windows.Forms.Label lblPadding;
        private System.Windows.Forms.NumericUpDown nudPadding;
        private System.Windows.Forms.ComboBox cbPaddingDir;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ListView lvInfo;
    }
}