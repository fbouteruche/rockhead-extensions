using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Rockhead.Extensions.Anthropic;

public class ClaudeImageContent : IClaudeContent
{
    [JsonPropertyName("source")]
    public required ClaudeImageSource Source { get; init; }
}
