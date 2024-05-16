using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Rockhead.Extensions.Anthropic;

public class ClaudeMessagesContentBlockStartChunk : IClaudeMessagesChunk
{
    [JsonPropertyName("index")]
    public int? Index { get; init; }

    [JsonPropertyName("content_block")]
    public BlockStartChunkContentBlock? ContentBlock { get; init; }

    public string? GetResponse()
    {
        return ContentBlock?.Text;
    }

    public string? GetStopReason()
    {
        return null;
    }

    public class BlockStartChunkContentBlock
    {
        [JsonPropertyName("type")]
        public string? Type { get; init; }

        [JsonPropertyName("text")]
        public string? Text { get; init; }
    }
}
