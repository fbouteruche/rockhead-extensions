﻿namespace Rockhead.Extensions;

public abstract record Model(string ModelId, bool StreamingSupported)
{
    public record NullModel() : Model(string.Empty, false);
    public abstract record Jurassic2(string ModelId, bool StreamingSupported) : Model(ModelId, StreamingSupported);
    public record Jurassic2MidV1() : Jurassic2("ai21.j2-mid-v1", false);
    public record Jurassic2UltraV1() : Jurassic2("ai21.j2-ultra-v1", false);
    public abstract record TitanText(string ModelId, bool StreamingSupported) : Model(ModelId, StreamingSupported);
    public record TitanTextLiteV1() : TitanText("amazon.titan-text-lite-v1", true);
    public record TitanTextExpressV1() : TitanText("amazon.titan-text-express-v1", true);
    public record TitanImageGeneratorV1() : Model("amazon.titan-image-generator-v1", false);
    public record TitanEmbedTextV1() : Model("amazon.titan-embed-text-v1", false);
    public record TitanEmbedImageV1() : Model("amazon.titan-embed-image-v1", false);
    public abstract record Claude(string ModelId, bool StreamingSupported) : Model(ModelId, StreamingSupported);
    public abstract record ClaudeTextCompletionSupport(string ModelId, bool StreamingSupported) : Claude(ModelId, StreamingSupported);
    public record ClaudeInstantV1() : ClaudeTextCompletionSupport("anthropic.claude-instant-v1", true);
    public record ClaudeV2() : ClaudeTextCompletionSupport("anthropic.claude-v2", true);
    public record ClaudeV2_1() : ClaudeTextCompletionSupport("anthropic.claude-v2:1", true);
    public record Claude3Sonnet() : Claude("anthropic.claude-3-sonnet-20240229-v1:0", true);
    public record Claude3Haiku() : Claude("anthropic.claude-3-haiku-20240307-v1:0", true);
    public record Claude3Opus() : Claude("anthropic.claude-3-opus-20240229-v1:0", true);
    public abstract record CommandText(string ModelId, bool StreamingSupported) : Model(ModelId, StreamingSupported);
    public record CommandTextV14() : CommandText("cohere.command-text-v14", true);
    public record CommandLightTextV14() : CommandText("cohere.command-light-text-v14", true);
    public abstract record Embed(string ModelId, bool StreamingSupported) : Model(ModelId, StreamingSupported);
    public record EmbedEnglishV3() : Embed("cohere.embed-english-v3", false);
    public record EmbedMultilingualV3() : Embed("cohere.embed-multilingual-v3", false);
    public abstract record Llama(string ModelId, bool StreamingSupported) : Model(ModelId, StreamingSupported);
    public record Llama213BChatV1() : Llama("meta.llama2-13b-chat-v1", true);
    public record Llama270BChatV1() : Llama("meta.llama2-70b-chat-v1", true);
    public record Llama38BInstructV1() : Llama("meta.llama3-8b-instruct-v1:0", true);
    public record Llama370BInstructV1() : Llama("meta.llama3-70b-instruct-v1:0", true);
    public abstract record StableDiffusionXl(string ModelId, bool StreamingSupported) : Model(ModelId, StreamingSupported);
    public record StableDiffusionXlV1() : StableDiffusionXl("stability.stable-diffusion-xl-v1", false);
    public abstract record Mistral(string ModelId, bool StreamingSupported) : Model(ModelId, StreamingSupported);
    public record Mistral7BInstruct() : Mistral("mistral.mistral-7b-instruct-v0:2", true);
    public record Mistral8x7BInstruct() : Mistral("mistral.mixtral-8x7b-instruct-v0:1", true);
    public record MistralLarge() : Mistral("mistral.mistral-large-2402-v1:0", true);

    public static bool IsSupported(string modelId)
    {
        return TryParse(modelId, out _);
    }
    
    public static bool IsStreamingSupported(string modelId)
    {
        return TryParse(modelId, out var model) && model.StreamingSupported;
    }
    
    public static bool TryParse(string modelId, out Model model)
    {
        try
        {
            model = Parse(modelId);
            return true;
        }
        catch (ArgumentException)
        {
            model = new NullModel();
            return false;
        }
    }
    
    public static Model Parse(string modelId) =>
        modelId switch
        {
            "ai21.j2-mid-v1" => new Jurassic2MidV1(),
            "ai21.j2-ultra-v1" => new Jurassic2UltraV1(),
            "amazon.titan-text-lite-v1" => new TitanTextLiteV1(),
            "amazon.titan-text-express-v1" => new TitanTextExpressV1(),
            "amazon.titan-image-generator-v1" => new TitanImageGeneratorV1(),
            "amazon.titan-embed-text-v1" => new TitanEmbedTextV1(),
            "amazon.titan-embed-image-v1" => new TitanEmbedImageV1(),
            "anthropic.claude-instant-v1" => new ClaudeInstantV1(),
            "anthropic.claude-v2" => new ClaudeV2(),
            "anthropic.claude-v2:1" => new ClaudeV2_1(),
            "anthropic.claude-3-sonnet-20240229-v1:0" => new Claude3Sonnet(),
            "anthropic.claude-3-haiku-20240307-v1:0" => new Claude3Haiku(),
            "anthropic.claude-3-opus-20240229-v1:0" => new Claude3Opus(),
            "cohere.command-text-v14" => new CommandTextV14(),
            "cohere.command-light-text-v14" => new CommandLightTextV14(),
            "cohere.embed-english-v3" => new EmbedEnglishV3(),
            "cohere.embed-multilingual-v3" => new EmbedMultilingualV3(),
            "meta.llama2-13b-chat-v1" => new Llama213BChatV1(),
            "meta.llama2-70b-chat-v1" => new Llama270BChatV1(),
            "meta.llama3-8b-instruct-v1:0" => new Llama38BInstructV1(),
            "meta.llama3-70b-instruct-v1:0" => new Llama370BInstructV1(),
            "stability.stable-diffusion-xl-v1" => new StableDiffusionXlV1(),
            "mistral.mistral-7b-instruct-v0:2" => new Mistral7BInstruct(),
            "mistral.mixtral-8x7b-instruct-v0:1" => new Mistral8x7BInstruct(),
            "mistral.mistral-large-2402-v1:0" => new MistralLarge(),
            _ => throw new ArgumentException($"{modelId} is not a supported or valid model Id")
        };
}