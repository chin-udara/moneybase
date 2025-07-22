using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MoneybaseChat.application;
using MoneybaseChat.application.iRepositories;

namespace MoneybaseChat.infrastructure.workers;

public class PulseMonitor(IServiceProvider serviceProvider) : BackgroundService
{

    private readonly IServiceProvider serviceProvider = serviceProvider;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {

            using var scope = serviceProvider.CreateScope();
            var chatRequestRepository = scope.ServiceProvider.GetRequiredService<IChatRequestRepository>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            await chatRequestRepository.MarkInactiveChats(stoppingToken);
            await unitOfWork.CommitAsync(stoppingToken);
            await Task.Delay(3000, stoppingToken);
        }
    }
}
