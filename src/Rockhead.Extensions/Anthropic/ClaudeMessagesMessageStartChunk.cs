using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Rockhead.Extensions.Anthropic;

public class ClaudeMessagesMessageStartChunk : IClaudeMessagesChunk
{
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    [JsonPropertyName("message")]
    public ClaudeMessagesResponse? Message { get; init; }

    public string? GetResponse()
    {
        return ((ClaudeTextContent?)Message?.Content.FirstOrDefault())?.Text;
    }

    public string? GetStopReason()
    {
        return null;
    }
}
