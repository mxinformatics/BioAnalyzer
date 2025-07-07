using Azure.Identity;
using Azure.Messaging.ServiceBus;
using BioAnalyzer.App.Contracts.Clients;
using BioAnalyzer.App.Infrastructure;
using Microsoft.Extensions.Options;

namespace BioAnalyzer.App.Services;

public class EventBusClient(IOptions<EventConfiguration> eventConfiguration) : IEventBusClient
{
    private readonly EventConfiguration _eventConfiguration = eventConfiguration.Value;
    
    
    public async Task Publish<TMessageType>(IList<TMessageType> messages)
    {
        var client = new ServiceBusClient(_eventConfiguration.ServiceBusNamespace,
            new DefaultAzureCredential());
        var sender = client.CreateSender(_eventConfiguration.LiteratureDownloadTopic);

        var messageBatch = await sender.CreateMessageBatchAsync();
        foreach (var message in messages)
        {
            if (!messageBatch.TryAddMessage(new ServiceBusMessage(System.Text.Json.JsonSerializer.Serialize(message))))
            {
                throw new Exception($"Message {message} is too large to fit in the batch.");
            }
        }

        try
        {
            await sender.SendMessagesAsync(messageBatch);
        }
        finally
        {
            await sender.DisposeAsync();
            await client.DisposeAsync();
        }
    }
}