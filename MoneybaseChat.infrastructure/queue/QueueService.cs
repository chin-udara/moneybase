using System.Text;
using MoneybaseChat.application.iRepositories;
using MoneybaseChat.application.iServices;
using MoneybaseChat.infrastructure.queue.rabbitmq;
using RabbitMQ.Client;

namespace MoneybaseChat.infrastructure.queue;

public class QueueService(IRabbitMqConnection rabbitMqConnection) : IQueueService
{
    private readonly IRabbitMqConnection rabbitMqConnection = rabbitMqConnection;

    public async Task PublishNewTeam(Guid teamIdentifier, CancellationToken cancellationToken)
    {
        await rabbitMqConnection.PublishNewTeam(teamIdentifier, cancellationToken);
    }

    public async Task<bool> Enqueue(Guid TeamIdentifier, Guid chatRequestIdentifier, CancellationToken cancellationToken)
    {
        var channel = await rabbitMqConnection.GetChannel(TeamIdentifier.ToString(), cancellationToken);
        await channel.BasicPublishAsync(rabbitMqConnection.ExchangeName, TeamIdentifier.ToString(), Encoding.UTF8.GetBytes(chatRequestIdentifier.ToString()), cancellationToken);
        return true;
    }
    public async Task<uint> GetQueueLength(Guid identifier, CancellationToken cancellationToken)
    {
        var channel = await rabbitMqConnection.GetChannel(identifier.ToString(), cancellationToken);
        var queueMeta = await channel.QueueDeclarePassiveAsync(identifier.ToString(), cancellationToken);
        return queueMeta.MessageCount;
    }
}

