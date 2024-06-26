﻿using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
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
using Rockhead.Extensions.MistralAI;
using Rockhead.Extensions.StabilityAI;

namespace Rockhead.Extensions
{
    public static class AmazonBedrockRuntimeClientExtension
    {
        /// <summary>
        /// Invoke a Jurassic 2 model (Mid or Ultra)
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="model">The Jurassic 2 model to invoke</param>
        /// <param name="prompt">The prompt</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>The Jurassic 2 model response</returns>
        public static async Task<Jurassic2Response?> InvokeJurassic2Async(this AmazonBedrockRuntimeClient client, Model.Jurassic2 model, string prompt, Jurassic2TextGenerationConfig? textGenerationConfig = null, CancellationToken cancellationToken = default)
        {
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
                    ModelId = model.ModelId,
                    ContentType = "application/json",
                    Accept = "application/json",
                    Body = AWSSDKUtils.GenerateMemoryStreamFromString(payload.ToJsonString())
                },
                cancellationToken).ConfigureAwait(false);
        
            return await JsonSerializer.DeserializeAsync<Jurassic2Response>(response.Body, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Invoke a Titan Image Generator G1 for text to image generation
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="textToImageParams">The text to image prompt definition</param>
        /// <param name="imageGenerationConfig">The image configuration definition. If null, default values will be used</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>The Titan Image Generator G1 response</returns>
        public static async Task<TitanImageGeneratorG1Response?> InvokeTitanImageGeneratorG1ForTextToImageAsync(this AmazonBedrockRuntimeClient client, TitanImageTextToImageParams textToImageParams, TitanImageGenerationConfig? imageGenerationConfig = null, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(textToImageParams);
            
            JsonObject payload = new JsonObject()
            {
                ["taskType"] = "TEXT_IMAGE",
                ["textToImageParams"] = JsonSerializer.SerializeToNode(textToImageParams)
            };

            if (imageGenerationConfig is not null)
            {
                payload.Add("imageGenerationConfig", JsonSerializer.SerializeToNode(imageGenerationConfig));
            }

            InvokeModelResponse response = await client.InvokeModelAsync(new InvokeModelRequest()
                {
                     ModelId = new Model.TitanImageGeneratorV1().ModelId,
                     ContentType = "application/json",
                     Accept = "application/json",
                     Body = AWSSDKUtils.GenerateMemoryStreamFromString(payload.ToJsonString())
                },
                cancellationToken).ConfigureAwait(false);
            
            return await JsonSerializer.DeserializeAsync<TitanImageGeneratorG1Response>(response.Body, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Invoke Stability AI Stable Diffusion XL v1 for text to image generation
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="model">The Stable Diffusion XL model to invoke
        /// <param name="textToImageParams">The text to image prompt definition</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>The Stability AI Stable Diffusion XL response</returns> 
        public static async Task<StableDiffusionResponse?> InvokeStableDiffusionXlForTextToImageAsync(this AmazonBedrockRuntimeClient client, Model.StableDiffusionXl model, StableDiffusionTextToImageParams textToImageParams, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(textToImageParams);
            Validator.ValidateObject(textToImageParams, new ValidationContext(textToImageParams), true);
            
            InvokeModelResponse response = await client.InvokeModelAsync(new InvokeModelRequest()
                {
                    ModelId = model.ModelId,
                    ContentType = "application/json",
                    Accept = "application/json",
                    Body = new MemoryStream(JsonSerializer.SerializeToUtf8Bytes(textToImageParams))
                },
                cancellationToken).ConfigureAwait(false);
            
            return await JsonSerializer.DeserializeAsync<StableDiffusionResponse>(response.Body, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Invoke a Titan Text G1 model (Lite or Express) for text completion
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="model">The Titan Text G1 model to invoke</param>
        /// <param name="inputText">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>The Titan Text G1 model response</returns>
        public static async Task<TitanTextResponse?> InvokeTitanTextG1Async(this AmazonBedrockRuntimeClient client, Model.TitanText model, string inputText, TitanTextGenerationConfig? textGenerationConfig = null, CancellationToken cancellationToken = default)
        {
            JsonObject payload = new ()
            {
                ["inputText"] = inputText
            };

            if (textGenerationConfig is not null)
            {
                Validator.ValidateObject(textGenerationConfig, new ValidationContext(textGenerationConfig), true);
                payload.Add("textGenerationConfig", JsonSerializer.SerializeToNode(textGenerationConfig));
            }

            InvokeModelResponse response = await client.InvokeModelAsync(new InvokeModelRequest()
                {
                    ModelId = model.ModelId,
                    ContentType = "application/json",
                    Accept = "application/json",
                    Body = new MemoryStream(JsonSerializer.SerializeToUtf8Bytes(payload))
                },
                cancellationToken).ConfigureAwait(false);
            
            return await JsonSerializer.DeserializeAsync<TitanTextResponse>(response.Body, cancellationToken: cancellationToken).ConfigureAwait(false);   
        }
        
        /// <summary>
        /// Invoke Titan Text G1 model for text completion with a response stream
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="model">The Titan Text G1 model to invoke</param>
        /// <param name="inputText">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>An asynchronous enumeration of Titan Text G1 model responses</returns>
        public static async IAsyncEnumerable<TitanTextStreamingResponse> InvokeTitanTextG1WithResponseStreamAsync(this AmazonBedrockRuntimeClient client, Model.TitanText model, string inputText, TitanTextGenerationConfig? textGenerationConfig = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            JsonObject payload = new ()
            {
                ["inputText"] = inputText
            };

            if (textGenerationConfig is not null)
            {
                Validator.ValidateObject(textGenerationConfig, new ValidationContext(textGenerationConfig), true);
                payload.Add("textGenerationConfig", JsonSerializer.SerializeToNode(textGenerationConfig));
            }

            InvokeModelWithResponseStreamResponse response = await client.InvokeModelWithResponseStreamAsync(new InvokeModelWithResponseStreamRequest()
                {
                    ModelId = model.ModelId,
                    ContentType = "application/json",
                    Accept = "application/json",
                    Body = AWSSDKUtils.GenerateMemoryStreamFromString(payload.ToJsonString())
                }, 
                cancellationToken).ConfigureAwait(false);

            Channel<TitanTextStreamingResponse> buffer = Channel.CreateUnbounded<TitanTextStreamingResponse>();
            bool isStreaming = true;

            response.Body.ChunkReceived += BodyOnChunkReceived;
            response.Body.StartProcessing();

            while ((!cancellationToken.IsCancellationRequested && isStreaming) || (!cancellationToken.IsCancellationRequested && buffer.Reader.Count  > 0))
            {
                yield return await buffer.Reader.ReadAsync(cancellationToken).ConfigureAwait(false);
            }
            response.Body.ChunkReceived -= BodyOnChunkReceived;
            
            yield break;
            
            async void BodyOnChunkReceived(object? sender, EventStreamEventReceivedArgs<PayloadPart> e)
            {
                var streamResponse = await JsonSerializer.DeserializeAsync<TitanTextStreamingResponse>(e.EventStreamEvent.Bytes, cancellationToken: cancellationToken).ConfigureAwait(false);

                if (streamResponse is null)
                {
                    throw new NullReferenceException($"Unable to deserialize {nameof(e.EventStreamEvent.Bytes)} to {nameof(TitanTextStreamingResponse)}");
                }
                
                if (streamResponse.CompletionReason != null)
                {
                    isStreaming = false;
                }

                await buffer.Writer.WriteAsync(streamResponse, cancellationToken).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Invoke Titan Embeddings G1 Text model for embedding generation
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="inputText">The input text describing the input image</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The Titan Embeddings G1 Text model response</returns>
        public static async Task<TitanEmbeddingsResponse?> InvokeTitanEmbeddingsG1TextAsync(this AmazonBedrockRuntimeClient client, string inputText, CancellationToken cancellationToken = default)
        {
            JsonObject payload = new ()
            {
                ["inputText"] = inputText
            };

            InvokeModelResponse response = await client.InvokeModelAsync(new InvokeModelRequest()
                {
                    ModelId = new Model.TitanEmbedTextV1().ModelId,
                    ContentType = "application/json",
                    Accept = "application/json",
                    Body = new MemoryStream(JsonSerializer.SerializeToUtf8Bytes(payload))
                },
                cancellationToken).ConfigureAwait(false);
            
            return await JsonSerializer.DeserializeAsync<TitanEmbeddingsResponse>(response.Body, cancellationToken: cancellationToken).ConfigureAwait(false);   
        }

        /// <summary>
        /// Invoke Titan Multimodal Embeddings G1 model for embedding generation
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="inputText">The input text describing the input image</param>
        /// <param name="inputImage">The input image to transform into embedding</param>
        /// <param name="embeddingConfig">The embedding generation configuration</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>The Titan Multimodal Embeddings G1 model response</returns>
        public static async Task<TitanEmbeddingsResponse?> InvokeTitanMultimodalEmbeddingsG1Async(this AmazonBedrockRuntimeClient client, string inputText, string inputImage, TitanMultimodalEmbeddingConfig? embeddingConfig = null, CancellationToken cancellationToken = default)
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
                    ModelId = new Model.TitanEmbedImageV1().ModelId,
                    ContentType = "application/json",
                    Accept = "application/json",
                    Body = new MemoryStream(JsonSerializer.SerializeToUtf8Bytes(payload))
                },
                cancellationToken).ConfigureAwait(false);
            
            return await JsonSerializer.DeserializeAsync<TitanEmbeddingsResponse>(response.Body, cancellationToken: cancellationToken).ConfigureAwait(false);   
        }

        /// <summary>
        /// Invoke a Claude model (Instant V1, V2, V2.1) for text completion
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="model">The Claude model to invoke</param>
        /// <param name="prompt">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>The Claude model response</returns>
        public static async Task<ClaudeTextGenerationResponse?> InvokeClaudeAsync(this AmazonBedrockRuntimeClient client, Model.Claude model, string prompt, ClaudeTextGenerationConfig? textGenerationConfig = null, CancellationToken cancellationToken = default)
        {
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
                    ModelId = model.ModelId,
                    ContentType = "application/json",
                    Accept = "application/json",
                    Body = AWSSDKUtils.GenerateMemoryStreamFromString(payload.ToJsonString())
                },
                cancellationToken).ConfigureAwait(false);

            return await JsonSerializer.DeserializeAsync<ClaudeTextGenerationResponse>(response.Body, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Invoke a Claude model (Instant V1, V2, V2.1, V3 Sonnet, V3 Haiku, V3 Opus) for text completion
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="model">The Claude model to invoke</param>
        /// <param name="prompt">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>The Claude model response</returns>
        public static async Task<ClaudeMessagesResponse?> InvokeClaudeMessagesAsync(this AmazonBedrockRuntimeClient client, Model.Claude model, ClaudeMessage message, ClaudeMessagesConfig? messagesConfig = null, CancellationToken cancellationToken = default)
        {
            if (messagesConfig != null)
            {
                Validator.ValidateObject(messagesConfig, new ValidationContext(messagesConfig), true);
            }
            else
            {
                messagesConfig = new ClaudeMessagesConfig() { MaxTokens = 1000, Messages = new List<ClaudeMessage>() };
            }
            messagesConfig.Messages.Add(message);

            JsonObject payload = JsonSerializer.SerializeToNode(messagesConfig)?.AsObject() ?? new();
            
            InvokeModelResponse response = await client.InvokeModelAsync(new InvokeModelRequest()
            {
                ModelId = model.ModelId,
                ContentType = "application/json",
                Accept = "application/json",
                Body = AWSSDKUtils.GenerateMemoryStreamFromString(payload.ToJsonString())
            },
                cancellationToken).ConfigureAwait(false);

            return await JsonSerializer.DeserializeAsync<ClaudeMessagesResponse>(response.Body, cancellationToken: cancellationToken).ConfigureAwait(false);
        }


        /// <summary>
        /// Invoke a Claude model (Instant V1, V2, V2.1) for text completion with a response stream
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="model">The Claude model to invoke</param>
        /// <param name="prompt">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>An asynchronous enumeration of Claude model responses</returns>
        public static async IAsyncEnumerable<ClaudeTextGenerationResponse> InvokeClaudeWithResponseStreamAsync(this AmazonBedrockRuntimeClient client, Model.Claude model, string prompt, ClaudeTextGenerationConfig? textGenerationConfig = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
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
                    ModelId = model.ModelId,
                    ContentType = "application/json",
                    Accept = "application/json",
                    Body = AWSSDKUtils.GenerateMemoryStreamFromString(payload.ToJsonString())
                }, 
                cancellationToken).ConfigureAwait(false);

            Channel<ClaudeTextGenerationResponse> buffer = Channel.CreateUnbounded<ClaudeTextGenerationResponse>();
            bool isStreaming = true;

            response.Body.ChunkReceived += BodyOnChunkReceived;
            response.Body.StartProcessing();

            while ((!cancellationToken.IsCancellationRequested && isStreaming) || (!cancellationToken.IsCancellationRequested && buffer.Reader.Count  > 0))
            {
                yield return await buffer.Reader.ReadAsync(cancellationToken).ConfigureAwait(false);
            }
            response.Body.ChunkReceived -= BodyOnChunkReceived;
            
            yield break;
            
            async void BodyOnChunkReceived(object? sender, EventStreamEventReceivedArgs<PayloadPart> e)
            {
                var streamResponse = await JsonSerializer.DeserializeAsync<ClaudeTextGenerationResponse>(e.EventStreamEvent.Bytes, cancellationToken: cancellationToken).ConfigureAwait(false);

                if (streamResponse is null)
                {
                    throw new NullReferenceException($"Unable to deserialize {nameof(e.EventStreamEvent.Bytes)} to {nameof(ClaudeTextGenerationResponse)}");
                }
                
                if (streamResponse.StopReason != null)
                {
                    isStreaming = false;
                }

                await buffer.Writer.WriteAsync(streamResponse, cancellationToken).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Invoke a Claude model (Instant V1, V2, V2.1, V3 Sonnet, V3 Haiku, V3 Opus) for text completion with a response stream
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="model">The Claude model to invoke</param>
        /// <param name="prompt">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>An asynchronous enumeration of Claude model responses</returns>
        public static async IAsyncEnumerable<IClaudeMessagesChunk> InvokeClaudeMessagesWithResponseStreamAsync(this AmazonBedrockRuntimeClient client, Model.Claude model, ClaudeMessage message, ClaudeMessagesConfig? messagesConfig = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            if (messagesConfig != null)
            {
                Validator.ValidateObject(messagesConfig, new ValidationContext(messagesConfig), true);
            }
            else
            {
                messagesConfig = new ClaudeMessagesConfig() { MaxTokens = 1000, Messages = new List<ClaudeMessage>() };
            }
            messagesConfig.Messages.Add(message);

            JsonObject payload = JsonSerializer.SerializeToNode(messagesConfig)?.AsObject() ?? new();

            InvokeModelWithResponseStreamResponse response = await client.InvokeModelWithResponseStreamAsync(new InvokeModelWithResponseStreamRequest()
            {
                ModelId = model.ModelId,
                ContentType = "application/json",
                Accept = "application/json",
                Body = AWSSDKUtils.GenerateMemoryStreamFromString(payload.ToJsonString())
            },
                cancellationToken).ConfigureAwait(false);

            Channel<IClaudeMessagesChunk> buffer = Channel.CreateUnbounded<IClaudeMessagesChunk>();
            bool isStreaming = true;

            response.Body.ChunkReceived += BodyOnChunkReceived;
            response.Body.StartProcessing();

            while ((!cancellationToken.IsCancellationRequested && isStreaming) || (!cancellationToken.IsCancellationRequested && buffer.Reader.Count > 0))
            {
                yield return await buffer.Reader.ReadAsync(cancellationToken).ConfigureAwait(false);
            }
            response.Body.ChunkReceived -= BodyOnChunkReceived;

            yield break;

            async void BodyOnChunkReceived(object? sender, EventStreamEventReceivedArgs<PayloadPart> e)
            {
                var message = new StreamReader(e.EventStreamEvent.Bytes).ReadToEnd();
                e.EventStreamEvent.Bytes.Position = 0;
                var streamResponse = await JsonSerializer.DeserializeAsync<IClaudeMessagesChunk>(e.EventStreamEvent.Bytes, cancellationToken: cancellationToken).ConfigureAwait(false);

                if (streamResponse is null)
                {
                    throw new NullReferenceException($"Unable to deserialize {nameof(e.EventStreamEvent.Bytes)} to {nameof(IClaudeMessagesChunk)}");
                }

                if (streamResponse is ClaudeMessagesMessageStopChunk)
                {
                    isStreaming = false;
                }

                await buffer.Writer.WriteAsync(streamResponse, cancellationToken).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Invoke a Command v14 model (Text or Light Text) for text completion
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="model">The Command model to invoke</param>
        /// <param name="prompt">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>The Command model response</returns>
        public static async Task<CommandResponse?> InvokeCommandV14Async(this AmazonBedrockRuntimeClient client, Model.CommandText model, string prompt, CommandTextGenerationConfig? textGenerationConfig = null, CancellationToken cancellationToken = default)
        {
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
                    ModelId = model.ModelId,
                    ContentType = "application/json",
                    Accept = "application/json",
                    Body = AWSSDKUtils.GenerateMemoryStreamFromString(payload.ToJsonString())
                },
                cancellationToken).ConfigureAwait(false);

            return await JsonSerializer.DeserializeAsync<CommandResponse>(response.Body, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Invoke a Embed V3 model (English or Multilingual) for embedding generation
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="model">The Embed model to invoke</param>
        /// <param name="texts">The text to transform into embedding</param>
        /// <param name="embeddingGenerationConfig">The embedding generation configuration</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>The Embed model response</returns>
        public static async Task<EmbedResponse?> InvokeEmbedV3Async(this AmazonBedrockRuntimeClient client, Model.Embed model, IEnumerable<string> texts, EmbedEmbeddingGenerationConfig? embeddingGenerationConfig = null, CancellationToken cancellationToken = default)
        {
            embeddingGenerationConfig ??= new EmbedEmbeddingGenerationConfig();
            Validator.ValidateObject(embeddingGenerationConfig, new ValidationContext(embeddingGenerationConfig), true);
            
            JsonObject payload = JsonSerializer.SerializeToNode(embeddingGenerationConfig)?.AsObject() ?? new ();
            payload.Add("texts", JsonSerializer.SerializeToNode(texts));
            
            InvokeModelResponse response = await client.InvokeModelAsync(new InvokeModelRequest()
                {
                    ModelId = model.ModelId,
                    ContentType = "application/json",
                    Accept = "application/json",
                    Body = AWSSDKUtils.GenerateMemoryStreamFromString(payload.ToJsonString())
                },
                cancellationToken).ConfigureAwait(false);

            return await JsonSerializer.DeserializeAsync<EmbedResponse>(response.Body, cancellationToken: cancellationToken).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Invoke a Command v14 model (Text or Light Text) for text completion with a response stream
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="model">The Command V14 model to invoke</param>
        /// <param name="prompt">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>An asynchronous enumeration of Command model responses</returns>
        public static async IAsyncEnumerable<CommandStreamingResponse> InvokeCommandV14WithResponseStreamAsync(this AmazonBedrockRuntimeClient client, Model.CommandText model, string prompt, CommandTextGenerationConfig? textGenerationConfig = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            textGenerationConfig ??= new CommandTextGenerationConfig();
            // We want to force streaming. The default is false. If we don't force the value to True, Cohere model won't stream even if we call the InvokeModelWithResponseStream API action.
            textGenerationConfig.Stream = true;
            
            Validator.ValidateObject(textGenerationConfig, new ValidationContext(textGenerationConfig), true);
            JsonObject? payload = JsonSerializer.SerializeToNode(textGenerationConfig)?.AsObject();
            
            payload??= new JsonObject();
            payload.Add("prompt", prompt);
            
            InvokeModelWithResponseStreamResponse response = await client.InvokeModelWithResponseStreamAsync(new InvokeModelWithResponseStreamRequest()
                {
                    ModelId = model.ModelId,
                    ContentType = "application/json",
                    Accept = "application/json",
                    Body = AWSSDKUtils.GenerateMemoryStreamFromString(payload.ToJsonString())
                }, 
                cancellationToken).ConfigureAwait(false);

            Channel<CommandStreamingResponse> buffer = Channel.CreateUnbounded<CommandStreamingResponse>();
            bool isStreaming = true;

            response.Body.ChunkReceived += BodyOnChunkReceived;
            response.Body.StartProcessing();

            while ((!cancellationToken.IsCancellationRequested && isStreaming) || (!cancellationToken.IsCancellationRequested && buffer.Reader.Count  > 0))
            {
                yield return await buffer.Reader.ReadAsync(cancellationToken).ConfigureAwait(false);
            }
            response.Body.ChunkReceived -= BodyOnChunkReceived;
            
            yield break;
            
            async void BodyOnChunkReceived(object? sender, EventStreamEventReceivedArgs<PayloadPart> e)
            {
                var streamResponse = await JsonSerializer.DeserializeAsync<CommandStreamingResponse>(e.EventStreamEvent.Bytes, cancellationToken: cancellationToken).ConfigureAwait(false);

                if (streamResponse is null)
                {
                    throw new NullReferenceException($"Unable to deserialize {nameof(e.EventStreamEvent.Bytes)} to {nameof(CommandStreamingResponse)}");
                }

                isStreaming = streamResponse.IsFinished ?? false;
                
                await buffer.Writer.WriteAsync(streamResponse, cancellationToken).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Invoke a Llama model (Llama 2 13B Chat, Llama 2 70B Chat, Llama 3 8B Instruct or Llama 3 70B Instruct) for text completion
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="model">The Llama model to invoke</param>
        /// <param name="prompt">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>The Llama model response</returns>
        public static async Task<LlamaResponse?> InvokeLlamaAsync(this AmazonBedrockRuntimeClient client, Model.Llama model, string prompt, LlamaTextGenerationConfig? textGenerationConfig = null, CancellationToken cancellationToken = default)
        {
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
                    ModelId = model.ModelId,
                    ContentType = "application/json",
                    Accept = "application/json",
                    Body = AWSSDKUtils.GenerateMemoryStreamFromString(payload.ToJsonString())
                },
                cancellationToken).ConfigureAwait(false);

            return await JsonSerializer.DeserializeAsync<LlamaResponse>(response.Body, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Invoke a Llama model (Llama 2 13B Chat, Llama 2 70B Chat, Llama 3 8B Instruct or Llama 3 70B Instruct) for text completion with a response stream
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="model">The Llama model to invoke</param>
        /// <param name="prompt">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>An asynchronous enumeration of Llama model responses</returns>
        public static async IAsyncEnumerable<LlamaResponse> InvokeLlamaWithResponseStreamAsync(this AmazonBedrockRuntimeClient client, Model.Llama model, string prompt, LlamaTextGenerationConfig? textGenerationConfig = null, [EnumeratorCancellation]CancellationToken cancellationToken = default)
        {
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
                    ModelId = model.ModelId,
                    ContentType = "application/json",
                    Accept = "application/json",
                    Body = AWSSDKUtils.GenerateMemoryStreamFromString(payload.ToJsonString())
                },
                cancellationToken).ConfigureAwait(false);

            Channel<LlamaResponse> buffer = Channel.CreateUnbounded<LlamaResponse>();
            bool isStreaming = true;

            response.Body.ChunkReceived += BodyOnChunkReceived;
            response.Body.StartProcessing();

            while ((!cancellationToken.IsCancellationRequested && isStreaming) || (!cancellationToken.IsCancellationRequested && buffer.Reader.Count  > 0))
            {
                yield return await buffer.Reader.ReadAsync(cancellationToken).ConfigureAwait(false);
            }
            response.Body.ChunkReceived -= BodyOnChunkReceived;
            
            yield break;
            
            async void BodyOnChunkReceived(object? sender, EventStreamEventReceivedArgs<PayloadPart> e)
            {
                var streamResponse = await JsonSerializer.DeserializeAsync<LlamaResponse>(e.EventStreamEvent.Bytes, cancellationToken: cancellationToken).ConfigureAwait(false);

                if (streamResponse is null)
                {
                    throw new NullReferenceException($"Unable to deserialize {nameof(e.EventStreamEvent.Bytes)} to {nameof(LlamaResponse)}");
                }

                if (streamResponse.GetStopReason() != null)
                {
                    isStreaming = false;
                }
                
                await buffer.Writer.WriteAsync(streamResponse, cancellationToken).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Invoke a Mistral model (Mistral 7B Instruct, Mistral 8x7B Instruct or Mistral Large) for text completion
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="model">The Mistral model to invoke</param>
        /// <param name="prompt">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>The Mistral model response</returns>
        public static async Task<MistralResponse?> InvokeMistralAsync(this AmazonBedrockRuntimeClient client, Model.Mistral model, string prompt, MistralTextGenerationConfig? textGenerationConfig = null, CancellationToken cancellationToken = default)
        {
            JsonObject? payload = null;
            if (textGenerationConfig != null)
            {
                Validator.ValidateObject(textGenerationConfig, new ValidationContext(textGenerationConfig), true);
                payload = JsonSerializer.SerializeToNode(textGenerationConfig)?.AsObject();
            }

            payload ??= new JsonObject();
            payload.Add("prompt", prompt);

            InvokeModelResponse response = await client.InvokeModelAsync(new InvokeModelRequest()
            {
                ModelId = model.ModelId,
                ContentType = "application/json",
                Accept = "application/json",
                Body = AWSSDKUtils.GenerateMemoryStreamFromString(payload.ToJsonString())
            },
                cancellationToken).ConfigureAwait(false);

            return await JsonSerializer.DeserializeAsync<MistralResponse>(response.Body, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Invoke a Mistral model (Mistral 7B Instruct, Mistral 8x7B Instruct or Mistral Large) for text completion with a response stream
        /// </summary>
        /// <param name="client">The Amazon Bedrock Runtime client object</param>
        /// <param name="model">The Mistral model to invoke</param>
        /// <param name="prompt">The input text to complete</param>
        /// <param name="textGenerationConfig">The text generation configuration</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>An asynchronous enumeration of Mistral model responses</returns>
        public static async IAsyncEnumerable<MistralResponse> InvokeMistralWithResponseStreamAsync(this AmazonBedrockRuntimeClient client, Model.Mistral model, string prompt, MistralTextGenerationConfig? textGenerationConfig = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            JsonObject? payload = null;
            if (textGenerationConfig != null)
            {
                Validator.ValidateObject(textGenerationConfig, new ValidationContext(textGenerationConfig), true);
                payload = JsonSerializer.SerializeToNode(textGenerationConfig)?.AsObject();
            }

            payload ??= new JsonObject();
            payload.Add("prompt", prompt);

            InvokeModelWithResponseStreamResponse response = await client.InvokeModelWithResponseStreamAsync(new InvokeModelWithResponseStreamRequest()
            {
                ModelId = model.ModelId,
                ContentType = "application/json",
                Accept = "application/json",
                Body = AWSSDKUtils.GenerateMemoryStreamFromString(payload.ToJsonString())
            },
                cancellationToken).ConfigureAwait(false);

            Channel<MistralResponse> buffer = Channel.CreateUnbounded<MistralResponse>();
            bool isStreaming = true;

            response.Body.ChunkReceived += BodyOnChunkReceived;
            response.Body.StartProcessing();

            while ((!cancellationToken.IsCancellationRequested && isStreaming) || (!cancellationToken.IsCancellationRequested && buffer.Reader.Count > 0))
            {
                yield return await buffer.Reader.ReadAsync(cancellationToken).ConfigureAwait(false);
            }
            response.Body.ChunkReceived -= BodyOnChunkReceived;

            yield break;

            async void BodyOnChunkReceived(object? sender, EventStreamEventReceivedArgs<PayloadPart> e)
            {
                var streamResponse = await JsonSerializer.DeserializeAsync<MistralResponse>(e.EventStreamEvent.Bytes, cancellationToken: cancellationToken).ConfigureAwait(false);

                if (streamResponse is null)
                {
                    throw new NullReferenceException($"Unable to deserialize {nameof(e.EventStreamEvent.Bytes)} to {nameof(MistralResponse)}");
                }

                if (streamResponse.GetStopReason() != null)
                {
                    isStreaming = false;
                }

                await buffer.Writer.WriteAsync(streamResponse, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}
