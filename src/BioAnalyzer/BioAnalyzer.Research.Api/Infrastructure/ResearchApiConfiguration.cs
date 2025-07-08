namespace BioAnalyzer.Research.Api.Infrastructure;

public class ResearchApiConfiguration
{
    public string EntrezBaseUrl { get; set; } = string.Empty;
    
    public string NcbiBaseUrl { get; set; } = string.Empty;
    public void ThrowIfInvalid()
    {
        if (string.IsNullOrWhiteSpace(EntrezBaseUrl))
        {
            throw new ArgumentException("EntrezBaseUrl must be provided in the configuration.");
        }
        if (string.IsNullOrWhiteSpace(NcbiBaseUrl))
        {
            throw new ArgumentException("NcbiBaseUrl must be provided in the configuration.");
        }
    }
}