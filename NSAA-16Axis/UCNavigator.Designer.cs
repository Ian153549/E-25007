namespace NSAA_16Axis
{
    partial class UCNavigator
    {
        /// <summary> 
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btUp = new System.Windows.Forms.Button();
            this.btDown = new System.Windows.Forms.Button();
            this.btLeft = new System.Windows.Forms.Button();
            this.btRight = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btUp
            // 
            this.btUp.Image = global::NSAA_16Axis.Properties.Resources.btUp_Image;
            this.btUp.Location = new System.Drawing.Point(38, 0);
            this.btUp.Name = "btUp";
            this.btUp.Size = new System.Drawing.Size(38, 19);
            this.btUp.TabIndex = 3;
            this.btUp.UseVisualStyleBackColor = true;
            this.btUp.Click += new System.EventHandler(this.BtUp_Click);
            // 
            // btDown
            // 
            this.btDown.Image = global::NSAA_16Axis.Properties.Resources.btDown_Image;
            this.btDown.Location = new System.Drawing.Point(38, 18);
            this.btDown.Name = "btDown";
            this.btDown.Size = new System.Drawing.Size(38, 19);
            this.btDown.TabIndex = 2;
            this.btDown.UseVisualStyleBackColor = true;
            this.btDown.Click += new System.EventHandler(this.BtDown_Click);
            // 
            // btLeft
            // 
            this.btLeft.Image = global::NSAA_16Axis.Properties.Resources.btLeft_Image;
            this.btLeft.Location = new System.Drawing.Point(7, 10);
            this.btLeft.Name = "btLeft";
            this.btLeft.Size = new System.Drawing.Size(32, 22);
            this.btLeft.TabIndex = 1;
            this.btLeft.UseVisualStyleBackColor = true;
            this.btLeft.Click += new System.EventHandler(this.BtLeft_Click);
            // 
            // btRight
            // 
            this.btRight.Image = global::NSAA_16Axis.Properties.Resources.btRight_Image;
            this.btRight.Location = new System.Drawing.Point(75, 10);
            this.btRight.Margin = new System.Windows.Forms.Padding(0);
            this.btRight.Name = "btRight";
            this.btRight.Size = new System.Drawing.Size(32, 22);
            this.btRight.TabIndex = 3;
            this.btRight.UseVisualStyleBackColor = true;
            this.btRight.Click += new System.EventHandler(this.BtRight_Click);
            // 
            // UCNavigator
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.Controls.Add(this.btUp);
            this.Controls.Add(this.btDown);
            this.Controls.Add(this.btLeft);
            this.Controls.Add(this.btRight);
            this.Name = "UCNavigator";
            this.Size = new System.Drawing.Size(116, 40);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btRight;
        private System.Windows.Forms.Button btLeft;
        private System.Windows.Forms.Button btDown;
        private System.Windows.Forms.Button btUp;
    }
}
