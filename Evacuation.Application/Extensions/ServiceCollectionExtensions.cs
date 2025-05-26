using Evacuations.Application.Dtos.Evacuations.Responses;
using Evacuations.Application.Services.Evacuations;
using Evacuations.Application.Services.Redis;
using Evacuations.Application.Services.Vehicles;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace Evacuations.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        var connetionString = configuration.GetConnectionString("Redis");
        var applicationAssembly = typeof(ServiceCollectionExtensions).Assembly;
        services.AddAutoMapper(applicationAssembly);

        services.AddValidatorsFromAssembly(applicationAssembly)
            .AddFluentValidationAutoValidation();

        services.AddStackExchangeRedisCache(configuration =>
        {
            configuration.Configuration = connetionString;
            configuration.InstanceName = "EvacuationsApp_";
        });

        services.AddScoped<IEvacuationsService, EvacuationsService>();
        services.AddScoped<IVehicleService, VehicleService>();
        services.AddScoped<ICacheService<EvacuationStatusResponseDto>, RedisCacheService>();

        return services;
    }
}
