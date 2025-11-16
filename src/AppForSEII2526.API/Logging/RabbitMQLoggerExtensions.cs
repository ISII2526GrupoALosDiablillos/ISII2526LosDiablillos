using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

<<<<<<< HEAD
namespace TodoApi.Logging;
=======
namespace AppForSEII2526.API.Logging;
>>>>>>> origin/development

public static class RabbitMQLoggerExtensions
{
    public static ILoggingBuilder AddRabbitMQ(
        this ILoggingBuilder builder,
        IConfigurationSection config)
    {
        builder.Services.Configure<RabbitMQLoggerConfiguration>(config);
        builder.Services.AddSingleton<ILoggerProvider, RabbitMQLoggerProvider>();
        return builder;
    }
<<<<<<< HEAD
}
=======
}
>>>>>>> origin/development
