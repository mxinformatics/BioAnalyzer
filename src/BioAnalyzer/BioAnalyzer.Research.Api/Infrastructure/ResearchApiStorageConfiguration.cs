using BioAnalyzer.AzureStorage.Contracts;

namespace BioAnalyzer.Research.Api.Infrastructure;

/// <summary>
/// Configuration for Azure Storage used in the Research API.
/// </summary>
public class ResearchApiStorageConfiguration : IAzureStorageConfiguration
{
    public string StorageConnectionString { get; set; } = string.Empty;
    public string DownloadTableName { get; set; } = string.Empty;
    public string DownloadContainerName { get; set; } = string.Empty;
    
    public void ThrowIfInvalid()
    {
        if (string.IsNullOrWhiteSpace(StorageConnectionString))
        {
            throw new ArgumentNullException(nameof(StorageConnectionString));
        }

        if (string.IsNullOrWhiteSpace(DownloadTableName))
        {
            throw new ArgumentNullException(nameof(DownloadTableName));
        }

        if (string.IsNullOrWhiteSpace(DownloadContainerName))
        {
            throw new ArgumentNullException(nameof(DownloadContainerName));
        }
    }
}