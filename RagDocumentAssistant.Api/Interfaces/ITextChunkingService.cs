using RagDocumentAssistant.Api.Models;

namespace RagDocumentAssistant.Api.Interfaces;

public interface ITextChunkingService
{
    List<DocumentChunk> CreateChunks(
        string text,
        Guid documentId);
}