using BioAnalyzer.Research.Api.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace BioAnalyzer.Research.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ChatController(IAIQueryService queryService) : ControllerBase
{
    public async Task<IActionResult> Query([FromQuery] string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return BadRequest("Query cannot be null or empty.");
        }

        var response = await queryService.QueryAsync(query).ConfigureAwait(false);
        return Ok(response);
    }
}