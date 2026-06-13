using CompraProgramada.Domain.ClientContext.Repositories;
using CompraProgramada.Domain.CustodyContext.Abstractions.Repositories;
using CompraProgramada.Domain.RecommendationBasketContext.Abstractions.Querys;
using CompraProgramada.Domain.SharedContext;
using CompraProgramada.Infrastructure.Persistence.ClientContext.Repositories;
using CompraProgramada.Infrastructure.Persistence.CustodyContext.Repositories;
using CompraProgramada.Infrastructure.Persistence.RecommendationBasketContext.Queries;
using CompraProgramada.Infrastructure.Persistence.SharedContext.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CompraProgramada.Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")));
        
        services.AddScoped<IUnitOfWork>(provider =>
            provider.GetRequiredService<AppDbContext>());
        
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<ICustodyRepository, CustodyRepository>();
        services.AddScoped<IBasketQuery, BasketQuery>();
        return services;
    }
}