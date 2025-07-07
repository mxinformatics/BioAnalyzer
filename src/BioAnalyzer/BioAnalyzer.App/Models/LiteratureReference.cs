namespace BioAnalyzer.App.Models;

public class LiteratureReference
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    
    public string PmcId { get; set; } = string.Empty;
    
    public string Doi { get; set; } = string.Empty;

    public bool CanDownload => !string.IsNullOrWhiteSpace(PmcId);
}