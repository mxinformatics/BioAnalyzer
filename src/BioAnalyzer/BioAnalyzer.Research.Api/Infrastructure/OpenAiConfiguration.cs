namespace BioAnalyzer.Research.Api.Infrastructure;

public class OpenAiConfiguration
{
    public string Endpoint { get; set; } = string.Empty;
    public string GptName { get; set; } = string.Empty;
    public string SearchEndpoint { get; set; } = string.Empty;
    
    public string SearchIndexName { get; set; } = string.Empty;
    public string SearchApiKey { get; set; } = string.Empty;
    public string OpenAiApiKey { get; set; } = string.Empty;
    
    
    public void ThrowIfInvalid()
    {
        if (string.IsNullOrWhiteSpace(Endpoint))
        {
            throw new InvalidOperationException("OpenAI Endpoint is required");
        }
        if (string.IsNullOrWhiteSpace(GptName))
        {
            throw new InvalidOperationException("OpenAI GPT Name is required");
        }
        if (string.IsNullOrWhiteSpace(SearchEndpoint))
        {
            throw new InvalidOperationException("Search Endpoint is required");
        }
        if (string.IsNullOrWhiteSpace(SearchApiKey))
        {
            throw new InvalidOperationException("Search API Key is required");
        }
        if (string.IsNullOrWhiteSpace(OpenAiApiKey))
        {
            throw new InvalidOperationException("OpenAI API Key is required");
        }
        if (string.IsNullOrWhiteSpace(SearchIndexName))
        {
            throw new InvalidOperationException("Search Index Name is required");
        }
    }
}