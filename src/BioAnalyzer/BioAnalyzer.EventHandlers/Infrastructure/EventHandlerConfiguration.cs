namespace BioAnalyzer.EventHandlers.Infrastructure;

public class EventHandlerConfiguration
{
    /// <summary>
    /// Connection string for the Azure Storage Account.
    /// </summary>
    public string DownloadFileStorage { get; set; } = string.Empty;
    
    /// <summary>
    /// Azure storage container name where the downloaded files will be stored.
    /// </summary>
    public string DownloadFileContainer { get; set; } = string.Empty;

}