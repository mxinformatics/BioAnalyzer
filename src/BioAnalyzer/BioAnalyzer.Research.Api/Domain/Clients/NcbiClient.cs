using System.Xml;
using BioAnalyzer.Research.Api.Domain.Models;
using Microsoft.Extensions.Options;

namespace BioAnalyzer.Research.Api.Domain.Clients;

public class NcbiClient(HttpClient httpClient,IOptions<ResearchApiConfiguration> apiConfiguration) : INcbiClient
{
    private readonly ResearchApiConfiguration _researchApiConfiguration = apiConfiguration.Value;
    public async Task<NcbiArticleResponse> GetArticleAsync(string pmCid)
    {
        var requestUri = $"{_researchApiConfiguration.NcbiBaseUrl}?verb=GetRecord&identifier=oai:pubmedcentral.nih.gov:{pmCid}&metadataPrefix=oai_dc";
        var result =await httpClient.GetAsync(requestUri).ConfigureAwait(false);

        if (!result.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Failed to get article: {result.ReasonPhrase}");
        }

        var content = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(content);
        return new NcbiArticleResponse(xmlDoc);
    }
}