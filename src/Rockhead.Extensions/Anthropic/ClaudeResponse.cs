using System.Text.Json.Serialization;

namespace Rockhead.Extensions.Anthropic;

public class ClaudeResponse : IFoundationModelResponse
{
    [JsonPropertyName("completion")] public string? Completion { get; init; }

    [JsonPropertyName("stop_reason")] public string? StopReason { get; init; }
        
    [JsonPropertyName("stop")] public string? Stop { get; init; }

    public string? GetResponse()
    {
        return Completion;
    }

    public string? GetStopReason()
    {
        return StopReason;
    }
}