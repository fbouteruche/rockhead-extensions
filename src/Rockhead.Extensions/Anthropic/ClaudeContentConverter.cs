using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Rockhead.Extensions.Anthropic;

public class ClaudeContentConverter : JsonConverter<IClaudeContent>
{
    public override IClaudeContent? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var result = JsonSerializer.Deserialize(ref reader, typeof(ClaudeImageContent), options);
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, IClaudeContent value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case null:
                JsonSerializer.Serialize(writer, (IClaudeContent?)null, options);
                break;
            default:
                var type = value.GetType();
                JsonSerializer.Serialize(writer, value, type, options);
                break;
        }
    }
}
