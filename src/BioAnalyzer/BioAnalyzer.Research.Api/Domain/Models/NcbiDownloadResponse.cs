using System.Xml;

namespace BioAnalyzer.Research.Api.Domain.Models;

public class NcbiDownloadResponse(string pmcId, XmlDocument xmlDoc)
{
    
    public string PmcId { get; } = pmcId;
    public XmlNode? DownloadElementText = xmlDoc.SelectSingleNode("/OA/records/record/link[@format='tgz']"); 

    public string ArchiveLink => xmlDoc.SelectSingleNode("/OA/records/record/link[@format='tgz']") is XmlElement linkElement
        ? linkElement.GetAttribute("href")
        : string.Empty;
    public string PdfLink => xmlDoc.SelectSingleNode("/OA/records/record/link[@format='pdf']") is XmlElement linkElement
        ? linkElement.GetAttribute("href")
        : string.Empty;
}