using BioAnalyzer.App.Contracts.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BioAnalyzer.App.Components.Pages;

public partial class DownloadFile(ISearchService searchService) : ComponentBase
{
    [Inject]
    IJSRuntime JsRuntime { get; set; } = null!;
    
    [Parameter]
    public string FileName { get; set; } = string.Empty;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (string.IsNullOrWhiteSpace(FileName))
        {
            return;
        }

        var response = await searchService.DownloadFile(FileName).ConfigureAwait(false);
        var responseStream = new MemoryStream(response);
        using var streamRef = new DotNetStreamReference(responseStream);
        await JsRuntime.InvokeVoidAsync("downloadFileFromStream", FileName, streamRef).ConfigureAwait(false);
    }
}