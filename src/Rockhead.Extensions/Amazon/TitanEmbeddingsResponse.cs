using System.Text;
using System.Text.Json.Serialization;

namespace Rockhead.Extensions.Amazon;

public class TitanEmbeddingsResponse : IFoundationModelResponse
{
    [JsonPropertyName("embedding")]
    public IEnumerable<float>? Embeddings { get; init; }
    
    [JsonPropertyName("inputTextTokenCount")]
    public int InputTextTokenCount { get; init; }
    
    public string? GetResponse()
    {
        return Embeddings?.Aggregate(new StringBuilder(), (s, f) => s.Append(' ').Append(f)).ToString();
    }

    public string? GetStopReason()
    {
        throw new NotImplementedException();
    }
}