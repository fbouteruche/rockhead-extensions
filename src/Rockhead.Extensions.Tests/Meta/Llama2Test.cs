using Amazon.BedrockRuntime;
using Rockhead.Extensions.Amazon;
using Rockhead.Extensions.Meta;

namespace Rockhead.Extensions.Tests.Meta;

public class Llama2Test
{
    private static readonly AmazonBedrockRuntimeClient BedrockRuntime = new();
    public static TheoryData<Model.Llama2> Models =>
        new TheoryData<Model.Llama2>
        {
            new Model.Llama213BChatV1(),
            new Model.Llama270BChatV1()
        };
    
    [Theory]
    [Trait("Category", "Integration")]
    [MemberData(nameof(Models))]
    public async Task InvokeLlama2Async_ShouldNotBeNullOrEmpty(Model.Llama2 model)
    {
        // Arrange
        const string prompt = "Describe in one sentence what it a large language model";
        
        // Act
        var response = await BedrockRuntime.InvokeLlama2Async(model, prompt);
        
        // Assert
        Assert.NotNull(response);
        Assert.NotNull(response.Generation);
        Assert.NotEmpty(response.Generation);
        Assert.NotNull(response.StopReason);
        Assert.NotEmpty(response.StopReason);
        Assert.NotNull(response.GenerationTokenCount);
        Assert.NotNull(response.PromptTokenCount);
        
    }
    
    [Theory]
    [Trait("Category", "Integration")]
    [MemberData(nameof(Models))]
    public async Task InvokeLlama2Async_ValidConfig_ShouldNotBeNullOrEmpty(Model.Llama2 model)
    {
        // Arrange
        const string prompt = "Describe in one sentence what it a large language model";
        var config = new Llama2TextGenerationConfig();
        
        // Act
        var response = await BedrockRuntime.InvokeLlama2Async(model, prompt, config);
        
        // Assert
        Assert.NotNull(response);
        Assert.NotNull(response.Generation);
        Assert.NotEmpty(response.Generation);
        Assert.NotNull(response.StopReason);
        Assert.NotEmpty(response.StopReason);
        Assert.NotNull(response.GenerationTokenCount);
        Assert.NotNull(response.PromptTokenCount);
    }
    
    [Theory()]
    [Trait("Category", "Integration")]
    [MemberData(nameof(Models))]
    public async Task InvokeLlama2WithResponseStreamAsync_ShouldNotBeNullOrEmpty(Model.Llama2 model)
    {
        // Arrange
        const string prompt = "Describe in one sentence what it a large language model";
        
        // Act
        int? promptTokenCount = null;
        await foreach (var chunk in BedrockRuntime.InvokeLlama2WithResponseStreamAsync(model, prompt))
        {
            // Assert
            Assert.NotNull(chunk);
            Assert.NotNull(chunk.Generation);
            Assert.NotEmpty(chunk.StopReason ?? chunk.Generation);
            Assert.NotNull(chunk.GenerationTokenCount);
            promptTokenCount ??= chunk.PromptTokenCount;
        }
        Assert.NotNull(promptTokenCount);
    }
    
    [Theory()]
    [Trait("Category", "Integration")]
    [MemberData(nameof(Models))]
    public async Task InvokeLlama2WithResponseStreamAsync_ValidConfig_ShouldNotBeNullOrEmpty(Model.Llama2 model)
    {
        // Arrange
        const string prompt = "Describe in one sentence what it a large language model";
        var config = new Llama2TextGenerationConfig();
        
        // Act
        int? promptTokenCount = null;
        await foreach (var chunk in BedrockRuntime.InvokeLlama2WithResponseStreamAsync(model, prompt, config))
        {
            // Assert
            Assert.NotNull(chunk);
            Assert.NotNull(chunk.Generation);
            Assert.NotEmpty(chunk.StopReason ?? chunk.Generation);
            Assert.NotNull(chunk.GenerationTokenCount);
            promptTokenCount ??= chunk.PromptTokenCount;
        }
        Assert.NotNull(promptTokenCount);
    }
}