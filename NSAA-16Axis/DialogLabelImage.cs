using Basler.Pylon;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//using System.Web.UI.WebControls;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;


namespace NSAA_16Axis
{
    public partial class DialogLabelImage : Form
    {
        string serviceExePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Python");
        List<string> imageFiles = new List<string>();
        int currentIndex = 0;
        string imageDir = "";
        List<YoloBox> boxes = new List<YoloBox>();
        bool isDrawing = false;
        int startX, startY, endX, endY;
        private Bitmap initialImage;
        private bool isInitial = false;
        private int nowMagni;
        private int currentRecipeNumber;
        private Dictionary<string, Int16> labelClassList = new Dictionary<string, Int16>();

        public DialogLabelImage()
        {
            InitializeComponent();
            currentRecipeNumber = GV.NowRecipeNumber;
            ApplyLanguage();
        }
        public DialogLabelImage(Bitmap initialImage, int nowMagnification)
        {
            InitializeComponent();
            this.initialImage = initialImage;
            isInitial = true;
            nowMagni = nowMagnification;
            currentRecipeNumber = GV.NowRecipeNumber;
            ApplyLanguage();
        }

        public DialogLabelImage(int recipeNumber)
        {
            InitializeComponent();
            currentRecipeNumber = recipeNumber;
            ApplyLanguage();
        }
        private void ApplyLanguage()
        {
            if (GV.AppSettingParm.Language != "default")
            {
                // 視窗標題會在 Load 事件中設定（包含 Recipe 編號和路徑）

                // 按鈕
                btnPrev.Text = GV.Dlang.strBtnPrev;
                btnNext.Text = GV.Dlang.strBtnNext;
                btnSave.Text = GV.Dlang.strBtnSave;
                btnClear.Text = GV.Dlang.strBtnClear;
                btAdd.Text = GV.Dlang.strBtnAddClass;
                btDelete.Text = GV.Dlang.strBtnDeleteClass;
                btTrain.Text = GV.Dlang.strBtnTrain;
                btDeleteImage.Text = GV.Dlang.strBtnDeleteImage;

                // 標籤
                if (groupBox1 != null)
                    groupBox1.Text = GV.Dlang.strLabelClass;  // "Label" 群組框
                if (lb != null)
                    lb.Text = GV.Dlang.strLabelNewClass;
            }
        }

        private void btnOpenFolder_Click(object sender, EventArgs e)
        {

        }
        private void DialogLabelImage_Load(object sender, EventArgs e)
        {
            string trainBasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Python", "Train");
            imageDir = Path.Combine(trainBasePath, $"{currentRecipeNumber}");
            try
            {
                if (!Directory.Exists(imageDir))
                {
                    Directory.CreateDirectory(imageDir);
                }
                imageFiles = Directory.GetFiles(imageDir, "*.*", SearchOption.TopDirectoryOnly)
                    .Where(f =>
                    {
                        string ext = Path.GetExtension(f).ToLower();
                        return ext == ".jpg" || ext == ".png" || ext == ".bmp";
                    })
                    .OrderBy(f => f) // 按路徑排序
                    .ToList();
                currentIndex = 0;

                labelClassList = AIClassList.LoadLabelClassNameFromJson(currentRecipeNumber);

                // 載入類別列表
                comboBoxClass.Items.Clear();
                foreach (var className in labelClassList.OrderBy(kvp => kvp.Value))
                {
                    comboBoxClass.Items.Add(className.Key);
                }
                if (comboBoxClass.Items.Count > 0)
                {
                    comboBoxClass.SelectedIndex = 0;
                }
                if (GV.AppSettingParm.Language != "default")
                {
                    this.Text = $"{GV.Dlang.strLabelImageTitle}{currentRecipeNumber} - {GV.Dlang.strLabelImagePath}:{imageDir}";
                }
                else
                {
                    this.Text = $"Recipe{currentRecipeNumber} - Path:{imageDir}";
                }
                if (btnOrganizeAndYaml != null)
                {
                    btnOrganizeAndYaml.Visible = false;
                }
                if (imageFiles.Count > 0)
                {
                    currentIndex = 0;
                    LoadCurrentImage();

                    // ✅ 如果有 initialImage，釋放它（不再使用）
                    if (initialImage != null)
                    {
                        initialImage.Dispose();
                        initialImage = null;
                    }
                    isInitial = false;
                }
                else
                {
                    // 沒有圖片時顯示空白
                    pictureBox1.Image = null;
                    boxes.Clear();
                    pictureBox1.Invalidate();

                }
            }
            catch (UnauthorizedAccessException ex)
            {
                string message = GV.AppSettingParm.Language != "default"
                    ? string.Format(GV.Dlang.strNoPermission, imageDir, ex.Message)
                    : $"沒有權限存取資料夾: {imageDir}\n錯誤: {ex.Message}";
                string title = GV.AppSettingParm.Language != "default"
                    ? GV.Dlang.strPermissionError
                    : "權限錯誤";
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                string message = GV.AppSettingParm.Language != "default"
                    ? string.Format(GV.Dlang.strLoadImageError, ex.Message)
                    : $"載入圖片時發生錯誤: {ex.Message}";
                string title = GV.AppSettingParm.Language != "default"
                    ? GV.Dlang.strError
                    : "錯誤";
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCurrentImage()
        {
            if (imageFiles.Count == 0) return;

            if (currentIndex < 0) currentIndex = 0;
            if (currentIndex >= imageFiles.Count) currentIndex = imageFiles.Count - 1;
            var imgPath = imageFiles[currentIndex];

            if (pictureBox1.Image != null && pictureBox1.Image != initialImage)
            {
                pictureBox1.Image.Dispose();
            }

            pictureBox1.Image = new Bitmap(imgPath);

            // 載入已存label
            boxes.Clear();
            string labelPath = Path.ChangeExtension(imgPath, ".txt");
            if (File.Exists(labelPath))
            {
                foreach (var line in File.ReadAllLines(labelPath))
                {
                    var parts = line.Split(' ');
                    if (parts.Length == 5)
                        
                    boxes.Add(new YoloBox
                        {
                            ClassId = int.Parse(parts[0]),
                            X = float.Parse(parts[1]),
                            Y = float.Parse(parts[2]),
                            W = float.Parse(parts[3]),
                            H = float.Parse(parts[4]),
                            ClassName = GetClassNameById(int.Parse(parts[0]))
                        });
                }
            }
            pictureBox1.Invalidate();
        }

        private string GetClassNameById(int classId)
        {
            try
            {
                if(classId>=0 && classId<comboBoxClass.Items.Count)
                {
                    return comboBoxClass.Items[classId].ToString();
                }
                foreach(var kvp in labelClassList)
                {
                    if(kvp.Value == classId)
                    {
                        return kvp.Key;
                    }
                }
                return "";
            }
            catch
            {
                return "";
            }
        }

        private void skPattern_MouseUp(object sender, MouseEventArgs e)
        {
            isDrawing = false;
            endX = e.X; endY = e.Y;
            int minX = Math.Min(startX, endX), minY = Math.Min(startY, endY);
            int maxX = Math.Max(startX, endX), maxY = Math.Max(startY, endY);

            // 轉 YOLO 格式（歸一化中心、寬高）
            float xCenter = (minX + maxX) / 2f / skPattern.Width;
            float yCenter = (minY + maxY) / 2f / skPattern.Height;
            float w = (maxX - minX) / (float)skPattern.Width;
            float h = (maxY - minY) / (float)skPattern.Height;

            int classId = comboBoxClass.SelectedIndex;
            boxes.Add(new YoloBox { ClassId = classId, X = xCenter, Y = yCenter, W = w, H = h });

            skPattern.Invalidate();
        }

        private void skPattern_Paint(object sender, PaintEventArgs e)
        {
            foreach (var box in boxes)
            {
                DrawYoloBox(e.Graphics, box, Color.Red);
            }
            if (isDrawing)
            {
                Rectangle rect = new Rectangle(
                    Math.Min(startX, endX), Math.Min(startY, endY),
                    Math.Abs(endX - startX), Math.Abs(endY - startY));
                e.Graphics.DrawRectangle(Pens.Lime, rect);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (isInitial)
                {
                    // 確保目錄存在
                    if (!Directory.Exists(imageDir))
                    {
                        Directory.CreateDirectory(imageDir);
                    }

                    string magnificationStr = $"X{nowMagni + 1}";
                    string fileName = $"labeled_{DateTime.Now:yyyyMMdd_HHmmss}_{magnificationStr}.bmp";
                    string imgPath = Path.Combine(imageDir, fileName);

                    // 儲存圖片（從 pictureBox1 儲存，因為 initialImage 可能已釋放）
                    pictureBox1.Image.Save(imgPath);

                    // 儲存標註
                    string labelPath = Path.ChangeExtension(imgPath, ".txt");
                    File.WriteAllLines(labelPath, boxes.Select(b => $"{b.ClassId} {b.X:F6} {b.Y:F6} {b.W:F6} {b.H:F6}"));

                    // 加入到 imageFiles 列表並排序
                    imageFiles.Add(imgPath);
                    imageFiles = imageFiles.OrderBy(f => f).ToList();

                    // 設定當前索引
                    currentIndex = imageFiles.IndexOf(imgPath);

                    // 釋放 initialImage 並清除標記
                    if (initialImage != null)
                    {
                        initialImage.Dispose();
                        initialImage = null;
                    }
                    isInitial = false;

                    //MessageBox.Show("圖片已儲存！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (imageFiles.Count == 0) return;

                    var imgPath = imageFiles[currentIndex];
                    var labelPath = Path.ChangeExtension(imgPath, ".txt");

                    // 儲存標註
                    File.WriteAllLines(labelPath, boxes.Select(b => $"{b.ClassId} {b.X:F6} {b.Y:F6} {b.W:F6} {b.H:F6}"));

                    //MessageBox.Show("標註已儲存！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    OrganizeDatasetAndLaunchTraining();
                    Thread.Sleep(500);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                string message = GV.AppSettingParm.Language != "default"
                    ? string.Format(GV.Dlang.strSaveError, ex.Message)
                    : $"儲存時發生錯誤: {ex.Message}";
                string title = GV.AppSettingParm.Language != "default"
                    ? GV.Dlang.strError
                    : "錯誤";
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDrawing = true;
                startX = e.X;
                startY = e.Y;
            }

        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                endX = e.X; endY = e.Y;
                pictureBox1.Invalidate();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (boxes.Count > 0)
                {
                    YoloBox removedBox = boxes[boxes.Count - 1];
                    boxes.RemoveAt(boxes.Count - 1);
                    pictureBox1.Invalidate();
                    SaveCurrentAnnotations();
                }
                else
                {
                    string message = GV.AppSettingParm.Language != "default"
                        ? GV.Dlang.strNoAnnotation
                        : "目前沒有可刪除的標記框！";
                    string title = GV.AppSettingParm.Language != "default"
                        ? GV.Dlang.strHint
                        : "提示";
                    MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }
            if (e.Button == MouseButtons.Left)
            {

                isDrawing = false;
                endX = e.X; endY = e.Y;
                int minX = Math.Min(startX, endX), minY = Math.Min(startY, endY);
                int maxX = Math.Max(startX, endX), maxY = Math.Max(startY, endY);

                // 轉 YOLO 格式（歸一化中心、寬高）
                float xCenter = (minX + maxX) / 2f / skPattern.Width;
                float yCenter = (minY + maxY) / 2f / skPattern.Height;
                float w = (maxX - minX) / (float)skPattern.Width;
                float h = (maxY - minY) / (float)skPattern.Height;

                int classId = comboBoxClass.SelectedIndex;
                string className = comboBoxClass.SelectedIndex >= 0 ? comboBoxClass.SelectedItem.ToString() : "";
                boxes.Add(new YoloBox { ClassId = classId, X = xCenter, Y = yCenter, W = w, H = h, ClassName = className });

                pictureBox1.Invalidate();
            }
        }

        private void SaveCurrentAnnotations()
        {
            try
            {
                if (isInitial)
                {
                    return;
                }
                if (imageFiles.Count == 0) return;
                var imgPath = imageFiles[currentIndex];
                var labelPath = Path.ChangeExtension(imgPath, ".txt");
                if (boxes.Count == 0)
                {
                    if (File.Exists(labelPath))
                    {
                        File.Delete(labelPath);
                    }
                    return;
                }

                // 儲存標註
                File.WriteAllLines(labelPath, boxes.Select(b => $"{b.ClassId} {b.X:F6} {b.Y:F6} {b.W:F6} {b.H:F6}"));
            }
            catch
            {

            }

        }
        //private string GetClassNameByIndex(int classId)
        //{
        //    try
        //    {
        //        if(classId >=0 && classId < comboBoxClass.Items.Count)
        //        {
        //            return comboBoxClass.Items[classId].ToString();
        //        }
        //    }
        //}

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            foreach (var box in boxes)
            {
                DrawYoloBoxWithLabel(e.Graphics, box, Color.Red);
            }
            if (isDrawing)
            {
                Rectangle rect = new Rectangle(
                    Math.Min(startX, endX), Math.Min(startY, endY),
                    Math.Abs(endX - startX), Math.Abs(endY - startY));
                e.Graphics.DrawRectangle(Pens.Lime, rect);
            }
        }

        private void DrawYoloBoxWithLabel(Graphics graphics, YoloBox box, Color color)
        {
            int w = pictureBox1.Width, h = pictureBox1.Height;
            int boxW = (int)(box.W * w), boxH = (int)(box.H * h);
            int boxX = (int)(box.X * w - boxW / 2), boxY = (int)(box.Y * h - boxH / 2);

            // 繪製邊框
            using (Pen pen = new Pen(color, 2))
            {
                graphics.DrawRectangle(pen, boxX, boxY, boxW, boxH);
            }

            // 繪製類別標籤
            if (!string.IsNullOrEmpty(box.ClassName))
            {
                // 準備標籤文字
                string labelText = box.ClassName;

                // 設定字體和測量文字大小
                using (Font font = new Font("Arial", 10, FontStyle.Bold))
                {
                    SizeF textSize = graphics.MeasureString(labelText, font);

                    // 計算標籤背景矩形
                    int padding = 4;
                    int labelWidth = (int)textSize.Width + padding * 2;
                    int labelHeight = (int)textSize.Height + padding;

                    // 標籤位置（在框的上方）
                    int labelX = boxX;
                    int labelY = boxY - labelHeight;

                    // 如果標籤超出上邊界，放在框內
                    if (labelY < 0)
                    {
                        labelY = boxY;
                    }

                    // 確保標籤不超出右邊界
                    if (labelX + labelWidth > w)
                    {
                        labelX = w - labelWidth;
                    }

                    // 繪製標籤背景
                    Rectangle labelRect = new Rectangle(labelX, labelY, labelWidth, labelHeight);
                    using (SolidBrush bgBrush = new SolidBrush(Color.FromArgb(200, color)))
                    {
                        graphics.FillRectangle(bgBrush, labelRect);
                    }

                    // 繪製標籤邊框
                    using (Pen labelPen = new Pen(color, 1))
                    {
                        graphics.DrawRectangle(labelPen, labelRect);
                    }

                    // 繪製文字
                    using (SolidBrush textBrush = new SolidBrush(Color.White))
                    {
                        graphics.DrawString(
                            labelText,
                            font,
                            textBrush,
                            labelX + padding,
                            labelY + padding / 2);
                    }
                }
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            SaveCurrentAnnotations();
            if (isInitial)
            {
                if (imageFiles.Count == 0) return;

                // 釋放 initialImage
                if (initialImage != null)
                {
                    initialImage.Dispose();
                    initialImage = null;
                }
                isInitial = false;

                // 載入最後一張圖片
                currentIndex = imageFiles.Count - 1;
                LoadCurrentImage();
                return;
            }

            if (imageFiles.Count == 0) return;

            if (currentIndex > 0)
            {
                currentIndex--;
                LoadCurrentImage();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            SaveCurrentAnnotations();
            if (isInitial)
            {
                if (imageFiles.Count == 0) return;

                // 釋放 initialImage
                if (initialImage != null)
                {
                    initialImage.Dispose();
                    initialImage = null;
                }
                isInitial = false;

                // 載入第一張圖片
                currentIndex = 0;
                LoadCurrentImage();
                return;
            }

            if (imageFiles.Count == 0) return;

            if (currentIndex < imageFiles.Count - 1)
            {
                currentIndex++;
                LoadCurrentImage();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {

            if (isInitial)
            {
                boxes.Clear();
                pictureBox1.Invalidate();
            }
            else
            {
                var imgPath = imageFiles[currentIndex];
                string labelPath = Path.ChangeExtension(imgPath, ".txt");
                string fileName = Path.GetFileName(imgPath);
                string message = GV.AppSettingParm.Language != "default"
                    ? string.Format(GV.Dlang.strConfirmClearAll, boxes.Count)
                    : $"確定要清除當前圖片的所有標註嗎?\n\n共有 {boxes.Count} 個標註框將被清除。";
                string title = GV.AppSettingParm.Language != "default"
                    ? GV.Dlang.strConfirmClear
                    : "確認清除";
                DialogResult result = MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    boxes.Clear();
                    pictureBox1.Invalidate();
                    SaveCurrentAnnotations();
                }
            }
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            string newClass = tbNewLabel.Text.Trim();
            if (string.IsNullOrEmpty(newClass))
            {
                string message = GV.AppSettingParm.Language != "default"
                    ? GV.Dlang.strEnterClassName
                    : "請輸入類別名稱！";
                string title = GV.AppSettingParm.Language != "default"
                    ? GV.Dlang.strHint
                    : "提示";
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (labelClassList.ContainsKey(newClass))
            {
                string message = GV.AppSettingParm.Language != "default"
                    ? string.Format(GV.Dlang.strClassExists, newClass)
                    : $"類別 '{newClass}' 已存在！";
                string title = GV.AppSettingParm.Language != "default"
                    ? GV.Dlang.strHint
                    : "提示";
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            labelClassList.Add(newClass, (Int16)labelClassList.Count);
            comboBoxClass.Items.Add(newClass);
            AIClassList.SaveLabelClassNameToJson(labelClassList, currentRecipeNumber);
            tbNewLabel.Clear();
            comboBoxClass.SelectedItem = newClass;
        }

        private void btnOrganizeAndYaml_Click(object sender, EventArgs e)
        {

            //            string srcImgDir = imageDir;
            //            string trainBasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Python", "Train");
            //            string ModelsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Python", "Models");
            //            string ModelsBackupPath = Path.Combine("C:\\AIData", "Backup");

            //            string trainModelDir = @"C:\Project\TrainModel\bin\x64\Debug";
            //            string trainModelConfig=Path.Combine(trainModelDir, "AIConfig.json");
            //            string labelImagePath = Path.Combine(trainBasePath, $"{currentRecipeNumber}");
            //            string projectDir = Path.Combine(trainBasePath, $"{currentRecipeNumber}", "dataset");

            //            string[] allImages = Directory.GetFiles(srcImgDir, "*.jpg")
            //                .Concat(Directory.GetFiles(srcImgDir, "*.png"))
            //                .Concat(Directory.GetFiles(srcImgDir, "*.bmp"))
            //                .ToArray();
            //            if (allImages.Length == 0)
            //            {
            //                MessageBox.Show("找不到任何圖片檔案！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //                return;
            //            }
            //            int totalImages = allImages.Length;
            //            int testCount = totalImages / 7;
            //            int valCount = totalImages / 7;
            //            int trainCount = totalImages - testCount - valCount;

            //            var rand = new Random();
            //            var shuffled = allImages.OrderBy(x => rand.Next()).ToList();

            //            //var trainImgs = shuffled.Take(allImages.Length - valCount).ToList();
            //            //var valImgs = shuffled.Skip(allImages.Length-valCount).ToList();
            //            var trainImgs = shuffled.Take(trainCount).ToList();
            //            var valImgs = shuffled.Skip(trainCount).Take(valCount).ToList();
            //            var testImgs = shuffled.Skip(trainCount + valCount).ToList();
            //            string[] folders =
            //            {
            //                "images/train","images/val","images/test",
            //                "labels/train","labels/val","labels/test"
            //            };
            //            foreach (var folder in folders)
            //                Directory.CreateDirectory(Path.Combine(projectDir, folder));

            //            MoveImgAndLabel(trainImgs, "images/train", "labels/train", projectDir);
            //            MoveImgAndLabel(valImgs, "images/val", "labels/val", projectDir);
            //            MoveImgAndLabel(testImgs, "images/test", "labels/test", projectDir);

            //            int nc = comboBoxClass.Items.Count;
            //            var names = string.Join(", ", comboBoxClass.Items.Cast<string>().Select(n => $"'{n}'"));
            //            string yaml =
            //        $@"train: ./images/train
            //val: ./images/val
            //test: ./images/test
            //nc: {nc}
            //names: [{names}]
            //";
            //            File.WriteAllText(Path.Combine(projectDir, "data.yaml"), yaml);
            //            try
            //            {
            //                string configPath = Path.Combine(serviceExePath, "AIConfig.json");

            //                // 讀取現有的 AIConfig.json（如果存在）
            //                Dictionary<string, object> configDict = new Dictionary<string, object>();

            //                if (File.Exists(configPath))
            //                {
            //                    try
            //                    {
            //                        string existingJson = File.ReadAllText(configPath);
            //                        var existingConfig = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(existingJson);

            //                        if (existingConfig != null)
            //                        {
            //                            // 保留現有所有參數
            //                            configDict = existingConfig;
            //                        }
            //                    }
            //                    catch (Exception ex)
            //                    {
            //                        System.Diagnostics.Debug.WriteLine($"讀取現有 AIConfig.json 失敗: {ex.Message}，將建立新檔案");
            //                    }
            //                }

            //                // 更新或新增 labeledRecipeNumber 參數
            //                configDict["labeledRecipeNumber"] = currentRecipeNumber;
            //                configDict["trainPath"] = projectDir;
            //                configDict["modelPath"] = ModelsPath;
            //                configDict["modelBackupPath"] = ModelsBackupPath;
            //                configDict["labelImage"]= labelImagePath;
            //                configDict["trainBasePath"] = trainBasePath;
            //                // 儲存到 AIConfig.json
            //                string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(configDict, Newtonsoft.Json.Formatting.Indented);
            //                File.WriteAllText(configPath, jsonContent);

            //                if(!Directory.Exists(trainModelDir))
            //                {
            //                    Directory.CreateDirectory(trainModelDir);
            //                }
            //                File.WriteAllText(trainModelConfig, jsonContent);

            //                System.Diagnostics.Debug.WriteLine($"✅ AIConfig.json 已更新: labeledRecipeNumber = {currentRecipeNumber}");
            //                System.Diagnostics.Debug.WriteLine($"   完整路徑: {configPath}");
            //            }
            //            catch (Exception ex)
            //            {
            //                System.Diagnostics.Debug.WriteLine($"❌ 儲存 labeledRecipeNumber 到 AIConfig.json 失敗: {ex.Message}");
            //                MessageBox.Show(
            //                    $"儲存配置到 AIConfig.json 時發生錯誤:\n{ex.Message}\n\n資料集整理已完成，但配置未更新。",
            //                    "警告",
            //                    MessageBoxButtons.OK,
            //                    MessageBoxIcon.Warning);
            //            }

            //            try
            //            {
            //                string labelImageExePath = @"C:\Project\TrainModel\bin\x64\Debug\TrainModel.exe";
            //                string labelImageDir = Path.GetDirectoryName(labelImageExePath);

            //                if (!File.Exists(labelImageExePath))
            //                {
            //                    System.Diagnostics.Debug.WriteLine($"⚠️ 找不到 LabelImage.exe: {labelImageExePath}");
            //                    MessageBox.Show(
            //                        $"找不到 LabelImage.exe！\n\n預期路徑:\n{labelImageExePath}\n\n請確認檔案是否存在。\n資料集整理已完成。",
            //                        "提示",
            //                        MessageBoxButtons.OK,
            //                        MessageBoxIcon.Information);
            //                }
            //                else
            //                {
            //                    ProcessStartInfo psi = new ProcessStartInfo
            //                    {
            //                        FileName = labelImageExePath,
            //                        WorkingDirectory = labelImageDir,
            //                        UseShellExecute = true,
            //                        WindowStyle = ProcessWindowStyle.Normal
            //                    };

            //                    Process.Start(psi);
            //                    System.Diagnostics.Debug.WriteLine($"✅ 已啟動 LabelImage.exe");
            //                    System.Diagnostics.Debug.WriteLine($"   執行檔路徑: {labelImageExePath}");
            //                    System.Diagnostics.Debug.WriteLine($"   工作目錄: {labelImageDir}");
            //                }
            //            }
            //            catch (Exception ex)
            //            {
            //                System.Diagnostics.Debug.WriteLine($"❌ 啟動 LabelImage.exe 失敗: {ex.Message}");
            //                MessageBox.Show(
            //                    $"啟動 LabelImage.exe 時發生錯誤:\n{ex.Message}\n\n資料集整理已完成。",
            //                    "錯誤",
            //                    MessageBoxButtons.OK,
            //                    MessageBoxIcon.Error);
            //            }
            //MessageBox.Show("資料集已整理、data.yaml已建立！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            OrganizeDatasetAndLaunchTraining();
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

        private void btDelete_Click(object sender, EventArgs e)
        {
            // 檢查是否有類別可以刪除
            if (comboBoxClass.Items.Count == 0)
            {
                string message = GV.AppSettingParm.Language != "default"
                    ? GV.Dlang.strNoAnnotation
                    : "目前沒有可刪除的類別！";
                string title = GV.AppSettingParm.Language != "default"
                    ? GV.Dlang.strHint
                    : "提示";
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string formTitle = GV.AppSettingParm.Language != "default"
                ? $"{GV.Dlang.strDeleteClassTitle}{currentRecipeNumber}"
                : $"刪除類別-Recipe{currentRecipeNumber}";
            // 建立類別選擇對話框
            Form deleteForm = new Form
            {
                Text = formTitle,
                Size = new System.Drawing.Size(400, 300),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };
            string labelText = GV.AppSettingParm.Language != "default"
                ? string.Format(GV.Dlang.strSelectClassToDelete, currentRecipeNumber)
                : $"請選擇要刪除的類別：(Recipe{currentRecipeNumber})";
            Label lblInfo = new Label
            {
                Text = labelText,
                Location = new System.Drawing.Point(10, 10),
                AutoSize = true
            };

            CheckedListBox checkedListBox = new CheckedListBox
            {
                Location = new System.Drawing.Point(10, 40),
                Size = new System.Drawing.Size(360, 150)
            };

            // 載入所有類別
            foreach (var item in comboBoxClass.Items)
            {
                checkedListBox.Items.Add(item.ToString());
            }
            string confirmText = GV.AppSettingParm.Language != "default"
                ? GV.Dlang.strConfirmDelete
                : "確認刪除";
            string cancelText = GV.AppSettingParm.Language != "default"
                ? GV.Dlang.fbtCancel
                : "取消";
            Button btnConfirm = new Button
            {
                Text = confirmText,
                Location = new System.Drawing.Point(200, 210),
                Size = new System.Drawing.Size(80, 30),
                DialogResult = DialogResult.OK
            };

            Button btnCancel = new Button
            {
                Text = cancelText,
                Location = new System.Drawing.Point(290, 210),
                Size = new System.Drawing.Size(80, 30),
                DialogResult = DialogResult.Cancel
            };

            deleteForm.Controls.Add(lblInfo);
            deleteForm.Controls.Add(checkedListBox);
            deleteForm.Controls.Add(btnConfirm);
            deleteForm.Controls.Add(btnCancel);
            deleteForm.AcceptButton = btnConfirm;
            deleteForm.CancelButton = btnCancel;

            if (deleteForm.ShowDialog() == DialogResult.OK)
            {
                // 獲取選中的類別
                List<string> classesToDelete = new List<string>();
                foreach (var item in checkedListBox.CheckedItems)
                {
                    classesToDelete.Add(item.ToString());
                }

                if (classesToDelete.Count == 0)
                {
                    string message = GV.AppSettingParm.Language != "default"
                        ? GV.Dlang.strNoClassSelected
                        : "未選擇任何類別！";
                    string title = GV.AppSettingParm.Language != "default"
                        ? GV.Dlang.strHint
                        : "提示";
                    MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string confirmMessage = GV.AppSettingParm.Language != "default"
                    ? string.Format(GV.Dlang.strConfirmDeleteClass, classesToDelete.Count, string.Join("\n", classesToDelete))
                    : $"確定要刪除以下 {classesToDelete.Count} 個類別嗎？\n\n{string.Join("\n", classesToDelete)}\n\n注意：這將會影響已標註的圖片！";
                string confirmTitle = GV.AppSettingParm.Language != "default"
                    ? GV.Dlang.strConfirmDelete
                    : "確認刪除";

                DialogResult result = MessageBox.Show(
                    confirmMessage,
                    confirmTitle,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        // 建立類別名稱到ID的映射
                        Dictionary<string, int> classNameToId = new Dictionary<string, int>();
                        for (int i = 0; i < comboBoxClass.Items.Count; i++)
                        {
                            classNameToId[comboBoxClass.Items[i].ToString()] = i;
                        }

                        // 從 AIClassList 和 comboBoxClass 中刪除類別
                        foreach (string className in classesToDelete)
                        {
                            labelClassList.Remove(className);
                            comboBoxClass.Items.Remove(className);
                        }
                        var classNames = labelClassList.Keys.ToList();
                        labelClassList.Clear();
                        for (int i = 0; i < classNames.Count; i++)
                        {
                            labelClassList.Add(classNames[i], (Int16)i);
                        }
                        AIClassList.SaveLabelClassNameToJson(labelClassList, currentRecipeNumber);
                        // 更新所有標註檔案中的 ClassId
                        UpdateLabelFilesAfterClassDeletion(classNameToId, classesToDelete);

                        // 重新載入當前圖片的標註
                        if (!isInitial && imageFiles.Count > 0)
                        {
                            LoadCurrentImage();
                        }
                        else
                        {
                            // 如果是初始圖片，清除當前標註中被刪除類別的框
                            boxes.RemoveAll(box => classesToDelete.Contains(GetClassNameById(box.ClassId, classNameToId)));
                            pictureBox1.Invalidate();
                        }

                        // 設定 comboBoxClass 的選擇
                        if (comboBoxClass.Items.Count > 0)
                        {
                            comboBoxClass.SelectedIndex = 0;
                        }

                        string successMessage = GV.AppSettingParm.Language != "default"
                            ? string.Format(GV.Dlang.strDeleteSuccess, classesToDelete.Count)
                            : $"成功刪除 {classesToDelete.Count} 個類別！";
                        string successTitle = GV.AppSettingParm.Language != "default"
                            ? GV.Dlang.strComplete
                            : "完成";
                        MessageBox.Show(successMessage, successTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        string errorMessage = GV.AppSettingParm.Language != "default"
                            ? string.Format(GV.Dlang.strDeleteError, ex.Message)
                            : $"刪除類別時發生錯誤: {ex.Message}";
                        string errorTitle = GV.AppSettingParm.Language != "default"
                            ? GV.Dlang.strError
                            : "錯誤";
                        MessageBox.Show(errorMessage, errorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // 輔助方法：更新所有標註檔案
        private void UpdateLabelFilesAfterClassDeletion(Dictionary<string, int> oldClassNameToId, List<string> deletedClasses)
        {
            // 獲取所有 .txt 標註檔案
            string[] labelFiles = Directory.GetFiles(imageDir, "*.txt", SearchOption.TopDirectoryOnly);

            // 建立新的類別ID映射
            Dictionary<int, int> oldIdToNewId = new Dictionary<int, int>();
            int newId = 0;

            for (int oldId = 0; oldId < comboBoxClass.Items.Count + deletedClasses.Count; oldId++)
            {
                string className = GetClassNameById(oldId, oldClassNameToId);
                if (className != null && !deletedClasses.Contains(className))
                {
                    oldIdToNewId[oldId] = newId;
                    newId++;
                }
            }

            foreach (string labelFile in labelFiles)
            {
                try
                {
                    List<string> updatedLines = new List<string>();
                    bool fileModified = false;

                    foreach (string line in File.ReadAllLines(labelFile))
                    {
                        var parts = line.Split(' ');
                        if (parts.Length == 5)
                        {
                            int oldClassId = int.Parse(parts[0]);

                            // 如果該類別被刪除，跳過這一行
                            if (!oldIdToNewId.ContainsKey(oldClassId))
                            {
                                fileModified = true;
                                continue;
                            }

                            // 更新類別ID
                            int newClassId = oldIdToNewId[oldClassId];
                            if (newClassId != oldClassId)
                            {
                                fileModified = true;
                            }

                            updatedLines.Add($"{newClassId} {parts[1]} {parts[2]} {parts[3]} {parts[4]}");
                        }
                    }

                    // 如果檔案有變更，寫回檔案
                    if (fileModified)
                    {
                        if (updatedLines.Count > 0)
                        {
                            File.WriteAllLines(labelFile, updatedLines);
                        }
                        else
                        {
                            // 如果所有標註都被刪除，刪除標註檔案
                            File.Delete(labelFile);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // 記錄錯誤但繼續處理其他檔案
                    System.Diagnostics.Debug.WriteLine($"更新檔案 {labelFile} 時發生錯誤: {ex.Message}");
                }
            }
        }

        // 輔助方法：根據ID獲取類別名稱
        private string GetClassNameById(int classId, Dictionary<string, int> classNameToId)
        {
            foreach (var kvp in classNameToId)
            {
                if (kvp.Value == classId)
                {
                    return kvp.Key;
                }
            }
            return null;
        }

        private async void btTrain_Click(object sender, EventArgs e)
        {
            try
            {
                string trainBasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Python", "Train");
                string modelsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Python", "Models");
                string projectDir = Path.Combine(trainBasePath, $"{currentRecipeNumber}", "dataset");
                string dataYamlPath = Path.Combine(projectDir, "data.yaml");
                string backupModelPath = Path.Combine("C:\\AIData", "Backup");
                bool trainingSuccess = false;

                if (!File.Exists(dataYamlPath))
                {
                    string message = GV.AppSettingParm.Language != "default"
                       ? GV.Dlang.strNoImageFound
                       : "找不到 data.yaml 檔案！\n請先執行「整理資料集」功能。";
                    string title = GV.AppSettingParm.Language != "default"
                        ? GV.Dlang.strError
                        : "錯誤";
                    MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string modelFileName = $"best{currentRecipeNumber}.pt";
                string modelPath = Path.Combine(modelsPath, modelFileName);

                if (!File.Exists(modelPath))
                {
                    string yolov8nPath = Path.Combine(modelsPath, "yolov8n.pt");
                    if (!File.Exists(yolov8nPath))
                    {
                        string message = GV.AppSettingParm.Language != "default"
                            ? GV.Dlang.strNoImageFound
                            : $"找不到模型檔案！\n請確保以下任一檔案存在：\n1. {modelPath}\n2. {yolov8nPath}";
                        string title = GV.AppSettingParm.Language != "default"
                            ? GV.Dlang.strError
                            : "錯誤";
                        MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    modelPath = yolov8nPath;
                }

                string confirmMessage = GV.AppSettingParm.Language != "default"
                     ? string.Format(GV.Dlang.strTrainingConfirm, currentRecipeNumber, Path.GetFileName(dataYamlPath), Path.GetFileName(modelPath))
                     : $"準備開始訓練 Recipe {currentRecipeNumber} 的模型\n\n" +
                       $"資料集: {Path.GetFileName(dataYamlPath)}\n" +
                       $"基礎模型: {Path.GetFileName(modelPath)}\n" +
                       $"訓練參數: epochs=50, imgsz=2000\n\n" +
                       $"訓練將在背景執行，您可以繼續使用程式。\n" +
                       $"確定要開始嗎？";
                string confirmTitle = GV.AppSettingParm.Language != "default"
                    ? GV.Dlang.strConfirmTraining
                    : "確認訓練";

                DialogResult confirmResult = MessageBox.Show(
                    confirmMessage,
                    confirmTitle,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmResult == DialogResult.No)
                {
                    return;
                }

                // ✅ 禁用訓練按鈕，顯示訓練中狀態
                btTrain.Enabled = false;
                btTrain.Text = "訓練中...";

                string launchedMessage = GV.AppSettingParm.Language != "default"
                    ? string.Format(GV.Dlang.strTrainingStarted, currentRecipeNumber)
                    : $"Recipe {currentRecipeNumber} 開始背景訓練！\n\n" +
                      $"訓練過程將在背景執行，您可以繼續使用程式。\n" +
                      $"訓練完成後會自動通知。";
                string launchedTitle = GV.AppSettingParm.Language != "default"
                    ? GV.Dlang.strTrainingLaunched
                    : "訓練已啟動";

                MessageBox.Show(launchedMessage, launchedTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);


                // ✅ 使用 Task.Run 在背景執行緒執行訓練
                await Task.Run(async () =>
                {
                    try
                    {
                        string arguments = $"task=detect mode=train model=\"{modelPath}\" data=\"{dataYamlPath}\" epochs=50 imgsz=2000 rect=True batch=2";

                        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = "yolo",
                            Arguments = arguments,
                            WorkingDirectory = projectDir,
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            CreateNoWindow = true,
                            WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden
                        };

                        using (System.Diagnostics.Process process = new System.Diagnostics.Process())
                        {
                            process.StartInfo = startInfo;

                            // 處理輸出
                            process.OutputDataReceived += (s, args) =>
                            {
                                if (!string.IsNullOrEmpty(args.Data))
                                {
                                    System.Diagnostics.Debug.WriteLine($"[YOLO訓練-Recipe{currentRecipeNumber}] {args.Data}");
                                }
                            };

                            process.ErrorDataReceived += (s, args) =>
                            {
                                if (!string.IsNullOrEmpty(args.Data))
                                {
                                    System.Diagnostics.Debug.WriteLine($"[YOLO錯誤-Recipe{currentRecipeNumber}] {args.Data}");
                                }
                            };

                            process.Start();
                            process.BeginOutputReadLine();
                            process.BeginErrorReadLine();

                            // ✅ 使用 WaitForExitAsync (自訂實現)
                            await WaitForExitAsync(process);

                            // ✅ 訓練完成後處理結果
                            if (process.ExitCode == 0)
                            {
                                // 尋找並複製新模型
                                string runsPath = Path.Combine(projectDir, "runs", "detect");
                                string latestTrainPath = GetLatestTrainFolder(runsPath);
                                trainingSuccess = true;
                                if (!string.IsNullOrEmpty(latestTrainPath))
                                {
                                    string newBestPt = Path.Combine(latestTrainPath, "weights", "best.pt");

                                    if (File.Exists(newBestPt))
                                    {
                                        string targetModelPath = Path.Combine(modelsPath, $"best{currentRecipeNumber}.pt");

                                        // 確保 Models 資料夾存在
                                        if (!Directory.Exists(modelsPath))
                                        {
                                            Directory.CreateDirectory(modelsPath);
                                        }

                                        // 備份舊模型
                                        if (File.Exists(targetModelPath))
                                        {
                                            string backupPath = Path.Combine(
                                                backupModelPath,
                                                $"best{currentRecipeNumber}_backup_{DateTime.Now:yyyyMMdd_HHmmss}.pt");
                                            File.Copy(targetModelPath, backupPath, true);
                                            System.Diagnostics.Debug.WriteLine($"[Recipe{currentRecipeNumber}] 舊模型已備份: {backupPath}");
                                        }

                                        // 複製新模型
                                        File.Copy(newBestPt, targetModelPath, true);
                                        System.Diagnostics.Debug.WriteLine($"[Recipe{currentRecipeNumber}] 新模型已儲存: {targetModelPath}");

                                        // ✅ 在 UI 執行緒上顯示完成訊息
                                        Invoke(new Action(() =>
                                        {
                                            string successMsg = GV.AppSettingParm.Language != "default"
                                                ? string.Format(GV.Dlang.strTrainingComplete, currentRecipeNumber, targetModelPath, latestTrainPath)
                                                : $"Recipe {currentRecipeNumber} 模型訓練完成！\n\n" +
                                                  $"新模型已儲存至:\n{targetModelPath}\n\n" +
                                                  $"訓練結果位於:\n{latestTrainPath}";
                                            string successTitle = GV.AppSettingParm.Language != "default"
                                                ? GV.Dlang.strTrainingSuccess
                                                : "訓練成功";
                                            MessageBox.Show(successMsg, successTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }));
                                        if (trainingSuccess)
                                        {
                                            // 在 UI 執行緒上執行重新載入
                                            Invoke(new Action(async () =>
                                            {
                                                GM.WriteToStatusTextBox("訓練完成，正在重新啟動 AI 服務並載入新模型...");

                                                try
                                                {
                                                    // 呼叫 FormMain 的重新啟動方法
                                                    bool reloadSuccess = await FormMain.HwndFormMain.RestartAIServiceAfterTraining(currentRecipeNumber);

                                                    if (reloadSuccess)
                                                    {
                                                        string msg = GV.AppSettingParm.Language != "default"
                                                            ? GV.Dlang.strTrainingSuccess
                                                            : $"訓練完成！\nAI 服務已重新啟動，新模型已載入 (Recipe {currentRecipeNumber})";
                                                        string title = GV.AppSettingParm.Language != "default"
                                                            ? GV.Dlang.strTrainingSuccess
                                                            : "訓練成功";
                                                        MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                    }
                                                    else
                                                    {
                                                        string msg = GV.AppSettingParm.Language != "default"
                                                            ? GV.Dlang.strWarning
                                                            : "訓練完成，但 AI 模型重新載入失敗。\n請手動檢查 Python 服務狀態。";
                                                        string title = GV.AppSettingParm.Language != "default"
                                                            ? GV.Dlang.strWarning
                                                            : "警告";
                                                        MessageBox.Show(msg, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    GM.WriteToStatusTextBox($"重新載入 AI 模型時發生錯誤: {ex.Message}");
                                                    MessageBox.Show(
                                                        $"重新載入失敗: {ex.Message}",
                                                        "錯誤",
                                                        MessageBoxButtons.OK,
                                                        MessageBoxIcon.Error
                                                    );
                                                }
                                            }));
                                        }
                                    }
                                    else
                                    {
                                        System.Diagnostics.Debug.WriteLine($"[Recipe{currentRecipeNumber}] 找不到新模型: {newBestPt}");

                                        //Invoke(new Action(() =>
                                        //{
                                        //    //MessageBox.Show(
                                        //    //    $"訓練完成但找不到新模型檔案！\n預期路徑: {newBestPt}",
                                        //    //    "警告",
                                        //    //    MessageBoxButtons.OK,
                                        //    //    MessageBoxIcon.Warning);
                                        //}));
                                    }
                                }
                                else
                                {
                                    System.Diagnostics.Debug.WriteLine($"[Recipe{currentRecipeNumber}] 找不到訓練輸出目錄");

                                    //Invoke(new Action(() =>
                                    //{
                                    //    MessageBox.Show(
                                    //        "訓練完成但無法找到輸出目錄！",
                                    //        "警告",
                                    //        MessageBoxButtons.OK,
                                    //        MessageBoxIcon.Warning);
                                    //}));
                                }
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine($"[Recipe{currentRecipeNumber}] 訓練失敗，退出代碼: {process.ExitCode}");

                                //Invoke(new Action(() =>
                                //{
                                //    MessageBox.Show(
                                //        $"Recipe {currentRecipeNumber} 模型訓練失敗！\n\n" +
                                //        $"退出代碼: {process.ExitCode}\n\n" +
                                //        $"請檢查：\n" +
                                //        $"1. 是否安裝了 YOLO (ultralytics)\n" +
                                //        $"2. data.yaml 格式是否正確\n" +
                                //        $"3. 標註資料是否完整",
                                //        "訓練失敗",
                                //        MessageBoxButtons.OK,
                                //        MessageBoxIcon.Error);
                                //}));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"[Recipe{currentRecipeNumber}] 訓練異常: {ex.Message}");

                        //Invoke(new Action(() =>
                        //{
                        //    MessageBox.Show(
                        //        $"訓練過程發生錯誤:\n{ex.Message}",
                        //        "錯誤",
                        //        MessageBoxButtons.OK,
                        //        MessageBoxIcon.Error);
                        //}));
                    }
                });
            }
            catch (Exception ex)
            {
                //MessageBox.Show(
                //    $"啟動訓練時發生錯誤:\n{ex.Message}",
                //    "錯誤",
                //    MessageBoxButtons.OK,
                //    MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine($"[Recipe{currentRecipeNumber}] 啟動訓練錯誤: {ex.Message}");
            }
            finally
            {
                // ✅ 恢復按鈕狀態
                btTrain.Enabled = true;
                btTrain.Text = "Train Model";
            }
        }

        private Task WaitForExitAsync(Process process)
        {
            var tcs = new TaskCompletionSource<bool>();

            process.EnableRaisingEvents = true;
            process.Exited += (sender, args) =>
            {
                tcs.TrySetResult(true);
            };

            if (process.HasExited)
            {
                tcs.TrySetResult(true);
            }

            return tcs.Task;
        }

        private string GetLatestTrainFolder(string runsPath)
        {
            try
            {
                if (!Directory.Exists(runsPath))
                    return null;

                var directories = Directory.GetDirectories(runsPath, "train*")
                    .OrderByDescending(d => Directory.GetCreationTime(d))
                    .ToList();

                return directories.FirstOrDefault();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"獲取訓練目錄失敗: {ex.Message}");
                return null;
            }
        }

        private void btDeleteImage_Click(object sender, EventArgs e)
        {
            try
            {
                if (isInitial || imageFiles.Count == 0) return;
                string currentImagePath = imageFiles[currentIndex];
                string currentImageName = Path.GetFileName(currentImagePath);
                string labelPath = Path.ChangeExtension(currentImagePath, ".txt");
                bool hasLabel = File.Exists(labelPath);


                string deleteMessage = GV.AppSettingParm.Language != "default"
                    ? GV.Dlang.strConfirmDeleteImage
                    : "確定要刪除當前圖片嗎？";
                string warningMessage = GV.AppSettingParm.Language != "default"
                    ? GV.Dlang.strCannotUndo
                    : "注意：此操作無法復原！";
                string confirmTitle = GV.AppSettingParm.Language != "default"
                    ? GV.Dlang.strConfirmDelete
                    : "確認刪除";

                DialogResult result = MessageBox.Show(
                    deleteMessage + "\n\n" + warningMessage,
                    confirmTitle,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.No)
                {
                    return;
                }
                int deletedIndex = currentIndex;
                if (pictureBox1.Image != null)
                {
                    pictureBox1.Image.Dispose();
                    pictureBox1.Image = null;
                }
                if (File.Exists(currentImagePath))
                {
                    File.Delete(currentImagePath);
                }
                if (hasLabel)
                {
                    File.Delete(labelPath);
                }
                imageFiles.RemoveAt(deletedIndex);
                if (imageFiles.Count == 0)
                {

                    currentIndex = 0;
                    boxes.Clear();
                    pictureBox1.Image = null;
                    pictureBox1.Invalidate();

                    string message = GV.AppSettingParm.Language != "default"
                        ? GV.Dlang.strImageDeleted
                        : "圖片已刪除！\n目前資料夾中已無圖片。";
                    string title = GV.AppSettingParm.Language != "default"
                        ? GV.Dlang.strDeleteComplete
                        : "刪除完成";
                    MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {

                    if (deletedIndex >= imageFiles.Count)
                    {

                        currentIndex = imageFiles.Count - 1;
                    }
                    else
                    {

                        currentIndex = deletedIndex;
                    }
                    LoadCurrentImage();
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                string message = GV.AppSettingParm.Language != "default"
                    ? string.Format(GV.Dlang.strNoPermission, "", ex.Message)
                    : $"沒有權限刪除檔案！\n錯誤: {ex.Message}";
                string title = GV.AppSettingParm.Language != "default"
                    ? GV.Dlang.strPermissionError
                    : "權限錯誤";
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (IOException ex)
            {
                string message = GV.AppSettingParm.Language != "default"
                    ? string.Format(GV.Dlang.strDeleteError, ex.Message)
                    : $"刪除檔案時發生 I/O 錯誤！\n錯誤: {ex.Message}\n\n可能原因：檔案正在被其他程式使用。";
                string title = GV.AppSettingParm.Language != "default"
                    ? GV.Dlang.strError
                    : "刪除失敗";
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                string message = GV.AppSettingParm.Language != "default"
                     ? string.Format(GV.Dlang.strDeleteError, ex.Message)
                     : $"刪除圖片時發生錯誤: {ex.Message}";
                string title = GV.AppSettingParm.Language != "default"
                    ? GV.Dlang.strError
                    : "錯誤";
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        void DrawYoloBox(Graphics graphics, YoloBox box, Color color)
        {
            //int w = skPattern.Width, h = skPattern.Height;
            int w = pictureBox1.Width, h = pictureBox1.Height;
            int boxW = (int)(box.W * w), boxH = (int)(box.H * h);
            int boxX = (int)(box.X * w - boxW / 2), boxY = (int)(box.Y * h - boxH / 2);
            graphics.DrawRectangle(new Pen(color, 2), boxX, boxY, boxW, boxH);
        }



        private void skPattern_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                endX = e.X; endY = e.Y;
                skPattern.Invalidate();
            }
        }

        private void skPattern_MouseDown(object sender, MouseEventArgs e)
        {
            isDrawing = true;
            startX = e.X;
            startY = e.Y;
        }
        private void OrganizeDatasetAndLaunchTraining()
        {
            string srcImgDir = imageDir;
            string trainBasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Python", "Train");
            string ModelsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Python", "Models");
            string ModelsBackupPath = Path.Combine("C:\\AIData", "Backup");

            string trainModelDir = @"C:\Project\TrainModel\bin\x64\Debug";
            string trainModelConfig = Path.Combine(trainModelDir, "AIConfig.json");
            string labelImagePath = Path.Combine(trainBasePath, $"{currentRecipeNumber}");
            string projectDir = Path.Combine(trainBasePath, $"{currentRecipeNumber}", "dataset");

            string[] allImages = Directory.GetFiles(srcImgDir, "*.jpg")
                .Concat(Directory.GetFiles(srcImgDir, "*.png"))
                .Concat(Directory.GetFiles(srcImgDir, "*.bmp"))
                .ToArray();

            if (allImages.Length == 0)
            {
                string message = GV.AppSettingParm.Language != "default"
                    ? GV.Dlang.strNoImageFound
                    : "找不到任何圖片檔案！";
                string title = GV.AppSettingParm.Language != "default"
                    ? GV.Dlang.strHint
                    : "提示";
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int totalImages = allImages.Length;
            int testCount = totalImages / 7;
            int valCount = totalImages / 7;
            int trainCount = totalImages - testCount - valCount;

            var rand = new Random();
            var shuffled = allImages.OrderBy(x => rand.Next()).ToList();

            var trainImgs = shuffled.Take(trainCount).ToList();
            var valImgs = shuffled.Skip(trainCount).Take(valCount).ToList();
            var testImgs = shuffled.Skip(trainCount + valCount).ToList();

            string[] folders =
            {
        "images/train", "images/val", "images/test",
        "labels/train", "labels/val", "labels/test"
    };

            foreach (var folder in folders)
                Directory.CreateDirectory(Path.Combine(projectDir, folder));

            MoveImgAndLabel(trainImgs, "images/train", "labels/train", projectDir);
            MoveImgAndLabel(valImgs, "images/val", "labels/val", projectDir);
            MoveImgAndLabel(testImgs, "images/test", "labels/test", projectDir);

            int nc = comboBoxClass.Items.Count;
            var names = string.Join(", ", comboBoxClass.Items.Cast<string>().Select(n => $"'{n}'"));
            string yaml = $@"train: ./images/train
val: ./images/val
test: ./images/test
nc: {nc}
names: [{names}]
";
            File.WriteAllText(Path.Combine(projectDir, "data.yaml"), yaml);

            // 儲存 AIConfig.json
            try
            {
                string configPath = Path.Combine(serviceExePath, "AIConfig.json");

                Dictionary<string, object> configDict = new Dictionary<string, object>();

                if (File.Exists(configPath))
                {
                    try
                    {
                        string existingJson = File.ReadAllText(configPath);
                        var existingConfig = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(existingJson);

                        if (existingConfig != null)
                        {
                            configDict = existingConfig;
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"讀取現有 AIConfig.json 失敗: {ex.Message}，將建立新檔案");
                    }
                }
                string trainModel = DetermineTrainModel(configPath, ModelsPath);
                configDict["labeledRecipeNumber"] = currentRecipeNumber;
                configDict["trainPath"] = projectDir;
                configDict["modelPath"] = ModelsPath;
                configDict["modelBackupPath"] = ModelsBackupPath;
                configDict["labelImage"] = labelImagePath;
                configDict["trainBasePath"] = trainBasePath;
                configDict["serviceExePath"] = serviceExePath;
                configDict["trainModel"] = trainModel;
                configDict["lastClassCount"] = comboBoxClass.Items.Count;
                configDict["lastClassNames"] = comboBoxClass.Items.Cast<string>().ToList();
                string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(configDict, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(configPath, jsonContent);

                if (!Directory.Exists(trainModelDir))
                {
                    Directory.CreateDirectory(trainModelDir);
                }
                File.WriteAllText(trainModelConfig, jsonContent);

                System.Diagnostics.Debug.WriteLine($"✅ AIConfig.json 已更新: labeledRecipeNumber = {currentRecipeNumber}");
                System.Diagnostics.Debug.WriteLine($"   完整路徑: {configPath}");
            }
            catch (Exception ex)
            {
                string message = GV.AppSettingParm.Language != "default"
                    ? string.Format(GV.Dlang.strSaveError, ex.Message)
                    : $"儲存配置到 AIConfig.json 時發生錯誤:\n{ex.Message}\n\n資料集整理已完成，但配置未更新。";
                string title = GV.AppSettingParm.Language != "default"
                    ? GV.Dlang.strWarning
                    : "警告";
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // 啟動 TrainModel.exe
            try
            {
                string labelImageExePath = @"C:\Project\TrainModel\bin\x64\Debug\TrainModel.exe";
                string labelImageDir = Path.GetDirectoryName(labelImageExePath);

                if (!File.Exists(labelImageExePath))
                {
                    string message = GV.AppSettingParm.Language != "default"
                        ? GV.Dlang.strNoImageFound
                        : $"找不到 TrainModel.exe！\n\n預期路徑:\n{labelImageExePath}\n\n請確認檔案是否存在。\n資料集整理已完成。";
                    string title = GV.AppSettingParm.Language != "default"
                        ? GV.Dlang.strHint
                        : "提示";
                    MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = labelImageExePath,
                        WorkingDirectory = labelImageDir,
                        UseShellExecute = true,
                        WindowStyle = ProcessWindowStyle.Normal
                    };

                    Process.Start(psi);
                    //System.Diagnostics.Debug.WriteLine($"✅ 已啟動 TrainModel.exe");
                    //System.Diagnostics.Debug.WriteLine($"   執行檔路徑: {labelImageExePath}");
                    //System.Diagnostics.Debug.WriteLine($"   工作目錄: {labelImageDir}");
                }
            }
            catch (Exception ex)
            {
                string message = GV.AppSettingParm.Language != "default"
                    ? string.Format(GV.Dlang.strDeleteError, ex.Message)
                    : $"啟動 TrainModel.exe 時發生錯誤:\n{ex.Message}\n\n資料集整理已完成。";
                string title = GV.AppSettingParm.Language != "default"
                    ? GV.Dlang.strError
                    : "錯誤";
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private string DetermineTrainModel(string configPath, string modelsPath)
        {
            string existingModelPath = Path.Combine(modelsPath, $"best{currentRecipeNumber}.pt");

            // 檢查是否有現有的訓練模型
            bool hasExistingModel = File.Exists(existingModelPath);

            // 檢查類別是否有變更
            bool labelChanged = CheckIfLabelChanged(configPath);

            // 第一次訓練或類別有變更時使用基礎模型
            if (!hasExistingModel || labelChanged)
            {
                System.Diagnostics.Debug.WriteLine($"[trainModel] 使用基礎模型 yolov8n.pt (hasExistingModel={hasExistingModel}, labelChanged={labelChanged})");
                return "yolov8n.pt";
            }

            // 使用現有的訓練模型
            System.Diagnostics.Debug.WriteLine($"[trainModel] 使用現有模型 best{currentRecipeNumber}.pt");
            return $"best{currentRecipeNumber}.pt";
        }
        private bool CheckIfLabelChanged(string configPath)
        {
            try
            {
                if (!File.Exists(configPath))
                {
                    return true; // 第一次訓練
                }

                string existingJson = File.ReadAllText(configPath);
                var existingConfig = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(existingJson);

                if (existingConfig == null)
                {
                    return true;
                }

                // 檢查是否為同一個 Recipe
                if (existingConfig.ContainsKey("labeledRecipeNumber"))
                {
                    int savedRecipeNumber = Convert.ToInt32(existingConfig["labeledRecipeNumber"]);
                    if (savedRecipeNumber != currentRecipeNumber)
                    {
                        return true; // 不同 Recipe，視為新訓練
                    }
                }
                else
                {
                    return true;
                }

                // 檢查類別數量是否變更
                if (existingConfig.ContainsKey("lastClassCount"))
                {
                    int savedClassCount = Convert.ToInt32(existingConfig["lastClassCount"]);
                    int currentClassCount = comboBoxClass.Items.Count;

                    if (savedClassCount != currentClassCount)
                    {
                        System.Diagnostics.Debug.WriteLine($"[labelChanged] 類別數量變更: {savedClassCount} -> {currentClassCount}");
                        return true;
                    }
                }
                else
                {
                    return true; // 沒有記錄過，視為新訓練
                }

                // 檢查類別名稱是否變更
                if (existingConfig.ContainsKey("lastClassNames"))
                {
                    var savedClassNames = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(
                        existingConfig["lastClassNames"].ToString());

                    var currentClassNames = comboBoxClass.Items.Cast<string>().ToList();

                    if (savedClassNames == null || !savedClassNames.SequenceEqual(currentClassNames))
                    {
                        System.Diagnostics.Debug.WriteLine($"[labelChanged] 類別名稱變更");
                        return true;
                    }
                }
                else
                {
                    return true;
                }

                return false; // 類別沒有變更
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[CheckIfLabelChanged] 檢查失敗: {ex.Message}");
                return true; // 發生錯誤時使用基礎模型
            }
        }
    }
}
