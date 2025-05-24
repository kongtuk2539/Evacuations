using Evacuations.Domain.Common;
using Evacuations.Domain.Entities.Evacuations;
using Evacuations.Domain.Entities.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace Evacuations.Infrastructure.Persistence;

internal class EvacuationsDbContext(DbContextOptions<EvacuationsDbContext> options) : DbContext(options)
{
    internal DbSet<EvacuationZone> EvacuationZones { get; set; }
    internal DbSet<Vehicle> Vehicles { get; set; }
    internal DbSet<EvacuationStatus> EvacuationStatuses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<EvacuationZone>()
            .HasMany(ez => ez.EvacuationStatus)
            .WithOne()
            .HasForeignKey(es => es.ZoneId)
            .IsRequired();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<IBaseEntity>();

        foreach (var entry in entries)
        {
            if (entry.State is EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
            }
            else if (entry.State is EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
