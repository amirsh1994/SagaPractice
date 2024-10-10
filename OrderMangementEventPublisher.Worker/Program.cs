using System.Data;
using Framework.Core.Domain;
using Framework.Messaging.Masstransit;
using Framework.OutBoxPublisher;
using MassTransit;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;

namespace OrderManagementEventPublisher.Worker;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddHostedService<Worker>();
            builder.Services.AddSingleton<IEventBus,MassTransitBusImplementation>();
            builder.Services.AddSingleton<OutBoxManager>();
            builder.Services.AddSingleton<IDbConnection>(option => new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host("localhost","/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                    cfg.ConfigureEndpoints(ctx);
                });
            });

            var host = builder.Build();
            host.Run();
        }
    }
