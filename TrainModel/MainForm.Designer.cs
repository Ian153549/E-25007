namespace Vino
{
    partial class MainForm
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

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tcControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btResult = new System.Windows.Forms.Button();
            this.btTesting = new System.Windows.Forms.Button();
            this.btTerminate = new System.Windows.Forms.Button();
            this.btTraining = new System.Windows.Forms.Button();
            this.btModeling = new System.Windows.Forms.Button();
            this.btLabel = new System.Windows.Forms.Button();
            this.gbEnhance = new System.Windows.Forms.GroupBox();
            this.tbLighting = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.tbContrast = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tbGamma = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.tbRotate = new System.Windows.Forms.TextBox();
            this.gbModel = new System.Windows.Forms.GroupBox();
            this.tbEpoch = new System.Windows.Forms.TextBox();
            this.tbBatch = new System.Windows.Forms.TextBox();
            this.tbHeight = new System.Windows.Forms.TextBox();
            this.tbWidth = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.gbPath = new System.Windows.Forms.GroupBox();
            this.btTest = new System.Windows.Forms.Button();
            this.btValidate = new System.Windows.Forms.Button();
            this.btTrain = new System.Windows.Forms.Button();
            this.tbTest = new System.Windows.Forms.TextBox();
            this.tbValidate = new System.Windows.Forms.TextBox();
            this.tbTrain = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lbTrainInfo = new System.Windows.Forms.ListBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.splitContainerTest = new System.Windows.Forms.SplitContainer();
            this.lbTestResult = new System.Windows.Forms.ListBox();
            this.panelImageView = new System.Windows.Forms.Panel();
            this.pbTestImage = new System.Windows.Forms.PictureBox();
            this.panelNavigation = new System.Windows.Forms.Panel();
            this.btSaveModel = new System.Windows.Forms.Button();
            this.lblImageInfo = new System.Windows.Forms.Label();
            this.btNextImage = new System.Windows.Forms.Button();
            this.btPrevImage = new System.Windows.Forms.Button();
            this.tcControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.gbEnhance.SuspendLayout();
            this.gbModel.SuspendLayout();
            this.gbPath.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTest)).BeginInit();
            this.splitContainerTest.Panel1.SuspendLayout();
            this.splitContainerTest.Panel2.SuspendLayout();
            this.splitContainerTest.SuspendLayout();
            this.panelImageView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbTestImage)).BeginInit();
            this.panelNavigation.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcControl
            // 
            this.tcControl.Controls.Add(this.tabPage1);
            this.tcControl.Location = new System.Drawing.Point(21, 21);
            this.tcControl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tcControl.Name = "tcControl";
            this.tcControl.SelectedIndex = 0;
            this.tcControl.Size = new System.Drawing.Size(1019, 422);
            this.tcControl.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btResult);
            this.tabPage1.Controls.Add(this.btTesting);
            this.tabPage1.Controls.Add(this.btTerminate);
            this.tabPage1.Controls.Add(this.btTraining);
            this.tabPage1.Controls.Add(this.btModeling);
            this.tabPage1.Controls.Add(this.btLabel);
            this.tabPage1.Controls.Add(this.gbEnhance);
            this.tabPage1.Controls.Add(this.gbModel);
            this.tabPage1.Controls.Add(this.gbPath);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage1.Size = new System.Drawing.Size(1011, 393);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Basic Info";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btResult
            // 
            this.btResult.Location = new System.Drawing.Point(773, 352);
            this.btResult.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btResult.Name = "btResult";
            this.btResult.Size = new System.Drawing.Size(139, 32);
            this.btResult.TabIndex = 3;
            this.btResult.Text = "Testing Result";
            this.btResult.UseVisualStyleBackColor = true;
            this.btResult.Visible = false;
            this.btResult.Click += new System.EventHandler(this.btResult_Click);
            // 
            // btTesting
            // 
            this.btTesting.Location = new System.Drawing.Point(628, 352);
            this.btTesting.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btTesting.Name = "btTesting";
            this.btTesting.Size = new System.Drawing.Size(139, 32);
            this.btTesting.TabIndex = 3;
            this.btTesting.Text = "Model Testing";
            this.btTesting.UseVisualStyleBackColor = true;
            this.btTesting.Click += new System.EventHandler(this.btTesting_Click);
            // 
            // btTerminate
            // 
            this.btTerminate.Location = new System.Drawing.Point(483, 352);
            this.btTerminate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btTerminate.Name = "btTerminate";
            this.btTerminate.Size = new System.Drawing.Size(139, 32);
            this.btTerminate.TabIndex = 3;
            this.btTerminate.Text = "Training Terminate";
            this.btTerminate.UseVisualStyleBackColor = true;
            this.btTerminate.Click += new System.EventHandler(this.btTerminate_Click);
            // 
            // btTraining
            // 
            this.btTraining.Location = new System.Drawing.Point(339, 352);
            this.btTraining.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btTraining.Name = "btTraining";
            this.btTraining.Size = new System.Drawing.Size(139, 32);
            this.btTraining.TabIndex = 3;
            this.btTraining.Text = "Model Training";
            this.btTraining.UseVisualStyleBackColor = true;
            this.btTraining.Click += new System.EventHandler(this.btTraining_Click);
            // 
            // btModeling
            // 
            this.btModeling.Location = new System.Drawing.Point(64, 352);
            this.btModeling.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btModeling.Name = "btModeling";
            this.btModeling.Size = new System.Drawing.Size(139, 32);
            this.btModeling.TabIndex = 3;
            this.btModeling.Text = "Model Generate";
            this.btModeling.UseVisualStyleBackColor = true;
            this.btModeling.Visible = false;
            this.btModeling.Click += new System.EventHandler(this.btModeling_Click);
            // 
            // btLabel
            // 
            this.btLabel.Location = new System.Drawing.Point(225, 352);
            this.btLabel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btLabel.Name = "btLabel";
            this.btLabel.Size = new System.Drawing.Size(108, 32);
            this.btLabel.TabIndex = 3;
            this.btLabel.Text = "Image Label";
            this.btLabel.UseVisualStyleBackColor = true;
            this.btLabel.Click += new System.EventHandler(this.btLabel_Click);
            // 
            // gbEnhance
            // 
            this.gbEnhance.Controls.Add(this.tbLighting);
            this.gbEnhance.Controls.Add(this.label9);
            this.gbEnhance.Controls.Add(this.label10);
            this.gbEnhance.Controls.Add(this.tbContrast);
            this.gbEnhance.Controls.Add(this.label11);
            this.gbEnhance.Controls.Add(this.tbGamma);
            this.gbEnhance.Controls.Add(this.label12);
            this.gbEnhance.Controls.Add(this.tbRotate);
            this.gbEnhance.Location = new System.Drawing.Point(5, 265);
            this.gbEnhance.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbEnhance.Name = "gbEnhance";
            this.gbEnhance.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbEnhance.Size = new System.Drawing.Size(999, 74);
            this.gbEnhance.TabIndex = 2;
            this.gbEnhance.TabStop = false;
            this.gbEnhance.Text = "Image Enhancement";
            // 
            // tbLighting
            // 
            this.tbLighting.Location = new System.Drawing.Point(131, 25);
            this.tbLighting.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbLighting.Name = "tbLighting";
            this.tbLighting.Size = new System.Drawing.Size(89, 25);
            this.tbLighting.TabIndex = 1;
            this.tbLighting.Text = "0";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(5, 35);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(119, 15);
            this.label9.TabIndex = 0;
            this.label9.Text = "Lighting Adjust(%)";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(244, 35);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(84, 15);
            this.label10.TabIndex = 2;
            this.label10.Text = "Contrast(±%)";
            // 
            // tbContrast
            // 
            this.tbContrast.Location = new System.Drawing.Point(368, 25);
            this.tbContrast.Name = "tbContrast";
            this.tbContrast.Size = new System.Drawing.Size(70, 25);
            this.tbContrast.TabIndex = 2;
            this.tbContrast.Text = "0";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(521, 35);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(51, 15);
            this.label11.TabIndex = 3;
            this.label11.Text = "Gamma";
            // 
            // tbGamma
            // 
            this.tbGamma.Location = new System.Drawing.Point(587, 25);
            this.tbGamma.Name = "tbGamma";
            this.tbGamma.Size = new System.Drawing.Size(70, 25);
            this.tbGamma.TabIndex = 3;
            this.tbGamma.Text = "1.0";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(729, 35);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(81, 15);
            this.label12.TabIndex = 4;
            this.label12.Text = "Rotate(±deg)";
            // 
            // tbRotate
            // 
            this.tbRotate.Location = new System.Drawing.Point(816, 25);
            this.tbRotate.Name = "tbRotate";
            this.tbRotate.Size = new System.Drawing.Size(70, 25);
            this.tbRotate.TabIndex = 4;
            this.tbRotate.Text = "0";
            // 
            // gbModel
            // 
            this.gbModel.Controls.Add(this.tbEpoch);
            this.gbModel.Controls.Add(this.tbBatch);
            this.gbModel.Controls.Add(this.tbHeight);
            this.gbModel.Controls.Add(this.tbWidth);
            this.gbModel.Controls.Add(this.label8);
            this.gbModel.Controls.Add(this.label7);
            this.gbModel.Controls.Add(this.label6);
            this.gbModel.Controls.Add(this.label5);
            this.gbModel.Controls.Add(this.label4);
            this.gbModel.Location = new System.Drawing.Point(5, 155);
            this.gbModel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbModel.Name = "gbModel";
            this.gbModel.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbModel.Size = new System.Drawing.Size(999, 92);
            this.gbModel.TabIndex = 1;
            this.gbModel.TabStop = false;
            this.gbModel.Text = "Model Setting";
            // 
            // tbEpoch
            // 
            this.tbEpoch.Location = new System.Drawing.Point(568, 51);
            this.tbEpoch.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbEpoch.Name = "tbEpoch";
            this.tbEpoch.Size = new System.Drawing.Size(89, 25);
            this.tbEpoch.TabIndex = 1;
            this.tbEpoch.Text = "50";
            // 
            // tbBatch
            // 
            this.tbBatch.Location = new System.Drawing.Point(368, 50);
            this.tbBatch.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbBatch.Name = "tbBatch";
            this.tbBatch.Size = new System.Drawing.Size(89, 25);
            this.tbBatch.TabIndex = 1;
            this.tbBatch.Text = "2";
            // 
            // tbHeight
            // 
            this.tbHeight.Location = new System.Drawing.Point(157, 51);
            this.tbHeight.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbHeight.Name = "tbHeight";
            this.tbHeight.Size = new System.Drawing.Size(89, 25);
            this.tbHeight.TabIndex = 1;
            this.tbHeight.Text = "2000";
            // 
            // tbWidth
            // 
            this.tbWidth.Location = new System.Drawing.Point(59, 51);
            this.tbWidth.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbWidth.Name = "tbWidth";
            this.tbWidth.Size = new System.Drawing.Size(89, 25);
            this.tbWidth.TabIndex = 1;
            this.tbWidth.Text = "2000";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(501, 54);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(43, 15);
            this.label8.TabIndex = 0;
            this.label8.Text = "Epoch";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(291, 55);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 15);
            this.label7.TabIndex = 0;
            this.label7.Text = "Batch Size";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(181, 32);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 15);
            this.label6.TabIndex = 0;
            this.label6.Text = "Height";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(83, 34);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 15);
            this.label5.TabIndex = 0;
            this.label5.Text = "Width";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 54);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 15);
            this.label4.TabIndex = 0;
            this.label4.Text = "Image";
            // 
            // gbPath
            // 
            this.gbPath.Controls.Add(this.btTest);
            this.gbPath.Controls.Add(this.btValidate);
            this.gbPath.Controls.Add(this.btTrain);
            this.gbPath.Controls.Add(this.tbTest);
            this.gbPath.Controls.Add(this.tbValidate);
            this.gbPath.Controls.Add(this.tbTrain);
            this.gbPath.Controls.Add(this.label3);
            this.gbPath.Controls.Add(this.label2);
            this.gbPath.Controls.Add(this.label1);
            this.gbPath.Location = new System.Drawing.Point(5, 6);
            this.gbPath.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbPath.Name = "gbPath";
            this.gbPath.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbPath.Size = new System.Drawing.Size(999, 132);
            this.gbPath.TabIndex = 0;
            this.gbPath.TabStop = false;
            this.gbPath.Text = "Path:";
            // 
            // btTest
            // 
            this.btTest.Location = new System.Drawing.Point(883, 96);
            this.btTest.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btTest.Name = "btTest";
            this.btTest.Size = new System.Drawing.Size(107, 25);
            this.btTest.TabIndex = 2;
            this.btTest.Text = "Select";
            this.btTest.UseVisualStyleBackColor = true;
            // 
            // btValidate
            // 
            this.btValidate.Location = new System.Drawing.Point(883, 65);
            this.btValidate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btValidate.Name = "btValidate";
            this.btValidate.Size = new System.Drawing.Size(107, 25);
            this.btValidate.TabIndex = 2;
            this.btValidate.Text = "Select";
            this.btValidate.UseVisualStyleBackColor = true;
            // 
            // btTrain
            // 
            this.btTrain.Location = new System.Drawing.Point(883, 32);
            this.btTrain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btTrain.Name = "btTrain";
            this.btTrain.Size = new System.Drawing.Size(107, 25);
            this.btTrain.TabIndex = 2;
            this.btTrain.Text = "Select";
            this.btTrain.UseVisualStyleBackColor = true;
            // 
            // tbTest
            // 
            this.tbTest.Location = new System.Drawing.Point(88, 96);
            this.tbTest.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbTest.Name = "tbTest";
            this.tbTest.Size = new System.Drawing.Size(775, 25);
            this.tbTest.TabIndex = 1;
            // 
            // tbValidate
            // 
            this.tbValidate.Location = new System.Drawing.Point(88, 65);
            this.tbValidate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbValidate.Name = "tbValidate";
            this.tbValidate.Size = new System.Drawing.Size(775, 25);
            this.tbValidate.TabIndex = 1;
            // 
            // tbTrain
            // 
            this.tbTrain.Location = new System.Drawing.Point(88, 32);
            this.tbTrain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbTrain.Name = "tbTrain";
            this.tbTrain.Size = new System.Drawing.Size(775, 25);
            this.tbTrain.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("新細明體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.Location = new System.Drawing.Point(5, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "Test";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新細明體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(5, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "Validate";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(5, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Training";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(21, 450);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1019, 478);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lbTrainInfo);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage2.Size = new System.Drawing.Size(1011, 449);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "Train Info";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lbTrainInfo
            // 
            this.lbTrainInfo.FormattingEnabled = true;
            this.lbTrainInfo.ItemHeight = 15;
            this.lbTrainInfo.Location = new System.Drawing.Point(7, 6);
            this.lbTrainInfo.Margin = new System.Windows.Forms.Padding(4);
            this.lbTrainInfo.Name = "lbTrainInfo";
            this.lbTrainInfo.Size = new System.Drawing.Size(993, 424);
            this.lbTrainInfo.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.splitContainerTest);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage3.Size = new System.Drawing.Size(1011, 449);
            this.tabPage3.TabIndex = 1;
            this.tabPage3.Text = "Test Result";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // splitContainerTest
            // 
            this.splitContainerTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerTest.Location = new System.Drawing.Point(3, 2);
            this.splitContainerTest.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainerTest.Name = "splitContainerTest";
            // 
            // splitContainerTest.Panel1
            // 
            this.splitContainerTest.Panel1.Controls.Add(this.lbTestResult);
            // 
            // splitContainerTest.Panel2
            // 
            this.splitContainerTest.Panel2.Controls.Add(this.panelImageView);
            this.splitContainerTest.Panel2.Controls.Add(this.panelNavigation);
            this.splitContainerTest.Size = new System.Drawing.Size(1005, 445);
            this.splitContainerTest.SplitterDistance = 374;
            this.splitContainerTest.SplitterWidth = 5;
            this.splitContainerTest.TabIndex = 0;
            // 
            // lbTestResult
            // 
            this.lbTestResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbTestResult.Font = new System.Drawing.Font("Consolas", 9F);
            this.lbTestResult.FormattingEnabled = true;
            this.lbTestResult.HorizontalScrollbar = true;
            this.lbTestResult.ItemHeight = 18;
            this.lbTestResult.Location = new System.Drawing.Point(0, 0);
            this.lbTestResult.Margin = new System.Windows.Forms.Padding(4);
            this.lbTestResult.Name = "lbTestResult";
            this.lbTestResult.ScrollAlwaysVisible = true;
            this.lbTestResult.Size = new System.Drawing.Size(374, 445);
            this.lbTestResult.TabIndex = 0;
            this.lbTestResult.SelectedIndexChanged += new System.EventHandler(this.lbTestResult_SelectedIndexChanged);
            // 
            // panelImageView
            // 
            this.panelImageView.Controls.Add(this.pbTestImage);
            this.panelImageView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelImageView.Location = new System.Drawing.Point(0, 0);
            this.panelImageView.Margin = new System.Windows.Forms.Padding(4);
            this.panelImageView.Name = "panelImageView";
            this.panelImageView.Size = new System.Drawing.Size(626, 407);
            this.panelImageView.TabIndex = 1;
            // 
            // pbTestImage
            // 
            this.pbTestImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbTestImage.Location = new System.Drawing.Point(0, 0);
            this.pbTestImage.Margin = new System.Windows.Forms.Padding(4);
            this.pbTestImage.Name = "pbTestImage";
            this.pbTestImage.Size = new System.Drawing.Size(626, 407);
            this.pbTestImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbTestImage.TabIndex = 0;
            this.pbTestImage.TabStop = false;
            // 
            // panelNavigation
            // 
            this.panelNavigation.Controls.Add(this.btSaveModel);
            this.panelNavigation.Controls.Add(this.lblImageInfo);
            this.panelNavigation.Controls.Add(this.btNextImage);
            this.panelNavigation.Controls.Add(this.btPrevImage);
            this.panelNavigation.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelNavigation.Location = new System.Drawing.Point(0, 407);
            this.panelNavigation.Margin = new System.Windows.Forms.Padding(4);
            this.panelNavigation.Name = "panelNavigation";
            this.panelNavigation.Size = new System.Drawing.Size(626, 38);
            this.panelNavigation.TabIndex = 0;
            // 
            // btSaveModel
            // 
            this.btSaveModel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btSaveModel.Location = new System.Drawing.Point(509, 4);
            this.btSaveModel.Margin = new System.Windows.Forms.Padding(4);
            this.btSaveModel.Name = "btSaveModel";
            this.btSaveModel.Size = new System.Drawing.Size(113, 29);
            this.btSaveModel.TabIndex = 3;
            this.btSaveModel.Text = "Save Model";
            this.btSaveModel.UseVisualStyleBackColor = true;
            this.btSaveModel.Click += new System.EventHandler(this.btSaveModel_Click);
            // 
            // lblImageInfo
            // 
            this.lblImageInfo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblImageInfo.Location = new System.Drawing.Point(201, 8);
            this.lblImageInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblImageInfo.Name = "lblImageInfo";
            this.lblImageInfo.Size = new System.Drawing.Size(224, 22);
            this.lblImageInfo.TabIndex = 2;
            this.lblImageInfo.Text = "0 / 0";
            this.lblImageInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btNextImage
            // 
            this.btNextImage.Location = new System.Drawing.Point(112, 4);
            this.btNextImage.Margin = new System.Windows.Forms.Padding(4);
            this.btNextImage.Name = "btNextImage";
            this.btNextImage.Size = new System.Drawing.Size(100, 29);
            this.btNextImage.TabIndex = 1;
            this.btNextImage.Text = "Next >";
            this.btNextImage.UseVisualStyleBackColor = true;
            this.btNextImage.Click += new System.EventHandler(this.btNextImage_Click);
            // 
            // btPrevImage
            // 
            this.btPrevImage.Location = new System.Drawing.Point(4, 4);
            this.btPrevImage.Margin = new System.Windows.Forms.Padding(4);
            this.btPrevImage.Name = "btPrevImage";
            this.btPrevImage.Size = new System.Drawing.Size(100, 29);
            this.btPrevImage.TabIndex = 0;
            this.btPrevImage.Text = "< Previous";
            this.btPrevImage.UseVisualStyleBackColor = true;
            this.btPrevImage.Click += new System.EventHandler(this.btPrevImage_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1104, 941);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.tcControl);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.tcControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.gbEnhance.ResumeLayout(false);
            this.gbEnhance.PerformLayout();
            this.gbModel.ResumeLayout(false);
            this.gbModel.PerformLayout();
            this.gbPath.ResumeLayout(false);
            this.gbPath.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.splitContainerTest.Panel1.ResumeLayout(false);
            this.splitContainerTest.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTest)).EndInit();
            this.splitContainerTest.ResumeLayout(false);
            this.panelImageView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbTestImage)).EndInit();
            this.panelNavigation.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox gbPath;
        private System.Windows.Forms.TextBox tbTest;
        private System.Windows.Forms.TextBox tbValidate;
        private System.Windows.Forms.TextBox tbTrain;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btTest;
        private System.Windows.Forms.Button btValidate;
        private System.Windows.Forms.Button btTrain;
        private System.Windows.Forms.GroupBox gbModel;
        private System.Windows.Forms.TextBox tbHeight;
        private System.Windows.Forms.TextBox tbWidth;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbBatch;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbEpoch;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btLabel;
        private System.Windows.Forms.GroupBox gbEnhance;
        private System.Windows.Forms.TextBox tbLighting;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btTraining;
        private System.Windows.Forms.Button btModeling;
        private System.Windows.Forms.Button btResult;
        private System.Windows.Forms.Button btTesting;
        private System.Windows.Forms.Button btTerminate;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ListBox lbTrainInfo;
        private System.Windows.Forms.SplitContainer splitContainerTest;
        private System.Windows.Forms.ListBox lbTestResult;
        private System.Windows.Forms.Panel panelImageView;
        private System.Windows.Forms.PictureBox pbTestImage;
        private System.Windows.Forms.Panel panelNavigation;
        private System.Windows.Forms.Label lblImageInfo;
        private System.Windows.Forms.Button btPrevImage;
        private System.Windows.Forms.Button btNextImage;
        private System.Windows.Forms.Button btSaveModel;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tbContrast;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tbGamma;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox tbRotate;
    }
}

