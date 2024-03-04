using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Channels;
using Amazon.BedrockRuntime;
using Amazon.BedrockRuntime.Model;
using Amazon.Runtime.EventStreams;
using Amazon.Util;
using Rockhead.Extensions.AI21Labs;
using Rockhead.Extensions.Amazon;
using Rockhead.Extensions.Anthropic;
using Rockhead.Extensions.Cohere;
using Rockhead.Extensions.Meta;
using Rockhead.Extensions.StabilityAI;

namespace Rockhead.Extensions
{
    public static class AmazonBedrockRuntimeClientExtension
    {
        /// <summary>
        /// Invoke Jurassic 2 model
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="modelId">The Jurassic 2 model Id</param>
        /// <param name="prompt">The prompt</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <returns>The Jurassic 2 model response</returns>
        private static async Task<Jurassic2Response?> InvokeJurassic2Async(this AmazonBedrockRuntimeClient client, string modelId, string prompt, Jurassic2TextGenerationConfig? textGenerationConfig = null)
        {
            if (modelId != ModelIds.AI21_LABS_JURASSIC_V2_MID_V1 && modelId != ModelIds.AI21_LABS_JURASSIC_V2_ULTRA_V1)
            {
                throw new ArgumentException($"modelId is {modelId}, expected {nameof(ModelIds.AI21_LABS_JURASSIC_V2_MID_V1)} or {nameof(ModelIds.AI21_LABS_JURASSIC_V2_ULTRA_V1)}");
            }
            JsonObject? payload = null;
            if (textGenerationConfig != null)
            {
                Validator.ValidateObject(textGenerationConfig, new ValidationContext(textGenerationConfig), true);
                payload = JsonSerializer.SerializeToNode(textGenerationConfig)?.AsObject();
            }

            payload ??= new();
            payload.Add("prompt", prompt);
            
            InvokeModelResponse response = await client.InvokeModelAsync(new InvokeModelRequest()
            {
                ModelId = modelId,
                ContentType = "application/json",
                Accept = "application/json",
                Body = AWSSDKUtils.GenerateMemoryStreamFromString(payload.ToJsonString())
            });

            return JsonSerializer.Deserialize<Jurassic2Response>(response.Body);
        }
        
        /// <summary>
        /// Invoke Jurassic 2 Mid for text completion
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="prompt">The prompt</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <returns>The Jurassic 2 Mid model response</returns>
        public static async Task<Jurassic2Response?> InvokeJurassic2MidAsync(this AmazonBedrockRuntimeClient client, string prompt, Jurassic2TextGenerationConfig? textGenerationConfig = null)
        {
            return await client.InvokeJurassic2Async(ModelIds.AI21_LABS_JURASSIC_V2_MID_V1, prompt, textGenerationConfig);
        }
        
        /// <summary>
        /// Invoke Jurassic 2 Ultra for text completion
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="prompt">The prompt</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <returns>The Jurassic 2 Mid model response</returns>
        public static async Task<Jurassic2Response?> InvokeJurassic2UltraAsync(this AmazonBedrockRuntimeClient client, string prompt, Jurassic2TextGenerationConfig? textGenerationConfig = null)
        {
            return await client.InvokeJurassic2Async(ModelIds.AI21_LABS_JURASSIC_V2_ULTRA_V1, prompt, textGenerationConfig);
        }
        
        /// <summary>
        /// Invoke Titan Image Generator G1 for text to image generation
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="textToImageParams">The text to image prompt definition</param>
        /// <param name="imageGenerationConfig">The image configuration definition. If null, default values will be used</param>
        /// <returns>The Titan Image Generator G1 response</returns>
        public static async Task<TitanImageGeneratorG1Response?> InvokeTitanImageGeneratorG1ForTextToImageAsync(
            this AmazonBedrockRuntimeClient client, TitanImageTextToImageParams textToImageParams, TitanImageGenerationConfig? imageGenerationConfig = null)
        {
            ArgumentNullException.ThrowIfNull(textToImageParams);
            JsonSerializerOptions jsonSerializerOptions = new(JsonSerializerDefaults.Web)
            {
                WriteIndented = true,
                Converters =
                {
                    new JsonStringEnumConverter<TitanImageGenerationConfig.ImageQuality>(JsonNamingPolicy.CamelCase)
                }
            };

            JsonObject payload = new JsonObject()
            {
                ["taskType"] = "TEXT_IMAGE",
                ["textToImageParams"] = JsonSerializer.SerializeToNode(textToImageParams, jsonSerializerOptions)
            };

            if (imageGenerationConfig is not null)
            {
                payload.Add("imageGenerationConfig", JsonSerializer.SerializeToNode(imageGenerationConfig, jsonSerializerOptions));
            }

            InvokeModelResponse response = await client.InvokeModelAsync(new InvokeModelRequest()
            {
                 ModelId = ModelIds.AMAZON_TITAN_IMAGE_GENERATOR_G1_V1,
                 ContentType = "application/json",
                 Accept = "application/json",
                 Body = AWSSDKUtils.GenerateMemoryStreamFromString(payload.ToJsonString())
            });
            
            return JsonSerializer.Deserialize<TitanImageGeneratorG1Response>(response.Body, new JsonSerializerOptions());
        }

        /// <summary>
        /// Invoke Stability AI Stable Diffusion XL v1 for text to image generation
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="textToImageParams">The text to image prompt definition</param>
        /// <returns>The Stability AI Stable Diffusion XL response</returns> 
        public static async Task<StableDiffusionResponse?> InvokeStabilityAIStableDiffusionXLv1ForTextToImageAsync(this AmazonBedrockRuntimeClient client, StableDiffusionTextToImageParams textToImageParams)
        {
            ArgumentNullException.ThrowIfNull(textToImageParams);
            Validator.ValidateObject(textToImageParams, new ValidationContext(textToImageParams), true);
            
            InvokeModelResponse response = await client.InvokeModelAsync(new InvokeModelRequest()
            {
                ModelId = ModelIds.STABILITY_AI_STABLE_DIFFUSION_XL_V0,
                ContentType = "application/json",
                Accept = "application/json",
                Body = new MemoryStream(JsonSerializer.SerializeToUtf8Bytes(textToImageParams))
            });
            
            return JsonSerializer.Deserialize<StableDiffusionResponse>(response.Body, new JsonSerializerOptions());
        }

        /// <summary>
        /// Invoke Titan Text G1 model for text completion
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="modelId">The model Id of the Titan Text G1 model</param>
        /// <param name="inputText">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <returns>The Titan Text G1 model response</returns>
        private static async Task<TitanTextResponse?> InvokeTitanTextG1Async(this AmazonBedrockRuntimeClient client, string modelId, string inputText, TitanTextGenerationConfig? textGenerationConfig = null)
        {
            if (modelId != ModelIds.AMAZON_TITAN_TEXT_LITE_G1_V1 && modelId != ModelIds.AMAZON_TITAN_TEXT_EXPRESS_G1_V1)
            {
                throw new ArgumentException($"modelId is {modelId}, expected {nameof(ModelIds.AMAZON_TITAN_TEXT_LITE_G1_V1)} or {nameof(ModelIds.AMAZON_TITAN_TEXT_EXPRESS_G1_V1)}");
            }
            JsonObject payload = new ()
            {
                ["inputText"] = inputText
            };

            if (textGenerationConfig is not null)
            {
                Validator.ValidateObject(textGenerationConfig, new ValidationContext(textGenerationConfig), true);
                payload.Add("imageGenerationConfig", JsonSerializer.SerializeToNode(textGenerationConfig));
            }

            InvokeModelResponse response = await client.InvokeModelAsync(new InvokeModelRequest()
            {
                ModelId = modelId,
                ContentType = "application/json",
                Accept = "application/json",
                Body = new MemoryStream(JsonSerializer.SerializeToUtf8Bytes(payload))
            });
            
            return JsonSerializer.Deserialize<TitanTextResponse>(response.Body);   
        }
        
        /// <summary>
        /// Invoke Titan Text G1 Express model for text completion
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="inputText">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <returns>The Titan Text G1 Express model response</returns>
        public static async Task<TitanTextResponse?> InvokeTitanTextG1ExpressAsync(this AmazonBedrockRuntimeClient client, string inputText, TitanTextGenerationConfig? textGenerationConfig = null)
        {
            return await client.InvokeTitanTextG1Async(ModelIds.AMAZON_TITAN_TEXT_EXPRESS_G1_V1, inputText, textGenerationConfig);
        }

        /// <summary>
        /// Invoke Titan Text G1 Lite model for text completion
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="inputText">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <returns>The Titan Text G1 Lite model response</returns>
        public static async Task<TitanTextResponse?> InvokeTitanTextG1LiteAsync(this AmazonBedrockRuntimeClient client, string inputText, TitanTextGenerationConfig? textGenerationConfig = null)
        {
            return await client.InvokeTitanTextG1Async(ModelIds.AMAZON_TITAN_TEXT_LITE_G1_V1, inputText, textGenerationConfig);
        }

        /// <summary>
        /// Invoke Titan Text G1 model for text completion with a response stream
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="modelId">The model Id of the Claude model</param>
        /// <param name="inputText">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>An asynchronous enumeration of Titan Text G1 model responses</returns>
        private static async IAsyncEnumerable<TitanTextStreamingResponse> InvokeTitanTextG1WithResponseStreamAsync(this AmazonBedrockRuntimeClient client, string modelId, string inputText, TitanTextGenerationConfig? textGenerationConfig = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            if (modelId != ModelIds.AMAZON_TITAN_TEXT_LITE_G1_V1 && modelId != ModelIds.AMAZON_TITAN_TEXT_EXPRESS_G1_V1)
            {
                throw new ArgumentException($"modelId is {modelId}, expected {nameof(ModelIds.AMAZON_TITAN_TEXT_LITE_G1_V1)} or {nameof(ModelIds.AMAZON_TITAN_TEXT_EXPRESS_G1_V1)}");
            }
            JsonObject payload = new ()
            {
                ["inputText"] = inputText
            };

            if (textGenerationConfig is not null)
            {
                Validator.ValidateObject(textGenerationConfig, new ValidationContext(textGenerationConfig), true);
                payload.Add("imageGenerationConfig", JsonSerializer.SerializeToNode(textGenerationConfig));
            }

            InvokeModelWithResponseStreamResponse response = await client.InvokeModelWithResponseStreamAsync(new InvokeModelWithResponseStreamRequest()
            {
                ModelId = modelId,
                ContentType = "application/json",
                Accept = "application/json",
                Body = AWSSDKUtils.GenerateMemoryStreamFromString(payload.ToJsonString())
            }, cancellationToken);

            Channel<TitanTextStreamingResponse> buffer = Channel.CreateUnbounded<TitanTextStreamingResponse>();
            bool isStreaming = true;

            response.Body.ChunkReceived += BodyOnChunkReceived;
            response.Body.StartProcessing();

            while ((!cancellationToken.IsCancellationRequested && isStreaming) || (!cancellationToken.IsCancellationRequested && buffer.Reader.Count  > 0))
            {
                yield return await buffer.Reader.ReadAsync(cancellationToken);
            }
            response.Body.ChunkReceived -= BodyOnChunkReceived;
            
            yield break;
            
            async void BodyOnChunkReceived(object? sender, EventStreamEventReceivedArgs<PayloadPart> e)
            {
                var temp = await new StreamReader(e.EventStreamEvent.Bytes).ReadToEndAsync();
                e.EventStreamEvent.Bytes.Position = 0;
                
                var streamResponse = JsonSerializer.Deserialize<TitanTextStreamingResponse>(e.EventStreamEvent.Bytes);

                if (streamResponse is null)
                {
                    throw new NullReferenceException($"Unable to deserialize {nameof(e.EventStreamEvent.Bytes)} to {nameof(TitanTextStreamingResponse)}");
                }
                
                if (streamResponse.CompletionReason != null)
                {
                    isStreaming = false;
                }

                await buffer.Writer.WriteAsync(streamResponse, cancellationToken);
            }
        }

        /// <summary>
        /// Invoke Titan Text G1 Express model for text completion with a response stream
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="inputText">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>An asynchronous enumeration of Claude model responses</returns>
        public static IAsyncEnumerable<TitanTextStreamingResponse> InvokeTitanTextG1ExpressWithResponseStreamAsync(this AmazonBedrockRuntimeClient client, string inputText, TitanTextGenerationConfig? textGenerationConfig = null, CancellationToken cancellationToken = default)
        {
            return client.InvokeTitanTextG1WithResponseStreamAsync(ModelIds.AMAZON_TITAN_TEXT_EXPRESS_G1_V1, inputText,
                textGenerationConfig, cancellationToken);
        }
        
        /// <summary>
        /// Invoke Titan Text G1 Lite model for text completion with a response stream
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="inputText">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>An asynchronous enumeration of Claude model responses</returns>
        public static IAsyncEnumerable<TitanTextStreamingResponse> InvokeTitanTextG1LiteWithResponseStreamAsync(this AmazonBedrockRuntimeClient client, string inputText, TitanTextGenerationConfig? textGenerationConfig = null, CancellationToken cancellationToken = default)
        {
            return client.InvokeTitanTextG1WithResponseStreamAsync(ModelIds.AMAZON_TITAN_TEXT_LITE_G1_V1, inputText, textGenerationConfig, cancellationToken);
        }
        
        /// <summary>
        /// Invoke Titan Embeddings G1 Text model for embedding generation
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="inputText">The input text describing the input image</param>
        /// <returns>The Titan Embeddings G1 Text model response</returns>
        public static async Task<TitanEmbeddingsResponse?> InvokeTitanEmbeddingsG1TextAsync(this AmazonBedrockRuntimeClient client, string inputText)
        {
            JsonObject payload = new ()
            {
                ["inputText"] = inputText
            };

            InvokeModelResponse response = await client.InvokeModelAsync(new InvokeModelRequest()
            {
                ModelId = ModelIds.AMAZON_TITAN_EMBEDDING_TEXT_G1_V1,
                ContentType = "application/json",
                Accept = "application/json",
                Body = new MemoryStream(JsonSerializer.SerializeToUtf8Bytes(payload))
            });
            
            return JsonSerializer.Deserialize<TitanEmbeddingsResponse>(response.Body);   
        }

        /// <summary>
        /// Invoke Titan Multimodal Embeddings G1 model for embedding generation
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="inputText">The input text describing the input image</param>
        /// <param name="inputImage">The input image to transform into embedding</param>
        /// <param name="embeddingConfig">The embedding generation configuration</param>
        /// <returns>The Titan Multimodal Embeddings G1 model response</returns>
        public static async Task<TitanEmbeddingsResponse?> InvokeTitanMultimodalEmbeddingsG1Async(this AmazonBedrockRuntimeClient client, string inputText, string inputImage, TitanMultimodalEmbeddingConfig? embeddingConfig = null)
        {
            JsonObject payload = new ()
            {
                ["inputText"] = inputText,
                ["inputImage"] = inputImage
            };

            if (embeddingConfig is not null)
            {
                Validator.ValidateObject(embeddingConfig, new ValidationContext(embeddingConfig), true);
                payload.Add("embeddingConfig", JsonSerializer.SerializeToNode(embeddingConfig));
            }

            InvokeModelResponse response = await client.InvokeModelAsync(new InvokeModelRequest()
            {
                ModelId = ModelIds.AMAZON_TITAN_EMBEDDING_IMAGE_G1_V1,
                ContentType = "application/json",
                Accept = "application/json",
                Body = new MemoryStream(JsonSerializer.SerializeToUtf8Bytes(payload))
            });
            
            return JsonSerializer.Deserialize<TitanEmbeddingsResponse>(response.Body);   
        }

        /// <summary>
        /// Invoke a Claude model for text completion
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="modelId">The model Id of the Claude model</param>
        /// <param name="prompt">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <returns>The Claude model response</returns>
        private static async Task<ClaudeResponse?> InvokeClaudeAsync(this AmazonBedrockRuntimeClient client, string modelId, string prompt, ClaudeTextGenerationConfig? textGenerationConfig = null)
        {
            if (modelId != ModelIds.ANTHROPIC_CLAUDE_V1 && modelId != ModelIds.ANTHROPIC_CLAUDE_V2 && modelId != ModelIds.ANTHROPIC_CLAUDE_V2_1 && modelId != ModelIds.ANTHROPIC_CLAUDE_INSTANT_V1 )
            {
                throw new ArgumentException($"modelId is {modelId}, expected {nameof(ModelIds.ANTHROPIC_CLAUDE_V1)} or {nameof(ModelIds.ANTHROPIC_CLAUDE_V2)} or {nameof(ModelIds.ANTHROPIC_CLAUDE_V2_1)} or {nameof(ModelIds.ANTHROPIC_CLAUDE_INSTANT_V1)}");
            }
            if (textGenerationConfig != null)
            {
                Validator.ValidateObject(textGenerationConfig, new ValidationContext(textGenerationConfig), true);
            }
            else
            {
                textGenerationConfig = new ClaudeTextGenerationConfig();
            }

            JsonObject payload = JsonSerializer.SerializeToNode(textGenerationConfig)?.AsObject() ?? new ();
            payload.Add("prompt", prompt);
            
            InvokeModelResponse response = await client.InvokeModelAsync(new InvokeModelRequest()
            {
                ModelId = modelId,
                ContentType = "application/json",
                Accept = "application/json",
                Body = AWSSDKUtils.GenerateMemoryStreamFromString(payload.ToJsonString())
            });

            return JsonSerializer.Deserialize<ClaudeResponse>(response.Body);
        }

        /// <summary>
        /// Invoke Claude Instant V1 model for text completion
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="prompt">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <returns>The Claude model response</returns>
        public static async Task<ClaudeResponse?> InvokeClaudeInstantV1Async(this AmazonBedrockRuntimeClient client, string prompt, ClaudeTextGenerationConfig? textGenerationConfig = null)
        {
            return await client.InvokeClaudeAsync(ModelIds.ANTHROPIC_CLAUDE_INSTANT_V1, prompt, textGenerationConfig);
        }
        
        /// <summary>
        /// Invoke Claude V2 model for text completion
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="prompt">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <returns>The Claude model response</returns>
        public static async Task<ClaudeResponse?> InvokeClaudeV2Async(this AmazonBedrockRuntimeClient client, string prompt, ClaudeTextGenerationConfig? textGenerationConfig = null)
        {
            return await client.InvokeClaudeAsync(ModelIds.ANTHROPIC_CLAUDE_V2, prompt, textGenerationConfig);
        }
        
        /// <summary>
        /// Invoke Claude V2.1 model for text completion
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="prompt">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <returns>The Claude model response</returns>
        public static async Task<ClaudeResponse?> InvokeClaudeV2_1Async(this AmazonBedrockRuntimeClient client, string prompt, ClaudeTextGenerationConfig? textGenerationConfig = null)
        {
            return await client.InvokeClaudeAsync(ModelIds.ANTHROPIC_CLAUDE_V2_1, prompt, textGenerationConfig);
        }

        /// <summary>
        /// Invoke a Claude model for text completion with a response stream
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="modelId">The model Id of the Claude model</param>
        /// <param name="prompt">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>An asynchronous enumeration of Claude model responses</returns>
        private static async IAsyncEnumerable<ClaudeResponse> InvokeClaudeWithResponseStreamAsync(this AmazonBedrockRuntimeClient client, string modelId, string prompt, ClaudeTextGenerationConfig? textGenerationConfig = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            if (modelId != ModelIds.ANTHROPIC_CLAUDE_V1 && modelId != ModelIds.ANTHROPIC_CLAUDE_V2 && modelId != ModelIds.ANTHROPIC_CLAUDE_V2_1 && modelId != ModelIds.ANTHROPIC_CLAUDE_INSTANT_V1 )
            {
                throw new ArgumentException($"modelId is {modelId}, expected {nameof(ModelIds.ANTHROPIC_CLAUDE_V1)} or {nameof(ModelIds.ANTHROPIC_CLAUDE_V2)} or {nameof(ModelIds.ANTHROPIC_CLAUDE_V2_1)} or {nameof(ModelIds.ANTHROPIC_CLAUDE_INSTANT_V1)}");
            }
            
            if (textGenerationConfig != null)
            {
                Validator.ValidateObject(textGenerationConfig, new ValidationContext(textGenerationConfig), true);
            }
            else
            {
                textGenerationConfig = new ClaudeTextGenerationConfig();
            }

            JsonObject payload = JsonSerializer.SerializeToNode(textGenerationConfig)?.AsObject() ?? new ();
            payload.Add("prompt", prompt);
            
            InvokeModelWithResponseStreamResponse response = await client.InvokeModelWithResponseStreamAsync(new InvokeModelWithResponseStreamRequest()
            {
                ModelId = modelId,
                ContentType = "application/json",
                Accept = "application/json",
                Body = AWSSDKUtils.GenerateMemoryStreamFromString(payload.ToJsonString())
            }, cancellationToken);

            Channel<ClaudeResponse> buffer = Channel.CreateUnbounded<ClaudeResponse>();
            bool isStreaming = true;

            response.Body.ChunkReceived += BodyOnChunkReceived;
            response.Body.StartProcessing();

            while ((!cancellationToken.IsCancellationRequested && isStreaming) || (!cancellationToken.IsCancellationRequested && buffer.Reader.Count  > 0))
            {
                yield return await buffer.Reader.ReadAsync(cancellationToken);
            }
            response.Body.ChunkReceived -= BodyOnChunkReceived;
            
            yield break;
            
            async void BodyOnChunkReceived(object? sender, EventStreamEventReceivedArgs<PayloadPart> e)
            {
                var streamResponse = JsonSerializer.Deserialize<ClaudeResponse>(e.EventStreamEvent.Bytes);

                if (streamResponse is null)
                {
                    throw new NullReferenceException($"Unable to deserialize {nameof(e.EventStreamEvent.Bytes)} to {nameof(ClaudeResponse)}");
                }
                
                if (streamResponse.StopReason != null)
                {
                    isStreaming = false;
                }

                await buffer.Writer.WriteAsync(streamResponse, cancellationToken);
            }
        }

        /// <summary>
        /// Invoke Claude Instant V1 model for text completion with a response stream
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="prompt">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>An asynchronous enumeration of Claude model responses</returns>
        public static IAsyncEnumerable<ClaudeResponse> InvokeClaudeInstantV1WithResponseStreamAsync(this AmazonBedrockRuntimeClient client, string prompt, ClaudeTextGenerationConfig? textGenerationConfig = null, CancellationToken cancellationToken = default)
        {
            return client.InvokeClaudeWithResponseStreamAsync(ModelIds.ANTHROPIC_CLAUDE_INSTANT_V1, prompt, textGenerationConfig, cancellationToken);
        }

        /// <summary>
        /// Invoke Claude V2 model for text completion with a response stream
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="prompt">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>An asynchronous enumeration of Claude model responses</returns>
        public static IAsyncEnumerable<ClaudeResponse> InvokeClaudeV2WithResponseStreamAsync(this AmazonBedrockRuntimeClient client, string prompt, ClaudeTextGenerationConfig? textGenerationConfig = null, CancellationToken cancellationToken = default)
        {
            return client.InvokeClaudeWithResponseStreamAsync(ModelIds.ANTHROPIC_CLAUDE_V2, prompt, textGenerationConfig, cancellationToken);
        }

        /// <summary>
        /// Invoke Claude V2.1 model for text completion with a response stream
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="prompt">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>An asynchronous enumeration of Claude model responses</returns>
        public static IAsyncEnumerable<ClaudeResponse> InvokeClaudeV2_1WithResponseStreamAsync(this AmazonBedrockRuntimeClient client, string prompt, ClaudeTextGenerationConfig? textGenerationConfig = null, CancellationToken cancellationToken = default)
        {
            return client.InvokeClaudeWithResponseStreamAsync(ModelIds.ANTHROPIC_CLAUDE_V2_1, prompt, textGenerationConfig, cancellationToken);
        }
        
        /// <summary>
        /// Invoke Command v14 model for text completion
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="modelId">The model Id of the Command model</param>
        /// <param name="prompt">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <returns>The Command model response</returns>
        private static async Task<CommandResponse?> InvokeCommandV14Async(this AmazonBedrockRuntimeClient client, string modelId, string prompt, CommandTextGenerationConfig? textGenerationConfig = null)
        {
            if (modelId != ModelIds.COHERE_COMMAND_TEXT_V14 && modelId != ModelIds.COHERE_COMMAND_TEXT_LIGHT_V14)
            {
                throw new ArgumentException($"modelId is {modelId}, expected {nameof(ModelIds.COHERE_COMMAND_TEXT_V14)} or {nameof(ModelIds.COHERE_COMMAND_TEXT_LIGHT_V14)}");
            }
            JsonObject? payload = null;
            if (textGenerationConfig != null)
            {
                Validator.ValidateObject(textGenerationConfig, new ValidationContext(textGenerationConfig), true);
                payload = JsonSerializer.SerializeToNode(textGenerationConfig)?.AsObject();
            }

            payload??= new JsonObject();
            payload.Add("prompt", prompt);
            
            InvokeModelResponse response = await client.InvokeModelAsync(new InvokeModelRequest()
            {
                ModelId = modelId,
                ContentType = "application/json",
                Accept = "application/json",
                Body = AWSSDKUtils.GenerateMemoryStreamFromString(payload.ToJsonString())
            });

            return JsonSerializer.Deserialize<CommandResponse>(response.Body);
        }
        
        /// <summary>
        /// Invoke Command v14 model for text completion
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="prompt">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <returns>The Command model response</returns>
        public static async Task<CommandResponse?> InvokeCommandV14Async(this AmazonBedrockRuntimeClient client, string prompt, CommandTextGenerationConfig? textGenerationConfig = null)
        {
            return await client.InvokeCommandV14Async(ModelIds.COHERE_COMMAND_TEXT_V14, prompt, textGenerationConfig);
        }
        
        /// <summary>
        /// Invoke Command Light v14 model for text completion
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="prompt">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <returns>The Command model response</returns>
        public static async Task<CommandResponse?> InvokeCommandLightV14Async(this AmazonBedrockRuntimeClient client, string prompt, CommandTextGenerationConfig? textGenerationConfig = null)
        {
            return await client.InvokeCommandV14Async(ModelIds.COHERE_COMMAND_TEXT_LIGHT_V14, prompt, textGenerationConfig);
        }

        /// <summary>
        /// Invoke Embed V3 model for embedding generation
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="modelId">The model Id of the Embed model</param>
        /// <param name="texts">The text to transform into embedding</param>
        /// <param name="embeddingGenerationConfig">The embedding generation configuration</param>
        /// <returns>The Embed model response</returns>
        private static async Task<EmbedResponse?> InvokeEmbedV3Async(this AmazonBedrockRuntimeClient client, string modelId, IEnumerable<string> texts, EmbedEmbeddingGenerationConfig? embeddingGenerationConfig = null)
        {
            embeddingGenerationConfig ??= new EmbedEmbeddingGenerationConfig();
            Validator.ValidateObject(embeddingGenerationConfig, new ValidationContext(embeddingGenerationConfig), true);
            
            JsonObject payload = JsonSerializer.SerializeToNode(embeddingGenerationConfig)?.AsObject() ?? new ();
            payload.Add("texts", JsonSerializer.SerializeToNode(texts));
            
            InvokeModelResponse response = await client.InvokeModelAsync(new InvokeModelRequest()
            {
                ModelId = modelId,
                ContentType = "application/json",
                Accept = "application/json",
                Body = AWSSDKUtils.GenerateMemoryStreamFromString(payload.ToJsonString())
            });

            return JsonSerializer.Deserialize<EmbedResponse>(response.Body);
        }
        
        /// <summary>
        /// Invoke Embed English V3 model for embedding generation
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="texts">The text to transform into embedding</param>
        /// <param name="embeddingGenerationConfig">The embedding generation configuration</param>
        /// <returns>The Embed model response</returns>
        public static async Task<EmbedResponse?> InvokeEmbedEnglishV3Async(this AmazonBedrockRuntimeClient client, IEnumerable<string> texts, EmbedEmbeddingGenerationConfig? embeddingGenerationConfig = null)
        {
            return await InvokeEmbedV3Async(client, ModelIds.COHERE_EMBED_ENGLISH_V3, texts, embeddingGenerationConfig);
        }
        
        /// <summary>
        /// Invoke Embed Multilingual V3 model for embedding generation
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="texts">The text to transform into embedding</param>
        /// <param name="embeddingGenerationConfig">The embedding generation configuration</param>
        /// <returns>The Embed model response</returns>
        public static async Task<EmbedResponse?> InvokeEmbedMultilingualV3Async(this AmazonBedrockRuntimeClient client, IEnumerable<string> texts, EmbedEmbeddingGenerationConfig? embeddingGenerationConfig = null)
        {
            return await InvokeEmbedV3Async(client, ModelIds.COHERE_EMBED_MULTILINGUAL_V3, texts, embeddingGenerationConfig);
        }

        /// <summary>
        /// Invoke Command v14 model for text completion with a response stream
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="modelId">The model Id of the Command model</param>
        /// <param name="prompt">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>An asynchronous enumeration of Command model responses</returns>
        private static async IAsyncEnumerable<CommandStreamingResponse> InvokeCommandV14WithResponseStreamAsync(this AmazonBedrockRuntimeClient client, string modelId, string prompt, CommandTextGenerationConfig? textGenerationConfig = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            if (modelId != ModelIds.COHERE_COMMAND_TEXT_V14 && modelId != ModelIds.COHERE_COMMAND_TEXT_LIGHT_V14)
            {
                throw new ArgumentException($"modelId is {modelId}, expected {nameof(ModelIds.COHERE_COMMAND_TEXT_V14)} or {nameof(ModelIds.COHERE_COMMAND_TEXT_LIGHT_V14)}");
            }
            JsonObject? payload = null;
            textGenerationConfig ??= new CommandTextGenerationConfig();
            // We want to force streaming. The default is false. If we don't force the value to True, Cohere model won't stream even if we call the InvokeModelWithResponseStream API action.
            textGenerationConfig.Stream = true;
            
            Validator.ValidateObject(textGenerationConfig, new ValidationContext(textGenerationConfig), true);
            payload = JsonSerializer.SerializeToNode(textGenerationConfig)?.AsObject();
            
            payload??= new JsonObject();
            payload.Add("prompt", prompt);
            
            InvokeModelWithResponseStreamResponse response = await client.InvokeModelWithResponseStreamAsync(new InvokeModelWithResponseStreamRequest()
            {
                ModelId = modelId,
                ContentType = "application/json",
                Accept = "application/json",
                Body = AWSSDKUtils.GenerateMemoryStreamFromString(payload.ToJsonString())
            }, cancellationToken);

            Channel<CommandStreamingResponse> buffer = Channel.CreateUnbounded<CommandStreamingResponse>();
            bool isStreaming = true;

            response.Body.ChunkReceived += BodyOnChunkReceived;
            response.Body.StartProcessing();

            while ((!cancellationToken.IsCancellationRequested && isStreaming) || (!cancellationToken.IsCancellationRequested && buffer.Reader.Count  > 0))
            {
                yield return await buffer.Reader.ReadAsync(cancellationToken);
            }
            response.Body.ChunkReceived -= BodyOnChunkReceived;
            
            yield break;
            
            async void BodyOnChunkReceived(object? sender, EventStreamEventReceivedArgs<PayloadPart> e)
            {
                var streamResponse = JsonSerializer.Deserialize<CommandStreamingResponse>(e.EventStreamEvent.Bytes);

                if (streamResponse is null)
                {
                    throw new NullReferenceException($"Unable to deserialize {nameof(e.EventStreamEvent.Bytes)} to {nameof(CommandStreamingResponse)}");
                }

                isStreaming = streamResponse.IsFinished ?? false;
                
                await buffer.Writer.WriteAsync(streamResponse, cancellationToken);
            }
        }
        
        /// <summary>
        /// Invoke Command v14 model for text completion with a response stream
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="prompt">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>An asynchronous enumeration of Command model responses</returns>
        public static IAsyncEnumerable<CommandStreamingResponse> InvokeCommandV14WithResponseStreamAsync(this AmazonBedrockRuntimeClient client, string prompt, CommandTextGenerationConfig? textGenerationConfig = null, CancellationToken cancellationToken = default)
        {
            return client.InvokeCommandV14WithResponseStreamAsync(ModelIds.COHERE_COMMAND_TEXT_V14, prompt, textGenerationConfig, cancellationToken);
        }
        
        /// <summary>
        /// Invoke Command Light v14 model for text completion with a response stream
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="prompt">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>An asynchronous enumeration of Command model responses</returns>
        public static IAsyncEnumerable<CommandStreamingResponse> InvokeCommandLightV14WithResponseStreamAsync(this AmazonBedrockRuntimeClient client, string prompt, CommandTextGenerationConfig? textGenerationConfig = null, CancellationToken cancellationToken = default)
        {
            return client.InvokeCommandV14WithResponseStreamAsync(ModelIds.COHERE_COMMAND_TEXT_LIGHT_V14, prompt, textGenerationConfig, cancellationToken);
        }

        /// <summary>
        /// Invoke Llama 2 models for text completion
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="modelId">The model Id of the Llama 2 model</param>
        /// <param name="prompt">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <returns>The Llama 2 model response</returns>
        private static async Task<Llama2Response?> InvokeLlama2Async(this AmazonBedrockRuntimeClient client, string modelId, string prompt, Llama2TextGenerationConfig? textGenerationConfig = null)
        {
            if (modelId != ModelIds.META_LLAMA2_13B_CHAT_V1 && modelId != ModelIds.META_LLAMA2_70B_CHAT_V1 && modelId != ModelIds.META_LLAMA2_13B_V1 && modelId != ModelIds.META_LLAMA2_70B_V1)
            {
                throw new ArgumentException($"modelId is {modelId}, expected {nameof(ModelIds.META_LLAMA2_13B_CHAT_V1)} or {nameof(ModelIds.META_LLAMA2_70B_CHAT_V1)} or {nameof(ModelIds.META_LLAMA2_13B_V1)} or {nameof(ModelIds.META_LLAMA2_70B_V1)}");
            }
            JsonObject? payload = null;
            if (textGenerationConfig != null)
            {
                Validator.ValidateObject(textGenerationConfig, new ValidationContext(textGenerationConfig), true);
                payload = JsonSerializer.SerializeToNode(textGenerationConfig)?.AsObject();
            }

            payload??= new JsonObject();
            payload.Add("prompt", prompt);
            
            InvokeModelResponse response = await client.InvokeModelAsync(new InvokeModelRequest()
            {
                ModelId = modelId,
                ContentType = "application/json",
                Accept = "application/json",
                Body = AWSSDKUtils.GenerateMemoryStreamFromString(payload.ToJsonString())
            });

            return JsonSerializer.Deserialize<Llama2Response>(response.Body);
        }
        
        /// <summary>
        /// Invoke Llama 2 Chat 13B model for text completion
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="prompt">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <returns>The Llama 2 Chat 13B model response</returns>
        public static async Task<Llama2Response?> InvokeLlama213BChatV1Async(this AmazonBedrockRuntimeClient client, string prompt, Llama2TextGenerationConfig? textGenerationConfig = null)
        {
            return await client.InvokeLlama2Async(ModelIds.META_LLAMA2_13B_CHAT_V1, prompt, textGenerationConfig);
        }
        
        /// <summary>
        /// Invoke Llama 2 Chat 70B model for text completion
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="prompt">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <returns>The Llama 2 Chat 70B model response</returns>
        public static async Task<Llama2Response?> InvokeLlama270BChatV1Async(this AmazonBedrockRuntimeClient client, string prompt, Llama2TextGenerationConfig? textGenerationConfig = null)
        {
            return await client.InvokeLlama2Async(ModelIds.META_LLAMA2_70B_CHAT_V1, prompt, textGenerationConfig);
        }

        /// <summary>
        /// Invoke Llama 2 models for text completion with a response stream
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="modelId">The model Id of the Llama 2 model</param>
        /// <param name="prompt">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>An asynchronous enumeration of Llama 2 model responses</returns>
        private static async IAsyncEnumerable<Llama2Response> InvokeLlama2WithResponseStreamAsync(this AmazonBedrockRuntimeClient client, string modelId, string prompt, Llama2TextGenerationConfig? textGenerationConfig = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            if (modelId != ModelIds.META_LLAMA2_13B_CHAT_V1 && modelId != ModelIds.META_LLAMA2_70B_CHAT_V1)
            {
                throw new ArgumentException($"modelId is {modelId}, expected {nameof(ModelIds.META_LLAMA2_13B_CHAT_V1)} or {nameof(ModelIds.META_LLAMA2_70B_CHAT_V1)}");
            }
            JsonObject? payload = null;
            if (textGenerationConfig != null)
            {
                Validator.ValidateObject(textGenerationConfig, new ValidationContext(textGenerationConfig), true);
                payload = JsonSerializer.SerializeToNode(textGenerationConfig)?.AsObject();
            }

            payload??= new JsonObject();
            payload.Add("prompt", prompt);
            
            InvokeModelWithResponseStreamResponse response = await client.InvokeModelWithResponseStreamAsync(new InvokeModelWithResponseStreamRequest()
            {
                ModelId = modelId,
                ContentType = "application/json",
                Accept = "application/json",
                Body = AWSSDKUtils.GenerateMemoryStreamFromString(payload.ToJsonString())
            }, cancellationToken);

            Channel<Llama2Response> buffer = Channel.CreateUnbounded<Llama2Response>();
            bool isStreaming = true;

            response.Body.ChunkReceived += BodyOnChunkReceived;
            response.Body.StartProcessing();

            while ((!cancellationToken.IsCancellationRequested && isStreaming) || (!cancellationToken.IsCancellationRequested && buffer.Reader.Count  > 0))
            {
                yield return await buffer.Reader.ReadAsync(cancellationToken);
            }
            response.Body.ChunkReceived -= BodyOnChunkReceived;
            
            yield break;
            
            async void BodyOnChunkReceived(object? sender, EventStreamEventReceivedArgs<PayloadPart> e)
            {
                var temp = await new StreamReader(e.EventStreamEvent.Bytes).ReadToEndAsync();
                e.EventStreamEvent.Bytes.Position = 0;
                var streamResponse = JsonSerializer.Deserialize<Llama2Response>(e.EventStreamEvent.Bytes);

                if (streamResponse is null)
                {
                    throw new NullReferenceException($"Unable to deserialize {nameof(e.EventStreamEvent.Bytes)} to {nameof(CommandStreamingResponse)}");
                }

                if (streamResponse.GetStopReason() != null)
                {
                    isStreaming = false;
                }
                
                await buffer.Writer.WriteAsync(streamResponse, cancellationToken);
            }
        }

        /// <summary>
        /// Invoke Llama 2 Chat 13B model for text completion with a response stream
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="prompt">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>An asynchronous enumeration of Llama 2 model responses</returns>
        public static IAsyncEnumerable<Llama2Response> InvokeLlama213BChatV1WithResponseStreamAsync(this AmazonBedrockRuntimeClient client, string prompt, Llama2TextGenerationConfig? textGenerationConfig = default, CancellationToken cancellationToken = default)
        {
            return client.InvokeLlama2WithResponseStreamAsync(ModelIds.META_LLAMA2_13B_CHAT_V1, prompt, textGenerationConfig, cancellationToken);
        }
        
        /// <summary>
        /// Invoke Llama 2 Chat 70B model for text completion with a response stream
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="prompt">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>An asynchronous enumeration of Llama 2 model responses</returns>
        public static IAsyncEnumerable<Llama2Response> InvokeLlama270BChatV1WithResponseStreamAsync(this AmazonBedrockRuntimeClient client, string prompt, Llama2TextGenerationConfig? textGenerationConfig = default, CancellationToken cancellationToken = default)
        {
            return client.InvokeLlama2WithResponseStreamAsync(ModelIds.META_LLAMA2_70B_CHAT_V1, prompt, textGenerationConfig, cancellationToken);
        }
    }
}
