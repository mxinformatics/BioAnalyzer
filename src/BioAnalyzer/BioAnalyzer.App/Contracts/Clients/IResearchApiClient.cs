using BioAnalyzer.App.Models;
using BioAnalyzer.App.Models.ResearchApi;

namespace BioAnalyzer.App.Contracts.Clients;

public interface IResearchApiClient
{
    Task<ICollection<string>> GetLiteratureReferenceIds(string searchTerm);

    Task<IList<LiteratureSummaryResult>> GetLiteratureSummary(IList<string> ids);
    
    Task<LiteratureAbstract> GetLiteratureAbstract(string pmcId);
    
    Task<LiteratureDownloadLinkResponse> DownloadReference(LiteratureReference reference);
    
    
}