namespace OrbitGuard.Api.Models;

public class ClimateAlert
{
    public int Id { get; set; }

    public int MonitoredAreaId { get; set; }

    public int? SatelliteReadingId { get; set; }

    public string AlertType { get; set; } = string.Empty;

    public string Severity { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Status { get; set; } = "OPEN";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public MonitoredArea? MonitoredArea { get; set; }

    public SatelliteReading? SatelliteReading { get; set; }
}