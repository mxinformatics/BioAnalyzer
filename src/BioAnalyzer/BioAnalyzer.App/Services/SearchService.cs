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
        return literatureReferenceIds.Select(refId => new LiteratureReference{ Id = refId}).ToList();
    }
}