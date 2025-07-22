using Microsoft.Extensions.DependencyInjection;
using MoneybaseChat.application;
using MoneybaseChat.application.iRepositories;
using MoneybaseChat.application.iServices;
using MoneybaseChat.infrastructure.persistence.repositories;
using MoneybaseChat.infrastructure.queue;
using MoneybaseChat.infrastructure.queue.rabbitmq;

namespace MoneybaseChat.infrastructure.ioc;
public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IChatRequestRepository, ChatRequestRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();
            services.AddScoped<IQueueService, QueueService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            // use rabbitmq as a singleton so that the same connection will be reused
            services.AddSingleton<IRabbitMqConnection, RabbitMqConnection>();
            return services;
        }
    }