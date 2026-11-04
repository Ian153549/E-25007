//using Basler.Pylon;
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
using System.Threading.Tasks;
//using System.Web.UI.WebControls;
using System.Windows.Forms;
using TrainModel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;


namespace NSAA_16Axis
{
    public partial class DialogLabelImage : Form
    {
        string serviceExePath = GV.AIConfig.serviceExePath;
        List<string> imageFiles = new List<string>();
        int currentIndex = 0;
        string imageDir = "";
        List<YoloBox> boxes = new List<YoloBox>();
        bool isDrawing = false;
        int startX, startY, endX, endY;
        private Bitmap initialImage;
        private bool isInitial = false;
        private int nowMagni;
        private int currentRecipeNumber = GV.AIConfig.LabeledRecipeNumber;
        private Dictionary<string, Int16> labelClassList = new Dictionary<string, Int16>();
        private int initialClassCount = 0;
        private string initialClassNames = "";
        public string LabelImageDirOverride { get; set; }
        private string _trainBasePath = string.Empty;
        private bool _isSaving = false;
        private class OrganizeContext
        {
            public string SrcImgDir;
            public string TrainBasePath;
            public string ModelsPath;
            public string ModelsBackupPath;
            public string TrainModelDir;
            public string LabelImagePath;
            public string ProjectDir;
            public string ServiceExePath;
            public int RecipeNumber;
            public List<string> ClassNames;
        }
        public DialogLabelImage()
        {
            InitializeComponent();
            ApplyLanguage();
            //currentRecipeNumber = GV.NowRecipeNumber;
        }
        public DialogLabelImage(Bitmap initialImage, int nowMagnification)
        {
            InitializeComponent();
            this.initialImage = initialImage;
            isInitial = true;
            nowMagni = nowMagnification;
            ApplyLanguage();
            //currentRecipeNumber = GV.NowRecipeNumber;
        }

        public DialogLabelImage(int recipeNumber)
        {
            InitializeComponent();
            currentRecipeNumber = recipeNumber;
            ApplyLanguage();
        }
        private void ApplyLanguage()
        {
            // Buttons
            btnSave.Text = LanguageResource.Get("Save");
            btnClear.Text = LanguageResource.Get("Clear");
            btnPrev.Text = LanguageResource.Get("Previous");
            btnNext.Text = LanguageResource.Get("Next");
            btAdd.Text = LanguageResource.Get("Add");
            btDelete.Text = LanguageResource.Get("Delete");
            btDeleteImage.Text = LanguageResource.Get("DeleteImage");

            if (btnOrganizeAndYaml != null)
            {
                btnOrganizeAndYaml.Text = LanguageResource.Get("OrganizeDataset");
            }

            // Labels
            lb.Text = LanguageResource.Get("NewClass");

            // GroupBoxes
            groupBox1.Text = LanguageResource.Get("Class");
            groupBox2.Text = LanguageResource.Get("Image");
        }
        // ===== LabelType / Padding 相關 =====
        private int GetPaddingPixels()
        {
            int value = (int)(nudPadding?.Value ?? 0);
            bool isShrink = cbPaddingDir?.SelectedItem?.ToString() == "內縮";
            return isShrink ? -value : value;
        }

        private Rectangle ApplyPaddingToRect(Rectangle rect, int paddingPx)
        {
            int x = rect.X - paddingPx;
            int y = rect.Y - paddingPx;
            int w = rect.Width + paddingPx * 2;
            int h = rect.Height + paddingPx * 2;
            if (w < 1) w = 1;
            if (h < 1) h = 1;
            return new Rectangle(x, y, w, h);
        }

        private bool IsSquareLabelType()
        {
            return cbLabelType != null && cbLabelType.SelectedItem != null && cbLabelType.SelectedItem.ToString() == "正方形";
        }

        private Rectangle BuildDrawRectangle(int sx, int sy, int ex, int ey)
        {
            int cx = (sx + ex) / 2;
            int cy = (sy + ey) / 2;

            int halfW, halfH;

            if (IsSquareLabelType())
            {
                // 正方形：以中心點為準，取長邊的一半作為邊長
                int half = Math.Max(Math.Abs(ex - sx), Math.Abs(ey - sy)) / 2;
                halfW = half;
                halfH = half;
            }
            else
            {
                // 方形：以中心點為準，保留原始寬高
                halfW = Math.Abs(ex - sx) / 2;
                halfH = Math.Abs(ey - sy) / 2;
            }

            return new Rectangle(cx - halfW, cy - halfH, halfW * 2, halfH * 2);
        }

        // ===== 座標轉換（PictureBox Zoom 模式）=====
        private Rectangle GetImageRectInPictureBox()
        {
            if (pictureBox1.Image == null)
                return pictureBox1.ClientRectangle;

            float imgW = pictureBox1.Image.Width;
            float imgH = pictureBox1.Image.Height;
            float boxW = pictureBox1.Width;
            float boxH = pictureBox1.Height;

            float scale = Math.Min(boxW / imgW, boxH / imgH);
            int drawW = (int)(imgW * scale);
            int drawH = (int)(imgH * scale);
            int offsetX = (int)((boxW - drawW) / 2f);
            int offsetY = (int)((boxH - drawH) / 2f);

            return new Rectangle(offsetX, offsetY, drawW, drawH);
        }

        private Rectangle NormalizeRectangle(int x1, int y1, int x2, int y2)
        {
            int left = Math.Min(x1, x2);
            int top = Math.Min(y1, y2);
            int right = Math.Max(x1, x2);
            int bottom = Math.Max(y1, y2);
            return Rectangle.FromLTRB(left, top, right, bottom);
        }

        private Rectangle ImageRectToPictureBoxRect(Rectangle imageRect)
        {
            Rectangle pbImageRect = GetImageRectInPictureBox();

            float scaleX = pbImageRect.Width / (float)pictureBox1.Image.Width;
            float scaleY = pbImageRect.Height / (float)pictureBox1.Image.Height;

            int x = pbImageRect.X + (int)Math.Round(imageRect.X * scaleX);
            int y = pbImageRect.Y + (int)Math.Round(imageRect.Y * scaleY);
            int w = (int)Math.Round(imageRect.Width * scaleX);
            int h = (int)Math.Round(imageRect.Height * scaleY);

            return new Rectangle(x, y, w, h);
        }

        private Rectangle PictureBoxRectToImageRect(Rectangle pictureBoxRect)
        {
            if (pictureBox1.Image == null)
                return Rectangle.Empty;

            Rectangle pbImageRect = GetImageRectInPictureBox();
            Rectangle clippedPbRect = Rectangle.Intersect(pictureBoxRect, pbImageRect);

            if (clippedPbRect.Width <= 0 || clippedPbRect.Height <= 0)
                return Rectangle.Empty;

            float scaleX = pictureBox1.Image.Width / (float)pbImageRect.Width;
            float scaleY = pictureBox1.Image.Height / (float)pbImageRect.Height;

            int x = (int)Math.Round((clippedPbRect.X - pbImageRect.X) * scaleX);
            int y = (int)Math.Round((clippedPbRect.Y - pbImageRect.Y) * scaleY);
            int w = Math.Max(1, (int)Math.Round(clippedPbRect.Width * scaleX));
            int h = Math.Max(1, (int)Math.Round(clippedPbRect.Height * scaleY));

            Rectangle imageBounds = new Rectangle(0, 0, pictureBox1.Image.Width, pictureBox1.Image.Height);
            return Rectangle.Intersect(new Rectangle(x, y, w, h), imageBounds);
        }

        // ===== 框選範圍內自動偵測輪廓，取得貼合的標註框 =====
        private bool TryBuildDetectedLabelRect(System.Drawing.Point startPoint, System.Drawing.Point endPoint, out Rectangle resultRect)
        {
            resultRect = Rectangle.Empty;

            if (pictureBox1.Image == null)
                return false;

            Rectangle pbImageRect = GetImageRectInPictureBox();
            Rectangle selectionPb = Rectangle.Intersect(
                NormalizeRectangle(startPoint.X, startPoint.Y, endPoint.X, endPoint.Y),
                pbImageRect);

            if (selectionPb.Width < 2 || selectionPb.Height < 2)
                return false;

            float scaleX = pictureBox1.Image.Width / (float)pbImageRect.Width;
            float scaleY = pictureBox1.Image.Height / (float)pbImageRect.Height;

            Rectangle selectionImg = new Rectangle(
                Math.Max(0, (int)Math.Floor((selectionPb.X - pbImageRect.X) * scaleX)),
                Math.Max(0, (int)Math.Floor((selectionPb.Y - pbImageRect.Y) * scaleY)),
                Math.Max(1, (int)Math.Ceiling(selectionPb.Width * scaleX)),
                Math.Max(1, (int)Math.Ceiling(selectionPb.Height * scaleY)));

            Rectangle imageBounds = new Rectangle(0, 0, pictureBox1.Image.Width, pictureBox1.Image.Height);
            selectionImg = Rectangle.Intersect(selectionImg, imageBounds);

            if (selectionImg.Width < 2 || selectionImg.Height < 2)
                return false;

            using (Bitmap bmp = new Bitmap(pictureBox1.Image))
            using (Mat src = OpenCvSharp.Extensions.BitmapConverter.ToMat(bmp))
            using (Mat roi = new Mat(src, new OpenCvSharp.Rect(selectionImg.X, selectionImg.Y, selectionImg.Width, selectionImg.Height)))
            using (Mat gray = new Mat())
            using (Mat blur = new Mat())
            using (Mat bin = new Mat())
            {
                if (roi.Channels() == 1)
                    roi.CopyTo(gray);
                else
                    Cv2.CvtColor(roi, gray, ColorConversionCodes.BGR2GRAY);

                Cv2.GaussianBlur(gray, blur, new OpenCvSharp.Size(3, 3), 0);
                Cv2.Threshold(blur, bin, 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);

                OpenCvSharp.Point[][] contours;
                HierarchyIndex[] hierarchy;
                Cv2.FindContours(bin, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

                if (contours == null || contours.Length == 0)
                    return false;

                OpenCvSharp.Rect bestRect = new OpenCvSharp.Rect();
                double bestArea = 0;

                foreach (var contour in contours)
                {
                    OpenCvSharp.Rect rect = Cv2.BoundingRect(contour);
                    double area = rect.Width * rect.Height;

                    if (area < 25)
                        continue;

                    if (area > bestArea)
                    {
                        bestArea = area;
                        bestRect = rect;
                    }
                }

                if (bestArea <= 0)
                    return false;

                Rectangle detectedImgRect = new Rectangle(
                    selectionImg.X + bestRect.X,
                    selectionImg.Y + bestRect.Y,
                    bestRect.Width,
                    bestRect.Height);

                int centerX = detectedImgRect.X + detectedImgRect.Width / 2;
                int centerY = detectedImgRect.Y + detectedImgRect.Height / 2;

                if (IsSquareLabelType())
                {
                    int side = Math.Max(detectedImgRect.Width, detectedImgRect.Height);
                    detectedImgRect = new Rectangle(
                        centerX - side / 2,
                        centerY - side / 2,
                        side,
                        side);
                }
                else
                {
                    detectedImgRect = new Rectangle(
                        centerX - detectedImgRect.Width / 2,
                        centerY - detectedImgRect.Height / 2,
                        detectedImgRect.Width,
                        detectedImgRect.Height);
                }

                detectedImgRect = ApplyPaddingToRect(detectedImgRect, GetPaddingPixels());
                detectedImgRect = Rectangle.Intersect(detectedImgRect, imageBounds);

                if (detectedImgRect.Width <= 0 || detectedImgRect.Height <= 0)
                    return false;

                resultRect = detectedImgRect;
                return true;
            }
        }

        // ===== Info 面板 =====
        private void RefreshLvInfo()
        {
            lvInfo.BeginUpdate();
            lvInfo.Items.Clear();

            if (lvInfo.View != View.Details)
                lvInfo.View = View.Details;

            if (lvInfo.Columns.Count == 0)
            {
                lvInfo.Columns.Add("項目", 110);
                lvInfo.Columns.Add("數量", 50);
            }

            int labeled = 0;
            int unlabeled = 0;
            foreach (var imgPath in imageFiles)
            {
                string labelPath = Path.ChangeExtension(imgPath, ".txt");
                if (File.Exists(labelPath))
                {
                    var lines = File.ReadAllLines(labelPath).Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();
                    if (lines.Length > 0)
                        labeled++;
                    else
                        unlabeled++;
                }
                else
                {
                    unlabeled++;
                }
            }

            var headerItem = new ListViewItem("__圖片統計__");
            headerItem.SubItems.Add("");
            headerItem.BackColor = System.Drawing.Color.LightGray;
            lvInfo.Items.Add(headerItem);

            AddInfoRow("已標註圖片", labeled.ToString());
            AddInfoRow("未標記圖片", unlabeled.ToString());
            AddInfoRow("圖片總數", imageFiles.Count.ToString());

            var classCount = new Dictionary<string, int>();
            foreach (var imgPath in imageFiles)
            {
                string labelPath = Path.ChangeExtension(imgPath, ".txt");
                if (!File.Exists(labelPath)) continue;

                foreach (var line in File.ReadAllLines(labelPath))
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    var parts = line.Split(' ');
                    if (parts.Length < 5) continue;

                    int classId;
                    if (int.TryParse(parts[0], out classId))
                    {
                        string className = GetClassNameById(classId);
                        if (string.IsNullOrEmpty(className))
                        {
                            className = "Class " + classId;
                        }
                        if (!classCount.ContainsKey(className))
                        {
                            classCount[className] = 0;
                        }
                        classCount[className]++;
                    }
                }
            }

            var headerItem2 = new ListViewItem("--類別統計---");
            headerItem2.SubItems.Add("");
            headerItem2.BackColor = System.Drawing.Color.LightGray;
            lvInfo.Items.Add(headerItem2);

            if (classCount.Count == 0)
            {
                AddInfoRow("(尚無標註)", "");
            }
            else
            {
                foreach (var kvp in classCount.OrderBy(k => k.Key))
                    AddInfoRow(kvp.Key, kvp.Value.ToString());
            }

            lvInfo.EndUpdate();
        }

        private void AddInfoRow(string label, string value)
        {
            var item = new ListViewItem(label);
            item.SubItems.Add(value);
            lvInfo.Items.Add(item);
        }
        private void btnOpenFolder_Click(object sender, EventArgs e)
        {

        }
        private void DialogLabelImage_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(LabelImageDirOverride) && Directory.Exists(LabelImageDirOverride))
            {
                // 以主畫面目前的資料集路徑為準
                imageDir = LabelImageDirOverride.TrimEnd('\\');

                DirectoryInfo recipeDir = new DirectoryInfo(imageDir);           // ...\Train\1
                int recipeFromPath;
                if (int.TryParse(recipeDir.Name, out recipeFromPath))
                {
                    currentRecipeNumber = recipeFromPath;
                }

                // 同步 GV.AIConfig，讓儲存 / 整理資料集 / data.yaml 都寫到同一組路徑
                if (GV.AIConfig != null && recipeDir.Parent != null)
                {
                    GV.AIConfig.LabeledRecipeNumber = currentRecipeNumber;
                    GV.AIConfig.trainBasePath = recipeDir.Parent.FullName;                   // ...\Train
                    GV.AIConfig.labelImage = imageDir;                                       // ...\Train\1
                    GV.AIConfig.TrainPath = Path.Combine(imageDir, "dataset");               // ...\Train\1\dataset
                    _trainBasePath = recipeDir.Parent.FullName;
                }
            }
            else
            {
                string trainBasePath = GV.AIConfig?.trainBasePath;
                imageDir = Path.Combine(trainBasePath, $"{currentRecipeNumber}");
            }
            //string trainBasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Python", "Train");
            //string trainBasePath = GV.AIConfig?.trainBasePath;
            //imageDir = Path.Combine(trainBasePath, $"{currentRecipeNumber}");
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
                this.Text = $"{LanguageResource.Get("Recipe")}{currentRecipeNumber} - Path:{imageDir}";
                labelClassList = AIClassList.LoadLabelClassNameFromJson(currentRecipeNumber);

                // 載入類別列表
                comboBoxClass.Items.Clear();
                foreach (var className in labelClassList.OrderBy(kvp => kvp.Value))
                {
                    comboBoxClass.Items.Add(className.Key);

                }
                initialClassCount = labelClassList.Count;
                initialClassNames = string.Join(",", labelClassList.Keys.OrderBy(k => k));
                if (comboBoxClass.Items.Count > 0)
                {
                    comboBoxClass.SelectedIndex = 0;
                }
                if (cbLabelType != null && cbLabelType.Items.Count > 0 && cbLabelType.SelectedIndex < 0)
                {
                    int rectIndex = cbLabelType.Items.IndexOf("方形");
                    cbLabelType.SelectedIndex = rectIndex >= 0 ? rectIndex : 0;
                }
                if (cbPaddingDir != null && cbPaddingDir.Items.Count > 0 && cbPaddingDir.SelectedIndex < 0)
                {
                    cbPaddingDir.SelectedIndex = 0;
                }
                this.Text = $"Recipe{currentRecipeNumber} - Path:{imageDir} ";
                if (btnOrganizeAndYaml != null)
                {
                    btnOrganizeAndYaml.Visible = false;
                }
                if (imageFiles.Count > 0)
                {
                    currentIndex = 0;
                    LoadCurrentImage();

                    //  如果有 initialImage，釋放它（不再使用）
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

                    // 顯示提示訊息（可選）
                    // MessageBox.Show(
                    //     $"Recipe {currentRecipeNumber} 資料夾中沒有圖片。\n請先擷取影像。",
                    //     "提示",
                    //     MessageBoxButtons.OK,
                    //     MessageBoxIcon.Information);
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(
                    string.Format(LanguageResource.Get("NoPermissionAccess"), imageDir, ex.Message),
                    LanguageResource.Get("PermissionError"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format(LanguageResource.Get("LoadError"), ex.Message),
                    LanguageResource.Get("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            RefreshLvInfo();
        }
        private bool ShouldUseBaseModel()
        {
            string modelDir = GV.AIConfig.ModelPath;
            if (string.IsNullOrEmpty(modelDir))
            {
                modelDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "models");
            }

            string existingModelName = $"best{currentRecipeNumber}.pt";
            string existingModelPath = Path.Combine(modelDir, existingModelName);

            // 1. 第一次訓練（沒有現有模型）
            if (!File.Exists(existingModelPath))
            {
                return true;
            }

            // 2. 檢查類別是否有變更（新增或刪除）
            int currentClassCount = labelClassList.Count;
            string currentClassNames = string.Join(",", labelClassList.Keys.OrderBy(k => k));

            // 與上次儲存的狀態比較
            int lastCount = GV.AIConfig?.lastClassCount ?? 0;
            List<string> lastNameList = GV.AIConfig?.lastClassNames ?? new List<string>();
            string lastClassNames = string.Join(",", lastNameList.OrderBy(k => k));

            if (currentClassCount != lastCount || currentClassNames != lastClassNames)
            {
                return true;
            }

            return false;
        }

        // 新增方法：更新 trainModel 設定
        private void UpdateTrainModelConfig()
        {
            string modelDir = GV.AIConfig.ModelPath;
            if (string.IsNullOrEmpty(modelDir))
            {
                modelDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "models");
            }

            string trainModel;
            if (ShouldUseBaseModel())
            {
                // 使用基礎模型 yolov8n.pt
                trainModel = "yolov8n.pt";
                Debug.WriteLine($"[TrainModel] 使用基礎模型: {trainModel}");
            }
            else
            {
                // 使用現有模型繼續訓練
                trainModel = $"best{currentRecipeNumber}.pt";
                Debug.WriteLine($"[TrainModel] 使用現有模型: {trainModel}");
            }

            // 更新 AIConfig
            GV.AIConfig.trainModel = trainModel;
            GV.AIConfig.lastClassCount = labelClassList.Count;
            GV.AIConfig.lastClassNames = labelClassList.Keys.OrderBy(k => k).ToList();
            GV.AIConfig.SaveToJson();
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
                    if (parts.Length != 5) continue;
                    //string className = GetClassNameById(int.Parse(parts[0]));
                    //if (parts.Length == 5)
                    //    boxes.Add(new YoloBox
                    //    {
                    //        ClassId = int.Parse(parts[0]),
                    //        X = float.Parse(parts[1]),
                    //        Y = float.Parse(parts[2]),
                    //        W = float.Parse(parts[3]),
                    //        H = float.Parse(parts[4]),
                    //        ClassName = className
                    //    });
                    int classId = int.Parse(parts[0]);
                    boxes.Add(new YoloBox
                    {
                        ClassId = classId,
                        X = float.Parse(parts[1]),
                        Y = float.Parse(parts[2]),
                        W = float.Parse(parts[3]),
                        H = float.Parse(parts[4]),
                        ClassName = GetClassNameById(classId)
                    });
                }
            }
            pictureBox1.Invalidate();
            RefreshLvInfo();
        }

        private string GetClassNameById(int classId)
        {
            try
            {
                if (classId >= 0 && classId < comboBoxClass.Items.Count)
                {
                    return comboBoxClass.Items[classId].ToString();
                }

                // 如果 ClassId 超出範圍，嘗試從 labelClassList 反查
                foreach (var kvp in labelClassList)
                {
                    if (kvp.Value == classId)
                    {
                        return kvp.Key;
                    }
                }

                return ""; // 找不到時的預設名稱
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

        //private void btnSave_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (isInitial)
        //        {
        //            // 確保目錄存在
        //            if (!Directory.Exists(imageDir))
        //            {
        //                Directory.CreateDirectory(imageDir);
        //            }

        //            string magnificationStr = $"X{nowMagni + 1}";
        //            string fileName = $"labeled_{DateTime.Now:yyyyMMdd_HHmmss}_{magnificationStr}.bmp";
        //            string imgPath = Path.Combine(imageDir, fileName);

        //            // 儲存圖片（從 pictureBox1 儲存，因為 initialImage 可能已釋放）
        //            pictureBox1.Image.Save(imgPath);

        //            // 儲存標註
        //            string labelPath = Path.ChangeExtension(imgPath, ".txt");
        //            File.WriteAllLines(labelPath, boxes.Select(b => $"{b.ClassId} {b.X:F6} {b.Y:F6} {b.W:F6} {b.H:F6}"));

        //            // 加入到 imageFiles 列表並排序
        //            imageFiles.Add(imgPath);
        //            imageFiles = imageFiles.OrderBy(f => f).ToList();

        //            // 設定當前索引
        //            currentIndex = imageFiles.IndexOf(imgPath);

        //            // 釋放 initialImage 並清除標記
        //            if (initialImage != null)
        //            {
        //                initialImage.Dispose();
        //                initialImage = null;
        //            }
        //            isInitial = false;

        //            //MessageBox.Show("圖片已儲存！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        }
        //        else
        //        {
        //            if (imageFiles.Count == 0) return;

        //            var imgPath = imageFiles[currentIndex];
        //            var labelPath = Path.ChangeExtension(imgPath, ".txt");

        //            // 儲存標註
        //            File.WriteAllLines(labelPath, boxes.Select(b => $"{b.ClassId} {b.X:F6} {b.Y:F6} {b.W:F6} {b.H:F6}"));
        //            OrganizeDatasetAndLaunchTraining();
        //            this.Close();
        //            //MessageBox.Show("標註已儲存！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        }
        //        RefreshLvInfo();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(
        //            string.Format(LanguageResource.Get("SaveError"), ex.Message),
        //            LanguageResource.Get("Error"),
        //            MessageBoxButtons.OK,
        //            MessageBoxIcon.Error);
        //    }
        //}
        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (_isSaving) return;      // 防止連點
            _isSaving = true;

            Stopwatch swTotal = Stopwatch.StartNew();
            Stopwatch sw = new Stopwatch();
            bool closeAfterSave = false;

            try
            {
                // 儲存期間鎖住整個表單，避免背景執行時使用者又修改類別 / 標註
                this.Enabled = false;
                this.UseWaitCursor = true;

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
                    sw.Restart();
                    pictureBox1.Image.Save(imgPath);
                    Debug.WriteLine($"[SaveTiming] SaveImage: {sw.ElapsedMilliseconds} ms");

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

                    RefreshLvInfo();
                }
                else
                {
                    if (imageFiles.Count == 0) return;

                    // 1) 儲存目前這張圖的標註（很小，留在 UI thread）
                    sw.Restart();
                    string imgPath = imageFiles[currentIndex];
                    string labelPath = Path.ChangeExtension(imgPath, ".txt");
                    File.WriteAllLines(labelPath, boxes.Select(b => $"{b.ClassId} {b.X:F6} {b.Y:F6} {b.W:F6} {b.H:F6}"));
                    Debug.WriteLine($"[SaveTiming] WriteLabel: {sw.ElapsedMilliseconds} ms (UI thread)");

                    // 2) 在 UI thread 先取得 UI 資料快照
                    OrganizeContext ctx = BuildOrganizeContext();

                    // 3) 耗時的整理資料集 / 複製檔案 / 寫設定 → 丟到背景執行緒
                    sw.Restart();
                    string message = await Task.Run(() => OrganizeDatasetCore(ctx));
                    Debug.WriteLine($"[SaveTiming] OrganizeDataset (background): {sw.ElapsedMilliseconds} ms");

                    if (!string.IsNullOrEmpty(message))
                    {
                        MessageBox.Show(
                            message,
                            LanguageResource.Get("Info"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }

                    closeAfterSave = true;   // 已要關閉，不再呼叫 RefreshLvInfo()
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format(LanguageResource.Get("SaveError"), ex.Message),
                    LanguageResource.Get("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                Debug.WriteLine($"[SaveTiming] Total click->done: {swTotal.ElapsedMilliseconds} ms");
                _isSaving = false;

                if (!this.IsDisposed)
                {
                    this.UseWaitCursor = false;
                    this.Enabled = true;
                }
            }

            if (closeAfterSave)
            {
                this.Close();
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

        //private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Right)
        //    {
        //        if (boxes.Count > 0)
        //        {
        //            YoloBox removedBox = boxes[boxes.Count - 1];
        //            boxes.RemoveAt(boxes.Count - 1);
        //            pictureBox1.Invalidate();
        //            SaveCurrentAnnotations();
        //        }
        //        else
        //        {
        //            MessageBox.Show(
        //               LanguageResource.Get("NoBoxToDelete"),
        //               LanguageResource.Get("Info"),
        //               MessageBoxButtons.OK,
        //               MessageBoxIcon.Information);
        //        }
        //        return;
        //    }
        //    if (e.Button == MouseButtons.Left)
        //    {

        //        isDrawing = false;
        //        endX = e.X; endY = e.Y;
        //        int minX = Math.Min(startX, endX), minY = Math.Min(startY, endY);
        //        int maxX = Math.Max(startX, endX), maxY = Math.Max(startY, endY);

        //        // 轉 YOLO 格式（歸一化中心、寬高）
        //        float xCenter = (minX + maxX) / 2f / skPattern.Width;
        //        float yCenter = (minY + maxY) / 2f / skPattern.Height;
        //        float w = (maxX - minX) / (float)skPattern.Width;
        //        float h = (maxY - minY) / (float)skPattern.Height;

        //        int classId = comboBoxClass.SelectedIndex;
        //        string className = comboBoxClass.SelectedIndex >= 0
        //    ? comboBoxClass.SelectedItem.ToString()
        //    : "";
        //        boxes.Add(new YoloBox { ClassId = classId, X = xCenter, Y = yCenter, W = w, H = h, ClassName = className });

        //        pictureBox1.Invalidate();
        //    }
        //}
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (boxes.Count > 0)
                {
                    boxes.RemoveAt(boxes.Count - 1);
                    pictureBox1.Invalidate();
                    SaveCurrentAnnotations();
                    RefreshLvInfo();
                }
                else
                {
                    MessageBox.Show(
                       LanguageResource.Get("NoBoxToDelete"),
                       LanguageResource.Get("Info"),
                       MessageBoxButtons.OK,
                       MessageBoxIcon.Information);
                }
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                isDrawing = false;
                endX = e.X;
                endY = e.Y;

                if (pictureBox1.Image == null)
                {
                    pictureBox1.Invalidate();
                    return;
                }

                // 1) 先嘗試在框選範圍內自動偵測輪廓
                Rectangle finalImgRect;
                if (!TryBuildDetectedLabelRect(new System.Drawing.Point(startX, startY), new System.Drawing.Point(endX, endY), out finalImgRect))
                {
                    // 2) 偵測失敗 → 使用拖曳框（依 LabelType 與 Padding）
                    Rectangle dragRect = BuildDrawRectangle(startX, startY, endX, endY);
                    if (dragRect.Width <= 0 || dragRect.Height <= 0)
                    {
                        pictureBox1.Invalidate();
                        return;
                    }

                    finalImgRect = PictureBoxRectToImageRect(dragRect);
                    if (finalImgRect.Width <= 0 || finalImgRect.Height <= 0)
                    {
                        pictureBox1.Invalidate();
                        return;
                    }

                    finalImgRect = ApplyPaddingToRect(finalImgRect, GetPaddingPixels());
                    Rectangle imageBounds = new Rectangle(0, 0, pictureBox1.Image.Width, pictureBox1.Image.Height);
                    finalImgRect = Rectangle.Intersect(finalImgRect, imageBounds);

                    if (finalImgRect.Width <= 0 || finalImgRect.Height <= 0)
                    {
                        pictureBox1.Invalidate();
                        return;
                    }
                }

                // 以「影像」像素為基準轉成 YOLO 正規化座標
                float xCenter = (finalImgRect.X + finalImgRect.Width / 2f) / pictureBox1.Image.Width;
                float yCenter = (finalImgRect.Y + finalImgRect.Height / 2f) / pictureBox1.Image.Height;
                float w = finalImgRect.Width / (float)pictureBox1.Image.Width;
                float h = finalImgRect.Height / (float)pictureBox1.Image.Height;

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

        //private void pictureBox1_Paint(object sender, PaintEventArgs e)
        //{
        //    foreach (var box in boxes)
        //    {
        //        DrawYoloBoxWithLabel(e.Graphics, box, Color.Red);
        //    }
        //    if (isDrawing)
        //    {
        //        Rectangle rect = new Rectangle(
        //            Math.Min(startX, endX), Math.Min(startY, endY),
        //            Math.Abs(endX - startX), Math.Abs(endY - startY));
        //        e.Graphics.DrawRectangle(Pens.Lime, rect);
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

            // 拖曳中的預覽框（套用 LabelType 與 Padding）
            if (isDrawing && pictureBox1.Image != null)
            {
                Rectangle dragRect = BuildDrawRectangle(startX, startY, endX, endY);
                if (dragRect.Width > 0 && dragRect.Height > 0)
                {
                    Rectangle previewImgRect = PictureBoxRectToImageRect(dragRect);
                    if (previewImgRect.Width > 0 && previewImgRect.Height > 0)
                    {
                        Rectangle imageBounds = new Rectangle(0, 0, pictureBox1.Image.Width, pictureBox1.Image.Height);
                        previewImgRect = ApplyPaddingToRect(previewImgRect, GetPaddingPixels());
                        previewImgRect = Rectangle.Intersect(previewImgRect, imageBounds);

                        if (previewImgRect.Width > 0 && previewImgRect.Height > 0)
                        {
                            Rectangle displayRect = ImageRectToPictureBoxRect(previewImgRect);
                            e.Graphics.DrawRectangle(Pens.Lime, displayRect);
                        }
                    }
                }
            }
        }

        //private void DrawYoloBoxWithLabel(Graphics graphics, YoloBox box, Color color)
        //{
        //    int w = pictureBox1.Width, h = pictureBox1.Height;
        //    int boxW = (int)(box.W * w), boxH = (int)(box.H * h);
        //    int boxX = (int)(box.X * w - boxW / 2), boxY = (int)(box.Y * h - boxH / 2);

        //    // 繪製邊框
        //    using (Pen pen = new Pen(color, 2))
        //    {
        //        graphics.DrawRectangle(pen, boxX, boxY, boxW, boxH);
        //    }

        //    // 繪製類別標籤
        //    if (!string.IsNullOrEmpty(box.ClassName))
        //    {
        //        // 準備標籤文字
        //        string labelText = box.ClassName;

        //        // 設定字體和測量文字大小
        //        using (Font font = new Font("Arial", 10, FontStyle.Bold))
        //        {
        //            SizeF textSize = graphics.MeasureString(labelText, font);

        //            // 計算標籤背景矩形
        //            int padding = 4;
        //            int labelWidth = (int)textSize.Width + padding * 2;
        //            int labelHeight = (int)textSize.Height + padding;

        //            // 標籤位置（在框的上方）
        //            int labelX = boxX;
        //            int labelY = boxY - labelHeight;

        //            // 如果標籤超出上邊界，放在框內
        //            if (labelY < 0)
        //            {
        //                labelY = boxY;
        //            }

        //            // 確保標籤不超出右邊界
        //            if (labelX + labelWidth > w)
        //            {
        //                labelX = w - labelWidth;
        //            }

        //            // 繪製標籤背景
        //            Rectangle labelRect = new Rectangle(labelX, labelY, labelWidth, labelHeight);
        //            using (SolidBrush bgBrush = new SolidBrush(Color.FromArgb(200, color)))
        //            {
        //                graphics.FillRectangle(bgBrush, labelRect);
        //            }

        //            // 繪製標籤邊框
        //            using (Pen labelPen = new Pen(color, 1))
        //            {
        //                graphics.DrawRectangle(labelPen, labelRect);
        //            }

        //            // 繪製文字
        //            using (SolidBrush textBrush = new SolidBrush(Color.White))
        //            {
        //                graphics.DrawString(
        //                    labelText,
        //                    font,
        //                    textBrush,
        //                    labelX + padding,
        //                    labelY + padding / 2);
        //            }
        //        }
        //    }
        //}
        private void DrawYoloBoxWithLabel(Graphics graphics, YoloBox box, Color color)
        {
            // 以「實際影像顯示區域」為基準繪製（Zoom 模式有留白）
            Rectangle imgRect = GetImageRectInPictureBox();
            int w = imgRect.Width, h = imgRect.Height;

            int boxW = (int)Math.Round(box.W * w);
            int boxH = (int)Math.Round(box.H * h);
            int boxX = imgRect.X + (int)Math.Round(box.X * w - boxW / 2f);
            int boxY = imgRect.Y + (int)Math.Round(box.Y * h - boxH / 2f);

            using (Pen pen = new Pen(color, 2))
            {
                graphics.DrawRectangle(pen, boxX, boxY, boxW, boxH);
            }

            if (!string.IsNullOrEmpty(box.ClassName))
            {
                string labelText = box.ClassName;
                using (Font font = new Font("Arial", 10, FontStyle.Bold))
                {
                    SizeF textSize = graphics.MeasureString(labelText, font);
                    int lp = 4;
                    int labelWidth = (int)textSize.Width + lp * 2;
                    int labelHeight = (int)textSize.Height + lp;

                    int labelX = boxX;
                    int labelY = boxY - labelHeight;
                    if (labelY < 0) labelY = boxY;
                    if (labelX + labelWidth > pictureBox1.Width) labelX = pictureBox1.Width - labelWidth;

                    Rectangle labelRect = new Rectangle(labelX, labelY, labelWidth, labelHeight);
                    using (SolidBrush bgBrush = new SolidBrush(Color.FromArgb(200, color)))
                        graphics.FillRectangle(bgBrush, labelRect);
                    using (Pen labelPen = new Pen(color, 1))
                        graphics.DrawRectangle(labelPen, labelRect);
                    using (SolidBrush textBrush = new SolidBrush(Color.White))
                        graphics.DrawString(labelText, font, textBrush, labelX + lp, labelY + lp / 2);
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
                DialogResult result = MessageBox.Show(
                    string.Format(LanguageResource.Get("ConfirmClearAnnotations"), boxes.Count),
                    LanguageResource.Get("ConfirmClear"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // 清除所有標註框
                    boxes.Clear();

                    // 刷新顯示
                    pictureBox1.Invalidate();

                    //if (File.Exists(labelPath))
                    //{
                    //    File.Delete(labelPath);
                    //}
                    SaveCurrentAnnotations();
                    RefreshLvInfo();
                }
            }
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            string newClass = tbNewLabel.Text.Trim();
            if (string.IsNullOrEmpty(newClass))
            {
                MessageBox.Show(
                    LanguageResource.Get("EnterClassName"),
                    LanguageResource.Get("Info"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }
            if (labelClassList.ContainsKey(newClass))
            {
                MessageBox.Show(
                    string.Format(LanguageResource.Get("ClassExists"), newClass),
                    LanguageResource.Get("Info"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
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

            string srcImgDir = imageDir;
            string trainBasePath = GV.AIConfig.trainBasePath;
            string ModelsPath = GV.AIConfig.ModelPath;

            string ModelsBackupPath = Path.Combine("C:\\AIData", "Backup");

            string trainModelDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
            string trainModelConfig = Path.Combine(trainModelDir, "AIConfig.json");
            string labelImagePath = Path.Combine(trainBasePath, $"{currentRecipeNumber}");
            string projectDir = Path.Combine(trainBasePath, $"{currentRecipeNumber}", "dataset");


            string[] allImages = Directory.GetFiles(srcImgDir, "*.jpg")
                .Concat(Directory.GetFiles(srcImgDir, "*.png"))
                .Concat(Directory.GetFiles(srcImgDir, "*.bmp"))
                .ToArray();
            if (allImages.Length == 0)
            {
                MessageBox.Show(
                LanguageResource.Get("NoImageFound"),
                LanguageResource.Get("Info"),
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
                return;
            }
            int totalImages = allImages.Length;
            int testCount = totalImages / 7;
            int valCount = totalImages / 7;
            int trainCount = totalImages - testCount - valCount;

            var rand = new Random();
            var shuffled = allImages.OrderBy(x => rand.Next()).ToList();

            //var trainImgs = shuffled.Take(allImages.Length - valCount).ToList();
            //var valImgs = shuffled.Skip(allImages.Length-valCount).ToList();
            var trainImgs = shuffled.Take(trainCount).ToList();
            var valImgs = shuffled.Skip(trainCount).Take(valCount).ToList();
            var testImgs = shuffled.Skip(trainCount + valCount).ToList();
            string[] folders =
            {
                "images/train","images/val","images/test",
                "labels/train","labels/val","labels/test"
            };
            foreach (var folder in folders)
                Directory.CreateDirectory(Path.Combine(projectDir, folder));

            MoveImgAndLabel(trainImgs, "images/train", "labels/train", projectDir);
            MoveImgAndLabel(valImgs, "images/val", "labels/val", projectDir);
            MoveImgAndLabel(testImgs, "images/test", "labels/test", projectDir);

            int nc = comboBoxClass.Items.Count;
            var names = string.Join(", ", comboBoxClass.Items.Cast<string>().Select(n => $"'{n}'"));
            string yaml =
        $@"train: ./images/train
val: ./images/val
test: ./images/test
nc: {nc}
names: [{names}]
";
            File.WriteAllText(Path.Combine(projectDir, "data.yaml"), yaml);
            try
            {
                string configPath = Path.Combine(serviceExePath, "AIConfig.json");

                // 讀取現有的 AIConfig.json（如果存在）
                Dictionary<string, object> configDict = new Dictionary<string, object>();

                if (File.Exists(configPath))
                {
                    try
                    {
                        string existingJson = File.ReadAllText(configPath);
                        var existingConfig = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(existingJson);

                        if (existingConfig != null)
                        {
                            // 保留現有所有參數
                            configDict = existingConfig;
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"讀取現有 AIConfig.json 失敗: {ex.Message}，將建立新檔案");
                    }
                }

                // 更新或新增 labeledRecipeNumber 參數
                configDict["labeledRecipeNumber"] = currentRecipeNumber;
                configDict["trainPath"] = projectDir;
                configDict["modelPath"] = ModelsPath;
                configDict["modelBackupPath"] = ModelsBackupPath;
                configDict["labelImage"] = labelImagePath;
                configDict["trainBasePath"] = trainBasePath;
                configDict["serviceExePath"] = serviceExePath;
                // 儲存到 AIConfig.json
                string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(configDict, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(configPath, jsonContent);

                if (!Directory.Exists(trainModelDir))
                {
                    Directory.CreateDirectory(trainModelDir);
                }
                File.WriteAllText(trainModelConfig, jsonContent);

                System.Diagnostics.Debug.WriteLine($" AIConfig.json 已更新: labeledRecipeNumber = {currentRecipeNumber}");
                System.Diagnostics.Debug.WriteLine($"   完整路徑: {configPath}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ 儲存 labeledRecipeNumber 到 AIConfig.json 失敗: {ex.Message}");
                MessageBox.Show(
                    $"儲存配置到 AIConfig.json 時發生錯誤:\n{ex.Message}\n\n資料集整理已完成，但配置未更新。",
                    "警告",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }

            try
            {
                string labelImageExePath = @"C:\Project\TrainModel\bin\x64\Debug\TrainModel.exe";
                string labelImageDir = Path.GetDirectoryName(labelImageExePath);

                if (!File.Exists(labelImageExePath))
                {
                    System.Diagnostics.Debug.WriteLine($"⚠️ 找不到 LabelImage.exe: {labelImageExePath}");
                    MessageBox.Show(
                        $"找不到 LabelImage.exe！\n\n預期路徑:\n{labelImageExePath}\n\n請確認檔案是否存在。\n資料集整理已完成。",
                        "提示",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
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
                    System.Diagnostics.Debug.WriteLine($" 已啟動 LabelImage.exe");
                    System.Diagnostics.Debug.WriteLine($"   執行檔路徑: {labelImageExePath}");
                    System.Diagnostics.Debug.WriteLine($"   工作目錄: {labelImageDir}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ 啟動 LabelImage.exe 失敗: {ex.Message}");
                MessageBox.Show(
                    $"啟動 LabelImage.exe 時發生錯誤:\n{ex.Message}\n\n資料集整理已完成。",
                    "錯誤",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            //MessageBox.Show("資料集已整理、data.yaml已建立！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show(
                    LanguageResource.Get("NoClassToDelete"),
                    LanguageResource.Get("Info"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            // 建立類別選擇對話框
            Form deleteForm = new Form
            {
                Text = $"{LanguageResource.Get("DeleteClass")} - {LanguageResource.Get("Recipe")}{currentRecipeNumber}",
                Size = new System.Drawing.Size(400, 300),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label lblInfo = new Label
            {
                Text = $"{LanguageResource.Get("SelectClassToDelete")} ({LanguageResource.Get("Recipe")}{currentRecipeNumber})",
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

            Button btnConfirm = new Button
            {
                Text = LanguageResource.Get("ConfirmDelete"),
                Location = new System.Drawing.Point(200, 210),
                Size = new System.Drawing.Size(80, 30),
                DialogResult = DialogResult.OK
            };

            Button btnCancel = new Button
            {
                Text = LanguageResource.Get("Cancel"),
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
                    MessageBox.Show(
                        LanguageResource.Get("NoClassSelected"),
                        LanguageResource.Get("Info"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }

                // 確認刪除
                DialogResult result = MessageBox.Show(
                    string.Format(LanguageResource.Get("ConfirmDeleteClasses"), classesToDelete.Count, string.Join("\n", classesToDelete)),
                    LanguageResource.Get("ConfirmDelete"),
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

                        MessageBox.Show(
                            string.Format(LanguageResource.Get("DeleteSuccess"), classesToDelete.Count),
                            LanguageResource.Get("Complete"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            string.Format(LanguageResource.Get("SaveError"), ex.Message),
                            LanguageResource.Get("Error"),
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
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


                DialogResult result = MessageBox.Show(
                   LanguageResource.Get("ConfirmDeleteImage"),
                   LanguageResource.Get("ConfirmDelete"),
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

                    MessageBox.Show(
                         LanguageResource.Get("ImageDeleted"),
                         LanguageResource.Get("DeleteComplete"),
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Information);
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
                MessageBox.Show(
                    string.Format(LanguageResource.Get("NoPermissionDelete"), ex.Message),
                    LanguageResource.Get("PermissionError"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (IOException ex)
            {
                MessageBox.Show(
                    string.Format(LanguageResource.Get("IOErrorDelete"), ex.Message),
                    LanguageResource.Get("DeleteFailed"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format(LanguageResource.Get("SaveError"), ex.Message),
                    LanguageResource.Get("Error"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
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
        //        private void OrganizeDatasetAndLaunchTraining()
        //        {
        //            string srcImgDir = imageDir;
        //            string trainBasePath = GV.AIConfig.trainBasePath;
        //            string ModelsPath = GV.AIConfig.ModelPath;

        //            string ModelsBackupPath = Path.Combine("C:\\AIData", "Backup");

        //            string trainModelDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
        //            string trainModelConfig = Path.Combine(trainModelDir, "AIConfig.json");
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
        //            UpdateTrainModelConfig();
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
        //                configDict["labelImage"] = labelImagePath;
        //                configDict["trainBasePath"] = trainBasePath;
        //                configDict["serviceExePath"] = serviceExePath;
        //                configDict["trainModel"] = GV.AIConfig.trainModel;  // 新增 trainModel
        //                configDict["lastClassCount"] = GV.AIConfig.lastClassCount;
        //                configDict["lastClassNames"] = GV.AIConfig.lastClassNames;
        //                // 儲存到 AIConfig.json
        //                string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(configDict, Newtonsoft.Json.Formatting.Indented);
        //                File.WriteAllText(configPath, jsonContent);

        //                if (!Directory.Exists(trainModelDir))
        //                {
        //                    Directory.CreateDirectory(trainModelDir);
        //                }
        //                File.WriteAllText(trainModelConfig, jsonContent);

        //                System.Diagnostics.Debug.WriteLine($" AIConfig.json 已更新: labeledRecipeNumber = {currentRecipeNumber}");
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

        //            //try
        //            //{
        //            //    string labelImageExePath = @"C:\Project\TrainModel\bin\x64\Debug\TrainModel.exe";
        //            //    string labelImageDir = Path.GetDirectoryName(labelImageExePath);

        //            //    if (!File.Exists(labelImageExePath))
        //            //    {
        //            //        System.Diagnostics.Debug.WriteLine($"⚠️ 找不到 LabelImage.exe: {labelImageExePath}");
        //            //        MessageBox.Show(
        //            //            $"找不到 LabelImage.exe！\n\n預期路徑:\n{labelImageExePath}\n\n請確認檔案是否存在。\n資料集整理已完成。",
        //            //            "提示",
        //            //            MessageBoxButtons.OK,
        //            //            MessageBoxIcon.Information);
        //            //    }
        //            //    else
        //            //    {
        //            //        ProcessStartInfo psi = new ProcessStartInfo
        //            //        {
        //            //            FileName = labelImageExePath,
        //            //            WorkingDirectory = labelImageDir,
        //            //            UseShellExecute = true,
        //            //            WindowStyle = ProcessWindowStyle.Normal
        //            //        };

        //            //        Process.Start(psi);
        //            //        System.Diagnostics.Debug.WriteLine($" 已啟動 LabelImage.exe");
        //            //        System.Diagnostics.Debug.WriteLine($"   執行檔路徑: {labelImageExePath}");
        //            //        System.Diagnostics.Debug.WriteLine($"   工作目錄: {labelImageDir}");
        //            //    }
        //            //}
        //            //catch (Exception ex)
        //            //{
        //            //    System.Diagnostics.Debug.WriteLine($"❌ 啟動 LabelImage.exe 失敗: {ex.Message}");
        //            //    MessageBox.Show(
        //            //        $"啟動 LabelImage.exe 時發生錯誤:\n{ex.Message}\n\n資料集整理已完成。",
        //            //        "錯誤",
        //            //        MessageBoxButtons.OK,
        //            //        MessageBoxIcon.Error);
        //            //}
        //        }
        private void OrganizeDatasetAndLaunchTraining()
        {
            string message = OrganizeDatasetCore(BuildOrganizeContext());
            if (!string.IsNullOrEmpty(message))
            {
                MessageBox.Show(message, LanguageResource.Get("Info"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private OrganizeContext BuildOrganizeContext()
        {
            string trainBasePath = GV.AIConfig.trainBasePath;

            return new OrganizeContext
            {
                SrcImgDir = imageDir,
                TrainBasePath = trainBasePath,
                ModelsPath = GV.AIConfig.ModelPath,
                ModelsBackupPath = Path.Combine("C:\\AIData", "Backup"),
                TrainModelDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory),
                LabelImagePath = Path.Combine(trainBasePath, currentRecipeNumber.ToString()),
                ProjectDir = Path.Combine(trainBasePath, currentRecipeNumber.ToString(), "dataset"),
                ServiceExePath = serviceExePath,
                RecipeNumber = currentRecipeNumber,
                ClassNames = comboBoxClass.Items.Cast<string>().ToList()
            };
        }

        // 可在背景執行緒執行；不可碰 UI 控制項。回傳 null 表示成功，否則回傳要顯示的警告訊息。
        private string OrganizeDatasetCore(OrganizeContext ctx)
        {
            Stopwatch sw = Stopwatch.StartNew();
            string warning = null;

            string[] allImages = Directory.GetFiles(ctx.SrcImgDir, "*.jpg")
                .Concat(Directory.GetFiles(ctx.SrcImgDir, "*.png"))
                .Concat(Directory.GetFiles(ctx.SrcImgDir, "*.bmp"))
                .ToArray();

            if (allImages.Length == 0)
            {
                return LanguageResource.Get("NoImageFound");
            }
            Debug.WriteLine($"[SaveTiming]   EnumerateImages: {sw.ElapsedMilliseconds} ms ({allImages.Length} files)");

            // 固定分組：依檔名雜湊排序（同一張圖每次都落在同一個 split，不會再洩漏到 train+val）
            sw.Restart();
            int totalImages = allImages.Length;
            int testCount = totalImages / 7;
            int valCount = totalImages / 7;
            int trainCount = totalImages - testCount - valCount;

            List<string> ordered = allImages
                .OrderBy(f => Fnv1a(Path.GetFileName(f)))
                .ToList();

            List<string> trainImgs = ordered.Take(trainCount).ToList();
            List<string> valImgs = ordered.Skip(trainCount).Take(valCount).ToList();
            List<string> testImgs = ordered.Skip(trainCount + valCount).ToList();

            // 增量同步：只複製有變更的檔案，並清掉不屬於該 split 的舊檔
            int copied = 0;
            copied += SyncSplit(trainImgs, "train", ctx.ProjectDir);
            copied += SyncSplit(valImgs, "val", ctx.ProjectDir);
            copied += SyncSplit(testImgs, "test", ctx.ProjectDir);
            Debug.WriteLine($"[SaveTiming]   CopyFiles: {sw.ElapsedMilliseconds} ms (copied {copied} files)");

            // data.yaml（內容相同就不重寫）
            sw.Restart();
            int nc = ctx.ClassNames.Count;
            string names = string.Join(", ", ctx.ClassNames.Select(n => $"'{n}'"));
            string yaml =
        $@"train: ./images/train
val: ./images/val
test: ./images/test
nc: {nc}
names: [{names}]
";
            string yamlPath = Path.Combine(ctx.ProjectDir, "data.yaml");
            if (!File.Exists(yamlPath) || File.ReadAllText(yamlPath) != yaml)
            {
                File.WriteAllText(yamlPath, yaml);
            }
            Debug.WriteLine($"[SaveTiming]   WriteYaml: {sw.ElapsedMilliseconds} ms");

            // 設定檔
            sw.Restart();
            UpdateTrainModelConfig();
            try
            {
                string configPath = Path.Combine(ctx.ServiceExePath, "AIConfig.json");

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
                        Debug.WriteLine($"讀取現有 AIConfig.json 失敗: {ex.Message}，將建立新檔案");
                    }
                }

                configDict["labeledRecipeNumber"] = ctx.RecipeNumber;
                configDict["trainPath"] = ctx.ProjectDir;
                configDict["modelPath"] = ctx.ModelsPath;
                configDict["modelBackupPath"] = ctx.ModelsBackupPath;
                configDict["labelImage"] = ctx.LabelImagePath;
                configDict["trainBasePath"] = ctx.TrainBasePath;
                configDict["serviceExePath"] = ctx.ServiceExePath;
                configDict["trainModel"] = GV.AIConfig.trainModel;
                configDict["lastClassCount"] = GV.AIConfig.lastClassCount;
                configDict["lastClassNames"] = GV.AIConfig.lastClassNames;

                string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(configDict, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(configPath, jsonContent);

                if (!Directory.Exists(ctx.TrainModelDir))
                {
                    Directory.CreateDirectory(ctx.TrainModelDir);
                }
                File.WriteAllText(Path.Combine(ctx.TrainModelDir, "AIConfig.json"), jsonContent);

                Debug.WriteLine($" AIConfig.json 已更新: labeledRecipeNumber = {ctx.RecipeNumber}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ 儲存 AIConfig.json 失敗: {ex.Message}");
                warning = $"儲存配置到 AIConfig.json 時發生錯誤:\n{ex.Message}\n\n資料集整理已完成，但配置未更新。";
            }
            Debug.WriteLine($"[SaveTiming]   WriteConfig: {sw.ElapsedMilliseconds} ms");

            return warning;
        }

        // 同步單一 split：只複製有變更的檔案，並刪除該 split 內已不屬於它的舊檔
        private int SyncSplit(List<string> imgs, string split, string projectDir)
        {
            string imgDstDir = Path.Combine(projectDir, "images", split);
            string lblDstDir = Path.Combine(projectDir, "labels", split);
            Directory.CreateDirectory(imgDstDir);
            Directory.CreateDirectory(lblDstDir);

            HashSet<string> keepImages = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            HashSet<string> keepLabels = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            int copied = 0;

            foreach (string srcImg in imgs)
            {
                string imgName = Path.GetFileName(srcImg);
                keepImages.Add(imgName);
                if (CopyIfChanged(srcImg, Path.Combine(imgDstDir, imgName)))
                    copied++;

                string srcTxt = Path.ChangeExtension(srcImg, ".txt");
                if (File.Exists(srcTxt))
                {
                    string txtName = Path.GetFileName(srcTxt);
                    keepLabels.Add(txtName);
                    if (CopyIfChanged(srcTxt, Path.Combine(lblDstDir, txtName)))
                        copied++;
                }
            }

            // 清除舊檔（例如上次被分到別的 split、或已被刪除的圖片）
            foreach (string f in Directory.GetFiles(imgDstDir))
            {
                string ext = Path.GetExtension(f).ToLowerInvariant();
                if ((ext == ".jpg" || ext == ".png" || ext == ".bmp") &&
                    !keepImages.Contains(Path.GetFileName(f)))
                {
                    TryDeleteFile(f);
                }
            }
            foreach (string f in Directory.GetFiles(lblDstDir, "*.txt"))
            {
                if (!keepLabels.Contains(Path.GetFileName(f)))
                {
                    TryDeleteFile(f);
                }
            }

            return copied;
        }

        // 檔案大小或修改時間不同才複製；File.Copy 會保留來源的修改時間，所以之後比對會相符
        private bool CopyIfChanged(string src, string dst)
        {
            FileInfo s = new FileInfo(src);
            FileInfo d = new FileInfo(dst);

            if (d.Exists && d.Length == s.Length && d.LastWriteTimeUtc == s.LastWriteTimeUtc)
            {
                return false;
            }

            File.Copy(src, dst, true);
            return true;
        }

        private void TryDeleteFile(string path)
        {
            try { File.Delete(path); }
            catch (Exception ex) { Debug.WriteLine($"刪除舊檔失敗 {path}: {ex.Message}"); }
        }

        // 穩定雜湊（string.GetHashCode 不保證跨版本 / 平台一致）
        private static uint Fnv1a(string text)
        {
            unchecked
            {
                uint hash = 2166136261;
                foreach (char c in text.ToLowerInvariant())
                {
                    hash ^= c;
                    hash *= 16777619;
                }
                return hash;
            }
        }

    }
}
