namespace Hibit.Infrastructure.Auth;

public class AppUserOptions
{
    public const string SectionName = "AppUser";

    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
