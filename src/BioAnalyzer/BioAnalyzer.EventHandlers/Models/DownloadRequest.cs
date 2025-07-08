namespace BioAnalyzer.EventHandlers.Models;

public class DownloadRequest
{
    public string PmcId { get; set; } = string.Empty;
    public string DownloadLink { get; set; } = string.Empty;
    
    public string Title { get; set; } = string.Empty;
    
    public string Doi { get; set; } = string.Empty;
    
    public string GetHttpDownloadLink()
    {
        return !string.IsNullOrEmpty(DownloadLink) ? DownloadLink.Replace("ftp://", "https://") : DownloadLink;
    }
}