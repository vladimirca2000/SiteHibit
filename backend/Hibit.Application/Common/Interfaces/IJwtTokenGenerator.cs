using Hibit.Domain.Entities;

namespace Hibit.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    (string AccessToken, DateTimeOffset ExpiresAt) Generate(Usuario usuario);
}
