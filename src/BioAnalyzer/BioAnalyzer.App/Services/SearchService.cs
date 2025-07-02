using BioAnalyzer.App.Contracts.Clients;
using BioAnalyzer.App.Contracts.Services;
using BioAnalyzer.App.Models;

namespace BioAnalyzer.App.Services;

public class SearchService(IResearchApiClient researchApiClient) : ISearchService
{
    public async Task<IList<LiteratureReference>> Search(SearchCriteria criteria)
    {
        if (string.IsNullOrWhiteSpace(criteria.SearchTerm))
        {
            return new List<LiteratureReference>();
        }
        var literatureReferenceIds = await researchApiClient.GetLiteratureReferenceIds(criteria.SearchTerm);

        if (literatureReferenceIds.Count > 0)
        {
            var literatureSummaries = await researchApiClient.GetLiteratureSummary(literatureReferenceIds.ToList());
            return literatureSummaries.Select(summary => 
                new LiteratureReference{ Id  = summary.Uid, Title = summary.Title, Doi =  summary.Doi, PmcId = summary.PmcId}).ToList();    
        }
        else
        {
            return new List<LiteratureReference>();
        }
        
        
    }

    public async Task<LiteratureAbstract> GetAbstract(string pmcId)
    {
        return await researchApiClient.GetLiteratureAbstract(pmcId).ConfigureAwait(false);
    }
}