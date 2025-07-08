using BioAnalyzer.AzureStorage.Contracts;
using BioAnalyzer.Research.Api.Domain.DataTransfer;
using BioAnalyzer.Research.Api.Domain.Models;
using BioAnalyzer.Research.Api.Infrastructure;
using Microsoft.Extensions.Options;

namespace BioAnalyzer.Research.Api.Domain.Clients;

public class StorageClient(ITableContext tableContext, IOptions<ResearchApiStorageConfiguration> storageConfiguration) : IStorageClient
{
    private readonly ResearchApiStorageConfiguration _storageConfiguration = storageConfiguration.Value;
        
    public async Task<LiteratureDownloadList> GetDownloadsAsync()
    {
        var downloadList = new LiteratureDownloadList();

        var downloads = await tableContext.GetAllAsync<LiteratureDownloadDto>(_storageConfiguration.DownloadTableName);
        downloadList.Downloads = MapDownloads(downloads);
        return downloadList;
    }
    
    private IList<LiteratureDownload> MapDownloads(IList<LiteratureDownloadDto> downloads)
    {
        var downloadList = new List<LiteratureDownload>();
        foreach (var download in downloads)
        {
            var literatureDownload = new LiteratureDownload
            {
                DownloadLink = download.DownloadLink,
                FileName = download.FileName,
                Title = download.Title,
            };
            downloadList.Add(literatureDownload);
        }
        return downloadList;
    }
}