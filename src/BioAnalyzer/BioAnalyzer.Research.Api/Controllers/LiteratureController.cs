using BioAnalyzer.Research.Api.Domain.Models;
using BioAnalyzer.Research.Api.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace BioAnalyzer.Research.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class LiteratureController(ILiteratureSearchService literatureSearchService) : ControllerBase
{

    
    [HttpGet(Name = "SearchLiterature")]
    public async Task<EntrezSearchResult> Search(string query)
    {
        var result = await literatureSearchService.SearchLiteratureAsync(query).ConfigureAwait(false);
        return result;
    }
    
    [HttpGet("summary", Name = "GetLiteratureSummary")]
    public async Task<IList<EntrezSummaryResult>> GetSummary([FromQuery] IList<string> ids)
    {
        if (ids == null || !ids.Any())
        {
            return new List<EntrezSummaryResult>();
        }
        
        var result = await literatureSearchService.GetLiteratureSummaries(ids).ConfigureAwait(false);
        return result;
    }
    
}