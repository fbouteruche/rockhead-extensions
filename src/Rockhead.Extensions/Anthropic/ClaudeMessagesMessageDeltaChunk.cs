using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static Rockhead.Extensions.Anthropic.ClaudeMessagesResponse;

namespace Rockhead.Extensions.Anthropic;

public class ClaudeMessagesMessageDeltaChunk : IClaudeMessagesChunk
{
    [JsonPropertyName("delta")]
    public MessageDelta? Delta { get; set; }

    [JsonPropertyName("usage")] public MessageDeltaChunkUsage? Usage { get; init; }

    public string? GetResponse()
    {
        return String.Empty;
    }

    public string? GetStopReason()
    {
        return Delta?.StopReason;
    }

    public class MessageDelta
    {   [JsonPropertyName("stop_reason")] public string? StopReason { get; init; }

        [JsonPropertyName("stop_sequence")] public string? StopSequence { get; init; }
    }

    public class MessageDeltaChunkUsage
    {
        [JsonPropertyName("output_tokens")] public int OutputTokens { get; init; }
    }
}
