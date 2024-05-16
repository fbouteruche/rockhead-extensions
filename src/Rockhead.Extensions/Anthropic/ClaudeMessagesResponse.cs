using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Rockhead.Extensions.Anthropic;

public class ClaudeMessagesResponse : ClaudeMessage, IFoundationModelResponse
{
    [JsonPropertyName("id")] public string? Id { get; init; }

    [JsonPropertyName("model")] public string? Model { get; init; }

    [JsonPropertyName("type")] public string? Type { get; init; }

    [JsonPropertyName("stop_reason")] public string? StopReason { get; init; }

    [JsonPropertyName("stop_sequence")] public string? StopSequence { get; init; }

    [JsonPropertyName("usage")] public ClaudeUsage? Usage { get; init; }

    public string? GetResponse()
    {
        return ((ClaudeTextContent?)Content?.LastOrDefault())?.Text;
    }

    public string? GetStopReason()
    {
        return StopReason;
    }

    public class ClaudeUsage
    {
        [JsonPropertyName("input_tokens")] public int InputTokens { get; init; }

        [JsonPropertyName("output_tokens")] public int OutputTokens { get; init; }
    }
}
