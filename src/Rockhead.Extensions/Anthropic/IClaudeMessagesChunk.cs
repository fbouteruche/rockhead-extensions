using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Rockhead.Extensions.Anthropic;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(ClaudeMessagesMessageStartChunk), "message_start")]
[JsonDerivedType(typeof(ClaudeMessagesContentBlockStartChunk), "content_block_start")]
[JsonDerivedType(typeof(ClaudeMessagesContentBlockDeltaChunk), "content_block_delta")]
[JsonDerivedType(typeof(ClaudeMessagesContentBlockStopChunk), "content_block_stop")]
[JsonDerivedType(typeof(ClaudeMessagesMessageDeltaChunk), "message_delta")]
[JsonDerivedType(typeof(ClaudeMessagesMessageStopChunk), "message_stop")]
public interface IClaudeMessagesChunk : IFoundationModelResponse
{
}
