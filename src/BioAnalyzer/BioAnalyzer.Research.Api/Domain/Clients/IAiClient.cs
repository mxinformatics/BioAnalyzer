namespace BioAnalyzer.Research.Api.Domain.Clients;

public interface IAiClient
{
    Task<string> QueryAsync(string query, CancellationToken cancellationToken= default); 
}