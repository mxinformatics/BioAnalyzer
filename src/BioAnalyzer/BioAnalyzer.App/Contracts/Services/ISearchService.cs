using BioAnalyzer.App.Models;
using BioAnalyzer.App.Models.ResearchApi;

namespace BioAnalyzer.App.Contracts.Services;

public interface ISearchService
{
    Task<LiteratureReferenceList> Search(SearchCriteria criteria);
    
    Task<LiteratureAbstract> GetAbstract(string pmcId);
    
    Task DownloadReference(LiteratureReference reference);
    
    Task<IList<LiteratureDownload>> GetDownloads();
    
    Task<byte[]> DownloadFile(string fileName);
    
}