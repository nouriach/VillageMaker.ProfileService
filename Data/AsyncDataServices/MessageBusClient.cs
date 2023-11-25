using System.Text;
using System.Text.Json;
using ProfileService.Domain.DTOs;
using RabbitMQ.Client;

namespace ProfileService.Data.AsyncDataServices;

public class MessageBusClient : IMessageBusClient
{
    private readonly IConfiguration _config;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public MessageBusClient(IConfiguration config)
    {
        _config = config;
        
        Console.WriteLine($"---> RabbitMQHost: {_config["RabbitMQHost"]}");
        Console.WriteLine($"---> RabbitMQPort: {_config["RabbitMQPort"]}");

        var factory = new ConnectionFactory() { HostName = _config["RabbitMQHost"], Port = int.Parse(_config["RabbitMQPort"])};
        try
        {
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
            
            Console.WriteLine($"---> Connected to MessageBus");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"---> Could not connect to MessageBus: {ex.Message}");
        }
    }
    public void PublishNewMaker(MakerPublishedDto makerPublishedDto)
    {
        var message = JsonSerializer.Serialize(makerPublishedDto);
        if (_connection.IsOpen)
        {
            Console.WriteLine("---> RabbitMQ Connection is open, sending message...");
            SendMessage(message);
        }
        else
        {
            Console.WriteLine("---> RabbitMQ Connection is closed, not sending.");
        }
    }

    private void SendMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(exchange: "trigger", routingKey: "", basicProperties: null, body: body);

        Console.WriteLine($"---> We have sent message: {message}");
    }

    public void Dispose()
    {
        Console.WriteLine("---> MessageBus disposed");
        if (_channel.IsOpen)
        {
            _channel.Close();
            _connection.Close();
        }
    }
    private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs eventArgs)
    {
        Console.WriteLine("---> RabbitMQ Connection Shutdown");
    }
}