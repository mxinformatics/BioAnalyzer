using BioAnalyzer.Research.Api.Domain.Clients;
using BioAnalyzer.Research.Api.Domain.Models;

namespace BioAnalyzer.Research.Api.Domain.Services;

public class LiteratureService(IStorageClient storageClient) : ILiteratureService
{
    public async Task<LiteratureDownloadList> GetDownloadsAsync()
    {
        return await storageClient.GetDownloadsAsync().ConfigureAwait(false);
    }
}