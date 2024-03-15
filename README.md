# Rockhead.Extensions

[![Build & Test](https://github.com/fbouteruche/rockhead-extensions/actions/workflows/dotnet.yml/badge.svg?branch=main)](https://github.com/fbouteruche/rockhead-extensions/actions/workflows/dotnet.yml)

*Rockhead.Extensions* is a set of extension methods for the AWS SDK for .NET Bedrock Runtime client. 
Amazon Bedrock is a serverless service making available through a unique API foundation models (FMs) from
Amazon, AI21 Labs, Anthropic, Cohere, Meta, Mistral AI and Stability AI.

*Rockhead.Extensions* provides you extension methods to the low-level Bedrock Runtime API with 
strongly typed parameters and responses for each supported foundation models. It makes your developer life easier.

## Rockhead.Extensions

Available on Nuget: https://www.nuget.org/packages/Rockhead.Extensions

- AI21 Labs extension methods
  - `InvokeJurassic2Async`
    - An extension method to invoke Jurassic 2 models to generate text with strongly type parameters and response
    - Support Jurassic 2 Mid or Ultra models
- Amazon extension methods
  - `InvokeTitanImageGeneratorG1ForTextToImageAsync`
    - An extension method to invoke Titan Image Generator G1 to generate images with strongly type parameters and response
  - `InvokeTitanTextG1Async`
    - An extension method to invoke Titan Text G1 models to generate text with strongly type parameters and response
    - Support Titan G1 Lite or Express
  - `InvokeTitanTextG1WithResponseStreamAsync`
    - An extension method to invoke Titan Text G1 models to generate text with strongly type parameters and returning an IAsyncEnumerable of strongly typed response
    - Support Titan G1 Lite or Express
  - `InvokeTitanEmbeddingsG1TextAsync`
    - An extension method to invoke Titan Embeddings G1 models to generate embeddings with strongly type parameters and response
  - `InvokeTitanMultimodalEmbeddingsG1Async`
    - An extension method to invoke Titan Multimodal Embeddings G1 models to generate embeddings with strongly type parameters and response
- Anthropic extension methods
  - `InvokeClaudeAsync`
    - An extension method to invoke Claude models to generate text with strongly type parameters and response
    - Support Claude Instant v1, Claude v2 and Claude v2.1
  - `InvokeClaudeWithResponseStreamAsync`
    - An extension method to invoke Claude models to generate text with strongly type parameters and returning an IAsyncEnumerable of strongly typed response
    - Support Claude Instant v1, Claude v2 and Claude v2.1
- Cohere extension methods
  - `InvokeCommandV14Async`
    - An extension method to invoke Command v14 models to generate text with strongly type parameters and response
    - Support Command v14 Text and Command v14 Light Text
  - `InvokeCommandV14WithResponseStreamAsync`
    - An extension method to invoke Command v14 models to generate text with strongly type parameters and returning an IAsyncEnumerable of strongly typed response
    - Support Command v14 Text and Command v14 Light Text
  - `InvokeEmbedV3Async`
- Meta extension methods
  - `InvokeLlama2Async`
    - An extension method to invoke Llama 2 models to generate text with strongly type parameters and response
    - Support Llama 2 13B Chat v1 and Llama 2 70B Chat v1
  - `InvokeLlama2WithResponseStreamAsync`
    - An extension method to invoke Llama 2 models to generate text with strongly type parameters and returning an IAsyncEnumerable of strongly typed response
    - Support Llama 2 13B Chat v1 and Llama 2 70B Chat v1
- Stability AI extension methods
  - `InvokeStableDiffusionXlForTextToImageAsync`
    - An extension method to invoke Stable Diffusion XL to generate images with strongly type parameters and response

## Setup

- *Rockhead.Extensions* package is available on NuGet: https://www.nuget.org/packages/Rockhead.Extensions
- For now, *Rockhead.Extensions* is only available for .NET 8
- You can install *Rockhead.Extensions* with the dotnet CLI:
```bash
dotnet add package rockhead.extensions
```

# Usage

Below are a few examples of using the extension methods to invoke different models.

## Invoke Claude v2.1 model

```csharp
public async Task<string> GetLlmDescription()
{
    const string prompt = @"Human: Describe in one sentence what it a large language model\n\nAssistant:";
    var config = new ClaudeTextGenerationConfig()
    {
        MaxTokensToSample = 2048,
        Temperature = 0.8f
    };
    
    var response = await BedrockRuntime.InvokeClaudeAsync(new Model.ClaudeV2_1(), prompt, config);
    
    return response?.Completion ?? "";
}
```

## Invoke Claude v2.1 model with a response stream

```csharp
public async Task<string> GetLlmDescription()
    {
        const string prompt = @"Human: Describe in one sentence what it a large language model\n\nAssistant:";
        var config = new ClaudeTextGenerationConfig()
        {
            MaxTokensToSample = 2048,
            Temperature = 0.8f
        };

        var response = new StringBuilder();
        await foreach (var chunk in BedrockRuntime.InvokeClaudeWithResponseStreamAsync(new Model.ClaudeV2_1(), prompt, config))
        {
            response.Append(chunk.Completion);
        }
    
        return response.ToString();
    }
```

## Invoke Llama 2 70B Chat

```csharp
public async Task<string> GetLlmDescription()
{
    const string prompt = @"Describe in one sentence what it a large language model";
    var config = new Llama2TextGenerationConfig()
    {
        MaxGenLen = 2048,
        Temperature = 0.8f
    };
    
    var response = await BedrockRuntime.InvokeLlama2Async(new Model.Llama270BChatV1(), prompt, config);
    
    return response?.Generation ?? "";
}
```
## Learn More

- [Learn more about Amazon Bedrock](https://docs.aws.amazon.com/bedrock/latest/userguide/what-is-bedrock.html)
- [Learn more about foundation model inference parameters](https://docs.aws.amazon.com/bedrock/latest/userguide/model-parameters.html)
- [C# code examples for Amazon Bedrock using the AWS SDK for .NET](https://docs.aws.amazon.com/bedrock/latest/userguide/service_code_examples.html)
