using BioAnalyzer.App.Contracts.Services;
using BioAnalyzer.App.Models;
using Microsoft.AspNetCore.Components;

namespace BioAnalyzer.App.Components;

public partial class AbstractPopup(ISearchService searchService) : ComponentBase
{
    [Parameter]
    public LiteratureReference? LiteratureReference { get; set; }
    private LiteratureReference? _literatureReference;
    public LiteratureAbstract? Abstract { get; set; }

    
    protected override void OnParametersSet()
    {
        _literatureReference = LiteratureReference;
        if(_literatureReference != null)
        {
            Abstract = searchService.GetAbstract(_literatureReference.PmcId).ConfigureAwait(false).GetAwaiter().GetResult();    
        }

    }
    
    public void Close()
    {
        Abstract = null;
        _literatureReference = null;
    }
}