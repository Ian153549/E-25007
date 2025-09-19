namespace MRLibrary
{
    partial class MessageBoxM
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
            this.label = new System.Windows.Forms.Label();
            this.plIcon = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btCancel = new System.Windows.Forms.Button();
            this.btOK = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label
            // 
            this.label.Location = new System.Drawing.Point(82, 8);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(356, 99);
            this.label.TabIndex = 6;
            this.label.Text = "label1";
            this.label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // plIcon
            // 
            this.plIcon.Location = new System.Drawing.Point(12, 28);
            this.plIcon.Name = "plIcon";
            this.plIcon.Size = new System.Drawing.Size(64, 64);
            this.plIcon.TabIndex = 7;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.btCancel);
            this.panel1.Controls.Add(this.btOK);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 141);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(473, 59);
            this.panel1.TabIndex = 8;
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(322, 21);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(88, 26);
            this.btCancel.TabIndex = 1;
            this.btCancel.Text = "取消";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Visible = false;
            // 
            // btOK
            // 
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.Location = new System.Drawing.Point(190, 21);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(88, 26);
            this.btOK.TabIndex = 0;
            this.btOK.Text = "確定";
            this.btOK.UseVisualStyleBackColor = true;
            // 
            // MessageBoxM
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(473, 200);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label);
            this.Controls.Add(this.plIcon);
            this.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Name = "MessageBoxM";
            this.Text = "MessageBoxM";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label;
        private System.Windows.Forms.Panel plIcon;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btOK;
    }
}