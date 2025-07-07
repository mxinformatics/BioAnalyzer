using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace BioAnalyzer.EventHandlers;

public class DownloadRequestHandler
{
    private readonly ILogger<DownloadRequestHandler> _logger;

    public DownloadRequestHandler(ILogger<DownloadRequestHandler> logger)
    {
        _logger = logger;
    }

    [Function(nameof(DownloadRequestHandler))]
    public async Task Run(
        [ServiceBusTrigger("download-document", Connection = "BioAnalyzerServiceBus")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        _logger.LogInformation("Message ID: {id}", message.MessageId);
        _logger.LogInformation("Message Body: {body}", message.Body);
        _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

        // Complete the message
        await messageActions.CompleteMessageAsync(message);
        
    }
}