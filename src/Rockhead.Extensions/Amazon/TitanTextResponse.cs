using System.Text.Json.Serialization;

namespace Rockhead.Extensions.Amazon
{
    public class TitanTextResponse : IFoundationModelResponse
    {
        public class AmazonTitanTextOutput
        {
            [JsonPropertyName("tokenCount")]
            public int TokenCount { get; set; }

            [JsonPropertyName("outputText")]
            public string? OutputText { get; set; }

            [JsonPropertyName("completionReason")]
            public string? CompletionReason { get; set; }
        }
        
        [JsonPropertyName("inputTextTokenCount")]
        public int InputTextTokenCount { get; init; }

        [JsonPropertyName("results")]
        public IEnumerable<AmazonTitanTextOutput>? Results { get; init; }

        public string? GetResponse()
        {
            return Results?.FirstOrDefault()?.OutputText;
        }

        public string? GetStopReason()
        {
            return Results?.FirstOrDefault()?.CompletionReason;
        }
    }
}
