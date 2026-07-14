using System.Net.Http.Json;
using Microsoft.JSInterop;
using Microsoft.Extensions.Options;
using Hibit.Web.Models;

namespace Hibit.Web.Services;

public interface IAuthService
{
    Task<bool> EnsureAuthenticatedAsync();
    Task LoginAsync();
    Task<bool> HasValidTokenAsync();
    Task<string?> GetTokenAsync();
    Task ClearTokenAsync();
}

public class AuthService : IAuthService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IJSRuntime _js;
    private readonly ApiSettings _settings;

    private const string TokenKey = "hibit_auth_token";
    private const string ExpiresKey = "hibit_auth_expires_at";
    private const string AuthClientName = "Auth";

    public AuthService(IHttpClientFactory httpClientFactory, IJSRuntime js, IOptions<ApiSettings> settings)
    {
        _httpClientFactory = httpClientFactory;
        _js = js;
        _settings = settings.Value;
    }

    public async Task<bool> EnsureAuthenticatedAsync()
    {
        if (await HasValidTokenAsync())
        {
            return true;
        }

        await LoginAsync();
        return await HasValidTokenAsync();
    }

    public async Task LoginAsync()
    {
        try
        {
            var http = _httpClientFactory.CreateClient(AuthClientName);
            var request = new LoginRequest
            {
                Username = _settings.AppUser.Username,
                Password = _settings.AppUser.Password
            };

            var response = await http.PostAsJsonAsync("api/auth/login", request);

            if (response.IsSuccessStatusCode)
            {
                var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
                if (loginResponse != null)
                {
                    await StoreTokenAsync(loginResponse);
                }
            }
        }
        catch
        {
            // API may be down during local development
        }
    }

    public async Task<bool> HasValidTokenAsync()
    {
        var token = await GetTokenAsync();
        var expiresAt = await GetExpiresAtAsync();

        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(expiresAt))
        {
            return false;
        }

        if (DateTimeOffset.TryParse(expiresAt, out var expiry))
        {
            return expiry > DateTimeOffset.UtcNow;
        }

        return false;
    }

    public async Task<string?> GetTokenAsync()
    {
        return await _js.InvokeAsync<string?>("sessionStorage.getItem", TokenKey);
    }

    public async Task ClearTokenAsync()
    {
        await _js.InvokeVoidAsync("sessionStorage.removeItem", TokenKey);
        await _js.InvokeVoidAsync("sessionStorage.removeItem", ExpiresKey);
    }

    private async Task StoreTokenAsync(LoginResponse response)
    {
        await _js.InvokeVoidAsync("sessionStorage.setItem", TokenKey, response.AccessToken);
        await _js.InvokeVoidAsync("sessionStorage.setItem", ExpiresKey, response.ExpiresAt);
    }

    private async Task<string?> GetExpiresAtAsync()
    {
        return await _js.InvokeAsync<string?>("sessionStorage.getItem", ExpiresKey);
    }
}
