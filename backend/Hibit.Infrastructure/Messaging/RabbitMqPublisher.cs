using System.Text;
using Hibit.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Hibit.Infrastructure.Messaging;

public class RabbitMqPublisher : IMessagePublisher, IDisposable
{
    private readonly RabbitMqOptions _options;
    private readonly ILogger<RabbitMqPublisher> _logger;
    private IConnection? _connection;
    private IModel? _channel;
    private readonly object _lock = new();

    public RabbitMqPublisher(IOptions<RabbitMqOptions> options, ILogger<RabbitMqPublisher> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public Task PublishAsync(string payload, CancellationToken cancellationToken = default)
    {
        EnsureChannel();

        var body = Encoding.UTF8.GetBytes(payload);
        var properties = _channel!.CreateBasicProperties();
        properties.ContentType = "application/octet-stream";
        properties.DeliveryMode = 2;

        _channel.BasicPublish(
            exchange: string.Empty,
            routingKey: _options.Queue,
            basicProperties: properties,
            body: body);

        _logger.LogInformation("Contact message published to queue {QueueName}.", _options.Queue);
        return Task.CompletedTask;
    }

    private void EnsureChannel()
    {
        if (_channel is { IsOpen: true })
        {
            return;
        }

        lock (_lock)
        {
            if (_channel is { IsOpen: true })
            {
                return;
            }

            var factory = new ConnectionFactory
            {
                HostName = _options.Host,
                Port = _options.Port,
                UserName = _options.User,
                Password = _options.Password,
                VirtualHost = _options.VirtualHost
            };

            _connection?.Dispose();
            _channel?.Dispose();

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(
                queue: _options.Queue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }
}
