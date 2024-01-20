using Granitos.Common.SecretVault.Models;

namespace Granitos.Common.SecretVault;

public sealed record SecretVaultOptions(
    CloudProviders CloudProvider,
    string? Url,
    string? Region,
    string? SecretBaseNamingConvention,
    string? SecretPrefix)
{
    public const string CloudProviderKeyName = "CloudProvider";
    public const string SectionName = "SecretVault";
}