<<<<<<< HEAD
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace TodoApi.Logging;
=======
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AppForSEII2526.API.Logging;
>>>>>>> origin/development

[ProviderAlias("RabbitMQ")]
public class RabbitMQLoggerProvider : ILoggerProvider
{
    private readonly RabbitMQLoggerConfiguration _config;
<<<<<<< HEAD
    private readonly Dictionary<string, RabbitMQLogger> _loggers = new();
    private readonly Object _lock = new Object();
=======
    private readonly ConcurrentDictionary<string, RabbitMQLogger> _loggers = new();
>>>>>>> origin/development

    public RabbitMQLoggerProvider(IOptions<RabbitMQLoggerConfiguration> config)
    {
        _config = config.Value;
    }

    public ILogger CreateLogger(string categoryName)
<<<<<<< HEAD
    {
        lock (_lock)
        {
            if (!_loggers.TryGetValue(categoryName, out var logger))
            {
                logger = new RabbitMQLogger(categoryName, _config);
                _loggers[categoryName] = logger;
            }

            return logger;
        }
    }
=======
        => _loggers.GetOrAdd(categoryName, name => new RabbitMQLogger(name, _config));
>>>>>>> origin/development

    public void Dispose()
    {
        foreach (var logger in _loggers.Values)
<<<<<<< HEAD
        {
            logger.Dispose();
        }

        _loggers.Clear();
    }
}
=======
            logger.Dispose();

        _loggers.Clear();
    }
}
>>>>>>> origin/development
