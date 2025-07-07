namespace BioAnalyzer.App.Contracts.Clients;

public interface IEventBusClient
{
    Task Publish<TMessageType>(IList<TMessageType> messages);   
}