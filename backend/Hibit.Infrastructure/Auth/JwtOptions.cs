namespace Hibit.Infrastructure.Auth;

public class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Secret { get; set; } = string.Empty;
    public string Issuer { get; set; } = "Hibit.Api";
    public string Audience { get; set; } = "Hibit.Web";
    public int ExpirationMinutes { get; set; } = 60;
}
