using System.Xml;

namespace BioAnalyzer.Research.Api.Domain.Models;

public class NcbiArticleResponse(XmlDocument xmlDoc)
{
    public string Title => xmlDoc.SelectSingleNode("/*[local-name()='OAI-PMH']/*[local-name()='GetRecord']/*[local-name()='record']/*[local-name()='metadata']/*[local-name()='dc']/*[local-name()='title']")?.InnerText ?? string.Empty;
    
    public string Description => xmlDoc.SelectSingleNode("/*[local-name()='OAI-PMH']/*[local-name()='GetRecord']/*[local-name()='record']/*[local-name()='metadata']/*[local-name()='dc']/*[local-name()='description']")?.InnerText ?? string.Empty;
}