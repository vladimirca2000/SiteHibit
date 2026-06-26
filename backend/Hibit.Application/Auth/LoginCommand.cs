using MediatR;

namespace Hibit.Application.Auth;

public record LoginCommand(string Username, string Password) : IRequest<LoginResponseDto>;
