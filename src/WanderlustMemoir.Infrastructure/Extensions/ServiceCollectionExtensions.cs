using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WanderlustMemoir.Domain.Repositories;
using WanderlustMemoir.Infrastructure.Data;
using WanderlustMemoir.Infrastructure.Repositories;

namespace WanderlustMemoir.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString)
    {
        // Add DbContext - using InMemory for demonstration
        services.AddDbContext<WanderlustMemoirDbContext>(options =>
            options.UseInMemoryDatabase("WanderlustMemoirDb"));

        // Add Repositories
        services.AddScoped<IDestinationRepository, DestinationRepository>();
        services.AddScoped<IVisitedPlaceRepository, VisitedPlaceRepository>();

        return services;
    }
}