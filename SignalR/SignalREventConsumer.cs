using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SREventConsumerService.SignalR;

namespace SREventConsumerService
{
    public class SignalREventConsumerService : IHostedService
    {
        private readonly ILogger<SignalREventConsumerService> _logger;
        private readonly SignalRConfig _config;
        private HubConnection _connection;

        public SignalREventConsumerService(ILogger<SignalREventConsumerService> logger, SignalRConfig config)
        {
            _logger = logger;
            _config = config;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _connection = new HubConnectionBuilder()
                    .WithUrl(_config.SignalRUrl)
                    .Build();
                _connection.On<string, string>("ReceiveAlert", (type, message) =>
                {
                    Console.WriteLine($"Received Alert: Type={type}, Message={message}");
                });

                await _connection.StartAsync(cancellationToken);
                _logger.LogInformation("Connected to SignalR hub");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while connecting to the SignalR hub");
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (_connection != null)
                {
                    await _connection.StopAsync(cancellationToken);
                    _logger.LogInformation("Disconnected from SignalR hub");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while disconnecting from the SignalR hub");
            }
        }
    }
}
