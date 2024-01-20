using Granitos.Common.SecretVault.Providers.Abstractions;

namespace Granitos.Common.SecretVault.Providers.GenericVaultClient;

internal abstract class GenericVaultClient : IGenericVaultClient
{
    private readonly SecretVaultOptions _secretVaultOptions;

    protected GenericVaultClient(SecretVaultOptions secretVaultOptions)
    {
        _secretVaultOptions = secretVaultOptions ?? throw new ArgumentNullException(nameof(secretVaultOptions));
    }

    internal string GetKey(string key, bool addPrefix = true)
    {
        if (string.IsNullOrWhiteSpace(_secretVaultOptions.SecretPrefix))
            return key;

        if (!addPrefix)
            return key;

        return key.Contains(_secretVaultOptions.SecretPrefix)
                ? key
                : $"{_secretVaultOptions.SecretPrefix}-{key}"
            ;
    }

    protected abstract Task<string> GetSecretAsync(string secretName, bool addPrefix = true);

    protected abstract Task<string> CreateSecretAsync(string secretName, string secretValue, bool addPrefix = true);

    protected abstract Task<string> CreateSecretAsync(string secretName, string secretValue, DateTimeOffset offset,
        bool addPrefix = true);
}