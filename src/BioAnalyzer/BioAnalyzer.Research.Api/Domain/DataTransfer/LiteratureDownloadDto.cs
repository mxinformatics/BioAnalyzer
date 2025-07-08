using Azure;
using Azure.Data.Tables;

namespace BioAnalyzer.Research.Api.Domain.DataTransfer;

public class LiteratureDownloadDto : ITableEntity
{
    public string DownloadLink { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string PmcId { get; set; } = string.Empty;
    public string Doi { get; set; } = string.Empty;
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}