
using Microsoft.AspNetCore.Mvc;
using RagDocumentAssistant.Api.Interfaces;

namespace RagDocumentAssistant.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmbeddingController : ControllerBase
{
    private readonly IEmbeddingService _embeddingService;

    public EmbeddingController(IEmbeddingService embeddingService)
    {
        _embeddingService = embeddingService;
    }

    [HttpPost("generate")]
    public async Task<IActionResult> Generate(
        [FromBody] string text,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return BadRequest("Text cannot be empty.");
        }

        try
        {
            var embedding = await _embeddingService
                .GenerateEmbeddingAsync(text, cancellationToken);

            return Ok(new
            {
                Dimensions = embedding.Length,
                Embedding = embedding
            });
        }
        catch (Exception)
        {
            return StatusCode(500,
                "Embedding generation failed. Check server logs.");
        }
    }
}
