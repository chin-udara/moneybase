using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MoneybaseChat.application.iRepositories;
using MoneybaseChat.infrastructure.queue.rabbitmq;

namespace MoneybaseChat.infrastructure.workers;

public class StartupWorker(IServiceProvider serviceProvider, IRabbitMqConnection rabbitMqConnection) : IHostedService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly IRabbitMqConnection _rabbitMqConnection = rabbitMqConnection;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var teamsRepo = scope.ServiceProvider.GetRequiredService<ITeamRepository>();
        var teams = await teamsRepo.GetAll();
        foreach (var team in teams)
        {
            Console.WriteLine("[x] Startup: New team found");
            await _rabbitMqConnection.PublishNewTeam(team.Identifier, cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
