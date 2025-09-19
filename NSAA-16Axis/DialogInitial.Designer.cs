namespace NSAA_16Axis
{
    partial class DialogInitial
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
            this.TextBoxInitial = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // TextBoxInitial
            // 
            this.TextBoxInitial.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.TextBoxInitial.Location = new System.Drawing.Point(23, 148);
            this.TextBoxInitial.Multiline = true;
            this.TextBoxInitial.Name = "TextBoxInitial";
            this.TextBoxInitial.ReadOnly = true;
            this.TextBoxInitial.Size = new System.Drawing.Size(284, 384);
            this.TextBoxInitial.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::NSAA_16Axis.Properties.Resources.Logo_正式版_;
            this.pictureBox1.Location = new System.Drawing.Point(23, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(284, 130);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // DialogInitial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Azure;
            this.ClientSize = new System.Drawing.Size(330, 546);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.TextBoxInitial);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DialogInitial";
            this.Text = "DialogInitial";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TextBoxInitial;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}