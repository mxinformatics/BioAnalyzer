using Azure;

namespace BioAnalyzer.EventHandlers.Models;

public class DownloadedLiterature : Azure.Data.Tables.ITableEntity
{
    public string PartitionKey { get; set; } = "DownloadedLiterature";
    public string RowKey { get; set; } = Guid.NewGuid().ToString();
    public string DownloadLink { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}