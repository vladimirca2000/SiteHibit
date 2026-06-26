namespace Hibit.Application.Common.Interfaces;

public interface IMessagePublisher
{
    Task PublishAsync(string encryptedPayload, CancellationToken cancellationToken = default);
}
