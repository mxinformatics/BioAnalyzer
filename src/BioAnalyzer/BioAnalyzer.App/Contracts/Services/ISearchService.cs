using BioAnalyzer.App.Models;
using BioAnalyzer.App.Models.ResearchApi;

namespace BioAnalyzer.App.Contracts.Services;

public interface ISearchService
{
    Task<IList<LiteratureReference>> Search(SearchCriteria criteria);
    
    Task<LiteratureAbstract> GetAbstract(string pmcId);
    
    Task DownloadReference(LiteratureReference reference);
    
    Task<IList<LiteratureDownload>> GetDownloads();
    
    
}