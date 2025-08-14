namespace BioAnalyzer.Research.Api.Domain.Services;

// ReSharper disable once InconsistentNaming
public interface IAIQueryService
{
    Task<string> QueryAsync(string query, CancellationToken cancellationToken = default);
}