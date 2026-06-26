namespace Hibit.Application.Common.Exceptions;

public class UnauthorizedException : Exception
{
    public UnauthorizedException()
        : base("Credenciais inválidas.")
    {
    }
}
