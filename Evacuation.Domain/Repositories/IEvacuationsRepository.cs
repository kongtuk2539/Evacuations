using Evacuations.Domain.Entities.Evacuations;

namespace Evacuations.Domain.Repositories;

public interface IEvacuationsRepository
{
    Task<EvacuationZone> CreateAsync(EvacuationZone entity);
    Task<List<EvacuationZone>> GetAllAsync();
    Task<IEnumerable<EvacuationStatus>> GetAllStatusesAsync();
    Task<EvacuationStatus?> GetStatusAsyn(Guid id);
    Task CreateStatusesAsync(List<EvacuationStatus> entities);
    Task ChangesAsync();
    Task DeleteZonesAsync();
    Task DeleteStatusesAsync();
}
