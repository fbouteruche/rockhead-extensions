using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Rockhead.Extensions.Amazon;

public class TitanMultimodalEmbeddingConfig
{
    [AllowedValues(256, 384, 1024)]
    [JsonPropertyName("outputEmbeddingLength")]
    public int? OutputEmbeddingLength { get; init; }
}