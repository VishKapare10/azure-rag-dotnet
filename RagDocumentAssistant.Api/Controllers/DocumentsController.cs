
using Microsoft.AspNetCore.Mvc;
using RagDocumentAssistant.Api.DTOs;
using RagDocumentAssistant.Api.Interfaces;
using RagDocumentAssistant.Api.Services;
using System.IO;

namespace RagDocumentAssistant.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocumentsController : ControllerBase
{
    private readonly IDocumentService _documentService;
    private readonly IPdfTextExtractor _pdfTextExtractor;
    private readonly ITextChunkingService _textChunkingService;
    private readonly IEmbeddingService _embeddingService;

    public DocumentsController(
     IDocumentService documentService,
     IPdfTextExtractor pdfTextExtractor,
     ITextChunkingService textChunkingService, IEmbeddingService embeddingService)
    {
        _documentService = documentService;
        _pdfTextExtractor = pdfTextExtractor;
        _textChunkingService = textChunkingService;
        _embeddingService = embeddingService;
    }

    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Upload(
        [FromForm] DocumentUploadRequest request,
        CancellationToken cancellationToken)
    {
        if (request.File == null)
        {
            return BadRequest("Please select a PDF file.");
        }

        try
        {
            var document = await _documentService.UploadAsync(
                request.File,
                cancellationToken);

            return Ok(document);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("extract-text")]
    [Consumes("multipart/form-data")]
    public IActionResult ExtractText(
     [FromForm] DocumentUploadRequest request)
    {
        if (request.File == null || request.File.Length == 0)
        {
            return BadRequest("Please select a PDF file.");
        }

        try
        {
            using var stream = request.File.OpenReadStream();

            var extractedText =
                _pdfTextExtractor.ExtractText(stream);

            return Ok(new
            {
                FileName = request.File.FileName,
                ExtractedText = extractedText,
                CharacterCount = extractedText.Length
            });
        }
        catch (Exception ex)
        {
            return BadRequest($"PDF extraction failed: {ex.Message}");
        }
    }

    [HttpPost("create-chunks")]
    [Consumes("multipart/form-data")]
    public IActionResult CreateChunks(
    [FromForm] DocumentUploadRequest request)
    {
        if (request.File == null || request.File.Length == 0)
        {
            return BadRequest("Please select a PDF file.");
        }

        try
        {
            using var stream = request.File.OpenReadStream();

            // Step 1: Extract PDF text
            var extractedText =
                _pdfTextExtractor.ExtractText(stream);

            if (string.IsNullOrWhiteSpace(extractedText))
            {
                return BadRequest("No text found in PDF.");
            }

            // Step 2: Generate Document ID
            var documentId = Guid.NewGuid();

            // Step 3: Create chunks
            var chunks = _textChunkingService.CreateChunks(
                extractedText,
                documentId);

            return Ok(new
            {
                DocumentId = documentId,
                FileName = request.File.FileName,
                TotalChunks = chunks.Count,
                Chunks = chunks
            });
        }
        catch (Exception ex)
        {
            return BadRequest($"Chunking failed: {ex.Message}");
        }
    }


    [HttpPost("generate-embeddings")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> GenerateEmbeddings(
        [FromForm] DocumentUploadRequest request,
        CancellationToken cancellationToken)
    {
        if (request.File == null || request.File.Length == 0)
        {
            return BadRequest("Please select a PDF file.");
        }

        try
        {
            using var stream = request.File.OpenReadStream();

            var extractedText = _pdfTextExtractor.ExtractText(stream);

            if (string.IsNullOrWhiteSpace(extractedText))
            {
                return BadRequest("No text found in PDF.");
            }

            var documentId = Guid.NewGuid();

            var chunks = _textChunkingService.CreateChunks(
                extractedText, documentId);

            foreach (var chunk in chunks)
            {
                chunk.Embedding =
                    await _embeddingService.GenerateEmbeddingAsync(
                        chunk.Content, cancellationToken);
            }

            return Ok(new
            {
                DocumentId = documentId,
                TotalChunks = chunks.Count,
                EmbeddedChunks = chunks.Count(c => c.Embedding != null),
                Dimensions = chunks.First().Embedding?.Length,
                Chunks = chunks
            });
        }
        catch (Exception)
        {
            return StatusCode(500,
                "Embedding generation failed. Check server logs.");
        }
    }

}
