using AutoMapper;
using Evacuations.Application.Dtos.Common;
using Evacuations.Application.Dtos.Evacuations.Requests;
using Evacuations.Application.Dtos.Evacuations.Responses;
using Evacuations.Application.Helpers;
using Evacuations.Domain.Entities.Evacuations;
using Evacuations.Domain.Entities.Vehicles;
using Evacuations.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Evacuations.Application.Services.Evacuations;

public record CalculatePlan(Guid ZoneId, Guid VehicleId, int NumberOfPeople, double ETH)
{
}

public class EvacuationsService(ILogger<EvacuationsService> logger,
    IMapper mapper,
    IEvacuationsRepository evacuationsRepository,
    IVehiclesRepository vehiclesRepository) : IEvacuationsService
{
    public async Task<IEnumerable<EvacuationPlanResponse>> GeneratePlanAsync()
    {
        var evacuationZones = await evacuationsRepository.GetAllAsync();

        if (!evacuationZones.Any()) throw new ArgumentException("No available evacuation zones");

        evacuationZones = evacuationZones
            .Where(ez => ez.NumberOfPeople > 0)
            .OrderByDescending(o => o.UrgencyLevel).ToList();

        List<EvacuationPlanResponse> evacuationPlans = new();
        List<EvacuationStatusResponse> evacuationStatuses = new();

        foreach (var zone in evacuationZones)
        {
            Vehicle? vehicle = await FindNearestVehicle(zone);

            if (vehicle is null) continue;

            double distance = CalculatorHelper.Haversine(toLocation(zone.Latitude, zone.Longitude),
                                                         toLocation(vehicle.Latitude, vehicle.Longitude));
            double estimatedTimeOfArrival = CalculatorHelper.ETAMinutes(distance, vehicle.Speed);

            var remainingPeople = ReduceNumberOfPeople(zone.NumberOfPeople, vehicle.Capacity);

            evacuationPlans.Add(new()
            {
                ZoneId = zone.Id,
                VehicleId = vehicle.Id,
                ETA = $"{estimatedTimeOfArrival} Minuite",
                NumberOfPeople = CalculatorRemainPeople(zone.NumberOfPeople, vehicle.Capacity)
            });

            evacuationStatuses.Add(new()
            {
                ZoneId = zone.Id,
                TotalEvacuated = zone.NumberOfPeople,
                RemainingPeople = remainingPeople,
            });

            zone.NumberOfPeople = remainingPeople;
        }

        await GenerateStatusAsync(evacuationStatuses);
        return evacuationPlans;
    }

    public async Task<EvacuationZoneResponseDto> CreateEvacuationZoneAsync(EvacuationZoneRequestDto evacuationZonesDto)
    {
        logger.LogInformation("Creating a new EvacuationZones {@EvacuationZones}", evacuationZonesDto);
        var evacuationZone = mapper.Map<EvacuationZone>(evacuationZonesDto);
        var result = await evacuationsRepository.CreateAsync(evacuationZone);
        return mapper.Map<EvacuationZoneResponseDto>(result);
    }

    public async Task<IEnumerable<EvacuationStatusResponse>> GetAllStatusAsync()
    {
        var result = await evacuationsRepository.GetAllStatusesAsync();
        return mapper.Map<IEnumerable<EvacuationStatusResponse>>(result);
    }

    private async Task<Vehicle?> FindNearestVehicle(EvacuationZone zone)
    {
        var vehicles = await vehiclesRepository.GetAllAsync();
        
        if (!vehicles.Any()) throw new ArgumentException("No available vehicles");

        var vehiclesIsAvailable = vehicles
            .Where(v => v.IsAvailable)
            .ToList();

        var nearestVehicle = vehiclesIsAvailable
            .OrderBy(o => CalculatorHelper.Haversine(toLocation(o.Latitude, o.Longitude),
                                                     toLocation(zone.Latitude, zone.Longitude)))
            .ThenByDescending(o => o.Capacity)
            .FirstOrDefault();

        if (nearestVehicle is null) return null;

        MarkVehicleAsUnavailable(nearestVehicle);

        return nearestVehicle;
    }

    private async Task GenerateStatusAsync(List<EvacuationStatusResponse> evacuationStatuses)
    {
        var entities = mapper.Map<List<EvacuationStatus>>(evacuationStatuses);
        await evacuationsRepository.CreateStatusesAsync(entities);
    }

    private LocationCoordinates toLocation(double lat, double lon)
    {
        return new() { Latitude = lat, Longitude = lon };
    }

    private void MarkVehicleAsUnavailable(Vehicle vehicle)
    {
        vehicle.IsAvailable = false;
    }

    private int ReduceNumberOfPeople(int numberOfPeople, int capacity)
    {
        return numberOfPeople <= capacity ? 0 : numberOfPeople - capacity;
    }

    private int CalculatorRemainPeople(int numberOfPeople, int capacity)
    {
        return numberOfPeople <= capacity ? numberOfPeople : capacity;
    }

    
}
