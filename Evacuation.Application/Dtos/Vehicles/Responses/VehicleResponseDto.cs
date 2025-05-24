using Evacuations.Application.Dtos.Common;

namespace Evacuations.Application.Dtos.Vehicles.Responses;

public class VehicleResponseDto
{
    public Guid VehicleId { get; set; }
    public int Capacity { get; set; }
    public string? Type { get; set; }
    public LocationCoordinates? LocationCoordinates { get; set; }
    public int Speed { get; set; }
}
