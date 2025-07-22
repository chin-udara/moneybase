using Microsoft.Extensions.Hosting;
using MoneybaseChat.infrastructure.queue.rabbitmq;
using RabbitMQ.Client;

namespace MoneybaseChat.infrastructure.workers;

public class QueueWorker(IRabbitMqConnection rabbitMqConnection) : BackgroundService
{
    private readonly IRabbitMqConnection _rabbitMqConnection = rabbitMqConnection;
    private IChannel? _channel;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _channel = await _rabbitMqConnection.StartConsumingTeamQueue(stoppingToken);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}
