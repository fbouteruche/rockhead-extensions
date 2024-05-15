using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ThirdParty.Json.LitJson;

namespace Rockhead.Extensions.Anthropic;

public class ClaudeMessagesConfig
{
    [JsonPropertyName("anthropic_version")]
    public string AnthropicVersion { get; } = "bedrock-2023-05-31";

    [JsonPropertyName("max_tokens")]

    public required int MaxTokens { get; init; }

    [JsonPropertyName("system")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? System { get; init; }

    [JsonPropertyName("messages")]
    public IList<ClaudeMessage> Messages { get; init; } = [];

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

    [JsonPropertyName("stop_sequences")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<string>? StopSequences { get; init; }
}
