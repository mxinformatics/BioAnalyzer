using BioAnalyzer.App.Models;
using BioAnalyzer.App.Models.ResearchApi;

namespace BioAnalyzer.App.Contracts.Clients;

public interface IResearchApiClient
{
    Task<LiteratureSearchResult> GetLiteratureReferences(string searchTerm, int startIndex);

    Task<IList<LiteratureSummaryResult>> GetLiteratureSummary(IList<string> ids);
    
    Task<LiteratureAbstract> GetLiteratureAbstract(string pmcId);
    
    Task<LiteratureDownloadLinkResponse> DownloadReference(LiteratureReference reference);
    
    Task<LiteratureDownloadsResponse> GetDownloads();
    
    Task<byte[]> DownloadFile(string fileName);
    
}