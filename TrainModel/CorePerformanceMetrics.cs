using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vino
{
    public class CorePerformanceMetrics
    {
        public string SummarySource { get; set; }

        // 精度
        public bool AccuracyAvailable { get; set; }
        public double Precision { get; set; }
        public double Recall { get; set; }
        public double Map50 { get; set; }
        public double Map5095 { get; set; }

        // 穩定性（功能測試）
        public int TotalTestCount { get; set; }
        public int SuccessTestCount { get; set; }
        public int FailTestCount { get; set; }
        public int TimeoutCount { get; set; }
        public int ExceptionCount { get; set; }
        public double StabilitySuccessRate { get; set; }

        // 低信心樣本（與場域驗證表同步）
        public int LowConfidenceCount { get; set; }

        // 反應時間
        public bool LatencyAvailable { get; set; }
        public double PreprocessMs { get; set; }
        public double InferenceMs { get; set; }
        public double PostprocessMs { get; set; }
        public double EndToEndMs { get; set; }
        public double MinInferenceMs { get; set; }
        public double MaxInferenceMs { get; set; }
        public double P95InferenceMs { get; set; }

        // 追溯資訊
        public string ModelPath { get; set; }
        public string TestPath { get; set; }
        public string LatencySource { get; set; }
        public DateTime TestTime { get; set; }
    }
}
