namespace Evacuations.Application.Dtos.Evacuations.Responses;

public class EvacuationStatusResponse
{
    public Guid ZoneId { get; set; }
    public int TotalEvacuated { get; set; }
    public int RemainingPeople { get; set; }
}
