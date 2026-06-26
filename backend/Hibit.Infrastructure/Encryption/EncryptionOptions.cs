namespace Hibit.Infrastructure.Encryption;

public class EncryptionOptions
{
    public const string SectionName = "Encryption";

    public string Key { get; set; } = string.Empty;
    public string Iv { get; set; } = string.Empty;
}
