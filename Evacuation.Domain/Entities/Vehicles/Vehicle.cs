using Evacuations.Domain.Common;

namespace Evacuations.Domain.Entities.Vehicles;

public class Vehicle : BaseEntity
{
    public int Capacity { get; set; }
    public string? Type { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int Speed { get; set; }
    public bool IsAvailable { get; set; }
}
