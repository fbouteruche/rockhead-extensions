using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Rockhead.Extensions.Anthropic;

public class ClaudeTextGenerationConfig
{
    [Range(0f, 1f)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("temperature")]
    public float? Temperature { get; init; }

    [Range(0f, 1f)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("top_p")]
    public float? TopP { get; init; }

    [Range(0, 500)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("top_k")] 
    public int? TopK { get; init; }

    [Range(0, 4096)]
    [JsonPropertyName("max_tokens_to_sample")]
    public int MaxTokensToSample { get; init; } = 200;

    [JsonPropertyName("stop_sequences")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<string>? StopSequences { get; init; }
}