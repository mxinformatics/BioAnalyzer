using BioAnalyzer.AzureStorage.Contracts;
using BioAnalyzer.AzureStorage.Contracts.Models;
using BioAnalyzer.Research.Api.Domain.DataTransfer;
using BioAnalyzer.Research.Api.Domain.Models;
using BioAnalyzer.Research.Api.Infrastructure;
using Microsoft.Extensions.Options;

namespace BioAnalyzer.Research.Api.Domain.Clients;

public class StorageClient(ITableContext tableContext, IBlobContext blobContext, IOptions<ResearchApiStorageConfiguration> storageConfiguration) : IStorageClient
{
    private readonly ResearchApiStorageConfiguration _storageConfiguration = storageConfiguration.Value;
        
    public async Task<LiteratureDownloadList> GetDownloadsAsync()
    {
        var downloadList = new LiteratureDownloadList();

        var downloads = await tableContext.GetAllAsync<LiteratureDownloadDto>(_storageConfiguration.DownloadTableName);
        downloadList.Downloads = MapDownloads(downloads);
        return downloadList;
    }

    public async Task<byte[]> DownloadFileAsync(string fileName)
    {
        var document = await blobContext.GetDocumentBytes(fileName, DocumentContentType.Pdf, new StorageContainer(_storageConfiguration.DownloadContainerName));
        return document.Content;
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
                PmcId = download.PmcId,
                Doi = download.Doi
            };
            downloadList.Add(literatureDownload);
        }
        return downloadList;
    }
}