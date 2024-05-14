using System.Text.Json.Serialization;

namespace Rockhead.Extensions.MistralAI;

public class MistralResponse : IFoundationModelResponse
{

    [JsonPropertyName("outputs")] public IEnumerable<MistralResponse.Output>? Outputs{ get; init; }


    public string? GetResponse()
    {
        return Outputs?.FirstOrDefault()?.Text;
    }

    public string? GetStopReason()
    {
        return Outputs?.FirstOrDefault()?.StopReason;
    }

    public class Output
    {
        [JsonPropertyName("text")] public string? Text { get; init; }

        [JsonPropertyName("stop_reason")] public string? StopReason { get; init; }
    }
}