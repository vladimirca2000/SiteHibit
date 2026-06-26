namespace Hibit.Infrastructure.Messaging;

public class RabbitMqOptions
{
    public const string SectionName = "RabbitMQ";

    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string User { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Queue { get; set; } = "hibit.contact";
    public string VirtualHost { get; set; } = "/";
}
