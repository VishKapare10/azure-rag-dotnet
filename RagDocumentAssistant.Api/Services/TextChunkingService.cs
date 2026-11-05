using RagDocumentAssistant.Api.Interfaces;
using RagDocumentAssistant.Api.Models;

namespace RagDocumentAssistant.Api.Services;

public class TextChunkingService : ITextChunkingService
{
    private const int ChunkSize = 500;
    private const int Overlap = 50;

    public List<DocumentChunk> CreateChunks(
        string text,
        Guid documentId)
    {
        var chunks = new List<DocumentChunk>();

        if (string.IsNullOrWhiteSpace(text))
        {
            return chunks;
        }

        int start = 0;
        int chunkIndex = 0;

        while (start < text.Length)
        {
            int length = Math.Min(
                ChunkSize,
                text.Length - start);

            string content = text.Substring(start, length);

            chunks.Add(new DocumentChunk
            {
                DocumentId = documentId,
                ChunkIndex = chunkIndex,
                Content = content,
                TokenCount = 0
            });

            chunkIndex++;

            if (start + length >= text.Length)
                break;

            start += ChunkSize - Overlap;
        }

        return chunks;
    }
}