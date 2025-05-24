using Evacuations.Application.Dtos.Vehicles.Requests;
using Evacuations.Application.Dtos.Vehicles.Responses;
using Evacuations.Application.Services.Vehicles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Evacuations.API.Controllers
{
    [Route("api/vehicles")]
    [ApiController]
    public class VehiclesController(IVehicleService vehicleService) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<VehicleResponseDto>> CreateVehicle(VehicleRequestDto vehicleDto)
        {
            var result = await vehicleService.CreateVehicleAsync(vehicleDto);
            return Ok(result);
        }
    }
}
