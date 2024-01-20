using System.Text;
using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Granitos.Common.SecretVault.Providers.Abstractions;

namespace Granitos.Common.SecretVault.Providers.AmazonWebServices;

public interface IAwsSecretManagerClient : IGenericVaultClient
{
    Task<string> SetSecret(CreateSecretRequest secret);
}

internal sealed class AwsSecretManagerClient : GenericVaultClient.GenericVaultClient, IAwsSecretManagerClient
{
    private readonly AmazonSecretsManagerClient _amazonSecretsManagerClient;
    
    public AwsSecretManagerClient(SecretVaultOptions secretVaultOptions) 
        : base(secretVaultOptions)
    {
        var region = RegionEndpoint.USEast1;
        
        if (!string.IsNullOrEmpty(secretVaultOptions.Region))
            region = RegionEndpoint.GetBySystemName(secretVaultOptions.Region);
        
        _amazonSecretsManagerClient = new AmazonSecretsManagerClient(region);
    }

    /// <summary>
    /// Gets the secret from the vault or secret manager
    /// returns it to the calling program.
    /// </summary>
    /// <param name="secretName">The name of the secret to retrieve.</param>
    /// <param name="addPrefix"></param>
    /// <returns>A string representing secret.</returns>
    protected override async Task<string> GetSecretAsync(string secretName, bool addPrefix = true)
    {
        try
        {
            GetSecretValueRequest request = new();
            var secretKey = GetKey(secretName, addPrefix);
            request.SecretId = secretKey;
            var response = await _amazonSecretsManagerClient.GetSecretValueAsync(request).ConfigureAwait(false);
            
            if (response is null) 
                return string.Empty;
            
            var secret = DecodeString(response);

            return !string.IsNullOrEmpty(secret) 
                ? secret 
                : string.Empty;
        }
        catch (Exception e)
        {
            throw new ApplicationException($"An error occurred while attempting to get the key-vault secret located at '{secretName}'", e);
        }
    }

    /// <summary>
    /// Creates the secret in the vault or secret manager
    /// returns the name of the secret to the calling program.
    /// </summary>
    /// <param name="secretName">The name of the secret to create.</param>
    /// <param name="secretValue">The value to attach to the secret</param>
    /// <param name="addPrefix"></param>
    /// <returns>A string representing the name of the secret.</returns>
    protected override async Task<string> CreateSecretAsync(string secretName, string secretValue, bool addPrefix = true)
    {
        try
        {
            var secret = new CreateSecretRequest()
            {
                SecretString = secretValue,
                Name = secretName,
                Description = secretName
            };

            return await SetSecret(secret);
        }
        catch (Exception e)
        {
            throw new ApplicationException($"An error occured while attempting to store the keyvault secret for {secretName}", e);
        }
    }

    /// <summary>
    /// Creates the secret in the vault (Expiration with offset is NOT supported with AWS Secret Manager)
    /// returns the name of the secret to the calling program.
    /// </summary>
    /// <param name="secretName">The name of the secret to create.</param>
    /// <param name="secretValue">The value to attach to the secret</param>
    /// <param name="offset">The date time offset after which the secret will expire</param>
    /// <param name="addPrefix"></param>
    /// <returns>A string representing the name of the secret.</returns>
    protected override async Task<string> CreateSecretAsync(string secretName, string secretValue, DateTimeOffset offset, bool addPrefix = true)
    {
        // Expiration date is not supported for AWS secrets.
        return await this.CreateSecretAsync(secretName, secretValue, addPrefix);
    }

    public async Task<string> SetSecret(CreateSecretRequest secret)
    {
        var createResponse = await _amazonSecretsManagerClient.CreateSecretAsync(secret);
        return createResponse.Name;
    }

    #region Private Methods

    private static string DecodeString(GetSecretValueResponse response)
    {
        if (response.SecretString is not null)
        {
            var secret = response.SecretString;
            return secret;
        }

        if (response.SecretBinary is null) 
            return string.Empty;
        
        var memoryStream = response.SecretBinary;
        var reader = new StreamReader(memoryStream);
        var decodedBinarySecret = Encoding.UTF8.GetString(Convert.FromBase64String(reader.ReadToEnd()));
        return decodedBinarySecret;

    }

    #endregion
}