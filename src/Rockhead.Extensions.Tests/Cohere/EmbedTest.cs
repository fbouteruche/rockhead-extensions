using Amazon;
using Amazon.BedrockRuntime;
using Rockhead.Extensions.Cohere;

namespace Rockhead.Extensions.Tests.Cohere;

public class EmbedTest
{
    private static readonly AmazonBedrockRuntimeClient BedrockRuntime = new(RegionEndpoint.USEast1);
    public static TheoryData<Model.Embed> Models =>
        new TheoryData<Model.Embed>
        {
            new Model.EmbedEnglishV3(),
            new Model.EmbedMultilingualV3()
        };
    
    [Theory]
    [Trait("Category", "Integration")]
    [MemberData(nameof(Models))]
    public async Task InvokeEmbedV3Async_ShouldNotBeNullOrEmpty(Model.Embed model)
    {
        // Arrange
        const string prompt = "Describe in one sentence what it a large language model";
        
        // Act
        var response = await BedrockRuntime.InvokeEmbedV3Async(model, new []{ prompt });
        
        // Assert
        Assert.NotNull(response);
        Assert.NotNull(response.Id);
        Assert.NotEmpty(response.Id);
        Assert.NotNull(response.ResponseType);
        Assert.NotEmpty(response.ResponseType);
        Assert.NotNull(response.Embeddings);
        Assert.NotEmpty(response.Embeddings!);
        foreach (var embeddings in response.Embeddings)
        {
            Assert.NotNull(embeddings);
            Assert.NotEmpty(embeddings);
        }
        Assert.NotNull(response.Texts);
        Assert.NotEmpty(response.Texts!);
        foreach (var text in response.Texts)
        {
            Assert.NotNull(text);
            Assert.NotEmpty(text);
        }
    }
    
    [Theory]
    [Trait("Category", "Integration")]
    [MemberData(nameof(Models))]
    public async Task InvokeEmbedV3Async_ValidConfig_ShouldNotBeNullOrEmpty(Model.Embed model)
    {
        // Arrange
        const string prompt = "Describe in one sentence what it a large language model";
        var config = new EmbedEmbeddingGenerationConfig();
        
        // Act
        var response = await BedrockRuntime.InvokeEmbedV3Async(model, new []{ prompt }, config);
        
        // Assert
        Assert.NotNull(response);
        Assert.NotNull(response.Id);
        Assert.NotEmpty(response.Id);
        Assert.NotNull(response.ResponseType);
        Assert.NotEmpty(response.ResponseType);
        Assert.NotNull(response.Embeddings);
        Assert.NotEmpty(response.Embeddings!);
        foreach (var embeddings in response.Embeddings)
        {
            Assert.NotNull(embeddings);
            Assert.NotEmpty(embeddings);
        }
        Assert.NotNull(response.Texts);
        Assert.NotEmpty(response.Texts!);
        foreach (var text in response.Texts)
        {
            Assert.NotNull(text);
            Assert.NotEmpty(text);
        }
    }
}