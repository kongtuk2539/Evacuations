using Evacuations.Domain.Common;
using Evacuations.Domain.Entities.Evacuations;
using Evacuations.Domain.Entities.Vehicles;
using Evacuations.Infrastructure.Persistence;

namespace Evacuations.Infrastructure.Seedes;

internal class EvacuationSeeder(EvacuationsDbContext dbContext) : IEvacuationSeeder
{
    public async Task Seed()
    {
        if (await dbContext.Database.CanConnectAsync())
        {
            if (!dbContext.EvacuationZones.Any())
            {
                var evacuation = GetEvacuation();
                dbContext.EvacuationZones.AddRange(evacuation);
                await dbContext.SaveChangesAsync();
            }

            if (!dbContext.Vehicles.Any())
            {
                var vehicles = GetVechicles();
                dbContext.Vehicles.AddRange(vehicles);
                await dbContext.SaveChangesAsync();
            }
        }
    }

    private IEnumerable<EvacuationZone> GetEvacuation()
    {
        List<EvacuationZone> evacuationZones = [
                new()
                {       
                    Latitude = 13.170012d,
                    Longitude = 100.572000d,
                    NumberOfPeople = 30,
                    UrgencyLevel = 3
                },
                new()
                {
                    Latitude = 13.270000d,
                    Longitude = 107.651004d,
                    NumberOfPeople = 80,
                    UrgencyLevel = 4
                },
                new()
                {
                    Latitude = 24.98d,
                    Longitude = 140.157d,
                    NumberOfPeople = 60,
                    UrgencyLevel = 5
                }
            ];

        return evacuationZones;
    }

    private IEnumerable<Vehicle> GetVechicles()
    {
        List<Vehicle> vehicles = [
                new()
                {
                    Capacity = 20,
                    Type = "bus",
                    Latitude = 20.87d,
                    Longitude = 108.87d,
                    Speed = 60,
                    IsAvailable = true
                },
                new()
                {
                    Capacity = 40,
                    Type = "van",
                    Latitude = 80.108001d,
                    Longitude = 208.890245d,
                    Speed = 40,
                    IsAvailable = true
                },
                new()
                {
                    Capacity = 15,
                    Type = "car",
                    Latitude = 101.210124d,
                    Longitude = 205.170014d,
                    Speed = 80,
                    IsAvailable = true
                }
            ];

        return vehicles;
    }
}
