using BioAnalyzer.App.Contracts.Services;
using BioAnalyzer.App.Models;
using BioAnalyzer.App.Models.ResearchApi;
using BioAnalyzer.App.State;
using Microsoft.AspNetCore.Components;

namespace BioAnalyzer.App.Components;

public partial class LiteratureSearch(ISearchService searchService) : ComponentBase
{
    
    [SupplyParameterFromForm]
    public SearchCriteria SearchCriteria { get; set; } = new SearchCriteria();

    public LiteratureReferenceList SearchResults { get; set; } = new LiteratureReferenceList();
    
    private LiteratureReference? _selectedLiteratureReference;
    private LiteratureReference? _downloadLiteratureReference;
    private bool _searchInProgress = false;
    
    [Inject]
    public ApplicationState ApplicationState { get; set; } = default!;
    private async Task OnSubmit()
    {
        SearchCriteria.StartIndex = 0;
        await Search();
    }

    private async Task Search()
    {
        SearchResults = new LiteratureReferenceList();
        _searchInProgress = true;
        var referenceList = await searchService.Search(SearchCriteria).ConfigureAwait(false);
        _searchInProgress = false;
        SearchResults = referenceList;
    }


    public async Task SearchNext()
    {
        SearchCriteria.StartIndex += 20;
        await Search();
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