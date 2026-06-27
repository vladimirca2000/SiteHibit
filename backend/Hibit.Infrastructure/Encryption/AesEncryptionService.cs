using System.Security.Cryptography;
using System.Text;
using Hibit.Application.Common.Interfaces;
using Microsoft.Extensions.Options;

namespace Hibit.Infrastructure.Encryption;

public class AesEncryptionService : IEncryptionService
{
    private readonly byte[] _key;
    private readonly byte[] _iv;

    public AesEncryptionService(IOptions<EncryptionOptions> options)
    {
        var settings = options.Value;

        if (string.IsNullOrWhiteSpace(settings.Key) || string.IsNullOrWhiteSpace(settings.Iv))
        {
            throw new InvalidOperationException("Encryption key and IV must be configured.");
        }

        try
        {
            _key = Convert.FromBase64String(settings.Key);
            _iv = Convert.FromBase64String(settings.Iv);
        }
        catch (FormatException ex)
        {
            throw new InvalidOperationException(
                "Encryption key and IV must be valid Base64 strings.",
                ex);
        }

        if (_key.Length != 32)
        {
            throw new InvalidOperationException("Encryption key must be 32 bytes (AES-256).");
        }

        if (_iv.Length != 16)
        {
            throw new InvalidOperationException("Encryption IV must be 16 bytes.");
        }
    }

    public string Encrypt(string plainText)
    {
        using var aes = Aes.Create();
        aes.Key = _key;
        aes.IV = _iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using var encryptor = aes.CreateEncryptor();
        var plainBytes = Encoding.UTF8.GetBytes(plainText);
        var cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
        return Convert.ToBase64String(cipherBytes);
    }

    public string Decrypt(string cipherText)
    {
        using var aes = Aes.Create();
        aes.Key = _key;
        aes.IV = _iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using var decryptor = aes.CreateDecryptor();
        var cipherBytes = Convert.FromBase64String(cipherText);
        var plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
        return Encoding.UTF8.GetString(plainBytes);
    }
}
