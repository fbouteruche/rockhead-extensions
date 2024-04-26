using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Rockhead.Extensions.Amazon
{
    public class TitanImageGenerationConfig
    {
        [Range(1, 5)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("numberOfImages")]
        public int? NumberOfImages { get; set; }

        [Range(1, 1408)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyOrder(4)]
        [JsonPropertyName("height")]
        public int? Height { get; set; }

        [Range(1, 1408)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyOrder(5)]
        [JsonPropertyName("width")]
        public int? Width { get; set; }

        [Range(1.1, 10.0)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyOrder(3)]
        [JsonPropertyName("cfgScale")]
        public float? CfgScale { get; set; }

        [Range(0, 214783647)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyOrder(6)]
        [JsonPropertyName("seed")]
        public int? Seed { get; set; }
    }
}