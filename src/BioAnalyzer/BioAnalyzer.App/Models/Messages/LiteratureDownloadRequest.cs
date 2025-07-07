namespace BioAnalyzer.App.Models.Messages;

public class LiteratureDownloadRequest
{
    public LiteratureDownloadRequest() {}
    public LiteratureDownloadRequest(string literatureId, string downloadLink, string title)
    {
        LiteratureId = literatureId;
        DownloadLink = downloadLink;
        Title = title;
    }
    public string LiteratureId { get; set; } = string.Empty;
    public string DownloadLink { get; set; } = string.Empty;
    
    public string Title { get; set; } = string.Empty;
}