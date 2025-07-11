using BioAnalyzer.Research.Api.Domain.Models;
using BioAnalyzer.Research.Api.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace BioAnalyzer.Research.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class LiteratureController(ILiteratureSearchService literatureSearchService, ILiteratureService literatureService) : ControllerBase
{

    
    [HttpGet(Name = "SearchLiterature")]
    [ProducesResponseType(typeof(EntrezSearchResult), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Search(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return BadRequest("Query cannot be null or empty.");
        }
        var result = await literatureSearchService.SearchLiteratureAsync(query).ConfigureAwait(false);
        return Ok(result);
    }
    
    [HttpGet("summary", Name = "GetLiteratureSummary")]
    [ProducesResponseType(typeof(IList<EntrezSummaryResult>), 200)]
    [ProducesResponseType(500)]
    public async Task<IList<EntrezSummaryResult>> GetSummary([FromQuery] IList<string> ids)
    {
        if (!ids.Any())
        {
            return new List<EntrezSummaryResult>();
        }
        
        var result = await literatureSearchService.GetLiteratureSummaries(ids).ConfigureAwait(false);
        return result;
    }
    
    [HttpGet("abstract", Name = "GetLiteratureAbstract")]
    [ProducesResponseType(typeof(ArticleAbstract), 200)]
    [ProducesResponseType(500)]
    public async Task<ArticleAbstract> GetAbstract([FromQuery] string pmcId)
    {
        var articleAbstract = await literatureSearchService.GetArticleAbstractAsync(pmcId).ConfigureAwait(false); 
        return articleAbstract;
    }
    
    [HttpGet("download", Name = "DownloadLiteratureReference")]
    [ProducesResponseType(typeof(LiteratureDownloadLinkResult), 200)]
    [ProducesResponseType(500)]
    public async Task<LiteratureDownloadLinkResult> DownloadReference([FromQuery] string pmcId)
    {
        var downloadLinkResponse = await literatureSearchService.GetLiteratureDownloadLinkAsync(pmcId).ConfigureAwait(false);
        return downloadLinkResponse;
    }
    
    [HttpGet("downloads/view", Name = "ViewDownloads")]
    [ProducesResponseType(typeof(LiteratureDownloadList), 200)]
    [ProducesResponseType(500)]
    public async Task<LiteratureDownloadList> ViewDownloads()
    {
        var downloadList = await literatureService.GetDownloadsAsync().ConfigureAwait(false);
        return downloadList;
    }

    [HttpGet("downloads/{fileName}", Name = "DownloadFile")]
    [ProducesResponseType(typeof(FileContentResult), 200)]
    [ProducesResponseType(500)]
    public async Task<FileContentResult> DownloadFile([FromRoute] string fileName)
    {
        var fileBytes = await literatureService.DownloadFileAsync(fileName).ConfigureAwait(false);
        return new FileContentResult(fileBytes, System.Net.Mime.MediaTypeNames.Application.Pdf);
        
    }
}