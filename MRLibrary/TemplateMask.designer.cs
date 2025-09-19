namespace TemplatesMask
{
    partial class TemplateMask
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.MaskTypeBox = new System.Windows.Forms.GroupBox();
            this.NoneButton = new System.Windows.Forms.RadioButton();
            this.RectangleButton = new System.Windows.Forms.RadioButton();
            this.CornerButton = new System.Windows.Forms.RadioButton();
            this.CrossButton = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.DownButton = new System.Windows.Forms.Button();
            this.LeftButton = new System.Windows.Forms.Button();
            this.UpButton = new System.Windows.Forms.Button();
            this.RightButton = new System.Windows.Forms.Button();
            this.CreateButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.MaskTypeBox.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(16, 15);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(400, 400);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // MaskTypeBox
            // 
            this.MaskTypeBox.AccessibleRole = System.Windows.Forms.AccessibleRole.SplitButton;
            this.MaskTypeBox.Controls.Add(this.NoneButton);
            this.MaskTypeBox.Controls.Add(this.RectangleButton);
            this.MaskTypeBox.Controls.Add(this.CornerButton);
            this.MaskTypeBox.Controls.Add(this.CrossButton);
            this.MaskTypeBox.Enabled = false;
            this.MaskTypeBox.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaskTypeBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.MaskTypeBox.Location = new System.Drawing.Point(436, 9);
            this.MaskTypeBox.Name = "MaskTypeBox";
            this.MaskTypeBox.Size = new System.Drawing.Size(221, 196);
            this.MaskTypeBox.TabIndex = 1;
            this.MaskTypeBox.TabStop = false;
            this.MaskTypeBox.Text = "Mark Type";
            // 
            // NoneButton
            // 
            this.NoneButton.AutoSize = true;
            this.NoneButton.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NoneButton.ForeColor = System.Drawing.Color.Black;
            this.NoneButton.Location = new System.Drawing.Point(6, 35);
            this.NoneButton.Name = "NoneButton";
            this.NoneButton.Size = new System.Drawing.Size(77, 28);
            this.NoneButton.TabIndex = 4;
            this.NoneButton.TabStop = true;
            this.NoneButton.Text = "None";
            this.NoneButton.UseVisualStyleBackColor = true;
            this.NoneButton.CheckedChanged += new System.EventHandler(this.NoneButton_CheckedChanged);
            // 
            // RectangleButton
            // 
            this.RectangleButton.AutoSize = true;
            this.RectangleButton.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RectangleButton.ForeColor = System.Drawing.Color.Black;
            this.RectangleButton.Location = new System.Drawing.Point(6, 157);
            this.RectangleButton.Name = "RectangleButton";
            this.RectangleButton.Size = new System.Drawing.Size(122, 28);
            this.RectangleButton.TabIndex = 3;
            this.RectangleButton.TabStop = true;
            this.RectangleButton.Text = "Rectangle";
            this.RectangleButton.UseVisualStyleBackColor = true;
            this.RectangleButton.CheckedChanged += new System.EventHandler(this.RectangleButton_CheckedChanged);
            // 
            // CornerButton
            // 
            this.CornerButton.AutoSize = true;
            this.CornerButton.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CornerButton.ForeColor = System.Drawing.Color.Black;
            this.CornerButton.Location = new System.Drawing.Point(6, 116);
            this.CornerButton.Name = "CornerButton";
            this.CornerButton.Size = new System.Drawing.Size(92, 28);
            this.CornerButton.TabIndex = 2;
            this.CornerButton.TabStop = true;
            this.CornerButton.Text = "Corner";
            this.CornerButton.UseVisualStyleBackColor = true;
            this.CornerButton.CheckedChanged += new System.EventHandler(this.CornerButton_CheckedChanged);
            // 
            // CrossButton
            // 
            this.CrossButton.AutoSize = true;
            this.CrossButton.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CrossButton.ForeColor = System.Drawing.Color.Black;
            this.CrossButton.Location = new System.Drawing.Point(6, 77);
            this.CrossButton.Name = "CrossButton";
            this.CrossButton.Size = new System.Drawing.Size(84, 28);
            this.CrossButton.TabIndex = 1;
            this.CrossButton.TabStop = true;
            this.CrossButton.Text = "Cross";
            this.CrossButton.UseVisualStyleBackColor = true;
            this.CrossButton.CheckedChanged += new System.EventHandler(this.CrossButton_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.DownButton);
            this.groupBox2.Controls.Add(this.LeftButton);
            this.groupBox2.Controls.Add(this.UpButton);
            this.groupBox2.Controls.Add(this.RightButton);
            this.groupBox2.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.Color.Blue;
            this.groupBox2.Location = new System.Drawing.Point(433, 223);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(224, 149);
            this.groupBox2.TabIndex = 19;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Template Moving";
            // 
            // DownButton
            // 
            this.DownButton.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.DownButton.ForeColor = System.Drawing.Color.Black;
            this.DownButton.Location = new System.Drawing.Point(90, 95);
            this.DownButton.Name = "DownButton";
            this.DownButton.Size = new System.Drawing.Size(50, 50);
            this.DownButton.TabIndex = 14;
            this.DownButton.Text = "↓";
            this.DownButton.UseVisualStyleBackColor = true;
            this.DownButton.Click += new System.EventHandler(this.DownButton_Click);
            // 
            // LeftButton
            // 
            this.LeftButton.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LeftButton.ForeColor = System.Drawing.Color.Black;
            this.LeftButton.Location = new System.Drawing.Point(30, 95);
            this.LeftButton.Name = "LeftButton";
            this.LeftButton.Size = new System.Drawing.Size(54, 50);
            this.LeftButton.TabIndex = 16;
            this.LeftButton.Text = " ←";
            this.LeftButton.UseVisualStyleBackColor = true;
            this.LeftButton.Click += new System.EventHandler(this.LeftButton_Click);
            // 
            // UpButton
            // 
            this.UpButton.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.UpButton.ForeColor = System.Drawing.Color.Black;
            this.UpButton.Location = new System.Drawing.Point(90, 39);
            this.UpButton.Name = "UpButton";
            this.UpButton.Size = new System.Drawing.Size(50, 50);
            this.UpButton.TabIndex = 13;
            this.UpButton.Text = "↑";
            this.UpButton.UseVisualStyleBackColor = true;
            this.UpButton.Click += new System.EventHandler(this.UpButton_Click);
            // 
            // RightButton
            // 
            this.RightButton.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.RightButton.ForeColor = System.Drawing.Color.Black;
            this.RightButton.Location = new System.Drawing.Point(146, 95);
            this.RightButton.Name = "RightButton";
            this.RightButton.Size = new System.Drawing.Size(50, 50);
            this.RightButton.TabIndex = 15;
            this.RightButton.Text = "→";
            this.RightButton.UseVisualStyleBackColor = true;
            this.RightButton.Click += new System.EventHandler(this.RightButton_Click);
            // 
            // CreateButton
            // 
            this.CreateButton.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.CreateButton.Location = new System.Drawing.Point(463, 378);
            this.CreateButton.Name = "CreateButton";
            this.CreateButton.Size = new System.Drawing.Size(166, 36);
            this.CreateButton.TabIndex = 20;
            this.CreateButton.Text = "Create";
            this.CreateButton.UseVisualStyleBackColor = true;
            this.CreateButton.Click += new System.EventHandler(this.CreateButton_Click);
            // 
            // TemplateMask
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.CreateButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.MaskTypeBox);
            this.Controls.Add(this.pictureBox1);
            this.Name = "TemplateMask";
            this.Size = new System.Drawing.Size(675, 436);
            this.Load += new System.EventHandler(this.TemplateMask_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.MaskTypeBox.ResumeLayout(false);
            this.MaskTypeBox.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox MaskTypeBox;
        private System.Windows.Forms.RadioButton NoneButton;
        private System.Windows.Forms.RadioButton RectangleButton;
        private System.Windows.Forms.RadioButton CornerButton;
        private System.Windows.Forms.RadioButton CrossButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button DownButton;
        private System.Windows.Forms.Button LeftButton;
        private System.Windows.Forms.Button UpButton;
        private System.Windows.Forms.Button RightButton;
        private System.Windows.Forms.Button CreateButton;
    }
}
