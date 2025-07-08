using Azure.Data.Tables;

namespace BioAnalyzer.AzureStorage.Contracts;

public interface ITableContext
{
    Task<IList<TEntityType>> GetAllAsync<TEntityType>(string tableName)
        where TEntityType : class, ITableEntity, new();
    
    
}