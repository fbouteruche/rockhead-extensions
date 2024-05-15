using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Rockhead.Extensions.Anthropic;

//[JsonConverter(typeof(ClaudeContentConverter))]
[JsonPolymorphic(TypeDiscriminatorPropertyName ="type")]
[JsonDerivedType(typeof(ClaudeTextContent), "text")]
[JsonDerivedType(typeof(ClaudeImageContent), "image")]
public interface IClaudeContent
{
}
