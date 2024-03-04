namespace Rockhead.Extensions;

public static class ModelIds
{
    private static readonly IList<string> Ids =
    [
        "anthropic.claude-instant-v1",
        "anthropic.claude-v1",
        "anthropic.claude-v2",
        "stability.stable-diffusion-xl-v0",
        "amazon.titan-text-lite-v1",
        "amazon.titan-text-express-v1",
        "amazon.titan-image-generator-v1",
        "cohere.command-text-v14",
        "ai21.j2-mid-v1",
        "ai21.j2-ultra-v1",
        "anthropic.claude-v2:1",
        "cohere.command-light-text-v14",
        "meta.llama2-13b-chat-v1",
        "meta.llama2-70b-chat-v1",
        "meta.llama2-13b-v1",
        "meta.llama2-70b-v1",
        "amazon.titan-embed-text-v1",
        "amazon.titan-embed-image-v1",
        "cohere.embed-english-v3",
        "cohere.embed-multilingual-v3"
    ];

    private static readonly IList<string> StreamingSupported =
    [
        "anthropic.claude-instant-v1",
        "anthropic.claude-v2",
        "amazon.titan-text-lite-v1",
        "amazon.titan-text-express-v1",
        "anthropic.claude-v2:1",
        "cohere.command-text-v14",
        "cohere.command-light-text-v14",
        "meta.llama2-13b-chat-v1",
        "meta.llama2-70b-chat-v1",
        "meta.llama2-13b-v1",
        "meta.llama2-70b-v1",
    ];

    public static string ANTHROPIC_CLAUDE_INSTANT_V1 => Ids[0];

    public static string ANTHROPIC_CLAUDE_V1 => Ids[1];

    public static string ANTHROPIC_CLAUDE_V2 => Ids[2];
        
    public static string ANTHROPIC_CLAUDE_V2_1 => Ids[10];

    public static string STABILITY_AI_STABLE_DIFFUSION_XL_V0 => Ids[3];

    public static string AMAZON_TITAN_TEXT_LITE_G1_V1 => Ids[4];

    public static string AMAZON_TITAN_TEXT_EXPRESS_G1_V1 => Ids[5];

    public static string AMAZON_TITAN_IMAGE_GENERATOR_G1_V1 => Ids[6];
        
    public static string AMAZON_TITAN_EMBEDDING_TEXT_G1_V1 => Ids[16];
        
    public static string AMAZON_TITAN_EMBEDDING_IMAGE_G1_V1 => Ids[17];

    public static string COHERE_COMMAND_TEXT_V14 => Ids[7];
        
    public static string COHERE_COMMAND_TEXT_LIGHT_V14 => Ids[11];
        
    public static string COHERE_EMBED_ENGLISH_V3 => Ids[18];
        
    public static string COHERE_EMBED_MULTILINGUAL_V3 => Ids[19];

    public static string AI21_LABS_JURASSIC_V2_MID_V1 => Ids[8];

    public static string AI21_LABS_JURASSIC_V2_ULTRA_V1 => Ids[9];
        
    public static string META_LLAMA2_13B_CHAT_V1 => Ids[12];
        
    public static string META_LLAMA2_70B_CHAT_V1 => Ids[13];
        
    public static string META_LLAMA2_13B_V1 => Ids[14];
        
    public static string META_LLAMA2_70B_V1 => Ids[15];


    public static bool IsSupported(string modelId)
    {
        return Ids.Contains(modelId);
    }

    public static bool IsStreamingSupported(string modelId)
    {
        return StreamingSupported.Contains(modelId);
    }


}