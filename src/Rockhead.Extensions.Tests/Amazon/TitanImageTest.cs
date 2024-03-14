using Amazon.BedrockRuntime;
using Rockhead.Extensions.Amazon;

namespace Rockhead.Extensions.Tests.Amazon;

public class TitanImageTest
{
    private static readonly AmazonBedrockRuntimeClient BedrockRuntime = new();
    
    [Fact]
    [Trait("Category", "Integration")]
    public async Task InvokeTitanImageGeneratorG1ForTextToImageAsync_ShouldNotBeNullOrEmpty()
    {
        // Arrange
        TitanImageTextToImageParams imageParams = new()
        {
            Text = "A sunset over the ocean"
        };
        
        // Act
        var response = await BedrockRuntime.InvokeTitanImageGeneratorG1ForTextToImageAsync(imageParams);
        
        // Assert
        Assert.NotNull(response);
        Assert.Null(response.Error);
        Assert.NotNull(response.Images);
        Assert.NotEmpty(response.Images);
        Assert.NotNull(response.Images.FirstOrDefault());
        Assert.NotEmpty(response.Images.FirstOrDefault()!);
    }
}