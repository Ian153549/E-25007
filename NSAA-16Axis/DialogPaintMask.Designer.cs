namespace NSAA_16Axis
{
    partial class DialogPaintMask
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
            this.components = new System.ComponentModel.Container();
            this.Timer1 = new System.Windows.Forms.Timer(this.components);
            this.BtCancel = new System.Windows.Forms.Button();
            this.BtOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.UdPenSize = new System.Windows.Forms.NumericUpDown();
            this.CbControl = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.ucGrayPaintMask = new NSAA_16Axis.UCGrayPaintMask();
            this.BTClear = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.UdPenSize)).BeginInit();
            this.SuspendLayout();
            // 
            // Timer1
            // 
            this.Timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // BtCancel
            // 
            this.BtCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtCancel.Location = new System.Drawing.Point(614, 113);
            this.BtCancel.Name = "BtCancel";
            this.BtCancel.Size = new System.Drawing.Size(93, 38);
            this.BtCancel.TabIndex = 0;
            this.BtCancel.Text = "取消";
            this.BtCancel.UseVisualStyleBackColor = true;
            this.BtCancel.Click += new System.EventHandler(this.BtCancel_Click);
            // 
            // BtOK
            // 
            this.BtOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.BtOK.Location = new System.Drawing.Point(614, 56);
            this.BtOK.Name = "BtOK";
            this.BtOK.Size = new System.Drawing.Size(93, 38);
            this.BtOK.TabIndex = 1;
            this.BtOK.Text = "確定";
            this.BtOK.UseVisualStyleBackColor = true;
            this.BtOK.Click += new System.EventHandler(this.BtOK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(199, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "使用滑鼠左鍵建立遮罩, 右鍵清除.";
            // 
            // UdPenSize
            // 
            this.UdPenSize.Location = new System.Drawing.Point(532, 22);
            this.UdPenSize.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.UdPenSize.Name = "UdPenSize";
            this.UdPenSize.Size = new System.Drawing.Size(120, 25);
            this.UdPenSize.TabIndex = 3;
            this.UdPenSize.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.UdPenSize.ValueChanged += new System.EventHandler(this.UdPenSize_ValueChanged);
            // 
            // CbControl
            // 
            this.CbControl.FormattingEnabled = true;
            this.CbControl.Items.AddRange(new object[] {
            "圖型筆",
            "方型筆",
            "線型",
            "圓型",
            "方框",
            "菱形筆",
            "菱形框"});
            this.CbControl.Location = new System.Drawing.Point(333, 22);
            this.CbControl.Name = "CbControl";
            this.CbControl.Size = new System.Drawing.Size(121, 25);
            this.CbControl.TabIndex = 4;
            this.CbControl.SelectedIndexChanged += new System.EventHandler(this.CbControl_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(469, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Pen Size";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(267, 26);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(60, 17);
            this.label25.TabIndex = 6;
            this.label25.Text = "畫筆形式";
            // 
            // ucGrayPaintMask
            // 
            this.ucGrayPaintMask.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ucGrayPaintMask.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ucGrayPaintMask.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.ucGrayPaintMask.LineThickness = 10;
            this.ucGrayPaintMask.Location = new System.Drawing.Point(22, 56);
            this.ucGrayPaintMask.Name = "ucGrayPaintMask";
            this.ucGrayPaintMask.PaintColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.ucGrayPaintMask.PenSize = 10;
            this.ucGrayPaintMask.Size = new System.Drawing.Size(517, 424);
            this.ucGrayPaintMask.TabIndex = 7;
            this.ucGrayPaintMask.Tool = NSAA_16Axis.UCGrayPaintMask.EnumTool.Pen;
            // 
            // BTClear
            // 
            this.BTClear.ForeColor = System.Drawing.Color.Red;
            this.BTClear.Location = new System.Drawing.Point(614, 442);
            this.BTClear.Name = "BTClear";
            this.BTClear.Size = new System.Drawing.Size(93, 38);
            this.BTClear.TabIndex = 8;
            this.BTClear.Text = "清除";
            this.BTClear.UseVisualStyleBackColor = true;
            this.BTClear.Click += new System.EventHandler(this.BTClear_Click_1);
            // 
            // DialogPaintMask
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(719, 510);
            this.Controls.Add(this.BTClear);
            this.Controls.Add(this.ucGrayPaintMask);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.CbControl);
            this.Controls.Add(this.UdPenSize);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtOK);
            this.Controls.Add(this.BtCancel);
            this.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Name = "DialogPaintMask";
            this.Text = "編輯遮罩";
            this.Load += new System.EventHandler(this.DialogPaintMask_Load);
            ((System.ComponentModel.ISupportInitialize)(this.UdPenSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer Timer1;
        private System.Windows.Forms.Button BtCancel;
        private System.Windows.Forms.Button BtOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown UdPenSize;
        private System.Windows.Forms.ComboBox CbControl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label25;
        private UCGrayPaintMask ucGrayPaintMask;
        private System.Windows.Forms.Button BTClear;
    }
}