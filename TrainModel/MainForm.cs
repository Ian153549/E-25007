using NSAA_16Axis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrainModel;
using System.Globalization;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using static NSAA_16Axis.DialogLabelImage;

namespace Vino
{
    public partial class MainForm : Form
    {
        private Process trainingProcess;
        private CancellationTokenSource trainingCancellation;
        private StringBuilder errorOutput = new StringBuilder();
        private StringBuilder standardOutput = new StringBuilder();
        private int lastEpochNumber = -1;
        private bool headerDisplayed = false;
        private bool validationHeaderDisplayed = false;
        private bool isFinalValidation = false;
        private bool stopAfterCurrentEpoch = false;
        private int totalEpochs = 0;
        private List<string> testResultImages = new List<string>();
        private int currentTestImageIndex = -1;
        private string latestPredictPath = string.Empty;
        private string bakModelPath = string.Empty;
        private MenuStrip menuStrip;
        private ToolStripMenuItem languageMenu;
        private ToolStripMenuItem menuEnglish;
        private ToolStripMenuItem menuTraditionalChinese;
        private ToolStripMenuItem menuSimplifiedChinese;
        private StreamWriter _logWriter;
        private const string AugSuffix = "_aug";
        private const double ConfOkThreshold = 0.80;
        private const double ConfReviewThreshold = 0.50;
        private int _progressLineCount = 0;
        private const int AugEveryN = 1; 
        private Stopwatch _trainSw;
        private Stopwatch _epochSw;
        private const string TrainDevice = "";
        private int _trainImageCount = 0;
        private int _trainBatch = 1;
        private Stopwatch _stepSw;
        private const bool ForceTrainBaselineMode = false; // true=強制速度基準條件
        private const string ForcedCacheMode = "cache=disk"; // 固定使用 disk cache
        private int _trainBatchCount = 0;
        private readonly StringBuilder _testResultLogBuffer = new StringBuilder();
        private readonly object _testResultLogLock = new object();

        private CorePerformanceMetrics _latestCoreMetrics = null;
        private const double StabilityMap50Threshold = 0.995;
        private CorePerformanceMetrics _currentTestingMetrics;
        private readonly List<double> _perImageInferenceMs = new List<double>();
        private readonly object _metricsLock = new object();
        private const int YoloTimeoutMs = 30 * 60 * 1000; // 單次 YOLO 程序逾時 30 分鐘
        public MainForm()
        {
            InitializeComponent();
            InitializeLanguageMenu();
            LoadPathsFromConfig();
            lbTrainInfo.Font = new Font("Consolas", 9F, FontStyle.Regular);
            lbTestResult.Font = new Font("Consolas", 9F, FontStyle.Regular);

            // 載入語言設定
            GV.AppSettingParm = AppSettingParm.LoadFromJson();
            GV.LoadLanguageFromConfig();
            ApplyLanguage();
            btTrain.Visible = true;
            btValidate.Visible = true;
            btTest.Visible = true;
            btResult.Visible = true;
            btResult.Click -= btResult_Click;
            btResult.Click += btResult_Click;
            btTrain.Click += btTrain_Click;
            btValidate.Click += btValidate_Click;
            btTest.Click += btTest_Click;
            this.FormClosing += (s, e) =>
            {
                try
                {
                    if (trainingProcess != null && !trainingProcess.HasExited)
                    {
                        KillProcessTree(trainingProcess);
                    }
                }
                catch { }
            };
        }

        private void btTrain_Click(object sender, EventArgs e)
        {
            SelectFolderToTextBox(tbTrain, "Select");
        }

        private void btValidate_Click(object sender, EventArgs e)
        {
            SelectFolderToTextBox(tbValidate, "Select");
        }

        private void btTest_Click(object sender, EventArgs e)
        {
            SelectFolderToTextBox(tbTest, "Select");
        }

        private void InitializeLanguageMenu()
        {
            menuStrip = new MenuStrip();
            languageMenu = new ToolStripMenuItem();
            menuEnglish = new ToolStripMenuItem();
            menuTraditionalChinese = new ToolStripMenuItem();
            menuSimplifiedChinese = new ToolStripMenuItem();

            // Language Menu
            languageMenu.Text = LanguageResource.Get("Language");

            menuEnglish.Text = "English";
            menuEnglish.Click += (s, e) => ChangeLanguage(LanguageType.English);

            menuTraditionalChinese.Text = "繁體中文";
            menuTraditionalChinese.Click += (s, e) => ChangeLanguage(LanguageType.TraditionalChinese);

            menuSimplifiedChinese.Text = "简体中文";
            menuSimplifiedChinese.Click += (s, e) => ChangeLanguage(LanguageType.SimplifiedChinese);

            languageMenu.DropDownItems.AddRange(new ToolStripItem[] {
                menuEnglish, menuTraditionalChinese, menuSimplifiedChinese
            });

            menuStrip.Items.Add(languageMenu);
            menuStrip.Location = new Point(0, 0);
            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);

            // 調整其他控制項位置
            tcControl.Location = new Point(16, menuStrip.Height + 5);
            tabControl1.Location = new Point(16, tcControl.Bottom + 10);
        }

        private void SelectFolderToTextBox(TextBox targetTextBox, string titleKey)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = LanguageResource.Get(titleKey);
                dialog.ShowNewFolderButton = true;

                if (!string.IsNullOrWhiteSpace(targetTextBox.Text) &&
                    System.IO.Directory.Exists(targetTextBox.Text))
                {
                    dialog.SelectedPath = targetTextBox.Text;
                }

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    targetTextBox.Text = dialog.SelectedPath;
                }
            }
        }

        private void ChangeLanguage(LanguageType language)
        {
            GV.SetLanguage(language);
            GV.AppSettingParm.SaveToJson();
            ApplyLanguage();
            UpdateLanguageMenuChecks();
        }

        private void UpdateLanguageMenuChecks()
        {
            menuEnglish.Checked = GV.CurrentLanguage == LanguageType.English;
            menuTraditionalChinese.Checked = GV.CurrentLanguage == LanguageType.TraditionalChinese;
            menuSimplifiedChinese.Checked = GV.CurrentLanguage == LanguageType.SimplifiedChinese;
        }

        private void ApplyLanguage()
        {
            // Form Title
            this.Text = LanguageResource.Get("FormTitle");

            // Language Menu
            languageMenu.Text = LanguageResource.Get("Language");

            // Tab Pages
            tabPage1.Text = LanguageResource.Get("BasicInfo");
            tabPage2.Text = LanguageResource.Get("TrainInfo");
            tabPage3.Text = LanguageResource.Get("TestResult");

            // GroupBoxes
            gbPath.Text = LanguageResource.Get("Path");
            gbModel.Text = LanguageResource.Get("ModelSetting");
            gbEnhance.Text = LanguageResource.Get("ImageEnhancement");

            // Labels
            label1.Text = LanguageResource.Get("Training");
            label2.Text = LanguageResource.Get("Validate");
            label3.Text = LanguageResource.Get("Test");
            label4.Text = LanguageResource.Get("Image");
            label5.Text = LanguageResource.Get("Width");
            label6.Text = LanguageResource.Get("Height");
            label7.Text = LanguageResource.Get("BatchSize");
            label8.Text = LanguageResource.Get("Epoch");
            label9.Text = LanguageResource.Get("LightingAdjust");
            label10.Text = LanguageResource.Get("ContrastAdjustment");
            label11.Text = LanguageResource.Get("Gamma");
            label12.Text = LanguageResource.Get("Rotate");

            // Buttons
            btTrain.Text = LanguageResource.Get("Select");
            btValidate.Text = LanguageResource.Get("Select");
            btTest.Text = LanguageResource.Get("Select");
            btLabel.Text = LanguageResource.Get("ImageLabel");
            btTraining.Text = LanguageResource.Get("ModelTraining");
            btTerminate.Text = LanguageResource.Get("TrainingTerminate");
            btTesting.Text = LanguageResource.Get("ModelTesting");
            btResult.Text = LanguageResource.Get("TestingResult");
            btModeling.Text = LanguageResource.Get("ModelGenerate");
            btPrevImage.Text = LanguageResource.Get("Previous");
            btNextImage.Text = LanguageResource.Get("Next");
            btSaveModel.Text = LanguageResource.Get("SaveModel");

            UpdateLanguageMenuChecks();
        }
        private void btLabel_Click(object sender, EventArgs e)
        {
            ////DialogLabelImage labelImage = new DialogLabelImage();
            ////labelImage.ShowDialog();
            //string reloadError;
            //if (!TryReloadDatasetContextFromTrainPath(out reloadError))
            //{
            //    MessageBox.Show(reloadError, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}

            //using (DialogLabelImage labelImage = new DialogLabelImage(GetCurrentRecipeNumber()))
            //{
            //    labelImage.ShowDialog();
            //}

            //    // 標記視窗可能更新了 AIConfig/data.yaml，關閉後再同步一次
            //    TryReloadDatasetPathsAndInfo(true);
            string labelImageDir;
            string error;
            if (!TryGetLabelImageDirFromTrainPath(out labelImageDir, out error))
            {
                MessageBox.Show(error, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (DialogLabelImage labelImage = new DialogLabelImage())
            {
                labelImage.LabelImageDirOverride = labelImageDir;   // ...\Train\1
                labelImage.ShowDialog();
            }

            // 標記後同步 data.yaml / 路徑資訊
            TryReloadDatasetPathsAndInfo(true);
        }
        private bool TryGetLabelImageDirFromTrainPath(out string labelImageDir, out string error)
        {
            labelImageDir = string.Empty;
            error = string.Empty;

            string trainPath = tbTrain.Text.Trim();
            if (string.IsNullOrEmpty(trainPath) || !Directory.Exists(trainPath))
            {
                error = "訓練集路徑無效，請先選擇正確資料夾。";
                return false;
            }

            DirectoryInfo dir = new DirectoryInfo(trainPath);
            while (dir != null)
            {
                if (string.Equals(dir.Name, "dataset", StringComparison.OrdinalIgnoreCase))
                {
                    if (dir.Parent == null)
                    {
                        break;
                    }
                    labelImageDir = dir.Parent.FullName;   // ...\Train\1
                    return true;
                }
                dir = dir.Parent;
            }

            error = "訓練集路徑中找不到 dataset 資料夾：" + trainPath;
            return false;
        }
        private int GetCurrentRecipeNumber()
        {
            if (GV.AIConfig != null && GV.AIConfig.LabeledRecipeNumber > 0)
            {
                return GV.AIConfig.LabeledRecipeNumber;
            }
            return 1;
        }

        private bool TryReloadDatasetContextFromTrainPath(out string error)
        {
            error = string.Empty;

            string trainPath = tbTrain.Text.Trim();
            if (string.IsNullOrEmpty(trainPath) || !Directory.Exists(trainPath))
            {
                error = "訓練集路徑無效，請先選擇正確路徑。";
                return false;
            }

            // trainPath: ...\dataset\images\train
            DirectoryInfo trainDir = new DirectoryInfo(trainPath);
            DirectoryInfo imagesDir = trainDir.Parent;
            DirectoryInfo datasetDir = imagesDir != null ? imagesDir.Parent : null;
            if (datasetDir == null)
            {
                error = "無法由訓練集路徑推導 dataset 資料夾。";
                return false;
            }

            string dataYamlPath = Path.Combine(datasetDir.FullName, "data.yaml");
            if (!File.Exists(dataYamlPath))
            {
                error = "找不到 data.yaml：" + dataYamlPath;
                return false;
            }

            string yamlBaseDir = Path.GetDirectoryName(dataYamlPath);
            string yamlTrain = GetYamlValue(dataYamlPath, "train");
            string yamlVal = GetYamlValue(dataYamlPath, "val");
            string yamlTest = GetYamlValue(dataYamlPath, "test");

            string resolvedTrain = ResolveYamlPath(yamlBaseDir, yamlTrain);
            string resolvedVal = ResolveYamlPath(yamlBaseDir, yamlVal);
            string resolvedTest = ResolveYamlPath(yamlBaseDir, yamlTest);

            if (Directory.Exists(resolvedTrain)) tbTrain.Text = resolvedTrain;
            if (Directory.Exists(resolvedVal)) tbValidate.Text = resolvedVal;
            if (Directory.Exists(resolvedTest)) tbTest.Text = resolvedTest;

            if (GV.AIConfig != null)
            {
                GV.AIConfig.TrainPath = yamlBaseDir; // dataset 根目錄
                GV.AIConfig.trainBasePath = Directory.GetParent(yamlBaseDir).FullName; // ...\Train\{recipe}
            }

            return true;
        }
        private bool TryReloadDatasetPathsAndInfo(bool writeLogToListBox)
        {
            try
            {
                string dataYamlPath;
                string reason;
                if (!TryFindAvailableDataYaml(out dataYamlPath, out reason))
                {
                    if (writeLogToListBox)
                    {
                        UpdateTrainInfo("[資料集] 未找到可用 data.yaml：" + reason);
                    }
                    return false;
                }

                string yamlBaseDir = Path.GetDirectoryName(dataYamlPath); // 預期為 ...\dataset
                string yamlTrain = GetYamlValue(dataYamlPath, "train");
                string yamlVal = GetYamlValue(dataYamlPath, "val");
                string yamlTest = GetYamlValue(dataYamlPath, "test");

                string trainPath = ResolveYamlPath(yamlBaseDir, yamlTrain);
                string valPath = ResolveYamlPath(yamlBaseDir, yamlVal);
                string testPath = ResolveYamlPath(yamlBaseDir, yamlTest);

                // 若 yaml 裡是相對路徑，會轉成絕對路徑；若資料夾存在就同步到 UI
                if (Directory.Exists(trainPath)) tbTrain.Text = trainPath;
                if (Directory.Exists(valPath)) tbValidate.Text = valPath;
                if (Directory.Exists(testPath)) tbTest.Text = testPath;

                // 同步回 AIConfig（供訓練、測試、結果解析使用）
                if (GV.AIConfig != null)
                {
                    GV.AIConfig.TrainPath = yamlBaseDir;
                }

                if (writeLogToListBox)
                {
                    UpdateTrainInfo("[資料集] 已重新載入 data.yaml");
                    UpdateTrainInfo("[資料集] data.yaml: " + dataYamlPath);
                    UpdateTrainInfo("[資料集] train: " + tbTrain.Text);
                    UpdateTrainInfo("[資料集] val: " + tbValidate.Text);
                    UpdateTrainInfo("[資料集] test: " + tbTest.Text);
                }

                return true;
            }
            catch (Exception ex)
            {
                if (writeLogToListBox)
                {
                    UpdateTrainInfo("[資料集] 重載失敗: " + ex.Message);
                }
                return false;
            }
        }
        private bool TryFindAvailableDataYaml(out string dataYamlPath, out string reason)
        {
            dataYamlPath = string.Empty;
            reason = string.Empty;

            List<string> candidates = new List<string>();

            // A. 從目前 UI 的 train 路徑反推 ...\dataset\data.yaml
            string uiTrain = tbTrain.Text.Trim();
            if (!string.IsNullOrEmpty(uiTrain) && Directory.Exists(uiTrain))
            {
                DirectoryInfo trainDir = new DirectoryInfo(uiTrain);           // ...\images\train
                DirectoryInfo imagesDir = trainDir.Parent;                     // ...\images
                DirectoryInfo datasetDir = imagesDir != null ? imagesDir.Parent : null; // ...\dataset
                if (datasetDir != null)
                {
                    candidates.Add(Path.Combine(datasetDir.FullName, "data.yaml"));
                }
            }

            // B. 從 AIConfig.TrainPath
            if (GV.AIConfig != null && !string.IsNullOrEmpty(GV.AIConfig.TrainPath))
            {
                candidates.Add(Path.Combine(GV.AIConfig.TrainPath, "data.yaml"));
            }

            // C. 從執行目錄推導（單獨啟動 TrainModel 時常用）
            int recipeNumber = 1;
            if (GV.AIConfig != null && GV.AIConfig.LabeledRecipeNumber > 0)
            {
                recipeNumber = GV.AIConfig.LabeledRecipeNumber;
            }
            //else if (GV.NowRecipeNumber > 0)
            //{
            //    recipeNumber = GV.NowRecipeNumber;
            //}
            //else if (GV.AIConfig != null && GV.AIConfig.LabeledRecipeNumber > 0)
            //{
            //    recipeNumber = GV.AIConfig.LabeledRecipeNumber;
            //}

            string defaultDataset = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Python", "Train", recipeNumber.ToString(), "dataset");
            candidates.Add(Path.Combine(defaultDataset, "data.yaml"));

            foreach (string candidate in candidates.Where(p => !string.IsNullOrWhiteSpace(p)).Distinct(StringComparer.OrdinalIgnoreCase))
            {
                if (File.Exists(candidate))
                {
                    dataYamlPath = candidate;
                    return true;
                }
            }

            reason = "候選路徑皆不存在。";
            return false;
        }

        private void LoadPathsFromConfig()
        {
            if (GV.AIConfig != null && !string.IsNullOrEmpty(GV.AIConfig.TrainPath))
            {
                string basePath = GV.AIConfig.TrainPath;
                tbTrain.Text = Path.Combine(basePath, "images", "train");
                tbValidate.Text = Path.Combine(basePath, "images", "val");
                tbTest.Text = Path.Combine(basePath, "images", "test");
            }

            // 備份目前的模型到 bak 資料夾
            BackupCurrentModelToBak();
        }
        private void BackupCurrentModelToBak()
        {
            try
            {
                string modelDir = GV.AIConfig.ModelPath;
                if (string.IsNullOrEmpty(modelDir))
                {
                    modelDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "models");
                }

                string modelName = $"best{GV.AIConfig.LabeledRecipeNumber}.pt";
                string currentModelPath = Path.Combine(modelDir, modelName);

                if (!File.Exists(currentModelPath))
                {
                    Debug.WriteLine($"[Backup] 模型不存在，跳過備份: {currentModelPath}");
                    return;
                }

                // 建立 bak 資料夾
                string bakDir = Path.Combine(modelDir, "bak");
                if (!Directory.Exists(bakDir))
                {
                    Directory.CreateDirectory(bakDir);
                }

                // 備份檔案路徑
                bakModelPath = Path.Combine(bakDir, modelName);

                // 複製模型到 bak 資料夾
                File.Copy(currentModelPath, bakModelPath, true);
                Debug.WriteLine($"[Backup] 模型已備份到: {bakModelPath}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Backup] 備份模型失敗: {ex.Message}");
            }
        }
        private void btSaveModel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "是否要保留目前訓練的模型？\n\n" +
                "• [是] - 保留目前的模型\n" +
                "• [否] - 還原為先前的模型",
                "儲存模型",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // 保留目前模型，刪除 bak 中的備份
                try
                {
                    if (!string.IsNullOrEmpty(bakModelPath) && File.Exists(bakModelPath))
                    {
                        File.Delete(bakModelPath);
                        Debug.WriteLine($"[Save] 已刪除備份: {bakModelPath}");
                    }
                    MessageBox.Show("已保留目前的模型。", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"刪除備份時發生錯誤：{ex.Message}", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                // 還原先前的模型
                RestorePreviousModel();
            }
        }

        private void RestorePreviousModel()
        {
            try
            {
                if (string.IsNullOrEmpty(bakModelPath) || !File.Exists(bakModelPath))
                {
                    MessageBox.Show("找不到備份的模型檔案，無法還原。", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string modelDir = GV.AIConfig.ModelPath;
                if (string.IsNullOrEmpty(modelDir))
                {
                    modelDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "models");
                }

                string modelName = $"best{GV.AIConfig.LabeledRecipeNumber}.pt";
                string targetModelPath = Path.Combine(modelDir, modelName);

                // 從 bak 複製回 ModelPath
                File.Copy(bakModelPath, targetModelPath, true);
                Debug.WriteLine($"[Restore] 模型已還原: {targetModelPath}");

                // 刪除 bak 中的備份
                File.Delete(bakModelPath);
                bakModelPath = string.Empty;

                MessageBox.Show("已還原為先前的模型。", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"還原模型時發生錯誤：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btModeling_Click(object sender, EventArgs e)
        {
//            string newClass = DialogLabelImage.tbNewLabel.Text.Trim();
//            if (string.IsNullOrEmpty(newClass) && !comboBoxClass.Items.Contains(newClass))
//            {
//                GV.AIClassList.AddClass(newClass);
//                comboBoxClass.Items.Add(newClass);
//            }
//            string srcImgDir = imageDir;
//            string projectDir = Path.Combine(srcImgDir, "..", "dataset");
//            string[] allImages = Directory.GetFiles(srcImgDir, "*.jpg")
//                .Concat(Directory.GetFiles(srcImgDir, "*.png"))
//                .Concat(Directory.GetFiles(srcImgDir, "*.bmp"))
//                .ToArray();
//            if (allImages.Length == 0)
//            {
//                MessageBox.Show("找不到任何圖片檔案！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                return;
//            }
//            int valCount = allImages.Length / 5;
//            var rand = new Random();
//            var shuffled = allImages.OrderBy(x => rand.Next()).ToList();
//            var trainImgs = shuffled.Take(allImages.Length - valCount).ToList();
//            var valImgs = shuffled.Skip(allImages.Length - valCount).ToList();
//            string[] folders =
//            {
//                "images/train","images/val","labels/train","labels/val"
//            };
//            foreach (var folder in folders)
//                Directory.CreateDirectory(Path.Combine(projectDir, folder));
//            MoveImgAndLabel(trainImgs, "images/train", "labels/train", projectDir);
//            MoveImgAndLabel(valImgs, "images/val", "labels/val", projectDir);
//            int nc = comboBoxClass.Items.Count;
//            var names = string.Join(", ", comboBoxClass.Items.Cast<string>().Select(n => $"'{n}'"));
//            string yaml =
//        $@"train: ./images/train
//val: ./images/val
//nc: {nc}
//names: [{names}]
//";
//            File.WriteAllText(Path.Combine(projectDir, "data.yaml"), yaml);
//            MessageBox.Show("資料集已整理、data.yaml已建立！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        void MoveImgAndLabel(List<string> imgs, string imgDst, string labelDst, string projectDir)
        {
            foreach (var imgPath in imgs)
            {
                string name = Path.GetFileName(imgPath);
                string txtPath = Path.ChangeExtension(imgPath, ".txt");
                File.Copy(imgPath, Path.Combine(projectDir, imgDst, name), true);
                if (File.Exists(txtPath))
                    File.Copy(txtPath, Path.Combine(projectDir, labelDst, Path.GetFileName(txtPath)), true);
            }
        }

        //private async void btTraining_Click(object sender, EventArgs e)
        //{
        //    bool wasTerminatedByUser = false;  // 追蹤是否為使用者終止
        //    try
        //    {
        //        string logDir = Path.Combine(@"C:\Log", DateTime.Now.ToString("yyyyMMdd"));
        //        Directory.CreateDirectory(logDir);
        //        string logFile = Path.Combine(logDir,
        //            $"TrainModel_{DateTime.Now:HHmmss}.log");
        //        _logWriter = new StreamWriter(logFile, append: false, encoding: System.Text.Encoding.UTF8)
        //        {
        //            AutoFlush = true
        //        };
        //        _logWriter.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ===== 訓練開始 =====");
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine($"[Log] 無法建立 log 檔案: {ex.Message}");
        //        // log 失敗不影響訓練，繼續執行
        //    }
        //    try
        //    {
        //        lbTrainInfo.Items.Clear();
        //        _progressLineCount = 0;
        //        lastEpochNumber = -1;
        //        headerDisplayed = false;
        //        validationHeaderDisplayed = false;
        //        isFinalValidation = false;
        //        stopAfterCurrentEpoch = false;
        //        // 1. 驗證輸入參數
        //        if (!ValidateTrainingInputs(out string errorMessage))
        //        {
        //            MessageBox.Show(errorMessage, "輸入錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            return;
        //        }

        //        // 2. 獲取訓練參數
        //        int width = int.Parse(tbWidth.Text.Trim());
        //        int height = int.Parse(tbHeight.Text.Trim());
        //        int batch = int.Parse(tbBatch.Text.Trim());
        //        int epoch = int.Parse(tbEpoch.Text.Trim());
        //        string lightingText = tbLighting.Text.Trim();
        //        int lighting = 0;
        //        bool hasLighting = !string.IsNullOrEmpty(lightingText) && int.TryParse(lightingText, out lighting) && lighting != 0;
        //        totalEpochs = epoch;
        //        // 3. 使用較大的值作為 imgsz (YOLO 訓練通常使用正方形或使用長邊)
        //        int imgsz = Math.Max(width, height);

        //        // 4. 準備模型路徑
        //        string modelPath = PrepareModelPath();

        //        // 5. 準備 data.yaml 路徑
        //        //string dataYamlPath = Path.Combine(GV.AIConfig.TrainPath, "data.yaml");
        //        //if (!File.Exists(dataYamlPath))
        //        //{
        //        //    MessageBox.Show($"找不到 data.yaml 檔案：{dataYamlPath}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        //    return;
        //        //}
        //        string dataYamlPath;
        //        if (!TryReloadDataYamlFromCurrentDataset(out dataYamlPath, out string yamlError))
        //        {
        //            MessageBox.Show(yamlError, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return;
        //        }

        //        // 6. 建立訓練命令
        //        StringBuilder cmdBuilder = new StringBuilder();
        //        cmdBuilder.Append($"task=detect mode=train model=\"{modelPath}\" data=\"{dataYamlPath}\" workers=1 cache=disk ");
        //        cmdBuilder.Append($"epochs={epoch} imgsz={imgsz} rect=True batch={batch}");
        //        if (!string.IsNullOrEmpty(TrainDevice))
        //        {
        //            cmdBuilder.Append(" device=" + TrainDevice);
        //        }

        //        if (hasLighting)
        //        {
        //            cmdBuilder.Append($" hsv_v={lighting / 100.0}"); // 亮度調整參數
        //        }
        //        double rotateDeg = double.Parse(tbRotate.Text.Trim(), CultureInfo.InvariantCulture);
        //        if (rotateDeg > 0)
        //        {
        //            cmdBuilder.Append(" degrees=" + rotateDeg.ToString("0.##", CultureInfo.InvariantCulture));
        //        }

        //        // 對比 / Gamma -> 產生 _aug 增強副本
        //        double contrastPct = double.Parse(tbContrast.Text.Trim(), CultureInfo.InvariantCulture);
        //        double gammaVal = double.Parse(tbGamma.Text.Trim(), CultureInfo.InvariantCulture);

        //        string trainCommand = cmdBuilder.ToString();

        //        // 7. 確認開始訓練
        //        DialogResult result = MessageBox.Show(
        //            $"訓練參數：\n\n" +
        //            $"圖片尺寸: {imgsz}\n" +
        //            $"批次大小: {batch}\n" +
        //            $"訓練週期: {epoch}\n" +
        //            (hasLighting ? $"亮度調整: {lighting}%\n" : "") + $"對比: {contrastPct}%  Gamma: {gammaVal}  旋轉: ±{rotateDeg}°\n" +
        //            $"\n是否開始訓練？",
        //            "確認訓練",
        //            MessageBoxButtons.YesNo,
        //            MessageBoxIcon.Question);

        //        if (result != DialogResult.Yes)
        //        {
        //            return;
        //        }

        //        // 8. 禁用訓練按鈕，啟用終止按鈕
        //        btTraining.Enabled = false;
        //        btTerminate.Enabled = true;

        //        string trainPathForAug = tbTrain.Text.Trim();
        //        _trainBatch = batch;
        //        _trainImageCount = Directory.GetFiles(trainPathForAug)
        //            .Count(f =>
        //            {
        //                string x = Path.GetExtension(f).ToLowerInvariant();
        //                return x == ".jpg" || x == ".png" || x == ".bmp";
        //            });
        //        bool needAug = Math.Abs(contrastPct) >= 0.001 || Math.Abs(gammaVal - 1.0) >= 0.001;
        //        System.Diagnostics.Stopwatch swAug = System.Diagnostics.Stopwatch.StartNew();
        //        if (needAug)
        //        {
        //            UpdateTrainInfo("[影像增強] 開始產生增強副本...");
        //            int augCount = await Task.Run(() => AugmentTrainImages(trainPathForAug, contrastPct, gammaVal));
        //            UpdateTrainInfo($"[影像增強] 完成：{augCount} 張 (Contrast={contrastPct}%, Gamma={gammaVal})，耗時 {swAug.ElapsedMilliseconds} ms");
        //        }
        //        else
        //        {
        //            // 沒設定增強：只清除舊副本，不跑完整流程
        //            await Task.Run(() => RemoveOldAugFiles(trainPathForAug));
        //            UpdateTrainInfo($"[影像增強] 未設定，已略過 (清除舊副本耗時 {swAug.ElapsedMilliseconds} ms)");
        //        }
        //        swAug.Stop();

        //        // 9. 執行訓練
        //        string cacheMode = await Task.Run(() => DecideCacheMode(trainPathForAug));
        //        trainCommand = trainCommand.Replace("cache=disk", cacheMode);
        //        UpdateTrainInfo("[訓練命令] " + trainCommand);
        //        try
        //        {
        //            await StartTrainingAsync(trainCommand);
        //            if (_trainSw != null)
        //            {
        //                UpdateTrainInfo($"[計時] 訓練總耗時 {_trainSw.Elapsed.TotalMinutes:F1} 分鐘");
        //            }
        //        }
        //        catch (OperationCanceledException)
        //        {
        //            // 使用者終止訓練，不視為錯誤
        //            wasTerminatedByUser = true;
        //        }

        //        // 10. 訓練完成後，處理模型檔案
        //        HandleTrainedModel();

        //        if (wasTerminatedByUser || stopAfterCurrentEpoch)
        //        {
        //            MessageBox.Show($"訓練已在 Epoch {lastEpochNumber} 後停止！\n模型已儲存。", "訓練終止", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        }
        //        else
        //        {
        //            MessageBox.Show("訓練完成！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        }
        //        //MessageBox.Show("訓練完成！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    }
        //    catch (Exception ex)
        //    {
        //        //MessageBox.Show($"訓練過程發生錯誤：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        if (stopAfterCurrentEpoch)
        //        {
        //            try
        //            {
        //                HandleTrainedModel();
        //                MessageBox.Show($"訓練已在 Epoch {lastEpochNumber} 後停止！\n模型已儲存。", "訓練終止", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            }
        //            catch
        //            {
        //                MessageBox.Show($"訓練過程發生錯誤：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show($"訓練過程發生錯誤：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //    }
        //    finally
        //    {
        //        btTraining.Enabled = true;
        //        btTerminate.Enabled = false;
        //        stopAfterCurrentEpoch = false;
        //        try
        //        {
        //            if (_logWriter != null)
        //            {
        //                _logWriter.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ===== 訓練結束 =====");
        //                _logWriter.Close();
        //                _logWriter.Dispose();
        //                _logWriter = null;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Debug.WriteLine($"[Log] 關閉 log 失敗: {ex.Message}");
        //        }
        //    }
        //}
        private async void btTraining_Click(object sender, EventArgs e)
        {
            bool wasTerminatedByUser = false;  // 追蹤是否為使用者終止
            try
            {
                string logDir = Path.Combine(@"C:\Log", DateTime.Now.ToString("yyyyMMdd"));
                Directory.CreateDirectory(logDir);
                string logFile = Path.Combine(logDir, $"TrainModel_{DateTime.Now:HHmmss}.log");
                _logWriter = new StreamWriter(logFile, append: false, encoding: System.Text.Encoding.UTF8)
                {
                    AutoFlush = true
                };
                _logWriter.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ===== 訓練開始 =====");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Log] 無法建立 log 檔案: {ex.Message}");
            }

            try
            {
                lbTrainInfo.Items.Clear();
                _progressLineCount = 0;
                lastEpochNumber = -1;
                headerDisplayed = false;
                validationHeaderDisplayed = false;
                isFinalValidation = false;
                stopAfterCurrentEpoch = false;

                if (!ValidateTrainingInputs(out string errorMessage))
                {
                    MessageBox.Show(errorMessage, "輸入錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int width = int.Parse(tbWidth.Text.Trim());
                int height = int.Parse(tbHeight.Text.Trim());
                int batch = int.Parse(tbBatch.Text.Trim());
                int epoch = int.Parse(tbEpoch.Text.Trim());
                string lightingText = tbLighting.Text.Trim();
                int lighting = 0;
                bool hasLighting = !string.IsNullOrEmpty(lightingText) && int.TryParse(lightingText, out lighting) && lighting != 0;
                totalEpochs = epoch;

                int imgsz = Math.Max(width, height);
                string modelPath = PrepareModelPath();

                string dataYamlPath;
                if (!TryReloadDataYamlFromCurrentDataset(out dataYamlPath, out string yamlError))
                {
                    MessageBox.Show(yamlError, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                double rotateDeg = double.Parse(tbRotate.Text.Trim(), CultureInfo.InvariantCulture);
                double contrastPct = double.Parse(tbContrast.Text.Trim(), CultureInfo.InvariantCulture);
                double gammaVal = double.Parse(tbGamma.Text.Trim(), CultureInfo.InvariantCulture);

                // 速度基準模式：強制最小成本
                if (ForceTrainBaselineMode)
                {
                    hasLighting = false;
                    lighting = 0;
                    rotateDeg = 0.0;
                    contrastPct = 0.0;
                    gammaVal = 1.0;
                }

                StringBuilder cmdBuilder = new StringBuilder();
                cmdBuilder.Append($"task=detect mode=train model=\"{modelPath}\" data=\"{dataYamlPath}\" workers=2 cache=disk ");
                cmdBuilder.Append($"epochs={epoch} imgsz={imgsz} rect=True batch={batch}");

                if (!string.IsNullOrEmpty(TrainDevice))
                {
                    cmdBuilder.Append(" device=" + TrainDevice);
                }

                if (hasLighting)
                {
                    cmdBuilder.Append($" hsv_v={lighting / 100.0}");
                }

                if (rotateDeg > 0)
                {
                    cmdBuilder.Append(" degrees=" + rotateDeg.ToString("0.##", CultureInfo.InvariantCulture));
                }

                string trainCommand = cmdBuilder.ToString();

                DialogResult result = MessageBox.Show(
                    $"訓練參數：\n\n" +
                    $"圖片尺寸: {imgsz}\n" +
                    $"批次大小: {batch}\n" +
                    $"訓練週期: {epoch}\n" +
                    (hasLighting ? $"亮度調整: {lighting}%\n" : "") +
                    $"對比: {contrastPct}%  Gamma: {gammaVal}  旋轉: ±{rotateDeg}°\n" +
                    (ForceTrainBaselineMode ? "\n[Baseline模式] 已強制關閉 Lighting/Rotate/Contrast/Gamma\n" : "") +
                    $"\n是否開始訓練？",
                    "確認訓練",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes) return;

                btTraining.Enabled = false;
                btTerminate.Enabled = true;

                string trainPathForAug = tbTrain.Text.Trim();
                _trainBatch = batch;
                _trainImageCount = 0;
                _trainBatchCount = 0;

                bool needAug = Math.Abs(contrastPct) >= 0.001 || Math.Abs(gammaVal - 1.0) >= 0.001;
                Stopwatch swAug = Stopwatch.StartNew();

                if (needAug)
                {
                    UpdateTrainInfo("[影像增強] 開始產生增強副本...");
                    int augCount = await Task.Run(() => AugmentTrainImages(trainPathForAug, contrastPct, gammaVal));
                    UpdateTrainInfo($"[影像增強] 完成：{augCount} 張 (Contrast={contrastPct}%, Gamma={gammaVal})，耗時 {swAug.ElapsedMilliseconds} ms");
                }
                else
                {
                    await Task.Run(() => RemoveOldAugFiles(trainPathForAug));
                    UpdateTrainInfo($"[影像增強] 未設定，已略過 (清除舊副本耗時 {swAug.ElapsedMilliseconds} ms)");
                }
                swAug.Stop();

                // 關鍵修正：增強完成後，重新計算實際訓練資料量
                RefreshTrainDatasetStatsAndLog(trainPathForAug, batch);

                // 關鍵修正：固定 cache=disk，避免變成 cache=False
                string cacheMode;
                if (ForceTrainBaselineMode)
                {
                    cacheMode = ForcedCacheMode;
                    UpdateTrainInfo("[快取評估] Baseline 模式：固定使用 cache=disk");
                }
                else
                {
                    cacheMode = await Task.Run(() => DecideCacheMode(trainPathForAug));
                    UpdateTrainInfo("[快取評估] 動態模式：" + cacheMode);
                }

                trainCommand = trainCommand.Replace("cache=disk", cacheMode);
                UpdateTrainInfo("[訓練命令] " + trainCommand);

                try
                {
                    await StartTrainingAsync(trainCommand);
                    if (_trainSw != null)
                    {
                        UpdateTrainInfo($"[計時] 訓練總耗時 {_trainSw.Elapsed.TotalMinutes:F1} 分鐘");
                    }
                }
                catch (OperationCanceledException)
                {
                    wasTerminatedByUser = true;
                }

                HandleTrainedModel();

                if (wasTerminatedByUser || stopAfterCurrentEpoch)
                {
                    MessageBox.Show($"訓練已在 Epoch {lastEpochNumber} 後停止！\n模型已儲存。", "訓練終止", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("訓練完成！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                if (stopAfterCurrentEpoch)
                {
                    try
                    {
                        HandleTrainedModel();
                        MessageBox.Show($"訓練已在 Epoch {lastEpochNumber} 後停止！\n模型已儲存。", "訓練終止", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch
                    {
                        MessageBox.Show($"訓練過程發生錯誤：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show($"訓練過程發生錯誤：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                btTraining.Enabled = true;
                btTerminate.Enabled = false;
                stopAfterCurrentEpoch = false;

                try
                {
                    if (_logWriter != null)
                    {
                        _logWriter.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ===== 訓練結束 =====");
                        _logWriter.Close();
                        _logWriter.Dispose();
                        _logWriter = null;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[Log] 關閉 log 失敗: {ex.Message}");
                }
            }
        }
        private int CountTrainImages(string trainPath)
        {
            if (string.IsNullOrWhiteSpace(trainPath) || !Directory.Exists(trainPath)) return 0;

            return Directory.GetFiles(trainPath)
                .Count(f =>
                {
                    string ext = Path.GetExtension(f).ToLowerInvariant();
                    return ext == ".jpg" || ext == ".png" || ext == ".bmp";
                });
        }

        private void RefreshTrainDatasetStatsAndLog(string trainPath, int batch)
        {
            _trainBatch = batch > 0 ? batch : 1;
            _trainImageCount = CountTrainImages(trainPath);
            _trainBatchCount = _trainImageCount > 0
                ? (int)Math.Ceiling(_trainImageCount / (double)_trainBatch)
                : 0;

            UpdateTrainInfo($"[訓練資料] {_trainImageCount} 張，batch={_trainBatch}，約 {_trainBatchCount} batches / epoch");
        }
        private void RemoveOldAugFiles(string trainPath)
        {
            if (string.IsNullOrEmpty(trainPath) || !Directory.Exists(trainPath)) return;

            DirectoryInfo trainDir = new DirectoryInfo(trainPath);
            DirectoryInfo datasetDir = trainDir.Parent != null ? trainDir.Parent.Parent : null;
            string labelDir = datasetDir != null
                ? Path.Combine(datasetDir.FullName, "labels", trainDir.Name)
                : string.Empty;

            DeleteAugArtifacts(trainPath, labelDir);
        }
        private int AugmentTrainImages(string trainPath, double contrastPct, double gamma)
        {
            if (string.IsNullOrEmpty(trainPath) || !Directory.Exists(trainPath)) return 0;

            string[] exts = { ".jpg", ".png", ".bmp" };

            // 推導 labels 資料夾: ...\dataset\images\train -> ...\dataset\labels\train
            DirectoryInfo trainDir = new DirectoryInfo(trainPath);
            DirectoryInfo imagesDir = trainDir.Parent;
            DirectoryInfo datasetDir = imagesDir != null ? imagesDir.Parent : null;
            string labelDir = datasetDir != null
                ? Path.Combine(datasetDir.FullName, "labels", trainDir.Name)
                : string.Empty;

            // 1) 清除舊的增強副本
            DeleteAugArtifacts(trainPath, labelDir);

            // 2) 無調整就不產生
            if (Math.Abs(contrastPct) < 0.001 && Math.Abs(gamma - 1.0) < 0.001) return 0;

            byte[] lut = BuildContrastGammaLut(contrastPct, gamma);
            List<string> sources = Directory.GetFiles(trainPath)
                .Where(p => exts.Contains(Path.GetExtension(p).ToLowerInvariant()))
                .Where((p, i) => i % AugEveryN == 0)
                .ToList();

            int count = 0;
            int total = sources.Count;
            ParallelOptions opt = new ParallelOptions
            {
                MaxDegreeOfParallelism = Math.Max(1, Environment.ProcessorCount / 2)
            };

            Parallel.ForEach(sources, opt, f =>
            {
                string name = Path.GetFileNameWithoutExtension(f);
                string ext = Path.GetExtension(f);
                string dst = Path.Combine(trainPath, name + AugSuffix + ext);

                using (Bitmap enhanced = ApplyLut(f, lut))
                {
                    ImageFormat fmt = ext.Equals(".png", StringComparison.OrdinalIgnoreCase) ? ImageFormat.Png
                                    : ext.Equals(".bmp", StringComparison.OrdinalIgnoreCase) ? ImageFormat.Bmp
                                    : ImageFormat.Jpeg;
                    enhanced.Save(dst, fmt);
                }

                if (!string.IsNullOrEmpty(labelDir))
                {
                    string srcLabel = Path.Combine(labelDir, name + ".txt");
                    if (File.Exists(srcLabel))
                        File.Copy(srcLabel, Path.Combine(labelDir, name + AugSuffix + ".txt"), true);
                }

                int done = Interlocked.Increment(ref count);
                if (done % 10 == 0 || done == total)
                {
                    UpdateTrainInfo($"[影像增強] 進度 {done}/{total}");
                }
            });
            return count;
        }
        private static byte[] BuildContrastGammaLut(double contrastPct, double gamma)
        {
            double factor = 1.0 + contrastPct / 100.0;
            byte[] lut = new byte[256];
            for (int i = 0; i < 256; i++)
            {
                double v = (i - 128.0) * factor + 128.0;          // 對比
                v = Math.Max(0, Math.Min(255, v)) / 255.0;
                v = Math.Pow(v, gamma) * 255.0;                    // Gamma
                lut[i] = (byte)Math.Max(0, Math.Min(255, Math.Round(v)));
            }
            return lut;
        }

        private static Bitmap ApplyLut(string path, byte[] lut)
        {
            Bitmap result;
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (Bitmap src = new Bitmap(fs))
            {
                result = new Bitmap(src.Width, src.Height, PixelFormat.Format24bppRgb);
                using (Graphics g = Graphics.FromImage(result))
                {
                    g.DrawImage(src, 0, 0, src.Width, src.Height);
                }
            }

            Rectangle rect = new Rectangle(0, 0, result.Width, result.Height);
            BitmapData bd = result.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            try
            {
                int stride = Math.Abs(bd.Stride);
                byte[] buf = new byte[stride * result.Height];
                System.Runtime.InteropServices.Marshal.Copy(bd.Scan0, buf, 0, buf.Length);
                for (int i = 0; i < buf.Length; i++) buf[i] = lut[buf[i]];
                System.Runtime.InteropServices.Marshal.Copy(buf, 0, bd.Scan0, buf.Length);
            }
            finally
            {
                result.UnlockBits(bd);
            }
            return result;
        }

        private bool ValidateTrainingInputs(out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrEmpty(tbWidth.Text.Trim()) || !int.TryParse(tbWidth.Text.Trim(), out int width) || width <= 0)
            {
                errorMessage = "請輸入有效的圖片寬度！";
                return false;
            }

            if (string.IsNullOrEmpty(tbHeight.Text.Trim()) || !int.TryParse(tbHeight.Text.Trim(), out int height) || height <= 0)
            {
                errorMessage = "請輸入有效的圖片高度！";
                return false;
            }

            if (string.IsNullOrEmpty(tbBatch.Text.Trim()) || !int.TryParse(tbBatch.Text.Trim(), out int batch) || batch <= 0)
            {
                errorMessage = "請輸入有效的批次大小！";
                return false;
            }

            if (string.IsNullOrEmpty(tbEpoch.Text.Trim()) || !int.TryParse(tbEpoch.Text.Trim(), out int epoch) || epoch <= 0)
            {
                errorMessage = "請輸入有效的訓練週期！";
                return false;
            }
            if (!double.TryParse(tbContrast.Text.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out double ct) || ct < -90 || ct > 300)
            {
                errorMessage = "對比調整請輸入 -90 ~ 300 (%)。";
                return false;
            }
            if (!double.TryParse(tbGamma.Text.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out double gm) || gm < 0.1 || gm > 5.0)
            {
                errorMessage = "Gamma 請輸入 0.1 ~ 5.0。";
                return false;
            }
            if (!double.TryParse(tbRotate.Text.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out double rt) || rt < 0 || rt > 180)
            {
                errorMessage = "旋轉角度請輸入 0 ~ 180 (±度)。";
                return false;
            }

            return true;
        }

        private string PrepareModelPath()
        {
            string modelDir = GV.AIConfig.ModelPath;
            if (string.IsNullOrEmpty(modelDir))
            {
                modelDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "models");
            }

            // 確保目錄存在
            if (!Directory.Exists(modelDir))
            {
                Directory.CreateDirectory(modelDir);
            }

            // 從 AIConfig 讀取 trainModel 設定
            string trainModel = GV.AIConfig.trainModel;

            if (string.IsNullOrEmpty(trainModel))
            {
                // 如果沒有設定，檢查是否有現有模型
                string existingModelName = $"best{GV.AIConfig.LabeledRecipeNumber}.pt";
                string existingModelPath = Path.Combine(modelDir, existingModelName);

                if (File.Exists(existingModelPath))
                {
                    trainModel = existingModelName;
                }
                else
                {
                    trainModel = "yolov8n.pt";
                }
            }

            string modelPath = Path.Combine(modelDir, trainModel);

            // 如果使用現有模型，先備份
            if (trainModel != "yolov8n.pt" && File.Exists(modelPath))
            {
                BackupOldModel(modelPath);
            }

            // 檢查模型檔案是否存在
            if (!File.Exists(modelPath))
            {
                if (trainModel == "yolov8n.pt")
                {
                    MessageBox.Show(
                        $"找不到基礎模型 yolov8n.pt，請將該檔案放置於：{modelPath}",
                        LanguageResource.Get("ModelNotFound"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                else
                {
                    // 如果指定的模型不存在，嘗試使用基礎模型
                    string baseModelPath = Path.Combine(modelDir, "yolov8n.pt");
                    if (File.Exists(baseModelPath))
                    {
                        Debug.WriteLine($"[YOLO] 指定模型 {trainModel} 不存在，改用基礎模型 yolov8n.pt");
                        modelPath = baseModelPath;
                    }
                    else
                    {
                        MessageBox.Show(
                            $"找不到模型檔案：{modelPath}\n也找不到基礎模型 yolov8n.pt",
                            LanguageResource.Get("ModelNotFound"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                }
            }

            Debug.WriteLine($"[YOLO] 使用模型: {modelPath}");
            return modelPath;
        }

        private void BackupOldModel(string modelPath)
        {
            try
            {
                string backupDir = GV.AIConfig.ModelBackupPath;
                if (string.IsNullOrEmpty(backupDir))
                {
                    backupDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "models", "backup");
                }

                // 確保備份目錄存在
                if (!Directory.Exists(backupDir))
                {
                    Directory.CreateDirectory(backupDir);
                }

                // 建立帶時間戳的備份檔名
                string fileName = Path.GetFileNameWithoutExtension(modelPath);
                string extension = Path.GetExtension(modelPath);
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string backupFileName = $"{fileName}_{timestamp}{extension}";
                string backupPath = Path.Combine(backupDir, backupFileName);

                // 複製檔案到備份目錄
                File.Copy(modelPath, backupPath, true);
            }
            catch (IOException ex)
            {
                MessageBox.Show(
                    $"備份模型時發生錯誤：{ex.Message}\n\n模型可能正被其他程式使用。請關閉相關程式後再試。",
                    "備份失敗",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                throw;
            }
        }

        private Task StartTrainingAsync(string trainCommand)
        {
            trainingCancellation = new CancellationTokenSource();
            errorOutput.Clear();
            standardOutput.Clear();

            return Task.Run(() =>
            {
                try
                {
                    // 獲取專案目錄 (TrainPath 的父目錄)
                    string projectDir = GV.AIConfig.TrainPath;
                    if (string.IsNullOrEmpty(projectDir))
                    {
                        projectDir = AppDomain.CurrentDomain.BaseDirectory;
                    }

                    UpdateTrainInfo("正在啟動 YOLO 訓練程序...");
                    Debug.WriteLine($"[YOLO] 工作目錄: {projectDir}");
                    Debug.WriteLine($"[YOLO] 訓練命令: {trainCommand}");

                    // 建立 ProcessStartInfo
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = "yolo",
                        Arguments = trainCommand,
                        WorkingDirectory = projectDir,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden
                    };

                    // 建立 Process
                    trainingProcess = new Process
                    {
                        StartInfo = startInfo,
                        EnableRaisingEvents = true
                    };

                    // 標準輸出事件處理
                    trainingProcess.OutputDataReceived += (sender, e) =>
                    {
                        if (!string.IsNullOrEmpty(e.Data))
                        {
                            standardOutput.AppendLine(e.Data);
                            Debug.WriteLine($"[YOLO Output] {e.Data}");

                            // 移除 ANSI 顏色碼
                            string cleanData = System.Text.RegularExpressions.Regex.Replace(e.Data, @"\x1B\[[^@-~]*[@-~]", "");
                            cleanData = cleanData.Trim();
                            if (cleanData.Contains("Ultralytics") || cleanData.Contains("CUDA") ||
                                cleanData.Contains("torch-") || cleanData.Contains("device"))
                            {
                                UpdateTrainInfo("[YOLO] " + cleanData);
                            }
                            bool isTqdmBar = cleanData.Contains("%|");
                            if (!isTqdmBar || cleanData.Contains("100%|"))
                            {
                                if (cleanData.Contains("train:") || cleanData.Contains("val:") ||
                                    cleanData.Contains("Caching") || cleanData.Contains("Scanning") ||
                                    cleanData.Contains("Starting training") || cleanData.Contains("Image sizes") ||
                                    cleanData.Contains("Overriding") || cleanData.Contains("Model summary"))
                                {
                                    UpdateTrainInfo("[YOLO] " + cleanData);
                                }
                            }

                            // 檢查是否為最終驗證標記（Validating best.pt）
                            if (cleanData.Contains("Validating") && cleanData.Contains("best.pt"))
                            {
                                isFinalValidation = true;
                                validationHeaderDisplayed = false;
                            }

                            // 只有在最終驗證階段才顯示驗證結果
                            if (isFinalValidation)
                            {
                                // 檢查驗證結果數據行 (all, cross, square 等類別名稱開頭，後面跟著數字)
                                bool isValidationRow = System.Text.RegularExpressions.Regex.IsMatch(cleanData, @"^\s*(all|[a-zA-Z_]\w*)\s+\d+\s+\d+\s+[\d.]+");

                                if (isValidationRow)
                                {
                                    // 顯示標題（一次）
                                    if (!validationHeaderDisplayed)
                                    {
                                        UpdateTrainInfo("");
                                        UpdateTrainInfo("================== 驗證結果 ==================");
                                        UpdateTrainInfo(FormatValidationHeader());
                                        validationHeaderDisplayed = true;
                                    }

                                    string formattedValidation = FormatValidationData(cleanData);
                                    if (!string.IsNullOrEmpty(formattedValidation))
                                    {
                                        UpdateTrainInfo(formattedValidation);
                                    }
                                }
                            }
                        }
                    };

                    // 錯誤輸出事件處理
                    trainingProcess.ErrorDataReceived += (sender, e) =>
                    {
                        if (!string.IsNullOrEmpty(e.Data))
                        {
                            errorOutput.AppendLine(e.Data);
                            string data = e.Data.Trim();
                            Debug.WriteLine($"[YOLO Stderr] {data}");

                            // 移除 ANSI 顏色碼
                            string cleanData = System.Text.RegularExpressions.Regex.Replace(data, @"\x1B\[[^@-~]*[@-~]", "");
                            cleanData = cleanData.Trim();
                            if (cleanData.Contains("Ultralytics") || cleanData.Contains("CUDA") ||
                                cleanData.Contains("torch-"))
                            {
                                UpdateTrainInfo("[YOLO] " + cleanData);
                            }
                            bool isTqdmBarErr = cleanData.Contains("%|");
                            if ((!isTqdmBarErr || cleanData.Contains("100%|")) &&
                                (cleanData.Contains("Scanning") || cleanData.Contains("Caching") ||
                                 cleanData.Contains("train:") || cleanData.Contains("val:")))
                            {
                                UpdateTrainInfo("[YOLO] " + cleanData);
                            }
                            // 檢查是否為最終驗證標記（Validating best.pt）- 也可能在 stderr
                            if (cleanData.Contains("Validating") && cleanData.Contains("best.pt"))
                            {
                                isFinalValidation = true;
                                validationHeaderDisplayed = false;
                            }

                            // 1. 檢查是否為訓練數據行（以 數字/數字 開頭）
                            bool isProgressRow = System.Text.RegularExpressions.Regex.IsMatch(cleanData, @"^\s*\d+/\d+\s");

                            if (isProgressRow)
                            {
                                string formattedData = FormatTrainingData(cleanData);
                                if (!string.IsNullOrEmpty(formattedData))
                                {
                                    UpdateTrainInfo(formattedData);
                                    
                                }
                                if (stopAfterCurrentEpoch)
                                {
                                    // 檢查是否為當前 epoch 的最後一筆資料（100%）
                                    var progressMatch = System.Text.RegularExpressions.Regex.Match(cleanData, @"(\d+)%\|");
                                    if (progressMatch.Success)
                                    {
                                        int progressPercent = int.Parse(progressMatch.Groups[1].Value);
                                        if (progressPercent >= 100)
                                        {
                                            // 當前 epoch 結束，設定標記讓主迴圈處理
                                            //UpdateTrainInfo($"Epoch {lastEpochNumber} 完成，正在停止訓練...");
                                            trainingCancellation?.Cancel();
                                        }
                                    }
                                }
                            }
                        }
                    };

                    // 啟動程序
                    bool started = trainingProcess.Start();
                    _trainSw = Stopwatch.StartNew();
                    _epochSw = null;
                    if (!started)
                    {
                        throw new Exception("無法啟動 YOLO 訓練程序。請確認 YOLO 已正確安裝且在系統 PATH 中。");
                    }
                    try
                    {
                        // Normal：訓練在背景 python 程序執行，不影響 UI 執行緒，不需壓低優先權
                        trainingProcess.PriorityClass = ProcessPriorityClass.Normal;
                    }
                    catch
                    {
                        // 權限不足時略過，不影響訓練
                    }
                    UpdateTrainInfo("YOLO 訓練程序已啟動");

                    // 開始異步讀取輸出
                    trainingProcess.BeginOutputReadLine();
                    trainingProcess.BeginErrorReadLine();

                    // 等待程序結束或取消
                    while (!trainingProcess.WaitForExit(1000))
                    {
                        if (trainingCancellation.Token.IsCancellationRequested)
                        {
                            // 終止訓練
                            if (trainingProcess != null && !trainingProcess.HasExited)
                            {
                                KillProcessTree(trainingProcess);
                                if (stopAfterCurrentEpoch)
                                {
                                    UpdateTrainInfo("訓練已在當前 Epoch 完成後停止");
                                }
                                else
                                {
                                    UpdateTrainInfo("訓練已被使用者強制終止");
                                }
                                Debug.WriteLine("[YOLO] 訓練已被使用者終止");
                            }
                            break;
                        }
                    }

                    // 檢查退出碼
                    if (trainingProcess.HasExited)
                    {
                        int exitCode = trainingProcess.ExitCode;
                        Debug.WriteLine($"[YOLO] 訓練程序結束，退出碼: {exitCode}");

                        if (trainingCancellation.Token.IsCancellationRequested && stopAfterCurrentEpoch)
                        {
                            UpdateTrainInfo("訓練已順利停止");
                            return; // 正常返回，不拋出異常
                        }
                        if (exitCode != 0 && !trainingCancellation.Token.IsCancellationRequested)
                        {
                            // 收集錯誤訊息
                            string errorDetails = errorOutput.ToString();
                            string standardDetails = standardOutput.ToString();

                            string detailedError = $"YOLO 訓練失敗，退出碼: {exitCode}\n\n";

                            if (!string.IsNullOrEmpty(errorDetails))
                            {
                                detailedError += "錯誤輸出:\n" + errorDetails.Substring(0, Math.Min(500, errorDetails.Length)) + "\n\n";
                            }

                            if (!string.IsNullOrEmpty(standardDetails))
                            {
                                // 取最後 500 字元
                                int startIndex = Math.Max(0, standardDetails.Length - 500);
                                detailedError += "標準輸出 (最後部分):\n" + standardDetails.Substring(startIndex);
                            }

                            // 常見錯誤提示
                            detailedError += "\n\n可能原因:\n";
                            detailedError += "1. 模型檔案路徑錯誤或檔案損壞\n";
                            detailedError += "2. data.yaml 配置錯誤\n";
                            detailedError += "3. 訓練資料路徑不正確\n";
                            detailedError += "4. 缺少必要的 Python 套件\n";
                            detailedError += "5. GPU/CUDA 配置問題\n";

                            UpdateTrainInfo($"訓練失敗，退出碼: {exitCode}");
                            Debug.WriteLine($"[YOLO] 詳細錯誤:\n{detailedError}");

                            throw new Exception(detailedError);
                        }
                        else if (exitCode == 0)
                        {
                            UpdateTrainInfo("訓練成功完成");
                        }
                    }
                }
                catch (System.ComponentModel.Win32Exception ex)
                {
                    string errorMsg = "無法啟動 YOLO 程序。\n\n" +
                                     "可能原因:\n" +
                                     "1. YOLO 未安裝或不在系統 PATH 中\n" +
                                     "2. Python 環境未正確配置\n\n" +
                                     $"錯誤詳情: {ex.Message}";

                    Debug.WriteLine($"[YOLO] {errorMsg}");
                    UpdateTrainInfo("啟動失敗");
                    throw new Exception(errorMsg, ex);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[YOLO] 訓練過程發生例外: {ex.Message}");
                    UpdateTrainInfo($"例外: {ex.Message}");
                    throw;
                }
                finally
                {
                    if (trainingProcess != null)
                    {
                        trainingProcess.Dispose();
                        trainingProcess = null;
                    }
                }
            }, trainingCancellation.Token);
        }
        //    private static readonly System.Text.RegularExpressions.Regex TrainingRowRegex =
        //new System.Text.RegularExpressions.Regex(
        //    @"^\s*(?<epoch>\d+/\d+)\s+" +
        //    @"(?:(?<gpu>\d+(?:\.\d+)?G)\s+)?" +
        //    @"(?<box>\d+(?:\.\d+)?)\s+" +
        //    @"(?<cls>\d+(?:\.\d+)?)\s+" +
        //    @"(?<dfl>\d+(?:\.\d+)?)\s+" +
        //    @"(?<instances>\d+)\s+" +
        //    @"(?<size>\d+)\s*$",
        //    System.Text.RegularExpressions.RegexOptions.Compiled |
        //    System.Text.RegularExpressions.RegexOptions.CultureInvariant);

        private string FormatTrainingHeader()
        {
            return string.Format("{0,8} {1,10} {2,10} {3,10} {4,10} {5,8} {6,9}",
    "Epoch", "box_loss", "cls_loss", "dfl_loss", "Instances", "Size", "Time(s)");
        }
        private static void KillProcessTree(Process p)
        {
            if (p == null) return;
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo("taskkill", "/PID " + p.Id + " /T /F")
                {
                    CreateNoWindow = true,
                    UseShellExecute = false
                };
                using (Process k = Process.Start(psi))
                {
                    if (k != null) k.WaitForExit(5000);
                }
            }
            catch { }

            try { if (!p.HasExited) p.Kill(); } catch { }
        }
        private string FormatTrainingData(string rawData)
        {
            try
            {
                // 移除 ANSI 顏色碼和特殊字元
                string normalized = System.Text.RegularExpressions.Regex.Replace(rawData, @"\x1B\[[^@-~]*[@-~]", "");
                normalized = normalized.Replace('\r', ' ').Replace('\n', ' ');
                normalized = System.Text.RegularExpressions.Regex.Replace(normalized, @"\s+", " ").Trim();

                // 提取 Epoch 資訊
                var epochMatch = System.Text.RegularExpressions.Regex.Match(normalized, @"^(\d+/\d+)");
                if (!epochMatch.Success)
                    return null;

                string epoch = epochMatch.Groups[1].Value;

                // 檢查是否為新的 Epoch，並在新 Epoch 開始時顯示標題
                int currentEpoch = int.Parse(epoch.Split('/')[0]);
                if (currentEpoch != lastEpochNumber)
                {
                    if (lastEpochNumber != -1)
                    {
                        UpdateTrainInfo("========================================");
                        if (_epochSw != null)
                        {
                            UpdateTrainInfo($"[計時] Epoch {lastEpochNumber} 耗時 {_epochSw.Elapsed.TotalSeconds:F1} s");
                            double sec = _epochSw.Elapsed.TotalSeconds;
                            UpdateTrainInfo($"[計時] 平均每張 {(_trainImageCount > 0 ? sec / _trainImageCount : 0):F2} s");
                        }
                    }
                    else if (_trainSw != null)
                    {
                        UpdateTrainInfo($"[計時] 程序啟動到第一個 batch：{_trainSw.Elapsed.TotalSeconds:F1} s");
                    }
                    _epochSw = Stopwatch.StartNew();
                    if (_trainImageCount > 0)
                    {
                        UpdateTrainInfo($"[計時] 本 Epoch 共 {_trainImageCount} 張，batch={_trainBatch}，約 {_trainBatchCount} batches");
                    }
                    // 每個新 Epoch 開始時顯示標題
                    UpdateTrainInfo(FormatTrainingHeader());
                    lastEpochNumber = currentEpoch;
                }

                // 提取所有浮點數（loss 值）
                var floatMatches = System.Text.RegularExpressions.Regex.Matches(normalized, @"(?<!\d)0\.\d+|[1-9]\d*\.\d+");

                // 提取所有整數（用於 Instances 和 Size）
                var intMatches = System.Text.RegularExpressions.Regex.Matches(normalized, @"(?<![0-9./])(\d+)(?![0-9./])");

                List<string> floats = new List<string>();
                foreach (System.Text.RegularExpressions.Match match in floatMatches)
                {
                    floats.Add(match.Value);
                }

                List<string> ints = new List<string>();
                foreach (System.Text.RegularExpressions.Match match in intMatches)
                {
                    string val = match.Groups[1].Value;
                    // 排除 epoch 中的數字和百分比
                    if (!epoch.Contains(val) && int.Parse(val) > 0)
                    {
                        ints.Add(val);
                    }
                }

                // 解析欄位：box_loss, cls_loss, dfl_loss
                string boxLoss = floats.Count > 0 ? floats[0] : "";
                string clsLoss = floats.Count > 1 ? floats[1] : "";
                string dflLoss = floats.Count > 2 ? floats[2] : "";

                // Instances 和 Size 是整數
                string instances = ints.Count > 0 ? ints[0] : "";
                string size = ints.Count > 1 ? ints[1] : "";

                string stepTime = "-";
                if (_stepSw != null)
                {
                    stepTime = _stepSw.Elapsed.TotalSeconds.ToString("F2", CultureInfo.InvariantCulture);
                }
                _stepSw = Stopwatch.StartNew();

                return string.Format("{0,8} {1,10} {2,10} {3,10} {4,10} {5,8} {6,9}",
                    epoch, boxLoss, clsLoss, dflLoss, instances, size, stepTime);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Format Error] {ex.Message}");
                return null;
            }
        }
        //private string FormatTrainingData(string rawData)
        //{
        //    try
        //    {
        //        string normalized = System.Text.RegularExpressions.Regex.Replace(rawData, @"\x1B\[[^@-~]*[@-~]", "");
        //        normalized = normalized.Replace('\r', ' ').Replace('\n', ' ');
        //        normalized = System.Text.RegularExpressions.Regex.Replace(normalized, @"\s+", " ").Trim();

        //        var match = TrainingRowRegex.Match(normalized);
        //        if (!match.Success)
        //            return null;

        //        string epoch = match.Groups["epoch"].Value;

        //        int currentEpoch;
        //        if (!int.TryParse(epoch.Split('/')[0], out currentEpoch))
        //            return null;

        //        if (currentEpoch != lastEpochNumber)
        //        {
        //            if (lastEpochNumber != -1)
        //            {
        //                UpdateTrainInfo("========================================");
        //            }

        //            UpdateTrainInfo(FormatTrainingHeader());
        //            lastEpochNumber = currentEpoch;
        //        }

        //        string boxLoss = match.Groups["box"].Value;
        //        string clsLoss = match.Groups["cls"].Value;
        //        string dflLoss = match.Groups["dfl"].Value;
        //        string instances = match.Groups["instances"].Value;
        //        string size = match.Groups["size"].Value;

        //        return string.Format("{0,8} {1,10} {2,10} {3,10} {4,10} {5,8}",
        //            epoch, boxLoss, clsLoss, dflLoss, instances, size);
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine("[FormatTrainingData Error] " + ex.Message);
        //        return null;
        //    }
        //}

        private string FormatValidationHeader()
        {
            return string.Format("{0,-15} {1,8} {2,10} {3,7} {4,10} {5,10} {6,10}",
                "Class", "Images", "Instances", "P", "R", "mAP50", "mAP50-95");
        }

        private string FormatValidationData(string rawData)
        {
            try
            {
                string normalized = System.Text.RegularExpressions.Regex.Replace(rawData, @"\s+", " ").Trim();
                string[] parts = normalized.Split(' ');

                if (parts.Length < 7)
                    return null;

                string className = parts[0];
                string images = parts[1];
                string instances = parts[2];
                string boxP = parts[3];
                string boxR = parts[4];
                string map50 = parts[5];
                string map5095 = parts[6];

                return string.Format("{0,-12} {1,8} {2,10} {3,10} {4,10} {5,10} {6,10}",
                    className, images, instances, boxP, boxR, map50, map5095);
            }
            catch
            {
                return null;
            }
        }
        private void UpdateTrainInfo(string message)
        {
            if (_logWriter != null)
            {
                lock (_logWriter)
                {
                    try
                    {
                        _logWriter.WriteLine($"[{DateTime.Now:HH:mm:ss}] {message}");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"[Log] 寫入失敗: {ex.Message}");
                    }
                }
            }

            if (lbTrainInfo.InvokeRequired)
            {
                // BeginInvoke：不等待 UI 處理完，避免擋住 YOLO 的輸出管線
                try
                {
                    lbTrainInfo.BeginInvoke(new Action<string>(AddTrainInfoItem), message);
                }
                catch (InvalidOperationException)
                {
                    // 視窗已關閉或尚未建立控制項，忽略
                }
                return;
            }

            AddTrainInfoItem(message);
        }

        private void AddTrainInfoItem(string message)
        {
            lbTrainInfo.Items.Add(message);
            lbTrainInfo.TopIndex = lbTrainInfo.Items.Count - 1;

            if (lbTrainInfo.Items.Count > 500)
            {
                lbTrainInfo.Items.RemoveAt(0);
            }
        }

        private void HandleTrainedModel()
        {
            try
            {
                // YOLO 訓練完成後，模型儲存在 TrainPath 底下的 runs/detect 中
                string trainPath = GV.AIConfig.TrainPath;
                if (string.IsNullOrEmpty(trainPath))
                {
                    trainPath = AppDomain.CurrentDomain.BaseDirectory;
                }

                string runsDetectPath = Path.Combine(trainPath, "runs", "detect");

                if (!Directory.Exists(runsDetectPath))
                {
                    MessageBox.Show(
                        $"找不到訓練結果目錄：{runsDetectPath}",
                        "警告",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                // 尋找最新的 train 資料夾 (train, train2, train3...)
                DirectoryInfo detectDir = new DirectoryInfo(runsDetectPath);
                DirectoryInfo latestTrainDir = detectDir.GetDirectories("train*")
                    .OrderByDescending(d => d.CreationTime)
                    .FirstOrDefault();

                if (latestTrainDir == null)
                {
                    MessageBox.Show(
                        "找不到任何訓練結果資料夾！",
                        "警告",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                // 訓練完成的模型路徑
                string defaultTrainedModelPath = Path.Combine(latestTrainDir.FullName, "weights", "best.pt");

                if (!File.Exists(defaultTrainedModelPath))
                {
                    MessageBox.Show(
                        $"找不到訓練完成的模型檔案：{defaultTrainedModelPath}",
                        "警告",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                // 目標路徑
                string modelDir = GV.AIConfig.ModelPath;
                if (string.IsNullOrEmpty(modelDir))
                {
                    modelDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "models");
                }

                // 確保目標目錄存在
                if (!Directory.Exists(modelDir))
                {
                    Directory.CreateDirectory(modelDir);
                }

                string targetModelName = $"best{GV.AIConfig.LabeledRecipeNumber}.pt";
                string targetModelPath = Path.Combine(modelDir, targetModelName);

                // 複製訓練完成的模型到目標位置
                int retryCount = 0;
                while (retryCount < 5)
                {
                    try
                    {
                        File.Copy(defaultTrainedModelPath, targetModelPath, true);
                        Debug.WriteLine($"[YOLO] 模型已成功儲存到：{targetModelPath}");
                        break;
                    }
                    catch (IOException)
                    {
                        retryCount++;
                        Thread.Sleep(1000); // 等待 1 秒後重試

                        if (retryCount >= 5)
                        {
                            MessageBox.Show(
                                $"無法儲存模型到目標位置：{targetModelPath}\n\n模型可能正被其他程式使用。",
                                "儲存失敗",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"處理訓練完成的模型時發生錯誤：{ex.Message}",
                    "錯誤",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btTerminate_Click(object sender, EventArgs e)
        {
            if (trainingProcess == null || trainingProcess.HasExited)
            {
                MessageBox.Show("目前沒有正在執行的訓練程序。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show(
                $"目前正在訓練 Epoch {lastEpochNumber}/{totalEpochs}\n\n" +
                "選擇終止方式：\n" +
                "• [是] - 等待當前 Epoch 完成後停止\n" +
                "• [否] - 立即強制終止\n" +
                "• [取消] - 繼續訓練",
                "終止訓練",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // 等待當前 epoch 結束後停止
                stopAfterCurrentEpoch = true;
                btTerminate.Enabled = false;
                //UpdateTrainInfo($"已設定：將在 Epoch {lastEpochNumber} 完成後停止訓練...");
            }
            else if (result == DialogResult.No)
            {
                // 立即強制終止
                trainingCancellation?.Cancel();
                UpdateTrainInfo("正在強制終止訓練...");
            }
        }
        private async void btTesting_Click(object sender, EventArgs e)
        {
            try
            {
                string testPath = tbTest.Text.Trim();
                if (string.IsNullOrEmpty(testPath) || !Directory.Exists(testPath))
                {
                    MessageBox.Show("請輸入有效的測試資料夾路徑！", "輸入錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string modelDir = GV.AIConfig.ModelPath;
                if (string.IsNullOrEmpty(modelDir))
                {
                    modelDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "models");
                }

                string modelName = $"best{GV.AIConfig.LabeledRecipeNumber}.pt";
                string modelPath = Path.Combine(modelDir, modelName);

                if (!File.Exists(modelPath))
                {
                    MessageBox.Show($"找不到模型檔案：{modelPath}\n\n請先進行模型訓練。", "模型不存在", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string[] testImages = Directory.GetFiles(testPath, "*.jpg")
                    .Concat(Directory.GetFiles(testPath, "*.png"))
                    .Concat(Directory.GetFiles(testPath, "*.bmp"))
                    .ToArray();

                if (testImages.Length == 0)
                {
                    MessageBox.Show("測試資料夾中找不到任何圖片檔案（.jpg, .png, .bmp）！", "無測試資料", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult result = MessageBox.Show(
                    $"測試參數：\n\n" +
                    $"模型路徑: {modelPath}\n" +
                    $"測試圖片數量: {testImages.Length}\n" +
                    $"\n是否開始測試？",
                    "確認測試",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes) return;

                // 在 UI thread 先取得所有需要的 UI 值
                int width = 640, height = 640;
                if (int.TryParse(tbWidth.Text.Trim(), out int w) && w > 0) width = w;
                if (int.TryParse(tbHeight.Text.Trim(), out int h) && h > 0) height = h;
                int imgsz = Math.Max(width, height);

                string dataYamlPath;
                string yamlReason;
                if (!TryFindAvailableDataYaml(out dataYamlPath, out yamlReason))
                {
                    dataYamlPath = string.Empty;   // 找不到就略過精度驗證
                }

                btTesting.Enabled = false;
                lbTestResult.Items.Clear();
                lock (_testResultLogLock)
                {
                    _testResultLogBuffer.Clear();
                    _testResultLogBuffer.AppendLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ===== 測試開始 =====");
                }
                testResultImages.Clear();
                currentTestImageIndex = -1;
                pbTestImage.Image = null;
                lblImageInfo.Text = "0 / 0";
                tabControl1.SelectedTab = tabPage3;

                // 建立「本次測試」專用 Metrics
                lock (_metricsLock) { _perImageInferenceMs.Clear(); }
                _currentTestingMetrics = new CorePerformanceMetrics
                {
                    SummarySource = "Model Testing",
                    ModelPath = modelPath,
                    TestPath = testPath,
                    TestTime = DateTime.Now,
                    TotalTestCount = testImages.Length
                };

                await RunTestingAsync(modelPath, testPath, dataYamlPath, imgsz);

                LoadTestResultImages();

                FinalizeTestingMetrics(_currentTestingMetrics);

                // 只顯示 + 存 Log，不再重新找資料
                ShowCorePerformanceMetrics("Model Testing", _currentTestingMetrics);
                SaveCorePerformanceMetricsLog(_currentTestingMetrics);
                SaveTestingResultReportLog();

                MessageBox.Show("測試完成！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                if (_currentTestingMetrics != null) _currentTestingMetrics.ExceptionCount++;
                MessageBox.Show($"測試過程發生錯誤：{ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btTesting.Enabled = true;
            }
        }

        private Task RunTestingAsync(string modelPath, string testPath, string dataYamlPath, int imgsz)
        {
            return Task.Run(() =>
            {
                CorePerformanceMetrics metrics = _currentTestingMetrics;
                string workDir = GV.AIConfig.TrainPath ?? AppDomain.CurrentDomain.BaseDirectory;

                UpdateTrainInfo("==================== 模型測試 ====================");
                UpdateTrainInfo($"模型: {Path.GetFileName(modelPath)}");
                UpdateTrainInfo($"測試路徑: {testPath}");
                UpdateTrainInfo("");

                // ---------- 階段 1：predict（穩定性 + 反應時間） ----------
                string predictCommand = $"task=detect mode=predict model=\"{modelPath}\" source=\"{testPath}\" imgsz={imgsz} save=True save_txt=True save_conf=True conf=0.25";
                UpdateTrainInfo("正在執行 YOLO 推論...");

                bool timedOut;
                int exitCode;
                try
                {
                    exitCode = RunYoloProcess(predictCommand, workDir, line =>
                    {
                        if (line.Contains("image") || line.Contains("Speed:") || line.Contains("Results saved"))
                        {
                            UpdateTrainInfo(line);
                        }
                        ParseTestingMetricsLine(line, true);
                    }, YoloTimeoutMs, out timedOut);
                }
                catch (System.ComponentModel.Win32Exception ex)
                {
                    metrics.ExceptionCount++;
                    UpdateTrainInfo("無法啟動 YOLO 程序，請確認 YOLO 已正確安裝。");
                    throw new Exception($"無法啟動 YOLO 程序: {ex.Message}", ex);
                }

                if (timedOut) metrics.TimeoutCount++;
                else if (exitCode != 0)
                {
                    metrics.ExceptionCount++;
                    UpdateTrainInfo($"測試程序結束，退出碼: {exitCode}");
                }

                // ---------- 階段 2：val（精度 P/R/mAP），失敗只影響精度 ----------
                if (!string.IsNullOrEmpty(dataYamlPath) && File.Exists(dataYamlPath))
                {
                    UpdateTrainInfo("正在執行 YOLO 精度驗證 (split=test)...");
                    string valCommand = $"task=detect mode=val model=\"{modelPath}\" data=\"{dataYamlPath}\" split=test imgsz={imgsz}" + (string.IsNullOrEmpty(TrainDevice) ? "" : " device=" + TrainDevice);
                    bool valTimedOut;
                    int valExit = RunYoloProcess(valCommand, workDir, line => ParseTestingMetricsLine(line, false), YoloTimeoutMs, out valTimedOut);
                    if (valTimedOut) UpdateTrainInfo("精度驗證逾時，精度標示為 N/A。");
                    else if (valExit != 0) UpdateTrainInfo($"精度驗證失敗(退出碼 {valExit})，可能缺少 labels/test，精度標示為 N/A。");
                }
                else
                {
                    UpdateTrainInfo("找不到 data.yaml，略過精度驗證。");
                }

                UpdateTrainInfo("");
                UpdateTrainInfo("==================== 測試結果摘要 ====================");

                // 解析結果 + 低信心樣本報表（同一次測試資料）
                ParseAndDisplayTestResults(testPath);
            });
        }

        /// <summary>執行 yolo 程序，逐行回呼(去除 ANSI)，支援逾時。回傳退出碼。</summary>
        private int RunYoloProcess(string arguments, string workDir, Action<string> onLine, int timeoutMs, out bool timedOut)
        {
            timedOut = false;
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "yolo",
                Arguments = arguments,
                WorkingDirectory = workDir,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            using (Process p = new Process())
            {
                p.StartInfo = startInfo;
                DataReceivedEventHandler handler = (s, e) =>
                {
                    if (string.IsNullOrEmpty(e.Data)) return;
                    string clean = System.Text.RegularExpressions.Regex.Replace(e.Data, @"\x1B\[[^@-~]*[@-~]", "").Trim();
                    if (clean.Length == 0) return;
                    Debug.WriteLine("[YOLO] " + clean);
                    onLine(clean);
                };
                p.OutputDataReceived += handler;
                p.ErrorDataReceived += handler;

                if (!p.Start()) throw new Exception("無法啟動 YOLO 程序。");
                p.BeginOutputReadLine();
                p.BeginErrorReadLine();

                if (!p.WaitForExit(timeoutMs))
                {
                    timedOut = true;
                    KillProcessTree(p);
                    return -1;
                }
                p.WaitForExit(); // 確保非同步輸出讀完
                return p.ExitCode;
            }
        }

        /// <summary>解析 YOLO 輸出。isPredict=true 解析反應時間；false 解析 P/R/mAP。</summary>
        private void ParseTestingMetricsLine(string text, bool isPredict)
        {
            CorePerformanceMetrics m = _currentTestingMetrics;
            if (m == null || string.IsNullOrWhiteSpace(text)) return;

            if (!isPredict)
            {
                // all  5  10  0.965  0.997  0.995  0.898
                Match all = Regex.Match(text,
                    @"^\s*all\s+(?<img>\d+)\s+(?<ins>\d+)\s+(?<p>\d*\.?\d+)\s+(?<r>\d*\.?\d+)\s+(?<m50>\d*\.?\d+)\s+(?<m5095>\d*\.?\d+)\s*$",
                    RegexOptions.IgnoreCase);
                if (all.Success)
                {
                    m.Precision = ParseInvariantDouble(all.Groups["p"].Value);
                    m.Recall = ParseInvariantDouble(all.Groups["r"].Value);
                    m.Map50 = ParseInvariantDouble(all.Groups["m50"].Value);
                    m.Map5095 = ParseInvariantDouble(all.Groups["m5095"].Value);
                    m.AccuracyAvailable = true;
                }
                return;
            }

            // 單張: image 1/5 path: 2016x2016 1 cross, 229.2ms
            Match perImage = Regex.Match(text, @"^image\s+\d+/\d+\s.*?,\s*(?<ms>\d*\.?\d+)ms\s*$", RegexOptions.IgnoreCase);
            if (perImage.Success)
            {
                lock (_metricsLock)
                {
                    _perImageInferenceMs.Add(ParseInvariantDouble(perImage.Groups["ms"].Value));
                }
                return;
            }

            // Speed: 4.2ms preprocess, 229.2ms inference, 3.7ms postprocess per image
            Match speed = Regex.Match(text,
                @"Speed:\s*(?<pre>\d*\.?\d+)ms preprocess,\s*(?<inf>\d*\.?\d+)ms inference,.*?(?<post>\d*\.?\d+)ms postprocess",
                RegexOptions.IgnoreCase);
            if (speed.Success)
            {
                m.PreprocessMs = ParseInvariantDouble(speed.Groups["pre"].Value);
                m.InferenceMs = ParseInvariantDouble(speed.Groups["inf"].Value);
                m.PostprocessMs = ParseInvariantDouble(speed.Groups["post"].Value);
                m.EndToEndMs = m.PreprocessMs + m.InferenceMs + m.PostprocessMs;
                m.LatencyAvailable = true;
                m.LatencySource = "Current model testing (YOLO predict Speed)";
            }
        }

        private static double ParseInvariantDouble(string value)
        {
            double result;
            return double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result) ? result : 0.0;
        }

        /// <summary>測試結束後統一計算穩定性與延遲統計。</summary>
        private void FinalizeTestingMetrics(CorePerformanceMetrics m)
        {
            // 成功 = predict 產出的結果圖數量(每張圖有結果代表該張執行成功)
            int produced = 0;
            if (!string.IsNullOrEmpty(latestPredictPath) && Directory.Exists(latestPredictPath))
            {
                HashSet<string> done = new HashSet<string>(
                    Directory.GetFiles(latestPredictPath)
                        .Where(f => new[] { ".jpg", ".png", ".bmp" }.Contains(Path.GetExtension(f).ToLowerInvariant()))
                        .Select(f => Path.GetFileNameWithoutExtension(f)),
                    StringComparer.OrdinalIgnoreCase);

                produced = Directory.GetFiles(m.TestPath)
                    .Where(f => new[] { ".jpg", ".png", ".bmp" }.Contains(Path.GetExtension(f).ToLowerInvariant()))
                    .Count(f => done.Contains(Path.GetFileNameWithoutExtension(f)));
            }

            m.SuccessTestCount = Math.Min(produced, m.TotalTestCount);
            m.FailTestCount = Math.Max(m.TotalTestCount - m.SuccessTestCount, 0);
            m.StabilitySuccessRate = m.TotalTestCount > 0 ? m.SuccessTestCount * 100.0 / m.TotalTestCount : 0.0;

            // 單張推論延遲統計
            List<double> list;
            lock (_metricsLock) { list = _perImageInferenceMs.OrderBy(x => x).ToList(); }
            if (list.Count > 0)
            {
                m.MinInferenceMs = list.First();
                m.MaxInferenceMs = list.Last();
                int idx = Math.Min(list.Count - 1, (int)Math.Ceiling(list.Count * 0.95) - 1);
                m.P95InferenceMs = list[Math.Max(idx, 0)];
            }
        }

        private void ParseAndDisplayTestResults(string testPath)
        {
            try
            {
                string runsDetectPath = Path.Combine(GV.AIConfig.TrainPath ?? AppDomain.CurrentDomain.BaseDirectory, "runs", "detect");

                if (!Directory.Exists(runsDetectPath))
                {
                    UpdateTestResult("找不到測試結果目錄。");
                    return;
                }

                DirectoryInfo detectDir = new DirectoryInfo(runsDetectPath);
                DirectoryInfo latestPredictDir = detectDir.GetDirectories("predict*")
                    .OrderByDescending(d => d.CreationTime)
                    .FirstOrDefault();

                if (latestPredictDir == null)
                {
                    UpdateTestResult("找不到預測結果資料夾。");
                    return;
                }

                latestPredictPath = latestPredictDir.FullName;
                UpdateTestResult($"結果路徑: {latestPredictPath}");
                UpdateTestResult("");

                string labelsPath = Path.Combine(latestPredictDir.FullName, "labels");
                if (Directory.Exists(labelsPath))
                {
                    string[] labelFiles = Directory.GetFiles(labelsPath, "*.txt");
                    Dictionary<int, int> classCount = new Dictionary<int, int>();
                    int totalDetections = 0;
                    int imagesWithDetection = 0;

                    UpdateTestResult(string.Format("{0,-25} {1,8}", "檔案名稱", "偵測數"));
                    UpdateTestResult(new string('-', 35));

                    foreach (string labelFile in labelFiles)
                    {
                        string[] lines = File.ReadAllLines(labelFile);
                        int detectionCount = lines.Length;
                        totalDetections += detectionCount;

                        if (detectionCount > 0)
                            imagesWithDetection++;

                        string fileName = Path.GetFileNameWithoutExtension(labelFile);
                        UpdateTestResult(string.Format("{0,-25} {1,8}", fileName, detectionCount));

                        foreach (string line in lines)
                        {
                            string[] parts = line.Split(' ');
                            if (parts.Length > 0 && int.TryParse(parts[0], out int classId))
                            {
                                if (!classCount.ContainsKey(classId))
                                    classCount[classId] = 0;
                                classCount[classId]++;
                            }
                        }
                    }

                    UpdateTestResult(new string('-', 35));
                    UpdateTestResult("");
                    UpdateTestResult("===== 統計摘要 =====");
                    UpdateTestResult($"測試圖片: {Directory.GetFiles(testPath, "*.jpg").Concat(Directory.GetFiles(testPath, "*.png")).Concat(Directory.GetFiles(testPath, "*.bmp")).Count()}");
                    UpdateTestResult($"有偵測結果: {imagesWithDetection}");
                    UpdateTestResult($"總偵測數: {totalDetections}");

                    if (classCount.Count > 0)
                    {
                        UpdateTestResult("");
                        UpdateTestResult("各類別統計:");
                        foreach (var kvp in classCount.OrderBy(x => x.Key))
                        {
                            UpdateTestResult($"  Class {kvp.Key}: {kvp.Value}");
                        }
                    }
                }
                AppendLowConfidenceReport(testPath, labelsPath);
            }
            catch (Exception ex)
            {
                UpdateTestResult($"解析錯誤: {ex.Message}");
            }
        }

        private void AppendLowConfidenceReport(string testPath, string labelsPath)
        {
            Dictionary<int, string> classNames = new Dictionary<int, string>();
            try
            {
                int recipe = GV.AIConfig != null ? GV.AIConfig.LabeledRecipeNumber : 1;
                Dictionary<string, short> dict = AIClassList.LoadLabelClassNameFromJson(recipe);
                if (dict != null) foreach (var kv in dict) classNames[kv.Value] = kv.Key;
            }
            catch { }
            string[] exts = { ".jpg", ".png", ".bmp" };
            List<string>images=Directory.GetFiles(testPath)
                .Where(p=>exts.Contains(Path.GetExtension(p).ToLowerInvariant()))
                .OrderBy(p=>p).ToList();
            UpdateTestResult("");
            UpdateTestResult("===== 低信心樣本｜場域驗證 =====");
            UpdateTestResult(string.Format("{0,-22} {1,-10} {2,10} {3,-10} {4}",
                "影像", "類別", "Confidence", "預測結果", "處理"));
            UpdateTestResult(new string('-', 66));
            int lowCount = 0;
            foreach(string img in images)
            {
                string name = Path.GetFileNameWithoutExtension(img);
                string labelFile = Path.Combine(labelsPath ?? string.Empty, name + ".txt");

                double minConf = 0.0;
                int clsId = -1;
                bool hasDet = false;

                if (!string.IsNullOrEmpty(labelsPath) && File.Exists(labelFile))
                {
                    foreach (string line in File.ReadAllLines(labelFile))
                    {
                        string[] p = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (p.Length < 6) continue;
                        if (!int.TryParse(p[0], out int id)) continue;
                        if (!double.TryParse(p[5], NumberStyles.Any, CultureInfo.InvariantCulture, out double conf)) continue;

                        if (!hasDet || conf < minConf)
                        {
                            minConf = conf;
                            clsId = id;
                            hasDet = true;
                        }
                    }
                }

                string result = !hasDet ? "NG" : minConf >= ConfOkThreshold ? "OK"
                              : minConf >= ConfReviewThreshold ? "待確認" : "NG";
                string action = result == "OK" ? "—" : "加入訓練";
                string cls = !hasDet ? "—"
                           : (classNames.ContainsKey(clsId) ? classNames[clsId] : "Class " + clsId);

                if (result == "OK") continue; // 只列低信心樣本
                lowCount++;

                UpdateTestResult(string.Format("{0,-22} {1,-10} {2,10} {3,-10} {4}",
                    Path.GetFileName(img), cls, minConf.ToString("F2", CultureInfo.InvariantCulture), result, action));
            }
            UpdateTestResult(new string('-', 66));
            if (_currentTestingMetrics != null) _currentTestingMetrics.LowConfidenceCount = lowCount;
            UpdateTestResult($"低信心樣本數: {lowCount} / {images.Count} (門檻: OK≥{ConfOkThreshold:F2}, 待確認≥{ConfReviewThreshold:F2})");
        }

        private void UpdateTestResult(string message)
        {
            if (lbTestResult.InvokeRequired)
            {
                lbTestResult.Invoke(new Action<string>(UpdateTestResult), message);
            }
            else
            {
                lbTestResult.Items.Add(message);
                lbTestResult.TopIndex = lbTestResult.Items.Count - 1;
                lock(_testResultLogLock)
                {
                    _testResultLogBuffer.AppendLine(message ?? string.Empty);
                }

                // 更新水平捲軸寬度
                UpdateTestResultHorizontalExtent();
            }
        }
        private void UpdateTestResultHorizontalExtent()
        {
            // 計算最大文字寬度以設定水平捲軸範圍
            int maxWidth = 0;
            using (Graphics g = lbTestResult.CreateGraphics())
            {
                foreach (var item in lbTestResult.Items)
                {
                    int itemWidth = (int)g.MeasureString(item.ToString(), lbTestResult.Font).Width;
                    if (itemWidth > maxWidth)
                    {
                        maxWidth = itemWidth;
                    }
                }
            }
            // 設定水平捲軸範圍（加一些邊距）
            lbTestResult.HorizontalExtent = maxWidth + 20;
        }
        private void LoadTestResultImages()
        {
            testResultImages.Clear();
            currentTestImageIndex = -1;

            if (string.IsNullOrEmpty(latestPredictPath) || !Directory.Exists(latestPredictPath))
                return;

            testResultImages = Directory.GetFiles(latestPredictPath, "*.jpg")
                .Concat(Directory.GetFiles(latestPredictPath, "*.png"))
                .OrderBy(f => f)
                .ToList();

            if (testResultImages.Count > 0)
            {
                currentTestImageIndex = 0;
                DisplayCurrentTestImage();
            }
            else
            {
                lblImageInfo.Text = "無結果圖片";
            }
        }
        private void SaveTestingResultReportLog()
        {
            try
            {
                string logDir = Path.Combine(@"C:\Log", DateTime.Now.ToString("yyyyMMdd"));
                Directory.CreateDirectory(logDir);

                string fileName = $"TestingReport_{DateTime.Now:HHmmss}.log";
                string logPath = Path.Combine(logDir, fileName);

                string content;
                lock (_testResultLogLock)
                {
                    _testResultLogBuffer.AppendLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ===== 測試結束 =====");
                    content = _testResultLogBuffer.ToString();
                }

                File.WriteAllText(logPath, content, Encoding.UTF8);

                // 若有最新 predict 資料夾，再複製一份進去，方便對照圖片
                if (!string.IsNullOrWhiteSpace(latestPredictPath) && Directory.Exists(latestPredictPath))
                {
                    string predictLogPath = Path.Combine(latestPredictPath, fileName);
                    File.WriteAllText(predictLogPath, content, Encoding.UTF8);
                }

                UpdateTestResult("測試報表 Log: " + logPath);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[TestingReportLog] " + ex.Message);
            }
        }

        private void DisplayCurrentTestImage()
        {
            if (currentTestImageIndex < 0 || currentTestImageIndex >= testResultImages.Count)
                return;

            try
            {
                string imagePath = testResultImages[currentTestImageIndex];

                // 釋放舊圖片
                if (pbTestImage.Image != null)
                {
                    pbTestImage.Image.Dispose();
                    pbTestImage.Image = null;
                }

                // 載入新圖片（使用 FileStream 避免檔案鎖定）
                using (FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
                {
                    pbTestImage.Image = Image.FromStream(fs);
                }

                lblImageInfo.Text = $"{currentTestImageIndex + 1} / {testResultImages.Count} - {Path.GetFileName(imagePath)}";

                btPrevImage.Enabled = currentTestImageIndex > 0;
                btNextImage.Enabled = currentTestImageIndex < testResultImages.Count - 1;
            }
            catch (Exception ex)
            {
                lblImageInfo.Text = $"載入失敗: {ex.Message}";
            }
        }

        private void btPrevImage_Click(object sender, EventArgs e)
        {
            if (currentTestImageIndex > 0)
            {
                currentTestImageIndex--;
                DisplayCurrentTestImage();
            }
        }

        private void btNextImage_Click(object sender, EventArgs e)
        {
            if (currentTestImageIndex < testResultImages.Count - 1)
            {
                currentTestImageIndex++;
                DisplayCurrentTestImage();
            }
        }

        private void lbTestResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 點擊結果列表時，嘗試顯示對應圖片
            if (lbTestResult.SelectedItem == null)
                return;

            string selectedText = lbTestResult.SelectedItem.ToString().Trim();
            if (string.IsNullOrEmpty(selectedText) || selectedText.StartsWith("-") || selectedText.StartsWith("="))
                return;

            // 嘗試從選中的文字中提取檔案名稱
            string[] parts = selectedText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            
            if (parts.Length > 0)
            {
                string fileName = Path.GetFileNameWithoutExtension(parts[0]);
                int index = testResultImages.FindIndex(f =>
                    Path.GetFileNameWithoutExtension(f).Equals(fileName, StringComparison.OrdinalIgnoreCase));

                if (index >= 0)
                {
                    currentTestImageIndex = index;
                    DisplayCurrentTestImage();
                }
            }
        }

        private void btResult_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage3;
            // 只顯示最近一次測試結果，不重新計算
            ShowCorePerformanceMetrics("Testing Result", _latestCoreMetrics);
        }

        private void ShowCorePerformanceMetrics(string sourceTag, CorePerformanceMetrics metrics)
        {
            try
            {
                if (metrics == null)
                {
                    UpdateTestResult("[核心性能摘要] 尚無測試資料，請先執行「模型測試」。");
                    return;
                }
                _latestCoreMetrics = metrics;

                string acc = metrics.AccuracyAvailable
                    ? $"Precision={metrics.Precision:F3}, Recall={metrics.Recall:F3}, mAP@0.5={metrics.Map50:F3}, mAP@0.5:0.95={metrics.Map5095:F3}"
                    : "N/A（本次測試未取得標註驗證結果）";
                string stab = $"測試 {metrics.TotalTestCount} / 成功 {metrics.SuccessTestCount} / 失敗 {metrics.FailTestCount} / Timeout {metrics.TimeoutCount} / Exception {metrics.ExceptionCount} / 成功率 {metrics.StabilitySuccessRate:F1}%";
                string lat = metrics.LatencyAvailable
                    ? $"前處理 {metrics.PreprocessMs:F1} ms / 推論 {metrics.InferenceMs:F1} ms / 後處理 {metrics.PostprocessMs:F1} ms / 端到端 {metrics.EndToEndMs:F1} ms"
                    : "N/A（本次測試未量測）";
                string dist = $"推論延遲 Min {metrics.MinInferenceMs:F1} / Max {metrics.MaxInferenceMs:F1} / P95 {metrics.P95InferenceMs:F1} ms";

                Action<Action<string>> write = w =>
                {
                    w("");
                    w("============== 核心性能指標驗證 ==============");
                    w($"來源: {sourceTag}");
                    w($"模型: {metrics.ModelPath}");
                    w($"測試資料: {metrics.TestPath}");
                    w($"精度 => {acc}");
                    w($"穩定性 => {stab}");
                    w($"反應時間 => {lat}");
                    if (metrics.LatencyAvailable) w($"延遲統計 => {dist}");
                    w($"反應時間資料來源 => {metrics.LatencySource ?? "Not measured"}");
                    w($"低信心樣本數 => {metrics.LowConfidenceCount}");
                    w("==============================================");
                };

                write(UpdateTrainInfo);
                write(UpdateTestResult);
            }
            catch (Exception ex)
            {
                UpdateTrainInfo($"[核心性能摘要] 生成失敗: {ex.Message}");
                UpdateTestResult($"[核心性能摘要] 生成失敗: {ex.Message}");
            }
        }

        private void SaveCorePerformanceMetricsLog(CorePerformanceMetrics m)
        {
            try
            {
                string dir = Path.Combine(@"C:\Log", DateTime.Now.ToString("yyyyMMdd"));
                Directory.CreateDirectory(dir);
                string file = Path.Combine(dir, $"CoreMetrics_{DateTime.Now:HHmmss}.log");

                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"[{m.TestTime:yyyy-MM-dd HH:mm:ss}]");
                sb.AppendLine("============== AI 核心性能指標驗證 ==============");
                sb.AppendLine($"來源           : {m.SummarySource}");
                sb.AppendLine($"模型           : {Path.GetFileName(m.ModelPath)}");
                sb.AppendLine($"測試資料       : {m.TestPath}");
                sb.AppendLine($"測試圖片數     : {m.TotalTestCount}");
                sb.AppendLine();
                sb.AppendLine("-------------- 精度 --------------");
                if (m.AccuracyAvailable)
                {
                    sb.AppendLine($"Precision      : {m.Precision:F3}");
                    sb.AppendLine($"Recall         : {m.Recall:F3}");
                    sb.AppendLine($"mAP@0.5        : {m.Map50:F3}");
                    sb.AppendLine($"mAP@0.5:0.95   : {m.Map5095:F3}");
                }
                else sb.AppendLine("N/A（本次測試未取得標註驗證結果）");
                sb.AppendLine();
                sb.AppendLine("-------------- 穩定性 --------------");
                sb.AppendLine($"總測試數       : {m.TotalTestCount}");
                sb.AppendLine($"成功           : {m.SuccessTestCount}");
                sb.AppendLine($"失敗           : {m.FailTestCount}");
                sb.AppendLine($"Timeout        : {m.TimeoutCount}");
                sb.AppendLine($"Exception      : {m.ExceptionCount}");
                sb.AppendLine($"成功率         : {m.StabilitySuccessRate:F1} %");
                sb.AppendLine($"低信心樣本數   : {m.LowConfidenceCount}");
                sb.AppendLine();
                sb.AppendLine("-------------- 反應速度 --------------");
                if (m.LatencyAvailable)
                {
                    sb.AppendLine($"Preprocess     : {m.PreprocessMs:F1} ms");
                    sb.AppendLine($"Inference      : {m.InferenceMs:F1} ms");
                    sb.AppendLine($"Postprocess    : {m.PostprocessMs:F1} ms");
                    sb.AppendLine($"End-to-End     : {m.EndToEndMs:F1} ms");
                    sb.AppendLine($"Inference Min/Max/P95 : {m.MinInferenceMs:F1} / {m.MaxInferenceMs:F1} / {m.P95InferenceMs:F1} ms");
                }
                else sb.AppendLine("N/A（本次測試未量測）");
                sb.AppendLine($"反應時間來源   : {m.LatencySource ?? "Not measured"}");
                sb.AppendLine("==================================================");

                File.WriteAllText(file, sb.ToString(), Encoding.UTF8);
                UpdateTestResult("性能指標 Log: " + file);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[CoreMetricsLog] " + ex.Message);
            }
        }
        private bool TryLoadMeasuredLatency(CorePerformanceMetrics m)
        {
            // 可依你們後續測試檔格式擴充
            // 目前先找最新 predict*/latency.csv
            try
            {
                string trainPath = GV.AIConfig != null ? GV.AIConfig.TrainPath : string.Empty;
                if (string.IsNullOrEmpty(trainPath)) return false;

                string detectPath = Path.Combine(trainPath, "runs", "detect");
                if (!Directory.Exists(detectPath)) return false;

                string[] latencyCsvs = Directory.GetFiles(detectPath, "latency.csv", SearchOption.AllDirectories);
                if (latencyCsvs.Length == 0) return false;

                string latest = latencyCsvs.OrderByDescending(f => File.GetLastWriteTime(f)).First();
                string[] lines = File.ReadAllLines(latest);
                if (lines.Length < 2) return false;

                string[] headers = lines[0].Split(',');
                string[] last = lines[lines.Length - 1].Split(',');

                int idxPre = FindColumnIndex(headers, "preprocess_ms");
                int idxInfer = FindColumnIndex(headers, "inference_ms");
                int idxPost = FindColumnIndex(headers, "postprocess_ms");
                int idxE2E = FindColumnIndex(headers, "end_to_end_ms");

                m.PreprocessMs = ParseDoubleAt(last, idxPre);
                m.InferenceMs = ParseDoubleAt(last, idxInfer);
                m.PostprocessMs = ParseDoubleAt(last, idxPost);
                m.EndToEndMs = ParseDoubleAt(last, idxE2E);
                m.LatencySource = "Measured latency.csv";

                if (m.EndToEndMs <= 0)
                {
                    m.EndToEndMs = m.PreprocessMs + m.InferenceMs + m.PostprocessMs;
                }

                return m.EndToEndMs > 0;
            }
            catch
            {
                return false;
            }
        }

        private int GetCurrentClassCount()
        {
            try
            {
                int recipe = GV.AIConfig != null ? GV.AIConfig.LabeledRecipeNumber : 1;
                Dictionary<string, short> classDict = AIClassList.LoadLabelClassNameFromJson(recipe);
                if (classDict == null) return 0;
                return classDict.Count;
            }
            catch
            {
                return 0;
            }
        }

        private void ApplyFallbackLatencyFromReport(int classCount, CorePerformanceMetrics m)
        {
            // 參照你提供期末報告（CPU保守條件）
            if (classCount <= 1)
            {
                // 單類別 cross
                m.PreprocessMs = 5.9;
                m.InferenceMs = 179.9;
                m.PostprocessMs = 0.4;
                m.EndToEndMs = 184.0;
                m.LatencySource = "Report baseline: single-class cross (CPU)";
            }
            else if (classCount == 2)
            {
                // 雙類別 方形/十字
                m.PreprocessMs = 4.2;
                m.InferenceMs = 229.2;
                m.PostprocessMs = 203.7;
                m.EndToEndMs = 437.0;
                m.LatencySource = "Report baseline: two-class square/cross (CPU)";
            }
            else
            {
                // 四類別
                m.PreprocessMs = 4.9;
                m.InferenceMs = 224.7;
                m.PostprocessMs = 72.1;
                m.EndToEndMs = 302.0;
                m.LatencySource = "Report baseline: four-class (CPU)";
            }
        }

        private static int FindColumnIndex(string[] headers, string name)
        {
            for (int i = 0; i < headers.Length; i++)
            {
                if (string.Equals(headers[i].Trim(), name, StringComparison.OrdinalIgnoreCase))
                    return i;
            }
            return -1;
        }

        private static double ParseDoubleAt(string[] row, int index)
        {
            if (index < 0 || index >= row.Length) return 0.0;

            double val;
            if (double.TryParse(row[index].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out val))
                return val;

            if (double.TryParse(row[index].Trim(), out val))
                return val;

            return 0.0;
        }

        private static int ParseIntAt(string[] row, int index)
        {
            if (index < 0 || index >= row.Length) return 0;

            int val;
            if (int.TryParse(row[index].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out val))
                return val;

            if (int.TryParse(row[index].Trim(), out val))
                return val;

            return 0;
        }
        private bool TryReloadDataYamlFromCurrentDataset(out string dataYamlPath, out string errorMessage)
        {
            dataYamlPath = string.Empty;
            errorMessage = string.Empty;

            string trainPath = tbTrain.Text.Trim();
            if (string.IsNullOrEmpty(trainPath) || !Directory.Exists(trainPath))
            {
                errorMessage = "訓練集路徑無效，請先選擇正確資料夾。";
                return false;
            }

            // 期望格式: ...\dataset\images\train
            DirectoryInfo trainDir = new DirectoryInfo(trainPath);
            DirectoryInfo imagesDir = trainDir.Parent;                 // ...\images
            DirectoryInfo datasetDir = imagesDir != null ? imagesDir.Parent : null; // ...\dataset

            if (datasetDir == null)
            {
                errorMessage = "無法由訓練集路徑推導 dataset 資料夾。";
                return false;
            }

            dataYamlPath = Path.Combine(datasetDir.FullName, "data.yaml");
            if (!File.Exists(dataYamlPath))
            {
                errorMessage = "找不到 data.yaml 檔案：" + dataYamlPath;
                return false;
            }

            // 重新讀取 data.yaml，並同步回 UI 路徑（支援相對/絕對路徑）
            string yamlTrain = GetYamlValue(dataYamlPath, "train");
            string yamlVal = GetYamlValue(dataYamlPath, "val");
            string yamlTest = GetYamlValue(dataYamlPath, "test");

            string yamlBaseDir = Path.GetDirectoryName(dataYamlPath);

            if (!string.IsNullOrWhiteSpace(yamlTrain))
            {
                string resolved = ResolveYamlPath(yamlBaseDir, yamlTrain);
                if (Directory.Exists(resolved)) tbTrain.Text = resolved;
            }
            if (!string.IsNullOrWhiteSpace(yamlVal))
            {
                string resolved = ResolveYamlPath(yamlBaseDir, yamlVal);
                if (Directory.Exists(resolved)) tbValidate.Text = resolved;
            }
            if (!string.IsNullOrWhiteSpace(yamlTest))
            {
                string resolved = ResolveYamlPath(yamlBaseDir, yamlTest);
                if (Directory.Exists(resolved)) tbTest.Text = resolved;
            }

            // 讓後續 WorkingDirectory / 結果解析使用最新 dataset
            if (GV.AIConfig != null)
            {
                GV.AIConfig.TrainPath = datasetDir.FullName;
            }

            return true;
        }

        private string GetYamlValue(string yamlPath, string key)
        {
            string[] lines = File.ReadAllLines(yamlPath);
            string prefix = key + ":";

            foreach (string raw in lines)
            {
                if (string.IsNullOrWhiteSpace(raw)) continue;

                string line = raw.Trim();
                if (line.StartsWith("#")) continue;
                if (!line.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)) continue;

                string value = line.Substring(prefix.Length).Trim();

                // 去掉行尾註解
                int commentIndex = value.IndexOf('#');
                if (commentIndex >= 0)
                {
                    value = value.Substring(0, commentIndex).Trim();
                }

                // 去掉引號
                value = value.Trim().Trim('"').Trim('\'');

                return value;
            }

            return string.Empty;
        }

        private string ResolveYamlPath(string baseDir, string pathValue)
        {
            if (Path.IsPathRooted(pathValue))
            {
                return Path.GetFullPath(pathValue);
            }

            return Path.GetFullPath(Path.Combine(baseDir, pathValue));
        }
        private int DeleteAugArtifacts(string trainPath, string labelDir)
        {
            int removed = 0;
            foreach (string f in Directory.GetFiles(trainPath, "*" + AugSuffix + ".*"))
            {
                string ext = Path.GetExtension(f).ToLowerInvariant();
                if (ext != ".jpg" && ext != ".png" && ext != ".bmp" && ext != ".npy") continue;

                File.Delete(f);
                if (ext == ".npy") continue;

                removed++;
                if (!string.IsNullOrEmpty(labelDir))
                {
                    string lf = Path.Combine(labelDir, Path.GetFileNameWithoutExtension(f) + ".txt");
                    if (File.Exists(lf)) File.Delete(lf);
                }
            }
            return removed;
        }
        private string DecideCacheMode(string trainPath)
        {
            try
            {
                string[] exts = { ".jpg", ".png", ".bmp" };
                List<string> imgs = Directory.GetFiles(trainPath)
                    .Where(f => exts.Contains(Path.GetExtension(f).ToLowerInvariant()))
                    .ToList();
                if (imgs.Count == 0) return "cache=False";

                // 以第一張圖的原始尺寸估算 .npy 大小（YOLO 存的是原圖解碼後的陣列）
                long bytesPerImage;
                using (Image img = Image.FromFile(imgs[0]))
                {
                    bytesPerImage = (long)img.Width * img.Height * 3;
                }

                double needGB = imgs.Count * (double)bytesPerImage * 1.5 / 1073741824.0;   // 含 50% 安全邊際
                string root = Path.GetPathRoot(Path.GetFullPath(trainPath));
                double freeGB = new DriveInfo(root).AvailableFreeSpace / 1073741824.0;

                UpdateTrainInfo($"[快取評估] train 圖片 {imgs.Count} 張，估算需要 {needGB:F1} GB，可用 {freeGB:F1} GB");

                if (needGB < freeGB)
                {
                    UpdateTrainInfo("[快取評估] 使用 cache=disk");
                    return "cache=disk";
                }
                UpdateTrainInfo("[快取評估] 空間不足，改用 cache=False（避免 YOLO 建立快取失敗）");
                return "cache=False";
            }
            catch (Exception ex)
            {
                UpdateTrainInfo("[快取評估] 失敗，改用 cache=False: " + ex.Message);
                return "cache=False";
            }
        }
    }
}
