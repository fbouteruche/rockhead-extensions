using Amazon.BedrockRuntime;
using Rockhead.Extensions.Amazon;
using Rockhead.Extensions.Cohere;

namespace Rockhead.Extensions.Tests.Cohere;

public class CommandTest
{
    private static readonly AmazonBedrockRuntimeClient BedrockRuntime = new();
    public static TheoryData<Model.CommandText> Models =>
        new TheoryData<Model.CommandText>
        {
            new Model.CommandTextV14(),
            new Model.CommandLightTextV14()
        };
    
    [Theory]
    [Trait("Category", "Integration")]
    [MemberData(nameof(Models))]
    public async Task InvokeCommandV14Async_ShouldNotBeNullOrEmpty(Model.CommandText model)
    {
        // Arrange
        const string prompt = "Describe in one sentence what it a large language model";
        
        // Act
        var response = await BedrockRuntime.InvokeCommandV14Async(model, prompt);
        
        // Assert
        Assert.NotNull(response);
        Assert.NotNull(response.Prompt);
        Assert.NotEmpty(response.Prompt);
        Assert.NotNull(response.Id);
        Assert.NotEmpty(response.Id);
        Assert.NotNull(response.Generations);
        Assert.NotEmpty(response.Generations!);
        foreach (var generation in response.Generations)
        {
            Assert.NotNull(generation);
            Assert.NotNull(generation.Id);
            Assert.NotEmpty(generation.Id!);
            Assert.NotNull(generation.Text);
            Assert.NotEmpty(generation.Text!);
            Assert.NotNull(generation.FinishReason);
        }
    }
    
    [Theory]
    [Trait("Category", "Integration")]
    [MemberData(nameof(Models))]
    public async Task InvokeCommandV14Async_ValidConfig_ShouldNotBeNullOrEmpty(Model.CommandText model)
    {
        // Arrange
        const string prompt = "Describe in one sentence what it a large language model";
        var config = new CommandTextGenerationConfig();
        
        // Act
        var response = await BedrockRuntime.InvokeCommandV14Async(model, prompt, config);
        
        // Assert
        Assert.NotNull(response);
        Assert.NotNull(response.Prompt);
        Assert.NotEmpty(response.Prompt);
        Assert.NotNull(response.Id);
        Assert.NotEmpty(response.Id);
        Assert.NotNull(response.Generations);
        Assert.NotEmpty(response.Generations!);
        Assert.NotEmpty(response.Generations!);
        foreach (var generation in response.Generations)
        {
            Assert.NotNull(generation);
            Assert.NotNull(generation.Id);
            Assert.NotEmpty(generation.Id!);
            Assert.NotNull(generation.Text);
            Assert.NotEmpty(generation.Text!);
            Assert.NotNull(generation.FinishReason); 
        }
    }
    
    [Theory()]
    [Trait("Category", "Integration")]
    [MemberData(nameof(Models))]
    public async Task InvokeCommandV14WithResponseStreamAsync_ShouldNotBeNullOrEmpty(Model.CommandText model)
    {
        // Arrange
        const string prompt = "Describe in one sentence what it a large language model";
        
        // Act
        await foreach (var chunk in  BedrockRuntime.InvokeCommandV14WithResponseStreamAsync(model, prompt))
        {
            // Assert
            chunk.Should().NotBeNull();
            if(chunk.IsFinished is not null && chunk.IsFinished.Value)
            {
                chunk.FinishReason.Should().NotBeNull();
            }
            else
            {
                chunk.Text.Should().NotBeNull();
                chunk.Text.Should().NotBeEmpty();
            }
        }
    }
    
    [Theory()]
    [Trait("Category", "Integration")]
    [MemberData(nameof(Models))]
    public async Task InvokeCommandV14WithResponseStreamAsync_ValidConfig_ShouldNotBeNullOrEmpty(Model.CommandText model)
    {
        // Arrange
        const string prompt = "Describe in one sentence what it a large language model";
        var config = new CommandTextGenerationConfig();
        
        // Act
        await foreach (var chunk in  BedrockRuntime.InvokeCommandV14WithResponseStreamAsync(model, prompt, config))
        {
            // Assert
            chunk.Should().NotBeNull();
            if(chunk.IsFinished is not null && chunk.IsFinished.Value)
            {
                chunk.FinishReason.Should().NotBeNull();
            }
            else
            {
                chunk.Text.Should().NotBeNull();
                chunk.Text.Should().NotBeEmpty();
            }
        }
    }
}
