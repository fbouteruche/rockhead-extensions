using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Rockhead.Extensions.Anthropic;

public class ClaudeMessagesContentBlockDeltaChunk : IClaudeMessagesChunk
{
    [JsonPropertyName("index")]
    public int? Index { get; init; }

    [JsonPropertyName("delta")]
    public BlockDeltaChunkContentBlock? Delta { get; init; }

    public string? GetResponse()
    {
        return Delta?.Text;
    }

    public string? GetStopReason()
    {
        return null;
    }

    public class BlockDeltaChunkContentBlock
    {
        [JsonPropertyName("type")]
        public string? Type { get; init; }

        [JsonPropertyName("text")]
        public string? Text { get; init; }
    }
}
