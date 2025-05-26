using Evacuations.Domain.Entities.Vehicles;
using Evacuations.Domain.Repositories;
using Evacuations.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Evacuations.Infrastructure.Repositories;

internal class VehiclesRepository(EvacuationsDbContext dbContext) : IVehiclesRepository
{
    public async Task<List<Vehicle>> GetAllAsync()
    {
        var vehicles = await dbContext.Vehicles
            .Where(v => v.IsAvailable)
            .Where(v => !v.IsDeleted)
            .ToListAsync();
        return vehicles;
    }

    public async Task<Vehicle> CreateAsync(Vehicle entity)
    {
        entity.IsAvailable = true;
        dbContext.Vehicles.Add(entity);
        await dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteVehiclesAsync()
    {
        await dbContext.Vehicles
            .Where(v => !v.IsDeleted)
            .ExecuteUpdateAsync(setters => setters.SetProperty(p => p.IsDeleted, true));
    }
}
