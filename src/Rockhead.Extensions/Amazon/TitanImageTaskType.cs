using System.Text.Json.Serialization;

namespace Rockhead.Extensions.Amazon;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TitanImageTaskType
{
    TextImage,
    InPainting,
    OutPainting,
    ImageVariation
}