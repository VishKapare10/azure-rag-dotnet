
namespace RagDocumentAssistant.Api.DTOs;

public class ChatRequest
{
    public string Question { get; set; } = string.Empty;

    public Guid? DocumentId { get; set; }
}
