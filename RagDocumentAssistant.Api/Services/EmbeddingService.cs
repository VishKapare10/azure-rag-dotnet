
using Azure;
using Azure.AI.OpenAI;
using OpenAI.Embeddings;
using RagDocumentAssistant.Api.Interfaces;

namespace RagDocumentAssistant.Api.Services;

public class EmbeddingService : IEmbeddingService
{
    private readonly EmbeddingClient _embeddingClient;

    public EmbeddingService(IConfiguration configuration)
    {
        var endpoint = configuration["AzureOpenAI:Endpoint"];
        var apiKey = configuration["AzureOpenAI:ApiKey"];
        var deploymentName =
            configuration["AzureOpenAI:EmbeddingDeployment"];

        if (string.IsNullOrWhiteSpace(endpoint) ||
            string.IsNullOrWhiteSpace(apiKey) ||
            string.IsNullOrWhiteSpace(deploymentName))
        {
            throw new InvalidOperationException(
                "Azure OpenAI configuration is missing.");
        }

        var azureClient = new AzureOpenAIClient(
            new Uri(endpoint),
            new AzureKeyCredential(apiKey));

        _embeddingClient =
            azureClient.GetEmbeddingClient(deploymentName);
    }

    public async Task<float[]> GenerateEmbeddingAsync(
        string text,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException(
                "Text cannot be empty.", nameof(text));
        }

        OpenAIEmbedding result =
            await _embeddingClient.GenerateEmbeddingAsync(
                text,
                cancellationToken: cancellationToken);

        return result.ToFloats().ToArray();
    }
}
