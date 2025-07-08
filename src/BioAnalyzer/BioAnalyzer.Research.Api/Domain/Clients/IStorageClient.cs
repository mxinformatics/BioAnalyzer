using BioAnalyzer.Research.Api.Domain.Models;

namespace BioAnalyzer.Research.Api.Domain.Clients;

public interface IStorageClient
{
    Task<LiteratureDownloadList> GetDownloadsAsync();
    
    Task<byte[]> DownloadFileAsync(string fileName);
}