namespace NSAA_16Axis
{
    partial class DialogCreatePatternMask
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
            this.templateMask1 = new TemplatesMask.TemplateMask();
            this.SuspendLayout();
            // 
            // templateMask1
            // 
            this.templateMask1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.templateMask1.Location = new System.Drawing.Point(10, 20);
            this.templateMask1.Name = "templateMask1";
            this.templateMask1.Size = new System.Drawing.Size(656, 424);
            this.templateMask1.TabIndex = 0;
            // 
            // DialogCreatePatternMask
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(682, 464);
            this.Controls.Add(this.templateMask1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "DialogCreatePatternMask";
            this.Text = "DialogCreatePatternMask";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DialogCreatePatternMask_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private TemplatesMask.TemplateMask templateMask1;
    }
}