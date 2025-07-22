using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MoneybaseChat.application;
using MoneybaseChat.application.iRepositories;
using MoneybaseChat.application.iServices;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MoneybaseChat.infrastructure.queue.rabbitmq;


public interface IRabbitMqConnection
{

    string ExchangeName { get; }
    Task<IConnection> GetConnection(CancellationToken cancellationToken);
    Task<IChannel> GetChannel(string queueName, CancellationToken cancellationToken);

    Task PublishNewTeam(Guid teamIdentifier, CancellationToken cancellationToken);
    Task<IChannel> StartConsumingTeamQueue(CancellationToken cancellationToken);
}

public class RabbitMqConnection(IServiceProvider serviceProvider, IOptions<RabbitMqServerSettings> settings) : IRabbitMqConnection, IAsyncDisposable
{
    private readonly IServiceProvider serviceProvider = serviceProvider;
    private readonly RabbitMqServerSettings rabbitMqServerSettings = settings.Value;
    private IConnection? connection;

    public string ExchangeName
    {
        get
        {
            return "moneybase-chat";
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (connection is not null)
        {
            await connection.CloseAsync();
            await connection.DisposeAsync();
        }
    }

    public async Task<IChannel> GetChannel(string queueName, CancellationToken cancellationToken)
    {
        var queueConnection = await GetConnection(cancellationToken);
        var channel = await queueConnection.CreateChannelAsync(null, cancellationToken);
        await channel.ExchangeDeclareAsync(ExchangeName, ExchangeType.Direct, cancellationToken: cancellationToken);
        await channel.QueueDeclareAsync(queueName, false, false, false, null, cancellationToken: cancellationToken);
        await channel.QueueBindAsync(queueName, ExchangeName, queueName, null, cancellationToken: cancellationToken);
        return channel;
    }

    public async Task PublishNewTeam(Guid teamIdentifier, CancellationToken cancellationToken)
    {
        Console.WriteLine("[x] Publishing new team {0}", teamIdentifier);
        var channel = await GetChannel("teams", cancellationToken);
        await channel.BasicPublishAsync(ExchangeName, "teams", Encoding.UTF8.GetBytes(teamIdentifier.ToString()), cancellationToken);
    }

    public async Task<IChannel> StartConsumingTeamQueue(CancellationToken cancellationToken)
    {
        var teamChannels = new List<IChannel>();
        var channel = await GetChannel("teams", cancellationToken);
        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine("[x] new team published {0}", Guid.Parse(message));
            teamChannels.Add(await ConsumeChatRequests(Guid.Parse(message), cancellationToken));
            // acknowledge the message if successfully created a new consumer
            await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
        };
        await channel.BasicConsumeAsync("teams", false, consumer, cancellationToken);
        await Task.Delay(Timeout.Infinite, cancellationToken);
        return channel;
    }


    private async Task<IChannel> ConsumeChatRequests(Guid teamIdentifier, CancellationToken cancellationToken)
    {
        Console.WriteLine("[x] Creating consumer for team: {0}", teamIdentifier);
        var channel = await GetChannel(teamIdentifier.ToString(), cancellationToken);
        await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false, cancellationToken);
        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            try
            {
                Console.WriteLine("[x] Team received a new chat request {0}", teamIdentifier);
                while (!cancellationToken.IsCancellationRequested)
                {
                    // manually create the repos
                    using var scope = serviceProvider.CreateScope();
                    var chatAssignService = scope.ServiceProvider.GetRequiredService<IChatAssignService>();


                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine("[x] Trying to assign the chat request to agent {0}", message);

                    if (await chatAssignService.Assign(teamIdentifier, Guid.Parse(message), cancellationToken))
                    {
                        await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
                        break;
                    }

                    // wait few seconds and loop again to see if the chat can be assigned
                    await Task.Delay(5000, cancellationToken);
                }
            }
            catch (Exception e)
            {

                Console.WriteLine("[XX] Error: {0}", e.Message);
                Console.WriteLine("[XXX] Stack Trace: {0}", e.StackTrace);
            }

        };
        await channel.BasicConsumeAsync(teamIdentifier.ToString(), false, consumer, cancellationToken);
        return channel;
    }

    public async Task<IConnection> GetConnection(CancellationToken cancellationToken)
    {
        if (connection != null && connection.IsOpen) return connection;

        var connectionFactory = new ConnectionFactory
        {
            Port = rabbitMqServerSettings.Port,
            HostName = rabbitMqServerSettings.Host,
            UserName = rabbitMqServerSettings.Username,
            Password = rabbitMqServerSettings.Password
        };

        connection = await connectionFactory.CreateConnectionAsync(cancellationToken);
        return connection;
    }
}