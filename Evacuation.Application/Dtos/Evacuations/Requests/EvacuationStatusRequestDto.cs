namespace Evacuations.Application.Dtos.Evacuations.Requests;

public class EvacuationStatusRequestDto
{
    public Guid Id { get; set; }
    public string? Status { get; set; }
}
