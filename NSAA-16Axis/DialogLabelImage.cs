using Basler.Pylon;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
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
        }
        public DialogLabelImage(Bitmap initialImage, int nowMagnification)
        {
            InitializeComponent();
            this.initialImage = initialImage;
            isInitial = true;
            nowMagni = nowMagnification;
            currentRecipeNumber = GV.NowRecipeNumber;
        }

        public DialogLabelImage(int recipeNumber)
        {
            InitializeComponent();
            currentRecipeNumber = recipeNumber;
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
                if(!Directory.Exists(imageDir))
                {
                    Directory.CreateDirectory(imageDir);
                }
                imageFiles = Directory.GetFiles(imageDir, "*.*", SearchOption.AllDirectories)
                    .Where(f =>
                    {
                        string ext = Path.GetExtension(f).ToLower();
                        return ext == ".jpg" || ext == ".png" || ext == ".bmp";
                    })
                    .OrderBy(f => f) // 按路徑排序
                    .ToList();
                currentIndex = 0;

                labelClassList = AIClassList.LoadLabelClassNameFromJson();
                if (labelClassList.Count == 0)
                {
                    Dictionary<string, Int16> inferenceClassList = GV.AIClassList.GetClassList();
                    foreach(var className in inferenceClassList)
                    {
                        labelClassList.Add(className.Key, className.Value);
                    }
                    AIClassList.SaveLabelClassNameToJson(labelClassList);
                }
                // 載入類別列表
                comboBoxClass.Items.Clear();
                foreach (var className in labelClassList.OrderBy(kvp=>kvp.Value))
                {
                    comboBoxClass.Items.Add(className.Key);

                }
                if(comboBoxClass.Items.Count > 0)
                {
                    comboBoxClass.SelectedIndex = 0;
                }
                   
                // 設定語言
                if (GV.AppSettingParm.Language != "default")
                {
                    this.Text = GV.Dlang.strImageLabel;
                }
                if (isInitial)
                {

                    //string fileName = $"labeled_{DateTime.Now:yyyyMMdd_HHmmss}_X{nowMagni}.bmp";
                    //string imgPath = Path.Combine(imageDir, fileName);

                    //// 儲存圖片
                    //initialImage.Save(imgPath);
                    //imageFiles.Add(imgPath);
                    //imageFiles = imageFiles.OrderBy(f => f).ToList();
                    //currentIndex = imageFiles.IndexOf(imgPath);

                    //// 顯示圖片
                    //pictureBox1.Image = new Bitmap(imgPath);
                    //boxes.Clear();
                    //pictureBox1.Invalidate();

                    //// 釋放 initialImage
                    //if (initialImage != null)
                    //{
                    //    initialImage.Dispose();
                    //    initialImage = null;
                    //}
                    //isInitial = false;
                    pictureBox1.Image = initialImage;
                    boxes.Clear();
                    pictureBox1.Invalidate();
                    currentIndex = imageFiles.Count;
                }
                else
                {               
                    LoadCurrentImage();
                }                                 
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(
                    $"沒有權限存取資料夾: {imageDir}\n錯誤: {ex.Message}",
                    "權限錯誤",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"載入圖片時發生錯誤: {ex.Message}",
                    "錯誤",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void LoadCurrentImage()
        {
            if (imageFiles.Count == 0) return;

            if (currentIndex < 0) currentIndex = 0;
            if (currentIndex >= imageFiles.Count) currentIndex = imageFiles.Count - 1;
            var imgPath = imageFiles[currentIndex];

            if(pictureBox1.Image != null && pictureBox1.Image != initialImage)
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
                            H = float.Parse(parts[4])
                        });
                }
            }
             pictureBox1.Invalidate();            
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
            //try
            //{
            //    if (isInitial)
            //    {
            //        // 確保目錄存在
            //        if (!Directory.Exists(imageDir))
            //        {
            //            Directory.CreateDirectory(imageDir);
            //        }

            //        string fileName = $"labeled_{DateTime.Now:yyyyMMdd_HHmmss}.bmp";
            //        string imgPath = Path.Combine(imageDir, fileName);

            //        // 儲存圖片
            //        pictureBox1.Image.Save(imgPath);

            //        // 儲存標註
            //        string labelPath = Path.ChangeExtension(imgPath, ".txt");
            //        File.WriteAllLines(labelPath, boxes.Select(b => $"{b.ClassId} {b.X:F6} {b.Y:F6} {b.W:F6} {b.H:F6}"));

            //        // 加入到 imageFiles 列表並排序
            //        imageFiles.Add(imgPath);
            //        imageFiles = imageFiles.OrderBy(f => f).ToList();

            //        // 設定當前索引
            //        currentIndex = imageFiles.IndexOf(imgPath);

            //        // 釋放 initialImage 並清除標記
            //        if (initialImage != null)
            //        {
            //            initialImage.Dispose();
            //            initialImage = null;
            //        }
            //        isInitial = false;

            //    }
            //    else
            //    {
            //        if (imageFiles.Count == 0) return;

            //        var imgPath = imageFiles[currentIndex];
            //        var labelPath = Path.ChangeExtension(imgPath, ".txt");

            //        // 儲存標註
            //        File.WriteAllLines(labelPath, boxes.Select(b => $"{b.ClassId} {b.X:F6} {b.Y:F6} {b.W:F6} {b.H:F6}"));

            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"儲存時發生錯誤: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
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
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"儲存時發生錯誤: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            isDrawing = true;
            startX = e.X;
            startY = e.Y;
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

            pictureBox1.Invalidate();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
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

        private void btnPrev_Click(object sender, EventArgs e)
        {

            //if (imageFiles.Count == 0) return;

            //if(currentIndex > 0)
            //{
            //    currentIndex--;
            //    LoadCurrentImage();
            //}
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

            //if (currentIndex < imageFiles.Count - 1)
            //{
            //    currentIndex++;
            //    LoadCurrentImage();
            //}
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
                $"確定要清除當前圖片的所有標註嗎?\n\n共有 {boxes.Count} 個標註框將被清除。",
                "確認清除",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // 清除所有標註框
                    boxes.Clear();

                    // 刷新顯示
                    pictureBox1.Invalidate();

                    if (File.Exists(labelPath))
                    {
                        File.Delete(labelPath);
                    }
                }
            }
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            string newClass = tbNewLabel.Text.Trim();
            if (string.IsNullOrEmpty(newClass))
            {
                MessageBox.Show("請輸入類別名稱！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (labelClassList.ContainsKey(newClass))
            {
                MessageBox.Show($"類別 '{newClass}' 已存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            labelClassList.Add(newClass, (Int16)labelClassList.Count);
            comboBoxClass.Items.Add(newClass);
            AIClassList.SaveLabelClassNameToJson(labelClassList);
            tbNewLabel.Clear();
            comboBoxClass.SelectedItem = newClass;
        }

        private void btnOrganizeAndYaml_Click(object sender, EventArgs e)
        {
            //string newClass = tbNewLabel.Text.Trim();
            //if(string.IsNullOrEmpty(newClass) && !comboBoxClass.Items.Contains(newClass))
            //{
            //    GV.AIClassList.AddClass(newClass);
            //    comboBoxClass.Items.Add(newClass);
            //}
            string srcImgDir = imageDir;
            string projectDir = Path.Combine(srcImgDir, "..", "dataset");
            string[] allImages = Directory.GetFiles(srcImgDir, "*.jpg")
                .Concat(Directory.GetFiles(srcImgDir, "*.png"))
                .Concat(Directory.GetFiles(srcImgDir, "*.bmp"))
                .ToArray();
            if (allImages.Length == 0)
            {
                MessageBox.Show("找不到任何圖片檔案！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int valCount = allImages.Length / 5;
            var rand = new Random();
            var shuffled = allImages.OrderBy(x => rand.Next()).ToList();
            var trainImgs = shuffled.Take(allImages.Length - valCount).ToList();
            var valImgs = shuffled.Skip(allImages.Length-valCount).ToList();
            string[] folders =
            {
                "images/train","images/val","labels/train","labels/val"
            };
            foreach (var folder in folders)
                Directory.CreateDirectory(Path.Combine(projectDir, folder));
            MoveImgAndLabel(trainImgs, "images/train", "labels/train", projectDir);
            MoveImgAndLabel(valImgs, "images/val", "labels/val", projectDir);
            int nc = comboBoxClass.Items.Count;
            var names = string.Join(", ", comboBoxClass.Items.Cast<string>().Select(n => $"'{n}'"));
            string yaml =
        $@"train: ./images/train
val: ./images/val
nc: {nc}
names: [{names}]
";
            File.WriteAllText(Path.Combine(projectDir, "data.yaml"), yaml);
            MessageBox.Show("資料集已整理、data.yaml已建立！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("目前沒有可刪除的類別！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 建立類別選擇對話框
            Form deleteForm = new Form
            {
                Text = "刪除類別",
                Size = new System.Drawing.Size(400, 300),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label lblInfo = new Label
            {
                Text = "請選擇要刪除的類別：",
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
                Text = "確認刪除",
                Location = new System.Drawing.Point(200, 210),
                Size = new System.Drawing.Size(80, 30),
                DialogResult = DialogResult.OK
            };

            Button btnCancel = new Button
            {
                Text = "取消",
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
                    MessageBox.Show("未選擇任何類別！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 確認刪除
                DialogResult result = MessageBox.Show(
                    $"確定要刪除以下 {classesToDelete.Count} 個類別嗎？\n\n{string.Join("\n", classesToDelete)}\n\n注意：這將會影響已標註的圖片！",
                    "確認刪除",
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
                        for(int i=0; i<classNames.Count; i++)
                        {
                            labelClassList.Add(classNames[i], (Int16)i);
                        }
                        AIClassList.SaveLabelClassNameToJson(labelClassList);
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

                        MessageBox.Show($"成功刪除 {classesToDelete.Count} 個類別！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"刪除類別時發生錯誤: {ex.Message}", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // 輔助方法：更新所有標註檔案
        private void UpdateLabelFilesAfterClassDeletion(Dictionary<string, int> oldClassNameToId, List<string> deletedClasses)
        {
            // 獲取所有 .txt 標註檔案
            string[] labelFiles = Directory.GetFiles(imageDir, "*.txt", SearchOption.AllDirectories);

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

        private void btTrain_Click(object sender, EventArgs e)
        {

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
    }
}
