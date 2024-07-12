
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SREventConsumerService.SignalR;


namespace SREventConsumerService
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    var signalRConfig = context.Configuration.GetSection("SignalRConfig").Get<SignalRConfig>();
                    services.AddSingleton(signalRConfig);
                    services.AddHostedService<SignalREventConsumerService>();
                });

            await builder.RunConsoleAsync();
        }
    }
}