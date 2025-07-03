namespace BioAnalyzer.App.Models.ResearchApi;

public class LiteratureDownloadLinkResponse
{
    public string PmcId { get; set; } = string.Empty;
    public string ArchiveLink { get; set; } = string.Empty;
    public string PdfLink { get; set; } = string.Empty;
}