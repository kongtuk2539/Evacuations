using Evacuations.Application.Dtos.Common;

namespace Evacuations.Application.Dtos.Vehicles.Requests;

public class VehicleRequestDto
{
    public int Capacity { get; set; }
    public string? Type { get; set; }
    public LocationCoordinates? LocationCoordinates { get; set; }
    public int Speed { get; set; }
}
