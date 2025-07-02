using BioAnalyzer.App.Contracts.Services;
using BioAnalyzer.App.Models;
using Microsoft.AspNetCore.Components;

namespace BioAnalyzer.App.Components;

public partial class LiteratureSearch(ISearchService searchService) : ComponentBase
{
    
    [SupplyParameterFromForm]
    public SearchCriteria SearchCriteria { get; set; } = new SearchCriteria();

    
    private async Task OnSubmit()
    {
        var references = await searchService.Search(SearchCriteria).ConfigureAwait(false);
        var refCount = references.Count;
    }
}