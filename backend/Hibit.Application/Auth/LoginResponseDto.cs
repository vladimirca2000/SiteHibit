namespace Hibit.Application.Auth;

public record LoginResponseDto(string AccessToken, DateTimeOffset ExpiresAt);
