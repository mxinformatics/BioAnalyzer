using BioAnalyzer.App.Contracts.Clients;
using BioAnalyzer.App.Models;
using BioAnalyzer.App.Models.ResearchApi;


namespace BioAnalyzer.App.Services;

public class ResearchApiClient(HttpClient httpClient) : IResearchApiClient
{
    public async Task<LiteratureSearchResult> GetLiteratureReferences(string searchTerm, int index)
    {
        var requestUri = $"/literature?query={Uri.EscapeDataString(searchTerm)}&startIndex={index}";
        var result = await httpClient.GetFromJsonAsync<LiteratureSearchResponse>(requestUri).ConfigureAwait(false);
        if (result == null)
        {
            throw new InvalidOperationException("Response content is null.");
        }

        return new LiteratureSearchResult
        {
            Count = result.Count,
            ReferenceIds = result.IdList,
            RetMax = result.RetMax,
            RetStart = result.RetStart

        };

    }

    public async Task<IList<LiteratureSummaryResult>> GetLiteratureSummary(IList<string> ids)
    {
        var requestUri = $"/literature/summary?ids={string.Join(",", ids)}";
        var result = await httpClient.GetFromJsonAsync<IList<LiteratureSummaryResult>>(requestUri).ConfigureAwait(false);
        if (result == null)
        {
            throw new InvalidOperationException("Response content is null.");
        }

        return result;
    }

    public async Task<LiteratureAbstract> GetLiteratureAbstract(string pmcId)
    {
        var requestUri = $"/literature/abstract?pmcId={pmcId}";
        var result = await httpClient.GetFromJsonAsync<LiteratureAbstract>(requestUri).ConfigureAwait(false);
        if (result == null)
        {
            throw new InvalidOperationException("Response content is null.");
        }
        return result;
    }

    public async Task<LiteratureDownloadLinkResponse> DownloadReference(LiteratureReference reference)
    {
        var requestUri = $"/literature/download?pmcId={reference.PmcId}";
        var result = await httpClient.GetFromJsonAsync<LiteratureDownloadLinkResponse>(requestUri).ConfigureAwait(false);
        if (result == null)
        {
            throw new InvalidOperationException("Response content is null.");
        }
        return result;
    }

    public async Task<LiteratureDownloadsResponse> GetDownloads()
    {
        var requestUri = "/literature/downloads/view";
        var result = await httpClient.GetFromJsonAsync<LiteratureDownloadsResponse>(requestUri).ConfigureAwait(false);
        if (result == null)
        {
            throw new InvalidOperationException("Response content is null.");
        }

        return result;
    }

    public async Task<byte[]> DownloadFile(string fileName)
    {
        var requestUri = $"/literature/downloads/{fileName}";
        var result = await httpClient.GetByteArrayAsync(requestUri).ConfigureAwait(false);
        return result;
    }
}