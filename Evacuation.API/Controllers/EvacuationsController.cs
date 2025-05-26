using Evacuations.Application.Dtos.Evacuations.Requests;
using Evacuations.Application.Dtos.Evacuations.Responses;
using Evacuations.Application.Services.Evacuations;
using Microsoft.AspNetCore.Mvc;

namespace Evacuations.API.Controllers;

[Route("api/evacuations")]
[ApiController]
public class EvacuationsController(IEvacuationsService evacuationsService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<EvacuationZoneResponseDto>> CreateEvacuationZones(EvacuationZoneRequestDto evacuationZonesDto)
    {
        var result = await evacuationsService.CreateEvacuationZoneAsync(evacuationZonesDto);
        return Ok(result);
    }

    [HttpPost]
    [Route("plan")]
    public async Task<ActionResult<IEnumerable<EvacuationPlanResponseDto>>> GeneratePlans()
    {
        return Ok(await evacuationsService.GeneratePlanAsync());
    }

    [HttpGet]
    [Route("status")]
    public async Task<ActionResult<IEnumerable<EvacuationStatusResponseDto>>> GetallStatuses()
    {
        return Ok(await evacuationsService.GetAllStatusAsync());
    }

    [HttpPut]
    [Route("update")]
    public async Task<ActionResult<EvacuationStatusResponseDto>> UpdateStatus(EvacuationStatusRequestDto evacuationStatus)
    {
        return Ok(await evacuationsService.UpdateStatusAsync(evacuationStatus));
    }

    [HttpDelete]
    [Route("clear")]
    public async Task<IActionResult> ClearAll()
    {
        await evacuationsService.ClearAllAsync();
        return NoContent();
    }
}
