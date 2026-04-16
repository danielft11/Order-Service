using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Order_Service_Infrastructure.Persistence.DBContext;
using Order_Service_Infrastructure.RabbitMQ.Publishers;
using System.Text;

namespace Order_Service_Infrastructure.Workers
{
    public class OutboxWorker(IServiceProvider serviceProvider) : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<OrderDBContext>();
            var publisher = scope.ServiceProvider.GetRequiredService<IEventPublisher>();

            while (!stoppingToken.IsCancellationRequested) 
            {    
                var messages = await context.OutboxMessages
                    .Where(m => m.ProcessedOn == null)
                    .ToListAsync(stoppingToken);

                foreach (var message in messages)
                {
                    try
                    {
                        var messageBytes = Encoding.UTF8.GetBytes(message.Content);
                        await publisher.PublishAsync(messageBytes);

                        message.ProcessedOn = DateTime.UtcNow;
                    }
                    catch (Exception ex)
                    {
                        message.Error = ex.Message;
                    }
                }

                await context.SaveChangesAsync(stoppingToken);

                await Task.Delay(5000, stoppingToken);
            };
        }
    }
}
