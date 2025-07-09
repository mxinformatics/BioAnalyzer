using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using BioAnalyzer.EventHandlers.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace BioAnalyzer.EventHandlers;

public class ExtractDocumentTextHandler
{
    private readonly ILogger<ExtractDocumentTextHandler> _logger;

    public ExtractDocumentTextHandler(ILogger<ExtractDocumentTextHandler> logger)
    {
        _logger = logger;
    }

    [Function(nameof(ExtractDocumentTextHandler))]
    [ServiceBusOutput("%TextExtractedTopic%", Connection = "BioAnalyzerServiceBusSend")]
    public async Task<TextExtracted> Run(
        [ServiceBusTrigger("%ExtractDocumentTextQueue%", Connection = "BioAnalyzerServiceBusListen")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        _logger.LogInformation("Message ID: {id}", message.MessageId);
        _logger.LogInformation("Message Body: {body}", message.Body);
        _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

        var downloadedLiterature = message.Body.ToObjectFromJson<DownloadedLiterature>();
        
        // Complete the message
        await messageActions.CompleteMessageAsync(message);

        return new TextExtracted
        {
            FileName = downloadedLiterature.FileName,
            Title = downloadedLiterature.Title,
            PmcId = downloadedLiterature.PmcId,
            Doi = downloadedLiterature.Doi
        };

    }
    
    // private static string ExtractTextFromPdf(byte[] pdfContent)
    // {
    //     var pdfStream = new MemoryStream(pdfContent);
    //     using var reader = new PdfReader(pdfStream);
    //     using var pdfDocument = new PdfDocument(reader);
    //     var text = new StringBuilder();
    //     for (var i = 1; i <= pdfDocument.GetNumberOfPages(); i++)
    //     {
    //         var strategy = new SimpleTextExtractionStrategy();
    //         var currentText = PdfTextExtractor.GetTextFromPage(pdfDocument.GetPage(i), strategy);
    //         text.Append(Encoding.UTF8.GetString(Encoding.Convert(Encoding.Default, Encoding.UTF8,
    //             Encoding.Default.GetBytes(currentText))));
    //     }
    //     return text.ToString();
    // }
}