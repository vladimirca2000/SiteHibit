namespace Hibit.Application.Common.Interfaces;

public interface IMessagePublisher
{
    Task PublishAsync(string payload, CancellationToken cancellationToken = default);
}
