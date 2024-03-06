using System.Text.Json.Serialization;

namespace Rockhead.Extensions.Amazon;

public class TitanImageTextToImageParams
{
    [JsonPropertyName("text")]
    public required string Text { get; init; }
    
    [JsonPropertyName("negativeText")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? NegativeText { get; set; }
    
}