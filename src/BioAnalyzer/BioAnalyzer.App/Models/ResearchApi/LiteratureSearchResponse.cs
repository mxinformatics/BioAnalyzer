namespace BioAnalyzer.App.Models.ResearchApi;

public class LiteratureSearchResponse
{
    public ICollection<string> IdList { get; set; } = new List<string>();
}