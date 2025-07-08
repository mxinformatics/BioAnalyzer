using System.Xml;
using BioAnalyzer.Research.Api.Domain.Models;
using BioAnalyzer.Research.Api.Infrastructure;
using Microsoft.Extensions.Options;

namespace BioAnalyzer.Research.Api.Domain.Clients;

public class NcbiClient(HttpClient httpClient,IOptions<ResearchApiConfiguration> apiConfiguration) : INcbiClient
{
    private readonly ResearchApiConfiguration _researchApiConfiguration = apiConfiguration.Value;
    public async Task<NcbiArticleResponse> GetArticleAsync(string pmCid)
    {
        var requestUri = $"{_researchApiConfiguration.NcbiBaseUrl}oai/oai.cgi?verb=GetRecord&identifier=oai:pubmedcentral.nih.gov:{pmCid}&metadataPrefix=oai_dc";
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

    public async Task<NcbiDownloadResponse> GetLiteratureDownloadLinkAsync(string pmcId)
    {
        var requestUri = $"{_researchApiConfiguration.NcbiBaseUrl}utils/oa/oa.fcgi?id=PMC{pmcId}";
        var result = await httpClient.GetAsync(requestUri).ConfigureAwait(false);
        if (!result.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Failed to get literature download link: {result.ReasonPhrase}");
            
        }
        var content = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(content);
        return new NcbiDownloadResponse(pmcId, xmlDoc);
    }
}