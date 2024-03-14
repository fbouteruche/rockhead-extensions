using Amazon.BedrockRuntime;
using Rockhead.Extensions.Amazon;

namespace Rockhead.Extensions.Tests.Amazon;

public class TitanTextTest
{
    private static readonly AmazonBedrockRuntimeClient BedrockRuntime = new();
    public static TheoryData<Model.TitanText> Models =>
        new TheoryData<Model.TitanText>
            {
                new Model.TitanTextExpressV1(),
                new Model.TitanTextLiteV1()
            };
    
    [Theory]
    [Trait("Category", "Integration")]
    [MemberData(nameof(Models))]
    public async Task InvokeTitanTextG1Async_ShouldNotBeNullOrEmpty(Model.TitanText model)
    {
        // Arrange
        const string prompt = "Describe in one sentence what it a large language model";
        
        // Act
        var response = await BedrockRuntime.InvokeTitanTextG1Async(model, prompt);
        
        // Assert
        Assert.NotNull(response);
        Assert.NotNull(response.Results);
        Assert.NotEmpty(response.Results);
        foreach (var result in response.Results)
        {
            Assert.NotNull(result.OutputText);
            Assert.NotEmpty(result.OutputText);
            Assert.NotNull(result.CompletionReason);
            Assert.NotEmpty(result.CompletionReason);
        }
    }
    
    [Theory]
    [Trait("Category", "Integration")]
    [MemberData(nameof(Models))]
    public async Task InvokeTitanTextG1Async_ValidConfig_ShouldNotBeNullOrEmpty(Model.TitanText model)
    {
        // Arrange
        string prompt = "Describe in one sentence what it a large language model";
        var config = new TitanTextGenerationConfig();
        
        // Act
        var response = await BedrockRuntime.InvokeTitanTextG1Async(model, prompt, config);
        
        // Assert
        Assert.NotNull(response);
        Assert.NotNull(response.Results);
        Assert.NotEmpty(response.Results);
        foreach (var result in response.Results)
        {
            Assert.NotNull(result.OutputText);
            Assert.NotEmpty(result.OutputText);
            Assert.NotNull(result.CompletionReason);
            Assert.NotEmpty(result.CompletionReason);
        }
    }
    
    [Theory()]
    [Trait("Category", "Integration")]
    [MemberData(nameof(Models))]
    public async Task InvokeTitanTextG1WithResponseStreamAsync_ShouldNotBeNullOrEmpty(Model.TitanText model)
    {
        // Arrange
        const string prompt = "Describe in one sentence what it a large language model";
        
        // Act
        await foreach (var chunk in BedrockRuntime.InvokeTitanTextG1WithResponseStreamAsync(model, prompt))
        {
            // Assert
            Assert.NotNull(chunk);
            Assert.NotNull(chunk.OutputText);
            Assert.NotEmpty(chunk.OutputText);
            Assert.NotNull(chunk.CompletionReason);
            Assert.NotEmpty(chunk.CompletionReason);
            Assert.NotNull(chunk.Index);
            Assert.NotNull(chunk.InputTextTokenCount);
            Assert.NotNull(chunk.TotalOutputTextTokenCount);
        }
    }
    
    [Theory()]
    [Trait("Category", "Integration")]
    [MemberData(nameof(Models))]
    public async Task InvokeTitanTextG1WithResponseStreamAsync_ValidConfig_ShouldNotBeNullOrEmpty(Model.TitanText model)
    {
        // Arrange
        string prompt = "Describe in one sentence what it a large language model";
        var config = new TitanTextGenerationConfig();
        
        // Act
        await foreach (var chunk in BedrockRuntime.InvokeTitanTextG1WithResponseStreamAsync(model, prompt, config))
        {
            // Assert
            Assert.NotNull(chunk);
            Assert.NotNull(chunk.OutputText);
            Assert.NotEmpty(chunk.OutputText);
            Assert.NotNull(chunk.CompletionReason);
            Assert.NotEmpty(chunk.CompletionReason);
            Assert.NotNull(chunk.Index);
            Assert.NotNull(chunk.InputTextTokenCount);
            Assert.NotNull(chunk.TotalOutputTextTokenCount);
        }
    }
}