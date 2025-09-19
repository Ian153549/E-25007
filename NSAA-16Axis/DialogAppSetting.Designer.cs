namespace NSAA_16Axis
{
    partial class DialogAppSetting
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
            this.checkBoxWheatherInitialLens = new System.Windows.Forms.CheckBox();
            this.cbEnableContactShift = new System.Windows.Forms.CheckBox();
            this.numericUpDown3By3SearchingSteps = new System.Windows.Forms.NumericUpDown();
            this.lbSearchSteps = new System.Windows.Forms.Label();
            this.cbEnableAutoSearch = new System.Windows.Forms.CheckBox();
            this.btnChangeEngineerPassword = new System.Windows.Forms.Button();
            this.btnChangeProductionModePassword = new System.Windows.Forms.Button();
            this.btnAbout = new System.Windows.Forms.Button();
            this.cbDownThruHome = new System.Windows.Forms.CheckBox();
            this.gbLanguage = new System.Windows.Forms.GroupBox();
            this.rbCChinese = new System.Windows.Forms.RadioButton();
            this.rbCustom = new System.Windows.Forms.RadioButton();
            this.rbDefault = new System.Windows.Forms.RadioButton();
            this.cbEmulationMode = new System.Windows.Forms.CheckBox();
            this.btCancel = new System.Windows.Forms.Button();
            this.btOK = new System.Windows.Forms.Button();
            this.cBPLCStatus = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3By3SearchingSteps)).BeginInit();
            this.gbLanguage.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBoxWheatherInitialLens
            // 
            this.checkBoxWheatherInitialLens.AutoSize = true;
            this.checkBoxWheatherInitialLens.Font = new System.Drawing.Font("Arial", 12F);
            this.checkBoxWheatherInitialLens.Location = new System.Drawing.Point(30, 26);
            this.checkBoxWheatherInitialLens.Name = "checkBoxWheatherInitialLens";
            this.checkBoxWheatherInitialLens.Size = new System.Drawing.Size(269, 27);
            this.checkBoxWheatherInitialLens.TabIndex = 0;
            this.checkBoxWheatherInitialLens.Text = "Initial zoom lens when start";
            this.checkBoxWheatherInitialLens.UseVisualStyleBackColor = true;
            this.checkBoxWheatherInitialLens.Visible = false;
            this.checkBoxWheatherInitialLens.CheckedChanged += new System.EventHandler(this.CheckBoxWheatherInitialLens_CheckedChanged);
            // 
            // cbEnableContactShift
            // 
            this.cbEnableContactShift.AutoSize = true;
            this.cbEnableContactShift.Font = new System.Drawing.Font("Arial", 12F);
            this.cbEnableContactShift.Location = new System.Drawing.Point(30, 108);
            this.cbEnableContactShift.Name = "cbEnableContactShift";
            this.cbEnableContactShift.Size = new System.Drawing.Size(269, 27);
            this.cbEnableContactShift.TabIndex = 1;
            this.cbEnableContactShift.Text = "Contact shift compensation";
            this.cbEnableContactShift.UseVisualStyleBackColor = true;
            this.cbEnableContactShift.Visible = false;
            this.cbEnableContactShift.CheckedChanged += new System.EventHandler(this.CheckBoxEnableContactShift_CheckedChanged);
            // 
            // numericUpDown3By3SearchingSteps
            // 
            this.numericUpDown3By3SearchingSteps.Font = new System.Drawing.Font("Arial", 12F);
            this.numericUpDown3By3SearchingSteps.Location = new System.Drawing.Point(165, 186);
            this.numericUpDown3By3SearchingSteps.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numericUpDown3By3SearchingSteps.Name = "numericUpDown3By3SearchingSteps";
            this.numericUpDown3By3SearchingSteps.Size = new System.Drawing.Size(75, 30);
            this.numericUpDown3By3SearchingSteps.TabIndex = 2;
            this.numericUpDown3By3SearchingSteps.Visible = false;
            // 
            // lbSearchSteps
            // 
            this.lbSearchSteps.AutoSize = true;
            this.lbSearchSteps.Font = new System.Drawing.Font("Arial", 12F);
            this.lbSearchSteps.Location = new System.Drawing.Point(27, 188);
            this.lbSearchSteps.Name = "lbSearchSteps";
            this.lbSearchSteps.Size = new System.Drawing.Size(169, 23);
            this.lbSearchSteps.TabIndex = 3;
            this.lbSearchSteps.Text = "Searching steps : ";
            this.lbSearchSteps.Visible = false;
            // 
            // cbEnableAutoSearch
            // 
            this.cbEnableAutoSearch.AutoSize = true;
            this.cbEnableAutoSearch.Location = new System.Drawing.Point(29, 148);
            this.cbEnableAutoSearch.Name = "cbEnableAutoSearch";
            this.cbEnableAutoSearch.Size = new System.Drawing.Size(162, 27);
            this.cbEnableAutoSearch.TabIndex = 4;
            this.cbEnableAutoSearch.Text = "Auto searching";
            this.cbEnableAutoSearch.UseVisualStyleBackColor = true;
            this.cbEnableAutoSearch.Visible = false;
            this.cbEnableAutoSearch.CheckedChanged += new System.EventHandler(this.CheckBoxEnableAutoSearch_CheckedChanged);
            // 
            // btnChangeEngineerPassword
            // 
            this.btnChangeEngineerPassword.Location = new System.Drawing.Point(12, 366);
            this.btnChangeEngineerPassword.Name = "btnChangeEngineerPassword";
            this.btnChangeEngineerPassword.Size = new System.Drawing.Size(215, 33);
            this.btnChangeEngineerPassword.TabIndex = 5;
            this.btnChangeEngineerPassword.Text = "Change engineer password";
            this.btnChangeEngineerPassword.UseVisualStyleBackColor = true;
            this.btnChangeEngineerPassword.Click += new System.EventHandler(this.ChanegePassword);
            // 
            // btnChangeProductionModePassword
            // 
            this.btnChangeProductionModePassword.Location = new System.Drawing.Point(12, 405);
            this.btnChangeProductionModePassword.Name = "btnChangeProductionModePassword";
            this.btnChangeProductionModePassword.Size = new System.Drawing.Size(266, 33);
            this.btnChangeProductionModePassword.TabIndex = 6;
            this.btnChangeProductionModePassword.Text = "Change production mode password";
            this.btnChangeProductionModePassword.UseVisualStyleBackColor = true;
            this.btnChangeProductionModePassword.Visible = false;
            this.btnChangeProductionModePassword.Click += new System.EventHandler(this.ChanegePassword);
            // 
            // btnAbout
            // 
            this.btnAbout.Location = new System.Drawing.Point(667, 24);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(95, 34);
            this.btnAbout.TabIndex = 7;
            this.btnAbout.Text = "about";
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new System.EventHandler(this.ButtonAbout_Click);
            // 
            // cbDownThruHome
            // 
            this.cbDownThruHome.AutoSize = true;
            this.cbDownThruHome.Location = new System.Drawing.Point(30, 66);
            this.cbDownThruHome.Name = "cbDownThruHome";
            this.cbDownThruHome.Size = new System.Drawing.Size(252, 27);
            this.cbDownThruHome.TabIndex = 8;
            this.cbDownThruHome.Text = "Lens down through home";
            this.cbDownThruHome.UseVisualStyleBackColor = true;
            this.cbDownThruHome.Visible = false;
            this.cbDownThruHome.CheckedChanged += new System.EventHandler(this.checkBoxDownThruHome_CheckedChanged);
            // 
            // gbLanguage
            // 
            this.gbLanguage.Controls.Add(this.rbCChinese);
            this.gbLanguage.Controls.Add(this.rbCustom);
            this.gbLanguage.Controls.Add(this.rbDefault);
            this.gbLanguage.Location = new System.Drawing.Point(341, 175);
            this.gbLanguage.Name = "gbLanguage";
            this.gbLanguage.Size = new System.Drawing.Size(340, 170);
            this.gbLanguage.TabIndex = 11;
            this.gbLanguage.TabStop = false;
            this.gbLanguage.Text = "Language Select";
            this.gbLanguage.Enter += new System.EventHandler(this.gbLanguage_Enter);
            // 
            // rbCChinese
            // 
            this.rbCChinese.AutoSize = true;
            this.rbCChinese.Location = new System.Drawing.Point(47, 121);
            this.rbCChinese.Name = "rbCChinese";
            this.rbCChinese.Size = new System.Drawing.Size(191, 27);
            this.rbCChinese.TabIndex = 15;
            this.rbCChinese.TabStop = true;
            this.rbCChinese.Text = "Simplified Chinese";
            this.rbCChinese.UseVisualStyleBackColor = true;
            // 
            // rbCustom
            // 
            this.rbCustom.AutoSize = true;
            this.rbCustom.Location = new System.Drawing.Point(47, 86);
            this.rbCustom.Name = "rbCustom";
            this.rbCustom.Size = new System.Drawing.Size(197, 27);
            this.rbCustom.TabIndex = 12;
            this.rbCustom.TabStop = true;
            this.rbCustom.Text = "Traditional Chinese";
            this.rbCustom.UseVisualStyleBackColor = true;
            // 
            // rbDefault
            // 
            this.rbDefault.AutoSize = true;
            this.rbDefault.Location = new System.Drawing.Point(47, 51);
            this.rbDefault.Name = "rbDefault";
            this.rbDefault.Size = new System.Drawing.Size(93, 27);
            this.rbDefault.TabIndex = 12;
            this.rbDefault.TabStop = true;
            this.rbDefault.Text = "English";
            this.rbDefault.UseVisualStyleBackColor = true;
            // 
            // cbEmulationMode
            // 
            this.cbEmulationMode.AutoSize = true;
            this.cbEmulationMode.Location = new System.Drawing.Point(30, 237);
            this.cbEmulationMode.Name = "cbEmulationMode";
            this.cbEmulationMode.Size = new System.Drawing.Size(173, 27);
            this.cbEmulationMode.TabIndex = 12;
            this.cbEmulationMode.Text = "Emulation Mode";
            this.cbEmulationMode.UseVisualStyleBackColor = true;
            this.cbEmulationMode.CheckedChanged += new System.EventHandler(this.EmulationMode_CheckedChanged);
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(588, 400);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(93, 38);
            this.btCancel.TabIndex = 14;
            this.btCancel.Text = "Cancel";
            this.btCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // btOK
            // 
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.Location = new System.Drawing.Point(695, 400);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(93, 38);
            this.btOK.TabIndex = 13;
            this.btOK.Text = "OK";
            this.btOK.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.BtOK_Click);
            // 
            // cBPLCStatus
            // 
            this.cBPLCStatus.AutoSize = true;
            this.cBPLCStatus.Location = new System.Drawing.Point(30, 270);
            this.cBPLCStatus.Name = "cBPLCStatus";
            this.cBPLCStatus.Size = new System.Drawing.Size(218, 27);
            this.cBPLCStatus.TabIndex = 15;
            this.cBPLCStatus.Text = "PLC Status Windows";
            this.cBPLCStatus.UseVisualStyleBackColor = true;
            // 
            // DialogAppSetting
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.cBPLCStatus);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.cbEmulationMode);
            this.Controls.Add(this.gbLanguage);
            this.Controls.Add(this.cbDownThruHome);
            this.Controls.Add(this.btnAbout);
            this.Controls.Add(this.btnChangeProductionModePassword);
            this.Controls.Add(this.btnChangeEngineerPassword);
            this.Controls.Add(this.cbEnableAutoSearch);
            this.Controls.Add(this.lbSearchSteps);
            this.Controls.Add(this.numericUpDown3By3SearchingSteps);
            this.Controls.Add(this.cbEnableContactShift);
            this.Controls.Add(this.checkBoxWheatherInitialLens);
            this.Font = new System.Drawing.Font("Arial", 12F);
            this.Name = "DialogAppSetting";
            this.Text = "AppSetting";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DialogAppSetting_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3By3SearchingSteps)).EndInit();
            this.gbLanguage.ResumeLayout(false);
            this.gbLanguage.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxWheatherInitialLens;
        private System.Windows.Forms.CheckBox cbEnableContactShift;
        private System.Windows.Forms.NumericUpDown numericUpDown3By3SearchingSteps;
        private System.Windows.Forms.Label lbSearchSteps;
        private System.Windows.Forms.CheckBox cbEnableAutoSearch;
        private System.Windows.Forms.Button btnChangeEngineerPassword;
        private System.Windows.Forms.Button btnChangeProductionModePassword;
        private System.Windows.Forms.Button btnAbout;
        private System.Windows.Forms.CheckBox cbDownThruHome;
        private System.Windows.Forms.GroupBox gbLanguage;
        private System.Windows.Forms.RadioButton rbCustom;
        private System.Windows.Forms.RadioButton rbDefault;
        private System.Windows.Forms.CheckBox cbEmulationMode;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.CheckBox cBPLCStatus;
        private System.Windows.Forms.RadioButton rbCChinese;
    }
}