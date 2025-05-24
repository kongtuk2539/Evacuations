using Evacuations.Application.Dtos.Evacuations.Requests;
using Evacuations.Application.Dtos.Evacuations.Responses;

namespace Evacuations.Application.Services.Evacuations;

public interface IEvacuationsService
{
    Task<EvacuationZoneResponseDto> CreateEvacuationZoneAsync(EvacuationZoneRequestDto evacuationZonesDto);
    Task<IEnumerable<EvacuationPlanResponse>> GeneratePlanAsync();
    Task<IEnumerable<EvacuationStatusResponse>> GetAllStatusAsync();
}
