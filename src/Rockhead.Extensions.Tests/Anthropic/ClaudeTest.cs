using Amazon.BedrockRuntime;
using Rockhead.Extensions.Amazon;
using Rockhead.Extensions.Anthropic;

namespace Rockhead.Extensions.Tests.Anthropic;

public class ClaudeTest
{
    private static readonly AmazonBedrockRuntimeClient BedrockRuntime = new();
    public static TheoryData<Model.Claude> Models =>
        new TheoryData<Model.Claude>
        {
            new Model.ClaudeInstantV1(),
            new Model.ClaudeV2(),
            new Model.ClaudeInstantV1()
        };
    
    [Theory]
    [Trait("Category", "Integration")]
    [MemberData(nameof(Models))]
    public async Task InvokeClaudeAsync_ShouldNotBeNullOrEmpty(Model.Claude model)
    {
        // Arrange
        const string prompt = @"Human: Describe in one sentence what it a large language model\n\nAssistant:";

        // Act
        var response = await BedrockRuntime.InvokeClaudeAsync(model, prompt);

        // Assert
        Assert.NotNull(response);
        Assert.NotNull(response.Completion);
        Assert.NotEmpty(response.Completion);
        Assert.NotNull(response.Stop);
        Assert.NotEmpty(response.Stop);
        Assert.NotNull(response.StopReason);
        Assert.NotEmpty(response.StopReason);
    }
    
    [Theory]
    [Trait("Category", "Integration")]
    [MemberData(nameof(Models))]
    public async Task InvokeClaudeAsync_ValidConfig_ShouldNotBeNullOrEmpty(Model.Claude model)
    {
        // Arrange
        const string prompt = @"Human: Describe in one sentence what it a large language model\n\nAssistant:";
        var config = new ClaudeTextGenerationConfig();
        
        // Act
        var response = await BedrockRuntime.InvokeClaudeAsync(model, prompt, config);
        
        // Assert
        Assert.NotNull(response);
        Assert.NotNull(response.Completion);
        Assert.NotEmpty(response.Completion);
        Assert.NotNull(response.Stop);
        Assert.NotEmpty(response.Stop);
        Assert.NotNull(response.StopReason);
        Assert.NotEmpty(response.StopReason);
    }
    
    [Theory()]
    [Trait("Category", "Integration")]
    [MemberData(nameof(Models))]
    public async Task InvokeClaudeWithResponseStreamAsync_ShouldNotBeNullOrEmpty(Model.Claude model)
    {
        // Arrange
        const string prompt = @"Human: Describe in one sentence what it a large language model\n\nAssistant:";

        // Act
        await foreach (var chunk in BedrockRuntime.InvokeClaudeWithResponseStreamAsync(model, prompt))
        {
            // Assert
            Assert.NotNull(chunk);
            Assert.NotNull(chunk.Completion);
            if (chunk.Stop is not null)
            {
                Assert.Empty(chunk.Completion);
                Assert.NotEmpty(chunk.Stop);
            }
            else
            {
                Assert.NotEmpty(chunk.Completion);
            }
            if (chunk.StopReason is not null)
            {
                Assert.NotEmpty(chunk.StopReason);
            }
        }
    }
    
    [Theory()]
    [Trait("Category", "Integration")]
    [MemberData(nameof(Models))]
    public async Task InvokeClaudeWithResponseStreamAsync_ValidConfig_ShouldNotBeNullOrEmpty(Model.Claude model)
    {
        // Arrange
        const string prompt = @"Human: Describe in one sentence what it a large language model\n\nAssistant:";
        var config = new ClaudeTextGenerationConfig();
        
        // Act
        await foreach (var chunk in BedrockRuntime.InvokeClaudeWithResponseStreamAsync(model, prompt, config))
        {
            // Assert
            Assert.NotNull(chunk);
            Assert.NotNull(chunk.Completion);
            if (chunk.Stop is not null)
            {
                Assert.Empty(chunk.Completion);
                Assert.NotEmpty(chunk.Stop);
            }
            else
            {
                Assert.NotEmpty(chunk.Completion);
            }
            if (chunk.StopReason is not null)
            {
                Assert.NotEmpty(chunk.StopReason);
            }
        }
    }
}