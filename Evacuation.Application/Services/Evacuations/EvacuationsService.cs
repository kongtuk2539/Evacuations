using AutoMapper;
using Evacuations.Application.Dtos.Evacuations.Requests;
using Evacuations.Application.Dtos.Evacuations.Responses;
using Evacuations.Application.Helpers;
using Evacuations.Application.Services.Redis;
using Evacuations.Domain.Common;
using Evacuations.Domain.Entities.Evacuations;
using Evacuations.Domain.Entities.Vehicles;
using Evacuations.Domain.Exceptions;
using Evacuations.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Evacuations.Application.Services.Evacuations;

public class EvacuationsService(ILogger<EvacuationsService> logger,
    IMapper mapper,
    IEvacuationsRepository evacuationsRepository,
    IVehiclesRepository vehiclesRepository,
    ICacheService<EvacuationStatusResponseDto> cacheService) : IEvacuationsService
{
    private readonly string keyEvacuationStatusesRedis = nameof(EvacuationStatus);
    public async Task<EvacuationZoneResponseDto> CreateEvacuationZoneAsync(EvacuationZoneRequestDto evacuationZonesDto)
    {
        logger.LogInformation("Creating a new EvacuationZones {@EvacuationZones}", evacuationZonesDto);
        var evacuationZone = mapper.Map<EvacuationZone>(evacuationZonesDto);
        var result = await evacuationsRepository.CreateAsync(evacuationZone);
        return mapper.Map<EvacuationZoneResponseDto>(result);
    }

    public async Task<IEnumerable<EvacuationStatusResponseDto>> GetAllStatusAsync()
    {
        logger.LogInformation("Getting all Evacuation Status");

        if(await cacheService.IsExistsAsync(keyEvacuationStatusesRedis))
        {
            logger.LogInformation("Getting all Evacuation Status from Redis");
            return await cacheService.GetAsync(keyEvacuationStatusesRedis);
        }
   
        var evacuationStatuses = await evacuationsRepository.GetAllStatusesAsync();

        if (!evacuationStatuses.Any())
            throw new NotFoundException(nameof(EvacuationStatus));

        var statusResponse = mapper.Map<IEnumerable<EvacuationStatusResponseDto>>(evacuationStatuses);

        await cacheService.SaveAsync(keyEvacuationStatusesRedis, statusResponse);

        return statusResponse;
    }

    public async Task<IEnumerable<EvacuationPlanResponseDto>> GeneratePlanAsync()
    {
        logger.LogInformation("GeneratePlan is called");
        var evacuationZones = await evacuationsRepository.GetAllAsync();

        if (!evacuationZones.Any())
            throw new NotFoundException(nameof(EvacuationZone));

        evacuationZones = evacuationZones
            .Where(ez => ez.NumberOfPeople > 0)
            .OrderByDescending(o => o.UrgencyLevel).ToList();

        List<EvacuationPlanResponseDto> evacuationPlans = new();
        List<EvacuationStatusResponseDto> evacuationStatuses = new();

        foreach (var zone in evacuationZones)
        {
            Vehicle? vehicle = await FindNearestVehicle(zone);

            if (vehicle is null) continue;

            double distance = CalculatorHelper.Haversine(AppHelper.ToLocation(zone.Latitude, zone.Longitude),
                                                         AppHelper.ToLocation(vehicle.Latitude, vehicle.Longitude));
            var estimatedTimeOfArrival = CalculatorHelper.ETAMinutes(distance, vehicle.Speed);

            var remainingPeople = CalculatorHelper.RemainOfPeople(zone.NumberOfPeople, vehicle.Capacity);
            var totalEvacuated = CalculatorHelper.EvacuatedPeople(zone.NumberOfPeople, vehicle.Capacity);

            var plan = new EvacuationPlanResponseDto()
            {
                ZoneId = zone.Id,
                VehicleId = vehicle.Id,
                ETA = $"{estimatedTimeOfArrival}",
                NumberOfPeople = remainingPeople
            };

            evacuationPlans.Add(plan);

            logger.LogInformation("Generate Plans: {@EvacuationPlan}", plan);

            var status = new EvacuationStatusResponseDto()
            {
                ZoneId = zone.Id,
                TotalEvacuated = totalEvacuated,
                RemainingPeople = remainingPeople,
                Status = EnumStatuses.PROGRESS,
            };

            evacuationStatuses.Add(status);
            logger.LogInformation("Create new statuses: {@EvacuationStatus}", status);

            zone.NumberOfPeople = remainingPeople;
        }

        await GenerateStatusAsync(evacuationStatuses);
        return evacuationPlans;
    }

    private async Task<Vehicle?> FindNearestVehicle(EvacuationZone zone)
    {
        logger.LogInformation("Find Vehicle to Zone: {@EvacuationZone}", zone);
        var vehicles = await vehiclesRepository.GetAllAsync();

        if (!vehicles.Any()) throw new NotFoundException(nameof(Vehicle));

        var nearestVehicle = vehicles
            .Where(v => v.IsAvailable)
            .OrderBy(o => CalculatorHelper.Haversine(AppHelper.ToLocation(o.Latitude, o.Longitude),
                                                     AppHelper.ToLocation(zone.Latitude, zone.Longitude)))
            .ThenByDescending(o => o.Capacity)
            .FirstOrDefault();

        if (nearestVehicle is null) return null;

        MarkVehicleAsUnavailable(nearestVehicle);

        return nearestVehicle;
    }

    private void MarkVehicleAsUnavailable(Vehicle vehicle)
    {
        vehicle.IsAvailable = false;
    }

    private async Task GenerateStatusAsync(List<EvacuationStatusResponseDto> evacuationStatuses)
    {
        var entities = mapper.Map<List<EvacuationStatus>>(evacuationStatuses);
        await evacuationsRepository.CreateStatusesAsync(entities);

        await UpdateStatusToCaches();
    }

    private async Task UpdateStatusToCaches()
    {
        await cacheService.RemoveAsync(keyEvacuationStatusesRedis);
        var statuses = await evacuationsRepository.GetAllStatusesAsync();
        await cacheService.SaveAsync(keyEvacuationStatusesRedis, mapper.Map<IEnumerable<EvacuationStatusResponseDto>>(statuses));
    }

    public async Task<EvacuationStatusResponseDto> UpdateStatusAsync(EvacuationStatusRequestDto evacuationStatus)
    {
        logger.LogInformation("Updating Evacuation Status with id: {Id} with status: {Status}", evacuationStatus.Id, evacuationStatus.Status.ToString());
        var entity = await evacuationsRepository.GetStatusAsyn(evacuationStatus.Id);

        if (entity is null)
            throw new NotFoundException(nameof(EvacuationStatus), evacuationStatus.Id.ToString());

        await ChangesStatus(entity, evacuationStatus.Status);

        
        await evacuationsRepository.ChangesAsync();
        return mapper.Map<EvacuationStatusResponseDto>(entity);
    }

    private async Task ChangesStatus(EvacuationStatus entityStatus, EnumStatuses status)
    {
        var entityZone = await evacuationsRepository.GetAsync(entityStatus.ZoneId);

        if (entityZone is null)
            throw new NotFoundException(nameof(EvacuationZone), entityStatus.ZoneId.ToString());

        switch (entityStatus.Status)
        {
            case EnumStatuses.PROGRESS:
                CheckStatusProgress(entityZone, entityStatus, status);
                break;
            case EnumStatuses.SUCCEED:
                CheckStatusSucceed(entityZone, entityStatus, status);
                break;
            case EnumStatuses.CANCEL:
                CheckStatusCancel(entityZone, entityStatus, status);
                break;
        }

        entityStatus.Status = status;
    }

    private void CheckStatusProgress(EvacuationZone entityZone, EvacuationStatus entityStatus, EnumStatuses status)
    {
        if (status == EnumStatuses.CANCEL)
        {
            entityZone.NumberOfPeople += entityStatus.TotalEvacuated;
        }
        return;
    }

    private void CheckStatusSucceed(EvacuationZone entityZone, EvacuationStatus entityStatus, EnumStatuses status)
    {
        if (status == EnumStatuses.CANCEL)
        {
            entityZone.NumberOfPeople += entityStatus.TotalEvacuated;
        }
        return;
    }

    private void CheckStatusCancel(EvacuationZone entityZone, EvacuationStatus entityStatus, EnumStatuses status)
    {
        if (status == EnumStatuses.SUCCEED || status == EnumStatuses.PROGRESS)
        {
            entityZone.NumberOfPeople -= entityStatus.TotalEvacuated;
        }
        return;
    }

    public async Task ClearAllAsync()
    {
        logger.LogWarning($"Clear all {typeof(EvacuationZone)}, {typeof(EvacuationStatus)}, {typeof(Vehicle)}");

        await evacuationsRepository.DeleteZonesAsync();
        await evacuationsRepository.DeleteStatusesAsync();
        await vehiclesRepository.DeleteVehiclesAsync();
        await cacheService.RemoveAsync(keyEvacuationStatusesRedis);
    }
}
