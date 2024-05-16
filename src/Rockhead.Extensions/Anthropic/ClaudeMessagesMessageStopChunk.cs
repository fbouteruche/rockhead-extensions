using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Rockhead.Extensions.Anthropic;

public class ClaudeMessagesMessageStopChunk : IClaudeMessagesChunk
{
    [JsonPropertyName("amazon-bedrock-invocationMetrics")]
    public AmazonBedrockInvocationMetrics? InvocationMetrics { get; init; }

    public string? GetResponse()
    {
        return null;
    }

    public string? GetStopReason()
    {
        return null;
    }

    public class AmazonBedrockInvocationMetrics
    {
        [JsonPropertyName("inputTokenCount")] public int? InputTokenCount { get; init; }

        [JsonPropertyName("outputTokenCount")] public int? OutputTokenCount { get; init; }

        [JsonPropertyName("invocationLatency")] public int? InvocationLatency { get; init; }

        [JsonPropertyName("firstByteLatency")] public int? FirstByteLatency { get; init; }
    }
}
