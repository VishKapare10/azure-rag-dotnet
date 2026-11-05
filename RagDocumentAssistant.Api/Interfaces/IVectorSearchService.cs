
using RagDocumentAssistant.Api.Models;

namespace RagDocumentAssistant.Api.Interfaces;

public interface IVectorSearchService
{
    Task<List<DocumentChunk>> SearchAsync(
        string query,
        Guid? documentId = null,
        CancellationToken cancellationToken = default);
}
