namespace BioAnalyzer.Research.Api.Domain.Models;

/// <summary>
/// Model for storing the results of an Entrez search.
/// </summary>
public class EntrezSearchResult
{
    public string Count { get; set; } = string.Empty;
    public string RetMax { get; set; } = string.Empty;
    public string RetStart { get; set; } = string.Empty;
    public ICollection<string> IdList { get; set; } = new List<string>();
}