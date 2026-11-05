using RagDocumentAssistant.Api.Interfaces;
using RagDocumentAssistant.Api.Models;

namespace RagDocumentAssistant.Api.Services;

public class DocumentService : IDocumentService
{
    private readonly IWebHostEnvironment _environment;

    public DocumentService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<Document> UploadAsync(
        IFormFile file,
        CancellationToken cancellationToken = default)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("Please select a PDF file.");
        }

        // Maximum upload size: 10 MB
        if (file.Length > 10 * 1024 * 1024)
        {
            throw new ArgumentException("File size cannot exceed 10 MB.");
        }

        var extension = Path.GetExtension(file.FileName);

        if (!string.Equals(extension, ".pdf",
            StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException("Only PDF files are allowed.");
        }

        // Validate PDF file signature
        await using var inputStream = file.OpenReadStream();

        var signature = new byte[5];

        var bytesRead = await inputStream.ReadAsync(
            signature.AsMemory(0, 5),
            cancellationToken);

        if (bytesRead != 5 ||
            System.Text.Encoding.ASCII.GetString(signature) != "%PDF-")
        {
            throw new ArgumentException("Invalid PDF file.");
        }

        // IMPORTANT: Reset stream to beginning
        inputStream.Position = 0;

        var uploadFolder = Path.Combine(
            _environment.ContentRootPath,
            "uploads");

        Directory.CreateDirectory(uploadFolder);

        var storedFileName = $"{Guid.NewGuid()}.pdf";

        var filePath = Path.Combine(
            uploadFolder,
            storedFileName);

        await using (var outputStream = new FileStream(
            filePath,
            FileMode.CreateNew,
            FileAccess.Write,
            FileShare.None,
            81920,
            useAsync: true))
        {
            await inputStream.CopyToAsync(
                outputStream,
                cancellationToken);
        }

        return new Document
        {
            Id = Guid.NewGuid(),
            FileName = Path.GetFileName(file.FileName),
            ContentType = "application/pdf",
            BlobUrl = filePath,
            UploadedAt = DateTime.UtcNow,
            Status = "Uploaded"
        };
    }
}