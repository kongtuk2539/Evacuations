using Evacuations.Domain.Common;

namespace Evacuations.Application.Dtos.Evacuations.Responses;

public class EvacuationStatusResponseDto
{
    public Guid Id { get; set; }
    public Guid ZoneId { get; set; }
    public int TotalEvacuated { get; set; }
    public int RemainingPeople { get; set; }
    public string? Status { get; set; }
}
