
namespace RagDocumentAssistant.Api.DTOs;

public class ChatResponse
{
    public string Answer { get; set; } = string.Empty;

    public List<string> Sources { get; set; } = [];
}
