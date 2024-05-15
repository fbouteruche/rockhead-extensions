using System.Text.Json.Serialization;

namespace Rockhead.Extensions.Anthropic;

public class ClaudeImageSource
{
    [JsonPropertyName("type")]
    public string Type { get; } = "base64";

    [JsonPropertyName("media_type")]
    public required string MediaType { get; init; }

    [JsonPropertyName("data")]
    public required string Data { get; init; }
}
