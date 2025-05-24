using Evacuations.Application.Dtos.Common;

namespace Evacuations.Application.Dtos.Evacuations.Responses;

public class EvacuationZoneResponseDto
{
    public Guid ZoneId { get; set; }
    public LocationCoordinates? LocationCoordinates { get; set; }
    public int NumberOfPeople { get; set; }
    public int UrgencyLevel { get; set; }
}
