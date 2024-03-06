using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Rockhead.Extensions.Amazon
{
    public class TitanImageGenerationConfig
    {
        [Range(1, 5)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyName("numberOfImages")]
        public int NumberOfImages { get; set; }

        [JsonPropertyName("quality")]
        public ImageQuality Quality { get; set; }

        [Range(1, 1024)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyOrder(4)]
        [JsonPropertyName("height")]
        public int Height { get; set; }

        [Range(1, 1024)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyOrder(5)]
        [JsonPropertyName("width")]
        public int Width { get; set; }

        [Range(1.0, 10.0)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonPropertyOrder(3)]
        [JsonPropertyName("cfgScale")]
        public float CfgScale { get; set; }

        [Range(0, 214783647)]
        [JsonPropertyOrder(6)]
        [JsonPropertyName("seed")]
        public int Seed { get; set; }

        public enum ImageQuality
        {
            Standard,
            Premium
        }
    }
}