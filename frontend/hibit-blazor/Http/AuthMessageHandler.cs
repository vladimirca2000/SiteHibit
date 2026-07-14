using System.Net.Http.Headers;
using Hibit.Web.Services;

namespace Hibit.Web.Http;

public class AuthMessageHandler : DelegatingHandler
{
    private readonly IAuthService _authService;
    private static readonly SemaphoreSlim RefreshLock = new(1, 1);

    public AuthMessageHandler(IAuthService authService)
    {
        _authService = authService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (IsLoginRequest(request))
        {
            return await base.SendAsync(request, cancellationToken);
        }

        await EnsureAuthenticatedAsync();

        var token = await _authService.GetTokenAsync();
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            await RefreshLock.WaitAsync(cancellationToken);
            try
            {
                await _authService.ClearTokenAsync();
                await _authService.LoginAsync();

                var newToken = await _authService.GetTokenAsync();
                if (!string.IsNullOrEmpty(newToken))
                {
                    var retryRequest = await CloneRequestAsync(request);
                    retryRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newToken);
                    return await base.SendAsync(retryRequest, cancellationToken);
                }
            }
            finally
            {
                RefreshLock.Release();
            }
        }

        return response;
    }

    private async Task EnsureAuthenticatedAsync()
    {
        if (!await _authService.HasValidTokenAsync())
        {
            await _authService.LoginAsync();
        }
    }

    private static bool IsLoginRequest(HttpRequestMessage request)
    {
        var path = request.RequestUri?.AbsolutePath ?? string.Empty;
        return path.Contains("/api/auth/login", StringComparison.OrdinalIgnoreCase);
    }

    private static async Task<HttpRequestMessage> CloneRequestAsync(HttpRequestMessage request)
    {
        var clone = new HttpRequestMessage(request.Method, request.RequestUri)
        {
            Version = request.Version
        };

        foreach (var header in request.Headers)
        {
            clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        if (request.Content != null)
        {
            var content = await request.Content.ReadAsByteArrayAsync();
            clone.Content = new ByteArrayContent(content);
            foreach (var header in request.Content.Headers)
            {
                clone.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

        return clone;
    }
}
