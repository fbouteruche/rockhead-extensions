using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Rockhead.Extensions.Anthropic;

public class ClaudeMessagesContentBlockStopChunk : IClaudeMessagesChunk
{
    [JsonPropertyName("index")]
    public int? Index { get; init; }

    public string? GetResponse()
    {
        return String.Empty;
    }

    public string? GetStopReason()
    {
        return null;
    }
}
