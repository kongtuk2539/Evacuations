using Evacuations.Domain.Entities.Evacuations;
using Evacuations.Domain.Repositories;
using Evacuations.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Evacuations.Infrastructure.Repositories;

internal class EvacuationsRepository(EvacuationsDbContext dbContext) : IEvacuationsRepository
{
    public async Task<List<EvacuationZone>> GetAllAsync()
    {
        var evacuationZones = await dbContext.EvacuationZones
            .Where(ez => !ez.IsDeleted)
            .ToListAsync();
        return evacuationZones;
    }

    public async Task<EvacuationZone?> GetAsync(Guid id)
    {
        var evacuationZone = await dbContext.EvacuationZones
            .Where(ez => !ez.IsDeleted)
            .FirstOrDefaultAsync(ez => ez.Id == id);
        return evacuationZone;
    }

    public async Task<EvacuationZone> CreateAsync(EvacuationZone entity)
    {
        dbContext.EvacuationZones.Add(entity);
        await dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task ChangesAsync()
            => await dbContext.SaveChangesAsync();

    public async Task CreateStatusesAsync(List<EvacuationStatus> entities)
    {
        dbContext.EvacuationStatuses.AddRange(entities);
        await dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<EvacuationStatus>> GetAllStatusesAsync()
    {
        var evacuationStatuses = await dbContext.EvacuationStatuses
            .Where(ez => !ez.IsDeleted)
            .ToListAsync();
        return evacuationStatuses;
    }

    public async Task<EvacuationStatus?> GetStatusAsyn(Guid id)
    {
        var evacuationStatus = await dbContext.EvacuationStatuses
            .Where(ez => !ez.IsDeleted)
            .FirstOrDefaultAsync(es => es.Id == id);
        return evacuationStatus;
    }

    public async Task DeleteZonesAsync()
    {
        await dbContext.EvacuationStatuses
            .Where(es => !es.IsDeleted)
            .ExecuteUpdateAsync(setters => setters.SetProperty(p => p.IsDeleted, true));
    }

    public async Task DeleteStatusesAsync()
    {
        await dbContext.EvacuationZones
            .Where(ez => !ez.IsDeleted)
            .ExecuteUpdateAsync(setters => setters.SetProperty(p => p.IsDeleted, true));
    }
}
