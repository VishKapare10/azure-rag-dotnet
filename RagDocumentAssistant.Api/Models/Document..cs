
namespace RagDocumentAssistant.Api.Models;

public class Document
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string FileName { get; set; } = string.Empty;

    public string ContentType { get; set; } = string.Empty;

    public string BlobUrl { get; set; } = string.Empty;

    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    public string Status { get; set; } = "Uploaded";
}
