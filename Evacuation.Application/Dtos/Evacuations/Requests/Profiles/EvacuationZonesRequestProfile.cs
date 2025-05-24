using AutoMapper;
using Evacuations.Application.Dtos.Common;
using Evacuations.Application.Dtos.Evacuations.Requests;
using Evacuations.Domain.Entities.Evacuations;

namespace Evacuations.Application.Dtos.Evacuations.Requests.Profiles;

public class EvacuationZonesRequestProfile : Profile
{
    public EvacuationZonesRequestProfile()
    { 
        CreateMap<EvacuationZoneRequestDto, EvacuationZone>()
            .ForMember(d => d.Latitude, opt =>
            opt.MapFrom(src => src.LocationCoordinates!.Latitude))
            .ForMember(d => d.Longitude, opt =>
            opt.MapFrom(src => src.LocationCoordinates!.Longitude));
    }
}
