using Framework.Core.Domain;
using Framework.Core.Messaging;
using Microsoft.Extensions.DependencyInjection;
using OrderManagement.Application.UseCase;
using OrderManagement.Domain.Contract.DataContract;
using OrderManagement.Domain.OrderAggregate;
using OrderManagement.Persistence.Ef;
using OrderManagement.Persistence.Mongodb;

namespace OrderManagement.Application;

public static class ApplicationResolver
{
    public static void AddApplicationResolver(this IServiceCollection service)
    {
        service.AddScoped<IBaseCommandHandler<CreateOrderCommand>, CreateOrderCommandHandler>();
        service.AddScoped<IBaseCommandHandler<ApproveOrderCommand>, ApproveOrderCommandHandler>();
        service.AddScoped<IBaseCommandHandler<RejectOrderCommand>, RejectOrderCommandHandler>();
        service.AddScoped<IGuidProvider, DefaultGuidProvider>();
        service.AddScoped<ICommandBuss, CommandBuss>();
        service.AddScoped<IOrderRepository, OrderRepositoryEf>();
        //service.AddScoped<IOrderRepository, OrderRepositoryMongodb>();
    }
}