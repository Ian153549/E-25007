namespace NSAA_16Axis
{
    partial class DialogManualPlcCom
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
            this.textBoxIp = new System.Windows.Forms.TextBox();
            this.IPAddress = new System.Windows.Forms.Label();
            this.textBoxSendMsg = new System.Windows.Forms.TextBox();
            this.textBoxReadMsg = new System.Windows.Forms.TextBox();
            this.fWrite = new System.Windows.Forms.Label();
            this.fRead = new System.Windows.Forms.Label();
            this.buttonSend = new System.Windows.Forms.Button();
            this.numericUpDownPort = new System.Windows.Forms.NumericUpDown();
            this.PLCPort = new System.Windows.Forms.Label();
            this.buttonConnect = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPort)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxIp
            // 
            this.textBoxIp.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.textBoxIp.Location = new System.Drawing.Point(148, 32);
            this.textBoxIp.Name = "textBoxIp";
            this.textBoxIp.Size = new System.Drawing.Size(200, 26);
            this.textBoxIp.TabIndex = 0;
            // 
            // IPAddress
            // 
            this.IPAddress.Location = new System.Drawing.Point(26, 41);
            this.IPAddress.Name = "IPAddress";
            this.IPAddress.Size = new System.Drawing.Size(103, 18);
            this.IPAddress.TabIndex = 1;
            this.IPAddress.Text = "IP : ";
            this.IPAddress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxSendMsg
            // 
            this.textBoxSendMsg.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.textBoxSendMsg.Location = new System.Drawing.Point(148, 144);
            this.textBoxSendMsg.Name = "textBoxSendMsg";
            this.textBoxSendMsg.Size = new System.Drawing.Size(237, 26);
            this.textBoxSendMsg.TabIndex = 2;
            // 
            // textBoxReadMsg
            // 
            this.textBoxReadMsg.Location = new System.Drawing.Point(148, 218);
            this.textBoxReadMsg.Multiline = true;
            this.textBoxReadMsg.Name = "textBoxReadMsg";
            this.textBoxReadMsg.ReadOnly = true;
            this.textBoxReadMsg.Size = new System.Drawing.Size(237, 65);
            this.textBoxReadMsg.TabIndex = 3;
            // 
            // fWrite
            // 
            this.fWrite.Location = new System.Drawing.Point(29, 153);
            this.fWrite.Name = "fWrite";
            this.fWrite.Size = new System.Drawing.Size(100, 18);
            this.fWrite.TabIndex = 4;
            this.fWrite.Text = "Write : ";
            this.fWrite.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // fRead
            // 
            this.fRead.Location = new System.Drawing.Point(26, 227);
            this.fRead.Name = "fRead";
            this.fRead.Size = new System.Drawing.Size(103, 18);
            this.fRead.TabIndex = 5;
            this.fRead.Text = "Read : ";
            this.fRead.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(404, 144);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(75, 26);
            this.buttonSend.TabIndex = 6;
            this.buttonSend.Text = "send";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // numericUpDownPort
            // 
            this.numericUpDownPort.Location = new System.Drawing.Point(148, 74);
            this.numericUpDownPort.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownPort.Name = "numericUpDownPort";
            this.numericUpDownPort.Size = new System.Drawing.Size(93, 26);
            this.numericUpDownPort.TabIndex = 7;
            // 
            // PLCPort
            // 
            this.PLCPort.Location = new System.Drawing.Point(23, 82);
            this.PLCPort.Name = "PLCPort";
            this.PLCPort.Size = new System.Drawing.Size(106, 18);
            this.PLCPort.TabIndex = 8;
            this.PLCPort.Text = "Port : ";
            this.PLCPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(261, 74);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(108, 26);
            this.buttonConnect.TabIndex = 9;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.button1_Click);
            // 
            // DialogManualPlcCom
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(493, 323);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.PLCPort);
            this.Controls.Add(this.numericUpDownPort);
            this.Controls.Add(this.buttonSend);
            this.Controls.Add(this.fRead);
            this.Controls.Add(this.fWrite);
            this.Controls.Add(this.textBoxReadMsg);
            this.Controls.Add(this.textBoxSendMsg);
            this.Controls.Add(this.IPAddress);
            this.Controls.Add(this.textBoxIp);
            this.Font = new System.Drawing.Font("Arial", 12F);
            this.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.Name = "DialogManualPlcCom";
            this.Text = "ManualPlcCom";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DialogManualPlcCom_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxIp;
        private System.Windows.Forms.Label IPAddress;
        private System.Windows.Forms.TextBox textBoxSendMsg;
        private System.Windows.Forms.TextBox textBoxReadMsg;
        private System.Windows.Forms.Label fWrite;
        private System.Windows.Forms.Label fRead;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.NumericUpDown numericUpDownPort;
        private System.Windows.Forms.Label PLCPort;
        private System.Windows.Forms.Button buttonConnect;
    }
}