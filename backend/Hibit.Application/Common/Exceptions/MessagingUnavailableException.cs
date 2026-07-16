namespace Hibit.Application.Common.Exceptions;

public class MessagingUnavailableException : Exception
{
    public MessagingUnavailableException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
