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
    
    private LiteratureReference? _selectedLiteratureReference;
    private LiteratureReference? _downloadLiteratureReference;
    private bool _searchInProgress = false;
    
    [Inject]
    public ApplicationState ApplicationState { get; set; } = default!;
    private async Task OnSubmit()
    {
        LiteratureReferences.Clear();
        _searchInProgress = true;
        var referenceList = await searchService.Search(SearchCriteria).ConfigureAwait(false);
        _searchInProgress = false;
        LiteratureReferences = referenceList.References.ToList();
        
    }
    
    public void ShowAbstractPopup(LiteratureReference literatureReference)
    {
        _selectedLiteratureReference = literatureReference;
        
    }
    
    public void DownloadReference(LiteratureReference literatureReference)
    {
        _downloadLiteratureReference = literatureReference;
    }
}