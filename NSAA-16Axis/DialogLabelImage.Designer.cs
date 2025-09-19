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
            this.ucLeftWaferNavigator = new NSAA_16Axis.UCNavigator();
            this.btnClear = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
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
            this.comboBoxClass.Location = new System.Drawing.Point(1339, 114);
            this.comboBoxClass.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxClass.Name = "comboBoxClass";
            this.comboBoxClass.Size = new System.Drawing.Size(139, 23);
            this.comboBoxClass.TabIndex = 61;
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(1339, 264);
            this.btnNext.Margin = new System.Windows.Forms.Padding(4);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(139, 56);
            this.btnNext.TabIndex = 60;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Location = new System.Drawing.Point(1339, 186);
            this.btnPrev.Margin = new System.Windows.Forms.Padding(4);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(139, 56);
            this.btnPrev.TabIndex = 59;
            this.btnPrev.Text = "Previous";
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(1339, 412);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(139, 56);
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
            this.btnOrganizeAndYaml.Location = new System.Drawing.Point(1339, 484);
            this.btnOrganizeAndYaml.Margin = new System.Windows.Forms.Padding(4);
            this.btnOrganizeAndYaml.Name = "btnOrganizeAndYaml";
            this.btnOrganizeAndYaml.Size = new System.Drawing.Size(139, 56);
            this.btnOrganizeAndYaml.TabIndex = 56;
            this.btnOrganizeAndYaml.Text = "OrganizeAndYaml";
            this.btnOrganizeAndYaml.UseVisualStyleBackColor = true;
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
            // ucLeftWaferNavigator
            // 
            this.ucLeftWaferNavigator.AutoSize = true;
            this.ucLeftWaferNavigator.Location = new System.Drawing.Point(1337, 559);
            this.ucLeftWaferNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.ucLeftWaferNavigator.Name = "ucLeftWaferNavigator";
            this.ucLeftWaferNavigator.Size = new System.Drawing.Size(143, 50);
            this.ucLeftWaferNavigator.TabIndex = 55;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(1339, 338);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(139, 56);
            this.btnClear.TabIndex = 63;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // DialogLabelImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1560, 929);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.comboBoxClass);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnOpenFolder);
            this.Controls.Add(this.btnOrganizeAndYaml);
            this.Controls.Add(this.ucLeftWaferNavigator);
            this.Controls.Add(this.skPattern);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DialogLabelImage";
            this.Text = "DialogLabelImage";
            this.Load += new System.EventHandler(this.DialogLabelImage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
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
    }
}