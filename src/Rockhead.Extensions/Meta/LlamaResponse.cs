using System.Text.Json.Serialization;

namespace Rockhead.Extensions.Meta;

public class LlamaResponse : IFoundationModelResponse
{
    
    [JsonPropertyName("generation")] public string? Generation { get; init; }
    
    [JsonPropertyName("prompt_token_count")] public int? PromptTokenCount { get; init; }
    
    [JsonPropertyName("generation_token_count")] public int? GenerationTokenCount { get; init; }
    
    [JsonPropertyName("stop_reason")] public string? StopReason { get; init; }
    
    public string? GetResponse()
    {
        return Generation;
    }

    public string? GetStopReason()
    {
        return StopReason;
    }
}