using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Order_Service_Application.Interfaces;
using Order_Service_Domain.Interfaces;
using Order_Service_Infrastructure.Persistence.DBContext;
using Order_Service_Infrastructure.Persistence.Repository;
using Order_Service_Infrastructure.RabbitMQ.Consumers;
using System.Reflection;

namespace Order_Service_Infrastructure
{
    public static class DependencyInjection
    {
        private static IServiceCollection _services;
        private static IConfiguration _configuration;

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            _services = services;
            _configuration = configuration;

            AddDBContext();
            AddRepositories();
            AddingMediatR();
            AddWorker();
            AddMassTransit();

            return services;
        }

        public static void AddDBContext() 
        {
            _services.AddDbContext<OrderDBContext>(options =>
            options.UseNpgsql(_configuration.GetConnectionString("DefaultConnection")));
        }

        public static void AddRepositories() 
        {
            _services.AddScoped<IOrderRepository, OrderRepository>();
            _services.AddScoped<IOutboxRepository, OutboxRepository>();
            _services.AddScoped<IProcessedMessageRepository, ProcessedMessageRepository>();
        }

        public static void AddingMediatR() => _services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.Load("Order-Service-Application")));

        public static void AddWorker() => _services.AddHostedService<Workers.OutboxWorker>();

        private static void AddMassTransit() 
        {
            _services.AddMassTransit(x =>
            {
                // registra os consumers
                x.AddConsumer<StockRejectedConsumer>();
                x.AddConsumer<StockReservedConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    // mapeia automaticamente consumers → filas
                    cfg.ConfigureEndpoints(context);
                });
            });
        }

    }
}
