using Evacuations.API.MIddlewares;
using Serilog;
using System.Text.Json.Serialization;

namespace Evacuations.API.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static IServiceCollection AddPresentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers().AddJsonOptions(options =>
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddScoped<ErrorHandlingMiddleware>();
        builder.Services.AddScoped<RequestTimeLoggingMiddleware>();

        builder.Host.UseSerilog((context, configuration) =>
            configuration
            .ReadFrom.Configuration(context.Configuration));

        return builder.Services;
    }
}
