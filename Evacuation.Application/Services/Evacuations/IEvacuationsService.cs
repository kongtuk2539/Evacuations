using Evacuations.Application.Dtos.Evacuations.Requests;
using Evacuations.Application.Dtos.Evacuations.Responses;

namespace Evacuations.Application.Services.Evacuations;

public interface IEvacuationsService
{
    Task<EvacuationZoneResponseDto> CreateEvacuationZoneAsync(EvacuationZoneRequestDto evacuationZonesDto);
    Task<IEnumerable<EvacuationPlanResponseDto>> GeneratePlanAsync();
    Task<IEnumerable<EvacuationStatusResponseDto>> GetAllStatusAsync();
    Task<EvacuationStatusResponseDto> UpdateStatusAsync(EvacuationStatusRequestDto evacuationStatus);
    Task ClearAllAsync();
}
