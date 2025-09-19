
namespace MRLibrary
{
    partial class SKZoomAndPanWindow
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // SKZoomAndPanWindow
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Name = "SKZoomAndPanWindow";
            this.Size = new System.Drawing.Size(800, 450);
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
