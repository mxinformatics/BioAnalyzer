using System.Formats.Tar;
using System.IO.Compression;
using System.Text;
using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
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

            if (downloadRequest.IsPdf)
            {
                await UploadPdf(downloadRequest, fileContent);    
            }
            else
            {
                await ExtractAndUploadPdf(downloadRequest, fileContent);
            }
        }
        else
        {
            _logger.LogError($"Failed to download file: {downloadRequest.DownloadLink}");
        }
    }

    private async Task ExtractAndUploadPdf(DownloadRequest downloadRequest, byte[] fileContent)
    {
        var tempDirectory = Path.Combine("./lit-references", Guid.NewGuid().ToString());
        var extractedDirectory = Path.Combine(tempDirectory, "extracted");
        Directory.CreateDirectory(tempDirectory);
        Directory.CreateDirectory(extractedDirectory);

        try
        {
            await using var gz = new GZipStream(new MemoryStream(fileContent), CompressionMode.Decompress,
                leaveOpen: true);
            await using var reader = new TarReader(gz, leaveOpen: true);
            var fileIndex = 0;
            while (await reader.GetNextEntryAsync() is { } entry)
            {
                if (entry.Name.EndsWith(".pdf"))
                {
                    var filePath = Path.Combine(extractedDirectory, $"tempFile-{fileIndex++}.pdf");
                    await entry.ExtractToFileAsync(filePath, overwrite: true);
                    await UploadLocalPdf(downloadRequest, filePath);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while extracting file");
        }
        finally
        {
            if (Directory.Exists(extractedDirectory))
            {
               Directory.Delete(extractedDirectory, true);
            }
            
        }
        
    }

    private async Task UploadPdf(DownloadRequest downloadRequest, byte[] fileContent)
    {
        var blobServiceClient = new BlobServiceClient(_configuration.DownloadFileStorage);
        var containerClient = blobServiceClient.GetBlobContainerClient(_configuration.DownloadFileContainer);
        var blobClient = containerClient.GetBlobClient(downloadRequest.UploadedFilename);
        var uploadOptions = new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders
            {
                ContentType = "application/pdf"
            }
        };
        await blobClient.UploadAsync(new BinaryData(fileContent), uploadOptions).ConfigureAwait(false);
    }

    private async Task UploadLocalPdf(DownloadRequest downloadRequest, string filePath)
    {
       ;
        var blobServiceClient = new BlobServiceClient(_configuration.DownloadFileStorage);
        var containerClient = blobServiceClient.GetBlobContainerClient(_configuration.DownloadFileContainer);
        var blobClient = containerClient.GetBlobClient(downloadRequest.UploadedFilename);
        await using var stream = File.OpenRead(filePath);
        var uploadOptions = new BlobUploadOptions
        {
            HttpHeaders = new BlobHttpHeaders
            {
                ContentType = "application/pdf"
            }
        };
        await blobClient.UploadAsync(stream, uploadOptions).ConfigureAwait(false);
    }
}