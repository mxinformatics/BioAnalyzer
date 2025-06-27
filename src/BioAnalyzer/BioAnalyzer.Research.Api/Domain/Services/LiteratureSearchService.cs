using BioAnalyzer.Research.Api.Domain.Clients;
using BioAnalyzer.Research.Api.Domain.Models;

namespace BioAnalyzer.Research.Api.Domain.Services;

/// <summary>
/// Implementation of the service for searching literature in the Entrez database.
/// </summary>
public class LiteratureSearchService(IEntrezClient client) : ILiteratureSearchService
{
    public async Task<EntrezSearchResult> SearchLiteratureAsync(string query)
    {
        return await client.LiteratureSearchAsync(query).ConfigureAwait(false);
    }

    public async Task<IList<EntrezSummaryResult>> GetLiteratureSummaries(IList<string> uids)
    {
        var response = await client.LiteratureSummaryAsync(uids).ConfigureAwait(false);
        return response.Results;
    }
}