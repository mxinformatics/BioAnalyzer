using BioAnalyzer.Research.Api.Domain.Models;

namespace BioAnalyzer.Research.Api.Domain.Services;

/// <summary>
/// Contract for searching literature in the Entrez database.
/// </summary>
public interface ILiteratureSearchService
{
    Task<EntrezSearchResult> SearchLiteratureAsync(string query);
    Task<IList<EntrezSummaryResult>> GetLiteratureSummaries(IList<string> uids);
    
    Task<ArticleAbstract> GetArticleAbstractAsync(string pmCid);
    
    Task<LiteratureDownloadLinkResult> GetLiteratureDownloadLinkAsync(string pmCid);
}