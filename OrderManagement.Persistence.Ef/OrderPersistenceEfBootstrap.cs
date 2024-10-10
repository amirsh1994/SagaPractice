using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OrderManagement.Persistence.Ef;

public static class  OrderPersistenceEfBootstrap
{
    public static void AddEfPersistenceResolver(this IServiceCollection service,IConfiguration configuration)
    {
        service.AddDbContext<OrderManagementDbContext>(option =>
        {
            option.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        },ServiceLifetime.Scoped);
    }
}