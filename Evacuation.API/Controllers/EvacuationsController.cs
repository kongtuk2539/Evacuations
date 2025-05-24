using Evacuations.Application.Dtos.Evacuations.Requests;
using Evacuations.Application.Dtos.Evacuations.Responses;
using Evacuations.Application.Services.Evacuations;
using FluentValidation;
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
    public async Task<ActionResult<IEnumerable<EvacuationPlanResponse>>> GeneratePlans()
    {
        return Ok(await evacuationsService.GeneratePlanAsync());
    }

    [HttpGet]
    [Route("status")]
    public async Task<ActionResult<IEnumerable<EvacuationStatusResponse>>> GetallStatuses()
    {
        return Ok(await evacuationsService.GetAllStatusAsync());
    }
}
