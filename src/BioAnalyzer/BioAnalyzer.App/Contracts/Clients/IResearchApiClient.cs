namespace BioAnalyzer.App.Contracts.Clients;

public interface IResearchApiClient
{
    Task<ICollection<string>> GetLiteratureReferenceIds(string searchTerm);
}