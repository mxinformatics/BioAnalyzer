using Azure;
using Azure.Storage.Blobs;
using BioAnalyzer.AzureStorage.Contracts;
using BioAnalyzer.AzureStorage.Contracts.Exceptions;
using BioAnalyzer.AzureStorage.Contracts.Models;

namespace BioAnalyzer.AzureStorage.Contexts;

/// <summary>
///  Implementation of the Azure Blob Context.
/// </summary>
public class AzureBlobContext(IAzureStorageConfiguration configuration) : IBlobContext
{
    /// <summary>
    /// Upload a single document to the specified container.
    /// </summary>
    /// <param name="document"></param>
    /// <param name="storageContainer"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task Upload(IBlobDocument document, StorageContainer storageContainer)
    {
        var container = new BlobContainerClient(configuration.StorageConnectionString, storageContainer.Name);
        await container.CreateIfNotExistsAsync().ConfigureAwait(false);

        try
        {
            var blobClient = container.GetBlobClient(document.Name);
            if (document is TextDocument textDocument)
            {
                await blobClient.UploadAsync(BinaryData.FromString(textDocument.Content)).ConfigureAwait(false);
            }
            else
            {
                var content = ((ByteDocument)document).Content;
                await blobClient.UploadAsync(new BinaryData(content)).ConfigureAwait(false);
            }

            if (document.Metadata.Count > 0)
            {
                await blobClient.SetMetadataAsync(document.Metadata).ConfigureAwait(false);

            }
        }
        catch (Exception ex)
        {
            if(IsNotFoundException(ex))
            {
                throw new BlobNotFoundException(string.Format(DocumentResources.ContainerNotFound, storageContainer.Name));
            }
            else
            {
                throw;
            }
        }
    }
    
    /// <summary>
    ///  Upload  a collection of documents to the specified container.
    /// </summary>
    /// <param name="documents"></param>
    /// <param name="storageContainer"></param>
    /// <returns></returns>
    public async Task Upload(IEnumerable<IBlobDocument> documents, StorageContainer storageContainer)
    {
        var uploadList = documents.Select(document => Upload(document, storageContainer)).ToList();
        await Task.WhenAll(uploadList).ConfigureAwait(false);
    }

    /// <summary>
    /// Return the text of a document from the specified container.
    /// </summary>
    /// <param name="documentName"></param>
    /// <param name="storageContainer"></param>
    /// <returns></returns>
    public async Task<string> GetDocumentText(string documentName, StorageContainer storageContainer)
    {
        return await DownloadDocumentText(documentName, storageContainer).ConfigureAwait(false);
    }


    /// <summary>
    /// Retrieve the text of a document from the specified container.
    /// </summary>
    /// <param name="blobName"></param>
    /// <param name="storageContainer"></param>
    /// <returns></returns>
    /// <exception cref="BlobNotFoundException"></exception>
    private async Task<string> DownloadDocumentText(string blobName, StorageContainer storageContainer)
    {
        try
        {
            var documentBlobClient = GetDocumentBlobClient(blobName, storageContainer);
            var response = await documentBlobClient.DownloadContentAsync().ConfigureAwait(false);
            return response.Value.Content.ToString();
        }
        catch (RequestFailedException ex)
        {
            if (IsNotFoundException(ex))
            {
                throw new BlobNotFoundException(string.Format(DocumentResources.DocumentNotFound, blobName));
            }

            throw;
        }

    }
    /// <summary>
    /// Returns true if the exception is a 404 or not found exception.
    /// </summary>
    /// <param name="ex"></param>
    /// <returns></returns>
    private bool IsNotFoundException(Exception ex)
    {
        return ex.Message.Contains("404") || ex.Message.Contains("not found", StringComparison.CurrentCultureIgnoreCase);
    }


    /// <summary>
    /// Return a blob client for the specified blob name and container.
    /// </summary>
    /// <param name="blobName"></param>
    /// <param name="storageContainer"></param>
    /// <returns></returns>
    private BlobClient GetDocumentBlobClient(string blobName, StorageContainer storageContainer)
    {
        var blobServiceClient = new BlobServiceClient(configuration.StorageConnectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(storageContainer.Name);
        return containerClient.GetBlobClient(blobName);
    }
}