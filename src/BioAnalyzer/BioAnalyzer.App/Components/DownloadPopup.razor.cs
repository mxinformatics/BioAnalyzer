using BioAnalyzer.App.Contracts.Services;
using BioAnalyzer.App.Models;
using Microsoft.AspNetCore.Components;

namespace BioAnalyzer.App.Components;

public partial class DownloadPopup(ISearchService searchService) : ComponentBase
{
    [Parameter]
    public LiteratureReference? LiteratureReference { get; set; }
    private LiteratureReference? _literatureReference;
    
    protected override void OnParametersSet()
    {
        _literatureReference = LiteratureReference;
        if (_literatureReference is not { CanDownload: true }) return;
        
        searchService.DownloadReference(_literatureReference).ConfigureAwait(false).GetAwaiter().GetResult();
        _literatureReference = null;

    }

    public void Close()
    {
        _literatureReference = null;
    }
}