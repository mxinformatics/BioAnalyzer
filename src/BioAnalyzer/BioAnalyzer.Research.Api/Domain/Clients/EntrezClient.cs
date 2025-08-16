using System.Xml;
using BioAnalyzer.Research.Api.Domain.Models;
using BioAnalyzer.Research.Api.Infrastructure;
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

    public async Task<EntrezSearchResult> LiteratureSearchAsync(string query, int startIndex)
    {
        var requestUri = $"{_configuration.EntrezBaseUrl}/esearch.fcgi?db=pubmed&term={Uri.EscapeDataString(query)}&retmode=json&retstart={startIndex}";
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

    public async Task<EntrezSummaryResponse> LiteratureSummaryAsync(IList<string> uids)
    {
        var requestUri = $"{_configuration.EntrezBaseUrl}/esummary.fcgi?db=pubmed&id={string.Join(",", uids)}&retmode=xml";
        var result = await _httpClient.GetAsync(requestUri).ConfigureAwait(false);
        if (!result.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Failed to get literature summary: {result.ReasonPhrase}");
        }
        if (result.Content == null)
        {
            throw new InvalidOperationException("Response content is null.");
        }
        var contentXml  =  await result.Content.ReadAsStringAsync().ConfigureAwait(false);
        // Parse the XML content to handle cases where the response is not in JSON format
        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(contentXml);
        return new EntrezSummaryResponse(xmlDoc);
    }
}