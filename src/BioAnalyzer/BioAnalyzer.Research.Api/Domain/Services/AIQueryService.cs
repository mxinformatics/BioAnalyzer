using BioAnalyzer.Research.Api.Domain.Clients;

namespace BioAnalyzer.Research.Api.Domain.Services;

// ReSharper disable once InconsistentNaming
public class AIQueryService(IAiClient aiClient) : IAIQueryService
{
    public async Task<string> QueryAsync(string query, CancellationToken cancellationToken = default)
    {
        return await aiClient.QueryAsync(query, cancellationToken);
    }
}