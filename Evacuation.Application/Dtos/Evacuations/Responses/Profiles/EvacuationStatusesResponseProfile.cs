using AutoMapper;
using Evacuations.Domain.Entities.Evacuations;

namespace Evacuations.Application.Dtos.Evacuations.Responses.Profiles;

public class EvacuationStatusesResponseProfile : Profile
{
    public EvacuationStatusesResponseProfile()
    {
        CreateMap<EvacuationStatusResponseDto, EvacuationStatus>();
        CreateMap<EvacuationStatus, EvacuationStatusResponseDto>();
    }
}
