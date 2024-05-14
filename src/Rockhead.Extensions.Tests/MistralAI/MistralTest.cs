using Amazon;
using Amazon.BedrockRuntime;
using Rockhead.Extensions.Meta;
using Rockhead.Extensions.MistralAI;

namespace Rockhead.Extensions.Tests.MistralAI;

public class MistralTest
{
    private static readonly AmazonBedrockRuntimeClient BedrockRuntime = new(RegionEndpoint.USEast1);
    public static TheoryData<Model.Mistral> Models =>
        new TheoryData<Model.Mistral>
        {
            new Model.Mistral7BInstruct(),
            new Model.Mistral8x7BInstruct(),
            new Model.MistralLarge()
        };

    [Theory]
    [Trait("Category", "Integration")]
    [MemberData(nameof(Models))]
    public async Task InvokeMistralModelAsync_ShouldNotBeNullOrEmpty(Model.Mistral model)
    {
        // Arrange
        const string prompt = "Describe in one sentence what it a large language model";

        // Act
        var response = await BedrockRuntime.InvokeMistralAsync(model, prompt);

        // Assert
        Assert.NotNull(response);
        Assert.NotNull(response.Outputs);
        Assert.NotEmpty(response.Outputs);
    }

    [Theory]
    [Trait("Category", "Integration")]
    [MemberData(nameof(Models))]
    public async Task InvokeMistralModelAsync_ValidConfig_ShouldNotBeNullOrEmpty(Model.Mistral model)
    {
        // Arrange
        const string prompt = "Describe in one sentence what it a large language model";
        var config = new MistralTextGenerationConfig();

        // Act
        var response = await BedrockRuntime.InvokeMistralAsync(model, prompt, config);

        // Assert
        Assert.NotNull(response);
        Assert.NotNull(response.Outputs);
        Assert.NotEmpty(response.Outputs);
    }

    [Theory()]
    [Trait("Category", "Integration")]
    [MemberData(nameof(Models))]
    public async Task InvokeMistralModelWithResponseStreamAsync_ShouldNotBeNullOrEmpty(Model.Mistral model)
    {
        // Arrange
        const string prompt = "Describe in one sentence what it a large language model";

        // Act
        await foreach (var chunk in BedrockRuntime.InvokeMistralWithResponseStreamAsync(model, prompt))
        {
            // Assert
            Assert.NotNull(chunk);
            Assert.NotNull(chunk.Outputs);
            Assert.NotEmpty(chunk.Outputs);
        }
    }

    [Theory()]
    [Trait("Category", "Integration")]
    [MemberData(nameof(Models))]
    public async Task InvokeMistralWithResponseStreamAsync_ValidConfig_ShouldNotBeNullOrEmpty(Model.Mistral model)
    {
        // Arrange
        const string prompt = "Describe in one sentence what it a large language model";
        var config = new MistralTextGenerationConfig();

        // Act
        await foreach (var chunk in BedrockRuntime.InvokeMistralWithResponseStreamAsync(model, prompt, config))
        {
            // Assert
            Assert.NotNull(chunk);
            Assert.NotNull(chunk.Outputs);
            Assert.NotEmpty(chunk.Outputs);
        }
    }
}