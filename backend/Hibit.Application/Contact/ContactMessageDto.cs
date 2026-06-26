namespace Hibit.Application.Contact;

public record ContactMessageDto(
    string Name,
    string Email,
    string? Phone,
    string Subject,
    string Message,
    bool ConsentGiven);
