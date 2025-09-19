namespace SemiAutomaticAligner
{
    partial class DialogSelectUserMode
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
            this.buttonProductionMode = new System.Windows.Forms.Button();
            this.buttonEducationalMode = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonProductionMode
            // 
            this.buttonProductionMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonProductionMode.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonProductionMode.Location = new System.Drawing.Point(87, 124);
            this.buttonProductionMode.Name = "buttonProductionMode";
            this.buttonProductionMode.Size = new System.Drawing.Size(191, 46);
            this.buttonProductionMode.TabIndex = 0;
            this.buttonProductionMode.Text = "Production Mode";
            this.buttonProductionMode.UseVisualStyleBackColor = true;
            this.buttonProductionMode.Click += new System.EventHandler(this.ButtonProductionMode_Click);
            // 
            // buttonEducationalMode
            // 
            this.buttonEducationalMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonEducationalMode.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonEducationalMode.Location = new System.Drawing.Point(87, 42);
            this.buttonEducationalMode.Name = "buttonEducationalMode";
            this.buttonEducationalMode.Size = new System.Drawing.Size(191, 46);
            this.buttonEducationalMode.TabIndex = 1;
            this.buttonEducationalMode.Text = "Educational mode";
            this.buttonEducationalMode.UseVisualStyleBackColor = true;
            this.buttonEducationalMode.Click += new System.EventHandler(this.ButtonEducationalMode_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClose.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonClose.Location = new System.Drawing.Point(87, 208);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(191, 46);
            this.buttonClose.TabIndex = 2;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // DialogSelectUserMode
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Azure;
            this.ClientSize = new System.Drawing.Size(380, 299);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonEducationalMode);
            this.Controls.Add(this.buttonProductionMode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DialogSelectUserMode";
            this.Text = "UserModeSelect";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonProductionMode;
        private System.Windows.Forms.Button buttonEducationalMode;
        private System.Windows.Forms.Button buttonClose;
    }
}