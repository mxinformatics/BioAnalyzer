using BioAnalyzer.App.Contracts.Clients;
using BioAnalyzer.App.Contracts.Services;
using BioAnalyzer.App.Models;
using BioAnalyzer.App.Models.Messages;
using BioAnalyzer.App.Models.ResearchApi;


namespace BioAnalyzer.App.Services;

public class SearchService(IResearchApiClient researchApiClient, IEventBusClient eventBusClient) : ISearchService
{
    public async Task<LiteratureReferenceList> Search(SearchCriteria criteria)
    {
        if (string.IsNullOrWhiteSpace(criteria.SearchTerm))
        {
            return new LiteratureReferenceList();
        }
        var searchResult = await researchApiClient.GetLiteratureReferences(criteria.SearchTerm);

        if (searchResult.Count > 0)
        {
            var literatureSummaries = await researchApiClient.GetLiteratureSummary(searchResult.ReferenceIds.ToList());
            var references = literatureSummaries.Select(summary => 
                new LiteratureReference{ Id  = summary.Uid, Title = summary.Title, Doi =  summary.Doi, PmcId = summary.PmcId}).ToList();
            return new LiteratureReferenceList
            {
                Count = searchResult.Count,
                RetMax = searchResult.RetMax,
                RetStart = searchResult.RetStart,
                References = references

            };
        }
        else
        {
            return new LiteratureReferenceList();
        }
        
        
    }

    public async Task<LiteratureAbstract> GetAbstract(string pmcId)
    {
        return await researchApiClient.GetLiteratureAbstract(pmcId).ConfigureAwait(false);
    }

    public async Task DownloadReference(LiteratureReference reference)
    {
      var downloadLinkResponse = await researchApiClient.DownloadReference(reference).ConfigureAwait(false);
      var downloadRequest = new LiteratureDownloadRequest(downloadLinkResponse.PmcId, downloadLinkResponse.DownloadLink, reference.Title, reference.Doi);
        if (!string.IsNullOrWhiteSpace(downloadRequest.DownloadLink))
        {
            await eventBusClient.Publish(new List<LiteratureDownloadRequest> { downloadRequest }).ConfigureAwait(false);
        }
        else
        {
            throw new InvalidOperationException($"No valid download link found for literature reference {reference.Id}");
        }
     
    }

    public async Task<IList<LiteratureDownload>> GetDownloads()
    {
        var response =  await researchApiClient.GetDownloads().ConfigureAwait(false);
        return response.Downloads;
    }

    public async Task<byte[]> DownloadFile(string fileName)
    {
        return await researchApiClient.DownloadFile(fileName).ConfigureAwait(false);
    }
}