using BioAnalyzer.App.Contracts.Services;
using BioAnalyzer.App.Models;
using BioAnalyzer.App.State;
using Microsoft.AspNetCore.Components;

namespace BioAnalyzer.App.Components;

public partial class LiteratureSearch(ISearchService searchService) : ComponentBase
{
    
    [SupplyParameterFromForm]
    public SearchCriteria SearchCriteria { get; set; } = new SearchCriteria();

    public IList<LiteratureReference> LiteratureReferences { get; set; } = new List<LiteratureReference>();
    [Inject]
    public ApplicationState ApplicationState { get; set; } = default!;
    private async Task OnSubmit()
    {
        LiteratureReferences.Clear();
        var references = await searchService.Search(SearchCriteria).ConfigureAwait(false);
        LiteratureReferences = references;
        
    }
}