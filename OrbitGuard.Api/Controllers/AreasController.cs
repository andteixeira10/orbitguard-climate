using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrbitGuard.Api.Data;
using OrbitGuard.Api.Models;

namespace OrbitGuard.Api.Controllers;

[ApiController]
[Route("api/areas")]
public class AreasController : ControllerBase
{
    private readonly OrbitGuardDbContext _context;

    public AreasController(OrbitGuardDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MonitoredArea>>> GetAll()
    {
        return await _context.MonitoredAreas
            .OrderBy(area => area.Id)
            .ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MonitoredArea>> GetById(int id)
    {
        var area = await _context.MonitoredAreas
            .Include(area => area.SatelliteReadings)
            .Include(area => area.ClimateAlerts)
            .FirstOrDefaultAsync(area => area.Id == id);

        if (area is null)
            return NotFound(new { message = "Monitored area not found." });

        return area;
    }

    [HttpPost]
    public async Task<ActionResult<MonitoredArea>> Create(MonitoredArea area)
    {
        area.Id = 0;
        area.CreatedAt = DateTime.UtcNow;

        _context.MonitoredAreas.Add(area);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = area.Id }, area);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, MonitoredArea updatedArea)
    {
        var area = await _context.MonitoredAreas.FindAsync(id);

        if (area is null)
            return NotFound(new { message = "Monitored area not found." });

        area.Name = updatedArea.Name;
        area.Location = updatedArea.Location;
        area.Latitude = updatedArea.Latitude;
        area.Longitude = updatedArea.Longitude;
        area.RiskLevel = updatedArea.RiskLevel;

        await _context.SaveChangesAsync();

        return Ok(area);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var area = await _context.MonitoredAreas.FindAsync(id);

        if (area is null)
            return NotFound(new { message = "Monitored area not found." });

        _context.MonitoredAreas.Remove(area);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}