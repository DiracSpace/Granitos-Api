namespace Granitos.Common.SecretVault.Providers.Abstractions;

public interface ISecretVaultClient
{
    Task<string> GetSecretAsync(string secretName, bool addPrefix = true);
    Task<string> CreateSecretAsync(string secretName, string secretValue, bool addPrefix = true);
    Task<string> CreateSecretAsync(string secretName, string secretValue, DateTimeOffset offset , bool addPrefix = true);
}