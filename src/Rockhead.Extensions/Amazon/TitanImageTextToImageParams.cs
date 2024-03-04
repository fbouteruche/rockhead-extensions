using System.Text.Json.Serialization;

namespace Rockhead.Extensions.Amazon;

public class TitanImageTextToImageParams
{
    public required string Text { get; init; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? NegativeText { get; set; }
    
}