using Microsoft.Extensions.DependencyInjection;
using MoneybaseChat.application.iServices;
using MoneybaseChat.application.services;


namespace MoneybaseChat.infrastructure.ioc;
public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
        services.AddScoped<IChatRequestPulseService, ChatRequestPulseService>();
        services.AddScoped<IInitiateChatService, InitiateChatService>();
        services.AddScoped<IChatAssignService, ChatAssignService>();
            
            return services;
        }
    }