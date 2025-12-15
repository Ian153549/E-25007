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
            this.lb = new System.Windows.Forms.Label();
            this.tbNewLabel = new System.Windows.Forms.TextBox();
            this.btAdd = new System.Windows.Forms.Button();
            this.btDelete = new System.Windows.Forms.Button();
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
            this.comboBoxClass.Location = new System.Drawing.Point(1339, 292);
            this.comboBoxClass.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxClass.Name = "comboBoxClass";
            this.comboBoxClass.Size = new System.Drawing.Size(139, 23);
            this.comboBoxClass.TabIndex = 61;
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(1337, 412);
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
            this.btnPrev.Location = new System.Drawing.Point(1337, 334);
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
            this.btnSave.Location = new System.Drawing.Point(1337, 560);
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
            this.btnOrganizeAndYaml.Location = new System.Drawing.Point(1337, 632);
            this.btnOrganizeAndYaml.Margin = new System.Windows.Forms.Padding(4);
            this.btnOrganizeAndYaml.Name = "btnOrganizeAndYaml";
            this.btnOrganizeAndYaml.Size = new System.Drawing.Size(139, 56);
            this.btnOrganizeAndYaml.TabIndex = 56;
            this.btnOrganizeAndYaml.Text = "OrganizeAndYaml";
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
            // ucLeftWaferNavigator
            // 
            this.ucLeftWaferNavigator.AutoSize = true;
            this.ucLeftWaferNavigator.Location = new System.Drawing.Point(1335, 707);
            this.ucLeftWaferNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.ucLeftWaferNavigator.Name = "ucLeftWaferNavigator";
            this.ucLeftWaferNavigator.Size = new System.Drawing.Size(143, 50);
            this.ucLeftWaferNavigator.TabIndex = 55;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(1337, 486);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(139, 56);
            this.btnClear.TabIndex = 63;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // lb
            // 
            this.lb.AutoSize = true;
            this.lb.Location = new System.Drawing.Point(1344, 120);
            this.lb.Name = "lb";
            this.lb.Size = new System.Drawing.Size(69, 15);
            this.lb.TabIndex = 64;
            this.lb.Text = "New Label";
            // 
            // tbNewLabel
            // 
            this.tbNewLabel.Location = new System.Drawing.Point(1339, 149);
            this.tbNewLabel.Name = "tbNewLabel";
            this.tbNewLabel.Size = new System.Drawing.Size(100, 25);
            this.tbNewLabel.TabIndex = 65;
            // 
            // btAdd
            // 
            this.btAdd.Location = new System.Drawing.Point(1445, 144);
            this.btAdd.Name = "btAdd";
            this.btAdd.Size = new System.Drawing.Size(95, 31);
            this.btAdd.TabIndex = 66;
            this.btAdd.Text = "Add";
            this.btAdd.UseVisualStyleBackColor = true;
            this.btAdd.Click += new System.EventHandler(this.btAdd_Click);
            // 
            // btDelete
            // 
            this.btDelete.Location = new System.Drawing.Point(1447, 201);
            this.btDelete.Name = "btDelete";
            this.btDelete.Size = new System.Drawing.Size(93, 38);
            this.btDelete.TabIndex = 67;
            this.btDelete.Text = "delete";
            this.btDelete.UseVisualStyleBackColor = true;
            this.btDelete.Click += new System.EventHandler(this.btDelete_Click);
            // 
            // DialogLabelImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1560, 929);
            this.Controls.Add(this.btDelete);
            this.Controls.Add(this.btAdd);
            this.Controls.Add(this.tbNewLabel);
            this.Controls.Add(this.lb);
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
        private System.Windows.Forms.Label lb;
        private System.Windows.Forms.TextBox tbNewLabel;
        private System.Windows.Forms.Button btAdd;
        private System.Windows.Forms.Button btDelete;
    }
}