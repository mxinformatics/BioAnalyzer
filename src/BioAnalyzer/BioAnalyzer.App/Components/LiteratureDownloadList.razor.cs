using BioAnalyzer.App.Contracts.Services;
using BioAnalyzer.App.Models.ResearchApi;
using Microsoft.AspNetCore.Components;

namespace BioAnalyzer.App.Components;

public partial class LiteratureDownloadList(ISearchService searchService) : ComponentBase
{
    public IList<LiteratureDownload> Downloads { get; set; } = new List<LiteratureDownload>();

    protected override async Task OnInitializedAsync()
    {
        Downloads = await searchService.GetDownloads().ConfigureAwait(false);
    }
}