using BioAnalyzer.AzureStorage.Contracts;

namespace BioAnalyzer.AzureStorage.Configuration;

/// <summary>
/// Configuration settings for Azure Storage.
/// </summary>
public class AzureStorageConfiguration(string connectionString) : IAzureStorageConfiguration
{
    public string StorageConnectionString { get; } = connectionString;
}