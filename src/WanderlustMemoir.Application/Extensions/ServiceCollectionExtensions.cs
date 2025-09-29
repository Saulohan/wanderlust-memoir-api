using Microsoft.Extensions.DependencyInjection;
using WanderlustMemoir.Application.Interfaces;
using WanderlustMemoir.Application.Services;
using WanderlustMemoir.Application.Mappings;

namespace WanderlustMemoir.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Add MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));
        
        // Add AutoMapper
        services.AddAutoMapper(typeof(MappingProfile).Assembly);
        
        // Add Application Services
        services.AddScoped<IDestinationService, DestinationService>();
        services.AddScoped<IVisitedPlaceService, VisitedPlaceService>();

        return services;
    }
}