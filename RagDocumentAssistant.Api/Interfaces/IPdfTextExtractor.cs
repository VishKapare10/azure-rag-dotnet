namespace RagDocumentAssistant.Api.Interfaces;

public interface IPdfTextExtractor
{
    string ExtractText(Stream pdfStream);
}