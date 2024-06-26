using Amazon;
using Amazon.BedrockRuntime;

namespace Rockhead.Extensions.Tests.Amazon;

public class TitanEmbeddingsTest
{
    private static readonly AmazonBedrockRuntimeClient BedrockRuntime = new(RegionEndpoint.USEast1);
    
    [Fact]
    [Trait("Category", "Integration")]
    public async Task InvokeTitanEmbeddingsG1TextAsync_ShouldNotBeNullOrEmpty()
    {
        // Arrange
        const string prompt = "Describe in one sentence what it a large language model";
        
        // Act
        var response = await BedrockRuntime.InvokeTitanEmbeddingsG1TextAsync(prompt);
        
        // Assert
        Assert.NotNull(response);
        Assert.NotNull(response.Embeddings);
        Assert.NotEmpty(response.Embeddings);
    }
}