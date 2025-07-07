namespace BioAnalyzer.EventHandlers.Models;

public class DownloadRequest
{
    public string LiteratureId { get; set; } = string.Empty;
    public string DownloadLink { get; set; } = string.Empty;
}