using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Rockhead.Extensions.Cohere;

public class CommandStreamingResponse : IFoundationModelResponse
{ 
    [JsonPropertyName("finish_reason")] public Reason? FinishReason { get; init; }
            
    [JsonPropertyName("id")] public string? Id { get; init; }
        
    [JsonPropertyName("text")] public string? Text { get; init; }
        
    [JsonPropertyName("prompt")] public string? Prompt { get; init; }
        
    [JsonPropertyName("likelihood")] public float? Likelihood { get; init; }
        
    [JsonPropertyName("token_likelihoods")] public IEnumerable<JsonObject>? TokenLikelihoods { get; init; }
        
    [JsonPropertyName("is_finished")] public bool? IsFinished { get; init; }
        
    [JsonPropertyName("index")] public int? Index { get; init; }
        
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Reason
    {
        COMPLETE,
        MAX_TOKENS,
        ERROR,
        ERROR_TOXIC
    }

    public string? GetResponse()
    {
        return Text;
    }

    public string? GetStopReason()
    {
        return FinishReason?.ToString();
    }
}