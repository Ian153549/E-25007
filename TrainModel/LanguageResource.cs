using NSAA_16Axis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainModel
{
    public enum LanguageType
    {
        English,
        TraditionalChinese,
        SimplifiedChinese
    }
    public static class LanguageResource
    {
        private static Dictionary<string, Dictionary<LanguageType, string>> _resources;

        static LanguageResource()
        {
            InitializeResources();
        }

        private static void InitializeResources()
        {
            _resources = new Dictionary<string, Dictionary<LanguageType, string>>
            {
                // MainForm
                ["FormTitle"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "AI Training Tool" },
                    { LanguageType.TraditionalChinese, "AI 訓練工具" },
                    { LanguageType.SimplifiedChinese, "AI 训练工具" }
                },

                // Tab Pages
                ["BasicInfo"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Basic Info" },
                    { LanguageType.TraditionalChinese, "基本資訊" },
                    { LanguageType.SimplifiedChinese, "基本信息" }
                },
                ["TrainInfo"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Train Info" },
                    { LanguageType.TraditionalChinese, "訓練資訊" },
                    { LanguageType.SimplifiedChinese, "训练信息" }
                },
                ["TestResult"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Test Result" },
                    { LanguageType.TraditionalChinese, "測試結果" },
                    { LanguageType.SimplifiedChinese, "测试结果" }
                },

                // GroupBox
                ["Path"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Path:" },
                    { LanguageType.TraditionalChinese, "路徑：" },
                    { LanguageType.SimplifiedChinese, "路径：" }
                },
                ["ModelSetting"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Model Setting" },
                    { LanguageType.TraditionalChinese, "模型設定" },
                    { LanguageType.SimplifiedChinese, "模型设置" }
                },
                ["ImageEnhancement"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Image Enhancement" },
                    { LanguageType.TraditionalChinese, "影像增強" },
                    { LanguageType.SimplifiedChinese, "图像增强" }
                },

                // Labels
                ["Training"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Training" },
                    { LanguageType.TraditionalChinese, "訓練集" },
                    { LanguageType.SimplifiedChinese, "训练集" }
                },
                ["Validate"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Validate" },
                    { LanguageType.TraditionalChinese, "驗證集" },
                    { LanguageType.SimplifiedChinese, "验证集" }
                },
                ["Test"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Test" },
                    { LanguageType.TraditionalChinese, "測試集" },
                    { LanguageType.SimplifiedChinese, "测试集" }
                },
                ["Image"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Image" },
                    { LanguageType.TraditionalChinese, "影像" },
                    { LanguageType.SimplifiedChinese, "图像" }
                },
                ["Width"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Width" },
                    { LanguageType.TraditionalChinese, "寬度" },
                    { LanguageType.SimplifiedChinese, "宽度" }
                },
                ["Height"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Height" },
                    { LanguageType.TraditionalChinese, "高度" },
                    { LanguageType.SimplifiedChinese, "高度" }
                },
                ["BatchSize"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Batch Size" },
                    { LanguageType.TraditionalChinese, "批次大小" },
                    { LanguageType.SimplifiedChinese, "批次大小" }
                },
                ["Epoch"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Epoch" },
                    { LanguageType.TraditionalChinese, "訓練週期" },
                    { LanguageType.SimplifiedChinese, "训练周期" }
                },
                ["LightingAdjust"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Lighting Adjust(%)" },
                    { LanguageType.TraditionalChinese, "亮度調整(%)" },
                    { LanguageType.SimplifiedChinese, "亮度调整(%)" }
                },

                // Buttons
                ["Select"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Select" },
                    { LanguageType.TraditionalChinese, "選擇" },
                    { LanguageType.SimplifiedChinese, "选择" }
                },
                ["ImageLabel"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Image Label" },
                    { LanguageType.TraditionalChinese, "影像標註" },
                    { LanguageType.SimplifiedChinese, "图像标注" }
                },
                ["ModelTraining"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Model Training" },
                    { LanguageType.TraditionalChinese, "模型訓練" },
                    { LanguageType.SimplifiedChinese, "模型训练" }
                },
                ["TrainingTerminate"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Training Terminate" },
                    { LanguageType.TraditionalChinese, "終止訓練" },
                    { LanguageType.SimplifiedChinese, "终止训练" }
                },
                ["ModelTesting"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Model Testing" },
                    { LanguageType.TraditionalChinese, "模型測試" },
                    { LanguageType.SimplifiedChinese, "模型测试" }
                },
                ["TestingResult"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Testing Result" },
                    { LanguageType.TraditionalChinese, "測試結果" },
                    { LanguageType.SimplifiedChinese, "测试结果" }
                },
                ["ModelGenerate"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Model Generate" },
                    { LanguageType.TraditionalChinese, "產生模型" },
                    { LanguageType.SimplifiedChinese, "生成模型" }
                },
                ["Previous"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "< Previous" },
                    { LanguageType.TraditionalChinese, "< 上一張" },
                    { LanguageType.SimplifiedChinese, "< 上一张" }
                },
                ["Next"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Next >" },
                    { LanguageType.TraditionalChinese, "下一張 >" },
                    { LanguageType.SimplifiedChinese, "下一张 >" }
                },
                ["SaveModel"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Save Model" },
                    { LanguageType.TraditionalChinese, "儲存模型" },
                    { LanguageType.SimplifiedChinese, "保存模型" }
                },

                // Messages
                ["ConfirmTraining"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Confirm Training" },
                    { LanguageType.TraditionalChinese, "確認訓練" },
                    { LanguageType.SimplifiedChinese, "确认训练" }
                },
                ["TrainingParams"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Training Parameters:" },
                    { LanguageType.TraditionalChinese, "訓練參數：" },
                    { LanguageType.SimplifiedChinese, "训练参数：" }
                },
                ["ImageSize"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Image Size" },
                    { LanguageType.TraditionalChinese, "圖片尺寸" },
                    { LanguageType.SimplifiedChinese, "图片尺寸" }
                },
                ["StartTraining"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Start Training?" },
                    { LanguageType.TraditionalChinese, "是否開始訓練？" },
                    { LanguageType.SimplifiedChinese, "是否开始训练？" }
                },
                ["TrainingComplete"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Training Complete!" },
                    { LanguageType.TraditionalChinese, "訓練完成！" },
                    { LanguageType.SimplifiedChinese, "训练完成！" }
                },
                ["Complete"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Complete" },
                    { LanguageType.TraditionalChinese, "完成" },
                    { LanguageType.SimplifiedChinese, "完成" }
                },
                ["Error"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Error" },
                    { LanguageType.TraditionalChinese, "錯誤" },
                    { LanguageType.SimplifiedChinese, "错误" }
                },
                ["Warning"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Warning" },
                    { LanguageType.TraditionalChinese, "警告" },
                    { LanguageType.SimplifiedChinese, "警告" }
                },
                ["InputError"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Input Error" },
                    { LanguageType.TraditionalChinese, "輸入錯誤" },
                    { LanguageType.SimplifiedChinese, "输入错误" }
                },
                ["InvalidWidth"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Please enter a valid image width!" },
                    { LanguageType.TraditionalChinese, "請輸入有效的圖片寬度！" },
                    { LanguageType.SimplifiedChinese, "请输入有效的图片宽度！" }
                },
                ["InvalidHeight"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Please enter a valid image height!" },
                    { LanguageType.TraditionalChinese, "請輸入有效的圖片高度！" },
                    { LanguageType.SimplifiedChinese, "请输入有效的图片高度！" }
                },
                ["InvalidBatchSize"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Please enter a valid batch size!" },
                    { LanguageType.TraditionalChinese, "請輸入有效的批次大小！" },
                    { LanguageType.SimplifiedChinese, "请输入有效的批次大小！" }
                },
                ["InvalidEpoch"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Please enter a valid epoch count!" },
                    { LanguageType.TraditionalChinese, "請輸入有效的訓練週期！" },
                    { LanguageType.SimplifiedChinese, "请输入有效的训练周期！" }
                },
                ["InvalidTestPath"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Please enter a valid test folder path!" },
                    { LanguageType.TraditionalChinese, "請輸入有效的測試資料夾路徑！" },
                    { LanguageType.SimplifiedChinese, "请输入有效的测试文件夹路径！" }
                },
                ["ModelNotFound"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Model file not found. Please train the model first." },
                    { LanguageType.TraditionalChinese, "找不到模型檔案，請先進行模型訓練。" },
                    { LanguageType.SimplifiedChinese, "找不到模型文件，请先进行模型训练。" }
                },
                ["NoTestImages"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "No image files found in the test folder (.jpg, .png, .bmp)!" },
                    { LanguageType.TraditionalChinese, "測試資料夾中找不到任何圖片檔案（.jpg, .png, .bmp）！" },
                    { LanguageType.SimplifiedChinese, "测试文件夹中找不到任何图片文件（.jpg, .png, .bmp）！" }
                },
                ["ConfirmTest"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Confirm Test" },
                    { LanguageType.TraditionalChinese, "確認測試" },
                    { LanguageType.SimplifiedChinese, "确认测试" }
                },
                ["TestParams"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Test Parameters:" },
                    { LanguageType.TraditionalChinese, "測試參數：" },
                    { LanguageType.SimplifiedChinese, "测试参数：" }
                },
                ["ModelPath"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Model Path" },
                    { LanguageType.TraditionalChinese, "模型路徑" },
                    { LanguageType.SimplifiedChinese, "模型路径" }
                },
                ["TestImageCount"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Test Image Count" },
                    { LanguageType.TraditionalChinese, "測試圖片數量" },
                    { LanguageType.SimplifiedChinese, "测试图片数量" }
                },
                ["StartTest"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Start Test?" },
                    { LanguageType.TraditionalChinese, "是否開始測試？" },
                    { LanguageType.SimplifiedChinese, "是否开始测试？" }
                },
                ["TestComplete"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Test Complete!" },
                    { LanguageType.TraditionalChinese, "測試完成！" },
                    { LanguageType.SimplifiedChinese, "测试完成！" }
                },
                ["SaveModelConfirm"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Do you want to keep the current trained model?" },
                    { LanguageType.TraditionalChinese, "是否要保留目前訓練的模型？" },
                    { LanguageType.SimplifiedChinese, "是否要保留当前训练的模型？" }
                },
                ["KeepModel"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "[Yes] - Keep current model" },
                    { LanguageType.TraditionalChinese, "[是] - 保留目前的模型" },
                    { LanguageType.SimplifiedChinese, "[是] - 保留当前的模型" }
                },
                ["RestoreModel"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "[No] - Restore previous model" },
                    { LanguageType.TraditionalChinese, "[否] - 還原為先前的模型" },
                    { LanguageType.SimplifiedChinese, "[否] - 还原为之前的模型" }
                },
                ["ModelKept"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Current model has been kept." },
                    { LanguageType.TraditionalChinese, "已保留目前的模型。" },
                    { LanguageType.SimplifiedChinese, "已保留当前的模型。" }
                },
                ["ModelRestored"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Previous model has been restored." },
                    { LanguageType.TraditionalChinese, "已還原為先前的模型。" },
                    { LanguageType.SimplifiedChinese, "已还原为之前的模型。" }
                },
                ["BackupNotFound"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Backup model not found, cannot restore." },
                    { LanguageType.TraditionalChinese, "找不到備份的模型檔案，無法還原。" },
                    { LanguageType.SimplifiedChinese, "找不到备份的模型文件，无法还原。" }
                },
                ["TerminateTraining"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Terminate Training" },
                    { LanguageType.TraditionalChinese, "終止訓練" },
                    { LanguageType.SimplifiedChinese, "终止训练" }
                },
                ["CurrentlyTraining"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Currently training Epoch" },
                    { LanguageType.TraditionalChinese, "目前正在訓練 Epoch" },
                    { LanguageType.SimplifiedChinese, "当前正在训练 Epoch" }
                },
                ["ChooseTerminateMethod"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Choose termination method:" },
                    { LanguageType.TraditionalChinese, "選擇終止方式：" },
                    { LanguageType.SimplifiedChinese, "选择终止方式：" }
                },
                ["WaitCurrentEpoch"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "[Yes] - Wait for current Epoch to complete" },
                    { LanguageType.TraditionalChinese, "[是] - 等待當前 Epoch 完成後停止" },
                    { LanguageType.SimplifiedChinese, "[是] - 等待当前 Epoch 完成后停止" }
                },
                ["ForceTerminate"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "[No] - Force terminate immediately" },
                    { LanguageType.TraditionalChinese, "[否] - 立即強制終止" },
                    { LanguageType.SimplifiedChinese, "[否] - 立即强制终止" }
                },
                ["ContinueTraining"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "[Cancel] - Continue training" },
                    { LanguageType.TraditionalChinese, "[取消] - 繼續訓練" },
                    { LanguageType.SimplifiedChinese, "[取消] - 继续训练" }
                },
                ["NoTrainingProcess"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "No training process is currently running." },
                    { LanguageType.TraditionalChinese, "目前沒有正在執行的訓練程序。" },
                    { LanguageType.SimplifiedChinese, "当前没有正在执行的训练程序。" }
                },
                ["Info"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Information" },
                    { LanguageType.TraditionalChinese, "提示" },
                    { LanguageType.SimplifiedChinese, "提示" }
                },
                ["TrainingTerminated"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Training terminated at Epoch {0}!\nModel saved." },
                    { LanguageType.TraditionalChinese, "訓練已在 Epoch {0} 後停止！\n模型已儲存。" },
                    { LanguageType.SimplifiedChinese, "训练已在 Epoch {0} 后停止！\n模型已保存。" }
                },

                // Test Result
                ["ResultPath"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Result Path" },
                    { LanguageType.TraditionalChinese, "結果路徑" },
                    { LanguageType.SimplifiedChinese, "结果路径" }
                },
                ["FileName"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "File Name" },
                    { LanguageType.TraditionalChinese, "檔案名稱" },
                    { LanguageType.SimplifiedChinese, "文件名称" }
                },
                ["DetectionCount"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Detections" },
                    { LanguageType.TraditionalChinese, "偵測數" },
                    { LanguageType.SimplifiedChinese, "检测数" }
                },
                ["StatisticsSummary"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Statistics Summary" },
                    { LanguageType.TraditionalChinese, "統計摘要" },
                    { LanguageType.SimplifiedChinese, "统计摘要" }
                },
                ["TotalTestImages"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Total Test Images" },
                    { LanguageType.TraditionalChinese, "測試圖片總數" },
                    { LanguageType.SimplifiedChinese, "测试图片总数" }
                },
                ["ImagesWithDetection"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Images with Detection" },
                    { LanguageType.TraditionalChinese, "有偵測結果的圖片" },
                    { LanguageType.SimplifiedChinese, "有检测结果的图片" }
                },
                ["TotalDetections"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Total Detections" },
                    { LanguageType.TraditionalChinese, "總偵測數量" },
                    { LanguageType.SimplifiedChinese, "总检测数量" }
                },
                ["ClassStatistics"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Class Statistics" },
                    { LanguageType.TraditionalChinese, "各類別統計" },
                    { LanguageType.SimplifiedChinese, "各类别统计" }
                },
                ["NoResultImages"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "No result images" },
                    { LanguageType.TraditionalChinese, "無結果圖片" },
                    { LanguageType.SimplifiedChinese, "无结果图片" }
                },

                // YOLO Messages
                ["StartingYOLO"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Starting YOLO training process..." },
                    { LanguageType.TraditionalChinese, "正在啟動 YOLO 訓練程序..." },
                    { LanguageType.SimplifiedChinese, "正在启动 YOLO 训练程序..." }
                },
                ["YOLOStarted"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "YOLO training process started" },
                    { LanguageType.TraditionalChinese, "YOLO 訓練程序已啟動" },
                    { LanguageType.SimplifiedChinese, "YOLO 训练程序已启动" }
                },
                ["TrainingSuccess"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Training completed successfully" },
                    { LanguageType.TraditionalChinese, "訓練成功完成" },
                    { LanguageType.SimplifiedChinese, "训练成功完成" }
                },
                ["ValidationResult"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Validation Result" },
                    { LanguageType.TraditionalChinese, "驗證結果" },
                    { LanguageType.SimplifiedChinese, "验证结果" }
                },
                ["ExecutingInference"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Executing YOLO inference..." },
                    { LanguageType.TraditionalChinese, "正在執行 YOLO 推論..." },
                    { LanguageType.SimplifiedChinese, "正在执行 YOLO 推理..." }
                },
                ["ModelTest"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Model Test" },
                    { LanguageType.TraditionalChinese, "模型測試" },
                    { LanguageType.SimplifiedChinese, "模型测试" }
                },
                ["TestPath"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Test Path" },
                    { LanguageType.TraditionalChinese, "測試路徑" },
                    { LanguageType.SimplifiedChinese, "测试路径" }
                },

                // Language Menu
                ["Language"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Language" },
                    { LanguageType.TraditionalChinese, "語言" },
                    { LanguageType.SimplifiedChinese, "语言" }
                },
                ["English"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "English" },
                    { LanguageType.TraditionalChinese, "英文" },
                    { LanguageType.SimplifiedChinese, "英文" }
                },
                ["TraditionalChinese"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Traditional Chinese" },
                    { LanguageType.TraditionalChinese, "繁體中文" },
                    { LanguageType.SimplifiedChinese, "繁体中文" }
                },
                ["SimplifiedChinese"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Simplified Chinese" },
                    { LanguageType.TraditionalChinese, "簡體中文" },
                    { LanguageType.SimplifiedChinese, "简体中文" }
                },
                // LabelImage
                ["DeleteClass"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Delete Class" },
                    { LanguageType.TraditionalChinese, "刪除類別" },
                    { LanguageType.SimplifiedChinese, "删除类别" }
                },
                ["SelectClassToDelete"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Select classes to delete:" },
                    { LanguageType.TraditionalChinese, "請選擇要刪除的類別：" },
                    { LanguageType.SimplifiedChinese, "请选择要删除的类别：" }
                },
                ["ConfirmDelete"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Confirm Delete" },
                    { LanguageType.TraditionalChinese, "確認刪除" },
                    { LanguageType.SimplifiedChinese, "确认删除" }
                },
                ["Cancel"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Cancel" },
                    { LanguageType.TraditionalChinese, "取消" },
                    { LanguageType.SimplifiedChinese, "取消" }
                },
                ["NoClassSelected"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "No class selected!" },
                    { LanguageType.TraditionalChinese, "未選擇任何類別！" },
                    { LanguageType.SimplifiedChinese, "未选择任何类别！" }
                },
                ["ConfirmDeleteClasses"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Are you sure you want to delete the following {0} classes?\n\n{1}\n\nNote: This will affect labeled images!" },
                    { LanguageType.TraditionalChinese, "確定要刪除以下 {0} 個類別嗎？\n\n{1}\n\n注意：這將會影響已標註的圖片！" },
                    { LanguageType.SimplifiedChinese, "确定要删除以下 {0} 个类别吗？\n\n{1}\n\n注意：这将会影响已标注的图片！" }
                },
                ["DeleteSuccess"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Successfully deleted {0} classes!" },
                    { LanguageType.TraditionalChinese, "成功刪除 {0} 個類別！" },
                    { LanguageType.SimplifiedChinese, "成功删除 {0} 个类别！" }
                },
                ["NoClassToDelete"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "No classes available to delete!" },
                    { LanguageType.TraditionalChinese, "目前沒有可刪除的類別！" },
                    { LanguageType.SimplifiedChinese, "当前没有可删除的类别！" }
                },
                ["EnterClassName"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Please enter a class name!" },
                    { LanguageType.TraditionalChinese, "請輸入類別名稱！" },
                    { LanguageType.SimplifiedChinese, "请输入类别名称！" }
                },
                ["ClassExists"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Class '{0}' already exists!" },
                    { LanguageType.TraditionalChinese, "類別 '{0}' 已存在！" },
                    { LanguageType.SimplifiedChinese, "类别 '{0}' 已存在！" }
                },
                ["ConfirmClearAnnotations"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Are you sure you want to clear all annotations for the current image?\n\n{0} annotation boxes will be cleared." },
                    { LanguageType.TraditionalChinese, "確定要清除當前圖片的所有標註嗎?\n\n共有 {0} 個標註框將被清除。" },
                    { LanguageType.SimplifiedChinese, "确定要清除当前图片的所有标注吗?\n\n共有 {0} 个标注框将被清除。" }
                },
                ["ConfirmClear"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Confirm Clear" },
                    { LanguageType.TraditionalChinese, "確認清除" },
                    { LanguageType.SimplifiedChinese, "确认清除" }
                },
                ["NoBoxToDelete"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "No annotation box available to delete!" },
                    { LanguageType.TraditionalChinese, "目前沒有可刪除的標記框！" },
                    { LanguageType.SimplifiedChinese, "当前没有可删除的标记框！" }
                },
                ["ConfirmDeleteImage"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Are you sure you want to delete the current image?\n\nNote: This action cannot be undone!" },
                    { LanguageType.TraditionalChinese, "確定要刪除當前圖片嗎？\n\n注意：此操作無法復原！" },
                    { LanguageType.SimplifiedChinese, "确定要删除当前图片吗？\n\n注意：此操作无法撤销！" }
                },
                ["ImageDeleted"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Image deleted!\nNo more images in the folder." },
                    { LanguageType.TraditionalChinese, "圖片已刪除！\n目前資料夾中已無圖片。" },
                    { LanguageType.SimplifiedChinese, "图片已删除！\n当前文件夹中已无图片。" }
                },
                ["DeleteComplete"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Delete Complete" },
                    { LanguageType.TraditionalChinese, "刪除完成" },
                    { LanguageType.SimplifiedChinese, "删除完成" }
                },
                ["PermissionError"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Permission Error" },
                    { LanguageType.TraditionalChinese, "權限錯誤" },
                    { LanguageType.SimplifiedChinese, "权限错误" }
                },
                ["NoPermissionDelete"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "No permission to delete the file!\nError: {0}" },
                    { LanguageType.TraditionalChinese, "沒有權限刪除檔案！\n錯誤: {0}" },
                    { LanguageType.SimplifiedChinese, "没有权限删除文件！\n错误: {0}" }
                },
                ["DeleteFailed"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Delete Failed" },
                    { LanguageType.TraditionalChinese, "刪除失敗" },
                    { LanguageType.SimplifiedChinese, "删除失败" }
                },
                ["IOErrorDelete"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "I/O error occurred while deleting!\nError: {0}\n\nPossible cause: File is in use by another program." },
                    { LanguageType.TraditionalChinese, "刪除檔案時發生 I/O 錯誤！\n錯誤: {0}\n\n可能原因：檔案正在被其他程式使用。" },
                    { LanguageType.SimplifiedChinese, "删除文件时发生 I/O 错误！\n错误: {0}\n\n可能原因：文件正在被其他程序使用。" }
                },
                ["NoImageFound"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "No image files found!" },
                    { LanguageType.TraditionalChinese, "找不到任何圖片檔案！" },
                    { LanguageType.SimplifiedChinese, "找不到任何图片文件！" }
                },
                ["DatasetOrganized"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Dataset organized and data.yaml created!" },
                    { LanguageType.TraditionalChinese, "資料集已整理、data.yaml已建立！" },
                    { LanguageType.SimplifiedChinese, "数据集已整理、data.yaml已创建！" }
                },
                ["ImageSaved"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Image saved!" },
                    { LanguageType.TraditionalChinese, "圖片已儲存！" },
                    { LanguageType.SimplifiedChinese, "图片已保存！" }
                },
                ["AnnotationSaved"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Annotations saved!" },
                    { LanguageType.TraditionalChinese, "標註已儲存！" },
                    { LanguageType.SimplifiedChinese, "标注已保存！" }
                },
                ["SaveError"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Error occurred while saving: {0}" },
                    { LanguageType.TraditionalChinese, "儲存時發生錯誤: {0}" },
                    { LanguageType.SimplifiedChinese, "保存时发生错误: {0}" }
                },
                ["LoadError"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Error occurred while loading images: {0}" },
                    { LanguageType.TraditionalChinese, "載入圖片時發生錯誤: {0}" },
                    { LanguageType.SimplifiedChinese, "加载图片时发生错误: {0}" }
                },
                ["NoPermissionAccess"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "No permission to access folder: {0}\nError: {1}" },
                    { LanguageType.TraditionalChinese, "沒有權限存取資料夾: {0}\n錯誤: {1}" },
                    { LanguageType.SimplifiedChinese, "没有权限访问文件夹: {0}\n错误: {1}" }
                },

                // DialogLabelImage Buttons
                ["Save"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Save" },
                    { LanguageType.TraditionalChinese, "儲存" },
                    { LanguageType.SimplifiedChinese, "保存" }
                },
                ["Clear"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Clear" },
                    { LanguageType.TraditionalChinese, "清除" },
                    { LanguageType.SimplifiedChinese, "清除" }
                },
                ["Add"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Add" },
                    { LanguageType.TraditionalChinese, "新增" },
                    { LanguageType.SimplifiedChinese, "添加" }
                },
                ["Delete"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Delete" },
                    { LanguageType.TraditionalChinese, "刪除" },
                    { LanguageType.SimplifiedChinese, "删除" }
                },
                ["DeleteImage"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Delete Image" },
                    { LanguageType.TraditionalChinese, "刪除圖片" },
                    { LanguageType.SimplifiedChinese, "删除图片" }
                },
                ["OrganizeDataset"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Organize Dataset" },
                    { LanguageType.TraditionalChinese, "整理資料集" },
                    { LanguageType.SimplifiedChinese, "整理数据集" }
                },
                ["Class"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Class" },
                    { LanguageType.TraditionalChinese, "類別" },
                    { LanguageType.SimplifiedChinese, "类别" }
                },
                ["NewClass"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "New Class" },
                    { LanguageType.TraditionalChinese, "新類別" },
                    { LanguageType.SimplifiedChinese, "新类别" }
                },
                ["Recipe"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Recipe" },
                    { LanguageType.TraditionalChinese, "配方" },
                    { LanguageType.SimplifiedChinese, "配方" }
                },
                ["CorePerformanceSummary"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Core Performance Summary" },
                    { LanguageType.TraditionalChinese, "核心性能指標摘要" },
                    { LanguageType.SimplifiedChinese, "核心性能指标摘要" }
                },
                ["AccuracyMetrics"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Accuracy Metrics" },
                    { LanguageType.TraditionalChinese, "精度指標" },
                    { LanguageType.SimplifiedChinese, "精度指标" }
                },
                ["StabilityMetrics"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Stability Metrics" },
                    { LanguageType.TraditionalChinese, "穩定性指標" },
                    { LanguageType.SimplifiedChinese, "稳定性指标" }
                },
                ["LatencyMetrics"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Latency Metrics" },
                    { LanguageType.TraditionalChinese, "反應時間指標" },
                    { LanguageType.SimplifiedChinese, "反应时间指标" }
                },
                ["ContrastAdjustment"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "ContrastAdjustment(±%)" },
                    { LanguageType.TraditionalChinese, "對比度調整(±%)" },
                    { LanguageType.SimplifiedChinese, "对比度调整(±%)" }
                },
                ["Gamma"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Gamma" },
                    { LanguageType.TraditionalChinese, "伽瑪" },
                    { LanguageType.SimplifiedChinese, "伽玛" }
                },
                ["Rotate"] = new Dictionary<LanguageType, string>
                {
                    { LanguageType.English, "Rotate(±deg)" },
                    { LanguageType.TraditionalChinese, "旋轉(±度)" },
                    { LanguageType.SimplifiedChinese, "旋转(±度)" }
                },

            };
        }

        public static string Get(string key)
        {
            return Get(key, GV.CurrentLanguage);
        }

        public static string Get(string key, LanguageType language)
        {
            if (_resources.TryGetValue(key, out var langDict))
            {
                if (langDict.TryGetValue(language, out var value))
                {
                    return value;
                }
                // Fallback to English
                if (langDict.TryGetValue(LanguageType.English, out var fallback))
                {
                    return fallback;
                }
            }
            return key; // Return key if not found
        }

        public static string Format(string key, params object[] args)
        {
            return string.Format(Get(key), args);
        }
    }
}
