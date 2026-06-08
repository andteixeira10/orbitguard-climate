using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrbitGuard.Api.Data;
using OrbitGuard.Api.Models;

namespace OrbitGuard.Api.Controllers;

[ApiController]
[Route("api/alerts")]
public class AlertsController : ControllerBase
{
    private readonly OrbitGuardDbContext _context;

    public AlertsController(OrbitGuardDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClimateAlert>>> GetAll()
    {
        return await _context.ClimateAlerts
            .Include(alert => alert.MonitoredArea)
            .Include(alert => alert.SatelliteReading)
            .OrderBy(alert => alert.Id)
            .ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ClimateAlert>> GetById(int id)
    {
        var alert = await _context.ClimateAlerts
            .Include(alert => alert.MonitoredArea)
            .Include(alert => alert.SatelliteReading)
            .FirstOrDefaultAsync(alert => alert.Id == id);

        if (alert is null)
            return NotFound(new { message = "Climate alert not found." });

        return alert;
    }

    [HttpPost]
    public async Task<ActionResult<ClimateAlert>> Create(ClimateAlert alert)
    {
        var areaExists = await _context.MonitoredAreas
            .AnyAsync(area => area.Id == alert.MonitoredAreaId);

        if (!areaExists)
            return BadRequest(new { message = "Monitored area does not exist." });

        if (alert.SatelliteReadingId is not null)
        {
            var readingExists = await _context.SatelliteReadings
                .AnyAsync(reading =>
                    reading.Id == alert.SatelliteReadingId &&
                    reading.MonitoredAreaId == alert.MonitoredAreaId);

            if (!readingExists)
                return BadRequest(new { message = "Satellite reading does not exist for this monitored area." });
        }

        alert.Id = 0;
        alert.CreatedAt = DateTime.UtcNow;

        if (string.IsNullOrWhiteSpace(alert.Status))
            alert.Status = "OPEN";

        _context.ClimateAlerts.Add(alert);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = alert.Id }, alert);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, ClimateAlert updatedAlert)
    {
        var alert = await _context.ClimateAlerts.FindAsync(id);

        if (alert is null)
            return NotFound(new { message = "Climate alert not found." });

        var areaExists = await _context.MonitoredAreas
            .AnyAsync(area => area.Id == updatedAlert.MonitoredAreaId);

        if (!areaExists)
            return BadRequest(new { message = "Monitored area does not exist." });

        if (updatedAlert.SatelliteReadingId is not null)
        {
            var readingExists = await _context.SatelliteReadings
                .AnyAsync(reading =>
                    reading.Id == updatedAlert.SatelliteReadingId &&
                    reading.MonitoredAreaId == updatedAlert.MonitoredAreaId);

            if (!readingExists)
                return BadRequest(new { message = "Satellite reading does not exist for this monitored area." });
        }

        alert.MonitoredAreaId = updatedAlert.MonitoredAreaId;
        alert.SatelliteReadingId = updatedAlert.SatelliteReadingId;
        alert.AlertType = updatedAlert.AlertType;
        alert.Severity = updatedAlert.Severity;
        alert.Description = updatedAlert.Description;
        alert.Status = updatedAlert.Status;

        await _context.SaveChangesAsync();

        return Ok(alert);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var alert = await _context.ClimateAlerts.FindAsync(id);

        if (alert is null)
            return NotFound(new { message = "Climate alert not found." });

        _context.ClimateAlerts.Remove(alert);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}