namespace OrbitGuard.Api.Models;

public class SatelliteReading
{
    public int Id { get; set; }

    public int MonitoredAreaId { get; set; }

    public decimal Temperature { get; set; }

    public decimal Humidity { get; set; }

    public decimal Rainfall { get; set; }

    public decimal VegetationIndex { get; set; }

    public DateTime CapturedAt { get; set; } = DateTime.UtcNow;

    public MonitoredArea? MonitoredArea { get; set; }

    public List<ClimateAlert> ClimateAlerts { get; set; } = new();
}