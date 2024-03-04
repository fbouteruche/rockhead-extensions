using System.Text;
using System.Text.Json.Serialization;

namespace Rockhead.Extensions.Cohere;

public class EmbedResponse : IFoundationModelResponse
{
    /// <summary>
    /// An array of embeddings, where each embedding is an array of floats with 1024 elements. The length of the embeddings array will be the same as the length of the original texts array.
    /// </summary>
    [JsonPropertyName("embeddings")] public IEnumerable<IEnumerable<float>>? Embeddings { get; init; }
    
    /// <summary>
    /// An identifier for the response.
    /// </summary>
    [JsonPropertyName("id")] public string? Id { get; init; }
    
    /// <summary>
    /// The type of the response
    /// </summary>
    [JsonPropertyName("response_type")] public string? ResponseType { get; init; }

    /// <summary>
    /// An array containing the text entries for which embeddings were returned.
    /// </summary>
    [JsonPropertyName("texts")] public IEnumerable<string>? Texts { get; init; }

    public string? GetResponse()
    {
        return Embeddings?.FirstOrDefault()?.Aggregate(new StringBuilder(), (s, f) => s.Append(' ').Append(f)).ToString();
    }

    public string? GetStopReason()
    {
        throw new NotImplementedException();
    }
}