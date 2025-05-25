using Evacuations.Domain.Entities.Vehicles;

namespace Evacuations.Domain.Repositories;

public interface IVehiclesRepository
{
    Task<Vehicle> CreateAsync(Vehicle entity);
    Task<List<Vehicle>> GetAllAsync();
    Task DeleteVehiclesAsync();
}
