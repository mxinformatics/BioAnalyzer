using System.Xml;

namespace BioAnalyzer.Research.Api.Domain.Models;

public class EntrezSummaryResponse
{
    // Parse DocSum nodes from the XML document extracting the Id element value and the Item with a Name of "Title".
    public EntrezSummaryResponse(XmlDocument xmlDoc)
    {
        //if (xmlDoc == null) return;

        foreach (XmlNode docSumNode in xmlDoc.SelectNodes("//DocSum"))
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
            Results.Add(result);
        }
    }
    public IList<EntrezSummaryResult> Results { get; set; } = new List<EntrezSummaryResult>();
}