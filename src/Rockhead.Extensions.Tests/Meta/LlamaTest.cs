using Amazon;
using Amazon.BedrockRuntime;
using Rockhead.Extensions.Meta;

namespace Rockhead.Extensions.Tests.Meta;

public class LlamaTest
{
    private static readonly AmazonBedrockRuntimeClient BedrockRuntime = new(RegionEndpoint.USEast1);
    public static TheoryData<Model.Llama> Models =>
        new TheoryData<Model.Llama>
        {
            new Model.Llama213BChatV1(),
            new Model.Llama270BChatV1(),
            new Model.Llama38BInstructV1(),
            new Model.Llama370BInstructV1()
        };
    
    [Theory]
    [Trait("Category", "Integration")]
    [MemberData(nameof(Models))]
    public async Task InvokeLlamaModelAsync_ShouldNotBeNullOrEmpty(Model.Llama model)
    {
        // Arrange
        const string prompt = "Describe in one sentence what it a large language model";
        
        // Act
        var response = await BedrockRuntime.InvokeLlamaAsync(model, prompt);
        
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
    public async Task InvokeLlamaModelAsync_ValidConfig_ShouldNotBeNullOrEmpty(Model.Llama model)
    {
        // Arrange
        const string prompt = "Describe in one sentence what it a large language model";
        var config = new LlamaTextGenerationConfig();
        
        // Act
        var response = await BedrockRuntime.InvokeLlamaAsync(model, prompt, config);
        
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
    public async Task InvokeLlamaModelWithResponseStreamAsync_ShouldNotBeNullOrEmpty(Model.Llama model)
    {
        // Arrange
        const string prompt = "Describe in one sentence what it a large language model";
        
        // Act
        int? promptTokenCount = null;
        await foreach (var chunk in BedrockRuntime.InvokeLlamaWithResponseStreamAsync(model, prompt))
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
    public async Task InvokeLlamaWithResponseStreamAsync_ValidConfig_ShouldNotBeNullOrEmpty(Model.Llama model)
    {
        // Arrange
        const string prompt = "Describe in one sentence what it a large language model";
        var config = new LlamaTextGenerationConfig();
        
        // Act
        int? promptTokenCount = null;
        await foreach (var chunk in BedrockRuntime.InvokeLlamaWithResponseStreamAsync(model, prompt, config))
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