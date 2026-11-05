using System.Text;
using UglyToad.PdfPig;
using RagDocumentAssistant.Api.Interfaces;

namespace RagDocumentAssistant.Api.Services;

public class PdfTextExtractor : IPdfTextExtractor
{
    public string ExtractText(Stream pdfStream)
    {
        if (pdfStream == null || !pdfStream.CanRead)
        {
            throw new ArgumentException("Invalid PDF stream.");
        }

        if (pdfStream.CanSeek)
        {
            pdfStream.Position = 0;
        }

        using var document = PdfDocument.Open(pdfStream);

        var textBuilder = new StringBuilder();

        foreach (var page in document.GetPages())
        {
            textBuilder.AppendLine(page.Text);
        }

        return textBuilder.ToString();
    }
}