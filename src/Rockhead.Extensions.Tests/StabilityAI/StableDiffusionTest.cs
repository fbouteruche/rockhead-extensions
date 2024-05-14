using Amazon;
using Amazon.BedrockRuntime;
using Rockhead.Extensions.StabilityAI;

namespace Rockhead.Extensions.Tests.StabilityAI;

public class StableDiffusionTest
{
    private static readonly AmazonBedrockRuntimeClient BedrockRuntime = new(RegionEndpoint.USEast1);
    
    [Fact()]
    [Trait("Category", "Integration")]
    public async Task InvokeTitanImageGeneratorG1ForTextToImageAsync_ShouldNotBeNullOrEmpty()
    {
        // Arrange
        var model = new Model.StableDiffusionXlV1();
        StableDiffusionTextToImageParams imageParams = new()
        {
            TextPrompts = new []{
                new StableDiffusionTextToImageParams.TextPrompt()
                {
                    Text = "A sunset over the ocean"
                }
            }
        };
        
        // Act
        var response = await BedrockRuntime.InvokeStableDiffusionXlForTextToImageAsync(model, imageParams);
        
        // Assert
        Assert.NotNull(response);
        Assert.NotNull(response.Artifacts);
        Assert.NotEmpty(response.Artifacts);
        foreach (var generatedImage in response.Artifacts)
        {
            Assert.NotNull(generatedImage.Base64);
            Assert.NotEmpty(generatedImage.Base64);
            Assert.NotNull(generatedImage.FinishReason);
            Assert.NotEmpty(generatedImage.FinishReason);   
            Assert.NotNull(generatedImage.Seed); 
        }
    }
}