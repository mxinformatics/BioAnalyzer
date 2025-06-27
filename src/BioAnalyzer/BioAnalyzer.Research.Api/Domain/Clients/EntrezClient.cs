using BioAnalyzer.Research.Api.Domain.Models;
using Microsoft.Extensions.Options;

namespace BioAnalyzer.Research.Api.Domain.Clients;


/// <summary>
/// Implementation of the Entrez client for searching the Entrez database.
/// </summary>
public class EntrezClient(HttpClient httpClient, IOptions<ResearchApiConfiguration> options)
    : IEntrezClient
{

    private readonly HttpClient _httpClient = httpClient;
    private readonly ResearchApiConfiguration _configuration = options.Value;

    public async Task<EntrezSearchResult> LiteratureSearchAsync(string query)
    {
        var requestUri = $"{_configuration.EntrezBaseUrl}?db=pubmed&term={Uri.EscapeDataString(query)}&retmode=json";
        var result = await _httpClient.GetAsync(requestUri).ConfigureAwait(false);
        if (!result.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Failed to search literature: {result.ReasonPhrase}");
        }
        if (result.Content == null)
        {
            throw new InvalidOperationException("Response content is null.");
        }
        var content = await result.Content.ReadFromJsonAsync<EntrezSearchResponse>();
        return content!.ESearchResult;
    }
}