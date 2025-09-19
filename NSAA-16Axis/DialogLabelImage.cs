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

        public DialogLabelImage()
        {
            InitializeComponent();            
        }

        private void btnOpenFolder_Click(object sender, EventArgs e)
        {

        }        
        private void DialogLabelImage_Load(object sender, EventArgs e)
        {            
            imageDir = @"C:\AIData";
            try
            {                
                imageFiles = Directory.GetFiles(imageDir, "*.*", SearchOption.AllDirectories)
                    .Where(f =>
                    {
                        string ext = Path.GetExtension(f).ToLower();
                        return ext == ".jpg" || ext == ".png" || ext == ".bmp";
                    })
                    .OrderBy(f => f) // 按路徑排序
                    .ToList();

                // 顯示找到的圖片數量
                if (imageFiles.Count == 0)
                {
                    MessageBox.Show(
                        $"在 {imageDir} 及其子資料夾中找不到任何圖片檔案(.jpg, .png, .bmp)",
                        "提示",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                currentIndex = 0;
                // 載入類別列表
                Dictionary<string, Int16> ClassList = GV.AIClassList.GetClassList();
                foreach (var className in ClassList)
                {
                    comboBoxClass.Items.Add(className);
                }
                // 設定語言
                if (GV.AppSettingParm.Language != "default")
                {
                    this.Text = GV.Dlang.strImageLabel;
                }
                // 載入第一張圖片
                LoadCurrentImage();              
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
            var imgPath = imageFiles[currentIndex];

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
            if (imageFiles.Count == 0) return;
            var imgPath = imageFiles[currentIndex];
            var labelPath = Path.ChangeExtension(imgPath, ".txt");
            File.WriteAllLines(labelPath, boxes.Select(b => $"{b.ClassId} {b.X:F6} {b.Y:F6} {b.W:F6} {b.H:F6}"));
            if(GV.AppSettingParm.Language != "default")
            {
                MessageBox.Show(GV.Dlang.strMessage1);
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
            if(currentIndex > 0)
            {
                currentIndex--;
                LoadCurrentImage();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if(currentIndex < imageFiles.Count - 1)
            {
                currentIndex++;
                LoadCurrentImage();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
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
