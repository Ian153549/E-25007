using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace NSAA_16Axis
{
    public class AIInferBox
    {
        public double x1 { get; set; }
        public double y1 { get; set; }
        public double x2 { get; set; }
        public double y2 { get; set; }
        public double conf { get; set; }
        public int @class { get; set; }
        public string name { get; set; }
    }

    public class AIInferResponse
    {
        public List<AIInferBox> Boxes { get; set; } = new List<AIInferBox>();
        public long? DataLatencyMs { get; set; }
        public double? PreprocessMs { get; set; }
        public double? InferenceMs { get; set; }
        public double? PostprocessMs { get; set; }
        public long? ServerTotalMs { get; set; }
        public long ProducedAtMs { get; set; }
        public long SentAtMs { get; set; }
        public long ReceivedAtMsClient { get; set; }
    }

    public static class AIInferenceClient
    {
        private static readonly HttpClient _httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(30)
        };

        public static async Task<AIInferResponse> InferAsync(Bitmap bitmap)
        {
            if (bitmap == null)
                throw new ArgumentNullException("bitmap");

            long producedAtMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                byte[] imageBytes = ms.ToArray();

                using (var content = new MultipartFormDataContent())
                using (var imageContent = new ByteArrayContent(imageBytes))
                using (var request = new HttpRequestMessage(HttpMethod.Post, "http://127.0.0.1:9300/infer"))
                {
                    imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
                    content.Add(imageContent, "image", "grab.png");
                    content.Add(new StringContent(producedAtMs.ToString()), "produced_at_ms");

                    request.Headers.Add("X-Produced-At-Ms", producedAtMs.ToString());
                    request.Content = content;

                    long sentAtMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                    HttpResponseMessage response = await _httpClient.SendAsync(request).ConfigureAwait(false);
                    long receivedAtMsClient = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                    response.EnsureSuccessStatusCode();

                    string json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var boxes = JsonConvert.DeserializeObject<List<AIInferBox>>(json) ?? new List<AIInferBox>();

                    var result = new AIInferResponse
                    {
                        Boxes = boxes,
                        ProducedAtMs = producedAtMs,
                        SentAtMs = sentAtMs,
                        ReceivedAtMsClient = receivedAtMsClient,
                        DataLatencyMs = GetHeaderLong(response, "X-Data-Latency-Ms"),
                        PreprocessMs = GetHeaderDouble(response, "X-Preprocess-Ms"),
                        InferenceMs = GetHeaderDouble(response, "X-Inference-Ms"),
                        PostprocessMs = GetHeaderDouble(response, "X-Postprocess-Ms"),
                        ServerTotalMs = GetHeaderLong(response, "X-Server-Total-Ms")
                    };

                    string latencyText = result.DataLatencyMs.HasValue ? result.DataLatencyMs.Value + " ms" : "未提供";
                    string preprocessText = result.PreprocessMs.HasValue ? result.PreprocessMs.Value.ToString("0.0") + " ms" : "未知";
                    string inferenceText = result.InferenceMs.HasValue ? result.InferenceMs.Value.ToString("0.0") + " ms" : "未知";
                    string postprocessText = result.PostprocessMs.HasValue ? result.PostprocessMs.Value.ToString("0.0") + " ms" : "未知";
                    string totalText = result.ServerTotalMs.HasValue ? result.ServerTotalMs.Value + " ms" : "未知";
                    long clientRoundTripMs = result.ReceivedAtMsClient - result.SentAtMs;

                    GM.WriteToStatusTextBox(
                        string.Format(
                            "AI資料擷取: 數據延遲={0}, Preprocess={1}, Inference={2}, Postprocess={3}, ServerTotal={4}, ClientRoundTrip={5} ms, Boxes={6}",
                            latencyText,
                            preprocessText,
                            inferenceText,
                            postprocessText,
                            totalText,
                            clientRoundTripMs,
                            result.Boxes.Count));

                    return result;
                }
            }
        }

        private static long? GetHeaderLong(HttpResponseMessage response, string headerName)
        {
            IEnumerable<string> values;
            if (!response.Headers.TryGetValues(headerName, out values))
                return null;

            long value;
            return long.TryParse(values.FirstOrDefault(), out value) ? (long?)value : null;
        }

        private static double? GetHeaderDouble(HttpResponseMessage response, string headerName)
        {
            IEnumerable<string> values;
            if (!response.Headers.TryGetValues(headerName, out values))
                return null;

            double value;
            return double.TryParse(values.FirstOrDefault(), out value) ? (double?)value : null;
        }
    }
}