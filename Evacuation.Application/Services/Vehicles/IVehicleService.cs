using Evacuations.Application.Dtos.Vehicles.Requests;
using Evacuations.Application.Dtos.Vehicles.Responses;

namespace Evacuations.Application.Services.Vehicles;

public interface IVehicleService
{
    Task<VehicleResponseDto> CreateVehicleAsync(VehicleRequestDto vehicleDto);
}
