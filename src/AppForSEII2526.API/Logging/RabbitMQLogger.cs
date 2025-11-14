<<<<<<< HEAD
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace TodoApi.Logging;
=======
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace AppForSEII2526.API.Logging;
>>>>>>> origin/development

public class RabbitMQLogger : ILogger, IDisposable
{
    private readonly string _name;
    private readonly RabbitMQLoggerConfiguration _config;
<<<<<<< HEAD
    private readonly IConnection _connection;
    private readonly RabbitMQ.Client.IModel _channel;
    private readonly IBasicProperties _properties;
=======

    private readonly RabbitMQ.Client.IConnection _connection;
    private readonly RabbitMQ.Client.IModel _channel;
    private readonly RabbitMQ.Client.IBasicProperties _properties;
>>>>>>> origin/development

    public RabbitMQLogger(string name, RabbitMQLoggerConfiguration config)
    {
        _name = name ?? throw new ArgumentNullException(nameof(name));
        _config = config ?? throw new ArgumentNullException(nameof(config));
<<<<<<< HEAD
        
        ValidateConfiguration(_config);

        var factory = new ConnectionFactory
=======

        ValidateConfiguration(_config);

        var factory = new RabbitMQ.Client.ConnectionFactory
>>>>>>> origin/development
        {
            HostName = _config.HostName,
            Port = _config.Port,
            UserName = _config.UserName,
            Password = _config.Password
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(
            exchange: _config.Exchange,
            type: _config.ExchangeType,
            durable: _config.Durable);

        _properties = _channel.CreateBasicProperties();
        _properties.Persistent = true;
        _properties.ContentType = "application/json";
<<<<<<< HEAD


=======
>>>>>>> origin/development
    }

    private static void ValidateConfiguration(RabbitMQLoggerConfiguration config)
    {
<<<<<<< HEAD
        if (string.IsNullOrEmpty(config.HostName))
            throw new ArgumentException("RabbitMQ HostName is required", nameof(config));
        if (config.Port <= 0)
            throw new ArgumentException("RabbitMQ Port must be greater than 0", nameof(config));
        if (string.IsNullOrEmpty(config.UserName))
            throw new ArgumentException("RabbitMQ UserName is required", nameof(config));
        if (string.IsNullOrEmpty(config.Password))
            throw new ArgumentException("RabbitMQ Password is required", nameof(config));
        if (string.IsNullOrEmpty(config.Exchange))
            throw new ArgumentException("RabbitMQ Exchange is required", nameof(config));
        if (string.IsNullOrEmpty(config.ExchangeType))
            throw new ArgumentException("RabbitMQ ExchangeType is required", nameof(config));
=======
        if (string.IsNullOrWhiteSpace(config.HostName))
            throw new ArgumentException("RabbitMQ HostName is required");
        if (config.Port <= 0)
            throw new ArgumentException("RabbitMQ Port must be greater than 0");
        if (string.IsNullOrWhiteSpace(config.UserName))
            throw new ArgumentException("RabbitMQ UserName is required");
        if (string.IsNullOrWhiteSpace(config.Password))
            throw new ArgumentException("RabbitMQ Password is required");
        if (string.IsNullOrWhiteSpace(config.Exchange))
            throw new ArgumentException("RabbitMQ Exchange is required");
        if (string.IsNullOrWhiteSpace(config.ExchangeType))
            throw new ArgumentException("RabbitMQ ExchangeType is required");
>>>>>>> origin/development
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default;

    public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
<<<<<<< HEAD
        if (!IsEnabled(logLevel))
            return;
=======
        if (!IsEnabled(logLevel)) return;
>>>>>>> origin/development

        try
        {
            var logEntry = new
            {
                Timestamp = DateTime.UtcNow,
<<<<<<< HEAD
                LogLevel = logLevel.ToString(),
=======
                Level = logLevel.ToString(),
>>>>>>> origin/development
                Category = _name,
                EventId = eventId.Id,
                EventName = eventId.Name,
                Message = formatter(state, exception),
                Exception = exception?.ToString()
            };
<<<<<<< HEAD
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(logEntry));
=======

            var json = JsonSerializer.Serialize(logEntry);
            var body = Encoding.UTF8.GetBytes(json);

>>>>>>> origin/development
            _channel.BasicPublish(
                exchange: _config.Exchange,
                routingKey: "",
                basicProperties: _properties,
<<<<<<< HEAD
                body: body
            );

=======
                body: body);
>>>>>>> origin/development
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error publishing log message to RabbitMQ: {ex.Message}");
        }
    }

    public void Dispose()
    {
        try
        {
            _channel?.Close();
            _channel?.Dispose();
            _connection?.Close();
            _connection?.Dispose();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error disposing RabbitMQ logger: {ex.Message}");
        }
<<<<<<< HEAD
        GC.SuppressFinalize(this);
    }
}
=======

        GC.SuppressFinalize(this);
    }
}
>>>>>>> origin/development
