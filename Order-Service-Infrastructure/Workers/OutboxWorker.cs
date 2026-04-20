using Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Order_Service_Infrastructure.Persistence.DBContext;

namespace Order_Service_Infrastructure.Workers
{
    public class OutboxWorker(IServiceProvider serviceProvider) : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested) 
            {
                using var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<OrderDBContext>();

                var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

                var messages = await context.OutboxMessages
                    .Where(m => m.ProcessedOn == null)
                    .ToListAsync(stoppingToken);

                foreach (var message in messages)
                {
                    try
                    {
                        var orderCreatedEvent = JsonConvert.DeserializeObject<OrderCreatedEvent>(message.Content) ?? throw new JsonException("Mensagem inválida.");
                        await publishEndpoint.Publish(orderCreatedEvent, stoppingToken);

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
