using Amazon;
using Amazon.BedrockRuntime;
using Rockhead.Extensions.AI21Labs;

namespace Rockhead.Extensions.Tests.AI21Labs;

public class Jurassic2Test
{
    private readonly AmazonBedrockRuntimeClient _bedrockRuntime = new(RegionEndpoint.USEast1);
    public static TheoryData<Model.Jurassic2> Models =>
        new TheoryData<Model.Jurassic2>
        {
            new Model.Jurassic2MidV1(),
            new Model.Jurassic2UltraV1()
        };

    [Theory]
    [Trait("Category", "Integration")]
    [MemberData(nameof(Models))]
    public async Task InvokeJurassic2Async_ShouldNotBeNullOrEmpty(Model.Jurassic2 model)
    {
        // Arrange
        const string prompt = "Describe in one sentence what it a large language model";
        
        // Act
        var response = await _bedrockRuntime.InvokeJurassic2Async(model, prompt);
        
        // Assert
        Assert.NotNull(response);
        Assert.NotNull(response.Completions);
        Assert.NotEmpty(response.Completions);
        Assert.NotNull(response.Completions.FirstOrDefault());
        Assert.NotNull(response.Completions.FirstOrDefault()!.Data);
        Assert.NotEmpty(response.Completions.FirstOrDefault()!.Data!.Text!);
        
    }
    
    [Theory]
    [Trait("Category", "Integration")]
    [MemberData(nameof(Models))]
    public async Task InvokeJurassic2Async_ValidConfig_ShouldNotBeNullOrEmpty(Model.Jurassic2 model)
    {
        // Arrange
        const string prompt = "Describe in one sentence what it a large language model";
        var config = new Jurassic2TextGenerationConfig();
        
        // Act
        var response = await _bedrockRuntime.InvokeJurassic2Async(model, prompt, config);
        
        // Assert
        Assert.NotNull(response);
        Assert.NotNull(response.Completions);
        Assert.NotEmpty(response.Completions);
        Assert.NotNull(response.Completions.FirstOrDefault());
        Assert.NotNull(response.Completions.FirstOrDefault()!.Data);
        Assert.NotEmpty(response.Completions.FirstOrDefault()!.Data!.Text!);
    }

}