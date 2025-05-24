namespace Evacuations.Application.Dtos.Evacuations.Responses;

public class EvacuationPlanResponse
{
    public Guid ZoneId { get; set; }
    public Guid VehicleId { get; set; }
    public string? ETA { get; set; }
    public int NumberOfPeople { get; set; }
}
