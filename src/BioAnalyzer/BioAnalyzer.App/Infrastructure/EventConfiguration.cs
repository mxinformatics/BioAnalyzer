namespace BioAnalyzer.App.Infrastructure;

public class EventConfiguration
{
    public string ServiceBusNamespace { get; set; } = string.Empty;
    public string LiteratureDownloadTopic { get; set; } = string.Empty;
    
    public void ThrowIfInvalid()
    {
        if (string.IsNullOrWhiteSpace(ServiceBusNamespace))
        {
            throw new ArgumentException("ServiceBusNamespace cannot be null or empty.", nameof(ServiceBusNamespace));
        }

        if (string.IsNullOrWhiteSpace(LiteratureDownloadTopic))
        {
            throw new ArgumentException("LiteratureDownloadTopic cannot be null or empty.", nameof(LiteratureDownloadTopic));
        }
    }
}