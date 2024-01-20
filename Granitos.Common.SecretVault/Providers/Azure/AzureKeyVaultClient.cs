using System.Configuration;
using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Granitos.Common.SecretVault.Providers.Abstractions;

namespace Granitos.Common.SecretVault.Providers.Azure;

public interface IAzureKeyVaultClient : IGenericVaultClient
{
    Task<string> SetSecret(KeyVaultSecret secret);
}

internal sealed class AzureKeyVaultClient : GenericVaultClient.GenericVaultClient, IAzureKeyVaultClient
{
    private static DefaultAzureCredential DefaultAzureCredential => new DefaultAzureCredential();
    private readonly SecretClient _secretClient;

    public AzureKeyVaultClient(SecretVaultOptions secretVaultOptions)
        : base(secretVaultOptions)
    {
        var clientOptions = new SecretClientOptions
        {
            Retry =
            {
                Delay = TimeSpan.FromSeconds(2),
                MaxDelay = TimeSpan.FromSeconds(16),
                MaxRetries = 5,
                Mode = RetryMode.Exponential,
            },
        };

        if (string.IsNullOrEmpty(secretVaultOptions.Url))
            throw new ConfigurationErrorsException($"Azure Keyvault URL should be set.");

        _secretClient = new SecretClient(new Uri(secretVaultOptions.Url), DefaultAzureCredential, clientOptions);
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
            var secretKey = GetKey(secretName, addPrefix);
            var secretResponse = await _secretClient.GetSecretAsync(secretKey).ConfigureAwait(false);

            return secretResponse.Value.Value;
        }
        catch (Exception e)
        {
            throw new ApplicationException(
                $"An error occurred while attempting to get the key-vault secret located at '{secretName}'", e);
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
    protected override async Task<string> CreateSecretAsync(string secretName, string secretValue,
        bool addPrefix = true)
    {
        try
        {
            var secretKey = GetKey(secretName, addPrefix);
            var secret = new KeyVaultSecret(secretKey, secretValue)
            {
                Properties =
                {
                    // Set expiration date to a default value of 2 year from current date to comply with the policy.
                    ExpiresOn = DateTimeOffset.Now.AddYears(2)
                }
            };
            return await SetSecret(secret);
        }
        catch (Exception e)
        {
            throw new ApplicationException(
                $"An error occured while attempting to store the keyvault secret for {secretName}", e);
        }
    }

    /// <summary>
    /// Creates the secret in the vault (Not supported with AWS Secret Manager)
    /// returns the name of the secret to the calling program.
    /// </summary>
    /// <param name="secretName">The name of the secret to create.</param>
    /// <param name="secretValue">The value to attach to the secret</param>
    /// <param name="offset">The date time offset after which the secret will expire</param>
    /// <param name="addPrefix"></param>
    /// <returns>A string representing the name of the secret.</returns>
    protected override async Task<string> CreateSecretAsync(string secretName, string secretValue,
        DateTimeOffset offset,
        bool addPrefix = true)
    {
        try
        {
            var secretKey = GetKey(secretName, addPrefix);
            var secret = new KeyVaultSecret(secretKey, secretValue)
            {
                Properties =
                {
                    ExpiresOn = offset
                }
            };
            return await SetSecret(secret);
        }
        catch (Exception e)
        {
            throw new ApplicationException(
                $"An error occured while attempting to store the keyvault secret for {secretName}", e);
        }
    }

    public async Task<string> SetSecret(KeyVaultSecret secret)
    {
        var response = await _secretClient.SetSecretAsync(secret);
        return response.Value.Name;
    }
}