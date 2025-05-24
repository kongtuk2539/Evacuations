using AutoMapper;
using Evacuations.Application.Dtos.Common;
using Evacuations.Application.Dtos.Vehicles.Requests;
using Evacuations.Domain.Entities.Vehicles;

namespace Evacuations.Application.Dtos.Vehicles.Requests.Profiles;

public class VehiclesRequestProfile : Profile
{
    public VehiclesRequestProfile()
    {
        CreateMap<VehicleRequestDto, Vehicle>()
            .ForMember(d => d.Latitude, opt =>
            opt.MapFrom(src => src.LocationCoordinates!.Latitude))
            .ForMember(d => d.Longitude, opt =>
            opt.MapFrom(src => src.LocationCoordinates!.Longitude));
    }
}
