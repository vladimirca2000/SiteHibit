using System.Security.Cryptography;
using FluentAssertions;
using Hibit.Infrastructure.Encryption;
using Microsoft.Extensions.Options;

namespace Hibit.Tests.Unit;

public class AesEncryptionServiceTests
{
    private static AesEncryptionService CreateService()
    {
        using var aes = Aes.Create();
        aes.GenerateKey();
        aes.GenerateIV();

        var options = Options.Create(new EncryptionOptions
        {
            Key = Convert.ToBase64String(aes.Key),
            Iv = Convert.ToBase64String(aes.IV)
        });

        return new AesEncryptionService(options);
    }

    [Fact]
    public void Encrypt_And_Decrypt_Should_RoundTrip()
    {
        var service = CreateService();
        const string plainText = "{\"name\":\"Test\"}";

        var encrypted = service.Encrypt(plainText);
        var decrypted = service.Decrypt(encrypted);

        encrypted.Should().NotBe(plainText);
        decrypted.Should().Be(plainText);
    }

    [Fact]
    public void Constructor_Should_Throw_When_Key_Is_Invalid_Length()
    {
        var options = Options.Create(new EncryptionOptions
        {
            Key = Convert.ToBase64String(new byte[16]),
            Iv = Convert.ToBase64String(new byte[16])
        });

        var act = () => new AesEncryptionService(options);
        act.Should().Throw<InvalidOperationException>();
    }
}
