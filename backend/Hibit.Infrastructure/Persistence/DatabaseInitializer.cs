using Hibit.Application.Common.Interfaces;
using Hibit.Domain.Entities;
using Hibit.Infrastructure.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace Hibit.Infrastructure.Persistence;

public static class DatabaseInitializer
{
    public static async Task InitializeAsync(IServiceProvider services, CancellationToken cancellationToken = default)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<AppDbContext>>();

        if (context.Database.IsRelational())
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

            await EnsureMySqlDatabaseExistsAsync(connectionString, logger, cancellationToken);

            var pending = (await context.Database.GetPendingMigrationsAsync(cancellationToken)).ToList();
            if (pending.Count > 0)
            {
                logger.LogInformation(
                    "Applying {Count} pending migration(s): {Migrations}",
                    pending.Count,
                    string.Join(", ", pending));
            }

            await context.Database.MigrateAsync(cancellationToken);
            logger.LogInformation("Database schema is up to date.");
        }
        else
        {
            await context.Database.EnsureCreatedAsync(cancellationToken);
        }

        await SeedApplicationUserAsync(scope.ServiceProvider, logger, cancellationToken);
    }

    private static async Task EnsureMySqlDatabaseExistsAsync(
        string connectionString,
        ILogger logger,
        CancellationToken cancellationToken)
    {
        var builder = new MySqlConnectionStringBuilder(connectionString);
        var databaseName = builder.Database;

        if (string.IsNullOrWhiteSpace(databaseName))
        {
            return;
        }

        builder.Database = string.Empty;

        await using var connection = new MySqlConnection(builder.ConnectionString);
        await connection.OpenAsync(cancellationToken);

        await using var command = connection.CreateCommand();
        command.CommandText =
            $"CREATE DATABASE IF NOT EXISTS `{databaseName}` CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;";
        await command.ExecuteNonQueryAsync(cancellationToken);

        logger.LogInformation("Database '{DatabaseName}' ensured.", databaseName);
    }

    private static async Task SeedApplicationUserAsync(
        IServiceProvider services,
        ILogger logger,
        CancellationToken cancellationToken)
    {
        var passwordHasher = services.GetRequiredService<IPasswordHasher>();
        var userRepository = services.GetRequiredService<IUserRepository>();
        var appUserOptions = services.GetRequiredService<IOptions<AppUserOptions>>().Value;

        if (await userRepository.AnyAsync(cancellationToken))
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(appUserOptions.Username) || string.IsNullOrWhiteSpace(appUserOptions.Password))
        {
            logger.LogWarning("AppUser credentials not configured. Skipping application user seed.");
            return;
        }

        var usuario = new Usuario
        {
            Username = appUserOptions.Username,
            PasswordHash = passwordHasher.Hash(appUserOptions.Password),
            Role = "Application",
            IsActive = true
        };

        await userRepository.AddAsync(usuario, cancellationToken);
        logger.LogInformation("Application user seeded successfully.");
    }
}
