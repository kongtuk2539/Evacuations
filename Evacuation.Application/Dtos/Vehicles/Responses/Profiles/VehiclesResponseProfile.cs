using AutoMapper;
using Evacuations.Application.Dtos.Common;
using Evacuations.Application.Dtos.Vehicles.Requests;
using Evacuations.Domain.Entities.Vehicles;

namespace Evacuations.Application.Dtos.Vehicles.Responses.Profiles;

public class VehiclesResponseProfile : Profile
{
    public VehiclesResponseProfile()
    {
        CreateMap<Vehicle, VehicleResponseDto>()
            .ForMember(d => d.VehicleId, opt => 
            opt.MapFrom(src => src.Id))
            .ForMember(d => d.LocationCoordinates, opt => opt.MapFrom(
                src => new LocationCoordinates
                {
                    Latitude = src.Latitude,
                    Longitude = src.Longitude,
                }
                ));
    }
}
