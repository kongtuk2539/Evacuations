using AutoMapper;
using Evacuations.Application.Dtos.Vehicles.Requests;
using Evacuations.Application.Dtos.Vehicles.Responses;
using Evacuations.Domain.Entities.Vehicles;
using Evacuations.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Evacuations.Application.Services.Vehicles;

public class VehicleService(ILogger<VehicleService> logger,
    IMapper mapper,
    IVehiclesRepository vehiclesRepository) : IVehicleService
{
    public async Task<VehicleResponseDto> CreateVehicleAsync(VehicleRequestDto vehicleDto)
    {
        logger.LogInformation("Creating a new Vehicle {@Vehicle}", vehicleDto);
        var vehicle = mapper.Map<Vehicle>(vehicleDto);
        var result = await vehiclesRepository.CreateAsync(vehicle);
        return mapper.Map<VehicleResponseDto>(result);
    }
}
