using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Rockhead.Extensions.Meta;

public class LlamaTextGenerationConfig
{
    [Range(0f, 1f)]
    [JsonPropertyName("temperature")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public float? Temperature { get; init; }
    
    [Range(0f, 1f)]
    [JsonPropertyName("top_p")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public float? TopP { get; init; }
    
    [Range(1, 2048)]
    [JsonPropertyName("max_gen_len")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MaxGenLen { get; init; }
}