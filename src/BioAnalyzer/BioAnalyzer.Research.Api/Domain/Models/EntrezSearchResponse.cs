using System.Text.Json.Serialization;

namespace BioAnalyzer.Research.Api.Domain.Models;

public class EntrezSearchResponse
{
    [JsonPropertyName("esearchresult")]
    public EntrezSearchResult ESearchResult { get; set; } = new EntrezSearchResult();
}