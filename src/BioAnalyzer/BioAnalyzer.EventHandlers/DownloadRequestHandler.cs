using System.Text;
using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
using BioAnalyzer.EventHandlers.Infrastructure;
using BioAnalyzer.EventHandlers.Models;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace BioAnalyzer.EventHandlers;

public class DownloadRequestHandler
{
    private readonly ILogger<DownloadRequestHandler> _logger;
    private readonly EventHandlerConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;
    public DownloadRequestHandler(ILogger<DownloadRequestHandler> logger, EventHandlerConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;

    }

    [Function(nameof(DownloadRequestHandler))]
    [ServiceBusOutput("%DocumentDownloadedTopic%", Connection = "BioAnalyzerServiceBusSend")]
    public async Task<DownloadedLiterature> Run(
        [ServiceBusTrigger("%DownloadDocumentQueue%", Connection = "BioAnalyzerServiceBusListen")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        _logger.LogInformation("Message ID: {id}", message.MessageId);

        var downloadRequest = message.Body.ToObjectFromJson<DownloadRequest>();
        if (downloadRequest != null)
        {
            try
            {
                await DownloadFile(downloadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while downloading file");
            }
                    
        }
        
        // Complete the message
        await messageActions.CompleteMessageAsync(message);

        return new DownloadedLiterature
        {
            FileName = $"{downloadRequest.PmcId}.pdf",
            Title = downloadRequest.Title,
            DownloadLink = downloadRequest.DownloadLink,
            Doi = downloadRequest.Doi,
            PmcId = downloadRequest.PmcId
            
        };


    }

    private  async Task DownloadFile(DownloadRequest downloadRequest)
    {
        if(string.IsNullOrEmpty(downloadRequest.DownloadLink))
        {
            return;
        }

        var httpClient = _httpClientFactory.CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Get,  downloadRequest.GetHttpDownloadLink());
        var response = await httpClient.SendAsync(request);
        
        if (response.IsSuccessStatusCode)
        {
            var fileContent = await response.Content.ReadAsByteArrayAsync();
            var blobServiceClient = new BlobServiceClient(_configuration.DownloadFileStorage);
            var containerClient = blobServiceClient.GetBlobContainerClient(_configuration.DownloadFileContainer);
            var blobClient = containerClient.GetBlobClient($"{downloadRequest.PmcId}.pdf");
            await blobClient.UploadAsync(new BinaryData(fileContent), true);
            
         //   var downloadedText = ExtractTextFromPdf(fileContent);
            

        }
        else
        {
            _logger.LogError($"Failed to download file: {downloadRequest.DownloadLink}");
        }
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