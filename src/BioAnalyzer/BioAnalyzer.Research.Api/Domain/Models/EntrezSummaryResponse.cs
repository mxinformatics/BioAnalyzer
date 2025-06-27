using System.Xml;

namespace BioAnalyzer.Research.Api.Domain.Models;

public class EntrezSummaryResponse
{
    // Parse DocSum nodes from the XML document extracting the Id element value and the Item with a Name of "Title".
    public EntrezSummaryResponse(XmlDocument xmlDoc)
    {
        var documents = xmlDoc.SelectNodes("//DocSum");
        if (documents == null)
        {
            return;
        }
        
        foreach (XmlNode docSumNode in documents)
        {
            var result = new EntrezSummaryResult();
            var uidNode = docSumNode.SelectSingleNode("Id");
            if (uidNode != null)
            {
                
                result.Uid = uidNode.InnerText;
            }

            var titleNode = docSumNode.SelectSingleNode("Item[@Name='Title']");
            if (titleNode != null)
            {
                result.Title = titleNode.InnerText;
            }
            
            var articleIds = docSumNode.SelectSingleNode("Item[@Name='ArticleIds']");
            var pmcIdNode = articleIds?.SelectSingleNode("Item[@Name='pmc']");
            if (pmcIdNode != null)
            {
                result.SetPmcId(pmcIdNode.InnerText);
            }
            var doiNode = articleIds?.SelectSingleNode("Item[@Name='doi']");
            if (doiNode != null)
            {
                result.Doi = doiNode.InnerText;
            }
            Results.Add(result);
        }
    }
    public IList<EntrezSummaryResult> Results { get; set; } = new List<EntrezSummaryResult>();
}