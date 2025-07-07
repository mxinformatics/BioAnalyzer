namespace BioAnalyzer.App.Models.Messages;

public class LiteratureDownloadRequest
{
    public LiteratureDownloadRequest() {}
    public LiteratureDownloadRequest(string literatureId, string downloadLink)
    {
        LiteratureId = literatureId;
        DownloadLink = downloadLink;
    }
    public string LiteratureId { get; set; } = string.Empty;
    public string DownloadLink { get; set; } = string.Empty;
}