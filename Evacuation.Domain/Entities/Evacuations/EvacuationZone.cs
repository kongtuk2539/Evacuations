using Evacuations.Domain.Common;

namespace Evacuations.Domain.Entities.Evacuations;

public class EvacuationZone : BaseEntity
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int NumberOfPeople { get; set; }
    public int UrgencyLevel { get; set; }

    public List<EvacuationStatus>? EvacuationStatus { get; set; }

}
