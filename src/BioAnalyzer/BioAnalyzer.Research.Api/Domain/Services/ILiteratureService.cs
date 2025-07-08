using BioAnalyzer.Research.Api.Domain.Models;

namespace BioAnalyzer.Research.Api.Domain.Services;

public interface ILiteratureService
{
    Task<LiteratureDownloadList> GetDownloadsAsync();
}