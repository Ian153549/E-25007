namespace NSAA_16Axis
{
    partial class UCPattern
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

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.gbPattern = new System.Windows.Forms.GroupBox();
            this.rbLight = new System.Windows.Forms.RadioButton();
            this.ucNavigator1 = new NSAA_16Axis.UCNavigator();
            this.tbLight = new System.Windows.Forms.TrackBar();
            this.btCreateMask = new System.Windows.Forms.Button();
            this.btSave = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.pbPattern = new System.Windows.Forms.PictureBox();
            this.gbPattern.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbLight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPattern)).BeginInit();
            this.SuspendLayout();
            // 
            // gbPattern
            // 
            this.gbPattern.BackColor = System.Drawing.Color.Beige;
            this.gbPattern.Controls.Add(this.rbLight);
            this.gbPattern.Controls.Add(this.ucNavigator1);
            this.gbPattern.Controls.Add(this.tbLight);
            this.gbPattern.Controls.Add(this.btCreateMask);
            this.gbPattern.Controls.Add(this.pbPattern);
            this.gbPattern.Controls.Add(this.btSave);
            this.gbPattern.Controls.Add(this.comboBox1);
            this.gbPattern.Controls.Add(this.label5);
            this.gbPattern.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gbPattern.Font = new System.Drawing.Font("Cambria", 15.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbPattern.ForeColor = System.Drawing.Color.Green;
            this.gbPattern.Location = new System.Drawing.Point(3, 3);
            this.gbPattern.Name = "gbPattern";
            this.gbPattern.Size = new System.Drawing.Size(225, 320);
            this.gbPattern.TabIndex = 13;
            this.gbPattern.TabStop = false;
            this.gbPattern.Text = "TopMask";
            // 
            // rbLight
            // 
            this.rbLight.AutoSize = true;
            this.rbLight.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbLight.Location = new System.Drawing.Point(159, 26);
            this.rbLight.Name = "rbLight";
            this.rbLight.Size = new System.Drawing.Size(60, 22);
            this.rbLight.TabIndex = 50;
            this.rbLight.TabStop = true;
            this.rbLight.Text = "Light";
            this.rbLight.UseVisualStyleBackColor = true;
            // 
            // ucNavigator1
            // 
            this.ucNavigator1.AutoSize = true;
            this.ucNavigator1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ucNavigator1.Location = new System.Drawing.Point(33, 29);
            this.ucNavigator1.Margin = new System.Windows.Forms.Padding(6);
            this.ucNavigator1.Name = "ucNavigator1";
            this.ucNavigator1.Size = new System.Drawing.Size(107, 40);
            this.ucNavigator1.TabIndex = 14;
            // 
            // tbLight
            // 
            this.tbLight.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbLight.LargeChange = 1;
            this.tbLight.Location = new System.Drawing.Point(172, 49);
            this.tbLight.Maximum = 255;
            this.tbLight.Name = "tbLight";
            this.tbLight.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbLight.Size = new System.Drawing.Size(45, 261);
            this.tbLight.TabIndex = 49;
            this.tbLight.Visible = false;
            // 
            // btCreateMask
            // 
            this.btCreateMask.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.btCreateMask.Font = new System.Drawing.Font("Arial", 12F);
            this.btCreateMask.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btCreateMask.Location = new System.Drawing.Point(33, 148);
            this.btCreateMask.Name = "btCreateMask";
            this.btCreateMask.Size = new System.Drawing.Size(116, 30);
            this.btCreateMask.TabIndex = 9;
            this.btCreateMask.Text = "Create mask";
            this.btCreateMask.UseVisualStyleBackColor = false;
            // 
            // btSave
            // 
            this.btSave.BackColor = System.Drawing.Color.LightCoral;
            this.btSave.Font = new System.Drawing.Font("Arial", 12F);
            this.btSave.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btSave.Location = new System.Drawing.Point(33, 112);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(116, 30);
            this.btSave.TabIndex = 6;
            this.btSave.Text = "SAVE";
            this.btSave.UseVisualStyleBackColor = false;
            this.btSave.Click += new System.EventHandler(this.button2_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBox1.Font = new System.Drawing.Font("Arial", 12F);
            this.comboBox1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Template",
            "Edge"});
            this.comboBox1.Location = new System.Drawing.Point(69, 78);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(100, 26);
            this.comboBox1.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label5.Location = new System.Drawing.Point(13, 82);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 18);
            this.label5.TabIndex = 0;
            this.label5.Text = "ALGO.";
            // 
            // pbPattern
            // 
            this.pbPattern.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbPattern.Location = new System.Drawing.Point(13, 184);
            this.pbPattern.Name = "pbPattern";
            this.pbPattern.Size = new System.Drawing.Size(153, 126);
            this.pbPattern.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbPattern.TabIndex = 7;
            this.pbPattern.TabStop = false;
            // 
            // UCPattern
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbPattern);
            this.Name = "UCPattern";
            this.Size = new System.Drawing.Size(228, 324);
            this.gbPattern.ResumeLayout(false);
            this.gbPattern.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbLight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPattern)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbPattern;
        private System.Windows.Forms.RadioButton rbLight;
        private UCNavigator ucNavigator1;
        private System.Windows.Forms.TrackBar tbLight;
        private System.Windows.Forms.Button btCreateMask;
        private System.Windows.Forms.PictureBox pbPattern;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label5;
    }
}
