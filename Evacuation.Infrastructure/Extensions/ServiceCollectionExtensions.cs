using Evacuations.Domain.Repositories;
using Evacuations.Infrastructure.Persistence;
using Evacuations.Infrastructure.Repositories;
using Evacuations.Infrastructure.Seedes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Evacuations.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connetionString = configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");
        services.AddDbContext<EvacuationsDbContext>(option =>
            option.UseSqlServer(connetionString)
            .EnableSensitiveDataLogging());

        services.AddScoped<IEvacuationsRepository, EvacuationsRepository>();
        services.AddScoped<IVehiclesRepository, VehiclesRepository>();
        services.AddScoped<IEvacuationSeeder, EvacuationSeeder>();

        return services;
    }
}
