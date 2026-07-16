using Hibit.Application.Common.Interfaces;
using Hibit.Infrastructure.Auth;
using Hibit.Infrastructure.Encryption;
using Hibit.Infrastructure.Messaging;
using Hibit.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hibit.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

        services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(
                connectionString,
                ServerVersion.Parse("8.0.0-mysql"),
                mySqlOptions => mySqlOptions.MigrationsAssembly(
                    typeof(AppDbContext).Assembly.GetName().Name)));

        services.AddOptions<EncryptionOptions>()
            .Bind(configuration.GetSection(EncryptionOptions.SectionName))
            .Validate(opts =>
                !string.IsNullOrWhiteSpace(opts.Key)
                && !string.IsNullOrWhiteSpace(opts.Iv)
                && TryFromBase64(opts.Key, out var key) && key.Length == 32
                && TryFromBase64(opts.Iv, out var iv) && iv.Length == 16,
                "Encryption Key must be 32 bytes (AES-256) and IV must be 16 bytes, both valid Base64 strings.")
            .ValidateOnStart();

        services.Configure<RabbitMqOptions>(configuration.GetSection(RabbitMqOptions.SectionName));
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.Configure<AppUserOptions>(configuration.GetSection(AppUserOptions.SectionName));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IEncryptionService, AesEncryptionService>();
        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        return services;
    }

    private static bool TryFromBase64(string value, out byte[] bytes)
    {
        try
        {
            bytes = Convert.FromBase64String(value);
            return true;
        }
        catch (Exception)
        {
            bytes = Array.Empty<byte>();
            return false;
        }
    }
}
