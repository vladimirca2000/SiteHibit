using System.Text.Json;
using Hibit.Application.Common.Interfaces;
using MediatR;

namespace Hibit.Application.Contact;

public class SubmitContactCommandHandler : IRequestHandler<SubmitContactCommand>
{
    private readonly IEncryptionService _encryptionService;
    private readonly IMessagePublisher _messagePublisher;

    public SubmitContactCommandHandler(
        IEncryptionService encryptionService,
        IMessagePublisher messagePublisher)
    {
        _encryptionService = encryptionService;
        _messagePublisher = messagePublisher;
    }

    public async Task Handle(SubmitContactCommand request, CancellationToken cancellationToken)
    {
        var payload = new ContactMessagePayload(
            request.Name,
            request.Email,
            request.Phone,
            request.Subject,
            request.Message,
            request.ConsentGiven,
            DateTimeOffset.UtcNow);

        var json = JsonSerializer.Serialize(payload);
        var encrypted = _encryptionService.Encrypt(json);
        await _messagePublisher.PublishAsync(encrypted, cancellationToken);
    }

    private sealed record ContactMessagePayload(
        string Name,
        string Email,
        string? Phone,
        string Subject,
        string Message,
        bool ConsentGiven,
        DateTimeOffset SubmittedAt);
}
