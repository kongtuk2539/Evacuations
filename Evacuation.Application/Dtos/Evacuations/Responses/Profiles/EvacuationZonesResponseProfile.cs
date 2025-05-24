using AutoMapper;
using Evacuations.Application.Dtos.Common;
using Evacuations.Application.Dtos.Evacuations.Requests;
using Evacuations.Domain.Entities.Evacuations;

namespace Evacuations.Application.Dtos.Evacuations.Responses.Profiles;

public class EvacuationZonesResponseProfile : Profile
{
    public EvacuationZonesResponseProfile()
    {
        CreateMap<EvacuationZone, EvacuationZoneResponseDto>()
            .ForMember(d => d.ZoneId, otp => 
            otp.MapFrom(src => src.Id ))
            .ForMember(d => d.LocationCoordinates, opt => opt.MapFrom(
                src => new LocationCoordinates
                {
                    Latitude = src.Latitude,
                    Longitude = src.Longitude,
                }
                ));
    }
}
