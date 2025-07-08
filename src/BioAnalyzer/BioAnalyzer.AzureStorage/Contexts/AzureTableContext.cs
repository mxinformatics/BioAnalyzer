using Azure;
using Azure.Data.Tables;
using Azure.Data.Tables.Models;
using BioAnalyzer.AzureStorage.Contracts;

namespace BioAnalyzer.AzureStorage.Contexts;

public class AzureTableContext(IAzureStorageConfiguration storageConfiguration) : ITableContext
{
    public async Task<IList<TEntityType>> GetAllAsync<TEntityType>(string tableName) where TEntityType : class, ITableEntity, new()
    {
        var serviceClient = new TableServiceClient(storageConfiguration.StorageConnectionString);
        var tableClient = serviceClient.GetTableClient(tableName);
        var queryResults = tableClient.QueryAsync<TEntityType>(filter: "", maxPerPage:20);
            
        var results = new List<TEntityType>();
        await foreach (var result in queryResults.AsPages().ConfigureAwait(false))
        {
            
            if (result.Values.Count > 0)
            {
               var x = result.Values[0];
                foreach (var entity in result.Values)
                {
                    results.Add(entity);
                    
                }
            }
            
        }
        return results;
    }
}


