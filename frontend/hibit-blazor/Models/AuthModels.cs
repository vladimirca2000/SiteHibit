namespace Hibit.Web.Models;

public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string ExpiresAt { get; set; } = string.Empty;
}

public class AppUserSettings
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class ApiSettings
{
    public string ApiUrl { get; set; } = string.Empty;
    public string MapEmbedUrl { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public AppUserSettings AppUser { get; set; } = new();
}