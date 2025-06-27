using BioAnalyzer.Research.Api.Domain.Models;
using BioAnalyzer.Research.Api.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace BioAnalyzer.Research.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class LiteratureController(ILiteratureSearchService literatureSearchService) : ControllerBase
{

    [HttpGet(Name = "Search")]
    public async Task<EntrezSearchResult> Search(string query)
    {
        var result = await literatureSearchService.SearchLiteratureAsync(query).ConfigureAwait(false);
        return result;
    }
}