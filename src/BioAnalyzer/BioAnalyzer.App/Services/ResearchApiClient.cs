using BioAnalyzer.App.Contracts.Clients;
using BioAnalyzer.App.Models.ResearchApi;


namespace BioAnalyzer.App.Services;

public class ResearchApiClient(HttpClient httpClient) : IResearchApiClient
{
    public async Task<ICollection<string>> GetLiteratureReferenceIds(string searchTerm)
    {
        var requestUri = $"/literature?query={Uri.EscapeDataString(searchTerm)}";
        var result = await httpClient.GetFromJsonAsync<LiteratureSearchResponse>(requestUri).ConfigureAwait(false);
        if (result == null)
        {
            throw new InvalidOperationException("Response content is null.");
        }
        return result.IdList;
    }
}