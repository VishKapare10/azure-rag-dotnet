
using Microsoft.AspNetCore.Http;

namespace RagDocumentAssistant.Api.DTOs;

public class DocumentUploadRequest
{
    public IFormFile? File { get; set; }
}
