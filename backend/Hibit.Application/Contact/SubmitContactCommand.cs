using MediatR;

namespace Hibit.Application.Contact;

public record SubmitContactCommand(
    string Name,
    string Email,
    string? Phone,
    string Subject,
    string Message,
    bool ConsentGiven) : IRequest;
