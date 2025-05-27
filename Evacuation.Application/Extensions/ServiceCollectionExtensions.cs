using Evacuations.Application.Dtos.Evacuations.Responses;
using Evacuations.Application.Services.Evacuations;
using Evacuations.Application.Services.Redis;
using Evacuations.Application.Services.Vehicles;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using Azure.Identity;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace Evacuations.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
  
        var redisConnection = configuration.GetConnectionString("Redis");
        services.AddStackExchangeRedisCache(configuration =>
        {
            configuration.Configuration = redisConnection;
            configuration.InstanceName = "evacuationcache.redis";
        });

        var applicationAssembly = typeof(ServiceCollectionExtensions).Assembly;
        services.AddAutoMapper(applicationAssembly);

        services.AddValidatorsFromAssembly(applicationAssembly)
            .AddFluentValidationAutoValidation();

        services.AddScoped<IEvacuationsService, EvacuationsService>();
        services.AddScoped<IVehicleService, VehicleService>();
        services.AddScoped<ICacheService<EvacuationStatusResponseDto>, RedisCacheService>();

        return services;
    }
}
