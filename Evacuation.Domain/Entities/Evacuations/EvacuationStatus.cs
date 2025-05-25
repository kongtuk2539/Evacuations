using Evacuations.Domain.Common;

namespace Evacuations.Domain.Entities.Evacuations;

public class EvacuationStatus : BaseEntity
{
    public Guid ZoneId { get; set; }
    public int TotalEvacuated { get; set; }
    public int RemainingPeople { get; set; }
    public EnumStatus Status { get; set; }
    public EvacuationZone? EvacuationZone { get; set; }
}
