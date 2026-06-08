namespace OrbitGuard.Api.Models;

public class MonitoredArea
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }

    public string RiskLevel { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<SatelliteReading> SatelliteReadings { get; set; } = new();

    public List<ClimateAlert> ClimateAlerts { get; set; } = new();
}