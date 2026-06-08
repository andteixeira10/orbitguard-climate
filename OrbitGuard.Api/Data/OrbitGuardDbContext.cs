using Microsoft.EntityFrameworkCore;
using OrbitGuard.Api.Models;

namespace OrbitGuard.Api.Data;

public class OrbitGuardDbContext : DbContext
{
    public OrbitGuardDbContext(DbContextOptions<OrbitGuardDbContext> options)
        : base(options)
    {
    }

    public DbSet<MonitoredArea> MonitoredAreas => Set<MonitoredArea>();
    public DbSet<SatelliteReading> SatelliteReadings => Set<SatelliteReading>();
    public DbSet<ClimateAlert> ClimateAlerts => Set<ClimateAlert>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MonitoredArea>(entity =>
        {
            entity.ToTable("monitored_areas");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(120).IsRequired();
            entity.Property(e => e.Location).HasColumnName("location").HasMaxLength(160).IsRequired();
            entity.Property(e => e.Latitude).HasColumnName("latitude").HasPrecision(10, 6);
            entity.Property(e => e.Longitude).HasColumnName("longitude").HasPrecision(10, 6);
            entity.Property(e => e.RiskLevel).HasColumnName("risk_level").HasMaxLength(30).IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
        });

        modelBuilder.Entity<SatelliteReading>(entity =>
        {
            entity.ToTable("satellite_readings");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MonitoredAreaId).HasColumnName("monitored_area_id");
            entity.Property(e => e.Temperature).HasColumnName("temperature").HasPrecision(6, 2);
            entity.Property(e => e.Humidity).HasColumnName("humidity").HasPrecision(6, 2);
            entity.Property(e => e.Rainfall).HasColumnName("rainfall").HasPrecision(6, 2);
            entity.Property(e => e.VegetationIndex).HasColumnName("vegetation_index").HasPrecision(5, 2);
            entity.Property(e => e.CapturedAt).HasColumnName("captured_at");

            entity.HasOne(e => e.MonitoredArea)
                .WithMany(e => e.SatelliteReadings)
                .HasForeignKey(e => e.MonitoredAreaId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ClimateAlert>(entity =>
        {
            entity.ToTable("climate_alerts");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.MonitoredAreaId).HasColumnName("monitored_area_id");
            entity.Property(e => e.SatelliteReadingId).HasColumnName("satellite_reading_id");
            entity.Property(e => e.AlertType).HasColumnName("alert_type").HasMaxLength(80).IsRequired();
            entity.Property(e => e.Severity).HasColumnName("severity").HasMaxLength(30).IsRequired();
            entity.Property(e => e.Description).HasColumnName("description").HasMaxLength(500).IsRequired();
            entity.Property(e => e.Status).HasColumnName("status").HasMaxLength(30).IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");

            entity.HasOne(e => e.MonitoredArea)
                .WithMany(e => e.ClimateAlerts)
                .HasForeignKey(e => e.MonitoredAreaId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.SatelliteReading)
                .WithMany(e => e.ClimateAlerts)
                .HasForeignKey(e => e.SatelliteReadingId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}