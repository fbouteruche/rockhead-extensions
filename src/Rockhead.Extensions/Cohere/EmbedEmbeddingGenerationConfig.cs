using System.Text.Json.Serialization;

namespace Rockhead.Extensions.Cohere;

public class EmbedEmbeddingGenerationConfig
{
    [JsonPropertyName("input_type")] public InputTypeEnum InputType { get; init; } = InputTypeEnum.search_document;
    
    [JsonPropertyName("truncate")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TruncateEnum? Truncate { get; init; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum InputTypeEnum
    {
        search_document,
        search_query,
        classification,
        clustering
    }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TruncateEnum
    {
        NONE,
        START,
        END
    }
}