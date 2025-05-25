using AutoMapper;
using Evacuations.Application.Dtos.Common;
using Evacuations.Application.Dtos.Evacuations.Requests;
using Evacuations.Application.Dtos.Evacuations.Responses;
using Evacuations.Application.Helpers;
using Evacuations.Domain.Common;
using Evacuations.Domain.Entities.Evacuations;
using Evacuations.Domain.Entities.Vehicles;
using Evacuations.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Evacuations.Application.Services.Evacuations;

public class EvacuationsService(ILogger<EvacuationsService> logger,
    IMapper mapper,
    IEvacuationsRepository evacuationsRepository,
    IVehiclesRepository vehiclesRepository) : IEvacuationsService
{
    public async Task<IEnumerable<EvacuationPlanResponseDto>> GeneratePlanAsync()
    {
        logger.LogInformation("GeneratePlan is called");
        var evacuationZones = await evacuationsRepository.GetAllAsync();

        if (!evacuationZones.Any()) throw new ArgumentException("No available evacuation zones");

        evacuationZones = evacuationZones
            .Where(ez => ez.NumberOfPeople > 0)
            .OrderByDescending(o => o.UrgencyLevel).ToList();

        List<EvacuationPlanResponseDto> evacuationPlans = new();
        List<EvacuationStatusResponseDto> evacuationStatuses = new();

        foreach (var zone in evacuationZones)
        {
            Vehicle? vehicle = await FindNearestVehicle(zone);

            if (vehicle is null) continue;

            double distance = CalculatorHelper.Haversine(toLocation(zone.Latitude, zone.Longitude),
                                                         toLocation(vehicle.Latitude, vehicle.Longitude));
            var estimatedTimeOfArrival = CalculatorHelper.ETAMinutes(distance, vehicle.Speed);

            var remainingPeople = ReduceNumberOfPeople(zone.NumberOfPeople, vehicle.Capacity);

            var plan = new EvacuationPlanResponseDto()
            {
                ZoneId = zone.Id,
                VehicleId = vehicle.Id,
                ETA = $"{estimatedTimeOfArrival}",
                NumberOfPeople = CalculatorRemainPeople(zone.NumberOfPeople, vehicle.Capacity)
            };

            evacuationPlans.Add(plan);
            logger.LogInformation("Generate Plans: {@EvacuationPlan}", plan);

            var status = new EvacuationStatusResponseDto()
            {
                ZoneId = zone.Id,
                TotalEvacuated = zone.NumberOfPeople,
                RemainingPeople = remainingPeople,
                Status = EnumStatus.PROGRESS.ToString(),
            };

            evacuationStatuses.Add(status);
            logger.LogInformation("Create new statuses: {@EvacuationStatus}", status);

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

    public async Task<IEnumerable<EvacuationStatusResponseDto>> GetAllStatusAsync()
    {
        var result = await evacuationsRepository.GetAllStatusesAsync();
        return mapper.Map<IEnumerable<EvacuationStatusResponseDto>>(result);
    }

    public async Task<EvacuationStatusResponseDto> UpdateStatusAsync(EvacuationStatusRequestDto evacuationStatus)
    {
        var entity = await evacuationsRepository.GetStatusAsyn(evacuationStatus.Id);
        entity!.Status = Enum.Parse<EnumStatus>(evacuationStatus.Status!.ToUpper());
        await evacuationsRepository.ChangesAsync();
        return mapper.Map<EvacuationStatusResponseDto>(entity);
    }

    public async Task ClearAllAsync()
    {
        await evacuationsRepository.DeleteZonesAsync();
        await evacuationsRepository.DeleteStatusesAsync();
        await vehiclesRepository.DeleteVehiclesAsync();
    }
    private async Task<Vehicle?> FindNearestVehicle(EvacuationZone zone)
    {
        logger.LogInformation("Find Vehicle to Zone: {@EvacuationZone}", zone);
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

    private async Task GenerateStatusAsync(List<EvacuationStatusResponseDto> evacuationStatuses)
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
