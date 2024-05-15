using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Rockhead.Extensions.Anthropic;

public class ClaudeTextContent : IClaudeContent
{
    [JsonPropertyName("text")]
    public required string Text { get; init; }
}
