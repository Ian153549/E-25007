namespace NSAA_16Axis
{
    partial class DialogCycleTest
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
            this.numericUpDownCycleTimes = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonStart = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCycleTimes)).BeginInit();
            this.SuspendLayout();
            // 
            // numericUpDownCycleTimes
            // 
            this.numericUpDownCycleTimes.Font = new System.Drawing.Font("Arial", 12F);
            this.numericUpDownCycleTimes.Location = new System.Drawing.Point(148, 43);
            this.numericUpDownCycleTimes.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericUpDownCycleTimes.Name = "numericUpDownCycleTimes";
            this.numericUpDownCycleTimes.Size = new System.Drawing.Size(91, 26);
            this.numericUpDownCycleTimes.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 12F);
            this.label1.Location = new System.Drawing.Point(12, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(130, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "Cycle test times : ";
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(259, 45);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(75, 26);
            this.buttonStart.TabIndex = 2;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.ButtonStart_Click);
            // 
            // DialogCycleTest
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(365, 118);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDownCycleTimes);
            this.Font = new System.Drawing.Font("Arial", 12F);
            this.Name = "DialogCycleTest";
            this.Text = "DialogCycleTest";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCycleTimes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numericUpDownCycleTimes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonStart;
    }
}