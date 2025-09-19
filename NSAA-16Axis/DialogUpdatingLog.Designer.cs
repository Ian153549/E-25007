namespace NSAA_16Axis
{
    partial class DialogUpdatingLog
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
            this.TextBoxUpdatingLog = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // TextBoxUpdatingLog
            // 
            this.TextBoxUpdatingLog.Font = new System.Drawing.Font("Arial", 12F);
            this.TextBoxUpdatingLog.Location = new System.Drawing.Point(12, 12);
            this.TextBoxUpdatingLog.Multiline = true;
            this.TextBoxUpdatingLog.Name = "TextBoxUpdatingLog";
            this.TextBoxUpdatingLog.ReadOnly = true;
            this.TextBoxUpdatingLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TextBoxUpdatingLog.Size = new System.Drawing.Size(376, 257);
            this.TextBoxUpdatingLog.TabIndex = 0;
            this.TextBoxUpdatingLog.TextChanged += new System.EventHandler(this.TextBoxUpdatingLog_TextChanged);
            // 
            // DialogUpdatingLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(401, 290);
            this.Controls.Add(this.TextBoxUpdatingLog);
            this.Name = "DialogUpdatingLog";
            this.Text = "DialogUpdatingLog";
            this.Load += new System.EventHandler(this.DialogUpdatingLog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TextBoxUpdatingLog;
    }
}