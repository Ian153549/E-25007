namespace MRVisionLib
{
    partial class SKZoomAndPanWindow
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
            this.SuspendLayout();
            // 
            // SKZoomAndPanWindow
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.Name = "SKZoomAndPanWindow";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SKZoomAndPanWindow_MouseDown);
            this.MouseEnter += new System.EventHandler(this.SKZoomAndPanWindow_MouseEnter);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SKZoomAndPanWindow_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SKZoomAndPanWindow_MouseUp);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.SKZoomAndPanWindow_MouseWheel);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
