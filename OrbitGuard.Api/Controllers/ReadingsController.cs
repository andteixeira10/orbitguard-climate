using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrbitGuard.Api.Data;
using OrbitGuard.Api.Models;

namespace OrbitGuard.Api.Controllers;

[ApiController]
[Route("api/readings")]
public class ReadingsController : ControllerBase
{
    private readonly OrbitGuardDbContext _context;

    public ReadingsController(OrbitGuardDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SatelliteReading>>> GetAll()
    {
        return await _context.SatelliteReadings
            .Include(reading => reading.MonitoredArea)
            .OrderBy(reading => reading.Id)
            .ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<SatelliteReading>> GetById(int id)
    {
        var reading = await _context.SatelliteReadings
            .Include(reading => reading.MonitoredArea)
            .Include(reading => reading.ClimateAlerts)
            .FirstOrDefaultAsync(reading => reading.Id == id);

        if (reading is null)
            return NotFound(new { message = "Satellite reading not found." });

        return reading;
    }

    [HttpPost]
    public async Task<ActionResult<SatelliteReading>> Create(SatelliteReading reading)
    {
        var areaExists = await _context.MonitoredAreas
            .AnyAsync(area => area.Id == reading.MonitoredAreaId);

        if (!areaExists)
            return BadRequest(new { message = "Monitored area does not exist." });

        reading.Id = 0;
        reading.CapturedAt = DateTime.UtcNow;

        _context.SatelliteReadings.Add(reading);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = reading.Id }, reading);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, SatelliteReading updatedReading)
    {
        var reading = await _context.SatelliteReadings.FindAsync(id);

        if (reading is null)
            return NotFound(new { message = "Satellite reading not found." });

        var areaExists = await _context.MonitoredAreas
            .AnyAsync(area => area.Id == updatedReading.MonitoredAreaId);

        if (!areaExists)
            return BadRequest(new { message = "Monitored area does not exist." });

        reading.MonitoredAreaId = updatedReading.MonitoredAreaId;
        reading.Temperature = updatedReading.Temperature;
        reading.Humidity = updatedReading.Humidity;
        reading.Rainfall = updatedReading.Rainfall;
        reading.VegetationIndex = updatedReading.VegetationIndex;

        await _context.SaveChangesAsync();

        return Ok(reading);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var reading = await _context.SatelliteReadings.FindAsync(id);

        if (reading is null)
            return NotFound(new { message = "Satellite reading not found." });

        _context.SatelliteReadings.Remove(reading);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}