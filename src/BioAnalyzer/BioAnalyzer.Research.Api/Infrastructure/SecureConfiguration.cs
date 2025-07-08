namespace BioAnalyzer.Research.Api.Infrastructure;

/// <summary>
/// Model for reading the secure configuration from the appsettings.json file
/// </summary>
public class SecureConfiguration
{
    public string KeyVaultUrl { get; set; } = string.Empty;
    public bool ExcludeEnvironmentCredential { get; set; } 
    
    /// <summary>
    /// When true configuration will be read from Azure Key Vault
    /// </summary>
    public bool UseKeyVault { get; set; }
    
    public void ThrowIfInvalid()
    {
        if (string.IsNullOrWhiteSpace(KeyVaultUrl))
        {
            throw new ArgumentNullException(nameof(KeyVaultUrl));
        }
    }
}    
