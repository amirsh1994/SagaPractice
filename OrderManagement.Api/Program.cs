
using Framework.Core.Domain;
using Framework.Core.Messaging;
using Framework.Messaging.Masstransit;
using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using OrderManagement.Api.EventHandler;
using OrderManagement.Application;
using OrderManagement.Persistence.Ef;
using OrderManagement.Persistence.Mongodb;
using RestSharp;

namespace OrderManagement.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddApplicationResolver();
        builder.Services.AddEfPersistenceResolver(builder.Configuration);
        builder.Services.AddScoped<ICommandBuss, CommandBuss>();
        builder.Services.AddScoped<IEventBus, MassTransitBusImplementation>();
        builder.Services.AddMongodbPersistenceResolver(builder.Configuration);
        builder.Services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Password("guest");
                    h.Username("guest");
                });
                cfg.ConfigureEndpoints(ctx);
            });
            x.AddConsumer<OrderQuantityProceedFailedEventHandlerFromOrderManagement>();
            x.AddConsumer<OrderQuantityProceedSuccessEventHandlerFromOrderManagement>();

        });
        builder.Services.AddHealthChecks()
            .AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new Exception("connectionString was not found"))
            .AddRabbitMQ(new Uri("amqp://guest:guest@localhost:5672"));
            //.AddMongoDb(builder.Configuration.GetConnectionString("MongodbConnection") ?? throw new Exception("connectionString was not found"));

        var app = builder.Build();
       

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();
        app.UseRouting()
            .UseEndpoints(endpoints =>
            {
                app.MapHealthChecks("hc");
                app.MapHealthChecks("hc_d", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });
        app.UseHealthChecksUI();
        //using (var scope = app.Services.CreateScope())
        //{
        //    var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();
        //    RegisterServices(eventBus);
        //}


        app.Run();
    }

    private static void RegisterServices(IEventBus eventBus)
    {

        //eventBus.Publish(new OrderServiceRegisteredEvent { Name = "Order", Url = "https://localhost:7147" });
        var client = new RestClient("https://localhost:7097");
        var request = new RestRequest("api/RegisterServices");
        request.AddBody(new { Name = "Order", Url = "https://localhost:7147" });
        var result = client.Post(request);
    }
}

//public class OrderServiceRegisteredEvent
//{
//    public string Name { get; set; }
//    public string Url { get; set; }
//}

