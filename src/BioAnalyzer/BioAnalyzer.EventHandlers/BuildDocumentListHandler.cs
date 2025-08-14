using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using BioAnalyzer.EventHandlers.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace BioAnalyzer.EventHandlers;

public class BuildDocumentListHandler
{
    private readonly ILogger<BuildDocumentListHandler> _logger;

    public BuildDocumentListHandler(ILogger<BuildDocumentListHandler> logger)
    {
        _logger = logger;
    }

    [Function(nameof(BuildDocumentListHandler))]
    [TableOutput("%LiteratureDownloadedTable%", Connection = "DownloadFileStorage")]
    public async Task<DownloadedLiterature> Run(
        [ServiceBusTrigger("%BuildDocumentListQueue%", Connection = "BioAnalyzerServiceBusListen")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        _logger.LogInformation("Message ID: {id}", message.MessageId);
    
        var downloadedLiterature = message.Body.ToObjectFromJson<DownloadedLiterature>();

        if (downloadedLiterature == null)
        {
            _logger.LogError("Downloaded literature is null for message ID: {id}", message.MessageId);
            await messageActions.DeadLetterMessageAsync(message);
            throw new InvalidOperationException("Downloaded literature is null for message ID: {id}");
        }
        await messageActions.CompleteMessageAsync(message);
        return downloadedLiterature;
    }
}