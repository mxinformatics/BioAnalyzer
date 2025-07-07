using BioAnalyzer.AzureStorage.Contracts.Models;

namespace BioAnalyzer.AzureStorage.Contracts;

/// <summary>
/// Contract for blob operations
/// </summary>
public interface IBlobContext
{
    Task Upload(IBlobDocument document, StorageContainer storageContainer);

    Task Upload(IEnumerable<IBlobDocument> documents, StorageContainer storageContainer);
    
    Task<string> GetDocumentText(string documentName, StorageContainer storageContainer);
}
