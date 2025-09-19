namespace NSAA_16Axis
{
    partial class DialogChangePassword
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
            this.lbOldPassword = new System.Windows.Forms.Label();
            this.lbNewPassword = new System.Windows.Forms.Label();
            this.lbVerify = new System.Windows.Forms.Label();
            this.textBoxOldPassword = new System.Windows.Forms.TextBox();
            this.textBoxNewPassword = new System.Windows.Forms.TextBox();
            this.textBoxVerifyPassword = new System.Windows.Forms.TextBox();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbOldPassword
            // 
            this.lbOldPassword.AutoSize = true;
            this.lbOldPassword.Location = new System.Drawing.Point(25, 29);
            this.lbOldPassword.Name = "lbOldPassword";
            this.lbOldPassword.Size = new System.Drawing.Size(116, 18);
            this.lbOldPassword.TabIndex = 0;
            this.lbOldPassword.Text = "Old password : ";
            // 
            // lbNewPassword
            // 
            this.lbNewPassword.AutoSize = true;
            this.lbNewPassword.Location = new System.Drawing.Point(25, 94);
            this.lbNewPassword.Name = "lbNewPassword";
            this.lbNewPassword.Size = new System.Drawing.Size(123, 18);
            this.lbNewPassword.TabIndex = 1;
            this.lbNewPassword.Text = "New password : ";
            this.lbNewPassword.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbVerify
            // 
            this.lbVerify.AutoSize = true;
            this.lbVerify.Location = new System.Drawing.Point(25, 160);
            this.lbVerify.Name = "lbVerify";
            this.lbVerify.Size = new System.Drawing.Size(59, 18);
            this.lbVerify.TabIndex = 2;
            this.lbVerify.Text = "Verify : ";
            // 
            // textBoxOldPassword
            // 
            this.textBoxOldPassword.Location = new System.Drawing.Point(155, 26);
            this.textBoxOldPassword.Name = "textBoxOldPassword";
            this.textBoxOldPassword.PasswordChar = '*';
            this.textBoxOldPassword.Size = new System.Drawing.Size(153, 26);
            this.textBoxOldPassword.TabIndex = 3;
            // 
            // textBoxNewPassword
            // 
            this.textBoxNewPassword.Location = new System.Drawing.Point(155, 91);
            this.textBoxNewPassword.Name = "textBoxNewPassword";
            this.textBoxNewPassword.PasswordChar = '*';
            this.textBoxNewPassword.Size = new System.Drawing.Size(153, 26);
            this.textBoxNewPassword.TabIndex = 4;
            // 
            // textBoxVerifyPassword
            // 
            this.textBoxVerifyPassword.Location = new System.Drawing.Point(155, 157);
            this.textBoxVerifyPassword.Name = "textBoxVerifyPassword";
            this.textBoxVerifyPassword.PasswordChar = '*';
            this.textBoxVerifyPassword.Size = new System.Drawing.Size(153, 26);
            this.textBoxVerifyPassword.TabIndex = 5;
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(344, 156);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(86, 30);
            this.btnConfirm.TabIndex = 6;
            this.btnConfirm.Text = "Confirm";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.ButtonConfirm_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(344, 112);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(86, 29);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // DialogChangePassword
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(453, 214);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.textBoxVerifyPassword);
            this.Controls.Add(this.textBoxNewPassword);
            this.Controls.Add(this.textBoxOldPassword);
            this.Controls.Add(this.lbVerify);
            this.Controls.Add(this.lbNewPassword);
            this.Controls.Add(this.lbOldPassword);
            this.Font = new System.Drawing.Font("Arial", 12F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DialogChangePassword";
            this.Text = "DialogChangePassword";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbOldPassword;
        private System.Windows.Forms.Label lbNewPassword;
        private System.Windows.Forms.Label lbVerify;
        private System.Windows.Forms.TextBox textBoxOldPassword;
        private System.Windows.Forms.TextBox textBoxNewPassword;
        private System.Windows.Forms.TextBox textBoxVerifyPassword;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Button btnCancel;
    }
}