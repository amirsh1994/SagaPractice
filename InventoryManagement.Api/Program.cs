
using Framework.Core.Domain;
using Framework.Core.Messaging;
using HealthChecks.UI.Client;
using InventoryManagement.Api.EventHandler;
using InventoryManagement.Application.UseCase;
using InventoryManagement.Domain.DataContract.DataContract;
using InventoryManagement.Domain.StockAggregate;
using InventoryManagement.Persistence.Ef;
using MassTransit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using RestSharp;

namespace InventoryManagement.Api;

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
        builder.Services.AddDbContext<InventoryDbContext>(option =>
        {
            option.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
        });
        builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
        builder.Services.AddSingleton<IGuidProvider, DefaultGuidProvider>();
        builder.Services.AddScoped<IBaseCommandHandler<CreateStockCommand>, CreateStockCommandHandler>();
        builder.Services.AddScoped<ICommandBuss, CommandBuss>();
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
            x.AddConsumer<OrderCreatedEventHandlerInInventory>();
        });
        builder.Services.AddScoped<IBaseCommandHandler<AdjustQuantityCommand>,AdjustmentQuantityCommandHandler>();
        builder.Services.AddScoped<IMongoClient>(option => new MongoClient("mongodb://localhost:27017"));
        builder.Services.AddHealthChecks()
            .AddRabbitMQ(new Uri("amqp://guest:guest@localhost:5672"))
            .AddSqlServer(builder.Configuration.GetConnectionString("Default") ??
                          throw new Exception("Connection string is nul"));
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
                endpoints.MapHealthChecks("hc");
                endpoints.MapHealthChecks("hc_d", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });

        app.UseHealthChecksUI();
        //RegisterServices();

        app.Run();


    }

    private static void RegisterServices()
    {
        var client = new RestClient("https://localhost:7097");
        var request = new RestRequest("api/RegisterServices");
        request.AddBody(new {Name= "Stock",Url= "https://localhost:7123" });
        var result = client.Post(request);
    }
}

