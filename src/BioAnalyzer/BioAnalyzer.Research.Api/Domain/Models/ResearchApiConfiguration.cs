namespace BioAnalyzer.Research.Api.Domain.Models;

public class ResearchApiConfiguration
{
    public string EntrezBaseUrl { get; set; } = string.Empty;
    
    public void ThrowIfInvalid()
    {
        if (string.IsNullOrWhiteSpace(EntrezBaseUrl))
        {
            throw new ArgumentException("EntrezBaseUrl must be provided in the configuration.");
        }
    }
}