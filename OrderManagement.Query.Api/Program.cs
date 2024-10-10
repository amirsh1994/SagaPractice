
using MassTransit;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using OrderManagement.Query.Api.EventHandler;
using OrderManagement.Query.Api.Persistence.Ef;
using OrderManagement.Query.Api.Persistence.mongodb;

namespace OrderManagement.Query.Api;

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
            builder.Services.AddDbContext<OrderManagementQueryDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
            },ServiceLifetime.Scoped);
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
                x.AddConsumer<OrderCreatedEventHandlerInOrderReadModel>();
            });
            builder.Services.AddScoped<MongoClient>(option => new MongoClient(builder.Configuration.GetConnectionString("MongodbConnection")));
            builder.Services.AddSingleton(sp =>
            {
                var mongodbClient = sp.GetRequiredService<MongoClient>();
                var databaseName = builder.Configuration.GetValue<string>("MongodbDatabase");
                return mongodbClient.GetDatabase(databaseName);
            });
            builder.Services.AddDbContext<OrderManagementQueryMongodbContext>(option =>
            {
                var client = builder.Services.BuildServiceProvider().GetRequiredService<MongoClient>();
                var databaseName = builder.Configuration["ConnectionStrings:MongodbDatabase"];
                option.UseMongoDB(client, databaseName);
            });


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

            app.Run();
        }
    }

