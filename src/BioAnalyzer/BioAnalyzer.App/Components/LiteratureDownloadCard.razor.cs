using BioAnalyzer.App.Models.ResearchApi;
using Microsoft.AspNetCore.Components;

namespace BioAnalyzer.App.Components;

public partial class LiteratureDownloadCard : ComponentBase
{
    [Parameter] public LiteratureDownload LiteratureDownload { get; set; } = null!;
}