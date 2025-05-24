using Evacuations.Application.Dtos.Common;
using System.ComponentModel.DataAnnotations;

namespace Evacuations.Application.Dtos.Evacuations.Requests;

public class EvacuationZoneRequestDto
{
    public LocationCoordinates? LocationCoordinates { get; set; }
    public int NumberOfPeople { get; set; }
    public int UrgencyLevel { get; set; }
}
