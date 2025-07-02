using BioAnalyzer.App.Models;

namespace BioAnalyzer.App.Contracts.Services;

public interface ISearchService
{
    Task<IList<LiteratureReference>> Search(SearchCriteria criteria);
    
    Task<LiteratureAbstract> GetAbstract(string pmcId);
}