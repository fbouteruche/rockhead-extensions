using System.Text.Json.Serialization;

namespace Rockhead.Extensions.Amazon
{
    public class TitanTextStreamingResponse : IFoundationModelResponse
    {
        [JsonPropertyName("outputText")] public string? OutputText { get; init; }
        
        [JsonPropertyName("totalOutputTextTokenCount")] public int? TotalOutputTextTokenCount { get; init; }
        
        [JsonPropertyName("index")] public int? Index { get; set; }
        [JsonPropertyName("completionReason")] public string? CompletionReason { get; set; }
        
        [JsonPropertyName("inputTextTokenCount")] public int? InputTextTokenCount { get; set; }

        public string? GetResponse()
        {
            return OutputText;
        }

        public string? GetStopReason()
        {
            return CompletionReason;
        }
    }
}
