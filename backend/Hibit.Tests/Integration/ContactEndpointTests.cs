using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using FluentAssertions;
using Hibit.Application.Auth;
using Hibit.Application.Common.Interfaces;
using Hibit.Application.Contact;
using Hibit.Domain.Entities;
using Hibit.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Hibit.Tests.Integration;

public class ContactEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private const string TestUsername = "hibit-app";
    private const string TestPassword = "hibit-app-2026";
    private const string TestJwtSecret = "test-jwt-secret-key-minimum-32-chars";

    private readonly WebApplicationFactory<Program> _factory;

    public ContactEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Testing");
            builder.ConfigureAppConfiguration((_, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ConnectionStrings:DefaultConnection"] = "Server=localhost;Port=3306;Database=hibit_test;User=root;Password=test;",
                    ["Jwt:Secret"] = TestJwtSecret,
                    ["Jwt:Issuer"] = "Hibit.Api",
                    ["Jwt:Audience"] = "Hibit.Web",
                    ["Jwt:ExpirationMinutes"] = "60",
                    ["AppUser:Username"] = TestUsername,
                    ["AppUser:Password"] = TestPassword,
                    ["Encryption:Key"] = "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=",
                    ["Encryption:Iv"] = "AAAAAAAAAAAAAAAAAAAAAA=="
                });
            });

            builder.ConfigureServices(services =>
            {
                var descriptors = services
                    .Where(d =>
                        d.ServiceType == typeof(DbContextOptions<AppDbContext>) ||
                        d.ServiceType == typeof(DbContextOptions) ||
                        d.ServiceType == typeof(AppDbContext))
                    .ToList();

                foreach (var descriptor in descriptors)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<AppDbContext>(options =>
                    options.UseInMemoryDatabase("HibitIntegrationTests"));

                services.RemoveAll<IEncryptionService>();
                services.RemoveAll<IMessagePublisher>();
                services.AddSingleton<IEncryptionService, TestEncryptionService>();
                services.AddSingleton<IMessagePublisher, TestMessagePublisher>();
            });
        });
    }

    [Fact]
    public async Task Post_Contact_Returns_Unauthorized_Without_Token()
    {
        var client = CreateClient();
        var payload = CreateValidPayload();

        var response = await client.PostAsJsonAsync("/api/contact", payload);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Post_Contact_Returns_Accepted_For_Valid_Request()
    {
        TestMessagePublisher.Reset();

        var client = CreateClient();
        await AuthenticateAsync(client);

        var response = await client.PostAsJsonAsync("/api/contact", CreateValidPayload());

        response.StatusCode.Should().Be(HttpStatusCode.Accepted);
        TestMessagePublisher.LastPayload.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Post_Contact_Returns_BadRequest_When_Consent_Missing()
    {
        var client = CreateClient();
        await AuthenticateAsync(client);

        var payload = CreateValidPayload() with { ConsentGiven = false };
        var response = await client.PostAsJsonAsync("/api/contact", payload);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Post_Login_Returns_Token_For_Valid_Credentials()
    {
        var client = CreateClient();
        var response = await client.PostAsJsonAsync("/api/auth/login", new LoginRequestDto(TestUsername, TestPassword));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
        body.Should().NotBeNull();
        body!.AccessToken.Should().NotBeNullOrWhiteSpace();
    }

    private HttpClient CreateClient()
    {
        var client = _factory.CreateClient();
        SeedApplicationUser();
        return client;
    }

    private void SeedApplicationUser()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

        context.Database.EnsureCreated();

        if (context.Usuarios.Any())
        {
            return;
        }

        context.Usuarios.Add(new Usuario
        {
            Username = TestUsername,
            PasswordHash = passwordHasher.Hash(TestPassword),
            Role = "Application",
            IsActive = true
        });
        context.SaveChanges();
    }

    private static ContactMessageDto CreateValidPayload() => new(
        Name: "Maria Souza",
        Email: "maria@example.com",
        Phone: null,
        Subject: "Contato",
        Message: "Olá, preciso de ajuda.",
        ConsentGiven: true);

    private static async Task AuthenticateAsync(HttpClient client)
    {
        var loginResponse = await client.PostAsJsonAsync(
            "/api/auth/login",
            new LoginRequestDto(TestUsername, TestPassword));

        loginResponse.EnsureSuccessStatusCode();
        var login = await loginResponse.Content.ReadFromJsonAsync<LoginResponseDto>();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", login!.AccessToken);
    }

    private sealed class TestEncryptionService : IEncryptionService
    {
        public string Encrypt(string plainText) => Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
        public string Decrypt(string cipherText) => Encoding.UTF8.GetString(Convert.FromBase64String(cipherText));
    }

    private sealed class TestMessagePublisher : IMessagePublisher
    {
        public static string? LastPayload { get; private set; }

        public static void Reset() => LastPayload = null;

        public Task PublishAsync(string encryptedPayload, CancellationToken cancellationToken = default)
        {
            LastPayload = encryptedPayload;
            return Task.CompletedTask;
        }
    }
}
