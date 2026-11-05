
using Microsoft.AspNetCore.Http;
using RagDocumentAssistant.Api.Models;

namespace RagDocumentAssistant.Api.Interfaces;

public interface IDocumentService
{
    Task<Document> UploadAsync(
        IFormFile file,
        CancellationToken cancellationToken = default);
}
