
using RagDocumentAssistant.Api.DTOs;

namespace RagDocumentAssistant.Api.Interfaces;

public interface IChatService
{
    Task<ChatResponse> GetAnswerAsync(
        ChatRequest request,
        CancellationToken cancellationToken = default);
}
