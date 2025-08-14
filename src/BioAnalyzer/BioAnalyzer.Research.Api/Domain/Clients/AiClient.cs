using System.ClientModel;
using System.Text;
using Azure;
using Azure.AI.OpenAI;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using BioAnalyzer.Research.Api.Infrastructure;
using Microsoft.Extensions.Options;
using OpenAI.Chat;

namespace BioAnalyzer.Research.Api.Domain.Clients;


public class AiClient(IOptions<OpenAiConfiguration> configuration) : IAiClient
{
    private readonly OpenAiConfiguration _configuration = configuration.Value;
    
    public async Task<string> QueryAsync(string query, CancellationToken cancellationToken = default)
    {
        var indexSearchResult = await QueryDocuments(query, cancellationToken);
        if (!string.IsNullOrWhiteSpace(indexSearchResult))   
        {
            return await QueryOpenAi(query, indexSearchResult, cancellationToken).ConfigureAwait(false);   
        }
        else
        {
            throw new InvalidOperationException("Search of document index returned no results");
        }
    }

    private async Task<string> QueryDocuments(string query, CancellationToken cancellationToken = default)
    {
        var searchClient = new SearchClient(
            new Uri(_configuration.SearchEndpoint),
            _configuration.SearchIndexName, 
            new AzureKeyCredential(_configuration.SearchApiKey)
        );
        
        var searchOptions = new SearchOptions
        {
          
            Size = 5,
            
        };
        var searchResults = await searchClient.SearchAsync<SearchDocument>(query, searchOptions, cancellationToken);
        var sources = new List<string>();
        await foreach (var result in searchResults.Value.GetResultsAsync())
        {
            var doc = result.Document;
            sources.Add(doc["title"] + ": " + doc["content"] + ": " + doc["url"]);
        }

        return string.Join("\n", sources);

    }
    
    private async Task<string> QueryOpenAi(string query, string sources, CancellationToken cancellationToken = default)
    {
        var openAiClient = new AzureOpenAIClient(new Uri(_configuration.Endpoint), new ApiKeyCredential(_configuration.OpenAiApiKey));
        var chatClient = openAiClient.GetChatClient(_configuration.GptName);
        
        var prompt = $"""
                      You are an experienced biological research with deep molecular biology knowledge.
                                  Answer the query using only the sources provided below in a friendly and concise bulleted manner
                                  Answer ONLY with the facts listed in the list of sources below.
                                  If there isn't enough information below, say you don't know.
                                  Do not generate answers that don't use the sources below.
                                  Query: {query}
                                  Sources: {sources}
                      """;
        
        var chatUpdates = chatClient.CompleteChatStreamingAsync(
            [ new UserChatMessage(prompt) ], 
            new ChatCompletionOptions
            {
              Temperature  = 0.0f,
              MaxOutputTokenCount = 1000
            },
            cancellationToken: cancellationToken);
        
        var response = new StringBuilder(500);
        await foreach (var chatUpdate in chatUpdates)
        {
            foreach (var contentPart in chatUpdate.ContentUpdate)
            {
                response.Append(contentPart.Text);
            }
        }
        
        return response.ToString();
    }
}